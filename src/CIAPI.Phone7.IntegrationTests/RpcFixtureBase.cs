using System;
using System.Text;
using CIAPI.DTO;
using CIAPI.Phone7.IntegrationTests;
using CIAPI.Rpc;
using CIAPI.Serialization;
using CIAPI.Streaming;
using Salient.ReflectiveLoggingAdapter;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;



namespace CIAPI.IntegrationTests.Streaming
{
    public class RpcFixtureBase : SilverlightTest
    {
        protected const string AppKey = "testkey-for-CIAPI.IntegrationTests";
        static RpcFixtureBase()
        {
            LogManager.CreateInnerLogger = (logName, logLevel, showLevel, showDateTime, showLogName, dateTimeFormat)
   => new SimpleDebugAppender(logName, logLevel, showLevel, showDateTime, showLogName, dateTimeFormat);
            // need to set up the serializer to be used by stream listeners
            StreamingClientFactory.SetSerializer(new Serializer());
        }

        public Client BuildRpcClient()
        {
            var rpcClient = new Client(Settings.RpcUri,Settings.StreamingUri, AppKey);
            rpcClient.LogIn(Settings.RpcUserName, Settings.RpcPassword);
            return rpcClient;
        }


        public IStreamingClient BuildStreamingClient()
        {
            var authenticatedClient = new CIAPI.Rpc.Client(Settings.RpcUri, Settings.StreamingUri, AppKey);
            authenticatedClient.LogIn(Settings.RpcUserName, Settings.RpcPassword);
            return StreamingClientFactory.CreateStreamingClient(Settings.StreamingUri, Settings.RpcUserName, authenticatedClient.Session);
        }
        protected ApiMarketInformationDTO[] GetAvailableCFDMarkets(Client rpcClient)
        {
            var marketList = rpcClient.Market.ListMarketInformationSearch(false, true, false, true, false, "GBP", 10, false);
            //Assert.That(marketList.MarketInformation.Length, Is.GreaterThanOrEqualTo(1), "There should be at least 1 CFD market availbe");
            return marketList.MarketInformation;
        }
     
    }

 
}