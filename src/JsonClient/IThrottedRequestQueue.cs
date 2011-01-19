using System;
using System.Net;

namespace CityIndex.JsonClient
{
    ///<summary>
    /// Describes a self throttling asynchronous request queue.
    ///</summary>
    public interface IThrottedRequestQueue
    {
        /// <summary>
        /// The number of requests that have been dispatched
        /// </summary>
        int DispatchedCount { get; }


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
        int MaxPendingRequests { get; }

        /// <summary>
        /// The number of pending (issued) requests
        /// </summary>
        int PendingRequests { get; }

        /// <summary>
        /// The quantitive portion (xxx) of the of 30 requests per 5 seconds
        /// </summary>
        int ThrottleWindowCount { get;}

        /// <summary>
        /// The temporal portion (yyy) of the of 30 requests per 5 seconds
        /// </summary>
        TimeSpan ThrottleWindowTime { get; }


        /// <summary>
        /// Adds a request to the end of the queue.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="request"></param>
        /// <param name="action"></param>
        void Enqueue(string url, WebRequest request, Action<IAsyncResult,RequestHolder> action);
 
    }
}