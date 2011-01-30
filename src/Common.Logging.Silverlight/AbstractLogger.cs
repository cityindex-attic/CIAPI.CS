using System;

namespace Common.Logging
{
    public abstract class AbstractLogger : ILog
    {
        // Fields
        private readonly WriteHandler Write;

        // Methods
        protected AbstractLogger()
        {
            Write = GetWriteHandler();
            if (Write == null)
            {
                Write = new WriteHandler(WriteInternal);
            }
        }

        #region ILog Members

        public virtual void Debug(Action<FormatMessageHandler> formatMessageCallback)
        {
            if (IsDebugEnabled)
            {
                Write(LogLevel.Debug, new FormatMessageCallbackFormattedMessage(formatMessageCallback), null);
            }
        }

        public virtual void Debug(object message)
        {
            if (IsDebugEnabled)
            {
                Write(LogLevel.Debug, message, null);
            }
        }

        public virtual void Debug(Action<FormatMessageHandler> formatMessageCallback, Exception exception)
        {
            if (IsDebugEnabled)
            {
                Write(LogLevel.Debug, new FormatMessageCallbackFormattedMessage(formatMessageCallback), exception);
            }
        }

        public virtual void Debug(IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback)
        {
            if (IsDebugEnabled)
            {
                Write(LogLevel.Debug, new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback),
                      null);
            }
        }

        public virtual void Debug(object message, Exception exception)
        {
            if (IsDebugEnabled)
            {
                Write(LogLevel.Debug, message, exception);
            }
        }

