using System;
using System.Threading;
using CIAPI.DTO;
using CIAPI.Rpc;
using CIAPI.Streaming;
using NUnit.Framework;
using StreamingClient;
using IStreamingClient = CIAPI.Streaming.IStreamingClient;

namespace CIAPI.IntegrationTests.Streaming
{
    [TestFixture]
    public class ClientAccountFixture : RpcFixtureBase
    {

        private Client _authenticatedClient;
        private IStreamingClient _streamingClient;

        [TestFixtureSetUp]
        public void SetupFixture()
        {
            _authenticatedClient = new Client(Settings.RpcUri, AppKey);
            _authenticatedClient.LogIn(Settings.RpcUserName, Settings.RpcPassword);
            _streamingClient = StreamingClientFactory.CreateStreamingClient(Settings.StreamingUri, Settings.RpcUserName, _authenticatedClient.Session);
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            _streamingClient.Dispose();
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
        public void CanConsumeTradeMarginStream()
        {


            var tradeMarginListener = _streamingClient.BuildTradeMarginListener();

            // set up a handler to respond to stream events

            var gate = new AutoResetEvent(false);
            TradeMarginDTO actual = null;
            tradeMarginListener.MessageReceived += (s, e) =>
            {

                Console.WriteLine(
                    "-----------------------------------------------");
                
                actual = e.Data;
                Console.WriteLine(actual.ToStringWithValues());
                Console.WriteLine(
                    "-----------------------------------------------");
                gate.Set();
            };


            // place a trade to give the margin listener something to listen to

            AccountInformationResponseDTO accounts = _authenticatedClient.AccountInformation.GetClientAndTradingAccount();

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
             var response = _authenticatedClient.TradesAndOrders.Trade(trade);
          





            gate.WaitOne(25000);

            _streamingClient.TearDownListener(tradeMarginListener);

            
            // close the tradeMarginListener

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

            _authenticatedClient.TradesAndOrders.Trade(trade);
           
            
            Assert.IsNotNull(actual,"did not get a streaming event");
       


        }
    }
}