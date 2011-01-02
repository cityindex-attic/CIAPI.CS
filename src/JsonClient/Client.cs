using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#if SILVERLIGHT
using System.Net.Browser;
#endif

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

        #endregion

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

        #region Protected Methods

        /// <summary>
        /// Provides an interaction point after the request has been created but just before it is queued up
        /// for execution.
        /// Derived classes may take this opportunity to modify the <paramref name="request"/>, <paramref name="cacheDuration"/> or the <paramref name="throttleScope"/>.
        /// The remaining parameters are for reference only and modification will have no effect on the execution of the request.
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
        public TDTO Request<TDTO>(string target, string uriTemplate, string method,
                                  Dictionary<string, object> parameters, TimeSpan cacheDuration, string throttleScope)
            where TDTO : class, new()
        {
#if SILVERLIGHT
            //TODO: add UI thread check and throw forbidden exception if so
#endif

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

                if (method.ToUpper() == "GET")
                {
                    url = ApplyUriTemplateParameters(parameters, url);
                }

                CacheItem<TDTO> item = _cache.GetOrCreate<TDTO>(url);

                switch (item.ItemState)
                {
                    case CacheItemState.New:

                        if (!_throttleScopes.ContainsKey(throttleScope))
                        {
                            throw new Exception(string.Format("throttle for scope '{0}' not found", throttleScope));
                        }

                        item.AddCallback(cb, state);
                        item.CacheDuration = cacheDuration;

                        WebRequest request = _requestFactory.Create(url);

                        request.Method = method.ToUpper();

#if !SILVERLIGHT
                        // silverlight crossdomain request does not support content type (?!)
                        request.ContentType = "application/json";
#endif

                        BeforeIssueRequest(request, url, target, uriTemplate, method, parameters, cacheDuration,
                                           throttleScope);

                        if (method.ToUpper() == "POST")
                        {
                            SetPostEntityAndEnqueueRequest<TDTO>(url, request, parameters, throttleScope);
                        }
                        else
                        {
                            EnqueueRequest<TDTO>(url, request, throttleScope);
                        }
                        break;
                    case CacheItemState.Pending:
                        item.AddCallback(cb, state);
                        break;
                    case CacheItemState.Complete:
                        new ApiAsyncResult<TDTO>(cb, state, true, item.ResponseText, null);
                        break;
                    default:
                        new ApiAsyncResult<TDTO>(cb, state, true, null,
                                                 new Exception("invalid item state, should never see this"));
                        break;
                }
            }
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
        public TDTO EndRequest<TDTO>(ApiAsyncResult<TDTO> asyncResult) where TDTO : class, new()
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
        /// <param name="request"></param>
        /// <param name="parameters"></param>
        /// <param name="throttleScope"></param>
        private void SetPostEntityAndEnqueueRequest<TDTO>(string url, WebRequest request,
                                                          Dictionary<string, object> parameters,
                                                          string throttleScope)
            where TDTO : class, new()
        {
            byte[] bodyValue = CreatePostEntity(parameters);

            request.BeginGetRequestStream(ac =>
                {
                    try
                    {
                        using (Stream requestStream = request.EndGetRequestStream(ac))
                        {
                            requestStream.Write(bodyValue, 0, bodyValue.Length);
                        }
                        EnqueueRequest<TDTO>(url, request, throttleScope);
                    }
                    catch (Exception ex)
                    {
                        lock (_cache)
                        {
                            CacheItem<TDTO> item = _cache.Remove<TDTO>(request.RequestUri.AbsoluteUri);
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


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TDTO"></typeparam>
        /// <param name="url"></param>
        /// <param name="webRequest"></param>
        /// <param name="throttleScope"></param>
        private void EnqueueRequest<TDTO>(string url, WebRequest webRequest, string throttleScope)
            where TDTO : class, new()
        {
            IThrottedRequestQueue throttle = _throttleScopes[throttleScope];

            // TODO: this anonymous method is used to create a closure to reduce
            // coupling to the throttle implementation. Would a non-anonymous technique
            // make understanding and porting of this method easier?

            throttle.Enqueue(url, webRequest, (ar, requestHolder) =>
                {
                    lock (_cache)
                    {
                        // the item had better be in the cache because if it is not
                        // and this throws then we have a deadlock as the callbacks are int
                        // the cache item that does not exist so no where to send the exception.
                        // TODO: add an OnException event to this class
                        CacheItem<TDTO> item = _cache.Get<TDTO>(url);
                        WebResponse response = null;
                        try
                        {
                            using (response = webRequest.EndGetResponse(ar))
                            using (Stream stream = response.GetResponseStream())
                            using (var reader = new StreamReader(stream))
                            {
                                string json = reader.ReadToEnd();

                                // TODO: check json for exception 
                                Exception seralizedException = null;

                                item.CompleteResponse(json, seralizedException);
                            }
                        }
                        catch (WebException wex)
                        {
                            // TODO: allow for retries on select exception types
                            // e.g. 50* server errors, timeouts and transport errors
                            // DO NOT RETRY THROTTLE, AUTHENTICATION OR ARGUMENT EXCEPTIONS ETC
                            bool shouldRetry = true; // TODO: identify qualifying exceptions

                            if (shouldRetry && item.RetryCount <= _retryCount)
                            {
                                // TODO: TEST THIS
                                item.RetryCount++;
                                item.ItemState = CacheItemState.New; // should already be New - check this
                                if (response != null)
                                {
                                    response.Close();
                                }

                                EnqueueRequest<TDTO>(url, webRequest, throttleScope);
                            }
                            else
                            {
                                var exception = new ApiException(wex);
                                if (item.RetryCount > 0)
                                {
                                    exception =
                                        new ApiException(
                                            exception.Message +
                                            String.Format("\r\nretried {0} times", item.RetryCount - 1), exception);
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
                    object paramValue = parameters[key];
                    if (paramValue != null)
                    {
                        parameters.Remove(key);
                        return paramValue.ToString();
                    }
                    return match.Value;
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
            return url + target + uriTemplate;
        }


        #endregion
    }
}