        public virtual void Debug(IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback,
                                  Exception exception)
        {
            if (IsDebugEnabled)
            {
                Write(LogLevel.Debug, new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback),
                      exception);
            }
        }

        public virtual void DebugFormat(string format, params object[] args)
        {
            if (IsDebugEnabled)
            {
                Write(LogLevel.Debug, new StringFormatFormattedMessage(null, format, args), null);
            }
        }

        public virtual void DebugFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            if (IsDebugEnabled)
            {
                Write(LogLevel.Debug, new StringFormatFormattedMessage(formatProvider, format, args), null);
            }
        }

        public virtual void DebugFormat(string format, Exception exception, params object[] args)
        {
            if (IsDebugEnabled)
            {
                Write(LogLevel.Debug, new StringFormatFormattedMessage(null, format, args), exception);
            }
        }

        public virtual void DebugFormat(IFormatProvider formatProvider, string format, Exception exception,
                                        params object[] args)
        {
            if (IsDebugEnabled)
            {
                Write(LogLevel.Debug, new StringFormatFormattedMessage(formatProvider, format, args), exception);
            }
        }

        public virtual void Error(Action<FormatMessageHandler> formatMessageCallback)
        {
            if (IsErrorEnabled)
            {
                Write(LogLevel.Error, new FormatMessageCallbackFormattedMessage(formatMessageCallback), null);
            }
        }

        public virtual void Error(object message)
        {
            if (IsErrorEnabled)
            {
                Write(LogLevel.Error, message, null);
            }
        }

        public virtual void Error(Action<FormatMessageHandler> formatMessageCallback, Exception exception)
        {
            if (IsErrorEnabled)
            {
                Write(LogLevel.Error, new FormatMessageCallbackFormattedMessage(formatMessageCallback), exception);
            }
        }

        public virtual void Error(IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback)
        {
            if (IsErrorEnabled)
            {
                Write(LogLevel.Error, new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback),
                      null);
            }
        }

        public virtual void Error(object message, Exception exception)
        {
            if (IsErrorEnabled)
            {
                Write(LogLevel.Error, message, exception);
            }
        }

        public virtual void Error(IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback,
                                  Exception exception)
        {
            if (IsErrorEnabled)
            {
                Write(LogLevel.Error, new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback),
                      exception);
            }
        }

        public virtual void ErrorFormat(string format, params object[] args)
        {
            if (IsErrorEnabled)
            {
                Write(LogLevel.Error, new StringFormatFormattedMessage(null, format, args), null);
            }
        }

        public virtual void ErrorFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            if (IsErrorEnabled)
            {
                Write(LogLevel.Error, new StringFormatFormattedMessage(formatProvider, format, args), null);
            }
        }

        public virtual void ErrorFormat(string format, Exception exception, params object[] args)
        {
            if (IsErrorEnabled)
            {
                Write(LogLevel.Error, new StringFormatFormattedMessage(null, format, args), exception);
            }
        }

        public virtual void ErrorFormat(IFormatProvider formatProvider, string format, Exception exception,
                                        params object[] args)
        {
            if (IsErrorEnabled)
            {
                Write(LogLevel.Error, new StringFormatFormattedMessage(formatProvider, format, args), exception);
            }
        }

        public virtual void Fatal(Action<FormatMessageHandler> formatMessageCallback)
        {
            if (IsFatalEnabled)
            {
                Write(LogLevel.Fatal, new FormatMessageCallbackFormattedMessage(formatMessageCallback), null);
            }
        }

        public virtual void Fatal(object message)
        {
            if (IsFatalEnabled)
            {
                Write(LogLevel.Fatal, message, null);
            }
        }

        public virtual void Fatal(Action<FormatMessageHandler> formatMessageCallback, Exception exception)
        {
            if (IsFatalEnabled)
            {
                Write(LogLevel.Fatal, new FormatMessageCallbackFormattedMessage(formatMessageCallback), exception);
            }
        }

        public virtual void Fatal(IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback)
        {
            if (IsFatalEnabled)
            {
                Write(LogLevel.Fatal, new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback),
                      null);
            }
        }

        public virtual void Fatal(object message, Exception exception)
        {
            if (IsFatalEnabled)
            {
                Write(LogLevel.Fatal, message, exception);
            }
        }

        public virtual void Fatal(IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback,
                                  Exception exception)
        {
            if (IsFatalEnabled)
            {
                Write(LogLevel.Fatal, new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback),
                      exception);
            }
        }

        public virtual void FatalFormat(string format, params object[] args)
        {
            if (IsFatalEnabled)
            {
                Write(LogLevel.Fatal, new StringFormatFormattedMessage(null, format, args), null);
            }
        }

        public virtual void FatalFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            if (IsFatalEnabled)
            {
                Write(LogLevel.Fatal, new StringFormatFormattedMessage(formatProvider, format, args), null);
            }
        }

        public virtual void FatalFormat(string format, Exception exception, params object[] args)
        {
            if (IsFatalEnabled)
            {
                Write(LogLevel.Fatal, new StringFormatFormattedMessage(null, format, args), exception);
            }
        }

        public virtual void FatalFormat(IFormatProvider formatProvider, string format, Exception exception,
                                        params object[] args)
        {
            if (IsFatalEnabled)
            {
                Write(LogLevel.Fatal, new StringFormatFormattedMessage(formatProvider, format, args), exception);
            }
        }

        public virtual void Info(Action<FormatMessageHandler> formatMessageCallback)
        {
            if (IsInfoEnabled)
            {
                Write(LogLevel.Info, new FormatMessageCallbackFormattedMessage(formatMessageCallback), null);
            }
        }

        public virtual void Info(object message)
        {
            if (IsInfoEnabled)
            {
                Write(LogLevel.Info, message, null);
            }
        }

        public virtual void Info(Action<FormatMessageHandler> formatMessageCallback, Exception exception)
        {
            if (IsInfoEnabled)
            {
                Write(LogLevel.Info, new FormatMessageCallbackFormattedMessage(formatMessageCallback), exception);
            }
        }

        public virtual void Info(IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback)
        {
            if (IsInfoEnabled)
            {
                Write(LogLevel.Info, new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback),
                      null);
            }
        }

        public virtual void Info(object message, Exception exception)
        {
            if (IsInfoEnabled)
            {
                Write(LogLevel.Info, message, exception);
            }
        }

        public virtual void Info(IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback,
                                 Exception exception)
        {
            if (IsInfoEnabled)
            {
                Write(LogLevel.Info, new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback),
                      exception);
            }
        }

        public virtual void InfoFormat(string format, params object[] args)
        {
            if (IsInfoEnabled)
            {
                Write(LogLevel.Info, new StringFormatFormattedMessage(null, format, args), null);
            }
        }

        public virtual void InfoFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            if (IsInfoEnabled)
            {
                Write(LogLevel.Info, new StringFormatFormattedMessage(formatProvider, format, args), null);
            }
        }

        public virtual void InfoFormat(string format, Exception exception, params object[] args)
        {
            if (IsInfoEnabled)
            {
                Write(LogLevel.Info, new StringFormatFormattedMessage(null, format, args), exception);
            }
        }

        public virtual void InfoFormat(IFormatProvider formatProvider, string format, Exception exception,
                                       params object[] args)
        {
            if (IsInfoEnabled)
            {
                Write(LogLevel.Info, new StringFormatFormattedMessage(formatProvider, format, args), exception);
            }
        }

        public virtual void Trace(Action<FormatMessageHandler> formatMessageCallback)
        {
            if (IsTraceEnabled)
            {
                Write(LogLevel.Trace, new FormatMessageCallbackFormattedMessage(formatMessageCallback), null);
            }
        }

        public virtual void Trace(object message)
        {
            if (IsTraceEnabled)
            {
                Write(LogLevel.Trace, message, null);
            }
        }

        public virtual void Trace(Action<FormatMessageHandler> formatMessageCallback, Exception exception)
        {
            if (IsTraceEnabled)
            {
                Write(LogLevel.Trace, new FormatMessageCallbackFormattedMessage(formatMessageCallback), exception);
            }
        }

        public virtual void Trace(IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback)
        {
            if (IsTraceEnabled)
            {
                Write(LogLevel.Trace, new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback),
                      null);
            }
        }

        public virtual void Trace(object message, Exception exception)
        {
            if (IsTraceEnabled)
            {
                Write(LogLevel.Trace, message, exception);
            }
        }

        public virtual void Trace(IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback,
                                  Exception exception)
        {
            if (IsTraceEnabled)
            {
                Write(LogLevel.Trace, new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback),
                      exception);
            }
        }

        public virtual void TraceFormat(string format, params object[] args)
        {
            if (IsTraceEnabled)
            {
                Write(LogLevel.Trace, new StringFormatFormattedMessage(null, format, args), null);
            }
        }

        public virtual void TraceFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            if (IsTraceEnabled)
            {
                Write(LogLevel.Trace, new StringFormatFormattedMessage(formatProvider, format, args), null);
            }
        }

        public virtual void TraceFormat(string format, Exception exception, params object[] args)
        {
            if (IsTraceEnabled)
            {
                Write(LogLevel.Trace, new StringFormatFormattedMessage(null, format, args), exception);
            }
        }

        public virtual void TraceFormat(IFormatProvider formatProvider, string format, Exception exception,
                                        params object[] args)
        {
            if (IsTraceEnabled)
            {
                Write(LogLevel.Trace, new StringFormatFormattedMessage(formatProvider, format, args), exception);
            }
        }

        public virtual void Warn(Action<FormatMessageHandler> formatMessageCallback)
        {
            if (IsWarnEnabled)
            {
                Write(LogLevel.Warn, new FormatMessageCallbackFormattedMessage(formatMessageCallback), null);
            }
        }

        public virtual void Warn(object message)
        {
            if (IsWarnEnabled)
            {
                Write(LogLevel.Warn, message, null);
            }
        }

        public virtual void Warn(Action<FormatMessageHandler> formatMessageCallback, Exception exception)
        {
            if (IsWarnEnabled)
            {
                Write(LogLevel.Warn, new FormatMessageCallbackFormattedMessage(formatMessageCallback), exception);
            }
        }

        public virtual void Warn(IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback)
        {
            if (IsWarnEnabled)
            {
                Write(LogLevel.Warn, new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback),
                      null);
            }
        }

        public virtual void Warn(object message, Exception exception)
        {
            if (IsWarnEnabled)
            {
                Write(LogLevel.Warn, message, exception);
            }
        }

        public virtual void Warn(IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback,
                                 Exception exception)
        {
            if (IsWarnEnabled)
            {
                Write(LogLevel.Warn, new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback),
                      exception);
            }
        }

        public virtual void WarnFormat(string format, params object[] args)
        {
            if (IsWarnEnabled)
            {
                Write(LogLevel.Warn, new StringFormatFormattedMessage(null, format, args), null);
            }
        }

        public virtual void WarnFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            if (IsWarnEnabled)
            {
                Write(LogLevel.Warn, new StringFormatFormattedMessage(formatProvider, format, args), null);
            }
        }

        public virtual void WarnFormat(string format, Exception exception, params object[] args)
        {
            if (IsWarnEnabled)
            {
                Write(LogLevel.Warn, new StringFormatFormattedMessage(null, format, args), exception);
            }
        }

        public virtual void WarnFormat(IFormatProvider formatProvider, string format, Exception exception,
                                       params object[] args)
        {
            if (IsWarnEnabled)
            {
                Write(LogLevel.Warn, new StringFormatFormattedMessage(formatProvider, format, args), exception);
            }
        }

        // Properties
        public abstract bool IsDebugEnabled { get; }

        public abstract bool IsErrorEnabled { get; }

        public abstract bool IsFatalEnabled { get; }

        public abstract bool IsInfoEnabled { get; }

        public abstract bool IsTraceEnabled { get; }

        public abstract bool IsWarnEnabled { get; }

        #endregion

        protected virtual WriteHandler GetWriteHandler()
        {
            return null;
        }

        protected abstract void WriteInternal(LogLevel level, object message, Exception exception);

        // Nested Types

        #region Nested type: FormatMessageCallbackFormattedMessage

        private class FormatMessageCallbackFormattedMessage
        {
            // Fields
            private readonly Action<FormatMessageHandler> formatMessageCallback;
            private readonly IFormatProvider formatProvider;
            private volatile string cachedMessage;

            // Methods
            public FormatMessageCallbackFormattedMessage(Action<FormatMessageHandler> formatMessageCallback)
            {
                this.formatMessageCallback = formatMessageCallback;
            }

            public FormatMessageCallbackFormattedMessage(IFormatProvider formatProvider,
                                                         Action<FormatMessageHandler> formatMessageCallback)
            {
                this.formatProvider = formatProvider;
                this.formatMessageCallback = formatMessageCallback;
            }

            private string FormatMessage(string format, params object[] args)
            {
                cachedMessage = string.Format(formatProvider, format, args);
                return cachedMessage;
            }

            public override string ToString()
            {
                if ((cachedMessage == null) && (formatMessageCallback != null))
                {
                    formatMessageCallback(FormatMessage);
                }
                return cachedMessage;
            }
        }

        #endregion

        #region Nested type: StringFormatFormattedMessage

        private class StringFormatFormattedMessage
        {
            // Fields
            private readonly object[] Args;
            private readonly IFormatProvider FormatProvider;
            private readonly string Message;
            private volatile string cachedMessage;

            // Methods
            public StringFormatFormattedMessage(IFormatProvider formatProvider, string message, params object[] args)
            {
                FormatProvider = formatProvider;
                Message = message;
                Args = args;
            }

            public override string ToString()
            {
                if (cachedMessage == null)
                {
                    cachedMessage = string.Format(FormatProvider, Message, Args);
                }
                return cachedMessage;
            }
        }

        #endregion

        #region Nested type: WriteHandler

        protected delegate void WriteHandler(LogLevel level, object message, Exception exception);

        #endregion
    }
}