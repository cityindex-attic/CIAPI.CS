using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CIAPI.Rpc;
using CIAPI.Serialization;
using NUnit.Framework;
using Salient.ReliableHttpClient;

namespace CassiniDev.Lib.Net35.Tests
{
    /// <summary>
    /// I really think this is the future of disconnected testing of CIAPI.
    /// 
    /// </summary>
    [TestFixture]
    public class CIAPIFixture : CassiniDevServer
    {
        List<RequestInfoBase> _requests;

        [TestFixtureSetUp]
        public void Start()
        {

            //doesn't matter, we are mocking the responses
            string content = Environment.CurrentDirectory;

            StartServer(content);

            SetupMockResponses();
        }

        
        [TestFixtureTearDown]
        public void Stop()
        {

            StopServer();
        }




        private void SetupMockResponses()
        {
            
            Server.ProcessRequest += ReturnMockResponse;

            var requestJson = File.ReadAllText("RecordedRequests01.txt");

            _requests = new Serializer().DeserializeObject<List<RequestInfoBase>>(requestJson);
        }


        void ReturnMockResponse(object sender, RequestInfoArgs e)
        {
            if (e.Url == "/session")
            {
                e.Response = _requests[0].ResponseText;
                e.Continue = false;
                return;
            }
            if (e.Url == "/news/dj/UK?MaxResults=100")
            {
                e.Response = _requests[1].ResponseText;
                e.Continue = false;
                return;
            }


            if (e.Url == "/news/dj/1416482")
            {
                e.Response = _requests[2].ResponseText;
                e.Continue = false;
                return;
            }

            if (e.Url == "/session/deleteSession?userName=Foo&session=5f28983b-0e0a-4a57-92af-0d07c6fdbc38")
            {
                e.Response = _requests[3].ResponseText;
                e.Continue = false;
                return;
            }



            throw new NotImplementedException("we are not expecting this request");
        }


        [Test]
        public void CanMockCiapiServerConversation()
        {

            Uri uri = new Uri(NormalizeUrl("/"));
            var rpcClient = new Client(uri, uri, "foobardotnet");


            rpcClient.LogIn("Foo", "Bar");

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
    }
}
