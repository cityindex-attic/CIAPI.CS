using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TradingApi.Client.Core.UnitTests
{
    [TestClass]
    public class JSONParserTests
    {
        [TestMethod]
        public void ShouldParseToUtc()
        {
            //14 Sep 2010 17:30:05 GMT
            const string jsonDate = @"\/Date(1284485405000)\/";
            var date = JSONParser.ParseJSONDateToUtc(jsonDate);
            Assert.AreEqual(DateTime.Parse("2010-09-14 17:30:05Z").ToUniversalTime(), date);
        }
    }
}
