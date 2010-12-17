using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace CIAPI.Core
{
    public class ApiException : Exception
    {
        public ApiException()
        {
        }
        public ApiException(Exception inner)
        {

        }
        public ApiException(WebException inner)
        {
            // TODO: get the response stream and try to get the errorcode and errormessage
        }
    }
}
