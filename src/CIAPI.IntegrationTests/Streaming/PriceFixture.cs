using System;
using System.Threading;
using CIAPI.DTO;
using CIAPI.Streaming;
using NUnit.Framework;
using IStreamingClient = CIAPI.Streaming.IStreamingClient;

namespace CIAPI.IntegrationTests.Streaming
{
    [TestFixture]
    public class PriceFixture
    {
        public static IStreamingClient BuildStreamingClient(
            string userName = "0x234",
            string password = "password")
        {
            const string apiUrl = "https://ciapipreprod.cityindextest9.co.uk/TradingApi/";

            var authenticatedClient = new CIAPI.Rpc.Client(new Uri(apiUrl));
            authenticatedClient.LogIn(userName, password);

            var streamingUri = new Uri("https://pushpreprod.cityindextest9.co.uk/CITYINDEXSTREAMING");

            return StreamingClientFactory.CreateStreamingClient(streamingUri, userName, authenticatedClient.SessionId);
        }

        [Test,Ignore("this test is best run interactively when market is responsive. after hours there is no pricing so test will fail")]
        public void CanConsumePriceStream()
        {
            var gate = new ManualResetEvent(false);

            var streamingClient = BuildStreamingClient();

            streamingClient.Connect();

            

            var priceListener = streamingClient.BuildPriceListener("PRICES.PRICE.71442");
            priceListener.Start();

            PriceDTO actual = null;

            //Trap the Price given by the update event for checking

            priceListener.MessageReceived += (s, e) =>
            {
                actual = e.Data;
                gate.Set();
            };


            if (!gate.WaitOne(TimeSpan.FromSeconds(5)))
            {
                Assert.Fail("A price update wasn't received in time");
            }

            priceListener.Stop();
            streamingClient.Disconnect();

            Assert.IsNotNull(actual);
            Assert.Greater(actual.TickDate, DateTime.UtcNow.AddSeconds(-10), "We're expecting a recent price");
        }
    }
}
