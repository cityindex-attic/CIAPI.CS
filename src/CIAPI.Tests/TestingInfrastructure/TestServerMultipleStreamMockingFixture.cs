using System;
using System.Collections.Specialized;
using System.Threading;
using System.Web;
using CIAPI.Rpc;
using NUnit.Framework;
using Salient.ReflectiveLoggingAdapter;

namespace CIAPI.Tests.TestingInfrastructure
{
    /// <summary>
    /// this simple multi stream mock runs with fiddler running but fails when fiddler is not running.
    /// 
    /// </summary>
    [TestFixture]
    public class TestServerMultipleStreamMockingFixture
    {
        static TestServerMultipleStreamMockingFixture()
        {
            //LogManager.CreateInnerLogger =
            //   (logName, logLevel, showLevel, showDateTime, showLogName, dateTimeFormat) =>
            //   new SimpleTraceAppender(logName, logLevel, showLevel, showDateTime, showLogName, dateTimeFormat);
        }

        [Test]
        public void MockMultipleStreams()
        {

            // set up the server to mock responses so that we can utilize actual client instances for testing

            var server = new TestServer();

            try
            {
                server.Start();

                server.ProcessRequest += (i, e) =>
                {


                    if (e.Request.Route.StartsWith("/lightstreamer"))
                    {
                        ProcessStreamingRequest(e);
                    }
                    else
                    {
                        ProcessRpcRequest(e);
                    }

                };


                // now use the libraries as you normally would 

                var uri = new Uri("http://localhost.:" + server.Port);
                var ctx = new Client(uri, uri, "foo");

                var loginResponse = ctx.LogIn("foo", "bar");


                // only difference here is that we MUST force polling mode on the lightstreamer client
                var streamingClient = ctx.CreateStreamingClient();


                bool defaultPricesListenerMessageRecieved = false;
                bool pricesListenerMessageRecieved = false;

                var defaultPricesListener = streamingClient.BuildDefaultPricesListener(9);
                defaultPricesListener.MessageReceived += (a, r) =>
                {
                    Console.WriteLine("\nDEFAULT PRICE\n" + r.ToStringWithValues());
                    defaultPricesListenerMessageRecieved = true;
                };

                var pricesListener = streamingClient.BuildPricesListener(new[] { 99498, 99500 });
                pricesListener.MessageReceived += (a, r) =>
                {
                    Console.WriteLine("\nPRICE\n" + r.ToStringWithValues());
                    pricesListenerMessageRecieved = true;
                };


                Thread.Sleep(3000);

                streamingClient.TearDownListener(defaultPricesListener);
                streamingClient.TearDownListener(pricesListener);
                streamingClient.Dispose();

                ctx.LogOut();
                ctx.Dispose();

                Assert.IsFalse(string.IsNullOrEmpty(loginResponse.Session), "login failed");
                Assert.IsTrue(defaultPricesListenerMessageRecieved, "no defaultPricesListenerMessageRecieved recieved");
                Assert.IsTrue(pricesListenerMessageRecieved, "no pricesListenerMessageRecieved  recieved");
            }
            finally
            {
                server.Stop();
            }
                

        }

        private void ProcessRpcRequest(ServerBase.RequestEventArgs e)
        {

            switch (e.Request.Route)
            {
                // RPC requests
                case "/session":
                    e.Response = TestServer.CreateRpcResponse("{\"Session\":\"D2FF3E4D-01EA-4741-86F0-437C919B5559\"}");
                    break;

                case "/session/deleteSession":
                    e.Response = TestServer.CreateRpcResponse("{\"LoggedOut\":true}");
                    break;

                default:
                    throw new Exception("unexpected request:" + e.Request.Route);

            }
        }

        private int _defaultPricesTableIndex;
        private int _pricesTableIndex;
        const string CITYINDEXSTREAMINGDEFAULTPRICES_SESSIONID = "S8f295c055c413e4bT4448618";
        const string CITYINDEXSTREAMING_SESSIONID = "S228ffc03baabb3edT4454468";


