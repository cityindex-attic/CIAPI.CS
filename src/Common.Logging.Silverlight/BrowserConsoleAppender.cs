using System;
using System.Text;
using System.Windows;
using System.Windows.Browser;

namespace Common.Logging
{
    public class BrowserConsoleAppender : AbstractSimpleLogger
    {
        // Methods
        public BrowserConsoleAppender(string logName, LogLevel logLevel, bool showLevel, bool showDateTime, bool showLogName, string dateTimeFormat)
            : base(logName, logLevel, showLevel, showDateTime, showLogName, dateTimeFormat)
        {
        }

        protected override void WriteInternal(LogLevel level, object message, Exception e)
        {
            var stringBuilder = new StringBuilder();
            this.FormatOutput(stringBuilder, level, message, e);

            WriteToConsole(stringBuilder.ToString());
        }

        private void WriteToConsole(string message)
        {
            if (!System.Windows.Deployment.Current.Dispatcher.CheckAccess())
                System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() => WriteToConsole(message));
            try
            {
                var isConsoleAvailable = (bool)HtmlPage.Window.Eval("typeof(console) != 'undefined' && typeof(console.log) != 'undefined'");
                if (!isConsoleAvailable) return;

                var console = (HtmlPage.Window.Eval("console.log") as ScriptObject);
                if (console != null)
                {
                    console.InvokeSelf(message);
                }
            }
            catch
            {
                //ignore
            }
        }

    }
}
