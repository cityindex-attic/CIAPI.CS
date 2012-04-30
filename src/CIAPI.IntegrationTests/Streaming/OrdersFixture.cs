using System;
using System.Collections.Generic;
using System.Threading;
using CIAPI.DTO;
using CIAPI.Streaming;
using Salient.ReflectiveLoggingAdapter;
using NUnit.Framework;


namespace CIAPI.IntegrationTests.Streaming
{
    [TestFixture]
    public class OrdersFixture : RpcFixtureBase
    {
        private ILog _logger = LogManager.GetLogger(typeof(PriceFixture));

        /// <summary>
        /// Test to replicate #131 - https://github.com/cityindex/CIAPI.CS/issues/131
        /// </summary>
        [Test]
        public void CanRecieveOrderNotification()
        {
            var MARKET_ID = 154297; //GBP/USD
            var gate = new ManualResetEvent(false);
            var rpcClient = BuildRpcClient();
            var streamingClient = rpcClient.CreateStreamingClient();
            var priceListener = streamingClient.BuildPricesListener(MARKET_ID);
            var ordersListener = streamingClient.BuildOrdersListener();

            try
            {
                OrderDTO newOrder = null;

                var marketInfo = rpcClient.Market.GetMarketInformation(MARKET_ID.ToString());
                var account = rpcClient.AccountInformation.GetClientAndTradingAccount();

                ordersListener.MessageReceived += (s, e) =>
                    {
                        newOrder = e.Data;
                        Console.WriteLine(
                            string.Format(
                                "New order has been recieved on Orders stream\r\n {0}",
                                e.Data.ToStringWithValues()));
                        gate.Set();
                    };

                priceListener.MessageReceived += (s, e) =>
                    {
                        var order = new NewTradeOrderRequestDTO
                                        {
                                            MarketId = e.Data.MarketId,
                                            BidPrice = e.Data.Bid,
                                            OfferPrice = e.Data.Offer,
                                            AuditId = e.Data.AuditId,
                                            Quantity = marketInfo.MarketInformation.WebMinSize.GetValueOrDefault() + 1,
                                            TradingAccountId = account.TradingAccounts[0].TradingAccountId,
                                            Direction = "buy"
                                        };

                        var response = rpcClient.TradesAndOrders.Trade(order);
                        rpcClient.MagicNumberResolver.ResolveMagicNumbers(response);
                        Assert.AreEqual("Accepted", response.Status_Resolved, string.Format("Error placing order: \r\n{0}", response.ToStringWithValues()));
                    };


                gate.WaitOne(TimeSpan.FromSeconds(15));

                Assert.IsNotNull(newOrder);
            }
            finally
            {
                streamingClient.TearDownListener(priceListener);
                streamingClient.TearDownListener(ordersListener);
                streamingClient.Dispose();
            }
        }
    }
}
