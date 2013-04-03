using System.Runtime.Serialization;

namespace Salient.HTTPArchiveModel
{
    /// <summary>
    /// </summary>
    [DataContract]
    public class Cookie
    {
        /// <summary>
        ///     name [string]
        ///     The name of the cookie.
        /// </summary>
        [DataMember(Name = "name")]
        public virtual string Name { get; set; }

        /// <summary>
        ///     value [string]
        ///     The cookie value.
        /// </summary>
        [DataMember(Name = "value")]
        public virtual string Value { get; set; }

        /// <summary>
        ///     path [string, optional]
        ///     The path pertaining to the cookie.
        /// </summary>
        [DataMember(Name = "path")]
        public virtual string Path { get; set; }

        /// <summary>
        ///     domain [string, optional]
        ///     The host of the cookie.
        /// </summary>
        [DataMember(Name = "domain")]
        public virtual string Domain { get; set; }

        /// <summary>
        ///     expires [string, optional]
        ///     Cookie expiration time.
        ///     (ISO 8601 - YYYY-MM-DDThh:mm:ss.sTZD, e.g. 2009-07-24T19:20:30.123+02:00).
        /// </summary>
        [DataMember(Name = "expires")]
        public virtual string Expires { get; set; }

        /// <summary>
        ///     httpOnly [boolean, optional]
        ///     Set to true if the cookie is HTTP only, false otherwise.
        /// </summary>
        [DataMember(Name = "httpOnly")]
        public virtual bool HttpOnly { get; set; }

        /// <summary>
        ///     secure [boolean, optional] (new in 1.2)
        ///     True if the cookie was transmitted over ssl, false otherwise.
        /// </summary>
        [DataMember(Name = "secure")]
        public virtual bool Secure { get; set; }

        /// <summary>
        ///     comment [string, optional] (new in 1.2)
        ///     A comment provided by the user or the application.
        /// </summary>
        [DataMember(Name = "comment")]
        public virtual string Comment { get; set; }
    }
}