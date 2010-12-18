using System;
using System.Threading;
using CIAPI.Core;
using CIAPI.Core.Tests;
using CIAPI.DTO;
using Microsoft.Silverlight.Testing;
using NUnit.Framework;


namespace Core.Silverlight.Tests
{
    [TestFixture]
    public class ApiContextTests : SilverlightTest
    {

        [Test]
        [Asynchronous]
        public void CanLoginAsync()
        {
            var ctx = new ApiContext(new Uri(TestConfig.ApiUrl), TestConfig.BasicAuthUsername, TestConfig.BasicAuthPassword);
            ctx.BeginCreateSession(ar =>
            {
                var response = ctx.EndCreateSession(ar);
                Assert.IsNotNullOrEmpty(response.Session);
                EnqueueTestComplete();
            }, null, TestConfig.ApiUsername, TestConfig.ApiPassword);

        }

        [Test]
        [Asynchronous]
        public void CanLogoutAsync()
        {
            var ctx = new ApiContext(new Uri(TestConfig.ApiUrl), TestConfig.BasicAuthUsername, TestConfig.BasicAuthPassword);
            ctx.BeginDeleteSession(ar =>
            {
                var response = ctx.EndDeleteSession(ar);
                Assert.IsTrue(response.LoggedOut);
                EnqueueTestComplete();
            }, null, TestConfig.ApiUsername, TestConfig.ApiTestSessionId);
        }


        [Test]
        [Asynchronous]
        public void CanGetNewsHeadlinesAsync()
        {
            var ctx = new ApiContext(new Uri(TestConfig.ApiUrl), TestConfig.BasicAuthUsername, TestConfig.BasicAuthPassword)
            {
                UserName = TestConfig.ApiUsername,
                SessionId = TestConfig.ApiTestSessionId
            };



            ctx.BeginListNewsHeadlines(ar =>
            {
                ListNewsHeadlinesResponseDTO response = ctx.EndListNewsHeadlines(ar);
                Assert.AreEqual(14, response.Headlines.Length);
                EnqueueTestComplete();
            }, null, "UK", 14);

        }
    }
}