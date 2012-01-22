using System;
using System.Text;

namespace CityIndex.ReflectiveLoggingAdapter
{
    public class DebugAppender : AbstractAppender
    {
        public DebugAppender(string logName, LogLevel logLevel, bool showLevel, bool showDateTime, bool showLogName,
                             string dateTimeFormat)
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