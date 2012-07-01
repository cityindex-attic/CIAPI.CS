using Lightstreamer.DotNet.Client.Log;
using System.Collections.Generic;

namespace Lightstreamer.DotNet.Client.Test
{
    class Log4NetLoggerWrapper : ILogger
    {
        private log4net.ILog wrapped;

        public Log4NetLoggerWrapper(log4net.ILog wrapped)
        {
            this.wrapped = wrapped;
        }

        public void Error(string line)
        {
            this.wrapped.Error(line);
        }

        public void Error(string line, System.Exception exception)
        {
            this.wrapped.Error(line,exception);
        }

        public void Warn(string line)
        {
            this.wrapped.Warn(line);
        }

        public void Warn(string line, System.Exception exception)
        {
            this.wrapped.Warn(line,exception);
        }

        public void Info(string line)
        {
            this.wrapped.Info(line);
        }

        public void Info(string line, System.Exception exception)
        {
            this.wrapped.Info(line,exception);
        }

        public void Debug(string line)
        {
            this.wrapped.Debug(line);
        }

        public void Debug(string line, System.Exception exception)
        {
            this.wrapped.Debug(line,exception);
        }

        public void Fatal(string line)
        {
            this.wrapped.Fatal(line);
        }

        public void Fatal(string line, System.Exception exception)
        {
            this.wrapped.Fatal(line,exception);
        }

        public bool IsDebugEnabled
        {
            get { return this.wrapped.IsDebugEnabled; }
        }

        public bool IsInfoEnabled
        {
            get { return this.wrapped.IsInfoEnabled; }
        }

        public bool IsWarnEnabled
        {
            get { return this.wrapped.IsWarnEnabled; }
        }

        public bool IsErrorEnabled
        {
            get { return this.wrapped.IsErrorEnabled; }
        }

        public bool IsFatalEnabled
        {
            get { return this.wrapped.IsFatalEnabled; }
        }
    }

    class Log4NetLoggerProviderWrapper : ILoggerProvider
    {
        private static IDictionary<string, Log4NetLoggerWrapper> logInstances = new Dictionary<string, Log4NetLoggerWrapper>();

        public ILogger GetLogger(string category)
        {
            lock (logInstances)
            {
                if (!logInstances.ContainsKey(category))
                {
                    logInstances[category] = new Log4NetLoggerWrapper(log4net.LogManager.GetLogger(category));
                }
                return logInstances[category];
            }
        }
            
    }
}
