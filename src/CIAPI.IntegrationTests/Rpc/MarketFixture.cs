using System;
using System.Threading;
using CIAPI.DTO;
using CIAPI.IntegrationTests.Streaming;
using CIAPI.Rpc;
using NUnit.Framework;

namespace CIAPI.IntegrationTests.Rpc
{
    /// <summary>
    /// WARNING!  Market Tags are a new feature, and have not been defined for many account operators.
    /// Try testing with an IFX Markets user account.
    /// </summary>
    [TestFixture]
    public class MarketFixture : RpcFixtureBase
    {
        [Test]
        public void CanGetMarketTags()
        {
            var rpcClient = BuildRpcClient();

            //Markets are grouped into a collection of tags.  
            //You can get a list of available tags from TagLookup
            var tagResponse = rpcClient.Market.TagLookup();
            Assert.IsTrue(tagResponse.Tags.Length > 0,"No tags have been defined for your user's account operator");
            
            Console.WriteLine(tagResponse.ToStringWithValues());
            /* Gives something like:
             * MarketInformationTagLookupResponseDTO: 
                    Tags=ApiPrimaryMarketTagDTO: 
                    Children=ApiMarketTagDTO: 
                    MarketTagId=8    Name=FX Majors    Type=2
                ApiMarketTagDTO: 
                    MarketTagId=9    Name=FX Minors    Type=2
                ApiMarketTagDTO: 
                    MarketTagId=14    Name=Euro Crosses    Type=2
                ApiMarketTagDTO: 
                    MarketTagId=15    Name=Sterling Crosses    Type=2
                    MarketTagId=7    Name=Currencies    Type=1
                ApiPrimaryMarketTagDTO: 
                    Children=ApiMarketTagDTO: 
                    MarketTagId=17    Name=Popular    Type=2
                    MarketTagId=16    Name=FX    Type=1
            */

        }

        [Test]
        public void CanSearchWithTags()
        {
            var rpcClient = BuildRpcClient();

            var tagResponse = rpcClient.Market.TagLookup();
            Assert.IsTrue(tagResponse.Tags.Length > 0, "No tags have been defined for your user's account operator");

            //Once you have a tag, you can search for all markets associated with that tag
            int tagId = tagResponse.Tags[0].MarketTagId;
            var allMarketsInTag = rpcClient.Market.SearchWithTags("", tagId, 100, false);
            Console.WriteLine(allMarketsInTag.ToStringWithValues());
            /* Gives something like:
             * MarketInformationSearchWithTagsResponseDTO: 
                    Markets=ApiMarketDTO: 
                    MarketId=400481134    Name=EUR/USD
                ApiMarketDTO: 
                    MarketId=400481136    Name=GBP/AUD
                ApiMarketDTO: 
                    MarketId=400481139    Name=GBP/JPY
                ApiMarketDTO: 
                    MarketId=400481142    Name=GBP/USD
                Tags=ApiMarketTagDTO: 
                    MarketTagId=8    Name=FX Majors    Type=2
                ApiMarketTagDTO: 
                    MarketTagId=9    Name=FX Minors    Type=2
                ApiMarketTagDTO: 
                    MarketTagId=14    Name=Euro Crosses    Type=2
                ApiMarketTagDTO: 
                    MarketTagId=15    Name=Sterling Crosses    Type=2
               */

            //Or, you can search for all markets in that tag that start with a specific string
            var allMarketsInTagContainingGBP = rpcClient.Market.SearchWithTags("GBP", tagId, 100, false);
            Console.WriteLine(allMarketsInTagContainingGBP.ToStringWithValues());
            /* Gives something like:
             * MarketInformationSearchWithTagsResponseDTO: 
                Markets=ApiMarketDTO: 
                MarketId=400481136    Name=GBP/AUD
            ApiMarketDTO: 
                MarketId=400481139    Name=GBP/JPY
            ApiMarketDTO: 
                MarketId=400481142    Name=GBP/USD
             */
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

            rpcClient.Market.ListMarketInformationSearch(false, true, true, false, false, "/", 100, false);
            rpcClient.Market.ListMarketInformationSearch(false, true, true, false, false, "\\", 100, false);
            rpcClient.Market.ListMarketInformationSearch(false, true, true, false, false, @"\", 100, false);
            rpcClient.Market.ListMarketInformationSearch(false, true, true, false, false, @"GBP \ USD", 100, false);
            rpcClient.Market.ListMarketInformationSearch(false, true, true, false, false, @"GBP \\ USD", 100, false);

            var gate = new AutoResetEvent(false);

            rpcClient.Market.BeginListMarketInformationSearch(false, true, true, false, false, @"\", 100, false, ar =>
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

            var response = rpcClient.Market.ListMarketInformationSearch(false, true, true, false, false, "GBP/USD", 100, false);
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