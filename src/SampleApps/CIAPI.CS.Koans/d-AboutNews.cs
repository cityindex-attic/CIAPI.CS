using System;
using System.Threading;
using CIAPI.CS.Koans.KoanRunner;
using CIAPI.DTO;
using CityIndex.JsonClient;
using NUnit.Framework;
using Client = CIAPI.Rpc.Client;

namespace CIAPI.CS.Koans
{
    [KoanCategory]
    public class AboutNews
    {
        private Client _rpcClient;
        private string USERNAME = "DM904310";
        private string PASSWORD = "password";
        private ListNewsHeadlinesResponseDTO _ukHeadlines;
        private ListNewsHeadlinesResponseDTO _ausHeadlines;

        [Koan]
        public void UsingNewsRequiresAValidSession()
        {
            _rpcClient = new Rpc.Client(new Uri("http://ciapipreprod.cityindextest9.co.uk/TradingApi"));
            
            _rpcClient.LogIn(USERNAME, PASSWORD);

            KoanAssert.That(_rpcClient.SessionId, Is.Not.Null.Or.Empty, "after logging in, you should have a valid session");
        }

        [Koan]
        public void YouCanFetchTheLatestNewsHeadlines()
        {
            const int numberOfHeadlines = 25;
            _ukHeadlines = _rpcClient.ListNewsHeadlines(category: "UK", maxResults: numberOfHeadlines);
            _ausHeadlines = _rpcClient.ListNewsHeadlines(category: "AUS", maxResults: numberOfHeadlines);

            KoanAssert.That(_ukHeadlines.Headlines.Length, Is.EqualTo(25), "You should get the number of headlines requested");

            //Each headline contains a StoryId, a Headline and a PublishDate
            KoanAssert.That(_ukHeadlines.Headlines[0].StoryId, Is.GreaterThan(0).And.LessThan(int.MaxValue), "StoryId is a positive integer");
            KoanAssert.That(_ukHeadlines.Headlines[0].Headline, Is.StringStarting("Swiss"), "Headline is a short string");
            KoanAssert.That(_ukHeadlines.Headlines[0].PublishDate, Is.GreaterThan(DateTime.UtcNow.AddYears(-1)), "PublishDate is in UTC");
        }

        [Koan]
        public void UsingTheStoryIdYouCanFetchTheStoryDetails()
        {
            var newsStory = _rpcClient.GetNewsDetail(_ukHeadlines.Headlines[0].StoryId.ToString());
            KoanAssert.That(newsStory.NewsDetail.Story, Is.Not.Null.Or.Empty, "You now have the full body of the news story");
            KoanAssert.That(newsStory.NewsDetail.Story, Is.StringContaining("<p>"), "which contains simple HTML");
        }

        [Koan]
        public void AskingForTooManyHeadlinesIsConsideredABadRequest()
        {
            try
            {
                const int maxHeadlines = 500;
                _ukHeadlines = _rpcClient.ListNewsHeadlines(category: "UK", maxResults: maxHeadlines+1);
            }
            catch (Exception ex)
            {
                KoanAssert.That(ex.Message, Is.StringContaining("(400)"), "The error will talk about (400) Bad Request");
            }
        }

        [Koan]
        public void AskingForAnInvalidStoryIdWillGetYouNullStoryDetails()
        {
            const int invalidStoryId = Int32.MaxValue;
            var newsStory = _rpcClient.GetNewsDetail(storyId: invalidStoryId.ToString());

            KoanAssert.That(newsStory.NewsDetail, Is.Null, "There are no details for an invalid story Id");
        }

        [Koan]
        public void EveryRequestCanBeMadeAsyncronouslyToPreventHangingYourUIThread()
        {
            var gate = new ManualResetEvent(false);
            GetNewsDetailResponseDTO newsDetailResponseDto = null;

            _rpcClient.BeginGetNewsDetail(
                storyId:_ukHeadlines.Headlines[0].StoryId.ToString(),
                callback: (response) =>
                              {
                                  newsDetailResponseDto = _rpcClient.EndGetNewsDetail(response);
                                  gate.Set();
                              }, 
                state: null);

            DoStuffInCurrentThreadyWhilstRequestHappensInBackground();

            gate.WaitOne(TimeSpan.FromSeconds(30)); //Never wait indefinately

            KoanAssert.That(newsDetailResponseDto.NewsDetail.Story, Is.Not.Empty, "You now have the full body of the news story");
        }

        private static void DoStuffInCurrentThreadyWhilstRequestHappensInBackground()
        {
            for (int i = 0; i < 10; i++)
            {
                Console.Write(".");
                Thread.Sleep(100);
            }
            Console.WriteLine();
        }

        [Koan]
        public void ExceptionsOnAsyncMethodsNeedToBeManagedCarefully()
        {
            const int maxHeadlines = 500;
            var gate = new ManualResetEvent(false);
            ListNewsHeadlinesResponseDTO listNewsHeadlinesResponseDto = null;

            _rpcClient.BeginListNewsHeadlines(
                category: "UK", maxResults: maxHeadlines + 1,
                callback: (response) =>
                {
                    try
                    {
                        listNewsHeadlinesResponseDto = _rpcClient.EndListNewsHeadlines(response);
                    }
                    catch (ApiException ex)
                    {
                        Console.WriteLine("ResponseTest: {0}", ex.ResponseText);
                        KoanAssert.That(ex.Message, Is.StringContaining("Invalid"), "The error will talk about (400) Bad Request");
                    }
                    
                    gate.Set();
                },
                state: null);

            gate.WaitOne(TimeSpan.FromSeconds(60)); //Never wait indefinately
        }
    }
}
