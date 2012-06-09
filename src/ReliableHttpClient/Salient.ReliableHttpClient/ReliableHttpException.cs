using System;
using System.IO;
using System.Net;
using System.Text;
using Salient.ReliableHttpClient.Serialization;

namespace Salient.ReliableHttpClient
{
    /// <summary>
    /// NOTE: this class must be kept serializable by not populating inner exception and keeping all
    /// properties serializable
    /// </summary>
    [Serializable]
    public class ReliableHttpException : Exception
    {


        public static ReliableHttpException Create(string message, Exception exception)
        {


            ReliableHttpException ex = new ReliableHttpException(message, exception);

            return ex;
        }


        public static ReliableHttpException Create(Exception exception)
        {

            if (exception is ReliableHttpException)
            {
                return (ReliableHttpException)exception;
            }

            //
            ReliableHttpException ex = new ReliableHttpException(exception);

            return ex;
        }

        public Object Response { get; set; }

        public int ErrorCode { get; set; }
        public int HttpStatus { get; set; }
        public string ResponseText { get; set; }
        public string InnerExceptionType { get; set; }
        public string InnerStackTrace { get; set; }
        public ReliableHttpException(string message)
            : base(message)
        {

        }
        public ReliableHttpException(string message, Exception ex)
            : base(message)
        {
            PopulateFrom(ex);
        }
        private void PopulateFrom(Exception ex)
        {
            InnerExceptionType = ex.GetType().FullName;
            InnerStackTrace = ex.StackTrace;
            if (ex is WebException)
            {
                var wex = (WebException)ex;
                if (wex.Response != null)
                {
                    using (Stream stream = wex.Response.GetResponseStream())
                    {
                        if (stream != null)
                        {
                            using (var reader = new StreamReader(stream))
                            {
                                string response = reader.ReadToEnd().Trim();
                                ResponseText = response;
                            }    
                        }
                        
                    }
                }
            }
        }
        public ReliableHttpException(Exception ex)
            : base(ex.Message)
        {
            PopulateFrom(ex);
        }
        /// <summary>
        /// basically a clone constructor
        /// </summary>
        /// <param name="ex"></param>
        public ReliableHttpException(string message, ReliableHttpException exception)
            : base(message)
        {
            InnerExceptionType = exception.InnerExceptionType;
            InnerStackTrace = exception.InnerStackTrace;

        }

        /// <summary>
        /// basically a clone constructor
        /// </summary>
        /// <param name="ex"></param>
        public ReliableHttpException(ReliableHttpException exception)
            : base(exception.Message)
        {
            InnerExceptionType = exception.InnerExceptionType;
            InnerStackTrace = exception.InnerStackTrace;

        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine(Message);
            sb.AppendLine(InnerExceptionType);
            sb.AppendLine(InnerStackTrace);
            return sb.ToString();
        }
    }
}