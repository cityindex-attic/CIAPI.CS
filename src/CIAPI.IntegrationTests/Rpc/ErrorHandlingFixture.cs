using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
 

        [Test]
        public void TheSameErrorShouldhaveTheSameErrorMessage()
        {
            var rpcClient = new Client(Settings.RpcUri);
            rpcClient.LogIn(Settings.RpcUserName, Settings.RpcPassword);

            var error1 = GetBadRequestErrorMessage(rpcClient);
            var error2 = GetBadRequestErrorMessage(rpcClient);
            var error3 = GetBadRequestErrorMessage(rpcClient);

            Console.WriteLine("Error1:{0}\nError2:{1}\nError3:{2}", error1, error2, error3);
            Assert.That(error1, Is.EqualTo(error2), "errors should be the same");
            Assert.That(error2, Is.EqualTo(error3), "errors should be the same");
        }

        private string GetBadRequestErrorMessage(Client rpcClient)
        {
            var errorMessage = "No error thrown";
            try
            {
                const int moreThanMaxHeadlines = 1000;
                rpcClient.ListNewsHeadlines(category: "UK", maxResults: moreThanMaxHeadlines);
            }
            catch (ApiException ex)
            {
                errorMessage = "Message: " + ex.Message + "\nResponseText: " + ex.ResponseText;
            }
            return errorMessage;
        }

        [Test]
        public void ErrorMessageShouldContainDetailsOfErrorResponseDTO()
        {
            var rpcClient = new Client(Settings.RpcUri);
            rpcClient.LogIn(Settings.RpcUserName, Settings.RpcPassword);

            var error1 = GetBadRequestErrorMessage(rpcClient);

            Assert.That(error1, Is.StringContaining("You cannot request more than 500 news headlines"));
        }
    }
}
