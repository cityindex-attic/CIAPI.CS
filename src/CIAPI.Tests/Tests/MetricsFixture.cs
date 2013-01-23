using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using CIAPI.DTO;
using CIAPI.RecordedTests.Infrastructure;
using CIAPI.Rpc;
using NUnit.Framework;

namespace CIAPI.RecordedTests
{
    [TestFixture, MocumentModeOverride(MocumentMode.Play),Ignore("test records but metrics entries not found... step through")]
    public class MetricsFixture : CIAPIRecordingFixtureBase
    {
        private const string METRICS_URL = "http://metrics.labs.cityindex.com/";
        [Test]
        public void CanSendMetrics()
        {



            var rpcClient = BuildUnauthenticatedRpcClient();
            Uri metricsUrl = BuildUri("http://metrics.labs.cityindex.com/LogEvent.ashx", 1);
            var metricsRecorder = new MetricsRecorder(rpcClient, metricsUrl, "metrics-session");
            metricsRecorder.Start();

            rpcClient.LogIn(UserName, Password);

            var headlines = rpcClient.News.ListNewsHeadlinesWithSource("dj", "UK", 100);

            foreach (var item in headlines.Headlines)
            {
                rpcClient.News.GetNewsDetail("dj", item.StoryId.ToString());
            }

            Thread.Sleep(1000);

            rpcClient.LogOut();

            metricsRecorder.Stop();

            var purgeHandle = rpcClient.ShutDown();

            if (!purgeHandle.WaitOne(60000))
            {
                throw new Exception("timed out waiting for client to purge");
            }

            rpcClient.Dispose();
            var tape = this.TrafficLog.List()[0];
            //metrics-session

            var metricssent = tape.log.entries.Any(e => e.request.url.IndexOf("metrics.labs.cityindex.com", StringComparison.CurrentCultureIgnoreCase) > -1 && e.request.postData.@params[1].value.Length > 0);

            Assert.IsTrue(metricssent, "did not find evidence of metrics being posted");
        }



        /// <summary>
        /// Test case for issue 175
        /// </summary>
        [Test,Ignore("need metrics credentials")]
        public void MetricsAreSentWithSelectedSessionIdentifier()
        {
            //var startTime = DateTime.UtcNow;
            //var metricsSession = "MetricsAreSentWithSelectedSessionIdentifier_" + Guid.NewGuid();
             

            //var rpcClient = BuildUnauthenticatedRpcClient();

            //var metricsRecorder = new MetricsRecorder(rpcClient, new Uri(METRICS_URL + "LogEvent.ashx"), metricsSession, Settings.AppMetrics_AccessKey);
            //metricsRecorder.Start();

            //rpcClient.LogIn(UserName, Password);

            //rpcClient.LogOut();

            //metricsRecorder.Stop();
            //rpcClient.Dispose();

    
    

            //using (var client = new WebClient())
            //{
            //    client.Credentials = new NetworkCredential(Settings.AppMetrics_UserName, Settings.AppMetrics_Password);
            //    client.QueryString["AppKey"] = "my-test-appkey";
            //    client.QueryString["StartTime"] = startTime.ToString("u");

            //    var response = client.DownloadString(METRICS_URL + "GetSessions.ashx");

            //    Assert.IsTrue(response.Contains(metricsSession));
            //}
        }
    }
}