using System;
using System.Threading;
using CIAPI.DTO;
using CIAPI.Streaming;
using CIAPI.Streaming.Lightstreamer;
using NUnit.Framework;

namespace CIAPI.IntegrationTests.Streaming
{
    [TestFixture]
    public class PriceFixture
    {
        public static LightstreamerClient BuildStreamingClient(
            string userName = "0x234",
            string password = "password")
        {
            const string apiUrl = "https://ciapipreprod.cityindextest9.co.uk/TradingApi/";

            var authenticatedClient = new CIAPI.Rpc.Client(new Uri(apiUrl));
            authenticatedClient.LogIn(userName, password);

            var streamingUri = new Uri("https://pushpreprod.cityindextest9.co.uk/CITYINDEXSTREAMING");

            return new LightstreamerClient(streamingUri, userName, authenticatedClient.SessionId);
        }

        [Test]
        public void CanConsumePriceStream()
        {
            var gate = new ManualResetEvent(false);

            var streamingClient = BuildStreamingClient();

            streamingClient.Connect();

            var priceListener = streamingClient.BuildListener<PriceDTO>("PRICES.MOCKPRICE.1000");
            priceListener.Start();

            PriceDTO actual = null;

            //Trap the Price given by the update event for checking

            priceListener.MessageRecieved += (s, e) =>
            {
                actual = e.Data;
                gate.Set();
            };


            if (!gate.WaitOne(TimeSpan.FromSeconds(5)))
            {
                Assert.Fail("A price update wasn't recieved in time");
            }

            priceListener.Stop();
            streamingClient.Disconnect();

            Assert.IsNotNull(actual);
            Assert.Greater(actual.TickDate, DateTime.UtcNow.AddSeconds(-10), "We're expecting a recent price");
        }
    }
}
