using System;
using System.Text;
using System.Windows;
 

namespace Common.Logging
{
    public class DebugAppender : AbstractSimpleLogger
    {
        // Methods
        public DebugAppender(string logName, LogLevel logLevel, bool showLevel, bool showDateTime, bool showLogName, string dateTimeFormat)
            : base(logName, logLevel, showLevel, showDateTime, showLogName, dateTimeFormat)
        {
        }

        protected override void WriteInternal(LogLevel level, object message, Exception e)
        {
            var stringBuilder = new StringBuilder();
            this.FormatOutput(stringBuilder, level, message, e);

            WriteToConsole(level, stringBuilder.ToString());
        }

        private void WriteToConsole(LogLevel level, string message)
        {

            System.Diagnostics.Debug.WriteLine(level.ToString() + " " + message);

        }

    }
}
