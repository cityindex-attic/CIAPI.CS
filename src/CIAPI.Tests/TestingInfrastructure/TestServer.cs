using System;
using System.Globalization;

namespace CIAPI.Tests.TestingInfrastructure
{
    public class TestServer : ServerBase
    {

        public TestServer()
            : base(GetAvailablePort(), 1024)
        {

        }
        public TestServer(int port, int receiveBuffer)
            : base(port, receiveBuffer)
        {
        }

        public static ResponseInfo CreateRpcResponse(string body)
        {

            var response = new ResponseInfo { Body = body };

            response.Headers["Cache-Control"] = "no-cache";
            response.Headers["Pragma"] = "no-cache";
            response.Headers["Content-Type"] = "text/json; charset=utf-8";
            response.Headers["Expires"] = "-1";
            response.Headers["Vary"] = "Accept-Encoding";
            response.Headers["Server"] = "TestServer";
            response.Headers["Date"] = DateTime.UtcNow.ToString("R");
            if (!string.IsNullOrEmpty(body))
            {
                response.Headers["Content-Length"] = body.Length.ToString(CultureInfo.InvariantCulture);
            }
            return response;
        }
        public static ResponseInfo CreateLightStreamerResponse(string body)
        {
            var response = new ResponseInfo { Body = body };

            response.Headers["Content-Type"] = "text/plain; charset=iso-8859-1";
            response.Headers["Cache-Control"] = "no-cache";
            response.Headers["Pragma"] = "no-cache";
            response.Headers["Expires"] = "Thu, 1 Jan 1970 00:00:00 UTC";
            response.Headers["Server"] = "TestServer";
            response.Headers["Date"] = DateTime.UtcNow.ToString("R");
            if (!string.IsNullOrEmpty(body))
            {
                response.Headers["Content-Length"] = body.Length.ToString(CultureInfo.InvariantCulture);
            }
            return response;
        }
        public override ResponseInfo HandleRequest(RequestInfo request)
        {
            var args = new RequestEventArgs { Request = request };
            OnProcessRequest(args);
            return args.Response;
        }

        public event EventHandler<RequestEventArgs> ProcessRequest;

        protected virtual void OnProcessRequest(RequestEventArgs e)
        {
            EventHandler<RequestEventArgs> handler = ProcessRequest;
            if (handler != null) handler(this, e);
        }
    }





}