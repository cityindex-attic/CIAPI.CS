using CIAPI.DTO;
using StreamingClient;

namespace CIAPI.Streaming
{
    public interface IStreamingClient : StreamingClient.IStreamingClient
    {
        IStreamingListener<PriceDTO> BuildPriceListener(string topic);
        IStreamingListener<NewsDTO> BuildNewsHeadlinesListener(string topic);
        IStreamingListener<ClientAccountMarginDTO> BuildClientAccountMarginListener(string topic);
        IStreamingListener<QuoteDTO> BuildQuoteListener(string topic);
    }
}