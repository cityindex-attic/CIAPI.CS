using System;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using CIAPI.DTO;
using CIAPI.Streaming;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CIAPI.Phone7.Tests
{
    [TestClass]
    public class IntegrationTests : SilverlightTest
    {
        [TestMethod, Ignore]
        [Asynchronous]
        public void CanLoginLogout()
        {
            var rpcClient = new Rpc.Client(App.RpcUri);
            rpcClient.BeginLogIn(App.RpcUserName, App.RpcPassword, ar =>
                {
                    rpcClient.EndLogIn(ar);
                    Assert.IsNotNull(rpcClient.Session);
                    rpcClient.BeginLogOut(ar2 =>
                        {
                            var loggedOut = rpcClient.EndLogOut(ar2);
                            Assert.IsTrue(loggedOut);
                            EnqueueTestComplete();
                        }, null);

                }, null);

        }

        [TestMethod, Ignore]
        [Asynchronous]
        public void CanGetHeadlines()
        {
            var rpcClient = new Rpc.Client(App.RpcUri);
            rpcClient.BeginLogIn(App.RpcUserName, App.RpcPassword, ar =>
            {
                rpcClient.EndLogIn(ar);
                Assert.IsNotNull(rpcClient.Session);
                rpcClient.News.BeginListNewsHeadlines("UK", 10, ar2 =>
                    {
                        var response = rpcClient.News.EndListNewsHeadlines(ar2);
                        Assert.IsTrue(response.Headlines.Length > 0, "expected headlines");
                        EnqueueTestComplete();
                    }, null);

            }, null);

        }

        [TestMethod]
        [Asynchronous]
        public void CanSubscribeToStream()
        {
            var rpcClient = new Rpc.Client(App.RpcUri);
            rpcClient.BeginLogIn(App.RpcUserName, App.RpcPassword, ar =>
            {
                rpcClient.EndLogIn(ar);

                EnqueueCallback(() =>
                {
                    Assert.IsNotNull(rpcClient.Session);

                    var streamingClient = StreamingClientFactory.CreateStreamingClient(App.StreamingUri, App.RpcUserName, rpcClient.Session);
                    var newsListener = streamingClient.BuildNewsHeadlinesListener("UK");



                    newsListener.MessageReceived += (s, e) =>
                    {
                        NewsDTO actual = null;
                        actual = e.Data;
                        newsListener.Stop();
                        streamingClient.Disconnect();


                        Assert.IsNotNull(actual);
                        Assert.IsFalse(string.IsNullOrEmpty(actual.Headline));


                        Assert.IsTrue(actual.StoryId > 0);
                        EnqueueTestComplete();
                    };

                    newsListener.Start();


                }

                    );


            }, null);

        }
    }
}
