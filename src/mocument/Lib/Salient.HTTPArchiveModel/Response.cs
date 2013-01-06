using System;
using System.Collections.Generic;

namespace Salient.HTTPArchiveModel
{
    /// <summary>
    /// This object contains detailed info about the response.
    /// </summary>
    [Serializable]
    public class Response
    {
        public Response()
        {
 
            headersSize = -1;
            bodySize = -1;
        }

        /// <summary>
        /// status [number]
        /// Response status.
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// statusText [string]
        /// Response status description.
        /// </summary>
        public string statusText { get; set; }

        /// <summary>
        /// httpVersion [string]
        /// Response HTTP Version.
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
        /// content [object]
        /// Details about the response body.
        /// </summary>
        public Content content { get; set; }

        /// <summary>
        /// redirectURL [string]
        /// Redirection target URL from the Location response header.
        /// </summary>
        public string redirectURL { get; set; }

        /// <summary>
        /// headersSize [number]
        /// Total number of bytes from the start of the HTTP response message until 
        /// (and including) the double CRLF before the body. 
        /// Set to -1 if the info is not available.
        /// 
        /// The size of received response-headers is computed only from headers that are 
        /// really received from the server. Additional headers appended by the browser are 
        /// not included in this number, but they appear in the list of header objects.
        /// </summary>
        public int headersSize { get; set; }

        /// <summary>
        /// bodySize [number]
        /// Size of the received response body in bytes.
        /// Set to zero in case of responses coming from the cache (304). 
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