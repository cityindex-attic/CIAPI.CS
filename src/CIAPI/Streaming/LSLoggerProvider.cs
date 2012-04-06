using System;
using Lightstreamer.DotNet.Client.Log;
using Salient.ReflectiveLoggingAdapter;

namespace StreamingClient.Lightstreamer
{
    internal class LSLoggerProvider : ILoggerProvider
    {
        public ILogger GetLogger(string category)
        {
            return new LSLogger(category, LogLevel.All, true, true, true, null);
        }
    }
}