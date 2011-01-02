using System;
using System.IO;
using System.Net;

namespace CityIndex.JsonClient
{
    /// <summary>
    /// An exception class that will retrieve the response text of the inner exception if it is a WebException
    /// </summary>
    public class ApiException : Exception
    {


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

    }
}