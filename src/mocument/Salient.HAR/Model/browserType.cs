using Newtonsoft.Json;

namespace Salient.HAR.Model
{
    /// <summary>
    /// Name and version info of used browser.
    /// </summary>
    public class BrowserType
    {

        [JsonProperty(PropertyName = "name")] 
        public string Name;

        [JsonProperty(PropertyName = "version")] 
        public string Version;

        [JsonProperty(PropertyName = "comment", DefaultValueHandling = DefaultValueHandling.Ignore)] 
        public string Comment = null;
    }
}


//var browserType = {  
//    "browserType": {  
//        "id": "browserType",  
//        "description": "Name and version info of used browser.",  
//        "type": "object",  
//        "optional": true,  
//        "properties": {  
//            "name": {"type": "string"},  
//            "version": {"type": "string"},  
//            "comment": {"type": "string", "optional": true}  
//        }  
//    }  
//};  
//  