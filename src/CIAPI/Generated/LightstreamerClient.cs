using System.Text.RegularExpressions;
using CIAPI.DTO;
using StreamingClient;
using System.Linq;

namespace CIAPI.Streaming.Lightstreamer
{
    public partial class LightstreamerClient
    {
        #region IStreamingClient Members

        public IStreamingListener<NewsDTO> BuildNewsHeadlinesListener(string category)
        {
            var topic = Regex.Replace("NEWS.HEADLINES.{category}", "{category}", category);
            return BuildListener<NewsDTO>("CITYINDEXSTREAMING",topic);
        }

        public IStreamingListener<PriceDTO> BuildPricesListener(string [] marketIds)
        {
          var topic = string.Join(" ", marketIds.Select(t => Regex.Replace("PRICES.PRICE.{marketIds}", "{marketIds}", t)).ToArray());
            return BuildListener<PriceDTO>("CITYINDEXSTREAMING",topic);
        }

        protected override string[] GetAdapterList()
        {
            return new [] { "CITYINDEXSTREAMING" };
        }

        #endregion
    }
}
