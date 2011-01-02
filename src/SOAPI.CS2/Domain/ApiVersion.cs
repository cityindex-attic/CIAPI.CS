using Newtonsoft.Json;

namespace SOAPI.CS2.Domain
{
    /// <summary>
    /// </summary>
    public class ApiVersion
    {
        /// <summary>
        ///   site revision
        /// </summary>
        [JsonProperty("revision")]
        public string Revision { get; set; }

        /// <summary>
        ///   api version
        /// </summary>
        [JsonProperty("version")]
        public string Version { get; set; }
    }
}