using System;

namespace CityIndex.ReflectiveLoggingAdapter
{
    public class LogManager
    {
        public delegate object CreateInnerLoggerDelegate(string logName, LogLevel logLevel, bool showLevel, bool showDateTime, bool showLogName,
                             string dateTimeFormat);

        static LogManager()
        {
            CreateInnerLogger  = GetInnerLogger;
        }

        public static CreateInnerLoggerDelegate CreateInnerLogger { get; set; }
        
        public static bool IsDebug = false;

        public static ILog GetLogger(Type type)
        {
            var inner = CreateInnerLogger(type.Name, LogLevel.All, true, true, true, "u");
            return new LogAdapter(inner);
        }

        public static ILog GetLogger(string logName, LogLevel logLevel, bool showLevel, bool showDateTime, bool showLogName,
                             string dateTimeFormat)
        {
            var inner = CreateInnerLogger(logName, logLevel, showLevel, showDateTime, showLogName, dateTimeFormat);
            return new LogAdapter(inner);
        }

        private static object GetInnerLogger(string logName, LogLevel logLevel, bool showLevel, bool showDateTime, bool showLogName,
                             string dateTimeFormat)
        {
            return new DebugAppender(logName, logLevel, showLevel, showDateTime,showLogName, dateTimeFormat);
        }

    }
}