using System;
using System.Threading;

namespace CityIndex.JsonClient
{
    /// <summary>
    /// Provides a base composition element for <see cref="CacheItem{TDTO}"/> to simplify program logic.
    /// </summary>
    public class CacheItemBase
    {
        public CacheItemBase()
        {
            ProcessingWaitHandle = new AutoResetEvent(false);
        }

        /// <summary>
        /// The result of this item's request, if any
        /// </summary>
        public string ResponseText { get; set; }

        /// <summary>
        /// The absolute time which this item should be expired and purged
        /// </summary>
        public DateTimeOffset Expiration { get; set; }

        /// <summary>
        /// The length of time to cache this item
        /// </summary>
        public TimeSpan CacheDuration { get; set; }

        /// <summary>
        /// The state of this cache item.
        /// </summary>
        public CacheItemState ItemState { get; set; }

        /// <summary>
        /// The exception, if any, that occurred while processing the request for this item.
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// A wait handle that enables blocking while this item's callbacks are being processed.
        /// </summary>
        public AutoResetEvent ProcessingWaitHandle { get; private set; }

        /// <summary>
        /// Indicates how many times the request for this item has been attempted
        /// </summary>
        public int RetryCount { get; set; }

        /// <summary>
        /// Signals that processing of queued callbacks is complete.
        /// </summary>
        public event EventHandler ProcessingComplete;


        /// <summary>
        /// Signals that processing of queued callbacks is complete.
        /// </summary>
        public void OnProcessingComplete()
        {
            EventHandler handler = ProcessingComplete;

            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }
}