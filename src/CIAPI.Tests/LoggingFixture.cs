using Salient.ReflectiveLoggingAdapter;
using NUnit.Framework;
using Rhino.Mocks;

namespace CIAPI.Tests
{
    [TestFixture]
    public class LoggingFixture
    {
        [Test]
        public void EnsureInnerLoggerIsCalled()
        {
            var mockInnerLogger = MockRepository.GenerateMock<ILog>();

            LogManager.CreateInnerLogger = (logName, logLevel, showLevel, showDateTime, showLogName, dateTimeFormat) =>
            {
                // create external logger implementation and return instance.
                // this will be called whenever CIAPI requires a logger
                return mockInnerLogger;
            };

            //Sky TODO - test the mockInnerLogger gets called by the RPC client and the StreamingClient
            
        }
        
    }

 
}