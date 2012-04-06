using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Salient.ReliableHttpClient.Serialization.JayRock
{
    public class Serializer : IJsonSerializer
    {


        public string SerializeObject(object value)
        {

            string result = Jayrock.Json.Conversion.JsonConvert.ExportToString(value);
            return result;




        }

        public T DeserializeObject<T>(string json)
        {
            T result = Jayrock.Json.Conversion.JsonConvert.Import<T>(json);
            return result;
        }
    }
}
