using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using log4net;
using Microsoft.Http;
using Microsoft.Http.Headers;

namespace TradingApi.Client.Core
{
    public class WebClient : IWebClient
    {
        private static ILog _log = LogManager.GetLogger(typeof(WebClient));
        public event EventHandler<WebResponseEventArgs> Response;
        public string ServerUrl { get ;  private set; }

        public WebClient(string tradingApiServerUrl)
        {
            ServerUrl = tradingApiServerUrl;
        }

        public HttpResponseMessage PostRequest<T>(string url, T data, Dictionary<string, string> webHeaderCollection)
        {
            var fullRequestUri = ServerUrl + url;

            var content = HttpContentExtensions.CreateJsonDataContract(data);
            HttpResponseMessage response;
            using (var client = new HttpClient())
            {
                foreach (var header in webHeaderCollection)
                {
                    client.DefaultHeaders.Add(header.Key, header.Value);
                }
                content.LoadIntoBuffer();
                _log.DebugFormat("POST {0} with data: {1} and headers: {2}", fullRequestUri, content.ReadAsString(), RequestHeadersAsString(client));
                response = client.Post(fullRequestUri, content);
            }

            return response;
        }

        private static string RequestHeadersAsString(HttpClient client)
        {
            var sb = new StringBuilder();
            sb.Append("{");
            foreach (var header in client.DefaultHeaders)
            {
                sb.AppendFormat(@"{0}:""{1}""", header.Key, header.Value);
            }
            sb.Append("}");

            return sb.ToString();
        }

        public HttpResponseMessage GetRequest(string url, Dictionary<string, string> webHeaderCollection)
        {
            var fullRequestUri = ServerUrl + url;
            HttpResponseMessage response;
            using (var client = new HttpClient())
            {

                foreach (var header in webHeaderCollection)
                {
                    client.DefaultHeaders.Add(header.Key, header.Value);
                }
                _log.DebugFormat("GET {0} with headers: {1}", fullRequestUri, RequestHeadersAsString(client));
                response = client.Get(fullRequestUri);
            }

            return response;
        }

        public void BeginGetRequest(string serviceUrl, Dictionary<string, string> webHeaderCollection)
        {
            var fullRequestUri = ServerUrl + serviceUrl;
            WebRequest webRequest = WebRequest.Create(fullRequestUri);
            webRequest.Method = "GET";

            var state = new RequestState { Request = webRequest, Data = new object(), Url = fullRequestUri };
            webRequest.BeginGetResponse(OnWebResponse,state);
        }

        private void OnWebResponse(IAsyncResult result)
        {
            if (Response == null) return;

            var state = (RequestState)result.AsyncState;
            
            string dataString;
            using (var responseStream = state.Request.EndGetResponse(result).GetResponseStream())
            {
                using (var streamReader = new StreamReader(responseStream))
                {
                    dataString = streamReader.ReadToEnd();
                }
            }

            Response(this,new WebResponseEventArgs{ResponseData = dataString});
        }

        private class RequestState
        {
            public WebRequest Request { get; set;}// holds the request
            public object Data { get; set;} // store any data in this
            public string Url { get; set;} // holds the Url to match up results (Database lookup, etc).
        }
    }
}