using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using CIAPI.DTO;
using CIAPI.Rpc;
using CityIndex.JsonClient;
using CityIndex.JsonClient.Tests;
using Newtonsoft.Json;
using NUnit.Framework;
using Client = CIAPI.Rpc.Client;

namespace CIAPI.Tests.Rpc
{

    [TestFixture]
    public class ExceptionHandling
    {
        [Test,Ignore, ExpectedException(typeof(ApiTimeoutException))]
        public void ReproAbortedRequest()
        {
            TestRequestFactory factory = new TestRequestFactory();
            var requestController = new RequestController(TimeSpan.FromSeconds(0), 2, factory, new ErrorResponseDTOJsonExceptionFactory(), new ThrottedRequestQueue(TimeSpan.FromSeconds(5), 30, 10, "data"), new ThrottedRequestQueue(TimeSpan.FromSeconds(3), 1, 3, "trading"));

            var ctx = new CIAPI.Rpc.Client(new Uri(TestConfig.RpcUrl), requestController);
            ctx.UserName = TestConfig.ApiUsername;
            ctx.Session = TestConfig.ApiTestSessionId;

            factory.CreateTestRequest("{}", TimeSpan.FromMinutes(1));

            ctx.Market.GetMarketInformation("FOO");

        }

    }
    [TestFixture]
    public class ApiContextTests
    {


        private const string NewsHeadlines12 = "{\"Headlines\":[{\"Headline\":\"(UK) Teenage girls often have babies fathered by men\",\"PublishDate\":\"\\/Date(1293727302736)\\/\",\"StoryId\":12654},{\"Headline\":\"(UK) Lung Cancer in Women Mushrooms\",\"PublishDate\":\"\\/Date(1293726702736)\\/\",\"StoryId\":12655},{\"Headline\":\"(UK) Include Your Children when Baking Cookies\",\"PublishDate\":\"\\/Date(1293726102736)\\/\",\"StoryId\":12656},{\"Headline\":\"(UK) Infertility unlikely to be passed on\",\"PublishDate\":\"\\/Date(1293725502736)\\/\",\"StoryId\":12657},{\"Headline\":\"(UK) Child's death ruins couple's holiday\",\"PublishDate\":\"\\/Date(1293724902736)\\/\",\"StoryId\":12658},{\"Headline\":\"(UK) Milk drinkers are turning to powder\",\"PublishDate\":\"\\/Date(1293724302736)\\/\",\"StoryId\":12659},{\"Headline\":\"(UK) Court Rules Boxer Shorts Are Indeed Underwear\",\"PublishDate\":\"\\/Date(1293723702736)\\/\",\"StoryId\":12660},{\"Headline\":\"(UK) Hospitals are Sued by 7 Foot Doctors\",\"PublishDate\":\"\\/Date(1293723102736)\\/\",\"StoryId\":12661},{\"Headline\":\"(UK) Lack of brains hinders research\",\"PublishDate\":\"\\/Date(1293722502736)\\/\",\"StoryId\":12662},{\"Headline\":\"(UK) New Vaccine May Contain Rabies\",\"PublishDate\":\"\\/Date(1293721902736)\\/\",\"StoryId\":12663},{\"Headline\":\"(UK) Two convicts evade noose, jury hung\",\"PublishDate\":\"\\/Date(1293721302736)\\/\",\"StoryId\":12664},{\"Headline\":\"(UK) Safety Experts Say School Bus Passengers Should Be Belted\",\"PublishDate\":\"\\/Date(1293720702736)\\/\",\"StoryId\":12665}]}";
        private const string NewsHeadlines14 = "{\"Headlines\":[{\"Headline\":\"(UK) Teenage girls often have babies fathered by men\",\"PublishDate\":\"\\/Date(1293727302736)\\/\",\"StoryId\":12654},{\"Headline\":\"(UK) Lung Cancer in Women Mushrooms\",\"PublishDate\":\"\\/Date(1293726702736)\\/\",\"StoryId\":12655},{\"Headline\":\"(UK) Include Your Children when Baking Cookies\",\"PublishDate\":\"\\/Date(1293726102736)\\/\",\"StoryId\":12656},{\"Headline\":\"(UK) Infertility unlikely to be passed on\",\"PublishDate\":\"\\/Date(1293725502736)\\/\",\"StoryId\":12657},{\"Headline\":\"(UK) Child's death ruins couple's holiday\",\"PublishDate\":\"\\/Date(1293724902736)\\/\",\"StoryId\":12658},{\"Headline\":\"(UK) Milk drinkers are turning to powder\",\"PublishDate\":\"\\/Date(1293724302736)\\/\",\"StoryId\":12659},{\"Headline\":\"(UK) Court Rules Boxer Shorts Are Indeed Underwear\",\"PublishDate\":\"\\/Date(1293723702736)\\/\",\"StoryId\":12660},{\"Headline\":\"(UK) Hospitals are Sued by 7 Foot Doctors\",\"PublishDate\":\"\\/Date(1293723102736)\\/\",\"StoryId\":12661},{\"Headline\":\"(UK) Lack of brains hinders research\",\"PublishDate\":\"\\/Date(1293722502736)\\/\",\"StoryId\":12662},{\"Headline\":\"(UK) New Vaccine May Contain Rabies\",\"PublishDate\":\"\\/Date(1293721902736)\\/\",\"StoryId\":12663},{\"Headline\":\"(UK) Two convicts evade noose, jury hung\",\"PublishDate\":\"\\/Date(1293721302736)\\/\",\"StoryId\":12664},{\"Headline\":\"(UK) Safety Experts Say School Bus Passengers Should Be Belted\",\"PublishDate\":\"\\/Date(1293720702736)\\/\",\"StoryId\":12665},{\"Headline\":\"(UK) Man Run Over by Freight Train Dies\",\"PublishDate\":\"\\/Date(1293720102736)\\/\",\"StoryId\":12666},{\"Headline\":\"(UK) Teenage girls often have babies fathered by men\",\"PublishDate\":\"\\/Date(1293727302736)\\/\",\"StoryId\":12654}]}";
        private const string BogusJson = "foo bar fu";
        private const string LoggedIn = "{\"Session\":\"D2FF3E4D-01EA-4741-86F0-437C919B5559\"}";
        private const string LoggedOut = "{\"LoggedOut\":true}";
        private const string AuthError = "{ \"ErrorMessage\": \"sample value\", \"ErrorCode\": 403 }";

