using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Common.Logging;
using Newtonsoft.Json;

namespace CityIndex.JsonClient
{
    /// <summary>
    /// A thread-safe, self purging cache of <see cref="CacheItem{TDTO}"/>
    /// </summary>
    public class RequestCache : IRequestCache
    {

        #region Fields

        private static readonly ILog Log = LogManager.GetLogger(typeof(RequestCache));
        private readonly TimeSpan _defaultCacheDuration;
        private readonly Dictionary<string, CacheItemBase> _items;
        private readonly object _lock;

        #endregion

        #region cTor

        

        /// <summary>
        /// Instantiates a <see cref="RequestCache"/> with supplied <paramref name="defaultCacheDuration"/>
        /// </summary>
        /// <param name="defaultCacheDuration">The default cache lifespan to apply to <see cref="CacheItem{TDTO}"/></param>
        
        public RequestCache(TimeSpan defaultCacheDuration)
        {
            

            _defaultCacheDuration = defaultCacheDuration;
            _lock = new object();
            _items = new Dictionary<string, CacheItemBase>();
        }

        #endregion

        #region IRequestCache Members

        /// <summary>
        /// Gets or creates a <see cref="CacheItem{TDTO}"/> for supplied url (case insensitive).
        /// If a matching <see cref="CacheItem{TDTO}"/> is found but has expired, it is replaced with a new <see cref="CacheItem{TDTO}"/>.
        /// </summary>
        /// <typeparam name="TDTO"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public CacheItem<TDTO> GetOrCreate<TDTO>(string url)
        {
            //lock (_lock)
            {
                url = url.ToLower();

                EnsureItemCurrency(url);

                return _items.ContainsKey(url)
                           ? GetItem<TDTO>(url)
                           : CreateAndAddItem<TDTO>(url);
            }
        }


        /// <summary>
        /// Returns a <see cref="CacheItem{TDTO}"/> keyed by url (case insensitive)
        /// </summary>
        /// <typeparam name="TDTO"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException">If url is not found in internal map</exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public CacheItem<TDTO> Get<TDTO>(string url)
        {
            //lock (_lock)
            {
                url = url.ToLower();
                if (_items.ContainsKey(url))
                {
                    return (CacheItem<TDTO>)_items[url];
                }
                throw new KeyNotFoundException("item for " + url + " was not found in the cache");
            }
        }

        /// <summary>
        /// Removes a <see cref="CacheItem{TDTO}"/> from the internal map
        /// </summary>
        /// <typeparam name="TDTO"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">
        /// If item is not completed, removing would result in orphaned callbacks effectively stalling the calling code.
        /// </exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public CacheItem<TDTO> Remove<TDTO>(string url)
        {
            //lock (_lock)
            {
                url = url.ToLower();
                CacheItem<TDTO> item = Get<TDTO>(url);
                if (item.ItemState != CacheItemState.Complete)
                {
                    throw new InvalidOperationException(
                        "Item is not completed. Removing would orphan asynchronous callbacks.");
                }
                _items.Remove(url);
                return item;
            }
        }

        #endregion

        #region Private implementation



        /// <summary>
        /// Is called on the purge timer to remove completed and expired <see cref="CacheItem{TDTO}"/> from the internal map.
        /// </summary>
        /// <param name="ignored"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void PurgeExpiredItems(object ignored)
        {
            //lock (_lock)
            {
                var toRemove = new List<string>();

                // ReSharper disable LoopCanBeConvertedToQuery
                foreach (var item in _items)
                // ReSharper restore LoopCanBeConvertedToQuery
                {
                    if (item.Value.Expiration <= DateTimeOffset.UtcNow &&
                        item.Value.ItemState == CacheItemState.Complete)
                    {
                        toRemove.Add(item.Key);
                    }
                }

                foreach (string item in toRemove)
                {
                    _items.Remove(item);
                    Log.Info(string.Format("Removed {0} from cache", item));
                }
            }
        }

        /// <summary>
        /// Creates and returns an empty <see cref="CacheItem{TDTO}"/> with default values and adds it to the internal map
        /// </summary>
        /// <typeparam name="TDTO"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        private CacheItem<TDTO> CreateAndAddItem<TDTO>(string url)
        {
            var item = new CacheItem<TDTO>
                           {
                               ItemState = CacheItemState.New,
                               CacheDuration = _defaultCacheDuration
                           };

            _items.Add(url, item);

            return item;
        }


        /// <summary>
        /// Fetches a <see cref="CacheItem{TDTO}"/> from internal map and blocks if 
        /// the <see cref="CacheItem{TDTO}"/> callbacks are being processed
        /// </summary>
        /// <typeparam name="TDTO"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        private CacheItem<TDTO> GetItem<TDTO>(string url)
        {
            var item = (CacheItem<TDTO>)_items[url];

            if (item.ItemState == CacheItemState.Processing)
            {
                // if currently processing callbacks we need to block
                item.ProcessingComplete += CacheItemProcessingComplete;
                item.ProcessingWaitHandle.WaitOne(); // TODO: timeout and throw if necessary
            }
            return item;
        }

        /// <summary>
        /// Signals processing complete on a <see cref="CacheItem{TDTO}"/> and cleans up the handler delegate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void CacheItemProcessingComplete(object sender, EventArgs e)
        {
            var item = (CacheItemBase)sender;
            item.ProcessingComplete -= CacheItemProcessingComplete;
            item.ProcessingWaitHandle.Set();
        }

        /// <summary>
        /// Finds a <see cref="CacheItem{TDTO}"/> keyed by url (case insensitive), if found, is completed and expired, removes the item.
        /// </summary>
        /// <param name="url"></param>
        private void EnsureItemCurrency(string url)
        {
            bool itemIsExpired = _items.ContainsKey(url) && _items[url].ItemState == CacheItemState.Complete
                                 && _items[url].Expiration <= DateTimeOffset.UtcNow;

            if (itemIsExpired)
            {
                _items.Remove(url);
            }
        }

        #endregion
    }
}