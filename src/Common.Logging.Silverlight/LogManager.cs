using System;

namespace Common.Logging
{
    public class LogManager
    {
        public static bool IsDebug = false;
        public static ILog GetLogger(Type type)
        {
            if (IsDebug)
            {
                return GetDebugLogger(type);
            }
            else
            {
                return GetBrowserLogger(type);    
            }
            
        }
        private static BrowserConsoleAppender GetBrowserLogger(Type type)
        {
            return new BrowserConsoleAppender(type.FullName, LogLevel.All, true, true, true, "u");
        }
        private static DebugAppender GetDebugLogger(Type type)
        {
            return new DebugAppender(type.FullName, LogLevel.All, true, true, true, "u");
        }
    }
}
