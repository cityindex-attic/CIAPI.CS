namespace TradingApi.Client.Core.Lightstreamer.LightstreamerDictionaryIndices
{
    public struct Price
    {
        public static readonly int MarketId = 1;
        public static readonly int LastUpdateTime = 2;
        public static readonly int PriceMid = 3;
        public static readonly int Bid = 4;
        public static readonly int Offer = 5;
        public static readonly int Direction = 6;
        public static readonly int Change = 7;
//        public static readonly int AuditId = 6;
//        public static readonly int Delta = 7;
//        public static readonly int ImpliedVolatility = 8;
//        public static readonly int Indicative = 10;
    }

    public struct ClientAccountMargin
    {
        public static readonly int MarginIndicator = 1;
        public static readonly int Cash = 2;
        public static readonly int CreditAllocation = 3;
        public static readonly int WaivedMarginRequirement = 4;
        public static readonly int OpenTradeEquity = 5;
        public static readonly int TradingResource = 6;
        public static readonly int NetEquity = 7;
        public static readonly int Margin = 8;
        public static readonly int TradableFunds = 9;
        public static readonly int TotalMarginRequirement = 10;
        public static readonly int CurrencyISOCode = 11;
    }

    public struct News
    {
        public static readonly int StoryId = 1;
        public static readonly int Headline = 2;
        public static readonly int PublishDate = 3;
    }
}