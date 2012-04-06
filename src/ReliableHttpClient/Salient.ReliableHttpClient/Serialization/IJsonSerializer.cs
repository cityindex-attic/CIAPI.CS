using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Salient.ReliableHttpClient.Serialization
{
    public interface IJsonSerializer
    {

        string SerializeObject(object value);
        T DeserializeObject<T>(string json);
    }



}

namespace Salient.ReliableHttpClient.Serialization.Defaults
{
    public class Serializer : IJsonSerializer
    {


        public string SerializeObject(object value)
        {
      

            var serializer = new DataContractJsonSerializer(value.GetType());

            byte[] bytes;
            using (var ms = new MemoryStream())
            {
                serializer.WriteObject(ms, value);
                bytes = ms.ToArray();
            }
            var json = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
            return json;
        }


        public T DeserializeObject<T>(string json)
        {
            using (MemoryStream stream = new MemoryStream(Encoding.Unicode.GetBytes(json)))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));

                T obj = (T)serializer.ReadObject(stream);
                return obj;
            }
        }
    }
}
