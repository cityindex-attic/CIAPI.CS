using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Common.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#if SILVERLIGHT
using System.Net.Browser;
#endif
// TODO: make all applicable public methods virtual
namespace CityIndex.JsonClient
{
    /// <summary>
    /// Provides a simple, yet fully functional, strongly typed json request dispatch replete with caching and throttling capabilities.
    /// Typically this class is used as a base for specialized derivations.
    /// </summary>
    public class Client : IJsonClient
    {
        #region Fields

        private static readonly ILog Log = LogManager.GetLogger(typeof(Client));
        private readonly IRequestCache _cache;
        private readonly IRequestFactory _requestFactory;
        private readonly int _retryCount;
        private readonly Dictionary<string, IThrottedRequestQueue> _throttleScopes;
        private readonly Uri _uri;
        private readonly object _lockObj = new object();

        #endregion

        ///<summary>
        ///</summary>
        ///<param name="uri"></param>
        ///<param name="cache"></param>
        ///<param name="requestFactory"></param>
        ///<param name="throttleScopes"></param>
        ///<param name="retryCount"></param>
        public Client(Uri uri, IRequestCache cache, IRequestFactory requestFactory,
                         Dictionary<string, IThrottedRequestQueue> throttleScopes, int retryCount)
        {
#if SILVERLIGHT
    // this enables the client framework stack - necessary for access to headers
            WebRequest.RegisterPrefix("http://", WebRequestCreator.ClientHttp);
            WebRequest.RegisterPrefix("https://", WebRequestCreator.ClientHttp);
#endif

            _retryCount = retryCount;
            _requestFactory = requestFactory;
            _throttleScopes = throttleScopes;
            _cache = cache;
            string url = uri.AbsoluteUri;
            if (!url.EndsWith("/"))
            {
                url = uri.AbsoluteUri + "/";
            }
            _uri = new Uri(url);
        }

        ///<summary>
        ///</summary>
        ///<param name="uri"></param>
        public Client(Uri uri)
            : this(uri, new RequestCache(), new RequestFactory(), new Dictionary<string, IThrottedRequestQueue> { { "default", new ThrottedRequestQueue() } }, 2)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="requestFactory"></param>
        public Client(Uri uri, IRequestFactory requestFactory)
            : this(uri, new RequestCache(), requestFactory, new Dictionary<string, IThrottedRequestQueue> { { "default", new ThrottedRequestQueue() } }, 2)
        {

        }

        #region Protected Methods

        /// <summary>
        /// Provides an interaction point after the request has been created but just before it is queued up
        /// for execution.
        /// Derived classes may take this opportunity to modify the <paramref name="request"/>, <paramref name="cacheDuration"/> or the <paramref name="throttleScope"/>.
        /// The remaining parameters are for reference only and modification will have no effect on the execution of the request.
        /// 
        /// TODO: perhaps a container object for the read-only reference values to avoid any confusion?
        ///       it could be argued that you can read the url of the request but having the components used to create
        ///       the url could make life a lot easier. ?
        /// </summary>
        /// <param name="request"></param>
        /// <param name="url"></param>
        /// <param name="target"></param>
        /// <param name="uriTemplate"></param>
        /// <param name="method"></param>
        /// <param name="parameters"></param>
        /// <param name="cacheDuration"></param>
        /// <param name="throttleScope"></param>
        protected virtual void BeforeIssueRequest(WebRequest request, string url, string target, string uriTemplate,
                                                  string method, Dictionary<string, object> parameters,
                                                  TimeSpan cacheDuration, string throttleScope)
        {
            // derived class implements this
        }


        /// <summary>
        /// Provides a interaction point just before the url for the request is built. Derived
        /// classes may take this opportunity to examine and modify the components used to 
        /// build the url for the request.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="uriTemplate"></param>
        /// <param name="method"></param>
        /// <param name="parameters"></param>
        /// <param name="cacheDuration"></param>
        /// <param name="throttleScope"></param>
        protected virtual void BeforeBuildUrl(string target, string uriTemplate,
                                              string method, Dictionary<string, object> parameters,
                                              TimeSpan cacheDuration, string throttleScope)
        {
            // derived class implements this
        }

