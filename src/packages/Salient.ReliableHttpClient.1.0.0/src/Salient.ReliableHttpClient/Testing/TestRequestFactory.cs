using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using Salient.ReflectiveLoggingAdapter;

namespace Salient.ReliableHttpClient.Testing
{
    public class TestRequestFactory : IRequestFactory
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(TestRequestFactory));

        public TestWebRequestPrepare PrepareResponse;
        //WebRequest _nextRequest;
        readonly object _lock = new object();
        public List<TestWebRequest> TestWebRequests { get; set; }
        public void AddTestRequest(TestWebRequest testRequest)
        {
            TestWebRequests.Add(testRequest);
        }
        public TestRequestFactory()
        {
            RequestTimeout = TimeSpan.FromSeconds(30);
        }



        public WebRequest Create(string url)
        {
            var request = new TestWebRequest(new Uri(url)) { PrepareResponse = PrepareResponseCore };
            return request;
        }

        private void PrepareResponseCore(TestWebRequest request)
        {
            if (PrepareResponse!=null)
            {
                PrepareResponse(request);
            }
        }

        public TimeSpan RequestTimeout { get; set; }

        //        /// <summary>Utility method for creating a TestWebRequest and setting
        //        /// it to be the next WebRequest to use.</summary>
        //        /// <param name="response">The response the TestWebRequest will return.</param>
        //        public TestWebRequest CreateTestRequest(string response)
        //        {
        //            return CreateTestRequest(response, TimeSpan.FromMilliseconds(10));
        //        }

        //        public TestWebRequest CreateTestRequest(string response, TimeSpan latency)
        //        {
        //            return CreateTestRequest(response, latency, null, null, null);
        //        }

        //        public TestWebRequest CreateTestRequest(string response, TimeSpan latency, Exception requestStreamException, Exception responseStreamException, Exception endGetResponseException)
        //        {
        //            var request = new TestWebRequest(response, latency, requestStreamException, responseStreamException, endGetResponseException);
        //#if !SILVERLIGHT
        //            request.Timeout = Convert.ToInt32(RequestTimeout.TotalMilliseconds);
        //#else
        //            //FIXME: Need a way to timeout requests in Silverlight
        //#endif
        //            NextRequest = request;
        //            return request;
        //        }

    }
}
