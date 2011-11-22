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
using CIAPI.DTO;
using CityIndex.JsonClient;
using CityIndex.JsonClient.Tests;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CIAPI.Silverlight.TestsMS
{
    [TestClass]
    public class IssueResolutionScratchpadFixture : SilverlightTest
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
        public void EnsureSilverlightCanConnect()
        {
            var client = new Rpc.Client(new Uri(TestConfig.RpcUrl));

            client.BeginLogIn(TestConfig.ApiUsername, TestConfig.ApiPassword,
                ar =>
                {
                    EnqueueCallback(() =>
                    {
                        try
                        {

                            client.EndLogIn(ar);
                            client.News.BeginListNewsHeadlinesWithSource("dj","UK", 10, ar2 =>
                                {
                                    EnqueueCallback(() =>
                                                        {
                                                            var headlines = client.News.EndListNewsHeadlinesWithSource(ar2);
                                                            Assert.IsTrue(headlines.Headlines.Length > 0);
                                                            client.BeginLogOut(ar3 =>
                                                                                   {

                                                                                       EnqueueCallback(() =>
                                                                                                           {
                                                                                                               var loggedOut = client.EndLogOut(ar3);
                                                                                                               Assert.
                                                                                                                   IsTrue
                                                                                                                   (loggedOut);
                                                                                                               EnqueueTestComplete();
                                                                                                           });
                                                                                   }, null);

                                                        });


                                }, null);

                        }
                        catch (Exception exc)
                        {
                            throw;
                        }

                    });

                }, null);
        }
    }
}
