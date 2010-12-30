using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization.Json;
using log4net;
using Microsoft.Http;
using RESTWebServicesDTO.Request;
using RESTWebServicesDTO.Response;
using TradingApi.Client.Core.Exceptions;
using TradingApi.Client.Core.Lightstreamer;

namespace TradingApi.Client.Core
{
    public class Connection
    {
        private static ILog _log = LogManager.GetLogger(typeof(Connection));
        public IWebClient WebClient { get; internal set; }
        private ILightstreamerConnection _lightstreamerConnection;
        public ILightstreamerConnection LightstreamerConnection
        {
            get
            {
                if (_lightstreamerConnection==null)
                {
                    _lightstreamerConnection = new LightstreamerConnection(_lightStreamerServerUrl, UserName, Session, _adapter);
                }
                return _lightstreamerConnection;
            }
            private set
            {
                _lightstreamerConnection = value;
            }
        }

        private readonly string _password;
        private readonly string _lightStreamerServerUrl;
        private readonly string _adapter;
        private string _session;

        public string UserName { get; private set; }

        public string Session
        {
            get
            {
                if (string.IsNullOrEmpty(_session))
                {
                    Authenticate(UserName, _password);
                }
                return _session;
            }
        }

        public Connection(string userName, string session, string tradingApiServerUrl)
        {
            UserName = userName;
            _session = session;
            WebClient = new WebClient(tradingApiServerUrl);
        }


        public Connection(string userName, string password, string tradingApiServerUrl, string lightStreamerServerUrl, string adapter)
        {
            WebClient = new WebClient(tradingApiServerUrl);
            UserName = userName;
            _password = password;
            _lightStreamerServerUrl = lightStreamerServerUrl;
            _adapter = adapter;
        }

        public CreateSessionResponseDTO Authenticate(string username, string password)
        {
            var response = UnauthenticatedPost("/Session", new LogOnRequestDTO {UserName = username, Password = password});
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new ApiCallException(String.Format("Http Status Code {0} {1}", (int)response.StatusCode, response.StatusCode), response.StatusCode);
            }

            var session = response.Content.ReadAsJsonDataContract<CreateSessionResponseDTO>();
            if(session.Session != "")
            {
                UserName = username;
                _session = session.Session;
            }
            else
            {
                UserName = "";
                _session = "";
            }

            return session;
        }

        private HttpResponseMessage UnauthenticatedPost<T>(string url, T postData)
        {
            return WebClient.PostRequest(url, postData, new Dictionary<string, string>());
        }

        public HttpResponseMessage Get(string url)
        {
            var webHeaderCollection = new Dictionary<string, string>
                                          {
                                              {"UserName", UserName},
                                              {"Session", Session}
                                          };
            return WebClient.GetRequest(url, webHeaderCollection);
        }

        public SessionDeletionResponseDTO Logout(string username, string session)
        {
            var response = UnauthenticatedPost("/Session?_method=DELETE", new SessionDeletionRequestDTO { UserName = username, Session = session });
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new ApiCallException(String.Format("Http Status Code {0} {1}", (int)response.StatusCode, response.StatusCode), response.StatusCode);
            }

            var logout = response.Content.ReadAsJsonDataContract<SessionDeletionResponseDTO>();
            if (logout.LoggedOut)
            {
                UserName = "";
                _session = "";
            }

            return logout;
        }

        public void Logout()
        {
            Logout(UserName, _session);
        }
    }
}
