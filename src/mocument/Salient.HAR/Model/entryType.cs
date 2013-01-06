using Newtonsoft.Json;

namespace Salient.HAR.Model
{
    /// <summary>
    /// Request and Response related info
    /// </summary>
    public class EntryType
    {
        [JsonProperty(PropertyName = "pageref", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Pageref;
    
        /// <summary>
        /// http://www.w3.org/TR/NOTE-datetime ISO 8601 - YYYY-MM-DDThh:mm:ss.sTZD, e.g. 2009-07-24T19:20:30.45+01:00
        /// </summary>
        [JsonProperty(PropertyName = "startedDateTime")] 
        public string StartedDateTime;
        [JsonProperty(PropertyName = "time")] 
        public int Time;        
        [JsonProperty(PropertyName = "request")] 
        public RequestType Request=new RequestType();

        [JsonProperty(PropertyName = "response")] 
        public ResponseType Response =new ResponseType();
        [JsonProperty(PropertyName = "cache")] 
        public CacheType Cache = new CacheType();
        
        [JsonProperty(PropertyName = "timings")] 
        public TimingsType Timings =new TimingsType();

        [JsonProperty(PropertyName = "serverIPAddress", DefaultValueHandling = DefaultValueHandling.Ignore)] 
        public string ServerIPAddress;
        [JsonProperty(PropertyName = "connection", DefaultValueHandling = DefaultValueHandling.Ignore)] 
        public string Connection;






        [JsonProperty(PropertyName = "comment", DefaultValueHandling = DefaultValueHandling.Ignore)] 
        public string Comment = null;



    }
}


//var entryType = {  
//    "entryType": {  
//        "id": "entryType",  
//        "description": "Request and Response related info",  
//        "optional": true,  
//        "properties": {  
//            "pageref": {"type": "string", "optional": true},  
//            "startedDateTime": {"type": "string", "format": "date-time", "pattern": dateTimePattern},  
//            "time": {"type": "integer", "min": 0},  
//            "request" : {"$ref": "requestType"},  
//            "response" : {"$ref": "responseType"},  
//            "cache" : {"$ref": "cacheType"},  
//            "timings" : {"$ref": "timingsType"},  
//            "serverIPAddress" : {"type": "string", "optional": true},  
//            "connection" : {"type": "string", "optional": true},  
//            "comment": {"type": "string", "optional": true}  
//        }  
//    }  
//};  
//  