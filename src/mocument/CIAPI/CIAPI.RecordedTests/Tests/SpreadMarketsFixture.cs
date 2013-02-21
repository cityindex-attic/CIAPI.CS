using System;
using CIAPI.DTO;
using CIAPI.RecordedTests.Infrastructure;
using NUnit.Framework;

namespace CIAPI.RecordedTests
{
    [TestFixture, MocumentModeOverride(MocumentMode.Play)]
    public class SpreadMarketsFixture : CIAPIRecordingFixtureBase
    {
        [Test]
        public void CanListSpreadMarkets()
        {
            try
            {
                var rpcClient = BuildRpcClient("CanListSpreadMarkets");

                AccountInformationResponseDTO accounts = rpcClient.AccountInformation.GetClientAndTradingAccount();

                // TODO: publish somewhere that search term is a 'StartsWith' not a 'Contains'
                // #APIBUG  GBP/USD fails - still have problem with path separators in params, even when url encoded

                var response = rpcClient.SpreadMarkets.ListSpreadMarkets("GBP/USD", null, accounts.ClientAccountId, 100, false);
                Assert.Greater(response.Markets.Length, 0);
                rpcClient.LogOut();
                rpcClient.Dispose();
            }
            catch (Exception exception)
            {

                Assert.Fail("\r\n{0}", GetErrorInfo(exception));
            }

        }
    }
}