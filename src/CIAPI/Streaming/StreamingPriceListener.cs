using CIAPI.DTO;
using Lightstreamer.DotNet.Client;

namespace CIAPI.Streaming
{
    public class StreamingPriceListener : StreamingListener<PriceDTO>
    {
        public StreamingPriceListener(string priceTopic, LSClient lsClient) :
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

    public class LightstreamerPriceDtoConverter : LightstreamerDtoConverterBase, IMessageConverter<PriceDTO>
    {
        public PriceDTO Convert(object data)
        {
            var updateInfo = (UpdateInfo) data;
            return new PriceDTO
                       {
                           MarketId = GetAsInt(updateInfo, 1),
                           TickDate = GetAsJSONDateTimeUtc(updateInfo, 2),
                           Price = GetAsDecimal(updateInfo, 3),
                           Bid = GetAsDecimal(updateInfo, 4),
                           Offer = GetAsDecimal(updateInfo, 5),
                           Direction = GetAsInt(updateInfo, 6),
                           Change = GetAsDecimal(updateInfo, 7),
                       };
        }
    }
}