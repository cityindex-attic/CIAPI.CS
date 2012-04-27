using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Salient.ReliableHttpClient.Serialization;

namespace Salient.ReliableHttpClient
{
    public class ClientBase : IDisposable
    {
        protected RequestController Controller;
        private readonly Dictionary<string, object> _headers = new Dictionary<string, object>();
        public IJsonSerializer Serializer { get; set; }
        
        public bool IsRecording
        {
            get { return !Controller.Recorder.Paused; }
        }

        private string _userAgent;
        protected string UserAgent
        {
            get { return _userAgent; }
            set
            {
                _userAgent = value;
                Controller.UserAgent = value;
            }
        }


        public ClientBase(IJsonSerializer serializer)
        {
            Controller = new RequestController(serializer);
            UserAgent = "Salient.ReliableHttpClient";
            Serializer = serializer;
            
        }

        public ClientBase(IJsonSerializer serializer, IRequestFactory factory)
            : this(serializer)
        {
            Controller = new RequestController(serializer, factory);
            UserAgent = "Salient.ReliableHttpClient";
        }






        protected virtual void BeforeIssueRequest(Uri uri, RequestMethod method, string body,
                                                  ContentType requestContentType,
                                                  ContentType responseContentType, TimeSpan cacheDuration, int timeout,
                                                  string target, string uriTemplate, int retryCount,
                                                  Dictionary<string, object> parameters)
        {
            // client implements
        }


        protected void SetHeader(string key, object value)
        {
            lock (_headers)
            {
                _headers[key] = value;
            }
        }

        protected void RemoveHeader(string key)
        {
            lock (_headers)
            {
                if (_headers.ContainsKey(key))
                {
                    _headers.Remove(key);
                }
            }
        }


        /// <summary>
        /// Composes the url for a request from components
        /// </summary>
        /// <param name="target"></param>
        /// <param name="uriTemplate"></param>
        /// <returns></returns>
        private static Uri BuildUrl(string target, string uriTemplate)
        {
            // TODO: need to have some preformatting of these components to allow
            // for inconsistencies from caller, e.g. ensure proper trailing and leading slashes
            string url = "";
            url = url + target + uriTemplate;
            url = url.TrimEnd('/');
            return new Uri(url);
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
                                                               parameters.Remove(key);
                                                               if (paramValue != null)
                                                               {
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


        private static void EncodeAndAddItem(ref StringBuilder baseRequest, string key, string dataItem)
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
            baseRequest.Append(HttpUtility.UrlEncode(dataItem));
        }


        public virtual string Request(RequestMethod method, string target, string uriTemplate,
                                      Dictionary<string, object> parameters, ContentType requestContentType,
                                      ContentType responseContentType, TimeSpan cacheDuration, int timeout,
                                      int retryCount)
        {
#if SILVERLIGHT
            if (System.Windows.Deployment.Current.Dispatcher.CheckAccess())
            {
                throw new ReliableHttpException("You cannot call this method from the UI thread.  Either use the asynchronous method: .Begin{name}, or call this from a background thread");
            }
#endif
            uriTemplate = uriTemplate ?? "";
            parameters = parameters ?? new Dictionary<string, object>();


            string response = null;
            Exception exception = null;
            var gate = new ManualResetEvent(false);

            BeginRequest(method, target, uriTemplate, parameters, requestContentType, responseContentType, cacheDuration,
                         timeout, retryCount, ar =>
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
                                                  }, null);

            // #FIXME: what is a good absolute master timeout?
            gate.WaitOne();
            // #FIXME: logic to catch stalls conflicts with throttle
            //if (!gate.WaitOne(30000))
            //{
            //    throw new ReliableHttpException("request stalled");
            //}

            if (exception != null)
            {
                throw exception;
            }

            return response;
        }


        public virtual Guid BeginRequest(
            RequestMethod method, string target, string uriTemplate, Dictionary<string, object> parameters,
            ContentType requestContentType,
            ContentType responseContentType, TimeSpan cacheDuration, int timeout, int retryCount,
            ApiAsyncCallback callback,
            object state)
        {
            string body = null;

