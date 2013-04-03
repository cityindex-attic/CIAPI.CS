using System;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using CIAPI.DTO;
using CIAPI.IntegrationTests.Streaming;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CIAPI.Phone7.IntegrationTests
{
     [TestClass]
    public class Class1 : RpcFixtureBase
    {
        [TestInitialize]
        public void TestFixtureSetUp()
        {

        }
        [TestMethod]
        public void noid()
        {
            var gate = new AutoResetEvent(false);
            new Thread(()=>
                           {

                               try
                               {
                                   var rpcClient = BuildRpcClient();

                                   AccountInformationResponseDTO accounts = rpcClient.AccountInformation.GetClientAndTradingAccount();
                                   var x = rpcClient.TradesAndOrders.ListOpenPositions(accounts.TradingAccounts[0].TradingAccountId);
                                   rpcClient.LogOut();
                                   rpcClient.Dispose();
                               }
                               finally
                               {
                                   gate.Set();
                               }
                           }).Start();
            gate.WaitOne();
        }
    }
}
