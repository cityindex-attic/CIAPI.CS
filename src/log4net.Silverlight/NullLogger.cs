using System;

namespace log4net
{
    public class NullLogger : ILog
    {
        public void Debug(string s)
        {
            // noop
        }

        public void Debug(object message)
        {
            // noop
        }

        public void Debug(object message, Exception exception)
        {
            // noop
        }

        public void DebugFormat(string format, params object[] args)
        {
            // noop
        }

        public void DebugFormat(string format, object arg0)
        {
            // noop
        }

        public void DebugFormat(IFormatProvider provider, string format, params object[] args)
        {
            // noop
        }

        public void DebugFormat(string format, object arg0, object arg1)
        {
            // noop
        }

        public void DebugFormat(string format, object arg0, object arg1, object arg2)
        {
            // noop
        }

        public void Error(object message)
        {
            // noop
        }

        public void Error(object message, Exception exception)
        {
            // noop
        }

        public void ErrorFormat(string format, object arg0)
        {
            // noop
        }

        public void ErrorFormat(string format, params object[] args)
        {
            // noop
        }

        public void ErrorFormat(IFormatProvider provider, string format, params object[] args)
        {
            // noop
        }

        public void ErrorFormat(string format, object arg0, object arg1)
        {
            // noop
        }

        public void ErrorFormat(string format, object arg0, object arg1, object arg2)
        {
            // noop
        }

        public void Fatal(object message)
        {
            // noop
        }

        public void Fatal(object message, Exception exception)
        {
            // noop
        }

        public void FatalFormat(string format, object arg0)
        {
            // noop
        }

        public void FatalFormat(string format, params object[] args)
        {
            // noop
        }

        public void FatalFormat(IFormatProvider provider, string format, params object[] args)
        {
            // noop
        }

        public void FatalFormat(string format, object arg0, object arg1)
        {
            // noop
        }

        public void FatalFormat(string format, object arg0, object arg1, object arg2)
        {
            // noop
        }

        public void Info(object message)
        {
            // noop
        }

        public void Info(object message, Exception exception)
        {
            // noop
        }

        public void InfoFormat(string format, object arg0)
        {
            // noop
        }

        public void InfoFormat(string format, params object[] args)
        {
            // noop
        }

        public void InfoFormat(IFormatProvider provider, string format, params object[] args)
        {
            // noop
        }

        public void InfoFormat(string format, object arg0, object arg1)
        {
            // noop
        }

        public void InfoFormat(string format, object arg0, object arg1, object arg2)
        {
            // noop
        }

        public void Warn(object message)
        {
            // noop
        }

        public void Warn(object message, Exception exception)
        {
            // noop
        }

        public void WarnFormat(string format, params object[] args)
        {
            // noop
        }

        public void WarnFormat(string format, object arg0)
        {
            // noop
        }

        public void WarnFormat(IFormatProvider provider, string format, params object[] args)
        {
            // noop
        }

        public void WarnFormat(string format, object arg0, object arg1)
        {
            // noop
        }

        public void WarnFormat(string format, object arg0, object arg1, object arg2)
        {
            // noop
        }

        public bool IsDebugEnabled
        {
            get { return false; }
        }

        public bool IsErrorEnabled
        {
            get { return false; }
        }

        public bool IsFatalEnabled
        {
            get { return false; }
        }

        public bool IsInfoEnabled
        {
            get { return false;  }
        }

        public bool IsWarnEnabled
        {
            get { return false; }
        }
    }
}