﻿using System;
using CIAPI.DTO;
using CIAPI.IntegrationTests.Streaming;
using CIAPI.Rpc;
using NUnit.Framework;

namespace CIAPI.IntegrationTests.Rpc
{
    [TestFixture]
    public class FaqIssueResolutionFixture : RpcFixtureBase
    {
        [Test]
        public void Issue243()
        {
            var rpcClient = BuildRpcClient();
            rpcClient.LogIn("xx663766", "password1");
            var x = rpcClient.TradesAndOrders.ListOpenPositions(0);



        } 


        [Test,Ignore("core functionality broken")]
        public void CanEmptyQueueAndShutdown()
        {
            using (var rpcClient = BuildRpcClient())
            {
                for (var i = 0; i < 1000; i++)
                {
                    Console.WriteLine(i + " queued");

                    rpcClient.BeginLogIn("dm" + i, "password", r =>
                                                               {
                                                                   try
                                                                   {
                                                                       rpcClient.EndLogIn(r);
                                                                       Console.WriteLine(r.AsyncState + " succeeded?!");
                                                                   }
                                                                   catch (Exception)
                                                                   {

                                                                       Console.WriteLine(r.AsyncState + " failed");

                                                                   }

                                                               }, i);
                }
                Console.WriteLine("killing queue and shutting down");
                using (var handle = rpcClient.ShutDown(true))
                {
                    handle.WaitOne();
                }
                Console.WriteLine("shut down");
            
            }
        }

        [Test]
        public void Purge()
        {
            using (var rpcClient = BuildRpcClient())
            {
                using (var handle = rpcClient.ShutDown())
                {
                    handle.WaitOne();
                }
                rpcClient.Dispose();
            }
        }

        /// <summary>
        /// http://faq.labs.cityindex.com/questions/why-getpricebars-sometimes-does-not-return-back-a-getpricebarresponsedto-object-or-throw-an-error
        /// </summary>
        [Test]
        public void Json_Ho_01()
        {

            var rpcClient = BuildRpcClient();



            GetPriceBarResponseDTO bars = rpcClient.PriceHistory.GetPriceBars("71442", "MINUTE", 1, "15");
            Assert.IsNotNull(bars);

            rpcClient.LogOut();
        }



        /// <summary>
        /// http://github.com/cityindex/CIAPI.CS/issues/42
        /// </summary>
        [Test, Ignore("test is broken - need proper setup")]
        public void Issue42()
        {

            var rpcClient = BuildRpcClient();

            var param = new NewStopLimitOrderRequestDTO
                            {
                                OrderId = 0,
                                MarketId = 99498,
                                Currency = null,
                                AutoRollover = false,
                                Direction = "buy",
                                Quantity = 10m,
                                BidPrice = 12094m,
                                OfferPrice = 12098m,
                                AuditId = "20110629-G2PREPROD3-0102794",
                                TradingAccountId = 400002249,
                                IfDone = null,
                                OcoOrder = null,
                                Applicability = null,
                                ExpiryDateTimeUTC = null,
                                Guaranteed = false,
                                TriggerPrice = 0m
                            };
            var response = rpcClient.TradesAndOrders.Order(param);


            rpcClient.LogOut();
        }


        /// <summary>
        /// http://github.com/cityindex/CIAPI.CS/issues/35
        /// </summary>
        [Test, Ignore("test is broken - need proper setup")]
        public void Issue35()
        {

            var rpcClient = BuildRpcClient();

            var accountInfo = rpcClient.AccountInformation.GetClientAndTradingAccount();
            var resp = rpcClient.TradesAndOrders.ListTradeHistory(accountInfo.ClientAccountId, 20);


            rpcClient.LogOut();
        }
    }





}
