using System;
using System.Net;
using System.Text;
using System.Threading;
using CIAPI.DTO;
using CIAPI.Rpc;
using CIAPI.Streaming.Testing;
using Newtonsoft.Json;
using NUnit.Framework;
using Salient.ReflectiveLoggingAdapter;
using Salient.ReliableHttpClient;
using Salient.ReliableHttpClient.Testing;

namespace CIAPI.Tests.Rpc
{
    [TestFixture]
    public class ApiContextTests : FixtureBase
    {
        static ApiContextTests()
        {
            LogManager.CreateInnerLogger =
                (logName, logLevel, showLevel, showDateTime, showLogName, dateTimeFormat) =>
                new SimpleTraceAppender(logName, logLevel, showLevel, showDateTime, showLogName, dateTimeFormat);
        }

        private const string NewsHeadlines12 =
            "{\"Headlines\":[{\"Headline\":\"(UK) Teenage girls often have babies fathered by men\",\"PublishDate\":\"\\/Date(1293727302736)\\/\",\"StoryId\":12654},{\"Headline\":\"(UK) Lung Cancer in Women Mushrooms\",\"PublishDate\":\"\\/Date(1293726702736)\\/\",\"StoryId\":12655},{\"Headline\":\"(UK) Include Your Children when Baking Cookies\",\"PublishDate\":\"\\/Date(1293726102736)\\/\",\"StoryId\":12656},{\"Headline\":\"(UK) Infertility unlikely to be passed on\",\"PublishDate\":\"\\/Date(1293725502736)\\/\",\"StoryId\":12657},{\"Headline\":\"(UK) Child's death ruins couple's holiday\",\"PublishDate\":\"\\/Date(1293724902736)\\/\",\"StoryId\":12658},{\"Headline\":\"(UK) Milk drinkers are turning to powder\",\"PublishDate\":\"\\/Date(1293724302736)\\/\",\"StoryId\":12659},{\"Headline\":\"(UK) Court Rules Boxer Shorts Are Indeed Underwear\",\"PublishDate\":\"\\/Date(1293723702736)\\/\",\"StoryId\":12660},{\"Headline\":\"(UK) Hospitals are Sued by 7 Foot Doctors\",\"PublishDate\":\"\\/Date(1293723102736)\\/\",\"StoryId\":12661},{\"Headline\":\"(UK) Lack of brains hinders research\",\"PublishDate\":\"\\/Date(1293722502736)\\/\",\"StoryId\":12662},{\"Headline\":\"(UK) New Vaccine May Contain Rabies\",\"PublishDate\":\"\\/Date(1293721902736)\\/\",\"StoryId\":12663},{\"Headline\":\"(UK) Two convicts evade noose, jury hung\",\"PublishDate\":\"\\/Date(1293721302736)\\/\",\"StoryId\":12664},{\"Headline\":\"(UK) Safety Experts Say School Bus Passengers Should Be Belted\",\"PublishDate\":\"\\/Date(1293720702736)\\/\",\"StoryId\":12665}]}";

        private const string NewsHeadlines14 =
            "{\"Headlines\":[{\"Headline\":\"(UK) Teenage girls often have babies fathered by men\",\"PublishDate\":\"\\/Date(1293727302736)\\/\",\"StoryId\":12654},{\"Headline\":\"(UK) Lung Cancer in Women Mushrooms\",\"PublishDate\":\"\\/Date(1293726702736)\\/\",\"StoryId\":12655},{\"Headline\":\"(UK) Include Your Children when Baking Cookies\",\"PublishDate\":\"\\/Date(1293726102736)\\/\",\"StoryId\":12656},{\"Headline\":\"(UK) Infertility unlikely to be passed on\",\"PublishDate\":\"\\/Date(1293725502736)\\/\",\"StoryId\":12657},{\"Headline\":\"(UK) Child's death ruins couple's holiday\",\"PublishDate\":\"\\/Date(1293724902736)\\/\",\"StoryId\":12658},{\"Headline\":\"(UK) Milk drinkers are turning to powder\",\"PublishDate\":\"\\/Date(1293724302736)\\/\",\"StoryId\":12659},{\"Headline\":\"(UK) Court Rules Boxer Shorts Are Indeed Underwear\",\"PublishDate\":\"\\/Date(1293723702736)\\/\",\"StoryId\":12660},{\"Headline\":\"(UK) Hospitals are Sued by 7 Foot Doctors\",\"PublishDate\":\"\\/Date(1293723102736)\\/\",\"StoryId\":12661},{\"Headline\":\"(UK) Lack of brains hinders research\",\"PublishDate\":\"\\/Date(1293722502736)\\/\",\"StoryId\":12662},{\"Headline\":\"(UK) New Vaccine May Contain Rabies\",\"PublishDate\":\"\\/Date(1293721902736)\\/\",\"StoryId\":12663},{\"Headline\":\"(UK) Two convicts evade noose, jury hung\",\"PublishDate\":\"\\/Date(1293721302736)\\/\",\"StoryId\":12664},{\"Headline\":\"(UK) Safety Experts Say School Bus Passengers Should Be Belted\",\"PublishDate\":\"\\/Date(1293720702736)\\/\",\"StoryId\":12665},{\"Headline\":\"(UK) Man Run Over by Freight Train Dies\",\"PublishDate\":\"\\/Date(1293720102736)\\/\",\"StoryId\":12666},{\"Headline\":\"(UK) Teenage girls often have babies fathered by men\",\"PublishDate\":\"\\/Date(1293727302736)\\/\",\"StoryId\":12654}]}";

