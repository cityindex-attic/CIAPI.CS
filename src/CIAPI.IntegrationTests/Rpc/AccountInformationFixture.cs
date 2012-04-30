using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using CIAPI.DTO;
using CIAPI.IntegrationTests.Streaming;
using CIAPI.Rpc;
using NUnit.Framework;
using Salient.ReliableHttpClient;


namespace CIAPI.IntegrationTests.Rpc
{
    [TestFixture]
    public class AccountInformationFixture : RpcFixtureBase
    {

        [Test]
        public void CanListOpenPositions()
        {
            var rpcClient = BuildRpcClient();

            AccountInformationResponseDTO accounts = rpcClient.AccountInformation.GetClientAndTradingAccount();
            rpcClient.TradesAndOrders.ListOpenPositions(accounts.TradingAccounts[0].TradingAccountId);
            rpcClient.LogOut();
            rpcClient.Dispose();

        }

        [Test,Ignore("if this test breaks the account is in unexpected state. test manually")]
        public void CanChangePassword()
        {
            const string NEWPASSWORD = "bingo72652";
            var rpcClient = new Client(Settings.RpcUri,Settings.StreamingUri, AppKey);
            
            //Login with existing credentials
            rpcClient.LogIn(Settings.RpcUserName, Settings.RpcPassword);

            //And change password
            var changePasswordResponse = rpcClient.Authentication.ChangePassword(new ApiChangePasswordRequestDTO()
                                                                                         {
                                                                                             UserName = Settings.RpcUserName,
                                                                                             Password = Settings.RpcPassword,
                                                                                             NewPassword = NEWPASSWORD
                                                                                         });

            Assert.IsTrue(changePasswordResponse.IsPasswordChanged);
            rpcClient.LogOut();

            //Make sure that login existing password fails 
            Assert.Throws<ReliableHttpException>(() => rpcClient.LogIn(Settings.RpcUserName, Settings.RpcUserName));

            //Login with changed password and change back
            rpcClient.LogIn(Settings.RpcUserName, NEWPASSWORD);
            changePasswordResponse = rpcClient.Authentication.ChangePassword(new ApiChangePasswordRequestDTO()
                                                                                         {
                                                                                             UserName = Settings.RpcUserName,
                                                                                             Password = NEWPASSWORD,
                                                                                             NewPassword = Settings.RpcPassword
                                                                                         });

            Assert.IsTrue(changePasswordResponse.IsPasswordChanged);
            rpcClient.LogOut();
            rpcClient.Dispose();
        }
    }
}
