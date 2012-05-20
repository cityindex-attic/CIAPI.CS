using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using CIAPI.DTO;
using CIAPI.Streaming;
using Salient.ReflectiveLoggingAdapter;
using Salient.ReliableHttpClient;

namespace CIAPI.Rpc
{
    // #TODO: reintegrate exception factory into ReliableHttpClient
    public partial class Client : ClientBase
    {


        private Dictionary<string, string> GetHeaders(string target)
        {
            var headers = new Dictionary<string, string>();
            if (target.IndexOf("/session", StringComparison.OrdinalIgnoreCase) == -1)
            {
                headers["UserName"] = UserName;
                if (Session == null)
                {
                    throw new ReliableHttpException("Session is null. Have you created a session? (logged on)");
                }
                headers["Session"] = Session;
            }
            return headers;
        }
        public Guid BeginRequest(RequestMethod method, string target, string uriTemplate, Dictionary<string, object> parameters, ContentType requestContentType, ContentType responseContentType, TimeSpan cacheDuration, int timeout, int retryCount, ReliableAsyncCallback callback, object state)
        {
            target = _rootUri.AbsoluteUri + "/" + target;
            Dictionary<string, string> headers = GetHeaders(target);
            return base.BeginRequest(method, target, uriTemplate, headers, parameters, requestContentType, responseContentType, cacheDuration, timeout, retryCount, callback, state);
        }

        public T Request<T>(RequestMethod method, string target, string uriTemplate, Dictionary<string, object> parameters, ContentType requestContentType, ContentType responseContentType, TimeSpan cacheDuration, int timeout, int retryCount)
        {
            target = _rootUri.AbsoluteUri + "/" + target;
            Dictionary<string, string> headers = GetHeaders(target);

            return base.Request<T>(method, target, uriTemplate, headers, parameters, requestContentType, responseContentType, cacheDuration, timeout, retryCount);
        }
        public string Request(RequestMethod method, string target, string uriTemplate, Dictionary<string, object> parameters, ContentType requestContentType, ContentType responseContentType, TimeSpan cacheDuration, int timeout, int retryCount)
        {
            target = _rootUri.AbsoluteUri + "/" + target;
            Dictionary<string, string> headers = GetHeaders(target);
            return base.Request(method, target, uriTemplate, headers, parameters, requestContentType, responseContentType, cacheDuration, timeout, retryCount);
        }


        public ReliableHttpException GetJsonException(ReliableHttpException ex)
        {
            if (!string.IsNullOrEmpty(ex.ResponseText))
            {
                try
                {

                    var err = Serializer.DeserializeObject<ApiErrorResponseDTO>(ex.ResponseText);
                    var ex2 = ReliableHttpException.Create(err.ErrorMessage, ex);
                    ex2.ErrorCode = err.ErrorCode;
                    ex2.HttpStatus = err.HttpStatus;
                    return ex2;
                }
                catch
                {
                    // swallow
                }
            }

            return null;
        }

        private Uri _streamingUri;
        private Uri _rootUri;
        private static readonly ILog Log = LogManager.GetLogger(typeof(Client));
        private static string _versionNumber;
        private static string GetVersionNumber()
        {
            if (string.IsNullOrEmpty(_versionNumber))
            {
                var asm = Assembly.GetExecutingAssembly();
                var parts = asm.FullName.Split(',');
                _versionNumber = parts[1].Split('=')[1];
            }
            return _versionNumber;
        }



        private MagicNumberResolver _magicNumberResolver;
        public MagicNumberResolver MagicNumberResolver
        {
            get
            {
                if (_magicNumberResolver == null)
                {
                    _magicNumberResolver = new MagicNumberResolver(this);
                }

                return _magicNumberResolver;
            }
        }

        public string UserName { get; set; }
        public string Session { get; set; }




        public override string EndRequest(ReliableAsyncResult result)
        {
            try
            {
                return base.EndRequest(result);
            }
            catch (ReliableHttpException ex)
            {
                ReliableHttpException ex2 = GetJsonException(ex);
                if (ex2 != null)
                {
                    throw ex2;
                }

                throw;
            }
            catch (Exception ex)
            {
                throw new DefectException("expected ReliableHttpException. see inner", ex);
            }
        }

        public IStreamingClient CreateStreamingClient()
        {

            return new LightstreamerClient(_streamingUri, UserName, Session, Serializer);

        }


        #region Authentication Wrapper

        /// <summary>
        /// Log In
        /// </summary>		
        /// <param name="userName">Username is case sensitive</param>
        /// <param name="password">Password is case sensitive</param>
        /// <returns></returns>
        public ApiLogOnResponseDTO LogIn(String userName, String password)
        {
            

            UserName = userName;
            Session = null;

            var response = Request<ApiLogOnResponseDTO>(RequestMethod.POST, "session", "/", new Dictionary<string, object>
                                                                                         {
                                                                                             {"apiLogOnRequest", new ApiLogOnRequestDTO()
                                                                                                               {
                                                                                                                   UserName=userName,
                                                                                                                   Password = password,
                                                                                                                   AppKey = AppKey,
                                                                                                                   AppVersion = UserAgent
                                                                                                               }}
                                                                                         }, ContentType.JSON, ContentType.JSON, TimeSpan.FromMilliseconds(0), 30000, 2);
            Session = response.Session;
            return response;
        }

