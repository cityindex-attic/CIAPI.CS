using CIAPI.DTO;
using StreamingClient;

namespace CIAPI.Streaming
{
    public interface IStreamingClient : StreamingClient.IStreamingClient
    {

        
        IStreamingListener<PriceDTO> BuildPricesListener(params string[] marketIds);
        IStreamingListener<NewsDTO> BuildNewsHeadlinesListener(string category);
        //IStreamingListener<ClientAccountMarginDTO> BuildClientAccountMarginListener();
        //IStreamingListener<QuoteDTO> BuildQuotesListener();
    }
}