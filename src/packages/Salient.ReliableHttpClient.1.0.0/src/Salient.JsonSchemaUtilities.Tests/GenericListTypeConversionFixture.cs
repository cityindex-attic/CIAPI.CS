using System;
using System.Collections.Generic;
using System.Drawing;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Salient.JsonSchemaUtilities.Tests
{
    [TestFixture]
    public class GenericListTypeConversionFixture : ConversionVerificationFixtureBase
    {

        [Test]
        public void VerifyGenericListOfDateTimeOffset()
        {
            Type typeToTest = typeof(List<DateTimeOffset>);

            var expected = new JObject(
                new JProperty("type", "array"),
                new JProperty("items",
                    new JObject(new JProperty("type", "string"),
                    new JProperty("format", "wcf-date")))
                ).ToString();

            const bool flatten = false;
            string actual = GetActual(typeToTest, expected,flatten);

            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void VerifyArrayOfInt()
        {
            int[] obj = new[] { 1, 2, 3, 4 };
            Type typeToTest = obj.GetType();

            var expected = new JObject(
                new JProperty("type", "array"),
                new JProperty("items",
                    new JObject(new JProperty("type", "number"),
                    new JProperty("format", "integer"),
                    new JProperty("minimum", -2147483648),
                    new JProperty("maximum", 2147483647)))
                ).ToString();

            const bool flatten = false;
            string actual = GetActual(typeToTest, expected,flatten);

            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void VerifyIEnumerableOfInt()
        {
            Type typeToTest = typeof(IEnumerable<int>);

            var expected = new JObject(
                new JProperty("type", "array"),
                new JProperty("items",
                    new JObject(new JProperty("type", "number"),
                    new JProperty("format", "integer"),
                    new JProperty("minimum", -2147483648),
                    new JProperty("maximum", 2147483647)))
                ).ToString();

            const bool flatten = false;
            string actual = GetActual(typeToTest, expected,flatten);

            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void VerifyGenericListOfInt()
        {
            Type typeToTest = typeof(List<int>);

            var expected = new JObject(
                new JProperty("type", "array"),
                new JProperty("items",
                    new JObject(new JProperty("type", "number"),
                    new JProperty("format", "integer"),
                    new JProperty("minimum", -2147483648),
                    new JProperty("maximum", 2147483647)))
                ).ToString();

            const bool flatten = false;
            string actual = GetActual(typeToTest, expected,flatten);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void VerifyGenericListOfNullableInt()
        {
            Type typeToTest = typeof(List<int?>);

            var expected = new JObject(
                new JProperty("type", "array"),
                new JProperty("items",
                    new JObject(new JProperty("type", new JArray("null", "number")),
                    new JProperty("format", "integer"),
                    new JProperty("minimum", -2147483648),
                    new JProperty("maximum", 2147483647)))
                ).ToString();

            const bool flatten = false;
            string actual = GetActual(typeToTest, expected,flatten);

            Assert.AreEqual(expected, actual);
        }

    }
}