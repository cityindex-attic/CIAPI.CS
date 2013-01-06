using Newtonsoft.Json;

namespace Salient.HAR.Model
{
    /// <summary>
    /// Helper name-value pair structure.
    /// </summary>
    public class RecordType
    {

        [JsonProperty(PropertyName = "name")] 
        public string Name;

        [JsonProperty(PropertyName = "value")] 
        public string Value;
        [JsonProperty(PropertyName = "comment", DefaultValueHandling = DefaultValueHandling.Ignore)] 
        public string Comment = null;

    }
}


//  
//var recordType = {  
//    "recordType": {  
//        "id": "recordType",  
//        "description": "Helper name-value pair structure.",  
//        "properties": {  
//            "name": {"type": "string"},  
//            "value": {"type": "string"},  
//            "comment": {"type": "string", "optional": true}  
//        }  
//    }  
//};  