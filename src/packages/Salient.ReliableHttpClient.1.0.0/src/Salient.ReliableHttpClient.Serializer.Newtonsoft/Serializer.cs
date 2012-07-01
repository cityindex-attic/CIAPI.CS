using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace Salient.ReliableHttpClient.Serialization.Newtonsoft
{
    public class Serializer : Salient.ReliableHttpClient.Serialization.IJsonSerializer
    {
        public string SerializeObject(object value)
        {
            return JsonConvert.SerializeObject(value);
        }

        public T DeserializeObject<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
