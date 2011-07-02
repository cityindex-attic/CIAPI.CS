using System;
using CityIndex.JsonClient.Converters;

namespace CIAPI.DTO
{


    public class TradingAccountDTO
    {
        public int TradingAccountId { get; set; }
        public string TradingAccountCode { get; set; }
        public string TradingAccountStatus { get; set; }
        public string TradingAccountType { get; set; }
    }
}
