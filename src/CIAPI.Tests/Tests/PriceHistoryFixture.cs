using CIAPI.RecordedTests.Infrastructure;
using NUnit.Framework;

namespace CIAPI.RecordedTests
{
    [TestFixture, MocumentModeOverride(MocumentMode.Play)]
    public class PriceHistoryFixture : CIAPIRecordingFixtureBase
    {
        //MarketId: 400158776
        //Name: "USD/SEK (per 0.0001) CFD"

        [Test]
        public void CanGetPriceBars()
        {
            var rpcClient = BuildRpcClient();

            var response = rpcClient.PriceHistory.GetPriceBars("400158776", "MINUTE", 10, "10");
            Assert.AreEqual(10, response.PriceBars.Length);

        }
        [Test]
        public void CanGetPriceTicks()
        {
            var rpcClient = BuildRpcClient();

            var response = rpcClient.PriceHistory.GetPriceTicks("400158776", "10");
            Assert.AreEqual(10, response.PriceTicks.Length);


        }
    }
}