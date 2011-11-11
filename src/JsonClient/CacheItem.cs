using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Common.Logging;

namespace CityIndex.JsonClient
{
    ///<summary>
    /// Provides a formatted dump of a CacheItem for inclusion in exception messages.
    ///</summary>

    /// <summary>
    /// Provides a generic composition element for request state.
    /// Contains a callback queue to service identical concurrent requests. 
    /// i.e. if, while a request is in process, an identical request is made,
    /// the caller may simply add the callback to this item and let the single
    /// request service both.
    /// </summary>
    /// <typeparam name="TDTO">The expected DTO type</typeparam>
    public class CacheItem<TDTO> : CacheItemBase 
    {
        

        private readonly Queue<CacheCallBack<TDTO>> _callbacks;

        ///<summary>
        ///</summary>
        public CacheItem()
        {
            _callbacks = new Queue<CacheCallBack<TDTO>>();
        }

        /// <summary>
        /// Adds an <see cref="ApiAsyncCallback{TDTO}"/> to be serviced when this item's 
        /// request completes.
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddCallback(ApiAsyncCallback<TDTO> callback, object state)
        {
            //lock (_callbacks)
            {
                _callbacks.Enqueue(new CacheCallBack<TDTO>
                    {
                        Callback = callback,
                        State = state
                    });
            }
        }

        /// <summary>
        /// Executes and dequeues each queued <see cref="ApiAsyncCallback{TDTO}"/> in order and then signals completion.
        /// </summary>
        /// <param name="json"></param>
        /// <param name="exception"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void CompleteResponse(string json, Exception exception)
        {
            //lock (_callbacks)
            {
                ResponseText = json;
                ItemState = CacheItemState.Processing;



                try
                {
                    while (_callbacks.Count > 0)
                    {
                        CacheCallBack<TDTO> callback = _callbacks.Dequeue();
                        if (exception != null)
                        {
                            
                        }
                        
                        new ApiAsyncResult<TDTO>(callback.Callback, callback.State, true, ResponseText, exception);

                    }
                }
                
                finally
                {
                    Expiration = DateTimeOffset.UtcNow.Add(CacheDuration);
                    ItemState = CacheItemState.Complete;
                    OnProcessingComplete();
                }
            }
        }
    }
}