using System;
using System.Threading;
using CIAPI.DTO;
using CIAPI.IntegrationTests.Streaming;
using CIAPI.Rpc;
using NUnit.Framework;

namespace CIAPI.IntegrationTests.Rpc
{
    [TestFixture]
    public class MarketFixture : RpcFixtureBase
    {
        [Test]
        public void CanGetMarketTags()
        {
            var rpcClient = BuildRpcClient();
            var tagResponse = rpcClient.Market.TagLookup();
            Assert.IsTrue(tagResponse.Tags.Length > 0,"no tags. not sure what this means");
        }

        [Test]
        public void CanSearchWithTags()
        {
            var rpcClient = BuildRpcClient();
            var tagResponse = rpcClient.Market.TagLookup();
            Assert.IsTrue(tagResponse.Tags.Length > 0, "no tags. not sure what this means");
            int tagId = tagResponse.Tags[0].MarketTagId;

            rpcClient.Market.SearchWithTags("USD", tagId, 100);

            Assert.Fail("cant get any tags with which to figure out how to test this method");
        }

        [Test]
        public void CanListMarketInformation()
        {



            var rpcClient = BuildRpcClient();



            var response = rpcClient.Market.ListMarketInformation(new ListMarketInformationRequestDTO()
                                                                      {


                                                                          MarketIds = new int[] { 71442 }
                                                                      });

            Assert.AreEqual(1, response.MarketInformation.Length);

            rpcClient.LogOut();
        }
        [Test]
        public void ListMarketInformationSearchQueryIsValidated()
        {
            var rpcClient = BuildRpcClient();

            rpcClient.Market.ListMarketInformationSearch(false, true, true, false, false, "/", 100);
            rpcClient.Market.ListMarketInformationSearch(false, true, true, false, false, "\\", 100);
            rpcClient.Market.ListMarketInformationSearch(false, true, true, false, false, @"\", 100);
            rpcClient.Market.ListMarketInformationSearch(false, true, true, false, false, @"GBP \ USD", 100);
            rpcClient.Market.ListMarketInformationSearch(false, true, true, false, false, @"GBP \\ USD", 100);

            var gate = new AutoResetEvent(false);

            rpcClient.Market.BeginListMarketInformationSearch(false, true, true, false, false, @"\", 100, ar =>
                {
                    var response = rpcClient.Market.EndListMarketInformationSearch(ar);
                    gate.Set();
                }, null);

            if (!gate.WaitOne(10000))
            {
                throw new TimeoutException();
            }

            rpcClient.LogOut();
        }
        [Test]
        public void CanListMarketInformationSearch()
        {
            var rpcClient = BuildRpcClient();

            var response = rpcClient.Market.ListMarketInformationSearch(false, true, true, false, false, "GBP/USD", 100);
            Assert.Greater(response.MarketInformation.Length, 1);
            rpcClient.LogOut();
        }
        [Test]
        public void CanGetMarketInformation()
        {

            var rpcClient = BuildRpcClient();

            for (int i = 0; i < 10; i++)
            {
                var response = rpcClient.Market.GetMarketInformation("71442");
                Assert.IsTrue(response.MarketInformation.MarketId == 71442);
            }

            rpcClient.LogOut();
        }


        [Test]
        public void CanSaveMarketInformation()
        {
            var rpcClient = BuildRpcClient();

            var clientAccount = rpcClient.AccountInformation.GetClientAndTradingAccount();


            var tolerances = new[]
            {
                new ApiMarketInformationSaveDTO()
                {
                    MarketId = 71442,
                    PriceTolerance = 10
                }
            };

            var saveMarketInfoRespnse = rpcClient.Market.SaveMarketInformation(new SaveMarketInformationRequestDTO()
            {
                MarketInformation = tolerances,
                TradingAccountId = clientAccount.SpreadBettingAccount.TradingAccountId
            });

            // ApiSaveMarketInformationResponseDTO is devoid of properties, nothing to check as long as the call succeeds



        }

    }


}