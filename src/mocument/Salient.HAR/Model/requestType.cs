using Newtonsoft.Json;

namespace Salient.HAR.Model
{
    /// <summary>
    /// Monitored request
    /// </summary>
    public class RequestType
    {
        [JsonProperty(PropertyName = "method")] 
        public string Method;

        [JsonProperty(PropertyName = "url")] 
        public string URL;

        [JsonProperty(PropertyName = "httpVersion")] 
        public string HTTPVersion;
        [JsonProperty(PropertyName = "cookies")] 
        public CookieType[] Cookies=new CookieType[]{};
        [JsonProperty(PropertyName = "headers")] 
        public RecordType[] Headers=new RecordType[]{};
        [JsonProperty(PropertyName = "queryString")] 
        public RecordType[] QueryString = new RecordType[]{};

        [JsonProperty(PropertyName = "postData", DefaultValueHandling = DefaultValueHandling.Ignore)] 
        public PostDataType PostData = null;

        [JsonProperty(PropertyName = "headersSize")]
        public int HeadersSize;

        [JsonProperty(PropertyName = "bodySize")] 
        public int BodySize;

        [JsonProperty(PropertyName = "comment", DefaultValueHandling = DefaultValueHandling.Ignore)] 
        public string Comment = null;





        //var requestType = {  
        //    "requestType": {  
        //        "id": "requestType",  
        //        "description": "Monitored request",  
        //        "properties": {  
        //            "method": {"type": "string"},  
        //            "url": {"type": "string"},  
        //            "httpVersion": {"type" : "string"},  
        //            "cookies" : {"type": "array", "items": {"$ref": "cookieType"}},  
        //            "headers" : {"type": "array", "items": {"$ref": "recordType"}},  
        //            "queryString" : {"type": "array", "items": {"$ref": "recordType"}},  
        //            "postData" : {"$ref": "postDataType"},  
        //            "headersSize" : {"type": "integer"},  
        //            "bodySize" : {"type": "integer"},  
        //            "comment": {"type": "string", "optional": true}  
        //        }  
        //    }  
        //};  
    }
}