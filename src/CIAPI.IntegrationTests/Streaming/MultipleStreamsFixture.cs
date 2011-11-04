using System;
using System.Threading;
using NUnit.Framework;

namespace CIAPI.IntegrationTests.Streaming
{
    [TestFixture]
    public class MultipleStreamsFixture : RpcFixtureBase
    {
        [Test]
        [Ignore("WIP")]
        public void CanSubscribeToTwoStreamsAtSameTime()
        {
            var gate = new ManualResetEvent(false);
            var priceRecieved = false;
            var marginRecieved = false;
            var streamingClient = BuildStreamingClient();

            var topics = new[] { 99498, 99500 };
            var pricesListener = streamingClient.BuildPricesListener(topics);
            pricesListener.MessageReceived +=
                (s, e) =>
                {
                    priceRecieved = true;

                    Console.WriteLine("########### {0} -> {1}", e.Data.MarketId, e.Data.Price);
                    if (marginRecieved && priceRecieved)
                        gate.Set();
                };

            var marginListener = streamingClient.BuildClientAccountMarginListener();
            marginListener.MessageReceived +=
                (s, e) =>
                    {
                        marginRecieved = true;
                        Console.WriteLine("########### \tequity: {0}", e.Data.NetEquity);
                        if (marginRecieved && priceRecieved)
                            gate.Set();
                    };

          

            
            gate.WaitOne(TimeSpan.FromSeconds(30));

            streamingClient.TearDownListener(pricesListener);
            streamingClient.TearDownListener(marginListener);
            streamingClient.Dispose();

            Assert.That(priceRecieved, Is.True, "No price was recieved");
            Assert.That(marginRecieved, Is.True, "No margin was recieved");
        }
    }
}
