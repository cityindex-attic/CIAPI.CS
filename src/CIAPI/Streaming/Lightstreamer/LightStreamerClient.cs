using System;
using System.Text.RegularExpressions;
using CIAPI.DTO;
using StreamingClient;

namespace CIAPI.Streaming.Lightstreamer
{
    public class LightstreamerClient: StreamingClient.Lightstreamer.LightstreamerClient, IStreamingClient
    {
        public LightstreamerClient(Uri streamingUri, string userName, string sessionId) : base(streamingUri, userName, sessionId)
        {
        }

        public IStreamingListener<PriceDTO> BuildPriceListener(string topic)
        {
            return BuildListener<PriceDTO>(topic, new Regex(@"^PRICES\.PRICE\.(\d+)$"));
        }

        public IStreamingListener<NewsDTO> BuildNewsHeadlinesListener(string topic)
        {
            return BuildListener<NewsDTO>(topic, new Regex(@"^NEWS\.(\w+)\.(\w+)$"));
        }

        public IStreamingListener<ClientAccountMarginDTO> BuildClientAccountMarginListener(string topic)
        {
            return BuildListener<ClientAccountMarginDTO>(topic, new Regex(@"^ALL$"));
        }

        public IStreamingListener<QuoteDTO> BuildQuoteListener(string topic)
        {
            return BuildListener<QuoteDTO>(topic, new Regex(@"^QUOTE\.ALL$"));
        }
    }
}