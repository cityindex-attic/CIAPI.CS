using System;

namespace TradingApi.Client.Core.ClientDTO
{
    public class Price
    {
        public int MarketId { get; set; }
        public decimal Bid { get; set; }
        public decimal Offer { get; set; }
        public int Direction { get; set; }
        public decimal Change { get; set; }
        public string AuditId { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public bool Indicative { get; set; }

        /// <summary>
        /// Only relevant for options
        /// </summary>
        public decimal Delta { get; set; }
        /// <summary>
        /// Only relevant for options
        /// </summary>
        public decimal ImpliedVolatility { get; set; }

        public override string ToString()
        {
            return string.Format("Price: MarketId={0},Bid={1},Offer={2},Direction={3},"+
                                 "Change={4},AuditId={5},Delta={6},ImpliedVolatility={7}"+
                                 ",LastUpdateTime={8},Indicative={9}",
                                 MarketId, Bid, Offer, Direction,
                                 Change,AuditId,Delta,ImpliedVolatility,
                                 LastUpdateTime.ToUniversalTime().ToString("o"), Indicative);
        }
    }
}