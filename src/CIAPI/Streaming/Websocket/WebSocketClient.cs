using System;
using System.Text.RegularExpressions;
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



        public IStreamingListener<NewsDTO> BuildNewsHeadlinesListener(string category)
        {
            string topic = Regex.Replace("NEWS.HEADLINES.{category}", "{category}", category);
            return BuildListener<NewsDTO>(topic); // regex validator dummied for now
        }

        public IStreamingListener<PriceDTO> BuildPricesListener(string[] marketIds)
        {
            throw new NotImplementedException();
        }

        public IStreamingListener<ClientAccountMarginDTO> BuildClientAccountMarginListener()
        {
            string topic = "ALL";
            return BuildListener<ClientAccountMarginDTO>(topic); // regex validator dummied for now
        }

        public IStreamingListener<QuoteDTO> BuildQuotesListener()
        {
            string topic = "QUOTE.ALL";
            return BuildListener<QuoteDTO>(topic); // regex validator dummied for now
        }
    }

}