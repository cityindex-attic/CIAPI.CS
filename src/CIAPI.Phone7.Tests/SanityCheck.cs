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
using StreamingClient;
using IStreamingClient = CIAPI.Streaming.IStreamingClient;

namespace CIAPI.Phone7.Tests
{
    [TestClass]
    public class IntegrationTests : SilverlightTest
    {
        [TestInitialize]
        public void TestFixtureSetUp()
        {
            // this enables the client framework stack - necessary for access to headers
            bool httpResult = WebRequest.RegisterPrefix("http://", System.Net.Browser.WebRequestCreator.ClientHttp);
            bool httpsResult = WebRequest.RegisterPrefix("https://", System.Net.Browser.WebRequestCreator.ClientHttp);
        }

        [TestMethod]
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

        [TestMethod]
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

        // need to figure out the correct structure for an async streaming test.

        /////// <summary>
        /////// WARNING! This test will fail if run over the weekend (as there are no prices)
        /////// </summary>
        //[TestMethod]
        //[Asynchronous]
        //public void CanSubscribeToStream()
        //{
        //    var rpcClient = new Rpc.Client(App.RpcUri);
        //    IStreamingClient streamingClient = null;
        //    IStreamingListener<PriceDTO> priceListener = null;
        //    rpcClient.BeginLogIn(App.RpcUserName, App.RpcPassword, ar =>
        //    {


        //        rpcClient.EndLogIn(ar);



        //        //EnqueueCallback(() =>
        //        //    {
        //        //        // 24/5 currency markets 
        //        //        //   154297	GBP/USD (per 0.0001) CFD
        //        //        //   154284	EUR/AUD (per 0.0001) CFD
        //        //        //   99524	EUR/CAD (per 0.0001) CFD

        //        //        Assert.IsTrue(
        //        //            DateTime.UtcNow.DayOfWeek != DayOfWeek.Saturday &&
        //        //            DateTime.UtcNow.DayOfWeek != DayOfWeek.Sunday,
        //        //            "This test cannot be run as there are no prices over the weekend");

        //        //        
        //        //        priceListener.MessageReceived += (s, e) =>
        //        //        {
        //        //            PriceDTO actual = null;
        //        //            actual = e.Data;
        //        //            priceListener.Stop();
        //        //            streamingClient.Disconnect();
        //        //            Assert.IsNotNull(actual);
        //        //            Assert.IsFalse(string.IsNullOrEmpty(actual.AuditId));

        //        //            Assert.IsTrue(actual.Price > 0);
        //        //            EnqueueTestComplete();
        //        //        };

        //        //    });


        //    }, null);


        //    EnqueueCallback(() =>
        //    {
        //        streamingClient = StreamingClientFactory.CreateStreamingClient(App.StreamingUri, App.RpcUserName, rpcClient.Session);
        //        streamingClient.Connect();
        //    });

        //    EnqueueCallback(() =>
        //    {

        //        priceListener = streamingClient.BuildPricesListener(new[] { 154297, 154284, 99524 });
        //    });


        //    EnqueueCallback(() =>
        //    {
        //        priceListener.Start();
        //    });

        //}
    }
}
