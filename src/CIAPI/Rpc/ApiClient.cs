using System;
using System.Collections.Generic;
using System.Reflection;
using CIAPI.DTO;
using CIAPI.Streaming;
using Salient.ReflectiveLoggingAdapter;
using Salient.ReliableHttpClient;
using System.Text.RegularExpressions;

namespace CIAPI.Rpc
{


    // #TODO: reintegrate exception factory into ReliableHttpClient
    public partial class Client : ClientBase
    {



        private readonly IStreamingClientFactory _streamingFactory;
        /// <summary>
        /// used as a null target for json deserialization test
        /// </summary>
        public class NullObject
        {
            /// <summary>
            /// 
            /// </summary>
            public NullObject()
            {

            }
        }





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



        /// <summary>
        /// 
        /// </summary>
        public bool Http200ErrorsOnly { get; set; }


        #region Request Dispatch

        private static string AppendQueryParameter(string uriTemplate, string paramName)
        {

            if (uriTemplate.Contains("?"))
            {

                uriTemplate = uriTemplate + "&";
            }
            else
            {
                uriTemplate = uriTemplate + "?";
            }
            string paramPair = paramName + "={" + paramName + "}";
            uriTemplate = uriTemplate + paramPair;
            return uriTemplate;
        }

        private static string PrepareUrl(string url, string target  )
        {
            target = target ?? ""; 
            return !url.EndsWith("/") && !target.StartsWith("/") ? url + "/" + target : url + target;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <param name="target"></param>
        /// <param name="uriTemplate"></param>
        /// <param name="parameters"></param>
        /// <param name="requestContentType"></param>
        /// <param name="responseContentType"></param>
        /// <param name="cacheDuration"></param>
        /// <param name="timeout"></param>
        /// <param name="retryCount"></param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        /// <exception cref="ObjectDisposedException"></exception>
        public Guid BeginRequest(RequestMethod method, string target, string uriTemplate, Dictionary<string, object> parameters, ContentType requestContentType, ContentType responseContentType, TimeSpan cacheDuration, int timeout, int retryCount, ReliableAsyncCallback callback, object state)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }

            var param = new Dictionary<string, object>(parameters ?? new Dictionary<string, object>());
            MungeUrlParams(uriTemplate, param);
            target = PrepareUrl(_rootUri.AbsoluteUri, target );
            
            if (Http200ErrorsOnly)
            {
                uriTemplate = AppendQueryParameter(uriTemplate, "only200");
                param.Add("only200", "true");
            }
            Dictionary<string, string> headers = GetHeaders(target);
            return base.BeginRequest(method, target, uriTemplate, headers, param, requestContentType, responseContentType, cacheDuration, timeout, retryCount, callback, state);
        }

        private void MungeUrlParams(string uriTemplate, Dictionary<string, object> parameters)
        {

            new Regex(@"{\w+}").Replace(uriTemplate, match =>
            {
                string key = match.Value.Substring(1, match.Value.Length - 2);
                if (parameters.ContainsKey(key))
                {
                    object paramValue = parameters[key];

                    if (paramValue != null && (paramValue is string))
                    {
                        var paramString = (string)paramValue;
                        // munge the value to make safe
                        paramString = paramString.Replace("/", " ");
                        parameters[key] = paramString;
                    }
                }
                return null;
            });

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <param name="target"></param>
        /// <param name="uriTemplate"></param>
        /// <param name="parameters"></param>
        /// <param name="requestContentType"></param>
        /// <param name="responseContentType"></param>
        /// <param name="cacheDuration"></param>
        /// <param name="timeout"></param>
        /// <param name="retryCount"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="ObjectDisposedException"></exception>
        public T Request<T>(RequestMethod method, string target, string uriTemplate, Dictionary<string, object> parameters, ContentType requestContentType, ContentType responseContentType, TimeSpan cacheDuration, int timeout, int retryCount)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }
          
            var param = new Dictionary<string, object>(parameters ?? new Dictionary<string, object>());
            MungeUrlParams(uriTemplate, param);

