using System;

namespace TradingApi.Client.Core.Lightstreamer
{
    public class StreamingEventArgs<T> : EventArgs
    {
        public T Item { get; set; }
    }
}