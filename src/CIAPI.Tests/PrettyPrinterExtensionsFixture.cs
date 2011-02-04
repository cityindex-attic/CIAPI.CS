using System;
using System.Threading;
using CIAPI.DTO;
using CIAPI.Streaming;
using NUnit.Framework;

namespace CIAPI.Tests
{
    [TestFixture]
    public class PrettyPrinterExtensionsFixture
    {
        [Test]
        public void CreateStringWithValuesOfEachPublicProperty()
        {
            var dto = new TheDto
                          {
                              AnInt = 12,
                              AString = "No place like 127.0.0.1",
                              ADate = new DateTime(2011, 02, 03, 13, 24, 45, 111),
                              ABool = false
                          };
            Assert.AreEqual("TheDto: \n\tAnInt=12\tAString=No place like 127.0.0.1\tADate=2011-02-03 13:24:45Z\tABool=False", dto.ToStringWithValues());
        }

        private class TheDto
        {
            public int AnInt { get; set; }
            public string AString { get; set; }
            public DateTime ADate { get; set; }
            public bool ABool { get; set; }
            private int _secret = 42;
        }
    }
}
