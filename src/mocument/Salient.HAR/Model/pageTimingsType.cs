using Newtonsoft.Json;

namespace Salient.HAR.Model
{
    /// <summary>
    /// Timing info about page load
    /// </summary>
    public class PageTimingsType
    {


        [JsonProperty(PropertyName = "onContentLoad", DefaultValueHandling = DefaultValueHandling.Ignore)] 
        public int? OnContentLoad;

        [JsonProperty(PropertyName = "onLoad", DefaultValueHandling = DefaultValueHandling.Ignore)] 
        public int? OnLoad;
        
        [JsonProperty(PropertyName = "comment", DefaultValueHandling = DefaultValueHandling.Ignore)] 
        public string Comment = null;

    }
}


//var pageTimingsType = {  
//    "pageTimingsType": {  
//        "id": "pageTimingsType",  
//        "description": "Timing info about page load",  
//        "properties": {  
//            "onContentLoad": {"type": "number", "optional": true, "min": -1},  
//            "onLoad": {"type": "number", "optional": true, "min": -1},  
//            "comment": {"type": "string", "optional": true}  
//        }  
//    }  
//};  
//  