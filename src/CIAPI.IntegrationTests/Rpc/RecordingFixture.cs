using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using CIAPI.DTO;
using CIAPI.IntegrationTests.Streaming;
using CIAPI.Rpc;
using CIAPI.Serialization;
using NUnit.Framework;
using Salient.ReliableHttpClient;

using Salient.ReliableHttpClient.Testing;

namespace CIAPI.IntegrationTests.Rpc
{
    [TestFixture]
    public class RecordingFixture : RpcFixtureBase
    {
        [Test]
        public void HowToUseRecorderWithStream()
        {
            var rpcClient = new Client(Settings.RpcUri, Settings.StreamingUri, AppKey);

            // start recording requests
            var stream = new MemoryStream();
            var streamRecorder = new StreamRecorder(rpcClient, stream);
            // open an array on the stream
            streamRecorder.Start();

            rpcClient.LogIn(Settings.RpcUserName, Settings.RpcPassword);





            // get some headlines
            var headlines = rpcClient.News.ListNewsHeadlinesWithSource("dj", "UK", 100);

            // get a story id from one of the headlines
            var storyId = headlines.Headlines[0].StoryId;

            // get the body of the story
            // the api team has yet again broken an established and published interface with yet another
            // half baked change. if you are going to require a key to retrieve detail you need to provide
            // the key on the master. again, the client is required to maintain corellation information (i.e. market type)
            // not impressed.
            var storyDetail = rpcClient.News.GetNewsDetail("dj", storyId.ToString());

            Assert.IsNotNullOrEmpty(storyDetail.NewsDetail.Story, "story was empty?");


            rpcClient.LogOut();

            // close the array in the stream
            streamRecorder.Stop();
            stream.Position = 0;

            var output = Encoding.UTF8.GetString(stream.ToArray());

            List<RequestInfoBase> requests = rpcClient.Serializer.DeserializeObject<List<RequestInfoBase>>(output);
            Assert.IsTrue(output.Contains("\"Target\": \"https://ciapi.cityindex.com/tradingapi/session\""));
        }

        [Test]
        public void HowToUseRecorder()
        {
            var rpcClient = new Client(Settings.RpcUri, Settings.StreamingUri, AppKey);

            // start recording requests
            var recorder = new Recorder(rpcClient);
            recorder.Start();

            rpcClient.LogIn(Settings.RpcUserName, Settings.RpcPassword);





            // get some headlines
            var headlines = rpcClient.News.ListNewsHeadlinesWithSource("dj", "UK", 100);

            // get a story id from one of the headlines
            var storyId = headlines.Headlines[0].StoryId;

            // get the body of the story
            // the api team has yet again broken an established and published interface with yet another
            // half baked change. if you are going to require a key to retrieve detail you need to provide
            // the key on the master. again, the client is required to maintain corellation information (i.e. market type)
            // not impressed.
            var storyDetail = rpcClient.News.GetNewsDetail("dj", storyId.ToString());

            Assert.IsNotNullOrEmpty(storyDetail.NewsDetail.Story, "story was empty?");


            rpcClient.LogOut();
            recorder.Stop();

            List<RequestInfoBase> requests = recorder.GetRequests();
            // let's serialize the recorded requests to simulate typical usage because you typically would use pre-canned data
            // to run unit tests agains.
            var requestsSerialized = rpcClient.Serializer.SerializeObject(requests);

            rpcClient.Dispose();





            // now we will use our recorded (and serialized) request data to run the same requests through the client 
            // without actually sending any requests over the wire.


            TestRequestFactory factory = new TestRequestFactory();

            rpcClient = new Client(Settings.RpcUri, Settings.StreamingUri, AppKey, new Serializer(), factory);
            rpcClient.IncludeIndexInHeaders = true;

            requests = rpcClient.Serializer.DeserializeObject<List<RequestInfoBase>>(requestsSerialized);
            var finder = new TestWebRequestFinder { Reference = requests };

            // setup a callback on the test request factory so that we can populate the response using the recorded data

            factory.PrepareResponse = testRequest =>
                {

                    // look for a matching request in our recording using the uri and request body
                    var match = finder.FindMatchExact(testRequest);

                    if (match == null)
                    {
                        throw new Exception("no matching request found");
                    }
                    finder.PopulateRequest(testRequest, match);
                };

            // now that our request stack is set up, we can make the same calls with repeatable results


            rpcClient.LogIn(Settings.RpcUserName, Settings.RpcPassword);

            // get some headlines
            headlines = rpcClient.News.ListNewsHeadlinesWithSource("dj", "UK", 100);

            // get a story id from one of the headlines
            storyId = headlines.Headlines[0].StoryId;

            storyDetail = rpcClient.News.GetNewsDetail("dj", storyId.ToString());

            Assert.IsNotNullOrEmpty(storyDetail.NewsDetail.Story, "story was empty?");


            rpcClient.LogOut();
            rpcClient.Dispose();
        }

