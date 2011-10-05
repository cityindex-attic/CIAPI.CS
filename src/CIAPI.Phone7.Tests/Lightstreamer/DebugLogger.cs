using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Lightstreamer.DotNet.Client.Log;

namespace CIAPI.Phone7.Tests.Lightstreamer
{
    public class DebugLogger : ILogger
    {

        public void Debug(string line, Exception exception)
        {
            System.Diagnostics.Debug.WriteLine(line + exception);
        }

        public void Debug(string line)
        {
            System.Diagnostics.Debug.WriteLine(line);
        }

        public void Error(string line, Exception exception)
        {
            System.Diagnostics.Debug.WriteLine(line + exception);
        }

        public void Error(string line)
        {
            System.Diagnostics.Debug.WriteLine(line);
        }

        public void Fatal(string line, Exception exception)
        {
            System.Diagnostics.Debug.WriteLine(line + exception);
        }

        public void Fatal(string line)
        {
            System.Diagnostics.Debug.WriteLine(line);
        }

        public void Info(string line, Exception exception)
        {
            System.Diagnostics.Debug.WriteLine(line + exception);
        }

        public void Info(string line)
        {
            System.Diagnostics.Debug.WriteLine(line);
        }

        public bool IsDebugEnabled
        {
            get { return true; }
        }

        public bool IsErrorEnabled
        {
            get { return true; }
        }

        public bool IsFatalEnabled
        {
            get { return true; }
        }

        public bool IsInfoEnabled
        {
            get { return true; }
        }

        public bool IsWarnEnabled
        {
            get { return true; }
        }

        public void Warn(string line, Exception exception)
        {
            System.Diagnostics.Debug.WriteLine(line + exception);
        }

        public void Warn(string line)
        {
            System.Diagnostics.Debug.WriteLine(line);
        }
    }
}
