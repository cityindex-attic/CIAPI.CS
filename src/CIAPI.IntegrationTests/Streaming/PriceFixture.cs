using System;
using System.Collections.Generic;
using System.Threading;
using CIAPI.DTO;
using CIAPI.Streaming;
using Common.Logging;
using NUnit.Framework;
using IStreamingClient = CIAPI.Streaming.IStreamingClient;

namespace CIAPI.IntegrationTests.Streaming
{
    [TestFixture]
    public class PriceFixture
    {
        private ILog _logger = LogManager.GetCurrentClassLogger();

        public static IStreamingClient BuildStreamingClient(
            string userName = "xx189949",
            string password = "password")
        {
            const string apiUrl = "https://ciapipreprod.cityindextest9.co.uk/TradingApi/";

            var authenticatedClient = new CIAPI.Rpc.Client(new Uri(apiUrl));
            authenticatedClient.LogIn(userName, password);

            var streamingUri = new Uri("https://pushpreprod.cityindextest9.co.uk");

            return StreamingClientFactory.CreateStreamingClient(streamingUri, userName, authenticatedClient.Session);
        }

        [
        Test
            //,Ignore("this test is best run interactively when market is responsive. after hours there is no pricing so test will fail")
        ]
        public void CanConsumePriceStream()
        {
            var gate = new ManualResetEvent(false);

            var streamingClient = BuildStreamingClient();

            //streamingClient.Connect();



            var priceListener = streamingClient.BuildPricesListener(71442);
            //priceListener.Start();

            PriceDTO actual = null;

            //Trap the Price given by the update event for checking


            priceListener.MessageReceived += (s, e) =>
            {
                actual = e.Data;
                gate.Set();
            };


            bool gotPriceInTime = true;
            if (!gate.WaitOne(TimeSpan.FromSeconds(15)))
            {
                gotPriceInTime = false;
                // don't want to throw while client is listening, hangs test
            }

            priceListener.Stop();
            //streamingClient.Disconnect();


            Assert.IsTrue(gotPriceInTime, "A price update wasn't received in time");
            Assert.IsNotNull(actual);
            Assert.Greater(actual.TickDate, DateTime.UtcNow.AddSeconds(-10), "We're expecting a recent price");
        }


        /* 24/5 fast pricing FX markets

         * SKY: these don't seem to be running very often - i just used the above and a 2 off to get the test running
         * please update with a longer list of valid markets
         * 
         * 
            {"MarketId":400481115,"Name":"AUD\/JPY"},
            {"MarketId":400481116,"Name":"AUD\/NZD"},
            {"MarketId":400481117,"Name":"AUD\/USD"},
            {"MarketId":400481118,"Name":"CAD\/CHF"},
            {"MarketId":400481119,"Name":"CAD\/JPY"},
            {"MarketId":400481120,"Name":"CHF\/JPY"},
            {"MarketId":400481121,"Name":"EUR\/AUD"},
            {"MarketId":400481122,"Name":"EUR\/CAD"},
        */

        [Test
            //, Ignore("Only works during market opening hours.  Run manually")
        ]
        public void CanSubscribeToMultiplePriceStreamsAtOnce()
        {
            var streamingClient = BuildStreamingClient();
            //streamingClient.Connect();
            var priceListener = streamingClient.BuildPricesListener(new[]{
                                                                         71442,
                                                                         71443
                                                                    });

            var prices = new Dictionary<string, PriceDTO>();
            priceListener.MessageReceived += (s, e) =>
                prices[e.Data.MarketId.ToString()] = e.Data;

            try
            {
                //priceListener.Start();

                Thread.Sleep(TimeSpan.FromSeconds(15));

                lock (prices)
                {
                    if (prices.Count == 0)
                    {
                        Assert.Fail("No price updates were recieved");
                    }

                    foreach (var price in prices)
                    {
                        _logger.DebugFormat(price.ToStringWithValues());
                    }
                }

            }
            finally
            {
                priceListener.Stop();
                //streamingClient.Disconnect();
            }
        }


    }
}
