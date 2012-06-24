using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using CIAPI.IntegrationTests.Streaming;
using CIAPI.Rpc;

using NUnit.Framework;
using Salient.ReliableHttpClient;
using Client = CIAPI.Rpc.Client;

namespace CIAPI.IntegrationTests.Rpc
{
    [TestFixture]
    public class AuthenticationFixture : RpcFixtureBase
    {
        [Test]
        public void LoginShouldCreateSession()
        {
            var rpcClient = new Client(Settings.RpcUri, Settings.StreamingUri, AppKey);
            rpcClient.LogIn(Settings.RpcUserName, Settings.RpcPassword);

            Assert.That(rpcClient.Session, Is.Not.Empty);

            rpcClient.LogOut();
            rpcClient.Dispose();
        }

 
        [Test]
        public void LoginUsingSessionShouldValidateSession()
        {
            var rpcClient = new Client(Settings.RpcUri, Settings.StreamingUri, AppKey);
            rpcClient.LogIn(Settings.RpcUserName, Settings.RpcPassword);

            Assert.That(rpcClient.Session, Is.Not.Empty);

            //This should work
            var rpcClientUsingSession = new Client(Settings.RpcUri, Settings.StreamingUri, AppKey);
            rpcClientUsingSession.LogInUsingSession(Settings.RpcUserName, rpcClient.Session);
            Assert.That(rpcClientUsingSession.Session, Is.Not.Empty);

            //After the session has been destroyed, trying to login using it should fail
            rpcClient.LogOut();
            Assert.Throws<ReliableHttpException>(() =>
                                                     {
                                                         rpcClientUsingSession.LogInUsingSession(
                                                             Settings.RpcUserName, rpcClient.Session);
                                                     });

            //And there shouldn't be a session
            Assert.IsNullOrEmpty(rpcClientUsingSession.Session);

            rpcClientUsingSession.LogOut();
            rpcClientUsingSession.Dispose();
            rpcClient.Dispose();
        }
 
    }
}
