using System;
using System.Collections.Generic;
using System.Net;
using log4net;

namespace CIAPI.Core.Tests
{

    public class TestRequestFactory : IRequestFactory
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(RequestFactory));
        public List<TestRequestInfo> RequestLog = new List<TestRequestInfo>();
        WebRequest _nextRequest;
        readonly object _lock = new object();

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

        


        /// <summary>Utility method for creating a TestWebRequest and setting
        /// it to be the next WebRequest to use.</summary>
        /// <param name="response">The response the TestWebRequest will return.</param>
        public TestWebRequest CreateTestRequest(string response)
        {
            return CreateTestRequest(response, 10);
        }

        public TestWebRequest CreateTestRequest(string response, int latency)
        {
            return CreateTestRequest(response, latency, null, null);
        }

        public TestWebRequest CreateTestRequest(string response, int latency, Exception requestStreamException, Exception responseStreamException)
        {
            var request = new TestWebRequest(response, latency, requestStreamException, responseStreamException);
            NextRequest = request;
            return request;
        }

    }
}