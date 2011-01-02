using Newtonsoft.Json;

namespace SOAPI.CS2.Domain
{
    ///<summary>
    ///</summary>
    public class Styling
    {

        ///<summary>
        ///</summary>
        [JsonProperty("link_color")]
        public string LinkColor { get; set; }

        ///<summary>
        ///</summary>
        [JsonProperty("tag_background_color")]
        public string TagBackgroundColor { get; set; }

        ///<summary>
        ///</summary>
        [JsonProperty("tag_foreground_color")]
        public string TagForegroundColor { get; set; }

    }
}