using CIAPI.DTO;
using Lightstreamer.DotNet.Client;

namespace CIAPI.Streaming.Lightstreamer
{
    public class LightstreamerNewsDtoConverter : LightstreamerDtoConverter<NewsDTO>
    {
        public override NewsDTO Convert(object data)
        {
            var updateInfo = (UpdateInfo)data;
            return new NewsDTO
                       {
                           StoryId = GetAsInt(updateInfo, GetFieldIndex(typeof(NewsDTO).GetProperty("StoryId"))),
                           Headline = GetAsString(updateInfo, GetFieldIndex(typeof(NewsDTO).GetProperty("Headline"))),
                           PublishDate = GetAsJSONDateTimeUtc(updateInfo, GetFieldIndex(typeof(NewsDTO).GetProperty("PublishDate")))
                       };
        }
    }
}