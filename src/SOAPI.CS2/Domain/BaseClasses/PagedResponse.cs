using System.Collections.Generic;
using Newtonsoft.Json;

namespace SOAPI.CS2.Domain
{
    public abstract class PagedResponse<TDTO> : CollectionResponse<TDTO> 
    {
        [JsonProperty("total")]
        public int Total { get; set; }
        [JsonProperty("page")]
        public int Page { get; set; }
        [JsonProperty("pagesize")]
        public int PageSize { get; set; }
    }
}