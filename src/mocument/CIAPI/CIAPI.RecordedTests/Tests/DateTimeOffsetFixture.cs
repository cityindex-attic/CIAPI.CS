using System;
using System.Linq;
using CIAPI.DTO;
using CIAPI.RecordedTests.Infrastructure;
using NUnit.Framework;
using Newtonsoft.Json;

namespace CIAPI.RecordedTests
{
    [TestFixture, MocumentModeOverride(MocumentMode.Play)]
    public class DateTimeOffsetFixture : CIAPIRecordingFixtureBase
    {
        [Test(Description = "Validates error condition reported in https://github.com/cityindex/CIAPI.CS/issues/133")]
        public void CanHandleResponseWithNullableDateTimeOffset()
        {
            var rpcClient = BuildRpcClient();

            try
            {
                GetMarketInformationResponseDTO result = rpcClient.Market.GetMarketInformation("400160010");
                Console.WriteLine(result.MarketInformation.ToStringWithValues());
                /*  Should give something like:
                        ApiMarketInformationDTO: 
                            MarketId=400160010    Name=UK 100 CFD    MarginFactor=1.00000000    MinMarginFactor=NULL    MarginFactorUnits=26    MaxMarginFactor=NULL    MinDistance=0.00    WebMinSize=3.00000000    MaxSize=2000.00000000    Market24H=True    PriceDecimalPlaces=0    DefaultQuoteLength=180    TradeOnWeb=True    LimitUp=False    LimitDown=False    LongPositionOnly=False    CloseOnly=False    MarketEod=    PriceTolerance=2.0    ConvertPriceToPipsMultiplier=10000    MarketSettingsTypeId=2    MarketSettingsType=CFD    MobileShortName=UK 100    CentralClearingType=No    CentralClearingTypeDescription=None    MarketCurrencyId=6    PhoneMinSize=3.00000000    DailyFinancingAppliedAtUtc=28/05/2012 21:00:00    NextMarketEodTimeUtc=28/05/2012 21:00:00    TradingStartTimeUtc=NULL    TradingEndTimeUtc=NULL    
                        MarketPricingTimes=ApiTradingDayTimesDTO: 
                            DayOfWeek=1    StartTimeUtc=ApiDateTimeOffsetDTO: 
                            UtcDateTime=2012-05-29 00:00:00Z    OffsetMinutes=60
                            EndTimeUtc=ApiDateTimeOffsetDTO: 
                            UtcDateTime=2012-05-29 20:00:00Z    OffsetMinutes=60

                        ApiTradingDayTimesDTO: 
                            DayOfWeek=2    StartTimeUtc=ApiDateTimeOffsetDTO: 
                            UtcDateTime=2012-05-29 00:00:00Z    OffsetMinutes=60
                            EndTimeUtc=ApiDateTimeOffsetDTO: 
                            UtcDateTime=2012-05-29 20:00:00Z    OffsetMinutes=60

                        ApiTradingDayTimesDTO: 
                            DayOfWeek=3    StartTimeUtc=ApiDateTimeOffsetDTO: 
                            UtcDateTime=2012-05-29 00:00:00Z    OffsetMinutes=60
                            EndTimeUtc=ApiDateTimeOffsetDTO: 
                            UtcDateTime=2012-05-29 20:00:00Z    OffsetMinutes=60

                        ApiTradingDayTimesDTO: 
                            DayOfWeek=4    StartTimeUtc=ApiDateTimeOffsetDTO: 
                            UtcDateTime=2012-05-29 00:00:00Z    OffsetMinutes=60
                            EndTimeUtc=ApiDateTimeOffsetDTO: 
                            UtcDateTime=2012-05-29 20:00:00Z    OffsetMinutes=60

                        ApiTradingDayTimesDTO: 
                            DayOfWeek=5    StartTimeUtc=ApiDateTimeOffsetDTO: 
                            UtcDateTime=2012-05-29 00:00:00Z    OffsetMinutes=60
                            EndTimeUtc=ApiDateTimeOffsetDTO: 
                            UtcDateTime=2012-05-29 20:00:00Z    OffsetMinutes=60

                            MarketBreakTimes=    MarketSpreads=ApiMarketSpreadDTO: 
                            SpreadTimeUtc=29/05/2012 20:00:00    Spread=6.000000000    SpreadUnits=27
                        ApiMarketSpreadDTO: 
                            SpreadTimeUtc=29/05/2012 06:04:00    Spread=4.000000000    SpreadUnits=27
                        ApiMarketSpreadDTO: 
                            SpreadTimeUtc=29/05/2012 06:50:00    Spread=6.000000000    SpreadUnits=27
                        ApiMarketSpreadDTO: 
                            SpreadTimeUtc=29/05/2012 07:00:00    Spread=2.000000000    SpreadUnits=27
                            GuaranteedOrderPremium=3.00    GuaranteedOrderPremiumUnits=1    GuaranteedOrderMinDistance=30.00    GuaranteedOrderMinDistanceUnits=27    PriceToleranceUnits=1.00000    MarketTimeZoneOffsetMinutes=60    BetPer=1.00000    MarketUnderlyingTypeId=2    MarketUnderlyingType=Index    ExpiryUtc=NULL

                        */
            }
            catch (Exception exception)
            {


                Assert.Fail("Should not throw exception getting market information\r\n{0}", GetErrorInfo(exception));
            }
            finally
            {
                rpcClient.LogOut();
                rpcClient.Dispose();
            }
        }


    }
}