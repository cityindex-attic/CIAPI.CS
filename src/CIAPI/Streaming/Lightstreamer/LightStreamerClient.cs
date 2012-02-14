using System;
using System.Text.RegularExpressions;
using CIAPI.DTO;
using Salient.ReflectiveLoggingAdapter;
using StreamingClient;
using System.Linq;
namespace CIAPI.Streaming.Lightstreamer
{
    public partial class LightstreamerClient : StreamingClient.Lightstreamer.LightstreamerClient, IStreamingClient
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(LightstreamerClient));

        public LightstreamerClient(Uri streamingUri, string userName, string sessionId)
            : base(streamingUri, userName, sessionId)
        {
            Log.Debug("LightstreamerClient created for " + string.Format("{1} {2} {0}", streamingUri, userName, sessionId));
        }

        public event EventHandler<MessageEventArgs<object>> MessageReceived;
        public event EventHandler<StatusEventArgs> StatusChanged;
    }
}