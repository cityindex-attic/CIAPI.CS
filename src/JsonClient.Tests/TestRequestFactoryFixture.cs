using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using NUnit.Framework;

namespace CityIndex.JsonClient.Tests
{
    /// <summary>
    /// Ensures the behavior of the mock http request layer
    /// </summary>
    [TestFixture]
    public class TestRequestFactoryFixture
    {
        [Test]
        public void CanCreateTestRequest()
        {
            var f = new TestRequestFactory();
            const string expected = "foo";
            f.CreateTestRequest(expected);
            WebRequest r = f.Create("");
            string actual = new StreamReader(r.GetResponse().GetResponseStream()).ReadToEnd();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CanInduceLatencyInSyncGetResponse()
        {
            const int desiredLatencyMs = 200;

            var f = new TestRequestFactory();
            const string expected = "foo";
            f.CreateTestRequest(expected, TimeSpan.FromMilliseconds(desiredLatencyMs), null, null, null);
            WebRequest webRequest = f.Create("");
            var sw = new Stopwatch();
            sw.Start();
            var actual = new StreamReader(webRequest.GetResponse().GetResponseStream()).ReadToEnd();
            sw.Stop();
            
            Assert.AreEqual(expected, actual);
            Assert.GreaterOrEqual(sw.ElapsedMilliseconds, desiredLatencyMs - 50, "incorrect latency");
            // TODO: Sometimes this seems to take *much* longer on the build server
            // Assert.LessOrEqual(sw.ElapsedMilliseconds, desiredLatencyMs + 50, "incorrect latency");
        }

        [Test]
        public void CanInduceLatencyInAsyncGetResponse()
        {
            const int desiredLatencyMs = 200;

            var f = new TestRequestFactory();
            const string expected = "foo";
            f.CreateTestRequest(expected, TimeSpan.FromMilliseconds(desiredLatencyMs), null, null, null);
            WebRequest r = f.Create("");
            var sw = new Stopwatch();
            sw.Start();
            string actual = "";
            using (var gate = new ManualResetEvent(false))
            {
                r.BeginGetResponse(ar =>
                                       {
                                           WebResponse response = r.EndGetResponse(ar);
                                           actual = new StreamReader(response.GetResponseStream()).ReadToEnd();
                                           gate.Set();
                                       }, null);
                gate.WaitOne(TimeSpan.FromSeconds(2));
            }
           
            sw.Stop();
            Assert.AreEqual(expected, actual);
            Assert.GreaterOrEqual(sw.ElapsedMilliseconds, desiredLatencyMs - 50, "incorrect latency");
            // TODO: Sometimes this seems to take *much* longer on the build server
            // Assert.LessOrEqual(sw.ElapsedMilliseconds, desiredLatencyMs + 50, "incorrect latency");
        }

        [Test, ExpectedException(typeof (Exception), ExpectedMessage = "request stream exception")]
        public void CanThrowExceptionOnRequestStream()
        {
            var f = new TestRequestFactory();
            f.CreateTestRequest("", TimeSpan.FromMilliseconds(0), new Exception("request stream exception"), null, null);
            WebRequest r = f.Create("");
            r.GetRequestStream();
            Assert.Fail("Expected exception");
        }

        [Test, ExpectedException(typeof (Exception), ExpectedMessage = "response stream exception")]
        public void CanThrowExceptionOnResponseStream()
        {
            var f = new TestRequestFactory();
            f.CreateTestRequest("", TimeSpan.FromMilliseconds(0), null, new Exception("response stream exception"), null);
            WebRequest r = f.Create("");
            r.GetResponse();
            Assert.Fail("Expected exception");
        }


        [Test, ExpectedException(typeof(Exception), ExpectedMessage = "response exception")]
        public void CanThrowExceptionOnEndGetResponse()
        {
            var f = new TestRequestFactory();
            f.CreateTestRequest("", TimeSpan.FromMilliseconds(0), null, new Exception("response exception"), null);
            WebRequest r = f.Create("");
            r.GetResponse();
            Assert.Fail("Expected exception");
        }
    }
}