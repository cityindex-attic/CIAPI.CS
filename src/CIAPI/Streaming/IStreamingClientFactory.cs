using System;
using Salient.ReliableHttpClient.Serialization;

namespace CIAPI.Streaming
{
    /// <summary>
    /// 
    /// </summary>
    public interface IStreamingClientFactory
    {
        //#TODO: need a shutdown method that closes all listeners on RPC client logoff/dispose
        /// <summary>
        /// 
        /// </summary>
        /// <param name="streamingUri"></param>
        /// <param name="userName"></param>
        /// <param name="session"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        IStreamingClient Create(Uri streamingUri, string userName, string session, IJsonSerializer serializer);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="streamingUri"></param>
        /// <param name="userName"></param>
        /// <param name="session"></param>
        /// <param name="usePolling"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        IStreamingClient Create(Uri streamingUri, string userName, string session, bool usePolling, IJsonSerializer serializer);


    }
}