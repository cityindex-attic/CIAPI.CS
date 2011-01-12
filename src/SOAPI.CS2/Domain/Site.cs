using System;
using System.Collections.Generic;
using System.Linq;
using CityIndex.JsonClient.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


namespace SOAPI.CS2.Domain
{
    ///<summary>
    ///</summary>
    public class Site 
    {
        private Statistics _statistics = new Statistics();

        public Site()
        {
            Styling = new Styling();
            Aliases = new List<string>();
        }

        #region Properties

        ///<summary>
        ///  various system statistics. 
        ///</summary>
        public Statistics Statistics { get; set; }

        /// <summary>
        ///   Other identities (usage TBD)
        /// </summary>
        [JsonProperty("aliases")]
        public List<string> Aliases { get; set; }

        /// <summary>
        ///   absolute path to the api endpoint for the site, sans the version string
        /// </summary>
        [JsonProperty("api_endpoint")]
        public string ApiEndpoint { get; set; }

        /// <summary>
        ///   description of the site, suitable for display to a user
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        ///   absolute path to an icon suitable for representing the site, it is a consumers responsibility to scale
        /// </summary>
        [JsonProperty("icon_url")]
        public string IconUrl { get; set; }

        /// <summary>
        ///   absolute path to the logo for the site
        /// </summary>
        [JsonProperty("logo_url")]
        public string LogoUrl { get; set; }

        /// <summary>
        ///   name of the site
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        ///   absolute path to the front page of the site
        /// </summary>
        [JsonProperty("site_url")]
        public string SiteUrl { get; set; }

        /// <summary>
        ///   state of this site.
        /// </summary>
        [JsonProperty("state"), JsonConverter(typeof(ApiEnumConverter))]
        public SiteState State { get; set; }

        ///<summary>
        ///</summary>
        [JsonProperty("styling")]
        public Styling Styling { get; set; }

        [JsonProperty("rate_limit_current")]
        public int RateLimitCurrent { get; set; }

        [JsonProperty("rate_limit_max")]
        public int RateLimitMax { get; set; }

        #endregion


        


        

    }
}
