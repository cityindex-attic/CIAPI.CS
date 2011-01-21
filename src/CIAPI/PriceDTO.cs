using System;

namespace CIAPI.DTO
{
    public class PriceDTO
    {
        public int MarketId { get; set; }
        public DateTime TickDate { get; set; }
        public decimal Bid { get; set; }
        public decimal Offer { get; set; }
        public decimal Price { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Change { get; set; }
        public int Direction { get; set; } //1 == up 0 == down
        public string AuditId { get; set; }
        public string PriceEngine { get; set; }
        public string Row_Update_Version { get; set; }
    }
}