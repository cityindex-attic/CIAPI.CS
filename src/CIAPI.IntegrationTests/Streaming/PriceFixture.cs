using System;
using System.Threading;
using CIAPI.DTO;
using CIAPI.Streaming;
using NUnit.Framework;

namespace CIAPI.IntegrationTests.Streaming
{
    [TestFixture]
    public class PriceFixture
    {
        [Test]
        public void CanConsumePriceStream()
        {
            var gate = new ManualResetEvent(false);

            var streamingClient = StreamingClientBuilder.BuildStreamingClient();

            streamingClient.Connect();

            const string priceTopic = "MOCKPRICE.1000";

            var priceListener = streamingClient.BuildPriceListener(priceTopic);
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
