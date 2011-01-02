using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace SOAPI.CS2.Tests
{
    [TestFixture]
    public class StackAuthClientFixture
    {
        [Test]
        public void Test()
        {
            var client = new StackAuthClient();
            var response = client.GetSites();
            Assert.Greater(response.ApiSites.Count,1);
            Assert.IsNotNull(response.ApiSites.First(s => s.SiteUrl == "http://stackoverflow.com"));
        }
    }
}
