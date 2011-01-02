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

    public class Client
    {


        private static readonly ILog Log = LogManager.GetLogger(typeof(Client));
        private readonly IRequestFactory _requestFactory;
        public Dictionary<string, IThrottedRequestQueue> ThrottleScopes;
        private readonly int _retryCount;
        private RequestCache Cache { get; set; }




        /// <summary>
        /// This is a place to add headers or anything else you would like to do just before the
        /// request is placed in the execution queue.
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


        protected virtual void BeforeBuildUrl(string target, string uriTemplate,
                                                   string method, Dictionary<string, object> parameters,
                                                   TimeSpan cacheDuration, string throttleScope)
        {

        }


        protected Client(Uri uri, RequestCache cache, IRequestFactory requestFactory, Dictionary<string, IThrottedRequestQueue> throttleScopes, int retryCount)
        {
#if !SILVERLIGHT
#else

            // this enables the client framework stack - necessary for access to headers
            WebRequest.RegisterPrefix("http://", WebRequestCreator.ClientHttp);
            WebRequest.RegisterPrefix("https://", WebRequestCreator.ClientHttp);
#endif

            _retryCount = retryCount;
            _requestFactory = requestFactory;
            ThrottleScopes = throttleScopes;
            Cache = cache;
            string url = uri.AbsoluteUri;
            if (!url.EndsWith("/"))
            {
                url = uri.AbsoluteUri + "/";
            }
            Uri = new Uri(url);
        }

        public Uri Uri { get; set; }


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
                                   Dictionary<string, object> parameters, TimeSpan cacheDuration, string throttleScope) where TDTO : class, new()
        {
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
                                        string method, Dictionary<string, object> parameters, TimeSpan cacheDuration, string throttleScope) where TDTO : class, new()
        {
            lock (Cache)
            {

                BeforeBuildUrl(target, uriTemplate, method, parameters, cacheDuration, throttleScope);

                string url = Uri.AbsoluteUri + target + uriTemplate;

                if (method.ToUpper() == "GET")
                {
                    url = ApplyUriTemplateParameters(parameters, url);
                }

                url = url.TrimEnd('/');



                CacheItem<TDTO> item = Cache.GetOrCreateCacheItem(url, cb, state, cacheDuration);

                switch (item.ItemState)
                {
                    case CacheItemState.New:
                        if (!ThrottleScopes.ContainsKey(throttleScope))
                        {
                            throw new Exception(string.Format("throttle for scope '{0}' not found", throttleScope));
                        }
                        var throttle = ThrottleScopes[throttleScope];

                        var request = _requestFactory.Create(url);

                        request.Method = method.ToUpper();

#if !SILVERLIGHT
                        // silverlight crossdomain request does not support content type (?!)
                        request.ContentType = "application/json";
#endif



                        BeforeIssueRequest(request, url, target, uriTemplate, method, parameters, cacheDuration, throttleScope);

                        if (method.ToUpper() == "POST")
                        {
                            SetPostEntityAndEnqueueRequest<TDTO>(url, request, parameters, throttle);
                        }
                        else
                        {
                            EnqueueRequest<TDTO>(url, request, throttle);
                        }
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
        /// Builds a JSOB from parameters and asynchronously feeds the request stream with the resultant JSON before sending the request
        /// </summary>
        /// <param name="url"></param>
        /// <param name="request"></param>
        /// <param name="parameters"></param>
        /// <param name="throttle"></param>
        private void SetPostEntityAndEnqueueRequest<TDTO>(string url, WebRequest request, Dictionary<string, object> parameters, IThrottedRequestQueue throttle)
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
                        EnqueueRequest<TDTO>(url, request, throttle);
                    }
                    catch (Exception ex)
                    {
                        lock (Cache)
                        {
                            CacheItem<TDTO> item = Cache.Remove<TDTO>(request.RequestUri.AbsoluteUri);
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
        /// Currrently this simply initiates the http request and wraps the end in our async result.
        /// In the near future this will be where the throttle/cache magic starts.
        /// </summary>
        /// <typeparam name="TDTO"></typeparam>
        /// <param name="url"></param>
        /// <param name="webRequest"></param>
        /// <param name="throttle"></param>
        // ReSharper disable MemberCanBeMadeStatic.Local
        // this method currently qualifies as a static member. please do not make it so, we will be doing housekeeping in here at a later date.
        private void EnqueueRequest<TDTO>(string url, WebRequest webRequest, IThrottedRequestQueue throttle) where TDTO : class, new()
        // ReSharper restore MemberCanBeMadeStatic.Local
        {

            throttle.Enqueue(url, webRequest, (ar, requestHolder) =>
                {
                    lock (Cache)
                    {

                        // the item had better be in the cache because if it is not
                        // and this throws then we have a deadlock as the callbacks are int
                        // the cache item that does not exist so no where to send the exception.
                        // TODO: add an OnException event to this class
                        CacheItem<TDTO> item = Cache.Get<TDTO>(url);
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

                            if (shouldRetry && item.RetryCount <= RetryCount)
                            {

                                // TODO: TEST THIS
                                item.RetryCount++;
                                item.ItemState = CacheItemState.New; // should already be New - check this
                                if (response != null)
                                {
                                    response.Close();
                                }
                                EnqueueRequest<TDTO>(url, webRequest, throttle);
                            }
                            else
                            {
                                var exception = new ApiException(wex);
                                if (item.RetryCount > 0)
                                {
                                    exception = new ApiException(exception.Message + String.Format("\r\nretried {0} times", item.RetryCount - 1), exception);
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


        public int RetryCount
        {
            get { return _retryCount; }
        }

        /// <summary>
        /// Standard async end implementation. 
        /// </summary>
        /// <typeparam name="TDTO"></typeparam>
        /// <param name="asyncResult"></param>
        /// <returns></returns>
        // ReSharper disable MemberCanBeMadeStatic.Local
        // this method currently qualifies as a static member. please do not make it so, we will be doing housekeeping in here at a later date.
        public TDTO EndRequest<TDTO>(ApiAsyncResult<TDTO> asyncResult) where TDTO : class, new()
        // ReSharper restore MemberCanBeMadeStatic.Local
        {
            return asyncResult.End();
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
            return url;
        }

        #endregion




    }


}