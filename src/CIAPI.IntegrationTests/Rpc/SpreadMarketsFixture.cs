using CIAPI.DTO;
using CIAPI.IntegrationTests.Streaming;
using CIAPI.Rpc;
using NUnit.Framework;

namespace CIAPI.IntegrationTests.Rpc
{
    [TestFixture]
    public class SpreadMarketsFixture : RpcFixtureBase
    {
        [Test]
        public void CanListSpreadMarkets()
        {
            var rpcClient = BuildRpcClient();

            AccountInformationResponseDTO accounts = rpcClient.AccountInformation.GetClientAndTradingAccount();

            var response = rpcClient.SpreadMarkets.ListSpreadMarkets("USD", null, accounts.ClientAccountId, 100);
            Assert.Greater(response.Markets.Length,0);
            
        }
    }
}