using System;
using Salient.ReliableHttpClient.Serialization;
namespace CIAPI.Streaming.Testing
{
}
namespace CIAPI.Streaming
{
    /// <summary>
    /// 
    /// </summary>
    public class LightStreamerStreamingClientFactory : IStreamingClientFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="streamingUri"></param>
        /// <param name="userName"></param>
        /// <param name="session"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public IStreamingClient Create(Uri streamingUri, string userName, string session, IJsonSerializer serializer)
        {
            return new LightstreamerClient(streamingUri, userName, session, false, serializer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="streamingUri"></param>
        /// <param name="userName"></param>
        /// <param name="session"></param>
        /// <param name="usePolling"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public IStreamingClient Create(Uri streamingUri, string userName, string session, bool usePolling, IJsonSerializer serializer)
        {
            return new LightstreamerClient(streamingUri, userName, session, usePolling, serializer);
        }
    }
}