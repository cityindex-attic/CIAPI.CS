using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using CIAPI.DTO;
using CIAPI.Streaming;
using Salient.ReflectiveLoggingAdapter;
using Salient.ReliableHttpClient;

namespace CIAPI.Rpc
{


    // #TODO: reintegrate exception factory into ReliableHttpClient
    public partial class Client : ClientBase
    {

        /// <summary>
        /// used as a null target for json deserialization test
        /// </summary>
        private class NullObject
        {

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
        public override string EndRequest(ReliableAsyncResult result)
        {
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


    }
}