using Newtonsoft.Json;

namespace Salient.HAR.Model
{
    /// <summary>
    /// Exported web page
    /// </summary>
    public class PageType
    {
        /// <summary>
        /// http://www.w3.org/TR/NOTE-datetime ISO 8601 - YYYY-MM-DDThh:mm:ss.sTZD, e.g. 2009-07-24T19:20:30.45+01:00
        /// </summary>
        [JsonProperty(PropertyName = "startedDateTime")] 
        public string StartedDateTime;

        [JsonProperty(PropertyName = "id")] public string Id;
      
        [JsonProperty(PropertyName = "title")] 
        public string Title;
        [JsonProperty(PropertyName = "pageTimings")] 
        public PageTimingsType PageTimings =new PageTimingsType();

  

        [JsonProperty(PropertyName = "comment", DefaultValueHandling = DefaultValueHandling.Ignore)] 
        public string Comment = null;
    }
}



//var pageType = {  
//    "pageType": {  
//        "id": "pageType",  
//        "description": "Exported web page",  
//        "optional": true,  
//        "properties": {  
//            "startedDateTime": {"type": "string", "format": "date-time", "pattern": dateTimePattern},  
//            "id": {"type": "string", "unique": true},  
//            "title": {"type": "string"},  
//            "pageTimings": {"$ref": "pageTimingsType"},  
//            "comment": {"type": "string", "optional": true}  
//        }  
//    }  
//};  
//  