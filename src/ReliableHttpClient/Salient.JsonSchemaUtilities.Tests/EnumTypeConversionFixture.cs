using System;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Salient.JsonSchemaUtilities.Tests
{
    /// <summary>
    /// NOTE: the json-schema spec does not provide a clean way to fully represent a dot net enum so this 
    /// description is extended with a custom 'options' array
    /// </summary>
    [TestFixture]
    public class EnumTypeConversionFixture : ConversionVerificationFixtureBase
    {
        [Test]
        public void VerifyFlagsEnum()
        {
            Type typeToTest = typeof(PaletteFlags);
            var typeExpected = new JObject(
                new JProperty("type", "#/PaletteFlags")
                ).ToString();

            var schemaExpected = new JObject(
                new JProperty("PaletteFlags",
                    new JObject(new JProperty("id", "#/PaletteFlags"),
                        new JProperty("type", "number"),
                        new JProperty("format", "bitmask"),
                        new JProperty("enum", new JArray(1, 2, 4)),
                        new JProperty("options", new JArray(
                            new JObject(new JProperty("value", 1), new JProperty("label", "HasAlpha")),
                            new JObject(new JProperty("value", 2), new JProperty("label", "GrayScale")),
                            new JObject(new JProperty("value", 4), new JProperty("label", "Halftone"))
                    ))))
                ).ToString();

            const bool flatten = false;
            var schema = new JObject();
            string actual = GetActual(typeToTest, typeExpected, flatten, schema);

            Assert.AreEqual(typeExpected, actual);

            Console.WriteLine("SCHEMA\n" + schema.ToString(Formatting.Indented));
            Assert.AreEqual(schemaExpected, schema.ToString(Formatting.Indented));
        }
        [Test]
        public void VerifyEnum()
        {
            Type typeToTest = typeof(Duplex);
            var typeExpected = new JObject(new JProperty("type", "#/Duplex")).ToString();
            var schemaExpected = new JObject(
                    new JProperty("Duplex", 
                        new JObject(
                            new JProperty("id", "#/Duplex"),
                            new JProperty("type", "number"),
                            new JProperty("enum", new JArray(1, 2, 3, -1)),
                            new JProperty("options", new JArray(
                                new JObject(new JProperty("value", 1), new JProperty("label", "Simplex")),
                                new JObject(new JProperty("value", 2), new JProperty("label", "Vertical")),
                                new JObject(new JProperty("value", 3), new JProperty("label", "Horizontal")),
                                new JObject(new JProperty("value", -1), new JProperty("label", "Default"))
                        ))))

                    ).ToString();

            const bool flatten = false;
            var schema = new JObject();
            string actual = GetActual(typeToTest, typeExpected, flatten, schema);

            Assert.AreEqual(typeExpected, actual);

            Console.WriteLine("SCHEMA\n" + schema.ToString(Formatting.Indented));
            Assert.AreEqual(schemaExpected, schema.ToString(Formatting.Indented));
        }



    }
}