using System;

namespace CIAPI.DTO
{
    public class PriceDTO
    {
        public int MarketId { get; set; }
        public decimal Price { get; set; }
        public decimal Bid { get; set; }
        public decimal Offer { get; set; }
        public int Direction { get; set; }
        public decimal Change { get; set; }
        public string AuditId { get; set; }
        public DateTime TickDate { get; set; }
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
            return string.Format("Price: MarketId={0}Price={10},Bid={1},Offer={2},Direction={3},"+
                                 "Change={4},AuditId={5},Delta={6},ImpliedVolatility={7}"+
                                 ",TickDate={8},Indicative={9}",
                                 MarketId, Bid, Offer, Direction,
                                 Change,AuditId,Delta,ImpliedVolatility,
                                 TickDate.ToUniversalTime().ToString("o"), Indicative, Price);
        }
    }
}