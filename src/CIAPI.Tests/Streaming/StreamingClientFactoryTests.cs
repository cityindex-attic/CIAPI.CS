using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIAPI.Streaming;
using CityIndex.JsonClient;
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

    [TestFixture]
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
        public void SubscribingToAnInvalidPriceTopicThrowsAnException()
        {
            Assert.Throws<InvalidTopicException>(() => _streamingClient.BuildPriceListener("BOGUS.TOPIC"));
        }

        [Test]
        public void SubscribingToAnInvalidNewsHeadlinesTopicThrowsAnException()
        {
            Assert.Throws<InvalidTopicException>(() => _streamingClient.BuildNewsHeadlinesListener("BOGUS.TOPIC"));
        }

        [Test]
        public void SubscribingToAnInvalidQuotesTopicThrowsAnException()
        {
            Assert.Throws<InvalidTopicException>(() => _streamingClient.BuildQuoteListener("BOGUS.TOPIC"));
        }

        [Test]
        public void SubscribingToAnInvalidClientAccountMarginTopicThrowsAnException()
        {
            Assert.Throws<InvalidTopicException>(() => _streamingClient.BuildClientAccountMarginListener("BOGUS.TOPIC"));
        }
    }
}
