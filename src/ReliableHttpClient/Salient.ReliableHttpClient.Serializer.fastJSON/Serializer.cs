using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using fastJSON;


namespace Salient.ReliableHttpClient.Serialization.fastJSON
{
    public class Serializer : IJsonSerializer
    {


        public string SerializeObject(object value)
        {
            var s = new JSONSerializer(true, true, true, true, true);
            string result = s.ConvertToJSON(value);
            return result;




        }

        public T DeserializeObject<T>(string json)
        {
            var s = new JsonParser(json);
            T result = (T) s.Decode();
            return result;
        }
    }
}
