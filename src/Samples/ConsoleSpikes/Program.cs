using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using CIAPI;
using CIAPI.Tests;
using Salient.ReliableHttpClient;
using CIAPI.DTO;


namespace ConsoleSpikes
{
 
    class Program
    {

        private const string AppKey = "testkey-for-ConsoleSpikes";

        static void Main(string[] args)
        {
     

            //RawJsonClient.SimpleRequest();

            //RawJsonClient.ParameterizedRequest();

            GetNewsSynchronously();

            GetNewsAsynchronously();
        }


        #region Async Implementation

        static CIAPI.Rpc.Client _ctx;
        private static ManualResetEvent _gate;

        private static void GetNewsAsynchronously()
        {
            _ctx = new CIAPI.Rpc.Client(new Uri(StaticTestConfig.RpcUrl), new Uri(StaticTestConfig.StreamingUrl), AppKey);

            _gate = new ManualResetEvent(false);
            BeginLogIn(StaticTestConfig.ApiUsername, StaticTestConfig.ApiPassword);

            _gate.WaitOne();



            Console.WriteLine("\r\nFinished.\r\nPress enter to continue\r\n");
            Console.ReadLine();
        }

        static void BeginLogIn(string userName, string password)
        {
            _ctx.BeginLogIn(userName, password, EndLoggedIn, null);
        }

        static void EndLoggedIn(ReliableAsyncResult result)
        {
            _ctx.EndLogIn(result);
            Console.WriteLine("\r\nLogged in.\r\n");

            BeginListNewsHeadlines("UK", 10);
        }

        static void BeginListNewsHeadlines(string category, int maxResults)
        {
            _ctx.News.BeginListNewsHeadlinesWithSource("dj", category, maxResults, EndListNewsHeadlines, null);
        }

        static void EndListNewsHeadlines(ReliableAsyncResult result)
        {
            var response = _ctx.News.EndListNewsHeadlinesWithSource(result);

            foreach (var item in response.Headlines)
            {
                Console.WriteLine("{0} {1} {2}\r\n", item.StoryId, item.Headline, item.PublishDate);
            }

            BeginLogOut();
        }


        static void BeginLogOut()
        {
            _ctx.BeginLogOut(EndLoggedOut, null);
        }

        static void EndLoggedOut(ReliableAsyncResult result)
        {
            _ctx.EndLogOut(result);
            Console.WriteLine("\r\nLogged out.\r\n");
            _gate.Set();
        }

        #endregion


        #region Sync implementation

        /// <summary>
        /// While the code is drastically simplified in comparison to the async pattern, you will typically
        /// want to do this on another thread and use BeingInvoke on the UI to marshal UI updates to the UI thread.
        /// This is probably the simplest pattern.
        /// </summary>
        private static void GetNewsSynchronously()
        {
            try
            {
                var ctx = new CIAPI.Rpc.Client(new Uri(StaticTestConfig.RpcUrl), new Uri(StaticTestConfig.StreamingUrl), AppKey);

                ctx.LogIn(StaticTestConfig.ApiUsername, StaticTestConfig.ApiPassword);

                var headlinesResponse = ctx.News.ListNewsHeadlinesWithSource("dj","UK", 10);

                foreach (var item in headlinesResponse.Headlines)
                {
                    // item contains id, date and headline.
                    Console.WriteLine("{0} {1} {2}\r\n", item.StoryId, item.Headline, item.PublishDate);

                    // fetch details to get all of the above and the body of the story
                    var detailResponse = ctx.News.GetNewsDetail("dj", item.StoryId.ToString());

                    Console.WriteLine("{0}", detailResponse.NewsDetail.Story.Substring(0, 35) + "...");
                    Console.WriteLine("\r\n-----------------------------------------------------------------------------\r\n");
                }

                ctx.LogOut();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.WriteLine("\r\nPress enter to continue\r\n");
                Console.ReadLine();
            }

        #endregion
        }
    }
}
