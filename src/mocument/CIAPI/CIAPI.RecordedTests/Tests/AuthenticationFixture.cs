using CIAPI.RecordedTests.Infrastructure;
using CIAPI.Rpc;
using NUnit.Framework;
using Salient.ReliableHttpClient;

namespace CIAPI.RecordedTests
{
    [TestFixture, MocumentModeOverride(MocumentMode.Play)]
    public class AuthenticationFixture : CIAPIRecordingFixtureBase
    {
        [Test]
        public void LoginShouldCreateSession()
        {
            Client rpcClient = BuildRpcClient("LoginShouldCreateSession");

            Assert.That(rpcClient.Session, Is.Not.Empty);

            rpcClient.LogOut();
            rpcClient.Dispose();
        }


        [Test]
        public void LoginUsingSessionShouldValidateSession()
        {
            Client rpcClient = BuildRpcClient("LoginUsingSessionShouldValidateSession");

            Assert.That(rpcClient.Session, Is.Not.Null.Or.Empty);

            //This should work
            Client rpcClientUsingSession = BuildUnauthenticatedRpcClient("LoginUsingSessionShouldValidateSession");

            rpcClientUsingSession.LogInUsingSession(UserName, rpcClient.Session);

            Assert.That(rpcClientUsingSession.Session, Is.Not.Null.Or.Empty);

            //After the session has been destroyed, trying to login using it should fail
            rpcClient.LogOut();
            Assert.Throws<ReliableHttpException>(
                () => rpcClientUsingSession.LogInUsingSession(UserName, rpcClient.Session));

            //And there shouldn't be a session
            Assert.IsNullOrEmpty(rpcClientUsingSession.Session);


            // this client is already logged out. should we swallow unauthorized exceptions in the logout methods?
            // rpcClientUsingSession.LogOut();
            rpcClientUsingSession.Dispose();
            rpcClient.Dispose();
        }
    }
}