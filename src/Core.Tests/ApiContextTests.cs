using System;
using System.Threading;
using CIAPI.DTO;
using NUnit.Framework;

namespace CIAPI.Core.Tests
{
    [TestFixture]
    public class ApiContextTests
    {

        [Test]
        public void CanLogin()
        {
            var ctx = new ApiContext(new Uri(TestConfig.ApiUrl), TestConfig.BasicAuthUsername, TestConfig.BasicAuthPassword);
            CreateSessionResponseDTO response = ctx.CreateSession(TestConfig.ApiUsername, TestConfig.ApiPassword);
            Assert.IsNotNullOrEmpty(response.Session);
        }

        [Test]
        public void CanLogout()
        {
            var ctx = new ApiContext(new Uri(TestConfig.ApiUrl), TestConfig.BasicAuthUsername, TestConfig.BasicAuthPassword);
            SessionDeletionResponseDTO response = ctx.DeleteSession(TestConfig.ApiUsername, TestConfig.ApiTestSessionId);
            Assert.IsTrue(response.LoggedOut);
        }

        [Test]
        public void CanGetNewsHeadlines()
        {
            var ctx = new ApiContext(new Uri(TestConfig.ApiUrl), TestConfig.BasicAuthUsername, TestConfig.BasicAuthPassword)
                          {
                              UserName = TestConfig.ApiUsername,
                              SessionId = TestConfig.ApiTestSessionId
                          };
            ListNewsHeadlinesResponseDTO response = ctx.ListNewsHeadlines("UK", 12);
            Assert.AreEqual(12, response.Headlines.Length);

        }

        [Test]
        public void CanGetNewsHeadlinesAsync()
        {
            var ctx = new ApiContext(new Uri(TestConfig.ApiUrl), TestConfig.BasicAuthUsername, TestConfig.BasicAuthPassword)
            {
                UserName = TestConfig.ApiUsername,
                SessionId = TestConfig.ApiTestSessionId
            };
            var gate = new ManualResetEvent(false);

            ListNewsHeadlinesResponseDTO response=null;

            ctx.BeginListNewsHeadlines(ar =>
                                           {
                                               response = ctx.EndListNewsHeadlines(ar);
                                               gate.Set();
                                           }, null, "UK", 14);

            gate.WaitOne();

            Assert.AreEqual(14, response.Headlines.Length);            
        }
    }
}
