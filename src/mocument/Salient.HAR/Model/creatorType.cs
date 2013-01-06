using Newtonsoft.Json;

namespace Salient.HAR.Model
{
    /// <summary>
    /// Name and version info of the log creator app.
    /// </summary>
    public class CreatorType
    {


        [JsonProperty(PropertyName = "name")] 
        public string Name;

        [JsonProperty(PropertyName = "version")] 
        public string Version;        
        
        [JsonProperty(PropertyName = "comment", DefaultValueHandling = DefaultValueHandling.Ignore)] 
        public string Comment = null;
    }
}


//var creatorType = {  
//    "creatorType": {  
//        "id": "creatorType",  
//        "description": "Name and version info of the log creator app.",  
//        "type": "object",  
//        "properties": {  
//            "name": {"type": "string"},  
//            "version": {"type": "string"},  
//            "comment": {"type": "string", "optional": true}  
//        }  
//    }  
//};  
//  