using System;
using System.Collections.Generic;
using System.Threading;
using CIAPI;
using CityIndex.JsonClient;
using CityIndex.JsonClient.Tests;
using CIAPI.DTO;
using Microsoft.Silverlight.Testing;
using NUnit.Framework;



namespace CIAPI.Silverlight.Tests
{
    [TestFixture]
    public class ApiContextTests : SilverlightTest
    {

        private const string NewsHeadlines12 = "{\"Headlines\":[{\"Headline\":\"(UK) Teenage girls often have babies fathered by men\",\"PublishDate\":\"\\/Date(1293727302736)\\/\",\"StoryId\":12654},{\"Headline\":\"(UK) Lung Cancer in Women Mushrooms\",\"PublishDate\":\"\\/Date(1293726702736)\\/\",\"StoryId\":12655},{\"Headline\":\"(UK) Include Your Children when Baking Cookies\",\"PublishDate\":\"\\/Date(1293726102736)\\/\",\"StoryId\":12656},{\"Headline\":\"(UK) Infertility unlikely to be passed on\",\"PublishDate\":\"\\/Date(1293725502736)\\/\",\"StoryId\":12657},{\"Headline\":\"(UK) Child's death ruins couple's holiday\",\"PublishDate\":\"\\/Date(1293724902736)\\/\",\"StoryId\":12658},{\"Headline\":\"(UK) Milk drinkers are turning to powder\",\"PublishDate\":\"\\/Date(1293724302736)\\/\",\"StoryId\":12659},{\"Headline\":\"(UK) Court Rules Boxer Shorts Are Indeed Underwear\",\"PublishDate\":\"\\/Date(1293723702736)\\/\",\"StoryId\":12660},{\"Headline\":\"(UK) Hospitals are Sued by 7 Foot Doctors\",\"PublishDate\":\"\\/Date(1293723102736)\\/\",\"StoryId\":12661},{\"Headline\":\"(UK) Lack of brains hinders research\",\"PublishDate\":\"\\/Date(1293722502736)\\/\",\"StoryId\":12662},{\"Headline\":\"(UK) New Vaccine May Contain Rabies\",\"PublishDate\":\"\\/Date(1293721902736)\\/\",\"StoryId\":12663},{\"Headline\":\"(UK) Two convicts evade noose, jury hung\",\"PublishDate\":\"\\/Date(1293721302736)\\/\",\"StoryId\":12664},{\"Headline\":\"(UK) Safety Experts Say School Bus Passengers Should Be Belted\",\"PublishDate\":\"\\/Date(1293720702736)\\/\",\"StoryId\":12665}]}";
        private const string NewsHeadlines14 = "{\"Headlines\":[{\"Headline\":\"(UK) Teenage girls often have babies fathered by men\",\"PublishDate\":\"\\/Date(1293727302736)\\/\",\"StoryId\":12654},{\"Headline\":\"(UK) Lung Cancer in Women Mushrooms\",\"PublishDate\":\"\\/Date(1293726702736)\\/\",\"StoryId\":12655},{\"Headline\":\"(UK) Include Your Children when Baking Cookies\",\"PublishDate\":\"\\/Date(1293726102736)\\/\",\"StoryId\":12656},{\"Headline\":\"(UK) Infertility unlikely to be passed on\",\"PublishDate\":\"\\/Date(1293725502736)\\/\",\"StoryId\":12657},{\"Headline\":\"(UK) Child's death ruins couple's holiday\",\"PublishDate\":\"\\/Date(1293724902736)\\/\",\"StoryId\":12658},{\"Headline\":\"(UK) Milk drinkers are turning to powder\",\"PublishDate\":\"\\/Date(1293724302736)\\/\",\"StoryId\":12659},{\"Headline\":\"(UK) Court Rules Boxer Shorts Are Indeed Underwear\",\"PublishDate\":\"\\/Date(1293723702736)\\/\",\"StoryId\":12660},{\"Headline\":\"(UK) Hospitals are Sued by 7 Foot Doctors\",\"PublishDate\":\"\\/Date(1293723102736)\\/\",\"StoryId\":12661},{\"Headline\":\"(UK) Lack of brains hinders research\",\"PublishDate\":\"\\/Date(1293722502736)\\/\",\"StoryId\":12662},{\"Headline\":\"(UK) New Vaccine May Contain Rabies\",\"PublishDate\":\"\\/Date(1293721902736)\\/\",\"StoryId\":12663},{\"Headline\":\"(UK) Two convicts evade noose, jury hung\",\"PublishDate\":\"\\/Date(1293721302736)\\/\",\"StoryId\":12664},{\"Headline\":\"(UK) Safety Experts Say School Bus Passengers Should Be Belted\",\"PublishDate\":\"\\/Date(1293720702736)\\/\",\"StoryId\":12665},{\"Headline\":\"(UK) Man Run Over by Freight Train Dies\",\"PublishDate\":\"\\/Date(1293720102736)\\/\",\"StoryId\":12666},{\"Headline\":\"(UK) Teenage girls often have babies fathered by men\",\"PublishDate\":\"\\/Date(1293727302736)\\/\",\"StoryId\":12654}]}";
        private const string LoggedIn = "{\"Session\":\"D2FF3E4D-01EA-4741-86F0-437C919B5559\"}";
        private const string LoggedOut = "{\"LoggedOut\":true}";
        private const string AuthError = "{ \"ErrorMessage\": \"sample value\", \"ErrorCode\": 403 }";

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
        }
        
