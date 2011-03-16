using System;
using CIAPI.Streaming;
using NUnit.Framework;
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
