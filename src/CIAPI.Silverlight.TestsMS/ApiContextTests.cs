using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using CIAPI.DTO;
using CityIndex.JsonClient;
using CityIndex.JsonClient.Tests;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CIAPI.Silverlight.TestsMS
{
    [TestClass]
    public class ApiContextTests : SilverlightTest
    {

        private const string NewsHeadlines12 = "{\"Headlines\":[{\"Headline\":\"(UK) Teenage girls often have babies fathered by men\",\"PublishDate\":\"\\/Date(1293727302736)\\/\",\"StoryId\":12654},{\"Headline\":\"(UK) Lung Cancer in Women Mushrooms\",\"PublishDate\":\"\\/Date(1293726702736)\\/\",\"StoryId\":12655},{\"Headline\":\"(UK) Include Your Children when Baking Cookies\",\"PublishDate\":\"\\/Date(1293726102736)\\/\",\"StoryId\":12656},{\"Headline\":\"(UK) Infertility unlikely to be passed on\",\"PublishDate\":\"\\/Date(1293725502736)\\/\",\"StoryId\":12657},{\"Headline\":\"(UK) Child's death ruins couple's holiday\",\"PublishDate\":\"\\/Date(1293724902736)\\/\",\"StoryId\":12658},{\"Headline\":\"(UK) Milk drinkers are turning to powder\",\"PublishDate\":\"\\/Date(1293724302736)\\/\",\"StoryId\":12659},{\"Headline\":\"(UK) Court Rules Boxer Shorts Are Indeed Underwear\",\"PublishDate\":\"\\/Date(1293723702736)\\/\",\"StoryId\":12660},{\"Headline\":\"(UK) Hospitals are Sued by 7 Foot Doctors\",\"PublishDate\":\"\\/Date(1293723102736)\\/\",\"StoryId\":12661},{\"Headline\":\"(UK) Lack of brains hinders research\",\"PublishDate\":\"\\/Date(1293722502736)\\/\",\"StoryId\":12662},{\"Headline\":\"(UK) New Vaccine May Contain Rabies\",\"PublishDate\":\"\\/Date(1293721902736)\\/\",\"StoryId\":12663},{\"Headline\":\"(UK) Two convicts evade noose, jury hung\",\"PublishDate\":\"\\/Date(1293721302736)\\/\",\"StoryId\":12664},{\"Headline\":\"(UK) Safety Experts Say School Bus Passengers Should Be Belted\",\"PublishDate\":\"\\/Date(1293720702736)\\/\",\"StoryId\":12665}]}";
        private const string NewsHeadlines14 = "{\"Headlines\":[{\"Headline\":\"(UK) Teenage girls often have babies fathered by men\",\"PublishDate\":\"\\/Date(1293727302736)\\/\",\"StoryId\":12654},{\"Headline\":\"(UK) Lung Cancer in Women Mushrooms\",\"PublishDate\":\"\\/Date(1293726702736)\\/\",\"StoryId\":12655},{\"Headline\":\"(UK) Include Your Children when Baking Cookies\",\"PublishDate\":\"\\/Date(1293726102736)\\/\",\"StoryId\":12656},{\"Headline\":\"(UK) Infertility unlikely to be passed on\",\"PublishDate\":\"\\/Date(1293725502736)\\/\",\"StoryId\":12657},{\"Headline\":\"(UK) Child's death ruins couple's holiday\",\"PublishDate\":\"\\/Date(1293724902736)\\/\",\"StoryId\":12658},{\"Headline\":\"(UK) Milk drinkers are turning to powder\",\"PublishDate\":\"\\/Date(1293724302736)\\/\",\"StoryId\":12659},{\"Headline\":\"(UK) Court Rules Boxer Shorts Are Indeed Underwear\",\"PublishDate\":\"\\/Date(1293723702736)\\/\",\"StoryId\":12660},{\"Headline\":\"(UK) Hospitals are Sued by 7 Foot Doctors\",\"PublishDate\":\"\\/Date(1293723102736)\\/\",\"StoryId\":12661},{\"Headline\":\"(UK) Lack of brains hinders research\",\"PublishDate\":\"\\/Date(1293722502736)\\/\",\"StoryId\":12662},{\"Headline\":\"(UK) New Vaccine May Contain Rabies\",\"PublishDate\":\"\\/Date(1293721902736)\\/\",\"StoryId\":12663},{\"Headline\":\"(UK) Two convicts evade noose, jury hung\",\"PublishDate\":\"\\/Date(1293721302736)\\/\",\"StoryId\":12664},{\"Headline\":\"(UK) Safety Experts Say School Bus Passengers Should Be Belted\",\"PublishDate\":\"\\/Date(1293720702736)\\/\",\"StoryId\":12665},{\"Headline\":\"(UK) Man Run Over by Freight Train Dies\",\"PublishDate\":\"\\/Date(1293720102736)\\/\",\"StoryId\":12666},{\"Headline\":\"(UK) Teenage girls often have babies fathered by men\",\"PublishDate\":\"\\/Date(1293727302736)\\/\",\"StoryId\":12654}]}";
        private const string LoggedIn = "{\"Session\":\"D2FF3E4D-01EA-4741-86F0-437C919B5559\"}";
        private const string LoggedOut = "{\"LoggedOut\":true}";
        private const string AuthError = "{ \"ErrorMessage\": \"sample value\", \"ErrorCode\": 403 }";

        [TestMethod]
        [Asynchronous]
        public void CanLoginAsync()
        {

            var ctx = BuildClientAndSetupResponse(LoggedIn);

            ctx.BeginLogIn(TestConfig.ApiUsername, TestConfig.ApiPassword, ar =>
            {
                ctx.EndLogIn(ar);
                EnqueueCallback(() =>
                                    {
                                        try
                                        {
                                            Assert.IsFalse(string.IsNullOrEmpty(ctx.SessionId));
                                        }
                                        finally
                                        {
                                            EnqueueTestComplete();
                                        }

                                    }

                    );


            }, null);

        }

        [TestMethod]
        [Asynchronous]
        public void CanLogoutAsync()
        {


            var ctx = BuildAuthenticatedClientAndSetupResponse(LoggedOut);


            ctx.BeginLogOut(ar =>
            {
                EnqueueCallback(() =>
                {
                    try
                    {
                        var response = ctx.EndLogOut(ar);
                        Assert.IsTrue(response);
                    }
                    finally
                    {
                        EnqueueTestComplete();
                    }

                }
            );


            }, null);
        }

        [TestMethod]
        [Asynchronous]
        public void CanGetNewsHeadlinesAsync()
        {

            var ctx = BuildAuthenticatedClientAndSetupResponse(NewsHeadlines14);

            ctx.News.BeginListNewsHeadlines("UK", 14, ar =>
                {
                    EnqueueCallback(() =>
                                        {
                                            try
                                            {
                                                ListNewsHeadlinesResponseDTO response = ctx.News.EndListNewsHeadlines(ar);
                                                Assert.AreEqual(14, response.Headlines.Length);
                                            }
                                            finally
                                            {
                                                EnqueueTestComplete();
                                            }

                                        });

                }, null);

        }

        #region Plumbing

        private CIAPI.Rpc.Client BuildAuthenticatedClientAndSetupResponse(string expectedJson)
        {
            CIAPI.Rpc.Client ctx = BuildClientAndSetupResponse(expectedJson);

            ctx.UserName = TestConfig.ApiUsername;
            ctx.SessionId = TestConfig.ApiTestSessionId;

            return ctx;
        }
        private CIAPI.Rpc.Client BuildClientAndSetupResponse(string expectedJson)
        {

            TestRequestFactory factory = new TestRequestFactory();
            var requestController = new RequestController(TimeSpan.FromSeconds(0), 2, factory, new NullJsonExceptionFactory(), new ThrottedRequestQueue(TimeSpan.FromSeconds(5), 30, 10, "data"), new ThrottedRequestQueue(TimeSpan.FromSeconds(3), 1, 3, "trading"));

            var ctx = new CIAPI.Rpc.Client(new Uri(TestConfig.RpcUrl), requestController);
            factory.CreateTestRequest(expectedJson);
            return ctx;
        }
        #endregion
    }
}