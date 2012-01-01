using System;
using System.Threading;
using CIAPI.DTO;
using CIAPI.IntegrationTests.Streaming;
using CIAPI.Streaming;
using NUnit.Framework;
using StreamingClient;
using IStreamingClient = CIAPI.Streaming.IStreamingClient;

namespace CIAPI.IntegrationTests.Rpc
{
    [TestFixture]
    public class TradesAndOrdersFixture : RpcFixtureBase
    {
        private IStreamingClient streamingClient;

        [TestFixtureSetUp]
        public void SetupFixture()
        {
            var authenticatedClient = new CIAPI.Rpc.Client(Settings.RpcUri);
            authenticatedClient.LogIn(Settings.RpcUserName, Settings.RpcPassword);
            streamingClient = StreamingClientFactory.CreateStreamingClient(Settings.StreamingUri, Settings.RpcUserName, authenticatedClient.Session);
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            streamingClient.Dispose();
        }

        [Test]
        public void CanTrade()
        {

            //MarketId: 80905
            //Name: "GBP/USD (per 0.0001) Rolling Spread"


            //MarketId: 400516274
            //Name: "GBP/USD (per 0.0001) Dec 11 Spread"

            var rpcClient = BuildRpcClient();



            AccountInformationResponseDTO accounts = rpcClient.AccountInformation.GetClientAndTradingAccount();

            PriceDTO marketInfo = GetMarketInfo(80905);

            NewTradeOrderRequestDTO trade = new NewTradeOrderRequestDTO()
            {
                AuditId = marketInfo.AuditId,
                AutoRollover = false,
                BidPrice = marketInfo.Bid,
                Close = null,
                Currency = null,
                Direction = "buy",
                IfDone = null,
                MarketId = marketInfo.MarketId,
                OfferPrice = marketInfo.Offer,
                Quantity = 1,
                QuoteId = null,
                TradingAccountId = accounts.SpreadBettingAccount.TradingAccountId
            };
            var response = rpcClient.TradesAndOrders.Trade(trade);
            rpcClient.MagicNumberResolver.ResolveMagicNumbers(response);

            Assert.AreEqual(response.Status_Resolved, "Accepted");

            marketInfo = GetMarketInfo(80905);

            int orderId = response.OrderId;
            trade = new NewTradeOrderRequestDTO()
            {
                AuditId = marketInfo.AuditId,
                AutoRollover = false,
                BidPrice = marketInfo.Bid,
                Close = new int[] { orderId },
                Currency = null,
                Direction = "sell",
                IfDone = null,
                MarketId = marketInfo.MarketId,
                OfferPrice = marketInfo.Offer,
                Quantity = 1,
                QuoteId = null,
                TradingAccountId = accounts.SpreadBettingAccount.TradingAccountId
            };

            response = rpcClient.TradesAndOrders.Trade(trade);
            rpcClient.MagicNumberResolver.ResolveMagicNumbers(response);

            Assert.AreEqual(response.Status_Resolved, "Accepted");

        }

        [Test]
        public void CanListActiveStopLimitOrders()
        {
            var rpcClient = BuildRpcClient();

            AccountInformationResponseDTO accounts = rpcClient.AccountInformation.GetClientAndTradingAccount();
            int tradingAccountId = accounts.CFDAccount.TradingAccountId;
            var response = rpcClient.TradesAndOrders.ListActiveStopLimitOrders(tradingAccountId);

            tradingAccountId = accounts.SpreadBettingAccount.TradingAccountId;
            response = rpcClient.TradesAndOrders.ListActiveStopLimitOrders(tradingAccountId);
        }

        [Test]
        public void CanListOpenPositions()
        {
            var rpcClient = BuildRpcClient();

            AccountInformationResponseDTO accounts = rpcClient.AccountInformation.GetClientAndTradingAccount();
            int tradingAccountId = accounts.SpreadBettingAccount.TradingAccountId;
            var response = rpcClient.TradesAndOrders.ListOpenPositions(tradingAccountId);

            tradingAccountId = accounts.CFDAccount.TradingAccountId;
            response = rpcClient.TradesAndOrders.ListOpenPositions(tradingAccountId);
        }

        [Test]
        public void CanListStopLimitOrderHistory()
        {
            var rpcClient = BuildRpcClient();

            AccountInformationResponseDTO accounts = rpcClient.AccountInformation.GetClientAndTradingAccount();
            int maxResults = 100;
            int tradingAccountId = accounts.CFDAccount.TradingAccountId;
            var response = rpcClient.TradesAndOrders.ListStopLimitOrderHistory(tradingAccountId, maxResults);

            tradingAccountId = accounts.SpreadBettingAccount.TradingAccountId;
            response = rpcClient.TradesAndOrders.ListStopLimitOrderHistory(tradingAccountId, maxResults);
        }

