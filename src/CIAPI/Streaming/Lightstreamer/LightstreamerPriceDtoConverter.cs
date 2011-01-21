using CIAPI.DTO;
using Lightstreamer.DotNet.Client;

namespace CIAPI.Streaming.Lightstreamer
{
    public class LightstreamerPriceDtoConverter : LightstreamerDtoConverter<PriceDTO>
    {
        public override PriceDTO Convert(object data)
        {
            var updateInfo = (UpdateInfo) data;
            return new PriceDTO
            {
                MarketId = GetAsInt(updateInfo, GetFieldIndex(typeof(PriceDTO).GetProperty("MarketId"))),
                TickDate = GetAsJSONDateTimeUtc(updateInfo, GetFieldIndex(typeof(PriceDTO).GetProperty("TickDate"))),
                Price = GetAsDecimal(updateInfo, GetFieldIndex(typeof(PriceDTO).GetProperty("Price"))),
                Bid = GetAsDecimal(updateInfo, GetFieldIndex(typeof(PriceDTO).GetProperty("Bid"))),
                Offer = GetAsDecimal(updateInfo, GetFieldIndex(typeof(PriceDTO).GetProperty("Offer"))),
                Direction = GetAsInt(updateInfo, GetFieldIndex(typeof(PriceDTO).GetProperty("Direction"))),
                Change = GetAsDecimal(updateInfo, GetFieldIndex(typeof(PriceDTO).GetProperty("Change"))),
            };
        }
    }
}