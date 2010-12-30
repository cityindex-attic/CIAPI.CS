using TradingApi.Client.Core.Domain;
using TradingApi.Client.Core.Lightstreamer;

namespace TradingApi.Client.Core.ClientDTO
{
    public class PriceUpdate : StreamingUpdate
    {
        public Price Price
        {
            get; set;
        }
    }
}