        #endregion

        #region Public Implementation

        #region Synchronous Wrapper




        /// <summary>
        /// Very simple synchronous wrapper of the begin/end methods.
        /// I have chosen not to simply use the synchronous .GetResponse() method of WebRequest to prevent evolution
        /// of code that will not port to silverlight. While it is against everything righteous and holy in the silverlight crowd
        /// to implement syncronous patterns, no matter how cleverly, there is just too much that can be done with a sync fetch, i.e. multi page, eager fetches, etc,
        /// to ignore it. We simply forbid usage on the UI thread with an exception. Simple.
        /// </summary>
        /// <typeparam name="TDTO"></typeparam>
        /// <param name="target"></param>
        /// <param name="uriTemplate"></param>
        /// <param name="method"></param>
        /// <param name="parameters"></param>
        /// <param name="cacheDuration"></param>
        /// <param name="throttleScope"></param>
        /// <returns></returns>
        public TDTO Request<TDTO>(string target, string uriTemplate, string method, Dictionary<string, object> parameters, TimeSpan cacheDuration, string throttleScope) where TDTO : class, new()
        {
#if SILVERLIGHT
            if (System.Windows.Application.Current.RootVisual.Dispatcher.CheckAccess())
            {
                throw new ApiException("You cannot call this method from the UI thread.  Either use the asynchronous method: .Begin{name}, or call this from a background thread");
            }
#endif
            uriTemplate = uriTemplate ?? "";
            parameters = parameters ?? new Dictionary<string, object>();
            throttleScope = throttleScope ?? "default";

            TDTO response = null;
            Exception exception = null;
            using (var gate = new ManualResetEvent(false))
            {
                BeginRequest<TDTO>(ar =>
                                       {
                                           try
                                           {
                                               response = EndRequest(ar);
                                           }
                                           catch (Exception ex)
                                           {
                                               exception = ex;
                                           }

                                           gate.Set();
                                       }, null, target, uriTemplate, method, parameters, cacheDuration, throttleScope);

                gate.WaitOne();
            }

            if (exception != null)
            {
                throw exception;
            }

            return response;
        }


        ///<summary>
        /// Very simple synchronous wrapper of the begin/end methods.
        ///</summary>
        ///<param name="target"></param>
        ///<param name="method"></param>
        ///<typeparam name="TDTO"></typeparam>
        ///<returns></returns>
        public TDTO Request<TDTO>(string target, string method) where TDTO : class, new()
        {
            return Request<TDTO>(target, null, method, null, TimeSpan.FromMilliseconds(0), null);
        }

        //string target, string method
        //string target, string uriTemplate, string method, Dictionary<string, object> parameters

        ///<summary>
        ///</summary>
        ///<param name="target"></param>
        ///<param name="uriTemplate"></param>
        ///<param name="method"></param>
        ///<param name="parameters"></param>
        ///<typeparam name="TDTO"></typeparam>
        ///<returns></returns>
        public TDTO Request<TDTO>(string target, string uriTemplate, string method, Dictionary<string, object> parameters) where TDTO : class, new()
        {
            return Request<TDTO>(target, uriTemplate, method, parameters, TimeSpan.FromMilliseconds(0), null);
        }



        #endregion

