using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using CIAPI.IntegrationTests.Streaming;
using CIAPI.Rpc;
using NUnit.Framework;

namespace CIAPI.IntegrationTests.Rpc
{
    [TestFixture]
    public class MetricsFixture : RpcFixtureBase
    {
        [Test]
        public void CanSendMetrics()
        {

            // set up a listener
            var log = new StringBuilder();
            var writer = new StringWriter(log);
            var listener = new TextWriterTraceListener(writer);
            Trace.Listeners.Add(listener);

            var rpcClient = new Client(Settings.RpcUri, Settings.StreamingUri, "my-test-appkey");
            var metricsRecorder = new MetricsRecorder(rpcClient, new Uri("http://metrics.labs.cityindex.com/LogEvent.ashx"), "metrics-session");
            metricsRecorder.Start();

            rpcClient.LogIn(Settings.RpcUserName, Settings.RpcPassword);

            var headlines = rpcClient.News.ListNewsHeadlinesWithSource("dj", "UK", 100);

            foreach (var item in headlines.Headlines)
            {
                rpcClient.News.GetNewsDetail("dj", item.StoryId.ToString());
            }

            Thread.Sleep(1000);

            rpcClient.LogOut();

            metricsRecorder.Stop();
            rpcClient.Dispose();

            Trace.Listeners.Remove(listener);

            var logText = log.ToString();

            Assert.IsTrue(logText.Contains("Latency message complete"), "did not find evidence of metrics being posted");
        }

        private const string MetricsUrl = "http://metrics.labs.cityindex.com/";

        /// <summary>
        /// Test case for issue 175
        /// </summary>
        [Test]
        public void MetricsAreSentWithSelectedSessionIdentifier()
        {
            var startTime = DateTime.UtcNow;
            var metricsSession = "MetricsAreSentWithSelectedSessionIdentifier_" + Guid.NewGuid();

            // set up a listener
            var log = new StringBuilder();
            var writer = new StringWriter(log);
            var listener = new TextWriterTraceListener(writer);
            Trace.Listeners.Add(listener);

            var rpcClient = new Client(Settings.RpcUri, Settings.StreamingUri, "my-test-appkey");

            var metricsRecorder = new MetricsRecorder(rpcClient, new Uri(MetricsUrl + "LogEvent.ashx"), metricsSession, Settings.AppMetrics_AccessKey);
            metricsRecorder.Start();

            rpcClient.LogIn(Settings.RpcUserName, Settings.RpcPassword);

            rpcClient.LogOut();

            metricsRecorder.Stop();
            rpcClient.Dispose();

            Trace.Listeners.Remove(listener);

            using (var client = new WebClient())
            {
                client.Credentials = new NetworkCredential(Settings.AppMetrics_UserName, Settings.AppMetrics_Password);
                client.QueryString["AppKey"] = "my-test-appkey";
                client.QueryString["StartTime"] = startTime.ToString("u");

                var response = client.DownloadString(MetricsUrl + "GetSessions.ashx");

                Assert.IsTrue(response.Contains(metricsSession));
            }
        }
    }
}