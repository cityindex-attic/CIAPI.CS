using System;
using System.Diagnostics;
using System.Threading;
using CIAPI.DTO;
using CIAPI.Rpc;
using CIAPI.Serialization;
using CIAPI.Streaming.Testing;
using CIAPI.StreamingClient;
using Newtonsoft.Json;
using NUnit.Framework;
using Salient.ReliableHttpClient.Testing;

namespace CIAPI.Tests.Streaming
{
    [TestFixture]
    public class TestStreamingListenerFixture
    {
        [Test]
        public void Foo()
        {
            var requestFactory = new TestRequestFactory
                {
                    PrepareResponse = rq =>
                                          {
                                              if (rq.RequestUri.AbsoluteUri == "http://foo.com/session")
                                              {
                                                  
                                              }

                                              Debugger.Break();
                                          }
                };

            var streamingFactory = new TestStreamingClientFactory();

            var rpcClient = new Client(new Uri("http://foo.com"), new Uri("http://foo.com"), "FOOBAR", new Serializer(), requestFactory, streamingFactory);

            ApiLogOnResponseDTO loginResponse = rpcClient.LogIn("MyUserName", "MyPassword");


        }
        [Test]
        public void HowToUseTestStreamingFactory()
        {
            // this is the factory you pass in the ctor of rpc.Client
            // keep an instance reference so you can feed the streams
            var streamingFactory = new TestStreamingClientFactory();

            // before the listener publishes the MessageReceived event
            // it will give you access to the data item and it's descriptors
            streamingFactory.CreateNewsMessage = (args) =>
                                                      {
                                                          // in this handler you can check the args.DataAdapter
                                                          // and args.Topic to inform your code
                                                          // in this case we just send foobar data
                                                          
                                                          NewsDTO dto = args.Data;
                                                          dto.Headline = "we are listening to " + args.DataAdapter + "." + args.Topic;
                                                      };



            var client = streamingFactory.Create(new Uri("http://foo.com"), "me", "pwd", new CIAPI.Serialization.Serializer());
            var listener = client.BuildNewsHeadlinesListener("YOUR CATEGORY");

            var gate = new AutoResetEvent(false);
            MessageEventArgs<NewsDTO> received = null;
            listener.MessageReceived += (sender, args) =>
                                            {
                                                received = args;
                                                gate.Set();

                                            };
            // we don't usually start listeners ourselves but for this test we do.
            listener.Start(0);


            gate.WaitOne();

            // tear down as usual
            client.TearDownListener(listener);


            Assert.AreEqual("we are listening to CITYINDEXSTREAMING.NEWS.HEADLINES.YOUR CATEGORY", received.Data.Headline);

        }
        [Test]
        public void Test()
        {

            var listener = new TestStreamingListener<ApiLookupDTO>("DATAADAPTER", "TOPIC");

            listener.CreateMessage += (e) =>
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