using TradingApi.Client.Core.Lightstreamer;

namespace TradingApi.Client.Core.ClientDTO
{
    public class NewsUpdate : StreamingUpdate
    {
        public News News
        {
            get; set;
        }
    }
}