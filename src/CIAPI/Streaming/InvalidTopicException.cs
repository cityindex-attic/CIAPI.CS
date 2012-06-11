using System;
#if SILVERLIGHT
using Salient.ReliableHttpClient;
#endif

namespace CIAPI.StreamingClient
{
    [Serializable]
    public class InvalidTopicException : Exception
    {
        public InvalidTopicException(string message):base(message)
        {
        }
    }
}