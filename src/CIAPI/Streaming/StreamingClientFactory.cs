using System;
using CIAPI.Streaming.Lightstreamer;


namespace CIAPI.Streaming
{
    public static class StreamingClientFactory
    {
        public static IStreamingClient CreateStreamingClient(Uri uri, string userName, string sessionId)
        {
            return new LightstreamerClient(uri, userName, sessionId);
//            return new WebSocketClient(uri, userName, sessionId);
        }
    }
}
