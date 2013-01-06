using Newtonsoft.Json;

namespace Salient.HAR.Model
{
    /// <summary>
    /// Monitored Response.
    /// </summary>
    public class ResponseType
    {
        [JsonProperty(PropertyName = "status")] 
        public int Status;
        [JsonProperty(PropertyName = "statusText")] 
        public string StatusText;
        [JsonProperty(PropertyName = "httpVersion")] 
        public string HTTPVersion;
        [JsonProperty(PropertyName = "cookies")] 
        public CookieType[] Cookies = new CookieType[]{};
        [JsonProperty(PropertyName = "headers")] 
        public RecordType[] Headers = new RecordType[]{};
        [JsonProperty(PropertyName = "content")] 
        public ContentType Content = new ContentType();
        [JsonProperty(PropertyName = "redirectURL")] 
        public string RedirectURL;

        [JsonProperty(PropertyName = "headersSize")]
        public int HeadersSize;
        [JsonProperty(PropertyName = "bodySize")] 
        public int BodySize;

        [JsonProperty(PropertyName = "comment", DefaultValueHandling = DefaultValueHandling.Ignore)] 
        public string Comment = null;








       
    }
}


//  
//var responseType = {  
//    "responseType": {  
//        "id": "responseType",  
//        "description": "Monitored Response.",  
//        "properties": {  
//            "status": {"type": "integer"},  
//            "statusText": {"type": "string"},  
//            "httpVersion": {"type": "string"},  
//            "cookies" : {"type": "array", "items": {"$ref": "cookieType"}},  
//            "headers" : {"type": "array", "items": {"$ref": "recordType"}},  
//            "content" : {"$ref": "contentType"},  
//            "redirectURL" : {"type": "string"},  
//            "headersSize" : {"type": "integer"},  
//            "bodySize" : {"type": "integer"},  
//            "comment": {"type": "string", "optional": true}  
//        }  
//    }  
//};  