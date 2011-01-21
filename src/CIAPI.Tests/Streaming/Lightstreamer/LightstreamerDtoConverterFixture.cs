using System;
using CIAPI.Streaming.Lightstreamer;
using NUnit.Framework;

namespace CIAPI.Tests.Streaming.Lightstreamer
{
    [TestFixture]
    public class LightstreamerDtoConverterFixture
    {
        private LightstreamerDtoConverter<TestDto> _testDtoConverter;

        [SetUp]
        public void SetUp()
        {
            _testDtoConverter = new TestDtoConverter();
        }

        [Test]
        public void CanReturnSpaceSeparatedListOfFields()
        {
            Assert.AreEqual("Field1 Field2 Field3", _testDtoConverter.GetFieldList());
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

 
        public class TestDtoConverter : LightstreamerDtoConverter<TestDto>
        {
            public override  TestDto Convert(object data)
            {
                return new TestDto();
            }
        }

        public class TestDto
        {
            public string Field1 { get; set; }
            public int Field2 { get; set; }
            public DateTime Field3 { get; set; }
        }
    }
}