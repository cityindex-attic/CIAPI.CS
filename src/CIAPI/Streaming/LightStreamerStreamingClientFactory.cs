using System;
using Salient.ReliableHttpClient.Serialization;
namespace CIAPI.Streaming.Testing
{
}
namespace CIAPI.Streaming
{
    public class LightStreamerStreamingClientFactory : IStreamingClientFactory
    {
        public IStreamingClient Create(Uri streamingUri, string userName, string session, IJsonSerializer serializer)
        {
            return new LightstreamerClient(streamingUri, userName, session, serializer);
        }
    }
}