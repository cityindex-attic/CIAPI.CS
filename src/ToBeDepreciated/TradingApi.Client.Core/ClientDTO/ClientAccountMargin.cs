namespace TradingApi.Client.Core.ClientDTO
{
    public class ClientAccountMargin
    {
        public decimal MarginIndicator { get; set; }
        public decimal Cash { get; set; }
        public decimal CreditAllocation { get; set; }
        public decimal WaivedMarginRequirement { get; set; }
        public decimal OpenTradeEquity { get; set; }
        public decimal TradingResource { get; set; }
        public decimal NetEquity { get; set; }
        public decimal Margin { get; set; }
        public decimal TradableFunds { get; set; }
        public decimal TotalMarginRequirement { get; set; }
        public string CurrencyISOCode { get; set; }

        public override string ToString()
        {
            return string.Format("ClientAccountMargin: MarginIndicator={0},Cash={1},"
                                +"CreditAllocation={2},WaivedMarginRequirement={3},"
                                +"OpenTradeEquity={4},TradingResource={5},"
                                +"NetEquity={6},Margin={7},TradableFunds={8},"
                                +"TotalMarginRequirement={9},CurrencyISOCode={10}",
                                 MarginIndicator, Cash, 
                                 CreditAllocation,WaivedMarginRequirement, 
                                 OpenTradeEquity, TradingResource,
                                 NetEquity, Margin, TradableFunds,
                                 TotalMarginRequirement, CurrencyISOCode);
        }
    }
}