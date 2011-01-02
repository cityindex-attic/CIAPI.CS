using System;
using System.Collections.Generic;
using System.Net;
using CityIndex.JsonClient;
using CityIndex.RestWebServices.DTO;

namespace CityIndex.RestWebServices
{
    public partial class ApiClient : Client
    {
        public ApiClient(Uri uri)
            : base(uri)
        {
        }

        public ApiClient(Uri uri, RequestCache cache, IRequestFactory requestFactory,
                          Dictionary<string, IThrottedRequestQueue> throttleScopes, int retryCount)
            : base(uri, cache, requestFactory, throttleScopes, retryCount)
        {
        }

        public string UserName { get; set; }
        public Guid SessionId { get; set; }

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
                // API advertises session id as a GUID but treats as a string internally so we need to ucase here.
                request.Headers["Session"] = SessionId.ToString().ToUpper();
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
            SessionId = Guid.Empty;

            var response = Request<CreateSessionResponseDTO>("session", "/", "POST", new Dictionary<string, object>
                {
                    {"UserName", userName},
                    {"Password", password},
                }, TimeSpan.FromMilliseconds(0), "data");
            SessionId = new Guid(response.Session);
        }

        /// <summary>
        /// Log In
        /// </summary>		
        /// <param name="callback"></param>
        /// <param name="state"></param>
        /// <param name="userName">Username is case sensitive</param>
        /// <param name="password">Password is case sensitive</param>
        /// <returns></returns>
        public void BeginLogIn(ApiAsyncCallback<CreateSessionResponseDTO> callback, object state, String userName,
                               String password)
        {
            UserName = userName;
            SessionId = Guid.Empty;

            BeginRequest(callback, state, "session", "/", "POST", new Dictionary<string, object>
                {
                    {"UserName", userName},
                    {"Password", password},
                }, TimeSpan.FromMilliseconds(0), "data");
        }

        public void EndLogIn(ApiAsyncResult<CreateSessionResponseDTO> asyncResult)
        {
            CreateSessionResponseDTO response = EndRequest(asyncResult);
            SessionId = new Guid(response.Session);
        }

        /// <summary>
        /// Log out
        /// </summary>		
        /// <returns></returns>
        public bool LogOut()
        {
            var response = Request<SessionDeletionResponseDTO>("session",
                                                               "/deleteSession?userName={userName}&session={session}",
                                                               "POST", new Dictionary<string, object>
                                                                   {
                                                                       {"userName", UserName},
                                                                       {"session", SessionId},
                                                                   }, TimeSpan.FromMilliseconds(0), "data");
            if (response.LoggedOut)
            {
                SessionId = Guid.Empty;
            }

            return response.LoggedOut;
        }

        /// <summary>
        /// Log out
        /// </summary>		
        /// <param name="callback"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public void BeginLogOut(ApiAsyncCallback<SessionDeletionResponseDTO> callback, object state)
        {
            BeginRequest(callback, state, "session", "/deleteSession?userName={userName}&session={session}", "POST",
                         new Dictionary<string, object>
                             {
                                 {"userName", UserName},
                                 {"session", SessionId},
                             }, TimeSpan.FromMilliseconds(0), "data");
        }

        public bool EndLogOut(ApiAsyncResult<SessionDeletionResponseDTO> asyncResult)
        {
            SessionDeletionResponseDTO response = EndRequest(asyncResult);

            if (response.LoggedOut)
            {
                SessionId = Guid.Empty;
            }

            return response.LoggedOut;
        }

        #endregion
    }
}