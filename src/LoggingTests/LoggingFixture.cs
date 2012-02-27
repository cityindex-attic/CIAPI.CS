using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using CIAPI.Rpc;
using CIAPI.Streaming;
using CIAPI.Tests;
using NUnit.Framework;
using Salient.JsonClient;
using Salient.JsonClient.Tests;
using Salient.ReflectiveLoggingAdapter;

namespace LoggingTests
{



    [TestFixture]
    public class LoggingFixture
    {
        private const string NewsHeadlines12 = "{\"Headlines\":[{\"Headline\":\"(UK) Teenage girls often have babies fathered by men\",\"PublishDate\":\"\\/Date(1293727302736)\\/\",\"StoryId\":12654},{\"Headline\":\"(UK) Lung Cancer in Women Mushrooms\",\"PublishDate\":\"\\/Date(1293726702736)\\/\",\"StoryId\":12655},{\"Headline\":\"(UK) Include Your Children when Baking Cookies\",\"PublishDate\":\"\\/Date(1293726102736)\\/\",\"StoryId\":12656},{\"Headline\":\"(UK) Infertility unlikely to be passed on\",\"PublishDate\":\"\\/Date(1293725502736)\\/\",\"StoryId\":12657},{\"Headline\":\"(UK) Child's death ruins couple's holiday\",\"PublishDate\":\"\\/Date(1293724902736)\\/\",\"StoryId\":12658},{\"Headline\":\"(UK) Milk drinkers are turning to powder\",\"PublishDate\":\"\\/Date(1293724302736)\\/\",\"StoryId\":12659},{\"Headline\":\"(UK) Court Rules Boxer Shorts Are Indeed Underwear\",\"PublishDate\":\"\\/Date(1293723702736)\\/\",\"StoryId\":12660},{\"Headline\":\"(UK) Hospitals are Sued by 7 Foot Doctors\",\"PublishDate\":\"\\/Date(1293723102736)\\/\",\"StoryId\":12661},{\"Headline\":\"(UK) Lack of brains hinders research\",\"PublishDate\":\"\\/Date(1293722502736)\\/\",\"StoryId\":12662},{\"Headline\":\"(UK) New Vaccine May Contain Rabies\",\"PublishDate\":\"\\/Date(1293721902736)\\/\",\"StoryId\":12663},{\"Headline\":\"(UK) Two convicts evade noose, jury hung\",\"PublishDate\":\"\\/Date(1293721302736)\\/\",\"StoryId\":12664},{\"Headline\":\"(UK) Safety Experts Say School Bus Passengers Should Be Belted\",\"PublishDate\":\"\\/Date(1293720702736)\\/\",\"StoryId\":12665}]}";
        private const string NewsHeadlines14 = "{\"Headlines\":[{\"Headline\":\"(UK) Teenage girls often have babies fathered by men\",\"PublishDate\":\"\\/Date(1293727302736)\\/\",\"StoryId\":12654},{\"Headline\":\"(UK) Lung Cancer in Women Mushrooms\",\"PublishDate\":\"\\/Date(1293726702736)\\/\",\"StoryId\":12655},{\"Headline\":\"(UK) Include Your Children when Baking Cookies\",\"PublishDate\":\"\\/Date(1293726102736)\\/\",\"StoryId\":12656},{\"Headline\":\"(UK) Infertility unlikely to be passed on\",\"PublishDate\":\"\\/Date(1293725502736)\\/\",\"StoryId\":12657},{\"Headline\":\"(UK) Child's death ruins couple's holiday\",\"PublishDate\":\"\\/Date(1293724902736)\\/\",\"StoryId\":12658},{\"Headline\":\"(UK) Milk drinkers are turning to powder\",\"PublishDate\":\"\\/Date(1293724302736)\\/\",\"StoryId\":12659},{\"Headline\":\"(UK) Court Rules Boxer Shorts Are Indeed Underwear\",\"PublishDate\":\"\\/Date(1293723702736)\\/\",\"StoryId\":12660},{\"Headline\":\"(UK) Hospitals are Sued by 7 Foot Doctors\",\"PublishDate\":\"\\/Date(1293723102736)\\/\",\"StoryId\":12661},{\"Headline\":\"(UK) Lack of brains hinders research\",\"PublishDate\":\"\\/Date(1293722502736)\\/\",\"StoryId\":12662},{\"Headline\":\"(UK) New Vaccine May Contain Rabies\",\"PublishDate\":\"\\/Date(1293721902736)\\/\",\"StoryId\":12663},{\"Headline\":\"(UK) Two convicts evade noose, jury hung\",\"PublishDate\":\"\\/Date(1293721302736)\\/\",\"StoryId\":12664},{\"Headline\":\"(UK) Safety Experts Say School Bus Passengers Should Be Belted\",\"PublishDate\":\"\\/Date(1293720702736)\\/\",\"StoryId\":12665},{\"Headline\":\"(UK) Man Run Over by Freight Train Dies\",\"PublishDate\":\"\\/Date(1293720102736)\\/\",\"StoryId\":12666},{\"Headline\":\"(UK) Teenage girls often have babies fathered by men\",\"PublishDate\":\"\\/Date(1293727302736)\\/\",\"StoryId\":12654}]}";
        private const string BogusJson = "foo bar fu";
        private const string LoggedIn = "{\"Session\":\"D2FF3E4D-01EA-4741-86F0-437C919B5559\"}";
        private const string LoggedOut = "{\"LoggedOut\":true}";
        private const string AuthError = "{ \"ErrorMessage\": \"sample value\", \"ErrorCode\": 403 }";


