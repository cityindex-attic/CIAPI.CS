using System;

namespace TradingApi.Client.Core.Lightstreamer
{
    public class StatusEventArgs : EventArgs
    {
        public string Status { get; set; }
    }
}