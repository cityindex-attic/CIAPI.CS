using System;

namespace Salient.HTTPArchiveModel
{
    [Serializable]
    public class CacheItem
    {
        /// <summary>
        /// expires [string, optional]
        /// Expiration time of the cache entry.
        /// </summary>
        public string expires { get; set; }

        /// <summary>
        /// lastAccess [string]
        /// The last time the cache entry was opened.
        /// </summary>
        public string lastAccess { get; set; }

        /// <summary>
        /// eTag [string]
        /// Etag
        /// </summary>
        public string eTag { get; set; }

        /// <summary>
        /// hitCount [number]
        /// The number of times the cache entry has been opened.
        /// </summary>
        public int hitCount { get; set; }

        /// <summary>
        /// comment [string, optional] (new in 1.2) - 
        /// A comment provided by the user or the application.
        /// </summary>
        public string comment { get; set; }
    }
}