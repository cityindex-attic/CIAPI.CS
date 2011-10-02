using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using NUnit.Framework;

namespace CIAPI.TradingController
{

    [TestFixture,Ignore]
    public class TestHarness
    {
        [Test]
        public void Test()
        {
            var controller = new Controller("https://ciapi.cityindex.com/tradingapi", "https://push.cityindex.com", "DM925308", "password");

            // streaming updates are recieved via events exposed on the controller..

            // hook up the price update event and just send it to console
            controller.PriceUpdate += (sender,e)=> Console.WriteLine("{2} {0} {1}", e.Data.MarketId, e.Data.Price, e.Data.TickDate);

            // if subscribing to news use controller.NewsUpdate

            // connect to the rpc and push servers, fetch initial static data and lookups
            controller.Connect();
            

            // subscribe to prices by 
            controller.SubscribePrices(80905);

            // wait a second to let some prices come in
            new ManualResetEvent(false).WaitOne(1000);


            // get a current price from which to make a trade
            // you can also respond to a particular price in the PriceUpdate event.

            var price = controller.GetCurrentPrices()[80905];

            var gate = new ManualResetEvent(false);

            // make a buy and then sell it
            controller.PlaceTradeAsync(controller.SpreadBettingAccount.TradingAccountId, price.MarketId, TradeDirection.Buy, 1, price.Bid, price.Offer, result =>
                {
                    var orderOpen = result.Data;
                    Assert.AreEqual("OK", orderOpen.Status_Resolved);

                    controller.PlaceTradeAsync(controller.SpreadBettingAccount.TradingAccountId, price.MarketId, TradeDirection.Sell, 1, price.Bid, price.Offer, result2 =>
                    {
                        var closeOrder = result2.Data;
                        Assert.AreEqual("OK", closeOrder.Status_Resolved);

                        gate.Set();
                    });
                });

            gate.WaitOne();


            // add markets to your price subscription
            controller.SubscribePrices(400535967, 81136, 400509294, 400535971);

            // wait a second
            new ManualResetEvent(false).WaitOne(1000);

            // remove markets from your price subscription
            controller.UnsubscribePrices(400535967, 81136, 400509294, 400535971);

            new ManualResetEvent(false).WaitOne(1000);

            // clear all subscriptions like so. this is just illustration, not necessary when disconnecting
            controller.UnsubscribeAllPrices();

            controller.Disconnect();
            
        }

   
    }
}
