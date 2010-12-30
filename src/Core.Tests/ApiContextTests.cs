using System;
using System.Threading;
using CIAPI.DTO;
using NUnit.Framework;

namespace CIAPI.Core.Tests
{
    [TestFixture]
    public class ApiContextTests
    {

        private const string NewsHeadlines12 = "{\"Headlines\":[{\"Headline\":\"(UK) Teenage girls often have babies fathered by men\",\"PublishDate\":\"\\/Date(1293727302736)\\/\",\"StoryId\":12654},{\"Headline\":\"(UK) Lung Cancer in Women Mushrooms\",\"PublishDate\":\"\\/Date(1293726702736)\\/\",\"StoryId\":12655},{\"Headline\":\"(UK) Include Your Children when Baking Cookies\",\"PublishDate\":\"\\/Date(1293726102736)\\/\",\"StoryId\":12656},{\"Headline\":\"(UK) Infertility unlikely to be passed on\",\"PublishDate\":\"\\/Date(1293725502736)\\/\",\"StoryId\":12657},{\"Headline\":\"(UK) Child's death ruins couple's holiday\",\"PublishDate\":\"\\/Date(1293724902736)\\/\",\"StoryId\":12658},{\"Headline\":\"(UK) Milk drinkers are turning to powder\",\"PublishDate\":\"\\/Date(1293724302736)\\/\",\"StoryId\":12659},{\"Headline\":\"(UK) Court Rules Boxer Shorts Are Indeed Underwear\",\"PublishDate\":\"\\/Date(1293723702736)\\/\",\"StoryId\":12660},{\"Headline\":\"(UK) Hospitals are Sued by 7 Foot Doctors\",\"PublishDate\":\"\\/Date(1293723102736)\\/\",\"StoryId\":12661},{\"Headline\":\"(UK) Lack of brains hinders research\",\"PublishDate\":\"\\/Date(1293722502736)\\/\",\"StoryId\":12662},{\"Headline\":\"(UK) New Vaccine May Contain Rabies\",\"PublishDate\":\"\\/Date(1293721902736)\\/\",\"StoryId\":12663},{\"Headline\":\"(UK) Two convicts evade noose, jury hung\",\"PublishDate\":\"\\/Date(1293721302736)\\/\",\"StoryId\":12664},{\"Headline\":\"(UK) Safety Experts Say School Bus Passengers Should Be Belted\",\"PublishDate\":\"\\/Date(1293720702736)\\/\",\"StoryId\":12665}]}";
        private const string NewsHeadlines14 = "{\"Headlines\":[{\"Headline\":\"(UK) Teenage girls often have babies fathered by men\",\"PublishDate\":\"\\/Date(1293727302736)\\/\",\"StoryId\":12654},{\"Headline\":\"(UK) Lung Cancer in Women Mushrooms\",\"PublishDate\":\"\\/Date(1293726702736)\\/\",\"StoryId\":12655},{\"Headline\":\"(UK) Include Your Children when Baking Cookies\",\"PublishDate\":\"\\/Date(1293726102736)\\/\",\"StoryId\":12656},{\"Headline\":\"(UK) Infertility unlikely to be passed on\",\"PublishDate\":\"\\/Date(1293725502736)\\/\",\"StoryId\":12657},{\"Headline\":\"(UK) Child's death ruins couple's holiday\",\"PublishDate\":\"\\/Date(1293724902736)\\/\",\"StoryId\":12658},{\"Headline\":\"(UK) Milk drinkers are turning to powder\",\"PublishDate\":\"\\/Date(1293724302736)\\/\",\"StoryId\":12659},{\"Headline\":\"(UK) Court Rules Boxer Shorts Are Indeed Underwear\",\"PublishDate\":\"\\/Date(1293723702736)\\/\",\"StoryId\":12660},{\"Headline\":\"(UK) Hospitals are Sued by 7 Foot Doctors\",\"PublishDate\":\"\\/Date(1293723102736)\\/\",\"StoryId\":12661},{\"Headline\":\"(UK) Lack of brains hinders research\",\"PublishDate\":\"\\/Date(1293722502736)\\/\",\"StoryId\":12662},{\"Headline\":\"(UK) New Vaccine May Contain Rabies\",\"PublishDate\":\"\\/Date(1293721902736)\\/\",\"StoryId\":12663},{\"Headline\":\"(UK) Two convicts evade noose, jury hung\",\"PublishDate\":\"\\/Date(1293721302736)\\/\",\"StoryId\":12664},{\"Headline\":\"(UK) Safety Experts Say School Bus Passengers Should Be Belted\",\"PublishDate\":\"\\/Date(1293720702736)\\/\",\"StoryId\":12665},{\"Headline\":\"(UK) Man Run Over by Freight Train Dies\",\"PublishDate\":\"\\/Date(1293720102736)\\/\",\"StoryId\":12666},{\"Headline\":\"(UK) Teenage girls often have babies fathered by men\",\"PublishDate\":\"\\/Date(1293727302736)\\/\",\"StoryId\":12654}]}";
        private const string LoggedIn = "{\"Session\":\"D2FF3E4D-01EA-4741-86F0-437C919B5559\"}";
        private const string LoggedOut = "{\"LoggedOut\":true}";
        private const string AuthError = "{ \"ErrorMessage\": \"sample value\", \"ErrorCode\": 403 }";
        [Test]
        public void CanLogin()
        {
            var requestFactory = new TestRequestFactory();
            requestFactory.CreateTestRequest(LoggedIn);
            var cache = new RequestCache();
            var ctx = new ApiContext(new Uri(TestConfig.ApiUrl), cache,requestFactory);
            CreateSessionResponseDTO response = ctx.CreateSession(TestConfig.ApiUsername, TestConfig.ApiPassword);
            Assert.IsNotNullOrEmpty(response.Session);

        }

 

        [Test,Ignore]
        public void ApiAuthenticationFailure()
        {
            var requestFactory = new TestRequestFactory();
            requestFactory.CreateTestRequest(AuthError);
            var cache = new RequestCache();
            var ctx = new ApiContext(new Uri(TestConfig.ApiUrl), cache, requestFactory);
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
        [Test]
        public void CanLogout()
        {
            var requestFactory = new TestRequestFactory();
            requestFactory.CreateTestRequest(LoggedOut);
            var cache = new RequestCache();
            var ctx = new ApiContext(new Uri(TestConfig.ApiUrl), cache, requestFactory);
            SessionDeletionResponseDTO response = ctx.DeleteSession(TestConfig.ApiUsername, TestConfig.ApiTestSessionId);
            Assert.IsTrue(response.LoggedOut);
        }

        [Test]
        public void CanGetNewsHeadlines()
        {
            var requestFactory = new TestRequestFactory();
            requestFactory.CreateTestRequest(NewsHeadlines12);
            var cache = new RequestCache();
            var ctx = new ApiContext(new Uri(TestConfig.ApiUrl), cache, requestFactory)
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
            using (var gate = new ManualResetEvent(false))
            {
                var requestFactory = new TestRequestFactory();
                requestFactory.CreateTestRequest(NewsHeadlines14);
                var cache = new RequestCache();
                var ctx = new ApiContext(new Uri(TestConfig.ApiUrl), cache, requestFactory)
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
