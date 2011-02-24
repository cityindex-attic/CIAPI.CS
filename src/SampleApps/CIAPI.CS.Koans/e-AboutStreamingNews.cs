using System;
using System.Collections.Generic;
using System.Threading;
using CIAPI.CS.Koans.KoanRunner;
using CIAPI.DTO;
using CIAPI.Streaming;
using StreamingClient;
using NUnit.Framework;
using Client = CIAPI.Rpc.Client;

namespace CIAPI.CS.Koans
{
    [KoanCategory]
    public class AboutStreamingNews: IDisposable
    {
        private string USERNAME = "enter_your_username";
        private string PASSWORD = "password";

        [Koan]
        public void ConnectingToTheNewsStreamRequiresAValidSession()
        {
            _rpcClient = new Rpc.Client(new Uri("http://ciapipreprod.cityindextest9.co.uk/TradingApi"));
            _rpcClient.LogIn(USERNAME, PASSWORD);

            //Data is streamed over a specific HTTP endpoint
            var STREAMING_URI = new Uri("http://pushpreprod.cityindextest9.co.uk/CITYINDEXSTREAMING");

            //A single streaming client (connection) allows listening to many streams (channels)
            _streamingClient =
                StreamingClientFactory.CreateStreamingClient(
                    STREAMING_URI, 
                    USERNAME,               /* Note how we use the same username */
                    _rpcClient.SessionId);  /* and and shared sessionId to connect */

            var gate = new ManualResetEvent(false);
            var isConnected = false;
            //The streaming client raises events when things happen
            _streamingClient.StatusChanged += (object sender, StatusEventArgs e) =>
                                                  {
                                                      if (e.Status.Contains("Connection established"))
                                                      {
                                                          isConnected = true;
                                                          gate.Set();
                                                      }
                                                  };
            //You have to connect the client to the server
            _streamingClient.Connect();

            gate.WaitOne(TimeSpan.FromSeconds(10));

            KoanAssert.That(isConnected, Is.EqualTo(FILL_ME_IN), "it takes a few seconds for the steam to get connected");
        }

        [Koan]
        public void YouListenToAStreamsOverAConnection()
        {
            //Beginning with a connected streamingClient, you create a listener expected a specific message type on a certain channel/topic
            const string topic = "NEWS.MOCKHEADLINES.UK"; //The mock news headlines stream publishes one "story" per second.
            var ukNewsListener = _streamingClient.BuildListener<NewsDTO>(topic);

            var ukNewsHeadlines = new List<NewsDTO>();
            ukNewsListener.MessageRecieved += (sender, message) => /* The MessageRecieved event is raised every time a new message arrives */
                                                  {
                                                      ukNewsHeadlines.Add(message.Data); //Its data property contains a typed DTO
                                                  };

            //You call start to begin listening
            ukNewsListener.Start();

            //Wait whilst some data is connected
            Thread.Sleep(TimeSpan.FromSeconds(5));

            //And stop to finish
            ukNewsListener.Stop();

            KoanAssert.That(ukNewsHeadlines.Count, Is.GreaterThan(FILL_IN_THE_CORRECT_VALUE), "On the mock news headlines stream we should get 1 headline per second");
        }

        [Koan]
        public void YouCanListenToMultipleStreamsOverASingleConnection()
        {
            var ukNewsListener = _streamingClient.BuildListener<NewsDTO>("NEWS.MOCKHEADLINES.UK");
            var gbpusdPriceListener = _streamingClient.BuildListener<PriceDTO>("PRICES.PRICE.154297");

            //Build as many listeners as you want
            var ukNewsHeadlines = new List<NewsDTO>();
            ukNewsListener.MessageRecieved += (sender, message) => ukNewsHeadlines.Add(message.Data);
            ukNewsListener.Start();

            var gbpusdPrices = new List<PriceDTO>();
            gbpusdPriceListener.MessageRecieved += (sender, message) => gbpusdPrices.Add(message.Data);
            gbpusdPriceListener.Start();

            //Wait whilst some data is connected
            Thread.Sleep(TimeSpan.FromSeconds(10));

            //Remember to stop them when you are done
            ukNewsListener.Stop();
            gbpusdPriceListener.Stop();

            KoanAssert.That(ukNewsHeadlines.Count, Is.GreaterThan(FILL_IN_THE_CORRECT_VALUE), "On the mock news headlines stream we should get 1 headline per second");
            KoanAssert.That(gbpusdPrices.Count, Is.GreaterThanOrEqualTo(FILL_IN_THE_CORRECT_VALUE), "GBP/USD Prices should come through fairly regularly");
        }

        public void Dispose()
        {
            //After you are done with the connection, you need to disconnect
            if (_streamingClient != null) _streamingClient.Disconnect();
        }

        private Client _rpcClient;
        private IStreamingClient _streamingClient;
        private int FILL_IN_THE_CORRECT_VALUE = 999;
        private object FILL_ME_IN = false;
    }
}
