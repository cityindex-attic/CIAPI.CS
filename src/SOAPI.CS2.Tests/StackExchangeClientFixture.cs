using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace SOAPI.CS2.Tests
{
    [TestFixture]
    public class StackExchangeClientFixture
    {
        private const string ApiKey = "bUuJ4fKQGEyKobrMVApUTw";
        [Test]
        public void GetUserById()
        {
            var client = new StackExchangeClient(ApiKey, "http://api.stackapps.com/1.0");
            var response = client.GetUserById(14, null, null, null, null, null, null, null, null);
            Assert.AreEqual(1, response.Items.Count);
            Assert.AreEqual("df4a7fbd8a054fd6193ca0ee62952f1f", response.Items[0].EmailHash);
        }
    }
}
