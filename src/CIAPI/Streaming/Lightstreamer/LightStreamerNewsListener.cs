using CIAPI.DTO;
using Lightstreamer.DotNet.Client;

namespace CIAPI.Streaming.Lightstreamer
{
    public class LightStreamerNewsListener : LightStreamerListener<NewsDTO>
    {
        public LightStreamerNewsListener(string topic, LSClient lsClient)
            : base(topic, lsClient)
        {

        }

        protected override void BeforeStart()
        {
            MessageConverter = new LightstreamerNewsDtoConverter();
            TableInfo = new SimpleTableInfo(Topic.ToUpper(),"RAW","StoryId Headline PublishDate",false)
                {
                    DataAdapter = "NEWS"
                };
        }
    }
}