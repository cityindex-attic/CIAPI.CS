using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CIAPI.Phone7.Tests
{
    [TestClass]
    public class IntegrationTests : SilverlightTest
    {
        [TestMethod]
        [Asynchronous]
        public void CanLoginLogout()
        {
            var rpcClient = new Rpc.Client(App.RpcUri);
            rpcClient.BeginLogIn(App.RpcUserName, App.RpcPassword, ar =>
                {
                    rpcClient.EndLogIn(ar);
                    Assert.IsNotNull(rpcClient.SessionId);
                    rpcClient.BeginLogOut(ar2 =>
                        {
                            var loggedOut = rpcClient.EndLogOut(ar2);
                            Assert.IsTrue(loggedOut);
                            EnqueueTestComplete();
                        }, null);

                }, null);

        }

        [TestMethod]
        [Asynchronous]
        public void CanGetHeadlines()
        {
            var rpcClient = new Rpc.Client(App.RpcUri);
            rpcClient.BeginLogIn(App.RpcUserName, App.RpcPassword, ar =>
            {
                rpcClient.EndLogIn(ar);
                Assert.IsNotNull(rpcClient.SessionId);
                rpcClient.BeginListNewsHeadlines("UK", 10, ar2 =>
                    {
                        var response = rpcClient.EndListNewsHeadlines(ar2);
                        Assert.IsTrue(response.Headlines.Length > 0, "expected headlines");
                        EnqueueTestComplete();
                    }, null);

            }, null);

        }

    }
}
