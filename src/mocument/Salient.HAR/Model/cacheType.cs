using Newtonsoft.Json;
using Salient.HAR.Serializatoin.Converters;

namespace Salient.HAR.Model
{
    /// <summary>
    /// Info about a response coming from the cache.
    /// </summary>
    /// <remarks>
    /// this class needs some special treatment for serialization. very 'interesting' approach taken by spec author
    /// http://www.softwareishard.com/blog/har-12-spec/#cache
    /// </remarks>
    //[JsonConverter(typeof(CacheTypeConverter ))]
    public class CacheType
    {
        [JsonProperty(PropertyName = "beforeRequest", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public CacheEntryType BeforeRequest;
        [JsonProperty(PropertyName = "afterRequest", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public CacheEntryType AfterRequest;


        [JsonProperty(PropertyName = "comment", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Comment = null;
    }
}

//var cacheType = {  
//    "cacheType": {  
//        "id": "cacheType",  
//        "description": "Info about a response coming from the cache.",  
//        "properties": {  
//            "beforeRequest": {"$ref": "cacheEntryType"},  
//            "afterRequest": {"$ref": "cacheEntryType"},  
//            "comment": {"type": "string", "optional": true}  
//        }  
//    }  
//};  
//  

