using System;
using System.Net;

namespace CIAPI.Core
{
    public interface IThrottedRequestQueue
    {
        int DispatchedCount { get; }
        /// <summary>
        /// The maximum number of allowed pending request.
        /// 
        /// The throttle window will keep us in compliance with the 
        /// letter of the law, but testing has shown that a large 
        /// number of outstanding requests result in a cascade of 
        /// (500) errors that does not stop. 
        /// 
        /// So we will block while there are > MaxPendingRequests 
        /// regardless of throttle window.
        /// 
        /// Defaults to 15 which has proven to be reliable.
        /// </summary>
        int MaxPendingRequests { get; }
        /// <summary>
        /// If you are interested in monitoring
        /// </summary>
        int PendingRequests { get; }
        /// <summary>
        /// The quantitive portion (xxx) of the of 30 requests per 5 seconds
        /// Defaults to published guidelines of 5 seconds
        /// </summary>
        int ThrottleWindowCount { get;}
        /// <summary>
        /// The temporal portion (yyy) of the of 30 requests per 5 seconds
        /// Defaults to the published guidelines of 30
        /// </summary>
        TimeSpan ThrottleWindowTime { get; }
        
        void Enqueue(string url, WebRequest request, Action<IAsyncResult,RequestHolder> action);
 
    }
}