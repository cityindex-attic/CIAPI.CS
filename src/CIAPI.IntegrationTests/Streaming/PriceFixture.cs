//using System;
//using System.Threading;
//using NUnit.Framework;

//namespace CIAPI.IntegrationTests.Streaming
//{
//    [TestFixture]
//    public class PriceFixture
//    {
//        [Test]
//        public void CanConsumePriceStream()
//        {
//            var gate = new ManualResetEvent(false);

//            const string apiUrl = "https://ciapipreprod.cityindextest9.co.uk/TradingApi/";
//            const string streamingUrl = "https://pushpreprod.cityindextest9.co.uk:443";
//            const string userName = "0X234";
//            const string password = "password";
//            const string adapterName = "CITYINDEXSTREAMING";

//            var ctx = new CIAPI.Rpc.Client(new Uri(apiUrl));
//            ctx.LogIn(userName, password);

//            ILightstreamerConnection connection = new LightstreamerConnection(streamingUrl, userName, ctx.SessionId.ToString(), adapterName);
//            var priceListener = new PriceListener("MOCKPRICE.", 1000, connection);
//            priceListener.Subscribe();
            
//            //Trap the Price given by the update event for checking
//            Price latestPrice = new NullPrice();
//            priceListener.Update += (s, e) =>
//            {
//                latestPrice = e.Item.Price;
//                Console.WriteLine(latestPrice.ToString());
//                gate.Set();
//            };

//            gate.WaitOne(TimeSpan.FromSeconds(5));
//            priceListener.Unsubscribe();

//            Assert.AreNotEqual(new NullPrice().ToString(), latestPrice.ToString());
//        }
//    }
//}
