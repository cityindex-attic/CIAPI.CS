using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TradingApi.Client.Core.UnitTests.Domain
{
    [TestClass]
    public class PriceTests
    {
        [TestMethod]
        public void ToStringShouldShowAllProperties()
        {
            var price = new PriceBuilder().Build();

            Assert.AreEqual("Price: MarketId=12345,Bid=10.5,Offer=10.6,Direction=1,"+
                            "Change=0.3,AuditId=123456-G2DEV-7890,Delta=0.3,ImpliedVolatility=0.1," +
                            "LastUpdateTime=2010-02-22T09:48:44.3959686Z,Indicative=True", 
                            price.ToString());
        }
    }
}
