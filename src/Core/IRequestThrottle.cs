using System;
using System.Net;


namespace Soapi.Net
{
    public interface IRequestThrottle
    {
        //ILog Logger { get; set; }
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
        int MaxPendingRequests { get; set; }
        /// <summary>
        /// If you are interested in monitoring
        /// </summary>
        int OutstandingRequests { get; }
        /// <summary>
        /// The quantitive portion (xxx) of the of 30 requests per 5 seconds
        /// Defaults to published guidelines of 5 seconds
        /// </summary>
        int ThrottleWindowCount { get; set; }
        /// <summary>
        /// The temporal portion (yyy) of the of 30 requests per 5 seconds
        /// Defaults to the published guidelines of 30
        /// </summary>
        TimeSpan ThrottleWindowTime { get; set; }
        /// <summary>
        /// This decrements the outstanding request count.
        /// 
        /// This MUST MUST MUST be called when a request has 
        /// completed regardless of status.
        /// 
        /// If a request fails, it may be wise to delay calling 
        /// this, e.g. cool down, for a few seconds, before 
        /// reissuing the request.
        /// </summary>
        void Complete();
        /// <summary>
        /// Create a WebRequest. This method will block if too many
        /// outstanding requests are pending or the throttle window
        /// threshold has been reached.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        WebRequest Create(Uri uri);
        /// <summary>
        /// Create a WebRequest. This method will block if too many
        /// outstanding requests are pending or the throttle window
        /// threshold has been reached.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        WebRequest Create(string url);
    }
}