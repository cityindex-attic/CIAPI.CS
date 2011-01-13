using CIAPI.DTO;
using Lightstreamer.DotNet.Client;

namespace CIAPI.Streaming
{
    public class StreamingNewsListener<TMessageConverter>: StreamingListener<NewsDTO, TMessageConverter>
        where TMessageConverter : IMessageConverter<NewsDTO>, new()
    {
        public StreamingNewsListener(string topic, LSClient lsClient)
            : base(topic, lsClient)
        {

        }

        protected override void BeforeStart()
        {
            TableInfo = new SimpleTableInfo(Topic.ToUpper(),"RAW","StoryId Headline PublishDate",false)
                {
                    DataAdapter = "NEWS"
                };
        }
    }
}