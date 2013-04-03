using System.Linq;

// these dto are missing from metadata
// ReSharper disable CheckNamespace
namespace CIAPI.DTO
// ReSharper restore CheckNamespace
{
    public partial class ApiChangePasswordRequestDTO
    {
        
    }
    public partial class ApiSaveAccountInformationRequestDTO
    {
        
    }


    public partial class AccountInformationResponseDTO
    {
        /// <summary>
        /// 
        /// </summary>
        public ApiTradingAccountDTO CFDAccount
        {
            get
            {
                return TradingAccounts.Where(a => a.TradingAccountType.Contains("CFD")).FirstOrDefault();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public ApiTradingAccountDTO SpreadBettingAccount
        {
            get
            {
                return TradingAccounts.Where(a => a.TradingAccountType.Contains("Spread Betting")).FirstOrDefault();
            }
        }
    }
}
