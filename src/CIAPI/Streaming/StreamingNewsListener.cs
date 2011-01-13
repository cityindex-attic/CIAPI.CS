using CIAPI.DTO;
using Lightstreamer.DotNet.Client;

namespace CIAPI.Streaming
{
    public class StreamingNewsListener : StreamingListener<NewsDTO>
    {
        public StreamingNewsListener(string topic, LSClient lsClient)
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