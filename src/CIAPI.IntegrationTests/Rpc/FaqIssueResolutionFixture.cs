using System;
using CIAPI.DTO;
using CIAPI.Rpc;
using NUnit.Framework;

namespace CIAPI.IntegrationTests.Rpc
{
    [TestFixture]
    public class FaqIssueResolutionFixture
    {

        /// <summary>
        /// http://faq.labs.cityindex.com/questions/why-getpricebars-sometimes-does-not-return-back-a-getpricebarresponsedto-object-or-throw-an-error
        /// </summary>
        [Test]
        public void Json_Ho_01()
        {

            var rpcClient = new Client(Settings.RpcUri);
            rpcClient.LogIn(Settings.RpcUserName, Settings.RpcPassword);

            GetPriceBarResponseDTO bars = rpcClient.GetPriceBars("71442", "MINUTE", 1, "15");
            Assert.IsNotNull(bars);

            rpcClient.LogOut();
        }



        /// <summary>
        /// https://github.com/cityindex/CIAPI.CS/issues/42
        /// </summary>
        [Test]
        public void Issue42()
        {

            var rpcClient = new Client(Settings.RpcUri);
            rpcClient.LogIn(Settings.RpcUserName, Settings.RpcPassword);

            var response = rpcClient.Order(0, 99498, null, false, "buy", 10m, 12094m, 12098m, "20110629-G2PREPROD3-0102794", 400002249, null, null, null, null, false, 0m);

            rpcClient.LogOut();
        }


        /// <summary>
        /// https://github.com/cityindex/CIAPI.CS/issues/35
        /// </summary>
        [Test]
        public void Issue35()
        {

            var rpcClient = new Client(Settings.RpcUri);
            rpcClient.LogIn("xx189949", "password");

            var accountInfo = rpcClient.GetClientAndTradingAccount();
            var resp = rpcClient.ListTradeHistory(accountInfo.ClientAccountId, 20);


            rpcClient.LogOut();
        }
    }





}
