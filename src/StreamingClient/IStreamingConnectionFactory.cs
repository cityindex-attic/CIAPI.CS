using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingApi.Client.Core.Lightstreamer;

namespace StreamingClient
{
    public interface IStreamingConnectionFactory
    {
        ILightstreamerConnection Create(Uri streamingUri, string userName, string sessionId);
    }
}
