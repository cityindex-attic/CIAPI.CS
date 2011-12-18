using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using CIAPI.DTO;
using CIAPI.IntegrationTests.Streaming;
using CIAPI.Rpc;
using CIAPI.Streaming;
using NUnit.Framework;

namespace CIAPI.IntegrationTests.Rpc
{
    [TestFixture]
    public class AccountInformationFixture : RpcFixtureBase
    {
        [Test]
        public void CanListOpenPositions()
        {

            var rpcClient = BuildRpcClient();

            AccountInformationResponseDTO accounts = rpcClient.AccountInformation.GetClientAndTradingAccount();
            rpcClient.TradesAndOrders.ListOpenPositions(accounts.TradingAccounts[0].TradingAccountId);
            rpcClient.LogOut();
            rpcClient.Dispose();

        }
 
    }
}
