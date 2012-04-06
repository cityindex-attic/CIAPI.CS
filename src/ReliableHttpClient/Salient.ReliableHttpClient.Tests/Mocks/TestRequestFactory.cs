using System;
using System.Collections.Generic;
using System.Net;
using Salient.ReflectiveLoggingAdapter;


namespace Salient.ReliableHttpClient.Tests
{
    public class TestRequestFactory : IRequestFactory
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(RequestFactory));
        public List<TestRequestInfo> RequestLog = new List<TestRequestInfo>();
        WebRequest _nextRequest;
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

        public WebRequest NextRequest
        {
            get { return _nextRequest; }
            set
            {
                lock (_lock)
                {
                    _nextRequest = value;
                }
            }
        }

        /// <summary>See <see cref="IWebRequestCreate.Create"/>.</summary>
        public WebRequest Create(string url)
        {
            lock (RequestLog)
            {
                RequestLog.Add(new TestRequestInfo
                {
                    Url = url,
                    Issued = DateTimeOffset.UtcNow
                });
            }
            return _nextRequest;
        }

        public TimeSpan RequestTimeout { get; set; }

        /// <summary>Utility method for creating a TestWebRequest and setting
        /// it to be the next WebRequest to use.</summary>
        /// <param name="response">The response the TestWebRequest will return.</param>
        public TestWebRequest CreateTestRequest(string response)
        {
            return CreateTestRequest(response, TimeSpan.FromMilliseconds(10));
        }

        public TestWebRequest CreateTestRequest(string response, TimeSpan latency)
        {
            return CreateTestRequest(response, latency, null, null, null);
        }

        public TestWebRequest CreateTestRequest(string response, TimeSpan latency, Exception requestStreamException, Exception responseStreamException, Exception endGetResponseException)
        {
            var request = new TestWebRequest(response, latency, requestStreamException, responseStreamException, endGetResponseException);
#if !SILVERLIGHT
            request.Timeout = Convert.ToInt32(RequestTimeout.TotalMilliseconds);
#else
            //FIXME: Need a way to timeout requests in Silverlight
#endif
            NextRequest = request;
            return request;
        }

    }
}