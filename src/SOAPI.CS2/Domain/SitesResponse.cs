using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace SOAPI.CS2.Domain
{
    public class SitesResponse
    {
        [JsonProperty("api_sites")]
        public List<Site> ApiSites { get; set; }
    }
}
