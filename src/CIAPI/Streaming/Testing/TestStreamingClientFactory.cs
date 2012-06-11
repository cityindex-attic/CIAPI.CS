using System;
using Salient.ReliableHttpClient.Serialization;

namespace CIAPI.Streaming.Testing
{
    public class TestStreamingClientFactory : IStreamingClientFactory
    {
        public IStreamingClient Create(Uri streamingUri, string userName, string session, IJsonSerializer serializer)
        {

            return new TestStreamingClient(streamingUri, userName, session, serializer);
        }
    }
}