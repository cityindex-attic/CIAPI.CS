using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
namespace Salient.HTTPArchiveModel
{
    /// <summary>
    /// This object contains detailed info about performed request.
    /// </summary>
    [DataContract]
    public class Request
    {
        public Request()
        {
 
            HeadersSize = -1;
            BodySize = -1;
        }

        #region Extended properties not in spec but transparent to applications not looking for them
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "host"), Obsolete]
        public virtual string Host { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "path"), Obsolete]
        public virtual string Path { get; set; }
        #endregion



        /// <summary>
        /// method [string]
        /// Request method (GET, POST, ...).
        /// </summary>
        [DataMember(Name = "method")]
        public virtual string Method { get; set; }

        /// <summary>
        /// url [string]
        /// Absolute URL of the request (fragments are not included).
        /// </summary>
        [DataMember(Name = "url")]
        public virtual string Url { get; set; }

        /// <summary>
        /// httpVersion [string]
        /// Request HTTP Version.
        /// </summary>
        [DataMember(Name = "httpVersion")]
        public virtual string HttpVersion { get; set; }

        /// <summary>
        /// cookies [array]
        /// List of cookie objects.
        /// </summary>
        [DataMember(Name = "cookies")]
        public virtual List<Cookie> Cookies { get; set; }

        /// <summary>
        /// headers [array]
        /// List of header objects.
        /// </summary>
        [DataMember(Name = "headers")]
        public virtual List<NameValuePair> Headers { get; set; }

        /// <summary>
        /// queryString [array]
        /// List of query parameter objects.
        /// </summary>
        [DataMember(Name = "queryString")]
        public virtual List<NameValuePair> QueryString { get; set; }

        /// <summary>
        /// postData [object, optional]
        /// Posted data info.
        /// </summary>
        [DataMember(Name = "postData")]
        public virtual PostData PostData { get; set; }

        /// <summary>
        /// headersSize [number]
        /// Total number of bytes from the start of the HTTP request message until 
        /// (and including) the double CRLF before the body. 
        /// Set to -1 if the info is not available.
        /// </summary>
        [DataMember(Name = "headersSize")]
        public virtual int HeadersSize { get; set; }

        /// <summary>
        /// bodySize [number]
        /// Size of the request body (POST data payload) in bytes. 
        /// Set to -1 if the info is not available.
        /// </summary>
        [DataMember(Name = "bodySize")]
        public virtual int BodySize { get; set; }

        /// <summary>
        /// comment [string, optional] (new in 1.2)
        /// A comment provided by the user or the application.
        /// </summary>
        [DataMember(Name = "comment")]
        public virtual string Comment { get; set; }
    }
}