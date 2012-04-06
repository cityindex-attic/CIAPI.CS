using System;
using System.Text;
using Salient.ReflectiveLoggingAdapter;


namespace Salient.ReliableHttpClient.TestCore
{
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
            OnLogEvent(new SimpleDebugAppenderEventArgs { Exception = exception, Level = level, Message = message });
        }

        public event EventHandler<SimpleDebugAppenderEventArgs> LogEvent;

        public void OnLogEvent(SimpleDebugAppenderEventArgs e)
        {
            EventHandler<SimpleDebugAppenderEventArgs> handler = LogEvent;
            if (handler != null) handler(this, e);
        }
    }
}