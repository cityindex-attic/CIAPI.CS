using System;
using System.Threading;
using CIAPI.DTO;
using CIAPI.RecordedTests.Infrastructure;
using CIAPI.Rpc;
using CIAPI.StreamingClient;
using NUnit.Framework;

namespace CIAPI.RecordedTests
{
    [TestFixture, MocumentModeOverride(MocumentMode.Record), Ignore("need setup for streaming")]
    public class SimulatedTradesAndOrdersFixture : CIAPIRecordingFixtureBase
    {
        //private CIAPI.Streaming.IStreamingClient _streamingClient;
        //private int _CFDmarketId;
        //private Client _rpcClient;
        //private AccountInformationResponseDTO _accounts;

        //[TestFixtureSetUp]
        //public void SetupFixture()
        //{
        //    _rpcClient = BuildRpcClient();
        //    _streamingClient = _rpcClient.CreateStreamingClient();
        //    _CFDmarketId = MarketFixture.GetAvailableCFDMarkets(_rpcClient)[0].MarketId;
        //    _accounts = _rpcClient.AccountInformation.GetClientAndTradingAccount();
        //}

        //[TestFixtureTearDown]
        //public void TearDown()
        //{
        //    _streamingClient.Dispose();
        //}

        //[Test, Explicit("This test only runs against PPE")]
        //public void CanSimulateTrade()
        //{
        //    var marketInfo = GetMarketInfo(_CFDmarketId);

        //    //BUY
        //    var trade = new NewTradeOrderRequestDTO()
        //    {
        //        AuditId = marketInfo.AuditId,
        //        AutoRollover = false,
        //        BidPrice = marketInfo.Bid,
        //        Close = null,
        //        Currency = null,
        //        Direction = "buy",
        //        IfDone = null,
        //        MarketId = marketInfo.MarketId,
        //        OfferPrice = marketInfo.Offer,
        //        Quantity = 1,
        //        QuoteId = null,
        //        TradingAccountId = _accounts.CFDAccount.TradingAccountId
        //    };
        //    var response = _rpcClient.TradesAndOrders.SimulateTrade(trade);
        //    _rpcClient.MagicNumberResolver.ResolveMagicNumbers(response);

        //    Assert.AreEqual("Accepted", response.Status_Resolved);

        //}

        //private PriceDTO GetMarketInfo(int marketId)
        //{
        //    IStreamingListener<PriceDTO> listener = null;
        //    PriceDTO marketInfo = null;

        //    try
        //    {
        //        listener = _streamingClient.BuildPricesListener(marketId);
        //        var gate = new AutoResetEvent(false);

        //        listener.MessageReceived += (o, s) =>
        //        {
        //            marketInfo = s.Data;
        //            gate.Set();
        //        };


        //        if (!gate.WaitOne(10000))
        //        {
        //            throw new Exception("timed out waiting for market data");
        //        }
        //    }
        //    finally
        //    {
        //        _streamingClient.TearDownListener(listener);
        //    }

        //    return marketInfo;
        //}
    }
}