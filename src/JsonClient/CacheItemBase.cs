using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;

namespace CityIndex.JsonClient
{
    /// <summary>
    /// Provides a base composition element for <see cref="CacheItem{TDTO}"/> to simplify program logic.
    /// </summary>
    public class CacheItemBase
    {
        ///<summary>
        ///</summary>
        public CacheItemBase()
        {
            ProcessingWaitHandle = new AutoResetEvent(false);
        }

        ///<summary>
        /// The url of this request
        ///</summary>
        public string Url { get; set; }

        public Dictionary<string, object> Parameters { get; set; }
        public string Method { get; set; }
        public string ThrottleScope { get; set; }
        public string Target { get; set; }
        public string UriTemplate { get; set; }
        public WebRequest Request { get; set; }

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

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("ItemState       : {0}\r\n", this.ItemState);
            sb.AppendFormat("Url             : {0}\r\n", this.Url ?? "NULL");
            sb.AppendFormat("Method          : {0}\r\n", this.Method ?? "NULL");
            sb.AppendFormat("Target          : {0}\r\n", this.Target ?? "NULL");
            sb.AppendFormat("UriTemplate     : {0}\r\n", this.UriTemplate ?? "NULL");

            if (this.Parameters != null)
            {
                sb.AppendFormat("Parameters      : \r\n");
                foreach (KeyValuePair<string, object> kvp in this.Parameters)
                {
                    sb.AppendFormat("\t{0}: {1}\r\n", kvp.Key, kvp.Key.ToLower() == "password" ? "*****" : kvp.Value ?? "NULL");
                }

            }

            if (this.Request != null)
            {
                sb.AppendFormat("Request URI     : {0}\r\n", this.Request.RequestUri.AbsoluteUri);

                if (this.Request.Headers != null)
                {
                    sb.AppendFormat("Request Headers : \r\n");
                    foreach (string header in this.Request.Headers)
                    {
                        sb.AppendFormat("\t{0}: {1}\r\n", header, this.Request.Headers[header] ?? "NULL");
                    }
                }

            }

            sb.AppendFormat("CacheDuration   : {0}\r\n", this.CacheDuration);
            sb.AppendFormat("RetryCount      : {0}\r\n", this.RetryCount);
            sb.AppendFormat("ThrottleScope   : {0}\r\n", this.ThrottleScope ?? "NULL");
            sb.AppendFormat("Expiration      : {0}\r\n", this.Expiration);
            sb.AppendFormat("ResponseText    : {0}\r\n", this.ResponseText ?? "NULL");

            if (this.Exception != null)
            {
                sb.AppendFormat("Exception       : {0}\r\n", this.Exception);
            }


            return sb.ToString();
        }
    }
}