using System;
using System.Threading;
using CIAPI.DTO;
using CIAPI.Streaming;
using Common.Logging;
using NUnit.Framework;

namespace CIAPI.IntegrationTests.Streaming
{
    [TestFixture]
    public class DefaultPricesFixture : RpcFixtureBase
    {
        private ILog _logger = LogManager.GetCurrentClassLogger();
        [Test]
        public void CanConsumeDefaultPricesStream()
        {
            var gate = new ManualResetEvent(false);

            var streamingClient = StreamingClientFactory.CreateStreamingClient(Settings.StreamingUri, "", "");

            var priceListener = streamingClient.BuildDefaultPricesListener(2347);

            PriceDTO actual = null;

            //Trap the Price given by the update event for checking
            priceListener.MessageReceived += (s, e) =>
            {
                actual = e.Data;
                gate.Set();
            };


            bool gotPriceInTime = true;
            if (!gate.WaitOne(TimeSpan.FromSeconds(15)))
            {
                gotPriceInTime = false;
                // don't want to throw while client is listening, hangs test
            }

            streamingClient.TearDownListener(priceListener);
            streamingClient.Dispose();


            Assert.IsTrue(gotPriceInTime, "A price update wasn't received in time");
            Assert.IsNotNull(actual);
            Assert.Greater(actual.TickDate, DateTime.UtcNow.AddSeconds(-10), "We're expecting a recent price");

        }
    }
}
