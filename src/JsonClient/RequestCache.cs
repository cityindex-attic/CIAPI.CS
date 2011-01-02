using System;
using System.Collections.Generic;
using System.Threading;
using log4net;

namespace CityIndex.JsonClient
{
    public class RequestCache
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(RequestCache));

        public RequestCache(TimeSpan purgeInterval)
        {
            _lock = new object();
            _items = new Dictionary<string, CacheItemBase>();
            new Timer(PurgeExpiredItems, null, TimeSpan.FromMilliseconds(10), purgeInterval);
        }

        public RequestCache()
        {
            _lock = new object();
            _items = new Dictionary<string, CacheItemBase>();
            new Timer(PurgeExpiredItems, null, TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(10));
        }


        private readonly object _lock;
        private readonly Dictionary<string, CacheItemBase> _items;

        private void PurgeExpiredItems(object ignored)
        {
            lock (_lock)
            {
                var toRemove = new List<string>();

                foreach (KeyValuePair<string, CacheItemBase> item in _items)
                {
                    if (item.Value.Expiration <= DateTimeOffset.UtcNow && item.Value.ItemState == CacheItemState.Complete)
                    {
                        toRemove.Add(item.Key);
                    }
                }

                foreach (var item in toRemove)
                {
                    _items.Remove(item);
                }

            }
        }

        public CacheItem<TDTO> GetOrCreateCacheItem<TDTO>(string url, ApiAsyncCallback<TDTO> cb, object state, TimeSpan cacheDuration) where TDTO : class,new()
        {
            lock (_lock)
            {
                url = url.ToLower();

                if (_items.ContainsKey(url) && _items[url].ItemState == CacheItemState.Complete && _items[url].Expiration <= DateTimeOffset.UtcNow)
                {
                    _items.Remove(url);
                }

                CacheItem<TDTO> item;

                if (_items.ContainsKey(url))
                {
                    item = (CacheItem<TDTO>)_items[url];

                    // if currently processing callbacks we need to wait

                    if (item.ItemState == CacheItemState.Processing)
                    {
                        using (var handle = new AutoResetEvent(false))
                        {
                            item.ProcessingComplete += (s, e) => handle.Set();
                            handle.WaitOne();
                        }
                    }

                    if (item.ItemState == CacheItemState.Pending)
                    {
                        // already working on this request, add our callback to the item
                        item.AddCallback(cb, state);
                    }

                }
                else
                {
                    item = new CacheItem<TDTO>
                    {
                        ItemState = CacheItemState.New,
                        CacheDuration = cacheDuration
                    };
                    item.AddCallback(cb, state);
                    Add(url, item);
                }

                return item;
            }
        }


        public void Add<TDTO>(string url, CacheItem<TDTO> item) where TDTO : class,new()
        {
            lock (_lock)
            {
                if (_items.ContainsKey(url))
                {
                    _items.Remove(url);
                }
                url = url.ToLower();
                _items.Add(url, item);
            }
        }

        /// <summary>
        /// NOTE: This method should only be called for items that are expected to be in the cache, typically those with state of pending.
        /// If not found, an exception is thown.
        /// </summary>
        /// <typeparam name="TDTO"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        public CacheItem<TDTO> Get<TDTO>(string url) where TDTO : class,new()
        {
            lock (_lock)
            {
                url = url.ToLower();
                if (_items.ContainsKey(url))
                {
                    return (CacheItem<TDTO>)_items[url];
                }
                throw new Exception("item for " + url + " was not found in the cache");
            }
        }

        public CacheItem<TDTO> Remove<TDTO>(string url) where TDTO : class,new()
        {
            lock (_lock)
            {
                url = url.ToLower();
                var item = Get<TDTO>(url);
                _items.Remove(url);
                return item;
            }
        }
    }
}