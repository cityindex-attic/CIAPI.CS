using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Salient.ReliableHttpClient.Serialization.ServiceStack
{
    public class Serializer : IJsonSerializer
    {


        public string SerializeObject(object value)
        {
            string result = 
            return result;




        }

        public T DeserializeObject<T>(string json)
        {
            T result = TypeSerializer.DeserializeFromString<T>(json);
            return result;
        }
    }
}
