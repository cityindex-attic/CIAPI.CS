using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Salient.HAR.Serializatoin.Converters
{
    public class CacheTypeConverter:JsonConverter 
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof (CacheTypeConverter);
        }
    }
}
