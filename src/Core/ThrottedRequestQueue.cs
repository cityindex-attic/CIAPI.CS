using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using log4net;

namespace CIAPI.Core
{
    /// <summary>
    /// TODO: allow for pausing
    /// TODO: need to go back to a singleton and implement as a queue collection - singleton throttle is very important in real world code especially web apps that proxy for arbitrary numbers of concurrent users.
    /// would like to allow for clearing the queue but dependencies on the cache make
    /// this a non starter. the only viable ways to that means is to merge throttle and cache.
    /// this will have to be a builder type action in the calling class that can corelate the
    /// queue items and the cache items and resolve the cache items that are cleared.
    /// this will probably mean the introduction of a 'cancelled' CacheItemState
    /// </summary>
    public sealed class ThrottedRequestQueue : IThrottedRequestQueue
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ThrottedRequestQueue));

        #region Fields

        private readonly Queue<DateTimeOffset> _requestTimes = new Queue<DateTimeOffset>();
        private readonly Queue<RequestHolder> _requests = new Queue<RequestHolder>();

        private int _dispatchedCount;
        private int _outstandingRequests;
        private Timer _timer;
        private bool _processingQueue;

        public int DispatchedCount
        {
            get { return _dispatchedCount; }
        }

        #endregion

        #region Constructors

        public ThrottedRequestQueue()
            : this(TimeSpan.FromSeconds(5), 30, 10)
        {
        }

        public ThrottedRequestQueue(TimeSpan throttleWindowTime, int throttleWindowCount, int maxPendingRequests)
        {
            _throttleWindowTime = throttleWindowTime;
            _throttleWindowCount = throttleWindowCount;
            _maxPendingRequests = maxPendingRequests;
            _timer = new Timer(ProcessQueue, null, 100, 50);
        }
        #endregion

        #region Fields

        private readonly int _maxPendingRequests;
        private readonly int _throttleWindowCount;
        private readonly TimeSpan _throttleWindowTime;
        private bool _notifiedWaitingOnMaxPending;
        private bool _notifiedWaitingOnWindow;
        #endregion

        #region Properties

        /// <summary>
        ///   The maximum number of allowed pending request.
        /// 
        ///   The throttle window will keep us in compliance with the 
        ///   letter of the law, but testing has shown that a large 
        ///   number of outstanding requests can result in infrastructural
        ///   issues.
        /// 
        ///   So we will defer queue processing while there are > MaxPendingRequests 
        ///   regardless of throttle window.
        /// </summary>
        public int MaxPendingRequests
        {
            get { return _maxPendingRequests; }
        }

        /// <summary>
        ///   If you are interested in monitoring
        /// </summary>
        public int PendingRequests
        {
            get { return _outstandingRequests; }
        }

        /// <summary>
        ///   The quantitive portion (xxx) of the of 30 requests per 5 seconds
        ///   Defaults to published guidelines of 5 seconds
        /// </summary>
        public int ThrottleWindowCount
        {
            get { return _throttleWindowCount; }
        }

        /// <summary>
        ///   The temporal portion (yyy) of the of 30 requests per 5 seconds
        ///   Defaults to the published guidelines of 30
        /// </summary>
        public TimeSpan ThrottleWindowTime
        {
            get { return _throttleWindowTime; }
        }

        #endregion

        #region Public Methods


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

                var request = _requests.Peek();

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



                    var requestIndex = _dispatchedCount;

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