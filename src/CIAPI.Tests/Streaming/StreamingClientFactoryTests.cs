using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CIAPI.Streaming;
using NUnit.Framework;
using Salient.ReflectiveLoggingAdapter;
using IStreamingClient = CIAPI.Streaming.IStreamingClient;

namespace CIAPI.Tests.Streaming
{
    [TestFixture]
    public class StreamingClientFactoryTests
    {
        static Dictionary<string, CapturingAppender> loggers;
        static StreamingClientFactoryTests()
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
        public void ReturnsAnIStreamingClient()
        {
            Assert.IsInstanceOf(typeof(IStreamingClient), 
                StreamingClientFactory.CreateStreamingClient(new Uri("http://a.server.com/"), "username", "sessionId"));
        }


        //[Test]
        //public void EnsureInnerLoggerIsCalled()
        //{


            

        //    //Sky TODO - test the mockInnerLogger gets called by the RPC client and the StreamingClient

        //    var client = StreamingClientFactory.CreateStreamingClient(new Uri("http://a.server.com/"), "username",
        //                                                              "sessionId");
            
        //    // this is a unit test, not integration. 
        //    var lsLogger = loggers["com.lightstreamer.ls_client.actions"];


        //    var factoryLogger = loggers["CIAPI.Streaming.Lightstreamer.LightstreamerClient"];
                
        //    Assert.Greater(factoryLogger.GetItems().ToArray().Length, 0);



        //}
    }
}