        private const string BogusJson = "foo bar fu";
        public const string LoggedIn = "{\"Session\":\"D2FF3E4D-01EA-4741-86F0-437C919B5559\"}";
        private const string LoggedOut = "{\"LoggedOut\":true}";
        private const string AuthError = "{ \"ErrorMessage\": \"sample value\", \"ErrorCode\": 403 }";


        [Test, Ignore]
        public void LoginUsingSessionShouldValidateSession()
        {
            TestRequestFactory requestFactory;
            TestStreamingClientFactory streamingFactory;
            Client rpcClient = BuildTestClient(out requestFactory, out streamingFactory);
            requestFactory.PrepareResponse = testRequest =>
            {
       
                testRequest.SetResponseStream(LoggedIn);
            };

             rpcClient.LogIn("foo", "foo");

            Assert.That(rpcClient.Session, Is.Not.Empty);

            //This should work
            Client rpcClientUsingSession = BuildTestClientExtracted( requestFactory,  streamingFactory);
            requestFactory.PrepareResponse = testRequest =>
            {
                string jsonConvertSerializeObject =
                    JsonConvert.SerializeObject(new CIAPI.DTO.AccountInformationResponseDTO() { TradingAccounts = new ApiTradingAccountDTO[] { new ApiTradingAccountDTO() { TradingAccountType = "Spread Betting" }, new ApiTradingAccountDTO() { TradingAccountType = "CFD" }, } });
                testRequest.SetResponseStream("{\"LogonUserName\":\"Sky Sanders - Test\",\"ClientAccountId\":400188637,\"ClientAccountCurrency\":\"USD\",\"AccountOperatorId\":4000,\"TradingAccounts\":[{\"TradingAccountId\":400282314,\"TradingAccountCode\":\"DM603751\",\"TradingAccountStatus\":\"Open\",\"TradingAccountType\":\"CFD\"},{\"TradingAccountId\":400282315,\"TradingAccountCode\":\"DM667890\",\"TradingAccountStatus\":\"Open\",\"TradingAccountType\":\"Spread Betting\"}],\"PersonalEmailAddress\":\"sky.sanders@gmail.com\",\"HasMultipleEmailAddresses\":false}");
            };

            rpcClientUsingSession.LogInUsingSession("foo", rpcClient.Session);

            Assert.That(rpcClientUsingSession.Session, Is.Not.Null.Or.Empty);
            requestFactory.PrepareResponse = testRequest =>
            {

                testRequest.SetResponseStream(LoggedOut);
            };
            //After the session has been destroyed, trying to login using it should fail
            rpcClient.LogOut();

            requestFactory.PrepareResponse = testRequest =>
            {

                testRequest.SetResponseStream(AuthError);
            };

            Assert.Throws<ReliableHttpException>(() => rpcClientUsingSession.LogInUsingSession("foo", rpcClient.Session));

            //And there shouldn't be a session
            Assert.IsNullOrEmpty(rpcClientUsingSession.Session);
            requestFactory.PrepareResponse = testRequest =>
            {

                testRequest.SetResponseStream(LoggedOut);
            };
            rpcClientUsingSession.LogOut();
            rpcClientUsingSession.Dispose();
            rpcClient.Dispose();
        }


