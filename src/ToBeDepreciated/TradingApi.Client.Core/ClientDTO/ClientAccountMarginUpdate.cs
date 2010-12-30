using TradingApi.Client.Core.Domain;
using TradingApi.Client.Core.Lightstreamer;

namespace TradingApi.Client.Core.ClientDTO
{
    public class ClientAccountMarginUpdate : StreamingUpdate
    {
        public ClientAccountMargin ClientAccountMargin { get; set; }
    }
}