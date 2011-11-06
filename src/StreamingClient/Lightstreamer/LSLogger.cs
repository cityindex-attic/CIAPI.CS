using System;
using Common.Logging;
using Lightstreamer.DotNet.Client.Log;
#if !SILVERLIGHT
    using Common.Logging.Simple;
#endif

namespace StreamingClient.Lightstreamer
{
    internal class LSLogger : ILogger
    {
        public LSLogger(string logName, LogLevel logLevel, bool showLevel, bool showDateTime, bool showLogName, string dateTimeFormat)
        {
#if SILVERLIGHT

#if WINDOWS_PHONE
            _wrapped = new DebugAppender(logName, logLevel, showLevel, showDateTime, showLogName, dateTimeFormat);
#else
            _wrapped = new DebugAppender(logName, logLevel, showLevel, showDateTime, showLogName, dateTimeFormat);
#endif
#else
            _wrapped = new Common.Logging.Simple.ConsoleOutLogger(logName, logLevel, showLevel, showDateTime, showLogName, dateTimeFormat);
#endif

        }
        private AbstractSimpleLogger _wrapped;

        public void Error(string line)
        {
            _wrapped.Error(line);
            
            
        }

        public void Error(string line, Exception exception)
        {
            _wrapped.Error(line,exception);
        }

        public void Warn(string line)
        {
            _wrapped.Warn(line);
        }

        public void Warn(string line, Exception exception)
        {
            _wrapped.Warn(line,exception);
        }

        public void Info(string line)
        {
            _wrapped.Info(line);
        }

        public void Info(string line, Exception exception)
        {
            _wrapped.Info(line,exception);
        }

        public void Debug(string line)
        {
            _wrapped.Debug(line);
        }

        public void Debug(string line, Exception exception)
        {
            _wrapped.Debug(line,exception);
        }

        public void Fatal(string line)
        {
            _wrapped.Fatal(line);
        }

        public void Fatal(string line, Exception exception)
        {
            _wrapped.Fatal(line,exception);
        }


        private bool _isDebugEnabled = true;
        public bool IsDebugEnabled
        {
            get { return _isDebugEnabled; }
            set { _isDebugEnabled = value; }
        }

        private bool _isInfoEnabled=true;
        public bool IsInfoEnabled
        {
            get { return _isInfoEnabled; }
            set { _isInfoEnabled = value; }
        }

        private bool _isWarnEnabled=true;
        public bool IsWarnEnabled
        {
            get { return _isWarnEnabled; }
            set { _isWarnEnabled = value; }
        }

        private bool _isErrorEnabled=true;
        public bool IsErrorEnabled
        {
            get { return _isErrorEnabled; }
            set { _isErrorEnabled = value; }
        }

        private bool _isFatalEnabled = true;
        public bool IsFatalEnabled
        {
            get { return _isFatalEnabled; }
            set { _isFatalEnabled = value; }
        }
    }
}