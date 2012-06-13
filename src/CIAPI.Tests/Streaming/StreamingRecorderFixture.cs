using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using CIAPI.DTO;
using CIAPI.Rpc;
using CIAPI.Streaming;
using CIAPI.Streaming.Testing;
using CIAPI.StreamingClient;
using NUnit.Framework;
using Salient.ReliableHttpClient.Testing;

namespace CIAPI.Tests.Streaming
{
    [TestFixture]
    public class StreamingRecorderFixture : FixtureBase
    {

 

        //#FIXME: something in here is causing ToStringWithValues to bleed into the other testes
        [Test]
        public void HowToUseSequentialMessageGenerator()
        {

            TestRequestFactory requestFactory;
            TestStreamingClientFactory streamingFactory;
            Client client = BuildAuthenticatedTestClient(out requestFactory, out streamingFactory);


            var serialized = File.ReadAllText("Streaming\\streamingprices01.txt");

            var messages = client.DeserializeJson<List<MessageEventArgs<PriceDTO>>>(serialized);

            var generator = new SequentialSerializedPriceDTOMessageGenerator(messages);


            var streamingClient = client.CreateStreamingClient();
            


            var priceListener = (TestStreamingListener<PriceDTO>)streamingClient.BuildDefaultPricesListener(2347);

            priceListener.CreateMessage += (s, e) =>
                {
                    var match = generator.GetNextMessage(e.DataAdapter, e.Topic);
                    SequentialSerializedPriceDTOMessageGenerator.Populate(e.Data, match.Data);
                    e.Data.TickDate = DateTime.Now;
                };

            priceListener.MessageReceived += (s, e) => Console.WriteLine(string.Format("xxxxx{0}.{1} {2}", e.DataAdapter, e.Topic, e.Data.ToStringWithValues()));

            var gate = new AutoResetEvent(false);
            priceListener.Start(0);

            gate.WaitOne(10000);

            priceListener.Stop();
            priceListener.Dispose();

        }

        [Test]
        public void HowToUseStreamingRecorder()
        {
            

            var listener = new TestStreamingListener<ApiLookupDTO>("DATAADAPTER", "TOPIC");

            listener.CreateMessage += (s, e) =>
            {
                e.Data.Description = "FOO";
            };

            var recorder = StreamingRecorder.Create(listener);
            recorder.Start();

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
            recorder.Stop();

            var messages = recorder.GetMessages();

            recorder.Dispose();
            
            listener.Dispose();

            if (exception != null)
            {
                Assert.Fail(exception.Message);
            }

            Assert.IsTrue(messages.Count > 0, "should be messages recorded");

        }
    }
}