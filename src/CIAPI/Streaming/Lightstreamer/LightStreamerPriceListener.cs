using CIAPI.DTO;
using Lightstreamer.DotNet.Client;

namespace CIAPI.Streaming.Lightstreamer
{
    public class LightStreamerPriceListener : LightStreamerListener<PriceDTO>
    {
        public LightStreamerPriceListener(string priceTopic, LSClient lsClient) :
            base(priceTopic, lsClient)
        {
        }

        protected override void BeforeStart()
        {
            MessageConverter = new LightstreamerPriceDtoConverter();
            TableInfo = new SimpleTableInfo(Topic.ToUpper(), "RAW", "MarketId TickDate Price Bid Offer Direction Change",
                                            false)
                            {
                                DataAdapter = "PRICES"
                            };
        }
    }
}