        [Test]
        public void CanLogin()
        {
            Console.WriteLine("CanLogin");

            var ctx = BuildAuthenticatedClientAndSetupResponse(LoggedIn);

            ctx.LogIn(TestConfig.ApiUsername, TestConfig.ApiPassword);

            Assert.IsNotNullOrEmpty(ctx.Session);

        }



        [Test, ExpectedException(typeof(ApiException), ExpectedMessage = "InvalidCredentials")]
        public void CanRecognize200JsonException()
        {

            var errorDto = new ApiErrorResponseDTO()
                               {
                                   ErrorCode = (int)ErrorCode.InvalidCredentials,
                                   ErrorMessage = "InvalidCredentials"

                               };

            var ctx = BuildAuthenticatedClientAndSetupResponse(JsonConvert.SerializeObject(errorDto));

            ctx.LogIn(TestConfig.ApiUsername, TestConfig.ApiPassword);

        }

        [Test]
        public void ApiAuthenticationFailure()
        {

            var errorDto = new ApiErrorResponseDTO()
            {
                ErrorCode = (int)ErrorCode.InvalidCredentials,
                ErrorMessage = "InvalidCredentials"

            };

            var ctx = BuildAuthenticatedClientAndSetupResponse(JsonConvert.SerializeObject(errorDto));

            try
            {
                ctx.LogIn("foo", "bar");
                Assert.Fail("Expected exception");
            }
            catch (ApiException ex)
            {
                Assert.AreEqual("InvalidCredentials", ex.Message, "FIXME: the API is just setting 401. it needs to send ErrorResponseDTO json as well.");
                Assert.AreEqual("{\"ErrorMessage\":\"InvalidCredentials\",\"ErrorCode\":4010}", ex.ResponseText);
            }
        }

        [Test]
        public void CanLogout()
        {
            Console.WriteLine("CanLogout");

            var ctx = BuildAuthenticatedClientAndSetupResponse(LoggedOut);

            bool response = ctx.LogOut();
            Assert.IsTrue(response);
        }

        [Test]
        public void CanGetNewsHeadlines()
        {
            Console.WriteLine("CanGetNewsHeadlines");

            var ctx = BuildAuthenticatedClientAndSetupResponse(NewsHeadlines12);

            ListNewsHeadlinesResponseDTO response = ctx.News.ListNewsHeadlines("UK", 12);
            Assert.AreEqual(12, response.Headlines.Length);

        }


        [Test]
        public void DeserializationExceptionIsProperlySurfacedBySyncRequests()
        {
            Console.WriteLine("DeserializationExceptionIsProperlySurfacedBySyncRequests");

            CIAPI.Rpc.Client ctx = BuildAuthenticatedClientAndSetupResponse(BogusJson);
            Assert.Throws<CIAPI.Rpc.ServerConnectionException>(() => ctx.News.GetNewsDetail("foobar"));
        }


