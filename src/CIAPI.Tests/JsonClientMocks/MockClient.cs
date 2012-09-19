using System;
using System.Text.RegularExpressions;
using CIAPI.Rpc;
using Salient.JsonClient;
using Salient.JsonClient.Tests;
using Client = CIAPI.Rpc.Client;

namespace CIAPI.Testing
{
    public class MockClient
    {
        private Client _client;
        private static TestRequestFactory _requestFactory;

        public static MockClient AuthenticatedClient()
        {
            _requestFactory = new TestRequestFactory();
            var requestController = new RequestController(TimeSpan.FromSeconds(0), 0, _requestFactory, new ErrorResponseDTOJsonExceptionFactory(), new ThrottledRequestQueue(TimeSpan.FromSeconds(5), 30, 10, "data"), new ThrottledRequestQueue(TimeSpan.FromSeconds(3), 1, 3, "trading"));

            var authenticatedClient = new MockClient
            {
                _client =
                    new Client(new Uri("https://mock.server.com/TradingAPI"), "mockAPIKEY",
                               requestController)
                    {
                        UserName = "MOCKUSERNAME",
                        Session = "MOCKSESSION"
                    }
            };
            return authenticatedClient;
        }

        public MockClient AddResponse(Regex requestUriRegEx, string response)
        {
            _requestFactory.AddTestRequest(TestWebRequest.CreateTestRequest(requestUriRegEx, response, TimeSpan.Zero, null, null, null));
            return this;
        }

        public MockClient AddResponse(TestWebRequest testWebRequest)
        {
            _requestFactory.AddTestRequest(testWebRequest);
            return this;
        }

        public Client Build()
        {
            return _client;
        }
    }
}