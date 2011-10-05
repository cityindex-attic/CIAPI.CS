using Lightstreamer.DotNet.Client.Log;

namespace CIAPI.Phone7.Tests.Lightstreamer
{
    public class DebugLoggerProvider : ILoggerProvider
    {
        public ILogger GetLogger(string category)
        {
            return new DebugLogger();
        }
    }
}