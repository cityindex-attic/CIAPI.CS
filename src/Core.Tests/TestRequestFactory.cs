using System.Net;

namespace CIAPI.Core.Tests
{
    public class TestRequestFactory : IRequestFactory
    {
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
            return _nextRequest;
        }

        /// <summary>Utility method for creating a TestWebRequest and setting
        /// it to be the next WebRequest to use.</summary>
        /// <param name="response">The response the TestWebRequest will return.</param>
        public TestWebRequest CreateTestRequest(string response)
        {
            var request = new TestWebRequest(response);
            NextRequest = request;
            return request;
        }
    }
}