using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using CityIndex.JsonClient;
using CityIndex.JsonClient.Tests;
using Microsoft.Silverlight.Testing;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace CIAPI.Silverlight.TestsMS
{
    [TestClass]
    public class SpikeFixture : SilverlightTest
    {
        [TestInitialize]
        public void TestFixtureSetUp()
        {
            // this enables the client framework stack - necessary for access to headers
            bool httpResult = WebRequest.RegisterPrefix("http://", System.Net.Browser.WebRequestCreator.ClientHttp);
            bool httpsResult = WebRequest.RegisterPrefix("https://", System.Net.Browser.WebRequestCreator.ClientHttp);
        }

        /// <summary>
        /// ok, this method works just fine, proving that it is the apicontext that is somehow farked and not the strategy.
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void RawWebRequestPOST()
        {
            var request = WebRequest.Create(TestConfig.ApiUrl +  "session");

            request.Method = "POST";
            request.BeginGetRequestStream(ac =>
            {
                string parameterJson = string.Format("{{\"UserName\": \"{0}\", \"Password\": \"{1}\"}}",TestConfig.ApiUsername,TestConfig.ApiPassword);
                var body = Encoding.UTF8.GetBytes(parameterJson);
                using (Stream stream = request.EndGetRequestStream(ac))
                {
                    stream.Write(body, 0, body.Length);
                }
                request.BeginGetResponse(ar =>
                    {
                        EnqueueCallback(() =>
                            {
                                string json = null;
                                using (var response = request.EndGetResponse(ar))
                                using (var responseStream = response.GetResponseStream())
                                using (var reader = new StreamReader(responseStream))
                                    json = reader.ReadToEnd();
                                Assert.IsFalse(string.IsNullOrEmpty(json));
                            });
                        EnqueueTestComplete();
                    }, null);
            }, null);

        }
    }
}
