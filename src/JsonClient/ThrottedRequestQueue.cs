using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading;
using Common.Logging;

namespace CityIndex.JsonClient
{
    /// <summary>
    /// 
    /// A self throttling asynchronous request queue.
    /// 
    /// TODO: allow for pausing
    /// would like to allow for clearing the queue but dependencies on the cache make
    /// this a non starter. the only viable ways to that means is to merge throttle and cache.
    /// this will have to be a builder type action in the calling class that can corelate the
    /// queue items and the cache items and resolve the cache items that are cleared.
    /// this will probably mean the introduction of a 'cancelled' CacheItemState
    /// </summary>
    public sealed class ThrottedRequestQueue : IThrottedRequestQueue
    {

        #region Fields

        private readonly string _scope;
        private static readonly ILog Log = LogManager.GetLogger(typeof(ThrottedRequestQueue));
        private readonly int _maxPendingRequests;
        private readonly Queue<DateTimeOffset> _requestTimes = new Queue<DateTimeOffset>();
        private readonly Queue<RequestHolder> _requests = new Queue<RequestHolder>();

        private readonly int _throttleWindowCount;
        private readonly TimeSpan _throttleWindowTime;
        private int _dispatchedCount;
        private bool _notifiedWaitingOnMaxPending;
        private bool _notifiedWaitingOnWindow;
        private int _outstandingRequests;
        private bool _processingQueue;
        private static object _lockTarget = new object();

        #endregion

        
        #region Constructors

        /// <summary>
        /// Insantiates a <see cref="ThrottedRequestQueue"/> with default parameters.
        /// throttleWindowTime = 5 seconds
        /// throttleWindowCount = 30
        /// maxPendingRequests = 10
        /// </summary>
        public ThrottedRequestQueue()
            : this(TimeSpan.FromSeconds(5), 30, 10, "default")
        {
        }

        /// <summary>
        /// Insantiates a <see cref="ThrottedRequestQueue"/> with supplied parameters.
        /// </summary>
        /// <param name="throttleWindowTime">The window in which to restrice issued requests to <paramref name="throttleWindowCount"/></param>
        /// <param name="throttleWindowCount">The maximum number of requests to issue in the amount of time described by <paramref name="throttleWindowTime"/></param>
        /// <param name="maxPendingRequests">The maximum allowed number of active requests.</param>
        /// <param name="scope"></param>
        public ThrottedRequestQueue(TimeSpan throttleWindowTime, int throttleWindowCount, int maxPendingRequests, string scope)
        {
            _scope = scope;
            _throttleWindowTime = throttleWindowTime;
            _throttleWindowCount = throttleWindowCount;
            _maxPendingRequests = maxPendingRequests;
        }

        #endregion

        #region IThrottedRequestQueue Members

        public string Scope
        {
            get { return _scope; }
        }

        /// <summary>
        /// The number of requests that have been dispatched
        /// </summary>
        public int DispatchedCount
        {
            get { return _dispatchedCount; }
        }


        /// <summary>
        /// The maximum number of allowed pending request.
        /// 
        /// The throttle window will keep us in compliance with the 
        /// letter of the law, but testing has shown that a large 
        /// number of outstanding requests result in a cascade of 
        /// (500) errors that does not stop. 
        /// 
        /// So we will defer processing while there are > MaxPendingRequests 
        /// regardless of throttle window.
        /// </summary>
        public int MaxPendingRequests
        {
            get { return _maxPendingRequests; }
        }


        /// <summary>
        /// The number of pending (issued) requests
        /// </summary>
        public int PendingRequests
        {
            get { return _outstandingRequests; }
        }


        /// <summary>
        /// The quantitive portion (xxx) of the of 30 requests per 5 seconds
        /// </summary>
        public int ThrottleWindowCount
        {
            get { return _throttleWindowCount; }
        }


        /// <summary>
        /// The temporal portion (yyy) of the of 30 requests per 5 seconds
        /// </summary>
        public TimeSpan ThrottleWindowTime
        {
            get { return _throttleWindowTime; }
        }




        /// <summary>
        /// Adds a request to the end of the queue.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="request"></param>
        /// <param name="action"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Enqueue(string url, WebRequest request, Action<IAsyncResult, RequestHolder> action)
        {
            //Log.DebugFormat("entering lock for {0}",url);
            //lock (_lockTarget)
            {
                //Log.DebugFormat("inside lock for {0}", url);
                // TODO: have a max queue length to keep things from getting out of hand - THEN we can throw an exception
                _requests.Enqueue(new RequestHolder
                    {
                        WebRequest = request,
                        Url = url,
                        AsyncResultHandler = action
                    });
                //Log.DebugFormat("exiting lock for {0}", url);
            }
            //Log.DebugFormat("outside lock for {0}", url);
        }