            if (parameters != null)
            {
                var localParams = new Dictionary<string, object>(parameters);
                uriTemplate = ApplyUriTemplateParameters(localParams, uriTemplate);

                if (localParams.Count > 0)
                {
                    switch (method)
                    {
                        case RequestMethod.GET:
                        case RequestMethod.HEAD:
                        case RequestMethod.DELETE:
                            throw new ArgumentException("unrecognized parameters");
                            break;
                        case RequestMethod.PUT:
                        case RequestMethod.POST:
                            switch (requestContentType)
                            {
                                case ContentType.JSON:
                                    // if post then parameters should contain zero or one items
                                    if (localParams.Count > 1)
                                    {
                                        throw new ArgumentException("POST method with too many parameters");
                                    }
                                    body = Serializer.SerializeObject(localParams.First().Value);

                                    break;
                                case ContentType.FORM:
                                    var sb = new StringBuilder();
                                    foreach (var p in localParams)
                                    {
                                        if (p.Value != null)
                                        {
                                            EncodeAndAddItem(ref sb, p.Key, p.Value.ToString());
                                        }
                                    }
                                    body = sb.ToString();
                                    break;
                                case ContentType.XML:
                                    throw new NotImplementedException();
                                    break;
                                case ContentType.TEXT:
                                    throw new NotImplementedException();
                                    break;
                            }
                            break;
                    }
                }
            }


            Uri uri = BuildUrl(target, uriTemplate);

            BeforeIssueRequest(uri, method, body, requestContentType, responseContentType, cacheDuration,
                               timeout, target, uriTemplate, retryCount, parameters);

            Guid id = Controller.BeginRequest(uri, method, body, _headers, requestContentType, responseContentType,
                                              cacheDuration, timeout, target, uriTemplate, retryCount, parameters,
                                              callback, state);
            return id;
        }



        public virtual string EndRequest(ReliableAsyncResult result)
        {
            try
            {
                result.End();
                return result.ResponseText;
            }
            catch (ReliableHttpException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DefectException("expecting only ReliableHttpException here. See inner for details", ex);
            }

        }

        #region Serialization

        public virtual T Request<T>(RequestMethod method, string target, string uriTemplate,
                                    Dictionary<string, object> parameters, ContentType requestContentType,
                                    ContentType responseContentType, TimeSpan cacheDuration, int timeout, int retryCount)
        {
            string response = Request(method, target, uriTemplate, parameters, requestContentType, responseContentType,
                                      cacheDuration, timeout, retryCount);
            // #TODO: reinstate injected json exception factory and use it here.
            // "Invalid response received.  Are you connecting to the correct server Url?"
            try
            {
                return Serializer.DeserializeObject<T>(response);
            }
            catch (Exception ex)
            {

                throw ReliableHttpException.Create(ex);
            }
        }


        public virtual T EndRequest<T>(ReliableAsyncResult result)
        {
            string response = EndRequest(result);
            try
            {
                return Serializer.DeserializeObject<T>(response);
            }
            catch (Exception ex)
            {
                
                throw ReliableHttpException.Create("Invalid response received.  Are you connecting to the correct server Url?", ex);
            }
        }


        public T DeserializeJson<T>(string json)
        {
            var obj = Serializer.DeserializeObject<T>(json);
            return obj;
        }

        #endregion

        #region IDisposable Members

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Controller != null)
                {
                    Controller.Dispose();
                    Controller = null;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion


        #region Recording

        public void StartRecording()
        {
            Controller.Recorder.Paused = false;
        }

        public void StopRecording()
        {
            Controller.Recorder.Paused = true;
        }

        public void ClearRecording()
        {
            Controller.Recorder.Clear();
        }

        public List<RequestInfoBase> GetRecording()
        {
            List<RequestInfoBase> requests = Controller.Recorder.GetRequests();
            return requests;
        }

        #endregion


        ///// <summary>
        ///// Serializes post entity
        ///// </summary>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //private byte[] CreatePostEntity(object value)
        //{
        //    string body = "";
        //    body = Serializer.SerializeObject(value);
        //    byte[] bodyValue = Encoding.UTF8.GetBytes(body);

        //    return bodyValue;
        //}
    }
}