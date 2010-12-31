using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using NUnit.Framework;

namespace CIAPI.Core.Tests
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
        public void CanInduceLatency()
        {
            var f = new TestRequestFactory();
            const string expected = "foo";
            f.CreateTestRequest(expected, 200, null, null);
            WebRequest r = f.Create("");
            var sw = new Stopwatch();
            sw.Start();
            string actual = new StreamReader(r.GetResponse().GetResponseStream()).ReadToEnd();
            sw.Stop();
            Assert.AreEqual(expected, actual);
            Assert.GreaterOrEqual(sw.ElapsedMilliseconds, 150, "incorrect latency");
        }

        [Test, ExpectedException(typeof (Exception), ExpectedMessage = "request stream exception")]
        public void CanThrowExceptionOnRequestStream()
        {
            var f = new TestRequestFactory();
            f.CreateTestRequest("", 0, new Exception("request stream exception"), null);
            WebRequest r = f.Create("");
            r.GetRequestStream();
        }

        [Test, ExpectedException(typeof (Exception), ExpectedMessage = "response stream exception")]
        public void CanThrowExceptionOnResponseStream()
        {
            var f = new TestRequestFactory();
            f.CreateTestRequest("", 0, null, new Exception("response stream exception"));
            WebRequest r = f.Create("");
            r.GetResponse();
        }
    }
}