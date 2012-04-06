using System;
using Newtonsoft.Json.Linq;

namespace Salient.JsonSchemaUtilities.Tests
{
    public class ConversionVerificationFixtureBase
    {
        public static string GetActual(Type typeToTest, string expected, bool flatten, JObject schema = null)
        {
            var converter = new ModelGenerator();
            var target = new JObject();

            converter.AssignSchemaType(typeToTest, ref target, ref schema, flatten);

            var actual = target.ToString();
            Console.WriteLine("type:{0}\nexpected:\n{1}", typeToTest.FullName, expected);
            Console.WriteLine("actual :\n{0}", actual);
            return actual;
        }
    }
}