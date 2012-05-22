using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace CIAPI.Serialization
{
    public class Serializer : Salient.ReliableHttpClient.Serialization.IJsonSerializer
    {


        public string SerializeObject(object value)
        {
            //Json.NET 4.5 changed the default dateformatting to ISO 8601 - http://james.newtonking.com/archive/2012/03/20/json-net-4-5-release-1-iso-dates-async-metro-build.aspx
            //Change it back to the previous default of MS style dates i.e - /Date(19372927629377)/
            return JsonConvert.SerializeObject(value, new JsonSerializerSettings { DateFormatHandling = DateFormatHandling.MicrosoftDateFormat, Formatting = Formatting.Indented });
        }

        public T DeserializeObject<T>(string json)
        {
             return JsonConvert.DeserializeObject<T>(json);
        }
    }
}