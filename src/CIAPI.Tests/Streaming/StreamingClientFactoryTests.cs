using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIAPI.Streaming;
using NUnit.Framework;
using StreamingClient;
using IStreamingClient = CIAPI.Streaming.IStreamingClient;

namespace CIAPI.Tests.Streaming
{
    [TestFixture]
    public class StreamingClientFactoryTests
    {
        [Test]
        public void ReturnsAnIStreamingClient()
        {
            Assert.IsInstanceOf(typeof(IStreamingClient), 
                StreamingClientFactory.CreateStreamingClient(new Uri("http://a.server.com/"), "username", "sessionId"));
        }
    }
}
