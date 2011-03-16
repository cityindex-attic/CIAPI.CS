using System;
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
            EnsureIsValidTopic(topic, "PRICES.");

            return BuildListener<PriceDTO>(topic);
        }

        public IStreamingListener<NewsDTO> BuildNewsHeadlinesListener(string topic)
        {
            EnsureIsValidTopic(topic, "NEWS.");

            return BuildListener<NewsDTO>(topic);
        }

        public IStreamingListener<ClientAccountMarginDTO> BuildClientAccountMarginListener(string topic)
        {
            EnsureIsValidTopic(topic, "CLIENTACCOUNT.");

            return BuildListener<ClientAccountMarginDTO>(topic);
        }

        public IStreamingListener<QuoteDTO> BuildQuoteListener(string topic)
        {
            EnsureIsValidTopic(topic, "QUOTE.");

            return BuildListener<QuoteDTO>(topic);
        }

        private static void EnsureIsValidTopic(string topic, string topictMustContain)
        {
            if (!topic.Contains(topictMustContain))
                throw new InvalidTopicException(string.Format("A price listener topic must contain {0}", topictMustContain));
        }
    }
}