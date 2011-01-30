using System;
using System.Globalization;
using System.Text;

namespace Common.Logging
{
    public abstract class AbstractSimpleLogger : AbstractLogger
    {
        // Fields
        private readonly string _dateTimeFormat;
        private readonly bool _hasDateTimeFormat;
        private readonly string _name;
        private readonly bool _showDateTime;
        private readonly bool _showLevel;
        private readonly bool _showLogName;
        private LogLevel _currentLogLevel;

        // Methods
        public AbstractSimpleLogger(string logName, LogLevel logLevel, bool showlevel, bool showDateTime,
                                    bool showLogName, string dateTimeFormat)
        {
            _name = logName;
            _currentLogLevel = logLevel;
            _showLevel = showlevel;
            _showDateTime = showDateTime;
            _showLogName = showLogName;
            _dateTimeFormat = dateTimeFormat;
            _hasDateTimeFormat = !string.IsNullOrEmpty(_dateTimeFormat);
        }

        // Properties

        public LogLevel CurrentLogLevel
        {
            get { return _currentLogLevel; }
            set { _currentLogLevel = value; }
        }


        public string DateTimeFormat
        {
            get { return _dateTimeFormat; }
        }


        public bool HasDateTimeFormat
        {
            get { return _hasDateTimeFormat; }
        }

        public override bool IsDebugEnabled
        {
            get { return IsLevelEnabled(LogLevel.Debug); }
        }

        public override bool IsErrorEnabled
        {
            get { return IsLevelEnabled(LogLevel.Error); }
        }

        public override bool IsFatalEnabled
        {
            get { return IsLevelEnabled(LogLevel.Fatal); }
        }

        public override bool IsInfoEnabled
        {
            get { return IsLevelEnabled(LogLevel.Info); }
        }

        public override bool IsTraceEnabled
        {
            get { return IsLevelEnabled(LogLevel.Trace); }
        }

        public override bool IsWarnEnabled
        {
            get { return IsLevelEnabled(LogLevel.Warn); }
        }


        public string Name
        {
            get { return _name; }
        }


        public bool ShowDateTime
        {
            get { return _showDateTime; }
        }


        public bool ShowLevel
        {
            get { return _showLevel; }
        }


        public bool ShowLogName
        {
            get { return _showLogName; }
        }

        protected virtual void FormatOutput(StringBuilder stringBuilder, LogLevel level, object message, Exception e)
        {
            if (stringBuilder == null)
            {
                throw new ArgumentNullException("stringBuilder");
            }
            if (_showDateTime)
            {
                if (_hasDateTimeFormat)
                {
                    stringBuilder.Append(DateTime.Now.ToString(_dateTimeFormat, CultureInfo.InvariantCulture));
                }
                else
                {
                    stringBuilder.Append(DateTime.Now);
                }
                stringBuilder.Append(" ");
            }
            if (_showLevel)
            {
                stringBuilder.Append(("[" + level.ToString().ToUpper() + "]").PadRight(8));
            }
            if (_showLogName)
            {
                stringBuilder.Append(_name).Append(" - ");
            }
            stringBuilder.Append(message);
            if (e != null)
            {
                stringBuilder.Append(Environment.NewLine).Append(e.ToString());
            }
        }

        protected virtual bool IsLevelEnabled(LogLevel level)
        {
            var num = (int) level;
            var num2 = (int) _currentLogLevel;
            return (num >= num2);
        }
    }
}