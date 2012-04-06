using System;
using System.Threading;
using CIAPI.DTO;
using CIAPI.IntegrationTests.Streaming;
using CIAPI.Rpc;
using CIAPI.Streaming;
using NUnit.Framework;
using StreamingClient;


namespace CIAPI.IntegrationTests.Rpc
{
    [TestFixture]
    public class TradesAndOrdersFixture : RpcFixtureBase
    {
        private CIAPI.Streaming.IStreamingClient _streamingClient;
        private int _CFDmarketId;
        private Client _rpcClient;
        private AccountInformationResponseDTO _accounts;

        [TestFixtureSetUp]
        public void SetupFixture()
        {
            _rpcClient = BuildRpcClient();
            _streamingClient = _rpcClient.CreateStreamingClient();
            _CFDmarketId = GetAvailableCFDMarkets(_rpcClient)[0].MarketId;
            _accounts = _rpcClient.AccountInformation.GetClientAndTradingAccount();
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            _streamingClient.Dispose();
        }

        [Test]
        public void CanTrade()
        {
            var marketInfo = GetMarketInfo(_CFDmarketId);

            //BUY
            var trade = new NewTradeOrderRequestDTO()
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
                TradingAccountId = _accounts.CFDAccount.TradingAccountId
            };
            var response = _rpcClient.TradesAndOrders.Trade(trade);
            _rpcClient.MagicNumberResolver.ResolveMagicNumbers(response);

            Assert.AreEqual(response.Status_Resolved, "Accepted");

            //And then SELL again
            marketInfo = GetMarketInfo(_CFDmarketId);

            var orderId = response.OrderId;
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
                TradingAccountId = _accounts.CFDAccount.TradingAccountId
            };

            response = _rpcClient.TradesAndOrders.Trade(trade);
            _rpcClient.MagicNumberResolver.ResolveMagicNumbers(response);

            Assert.AreEqual(response.Status_Resolved, "Accepted");

        }

        [Test]
        public void CanListActiveStopLimitOrders()
        {
            int tradingAccountId = _accounts.CFDAccount.TradingAccountId;
            var response = _rpcClient.TradesAndOrders.ListActiveStopLimitOrders(tradingAccountId);

            tradingAccountId = _accounts.SpreadBettingAccount.TradingAccountId;
            response = _rpcClient.TradesAndOrders.ListActiveStopLimitOrders(tradingAccountId);
        }

        [Test]
        public void CanListOpenPositions()
        {
            int tradingAccountId = _accounts.SpreadBettingAccount.TradingAccountId;
            var response = _rpcClient.TradesAndOrders.ListOpenPositions(tradingAccountId);

            tradingAccountId = _accounts.CFDAccount.TradingAccountId;
            response = _rpcClient.TradesAndOrders.ListOpenPositions(tradingAccountId);
        }

        [Test]
        public void CanListStopLimitOrderHistory()
        {
            int maxResults = 100;
            int tradingAccountId = _accounts.CFDAccount.TradingAccountId;
            var response = _rpcClient.TradesAndOrders.ListStopLimitOrderHistory(tradingAccountId, maxResults);

            tradingAccountId = _accounts.SpreadBettingAccount.TradingAccountId;
            response = _rpcClient.TradesAndOrders.ListStopLimitOrderHistory(tradingAccountId, maxResults);
        }

        [Test]
        public void CanListTradeHistory()
        {
            // tests issue with 77th trade history item issue - this is definitely a server error
            // http://faq.labs.cityindex.com/questions/internal-server-error-when-trying-to-get-trade-history
            // while it is suggested that each side of the trade may be in different currencies
            // several accounts reportedly reprod the error, so short of a dedicated account with which
            // to make the same 77+ trades all we can do is check for 77 max

            int maxResults = 77;
            int tradingAccountId = _accounts.CFDAccount.TradingAccountId;
            var response = _rpcClient.TradesAndOrders.ListTradeHistory(tradingAccountId, maxResults);
            Assert.IsTrue(response.TradeHistory.Length > 0);

            tradingAccountId = _accounts.SpreadBettingAccount.TradingAccountId;
            response = _rpcClient.TradesAndOrders.ListTradeHistory(tradingAccountId, maxResults);
            Assert.IsTrue(response.TradeHistory.Length > 0);

        }

        [Test]
        public void CanOrder()
        {
            var order = new NewStopLimitOrderRequestDTO()
                                                    {

                                                    };
            var response = _rpcClient.TradesAndOrders.Order(order);
        }

        [Test]
        public void CanCancelOrder()
        {
            var order = new CancelOrderRequestDTO();
            var response = _rpcClient.TradesAndOrders.CancelOrder(order);
        }

        [Test]
        public void CanGetOrder()
        {
            string order = "";
            var response = _rpcClient.TradesAndOrders.GetOrder(order);
        }

        [Test]
        public void CanGetOpenPosition()
        {
            string orderId = "";
            var response = _rpcClient.TradesAndOrders.GetOpenPosition(orderId);
        }
        [Test]
        public void CanGetActiveStopLimitOrder()
        {
            string orderId = "";
            var response = _rpcClient.TradesAndOrders.GetActiveStopLimitOrder(orderId);
        }

        private PriceDTO GetMarketInfo(int marketId)
        {
            IStreamingListener<PriceDTO> listener = null;
            PriceDTO marketInfo = null;

            try
            {
                listener = _streamingClient.BuildPricesListener(marketId);
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
                _streamingClient.TearDownListener(listener);
            }

            return marketInfo;
        }


        [Test]
        public void CanUpdateOrder()
        {
            var update = new UpdateStopLimitOrderRequestDTO();
            var response = _rpcClient.TradesAndOrders.UpdateOrder(update);
        }

        [Test]
        public void CanUpdateTrade()
        {
            var rpcClient = BuildRpcClient();

            AccountInformationResponseDTO accounts = rpcClient.AccountInformation.GetClientAndTradingAccount();

            PriceDTO marketInfo = GetMarketInfo(_CFDmarketId);

            var trade = new NewTradeOrderRequestDTO()
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
                TradingAccountId = accounts.CFDAccount.TradingAccountId
            };
            var order = rpcClient.TradesAndOrders.Trade(trade);
            rpcClient.MagicNumberResolver.ResolveMagicNumbers(order);

            Assert.AreEqual(order.Status_Resolved, "Accepted");

            var update = new UpdateTradeOrderRequestDTO
                             {
                    OrderId = order.OrderId,
                    MarketId = trade.MarketId,
                    Currency = trade.Currency,
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
                                            TradingAccountId = accounts.CFDAccount.TradingAccountId    
                                        }
                                }
                        },
                    AuditId = trade.AuditId,
                    AutoRollover = trade.AutoRollover,
                    BidPrice = trade.BidPrice,
                    Close = trade.Close,
                    Direction = trade.Direction,
                    OfferPrice = trade.OfferPrice,
                    Quantity = trade.Quantity,
                    QuoteId = trade.QuoteId,
                    TradingAccountId = trade.TradingAccountId
                };

            var response = rpcClient.TradesAndOrders.UpdateTrade(update);
        }
    }
}