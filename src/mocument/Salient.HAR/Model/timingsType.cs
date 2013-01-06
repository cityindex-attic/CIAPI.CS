using Newtonsoft.Json;

namespace Salient.HAR.Model
{
    /// <summary>
    /// Info about request-response timing.
    /// </summary>
    public class TimingsType
    {
        [JsonProperty(PropertyName = "dns", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? DNS = null;
        [JsonProperty(PropertyName = "connect", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? Connect = null;
        [JsonProperty(PropertyName = "blocked", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? Blocked = null;


        [JsonProperty(PropertyName = "send")]
        public int Send ;


        [JsonProperty(PropertyName = "wait")]
        public int Wait ;
        [JsonProperty(PropertyName = "receive")]
        public int  Receive ;

        [JsonProperty(PropertyName = "ssl", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? SSL = null;




        [JsonProperty(PropertyName = "comment", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Comment = null;

        //var timingsType = {  
        //    "timingsType": {  
        //        "id": "timingsType",  
        //        "description": "Info about request-response timing.",  
        //        "properties": {  
        //            "dns": {"type": "integer", "optional": true, "min": -1},  
        //            "connect": {"type": "integer", "optional": true, "min": -1},  
        //            "blocked": {"type": "integer", "optional": true, "min": -1},  
        //            "send": {"type": "integer", "min": -1},  
        //            "wait": {"type": "integer", "min": -1},  
        //            "receive": {"type": "integer", "min": -1},  
        //            "ssl": {"type": "integer", "optional": true, "min": -1},  
        //            "comment": {"type": "string", "optional": true}  
        //        }  
        //    }  
        //};  
    }
}