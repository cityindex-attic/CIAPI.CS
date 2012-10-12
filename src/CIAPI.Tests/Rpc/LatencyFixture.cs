using System;
using CassiniDev;
using CIAPI.Rpc;
using NUnit.Framework;
using Salient.ReflectiveLoggingAdapter;
using Salient.ReliableHttpClient;

namespace CIAPI.Tests.Rpc
{
    [TestFixture]
    public class LatencyFixture
    {
        static LatencyFixture()
        {
            LogManager.CreateInnerLogger =
                (logName, logLevel, showLevel, showDateTime, showLogName, dateTimeFormat) =>
                new SimpleTraceAppender(logName, logLevel, showLevel, showDateTime, showLogName, dateTimeFormat);
        }

        public const string LoggedIn = "{\"Session\":\"D2FF3E4D-01EA-4741-86F0-437C919B5559\"}";
        [Test]
        public void CheckNetLatency()
        {
            Console.WriteLine("Checking .net latency");

            var server = new CassiniDevServer();
            server.StartServer(Environment.CurrentDirectory);

            var ctx = new Client(new Uri(server.NormalizeUrl("/")), new Uri(server.NormalizeUrl("/")), "foo");

            DateTimeOffset requestRecieved = DateTimeOffset.MinValue;
            RequestCompletedEventArgs requestInfo = null;
            ctx.RequestCompleted += (i, e) =>
                                        {
                                            requestInfo = e;
                                        };
            server.Server.ProcessRequest += (i, e) =>
                                                {
                                                    e.Continue = false;
                                                    e.Response = LoggedIn;
                                                    e.ResponseStatus = 200;
                                                    requestRecieved = DateTimeOffset.UtcNow;

                                                };


            try
            {
                ctx.LogIn(Settings.RpcUserName, Settings.RpcPassword);
            }
            finally
            {
                server.Dispose();
            }

            Console.WriteLine("elapsed   {0}", requestInfo.Info.Watch.ElapsedMilliseconds);

            // #TODO: not sure i like the complete removal of temporal data

            //Console.WriteLine("issued   {0}", requestInfo.Info.Issued.Ticks);
            //Console.WriteLine("recieved {0}", requestRecieved.Ticks);
            //Console.WriteLine("competed {0}", requestInfo.Info.Completed.Ticks);

            //Console.WriteLine("issued to recieved {0}", TimeSpan.FromTicks(requestRecieved.Ticks - requestInfo.Info.Issued.Ticks));
            //Console.WriteLine("recieved to completed {0}", TimeSpan.FromTicks(requestInfo.Info.Completed.Ticks - requestRecieved.Ticks));
            //Console.WriteLine("issued to completed {0}", TimeSpan.FromTicks(requestInfo.Info.Completed.Ticks - requestInfo.Info.Issued.Ticks));


            

            Assert.IsNotNullOrEmpty(ctx.Session);



            ctx.Dispose();
        }
    }
}