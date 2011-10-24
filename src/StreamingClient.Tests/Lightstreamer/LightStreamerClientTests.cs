using System;
using System.Text.RegularExpressions;
using NUnit.Framework;
using StreamingClient.Lightstreamer;

namespace StreamingClient.Tests.Lightstreamer
{
    [TestFixture, Ignore("can no longer create a listener on an adapter that does not exist: move the tests to integration or implement test client")]
    public class LightStreamerClientTests
    {
        private LightstreamerClient _lightstreamerClient;
        private static readonly Regex _anythingMask = new Regex(@".*");

        [SetUp]
        public void SetUp()
        {
            _lightstreamerClient = new PartialLightstreamerClient(new Uri("http://some.sever.com/"), "", "");
        }

        [Test]
        public void OnlyCreatesOneListenerPerTopic()
        {
            var listener1 = _lightstreamerClient.BuildListener<AMessageTypeDto>("FOO","TOPIC.ONE");
            var listener2 = _lightstreamerClient.BuildListener<AMessageTypeDto>("FOO", "TOPIC.ONE");

            Assert.That(listener1, Is.EqualTo(listener2), "Different listener instances should not be created.  There should only ever be one listener / topic");
        }

        [Test]
        public void CreatesDifferentListenersForDifferentTopics()
        {
            var listener1 = _lightstreamerClient.BuildListener<AMessageTypeDto>("FOO", "TOPIC.ONE");
            var listener2 = _lightstreamerClient.BuildListener<AMessageTypeDto>("FOO", "TOPIC.TWO");

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
            
        }

        [Test]
        public void SubscriptionToMultipleTopicsSubscribesToSingleSpaceSeparatedTopic()
        {
            var sampleLightstreamerClient = ((PartialLightstreamerClient)_lightstreamerClient);

            var multiListener = sampleLightstreamerClient.BuildSampleListener(new[]
                                                                                  {
                                                                                      "SAMPLES.SAMPLE.1",
                                                                                      "SAMPLES.SAMPLE.2",
                                                                                      "SAMPLES.SAMPLE.3"
                                                                                  });
            Assert.That(multiListener.Topic, Is.EqualTo("SAMPLES.SAMPLE.1 SAMPLES.SAMPLE.2 SAMPLES.SAMPLE.3"));
        }

        [Test]
        public void ValidatesEveryTopicWhenBuildingMultiListener()
        {
            var sampleLightstreamerClient = ((PartialLightstreamerClient)_lightstreamerClient);

            var ex =
                Assert.Throws<InvalidTopicException>(
                    () => sampleLightstreamerClient.BuildSampleListener(new[]
                                                                                  {
                                                                                      "SAMPLES.SAMPLE.1",
                                                                                      "INVALID.TOPIC",
                                                                                      "SAMPLES.SAMPLE.3"
                                                                                  }));

            Assert.That(ex.Message.Contains("topic"), "error message should talk about the topic");
            
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

            
            public IStreamingListener<AMessageTypeDto> BuildSampleListener(string topic)
            {
                return BuildListener<AMessageTypeDto>("FOO", topic);
            }

            public IStreamingListener<AMessageTypeDto> BuildSampleListener(string[] topics)
            {
                return BuildListener<AMessageTypeDto>("FOO",string.Join(" ", topics));
            }

            //protected override string[] GetAdapterList()
            //{
            //    return new string[]{"FOO"};
            //}
        }
    }
}