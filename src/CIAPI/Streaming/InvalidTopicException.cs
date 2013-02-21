using System;
#if SILVERLIGHT
using Salient.ReliableHttpClient;
#endif

namespace CIAPI.StreamingClient
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class InvalidTopicException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public InvalidTopicException(string message):base(message)
        {
        }
    }
}