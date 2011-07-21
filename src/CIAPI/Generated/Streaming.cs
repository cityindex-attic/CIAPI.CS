using System.Text.RegularExpressions;
using CIAPI.DTO;
using StreamingClient;

namespace CIAPI.Streaming.Lightstreamer
{
    public partial class LightstreamerClient
    {
        #region IStreamingClient Members

        public IStreamingListener<NewsDTO> BuildNewsHeadlinesListener(string category)
        {
            string topic = Regex.Replace("NEWS.HEADLINES.{category}", "{category}", category);
            return BuildListener<NewsDTO>(topic);
        }

        public IStreamingListener<PriceDTO> BuildPricesListener(string[] marketIds)
        {
            string topic = Regex.Replace("PRICES.PRICE.{marketIds}", "{marketIds}", string.Join(" ", marketIds));
            return BuildListener<PriceDTO>(topic);
        }

        public IStreamingListener<ClientAccountMarginDTO> BuildClientAccountMarginListener()
        {
            string topic = "ALL";
            return BuildListener<ClientAccountMarginDTO>(topic);
        }

        public IStreamingListener<QuoteDTO> BuildQuotesListener()
        {
            string topic = "QUOTE.ALL";
            return BuildListener<QuoteDTO>(topic);
        }


        #endregion
    }
}
