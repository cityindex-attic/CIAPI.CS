using Newtonsoft.Json;

namespace Salient.HAR.Model
{
    /// <summary>
    /// Info about cache entry.
    /// </summary>
    public class CacheEntryType
    {
        [JsonProperty(PropertyName = "expires", DefaultValueHandling = DefaultValueHandling.Ignore)] 
        public string Expires;
        [JsonProperty(PropertyName = "lastAccess")] 
        public string LastAccess;
        [JsonProperty(PropertyName = "eTag")] 
        public string ETag;


        [JsonProperty(PropertyName = "hitCount")] 
        public int HitCount;
        [JsonProperty(PropertyName = "comment", DefaultValueHandling = DefaultValueHandling.Ignore)] 
        public string Comment = null;


    }
}
//var cacheEntryType = {  
//    "cacheEntryType": {  
//        "id": "cacheEntryType",  
//        "optional": true,  
//        "description": "Info about cache entry.",  
//        "properties": {  
//            "expires": {"type": "string", optional: "true"},  
//            "lastAccess": {"type": "string"},  
//            "eTag": {"type": "string"},  
//            "hitCount": {"type": "integer"},  
//            "comment": {"type": "string", "optional": true}  
//        }  
//    }  
//};  
//  
