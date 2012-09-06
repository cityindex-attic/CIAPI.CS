using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Salient.ReflectiveLoggingAdapter;
using Salient.ReliableHttpClient;

namespace CIAPI.Rpc
{
    public class MetricsRecorder : Recorder
    {
        private readonly string _metricsSession;
        private readonly string _metricsAccessKey;

        public Uri AppmetricsUri { get; private set; }
        private readonly Timer _metricsTimer;
        
        private static readonly ILog Log = LogManager.GetLogger(typeof(MetricsRecorder));

        public MetricsRecorder(Client client, Uri appmetricsUri, string metricsSession, string metricsAccessKey = null)
            : base(client)
        {
            _metricsSession = metricsSession;
            _metricsAccessKey = metricsAccessKey;
            AppmetricsUri = appmetricsUri;
            
            _metricsTimer = new Timer(ignored => PostMetrics(), null, 1000, 10000);
        }

        public override void Stop()
        {
            _metricsTimer.Change(int.MaxValue, int.MaxValue);
            PostMetrics();
            base.Stop();
        }
        private void PostMetrics()
        {
            
            if(Paused )
            {
                return;
            }


            var records = GetRequests();

            if (records.Count == 0)
            {
                return;
            }

            Clear();

            

            var sb = new StringBuilder();
            foreach (var item in records)
            {
                if (item.Exception != null)
                    continue;
                try
                {
                    if (item.Uri.AbsoluteUri != AppmetricsUri.AbsoluteUri)
                    {
                        sb.AppendLine(string.Format("{0}\tLatency {1}\t{2}", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fffffff"), item.Uri, item.Completed.Subtract(item.Issued).TotalSeconds));
                    }

                }
                catch (Exception)
                {
                    //swallow. this is just a safety swallow. no need to log
                }
            }


            var latencyData = sb.ToString();

            Log.Debug("LATENCY:/n" + latencyData);

            try
            {
                Client.BeginRequest(RequestMethod.POST, AppmetricsUri.AbsoluteUri, "", new Dictionary<string, string>(), 
                    new Dictionary<string, object>
                        {
                            { "AccessKey", _metricsAccessKey }, 
                            { "MessageAppKey", ((Client)Client).AppKey ?? "null" }, 
                            { "MessageSession", _metricsSession }, 
                            { "MessagesList", latencyData }
                        },
                        ContentType.FORM,
                        ContentType.TEXT,
                        TimeSpan.FromMilliseconds(0),
                        30000,
                        0, ar =>
                            {
                                try
                                {
                                    ((Client)Client).EndRequest(ar);
                                    Log.Debug("Latency message complete.");
                                }
                                catch (Exception ex)
                                {

                                    Log.Error("Latency message failed to complete.", ex);
                                }

                            }, null);

            }
            catch (Exception ex2)
            {
                Log.Error("Latency message failed to be issued.", ex2);
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_metricsTimer != null)
                {
                    _metricsTimer.Dispose();
                }
            }

            base.Dispose(disposing);
        }
    }
}