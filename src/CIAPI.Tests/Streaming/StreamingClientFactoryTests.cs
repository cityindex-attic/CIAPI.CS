using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CIAPI.Rpc;
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
            var rpcClient = new Client(new Uri("http://foo.com"), new Uri("http://foo.com"), "nullAndVoid");
            Assert.IsInstanceOf(typeof(IStreamingClient), rpcClient.CreateStreamingClient());
        }



    }
}
