using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using CityIndex.ReflectiveLoggingAdapter;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace CityIndex.JsonClient
{
    public enum ContentType
    {
        JSON,

        FORM
    }

    public enum Compression
    {
        NONE,
        GZIP,
        DEFLATE
    }

    ///<summary>
    /// Default throttle scope manager implementation
    ///</summary>
    public class RequestController : IRequestController
    {
        ///<summary>
        ///</summary>
        public ContentType ContentType { get; set; }

        ///<summary>
        ///</summary>
        public Compression Compression { get; set; }
        public string BasicHttpAuthUsername { get; set; }
        public string BasicHttpAuthPassword { get; set; }

        private IJsonExceptionFactory _jsonExceptionFactory;
        private const int BackgroundInterval = 50;
        private readonly Thread _backgroundThread;
        private readonly IRequestCache _cache;
        private readonly object _lock = new object();
        private readonly IRequestFactory _requestFactory;
        private readonly int _retryCount;
        private readonly Dictionary<string, IThrottledRequestQueue> _scopes;
        private readonly AutoResetEvent _waitHandle;
        private static ILog Log = LogManager.GetLogger(typeof(RequestController));

        private bool _disposed;
        private volatile bool _disposing;

        ///<summary>
        ///</summary>
        public RequestController(TimeSpan defaultCacheDuration, int retryCount, IRequestFactory requestFactory, IJsonExceptionFactory jsonExceptionFactory, params IThrottledRequestQueue[] scopes)
        {

            _jsonExceptionFactory = jsonExceptionFactory;
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
        {
            CacheItem<TDTO> item = Cache.GetOrCreate<TDTO>(url);

            switch (item.ItemState)
            {
                case CacheItemState.New:

                    item.AddCallback(cb, state);
                    item.CacheDuration = cacheDuration.Milliseconds == -1 ? TimeSpan.FromDays(1) : cacheDuration;
                    item.Method = method;
                    item.Parameters = parameters;
                    item.Target = target;
                    item.ThrottleScope = throttleScope;
                    item.UriTemplate = uriTemplate;
                    item.Url = url;


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

        public string UserAgent { get; set; }

        public IRequestCache Cache
        {
            get { return _cache; }
        }

        public IThrottledRequestQueue this[string key]
        {
            get { return _scopes[key]; }
        }

        private static void SetBasicAuthHeader(WebRequest req, String userName, String userPassword)
        {
            string authInfo = userName + ":" + userPassword;
            authInfo = Convert.ToBase64String(Encoding.UTF8.GetBytes(authInfo));
            req.Headers["Authorization"] = "Basic " + authInfo;
        }

        public void CreateRequest<TDTO>(string url)
        {

            // silverlight/phone7 crash when certain content is contained in url
            // in our case this is a back slash. this seems to be a known issue with phone7. is fixed in phone7.1
            // see http://social.microsoft.com/Forums/fi-FI/Offtopic/thread/f1df988a-403d-43b4-9f5d-20ebde39e55e amongs others

#if WINDOWS_PHONE
            if (url.Contains("\\"))
            {
                throw new ArgumentException("Windows Phone 7 does not like back slashes in urls. Is fixed in 7.1");
            }
#endif

            CacheItem<TDTO> item = _cache.Get<TDTO>(url);


            item.Request = _requestFactory.Create(url);
            if (item.Request is HttpWebRequest)
            {
                ((HttpWebRequest)item.Request).UserAgent = UserAgent;
            }

            if (!string.IsNullOrEmpty(BasicHttpAuthUsername))
            {
                SetBasicAuthHeader(item.Request, BasicHttpAuthUsername, BasicHttpAuthPassword);
            }



#if !SILVERLIGHT
            // silverlight crossdomain request does not support content type (?!)
            switch (ContentType)
            {
                case ContentType.FORM:
                    item.Request.ContentType = "application/x-www-form-urlencoded";
                    break;
                case ContentType.JSON:
                    item.Request.ContentType = "application/json";
                    break;
            }
            if (item.Request is HttpWebRequest)
            {
                ((HttpWebRequest)item.Request).Accept = "application/json";
            }


#endif
            item.Request.Method = item.Method.ToUpper();

            OnBeforeIssueRequest(new CacheItemEventArgs(item));

            if (item.Method.ToUpper() == "POST" )
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
                //lock (_lock)
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
        {
            CacheItem<TDTO> item = _cache.Get<TDTO>(url);


            byte[] bodyValue = new byte[] { };
            switch (ContentType)
            {
                case ContentType.JSON:
                    // if post then parameters should contain zero or one items
                    if (item.Parameters.Count > 1)
                    {

                        throw new ArgumentException("POST method with too many parameters");

                    }
                    if (item.Parameters.Count == 1)
                    {
                        bodyValue = CreatePostEntity(item.Parameters.First().Value);
                    }

                    break;
                case ContentType.FORM:
                    var sb = new StringBuilder();
                    foreach (var p in item.Parameters)
                    {
                        EncodeAndAddItem(ref sb, p.Key, p.Value.ToString());
                    }
                    bodyValue = Encoding.UTF8.GetBytes(sb.ToString());
                    break;
            }


            item.Request.BeginGetRequestStream(ac =>
                {
                    //lock (_lock)
                    {
                        try
                        {
                            using (Stream requestStream = item.Request.EndGetRequestStream(ac))
                            {
                                requestStream.Write(bodyValue, 0, bodyValue.Length);
                                requestStream.Flush();
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
        /// Serializes post entity
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static byte[] CreatePostEntity(object value)
        {
            // #TODO: this could be exposed on interface and made virtual to allow custom serialization

            string body = "";



            body = JsonConvert.SerializeObject(value, Formatting.None, new JsonConverter[] { });

            byte[] bodyValue = Encoding.UTF8.GetBytes(body);

            return bodyValue;
        }

        private void EncodeAndAddItem(ref StringBuilder baseRequest, string key, string dataItem)
        {
            if (baseRequest == null)
            {
                baseRequest = new StringBuilder();
            }
            if (baseRequest.Length != 0)
            {
                baseRequest.Append("&");
            }
            baseRequest.Append(key);
            baseRequest.Append("=");
            baseRequest.Append(CityIndex.JsonClient.HttpUtility.UrlEncode(dataItem));
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TDTO"></typeparam>
        private void EnqueueRequest<TDTO>(string url)
        {
            CacheItem<TDTO> outerItem = _cache.Get<TDTO>(url);
            IThrottledRequestQueue throttle = this[outerItem.ThrottleScope];

            WebRequest request = outerItem.Request;

            throttle.Enqueue(url, request, (ar, requestHolder) =>
                {
                    //lock (_lock)
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

                                item.ResponseText = json;

                                Exception seralizedException = _jsonExceptionFactory.ParseException(json);

                                Log.Debug(string.Format("request completed:\r\nITEM\r\n{0}", item));

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
                            bool shouldRetry = new RequestRetryDiscriminator().ShouldRetry(wex);

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
                                string responseText = null;

                                responseText = ApiException.GetResponseText(wex);
                                item.ResponseText = responseText;
                                bool isTimeout = wex.Message.ToLower().Contains("the request was aborted");

                                if (item.RetryCount > 0)
                                {
                                    if (isTimeout)
                                    {
                                        // TODO: this is ugly
                                        exception =
                                        new ApiTimeoutException(
                                            "Request timed out" +
                                            String.Format("\r\nretried {0} times", item.RetryCount) + "\r\nREQUEST INFO:\r\n" + item.ToString(), wex);
                                    }
                                    else
                                    {
                                        exception =
                                            new ApiException(
                                                wex.Message +
                                                String.Format("\r\nretried {0} times", item.RetryCount) + "\r\nREQUEST INFO:\r\n" + item.ToString(), wex);
                                    }

                                }
                                else
                                {
                                    if (isTimeout)
                                    {
                                        // TODO: this is ugly

                                        exception =
                                        new ApiTimeoutException(
                                            "Request timed out" + "\r\nREQUEST INFO:\r\n" + item.ToString(), wex);
                                    }
                                    else
                                    {
                                        exception =
                                            new ApiException(
                                                wex.Message + "\r\nREQUEST INFO:\r\n" + item.ToString(), wex);
                                    }


                                }

                                exception.ResponseText = responseText;

                                Log.Debug(string.Format("request completed with error:\r\n{0}", exception));

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
                                Log.Debug(string.Format("request completed with error:\r\n{0}", ex));

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