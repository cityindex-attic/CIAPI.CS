using System.Text.RegularExpressions;
using CIAPI.DTO;
using CIAPI.StreamingClient;
using System.Linq;
using System;

namespace CIAPI.Streaming
{
    
    /// <summary>
    /// 
    /// </summary>
    public partial class LightstreamerClient
    {

        #region IStreamingClient Members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public IStreamingListener<NewsDTO> BuildNewsHeadlinesListener(string category)
        {

            var topic = Regex.Replace("NEWS.HEADLINES.{category}", "{category}", category.ToString());
            return BuildListener<NewsDTO>("STREAMINGALL", "MERGE", true, topic);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="marketIds"></param>
        /// <returns></returns>
        public IStreamingListener<PriceDTO> BuildPricesListener(int [] marketIds)
        {
            

          var topic = string.Join(" ", marketIds.Select(t => Regex.Replace("PRICES.PRICE.{marketIds}", "{marketIds}", t.ToString())).ToArray());
          return BuildListener<PriceDTO>("STREAMINGALL", "MERGE", true, topic);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IStreamingListener<QuoteDTO> BuildQuotesListener()
        {
            
            return BuildListener<QuoteDTO>("STREAMINGALL", "MERGE", true, "QUOTES.QUOTES");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IStreamingListener<ClientAccountMarginDTO> BuildClientAccountMarginListener()
        {
            return BuildListener<ClientAccountMarginDTO>("STREAMINGALL", "MERGE", true, "CLIENTACCOUNTMARGIN.CLIENTACCOUNTMARGIN");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IStreamingListener<OrderDTO> BuildOrdersListener()
        {
            return BuildListener<OrderDTO>("STREAMINGALL", "MERGE", true, "ORDERS.ORDERS");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountOperatorId"></param>
        /// <returns></returns>
        public IStreamingListener<PriceDTO> BuildDefaultPricesListener(int accountOperatorId)
        {
            return BuildListener<PriceDTO>("CITYINDEXSTREAMINGDEFAULTPRICES", "MERGE", true, "PRICES.AC"+ accountOperatorId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IStreamingListener<TradeMarginDTO> BuildTradeMarginListener()
        {

            return BuildListener<TradeMarginDTO>("STREAMINGALL", "RAW", false, "TRADEMARGIN.TRADEMARGIN");
        }

        #endregion
    }
}
