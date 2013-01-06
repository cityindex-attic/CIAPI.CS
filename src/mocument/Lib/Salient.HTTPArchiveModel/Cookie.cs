using System;

namespace Salient.HTTPArchiveModel
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class Cookie
    {
        /// <summary>
        /// name [string]
        /// The name of the cookie.
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// value [string]
        /// The cookie value.
        /// </summary>
        public string value { get; set; }

        /// <summary>
        /// path [string, optional]
        /// The path pertaining to the cookie.
        /// </summary>
        public string path { get; set; }

        /// <summary>
        /// domain [string, optional]
        /// The host of the cookie.
        /// </summary>
        public string domain { get; set; }

        /// <summary>
        /// expires [string, optional]
        /// Cookie expiration time. 
        /// (ISO 8601 - YYYY-MM-DDThh:mm:ss.sTZD, e.g. 2009-07-24T19:20:30.123+02:00).
        /// </summary>
        public string expires { get; set; }

        /// <summary>
        /// httpOnly [boolean, optional]
        /// Set to true if the cookie is HTTP only, false otherwise.
        /// </summary>
        public bool httpOnly { get; set; }

        /// <summary>
        /// secure [boolean, optional] (new in 1.2)
        /// True if the cookie was transmitted over ssl, false otherwise.
        /// </summary>
        public bool secure { get; set; }

        /// <summary>
        /// comment [string, optional] (new in 1.2)
        /// A comment provided by the user or the application.
        /// </summary>
        public string comment { get; set; }
    }
}