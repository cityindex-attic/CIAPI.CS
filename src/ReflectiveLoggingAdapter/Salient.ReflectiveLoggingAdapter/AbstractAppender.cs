using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace CityIndex.ReflectiveLoggingAdapter
{
    public class CapturingAppender : AbstractAppender
    {
        public class LogItem
        {
            public LogLevel Level { get; set; }
            public string Message { get; set; }
            public Exception Exception { get; set; }
        }
        public CapturingAppender(string logName, LogLevel logLevel, bool showLevel, bool showDateTime, bool showLogName, string dateTimeFormat)
            : base(logName, logLevel, showLevel, showDateTime, showLogName, dateTimeFormat)
        {
            _items = new List<LogItem>();
        }

        private readonly List<LogItem> _items;
        public IEnumerable<LogItem> GetItems()
        {
            lock (_items)
            {
                LogItem[] items = _items.ToArray();
                _items.Clear();
                return items;
            }

        }



        protected override void WriteInternal(LogLevel level, object message, Exception exception)
        {
            _items.Add(new LogItem() { Exception = exception, Level = level, Message = message.ToString() });
        }
    }
    public abstract class AbstractAppender : ILog
    {
        protected AbstractAppender(string logName, LogLevel logLevel, bool showLevel, bool showDateTime, bool showLogName,
                                string dateTimeFormat)
        {
            LogName = logName;
            Level = logLevel;
            ShowLevel = showLevel;
            ShowDateTime = showDateTime;
            ShowLogName = showLogName;
            DateTimeFormat = dateTimeFormat;
            HasDateTimeFormat = !string.IsNullOrEmpty(dateTimeFormat);
        }

        #region Properties

        public bool IsTraceEnabled
        {
            get { return IsLevelEnabled(LogLevel.Trace); }
        }

        public bool IsDebugEnabled
        {
            get { return IsLevelEnabled(LogLevel.Debug); }
        }

        public bool IsErrorEnabled
        {
            get { return IsLevelEnabled(LogLevel.Error); }
        }

        public bool IsFatalEnabled
        {
            get { return IsLevelEnabled(LogLevel.Fatal); }
        }

        public bool IsInfoEnabled
        {
            get { return IsLevelEnabled(LogLevel.Info); }
        }

        public bool IsWarnEnabled
        {
            get { return IsLevelEnabled(LogLevel.Warn); }
        }

        #endregion
        protected virtual bool IsLevelEnabled(LogLevel level)
        {
            int iLevel = (int)level;
            int iCurrentLogLevel = (int)Level;

            // return iLevel.CompareTo(iCurrentLogLevel); better ???
            return (iLevel >= iCurrentLogLevel);
        }
        public bool HasDateTimeFormat = false;
        public bool ShowLevel { get; set; }
        public bool ShowDateTime { get; set; }
        public bool ShowLogName { get; set; }
        public string DateTimeFormat { get; set; }
        public string LogName { get; set; }
        public LogLevel Level { get; set; }

        protected abstract void WriteInternal(LogLevel level, object message, Exception exception);

        protected virtual void FormatOutput(StringBuilder stringBuilder, LogLevel level, object message, Exception e)
        {
            if (stringBuilder == null)
            {
                throw new ArgumentNullException("stringBuilder");
            }

            // Append date-time if so configured
            if (ShowDateTime)
            {
                if (HasDateTimeFormat)
                {
                    stringBuilder.Append(DateTime.Now.ToString(DateTimeFormat, CultureInfo.InvariantCulture));
                }
                else
                {
                    stringBuilder.Append(DateTime.Now);
                }

                stringBuilder.Append(" ");
            }

            if (ShowLevel)
            {
                // Append a readable representation of the log level
                stringBuilder.Append(("[" + level.ToString().ToUpper() + "]").PadRight(8));
            }

            // Append the name of the log instance if so configured
            if (ShowLogName)
            {
                stringBuilder.Append(LogName).Append(" - ");
            }

            // Append the message
            stringBuilder.Append(message);

            // Append stack trace if not null
            if (e != null)
            {
                stringBuilder.Append(Environment.NewLine).Append(e.ToString());
            }
        }
        #region ILog Members

        public void Debug(object message)
        {
            if (IsDebugEnabled)
            {
                WriteInternal(LogLevel.Debug, message, null);
            }

        }

        public void Debug(object message, Exception exception)
        {
            if (IsDebugEnabled)
            {
                WriteInternal(LogLevel.Debug, message, exception);
            }

        }

        public void DebugFormat(string format, params object[] args)
        {
            if (IsDebugEnabled)
            {
                WriteInternal(LogLevel.Debug, string.Format(CultureInfo.InvariantCulture, format, args), null);
            }

        }


        public void DebugFormat(string format, Exception exception, params object[] args)
        {
            if (IsDebugEnabled)
            {
                WriteInternal(LogLevel.Debug, string.Format(CultureInfo.InvariantCulture, format, args), exception);
            }

        }


        public void Error(object message)
        {
            if (IsErrorEnabled)
            {
                WriteInternal(LogLevel.Error, message, null);
            }

        }

        public void Error(object message, Exception exception)
        {
            if (IsErrorEnabled)
            {
                WriteInternal(LogLevel.Error, message, exception);
            }

        }

        public void ErrorFormat(string format, params object[] args)
        {
            if (IsErrorEnabled)
            {
                WriteInternal(LogLevel.Error, string.Format(CultureInfo.InvariantCulture, format, args), null);
            }

        }


        public void ErrorFormat(string format, Exception exception, params object[] args)
        {
            if (IsErrorEnabled)
            {
                WriteInternal(LogLevel.Error, string.Format(CultureInfo.InvariantCulture, format, args), exception);
            }


        }


        public void Fatal(object message)
        {
            if (IsFatalEnabled)
            {
                WriteInternal(LogLevel.Fatal, message, null);
            }

        }

        public void Fatal(object message, Exception exception)
        {
            if (IsFatalEnabled)
            {
                WriteInternal(LogLevel.Fatal, message, exception);
            }


        }

        public void FatalFormat(string format, params object[] args)
        {
            if (IsFatalEnabled)
            {
                WriteInternal(LogLevel.Fatal, string.Format(CultureInfo.InvariantCulture, format, args), null);
            }


        }


        public void FatalFormat(string format, Exception exception, params object[] args)
        {
            if (IsFatalEnabled)
            {
                WriteInternal(LogLevel.Fatal, string.Format(CultureInfo.InvariantCulture, format, args), exception);
            }


        }


        public void Info(object message)
        {
            if (IsInfoEnabled)
            {
                WriteInternal(LogLevel.Info, message, null);
            }


        }

        public void Info(object message, Exception exception)
        {
            if (IsInfoEnabled)
            {
                WriteInternal(LogLevel.Info, message, exception);
            }


        }

        public void InfoFormat(string format, params object[] args)
        {
            if (IsInfoEnabled)
            {
                WriteInternal(LogLevel.Info, string.Format(CultureInfo.InvariantCulture, format, args), null);
            }


        }


        public void InfoFormat(string format, Exception exception, params object[] args)
        {
            if (IsInfoEnabled)
            {
                WriteInternal(LogLevel.Info, string.Format(CultureInfo.InvariantCulture, format, args), exception);
            }


        }


        public void Trace(object message)
        {
            if (IsTraceEnabled)
            {
                WriteInternal(LogLevel.Trace, message, null);
            }


        }

        public void Trace(object message, Exception exception)
        {
            if (IsTraceEnabled)
            {
                WriteInternal(LogLevel.Trace, message, exception);
            }


        }

        public void TraceFormat(string format, params object[] args)
        {
            if (IsTraceEnabled)
            {
                WriteInternal(LogLevel.Trace, string.Format(CultureInfo.InvariantCulture, format, args), null);
            }


        }


        public void TraceFormat(string format, Exception exception, params object[] args)
        {
            if (IsTraceEnabled)
            {
                WriteInternal(LogLevel.Trace, string.Format(CultureInfo.InvariantCulture, format, args), exception);
            }


        }


        public void Warn(object message)
        {
            if (IsWarnEnabled)
            {
                WriteInternal(LogLevel.Warn, message, null);
            }


        }

        public void Warn(object message, Exception exception)
        {
            if (IsWarnEnabled)
            {
                WriteInternal(LogLevel.Warn, message, exception);
            }


        }

        public void WarnFormat(string format, params object[] args)
        {
            if (IsWarnEnabled)
            {
                WriteInternal(LogLevel.Warn, string.Format(CultureInfo.InvariantCulture, format, args), null);
            }


        }


        public void WarnFormat(string format, Exception exception, params object[] args)
        {
            if (IsWarnEnabled)
            {
                WriteInternal(LogLevel.Warn, string.Format(CultureInfo.InvariantCulture, format, args), exception);
            }


        }

        #endregion
    }
}