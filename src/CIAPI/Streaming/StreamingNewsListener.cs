using System;
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

    public class LightstreamerNewsDtoConverter : IMessageConverter<NewsDTO>
    {
        public NewsDTO Convert(object data)
        {
            var updateInfo = (UpdateInfo)data;
            return new NewsDTO
            {
                StoryId = GetAsInt(updateInfo, 1),
                Headline = GetAsString(updateInfo, 2),
                PublishDate = GetAsJSONDateTimeUtc(updateInfo, 3)
            };
        }

        private static DateTime GetAsJSONDateTimeUtc(UpdateInfo updateInfo, int pos)
        {
            //TODO: DO proper conversion
            return DateTime.Now;
        }

        private static string GetAsString(UpdateInfo updateInfo, int pos)
        {
            return GetCurrentValue(updateInfo, pos);
        }

        private static int GetAsInt(UpdateInfo updateInfo, int pos)
        {
            return System.Convert.ToInt32(GetCurrentValue(updateInfo, pos));
        }

        private static string GetCurrentValue(UpdateInfo updateInfo, int pos)
        {
            return updateInfo.IsValueChanged(pos)
                       ? updateInfo.GetNewValue(pos)
                       : updateInfo.GetOldValue(pos);
        }
    }
}