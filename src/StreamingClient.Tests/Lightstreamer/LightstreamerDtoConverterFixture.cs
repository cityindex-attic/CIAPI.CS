using System;
using NUnit.Framework;
using StreamingClient.Lightstreamer;

namespace StreamingClient.Tests.Lightstreamer
{
    [TestFixture]
    public class LightstreamerDtoConverterFixture
    {
        private LightstreamerDtoConverter<TestDto> _testDtoConverter;

        [SetUp]
        public void SetUp()
        {
            _testDtoConverter = new LightstreamerDtoConverter<TestDto>();
        }

        [Test] public void CanConvertNullableJsonValues()
        {


            Assert.IsNull(LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(DateTime?), "DateTime?", ""));
            Assert.IsNull(LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(DateTime?), "DateTime?", null));

            Assert.IsNull(LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(Boolean?), "Boolean?", ""));
            Assert.IsNull(LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(Boolean?), "Boolean?", null));

            Assert.IsNull( LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(Byte?), "Byte", ""));
            Assert.IsNull( LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(Byte?), "Byte", null));

            Assert.IsNull(LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(Char?), "Char?", ""));
            Assert.IsNull(LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(Char?), "Char?", null));

            Assert.IsNull( LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(Decimal?), "Decimal?", ""));
            Assert.IsNull( LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(Decimal?), "Decimal?", null));

            Assert.IsNull( LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(Double?), "Double?", ""));
            Assert.IsNull( LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(Double?), "Double?", null));

            Assert.IsNull( LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(Int16?), "Int16?", ""));
            Assert.IsNull( LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(Int16?), "Int16?", null));

            Assert.IsNull( LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(Int32?), "Int32?", ""));
            Assert.IsNull( LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(Int32?), "Int32?", null));

            Assert.IsNull( LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(Int64?), "Int64?", ""));
            Assert.IsNull( LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(Int64?), "Int64?", null));

            Assert.IsNull( LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(SByte?), "SByte?", ""));
            Assert.IsNull( LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(SByte?), "SByte?", null));

            Assert.IsNull( LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(Single?), "Single?", ""));
            Assert.IsNull( LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(Single?), "Single?", null));

            Assert.IsNull( LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(UInt16?), "UInt16?", ""));
            Assert.IsNull( LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(UInt16?), "UInt16?", null));

            Assert.IsNull( LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(UInt32?), "UInt32?", ""));
            Assert.IsNull( LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(UInt32?), "UInt32?", null));

            Assert.IsNull( LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(UInt64?), "UInt64?", ""));
            Assert.IsNull( LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(UInt64?), "UInt64?", null));

            Assert.Throws<NotImplementedException>(() => LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(Object), "Object", ""));
            Assert.Throws<NotImplementedException>(() => LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(DBNull), "DBNull", ""));



        }
        [Test]
        public void CanConvertNullJsonValues()
        {
         
            Assert.AreEqual(string.Empty,LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(string), "string", ""));
            Assert.IsNull(LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(string), "string", null));

            Assert.AreEqual(DateTime.MinValue,LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(DateTime), "DateTime", ""));
            Assert.AreEqual(DateTime.MinValue, LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(DateTime), "DateTime", null));

            Assert.AreEqual(false, LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(Boolean), "Boolean", ""));
            Assert.AreEqual(false,LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(Boolean), "Boolean", null));

            Assert.AreEqual(0, LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(Byte), "Byte", ""));
            Assert.AreEqual(0, LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(Byte), "Byte", null));

            Assert.AreEqual(default(char), LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(Char), "Char", ""));
            Assert.AreEqual(default(char), LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(Char), "Char", null));

            Assert.AreEqual(0, LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(Decimal), "Decimal", ""));
            Assert.AreEqual(0, LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(Decimal), "Decimal", null));

            Assert.AreEqual(0, LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(Double), "Double", ""));
            Assert.AreEqual(0, LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(Double), "Double", null));

            Assert.AreEqual(0, LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(Int16), "Int16", ""));
            Assert.AreEqual(0, LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(Int16), "Int16", null));

            Assert.AreEqual(0, LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(Int32), "Int32", ""));
            Assert.AreEqual(0, LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(Int32), "Int32", null));

            Assert.AreEqual(0, LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(Int64), "Int64", ""));
            Assert.AreEqual(0, LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(Int64), "Int64", null));

            Assert.AreEqual(0, LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(SByte), "SByte", ""));
            Assert.AreEqual(0, LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(SByte), "SByte", null));

            Assert.AreEqual(0, LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(Single), "Single", ""));
            Assert.AreEqual(0, LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(Single), "Single", null));

            Assert.AreEqual(0, LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(UInt16), "UInt16", ""));
            Assert.AreEqual(0, LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(UInt16), "UInt16", null));

            Assert.AreEqual(0, LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(UInt32), "UInt32", ""));
            Assert.AreEqual(0, LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(UInt32), "UInt32", null));

            Assert.AreEqual(0, LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(UInt64), "UInt64", ""));
            Assert.AreEqual(0,LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(UInt64), "UInt64", null));

            Assert.Throws<NotImplementedException>(() => LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(Object), "Object", ""));
            Assert.Throws<NotImplementedException>(() => LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(DBNull), "DBNull", ""));

            
            
        }
        [Test]
        public void CanConvertJsonValues()
        {

            Assert.AreEqual(DateTime.Parse("2011-11-21 07:28:06.000"), LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(DateTime), "DateTime", "/Date(1321860486000)/"));


            Assert.AreEqual("FOO", LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(string), "string", "FOO"));
            
            Assert.AreEqual(false, LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(Boolean), "Boolean", "false"));
            Assert.AreEqual(false, LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(Boolean), "Boolean", "False"));

            Assert.AreEqual(1, LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(Byte), "Byte", "1"));
            

            Assert.AreEqual('1', LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(Char), "Char", "1"));
            

            Assert.AreEqual(1, LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(Decimal), "Decimal", "1"));
            

            Assert.AreEqual(1, LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(Double), "Double", "1"));
            

            Assert.AreEqual(1, LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(Int16), "Int16", "1"));
            

            Assert.AreEqual(1, LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(Int32), "Int32", "1"));
            

            Assert.AreEqual(1, LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(Int64), "Int64", "1"));
            

            Assert.AreEqual(1, LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(SByte), "SByte", "1"));
            

            Assert.AreEqual(1, LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(Single), "Single", "1"));
            

            Assert.AreEqual(1, LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(UInt16), "UInt16", "1"));
            

            Assert.AreEqual(1, LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(UInt32), "UInt32", "1"));
            

            Assert.AreEqual(1, LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(UInt64), "UInt64", "1"));
            

            Assert.Throws<NotImplementedException>(() => LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(Object), "Object", ""));
            Assert.Throws<NotImplementedException>(() => LightstreamerDtoConverter<TestDto>.ConvertPropertyValue(typeof(DBNull), "DBNull", ""));



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