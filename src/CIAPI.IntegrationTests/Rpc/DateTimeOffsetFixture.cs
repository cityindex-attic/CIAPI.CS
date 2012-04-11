using System;
using CIAPI.IntegrationTests.Streaming;
using NUnit.Framework;

namespace CIAPI.IntegrationTests.Rpc
{
    [TestFixture]
    public class DateTimeOffsetFixture : RpcFixtureBase
    {

        [Test(Description = "Validates error condition reported in https://github.com/cityindex/CIAPI.CS/issues/133")]
        public void CanHandleResponseWithNullableDateTimeOffset()
        {
            var rpcClient = BuildRpcClient();

            try
            {
                rpcClient.Market.GetMarketInformation("400160010");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                Assert.Fail("Should not throw exception getting market information");
            }
            finally
            {
                rpcClient.LogOut();
                rpcClient.Dispose();
            }
        }
    }

 
}