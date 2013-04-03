using System;
using System.Runtime.Serialization;

namespace Salient.ReliableHttpClient.Exceptions
{
    [DataContract]
    public class TimeoutException : ReliableHttpException
    {
        
        public TimeoutException(string message, Exception ex)
            : base(message, ex)
        {
        }
        public TimeoutException(Exception ex)
            : base(ex)
        {

        }


    }
}