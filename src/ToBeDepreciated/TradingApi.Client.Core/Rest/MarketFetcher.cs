using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using TradingApi.CoreDTO;
using Microsoft.Http;
using RESTWebServicesDTO.Response;
using TradingApi.Client.Core.Markets;


namespace TradingApi.Client.Core
{
    public class MarketFetcher
    {
        private readonly Connection _currentConnection;

        public event EventHandler<MarketResponseEventArgs> PriceHistoryResponse;

        public MarketFetcher(Connection currentConnection)
        {
            _currentConnection = currentConnection;
        }

        //MarketName={searchByMarketName}&MarketCode={searchByMarketCode}&ClientAccountId={clientAccountId}&MaxResults={maxResults}
        public ListCfdMarketsResponseDTO CfdMarketRequest(string query, bool searchByMarketName, bool searchByMarketCode, int clientAccountId, int maxResults)
        {
            var url = string.Format("/cfd/markets?MarketName={0}&MarketCode={1}&ClientAccountId={2}&MaxResults={3}", searchByMarketName? query : "", searchByMarketCode? query : "", clientAccountId, maxResults);
            Dictionary<string, string> webHeaderCollection = new Dictionary<string, string>();
            webHeaderCollection.Add("UserName", _currentConnection.UserName);
            webHeaderCollection.Add("Session", _currentConnection.Session);
            var httpResponseMessage = _currentConnection.WebClient.GetRequest(url, webHeaderCollection);
            if(httpResponseMessage.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception(String.Format("Http Status Code {0} {1}", (int)httpResponseMessage.StatusCode, httpResponseMessage.StatusCode));
            }

            var markets = httpResponseMessage.Content.ReadAsJsonDataContract<ListCfdMarketsResponseDTO>();
            return markets;
        }

        public ListSpreadMarketsResponseDTO SpreadMarketRequest(string query, bool searchByMarketName, bool searchByMarketCode, int clientAccountId, int maxResults)
        {
            var url = string.Format("/spread/markets?MarketName={0}&MarketCode={1}&ClientAccountId={2}&MaxResults={3}", searchByMarketName ? query : "", searchByMarketCode ? query : "", clientAccountId, maxResults);
            Dictionary<string, string> webHeaderCollection = new Dictionary<string, string>();
            webHeaderCollection.Add("UserName", _currentConnection.UserName);
            webHeaderCollection.Add("Session", _currentConnection.Session);
            var httpResponseMessage = _currentConnection.WebClient.GetRequest(url, webHeaderCollection);
            if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception(String.Format("Http Status Code {0} {1}", (int)httpResponseMessage.StatusCode, httpResponseMessage.StatusCode));
            }

            var markets = httpResponseMessage.Content.ReadAsJsonDataContract<ListSpreadMarketsResponseDTO>();
            return markets;
        }
    }

    public class MarketResponseEventArgs : EventArgs
    {
        public List<MarketDTO> Markets { get; set; }
    }
}