        private void ProcessStreamingRequest(ServerBase.RequestEventArgs e)
        {
            NameValueCollection parameters;
            if (e.Request.Body != null)
            {

                parameters = HttpUtility.ParseQueryString(e.Request.Body);
            }
            else
            {
                throw new Exception("request body is null for " + e.Request.Route);

            }
            string adapter = parameters["LS_adapter_set"];
            string session = parameters["LS_session"];


            switch (e.Request.Route)
            {
                    // Streaming requests




                case "/lightstreamer/create_session.txt":
                    // build up a session for adapter set


                    string thisSessionId;
                    switch (adapter)
                    {
                        case "CITYINDEXSTREAMING":
                            thisSessionId = CITYINDEXSTREAMING_SESSIONID;
                            break;
                        case "CITYINDEXSTREAMINGDEFAULTPRICES":
                            thisSessionId = CITYINDEXSTREAMINGDEFAULTPRICES_SESSIONID;
                            break;
                        default:
                            throw new NotImplementedException("unexpected adapter");
                    }

                    e.Response = TestServer.CreateLightStreamerResponse(string.Format(STREAMING_RESPONSE_HEADER, thisSessionId) + "PROBE\nLOOP");
                    break;


                case "/lightstreamer/control.txt":
                    // this is where we associate topics (tables) to a session
                    switch (parameters["LS_op"])
                    {
                        case "add":
                            string table = parameters["LS_table"];

                            switch (session)
                            {
                                case CITYINDEXSTREAMING_SESSIONID:
                                    _pricesTableIndex = int.Parse(table);
                                    break;
                                case CITYINDEXSTREAMINGDEFAULTPRICES_SESSIONID:
                                    _defaultPricesTableIndex = int.Parse(table);
                                    break;
                                default:
                                    throw new NotImplementedException("unexpected adapter");
                            }

                            break;
                        case "destroy":
                            break;
                    }

                    e.Response = TestServer.CreateLightStreamerResponse(@"OK
");
                    break;
                case "/lightstreamer/bind_session.txt":
                    // this is where we return data. we can't use long polling with cassinidev 
                    // we are providing static response here but a programmatic approach could be taken


                    string response = string.Format(STREAMING_RESPONSE_HEADER, session);

                    response = response + "PROBE\n";

                    switch (session)
                    {
                        case CITYINDEXSTREAMINGDEFAULTPRICES_SESSIONID:


                            response = response + string.Format(@"{0},1|sbPreProdFXAPP475974420|1.61793|-0.00114|1|1.62006|1.61737|400494226|1.61799|1.61796|0|\u005C/Date(1349422105265)\u005C/
{0},4|sbPreProdFXAPP1416588099|0.93575|-0.00275|0|0.93892|0.93508|400494241|0.93603|0.93589|0|\u005C/Date(1349353115700)\u005C/
{0},6|sbPreProdFXAPP475774416|1.21135|0.00019|1|1.21191|1.21109|400494215|1.21160|1.21147|0|\u005C/Date(1349422103923)\u005C/
{0},7|sbPreProdFXAPP475974395|101.908|-0.245|1|102.279|101.863|400494220|101.929|101.918|0|\u005C/Date(1349422105171)\u005C/
{0},8|sbPreProdFXAPP475774545|1.02494|0.00097|1|1.02746|1.02385|400494179|1.02513|1.02503|0|\u005C/Date(1349422104906)\u005C/
{0},3|sbPreProdFXAPP475824759|1.610|-0.001|1|1.620|1.617|400494246|1.625|1.618|0|\u005C/Date(1349422105265)\u005C/
{0},2|sbPreProdFXAPP475824757|1.61791|-0.00114|1|1.62006|1.61737|400494234|1.61801|1.61796|0|\u005C/Date(1349422105265)\u005C/
", _defaultPricesTableIndex);
                            break;
                        case CITYINDEXSTREAMING_SESSIONID:
                            response = response + string.Format(@"{0},2|sbRDBProdFX54104831|5829.5|-4.3|0|5834.3|5817.3|99500|5830.5|5830.0|0|\u005C/Date(1349972219032)\u005C/
{0},1|sbRDBProdFX60690836|13380.0|34.0|1|13427.5|13293.5|99498|13381.0|13380.5|0|\u005C/Date(1349972224789)\u005C/", _pricesTableIndex);
                            break;
                        default:
                            throw new NotImplementedException("unexpected session");
                    }


                    response = response + "\nLOOP\n";
                    e.Response = TestServer.CreateLightStreamerResponse(response);
                    break;

                default:
                    throw new Exception("unexpected request:" + e.Request.Route);

            }
        }

        private const string STREAMING_RESPONSE_HEADER = @"OK
SessionId:{0}
ControlAddress:localhost.
KeepaliveMillis:30000
MaxBandwidth:0.0
RequestLimit:50000
ServerName:Lightstreamer HTTP Server

";
    }

}
