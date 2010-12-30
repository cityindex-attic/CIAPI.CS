using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using TradingApi.Client.Core.ClientDTO;
using TradingApi.CoreDTO;

namespace TradingApi.Client.Core.UnitTests
{
    [TestClass]
    public class PriceHistoryFetcherTests
    {
        [TestMethod]
        public void FetchPriceHistoryReturnsCollectionOfPriceBars()
        {
            var historyRequest = new PriceHistoryRequest
                                     {
                                         MarketId = "-100",
                                         Interval = "DAY",
                                         Span = 1,
                                         NumberOfBars = 5
                                     };

            var expectedUrl = string.Format(
                "/market/{0}/barhistory?interval={1}&span={2}&pricebars={3}",
                historyRequest.MarketId, 
                historyRequest.Interval,
                historyRequest.Span, historyRequest.NumberOfBars);

            const string expectedResponseData = @"{""PartialPriceBar"":{""BarDate"":""\/Date(1283866477460)\/"",""Close"":10.0,""High"":15.0,""Low"":5.0,""Open"":9.0},""PriceBars"":[{""BarDate"":""\/Date(1283866477460)\/"",""Close"":10.0,""High"":15.0,""Low"":5.0,""Open"":9.0},{""BarDate"":""\/Date(1283866477460)\/"",""Close"":10.0,""High"":15.0,""Low"":5.0,""Open"":9.0},{""BarDate"":""\/Date(1283866477460)\/"",""Close"":10.0,""High"":15.0,""Low"":5.0,""Open"":9.0},{""BarDate"":""\/Date(1283866477460)\/"",""Close"":10.0,""High"":15.0,""Low"":5.0,""Open"":9.0},{""BarDate"":""\/Date(1283866477460)\/"",""Close"":10.0,""High"":15.0,""Low"":5.0,""Open"":9.0}]}";


            IWebClient mockWebClient = SetUpMockWebClientWithExpectedResponse(expectedUrl, expectedResponseData);
            var connectionWithMockWebClient = new Connection("john", "smith", "http://test.server/TradingAPI/",
                                                             "http://lightstreamer:8080", "CITYINDEXPRICING");
            connectionWithMockWebClient.WebClient = mockWebClient;
            var priceHistoryFetcher = new PriceHistoryFetcher(connectionWithMockWebClient);
            var priceHistoryBars = new List<PriceBarDTO>();
            priceHistoryFetcher.PriceHistoryResponse += (s, e) =>
            {
                priceHistoryBars = new List<PriceBarDTO>(e.GetPriceBarResponseDTO.PriceBars);
            };

            priceHistoryFetcher.BeginPriceHistoryRequest(historyRequest);

            mockWebClient.VerifyAllExpectations();

            Assert.AreEqual(5, priceHistoryBars.Count);
            Assert.AreEqual(9.0m, priceHistoryBars[0].Open);
            Assert.AreEqual(15.0m, priceHistoryBars[0].High);
            Assert.AreEqual(5.0m, priceHistoryBars[0].Low);
            Assert.AreEqual(10.0m, priceHistoryBars[0].Close);
        }

        private IWebClient SetUpMockWebClientWithExpectedResponse(string calledUrl, string expectedResponseData)
        {
            var mockWebClient = MockRepository.GenerateMock<IWebClient>();
            mockWebClient
                .Expect(x => x.BeginGetRequest(Arg<string>.Is.Equal(calledUrl), Arg<Dictionary<string,string>>.Is.Anything))
                .WhenCalled(x => mockWebClient.Raise(t => t.Response += (s, e) =>{}, this,
                                                     new WebResponseEventArgs {ResponseData = expectedResponseData}));
            return mockWebClient;
        }
    }
}