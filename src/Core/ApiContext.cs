using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CIAPI.Core
{
    public partial class ApiContext
    {

        public string BasicUid { get; set; }
        public string BasicPwd { get; set; }

        public Uri Uri { get; set; }
        public string UserName { get; set; }
        public Guid SessionId { get; set; }

        public ApiContext(Uri uri)
        {
            Uri = uri;
        }
        public ApiContext(Uri uri, string basicUid, string basicPwd)
            : this(uri)
        {

            BasicUid = basicUid;
            BasicPwd = basicPwd;
        }

        private T Request<T>(string target, string uriTemplate, string method, Dictionary<string, object> parameters) where T : class,new()
        {
            T response = null;
            using (var gate = new ManualResetEvent(false))
            {
                BeginRequest<T>(ar =>
                {
                    response = ar.End();
                    gate.Set();
                }, null, target, uriTemplate, method, parameters);
                gate.WaitOne();
            }
            return response;
        }

        private void BeginRequest<T>(ApiAsyncCallback<T> cb, object state, string target, string uriTemplate, string method, Dictionary<string, object> parameters) where T : class,new()
        {


            var uri = Uri.AbsoluteUri + target + uriTemplate;
            if (method.ToUpper() == "GET")
            {
                var paramRx = new Regex(@"{\w+}");
                uri = paramRx.Replace(uri, match =>
                {
                    var paramValue = parameters[match.Value.Substring(1, match.Value.Length - 2)];
                    if (paramValue != null)
                    {
                        return paramValue.ToString();
                    }
                    return match.Value;
                });

                var cleanupRx = new Regex(@"\w+={\w+}");
                uri = cleanupRx.Replace(uri, "");

            }


            var request = (HttpWebRequest)WebRequest.Create(uri.TrimEnd('/'));
            request.Method = method;



            if (uri.IndexOf("/session", StringComparison.OrdinalIgnoreCase) == -1)
            {
                request.Headers.Add("UserName", UserName);
                request.Headers.Add("Session", SessionId.ToString().ToUpper()); // FIXME: (in the API + SMD) API advertises session id as a GUID but treats as a string internally so we need to ucase here. 
            }

            request.ContentType = "application/json";

            if (BasicUid != null)
            {
                request.Credentials = new NetworkCredential(BasicUid, BasicPwd);
            }

            if (method.ToUpper() == "POST")
            {
                var body = new JObject();
                foreach (KeyValuePair<string, object> kvp in parameters)
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
                var bodyValue = Encoding.UTF8.GetBytes(body.ToString());
                var requestStream = request.GetRequestStream();
                requestStream.Write(bodyValue, 0, bodyValue.Length);

            }
            request.BeginGetResponse(ar =>
            {
                try
                {

                    using (var response = (HttpWebResponse)request.EndGetResponse(ar))
                    using (var stream = response.GetResponseStream())
                    using (var reader = new StreamReader(stream))
                    {
                        string json = reader.ReadToEnd();
                        var result = JsonConvert.DeserializeObject<T>(json);
                        new ApiAsyncResult<T>(cb, state, true, result, null);
                    }
                }
                catch (WebException we)
                {
                    var ex = new ApiException(we);
                    new ApiAsyncResult<T>(cb, state, true, null, ex);
                }
            }, state);

        }

        /// <summary>
        /// Standard async end implementation. A simpler method of obtaining the respose is to call .End() on the ApiAsyncResult
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="asyncResult"></param>
        /// <returns></returns>
        private T EndRequest<T>(ApiAsyncResult<T> asyncResult) where T : class,new()
        {
            return asyncResult.End();
        }
    }
}
