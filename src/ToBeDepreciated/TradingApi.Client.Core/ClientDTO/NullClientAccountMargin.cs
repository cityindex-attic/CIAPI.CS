using TradingApi.Client.Core.ClientDTO;

namespace TradingApi.Client.Core.Domain
{
    public class NullClientAccountMargin : ClientAccountMargin
    {
        public override string ToString()
        {
            return "ClientAccountMargin: <Null ClientAccountMargin>";
        }
    }
}