using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;


namespace Salient.ReliableHttpClient.Serialization.SystemWeb
{
    public class Serializer : IJsonSerializer
    {


        public string SerializeObject(object value)
        {
            string result = new JavaScriptSerializer().Serialize(value);
            return result;




        }

        public T DeserializeObject<T>(string json)
        {
            
            T result = new JavaScriptSerializer().Deserialize<T>(json);
            return result;
        }
    }
}
