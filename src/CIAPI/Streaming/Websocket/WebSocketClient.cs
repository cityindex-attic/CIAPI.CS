using System;
using CIAPI.DTO;
using StreamingClient;
using StreamingClient.Websocket;

namespace CIAPI.Streaming.Websocket
{
    public class WebSocketClient: StompOverWebsocketClient, IStreamingClient
    {
        public WebSocketClient(Uri streamingUri, string userName, string sessionId)
            : base(streamingUri, userName, sessionId)
        {
        }

        public IStreamingListener<PriceDTO> BuildPriceListener(string topic)
        {
            return BuildListener<PriceDTO>(topic);
        }

        public IStreamingListener<PriceDTO> BuildPriceListener(string[] topics)
        {
            throw new NotImplementedException();
        }

        public IStreamingListener<NewsDTO> BuildNewsHeadlinesListener(string topic)
        {
            return BuildListener<NewsDTO>(topic);
        }

        public IStreamingListener<ClientAccountMarginDTO> BuildClientAccountMarginListener(string topic)
        {
            return BuildListener<ClientAccountMarginDTO>(topic);
        }

        public IStreamingListener<QuoteDTO> BuildQuoteListener(string topic)
        {
            return BuildListener<QuoteDTO>(topic);
        }
    }

}