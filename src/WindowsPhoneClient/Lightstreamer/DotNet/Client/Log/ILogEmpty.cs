namespace Lightstreamer.DotNet.Client.Log
{
    using System;

    internal class ILogEmpty : ILogger
    {
        public void Debug(string p)
        {
        }

        public void Debug(string p, Exception e)
        {
        }

        public void Error(string p)
        {
        }

        public void Error(string p, Exception e)
        {
        }

        public void Fatal(string p)
        {
        }

        public void Fatal(string p, Exception e)
        {
        }

        public void Info(string p)
        {
        }

        public void Info(string p, Exception e)
        {
        }

        public void Warn(string p)
        {
        }

        public void Warn(string p, Exception e)
        {
        }

        public bool IsDebugEnabled
        {
            get
            {
                return false;
            }
        }

        public bool IsErrorEnabled
        {
            get
            {
                return false;
            }
        }

        public bool IsFatalEnabled
        {
            get
            {
                return false;
            }
        }

        public bool IsInfoEnabled
        {
            get
            {
                return false;
            }
        }

        public bool IsWarnEnabled
        {
            get
            {
                return false;
            }
        }
    }
}

