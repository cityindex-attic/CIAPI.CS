using System;
using CIAPI.DTO;
using CIAPI.StreamingClient;

namespace CIAPI.Streaming
{
    /// <summary>
    /// 
    /// </summary>
    public interface IStreamingClient : IDisposable 
    {
        //#TODO: need a shutdown method that closes all listeners on RPC client logoff/dispose

        //event EventHandler<MessageEventArgs<object>> MessageReceived;
        /// <summary>
        /// 
        /// </summary>
        event EventHandler<StatusEventArgs> StatusChanged;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="marketIds"></param>
        /// <returns></returns>
        IStreamingListener<PriceDTO> BuildPricesListener(params int[] marketIds);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        IStreamingListener<NewsDTO> BuildNewsHeadlinesListener(string category);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IStreamingListener<QuoteDTO> BuildQuotesListener();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IStreamingListener<ClientAccountMarginDTO> BuildClientAccountMarginListener();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IStreamingListener<OrderDTO> BuildOrdersListener();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountOperatorId"></param>
        /// <returns></returns>
        IStreamingListener<PriceDTO> BuildDefaultPricesListener(int accountOperatorId);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IStreamingListener<TradeMarginDTO> BuildTradeMarginListener();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="listener"></param>
        void TearDownListener(IStreamingListener listener);
    }
}