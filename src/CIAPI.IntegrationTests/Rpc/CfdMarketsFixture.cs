using System.Linq;
using CIAPI.DTO;
using CIAPI.Rpc;
using NUnit.Framework;

namespace CIAPI.IntegrationTests.Rpc
{
    [TestFixture]
    public class CfdMarketsFixture
    {
        [Test]
        public void Test()
        {
            var rpcClient = new Client(Settings.RpcUri);
            rpcClient.LogIn(Settings.RpcUserName, Settings.RpcPassword);

            AccountInformationResponseDTO accounts = rpcClient.AccountInformation.GetClientAndTradingAccount();

            var response = rpcClient.CFDMarkets.ListCfdMarkets("USD", null, accounts.CFDAccount.TradingAccountId, 100);
            
            rpcClient.LogOut();
            rpcClient.Dispose();
        }
    }
}