namespace Lightstreamer.DotNet.Client.Log
{
    using System;
    using System.Collections.Generic;

    internal class LogManager
    {
        private static ILoggerProvider currentLoggerProvider = null;
        private static IDictionary<string, ILog> logInstances = new Dictionary<string, ILog>();

        internal static ILog GetLogger(string category)
        {
            lock (logInstances)
            {
                if (!logInstances.ContainsKey(category))
                {
                    if (currentLoggerProvider != null)
                    {
                        logInstances[category] = new ILog(currentLoggerProvider.GetLogger(category));
                    }
                    else
                    {
                        logInstances[category] = new ILog();
                    }
                }
                return logInstances[category];
            }
        }

        internal static void SetLoggerProvider(ILoggerProvider ilp)
        {
            lock (logInstances)
            {
                currentLoggerProvider = ilp;
                foreach (KeyValuePair<string, ILog> aLog in logInstances)
                {
                    if (ilp == null)
                    {
                        aLog.Value.setWrappedInstance(null);
                    }
                    else
                    {
                        aLog.Value.setWrappedInstance(currentLoggerProvider.GetLogger(aLog.Key));
                    }
                }
            }
        }
    }
}

