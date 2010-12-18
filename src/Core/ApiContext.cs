using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

        /// <summary>
        /// Authenticates the request with the API
        /// </summary>
        /// <param name="request"></param>
        private void SetSessionHeaders(HttpWebRequest request)
        {
            request.Headers["UserName"] = UserName;
            // API advertises session id as a GUID but treats as a string internally so we need to ucase here.
            request.Headers["Session"] = SessionId.ToString().ToUpper();
        }


        /// <summary>
        /// Instantiates an ApiContext with support for a Basic Authentication gate.
        /// NOTE: the basicUid and basicPwd are for satisfying IIS, NOT for authenticating against the API.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="basicUid"></param>
        /// <param name="basicPwd"></param>
        public ApiContext(Uri uri, string basicUid, string basicPwd)
            : this(uri)
        {
            BasicUid = basicUid;
            BasicPwd = basicPwd;
        }

        public string BasicUid { get; set; }
        public string BasicPwd { get; set; }

        /// <summary>
        /// Instantiates an ApiContext that does not require support for Basic Authentication
        /// </summary>
        /// <param name="uri"></param>
        public ApiContext(Uri uri)
        {
            Uri = uri;
        }


        public Uri Uri { get; set; }
        public string UserName { get; set; }
        public Guid SessionId { get; set; }



        #region Synchronous Wrapper

        /// <summary>
        /// Very simple synchronous wrapper of the begin/end methods.
        /// I have chosen not to simply use the synchronous .GetResponse() method of HttpWebRequest to prevent evolution
        /// of code that will not port to silverlight. While it is against everything righteous and holy in the silverlight crowd
        /// to implement syncronous patterns, no matter how cleverly, there is just too much that can be done with a sync fetch, i.e. multi page, eager fetches, etc,
        /// to ignore it. We simply forbid usage on the UI thread with an exception. Simple.
        /// </summary>
        /// <typeparam name="TDTO"></typeparam>
        /// <param name="target"></param>
        /// <param name="uriTemplate"></param>
        /// <param name="method"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private TDTO Request<TDTO>(string target, string uriTemplate, string method,
                                   Dictionary<string, object> parameters) where TDTO : class, new()
        {
            TDTO response = null;
            Exception exception = null;
            using (var gate = new ManualResetEvent(false))
            {
                BeginRequest<TDTO>(ar =>
                {

                    try
                    {
                        response = ar.End();
                    }
                    catch (Exception ex)
                    {
                        exception = ex;
                    }

                    gate.Set();

                }, null, target, uriTemplate, method, parameters);

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
        private void BeginRequest<TDTO>(ApiAsyncCallback<TDTO> cb, object state, string target, string uriTemplate,
                                        string method, Dictionary<string, object> parameters) where TDTO : class, new()
        {
            string uri = Uri.AbsoluteUri + target + uriTemplate;

            if (method.ToUpper() == "GET")
            {
                uri = ApplyUriTemplateParameters(parameters, uri);
            }

            var request = (HttpWebRequest)WebRequest.Create(uri.TrimEnd('/'));


            if (BasicUid != null)
            {
                // TODO: credentials don't seem to be getting honored by silverlight? not a huge issue right now, but what is going on?
                request.Credentials = new NetworkCredential(BasicUid, BasicPwd);
#if SILVERLIGHT
                request.UseDefaultCredentials = false;
#endif
            }


            request.Method = method.ToUpper();

#if !SILVERLIGHT
            // silverlight crossdomain request does not support content type (?!)
            request.ContentType = "application/json";
#endif

            if (uri.IndexOf("/session", StringComparison.OrdinalIgnoreCase) == -1)
            {
                SetSessionHeaders(request);
            }

            if (method.ToUpper() == "POST")
            {
                SetPostEntityAndExecuteRequest(cb, state, parameters, request);
            }
            else
            {
                ExecuteRequest(cb, state, request);
            }
        }


        /// <summary>
        /// Builds a JSOB from parameters and asynchronously feeds the request stream with the resultant JSON before sending the request
        /// </summary>
        /// <param name="state"></param>
        /// <param name="parameters"></param>
        /// <param name="request"></param>
        /// <param name="cb"></param>
        private void SetPostEntityAndExecuteRequest<TDTO>(ApiAsyncCallback<TDTO> cb, object state,
                                                          Dictionary<string, object> parameters, HttpWebRequest request)
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

                        ExecuteRequest(cb, state, request);
                    }
                    catch (Exception ex)
                    {
                        new ApiAsyncResult<TDTO>(cb, state, true, null, new ApiException(ex));
                    }
                }, state);
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
        /// <param name="cb"></param>
        /// <param name="state"></param>
        /// <param name="request"></param>
        // ReSharper disable MemberCanBeMadeStatic.Local
        // this method currently qualifies as a static member. please do not make it so, we will be doing housekeeping in here at a later date.
        private void ExecuteRequest<TDTO>(ApiAsyncCallback<TDTO> cb, object state, HttpWebRequest request)
            // ReSharper restore MemberCanBeMadeStatic.Local
            where TDTO : class, new()
        {
            request.BeginGetResponse(ar =>
                {
                    try
                    {
                        using (var response = (HttpWebResponse)request.EndGetResponse(ar))
                        using (Stream stream = response.GetResponseStream())
                        using (var reader = new StreamReader(stream))
                        {
                            string json = reader.ReadToEnd();
                            var result = JsonConvert.DeserializeObject<TDTO>(json);
                            new ApiAsyncResult<TDTO>(cb, state, true, result, null);
                        }
                    }
                    catch (WebException wex)
                    {
                        new ApiAsyncResult<TDTO>(cb, state, true, null, new ApiException(wex));
                    }
                    catch (Exception ex)
                    {
                        new ApiAsyncResult<TDTO>(cb, state, true, null, new ApiException(ex));
                    }
                }, state);
        }


        /// <summary>
        /// Standard async end implementation. 
        /// </summary>
        /// <typeparam name="TDTO"></typeparam>
        /// <param name="asyncResult"></param>
        /// <returns></returns>
        // ReSharper disable MemberCanBeMadeStatic.Local
        // this method currently qualifies as a static member. please do not make it so, we will be doing housekeeping in here at a later date.
        private TDTO EndRequest<TDTO>(ApiAsyncResult<TDTO> asyncResult) where TDTO : class, new()
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
    } 

        #endregion
}