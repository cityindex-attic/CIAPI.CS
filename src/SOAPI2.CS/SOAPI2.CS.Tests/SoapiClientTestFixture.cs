using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace SOAPI2.Tests
{
    [TestFixture]
    public class SoapiClientTestFixture
    {
        const string Apikey = "SFh4Ag1Pid7I4i)VDYjyIw((";
        private const string AppId = "66";

        [Test, Ignore("kevin doesn't like this method being called more than very infrequently.... because his backend sucks?")]
        public void CanGetSites()
        {
            var client = new SoapiClient(Apikey,AppId);
            var response = client.GetSites(1, 10);
            Assert.AreEqual(10, response.Items.Count);
        }

        [Test]
        public void CanGetStackOverflowErrors()
        {
            var client = new SoapiClient(Apikey, AppId);
            var response = client.GetErrors("stackoverflow", 1, 100);
            Assert.Greater(response.Items.Count, 0);

        }
        [Test]
        public void CanGetInfo()
        {
            var client = new SoapiClient(Apikey, AppId);
            var response = client.GetInfo("stackoverflow");
 
            Assert.Greater(response.Items.Count, 0);
        }
    }
}
