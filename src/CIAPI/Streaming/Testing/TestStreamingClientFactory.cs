using System;
using CIAPI.DTO;
using CIAPI.StreamingClient;
using Salient.ReliableHttpClient.Serialization;

namespace CIAPI.Streaming.Testing
{
    

    public class TestStreamingClientFactory : IStreamingClientFactory
    {
        internal void OnCreateTradeMarginMessage(MessageEventArgs<TradeMarginDTO> e)
        {
            Action<MessageEventArgs<TradeMarginDTO>> handler = CreateTradeMarginMessage;
            if (handler != null)
            {
                handler(e);
            }
        }
        internal void OnCreateOrderMessage(MessageEventArgs<OrderDTO> e)
        {
            Action<MessageEventArgs<OrderDTO>> handler = CreateOrderMessage;
            if (handler != null)
            {
                handler(e);
            }
        }
        internal void OnCreateClientAccountMarginMessage(MessageEventArgs<ClientAccountMarginDTO> e)
        {
            Action<MessageEventArgs<ClientAccountMarginDTO>> handler = CreateClientAccountMarginMessage;
            if (handler != null)
            {
                handler(e);
            }
        }
        internal void OnCreateQuoteMessage(MessageEventArgs<QuoteDTO> e)
        {
            Action<MessageEventArgs<QuoteDTO>> handler = CreateQuoteMessage;
            if (handler != null)
            {
                handler(e);
            }
        }
        internal void OnCreatePriceMessage(MessageEventArgs<PriceDTO> e)
        {
            Action<MessageEventArgs<PriceDTO>> handler = CreatePriceMessage;
            if (handler != null)
            {
                handler(e);
            }
        }
        internal void OnCreateNewsMessage(MessageEventArgs<NewsDTO> e)
        {
            Action<MessageEventArgs<NewsDTO>> handler = CreateNewsMessage;
            if (handler != null)
            {
                handler(e);
            }
        }

        public Action<MessageEventArgs<NewsDTO>>  CreateNewsMessage;
        public Action<MessageEventArgs<PriceDTO>> CreatePriceMessage;
        public Action<MessageEventArgs<QuoteDTO>> CreateQuoteMessage;
        public Action<MessageEventArgs<ClientAccountMarginDTO>> CreateClientAccountMarginMessage;
        public Action<MessageEventArgs<OrderDTO>> CreateOrderMessage;
        public Action<MessageEventArgs<TradeMarginDTO>> CreateTradeMarginMessage;

        public IStreamingClient Create(Uri streamingUri, string userName, string session, IJsonSerializer serializer)
        {

            return new TestStreamingClient(streamingUri, userName, session, serializer,this);
        }
    }
}