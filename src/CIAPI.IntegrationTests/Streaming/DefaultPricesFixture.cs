using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CIAPI.DTO;
using CIAPI.Streaming;
using CIAPI.StreamingClient;
using NUnit.Framework;

namespace CIAPI.IntegrationTests.Streaming
{
    [TestFixture]
    public class DefaultPricesFixture : RpcFixtureBase
    {
        private const int IFX_POLAND_ACCOUNT_OPERATOR_ID = 2347;

        [Test]
        public void CanConsumeDefaultPricesStream()
        {
            var rpcClient = this.BuildRpcClient();
            var streamingClient = rpcClient.CreateStreamingClient();

            var priceListener = streamingClient.BuildDefaultPricesListener(IFX_POLAND_ACCOUNT_OPERATOR_ID);
            var recorder = StreamingRecorder.Create(priceListener);
            recorder.Start();
            var tableOfPrices = new Dictionary<int, PriceDTO>();

            //A collection of different prices will stream in.
            //You need to collect a few before you get an idea of which prices you will get
            priceListener.MessageReceived += (s, e) =>
            {
                var newPrice = e.Data;
                Console.WriteLine(e.Data.Price);
                if (tableOfPrices.ContainsKey(newPrice.MarketId))
                {
                    tableOfPrices[newPrice.MarketId] = newPrice;
                }
                else
                {
                    tableOfPrices.Add(newPrice.MarketId,newPrice);
                }
            };

            Thread.Sleep(15000); //Wait for some prices to come in
            recorder.Stop();
            recorder.Dispose();
            List<MessageEventArgs<PriceDTO>> messages = recorder.GetMessages();
            string serializedMessages = rpcClient.Serializer.SerializeObject(messages);
            streamingClient.TearDownListener(priceListener);
            streamingClient.Dispose();

            tableOfPrices.Values.ToList().ForEach(p => Console.WriteLine(" ############ {0} - {1}", p.MarketId, p.Bid));

            Assert.Greater(tableOfPrices.Values.Count, 1, "We're expecting at least one price");
        }
    }
}
