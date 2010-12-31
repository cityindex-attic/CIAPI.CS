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
using Soapi.Net;

#if SILVERLIGHT
using System.Net.Browser;
#endif

namespace CIAPI.Core
{

    public partial class ApiContext
    {
#if !SILVERLIGHT
#else

        static ApiContext()
        {
            // this enables the client framework stack - necessary for access to headers
            WebRequest.RegisterPrefix("http://", WebRequestCreator.ClientHttp);
            WebRequest.RegisterPrefix("https://", WebRequestCreator.ClientHttp);
        }

#endif

        private static readonly ILog Log = LogManager.GetLogger(typeof(ApiContext));

        public Dictionary<string, IRequestThrottle> ThrottleScopes;
        
        private RequestCache Cache { get; set; }

        /// <summary>
        /// Authenticates the request with the API
        /// </summary>
        /// <param name="request"></param>
        private void SetSessionHeaders(WebRequest request)
        {
            request.Headers["UserName"] = UserName;
            // API advertises session id as a GUID but treats as a string internally so we need to ucase here.
            request.Headers["Session"] = SessionId.ToString().ToUpper();
        }



        /// <summary>
        /// Instantiates an ApiContext that does not require support for Basic Authentication
        /// </summary>
        /// <param name="uri"></param>
        public ApiContext(Uri uri)
            : this(uri, new RequestCache(), new Dictionary<string, IRequestThrottle>
                {
                    { "data", new RequestThrottle(new RequestFactory(), TimeSpan.FromSeconds(5),30,10) }, 
                    { "trading", new RequestThrottle(new RequestFactory(), TimeSpan.FromSeconds(3),1,10) }
                })
        {

        }
        public ApiContext(Uri uri, RequestCache cache, Dictionary<string, IRequestThrottle> throttleScopes)
        {
            ThrottleScopes = throttleScopes;

            Cache = cache;
            Uri = uri;
        }

        public Uri Uri { get; set; }
        public string UserName { get; set; }
        public Guid SessionId { get; set; }

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
        private TDTO Request<TDTO>(string target, string uriTemplate, string method,
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
        private void BeginRequest<TDTO>(ApiAsyncCallback<TDTO> cb, object state, string target, string uriTemplate,
                                        string method, Dictionary<string, object> parameters, TimeSpan cacheDuration, string throttleScope) where TDTO : class, new()
        {
            lock (Cache)
            {
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

                        var request = throttle.Create(url);

                        request.Method = method.ToUpper();

#if !SILVERLIGHT
                        // silverlight crossdomain request does not support content type (?!)
                        request.ContentType = "application/json";
#endif

                        if (url.IndexOf("/session", StringComparison.OrdinalIgnoreCase) == -1)
                        {
                            SetSessionHeaders(request);
                        }

                        if (method.ToUpper() == "POST")
                        {
                            SetPostEntityAndExecuteRequest<TDTO>(url, request, parameters,throttle);
                        }
                        else
                        {
                            ExecuteRequest<TDTO>(url, request, throttle);
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
        private void SetPostEntityAndExecuteRequest<TDTO>(string url, WebRequest request, Dictionary<string, object> parameters, IRequestThrottle throttle)
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
                        ExecuteRequest<TDTO>(url, request, throttle);
                    }
                    catch (Exception ex)
                    {
                        lock (Cache)
                        {
                            throttle.Complete();
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
        /// <param name="request"></param>
        /// <param name="throttle"></param>
        // ReSharper disable MemberCanBeMadeStatic.Local
        // this method currently qualifies as a static member. please do not make it so, we will be doing housekeeping in here at a later date.
        private void ExecuteRequest<TDTO>(string url, WebRequest request, IRequestThrottle throttle) where TDTO : class, new()
        // ReSharper restore MemberCanBeMadeStatic.Local
        {
            request.BeginGetResponse(ar =>
                {


                    lock (Cache)
                    {
                        try
                        {
                            // the item had better be in the cache because if it is not
                            // and this throws then we have a deadlock as the callbacks are int
                            // the cache item that does not exist so no where to send the exception.
                            // TODO: add an OnException event to this class
                            CacheItem<TDTO> item = Cache.Get<TDTO>(url);

                            try
                            {

                                using (var response = request.EndGetResponse(ar))
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
                                item.CompleteResponse(null, new ApiException(wex));
                            }
                            catch (Exception ex)
                            {
                                item.CompleteResponse(null, new ApiException(ex));
                            }
                        }
                        finally
                        {
                            throttle.Complete();
                        }
                    }
                }, null);
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
        /// <param name="uri"></param>
        /// <returns></returns>
        private static string ApplyUriTemplateParameters(Dictionary<string, object> parameters, string uri)
        {
            uri = new Regex(@"{\w+}").Replace(uri, match =>
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
            return new Regex(@"\w+={\w+}").Replace(uri, "");
        }

        #endregion
    }


}