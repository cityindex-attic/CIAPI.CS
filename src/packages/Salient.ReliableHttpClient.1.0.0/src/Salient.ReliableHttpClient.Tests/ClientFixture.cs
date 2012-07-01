using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using Salient.ReliableHttpClient.Serialization.Newtonsoft;

namespace Salient.ReliableHttpClient.Tests
{
    [TestFixture]
    public class ClientFixture : LoggingFixtureBase
    {
        public class FooClass
        {
            [Test]
            public void TestRecorder()
            {

                var client = new ClientBase(new Serializer());
                var recorder = new Recorder(client);
                recorder .Start();
                
                var gate = new AutoResetEvent(false);
                Exception exception = null;
                FooClass result = null;
                Guid id = client.BeginRequest(RequestMethod.GET, "http://api.geonames.org", "/citiesJSON?north={north}&south={south}&east{east}&west={west}&lang={lang}&username={username}", new Dictionary<string, string>(),new Dictionary<string, object>
                                                                 {
                                                                     {"north",44.1},
                                                                     {"south",-9.9},
                                                                     {"east",-22.4},
                                                                     {"west",55.2},
                                                                     {"lang","de"},
                                                                     {"username","demo"}
                                                                 }, ContentType.TEXT, ContentType.JSON, TimeSpan.FromSeconds(1), 3000, 0, ar =>
                                                                 {
                                                                     try
                                                                     {
                                                                         
                                                                         result = client.EndRequest<FooClass>(ar);
                                                                         var responsetext = ar.ResponseText;

                                                                     }
                                                                     catch (Exception ex)
                                                                     {
                                                                         exception = ex;
                                                                     }
                                                                     gate.Set();

                                                                 }, null);
                if (!gate.WaitOne(10000))
                {
                    throw new Exception("timed out");
                }

                // verify cache has purged
                gate.WaitOne(3000);

                if (exception != null)
                {
                    Assert.Fail(exception.Message);
                }
                recorder .Stop();
                List<RequestInfoBase> recorded = recorder.GetRequests();
                recorder .Dispose();
                Assert.IsTrue(recorded.Count==1);
                var recordedJson = client.Serializer.SerializeObject(recorded);
                List<RequestInfoBase> deserializedRecording =
                    client.Serializer.DeserializeObject<List<RequestInfoBase>>(recordedJson);
                Assert.IsTrue(deserializedRecording.Count == 1);
            }
            [Test]
            public void Test()
            {

                var client = new ClientBase(new Serializer());
                var gate = new AutoResetEvent(false);
                Exception exception = null;
                FooClass result = null;
                Guid id = client.BeginRequest(RequestMethod.GET, "http://api.geonames.org", "/citiesJSON?north={north}&south={south}&east{east}&west={west}&lang={lang}&username={username}", new Dictionary<string, string>(),new Dictionary<string, object>
                                                                 {
                                                                     {"north",44.1},
                                                                     {"south",-9.9},
                                                                     {"east",-22.4},
                                                                     {"west",55.2},
                                                                     {"lang","de"},
                                                                     {"username","demo"}
                                                                 }, ContentType.TEXT, ContentType.JSON, TimeSpan.FromSeconds(1), 3000, 0, ar =>
                                                                           {
                                                                               try
                                                                               {
                                                                                   result = client.EndRequest<FooClass>(ar);
                                                                                   var responsetext = ar.ResponseText;

                                                                               }
                                                                               catch (Exception ex)
                                                                               {
                                                                                   exception = ex;
                                                                               }
                                                                               gate.Set();

                                                                           }, null);
                if (!gate.WaitOne(10000))
                {
                    throw new Exception("timed out");
                }

                // verify cache has purged
                gate.WaitOne(3000);

                if (exception != null)
                {
                    Assert.Fail(exception.Message);
                }

                var output = GetLogOutput();
            }
        }
    }
}