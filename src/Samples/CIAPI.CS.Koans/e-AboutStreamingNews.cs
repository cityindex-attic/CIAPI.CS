using System;
using System.Collections.Generic;
using System.Threading;
using CIAPI.CS.Koans.KoanRunner;
using CIAPI.DTO;
using CIAPI.Streaming;
using CIAPI.Tests;
using CIAPI.StreamingClient;
using NUnit.Framework;
using Client = CIAPI.Rpc.Client;
using IStreamingClient = CIAPI.Streaming.IStreamingClient;

namespace CIAPI.CS.Koans
{
    //[KoanCategory(Order = 4)]  streaming news is currently dead - waiting for resurrection
    public class AboutStreamingNews: IDisposable
    {
        private string USERNAME = Settings.RpcUserName;
        private string PASSWORD = Settings.RpcPassword;
        private string AppKey = "KOAN_APPKEY";

        [Koan(Order = 1)]
        public void ConnectingToTheNewsStreamRequiresAValidSession()
        {
            _rpcClient = new Rpc.Client(Settings.RpcUri, Settings.StreamingUri, AppKey);
            _rpcClient.LogIn(USERNAME, PASSWORD);

            

            //A single streaming client (connection) allows listening to many streams (channels)
            _streamingClient =
                _rpcClient.CreateStreamingClient();  /* and and shared sessionId to connect */

            var gate = new ManualResetEvent(false);
            
            //The streaming client raises events when things happen
            _streamingClient.StatusChanged += (object sender, StatusEventArgs e) =>
                                                  {
                                                      if (e.Status.Contains("Connection established"))
                                                      {
                                                          gate.Set();
                                                      }
                                                  };
            //You have to connect the client to the server
            
 
        }

        //[Koan(Order = 2)]
        public void YouListenToAStreamsOverAConnection()
        {
            //Beginning with a connected streamingClient, you create a listener expected a specific message type on a certain channel/topic
            const string topic = "UK"; //The mock news headlines stream publishes one "story" per second.
            var ukNewsListener = _streamingClient.BuildNewsHeadlinesListener(topic);

            var ukNewsHeadlines = new List<NewsDTO>();
            ukNewsListener.MessageReceived += (sender, message) => /* The MessageReceived event is raised every time a new message arrives */
                                                  {
                                                      ukNewsHeadlines.Add(message.Data); //Its data property contains a typed DTO
                                                  };

            //You call start to begin listening
            

            //Wait whilst some data is connected
            new AutoResetEvent(false).WaitOne(20000);
            

            //And tear down to finish
            _streamingClient.TearDownListener(ukNewsListener);
            

            KoanAssert.That(ukNewsHeadlines.Count, Is.GreaterThan(3), "On the mock news headlines stream we should get 1 headline per second");
        }

        //[Koan(Order = 3)]
        public void YouCanListenToMultipleStreamsOverASingleConnection()
        {
            var ukNewsListener = _streamingClient.BuildNewsHeadlinesListener("UK");
            var gbpusdPriceListener = _streamingClient.BuildPricesListener(154297);

            //Build as many listeners as you want
            var ukNewsHeadlines = new List<NewsDTO>();
            ukNewsListener.MessageReceived += (sender, message) => ukNewsHeadlines.Add(message.Data);
            

            var gbpusdPrices = new List<PriceDTO>();
            gbpusdPriceListener.MessageReceived += (sender, message) => gbpusdPrices.Add(message.Data);
            

            //Wait whilst some data is connected
            new AutoResetEvent(false).WaitOne(20000);

            //Remember to tear them down when you are done
            _streamingClient.TearDownListener(ukNewsListener);
            _streamingClient.TearDownListener(gbpusdPriceListener);
            

            KoanAssert.That(ukNewsHeadlines.Count, Is.GreaterThan(8), "On the mock news headlines stream we should get 1 headline per second");
            KoanAssert.That(gbpusdPrices.Count, Is.GreaterThanOrEqualTo(1), "GBP/USD Prices should come through fairly regularly");
        }

        public void Dispose()
        {
            //After you are done with the connection, you need to disconnect
            if (_streamingClient != null)
            {
                _streamingClient.Dispose();
            }
        }

        private Client _rpcClient;
        private IStreamingClient _streamingClient;


// ReSharper disable UnusedMember.Local
        private int FILL_IN_THE_CORRECT_VALUE = 999;
// ReSharper restore UnusedMember.Local
// ReSharper disable UnusedMember.Local
        private object FILL_ME_IN = false;
// ReSharper restore UnusedMember.Local

    }
}
