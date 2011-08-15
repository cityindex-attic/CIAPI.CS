using System;
using System.Collections.Generic;
using System.Net;
using CIAPI.DTO;
using CityIndex.JsonClient;

namespace CIAPI.Rpc
{
    public partial class Client : CityIndex.JsonClient.Client
    {
    


        public string UserName { get; set; }
        public string Session { get; set; }

        /// <summary>
        /// Authenticates the request with the API using request headers.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="url"></param>
        /// <param name="target"></param>
        /// <param name="uriTemplate"></param>
        /// <param name="method"></param>
        /// <param name="parameters"></param>
        /// <param name="cacheDuration"></param>
        /// <param name="throttleScope"></param>
        protected override void BeforeIssueRequest(WebRequest request, string url, string target, string uriTemplate,
                                                   string method, Dictionary<string, object> parameters,
                                                   TimeSpan cacheDuration, string throttleScope)
        {
            if (url.IndexOf("/session", StringComparison.OrdinalIgnoreCase) == -1)
            {
                request.Headers["UserName"] = UserName;
                if (Session == null)
                {
                    throw new ApiException("Session is null. Have you created a session? (logged on)");
                }
                request.Headers["Session"] = Session;
            }
        }

        #region AUthentication Wrapper

        /// <summary>
        /// Log In
        /// </summary>		
        /// <param name="userName">Username is case sensitive</param>
        /// <param name="password">Password is case sensitive</param>
        /// <returns></returns>
        public void LogIn(String userName, String password)
        {
            UserName = userName;
            Session = null;

            var response = Request<ApiLogOnResponseDTO>("session", "/", "POST", new Dictionary<string, object>
                                                                                         {
                                                                                             {"apiLogOnRequest", new ApiLogOnRequestDTO()
                                                                                                               {
                                                                                                                   UserName=userName,
                                                                                                                   Password = password
                                                                                                               }}
                                                                                         }, TimeSpan.FromMilliseconds(0),
                                                             "data");
            Session = response.Session;
        }

        /// <summary>
        /// Log In
        /// </summary>		
        /// <param name="callback"></param>
        /// <param name="state"></param>
        /// <param name="userName">Username is case sensitive</param>
        /// <param name="password">Password is case sensitive</param>
        /// <returns></returns>
        public void BeginLogIn(String userName, String password, ApiAsyncCallback<ApiLogOnResponseDTO> callback,
                               object state)
        {
            UserName = userName;
            Session = null;

            BeginRequest(callback, state, "session", "/", "POST", new Dictionary<string, object>
                                                                      {
                                                                       {"apiLogOnRequest", new ApiLogOnRequestDTO()
                                                                        {
                                                                            UserName=userName,
                                                                            Password = password
                                                                            }
                                                                         }
                                                                      }, TimeSpan.FromMilliseconds(0), "data");
        }

        public override TDTO EndRequest<TDTO>(ApiAsyncResult<TDTO> asyncResult)
        {
            try
            {
                TDTO response = base.EndRequest(asyncResult);
                return response;
            }
            catch (ApiSerializationException ex)
            {
                throw new ServerConnectionException(
                    "Invalid response received.  Are you connecting to the correct server Url?  See ResponseText property for further details of response received.",
                    ex.ResponseText);
            }
        }


        public void EndLogIn(ApiAsyncResult<ApiLogOnResponseDTO> asyncResult)
        {
            ApiLogOnResponseDTO response = EndRequest(asyncResult);
            Session = response.Session;
        }

        /// <summary>
        /// Log out
        /// </summary>		
        /// <returns></returns>
        public bool LogOut()
        {
            var response = Request<ApiLogOffResponseDTO>("session",
                                                               "/deleteSession?userName={userName}&session={session}",
                                                               "POST", new Dictionary<string, object>
                                                                           {
                                                                               {"userName", UserName},
                                                                               {"session", Session},
                                                                           }, TimeSpan.FromMilliseconds(0), "data");
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
        public void BeginLogOut(ApiAsyncCallback<ApiLogOffResponseDTO> callback, object state)
        {
            BeginRequest(callback, state, "session", "/deleteSession?userName={userName}&session={session}", "POST",
                         new Dictionary<string, object>
                             {
                                 {"userName", UserName},
                                 {"session", Session},
                             }, TimeSpan.FromMilliseconds(0), "data");
        }

        public bool EndLogOut(ApiAsyncResult<ApiLogOffResponseDTO> asyncResult)
        {
            ApiLogOffResponseDTO response = EndRequest(asyncResult);

            if (response.LoggedOut)
            {
                Session = null;
            }

            return response.LoggedOut;
        }

        #endregion
    }
}