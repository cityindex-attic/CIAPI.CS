using Newtonsoft.Json;

namespace SOAPI2.CS.DTO
{
    public class CommonResponseBase
    {
        // All responses in the Stack Exchange API share a common format, so as to make parsing these responses simpler.
        // 
        // The error_* fields, while technically elligible for filtering, will not actually be excluded in an error case. This is by design.
        // 
        // page and page_size are whatever was passed into the method.
        // 
        // If you're looking to just select total, exclude the items field in favor of excluding all the properties on the returned type.
        // 
        // When building filters, this common wrapper object has no name. Refer to it with a leading ., so the items field would be refered to via .items.
        // 
        // The backoff field is only set when the API detects the request took an unusually long time to run. When it is set an application must wait that number of seconds before calling that method again. Note that for the purposes of throttling and backoff, the /me routes are considered the same as their /users/{ids} equivalent.


        [JsonProperty("backoff")]
        public int Backoff { get; set; }
        [JsonProperty("error_id")]
        public int ErrorId { get; set; }
        [JsonProperty("error_message")]
        public string ErrorMessage { get; set; }
        [JsonProperty("error_name")]
        public string ErrorName { get; set; }
        [JsonProperty("has_more")]
        public bool HasMore { get; set; }
        [JsonProperty("page")]
        public int Page { get; set; }
        [JsonProperty("page_size")]
        public int PageSize { get; set; }
        [JsonProperty("quota_max")]
        public int QuotaMax { get; set; }
        [JsonProperty("quota_remaining")]
        public int QuotaRemaining { get; set; }
        [JsonProperty("total")]
        public int Total { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
    }
    public class CommonResponse<TDTO> : CommonResponseBase where TDTO : class,new()
    {
    
        [JsonProperty("items")]
        public TDTO[] Items { get; set; }


        
    }
}

