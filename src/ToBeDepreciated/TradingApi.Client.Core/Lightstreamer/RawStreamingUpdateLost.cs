using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingApi.Client.Core.Lightstreamer
{
    public class RawStreamingUpdateLost : Subscription
    {
        public int LostUpdates { get; set; }
    }
}