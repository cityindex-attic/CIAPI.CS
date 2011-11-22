using CIAPI.IntegrationTests.Streaming;
using CIAPI.Rpc;
using NUnit.Framework;
using System.Text.RegularExpressions;

namespace CIAPI.IntegrationTests.Rpc
{
    [TestFixture]
    public class NewsFixture : RpcFixtureBase
    {
        [Test]
        public void HowToUseNews()
        {
            var rpcClient = BuildRpcClient();




            // get some headlines
            var headlines = rpcClient.News.ListNewsHeadlines("UK", 100);

            // get a story id from one of the headlines
            var storyId = headlines.Headlines[0].StoryId;

            // get the body of the story
            var storyDetail = rpcClient.News.GetNewsDetail(storyId.ToString());

            Assert.IsNotNullOrEmpty(storyDetail.NewsDetail.Story, "story was empty?");


            rpcClient.LogOut();
            rpcClient.Dispose();
        }
        [Test]
        public void CanListNewsHeadlinesWithSource()
        {
            var rpcClient = BuildRpcClient();




            // get some headlines
            var headlines = rpcClient.News.ListNewsHeadlinesWithSource("dj","UK", 100);

            // get a story id from one of the headlines
            var storyId = headlines.Headlines[0].StoryId;

            // get the body of the story
            var storyDetail = rpcClient.News.GetNewsDetail(storyId.ToString());

            Assert.IsNotNullOrEmpty(storyDetail.NewsDetail.Story, "story was empty?");


            rpcClient.LogOut();
            rpcClient.Dispose();
        }
    }

 
}