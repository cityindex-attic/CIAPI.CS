using System.Text.RegularExpressions;
using CIAPI.DTO;
using StreamingClient;
using System.Linq;
using System;

namespace CIAPI.Streaming.Lightstreamer
{
    
    public partial class LightstreamerClient
    {
        #region IStreamingClient Members

        public IStreamingListener<NewsDTO> BuildNewsHeadlinesListener(string category)
        {
            var topic = Regex.Replace("NEWS.HEADLINES.{category}", "{category}", category.ToString());
            return BuildListener<NewsDTO>("CITYINDEXSTREAMING",topic);
        }

        public IStreamingListener<PriceDTO> BuildPricesListener(int [] marketIds)
        {
          var topic = string.Join(" ", marketIds.Select(t => Regex.Replace("PRICES.PRICE.{marketIds}", "{marketIds}", t.ToString())).ToArray());
            return BuildListener<PriceDTO>("CITYINDEXSTREAMING",topic);
        }

        public IStreamingListener<QuoteDTO> BuildQuotesListener()
        {
            string topic = "QUOTES";
            return BuildListener<QuoteDTO>("STREAMINGTRADINGACCOUNT",topic);
        }

        public IStreamingListener<ClientAccountMarginDTO> BuildClientAccountMarginListener()
        {
            string topic = "CLIENTACCOUNTMARGIN";
            return BuildListener<ClientAccountMarginDTO>("STREAMINGCLIENTACCOUNT",topic);
        }

        public IStreamingListener<OrderDTO> BuildOrdersListener()
        {
            string topic = "ORDERS";
            return BuildListener<OrderDTO>("STREAMINGCLIENTACCOUNT",topic);
        }

        public IStreamingListener<PriceDTO> BuildDefaultPricesListener(int accountOperatorId)
        {
            return BuildListener<PriceDTO>("CITYINDEXSTREAMINGDEFAULTPRICES", "PRICES.AC"+ accountOperatorId);
        }

 

        #endregion
    }
}
