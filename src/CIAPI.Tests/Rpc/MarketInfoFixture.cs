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
                                               "{\"MarketInformation\":{\"MarketId\":400494178,\"Name\":\"AUD/NZD\",\"MarginFactor\":1.00000000,\"MinMarginFactor\":null,\"MaxMarginFactor\":null,\"MarginFactorUnits\":26,\"MinDistance\":0.02,\"WebMinSize\":5000.00000000,\"MaxSize\":500000.00000000,\"Market24H\":true,\"PriceDecimalPlaces\":5,\"DefaultQuoteLength\":5,\"TradeOnWeb\":true,\"LimitUp\":false,\"LimitDown\":false,\"LongPositionOnly\":false,\"CloseOnly\":false,\"MarketEod\":[],\"PriceTolerance\":2.0,\"ConvertPriceToPipsMultiplier\":10000,\"MarketSettingsTypeId\":2,\"MarketSettingsType\":\"CFD\",\"MobileShortName\":null,\"CentralClearingType\":\"No\",\"CentralClearingTypeDescription\":\"None\",\"MarketCurrencyId\":20,\"PhoneMinSize\":5000.00000000,\"DailyFinancingAppliedAtUtc\":\"\\/Date(1337288400000)\\/\",\"NextMarketEodTimeUtc\":\"\\/Date(1337288400000)\\/\",\"TradingStartTimeUtc\":null,\"TradingEndTimeUtc\":null,\"MarketPricingTimes\":[{\"DayOfWeek\":1,\"StartTimeUtc\":{\"UtcDateTime\":\"\\/Date(1337288400000)\\/\",\"OffsetMinutes\":600},\"EndTimeUtc\":null},{\"DayOfWeek\":5,\"StartTimeUtc\":null,\"EndTimeUtc\":{\"UtcDateTime\":\"\\/Date(1337372100000)\\/\",\"OffsetMinutes\":-240}}],\"MarketBreakTimes\":[],\"MarketSpreads\":[{\"SpreadTimeUtc\":null,\"Spread\":0.00074000,\"SpreadUnits\":27}],\"GuaranteedOrderPremium\":10.00,\"GuaranteedOrderPremiumUnits\":1,\"GuaranteedOrderMinDistance\":100.00,\"GuaranteedOrderMinDistanceUnits\":27,\"PriceToleranceUnits\":1.00000,\"MarketTimeZoneOffsetMinutes\":-240,\"BetPer\":1.00000,\"MarketUnderlyingTypeId\":4,\"MarketUnderlyingType\":\"FX\",\"ExpiryUtc\":null}}"
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
                        Is.EqualTo(DateTime.Parse("2012-05-18 20:15:00")));


            rpcClient.Dispose();
        }
    }
}
