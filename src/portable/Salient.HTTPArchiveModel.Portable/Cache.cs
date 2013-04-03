using System;
using System.Runtime.Serialization;
namespace Salient.HTTPArchiveModel
{
    /// <summary>
    /// This objects contains info about a request coming from browser cache.
    /// </summary>
    [DataContract]
    public class Cache
    {
        public Cache()
        {
 
        }

        /// <summary>
        /// beforeRequest [object, optional]
        /// State of a cache entry before the request. 
        /// Leave out this field if the information is not available.
        /// </summary>
        [DataMember(Name = "beforeRequest")]
        public virtual CacheItem BeforeRequest { get; set; }

        /// <summary>
        /// afterRequest [object, optional]
        /// State of a cache entry after the request. 
        /// Leave out this field if the information is not available.
        /// </summary>
        [DataMember(Name = "afterRequest")]
        public virtual CacheItem AfterRequest { get; set; }

        /// <summary>
        /// comment [string, optional] (new in 1.2)
        /// A comment provided by the user or the application.
        /// </summary>
        [DataMember(Name = "comment")]
        public virtual string Comment { get; set; }
    }
}