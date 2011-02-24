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
                Assert.That(ex, Is.TypeOf(typeof(ServerConnectionException)));
                Assert.That(ex.Message, Is.StringContaining("server Url"),"Expecting some info explaining that the server Url is invalid");                
            }
        }

        [Test]
        public void LogOutShouldInvalidateSession()
        {
            var rpcClient = new Client(Settings.RpcUri);
            rpcClient.LogIn(Settings.RpcUserName, Settings.RpcPassword);

            var headlines = rpcClient.ListNewsHeadlines("UK", 3);
            Assert.That(headlines.Headlines.Length, Is.GreaterThan(0), "you should have a set of headlines");

            rpcClient.LogOut();

            try
            {
                rpcClient.ListNewsHeadlines("AUS", 4);
                Assert.Fail("the previous line should have thrown an 'SessionId is null. Have you created a session? (logged in)' exception");
            }
            catch (ApiException e)
            {
                Assert.That(e.Message, Is.StringContaining("SessionId is null. Have you created a session? (logged in)"), "The client should have rejected the request");
            }
        }
    }
}
