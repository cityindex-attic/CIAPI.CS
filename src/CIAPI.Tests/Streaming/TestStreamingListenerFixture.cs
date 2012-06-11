using System;
using System.Threading;
using CIAPI.DTO;
using CIAPI.Streaming.Testing;
using NUnit.Framework;

namespace CIAPI.Tests.Streaming
{
    [TestFixture]
    public class TestStreamingListenerFixture
    {
        [Test]
        public void Test()
        {
            
            var listener = new TestStreamingListener<ApiLookupDTO>("foo", "foo");
            listener.CreateMessage += (sender, e) =>
            {
                e.Data.Description = "FOO";
            };
 
            var gate = new AutoResetEvent(false);
            int counter = 0;
            Exception exception = null;
            listener.MessageReceived += (sender, e) =>
            {
                Console.WriteLine("got {0} ", e.Data.Description);
                if (++counter > 10)
                {
                    gate.Set();
                }
                try
                {
                    if (e.Data.Description != "FOO")
                    {
                        throw new Exception("expected description FOO");
                    }
                }
                catch (Exception ex)
                {

                    exception = ex;
                    gate.Set();
                }
            };

            listener.Start(0);

            gate.WaitOne();
            listener.Stop();
 
            listener.Dispose();

            if (exception != null)
            {
                Assert.Fail(exception.Message);
            }
        }


    }
}