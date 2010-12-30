using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.Http;
using RESTWebServicesDTO.Response;
using System.Runtime.Serialization.Json;
using TradingApi.Client.Core.ClientDTO;

namespace TradingApi.Client.Core
{
    public class PriceHistoryFetcher
    {
        private readonly Connection _currentConnection;

        public event EventHandler<PriceHistoryResponseEventArgs> PriceHistoryResponse;
        
        public PriceHistoryFetcher(Connection currentConnection)
        {
            _currentConnection = currentConnection;
        }

        public GetPriceBarResponseDTO GetPriceHistoryBars(PriceHistoryRequest request)
        {
            var url = string.Format("/market/{0}/barhistory?interval={1}&span={2}&pricebars={3}",
                                  request.MarketId, request.Interval,
                                  request.Span, request.NumberOfBars);
            var httpResponseMessage = _currentConnection.Get(url);
            if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception(String.Format("Http Status Code {0} {1}", (int)httpResponseMessage.StatusCode, httpResponseMessage.StatusCode));
            }

            return httpResponseMessage.Content.ReadAsJsonDataContract<GetPriceBarResponseDTO>();
        }

        public GetPriceTickResponseDTO GetPriceHistoryTicks(PriceHistoryRequest request)
        {
            var url = string.Format("/market/{0}/tickhistory?priceticks={1}",
                                   request.MarketId, request.NumberOfBars);
            var httpResponseMessage = _currentConnection.Get(url);
            if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception(String.Format("Http Status Code {0} {1}", (int)httpResponseMessage.StatusCode, httpResponseMessage.StatusCode));
            }

            return httpResponseMessage.Content.ReadAsJsonDataContract<GetPriceTickResponseDTO>();
        }

        public void BeginPriceHistoryRequest(PriceHistoryRequest request)
        {
            string url;

            if (request.Interval.ToUpper()=="TICKS")
            {
                url = string.Format("/market/{0}/tickhistory?priceticks={1}",
                                    request.MarketId, request.NumberOfBars);

                _currentConnection.WebClient.Response += OnWebClientTickResponse;    
            }
            else
            {
                url = string.Format("/market/{0}/barhistory?interval={1}&span={2}&pricebars={3}",
                                  request.MarketId, request.Interval,
                                  request.Span, request.NumberOfBars);

                _currentConnection.WebClient.Response += OnWebClientBarResponse;
            }
            
            _currentConnection.WebClient.BeginGetRequest(url, new Dictionary<string, string>());
        }

        private void OnWebClientBarResponse(object s, WebResponseEventArgs webResponseEventArgs)
        {
            if (PriceHistoryResponse == null) return;
            
            var resp = new HttpResponseMessage { Content = HttpContent.Create(webResponseEventArgs.ResponseData) };

            PriceHistoryResponse(s,
                                 new PriceHistoryResponseEventArgs
                                     {
                                         GetPriceBarResponseDTO = resp.Content.ReadAsJsonDataContract<GetPriceBarResponseDTO>()
                                     });
        }

        private void OnWebClientTickResponse(object s, WebResponseEventArgs webResponseEventArgs)
        {
            if (PriceHistoryResponse == null) return;

            var resp = new HttpResponseMessage { Content = HttpContent.Create(webResponseEventArgs.ResponseData) };
            PriceHistoryResponse(s,
                                new PriceHistoryResponseEventArgs
                                {
                                    GetPriceTickResponseDTO = resp.Content.ReadAsJsonDataContract<GetPriceTickResponseDTO>()
                                });
        }
    }

    public class PriceHistoryResponseEventArgs : EventArgs
    {
        public GetPriceBarResponseDTO GetPriceBarResponseDTO { get; set; }
        public GetPriceTickResponseDTO GetPriceTickResponseDTO { get; set; }
    }
}
