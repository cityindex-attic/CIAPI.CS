using System;
using System.Collections.Generic;

namespace Salient.HTTPArchiveModel
{
    /// <summary>
    /// This object contains detailed info about performed request.
    /// </summary>
    [Serializable]
    public class Request
    {
        public Request()
        {
 
            headersSize = -1;
            bodySize = -1;
        }

        #region Extended properties not in spec but transparent to applications not looking for them
        /// <summary>
        /// 
        /// </summary>
        public string host { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string path { get; set; }
        #endregion



        /// <summary>
        /// method [string]
        /// Request method (GET, POST, ...).
        /// </summary>
        public string method { get; set; }

        /// <summary>
        /// url [string]
        /// Absolute URL of the request (fragments are not included).
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// httpVersion [string]
        /// Request HTTP Version.
        /// </summary>
        public string httpVersion { get; set; }

        /// <summary>
        /// cookies [array]
        /// List of cookie objects.
        /// </summary>
        public List<Cookie> cookies { get; set; }

        /// <summary>
        /// headers [array]
        /// List of header objects.
        /// </summary>
        public List<NameValuePair> headers { get; set; }

        /// <summary>
        /// queryString [array]
        /// List of query parameter objects.
        /// </summary>
        public List<NameValuePair> queryString { get; set; }

        /// <summary>
        /// postData [object, optional]
        /// Posted data info.
        /// </summary>
        public PostData postData { get; set; }

        /// <summary>
        /// headersSize [number]
        /// Total number of bytes from the start of the HTTP request message until 
        /// (and including) the double CRLF before the body. 
        /// Set to -1 if the info is not available.
        /// </summary>
        public int headersSize { get; set; }

        /// <summary>
        /// bodySize [number]
        /// Size of the request body (POST data payload) in bytes. 
        /// Set to -1 if the info is not available.
        /// </summary>
        public int bodySize { get; set; }

        /// <summary>
        /// comment [string, optional] (new in 1.2)
        /// A comment provided by the user or the application.
        /// </summary>
        public string comment { get; set; }
    }
}