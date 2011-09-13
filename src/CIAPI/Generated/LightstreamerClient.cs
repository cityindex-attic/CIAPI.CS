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
            const string validator = "(US|UK|ALL)";
            if (!Regex.IsMatch(category, validator))
            {
                throw new Exception("Invalid category:" + category + "\r\nMust match expression " + validator);
            }
            var topic = Regex.Replace("NEWS.HEADLINES.{category}", "{category}", category.ToString());
            return BuildListener<NewsDTO>("CITYINDEXSTREAMING",topic);
        }

        public IStreamingListener<PriceDTO> BuildPricesListener(int [] marketIds)
        {
          var topic = string.Join(" ", marketIds.Select(t => Regex.Replace("PRICES.PRICE.{marketIds}", "{marketIds}", t.ToString())).ToArray());
            return BuildListener<PriceDTO>("CITYINDEXSTREAMING",topic);
        }

        protected override string[] GetAdapterList()
        {
            return new [] { "CITYINDEXSTREAMING" };
        }

        #endregion
    }
}
