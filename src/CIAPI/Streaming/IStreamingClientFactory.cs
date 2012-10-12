using System;
using Salient.ReliableHttpClient.Serialization;

namespace CIAPI.Streaming
{
    public interface IStreamingClientFactory
    {
        //#TODO: need a shutdown method that closes all listeners on RPC client logoff/dispose
        IStreamingClient Create(Uri streamingUri, string userName, string session, IJsonSerializer serializer);
        IStreamingClient Create(Uri streamingUri, string userName, string session, bool usePolling, IJsonSerializer serializer);


    }
}