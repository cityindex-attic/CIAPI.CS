using TradingApi.Client.Core.Lightstreamer;
using TradingApi.CoreDTO;

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