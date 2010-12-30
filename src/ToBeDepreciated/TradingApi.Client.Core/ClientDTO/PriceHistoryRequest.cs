namespace TradingApi.Client.Core.ClientDTO
{
    public class PriceHistoryRequest
    {
        public string MarketId { get; set; }
        public string Interval { get; set; }
        public int Span { get; set; }
        public int NumberOfBars { get; set; }
    }
}