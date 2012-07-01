using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Salient.JsonSchemaUtilities.Tests
{
    [TestFixture]
    public class NullablePrimitiveTypeConversionFixture : ConversionVerificationFixtureBase
    {
        [Test]
        public void VerifyNullableBoolean()
        {
            Type typeToTest = typeof(bool?);
            const bool flatten = false;
            var expected = new JObject(
                new JProperty("type", new JArray("null", "boolean"))).ToString();

            string actual = GetActual(typeToTest, expected, flatten);

            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void VerifyNullableDateTime()
        {
            Type typeToTest = typeof(DateTime?);

            var expected = new JObject(
                new JProperty("type", new JArray("null", "string")),
                new JProperty("format", "wcf-date")
                ).ToString();
            const bool flatten = false;
            string actual = GetActual(typeToTest, expected,flatten);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void VerifyNullableDateTimeOffset()
        {
            Type typeToTest = typeof(DateTimeOffset?);

            var expected = new JObject(
                new JProperty("type", new JArray("null", "string")),
                new JProperty("format", "wcf-date")
                ).ToString();

            
            string actual = GetActual(typeToTest, expected,false);

            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void VerifyNullableByte()
        {
            Type typeToTest = typeof(byte?);

            var expected = new JObject(
                new JProperty("type", new JArray("null", "number")),
                new JProperty("format", "integer"),
                new JProperty("minimum", 0),
                new JProperty("maximum", 255)
                ).ToString();

            const bool flatten = false;
            string actual = GetActual(typeToTest, expected,flatten);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void VerifyNullableChar()
        {
            Type typeToTest = typeof(char?);

            var expected = new JObject(
                new JProperty("type", new JArray("null", "string")),
                new JProperty("minLength", 0),
                new JProperty("maxLength", 1)
                ).ToString();

            const bool flatten = false;
            string actual = GetActual(typeToTest, expected,flatten);

            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void VerifyNullableDecimal()
        {
            Type typeToTest = typeof(decimal?);

            var expected = new JObject(
                new JProperty("type", new JArray("null", "number")),
                new JProperty("format", "decimal"),
                new JProperty("minimum", -7.9228162514264338E+28),
                new JProperty("maximum", 7.9228162514264338E+28
                    )
                ).ToString();

            const bool flatten = false;
            string actual = GetActual(typeToTest, expected,flatten);

            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void VerifyNullableDouble()
        {
            Type typeToTest = typeof(double?);

            var expected = new JObject(
                new JProperty("type", new JArray("null", "number")),
                new JProperty("minimum", -1.7976931348623157E+308),
                new JProperty("maximum", 1.7976931348623157E+308)
                ).ToString();

            const bool flatten = false;
            string actual = GetActual(typeToTest, expected,flatten);

            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void VerifyNullableSByte()
        {
            Type typeToTest = typeof(sbyte?);

            var expected = new JObject(
                new JProperty("type", new JArray("null", "number")),
                new JProperty("format", "integer"),
                new JProperty("minimum", -128),
                new JProperty("maximum", 127)
                ).ToString();

            const bool flatten = false;
            string actual = GetActual(typeToTest, expected,flatten);

            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void VerifyNullableSingle()
        {
            Type typeToTest = typeof(Single?);

            var expected = new JObject(
                new JProperty("type", new JArray("null", "number")),
                new JProperty("minimum", -3.4028234663852886E+38),
                new JProperty("maximum", 3.4028234663852886E+38)
                ).ToString();

            const bool flatten = false;
            string actual = GetActual(typeToTest, expected,flatten);

            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void VerifyNullableUInt16()
        {
            Type typeToTest = typeof(UInt16?);

            var expected = new JObject(
                new JProperty("type", new JArray("null", "number")),
                new JProperty("format", "integer"),
                new JProperty("minimum", 0),
                new JProperty("maximum", 65535)
                ).ToString();

            const bool flatten = false;
            string actual = GetActual(typeToTest, expected,flatten);

            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void VerifyNullableUInt32()
        {
            Type typeToTest = typeof(UInt32?);

            var expected = new JObject(
                new JProperty("type", new JArray("null", "number")),
                new JProperty("format", "integer"),
                new JProperty("minimum", 0),
                new JProperty("maximum", 4294967295)
                ).ToString();

            const bool flatten = false;
            string actual = GetActual(typeToTest, expected,flatten);

            Assert.AreEqual(expected, actual);
        }


        [Test, ExpectedException(typeof(System.NotImplementedException))]
        public void VerifyNullableUInt64()
        {
            Type typeToTest = typeof(UInt64);

            var expected = new JObject(
                new JProperty("type", new JArray("null", "number")),
                new JProperty("format", "integer"),
                new JProperty("minimum", -9223372036854775808),
                new JProperty("maximum", 9223372036854775807)
                ).ToString();

            const bool flatten = false;
            string actual = GetActual(typeToTest, expected,flatten);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void VerifyNullableInt16()
        {
            Type typeToTest = typeof(Int16?);

            var expected = new JObject(
                new JProperty("type", new JArray("null", "number")),
                new JProperty("format", "integer"),
                new JProperty("minimum", -32768),
                new JProperty("maximum", 32767)
                ).ToString();
            const bool flatten = false;
            string actual = GetActual(typeToTest, expected,flatten);

            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void VerifyNullableInt32()
        {
            Type typeToTest = typeof(Int32?);

            var expected = new JObject(
                new JProperty("type", new JArray("null", "number")),
                new JProperty("format", "integer"),
                new JProperty("minimum", -2147483648),
                new JProperty("maximum", 2147483647)
                ).ToString();
            const bool flatten = false;
            string actual = GetActual(typeToTest, expected,flatten);

            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void VerifyNullableInt64()
        {
            Type typeToTest = typeof(Int64?);

            var expected = new JObject(
                new JProperty("type", new JArray("null", "number")),
                new JProperty("format", "integer"),
                new JProperty("minimum", -9223372036854775808),
                new JProperty("maximum", 9223372036854775807)
                ).ToString();

            const bool flatten = false;
            string actual = GetActual(typeToTest, expected,flatten);

            Assert.AreEqual(expected, actual);
        }


    }
}