        /// <summary>
        /// Log In
        /// </summary>		
        /// <param name="callback"></param>
        /// <param name="state"></param>
        /// <param name="userName">Username is case sensitive</param>
        /// <param name="password">Password is case sensitive</param>
        /// <returns></returns>
        public void BeginLogIn(String userName, String password, ReliableAsyncCallback callback,
                               object state)
        {
            UserName = userName;
            Session = null;
            BeginRequest(RequestMethod.POST, "session", "/", new Dictionary<string, object>
                                                                      {
                                                                       {"apiLogOnRequest", new ApiLogOnRequestDTO()
                                                                        {
                                                                            UserName=userName,
                                                                            Password = password,
                                                                            AppKey = AppKey,
                                                                            AppVersion = UserAgent
                                                                            }
                                                                         }
                                                                      }, ContentType.JSON, ContentType.JSON, TimeSpan.Zero, 30000, 2, callback, state);
        }


        public ApiLogOnResponseDTO EndLogIn(ReliableAsyncResult asyncResult)
        {
            ApiLogOnResponseDTO response = EndRequest<ApiLogOnResponseDTO>(asyncResult);
            Session = response.Session;
            return response;
        }

        /// <summary>
        /// Log out
        /// </summary>		
        /// <returns></returns>
        public bool LogOut()
        {
            var response = Request<ApiLogOffResponseDTO>(RequestMethod.POST, "session",
                                                              "/deleteSession?userName={userName}&session={session}",
                                                               new Dictionary<string, object>
                                                                           {
                                                                               {"userName", UserName},
                                                                               {"session", Session},
                                                                           }, ContentType.JSON, ContentType.JSON, TimeSpan.FromMilliseconds(0), 30000, 2);
            if (response.LoggedOut)
            {
                Session = null;
            }

            return response.LoggedOut;
        }

        /// <summary>
        /// Log out
        /// </summary>		
        /// <param name="callback"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public void BeginLogOut(ReliableAsyncCallback callback, object state)
        {
            BeginRequest(RequestMethod.POST, "session", "/deleteSession?userName={userName}&session={session}",
                         new Dictionary<string, object>
                             {
                                 {"userName", UserName},
                                 {"session", Session},
                             }, ContentType.JSON, ContentType.JSON, TimeSpan.FromMilliseconds(0), 30000, 2, callback, state);
        }

        public bool EndLogOut(ReliableAsyncResult asyncResult)
        {
            ApiLogOffResponseDTO response = EndRequest<ApiLogOffResponseDTO>(asyncResult);

            if (response.LoggedOut)
            {
                Session = null;
            }

            return response.LoggedOut;
        }

        #endregion

        private Timer _metricsTimer;

        private void DisposeMetricsTimer()
        {

            try
            {
                if (_metricsTimer != null)
                {
                    _metricsTimer.Dispose();
                    _metricsTimer = null;
                }
            }
            catch
            {

                //swallow
            }

        }
        public void StartMetrics()
        {
            DisposeMetricsTimer();
            StartRecording(null);
            _metricsTimer = new Timer(ignored => PostMetrics(), null, 1000, 10000);
        }
        public void StopMetrics()
        {
            StopRecording();
            DisposeMetricsTimer();
        }

        private void PostMetrics()
        {
            string appmetricsUrl = "http://metrics.labs.cityindex.com/LogEvent.ashx";

            if (!IsRecording)
            {
                return;
            }

            var records = GetRecording();
            ClearRecording();
            if (records.Count == 0)
            {
                return;
            }

            var sb = new StringBuilder();
            foreach (var item in records)
            {
                try
                {
                    if (item.Uri.AbsoluteUri != appmetricsUrl)
                    {
                        sb.AppendLine(string.Format("{0}\tLatency {1}\t{2}", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fffffff"), item.Uri, item.Completed.Subtract(item.Issued).TotalSeconds));
                    }
                    
                }
                catch
                {

                    //swallow
                }
            }


            var latencyData = sb.ToString();
            Log.Debug("LATENCY:/n" + latencyData);

            base.BeginRequest(RequestMethod.POST,
                appmetricsUrl,
                "",
                new Dictionary<string, string>(),
                new Dictionary<string, object> { { "MessageAppKey", AppKey ?? "null" }, { "MessageSession", Session ?? "null" }, { "MessagesList", latencyData } },
                ContentType.FORM,
                ContentType.TEXT,
                TimeSpan.FromMilliseconds(0),
                30000,
                0, ar =>
                       {
                           EndRequest(ar);
                           Log.Debug("Latency message complete.");

                       }, null);


        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                DisposeMetricsTimer();
            }
            catch(Exception ex)
            {
                //swallow
                Log.Error("Error disposing metrics timer: /r/n" + ex.ToString());
            }
            base.Dispose(disposing);
        }
    }


}