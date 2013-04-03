using System;
using CIAPI.DTO;
using CIAPI.StreamingClient;
using Salient.ReliableHttpClient.Serialization;

namespace CIAPI.Streaming.Testing
{
    

    /// <summary>
    /// 
    /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public Action<MessageEventArgs<NewsDTO>>  CreateNewsMessage;
        /// <summary>
        /// 
        /// </summary>
        public Action<MessageEventArgs<PriceDTO>> CreatePriceMessage;
        /// <summary>
        /// 
        /// </summary>
        public Action<MessageEventArgs<QuoteDTO>> CreateQuoteMessage;
        /// <summary>
        /// 
        /// </summary>
        public Action<MessageEventArgs<ClientAccountMarginDTO>> CreateClientAccountMarginMessage;
        /// <summary>
        /// 
        /// </summary>
        public Action<MessageEventArgs<OrderDTO>> CreateOrderMessage;
        /// <summary>
        /// 
        /// </summary>
        public Action<MessageEventArgs<TradeMarginDTO>> CreateTradeMarginMessage;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="streamingUri"></param>
        /// <param name="userName"></param>
        /// <param name="session"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public IStreamingClient Create(Uri streamingUri, string userName, string session, IJsonSerializer serializer)
        {

            return new TestStreamingClient(streamingUri, userName, session, serializer,this);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="streamingUri"></param>
        /// <param name="userName"></param>
        /// <param name="session"></param>
        /// <param name="usePolling"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public IStreamingClient Create(Uri streamingUri, string userName, string session, bool usePolling, IJsonSerializer serializer)
        {
            return new TestStreamingClient(streamingUri, userName, session, serializer, this);
        }
    }
}