        [Test]
        public void ReplaySerializedRequests()
        {
            var serialized = File.ReadAllText("RPC\\RecordedRequests01.txt");


            TestRequestFactory factory = new TestRequestFactory();

            var rpcClient = new Client(Settings.RpcUri, Settings.StreamingUri, AppKey, new Serializer(), factory);


            var requests = rpcClient.Serializer.DeserializeObject<List<RequestInfoBase>>(serialized);
            var finder = new TestWebRequestFinder { Reference = requests };

            // setup a callback on the test request factory so that we can populate the response using the recorded data

            factory.PrepareResponse = testRequest =>
            {

                // look for a matching request in our recording using the uri and request body
                var match = finder.FindMatchExact(testRequest);

                if (match == null)
                {
                    throw new Exception("no matching request found");
                }


                finder.PopulateRequest(testRequest, match);

            };

            // now that our request stack is set up, we can make the same calls with repeatable results


            rpcClient.LogIn(Settings.RpcUserName, Settings.RpcPassword);
            Assert.AreEqual("5f28983b-0e0a-4a57-92af-0d07c6fdbc38", rpcClient.Session);

            // get some headlines
            var headlines = rpcClient.News.ListNewsHeadlinesWithSource("dj", "UK", 100);
            Assert.AreEqual(100, headlines.Headlines.Length);

            // get a story id from one of the headlines
            var storyId = headlines.Headlines[0].StoryId;
            Assert.AreEqual(1416482, storyId);

            var storyDetail = rpcClient.News.GetNewsDetail("dj", storyId.ToString());

            Assert.IsTrue(storyDetail.NewsDetail.Story.Contains("By Anita Greil "));

            rpcClient.LogOut();


            rpcClient.Dispose();
        }

        [Test]
        public void ReplaySerializedRequestsByIndex()
        {
            var serialized = File.ReadAllText("RPC\\RecordedRequests02.txt");


            TestRequestFactory factory = new TestRequestFactory();

            var rpcClient = new Client(Settings.RpcUri, Settings.StreamingUri, AppKey, new Serializer(), factory);
            // adds an 'x-request-index' header to each request
            rpcClient.IncludeIndexInHeaders = true;

            var requests = rpcClient.Serializer.DeserializeObject<List<RequestInfoBase>>(serialized);
            var finder = new TestWebRequestFinder { Reference = requests };

            // setup a callback on the test request factory so that we can populate the response using the recorded data

            factory.PrepareResponse = testRequest =>
            {

                // look for a matching request in our recording using the uri and request body
                var match = finder.FindMatchBySingleHeader(testRequest, "x-request-index");

                if (match == null)
                {
                    throw new Exception("no matching request found");
                }

                finder.PopulateRequest(testRequest, match);
            };

            // now that our request stack is set up, we can make the same calls with repeatable results


            rpcClient.LogIn(Settings.RpcUserName, Settings.RpcPassword);
            Assert.AreEqual("ecbeff35-e5b7-4c15-bb2e-52232360f575", rpcClient.Session);

            // get some headlines
            var headlines = rpcClient.News.ListNewsHeadlinesWithSource("dj", "UK", 100);
            Assert.AreEqual(100, headlines.Headlines.Length);

            // get a story id from one of the headlines
            var storyId = headlines.Headlines[0].StoryId;
            Assert.AreEqual(1409880, storyId);

            var storyDetail = rpcClient.News.GetNewsDetail("dj", storyId.ToString());

            Assert.IsTrue(storyDetail.NewsDetail.Story.Contains("The latest official U.K. data release Thursday"));

            rpcClient.LogOut();


            rpcClient.Dispose();
        }
    }
}
