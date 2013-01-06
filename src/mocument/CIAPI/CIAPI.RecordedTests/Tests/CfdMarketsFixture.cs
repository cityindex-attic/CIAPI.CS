using CIAPI.DTO;
using CIAPI.RecordedTests.Infrastructure;
using CIAPI.Rpc;
using NUnit.Framework;

namespace CIAPI.RecordedTests
{
    [TestFixture, MocumentModeOverride(MocumentMode.Play)]
    public class CfdMarketsFixture : CIAPIRecordingFixtureBase
    {
        [Test]
        public void CanListCFDMarkets()
        {
            Client rpcClient = BuildRpcClient();

            AccountInformationResponseDTO accounts = rpcClient.AccountInformation.GetClientAndTradingAccount();

            ListCfdMarketsResponseDTO response = rpcClient.CFDMarkets.ListCfdMarkets("USD", null,
                                                                                     accounts.ClientAccountId, 500,
                                                                                     false);

            Assert.Greater(response.Markets.Length, 0, "no markets returned");


            rpcClient.LogOut();
            rpcClient.Dispose();
        }
    }
}