            target = PrepareUrl(_rootUri.AbsoluteUri, target );   
            if (Http200ErrorsOnly)
            {
                uriTemplate = AppendQueryParameter(uriTemplate, "only200");
                param.Add("only200", "true");
            }
            Dictionary<string, string> headers = GetHeaders(target);

            return base.Request<T>(method, target, uriTemplate, headers, param, requestContentType, responseContentType, cacheDuration, timeout, retryCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <param name="target"></param>
        /// <param name="uriTemplate"></param>
        /// <param name="parameters"></param>
        /// <param name="requestContentType"></param>
        /// <param name="responseContentType"></param>
        /// <param name="cacheDuration"></param>
        /// <param name="timeout"></param>
        /// <param name="retryCount"></param>
        /// <returns></returns>
        /// <exception cref="ObjectDisposedException"></exception>
        public string Request(RequestMethod method, string target, string uriTemplate, Dictionary<string, object> parameters, ContentType requestContentType, ContentType responseContentType, TimeSpan cacheDuration, int timeout, int retryCount)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }

            
            var param = new Dictionary<string, object>(parameters ?? new Dictionary<string, object>());
            MungeUrlParams(uriTemplate, param);
            target = PrepareUrl(_rootUri.AbsoluteUri, target );
            if (Http200ErrorsOnly)
            {
                uriTemplate = AppendQueryParameter(uriTemplate, "only200");
                param.Add("only200", "true");
            }
            Dictionary<string, string> headers = GetHeaders(target);
            return base.Request(method, target, uriTemplate, headers, param, requestContentType, responseContentType, cacheDuration, timeout, retryCount);
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            _disposed = true;
            if (disposing)
            {
                // dispose components
            }

