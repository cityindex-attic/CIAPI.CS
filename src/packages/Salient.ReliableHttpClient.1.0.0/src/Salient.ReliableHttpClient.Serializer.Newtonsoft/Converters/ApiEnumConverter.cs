using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Salient.ReliableHttpClient.Serialization.Newtonsoft
{
    /// <summary>
    /// Converts an <see cref="Enum"/> to and from its name string value.
    /// </summary>
    public class ApiEnumConverter : StringEnumConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            using (var s = new StringWriter())
            {
                using (var w = new JsonTextWriter(s))
                {
                    base.WriteJson(w, value, serializer);
                }
                writer.WriteValue(s.ToString().ToLower().Trim('"'));
            }
        }
    }
}