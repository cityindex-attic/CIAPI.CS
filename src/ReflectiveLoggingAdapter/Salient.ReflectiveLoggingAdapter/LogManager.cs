using System;

namespace CityIndex.ReflectiveLoggingAdapter
{
    public class LogManager
    {
        public delegate object CreateInnerLoggerDelegate(Type type);

        static LogManager()
        {
            CreateInnerLogger  = GetInnerLogger;
        }

        public static CreateInnerLoggerDelegate CreateInnerLogger { get; set; }
        
        public static bool IsDebug = false;

        public static ILog GetLogger(Type type)
        {
            object inner = GetInnerLogger(type);
            return new LogAdapter(inner);
        }

        private static object GetInnerLogger(Type type)
        {
            return new DebugAppender(type.Name, LogLevel.All, true, true, true, "u");
        }

    }
}