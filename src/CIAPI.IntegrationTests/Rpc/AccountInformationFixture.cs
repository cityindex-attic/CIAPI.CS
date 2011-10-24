using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using CIAPI.DTO;
using CIAPI.Rpc;
using CIAPI.Streaming;
using NUnit.Framework;

namespace CIAPI.IntegrationTests.Rpc
{
    [TestFixture]
    public class AccountInformationFixture
    {
        [Test]
        public void CanListOpenPositions()
        {

            var rpcClient = new Client(Settings.RpcUri);
            rpcClient.LogIn(Settings.RpcUserName, Settings.RpcPassword);
            AccountInformationResponseDTO accounts = rpcClient.AccountInformation.GetClientAndTradingAccount();
            rpcClient.TradesAndOrders.ListOpenPositions(accounts.TradingAccounts[0].TradingAccountId);
            rpcClient.LogOut();

   
        }
 
    }
}
