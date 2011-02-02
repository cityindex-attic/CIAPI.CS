using System;
using System.Net;
using Common.Logging;


namespace CityIndex.JsonClient
{
    /// <summary>
    /// Default RequestFactory. Returns instances of <see cref="WebRequest"/>.
    /// </summary>
    public class RequestFactory : IRequestFactory
    {
        private const int DEFAULT_REQUEST_TIMEOUT_IN_SEC = 30;
        private static readonly ILog Log = LogManager.GetLogger(typeof(RequestFactory));
        /// <summary>
        /// The amount of time to wait for a response before throwing an exception.
        /// Defaults to 30 seconds
        /// </summary>
        public TimeSpan RequestTimeout { get; set; }

        ///<summary>
        ///</summary>
        public RequestFactory()
        {
            RequestTimeout = TimeSpan.FromSeconds(DEFAULT_REQUEST_TIMEOUT_IN_SEC);

#if !SILVERLIGHT
            DecompressionMethods = DecompressionMethods.GZip | DecompressionMethods.Deflate;
#endif
        }

#if !SILVERLIGHT
        public DecompressionMethods DecompressionMethods { get; set; }
#endif 
        ///<summary>
        /// Returns instances of <see cref="WebRequest"/>.
        ///</summary>
        ///<param name="uri"></param>
        ///<returns></returns>
        public WebRequest Create(string uri)
        {
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(uri);
#if !SILVERLIGHT
            request.Timeout = Convert.ToInt32(RequestTimeout.TotalMilliseconds);
#else
            //FIXME: Need a way to timeout requests in Silverlight
#endif


#if !SILVERLIGHT
            request.AutomaticDecompression = DecompressionMethods;
#endif


            return request;
        }

       
    }
}