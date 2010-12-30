using TradingApi.Client.Core.ClientDTO;

namespace TradingApi.Client.Core.Domain
{
    public class NullPrice : Price
    {
        public override string ToString()
        {
            return "Price: <Null price>";
        }
    }
}