        #endregion
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void ProcessQueue(object ignored)
        {
            //Log.DebugFormat("entering lock for ProcessQueue()");
            //lock (_lockTarget)
            {
                //Log.DebugFormat("inside lock for ProcessQueue()");
                if (_processingQueue) return;
                if (_requests.Count == 0) return;

                RequestHolder request = _requests.Peek();

                _processingQueue = true;

                try
                {
                    if (ThereAreMoreOutstandingRequestsThanIsAllowed()) return;

                    if (_requestTimes.Count > ThrottleWindowCount)
                    {
                        throw new Exception("request time queue got to be longer than window somehow");
                    }

                    if (_requestTimes.Count == ThrottleWindowCount)
                    {
                        DateTimeOffset head = _requestTimes.Peek();
                        TimeSpan waitTime = (ThrottleWindowTime - (DateTimeOffset.UtcNow - head));

                        if (waitTime.TotalMilliseconds > 0)
                        {
                            if (!_notifiedWaitingOnWindow)
                            {
                                string msgWaiting = string.Format("Waiting: " + waitTime + " to send " + request.Url);
                                Log.Info(msgWaiting);

                                _notifiedWaitingOnWindow = true;
                            }
                            return;
                        }
                        _requestTimes.Dequeue();
                    }


                    // good to go. 
                    _notifiedWaitingOnWindow = false;

                    _requestTimes.Enqueue(DateTimeOffset.UtcNow);
                    _dispatchedCount += 1;

                    request.RequestIndex = _dispatchedCount;

                    try
                    {
                        var webRequestAsyncResult = request.WebRequest.BeginGetResponse(ar =>
                            {
                                Log.Info(string.Format("Received #{0} : {1} ", request.RequestIndex, request.Url));

                                _outstandingRequests--;


                                request.AsyncResultHandler(ar, request);
                            }, null);


                        EnsureRequestWillAbortAfterTimeout(request, webRequestAsyncResult);

                        Log.Info(string.Format("Dispatched #{0} : {1} ", request.RequestIndex, request.Url));
                    }
                    catch (Exception ex)
                    {
                        Log.Error(string.Format("Error dispatching #{0} : {1} \r\n{2}", request.RequestIndex, request.Url, ex.Message));

                        throw;
                    }
                    finally
                    {
                        _requests.Dequeue();
                        _outstandingRequests++; // TODO: should this really be here if there was an error that prevented dispatch?
                    }

                }
                finally
                {
                    _processingQueue = false;
                }
                //Log.DebugFormat("exiting lock for ProcessQueue()");
            }
            //Log.DebugFormat("outside lock for ProcessQueue()");
        }

        private bool ThereAreMoreOutstandingRequestsThanIsAllowed()
        {
            if (_outstandingRequests > MaxPendingRequests)
            {
                if (!_notifiedWaitingOnMaxPending)
                {
                    string msgMaxPending = string.Format("Waiting: pending requests {0}", _outstandingRequests);
                    Log.Info(msgMaxPending);

                    _notifiedWaitingOnMaxPending = true;
                }

                return true;
            }

            _notifiedWaitingOnMaxPending = false;
            return false;
        }

        private static void EnsureRequestWillAbortAfterTimeout(RequestHolder request, IAsyncResult result)
        {
            //TODO: How can we timeout a request for Silverlight, when calls to AsyncWaitHandle throw the following:
            //   Specified method is not supported. at System.Net.Browser.OHWRAsyncResult.get_AsyncWaitHandle() 

            // DAVID: i don't think that the async methods have a timeout parameter. we will need to build one into 
            // it. will not be terribly clean as it will prolly have to span both the throttle and the cache. I will look into it


#if !SILVERLIGHT
            ThreadPool.RegisterWaitForSingleObject(
                    waitObject: result.AsyncWaitHandle,
                    callBack: (state, isTimedOut) =>
                        {
                            if (!isTimedOut) return;
                            if (state.GetType() != typeof(RequestHolder)) return;

                            var rh = (RequestHolder)state;
                            Log.Error(string.Format("Aborting #{0} : {1} because it has exceeded timeout {2}", rh.RequestIndex, rh.WebRequest.RequestUri, rh.RequestTimeout));
                            rh.WebRequest.Abort();
                        },
                    state: request,
                    timeout: request.RequestTimeout,
                    executeOnlyOnce: true);
#endif
        }



        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // TODO: wait for pending requests?
            }



        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}