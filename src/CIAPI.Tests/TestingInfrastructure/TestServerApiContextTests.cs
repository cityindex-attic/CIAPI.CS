using System;
using System.Net;
using System.Threading;
using CIAPI.DTO;
using CIAPI.Rpc;
using Newtonsoft.Json;
using NUnit.Framework;
using Salient.ReflectiveLoggingAdapter;

namespace CIAPI.Tests.TestingInfrastructure
{
    [TestFixture]
    public class TestServerApiContextTests
    {
        static TestServerApiContextTests()
        {
            //LogManager.CreateInnerLogger =
            //    (logName, logLevel, showLevel, showDateTime, showLogName, dateTimeFormat) =>
            //    new SimpleTraceAppender(logName, logLevel, showLevel, showDateTime, showLogName, dateTimeFormat);
           
        }
 
        [Test]
        public void CanLogin()
        {
    
            var server = new TestServer();
            server.Start();

            
            
            server.ProcessRequest += (s, e) =>
                                         {
                                             var dto = new ApiLogOnResponseDTO
                                                           {
                                                               AllowedAccountOperator = true,
                                                               PasswordChangeRequired = false,
                                                               Session =
                                                                   "86c6b0df-24d4-4b3f-b699-688626817599"
                                                           };
                                             string json = JsonConvert.SerializeObject(dto);
                                             e.Response = TestServer.CreateRpcResponse(json);
                                         };
            try
            {

                var ctx = new Client(new Uri("http://localhost.:" + server.Port), new Uri("http://localhost.:" + server.Port), "foo");

                ctx.LogIn(Settings.RpcUserName, Settings.RpcPassword);
                
                Assert.IsNotNullOrEmpty(ctx.Session);
                ctx.Dispose();

            }
            finally
            {
                server.Stop();
            }



            

        }

        [Test]
        public void CanStream()
        {
            var server = new TestServer();
            server.Start();



            server.ProcessRequest += (s, e) =>
                                         {
                                             const string defaultPricesSessionid = "S8f295c055c413e4bT4448618";

                                             switch (e.Request.Route)
                {
                    // Streaming requests




                    case "/lightstreamer/create_session.txt":
                        // build up a session for adapter set

                        string response = string.Format(@"OK
SessionId:{0}
ControlAddress:localhost.
KeepaliveMillis:30000
MaxBandwidth:0.0
RequestLimit:50000
ServerName:Lightstreamer HTTP Server

PROBE
LOOP
", defaultPricesSessionid);


                        e.Response = TestServer.CreateLightStreamerResponse(response);
                        break;


                    case "/lightstreamer/control.txt":
                        // this is where we associate topics (tables) to a session

                        e.Response = TestServer.CreateLightStreamerResponse(@"OK
");
                        
                        break;
                    case "/lightstreamer/bind_session.txt":
                        // this is where we return data. we can't use long polling with cassinidev 


                        e.Response = TestServer.CreateLightStreamerResponse(string.Format(@"OK
SessionId:{0}
ControlAddress:localhost.
KeepaliveMillis:30000
MaxBandwidth:0.0
RequestLimit:50000

PROBE
1,5|#|#|#|#|#|#|#|#|#|#|#
1,1|sbPreProdFXAPP475974420|1.61793|-0.00114|1|1.62006|1.61737|400494226|1.61799|1.61796|0|\u005C/Date(1349422105265)\u005C/
1,4|sbPreProdFXAPP1416588099|0.93575|-0.00275|0|0.93892|0.93508|400494241|0.93603|0.93589|0|\u005C/Date(1349353115700)\u005C/
1,6|sbPreProdFXAPP475774416|1.21135|0.00019|1|1.21191|1.21109|400494215|1.21160|1.21147|0|\u005C/Date(1349422103923)\u005C/
1,7|sbPreProdFXAPP475974395|101.908|-0.245|1|102.279|101.863|400494220|101.929|101.918|0|\u005C/Date(1349422105171)\u005C/
1,8|sbPreProdFXAPP475774545|1.02494|0.00097|1|1.02746|1.02385|400494179|1.02513|1.02503|0|\u005C/Date(1349422104906)\u005C/
1,3|sbPreProdFXAPP475824759|1.610|-0.001|1|1.620|1.617|400494246|1.625|1.618|0|\u005C/Date(1349422105265)\u005C/
1,2|sbPreProdFXAPP475824757|1.61791|-0.00114|1|1.62006|1.61737|400494234|1.61801|1.61796|0|\u005C/Date(1349422105265)\u005C/
LOOP

", defaultPricesSessionid));
                        

                        break;

                    default:
                        throw new Exception("unexpected request:" + e.Request.Route);

                }
                                         };



            try
            {

                var ctx = new Client(new Uri("http://localhost.:" + server.Port), new Uri("http://localhost.:" + server.Port), "foo");
                ctx.Session = "session";
                ctx.UserName = "foo";

                var streaming = ctx.CreateStreamingClient();
                var listener = streaming.BuildDefaultPricesListener(9);

                bool streamingMessageRecieved = false;

                listener.MessageReceived += (a, r) =>
                {
                    Console.WriteLine(r.Data.ToStringWithValues());
                    streamingMessageRecieved = true;

                };
                Thread.Sleep(3000);

                streaming.TearDownListener(listener);
                streaming.Dispose();
                ctx.Dispose();

                Assert.IsTrue(streamingMessageRecieved, "no streaming message recieved");



            }
            finally
            {
                server.Stop();
            }
        }
    }
}