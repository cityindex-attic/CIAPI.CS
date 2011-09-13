namespace Lightstreamer.DotNet.Client.Log
{
    using System;

    internal class ILog : ILogger
    {
        private static ILogger placeholder = new ILogEmpty();
        private ILogger wrappedLogger;

        internal ILog()
        {
            this.wrappedLogger = placeholder;
        }

        internal ILog(ILogger iLogger)
        {
            this.wrappedLogger = placeholder;
            this.wrappedLogger = iLogger;
        }

        public void Debug(string p)
        {
            this.wrappedLogger.Debug(p);
        }

        public void Debug(string p, Exception e)
        {
            this.wrappedLogger.Debug(p, e);
        }

        public void Error(string p)
        {
            this.wrappedLogger.Error(p);
        }

        public void Error(string p, Exception e)
        {
            this.wrappedLogger.Error(p, e);
        }

        public void Fatal(string p)
        {
            this.wrappedLogger.Fatal(p);
        }

        public void Fatal(string p, Exception e)
        {
            this.wrappedLogger.Fatal(p);
        }

        public void Info(string p)
        {
            this.wrappedLogger.Info(p);
        }

        public void Info(string p, Exception e)
        {
            this.wrappedLogger.Info(p, e);
        }

        internal void setWrappedInstance(ILogger iLogger)
        {
            if (iLogger == null)
            {
                this.wrappedLogger = placeholder;
            }
            else
            {
                this.wrappedLogger = iLogger;
            }
        }

        public void Warn(string p)
        {
            this.wrappedLogger.Warn(p);
        }

        public void Warn(string p, Exception e)
        {
            this.wrappedLogger.Warn(p, e);
        }

        public bool IsDebugEnabled
        {
            get
            {
                return this.wrappedLogger.IsDebugEnabled;
            }
        }

        public bool IsErrorEnabled
        {
            get
            {
                return this.wrappedLogger.IsErrorEnabled;
            }
        }

        public bool IsFatalEnabled
        {
            get
            {
                return this.wrappedLogger.IsFatalEnabled;
            }
        }

        public bool IsInfoEnabled
        {
            get
            {
                return this.wrappedLogger.IsInfoEnabled;
            }
        }

        public bool IsWarnEnabled
        {
            get
            {
                return this.wrappedLogger.IsWarnEnabled;
            }
        }
    }
}

