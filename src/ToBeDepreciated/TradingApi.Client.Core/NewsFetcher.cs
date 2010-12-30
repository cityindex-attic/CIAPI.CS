using System;
using System.Net;
using System.Runtime.Serialization.Json;
using log4net;
using RESTWebServicesDTO.Response;

namespace TradingApi.Client.Core
{
    public class NewsFetcher
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(NewsFetcher));
        private readonly Connection _currentConnection;

        public NewsFetcher(Connection currentConnection)
        {
            _currentConnection = currentConnection;
        }

        public ListNewsHeadlinesResponseDTO ListNewsHeadlines(string category, int maxResults)
        {
            var url = string.Format("/news?Category={0}&MaxResults={1}", category, maxResults);
            var httpResponseMessage = _currentConnection.Get(url);
            if(httpResponseMessage.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception(String.Format("Http Status Code {0} {1}", (int)httpResponseMessage.StatusCode, httpResponseMessage.StatusCode));
            }

            var news = httpResponseMessage.Content.ReadAsJsonDataContract<ListNewsHeadlinesResponseDTO>();
            return news;
        }

        public GetNewsDetailResponseDTO GetNewsDetail(int storyId)
        {
            var url = string.Format("/news/{0}", storyId);
            var httpResponseMessage = _currentConnection.Get(url);
            if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception(String.Format("Http Status Code {0} {1}", (int)httpResponseMessage.StatusCode, httpResponseMessage.StatusCode));
            }

            var newsDetail = httpResponseMessage.Content.ReadAsJsonDataContract<GetNewsDetailResponseDTO>();
            return newsDetail;
        }
    }
}
