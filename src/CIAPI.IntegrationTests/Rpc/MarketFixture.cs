using CIAPI.DTO;
using CIAPI.Rpc;
using NUnit.Framework;

namespace CIAPI.IntegrationTests.Rpc
{
    [TestFixture]
    public class MarketFixture
    {
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

        [Test]
        public void CanSetWatchList()
        {

            var rpcClient = new Client(Settings.RpcUri);
            rpcClient.LogIn(Settings.RpcUserName, Settings.RpcPassword);

            ApiSaveWatchlistRequestDTO input = new ApiSaveWatchlistRequestDTO()
            {
                Watchlist = new ApiClientAccountWatchlistDTO()
                {
                    DisplayOrder = 1,
                    WatchlistDescription = "default",
                    Items = new ApiClientAccountWatchlistItemDTO[]
                                                                                           {
                                                                                               new ApiClientAccountWatchlistItemDTO()
                                                                                                   {
                                                                                                       DisplayOrder = 1,
                                                                                                       MarketId = 1,
                                                                                                       WatchlistId = 71442
                                                                                                   }, 
                                                                           },
                    WatchlistId = 1
                }

            };
            var response = rpcClient.Watchlist.SaveWatchlist(input);
            rpcClient.LogOut();
        }
    }
}