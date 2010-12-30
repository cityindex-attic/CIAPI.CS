using System;
using Rhino.Mocks;
using TradingApi.Client.Core.Domain;
using TradingApi.Client.Core.Lightstreamer;
using TradingApi.Client.Core.Lightstreamer.LightstreamerDictionaryIndices;
using Indices = TradingApi.Client.Core.Lightstreamer.LightstreamerDictionaryIndices;
using Price = TradingApi.Client.Core.ClientDTO.Price;

namespace TradingApi.Client.Core.UnitTests.Domain
{
    public class PriceBuilder
    {
        public int MarketId = 12345;
        public decimal Bid = 10.5m;
        public decimal Offer = 10.6m;
        public int Direction = 1;
        public decimal Change = 0.3m;
        public string AuditId = "123456-G2DEV-7890";
        public decimal Delta = 0.3m;
        public decimal ImpliedVolatility = 0.1m;
        /// <summary>
        /// Default: 2010-02-22 09:48:44
        /// </summary>
        public DateTime LastUpdateTime = DateTime.FromFileTime(129113057243959686);
        public bool Indicative = true;

        public Price Build()
        {
            return new Price
            {
                MarketId = MarketId,
                Bid = Bid,
                Offer = Offer,
                Direction = Direction,
                Change = Change,
                AuditId = AuditId,
                Delta = Delta,
                ImpliedVolatility = ImpliedVolatility,
                LastUpdateTime = LastUpdateTime, 
                Indicative = Indicative
            };
        }

        public LightstreamerEventArgs<StreamingUpdate> CreateMockStreamingUpdateForPrice(Price price)
        {
            var args = new LightstreamerEventArgs<StreamingUpdate>();
            args.Item = new StreamingUpdate();
            args.Item.ItemName = string.Format("price.{0}", price.MarketId);
            args.Item.ItemPosition = 1;
            args.Item.Update = MockRepository.GenerateMock<UpdateDetails>();
            args.Item.Update.Expect(x => x.GetAsLong(Indices.Price.MarketId)).Return(price.MarketId);
            args.Item.Update.Expect(x => x.GetAsJSONDateTimeUtc(Indices.Price.LastUpdateTime)).Return(price.LastUpdateTime);
            args.Item.Update.Expect(x => x.GetAsDecimal(Indices.Price.Bid)).Return(price.Bid);
            args.Item.Update.Expect(x => x.GetAsDecimal(Indices.Price.Offer)).Return(price.Offer);
            args.Item.Update.Expect(x => x.GetAsInt(Indices.Price.Direction)).Return(price.Direction);
            args.Item.Update.Expect(x => x.GetAsDecimal(Indices.Price.Change)).Return(price.Change);
//            args.Item.Update.Expect(x => x.GetAsString(Indices.Price.AuditId)).Return(price.AuditId);
//            args.Item.Update.Expect(x => x.GetAsDecimal(Indices.Price.Delta)).Return(price.Delta);
//            args.Item.Update.Expect(x => x.GetAsDecimal(Indices.Price.ImpliedVolatility)).Return(price.ImpliedVolatility);
//            args.Item.Update.Expect(x => x.GetAsDateTime(Indices.Price.LastUpdateTime)).Return(price.LastUpdateTime);
//            args.Item.Update.Expect(x => x.GetAsBool(Indices.Price.Indicative)).Return(price.Indicative);
            return args;
        }
    }
}
