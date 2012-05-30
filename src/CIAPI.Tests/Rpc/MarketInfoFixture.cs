using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIAPI.Rpc;
using CIAPI.Serialization;
using NUnit.Framework;
using Salient.ReliableHttpClient;
using Salient.ReliableHttpClient.Testing;

namespace CIAPI.Tests.Rpc
{
    [TestFixture]
    public class MarketInfoFixture
    {
        [Test]
        public void ShouldWorkWithNewGetMarketInformationResponseDTO()
        {
            var factory = new TestRequestFactory();

            var rpcClient = new Client(new Uri("https://test.com/tradingapi"), new Uri("https://test.com"), "test", new Serializer(), factory);

            var requests = new List<RequestInfoBase>
                               {
                                   new RequestInfoBase
                                       {
                                           Index = 0,
                                           ResponseText =
                                               "{\"AllowedAccountOperator\":false,\"PasswordChangeRequired\":false,\"Session\":\"99be8650-d9a3-47cc-a506-044e87db457d\"}"
                                       },
                                   new RequestInfoBase
                                       {
                                           Index = 1,
                                           ResponseText =
                                               "{\"MarketInformation\":{\"MarketId\":400160010,\"Name\":\"UK 100 CFD\",\"MarginFactor\":1.00000000,\"MinMarginFactor\":null,\"MaxMarginFactor\":null,\"MarginFactorUnits\":26,\"MinDistance\":0.00,\"WebMinSize\":3.00000000,\"MaxSize\":2000.00000000,\"Market24H\":true,\"PriceDecimalPlaces\":0,\"DefaultQuoteLength\":180,\"TradeOnWeb\":true,\"LimitUp\":false,\"LimitDown\":false,\"LongPositionOnly\":false,\"CloseOnly\":false,\"MarketEod\":[],\"PriceTolerance\":2.0,\"ConvertPriceToPipsMultiplier\":10000,\"MarketSettingsTypeId\":2,\"MarketSettingsType\":\"CFD\",\"MobileShortName\":\"UK 100\",\"CentralClearingType\":\"No\",\"CentralClearingTypeDescription\":\"None\",\"MarketCurrencyId\":6,\"PhoneMinSize\":3.00000000,\"DailyFinancingAppliedAtUtc\":\"\\/Date(1338238800000)\\/\",\"NextMarketEodTimeUtc\":\"\\/Date(1338238800000)\\/\",\"TradingStartTimeUtc\":null,\"TradingEndTimeUtc\":null,\"MarketPricingTimes\":[{\"DayOfWeek\":1,\"StartTimeUtc\":{\"UtcDateTime\":\"\\/Date(1338249600000)\\/\",\"OffsetMinutes\":60},\"EndTimeUtc\":{\"UtcDateTime\":\"\\/Date(1338321600000)\\/\",\"OffsetMinutes\":60}},{\"DayOfWeek\":2,\"StartTimeUtc\":{\"UtcDateTime\":\"\\/Date(1338249600000)\\/\",\"OffsetMinutes\":60},\"EndTimeUtc\":{\"UtcDateTime\":\"\\/Date(1338321600000)\\/\",\"OffsetMinutes\":60}},{\"DayOfWeek\":3,\"StartTimeUtc\":{\"UtcDateTime\":\"\\/Date(1338249600000)\\/\",\"OffsetMinutes\":60},\"EndTimeUtc\":{\"UtcDateTime\":\"\\/Date(1338321600000)\\/\",\"OffsetMinutes\":60}},{\"DayOfWeek\":4,\"StartTimeUtc\":{\"UtcDateTime\":\"\\/Date(1338249600000)\\/\",\"OffsetMinutes\":60},\"EndTimeUtc\":{\"UtcDateTime\":\"\\/Date(1338321600000)\\/\",\"OffsetMinutes\":60}},{\"DayOfWeek\":5,\"StartTimeUtc\":{\"UtcDateTime\":\"\\/Date(1338249600000)\\/\",\"OffsetMinutes\":60},\"EndTimeUtc\":{\"UtcDateTime\":\"\\/Date(1338321600000)\\/\",\"OffsetMinutes\":60}}],\"MarketBreakTimes\":[],\"MarketSpreads\":[{\"SpreadTimeUtc\":\"\\/Date(1338321600000)\\/\",\"Spread\":6.000000000,\"SpreadUnits\":27},{\"SpreadTimeUtc\":\"\\/Date(1338271440000)\\/\",\"Spread\":4.000000000,\"SpreadUnits\":27},{\"SpreadTimeUtc\":\"\\/Date(1338274200000)\\/\",\"Spread\":6.000000000,\"SpreadUnits\":27},{\"SpreadTimeUtc\":\"\\/Date(1338274800000)\\/\",\"Spread\":2.000000000,\"SpreadUnits\":27}],\"GuaranteedOrderPremium\":3.00,\"GuaranteedOrderPremiumUnits\":1,\"GuaranteedOrderMinDistance\":30.00,\"GuaranteedOrderMinDistanceUnits\":27,\"PriceToleranceUnits\":1.00000,\"MarketTimeZoneOffsetMinutes\":60,\"BetPer\":1.00000,\"MarketUnderlyingTypeId\":2,\"MarketUnderlyingType\":\"Index\",\"ExpiryUtc\":null}}"
                                       }
                               };
            var finder = new TestWebRequestFinder { Reference = requests };
            var requestCounter = 0;
            factory.PrepareResponse = testRequest =>
            {
                finder.PopulateRequest(testRequest, requests[requestCounter]);
                requestCounter++;
            };

            rpcClient.LogIn("username", "password");

            var marketInfo = rpcClient.Market.GetMarketInformation("400494178");

            Assert.IsNotNullOrEmpty(marketInfo.MarketInformation.Name, "Market should have a name");
            Assert.That(marketInfo.MarketInformation.MarketPricingTimes[1].EndTimeUtc.UtcDateTime,
                        Is.EqualTo(DateTime.Parse("2012-05-29 20:00:00.000")));


            rpcClient.Dispose();
        }
    }
}
