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

        
    }
}