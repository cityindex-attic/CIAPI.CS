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

 

        public IStreamingListener<PriceDTO> BuildPricesListener(string[] topics)
        {
            throw new NotImplementedException();
        }

        public IStreamingListener<NewsDTO> BuildNewsHeadlinesListener(string topic)
        {
            return BuildListener<NewsDTO>(topic);
        }

        public IStreamingListener<ClientAccountMarginDTO> BuildClientAccountMarginListener()
        {
            return BuildListener<ClientAccountMarginDTO>("");
        }

        public IStreamingListener<QuoteDTO> BuildQuotesListener()
        {
            return BuildListener<QuoteDTO>("");
        }
    }

}