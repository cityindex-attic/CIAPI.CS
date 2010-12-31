using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using NUnit.Framework;


namespace CIAPI.Core.Tests
{
    [TestFixture]
    public class ThrottleFixture
    {
        [Test]
        public void ThrottleBlocksWhenThresholdExceeded()
        {
            var f = new TestRequestFactory();
            var t = new ThrottedRequestQueue(TimeSpan.FromSeconds(3), 1, 10);

            var sw = new Stopwatch();
            sw.Start();
            var handles = new List<WaitHandle>();
            
            for (int i = 0; i < 3; i++)
            {
                var gate = new AutoResetEvent(false);
                handles.Add(gate);

                f.CreateTestRequest("foo" + i);
                const string url = "http://tmpuri.org";
                var r = f.Create(url);
                int i1 = i;
                t.Enqueue(url, r, ar =>
                    {
                        string expected = "foo" + i1;    
                        var response = r.EndGetResponse(ar);
                        var actual = new StreamReader(response.GetResponseStream()).ReadToEnd();
                        Trace.WriteLine(actual);
                        Assert.AreEqual(expected, actual);
                        gate.Set();
                    });

            }
            WaitHandle.WaitAll(handles.ToArray());
            sw.Stop();

            Assert.GreaterOrEqual(sw.ElapsedMilliseconds, 5800, "3 requests, 2 throttled - expect almost 6 seconds delay for 3 requests");
        }
    }
}
