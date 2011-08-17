using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Controls;
using CIAPI.Streaming;
using CityIndex.JsonClient;
using CityIndex.JsonClient.Tests;
using Microsoft.Silverlight.Testing;
using System.Text.RegularExpressions;
using Microsoft.Silverlight.Testing.Harness;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace CIAPI.Silverlight.TestsMS
{
    [TestClass]
    public class IntegrationTests01 : SilverlightTest
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
        public void CanLogIn()
        {

            var ctx = new Rpc.Client(new Uri(TestConfig.RpcUrl));

            ctx.BeginLogIn(TestConfig.ApiUsername, TestConfig.ApiPassword, ar =>
            {
                ctx.EndLogIn(ar);
                EnqueueCallback(() =>
                {
                    try
                    {
                        Assert.IsFalse(string.IsNullOrEmpty(ctx.Session));
                    }
                    finally
                    {
                        EnqueueTestComplete();
                    }

                }

                    );


            }, null);

        }

        [TestMethod]
        [Asynchronous]
        public void CanSubscribeToStream()
        {

   
            var ctx = new Rpc.Client(new Uri(TestConfig.RpcUrl));

            ctx.BeginLogIn(TestConfig.ApiUsername, TestConfig.ApiPassword, ar =>
                {

                    EnqueueCallback(() =>
                        {
                            ctx.EndLogIn(ar);

                            Assert.IsFalse(string.IsNullOrEmpty(ctx.Session));

                        });
                    EnqueueCallback(() =>
                        {
                            var streamingClient = StreamingClientFactory.CreateStreamingClient(new Uri(TestConfig.StreamingUri), ctx.UserName, ctx.Session);
                            System.Diagnostics.Debug.WriteLine("created streaming client");
                            streamingClient.Connect();
                            System.Diagnostics.Debug.WriteLine("streaming client connected");
                        });


                    EnqueueTestComplete();


                }, null);

        }
    }
}
