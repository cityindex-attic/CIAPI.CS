using System;
using System.Net;
using System.Text;
using System.Threading;
using CassiniDev;
using CIAPI.DTO;
using CIAPI.Rpc;
using CIAPI.Streaming.Testing;
using Newtonsoft.Json;
using NUnit.Framework;
using Salient.ReflectiveLoggingAdapter;
using Salient.ReliableHttpClient;


namespace CIAPI.Tests.Rpc
{
    [TestFixture]
    public class CassiniDevApiContextTests
    {
        static CassiniDevApiContextTests()
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



        //[Test]
        //public void LoginUsingSessionShouldValidateSession()
        //{


        //    Client rpcClient = new Client(new Uri(NormalizeUrl("/")), new Uri(NormalizeUrl("/")), "foo");
        //    EventHandler<RequestInfoArgs> mockingHandler = (i, e) =>
        //    {
        //        e.Continue = false;
        //        e.Response = LoggedIn;

        //    };

        //    Server.ProcessRequest += mockingHandler;



        //     rpcClient.LogIn("foo", "foo");

        //    Assert.That(rpcClient.Session, Is.Not.Empty);



        //    //This should work
        //    Client rpcClientUsingSession = BuildTestClientExtracted( requestFactory,  streamingFactory);


        //    requestFactory.PrepareResponse = testRequest =>
        //    {
        //           testRequest.SetResponseStream("{\"LogonUserName\":\"Sky Sanders - Test\",\"ClientAccountId\":400188637,\"ClientAccountCurrency\":\"USD\",\"AccountOperatorId\":4000,\"TradingAccounts\":[{\"TradingAccountId\":400282314,\"TradingAccountCode\":\"DM696495\",\"TradingAccountStatus\":\"Open\",\"TradingAccountType\":\"CFD\"},{\"TradingAccountId\":400282315,\"TradingAccountCode\":\"DM667890\",\"TradingAccountStatus\":\"Open\",\"TradingAccountType\":\"Spread Betting\"}],\"PersonalEmailAddress\":\"sky.sanders@gmail.com\",\"HasMultipleEmailAddresses\":false}");
        //    };

        //    rpcClientUsingSession.LogInUsingSession("foo", rpcClient.Session);

        //    Assert.That(rpcClientUsingSession.Session, Is.Not.Null.Or.Empty);
        //    requestFactory.PrepareResponse = testRequest =>
        //    {

        //        testRequest.SetResponseStream(LoggedOut);
        //    };
        //    //After the session has been destroyed, trying to login using it should fail
        //    rpcClient.LogOut();

        //    requestFactory.PrepareResponse = testRequest =>
        //    {

        //        testRequest.SetResponseStream(AuthError);
        //    };

        //    Assert.Throws<ReliableHttpException>(() => rpcClientUsingSession.LogInUsingSession("foo", rpcClient.Session));

        //    //And there shouldn't be a session
        //    Assert.IsNullOrEmpty(rpcClientUsingSession.Session);
        //    requestFactory.PrepareResponse = testRequest =>
        //    {

        //        testRequest.SetResponseStream(LoggedOut);
        //    };

        //    rpcClientUsingSession.LogOut();

        //    Server.ProcessRequest -= mockingHandler;

        //    rpcClientUsingSession.Dispose();
        //    rpcClient.Dispose();
        //}


        [Test]
        public void CanLogin()
        {
            Console.WriteLine("CanLogin");

            var server = new CassiniDevServer();
            server.StartServer(Environment.CurrentDirectory);

            var ctx = new Client(new Uri(server.NormalizeUrl("/")), new Uri(server.NormalizeUrl("/")), "foo");


            server.Server.ProcessRequest += (i, e) =>
            {
                e.Continue = false;
                e.Response = LoggedIn;
                e.ResponseStatus = 200;

            };


            try
            {
                ctx.LogIn(TestConfig.ApiUsername, TestConfig.ApiPassword);
            }
            finally
            {
                server.Dispose();
            }



            Assert.IsNotNullOrEmpty(ctx.Session);

        }

        [Test]
        public void ApiAuthenticationFailure()
        {
            Console.WriteLine("ApiAuthenticationFailure");

            var server = new CassiniDevServer();
            server.StartServer(Environment.CurrentDirectory);

            var errorDto = new ApiErrorResponseDTO
                               {
                                   ErrorCode = (int)ErrorCode.InvalidCredentials,
                                   ErrorMessage = "InvalidCredentials"
                               };



            var ctx = new Client(new Uri(server.NormalizeUrl("/")), new Uri(server.NormalizeUrl("/")), "foo")
                          {
                              UserName = "foo",
                              Session = "123"
                          }; // authenticated

            server.Server.ProcessRequest += (i, e) =>
            {
                e.Continue = false;
                e.Response = JsonConvert.SerializeObject(errorDto);

            };


            Exception ex = null;
            try
            {
                ctx.LogIn("foo", "bar");
                Assert.Fail("Expected exception");
            }
            catch (Exception ex2)
            {
                ex = ex2;
            }
            finally
            {
                server.Dispose();
            }



            if (!(ex is ReliableHttpException))
            {
                Assert.Fail("Expected ReliableHttpException, got " + ex.GetType().FullName);
            }

            Assert.AreEqual("InvalidCredentials", ex.Message, "FIXME: the API is just setting 401. it needs to send ErrorResponseDTO json as well.");
            Assert.AreEqual("{\"HttpStatus\":0,\"ErrorMessage\":\"InvalidCredentials\",\"ErrorCode\":4010}", ((ReliableHttpException)ex).ResponseText);

        }

