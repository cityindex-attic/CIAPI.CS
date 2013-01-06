using Newtonsoft.Json;

namespace Salient.HAR.Model
{
    /// <summary>
    /// HTTP Archive structure
    /// </summary>
    public class LogType
    {
        [JsonProperty(PropertyName = "version")] 
        public string Version;

        [JsonProperty(PropertyName = "creator")]
        public CreatorType Creator = new CreatorType();
        
        [JsonProperty(PropertyName = "browser")] 
        public BrowserType Browser;


        [JsonProperty(PropertyName = "pages", DefaultValueHandling = DefaultValueHandling.Ignore)] 
        public PageType[] Pages;

        
        [JsonProperty(PropertyName = "entries")] 
        public EntryType[] Entries =new EntryType[]{};


        [JsonProperty(PropertyName = "comment", DefaultValueHandling = DefaultValueHandling.Ignore)] 
        public string Comment = null;
    }
}

//var logType = {  
//    "logType": {  
//        "id": "logType",  
//        "description": "HTTP Archive structure.",  
//        "type": "object",  
//        "properties": {  
//            "log": {  
//                "type": "object",  
//                "properties": {  
//                    "version": {"type": "string"},  
//                    "creator": {"$ref": "creatorType"},  
//                    "browser": {"$ref": "browserType"},  
//                    "pages": {"type": "array", "optional": true, "items": {"$ref": "pageType"}},  
//                    "entries": {"type": "array", "items": {"$ref": "entryType"}},  
//                    "comment": {"type": "string", "optional": true}  
//                }  
//            }  
//        }  
//    }  
//};  
//  