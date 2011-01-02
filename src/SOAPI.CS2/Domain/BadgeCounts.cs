using Newtonsoft.Json;

namespace SOAPI.CS2.Domain
{
    /// <summary>
    /// </summary>
    public class BadgeCounts
    {

        /// <summary>
        ///   number of bronze badges received
        /// </summary>
        [JsonProperty("bronze")]
        public int? Bronze { get; set; }

        /// <summary>
        ///   number of gold badges received
        /// </summary>
        [JsonProperty("gold")]
        public int? Gold { get; set; }

        /// <summary>
        ///   number of silver badges received
        /// </summary>
        [JsonProperty("silver")]
        public int? Silver { get; set; }

    }
}