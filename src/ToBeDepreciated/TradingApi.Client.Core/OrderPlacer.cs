using System;
using System.Collections.Generic;
using System.Threading;
using TradingApi.Client.Core.UnitTests;

namespace TradingApi.Client.Core
{
    public class OrderPlacer
    {
        private readonly Settings _settings;
        private readonly IWebClient _webClient;

        public event EventHandler<OrderPlacerEventArgs> PlaceStopLimitOrderResponse;

        public OrderPlacer(Settings settings, IWebClient webClient)
        {
            _webClient = webClient;
            _settings = settings;
            _webClient.Response += OnWebClientResponse;
        }

        public void PlaceStopLimitOrder(StopLimitOrderDTO stopLimitOrderDTO)
        {
            _webClient.BeginGetRequest(_settings.TradingAPIUrlBase + "/OrderService/PlaceStopLimitOrder", new Dictionary<string, string>());
        }


        private void OnWebClientResponse(object s, WebResponseEventArgs webResponseEventArgs)
        {
            if (PlaceStopLimitOrderResponse == null) return;

            var orderConfirmation = new OrderConfirmationDTO();

            orderConfirmation.OrderId = 1001;

            PlaceStopLimitOrderResponse(s,
                                 new OrderPlacerEventArgs { StopLimitOrder = orderConfirmation });
        }
    
    
    
    }

    public class OrderPlacerEventArgs : EventArgs
    {
        public OrderConfirmationDTO StopLimitOrder { get; set; }
    }

    public class OrderConfirmationDTO
    {
        public int OrderId;
    }

    public class StopLimitOrderDTO
    {
    }
}