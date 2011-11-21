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

            // TODO: publish somewhere that search term is a 'StartsWith' not a 'Contains'
            var response = rpcClient.SpreadMarkets.ListSpreadMarkets("GBP/USD", null, accounts.ClientAccountId, 100);
            Assert.Greater(response.Markets.Length,0);
            
        }
    }
}