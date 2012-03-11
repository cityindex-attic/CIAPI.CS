using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using CIAPI.DTO;
using CIAPI.IntegrationTests.Streaming;
using CIAPI.Streaming;
using Salient.JsonClient;
using NUnit.Framework;
using Client = CIAPI.Rpc.Client;

namespace CIAPI.IntegrationTests.Rpc.Async
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