        [Test]
        public void ApiAuthenticationFailure()
        {
            var errorDto = new ApiErrorResponseDTO
                               {
                                   ErrorCode = (int) ErrorCode.InvalidCredentials,
                                   ErrorMessage = "InvalidCredentials"
                               };


            TestRequestFactory requestFactory;
            TestStreamingClientFactory streamingFactory;
            Client ctx = BuildAuthenticatedTestClient(out requestFactory, out streamingFactory);
            requestFactory.PrepareResponse = testRequest =>
                                                 {
                                                     string jsonConvertSerializeObject =
                                                         JsonConvert.SerializeObject(errorDto);
                                                     testRequest.ResponseStream =
                                                         new TestWebStream(
                                                             Encoding.UTF8.GetBytes(jsonConvertSerializeObject));
                                                 };

            try
            {
                ctx.LogIn("foo", "bar");
                Assert.Fail("Expected exception");
            }
            catch (ReliableHttpException ex)
            {
                Assert.AreEqual("InvalidCredentials", ex.Message,
                                "FIXME: the API is just setting 401. it needs to send ErrorResponseDTO json as well.");
                Assert.AreEqual("{\"HttpStatus\":0,\"ErrorMessage\":\"InvalidCredentials\",\"ErrorCode\":4010}",
                                ex.ResponseText);
            }
        }

        [Test]
        public void CanGetNewsHeadlines()
        {
            Console.WriteLine("CanGetNewsHeadlines");


            TestRequestFactory requestFactory;
            TestStreamingClientFactory streamingFactory;
            Client ctx = BuildAuthenticatedTestClient(out requestFactory, out streamingFactory);
            requestFactory.PrepareResponse =
                testRequest => { testRequest.ResponseStream = new TestWebStream(Encoding.UTF8.GetBytes(NewsHeadlines12)); };

            ListNewsHeadlinesResponseDTO response = ctx.News.ListNewsHeadlinesWithSource("dj", "UK", 12);
            Assert.AreEqual(12, response.Headlines.Length);
        }


        [Test]
        public void CanGetNewsHeadlinesAsync()
        {
            Console.WriteLine("CanGetNewsHeadlinesAsync");


            TestRequestFactory requestFactory;
            TestStreamingClientFactory streamingFactory;

            Client ctx = BuildAuthenticatedTestClient(out requestFactory, out streamingFactory);
            requestFactory.PrepareResponse =
                testRequest => { testRequest.ResponseStream = new TestWebStream(Encoding.UTF8.GetBytes(NewsHeadlines14)); };


            var gate = new ManualResetEvent(false);
            ctx.News.BeginListNewsHeadlinesWithSource("dj", "UK", 14, ar =>
                                                                          {
                                                                              ListNewsHeadlinesResponseDTO response =
                                                                                  ctx.News.
                                                                                      EndListNewsHeadlinesWithSource(ar);
                                                                              Assert.AreEqual(14,
                                                                                              response.Headlines.Length);
                                                                              gate.Set();
                                                                          }, null);

            gate.WaitOne(TimeSpan.FromSeconds(3));
        }

        [Test]
        public void CanLogin()
        {
            Console.WriteLine("CanLogin");


            TestRequestFactory requestFactory;
            TestStreamingClientFactory streamingFactory;
            Client ctx = BuildTestClient(out requestFactory, out streamingFactory);
            requestFactory.PrepareResponse =
                testRequest => { testRequest.ResponseStream = new TestWebStream(Encoding.UTF8.GetBytes(LoggedIn)); };


            ctx.LogIn(TestConfig.ApiUsername, TestConfig.ApiPassword);

            Assert.IsNotNullOrEmpty(ctx.Session);
        }

        [Test]
        public void CanLogout()
        {
            Console.WriteLine("CanLogout");


            TestRequestFactory requestFactory;
            TestStreamingClientFactory IStreamingClientFactory;
            Client ctx = BuildAuthenticatedTestClient(out requestFactory, out IStreamingClientFactory);
            requestFactory.PrepareResponse =
                testRequest => { testRequest.ResponseStream = new TestWebStream(Encoding.UTF8.GetBytes(LoggedOut)); };

            bool response = ctx.LogOut();
            Assert.IsTrue(response);
        }

        [Test, ExpectedException(typeof (InvalidCredentialsException), ExpectedMessage = "InvalidCredentials")]
        public void CanRecognize200JsonException()
        {
            var errorDto = new ApiErrorResponseDTO
                               {
                                   ErrorCode = (int) ErrorCode.InvalidCredentials,
                                   ErrorMessage = "InvalidCredentials"
                               };

            TestRequestFactory requestFactory;
            TestStreamingClientFactory streamingFactory;
            Client ctx = BuildAuthenticatedTestClient(out requestFactory, out streamingFactory);
            requestFactory.PrepareResponse =
                testRequest =>
                    {
                        testRequest.ResponseStream =
                            new TestWebStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(errorDto)));
                    };

