using CIAPI.DTO;
using Lightstreamer.DotNet.Client;

namespace CIAPI.Streaming.Lightstreamer
{
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