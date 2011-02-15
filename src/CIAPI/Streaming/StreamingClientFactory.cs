using System;
using CIAPI.Streaming.Lightstreamer;
using StreamingClient;

namespace CIAPI.Streaming
{
    public static class StreamingClientFactory
    {
        public static IStreamingClient CreateStreamingClient(Uri uri, string userName, string sessionId)
        {
            return new LightstreamerClient(uri, userName, sessionId);
//            return new CIAPI.Streaming.Websocket.StompOverWebsocketClient(uri, userName, sessionId);
        }
    }
}
