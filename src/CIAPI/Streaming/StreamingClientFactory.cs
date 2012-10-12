using System;
using CIAPI.Serialization;
using Salient.ReliableHttpClient.Serialization;
using CIAPI.StreamingClient;


namespace CIAPI.Streaming
{
    public static class StreamingClientFactory
    {

        private static IJsonSerializer _serializer = new Serializer();

        public static void SetSerializer(IJsonSerializer serializer)
        {
            _serializer = serializer;

        }


        public static IStreamingClient CreateStreamingClient(Uri uri, string userName, string sessionId, bool usePolling)
        {
            if (_serializer == null)
            {
                throw new Exception("Serializer must be set before calling CreateStreamingClient");
            }
            return new LightstreamerClient(uri, userName, sessionId, usePolling, _serializer);

        }
        public static IStreamingClient CreateStreamingClient(Uri uri, string userName, string sessionId)
        {
            return CreateStreamingClient(uri, userName, sessionId, false);

        }
    }
}
