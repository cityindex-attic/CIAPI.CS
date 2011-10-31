using CIAPI.DTO;
using CIAPI.Rpc;
using NUnit.Framework;

namespace CIAPI.IntegrationTests.Rpc
{
    [TestFixture]
    public class MarketFixture
    {

        [Test]
        public void CanSaveMarketInformation()
        {
            var rpcClient = new Client(Settings.RpcUri);
            rpcClient.LogIn(Settings.RpcUserName, Settings.RpcPassword);
            var clientAccount = rpcClient.AccountInformation.GetClientAndTradingAccount();

            //rpcClient.Market.GetMarketInformation()
            //rpcClient.Market.ListMarketInformation()
            var saveMarketInfoRespnse = rpcClient.Market.SaveMarketInformation(new SaveMarketInformationRequestDTO()
                                                                                   {
                                                                                       MarketInformation =
                                                                                           new ApiMarketInformationSaveDTO
                                                                                           [] {},
                                                                                       TradingAccountId =
                                                                                           clientAccount.ClientAccountId
                                                                                   });

            Assert.Fail("need usage guidance");
        }

        [Test]
        public void CanListMarketInformation()
        {



            var rpcClient = new Client(Settings.RpcUri);
            rpcClient.LogIn(Settings.RpcUserName, Settings.RpcPassword);


            
            var response = rpcClient.Market.ListMarketInformation(new ListMarketInformationRequestDTO()
                                                                      {


                                                                          MarketIds = new int[] { 71442 }
                                                                      });

            Assert.AreEqual(1, response.MarketInformation.Length);

            rpcClient.LogOut();
        }
        [Test]
        public void CanGetMarketInformation()
        {

            var rpcClient = new Client(Settings.RpcUri);
            rpcClient.LogIn(Settings.RpcUserName, Settings.RpcPassword);

            for (int i = 0; i < 10; i++)
            {
                var response = rpcClient.Market.GetMarketInformation("71442");
                Assert.IsTrue(response.MarketInformation.MarketId == 71442);
            }

            rpcClient.LogOut();
        }


    }


}