using System;
using System.Collections.ObjectModel;
using NUnit.Framework;
using TradingApi.Client.Core.Domain;
using TradingApi.CoreDTO;

namespace TradingApi.Client.Core.UnitTests.Domain
{
    [TestFixture]
    public class PriceBarTests
    {
        /// <summary>
        /// Default: 2010-02-22 09:48:44
        /// </summary>
        public DateTime sampleTime = DateTime.FromFileTime(129113057243959686);

        [Test]
        public void ToStringShouldShowAllProperties()
        {
            var pricebar = new PriceBarDTO
                               {
                                   BarDate = sampleTime,
                                   Open = 1.2m,
                                   High = 3.4m,
                                   Low = 5.6m,
                                   Close = 7.8m
                               };

            Assert.AreEqual("PriceBarDTO: BarDate=2010-02-22T09:48:44.3959686Z,"+
                            "Open=1.2,High=3.4,Low=5.6,Close=7.8,",
                            pricebar.ToString());
        }
        
    }
}
