using Newtonsoft.Json;

namespace Salient.HAR.Model
{
    /// <summary>
    /// Response content
    /// </summary>
    public class ContentType
    {
        [JsonProperty(PropertyName = "size")] 
        public int Size;

        [JsonProperty(PropertyName = "compression", DefaultValueHandling = DefaultValueHandling.Ignore)] 
        public int Compression;

        [JsonProperty(PropertyName = "mimeType")] 
        public string MIMEType;
        [JsonProperty(PropertyName = "text", DefaultValueHandling = DefaultValueHandling.Ignore)] 
        public string Text;
        [JsonProperty(PropertyName = "encoding", DefaultValueHandling = DefaultValueHandling.Ignore)] 
        public string Encoding;



        [JsonProperty(PropertyName = "comment", DefaultValueHandling = DefaultValueHandling.Ignore)] 
        public string Comment = null;
    }
}


//  
//var contentType = {  
//    "contentType": {  
//        "id": "contentType",  
//        "description": "Response content",  
//        "properties": {  
//            "size": {"type": "integer"},  
//            "compression": {"type": "integer", "optional": true},  
//            "mimeType": {"type": "string"},  
//            "text": {"type": "string", "optional": true},  
//            "encoding": {"type": "string", "optional": true},  
//            "comment": {"type": "string", "optional": true}  
//        }  
//    }  
//};  
//  
