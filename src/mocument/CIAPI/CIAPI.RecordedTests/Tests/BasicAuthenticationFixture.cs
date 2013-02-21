using System;
using CIAPI.RecordedTests.Infrastructure;
using CIAPI.Rpc;
using NUnit.Framework;

namespace CIAPI.RecordedTests
{
    [TestFixture, MocumentModeOverride(MocumentMode.Play)]
    public class BasicAuthenticationFixture : CIAPIRecordingFixtureBase
    {
        [Test]
        public void CanCreateAndLoginAndLogout()
        {
            var client = BuildRpcClient("CanCreateAndLoginAndLogout");

            client.LogOut();
            client.Dispose();
        }

        [Test]
        public void CanCreateAndThenLoginAndLogout()
        {
            var client = BuildUnauthenticatedRpcClient("CanCreateAndThenLoginAndLogout");
            client.LogIn(UserName, Password);
            client.LogOut();
            client.Dispose();
        }

    }
}