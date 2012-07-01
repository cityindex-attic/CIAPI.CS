using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Salient.JsonSchemaUtilities.Tests
{
    public enum SimpleEnum
    {
        Foo,
        Bar
    }
    public class SimpleClass
    {
        public string StringProp { get; set; }
        public bool BooleanProp { get; set; }
        public SimpleEnum SimpleEnumProp { get; set; }
        public SimpleEnum SimpleEnumProp2 { get; set; }

    }

    public class DerivedClass : SimpleClass
    {
        public XmlTypeMapping DerivedObjectProp { get; set; }
        public string DerivedStringProp { get; set; }
        public PaletteFlags SystemFlagsEnum { get; set; }
    }
    [TestFixture]
    public class MetaDataFixture
    {
        [Test]
        public void SimpleSchema()
        {
            var generator = new ModelGenerator();
            var schema = new JObject();
            const bool flatten = false;
            generator.EmitType(typeof(SimpleClass), ref schema, flatten);

            Console.WriteLine(schema.ToString());
        }

        [Test]
        public void DerivedSchema()
        {
            var generator = new ModelGenerator();
            var schema = new JObject();
            const bool flatten = false;
            generator.EmitType(typeof(DerivedClass), ref schema, flatten);

            Console.WriteLine(schema.ToString());
        }
    }
}
