using System;
using System.Net;

namespace CityIndex.JsonClient
{
    public class RequestHolder
    {
        public string Url { get; set; }
        public WebRequest WebRequest { get; set; }
        public Action<IAsyncResult, RequestHolder> AsyncResultHandler { get; set; }
        
    }
}