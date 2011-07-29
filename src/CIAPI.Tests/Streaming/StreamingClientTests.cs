using System;
using CIAPI.Streaming;
using NUnit.Framework;
using StreamingClient;
using IStreamingClient = CIAPI.Streaming.IStreamingClient;

namespace CIAPI.Tests.Streaming
{
    [TestFixture, Ignore("can no longer create a listener on an adapter that does not exist: move the tests to integration or implement test client")]
    public class StreamingClientTests
    {
        private IStreamingClient _streamingClient;

        [SetUp]
        public void SetUp()
        {
            _streamingClient = StreamingClientFactory.CreateStreamingClient(new Uri("http://a.server.com/"),
                                                                            "username", "sessionId");

        }

        
        
        [Test]
        public void SubscribingToAnInvalidNewsHeadlinesTopicThrowsAnException()
        {
            Assert.Throws<InvalidTopicException>(() => _streamingClient.BuildNewsHeadlinesListener("BOGUS.TOPIC"));
        }

        //[Test]
        //public void SubscribingToAnInvalidQuotesTopicThrowsAnException()
        //{
        //    Assert.Throws<InvalidTopicException>(() => _streamingClient.BuildQuotesListener());
        //}

        //[Test]
        //public void SubscribingToAnInvalidClientAccountMarginTopicThrowsAnException()
        //{
        //    Assert.Throws<InvalidTopicException>(() => _streamingClient.BuildClientAccountMarginListener());
        //}
    }
}