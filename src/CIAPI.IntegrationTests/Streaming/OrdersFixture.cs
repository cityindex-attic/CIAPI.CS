using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
                        Console.WriteLine(string.Format("Trade/order placed: \r\n{0}", response.ToStringWithValues()));
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

        /// <summary>
        /// Test to replicate issue seen in Windows phone app, where TradeMargin messages only arrive
        /// for first two open positions
        /// </summary>
        [Test, Explicit("Must run this test using an account that has 3+ open positions")]
        public void RecievesTradeMarginMessagesForMoreThanTwoAllOpenPositions()
        {
            var gate = new ManualResetEvent(false);
            var rpcClient = BuildRpcClient();
            var accounts = rpcClient.AccountInformation.GetClientAndTradingAccount();
            var openPositions = rpcClient.TradesAndOrders.ListOpenPositions(accounts.TradingAccounts[0].TradingAccountId);

            Assert.That(openPositions.OpenPositions.Length, Is.GreaterThanOrEqualTo(3), "This test must be run using an account that has 3 or more open positions");
            
            Console.WriteLine(string.Format(
                        "There are {0} open positions on markets: {1}",
                        openPositions.OpenPositions.Length,
                        openPositions.OpenPositions.Select(p => p.MarketId).ToList().ToStringWithValues()));

            var streamingClient = rpcClient.CreateStreamingClient();
            var tradeMarginListener = streamingClient.BuildTradeMarginListener();

            var marketsThatTradeMarginHasBeenRecievedFor = new ArrayList();

            tradeMarginListener.MessageReceived += (s, e) =>
            {
                Console.WriteLine(
                        string.Format(
                            "TradeMarginDTO recieved for market {0}",
                            e.Data.MarketId));

                if (marketsThatTradeMarginHasBeenRecievedFor.Contains(e.Data.MarketId)) return;

                marketsThatTradeMarginHasBeenRecievedFor.Add(e.Data.MarketId);
                if (openPositions.OpenPositions.Length == marketsThatTradeMarginHasBeenRecievedFor.Count)
                {
                    gate.Set();
                }
            };

            try
            {
                if (!gate.WaitOne(TimeSpan.FromMinutes(1)))
                {
                    Assert.Fail("TradeMarginDTO message not recieved for all open positions. OpenPositions on markets: {0} but only recieved TradeMarginDTOs for markets {1}",
                        openPositions.OpenPositions.Select(p => p.MarketId).ToStringWithValues(),
                        marketsThatTradeMarginHasBeenRecievedFor.ToStringWithValues());
                }
            } 
            finally
            {
                Console.WriteLine(string.Format(
                        "OpenPositions on markets: {0}, Recieved TradeMarginDTOs for markets {1}",
                        openPositions.OpenPositions.Select(p => p.MarketId).ToStringWithValues(),
                        marketsThatTradeMarginHasBeenRecievedFor.ToStringWithValues()));
                streamingClient.TearDownListener(tradeMarginListener);
                streamingClient.Dispose();
            }
        }
    }
}
