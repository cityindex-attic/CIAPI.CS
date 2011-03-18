﻿using System;
using System.Text.RegularExpressions;
using NUnit.Framework;
using StreamingClient.Lightstreamer;

namespace StreamingClient.Tests.Lightstreamer
{
    [TestFixture]
    public class LightStreamerClientTests
    {
        private LightstreamerClient _lightstreamerClient;
        private static readonly Regex _anythingMask = new Regex(@".*");

        [SetUp]
        public void SetUp()
        {
            _lightstreamerClient = new PartialLightstreamerClient(new Uri("http://some.sever.com/"), "", "" );
        }

        [Test]
        public void OnlyCreatesOneListenerPerTopic()
        {
            var listener1 = _lightstreamerClient.BuildListener<AMessageTypeDto>("TOPIC.ONE", _anythingMask);
            var listener2 = _lightstreamerClient.BuildListener<AMessageTypeDto>("TOPIC.ONE", _anythingMask);

            Assert.That(listener1, Is.EqualTo(listener2), "Different listener instances should not be created.  There should only ever be one listener / topic");
        }

        [Test]
        public void CreatesDifferentListenersForDifferentTopics()
        {
            var listener1 = _lightstreamerClient.BuildListener<AMessageTypeDto>("TOPIC.ONE", _anythingMask);
            var listener2 = _lightstreamerClient.BuildListener<AMessageTypeDto>("TOPIC.TWO", _anythingMask);

            Assert.That(listener1, Is.Not.EqualTo(listener2), "Different topics should have different listeners");
        }

        [Test]
        public void ValidatesTopicWhenBuildingListener()
        {
            var sampleLightstreamerClient = ((PartialLightstreamerClient)_lightstreamerClient);

            var ex =
                Assert.Throws<InvalidTopicException>(
                    () => sampleLightstreamerClient.BuildSampleListener("TOPIC.ONE"));

            Assert.That(ex.Message.Contains("topic"), "error message should talk about the topic");
            Assert.That(ex.Message.Contains(sampleLightstreamerClient.SAMPLE_TOPIC_MASK.ToString()), "error message should indicate the expected regex mask");
        }

        public class AMessageTypeDto
        {
        }

        private class PartialLightstreamerClient : LightstreamerClient
        {
            public PartialLightstreamerClient(Uri streamingUri, string userName, string sessionId)
                : base(streamingUri, userName, sessionId)
            {
            }

            public readonly Regex SAMPLE_TOPIC_MASK = new Regex(@"SAMPLES.SAMPLE.(\d+)");
            public IStreamingListener<AMessageTypeDto> BuildSampleListener(string topic)
            {
                return BuildListener<AMessageTypeDto>(topic, SAMPLE_TOPIC_MASK);
            }
        }
    }
}