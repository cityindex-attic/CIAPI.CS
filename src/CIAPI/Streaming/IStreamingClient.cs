using System;
using CIAPI.DTO;
using CIAPI.StreamingClient;

namespace CIAPI.Streaming
{
    public interface IStreamingClient : IDisposable 
    {
        //#TODO: need a shutdown method that closes all listeners on RPC client logoff/dispose

        //event EventHandler<MessageEventArgs<object>> MessageReceived;
        event EventHandler<StatusEventArgs> StatusChanged;
        IStreamingListener<PriceDTO> BuildPricesListener(params int[] marketIds);
        IStreamingListener<NewsDTO> BuildNewsHeadlinesListener(string category);
        IStreamingListener<QuoteDTO> BuildQuotesListener();

        IStreamingListener<ClientAccountMarginDTO> BuildClientAccountMarginListener();
        IStreamingListener<OrderDTO> BuildOrdersListener();
        IStreamingListener<PriceDTO> BuildDefaultPricesListener(int accountOperatorId);
        IStreamingListener<TradeMarginDTO> BuildTradeMarginListener();
        void TearDownListener(IStreamingListener listener);
    }
}