using Newtonsoft.Json;

namespace Salient.HAR.Model
{
    /// <summary>
    /// Posted data info.
    /// </summary>
    public class PostDataType
    {


        [JsonProperty(PropertyName = "mimeType")] 
        public string MIMEType;

        [JsonProperty(PropertyName = "text", DefaultValueHandling = DefaultValueHandling.Ignore)] 
        public string Text;
        [JsonProperty(PropertyName = "params", DefaultValueHandling = DefaultValueHandling.Ignore)] 
        public ParamType[] Params;
        [JsonProperty(PropertyName = "comment", DefaultValueHandling = DefaultValueHandling.Ignore)] 
        public string Comment = null;

  
    }
}


//var postDataType = {  
//    "postDataType": {  
//        "id": "postDataType",  
//        "description": "Posted data info.",  
//        "optional": true,  
//        "properties": {  
//            "mimeType": {"type": "string"},  
//            "text": {"type": "string", "optional": true},  
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
//            "comment": {"type": "string", "optional": true}  
//        }  
//    }  
//};  