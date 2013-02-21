using CIAPI.DTO;
using CIAPI.RecordedTests.Infrastructure;
using NUnit.Framework;

namespace CIAPI.RecordedTests
{
    [TestFixture, MocumentModeOverride(MocumentMode.Play)]
    public class FaqIssueResolutionFixture : CIAPIRecordingFixtureBase
    {
        /// <summary>
        /// http://faq.labs.cityindex.com/questions/why-getpricebars-sometimes-does-not-return-back-a-getpricebarresponsedto-object-or-throw-an-error
        /// </summary>
        [Test]
        public void Json_Ho_01()
        {

            var rpcClient = BuildRpcClient("Json_Ho_01");



            GetPriceBarResponseDTO bars = rpcClient.PriceHistory.GetPriceBars("71442", "MINUTE", 1, "15");
            Assert.IsNotNull(bars);

            rpcClient.LogOut();
        }



        /// <summary>
        /// http://github.com/cityindex/CIAPI.CS/issues/42
        /// </summary>
        [Test, Ignore("test is broken - need proper setup")]
        public void Issue42()
        {

            var rpcClient = BuildRpcClient("Issue42");

            var param = new NewStopLimitOrderRequestDTO
            {
                OrderId = 0,
                MarketId = 99498,
                Currency = null,
                AutoRollover = false,
                Direction = "buy",
                Quantity = 10m,
                BidPrice = 12094m,
                OfferPrice = 12098m,
                AuditId = "20110629-G2PREPROD3-0102794",
                TradingAccountId = 400002249,
                IfDone = null,
                OcoOrder = null,
                Applicability = null,
                ExpiryDateTimeUTC = null,
                Guaranteed = false,
                TriggerPrice = 0m
            };
            var response = rpcClient.TradesAndOrders.Order(param);


            rpcClient.LogOut();
        }


        /// <summary>
        /// http://github.com/cityindex/CIAPI.CS/issues/35
        /// </summary>
        [Test, Ignore("test is broken - need proper setup")]
        public void Issue35()
        {

            var rpcClient = BuildRpcClient("Issue35");

            var accountInfo = rpcClient.AccountInformation.GetClientAndTradingAccount();
            var resp = rpcClient.TradesAndOrders.ListTradeHistory(accountInfo.ClientAccountId, 20);


            rpcClient.LogOut();
        }
    }
}