        [Test]
        public void DeserializationExceptionIsProperlySurfacedByAsyncRequests()
        {
            Console.WriteLine("DeserializationExceptionIsProperlySurfacedByAsyncRequests");

            CIAPI.Rpc.Client ctx = BuildAuthenticatedClientAndSetupResponse(BogusJson);

            var gate = new ManualResetEvent(false);
            ctx.News.BeginListNewsHeadlines("UK", 14, ar =>
                {
                    try
                    {
                        ctx.News.EndListNewsHeadlines(ar);
                        Assert.Fail("expected exception");
                    }
                    catch (Exception ex)
                    {
                        Assert.IsInstanceOf(typeof(CIAPI.Rpc.ServerConnectionException), ex, "expected ServerConnectionException but got " + ex.GetType().Name);
                    }
                    finally
                    {
                        gate.Set();
                    }
                }, null);

            gate.WaitOne(TimeSpan.FromSeconds(3));
        }

        [Test]
        public void CanGetNewsHeadlinesAsync()
        {
            Console.WriteLine("CanGetNewsHeadlinesAsync");

            CIAPI.Rpc.Client ctx = BuildAuthenticatedClientAndSetupResponse(NewsHeadlines14);

            var gate = new ManualResetEvent(false);
            ctx.News.BeginListNewsHeadlines("UK", 14, ar =>
                {
                    ListNewsHeadlinesResponseDTO response = ctx.News.EndListNewsHeadlines(ar);
                    Assert.AreEqual(14, response.Headlines.Length);
                    gate.Set();
                }, null);

            gate.WaitOne(TimeSpan.FromSeconds(3));
        }


        [Test]
        public void SpecificRequestExceptionsAreRetriedTheCorrectNumberOfTimes()
        {
            Console.WriteLine("SpecificRequestExceptionsAreRetriedTheCorrectNumberOfTimes");

            var gate = new ManualResetEvent(false);


            const int EXPECTED_RETRY_COUNT = 2;

            IRequestController controller;
            CIAPI.Rpc.Client ctx = BuildAuthenticatedClientAndSetupResponse("");


            ((TestRequestFactory)ctx.RequestController.RequestFactory).CreateTestRequest(NewsHeadlines14, TimeSpan.FromMilliseconds(300), null, null,
                                             new WebException("(500) internal server error"));

            Exception exception = null;

            ctx.News.BeginListNewsHeadlines("UK", 14, ar =>
                                                     {
                                                         try
                                                         {
                                                             var response = ctx.News.EndListNewsHeadlines(ar);
                                                         }
                                                         catch (Exception ex)
                                                         {
                                                             exception = ex;
                                                         }
                                                         finally
                                                         {
                                                             gate.Set();
                                                         }
                                                     }, null);
            gate.WaitOne(TimeSpan.FromSeconds(30));
            Assert.IsNotNull(exception, "expected exception, got none");
            Assert.IsTrue(exception.Message.Contains(string.Format("(500) internal server error\r\nretried {0} times", EXPECTED_RETRY_COUNT)), "error message incorrect. got " + exception.Message);
        }

        [Test]
        public void ShouldThrowExceptionIfRequestTimesOut()
        {

            Client ctx = BuildClientAndSetupResponse("");

            ((TestRequestFactory)ctx.RequestController.RequestFactory).CreateTestRequest(LoggedIn, TimeSpan.FromSeconds(300));

            Assert.Throws<ApiException>(() => ctx.LogIn("foo", "bar"));



        }


        [Test]
        public void NonRetryableExceptionFailsInsteadOfRetrying()
        {
            Console.WriteLine("NonRetryableExceptionFailsInsteadOfRetrying");

            var ctx = BuildClientAndSetupResponse("");
            ((TestRequestFactory)ctx.RequestController.RequestFactory).CreateTestRequest("", TimeSpan.FromMilliseconds(300), null, null, new WebException("(401) Unauthorized"));

            Assert.Throws<ApiException>(() => ctx.LogIn("foo", "bar"));
        }

        #region Plumbing

        private CIAPI.Rpc.Client BuildAuthenticatedClientAndSetupResponse(string expectedJson)
        {
            CIAPI.Rpc.Client ctx = BuildClientAndSetupResponse(expectedJson);

            ctx.UserName = TestConfig.ApiUsername;
            ctx.Session = TestConfig.ApiTestSessionId;

            return ctx;
        }
        private CIAPI.Rpc.Client BuildClientAndSetupResponse(string expectedJson)
        {

            TestRequestFactory factory = new TestRequestFactory();
            var requestController = new RequestController(TimeSpan.FromSeconds(0), 2, factory, new ErrorResponseDTOJsonExceptionFactory(), new ThrottedRequestQueue(TimeSpan.FromSeconds(5), 30, 10, "data"), new ThrottedRequestQueue(TimeSpan.FromSeconds(3), 1, 3, "trading"));

            var ctx = new CIAPI.Rpc.Client(new Uri(TestConfig.RpcUrl), requestController);
            factory.CreateTestRequest(expectedJson);
            return ctx;
        }
        #endregion
    }
}
