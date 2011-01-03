using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using log4net;

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
        private Timer _timer;

        #endregion

        #region Constructors

        /// <summary>
        /// Insantiates a <see cref="ThrottedRequestQueue"/> with default parameters.
        /// throttleWindowTime = 5 seconds
        /// throttleWindowCount = 30
        /// maxPendingRequests = 10
        /// </summary>
        public ThrottedRequestQueue()
            : this(TimeSpan.FromSeconds(5), 30, 10)
        {
        }

        /// <summary>
        /// Insantiates a <see cref="ThrottedRequestQueue"/> with supplied parameters.
        /// </summary>
        /// <param name="throttleWindowTime">The window in which to restrice issued requests to <paramref name="throttleWindowCount"/></param>
        /// <param name="throttleWindowCount">The maximum number of requests to issue in the amount of time described by <paramref name="throttleWindowTime"/></param>
        /// <param name="maxPendingRequests">The maximum allowed number of active requests.</param>
        public ThrottedRequestQueue(TimeSpan throttleWindowTime, int throttleWindowCount, int maxPendingRequests)
        {
            _throttleWindowTime = throttleWindowTime;
            _throttleWindowCount = throttleWindowCount;
            _maxPendingRequests = maxPendingRequests;
            _timer = new Timer(ProcessQueue, null, 100, 50);
        }

        #endregion

        #region IThrottedRequestQueue Members

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
        public void Enqueue(string url, WebRequest request, Action<IAsyncResult, RequestHolder> action)
        {
            lock (_requests)
            {
                // TODO: have a max queue length to keep things from getting out of hand - THEN we can throw an exception
                _requests.Enqueue(new RequestHolder
                    {
                        WebRequest = request,
                        Url = url,
                        AsyncResultHandler = action
                    });
            }
        }

        #endregion

        private void ProcessQueue(object ignored)
        {
            lock (_requests)
            {
                if (_processingQueue)
                {
                    return;
                }

                if (_requests.Count == 0)
                {
                    return;
                }

                RequestHolder request = _requests.Peek();

                _processingQueue = true;

                try
                {
                    if (_outstandingRequests > MaxPendingRequests)
                    {
                        if (!_notifiedWaitingOnMaxPending)
                        {
                            string msgMaxPending = string.Format("Waiting: pending requests {0}", _outstandingRequests);
                            Log.Debug(msgMaxPending);

                            _notifiedWaitingOnMaxPending = true;
                        }

                        return;
                    }

                    _notifiedWaitingOnMaxPending = false;

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
                                Log.Debug(msgWaiting);

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


                    int requestIndex = _dispatchedCount;

                    // TODO: should not get an exception here but need to allow for one

                    request.WebRequest.BeginGetResponse(ar =>
                        {
                            string msgIssued = string.Format("Recieved #{0} : {1} ", requestIndex, request.Url);
                            Log.Debug(msgIssued);

                            _outstandingRequests--;

                            request.AsyncResultHandler(ar, request);
                        }, null);

                    string msgDispatched = string.Format("Dispatched #{0} : {1} ", _dispatchedCount, request.Url);
                    Log.Debug(msgDispatched);

                    _requests.Dequeue();

                    _outstandingRequests++;
                }
                finally
                {
                    _processingQueue = false;
                }
            }
        }
    }
}