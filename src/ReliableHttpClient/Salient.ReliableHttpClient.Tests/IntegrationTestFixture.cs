using System.Net;
using CassiniDev;
using NUnit.Framework;

namespace Salient.ReliableHttpClient.Tests
{
    /// <summary>
    /// </summary>
    [TestFixture]
    public class IntegrationTestFixture : CassiniDevServer
    {
        [TestFixtureSetUp]
        public void Setup()
        {
            string location = new ContentLocator(@"Salient.ReliableHttpClient.TestWeb").LocateContent();
            StartServer(location);
            
        }


        [Test]
        public void TestServer()
        {
            var client = new WebClient();
            var response = client.DownloadString(NormalizeUrl("Handler1.ashx"));
            Assert.AreEqual("Hello World",response);

        }
        
    }
}