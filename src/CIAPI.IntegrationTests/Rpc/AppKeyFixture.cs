using CIAPI.DTO;
using CIAPI.IntegrationTests.Streaming;
using CIAPI.Rpc;
using NUnit.Framework;

namespace CIAPI.IntegrationTests.Rpc
{
    [TestFixture]
    public class AppKeyFixture : RpcFixtureBase
    {
        [Test]
        public void AppKeyIsAppendedToLogonRequest()
        {
            // look at the log to verify - need to expose interals and provide a means to examine the cache to verify programmatically

            var rpcClient = new Client(Settings.RpcUri,Settings.StreamingUri, "my-test-appkey");
            rpcClient.LogIn(Settings.RpcUserName, Settings.RpcPassword);

            rpcClient.LogOut();
            rpcClient.Dispose();
        }
    }
}