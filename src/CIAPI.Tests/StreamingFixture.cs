using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using CityIndex.JsonClient;
using CityIndex.JsonClient.Tests;
using NUnit.Framework;
using TradingApi.Client.Core.ClientDTO;

namespace CIAPI.Tests
{
    [TestFixture]
    public class StreamingFixture
    {
        private TestStreamingConnectionFactory _testStreamingConnectionFactory;
        private const string LoggedIn = "{\"Session\":\"D2FF3E4D-01EA-4741-86F0-437C919B5559\"}";
        private const string LoggedOut = "{\"LoggedOut\":true}";

        private static Rpc.Client BuildDefaultTestClient()
        {
            Uri rpcUri = new Uri("https://rpc.com/TradigApi");
            Uri streamingUri = new Uri("https://streaming.com");
            return new RpcClient(rpcUri, streamingUri);
        }

        [Test]
        public void ApiClientCannotCreateSubscriptionIfNotAuthenticated()
        {
            const string path = "foo";
            RpcClient ctx = BuildDefaultTestClient();
            Assert.Throws<InvalidOperationException>(() => ctx.CreateNewsSubscription(path));
        }

        [Test]
        public void ApiClientCanCreateSubscriptionWhenLoggedIn()
        {
            const string path = "foo";
            RpcClient ctx = CreateAuthenticatedApiClient();
            var actual = ctx.CreateNewsSubscription(path);
            Assert.IsInstanceOf(typeof(NewsHeadlineSubscription), actual);
        }


        [Test]
        public void ASubscriptionWillRaiseUpdateEvents()
        {
            const string path = "foo";
            Rpc.Client ctx = CreateAuthenticatedApiClient();

            var sub = ctx.CreateNewsSubscription(path);

            var ctxStreamingConnection = (TestStreamingConnection) ctx.StreamingConnection;
            ctxStreamingConnection.FireUpdate("");
            

        }

        private RpcClient CreateAuthenticatedApiClient()
        {
            var requestFactory = new TestRequestFactory();

            var throttleScopes = new Dictionary<string, IThrottedRequestQueue>
		                {
		                    {"data", new ThrottedRequestQueue(TimeSpan.FromSeconds(5), 30, 10)},
		                    {"trading", new ThrottedRequestQueue(TimeSpan.FromSeconds(3), 1, 10)}
		                };

            requestFactory.CreateTestRequest(LoggedIn);

            _testStreamingConnectionFactory = new TestStreamingConnectionFactory();

            var ctx = new RpcClient(new Uri(TestConfig.RpcUrl), new Uri(TestConfig.StreamingUrl), new RequestCache(), requestFactory, _testStreamingConnectionFactory, throttleScopes, 3);

            ctx.LogIn(TestConfig.ApiUsername, TestConfig.ApiPassword);
            return ctx;
        }
    }

    
}
