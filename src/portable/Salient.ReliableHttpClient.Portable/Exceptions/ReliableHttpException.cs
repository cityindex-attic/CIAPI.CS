using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Text;

namespace Salient.ReliableHttpClient.Exceptions
{
    /// <summary>
    ///     NOTE: this class must be kept serializable by not populating inner exception and keeping all
    ///     properties serializable
    /// </summary>
    [DataContract]
    public class ReliableHttpException : Exception
    {
        public ReliableHttpException(string message)
            : base(message)
        {
        }

        public ReliableHttpException(string message, Exception ex)
            : base(message)
        {
            PopulateFrom(ex);
        }

        public ReliableHttpException(Exception ex)
            : base(ex.Message)
        {
            PopulateFrom(ex);
        }

        /// <summary>
        ///     basically a clone constructor
        /// </summary>
        public ReliableHttpException(string message, ReliableHttpException exception)
            : base(message)
        {
            InnerExceptionType = exception.InnerExceptionType;
            InnerStackTrace = exception.InnerStackTrace;
        }

        /// <summary>
        ///     basically a clone constructor
        /// </summary>
        public ReliableHttpException(ReliableHttpException exception)
            : base(exception.Message)
        {
            InnerExceptionType = exception.InnerExceptionType;
            InnerStackTrace = exception.InnerStackTrace;
        }

        [DataMember]
        public RequestData RequestData { get; set; }

        [DataMember]
        public string InnerExceptionType { get; set; }

        [DataMember]
        public string InnerStackTrace { get; set; }

        public static ReliableHttpException Create(string message, Exception exception)
        {
            ReliableHttpException ex;
            ex = exception.Message.Contains("The request was aborted")
                     ? new TimeoutException(message, exception)
                     : new ReliableHttpException(message, exception);

            return ex;
        }


        public static ReliableHttpException Create(Exception exception)
        {
            if (exception is ReliableHttpException)
            {
                return (ReliableHttpException) exception;
            }

            ReliableHttpException ex;
            if (exception.Message.Contains("The request was aborted"))
            {
                ex = new TimeoutException(exception);
            }
            else
            {
                ex = new ReliableHttpException(exception);
            }

            return ex;
        }

        private void PopulateFrom(Exception ex)
        {
            RequestData = new RequestData();
            InnerExceptionType = ex.GetType().FullName;
            InnerStackTrace = ex.ToString();
            if (ex is WebException)
            {
                var wex = (WebException) ex;
                if (wex.Response != null)
                {
                    using (Stream stream = wex.Response.GetResponseStream())
                    {
                        if (stream != null)
                        {
                            using (var reader = new StreamReader(stream))
                            {
                                string response = reader.ReadToEnd().Trim();
                                RequestData.Response.Content.Text = response;
                            }
                        }
                    }
                }
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine(Message);
            sb.AppendLine(InnerExceptionType);
            sb.AppendLine(StackTrace);
            sb.AppendLine(InnerStackTrace);
            return sb.ToString();
        }
    }
}