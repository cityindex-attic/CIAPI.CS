using CIAPI.IntegrationTests.Streaming;

using Salient.ReflectiveLoggingAdapter;
using NUnit.Framework;

namespace CIAPI.IntegrationTests
{
    [TestFixture]
    public class LoggingFixture : RpcFixtureBase
    {

        [Test]
        public void DefaultLogging()
        {
            //By default CIAPI.CS will log to the Debug Console
            var rpcClient = BuildRpcClient();

            //Make some HTTP requests
            rpcClient.News.ListNewsHeadlinesWithSource("dj", "UK", 100);
            rpcClient.LogOut();
            rpcClient.Dispose();

            //Check your debug console - you will see some messages about the HTTP traffic that has happened
        }

        [Test]
        public void RouteLoggingToLog4Net()
        {
            //Make Log4Net to configure itself from the App.Config
            log4net.Config.XmlConfigurator.Configure();

            //replace the default DebugLogger with Log4Net, which - in this example - has been configured in App.Config to 
            //write to the console and prefix each message with [LOG4NET]
            LogManager.CreateInnerLogger = (logName, logLevel, showLevel, showDateTime, showLogName, dateTimeFormat) =>
            {
                // create external logger implementation and return instance.
                // this will be called whenever CIAPI requires a logger
                return log4net.LogManager.GetLogger(logName);
            };

            var rpcClient = BuildRpcClient();

            //Make some HTTP requests
            rpcClient.News.ListNewsHeadlinesWithSource("dj", "UK", 100);
            rpcClient.LogOut();
            rpcClient.Dispose();

            //Check your debug console - you will see some messages about the HTTP traffic that has happened - but prefixed with [LOG4NET]
        }
        
    }

 
}