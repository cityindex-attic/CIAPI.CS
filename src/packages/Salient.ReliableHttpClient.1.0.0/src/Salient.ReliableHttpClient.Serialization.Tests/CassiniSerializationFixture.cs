using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using CassiniDev;
using NUnit.Framework;
using Salient.ReliableHttpClient.ReferenceImplementation;
using Salient.ReliableHttpClient.Serialization.Tests.TestTypes;
using Salient.ReliableHttpClient.TestCore;
//DataContractJsonSerializer 
namespace Salient.ReliableHttpClient.Serialization.Tests
{
    public abstract class CassiniSerializationFixture : LoggingCassiniDevServer
    {
        public abstract IJsonSerializer Serializer { get; }
        [TestFixtureSetUp]
        public void Setup()
        {
            string location = new ContentLocator(@"Salient.ReliableHttpClient.TestWeb").LocateContent();
            StartServer(location);
        }


        /// <summary>
        /// this tests does not need to live in a live server
        /// </summary>
        [Test]
        public void CanSerializeRecursiveTypes()
        {
            var client = new SampleClient(RootUrl.TrimEnd('/'));
            client.Serializer = Serializer;

            var obj = new TestTypes.RecursiveClass()
                          {
                              Id = "1",
                              Nested = new RecursiveClass() { Id = "2" }
                          };
            string json = client.Serializer.SerializeObject(obj);
            RecursiveClass obj2 = client.Serializer.DeserializeObject<RecursiveClass>(json);
        }
        /// <summary>
        /// this tests does not need to live in a live server
        /// </summary>
        [Test, Ignore("everybody crashes hard on this, as expected.")]
        public void CanSerializeSelfReferencingRecursiveTypes()
        {
            var client = new SampleClient(RootUrl.TrimEnd('/'));
            client.Serializer = Serializer;
            var nested = new RecursiveClass { Id = "2" };
            var obj = new RecursiveClass
            {
                Id = "1",
                Nested = nested
            };
            nested.Nested = obj;
            var json = client.Serializer.SerializeObject(obj);
            var obj2 = client.Serializer.DeserializeObject<RecursiveClass>(json);
        }


        /// <summary>
        /// very strange - servicestack fails on the simplest of classes even when decorated to the hilt.
        /// should write a standalone test and see what's up.
        /// </summary>
        [Test]
        public void TestServer()
        {
            var gate = new AutoResetEvent(false);
            Exception exception = null;
            TestClass result = null;
            var client = new SampleClient(RootUrl.TrimEnd('/'));
            client.Serializer = Serializer;
            client.BeginGetTestClass(ar =>
            {

                try
                {

                    result = client.EndGetTestClass(ar);
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