        [Test]
        public void CanGetNewsHeadlines()
        {
            Console.WriteLine("CanGetNewsHeadlines");

            var server = new CassiniDevServer();
            server.StartServer(Environment.CurrentDirectory);

            var ctx = new Client(new Uri(server.NormalizeUrl("/")), new Uri(server.NormalizeUrl("/")), "foo")
                          {
                              UserName = "foo",
                              Session = "123"
                          };

            server.Server.ProcessRequest += (i, e) =>
            {
                e.Continue = false;
                e.Response = NewsHeadlines12;

            };


            ListNewsHeadlinesResponseDTO response = null;
            try
            {

                response = ctx.News.ListNewsHeadlinesWithSource("dj", "UK", 12);
            }
            finally
            {
                server.Dispose();
            }

            Assert.AreEqual(12, response.Headlines.Length);


        }


        [Test]
        public void CanGetNewsHeadlinesAsync()
        {
            Console.WriteLine("CanGetNewsHeadlinesAsync");

            var server = new CassiniDevServer();
            server.StartServer(Environment.CurrentDirectory);

            var ctx = new Client(new Uri(server.NormalizeUrl("/")), new Uri(server.NormalizeUrl("/")), "foo")
                          {
                              UserName = "foo",
                              Session = "123"
                          };


            server.Server.ProcessRequest += (i, e) =>
            {
                e.Continue = false;
                e.Response = NewsHeadlines14;

            };



            var gate = new ManualResetEvent(false);

            ListNewsHeadlinesResponseDTO response = null;
            Exception ex = null;
            ctx.News.BeginListNewsHeadlinesWithSource("dj", "UK", 14, ar =>
                                                                          {
                                                                              try
                                                                              {
                                                                                  response = ctx.News.EndListNewsHeadlinesWithSource(ar);
                                                                              }
                                                                              catch (Exception ex2)
                                                                              {
                                                                                  ex = ex2;
                                                                              }
                                                                              finally
                                                                              {
                                                                                  gate.Set();
                                                                              }

                                                                          }, null);

            gate.WaitOne();

            server.Dispose();

            Assert.AreEqual(14, response.Headlines.Length);

        }



        [Test]
        public void CanLogout()
        {
            Console.WriteLine("CanLogout");
            var server = new CassiniDevServer();
            server.StartServer(Environment.CurrentDirectory);

            var ctx = new Client(new Uri(server.NormalizeUrl("/")), new Uri(server.NormalizeUrl("/")), "foo")
                          {
                              UserName = "foo",
                              Session = "123"
                          };


            server.Server.ProcessRequest += (i, e) =>
            {
                e.Continue = false;
                e.Response = LoggedOut;

            };


            bool response;
            try
            {
                response = ctx.LogOut();
            }
            finally
            {
                server.Dispose();
            }

            Assert.IsTrue(response);

        }

        [Test, ExpectedException(typeof(InvalidCredentialsException), ExpectedMessage = "InvalidCredentials")]
        public void CanRecognize200JsonException()
        {
            var server = new CassiniDevServer();
            server.StartServer(Environment.CurrentDirectory);

            var ctx = new Client(new Uri(server.NormalizeUrl("/")), new Uri(server.NormalizeUrl("/")), "foo")
            {
                UserName = "foo",
                Session = "123"
            };


            var errorDto = new ApiErrorResponseDTO
                               {
                                   ErrorCode = (int)ErrorCode.InvalidCredentials,
                                   ErrorMessage = "InvalidCredentials"
                               };


            string jsonConvertSerializeObject = JsonConvert.SerializeObject(errorDto);

            server.Server.ProcessRequest += (i, e) =>
            {
                e.Continue = false;

                e.Response = jsonConvertSerializeObject;

            };

            ApiLogOnResponseDTO response = null;
            try
            {

                response = ctx.LogIn(TestConfig.ApiUsername, TestConfig.ApiPassword);
            }
            finally
            {
                server.Dispose();
            }

            Assert.IsNotNull(response);
        }

