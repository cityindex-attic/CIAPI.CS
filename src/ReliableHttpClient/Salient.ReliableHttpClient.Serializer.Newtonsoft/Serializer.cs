using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

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
            try
            {
                string pattern = "{\\s*\"DateTime\":\\s*\"\\\\/Date\\((?<dt>\\d+)\\)\\\\/\",\\s*\"OffsetMinutes\":\\s*(?<offset>-?\\d+)\\s*}";
                json = Regex.Replace(json, pattern, "\"\\/Date($1+0100)\\/\"");


            }
            catch 
            {
                
                //swallow for now
            }

            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
