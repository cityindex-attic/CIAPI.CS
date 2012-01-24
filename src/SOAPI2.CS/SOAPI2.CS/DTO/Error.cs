using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace SOAPI2.CS.DTO
{
    public class Error
    {
        [JsonProperty("error_id")]
        public int ErrorId { get; set; }
        [JsonProperty("error_name")]
        public string ErrorName { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
