using System;
using System.Net;

namespace CityIndex.JsonClient
{
    /// <summary>
    /// Composition element for request related fields.
    /// </summary>
    public class RequestHolder
    {
        /// <summary>
        /// The url of the request
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// The request instance
        /// </summary>
        public WebRequest WebRequest { get; set; }

        /// <summary>
        /// The handler
        /// </summary>
        public Action<IAsyncResult, RequestHolder> AsyncResultHandler { get; set; }

        ///<summary>
        ///</summary>
        public TimeSpan RequestTimeout
        {
            get
            {
#if !SILVERLIGHT
                return TimeSpan.FromMilliseconds(WebRequest.Timeout);
#else
    //FIXME: Need a way to set this when creating the request Silverlight (see related fix me in RequestFactory.Create
                return TimeSpan.FromMilliseconds(30*1000);
#endif
            }
        }

        ///<summary>
        ///</summary>
        public int RequestIndex { get; set; }
    }
}