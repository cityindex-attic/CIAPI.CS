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

            WriteToConsole(level, stringBuilder.ToString());
        }

        private void WriteToConsole(LogLevel level, string message)
        {
            if (!Deployment.Current.Dispatcher.CheckAccess())
                Deployment.Current.Dispatcher.BeginInvoke(delegate { WriteToConsole(level, message); });
            try
            {
                var safeMessage = message.Replace('"', '\'').Replace("\r\n", @"\n");
                switch (level)
                {
                    case LogLevel.Fatal:
                    case LogLevel.Error:
                        HtmlPage.Window.Eval("try {console.error(\"" + safeMessage + "\") } catch(e) { try {console.log(\"" + safeMessage + "\") } catch(e) { /* no logging available, so do nothing */ } }");
                        break;
                    case LogLevel.Warn:
                        HtmlPage.Window.Eval("try {console.warn(\"" + safeMessage + "\") } catch(e)  { try {console.log(\"" + safeMessage + "\") } catch(e) { /* no logging available, so do nothing */ } }");
                        break;
                    case LogLevel.Debug:
                        HtmlPage.Window.Eval("try {console.debug(\"" + safeMessage + "\") } catch(e)  { try {console.log(\"" + safeMessage + "\") } catch(e) { /* no logging available, so do nothing */ } }");
                        break;
                    case LogLevel.Info:
                        HtmlPage.Window.Eval("try {console.info(\"" + safeMessage + "\") } catch(e)  { try {console.log(\"" + safeMessage + "\") } catch(e) { /* no logging available, so do nothing */ } }");
                        break;
                    default:
                        HtmlPage.Window.Eval("try {console.log(\"" + safeMessage + "\") } catch(e) { /* no logging available, so do nothing */ }");
                        break;
                }
            }
            catch
            {
                //ignore
            }
        }

    }
}
