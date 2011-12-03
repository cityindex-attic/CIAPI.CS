using CIAPI.IntegrationTests.Streaming;
using CIAPI.Rpc;
using NUnit.Framework;

namespace CIAPI.IntegrationTests.Rpc
{
    [TestFixture]
    public class PriceHistoryFixture : RpcFixtureBase
    {
        
        [Test]
        public void CanGetPriceBars()
        {
            var rpcClient = BuildRpcClient();

            var response = rpcClient.PriceHistory.GetPriceBars("71442", "MINUTE", 10, "10");
            Assert.AreEqual(10, response.PriceBars.Length);

        }
        [Test]
        public void CanGetPriceTicks()
        {
            var rpcClient = BuildRpcClient();

            var response = rpcClient.PriceHistory.GetPriceTicks("71442","10");
            Assert.AreEqual(10, response.PriceTicks.Length);

            
        }
    }
}