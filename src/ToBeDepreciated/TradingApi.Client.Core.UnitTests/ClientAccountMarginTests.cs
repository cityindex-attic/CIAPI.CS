using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TradingApi.Client.Core.UnitTests.Domain
{
    [TestClass]
    public class ClientAccountMarginTests
    {
        [TestMethod]
        public void ToStringShouldShowAllProperties()
        {
            var clientAccountMargin = new ClientAccountMarginBuilder().Build();

            Assert.AreEqual("ClientAccountMargin: MarginIndicator=10.5,Cash=18.766,"
                            + "CreditAllocation=0.3,WaivedMarginRequirement=29876.27,"
                            + "OpenTradeEquity=12876.0,TradingResource=62566.33,"
                            + "NetEquity=625166.3643,Margin=253.2372735271,TradableFunds=37627.34,"
                            + "TotalMarginRequirement=18.12,CurrencyISOCode=GBP",
                            clientAccountMargin.ToString());
        }
    }
}