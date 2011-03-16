using System;
using NUnit.Framework;
using StreamingClient.Lightstreamer;
using LightstreamerClient = StreamingClient.Lightstreamer.LightstreamerClient;

namespace StreamingClient.Tests.Lightstreamer
{
    [TestFixture]
    public class LightStreamerClientTests
    {
        private LightstreamerClient _lightstreamerClient;

        [SetUp]
        public void SetUp()
        {
            _lightstreamerClient = new PartialLightstreamerClient(new Uri("http://some.sever.com/"), "", "" );
        }

        [Test]
        public void OnlyCreatesOneListenerPerTopic()
        {
            var listener1 = _lightstreamerClient.BuildListener<AMessageTypeDto>("TOPIC.ONE");
            var listener2 = _lightstreamerClient.BuildListener<AMessageTypeDto>("TOPIC.ONE");

            Assert.That(listener1, Is.EqualTo(listener2), "Different listener instances should not be created.  There should only ever be one listener / topic");
        }

        [Test]
        public void CreatesDifferentListenersForDifferentTopics()
        {
            var listener1 = _lightstreamerClient.BuildListener<AMessageTypeDto>("TOPIC.ONE");
            var listener2 = _lightstreamerClient.BuildListener<AMessageTypeDto>("TOPIC.TWO");

            Assert.That(listener1, Is.Not.EqualTo(listener2), "Different topics should have different listeners");
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
        }
    }

    

    [TestFixture]
    public class LightstreamerDtoConverterFixture
    {
        private LightstreamerDtoConverter<TestDto> _testDtoConverter;

        [SetUp]
        public void SetUp()
        {
            _testDtoConverter = new LightstreamerDtoConverter<TestDto>();
        }

        [Test]
        public void CanReturnSpaceSeparatedListOfFields()
        {
            Assert.AreEqual("Field1 Field2 Field3 Field4", _testDtoConverter.GetFieldList());
        }

        [Test]
        public void Field1HasFieldIndexOf1()
        {
            Assert.AreEqual(1, _testDtoConverter.GetFieldIndex(typeof(TestDto).GetProperty("Field1")));
        }

        [Test]
        public void Field3HasFieldIndexOf3()
        {
            Assert.AreEqual(3, _testDtoConverter.GetFieldIndex(typeof(TestDto).GetProperty("Field3")));
        }

        [Test]
        public void CanPopulatePropertiesOfTypeString()
        {
            var dto = new TestDto();
            _testDtoConverter.PopulateProperty(dto, "Field1", "stringValue");
            Assert.AreEqual("stringValue", dto.Field1);
        }

        [Test]
        public void CanPopulatePropertiesOfTypeInt()
        {
            var dto = new TestDto();
            _testDtoConverter.PopulateProperty(dto, "Field2", "42");
            Assert.AreEqual(42, dto.Field2);
        }

        [Test]
        public void CanPopulatePropertiesOfTypeDecimal()
        {
            var dto = new TestDto();
            _testDtoConverter.PopulateProperty(dto, "Field4", "185.743725183137");
            Assert.AreEqual(185.743725183137d, dto.Field4);
        }

        [Test]
        public void CanPopulatePropertiesOfTypeDateTime()
        {
            var dto = new TestDto();
            _testDtoConverter.PopulateProperty(dto, "Field3", @"\/Date(1295580161923)\/");
            Assert.AreEqual("2011-01-21 03:22:41Z", dto.Field3.ToString("u"));
        }
 
        public class TestDto
        {
            public string Field1 { get; set; }
            public int Field2 { get; set; }
            public DateTime Field3 { get; set; }
            public decimal Field4 { get; set; }
        }
    }
}