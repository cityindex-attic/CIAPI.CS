using System;
using System.Threading;
using CIAPI.DTO;
using CIAPI.Streaming;
using Newtonsoft.Json;
using NUnit.Framework;
using StreamingClient;
using IStreamingClient = CIAPI.Streaming.IStreamingClient;

namespace CIAPI.IntegrationTests.Streaming
{
    [TestFixture]
    public class NewsFixture
    {
        public static IStreamingClient BuildStreamingClient(
            string userName = "xx189949",
            string password = "password")
        {
            const string apiUrl = "http://ciapipreprod.cityindextest9.co.uk/TradingApi/";

            var authenticatedClient = new CIAPI.Rpc.Client(new Uri(apiUrl));
            authenticatedClient.LogIn(userName, password);

            var streamingUri = new Uri("http://pushpreprod.cityindextest9.co.uk");

            return StreamingClientFactory.CreateStreamingClient(streamingUri, userName, authenticatedClient.SessionId);
        }

        [Test]
        public void CanConsumeNewsStream()
        {
            var gate = new ManualResetEvent(false);

            var streamingClient = BuildStreamingClient();
            streamingClient.Connect();

            var newsListener = streamingClient.BuildNewsHeadlinesListener("NEWS.MOCKHEADLINES.UK");
            newsListener.Start();

            NewsDTO actual = null;

            //Trap the Price given by the update event for checking
            newsListener.MessageReceived += (s, e) =>
            {
                actual = e.Data;
                gate.Set();
            };


            if (!gate.WaitOne(TimeSpan.FromSeconds(5)))
            {
                Assert.Fail("timed out");
            }

            newsListener.Stop();
            streamingClient.Disconnect();

            Assert.IsNotNull(actual);
            Assert.IsNotEmpty(actual.Headline);

            Assert.Greater(actual.PublishDate, DateTime.UtcNow.AddMonths(-1));
            Assert.Greater(actual.StoryId, 0);
        }
    }
}
