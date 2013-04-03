using System;
using System.Threading;

namespace Salient.ReliableHttpClient
{
    public class HttpClient
    {
        private readonly RequestController _controller;

        public HttpClient()
        {
            _controller = new RequestController();
        }

        public void BeginRequest(RequestData data, HttpAsyncCallback callback, Object state)
        {
            _controller.BeginRequest(data, callback, state);
        }

        public void EndRequest(HttpAsyncResult result)
        {
            _controller.EndRequest(result);
        }

        public HttpAsyncResult Request(RequestData data, object state)
        {
            ThrowUIThreadExceptionIfSilverlight();

            var result = new HttpAsyncResult();
            var gate = new ManualResetEvent(false);

            BeginRequest(data, ar =>
                {
                    EndRequest(ar);
                    gate.Set();
                }, state);


            gate.WaitOne();

            return result;
        }


        private static void ThrowUIThreadExceptionIfSilverlight()
        {
            DesignerPlatformLibrary platform = DesignerLibrary.DetectedDesignerLibrary;
            switch (platform)
            {
                case DesignerPlatformLibrary.Silverlight:
                    throw new InvalidOperationException(
                        "You cannot call this method from the UI thread.  Either use the asynchronous method: .Begin{name}, or call this from a background thread");

                case DesignerPlatformLibrary.Net:
                case DesignerPlatformLibrary.WinRT:
                case DesignerPlatformLibrary.Unknown:
                    break;
            }
        }
    }
}