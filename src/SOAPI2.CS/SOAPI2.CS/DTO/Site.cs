using System;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace SOAPI2.CS.DTO
{
    public class Site
    {
        [JsonProperty("site_type")]
        public string SiteType { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("logo_url")]
        public Uri LogoUrl { get; set; }
        [JsonProperty("api_site_parameter")]
        public string ApiSiteParameter { get; set; }
        [JsonProperty("site_url")]
        public Uri SiteUrl { get; set; }
        [JsonProperty("audience")]
        public string Audience { get; set; }
        [JsonProperty("icon_url")]
        public Uri IconUrl { get; set; }
        [JsonProperty("aliases")]
        public Uri[] Aliases { get; set; }
        [JsonProperty("site_state")]
        public string SiteState { get; set; }
        [JsonProperty("styling")]
        public Styling Styling { get; set; }
        // TODO: date converter
        [JsonProperty("launch_date")]
        public string LaunchDate { get; set; }
        [JsonProperty("favicon_url")]
        public Uri FaviconUrl { get; set; }
        [JsonProperty("related_sites")]
        public Site[] RelatedSites { get; set; }
        [JsonProperty("markdown_extensions")]
        public string[] MarkdownExtensions { get; set; }
        [JsonProperty("relation")]
        public string Relation { get; set; }
    }
}
