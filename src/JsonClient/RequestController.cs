using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CityIndex.JsonClient
{
    ///<summary>
    /// Default throttle scope manager implementation
    ///</summary>
    public class RequestController : IRequestController
    {
        private const int BackgroundInterval = 50;
        private readonly Thread _backgroundThread;
        private readonly IRequestCache _cache;
        private readonly object _lock = new object();
        private readonly IRequestFactory _requestFactory;
        private readonly int _retryCount;
        private readonly Dictionary<string, IThrottedRequestQueue> _scopes;
        private readonly AutoResetEvent _waitHandle;

        private bool _disposed;
        private volatile bool _disposing;

        ///<summary>
        ///</summary>
        public RequestController(TimeSpan defaultCacheDuration, int retryCount,IRequestFactory requestFactory, params IThrottedRequestQueue[] scopes)
        {
            _requestFactory = requestFactory;
            _retryCount = retryCount;
            _scopes = scopes.ToDictionary(d => d.Scope);
            _cache = new RequestCache(defaultCacheDuration);

            _waitHandle = new AutoResetEvent(false);
            _backgroundThread = new Thread(BackgroundProcess);
            _backgroundThread.Start();
        }

        public IRequestFactory RequestFactory
        {
            get { return _requestFactory; }
        }

        ///<summary>
        ///</summary>
        public event EventHandler<CacheItemEventArgs> BeforeIssueRequest;

        ///<summary>
        ///</summary>
        public event EventHandler<CacheItemEventArgs> BeforeBuildUrl;

        ///<summary>
        ///</summary>
        ///<param name="target"></param>
        ///<param name="uriTemplate"></param>
        ///<param name="method"></param>
        ///<param name="parameters"></param>
        ///<param name="cacheDuration"></param>
        ///<param name="throttleScope"></param>
        ///<param name="url"></param>
        ///<param name="cb"></param>
        ///<param name="state"></param>
        ///<typeparam name="TDTO"></typeparam>
        public void ProcessCacheItem<TDTO>(string target, string uriTemplate, string method,
                                           Dictionary<string, object> parameters, TimeSpan cacheDuration,
                                           string throttleScope, string url, ApiAsyncCallback<TDTO> cb, object state)
            where TDTO : class, new()
        {
            CacheItem<TDTO> item = Cache.GetOrCreate<TDTO>(url);

            switch (item.ItemState)
            {
                case CacheItemState.New:

                    item.AddCallback(cb, state);
                    item.CacheDuration = cacheDuration;
                    item.Method = method;
                    item.Parameters = parameters;
                    item.Target = target;
                    item.ThrottleScope = throttleScope;
                    item.UriTemplate = uriTemplate;
                    item.Url = url;

                    WebRequest request = RequestFactory.Create(url);
                    request.Method = method.ToUpper();

#if !SILVERLIGHT
                    // silverlight crossdomain request does not support content type (?!)
                    request.ContentType = "application/json";
#endif
                    item.ItemState = CacheItemState.Pending;
                    CreateRequest<TDTO>(url);
                    break;
                case CacheItemState.Pending:
                    item.AddCallback(cb, state);
                    break;
                case CacheItemState.Complete:
                    new ApiAsyncResult<TDTO>(cb, state, true, item.ResponseText, null);
                    break;
            }
        }

        public int RetryCount
        {
            get { return _retryCount; }
        }

        public IRequestCache Cache
        {
            get { return _cache; }
        }

        public IThrottedRequestQueue this[string key]
        {
            get { return _scopes[key]; }
        }

        public void CreateRequest<TDTO>(string url)
            where TDTO : class, new()
        {
            CacheItem<TDTO> item = _cache.Get<TDTO>(url);


            item.Request = _requestFactory.Create(url);
            item.Request.Method = item.Method.ToUpper();

            OnBeforeIssueRequest(new CacheItemEventArgs(item));

            if (item.Method.ToUpper() == "POST")
            {
                SetPostEntityAndEnqueueRequest<TDTO>(url);
            }
            else
            {
                EnqueueRequest<TDTO>(url);
            }
        }

        protected virtual void OnBeforeIssueRequest(CacheItemEventArgs e)
        {
            EventHandler<CacheItemEventArgs> handler = BeforeIssueRequest;
            if (handler != null) handler(this, e);
        }

        protected virtual void OnBeforeBuildUrl(CacheItemEventArgs e)
        {
            EventHandler<CacheItemEventArgs> handler = BeforeBuildUrl;
            if (handler != null) handler(this, e);
        }

        private void BackgroundProcess(object ignored)
        {
            while (true)
            {
                lock (_lock)
                {
                    // passive shut down of thread to avoid spurious ThreadAbortException from
                    // popping up in arbitrary places as is wont to happen when just killing a thread.
                    if (_disposing)
                    {
                        return;
                    }

                    // TODO: how/why/should we surface exceptions?
                    // Any exceptions leaking from this point should be critical
                    // unhandled exceptions. Most exceptions will just be passed to the
                    // async completion callbacks.

                    Cache.PurgeExpiredItems(null);


                    foreach (var scope in _scopes)
                    {
                        scope.Value.ProcessQueue(null);
                    }
                }

                _waitHandle.WaitOne(BackgroundInterval);
            }
        }

        /// <summary>
        /// Builds a JSOB from parameters and asynchronously feeds the request stream with the resultant JSON before sending the request
        /// </summary>
        /// <param name="url"></param>
        private void SetPostEntityAndEnqueueRequest<TDTO>(string url)
            where TDTO : class, new()
        {
            CacheItem<TDTO> item = _cache.Get<TDTO>(url);


            byte[] bodyValue = CreatePostEntity(item.Parameters);

            item.Request.BeginGetRequestStream(ac =>
                {
                    lock (_lock)
                    {
                        try
                        {
                            using (Stream requestStream = item.Request.EndGetRequestStream(ac))
                            {
                                requestStream.Write(bodyValue, 0, bodyValue.Length);
                            }

                            EnqueueRequest<TDTO>(url);
                        }
                        catch (Exception ex)
                        {
                            try
                            {
                                item.CompleteResponse(null, ApiException.Create(ex));
                            }
                            finally
                            {
                                _cache.Remove<TDTO>(item.Url);
                            }
                        }
                    }
                }, null);
        }

        /// <summary>
        /// Builds a JSON object from a dictionary and returns encoded bytes
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private static byte[] CreatePostEntity(Dictionary<string, object> parameters)
        {
            var body = new JObject();
            foreach (var kvp in parameters)
            {
                object value = kvp.Value;

                if (value is Guid)
                {
                    // HACK: to deal with "System.ArgumentException : Could not determine JSON object type for type System.Guid."
                    value = value.ToString();
                }

                if (value != null)
                {
                    body[kvp.Key] = new JValue(value);
                }
            }

            byte[] bodyValue = Encoding.UTF8.GetBytes(body.ToString(Formatting.None, new JsonConverter[] { }));

            return bodyValue;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TDTO"></typeparam>
        private void EnqueueRequest<TDTO>(string url)
            where TDTO : class, new()
        {
            CacheItem<TDTO> outerItem = _cache.Get<TDTO>(url);
            IThrottedRequestQueue throttle = this[outerItem.ThrottleScope];

            WebRequest request = outerItem.Request;

            throttle.Enqueue(url, request, (ar, requestHolder) =>
                {
                    lock (_lock)
                    {
                        RequestHolder holder = requestHolder;

                        // the item had better be in the cache because if it is not
                        // and this throws then we have a deadlock as the callbacks are int
                        // the cache item that does not exist so no where to send the exception.
                        // TODO: add an OnException event to this class

                        CacheItem<TDTO> item = _cache.Get<TDTO>(url);

                        WebResponse response = null;

                        try
                        {
                            using (response = holder.WebRequest.EndGetResponse(ar))
                            using (Stream stream = response.GetResponseStream())
                            // ReSharper disable AssignNullToNotNullAttribute
                            using (var reader = new StreamReader(stream))
                            // ReSharper restore AssignNullToNotNullAttribute
                            {
                                string json = reader.ReadToEnd();

                                // TODO: check json for exception. question is: how to get the type in? a factory class that accepts json?
                                Exception seralizedException = null;

                                try
                                {
                                    item.CompleteResponse(json, seralizedException);
                                }
                                catch (Exception ex)
                                {
                                    // TODO: test this
                                    _cache.Remove<TDTO>(item.Url);
                                    throw new ResponseHandlerException("Unhandled exception in caller's response handler", ex);
                                }
                            }
                        }
                        catch (WebException wex)
                        {
                            bool shouldRetry =
                                new RequestRetryDiscriminator().ShouldRetry(wex);

                            if (shouldRetry && item.RetryCount < RetryCount)
                            {
                                item.RetryCount++;
                                item.ItemState = CacheItemState.Pending;

                                if (response != null)
                                {
                                    response.Close();
                                }

                                CreateRequest<TDTO>(url);
                            }
                            else
                            {
                                ApiException exception;

                                if (item.RetryCount > 0)
                                {
                                    exception =
                                        new ApiException(
                                            wex.Message +
                                            String.Format("\r\nretried {0} times", item.RetryCount) + "\r\nREQUEST INFO:\r\n" + item.ToString(), wex);
                                }
                                else
                                {
                                    exception =
                                        new ApiException(
                                            wex.Message + "\r\nREQUEST INFO:\r\n" + item.ToString(), wex);
                                }

                                try
                                {
                                    item.CompleteResponse(null, exception);
                                }
                                finally
                                {
                                    _cache.Remove<TDTO>(item.Url);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            try
                            {
                                item.CompleteResponse(null, new ApiException(ex.Message + "\r\nREQUEST INFO:\r\n" + item.ToString(), ex));
                            }
                            finally
                            {
                                _cache.Remove<TDTO>(item.Url);
                            }
                        }
                    }
                });
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _disposing = true;
                    while (_backgroundThread.IsAlive)
                    {
                        Thread.Sleep(100);
                    }
                }

                _disposed = true;
            }
        }
    }
}