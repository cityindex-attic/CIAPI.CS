using System;
using Lightstreamer.DotNet.Client.Log;

namespace StreamingClient.Lightstreamer
{
    internal class LSLogger : ILogger
    {
        public void Error(string line)
        {
            System.Diagnostics.Debug.WriteLine("ERROR: " + line);
            
        }

        public void Error(string line, Exception exception)
        {
            System.Diagnostics.Debug.WriteLine("ERROR: " + line);
            System.Diagnostics.Debug.WriteLine(exception);
        }

        public void Warn(string line)
        {
            System.Diagnostics.Debug.WriteLine("WARN: " + line);
        }

        public void Warn(string line, Exception exception)
        {
            System.Diagnostics.Debug.WriteLine("WARN: " + line);
            System.Diagnostics.Debug.WriteLine(exception);
        }

        public void Info(string line)
        {
            System.Diagnostics.Debug.WriteLine("INFO: " + line);
        }

        public void Info(string line, Exception exception)
        {
            System.Diagnostics.Debug.WriteLine("INFO: " + line);
            System.Diagnostics.Debug.WriteLine(exception);
        }

        public void Debug(string line)
        {
            System.Diagnostics.Debug.WriteLine("DEBUG: " + line);
        }

        public void Debug(string line, Exception exception)
        {
            System.Diagnostics.Debug.WriteLine("DEBUG: " + line);
            System.Diagnostics.Debug.WriteLine("DEBUG: " + exception);
        }

        public void Fatal(string line)
        {
            System.Diagnostics.Debug.WriteLine("FATAL: " + line);
        }

        public void Fatal(string line, Exception exception)
        {
            System.Diagnostics.Debug.WriteLine("FATAL: " + line);
            System.Diagnostics.Debug.WriteLine(exception);
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