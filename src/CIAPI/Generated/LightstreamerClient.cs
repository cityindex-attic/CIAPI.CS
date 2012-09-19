using System.Text.RegularExpressions;
using CIAPI.DTO;
using CIAPI.StreamingClient;
using System.Linq;
using System;

namespace CIAPI.Streaming
{
    
    public partial class LightstreamerClient
    {
        #region IStreamingClient Members

        public IStreamingListener<NewsDTO> BuildNewsHeadlinesListener(string category)
        {
            var topic = Regex.Replace("NEWS.HEADLINES.{category}", "{category}", category.ToString());
            return BuildListener<NewsDTO>("CITYINDEXSTREAMING", "MERGE", true, topic);
        }

        public IStreamingListener<PriceDTO> BuildPricesListener(int [] marketIds)
        {
          var topic = string.Join(" ", marketIds.Select(t => Regex.Replace("PRICES.PRICE.{marketIds}", "{marketIds}", t.ToString())).ToArray());
          return BuildListener<PriceDTO>("CITYINDEXSTREAMING", "MERGE", true, topic);
        }

        public IStreamingListener<QuoteDTO> BuildQuotesListener()
        {
            string topic = "QUOTES";
            return BuildListener<QuoteDTO>("STREAMINGTRADINGACCOUNT", "MERGE", true, topic);
        }

        public IStreamingListener<ClientAccountMarginDTO> BuildClientAccountMarginListener()
        {
            string topic = "CLIENTACCOUNTMARGIN";
            return BuildListener<ClientAccountMarginDTO>("STREAMINGCLIENTACCOUNT", "MERGE", true, topic);
        }

        public IStreamingListener<OrderDTO> BuildOrdersListener()
        {
            string topic = "ORDERS";
            return BuildListener<OrderDTO>("STREAMINGCLIENTACCOUNT", "MERGE", true, topic);
        }

        public IStreamingListener<PriceDTO> BuildDefaultPricesListener(int accountOperatorId)
        {
            return BuildListener<PriceDTO>("CITYINDEXSTREAMINGDEFAULTPRICES", "MERGE", true, "PRICES.AC"+ accountOperatorId);
        }

        public IStreamingListener<TradeMarginDTO> BuildTradeMarginListener()
        {
            return BuildListener<TradeMarginDTO>("STREAMINGCLIENTACCOUNT", "RAW", false, "TRADEMARGIN.ALL" );
        }

        #endregion
    }
}