        [Test]
        [Asynchronous]
        public void CanLoginAsync()
        {
            var requestFactory = new TestRequestFactory();

            var throttleScopes = new Dictionary<string, IThrottedRequestQueue>
                {
                    {"data", new ThrottedRequestQueue(TimeSpan.FromSeconds(5), 30, 10)},
                    {"trading", new ThrottedRequestQueue(TimeSpan.FromSeconds(3), 1, 10)}
                };

            var ctx = new CIAPI.Rpc.Client(new Uri(TestConfig.ApiUrl), new RequestCache(), requestFactory, throttleScopes, 3);

            requestFactory.CreateTestRequest(LoggedIn);

            ctx.BeginCreateSession( TestConfig.ApiUsername, TestConfig.ApiPassword, ar =>
            {
                var response = ctx.EndCreateSession(ar);
                Assert.IsNotNullOrEmpty(response.Session);
                EnqueueTestComplete();
            }, null);

        }

        [Test]
        [Asynchronous]
        public void CanLogoutAsync()
        {

            var requestFactory = new TestRequestFactory();

            var throttleScopes = new Dictionary<string, IThrottedRequestQueue>
                {
                    {"data", new ThrottedRequestQueue(TimeSpan.FromSeconds(5), 30, 10)},
                    {"trading", new ThrottedRequestQueue(TimeSpan.FromSeconds(3), 1, 10)}
                };

            requestFactory.CreateTestRequest(LoggedOut);

            var ctx = new CIAPI.Rpc.Client(new Uri(TestConfig.ApiUrl), new RequestCache(), requestFactory, throttleScopes, 3);

            ctx.BeginDeleteSession(TestConfig.ApiUsername, TestConfig.ApiTestSessionId, ar =>
            {
                var response = ctx.EndDeleteSession(ar);
                Assert.IsTrue(response.LoggedOut);
                EnqueueTestComplete();
            }, null);
        }


        [Test]
        [Asynchronous]
        public void CanGetNewsHeadlinesAsync()
        {

            var requestFactory = new TestRequestFactory();

            var throttleScopes = new Dictionary<string, IThrottedRequestQueue>
                {
                    {"data", new ThrottedRequestQueue(TimeSpan.FromSeconds(5), 30, 10)},
                    {"trading", new ThrottedRequestQueue(TimeSpan.FromSeconds(3), 1, 10)}
                };

            requestFactory.CreateTestRequest(NewsHeadlines14);

            var ctx = new CIAPI.Rpc.Client(new Uri(TestConfig.ApiUrl), new RequestCache(), requestFactory, throttleScopes, 3)
            {
                UserName = TestConfig.ApiUsername,
                SessionId = TestConfig.ApiTestSessionId
            };


            ctx.BeginListNewsHeadlines("UK", 14, ar =>
            {
                ListNewsHeadlinesResponseDTO response = ctx.EndListNewsHeadlines(ar);
                Assert.AreEqual(14, response.Headlines.Length);
                EnqueueTestComplete();
            }, null);

        }
    }
}