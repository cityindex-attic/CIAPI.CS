using Salient.ReliableHttpClient.Serialization;

namespace Salient.ReliableHttpClient.Serializer.SystemJson
{
    public class Serializer : IJsonSerializer
    {


        public string SerializeObject(object value)
        {
            System.Json.JsonObject.
            string result = TypeSerializer.SerializeToString(value);
            return result;




        }

        public T DeserializeObject<T>(string json)
        {
            T result = TypeSerializer.DeserializeFromString<T>(json);
            return result;
        }
    }
}
