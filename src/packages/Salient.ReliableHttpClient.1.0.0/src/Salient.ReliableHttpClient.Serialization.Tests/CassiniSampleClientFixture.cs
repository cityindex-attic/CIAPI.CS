using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using CassiniDev;
using NUnit.Framework;
using Salient.ReliableHttpClient.ReferenceImplementation;
using Salient.ReliableHttpClient.Serialization.Newtonsoft;
using Salient.ReliableHttpClient.Serialization.Tests.TestTypes;
using Salient.ReliableHttpClient.TestCore;
//DataContractJsonSerializer 
namespace Salient.ReliableHttpClient.Serialization.Tests
{
    public class CassiniSampleClientFixture : LoggingCassiniDevServer
    {
        

        [TestFixtureSetUp]
        public void Setup()
        {
            string location = new ContentLocator(@"WcfRestService1").LocateContent();
            StartServer(location);
        }


        [Test]
        public void TestServerGet()
        {
            var gate = new AutoResetEvent(false);
            Exception exception = null;
            SampleItem result = null;
            var client = new SampleClient(RootUrl.TrimEnd('/'))
            {
                Serializer = new Serializer()
            };
            client.BeginGetService1(1,ar =>
            {

                try
                {

                    result = client.EndGetService1(ar);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
                gate.Set();
            }, null);

            if (!gate.WaitOne(60000))
            {
                throw new Exception("timed out");
            }
            if (exception != null)
            {
                throw exception;
            }

            Assert.AreEqual(1, result.Id);
        }
  
        [Test]
        public void TestServerList()
        {
            var gate = new AutoResetEvent(false);
            Exception exception = null;
            List<SampleItem> result = null;
            var client = new SampleClient(RootUrl.TrimEnd('/'))
                             {
                                 Serializer = new Serializer()
                             };
            client.BeginListService1(ar =>
            {

                try
                {

                    result = client.EndListService1(ar);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
                gate.Set();
            }, null);

            if (!gate.WaitOne(60000))
            {
                throw new Exception("timed out");
            }
            if (exception != null)
            {
                throw exception;
            }

            Assert.AreEqual(1, result[0].Id);
        }

        [Test]
        public void TestServerCreate()
        {
            var gate = new AutoResetEvent(false);
            Exception exception = null;
            SampleItem item = new SampleItem(){StringValue = "foo"};
            SampleItem result = null;
            var client = new SampleClient(RootUrl.TrimEnd('/'))
            {
                Serializer = new Serializer()
            };
            client.BeginCreateService1(item ,ar =>
            {

                try
                {

                    result = client.EndCreateService1(ar);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
                gate.Set();
            }, null);

            if (!gate.WaitOne(60000))
            {
                throw new Exception("timed out");
            }
            if (exception != null)
            {
                throw exception;
            }

            Assert.AreEqual(1, result.Id);
        }
    }


}
