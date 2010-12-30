using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.Http;

namespace TradingApi.Client.Core
{
    public interface IWebClient
    {
        HttpResponseMessage PostRequest<T>(string serviceUrl, T data, Dictionary<string, string> webHeaderCollection);
        HttpResponseMessage GetRequest(string url, Dictionary<string, string> webHeaderCollection);

        void BeginGetRequest(string serviceUrl, Dictionary<string, string> webHeaderCollection);
        event EventHandler<WebResponseEventArgs> Response;
        string ServerUrl { get; }
    }

    public class WebResponseEventArgs : EventArgs
    {
        public string ResponseData { get; set; }
    }
}
