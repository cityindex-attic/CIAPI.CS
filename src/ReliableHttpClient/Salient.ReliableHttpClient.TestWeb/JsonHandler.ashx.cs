using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salient.ReliableHttpClient.TestWeb
{
    /// <summary>
    /// Summary description for JsonHandler
    /// </summary>
    public class JsonHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write("Hello World");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}