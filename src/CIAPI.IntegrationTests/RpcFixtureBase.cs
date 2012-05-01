using System;
using System.Text;
using CIAPI.DTO;
using CIAPI.Rpc;
using CIAPI.Streaming;
using NUnit.Framework;
using Salient.ReflectiveLoggingAdapter;



namespace CIAPI.IntegrationTests.Streaming
{
    public class RpcFixtureBase 
    {
        protected ApiMarketInformationDTO[] GetAvailableCFDMarkets(Client rpcClient)
        {
            var marketList = rpcClient.Market.ListMarketInformationSearch(false, true, false, true, false, "GBP", 10, false);
            Assert.That(marketList.MarketInformation.Length, Is.GreaterThanOrEqualTo(1), "There should be at least 1 CFD market availbe");
            return marketList.MarketInformation;
        }

        protected const string AppKey = "testkey-for-CIAPI.IntegrationTests";
        static RpcFixtureBase()
        {
            LogManager.CreateInnerLogger = (logName, logLevel, showLevel, showDateTime, showLogName, dateTimeFormat)
   => new SimpleDebugAppender(logName, logLevel, showLevel, showDateTime, showLogName, dateTimeFormat);

            
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

    public class SimpleDebugAppender : AbstractAppender
    {
        public SimpleDebugAppender(string logName, LogLevel logLevel, bool showLevel, bool showDateTime, bool showLogName, string dateTimeFormat)
            : base(logName, logLevel, showLevel, showDateTime, showLogName, dateTimeFormat)
        {
        }

        protected override void WriteInternal(LogLevel level, object message, Exception exception)
        {
            var sb = new StringBuilder();
            FormatOutput(sb, level, message, exception);
            System.Diagnostics.Debug.WriteLine(sb.ToString());
        }


    }
}