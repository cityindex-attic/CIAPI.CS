using System.Text;
using Salient.ReflectiveLoggingAdapter;

namespace Salient.ReliableHttpClient.Tests
{
    public class LoggingFixtureBase
    {
        static readonly StringBuilder LogOutput=new StringBuilder();
        public static string GetLogOutput()
        {
            lock (LogOutput)
            {
                var output = LogOutput.ToString();
                LogOutput.Clear();
                return output;
            }
        }
        static LoggingFixtureBase()
        {

            //Hook up a logger for the CIAPI.CS libraries
            LogManager.CreateInnerLogger = (logName, logLevel, showLevel, showDateTime, showLogName, dateTimeFormat)
                                           =>
                                               {
                                                   SimpleDebugAppender logger = new SimpleDebugAppender(logName, logLevel, showLevel, showDateTime, showLogName, dateTimeFormat);
                                                   logger.LogEvent += (s, e) =>
                                                                          {
                                                                              lock (LogOutput)
                                                                              {
                                                                                  LogOutput.AppendLine(string.Format("{0} {1} {2}", e.Level, e.Message, e.Exception));
                                                                              }
                                                                          };
                                                   return logger;
                                               };
        }
    }
}