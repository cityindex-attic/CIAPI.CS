using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Salient.JsonSchemaUtilities.Tests

{
    [TestFixture]
    public class PrimitiveTypeConversionFixture : ConversionVerificationFixtureBase
    {
        [Test]
        public void VerifyBoolean()
        {
            Type typeToTest = typeof(bool);

            var expected = new JObject(
                new JProperty("type", "boolean")).ToString();
            const bool flatten = false;
            string actual = GetActual(typeToTest, expected,flatten);

            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void VerifyDateTimeOffset()
        {
            Type typeToTest = typeof(DateTimeOffset);

            var expected = new JObject(
                new JProperty("type", "string"),
                new JProperty("format", "wcf-date")
                ).ToString();
            const bool flatten = false;
            string actual = GetActual(typeToTest, expected,flatten);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void VerifyDateTime()
        {
            Type typeToTest = typeof(DateTime);

            var expected = new JObject(
                new JProperty("type", "string"),
                new JProperty("format", "wcf-date")
                ).ToString();

            const bool flatten = false;
            string actual = GetActual(typeToTest, expected,flatten);

            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void VerifyByte()
        {
            Type typeToTest = typeof(byte);

            var expected = new JObject(
                new JProperty("type", "number"),
                new JProperty("format", "integer"),
                new JProperty("minimum", 0),
                new JProperty("maximum", 255)
                ).ToString();

            const bool flatten = false;
            string actual = GetActual(typeToTest, expected,flatten);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void VerifyChar()
        {
            Type typeToTest = typeof(char);

            var expected = new JObject(
                  new JProperty("type", "string"),
                  new JProperty("minLength", 0),
                  new JProperty("maxLength", 1)
                  ).ToString();
            const bool flatten = false;
            string actual = GetActual(typeToTest, expected,flatten);

            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void VerifyDecimal()
        {
            Type typeToTest = typeof(decimal);

            var expected = new JObject(
                new JProperty("type", "number"),
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
        public void VerifyDouble()
        {
            Type typeToTest = typeof(double);

            var expected = new JObject(
                new JProperty("type", "number"),
                new JProperty("minimum", -1.7976931348623157E+308),
                new JProperty("maximum", 1.7976931348623157E+308)
                ).ToString();

            const bool flatten = false;
            string actual = GetActual(typeToTest, expected,flatten);

            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void VerifySByte()
        {
            Type typeToTest = typeof(sbyte);

            var expected = new JObject(
                new JProperty("type", "number"),
                new JProperty("format", "integer"),
                new JProperty("minimum", -128),
                new JProperty("maximum", 127)
                ).ToString();

            const bool flatten = false;
            string actual = GetActual(typeToTest, expected,flatten);

            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void VerifySingle()
        {
            Type typeToTest = typeof(Single);

            var expected = new JObject(
                new JProperty("type", "number"),
                new JProperty("minimum", -3.4028234663852886E+38),
                new JProperty("maximum", 3.4028234663852886E+38)
                ).ToString();

            const bool flatten = false;
            string actual = GetActual(typeToTest, expected,flatten);

            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void VerifyUInt16()
        {
            Type typeToTest = typeof(UInt16);

            var expected = new JObject(
                new JProperty("type", "number"),
                new JProperty("format", "integer"),
                new JProperty("minimum", 0),
                new JProperty("maximum", 65535)
                ).ToString();

            const bool flatten = false;
            string actual = GetActual(typeToTest, expected,flatten);

            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void VerifyUInt32()
        {
            Type typeToTest = typeof(UInt32);

            var expected = new JObject(
                new JProperty("type", "number"),
                new JProperty("format", "integer"),
                new JProperty("minimum", 0),
                new JProperty("maximum", 4294967295)
                ).ToString();

            const bool flatten = false;
            string actual = GetActual(typeToTest, expected,flatten);

            Assert.AreEqual(expected, actual);
        }


        [Test, ExpectedException(typeof(System.NotImplementedException))]
        public void VerifyUInt64()
        {
            Type typeToTest = typeof(UInt64);

            var expected = new JObject(
                new JProperty("type", "number"),
                new JProperty("format", "integer"),
                new JProperty("minimum", -9223372036854775808),
                new JProperty("maximum", 9223372036854775807)
                ).ToString();

            const bool flatten = false;
            string actual = GetActual(typeToTest, expected,flatten);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void VerifyInt16()
        {
            Type typeToTest = typeof(Int16);

            var expected = new JObject(
                new JProperty("type", "number"),
                new JProperty("format", "integer"),
                new JProperty("minimum", -32768),
                new JProperty("maximum", 32767)
                ).ToString();

            const bool flatten = false;
            string actual = GetActual(typeToTest, expected,flatten);

            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void VerifyInt32()
        {
            Type typeToTest = typeof(Int32);

            var expected = new JObject(
                new JProperty("type", "number"),
                new JProperty("format", "integer"),
                new JProperty("minimum", -2147483648),
                new JProperty("maximum", 2147483647)
                ).ToString();
            const bool flatten = false;
            string actual = GetActual(typeToTest, expected,flatten);

            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void VerifyInt64()
        {
            Type typeToTest = typeof(Int64);

            var expected = new JObject(
                new JProperty("type", "number"),
                new JProperty("format", "integer"),
                new JProperty("minimum", -9223372036854775808),
                new JProperty("maximum", 9223372036854775807)
                ).ToString();

            const bool flatten = false;
            string actual = GetActual(typeToTest, expected,flatten);

            Assert.AreEqual(expected, actual);
        }




        [Test]
        public void VerifyString()
        {
            Type typeToTest = typeof(string);

            var expected = new JObject(
                new JProperty("type", "string")).ToString();

            const bool flatten = false;
            string actual = GetActual(typeToTest, expected,flatten);

            Assert.AreEqual(expected, actual);
        }




    }
}
