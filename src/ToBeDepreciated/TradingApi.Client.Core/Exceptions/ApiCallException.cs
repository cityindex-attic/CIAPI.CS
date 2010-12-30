using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace TradingApi.Client.Core.Exceptions
{
    public class ApiCallException : Exception
    {
        public HttpStatusCode HttpStatusCode;

        public ApiCallException(string message, HttpStatusCode httpStatusCode) : base(message)
        {
            HttpStatusCode = httpStatusCode;
        }
    }
}
