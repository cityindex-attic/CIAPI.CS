using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using CIAPI.IntegrationTests.Streaming;
using CIAPI.Rpc;

using NUnit.Framework;
using Salient.ReliableHttpClient;
using Client = CIAPI.Rpc.Client;

namespace CIAPI.IntegrationTests.Rpc
{
    [TestFixture]
    public class ErrorHandlingFixture : RpcFixtureBase
    { 
        [Test]
        public void ShouldGiveGuidanceWhenSpecifyingInvalidServerName()
        {
            try
            {
                var rpcClient = new Client(new Uri("http://invalidservername.asiuhd8h38hsh.wam/TradingApi"), new Uri("http://foo.bar.com"), AppKey);
                rpcClient.LogIn("username", "password");
                Assert.Fail("Expected exception");
            }
            catch (Exception ex)
            {
                Assert.That(ex.Message, Is.StringContaining("Invalid response received.  Are you connecting to the correct server Url?"), "Expecting some info explaining that the server Url is invalid");
            }
        }

        [Test]
        public void Http200ErrorsShouldThrow()
        {
            try
            {
                var rpcClient = BuildRpcClient();
                rpcClient.Http200ErrorsOnly = true;

                const int moreThanMaxHeadlines = 1000;
                rpcClient.News.ListNewsHeadlinesWithSource(source: "dj", category: "UK", maxResults: moreThanMaxHeadlines);
            }
            catch (Exception ex)
            {
                Assert.That(ex.Message, Is.StringContaining("You cannot request more than 500 news headlines"), "Expecting some info explaining that the server Url is invalid");
            }
        }

        [Test]
        public void LogOutShouldInvalidateSession()
        {
            var rpcClient = BuildRpcClient();

            var headlines = rpcClient.News.ListNewsHeadlinesWithSource("dj", "UK", 3);
            Assert.That(headlines.Headlines.Length, Is.GreaterThan(0), "you should have a set of headlines");

            rpcClient.LogOut();

            try
            {
                rpcClient.News.ListNewsHeadlinesWithSource("dj", "AUS", 4);
                Assert.Fail("the previous line should have thrown an 'Session is null. Have you created a session? (logged in)' exception");
            }
            catch (ReliableHttpException e)
            {
                Assert.That(e.Message, Is.StringContaining("Session is null. Have you created a session? (logged on)"), "The client should have rejected the request");
            }
        }


        [Test]
        public void TheSameErrorShouldhaveTheSameErrorMessage()
        {
            var rpcClient = BuildRpcClient();

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
                rpcClient.News.ListNewsHeadlinesWithSource(source: "dj", category: "UK", maxResults: moreThanMaxHeadlines);
            }
            catch (ReliableHttpException ex)
            {
                errorMessage = "Message: " + ex.Message;
            }
            return errorMessage;
        }

        [Test]
        public void ErrorMessageShouldContainDetailsOfErrorResponseDTO()
        {
            var rpcClient = BuildRpcClient();

            var error1 = GetBadRequestErrorMessage(rpcClient);

            Assert.That(error1, Is.StringContaining("You cannot request more than 500 news headlines"));
        }


        [Test,Ignore("endpoint removed")]
        public void Issue166ExceptionEndpointShouldThrowCorrectException()
        {
            var rpcClient = BuildRpcClient();

            try
            {
                var response = rpcClient.ExceptionHandling.GenerateException(4003);
            }
            catch (ReliableHttpException ex)
            {

                Assert.AreEqual(4003, ex.ErrorCode, "expecting a 4003");
            }
        }

    }
}
