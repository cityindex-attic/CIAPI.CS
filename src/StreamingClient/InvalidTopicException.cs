using System;

namespace StreamingClient
{
    public class InvalidTopicException : Exception
    {
        public InvalidTopicException(string message):base(message)
        {
        }
    }
}