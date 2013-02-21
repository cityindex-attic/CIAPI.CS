using System.Linq;
using CIAPI.DTO;
using CIAPI.RecordedTests.Infrastructure;
using CIAPI.Rpc;
using NUnit.Framework;

namespace CIAPI.RecordedTests
{
    [TestFixture, MocumentModeOverride(MocumentMode.Play)]
    public class AppKeyFixture : CIAPIRecordingFixtureBase
    {
        [Test]
        public void AppKeyIsAppendedToLogonRequest()
        {

            var rpcClient = BuildRpcClient("AppKeyIsAppendedToLogonRequest");
            rpcClient.LogOut();
            rpcClient.Dispose();

            // while tests can be  written without knowledge of the http transport layer,
            // it is possible for a test to examine, in minute detail, the traffic it has generated.

            var entry = this.TrafficLog.List().First();
            var r = entry.log.entries[0].request;
            var dto = rpcClient.Serializer.DeserializeObject<ApiLogOnRequestDTO>(r.postData.text);
            Assert.AreEqual(ApiKey, dto.AppKey);

        }

    }
}