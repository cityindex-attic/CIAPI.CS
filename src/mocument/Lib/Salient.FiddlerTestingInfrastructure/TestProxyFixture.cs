using System;
using System.Threading;
using NUnit.Framework;

namespace Salient.FiddlerTestingInfrastructure
{
    [TestFixture]
    public class TestProxyFixture
    {
        [Test, Ignore(@"
                thread access to waithandle is non-deterministic so the assertion is too naive to prove this test passes 
                but when thread access is controlled in expected order test passes which, along with watching console out, 
                proves the blocking works as expected")]
        public void TestProxyWillBlock()
        {
            // we want to ensure that the static class TestProxyFactory, which wraps the static FiddlerApplication class, will block
            // so that there is only ever one proxy running in any one process.

            // this is going to be an interesting test to write......

            var proxies = new TestProxyFactory.TestProxy[3];

            var handles = new WaitHandle[3];
            handles[0] = new ManualResetEvent(false);
            handles[1] = new ManualResetEvent(false);
            handles[2] = new ManualResetEvent(false);


            for (int i = 0; i < 3; i++)
            {
                new Thread((o) =>
                               {
                                   var index = (int) o;
                                   Console.WriteLine("creating TestProxy #{0}", index);
                                   TestProxyFactory.TestProxy testProxy = TestProxyFactory.Create();
                                   proxies[index] = testProxy;
                                   Console.WriteLine("starting TestProxy #{0}", index);
                                   testProxy.Start();
                                   Console.WriteLine("started TestProxy #{0}", index);
                                   Thread.Sleep(2000);
                                   Console.WriteLine("stopping TestProxy #{0}", index);
                                   testProxy.Stop();
                                   Console.WriteLine("TestProxy #{0} stopped", index);
                                   ((ManualResetEvent) handles[index]).Set();
                               }).Start(i);
                Thread.Sleep(100);
            }

            Console.WriteLine("waiting.....");
            WaitHandle.WaitAll(handles);
            Console.WriteLine("finished");

            // verify that the start time of one was later than the end time of the previous
            Assert.Greater(proxies[1].StartTime.Ticks, proxies[0].StopTime.Ticks);
            Assert.Greater(proxies[2].StartTime.Ticks, proxies[1].StopTime.Ticks);
        }
    }
}