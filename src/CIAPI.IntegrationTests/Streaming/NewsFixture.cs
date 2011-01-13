using System;
using System.Threading;
using CIAPI.DTO;
using CIAPI.Streaming;
using NUnit.Framework;

namespace CIAPI.IntegrationTests.Streaming
{
    [TestFixture]
    public class NewsFixture
    {
        [Test]
        public void CanConsumeNewsStream()
        {
            var gate = new ManualResetEvent(false);

            const string apiUrl = "https://ciapipreprod.cityindextest9.co.uk/TradingApi/";
            const string userName = "0x234";
            const string password = "password";

            var authenticatedClient = new Rpc.Client(new Uri(apiUrl));
            authenticatedClient.LogIn(userName, password);
           
            //TODO: test retry mechanism - seems to get stuck in a closed loop
            var streamingUri = new Uri("https://pushpreprod.cityindextest9.co.uk/CITYINDEXSTREAMING");

            var streamingClient = new Client(streamingUri, userName,authenticatedClient.SessionId);

            streamingClient.Connect();

            const string newsTopic = "MOCKHEADLINES.UK";

            var newsListener = streamingClient.BuildNew(newsTopic);
            newsListener.Start();

            NewsDTO actual = null;

            //Trap the Price given by the update event for checking

            newsListener.MessageRecieved += (s, e) =>
            {
                actual = e.Data;
                gate.Set();
            };


            if (!gate.WaitOne(TimeSpan.FromSeconds(5)))
            {
                Assert.Fail("timed out");
            }

            newsListener.Stop();

            Assert.IsNotNull(actual);
        }
    }
}
