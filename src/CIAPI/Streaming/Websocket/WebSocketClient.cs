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

 

        public IStreamingListener<PriceDTO> BuildPricesListener(int[] marketIds)
        {
            throw new NotImplementedException();
        }

        public IStreamingListener<NewsDTO> BuildNewsHeadlinesListener(string category)
        {
            return BuildListener<NewsDTO>(category);
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