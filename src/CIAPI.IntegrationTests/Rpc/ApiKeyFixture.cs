using CIAPI.DTO;
using CIAPI.IntegrationTests.Streaming;
using NUnit.Framework;

namespace CIAPI.IntegrationTests.Rpc
{
    [TestFixture]
    public class ApiKeyFixture : RpcFixtureBase
    {
        [Test]
        public void ApiKeyIsAppended()
        {
            // look at the log to verify - need to expose interals and provide a means to examine the cache to verify programmatically

            var rpcClient = BuildRpcClient("2mn8skr4uhgeafktwqewvw64");

            AccountInformationResponseDTO accounts = rpcClient.AccountInformation.GetClientAndTradingAccount();
            
            rpcClient.TradesAndOrders.ListOpenPositions(accounts.TradingAccounts[0].TradingAccountId);
            rpcClient.LogOut();
            rpcClient.Dispose();
        }
    }
}