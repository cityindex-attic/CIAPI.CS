using System;

namespace TradingApi.Client.Core.Lightstreamer
{
    public class LightstreamerEventArgs<T> : EventArgs
    {
        public T Item { get; set; }
    }
}