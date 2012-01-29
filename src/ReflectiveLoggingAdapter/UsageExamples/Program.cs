using System;
using CityIndex.ReflectiveLoggingAdapter;

namespace UsageExamples
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            DefaultConsoleLoggerSimple();
            DefaultConsoleLoggerDetailed();
            ExternalLogging();
            Console.WriteLine("press a key");
            Console.ReadKey();
        }

        private static void DefaultConsoleLoggerSimple()
        {
            // typical simple log instantiation. default logger logs to
            // debug console
            ILog log = LogManager.GetLogger(typeof (Program));
            log.Debug("simple default logger");
        }

        private static void DefaultConsoleLoggerDetailed()
        {
            // typical detailed log instantiation. default logger logs to
            // debug console
            ILog log = LogManager.GetLogger("log", LogLevel.All, true, true, true, "u");
            log.Debug("detailed default logger");
        }

        private static void ExternalLogging()
        {
            log4net.Config.XmlConfigurator.Configure();
            // demonstrates how to inject an external logger
            LogManager.CreateInnerLogger = (logName, logLevel, showLevel, showDateTime, showLogName, dateTimeFormat) =>
                                               {
                                                   // create external logger implementation and return instance.
                                                   // this will be called whenever CIAPI requires a logger
                                                   return log4net.LogManager.GetLogger(logName);
                                               };

            ILog log = LogManager.GetLogger(typeof (Program));
            log.Debug("external logger");
            ;
        }
    }
}