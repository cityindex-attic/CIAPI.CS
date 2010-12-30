using System;
using System.Threading;
using CIAPI.DTO;
using NUnit.Framework;

namespace CIAPI.Core.Tests
{
    [TestFixture]
    public class ApiContextTests
    {

        [Test, Ignore("TODO: Remove dependancy on broken AWS instance of RESTWebServices")]
        public void CanLogin()
        {

            var ctx = new ApiContext(new Uri(TestConfig.ApiUrl));
            CreateSessionResponseDTO response = ctx.CreateSession(TestConfig.ApiUsername, TestConfig.ApiPassword);
            Assert.IsNotNullOrEmpty(response.Session);

        }

        [Test, Ignore("TODO: Remove?  Don't think this is a valid test anymore")]
        public void BasicAuthenticationFailure()
        {

            var ctx = new ApiContext(new Uri(TestConfig.ApiUrl));
            try
            {
                ctx.CreateSession(TestConfig.ApiUsername, TestConfig.ApiPassword);
                Assert.Fail("Expected exception");
            }
            catch (ApiException ex)
            {

                Assert.AreEqual("The remote server returned an error: (401) Unauthorized.", ex.Message);
            }
        }

        [Test, Ignore("FIXME: the API is just setting 401. it needs to send ErrorResponseDTO json as well.")]
        public void ApiAuthenticationFailure()
        {
            var ctx = new ApiContext(new Uri(TestConfig.ApiUrl), TestConfig.BasicAuthUsername, TestConfig.BasicAuthPassword);
            try
            {

                ctx.CreateSession("foo", "bar");
                Assert.Fail("Expected exception");
            }
            catch (ApiException ex)
            {


                Assert.AreEqual("[insert api unauthrized]", ex.Message, "FIXME: the API is just setting 401. it needs to send ErrorResponseDTO json as well.");
                Assert.AreEqual("[insert error response dto json]", ex.ResponseText);
            }
        }
        [Test, Ignore("TODO: Remove dependancy on broken AWS instance of RESTWebServices")]
        public void CanLogout()
        {
            var ctx = new ApiContext(new Uri(TestConfig.ApiUrl));
            SessionDeletionResponseDTO response = ctx.DeleteSession(TestConfig.ApiUsername, TestConfig.ApiTestSessionId);
            Assert.IsTrue(response.LoggedOut);
        }

        [Test, Ignore("TODO: Remove dependancy on broken AWS instance of RESTWebServices")]
        public void CanGetNewsHeadlines()
        {
            var ctx = new ApiContext(new Uri(TestConfig.ApiUrl))
                          {
                              UserName = TestConfig.ApiUsername,
                              SessionId = TestConfig.ApiTestSessionId
                          };
            ListNewsHeadlinesResponseDTO response = ctx.ListNewsHeadlines("UK", 12);
            Assert.AreEqual(12, response.Headlines.Length);

        }

        [Test, Ignore("TODO: Remove dependancy on broken AWS instance of RESTWebServices")]
        public void CanGetNewsHeadlinesAsync()
        {
            using (var gate = new ManualResetEvent(false))
            {
                var ctx = new ApiContext(new Uri(TestConfig.ApiUrl))
                {
                    UserName = TestConfig.ApiUsername,
                    SessionId = TestConfig.ApiTestSessionId
                };

                ctx.BeginListNewsHeadlines(ar =>
                    {
                        ListNewsHeadlinesResponseDTO response = ctx.EndListNewsHeadlines(ar);
                        Assert.AreEqual(14, response.Headlines.Length);
                        gate.Set();
                    }, null, "UK", 14);

                gate.WaitOne();
            }
        }
    }
}
