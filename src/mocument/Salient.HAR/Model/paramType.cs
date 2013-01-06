using Newtonsoft.Json;

namespace Salient.HAR.Model
{
    public class ParamType
    {



        [JsonProperty(PropertyName = "name")] 
        public string Name;

        [JsonProperty(PropertyName = "value")] 
        public string Value;
        [JsonProperty(PropertyName = "fileName", DefaultValueHandling = DefaultValueHandling.Ignore)] 
        public string FileName;
        [JsonProperty(PropertyName = "contentType", DefaultValueHandling = DefaultValueHandling.Ignore)] 
        public string ContentType;
        [JsonProperty(PropertyName = "comment", DefaultValueHandling = DefaultValueHandling.Ignore)] 
        public string Comment = null;
    }
}

//            "params": {  
//                "type": "array",  
//                "optional": true,  
//                "properties": {  
//                    "name": {"type": "string"},  
//                    "value": {"type": "string", "optional": true},  
//                    "fileName": {"type": "string", "optional": true},  
//                    "contentType": {"type": "string", "optional": true},  
//                    "comment": {"type": "string", "optional": true}  
//                }  
//            },  