using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIAPI.Rpc;
using CityIndex.JsonClient;
using NUnit.Framework;
using Client = CIAPI.Rpc.Client;

namespace CIAPI.IntegrationTests.Rpc
{
    [TestFixture]
    public class ErrorHandlingFixture
    {
        [Test]
        public void ShouldGiveGuidanceWhenSpecifyingInvalidServerName()
        {
            try
            {
                var rpcClient = new Client(new Uri("http://invalidservername.asiuhd8h38hsh.wam/TradingApi"));
                rpcClient.LogIn("username", "password");
            }
            catch (Exception ex)
            {
                Assert.That(ex.Message, Is.StringContaining("DNS"),"Expecting some info explaining that the server name used cannot be found in DNS");                
            }
        }
    }
}
