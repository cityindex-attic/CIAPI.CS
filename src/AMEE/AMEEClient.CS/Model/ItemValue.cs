using Newtonsoft.Json;

namespace AMEEClient.Model
{
    public class ItemValue
    {
        [JsonProperty("itemValueDefinition")]
        public ItemValueDefinition ItemValueDefinition;
    }
}