using Newtonsoft.Json;

namespace Salient.HAR.Model
{
    /// <summary>
    /// Cookie description
    /// </summary>
    public class CookieType
    {

        [JsonProperty(PropertyName = "name")] 
        public string Name;
        
        [JsonProperty(PropertyName = "value")] 
        public string Value;
        [JsonProperty(PropertyName = "path", DefaultValueHandling = DefaultValueHandling.Ignore)] 
        public string Path;
        [JsonProperty(PropertyName = "domain", DefaultValueHandling = DefaultValueHandling.Ignore)] 
        public string Domain;

        [JsonProperty(PropertyName = "expires", DefaultValueHandling = DefaultValueHandling.Ignore)] 
        public string Expires;

        [JsonProperty(PropertyName = "httpOnly", DefaultValueHandling = DefaultValueHandling.Ignore)] 
        public bool HTTPOnly;


        [JsonProperty(PropertyName = "secure", DefaultValueHandling = DefaultValueHandling.Ignore)] 
        public bool Secure;
        
        [JsonProperty(PropertyName = "comment", DefaultValueHandling = DefaultValueHandling.Ignore)] 
        public string Comment = null;
    }
}


//  
//var cookieType = {  
//    "cookieType": {  
//        "id": "cookieType",  
//        "description": "Cookie description.",  
//        "properties": {  
//            "name": {"type": "string"},  
//            "value": {"type": "string"},  
//            "path": {"type": "string", "optional": true},  
//            "domain" : {"type": "string", "optional": true},  
//            "expires" : {"type": "string", "optional": true},  
//            "httpOnly" : {"type": "boolean", "optional": true},  
//            "secure" : {"type": "boolean", "optional": true},  
//            "comment": {"type": "string", "optional": true}  
//        }  
//    }  
//}  
//  