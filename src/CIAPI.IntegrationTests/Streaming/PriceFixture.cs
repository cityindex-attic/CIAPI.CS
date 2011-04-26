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
            string userName = "0x234",
            string password = "password")
        {
            const string apiUrl = "https://ciapipreprod.cityindextest9.co.uk/TradingApi/";

            var authenticatedClient = new CIAPI.Rpc.Client(new Uri(apiUrl));
            authenticatedClient.LogIn(userName, password);

            var streamingUri = new Uri("https://pushpreprod.cityindextest9.co.uk/CITYINDEXSTREAMING");

            return StreamingClientFactory.CreateStreamingClient(streamingUri, userName, authenticatedClient.SessionId);
        }

        [Test,Ignore("this test is best run interactively when market is responsive. after hours there is no pricing so test will fail")]
        public void CanConsumePriceStream()
        {
            var gate = new ManualResetEvent(false);

            var streamingClient = BuildStreamingClient();

            streamingClient.Connect();

            

            var priceListener = streamingClient.BuildPriceListener("PRICES.PRICE.71442");
            priceListener.Start();

            PriceDTO actual = null;

            //Trap the Price given by the update event for checking

            priceListener.MessageReceived += (s, e) =>
            {
                actual = e.Data;
                gate.Set();
            };


            if (!gate.WaitOne(TimeSpan.FromSeconds(5)))
            {
                Assert.Fail("A price update wasn't received in time");
            }

            priceListener.Stop();
            streamingClient.Disconnect();

            Assert.IsNotNull(actual);
            Assert.Greater(actual.TickDate, DateTime.UtcNow.AddSeconds(-10), "We're expecting a recent price");
        }


        /* 24/5 fast pricing FX markets

            {"MarketId":400481115,"Name":"AUD\/JPY"},
            {"MarketId":400481116,"Name":"AUD\/NZD"},
            {"MarketId":400481117,"Name":"AUD\/USD"},
            {"MarketId":400481118,"Name":"CAD\/CHF"},
            {"MarketId":400481119,"Name":"CAD\/JPY"},
            {"MarketId":400481120,"Name":"CHF\/JPY"},
            {"MarketId":400481121,"Name":"EUR\/AUD"},
            {"MarketId":400481122,"Name":"EUR\/CAD"},
        */

        [Test, Ignore("Only works during market opening hours.  Run manually")]
        public void CanSubscribeToMultiplePriceStreamsAtOnce()
        {
            var streamingClient = BuildStreamingClient();
            streamingClient.Connect();
            var priceListener = streamingClient.BuildPriceListener(new[]{
                                                                         "PRICES.PRICE.400481115",
                                                                         "PRICES.PRICE.400481116",
                                                                         "PRICES.PRICE.400481118",
                                                                         "PRICES.PRICE.400481119",
                                                                         "PRICES.PRICE.400481120",
                                                                         "PRICES.PRICE.400481121",
                                                                         "PRICES.PRICE.400481122"
                                                                    });
            
            var prices = new List<PriceDTO>();
            priceListener.MessageReceived += (s, e) => prices.Add(e.Data);

            try
            {
                priceListener.Start();

                Thread.Sleep(TimeSpan.FromSeconds(15));
                
                if (prices.Count == 0)
                {
                    Assert.Fail("No price updates were recieved");
                }

                foreach (var price in prices)
                {
                    _logger.DebugFormat(price.ToStringWithValues());
                }
            }
            finally
            {
                priceListener.Stop();
                streamingClient.Disconnect();
            }
        }


    }
}
