﻿using System;
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
            return BuildListener<PriceDTO>(topic);
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