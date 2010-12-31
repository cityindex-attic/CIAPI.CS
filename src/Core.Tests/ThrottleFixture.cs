using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Soapi.Net;

namespace CIAPI.Core.Tests
{
    [TestFixture]
    public class ThrottleFixture
    {
        [Test]
        public void ThrottleBlocksWhenThresholdExceeded()
        {
            var f = new TestRequestFactory();
            var t = new RequestThrottle(f, TimeSpan.FromSeconds(3), 1, 10);
            f.CreateTestRequest("foo");
            var sw = new Stopwatch();
            sw.Start();

            for (int i = 0; i < 3; i++)
            {
                var r = t.Create("http://tmpuri.org");
                new StreamReader(r.GetResponse().GetResponseStream()).ReadToEnd();
            }

            sw.Stop();

            Assert.GreaterOrEqual(sw.ElapsedMilliseconds, 5800, "3 requests, 2 throttled - expect almost 6 seconds delay for 3 requests");
        }
    }
}
