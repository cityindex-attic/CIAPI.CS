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
        #region Synchronous
        [Test]
        public void CanCreateTestRequest_Synchronous()
        {
            var f = new TestRequestFactory();
            const string expected = "foo";
            f.CreateTestRequest(expected);
            WebRequest r = f.Create("http://testuri.org");
            string actual = new StreamReader(r.GetResponse().GetResponseStream()).ReadToEnd();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CanInduceLatency_Synchronous()
        {
            var f = new TestRequestFactory();
            const string expected = "foo";
            f.CreateTestRequest(expected, 200, null, null, null);
            WebRequest r = f.Create("http://testuri.org");
            var sw = new Stopwatch();
            sw.Start();
            string actual = new StreamReader(r.GetResponse().GetResponseStream()).ReadToEnd();
            sw.Stop();
            Assert.AreEqual(expected, actual);
            Assert.GreaterOrEqual(sw.ElapsedMilliseconds, 150, "incorrect latency");
        }


        [Test, ExpectedException(typeof(Exception), ExpectedMessage = "request stream exception")]
        public void CanThrowExceptionOnRequestStream_Synchronous()
        {
            var f = new TestRequestFactory();
            f.CreateTestRequest("", 0, new Exception("request stream exception"), null, null);
            WebRequest r = f.Create("http://testuri.org");
            r.GetRequestStream();
            Assert.Fail("Expected exception");
        }

        [Test, ExpectedException(typeof(Exception), ExpectedMessage = "response stream exception")]
        public void CanThrowExceptionOnResponseStream_Synchronous()
        {
            var f = new TestRequestFactory();
            f.CreateTestRequest("", 0, null, new Exception("response stream exception"), null);
            WebRequest r = f.Create("http://testuri.org");
            r.GetResponse();
            Assert.Fail("Expected exception");
        }


        [Test, ExpectedException(typeof(Exception), ExpectedMessage = "response exception")]
        public void CanThrowExceptionOnEndGetResponse_Synchronous()
        {
            var f = new TestRequestFactory();
            f.CreateTestRequest("", 0, null, new Exception("response exception"), null);
            WebRequest r = f.Create("http://testuri.org");
            r.GetResponse();
            Assert.Fail("Expected exception");
        } 
        #endregion


        #region Asynchronous
        [Test]
        public void CanCreateTestRequest_Asynchronous()
        {


            using (var gate = new AutoResetEvent(true))
            {
                var f = new TestRequestFactory();
                const string expected = "foo";
                f.CreateTestRequest(expected);
                WebRequest r = f.Create("http://testuri.org");
                r.BeginGetResponse(ar =>
                                       {
                                           
                                       }, null);
                string actual = new StreamReader(r.GetResponse().GetResponseStream()).ReadToEnd();
                

                if(!gate.WaitOne(1000))
                {
                    Assert.Fail("timed out");
                }
                Assert.AreEqual(expected, actual);
            }
        }

        [Test]
        public void CanInduceLatency_Asynchronous()
        {
            using (var gate = new AutoResetEvent(true))
            {

                var f = new TestRequestFactory();
                const string expected = "foo";
                f.CreateTestRequest(expected, 200, null, null, null);
                WebRequest r = f.Create("http://testuri.org");
                var sw = new Stopwatch();
                sw.Start();
                r.BeginGetResponse(ar =>
                {

                }, null);

                string actual = new StreamReader(r.GetResponse().GetResponseStream()).ReadToEnd();
                
                
                if (!gate.WaitOne(1000))
                {
                    Assert.Fail("timed out");
                }

                sw.Stop();
                Assert.AreEqual(expected, actual);
                Assert.GreaterOrEqual(sw.ElapsedMilliseconds, 150, "incorrect latency");

            }

            
        }


        [Test, ExpectedException(typeof(Exception), ExpectedMessage = "request stream exception")]
        public void CanThrowExceptionOnRequestStream_Asynchronous()
        {
            using (var gate = new AutoResetEvent(true))
            {
                var f = new TestRequestFactory();
                f.CreateTestRequest("", 0, new Exception("request stream exception"), null, null);
                WebRequest r = f.Create("http://testuri.org");
                r.BeginGetResponse(ar =>
                {

                }, null);

                r.GetRequestStream();
                Assert.Fail("Expected exception");

                if (!gate.WaitOne(1000))
                {
                    Assert.Fail("timed out");
                }
            }

            
        }

        [Test, ExpectedException(typeof(Exception), ExpectedMessage = "response stream exception")]
        public void CanThrowExceptionOnResponseStream_Asynchronous()
        {
            using (var gate = new AutoResetEvent(true))
            {
                var f = new TestRequestFactory();
                f.CreateTestRequest("", 0, null, new Exception("response stream exception"), null);
                WebRequest r = f.Create("http://testuri.org");
                r.BeginGetResponse(ar =>
                {

                }, null);
                r.GetResponse();
                Assert.Fail("Expected exception");


                if (!gate.WaitOne(1000))
                {
                    Assert.Fail("timed out");
                }
            }


        }


        [Test, ExpectedException(typeof(Exception), ExpectedMessage = "response exception")]
        public void CanThrowExceptionOnEndGetResponse_Asynchronous()
        {
            using (var gate = new AutoResetEvent(true))
            {
                var f = new TestRequestFactory();
                f.CreateTestRequest("", 0, null, new Exception("response exception"), null);
                WebRequest r = f.Create("http://testuri.org");

                r.BeginGetResponse(ar =>
                {

                }, null);

                r.GetResponse();
                Assert.Fail("Expected exception");

                if (!gate.WaitOne(1000))
                {
                    Assert.Fail("timed out");
                }
            }

            
        }
        #endregion
    }
}