            ctx.LogIn(TestConfig.ApiUsername, TestConfig.ApiPassword);
        }

        [Test]
        public void DeserializationExceptionIsProperlySurfacedByAsyncRequests()
        {
            Console.WriteLine("DeserializationExceptionIsProperlySurfacedByAsyncRequests");


            TestRequestFactory requestFactory;
            TestStreamingClientFactory streamingFactory;

            Client ctx = BuildAuthenticatedTestClient(out requestFactory, out streamingFactory);
            requestFactory.PrepareResponse =
                testRequest => { testRequest.ResponseStream = new TestWebStream(Encoding.UTF8.GetBytes(BogusJson)); };


            var gate = new ManualResetEvent(false);
            Exception exception = null;
            ctx.News.BeginListNewsHeadlinesWithSource("dj", "UK", 14, ar =>
                                                                          {
                                                                              try
                                                                              {
                                                                                  ctx.News.
                                                                                      EndListNewsHeadlinesWithSource(ar);
                                                                                  Assert.Fail("expected exception");
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

            gate.WaitOne(TimeSpan.FromSeconds(3));
            Assert.IsInstanceOf(typeof (ServerConnectionException), exception,
                                "expected ServerConnectionException but got " + exception.GetType().Name);
        }

        [Test]
        public void DeserializationExceptionIsProperlySurfacedBySyncRequests()
        {
            Console.WriteLine("DeserializationExceptionIsProperlySurfacedBySyncRequests");


            TestRequestFactory requestFactory;
            TestStreamingClientFactory streamingFactory;
            Client ctx = BuildAuthenticatedTestClient(out requestFactory, out streamingFactory);
            requestFactory.PrepareResponse =
                testRequest => { testRequest.ResponseStream = new TestWebStream(Encoding.UTF8.GetBytes(BogusJson)); };
            Assert.Throws<ServerConnectionException>(() => ctx.News.GetNewsDetail("dj", "foobar"));
        }


        [Test]
        public void NonRetryableExceptionFailsInsteadOfRetrying()
        {
            Console.WriteLine("NonRetryableExceptionFailsInsteadOfRetrying");
            TestRequestFactory requestFactory;
            TestStreamingClientFactory streamingFactory;

            Client ctx = BuildTestClient(out requestFactory, out streamingFactory);
            requestFactory.PrepareResponse =
                testRequest => { testRequest.EndGetResponseException = new WebException("(401) Unauthorized"); };


            Assert.Throws<ReliableHttpException>(() => ctx.LogIn("foo", "bar"));
        }

        [Test, Ignore("need to examine timeout functionality of both TestWebRequest and HttpWebRequest")]
        public void ShouldThrowExceptionIfRequestTimesOut()  
        {
            TestRequestFactory requestFactory;
            TestStreamingClientFactory streamingFactory;

            Client ctx = BuildTestClient(out requestFactory, out streamingFactory);
            requestFactory.PrepareResponse = testRequest => { testRequest.Latency = TimeSpan.FromSeconds(300); };


            Assert.Throws<ReliableHttpException>(() => ctx.LogIn("foo", "bar"));
        }

        [Test]
        public void SpecificRequestExceptionsAreRetriedTheCorrectNumberOfTimes()
        {
            Console.WriteLine("SpecificRequestExceptionsAreRetriedTheCorrectNumberOfTimes");

            var gate = new ManualResetEvent(false);


            const int EXPECTED_ATTEMPT_COUNT = 3;
            TestRequestFactory requestFactory;
            TestStreamingClientFactory streamingFactory;

            Client ctx = BuildAuthenticatedTestClient(out requestFactory, out streamingFactory);
            requestFactory.PrepareResponse =
                testRequest => { testRequest.EndGetResponseException = new WebException("(500) internal server error"); };

            Exception exception = null;

            ctx.News.BeginListNewsHeadlinesWithSource("dj", "UK", 14, ar =>
                                                                          {
                                                                              try
                                                                              {
                                                                                  ListNewsHeadlinesResponseDTO response
                                                                                      =
                                                                                      ctx.News.
                                                                                          EndListNewsHeadlinesWithSource
                                                                                          (ar);
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
            Assert.IsTrue(
                exception.Message.Contains(string.Format("(500) internal server error - failed {0} times",
                                                         EXPECTED_ATTEMPT_COUNT)),
                "error message incorrect. got " + exception.Message);
        }
    }
}