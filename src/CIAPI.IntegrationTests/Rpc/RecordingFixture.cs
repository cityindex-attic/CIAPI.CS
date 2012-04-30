using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using CIAPI.DTO;
using CIAPI.IntegrationTests.Streaming;
using CIAPI.Rpc;
using NUnit.Framework;
using Salient.ReliableHttpClient;
using Salient.ReliableHttpClient.Serialization.Newtonsoft;
using Salient.ReliableHttpClient.Testing;

namespace CIAPI.IntegrationTests.Rpc
{
    [TestFixture,Ignore]
    public class RecordingFixture : RpcFixtureBase
    {
        [Test]
        public void HowToUseRecorder()
        {
            var rpcClient = new Client(Settings.RpcUri, Settings.StreamingUri, AppKey);

            // start recording requests

            rpcClient.StartRecording();

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
            rpcClient.StopRecording();

            List<RequestInfoBase> requests = rpcClient.GetRecording();
            var requestsSerialized = rpcClient.Serializer.SerializeObject(requests);
            rpcClient.Dispose();


            TestRequestFactory factory = new TestRequestFactory();
            rpcClient = new Client(Settings.RpcUri, Settings.StreamingUri, AppKey, new Serializer(), factory);

            requests = rpcClient.Serializer.DeserializeObject<List<RequestInfoBase>>(requestsSerialized);
            var finder = new TestWebRequestFinder();
            finder.Reference = requests;
            var requestIndex = 0;

            factory.PrepareResponse = request =>
            {

                var match = finder.FindMatchExact(request);

                if (match == null)
                {
                    throw new Exception("no matching request found");
                }

                // now set up the response...

                request.GetResponse();
#if DEBUG
                Debugger.Break();
#endif
            };

            rpcClient.LogIn(Settings.RpcUserName, Settings.RpcPassword);





            // get some headlines
            headlines = rpcClient.News.ListNewsHeadlinesWithSource("dj", "UK", 100);

            // get a story id from one of the headlines
            storyId = headlines.Headlines[0].StoryId;

            storyDetail = rpcClient.News.GetNewsDetail("dj", storyId.ToString());

            Assert.IsNotNullOrEmpty(storyDetail.NewsDetail.Story, "story was empty?");


            rpcClient.LogOut();
            rpcClient.StopRecording();

            rpcClient.Dispose();
        }
    }
}
