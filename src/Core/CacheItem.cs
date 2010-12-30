using System;
using System.Collections.Generic;
using System.Net;

namespace CIAPI.Core
{
    public enum CacheItemState
    {
        New,
        Pending,
        Processing,
        Complete
    }
    public class CacheItemBase
    {
        public string ResponseText { get; set; }
        public DateTimeOffset Expiration { get; set; }
        public TimeSpan CacheDuration { get; set; }
        public CacheItemState ItemState { get; set; }
        public Exception Exception { get; set; }
        public event EventHandler ProcessingComplete;

        public void OnProcessingComplete(EventArgs e)
        {
            EventHandler handler = ProcessingComplete;
            if (handler != null) handler(this, e);
        }
    }

    public class CacheItem<TDTO> : CacheItemBase where TDTO : class,new()
    {


        public CacheItem()
        {
            _callbacks = new Queue<CacheCallBack<TDTO>>();
        }

        private readonly Queue<CacheCallBack<TDTO>> _callbacks;

        public void AddCallback(ApiAsyncCallback<TDTO> cb, object state)
        {
            lock (_callbacks)
            {
                _callbacks.Enqueue(new CacheCallBack<TDTO>
                    {
                        Callback = cb, 
                        State = state
                    });
            }
        }

        public void CompleteResponse(string json, Exception exception)
        {
            lock (_callbacks)
            {
                ResponseText = json;
                ItemState = CacheItemState.Processing;

                try
                {
                    while (_callbacks.Count > 0)
                    {
                        CacheCallBack<TDTO> callback = _callbacks.Dequeue();
                        new ApiAsyncResult<TDTO>(callback.Callback, callback.State, true, ResponseText, exception);
                    }
                }
                finally
                {
                    Expiration = DateTimeOffset.UtcNow.Add(CacheDuration);
                    ItemState = CacheItemState.Complete;
                    OnProcessingComplete(EventArgs.Empty);
                }
            }
        }
    }
}