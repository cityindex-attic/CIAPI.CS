using System;
using CIAPI.Serialization;
using Salient.ReliableHttpClient.Serialization;
using StreamingClient;


namespace CIAPI.Streaming
{
    public static class StreamingClientFactory
    {

        private static IJsonSerializer _serializer = new Serializer();

        public static void SetSerializer(IJsonSerializer serializer)
        {
            _serializer = serializer;
        }

        public static IStreamingClient CreateStreamingClient(Uri uri, string userName, string sessionId)
        {
            if (_serializer == null)
            {
                throw new Exception("Serializer must be set before calling CreateStreamingClient");
            }
            return new LightstreamerClient(uri, userName, sessionId, _serializer);

        }
    }
}
