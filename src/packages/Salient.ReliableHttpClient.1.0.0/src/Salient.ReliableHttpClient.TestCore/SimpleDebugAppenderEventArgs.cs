using System;
using Salient.ReflectiveLoggingAdapter;

namespace Salient.ReliableHttpClient.TestCore
{
    public class SimpleDebugAppenderEventArgs : EventArgs
    {
        public LogLevel Level;
        public object Message;
        public Exception Exception;
    }
}