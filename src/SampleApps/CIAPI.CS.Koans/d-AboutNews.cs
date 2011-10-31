using System;
using System.Threading;
using CIAPI.CS.Koans.KoanRunner;
using CIAPI.DTO;
using CityIndex.JsonClient;
using NUnit.Framework;
using Client = CIAPI.Rpc.Client;

namespace CIAPI.CS.Koans
{
    [KoanCategory(Order = 3)]
    public class AboutNews
    {
        private string USERNAME = "xx189949";
        private string PASSWORD = "password";

        [Koan(Order = 0)]
        public void UsingNewsRequiresAValidSession()
        {
            _rpcClient = new Rpc.Client(new Uri("https://ciapipreprod.cityindextest9.co.uk/TradingApi"));

            _rpcClient.LogIn(USERNAME, PASSWORD);

            KoanAssert.That(_rpcClient.Session, Is.Not.Null.Or.Empty, "after logging in, you should have a valid session");
        }

        [Koan(Order = 1)]
        public void YouCanFetchTheLatestNewsHeadlines()
        {
            const int numberOfHeadlines = 25;
            _ukHeadlines = _rpcClient.News.ListNewsHeadlines(category: "UK", maxResults: numberOfHeadlines);
            _ausHeadlines = _rpcClient.News.ListNewsHeadlines(category: "AUS", maxResults: numberOfHeadlines);

            KoanAssert.That(_ukHeadlines.Headlines.Length, Is.EqualTo(25), "You should get the number of headlines requested");

            //Each headline contains a StoryId, a Headline and a PublishDate
            KoanAssert.That(_ukHeadlines.Headlines[0].StoryId, Is.GreaterThan(0).And.LessThan(int.MaxValue), "StoryId is a positive integer");

            // sky: not sure about this one.. requires user to guess or get the error and then come back. is this intended.
            KoanAssert.That(_ukHeadlines.Headlines[0].Headline, Is.Not.Null.Or.Empty, "Headline is a short string");
            KoanAssert.That(_ukHeadlines.Headlines[0].PublishDate, Is.GreaterThan(new DateTime(2010, 12, 8)), "PublishDate is in UTC");
        }

        [Koan(Order = 3)]
        public void UsingTheStoryIdYouCanFetchTheStoryDetails()
        {
            var newsStory = _rpcClient.News.GetNewsDetail(_ukHeadlines.Headlines[0].StoryId.ToString());
            KoanAssert.That(newsStory.NewsDetail.Story, Is.Not.Null.Or.Empty, "You now have the full body of the news story");
            KoanAssert.That(newsStory.NewsDetail.Story, Is.StringContaining("<p>"), "which contains simple HTML");
        }

        [Koan(Order = 4)] 
        // DAVID: we have a serious issue regarding subsequent exceptions (i am working on it)- as it is you get one exception, pick one. 
        // I think the 'be diligent catching async errors' is more important.
        public void AskingForTooManyHeadlinesIsConsideredABadRequest()
        {
            try
            {
                const int maxHeadlines = 500;
                _ukHeadlines = _rpcClient.News.ListNewsHeadlines(category: "UK", maxResults: maxHeadlines + 1);
            }
            catch (Exception ex)
            {
                KoanAssert.That(ex.Message, Is.StringContaining("(400) Bad Request"), "The error will talk about (400) Bad Request");
            }
        }
 
        [Koan(Order = 5)]
        public void AskingForAnInvalidStoryIdWillGetYouNullStoryDetails()
        {
            const int invalidStoryId = Int32.MaxValue;
            var newsStory = _rpcClient.News.GetNewsDetail(storyId: invalidStoryId.ToString());

            KoanAssert.That(newsStory.NewsDetail, Is.EqualTo(null), "There are no details for an invalid story Id");
        }

       [Koan(Order = 6)]
        public void EveryRequestCanBeMadeAsyncronouslyToPreventHangingYourUIThread()
        {
            var gate = new ManualResetEvent(false);
            GetNewsDetailResponseDTO newsDetailResponseDto = null;

            _rpcClient.News.BeginGetNewsDetail(
                storyId: _ukHeadlines.Headlines[0].StoryId.ToString(),
                callback: (response) =>
                              {
                                  newsDetailResponseDto = _rpcClient.News.EndGetNewsDetail(response);
                                  gate.Set();
                              },
                state: null);

            //DoStuffInCurrentThreadyWhilstRequestHappensInBackground();

            gate.WaitOne(TimeSpan.FromSeconds(30)); //Never wait indefinately

            KoanAssert.That(newsDetailResponseDto.NewsDetail.Story, Is.StringContaining("<p>"), "You now have the full body of the news story");
        }

        private static void DoStuffInCurrentThreadyWhilstRequestHappensInBackground()
        {
            for (int i = 0; i < 10; i++)
            {
                Console.Write(".");
                new AutoResetEvent(false).WaitOne(100);
            }
            Console.WriteLine();
        }

        /// <summary>
        /// I am sure you took a swing at why we are getting an invalid json error when the server clearly returns a 400, no?
        /// This is obviously a threading issue.
        /// </summary>
       [Koan(Order = 7)]
        public void ExceptionsOnAsyncMethodsNeedToBeManagedCarefully()
        {
            const int maxHeadlines = 500;
            var gate = new ManualResetEvent(false);
            ListNewsHeadlinesResponseDTO listNewsHeadlinesResponseDto = null;

            _rpcClient.News.BeginListNewsHeadlines(
                category: "UK", maxResults: maxHeadlines + 1,
                callback: (response) =>
                {
                    try
                    {
                        listNewsHeadlinesResponseDto = _rpcClient.News.EndListNewsHeadlines(response);
                    }
                    catch (ApiException ex)
                    {
                        Console.WriteLine("ResponseTest: {0}", ex.ResponseText);
                        KoanAssert.That(ex.Message, Is.StringContaining("(400) Bad Request"), "The error will talk about (400) Bad Request");
                    }

                    gate.Set();
                },
                state: null);

            gate.WaitOne(TimeSpan.FromSeconds(60)); //Never wait indefinately
        }

        private Client _rpcClient;
        private ListNewsHeadlinesResponseDTO _ukHeadlines;
        private ListNewsHeadlinesResponseDTO _ausHeadlines;
        private string FILL_ME_IN = "replace FILL_ME_IN with the correct value";
        private DateTime FILL_ME_IN_DATE = DateTime.UtcNow;
    }
}
