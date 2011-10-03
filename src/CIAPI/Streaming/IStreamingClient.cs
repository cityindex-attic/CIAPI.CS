using CIAPI.DTO;
using StreamingClient;

namespace CIAPI.Streaming
{
    public interface IStreamingClient : StreamingClient.IStreamingClient
    {

        
        IStreamingListener<PriceDTO> BuildPricesListener(params int[] marketIds);
        IStreamingListener<NewsDTO> BuildNewsHeadlinesListener(string category);
        IStreamingListener<QuoteDTO> BuildQuotesListener();

        IStreamingListener<ClientAccountMarginDTO> BuildClientAccountMarginListener();
        IStreamingListener<OrderDTO> BuildOrdersListener();
        IStreamingListener<PriceDTO> BuildDefaultPricesListener();
    }
}