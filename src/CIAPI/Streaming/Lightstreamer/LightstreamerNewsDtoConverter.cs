using CIAPI.DTO;
using Lightstreamer.DotNet.Client;

namespace CIAPI.Streaming.Lightstreamer
{
    public class LightstreamerNewsDtoConverter : LightstreamerDtoConverterBase, IMessageConverter<NewsDTO>
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
    }
}