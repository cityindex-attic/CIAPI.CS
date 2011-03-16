using System;

namespace StreamingClient
{
    public class InvalidTopicException : ApplicationException
    {
        public InvalidTopicException(string message):base(message)
        {
        }
    }
}