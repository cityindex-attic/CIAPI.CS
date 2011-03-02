using System;
using System.Collections.Generic;

namespace CityIndex.JsonClient
{
    /// <summary>
    /// A thread-safe, self purging cache of <see cref="CacheItem{TDTO}"/>
    /// </summary>
    public interface IRequestCache : IDisposable
    {
        /// <summary>
        /// Gets or creates a <see cref="CacheItem{TDTO}"/> for supplied url (case insensitive).
        /// If a matching <see cref="CacheItem{TDTO}"/> is found but has expired, it is replaced with a new <see cref="CacheItem{TDTO}"/>.
        /// </summary>
        /// <typeparam name="TDTO"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        CacheItem<TDTO> GetOrCreate<TDTO>(string url) where TDTO : class,new();

        /// <summary>
        /// Returns a <see cref="CacheItem{TDTO}"/> keyed by url (case insensitive)
        /// </summary>
        /// <typeparam name="TDTO"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException">If url is not found in internal map</exception>
        CacheItem<TDTO> Get<TDTO>(string url) where TDTO : class,new();

        /// <summary>
        /// Removes a <see cref="CacheItem{TDTO}"/> from the internal map
        /// </summary>
        /// <typeparam name="TDTO"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">
        /// If item is not completed, removing would result in orphaned callbacks effectively stalling the calling code.
        /// </exception>
        CacheItem<TDTO> Remove<TDTO>(string url) where TDTO : class,new();
    }
}