        static Dictionary<string, CapturingAppender> loggers;
        static LoggingFixture()
        {
            loggers = new Dictionary<string, CapturingAppender>();
            LogManager.CreateInnerLogger = (logName, logLevel, showLevel, showDateTime, showLogName, dateTimeFormat) =>
            {
                // create external logger implementation and return instance.
                // this will be called whenever CIAPI requires a logger
                var l = new CapturingAppender(logName, logLevel, showLevel, showDateTime, showLogName,
                                                       dateTimeFormat);
                loggers.Add(logName, l);
                return l;
            };

        }
        [Test]
        public void EnsureStreamingInnerLoggerIsCalled()
        {




            //Sky TODO - test the mockInnerLogger gets called by the RPC client and the StreamingClient

            var client = StreamingClientFactory.CreateStreamingClient(new Uri("http://a.server.com/"), "username",
                                                                      "sessionId");

            try
            {
                client.BuildNewsHeadlinesListener("BOGUS.TOPIC");
                Assert.Fail("should have thrown");
            }
            catch 
            {
                
                // swallow
            }

            new AutoResetEvent(false).WaitOne(10000);
            // this is a unit test, not integration. need to figure out how to get the ls lib to do something.
            // maybe just a bad uri?

            var lsLogger = loggers["CIAPI.Streaming.Lightstreamer.LightstreamerClient"];
            var actionsLogger = loggers["com.lightstreamer.ls_client.actions"];
            var streamLogger = loggers["com.lightstreamer.ls_client.stream"];
            var sessionLogger = loggers["com.lightstreamer.ls_client.session"];
            var protocolLogger = loggers["com.lightstreamer.ls_client.protocol"];

            
            Assert.Greater(lsLogger.GetItems().ToArray().Length, 0);
            Assert.Greater(actionsLogger.GetItems().ToArray().Length, 0);
            Assert.Greater(streamLogger.GetItems().ToArray().Length, 0);
            Assert.Greater(sessionLogger.GetItems().ToArray().Length, 0);
            Assert.Greater(protocolLogger.GetItems().ToArray().Length, 0);
   

            

            

        }

        [Test]
        public void EnsureRpcInnerLoggerIsCalled()
        {



            var ctx = BuildAuthenticatedClientAndSetupResponse(LoggedIn);

            ctx.LogIn(TestConfig.ApiUsername, TestConfig.ApiPassword);

            new AutoResetEvent(false).WaitOne(10000);
            // this is a unit test, not integration. need to figure out how to get the ls lib to do something.
            // maybe just a bad uri?


            var RequestFactoryLogger = loggers["Salient.JsonClient.RequestFactory"];
            var ThrottledRequestQueueLogger = loggers["Salient.JsonClient.ThrottledRequestQueue"];
            var RequestControllerLogger = loggers["Salient.JsonClient.RequestController"];
            var RequestCacheLogger = loggers["Salient.JsonClient.RequestCache"];
            var ClientLogger = loggers["CIAPI.Rpc.Client"];
            var JsonClientLogger = loggers["Salient.JsonClient.Client"];



            Assert.Greater(RequestFactoryLogger.GetItems().ToArray().Length, 0);
            Assert.Greater(ThrottledRequestQueueLogger.GetItems().ToArray().Length, 0);
            Assert.Greater(RequestControllerLogger.GetItems().ToArray().Length, 0);
            Assert.Greater(RequestCacheLogger.GetItems().ToArray().Length, 0);
            Assert.Greater(ClientLogger.GetItems().ToArray().Length, 0);
            Assert.Greater(JsonClientLogger.GetItems().ToArray().Length, 0);
            


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
            var requestController = new RequestController(TimeSpan.FromSeconds(0), 2, factory, new ErrorResponseDTOJsonExceptionFactory(), new ThrottledRequestQueue(TimeSpan.FromSeconds(5), 30, 10, "data"), new ThrottledRequestQueue(TimeSpan.FromSeconds(3), 1, 3, "trading"));

            var ctx = new CIAPI.Rpc.Client(new Uri(TestConfig.RpcUrl), "mockApiKey", requestController);
            factory.CreateTestRequest(expectedJson);
            return ctx;
        }
        #endregion
    }
}