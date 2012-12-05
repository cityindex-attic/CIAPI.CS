using System;
using System.Text;
using System.Threading;
using CIAPI.DTO;
using CIAPI.Rpc;
using CIAPI.Streaming;
using CIAPI.StreamingClient;
using CIAPI.Tests;
using NUnit.Framework;
using Salient.ReflectiveLoggingAdapter;



namespace CIAPI.IntegrationTests.Streaming
{
    [TestFixture]
    public class DisposableFixture : RpcFixtureBase
    {
        [Test]
        public void ShouldThrowObjectDisposedException()
        {

            var client = this.BuildRpcClient();
            client.LogIn(Settings.RpcUserName, Settings.RpcPassword);
            IStreamingClient streamingClient = client.CreateStreamingClient();
            
            
            client.Dispose();
            streamingClient.Dispose();

            Assert.Throws(typeof(ObjectDisposedException), () => client.BeginLogIn("", "", a => { }, null), "async calls not guarded");
            Assert.Throws(typeof(ObjectDisposedException), () => client.AccountInformation.GetClientAndTradingAccount(), "sync calls not guarded");
            Assert.Throws(typeof(ObjectDisposedException), () => client.CreateStreamingClient(), "streaming client factory method not guarded");
            Assert.Throws(typeof(ObjectDisposedException), () => streamingClient.BuildDefaultPricesListener(9), "streaming client listener factory method not guarded");



        }

    }
    public class RpcFixtureBase 
    {
        protected ApiMarketInformationDTO[] GetAvailableCFDMarkets(Client rpcClient)
        {
            var marketList = rpcClient.Market.ListMarketInformationSearch(false, true, false, true, false, true, "GBP", 10, false);
            Assert.That(marketList.MarketInformation.Length, Is.GreaterThanOrEqualTo(1), "There should be at least 1 CFD market availbe");
            return marketList.MarketInformation;
        }
        public RpcFixtureBase()
        {
            Thread.Sleep(1500);
        }
        protected const string AppKey = "testkey-for-CIAPI.IntegrationTests";
        static RpcFixtureBase()
        {
            LogManager.CreateInnerLogger = (logName, logLevel, showLevel, showDateTime, showLogName, dateTimeFormat)
   => new SimpleTraceAppender(logName, logLevel, showLevel, showDateTime, showLogName, dateTimeFormat);

            
        }

        public Client BuildRpcClient()
        {
            var rpcClient = new Client(Settings.RpcUri, Settings.StreamingUri, AppKey);
            rpcClient.LogIn(Settings.RpcUserName, Settings.RpcPassword);
            return rpcClient;
        }


        public IStreamingClient BuildStreamingClient()
        {
            var authenticatedClient = new CIAPI.Rpc.Client(Settings.RpcUri, Settings.StreamingUri, AppKey);
            authenticatedClient.LogIn(Settings.RpcUserName, Settings.RpcPassword);
            return authenticatedClient.CreateStreamingClient();
        }

 
    }




    

}