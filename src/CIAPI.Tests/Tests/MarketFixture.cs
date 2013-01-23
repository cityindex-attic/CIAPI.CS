using System;
using System.Linq;
using System.Threading;
using CIAPI.DTO;
using CIAPI.RecordedTests.Infrastructure;
using CIAPI.Rpc;
using NUnit.Framework;

namespace CIAPI.RecordedTests
{
    [TestFixture, MocumentModeOverride(MocumentMode.Play),Category("APIBUG")]
    public class MarketFixture : CIAPIRecordingFixtureBase
    {
        [Test]
        public void CanGetMarketTags()
        {
            var rpcClient = BuildRpcClient();

            //Markets are grouped into a collection of tags.  
            //You can get a list of available tags from TagLookup
            var tagResponse = rpcClient.Market.TagLookup();
            Assert.IsTrue(tagResponse.Tags.Length > 0, "No tags have been defined for your user's account operator");

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
            int tagId = tagResponse.Tags.First(t => t.Name.Contains("FX")).MarketTagId;
            var allMarketsInTag = rpcClient.Market.SearchWithTags("", tagId, true, true, true, true, true, true, 100, false);
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
            var allMarketsInTagContainingGBP = rpcClient.Market.SearchWithTags("GBP", tagId, true, true, true, true, true, true, 100, false);
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
            var marketList = GetAvailableCFDMarkets(rpcClient);

            var response = rpcClient.Market.ListMarketInformation(
                new ListMarketInformationRequestDTO
                {
                    MarketIds = marketList.ToList().Select(m => m.MarketId).ToArray()
                }
            );

            Assert.AreEqual(marketList.Length, response.MarketInformation.Length);

            rpcClient.LogOut();
        }

        [Test]
        public void ListMarketInformationSearchQueryIsValidated()
        {
            var rpcClient = BuildRpcClient();

            rpcClient.Market.ListMarketInformationSearch(false, true, true, false, false, true, "/", 100, false);
            rpcClient.Market.ListMarketInformationSearch(false, true, true, false, false, true, "\\", 100, false);
            rpcClient.Market.ListMarketInformationSearch(false, true, true, false, false, true, @"\", 100, false);
            rpcClient.Market.ListMarketInformationSearch(false, true, true, false, false, true, @"GBP \ USD", 100, false);
            rpcClient.Market.ListMarketInformationSearch(false, true, true, false, false, true, @"GBP \\ USD", 100, false);

            var gate = new AutoResetEvent(false);

            rpcClient.Market.BeginListMarketInformationSearch(false, true, true, false, false, true, @"\", 100, false, ar =>
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

            var response = rpcClient.Market.ListMarketInformationSearch(false, true, false, true, false, true, "GBP", 10, false);
            Assert.Greater(response.MarketInformation.Length, 1);
            rpcClient.LogOut();
        }

        /// <summary>
        /// Test showing issue #167 - https://github.com/cityindex/CIAPI.CS/issues/167 -
        /// has stopped occuring
        /// </summary>
        [Test, Ignore("Long running")]
        public void MultipleRequestsToCanGetMarketInformationShouldNotThrowAnyExceptions()
        {
            var rpcClient = BuildRpcClient();

            for (var i = 0; i < 100; i++)
            {
                var response = rpcClient.Market.GetMarketInformation("154303");
                Assert.That(response.MarketInformation.Name.Length, Is.GreaterThan(1));
                Thread.Sleep(1100);
            }

            rpcClient.LogOut();
        }

        //
        [Test]
        public void CanGetMarketInformationWithPathChar()
        {
            var rpcClient = BuildRpcClient();
            var account = rpcClient.AccountInformation.GetClientAndTradingAccount();

            var response = rpcClient.SpreadMarkets.ListSpreadMarkets("GBP/CAD", null, account.ClientAccountId, 19, false);



            Assert.That(response.Markets.Length, Is.GreaterThan(1));

            rpcClient.LogOut();
        }
        [Test]
        public void CanGetMarketInformation()
        {
            var rpcClient = BuildRpcClient();
            var marketList = GetAvailableCFDMarkets(rpcClient);

            var response = rpcClient.Market.GetMarketInformation(marketList[0].MarketId.ToString());
            string stringWithValues = response.MarketInformation.ToStringWithValues();
            Console.WriteLine(stringWithValues);

            /* Returns data like the following (2012/04/27)
            ApiMarketInformationDTO: 
                MarketId=154291    Name=GBP/AUD (per 0.0001) CFD    MarginFactor=1.00000000    MinMarginFactor=NULL    
                MaxMarginFactor=NULL    MarginFactorUnits=26    MinDistance=0.00    WebMinSize=1.00000000    
                MaxSize=2200.00000000    Market24H=True    PriceDecimalPlaces=5    DefaultQuoteLength=180    TradeOnWeb=True    
                LimitUp=False    LimitDown=False    LongPositionOnly=False    CloseOnly=False    MarketEod=    
                PriceTolerance=10.00000000    ConvertPriceToPipsMultiplier=10000    MarketSettingsTypeId=2    
                MarketSettingsType=CFD    MobileShortName=GBP/AUD    CentralClearingType=No    
                CentralClearingTypeDescription=None    MarketCurrencyId=1    PhoneMinSize=5.00000000    
                DailyFinancingAppliedAtUtc=26/04/2012 21:00:00    NextMarketEodTimeUtc=26/04/2012 21:00:00    
                TradingStartTimeUtc=NULL    TradingEndTimeUtc=NULL    
                MarketPricingTimes=ApiTradingDayTimesDTO: 
                    DayOfWeek=1    StartTimeUtc=26/04/2012 11:00:00 +00:00    EndTimeUtc=NULL
            ApiTradingDayTimesDTO: 
                DayOfWeek=5    StartTimeUtc=NULL    EndTimeUtc=28/04/2012 00:15:00 +00:00
                MarketBreakTimes=    
                MarketSpreads=ApiMarketSpreadDTO: 
                    SpreadTimeUtc=NULL    Spread=0.00067000    SpreadUnits=27
                    GuaranteedOrderPremium=8.00    GuaranteedOrderPremiumUnits=1    GuaranteedOrderMinDistance=75.00    GuaranteedOrderMinDistanceUnits=27
            */

            Assert.That(response.MarketInformation.Name.Length, Is.GreaterThan(1));

            rpcClient.LogOut();
        }

        [Test]
        public void CanSaveMarketInformation()
        {
            var rpcClient = BuildRpcClient();

            var clientAccount = rpcClient.AccountInformation.GetClientAndTradingAccount();
            var marketList = GetAvailableCFDMarkets(rpcClient);

            var tolerances = new[]
            {
                new ApiMarketInformationSaveDTO
                    {
                    MarketId = marketList[0].MarketId,
                    PriceTolerance = 10
                }
            };

            var saveMarketInfoRespnse = rpcClient.Market.SaveMarketInformation(new SaveMarketInformationRequestDTO()
            {
                MarketInformation = tolerances,
                TradingAccountId = clientAccount.CFDAccount.TradingAccountId
            });

            // ApiSaveMarketInformationResponseDTO is devoid of properties, nothing to check as long as the call succeeds



        }
        public static ApiMarketInformationDTO[] GetAvailableCFDMarkets(Client rpcClient)
        {
            var marketList = rpcClient.Market.ListMarketInformationSearch(false, true, false, true, false, true, "GBP", 10, false);
            Assert.That(marketList.MarketInformation.Length, Is.GreaterThanOrEqualTo(1), "There should be at least 1 CFD market availbe");
            return marketList.MarketInformation;
        }
    }
}