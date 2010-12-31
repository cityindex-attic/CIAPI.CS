using System;
using System.Net;

namespace CIAPI.Core
{
    public class HttpRequest
    {
        public string Url { get; set; }
        public WebRequest WebRequest { get; set; }
        public Action<IAsyncResult> AsyncResultHandler { get; set; }
    }
}