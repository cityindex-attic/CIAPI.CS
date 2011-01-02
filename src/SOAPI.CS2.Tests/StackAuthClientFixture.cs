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
        public void GetSites()
        {
            var client = new StackAuthClient();
            var response = client.GetSites();
            Assert.Greater(response.Items.Count,1);
            Assert.IsNotNull(response.Items.First(s => s.SiteUrl == "http://stackoverflow.com"));
        }

        [Test]
        public void GetAssociatedUsers()
        {
            var client = new StackAuthClient();
            var response = client.GetAssociatedUsers(new Guid("e58345f5-0f7b-4261-b449-3959c596f91f"));
            Assert.Greater(response.Items.Count, 1);
            Assert.IsNotNull(response.Items.First(s => s.OnSite.SiteUrl == "http://stackapps.com"));
        }
    }
}
