using Newtonsoft.Json;

namespace SOAPI2.CS.DTO
{
    public class Styling
    {
        [JsonProperty("link_color")]
        public string LinkColor { get; set; }
        [JsonProperty("tag_foreground_color")]
        public string TagForegroundColor { get; set; }
        [JsonProperty("tag_background_color")]
        public string TagBackgroundColor { get; set; }
    }
}