        [Test]
        public void CanListTradeHistory()
        {
            // tests issue with 77th trade history item issue - this is definitely a server error
            // http://faq.labs.cityindex.com/questions/internal-server-error-when-trying-to-get-trade-history
            // while it is suggested that each side of the trade may be in different currencies
            // several accounts reportedly reprod the error, so short of a dedicated account with which
            // to make the same 77+ trades all we can do is check for 77 max

            var rpcClient = BuildRpcClient();

            AccountInformationResponseDTO accounts = rpcClient.AccountInformation.GetClientAndTradingAccount();
            int maxResults = 77;
            int tradingAccountId = accounts.CFDAccount.TradingAccountId;
            var response = rpcClient.TradesAndOrders.ListTradeHistory(tradingAccountId, maxResults);
            Assert.IsTrue(response.TradeHistory.Length > 0);

            tradingAccountId = accounts.SpreadBettingAccount.TradingAccountId;
            response = rpcClient.TradesAndOrders.ListTradeHistory(tradingAccountId, maxResults);
            Assert.IsTrue(response.TradeHistory.Length > 0);

        }

        [Test]
        public void CanOrder()
        {
            var rpcClient = BuildRpcClient();

            AccountInformationResponseDTO accounts = rpcClient.AccountInformation.GetClientAndTradingAccount();
            NewStopLimitOrderRequestDTO order = new NewStopLimitOrderRequestDTO()
                                                    {

                                                    };
            var response = rpcClient.TradesAndOrders.Order(order);
        }

        [Test]
        public void CanCancelOrder()
        {
            var rpcClient = BuildRpcClient();

            AccountInformationResponseDTO accounts = rpcClient.AccountInformation.GetClientAndTradingAccount();
            CancelOrderRequestDTO order = new CancelOrderRequestDTO();
            var response = rpcClient.TradesAndOrders.CancelOrder(order);
        }

        [Test]
        public void CanGetOrder()
        {
            var rpcClient = BuildRpcClient();

            AccountInformationResponseDTO accounts = rpcClient.AccountInformation.GetClientAndTradingAccount();
            string order = "";
            var response = rpcClient.TradesAndOrders.GetOrder(order);
        }

        [Test]
        public void CanGetOpenPosition()
        {
            var rpcClient = BuildRpcClient();

            AccountInformationResponseDTO accounts = rpcClient.AccountInformation.GetClientAndTradingAccount();

            string orderId = "";
            var response = rpcClient.TradesAndOrders.GetOpenPosition(orderId);
        }
        [Test]
        public void CanGetActiveStopLimitOrder()
        {
            var rpcClient = BuildRpcClient();

            AccountInformationResponseDTO accounts = rpcClient.AccountInformation.GetClientAndTradingAccount();
            string orderId = "";
            var response = rpcClient.TradesAndOrders.GetActiveStopLimitOrder(orderId);
        }



        private PriceDTO GetMarketInfo(int marketId)
        {
            IStreamingListener<PriceDTO> listener = null;
            PriceDTO marketInfo = null;

            try
            {
                listener = streamingClient.BuildPricesListener(marketId);
                var gate = new AutoResetEvent(false);

                listener.MessageReceived += (o, s) =>
                                        {
                                            marketInfo = s.Data;
                                            gate.Set();
                                        };


                if (!gate.WaitOne(10000))
                {
                    throw new Exception("timed out waiting for market data");
                }
            }
            finally
            {
                streamingClient.TearDownListener(listener);
            }

            return marketInfo;
        }


        [Test]
        public void CanUpdateOrder()
        {
            var rpcClient = BuildRpcClient();

            AccountInformationResponseDTO accounts = rpcClient.AccountInformation.GetClientAndTradingAccount();
            UpdateStopLimitOrderRequestDTO update = new UpdateStopLimitOrderRequestDTO();
            var response = rpcClient.TradesAndOrders.UpdateOrder(update);
        }

        [Test]
        public void CanUpdateTrade()
        {
            var rpcClient = BuildRpcClient();



            AccountInformationResponseDTO accounts = rpcClient.AccountInformation.GetClientAndTradingAccount();

            PriceDTO marketInfo = GetMarketInfo(80905);

            NewTradeOrderRequestDTO trade = new NewTradeOrderRequestDTO()
            {
                AuditId = marketInfo.AuditId,
                AutoRollover = false,
                BidPrice = marketInfo.Bid,
                Close = null,
                Currency = null,
                Direction = "buy",
                IfDone = null,
                MarketId = marketInfo.MarketId,
                OfferPrice = marketInfo.Offer,
                Quantity = 1,
                QuoteId = null,
                TradingAccountId = accounts.SpreadBettingAccount.TradingAccountId
            };
            var order = rpcClient.TradesAndOrders.Trade(trade);
            rpcClient.MagicNumberResolver.ResolveMagicNumbers(order);

            Assert.AreEqual(order.Status_Resolved, "Accepted");
            UpdateTradeOrderRequestDTO update = new UpdateTradeOrderRequestDTO()
                {
                    OrderId = order.OrderId,
                    IfDone = new[] 
                        { 
                            new ApiIfDoneDTO 
                                { 
                                    Stop = new ApiStopLimitOrderDTO
                                        {
                                            TriggerPrice = marketInfo.Offer+10,
                                            Direction = "sell",
                                            IfDone = null,
                                            MarketId = marketInfo.MarketId,
                                            Quantity = 1,
                                            TradingAccountId = accounts.SpreadBettingAccount.TradingAccountId    
                                        }
                                }
                        }
                };
            var response = rpcClient.TradesAndOrders.UpdateTrade(update);
        }
    }
}