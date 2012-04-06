using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Salient.ReliableHttpClient.Serialization.JSON
{
    public class Serializer : IJsonSerializer
    {


        public string SerializeObject(object value)
        {

            string result = Json.JsonParser.Serialize(value);
            return result;




        }

        public T DeserializeObject<T>(string json)
        {
            T result = Json.JsonParser.Deserialize<T>(json);
            return result;
        }
    }
}