            base.Dispose(disposing);
        }
        private bool _disposed;

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
                _versionNumber.Substring(0, _versionNumber.LastIndexOf(".")-2);
            }
            return _versionNumber;
        }



        private MagicNumberResolver _magicNumberResolver;
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Session { get; set; }




        private ReliableHttpException CreateApiException(string responseText)
        {
            ReliableHttpException ex2 = null;
            try
            {
                var err = Serializer.DeserializeObject<ApiErrorResponseDTO>(responseText);
                switch (err.ErrorCode)
                {
                    case (int)ErrorCode.Forbidden:
                        ex2 = new ForbiddenException(err.ErrorMessage);
                        break;
                    case (int)ErrorCode.InternalServerError:
                        ex2 = new InternalServerErrorException(err.ErrorMessage);
                        break;
                    case (int)ErrorCode.InvalidCredentials:
                        ex2 = new InvalidCredentialsException(err.ErrorMessage);
                        break;
                    case (int)ErrorCode.InvalidJsonRequest:
                        ex2 = new InvalidJsonRequestException(err.ErrorMessage);
                        break;
                    case (int)ErrorCode.InvalidJsonRequestCaseFormat:
                        ex2 = new InvalidJsonRequestCaseFormatException(err.ErrorMessage);
                        break;
                    case (int)ErrorCode.InvalidParameterType:
                        ex2 = new InvalidParameterTypeException(err.ErrorMessage);
                        break;
                    case (int)ErrorCode.InvalidParameterValue:
                        ex2 = new InvalidParameterValueException(err.ErrorMessage);
                        break;
                    case (int)ErrorCode.InvalidSession:
                        ex2 = new InvalidSessionException(err.ErrorMessage);
                        break;
                    case (int)ErrorCode.NoDataAvailable:
                        ex2 = new NoDataAvailableException(err.ErrorMessage);
                        break;
                    case (int)ErrorCode.ParameterMissing:
                        ex2 = new ParameterMissingException(err.ErrorMessage);
                        break;
                    case (int)ErrorCode.Throttling:
                        ex2 = new ThrottlingException(err.ErrorMessage);
                        break;
                    default:
                        ex2 = new ReliableHttpException(err.ErrorMessage);
                        break;

                }

                ex2.ResponseText = responseText;
                ex2.ErrorCode = err.ErrorCode;
                ex2.HttpStatus = err.HttpStatus;
            }
            // ReSharper disable EmptyGeneralCatchClause
            catch
            // ReSharper restore EmptyGeneralCatchClause
            {
                //swallow
            }
            return ex2;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="ReliableHttpException"></exception>
        /// <exception cref="ServerConnectionException"></exception>
        public override string EndRequest(ReliableAsyncResult result)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }
            string responseText;
            try
            {
                responseText = base.EndRequest(result);
            }
            catch (ReliableHttpException ex)
            {
                ReliableHttpException ex2 = null;

                if (!string.IsNullOrEmpty(ex.ResponseText))
                {
                    try
                    {
                        ex2 = CreateApiException(ex.ResponseText);
                    }
                    catch
                    {
                        // swallow
                    }
                }

                if (ex2 != null)
                {
                    throw ex2;
                }

                throw;
            }
            catch (Exception ex)
            {
                throw ReliableHttpException.Create(ex);
            }

            if (responseText.Contains("\"HttpStatus\"") && responseText.Contains("\"ErrorMessage\"") && responseText.Contains("\"ErrorCode\""))
            {

                ReliableHttpException ex2 = CreateApiException(responseText);
                if (ex2 != null)
                {
                    throw ex2;
                }
            }

            // at this point, if we don't have json then it is an error

            try
            {
                Serializer.DeserializeObject<NullObject>(responseText);
            }
            catch
            {

                throw new ServerConnectionException("Invalid response received.  Are you connecting to the correct server Url?", responseText);
            }

            return responseText;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IStreamingClient CreateStreamingClient()
        {

            return CreateStreamingClient(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="usePolling"></param>
        /// <returns></returns>
        /// <exception cref="ObjectDisposedException"></exception>
        public IStreamingClient CreateStreamingClient(bool usePolling)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }

            return _streamingFactory.Create(_streamingUri, UserName, Session, usePolling, Serializer);
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
                                                                                                                   AppVersion = "CIAPI.CS"
                                                                                                                   // #TODO: stop putting version in this DTO - cannot record with versions changing. Version is available in headers
                                                                                                               }}
                                                                                         }, ContentType.JSON, ContentType.JSON, TimeSpan.FromMilliseconds(0), 30000, 2);

            //#FIXME: timeout is not throwing exception - just null response
            if (response == null)
            {
                throw new Exception("No response recieved");
            }
            Session = response.Session;
            return response;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="session"></param>
        public void LogInUsingSession(string username, string session)
        {
            try
            {
                this.UserName = username;
                this.Session = session;
                AccountInformationResponseDTO d = this.AccountInformation.GetClientAndTradingAccount();

            }
            catch
            {
                Session = null;
                throw;
            }
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
                                                                            AppVersion = "CIAPI.CS"
                                                                               // #TODO: stop putting version in this DTO - cannot record with versions changing. Version is available in headers
                                                                                                           
                                                                            }
                                                                         }
                                                                      }, ContentType.JSON, ContentType.JSON, TimeSpan.Zero, 30000, 2, callback, state);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="asyncResult"></param>
        /// <returns></returns>
        public ApiLogOnResponseDTO EndLogIn(ReliableAsyncResult asyncResult)
        {
            var response = EndRequest<ApiLogOnResponseDTO>(asyncResult);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="asyncResult"></param>
        /// <returns></returns>
        public bool EndLogOut(ReliableAsyncResult asyncResult)
        {
            var response = EndRequest<ApiLogOffResponseDTO>(asyncResult);

            if (response.LoggedOut)
            {
                Session = null;
            }

            return response.LoggedOut;
        }

        #endregion


    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class InvalidCredentialsException : ReliableHttpException
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public InvalidCredentialsException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public InvalidCredentialsException(string message, Exception ex)
            : base(message, ex)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        public InvalidCredentialsException(Exception ex)
            : base(ex)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public InvalidCredentialsException(string message, ReliableHttpException exception)
            : base(message, exception)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        public InvalidCredentialsException(ReliableHttpException exception)
            : base(exception)
        {
        }
    }
}