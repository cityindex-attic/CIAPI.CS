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
        private static Uri RPC_URI = new Uri("http://ciapipreprod.cityindextest9.co.uk/TradingApi");
        private const string USERNAME_VALID = "DM904310";
        private const string PASSWORD_VALID = "password";

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
                Assert.That(ex, Is.TypeOf(typeof(ServerConnectionException)));
                Assert.That(ex.Message, Is.StringContaining("server Url"),"Expecting some info explaining that the server Url is invalid");                
            }
        }

        [Test]
        public void LogOutShouldInvalidateSession()
        {
            var rpcClient = new Client(RPC_URI);
            rpcClient.LogIn(USERNAME_VALID, PASSWORD_VALID);

            var headlines = rpcClient.ListNewsHeadlines("UK", 3);
            Assert.That(headlines.Headlines.Length, Is.GreaterThan(0), "you should have a set of headlines");

            rpcClient.LogOut();

            try
            {
                rpcClient.ListNewsHeadlines("AUS", 4);
                Assert.Fail("the previous line should have thrown an (401) Unauthorized exception");
            }
            catch (ApiException e)
            {
                Assert.That(e.Message, Is.StringContaining("(401) Unauthorized"), "The session token should be invalid after logging out");
            }
        }
    }
}
