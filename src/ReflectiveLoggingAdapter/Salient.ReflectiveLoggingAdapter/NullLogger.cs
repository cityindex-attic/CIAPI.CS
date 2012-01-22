using System;

namespace CityIndex.ReflectiveLoggingAdapter
{
    public class LoggerEventArgs : EventArgs
    {
        public LoggerEventArgs(string message)
        {
            Message = message;
        }

        public string Message { get; set; }
    }

    public class TestLogger
    {
        public event EventHandler<LoggerEventArgs> MethodCalled;

        private void OnMethodCalled(string message)
        {
            var e = new LoggerEventArgs(message);
            EventHandler<LoggerEventArgs> handler = MethodCalled;
            if (handler != null) handler(this, e);
        }

        public virtual void Debug(object message)
        {
            System.Diagnostics.Debug.WriteLine(message);
            OnMethodCalled("Debug(object message)");
        }

        public virtual void Debug(object message, Exception exception)
        {
            OnMethodCalled("Debug(object message, Exception exception)");
        }

        public virtual void DebugFormat(string format, params object[] args)
        {
            OnMethodCalled("DebugFormat(string format, params object[] args)");
        }


        public virtual void DebugFormat(string format, Exception exception, params object[] args)
        {
            OnMethodCalled("DebugFormat(string format, Exception exception, params object[] args)");
        }


        public virtual void Error(object message)
        {
            OnMethodCalled("Error(object message)");
        }

        public virtual void Error(object message, Exception exception)
        {
            OnMethodCalled("Error(object message, Exception exception)");
        }

        public virtual void ErrorFormat(string format, params object[] args)
        {
            OnMethodCalled("ErrorFormat(string format, params object[] args)");
        }


        public virtual void ErrorFormat(string format, Exception exception, params object[] args)
        {
            OnMethodCalled("ErrorFormat(string format, Exception exception, params object[] args)");
        }


        public virtual void Fatal(object message)
        {
            OnMethodCalled("Fatal(object message)");
        }

        public virtual void Fatal(object message, Exception exception)
        {
            OnMethodCalled("Fatal(object message, Exception exception)");
        }

        public virtual void FatalFormat(string format, params object[] args)
        {
            OnMethodCalled("FatalFormat(string format, params object[] args)");
        }


        public virtual void FatalFormat(string format, Exception exception, params object[] args)
        {
            OnMethodCalled("FatalFormat(string format, Exception exception, params object[] args)");
        }


        public virtual void Info(object message)
        {
            OnMethodCalled("Info(object message)");
        }

        public virtual void Info(object message, Exception exception)
        {
            OnMethodCalled("Info(object message, Exception exception)");
        }

        public virtual void InfoFormat(string format, params object[] args)
        {
            OnMethodCalled("InfoFormat(string format, params object[] args)");
        }


        public virtual void InfoFormat(string format, Exception exception, params object[] args)
        {
            OnMethodCalled("InfoFormat(string format, Exception exception, params object[] args)");
        }


        public virtual void Trace(object message)
        {
            OnMethodCalled("Trace(object message)");
        }

        public virtual void Trace(object message, Exception exception)
        {
            OnMethodCalled("Trace(object message, Exception exception)");
        }

        public virtual void TraceFormat(string format, params object[] args)
        {
            OnMethodCalled("TraceFormat(string format, params object[] args)");
        }


        public virtual void TraceFormat(string format, Exception exception, params object[] args)
        {
            OnMethodCalled("TraceFormat(string format, Exception exception, params object[] args)");
        }

        

        public virtual void Warn(object message)
        {
            OnMethodCalled("Warn(object message)");
        }

        public virtual void Warn(object message, Exception exception)
        {
            OnMethodCalled("Warn(object message, Exception exception)");
        }

        public virtual void WarnFormat(string format, params object[] args)
        {
            OnMethodCalled("WarnFormat(string format, params object[] args)");
        }

        

        public virtual void WarnFormat(string format, Exception exception, params object[] args)
        {
            OnMethodCalled("WarnFormat(string format, Exception exception, params object[] args)");
        }


    }
}