        #region Asynchronous Implementation
        /// <summary>
        /// Standard async Begin implementation.
        /// </summary>
        /// <typeparam name="TDTO"></typeparam>
        /// <param name="cb"></param>
        /// <param name="state"></param>
        /// <param name="target"></param>
        /// <param name="uriTemplate"></param>
        /// <param name="method"></param>
        /// <param name="parameters"></param>
        /// <param name="cacheDuration"></param>
        /// <param name="throttleScope"></param>
        public void BeginRequest<TDTO>(ApiAsyncCallback<TDTO> cb, object state, string target, string uriTemplate,
                                       string method, Dictionary<string, object> parameters, TimeSpan cacheDuration,
                                       string throttleScope) where TDTO : class, new()
        {
            lock (_cache)
            {
                BeforeBuildUrl(target, uriTemplate, method, parameters, cacheDuration, throttleScope);

                string url = BuildUrl(target, uriTemplate, _uri.AbsoluteUri);

                //if (method.ToUpper() == "GET") NOTE: not sure why I was limiting this to GET. must have had a reason. let's see where it bites us....
                {
                    url = ApplyUriTemplateParameters(parameters, url);
                }

                CacheItem<TDTO> item = _cache.GetOrCreate<TDTO>(url);

                switch (item.ItemState)
                {
                    case CacheItemState.New:

                        if (!_throttleScopes.ContainsKey(throttleScope))
                        {
                            _cache.Remove<TDTO>(url);
                            throw new Exception(string.Format("Throttle for scope '{0}' not found.\r\n{1}", throttleScope, item));
                        }

                        item.AddCallback(cb, state);
                        item.CacheDuration = cacheDuration;
                        item.Method = method;
                        item.Parameters = parameters;
                        item.Target = target;
                        item.ThrottleScope = throttleScope;
                        item.UriTemplate = uriTemplate;
                        item.Url = url;

                        WebRequest request = _requestFactory.Create(url);
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
        }



        ///<summary>
        ///</summary>
        ///<param name="cb"></param>
        ///<param name="state"></param>
        ///<param name="target"></param>
        ///<param name="method"></param>
        ///<typeparam name="TDTO"></typeparam>
        public void BeginRequest<TDTO>(ApiAsyncCallback<TDTO> cb, object state, string target, string method) where TDTO : class, new()
        {
            BeginRequest(cb, state, target, null, method, null, TimeSpan.FromMilliseconds(0), "default");
        }


        ///<summary>
        ///</summary>
        ///<param name="cb"></param>
        ///<param name="state"></param>
        ///<param name="target"></param>
        ///<param name="uriTemplate"></param>
        ///<param name="method"></param>
        ///<param name="parameters"></param>
        ///<typeparam name="TDTO"></typeparam>
        public void BeginRequest<TDTO>(ApiAsyncCallback<TDTO> cb, object state, string target, string uriTemplate, string method, Dictionary<string, object> parameters) where TDTO : class, new()
        {
            BeginRequest(cb, state, target, uriTemplate, method, parameters, TimeSpan.FromMilliseconds(0), "default");
        }

        /// <summary>
        /// Standard async end implementation. Calling code passes in the ApiAsyncResult that is returned to the callback
        /// and the response data is returned. If an exception occurred during execution of the request, it will now be
        /// rethrown.
        /// </summary>
        /// <typeparam name="TDTO"></typeparam>
        /// <param name="asyncResult"></param>
        /// <returns></returns>
        /// <exception cref="ApiException">the exception, if any, that occurred during execution of the request</exception>
        // ReSharper disable MemberCanBeMadeStatic.Local
        // this method currently qualifies as a static member. please do not make it so, we will be doing housekeeping in here at a later date.
        public virtual TDTO EndRequest<TDTO>(ApiAsyncResult<TDTO> asyncResult) where TDTO : class, new()
        // ReSharper restore MemberCanBeMadeStatic.Local
        {

            return asyncResult.End();
        }


        #endregion

        #endregion

        #region Private implementation

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
                    lock (_cache)
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

                            _cache.Remove<TDTO>(item.Url);
                            item.CompleteResponse(null, new ApiException(ex));
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



        private void CreateRequest<TDTO>(string url)
            where TDTO : class, new()
        {
            CacheItem<TDTO> item = _cache.Get<TDTO>(url);


            item.Request = _requestFactory.Create(url);
            item.Request.Method = item.Method.ToUpper();

            BeforeIssueRequest(item.Request, item.Url, item.Target, item.UriTemplate, item.Method, item.Parameters, item.CacheDuration, item.ThrottleScope);

            if (item.Method.ToUpper() == "POST")
            {
                SetPostEntityAndEnqueueRequest<TDTO>(url);
            }
            else
            {
                EnqueueRequest<TDTO>(url);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TDTO"></typeparam>
        private void EnqueueRequest<TDTO>(string url)
            where TDTO : class, new()
        {
            CacheItem<TDTO> outerItem = _cache.Get<TDTO>(url);
            IThrottedRequestQueue throttle = _throttleScopes[outerItem.ThrottleScope];

            WebRequest request = outerItem.Request;

            throttle.Enqueue(url, request, (ar, requestHolder) =>
                {

                    lock (_cache)
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
                            using (var reader = new StreamReader(stream))
                            {
                                string json = reader.ReadToEnd();

                                // TODO: check json for exception 
                                Exception seralizedException = null;

                                try
                                {
                                    item.CompleteResponse(json, seralizedException);
                                }
                                catch (Exception ex)
                                {

                                    throw new ResponseHandlerException("Unhandled exception in caller's response handler", ex);
                                }
                            }
                        }
                        catch (WebException wex)
                        {
                            // TODO: allow for retries on select exception types
                            // e.g. 50* server errors, timeouts and transport errors
                            // DO NOT RETRY THROTTLE, AUTHENTICATION OR ARGUMENT EXCEPTIONS ETC
                            bool shouldRetry = new RequestRetryDiscriminator().ShouldRetry(wex);

                            if (shouldRetry && item.RetryCount < _retryCount)
                            {
                                // FIXME: We need to rebuild the request CANNOT REUSE HTTPWEBREQUEST
                                item.RetryCount++;
                                item.ItemState = CacheItemState.Pending; // should already be pending - check this

                                if (response != null)
                                {
                                    response.Close();
                                }

                                CreateRequest<TDTO>(url);
                            }
                            else
                            {
                                var exception = new ApiException(wex);
                                if (item.RetryCount > 0)
                                {
                                    exception =
                                        new ApiException(
                                            exception.Message +
                                            String.Format("\r\nretried {0} times", item.RetryCount), exception);
                                }
                                item.CompleteResponse(null, exception);
                            }
                        }
                        catch (Exception ex)
                        {
                            item.CompleteResponse(null, new ApiException(ex));
                        }
                    }
                });
        }


        /// <summary>
        /// Replaces templates with parameter values, if present, and cleans up missing templates.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        private static string ApplyUriTemplateParameters(Dictionary<string, object> parameters, string url)
        {
            url = new Regex(@"{\w+}").Replace(url, match =>
                {
                    string key = match.Value.Substring(1, match.Value.Length - 2);
                    if (parameters.ContainsKey(key))
                    {
                        object paramValue = parameters[key];
                        if (paramValue != null)
                        {
                            parameters.Remove(key);
                            return paramValue.ToString();
                        }
                    }
                    return null;
                });

            // clean up unused templates
            url = new Regex(@"\w+={\w+}").Replace(url, "");
            // clean up orphaned ampersands
            url = new Regex(@"&{2,}").Replace(url, "&");
            // clean up broken query
            url = new Regex(@"\?&").Replace(url, "?");

            url = url.TrimEnd('/');

            return url;
        }

        /// <summary>
        /// Composes the url for a request from components
        /// </summary>
        /// <param name="target"></param>
        /// <param name="uriTemplate"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        private static string BuildUrl(string target, string uriTemplate, string url)
        {
            // TODO: need to have some preformatting of these components to allow
            // for inconsistencies from caller, e.g. ensure proper trailing and leading slashes
            url = url + target + uriTemplate;
            url = url.TrimEnd('/');
            return url;
        }


        #endregion
    }
}