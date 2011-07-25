using System;
using System.Text.RegularExpressions;
using CIAPI.DTO;
using StreamingClient;
using System.Linq;
namespace CIAPI.Streaming.Lightstreamer
{
    public partial class LightstreamerClient : StreamingClient.Lightstreamer.LightstreamerClient, IStreamingClient
    {
        public LightstreamerClient(Uri streamingUri, string userName, string sessionId)
            : base(streamingUri, userName, sessionId)
        {
        }
    }
}