using System.Runtime.CompilerServices;
using CIAPI.DTO;
using CIAPI.RecordedTests.Infrastructure;
using CIAPI.Rpc;
using NUnit.Framework;
using Salient.ReliableHttpClient;

namespace CIAPI.RecordedTests
{
    [TestFixture, MocumentModeOverride(MocumentMode.Play)]
    public class AccountInformationFixture : CIAPIRecordingFixtureBase
    {
        [Test]
        public void CanListOpenPositions()
        {
            var rpcClient = BuildRpcClient("CanListOpenPositions");

            AccountInformationResponseDTO accounts = rpcClient.AccountInformation.GetClientAndTradingAccount();
            rpcClient.TradesAndOrders.ListOpenPositions(accounts.TradingAccounts[0].TradingAccountId);
            rpcClient.LogOut();
            rpcClient.Dispose();

        }

        [Test]
         
        public void CanChangePassword()
        {
            string NEWPASSWORD = "new" + Password;
            string OLDPASSWORD = Password;

            var rpcClient = BuildUnauthenticatedRpcClient("CanChangePassword");

            //Login with existing credentials
            rpcClient.LogIn(UserName, OLDPASSWORD);

            //And change password
            var changePasswordResponse = rpcClient.Authentication.ChangePassword(new ApiChangePasswordRequestDTO()
            {
                UserName = UserName,
                Password = OLDPASSWORD,
                NewPassword = NEWPASSWORD
            });

            Assert.IsTrue(changePasswordResponse.IsPasswordChanged);
            rpcClient.LogOut();

#if FALSE
            // enable this part of the test for recording so that the password 
            // is changed back.
            // disable this part of the test for play or test will fail

            //Make sure that login existing password fails 
            Assert.Throws<CIAPI.Rpc.InvalidCredentialsException>(() => rpcClient.LogIn(UserName, OLDPASSWORD));

            //Login with changed password and change back
            rpcClient.LogIn(UserName, NEWPASSWORD);
            changePasswordResponse = rpcClient.Authentication.ChangePassword(new ApiChangePasswordRequestDTO()
            {
                UserName = UserName,
                Password = NEWPASSWORD,
                NewPassword = OLDPASSWORD
            });

            Assert.IsTrue(changePasswordResponse.IsPasswordChanged);
            rpcClient.LogOut();

#endif
            rpcClient.Dispose();
        }
    }
}