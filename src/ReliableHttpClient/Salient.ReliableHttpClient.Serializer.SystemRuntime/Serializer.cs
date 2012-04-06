using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;


namespace Salient.ReliableHttpClient.Serialization.SystemRuntime
{
    public class Serializer : IJsonSerializer
    {


        public string SerializeObject(object value)
        {

            var serializer = new DataContractJsonSerializer(value.GetType());
            string json;
            using (var ms = new MemoryStream())
            {
                serializer.WriteObject(ms, value);
                json = Encoding.Default.GetString(ms.ToArray());
            }

            return json;


        }

        public T DeserializeObject<T>(string json)
        {
            MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(json));
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            T result = (T)serializer.ReadObject(ms);
            ms.Close();
            return result;
        }
    }
}
