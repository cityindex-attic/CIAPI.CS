using System;
using CityIndex.JsonClient.Converters;
using Newtonsoft.Json;
namespace CIAPI.DTO
{

    public class AccountInformationResponseDTO
    {
        public string LogonUserName { get; set; }
        public int ClientAccountId { get; set; }
        public string ClientAccountCurrency { get; set; }
        public TradingAccountDTO[] TradingAccounts { get; set; }
    }

    public class TradingAccountDTO
    {
        public int TradingAccountId { get; set; }
        public string TradingAccountCode { get; set; }
        public string TradingAccountStatus { get; set; }
        public string TradingAccountType { get; set; }
    }
}