        [Test]
        public void DeserializationExceptionIsProperlySurfacedByAsyncRequests()
        {
            Console.WriteLine("DeserializationExceptionIsProperlySurfacedByAsyncRequests");

            var server = new CassiniDevServer();
            server.StartServer(Environment.CurrentDirectory);


            var ctx = new Client(new Uri(server.NormalizeUrl("/")), new Uri(server.NormalizeUrl("/")), "foo")
                          {
                              UserName = "foo",
                              Session = "123"
                          };


            server.Server.ProcessRequest += (i, e) =>
            {
                e.Continue = false;
                e.Response = BogusJson;

            };




            var gate = new ManualResetEvent(false);
            Exception exception = null;
            ctx.News.BeginListNewsHeadlinesWithSource("dj", "UK", 14, ar =>
                                                                          {
                                                                              try
                                                                              {
                                                                                  ctx.News.EndListNewsHeadlinesWithSource(ar);
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

            gate.WaitOne();

            server.Dispose();
            Assert.IsNotNull(exception, "Expected an exception");
            Assert.IsInstanceOf(typeof(ServerConnectionException), exception, "expected ServerConnectionException but got " + exception.GetType().Name);
        }

        [Test]
        public void DeserializationExceptionIsProperlySurfacedBySyncRequests()
        {
            Console.WriteLine("DeserializationExceptionIsProperlySurfacedBySyncRequests");
            var server = new CassiniDevServer();
            server.StartServer(Environment.CurrentDirectory);

            var ctx = new Client(new Uri(server.NormalizeUrl("/")), new Uri(server.NormalizeUrl("/")), "foo")
            {
                UserName = "foo",
                Session = "123"
            };

            EventHandler<RequestInfoArgs> mockingHandler = (i, e) =>
            {
                e.Continue = false;
                e.Response = BogusJson;

            };

            server.Server.ProcessRequest += mockingHandler;
            Exception ex = null;
            try
            {
                ctx.News.GetNewsDetail("dj", "foobar");
            }
            catch (Exception ex2)
            {
                ex = ex2;
            }

            finally
            {
                server.Dispose();
            }

            Assert.IsNotNull(ex, "Expected and exception");

            Assert.IsInstanceOf(typeof(ServerConnectionException), ex);
        }


        [Test]
        public void NonRetryableExceptionFailsInsteadOfRetrying()
        {
            Console.WriteLine("NonRetryableExceptionFailsInsteadOfRetrying");
            var server = new CassiniDevServer();
            server.StartServer(Environment.CurrentDirectory);

            Client ctx = new Client(new Uri(server.NormalizeUrl("/")), new Uri(server.NormalizeUrl("/")), "foo");

            string jsonConvertSerializeObject = JsonConvert.SerializeObject(new ApiErrorResponseDTO() { ErrorCode = 403 });


            EventHandler<RequestInfoArgs> mockingHandler = (i, e) =>
            {
                e.Continue = false;

                e.Response = jsonConvertSerializeObject;
                e.ResponseStatus = 403;

            };

            server.Server.ProcessRequest += mockingHandler;





            try
            {
                Assert.Throws<ForbiddenException>(() => ctx.LogIn("foo", "bar"));
            }
            finally
            {
                server.Dispose();
                Thread.Sleep(5000);
            }


        }

        [Test, Ignore("need to examine timeout functionality of both TestWebRequest and HttpWebRequest")]
        public void ShouldThrowExceptionIfRequestTimesOut()
        {

            var server = new CassiniDevServer();
            server.StartServer(Environment.CurrentDirectory);

            Client ctx = new Client(new Uri(server.NormalizeUrl("/")), new Uri(server.NormalizeUrl("/")), "foo");


            EventHandler<RequestInfoArgs> mockingHandler = (i, e) =>
            {
                e.Continue = false;
                e.Response = "";
                Thread.Sleep(TimeSpan.FromSeconds(40));

            };

            Assert.Throws<ReliableHttpException>(() => ctx.LogIn("foo", "bar"));

            server.Server.ProcessRequest += mockingHandler;
            Thread.Sleep(5000);

        }

        [Test, Ignore("FIXME")]
        public void SpecificRequestExceptionsAreRetriedTheCorrectNumberOfTimes()
        {
            Console.WriteLine("SpecificRequestExceptionsAreRetriedTheCorrectNumberOfTimes");

            var server = new CassiniDevServer();
            server.StartServer(Environment.CurrentDirectory);

            var gate = new ManualResetEvent(false);


            const int EXPECTED_ATTEMPT_COUNT = 3;


            Client ctx = new Client(new Uri(server.NormalizeUrl("/")), new Uri(server.NormalizeUrl("/")), "foo"); // authenticated
            ctx.UserName = "foo";
            ctx.Session = "123";


            string jsonConvertSerializeObject = JsonConvert.SerializeObject(new ApiErrorResponseDTO() { HttpStatus = 500, ErrorMessage = "internal server error", ErrorCode = 500 });

            EventHandler<RequestInfoArgs> mockingHandler = (i, e) =>
            {
                e.Continue = false;

                e.Response = jsonConvertSerializeObject;
                e.ResponseStatus = 500;

            };

            server.Server.ProcessRequest += mockingHandler;



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

            server.Dispose();

            Thread.Sleep(5000);

            Assert.IsNotNull(exception, "expected exception, got none");
            Assert.IsTrue(
                exception.Message.Contains(string.Format("(500) internal server error - failed {0} times",
                                                         EXPECTED_ATTEMPT_COUNT)),
                "error message incorrect. got " + exception.Message);
        }
    }
}