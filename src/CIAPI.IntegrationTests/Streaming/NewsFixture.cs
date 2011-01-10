using System;
using System.Threading;
using NUnit.Framework;
using TradingApi.Client.Core;
using TradingApi.Client.Core.ClientDTO;
using TradingApi.Client.Core.Lightstreamer;

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
            const string streamingUrl = "https://pushpreprod.cityindextest9.co.uk/";
            const string userName = "0X234";
            const string password = "password";
            const string adapterName = "CITYINDEXSTREAMING";

            var ctx = new ApiClient(new Uri(apiUrl));
            ctx.LogIn(userName, password);

            ILightstreamerConnection connection = new LightstreamerConnection(streamingUrl, userName, ctx.SessionId.ToString(), adapterName);
            var newsClient = new NewsListener("MOCKHEADLINES.", "UK", connection);
            newsClient.Subscribe();

            News latestNewsHeadline = new NullNews();
            //Trap the Price given by the update event for checking
            newsClient.Update += (s, e) =>
            {
                latestNewsHeadline = e.Item.News;
                Console.WriteLine(latestNewsHeadline.ToString());
                gate.Set();
            };

            gate.WaitOne(TimeSpan.FromSeconds(5));
            newsClient.Unsubscribe();

            Assert.AreNotEqual(new NullNews().ToString(), latestNewsHeadline.ToString());
        }
    }
}
