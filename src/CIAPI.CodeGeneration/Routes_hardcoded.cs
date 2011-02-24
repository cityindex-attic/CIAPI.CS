using System;
using System.Collections.Generic;
using CityIndex.JsonClient;
using CIAPI.DTO;

namespace CIAPI.Rpc
{
    public partial class Client
    {
        public AccountInformationResponseDTO GetClientAndTradingAccount()
        {
            return Request<AccountInformationResponseDTO>("UserAccount", "/ClientAndTradingAccount", "GET", new Dictionary<string, object>() { }, TimeSpan.FromMilliseconds(0), "data");
        }

        public void BeginGetClientAndTradingAccount(ApiAsyncCallback<AccountInformationResponseDTO> callback, object state)
        {
            BeginRequest(callback,state, "UserAccount", "/ClientAndTradingAccount", "GET", new Dictionary<string, object>() { }, TimeSpan.FromMilliseconds(0), "data");
        }

        public AccountInformationResponseDTO EndClientAndTradingAccount(ApiAsyncResult<AccountInformationResponseDTO> asyncResult)
        {
            return EndRequest(asyncResult);
        }
    }
}