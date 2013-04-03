using System;
using Salient.ReliableHttpClient.Exceptions;

namespace Salient.ReliableHttpClient
{
    public class HttpAsyncResult
    {
        public RequestData Data { get; set; }
        public Object State { get; set; }
        public ReliableHttpException Exception { get; set; }
    }
}