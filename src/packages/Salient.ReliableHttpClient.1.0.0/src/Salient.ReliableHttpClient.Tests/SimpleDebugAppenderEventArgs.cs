using System;
using Salient.ReflectiveLoggingAdapter;

namespace Salient.ReliableHttpClient.Tests
{
    public class SimpleDebugAppenderEventArgs : EventArgs
    {
        public LogLevel Level;
        public object Message;
        public Exception Exception;
    }
}