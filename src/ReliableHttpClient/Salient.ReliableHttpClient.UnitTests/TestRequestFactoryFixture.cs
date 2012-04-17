using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using NUnit.Framework;
using Salient.ReliableHttpClient.ReferenceImplementation;
using Salient.ReliableHttpClient.Testing;

namespace Salient.ReliableHttpClient.UnitTests
{
    

    [TestFixture]
    public class TestRequestFactoryFixture
    {

        // need to create a request processor that can perform common tasks such as locating a matching request keying off as many
        // properties as necessary, including request stream for posts...
        [Test]
        public void Test()
        {
            var requestFactory = new TestRequestFactory();
            requestFactory.RequestTimeout = TimeSpan.FromSeconds(30);
            requestFactory.PrepareResponse = r =>
                                                 {
                                                     byte[] responseBytes = Encoding.UTF8.GetBytes("{\"Id\":1}");
                                                     r.ResponseStream.Write(responseBytes, 0, responseBytes.Length);
                                                     r.ResponseStream.Seek(0, SeekOrigin.Begin);
                                                 };
            var client = new SampleClient("http://foo.com", requestFactory);

            var gate = new AutoResetEvent(false);
            TestClass response = null;
            client.BeginGetTestClass(ar =>
                                         {

                                             response = client.EndGetTestClass(ar);
                                             gate.Set();
                                         }, null);

            if (!gate.WaitOne(40000))
            {
                throw new Exception("timed out");
            }

            Assert.AreEqual(1, response.Id);

        }
    }
}
