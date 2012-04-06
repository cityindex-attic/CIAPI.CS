using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Salient.ReliableHttpClient.TestWeb
{
    /// <summary>
    /// Summary description for SampleClientHandler
    /// </summary>
    public class SampleClientHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string shouldThrow = context.Request.QueryString["throw"];
            context.Response.ContentType = "application/json";

            if (!string.IsNullOrEmpty(shouldThrow))
            {
      
                throw new Exception("FOO");
            }
            else
            {
                var response = new { Id = 1 };
                context.Response.Write(JsonConvert.SerializeObject(response));
            }

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