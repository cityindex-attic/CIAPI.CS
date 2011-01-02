using System;
using System.IO;
using System.Net;

namespace CityIndex.JsonClient
{
    public class ApiException : Exception
    {
        private readonly string _message;

        public ApiException()
        {
        }

        public ApiException(Exception inner)
            : base(inner.Message, inner)
        {
        }

        public ApiException(string message, WebException inner)
            : base(message, inner)
        {
            try
            {
                using (Stream stream = inner.Response.GetResponseStream())
                {
                    if (stream != null)
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            ResponseText = reader.ReadToEnd();

                        }
                    }
                }
            }
            catch
            {
                ResponseText = "Could not parse exception response";
            }
        }

        public ApiException(string message)
            : base(message)
        {
        }

        public ApiException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public string ResponseText { get; private set; }

        public override string Message
        {
            get { return _message ?? base.Message; }
        }
    }
}