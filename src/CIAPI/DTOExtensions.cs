using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIAPI.DTO;

namespace CIAPI.DTO
{
    public partial class AccountInformationResponseDTO
    {
        public ApiTradingAccountDTO CFDAccount
        {
            get
            {
                return this.TradingAccounts.Where(a => a.TradingAccountType.Contains("CFD")).FirstOrDefault();
            }
        }
        public ApiTradingAccountDTO SpreadBettingAccount
        {
            get
            {
                return TradingAccounts.Where(a => a.TradingAccountType.Contains("Spread Betting")).FirstOrDefault();
            }
        }
    }
}
