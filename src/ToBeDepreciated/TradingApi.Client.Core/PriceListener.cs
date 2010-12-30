using System;
using Lightstreamer.DotNet.Client;
using TradingApi.Client.Core.ClientDTO;
using TradingApi.Client.Core.Domain;
using TradingApi.Client.Core.Lightstreamer;
using Indices = TradingApi.Client.Core.Lightstreamer.LightstreamerDictionaryIndices;

namespace TradingApi.Client.Core
{
    public class PriceListener : LightstreamerListener
    {
        public PriceListener(int marketId, ILightstreamerConnection connection):
            this("PRICE.", marketId, connection){}

        public PriceListener(string priceItemPrefix, int marketId, ILightstreamerConnection connection):base(connection)
        {
            MarketId = marketId;
            ItemPrefix = priceItemPrefix;
        }
        protected int MarketId { get; private set; }
        protected string ItemPrefix { get; private set; }

        public event EventHandler<LightstreamerEventArgs<PriceUpdate>> Update;
        protected internal override void OnUpdate(LightstreamerEventArgs<StreamingUpdate> e)
        {
            if (Update != null && !e.Item.Update.IsNull())
            {
                Update(this, ConvertStreamingUpdateToPriceUpdate(e));
            }
        }

        private LightstreamerEventArgs<PriceUpdate> ConvertStreamingUpdateToPriceUpdate(LightstreamerEventArgs<StreamingUpdate> args)
        {
            var update = new PriceUpdate();
            try
            {
                update.ItemName = args.Item.ItemName;
                update.ItemPosition = args.Item.ItemPosition;
                update.Update = args.Item.Update;

                var ud = args.Item.Update;

                update.Price = new Price
                   {
                       MarketId = ud.GetAsInt(Indices.Price.MarketId),
                       LastUpdateTime = ud.GetAsJSONDateTimeUtc(Indices.Price.LastUpdateTime),
                       Bid = ud.GetAsDecimal(Indices.Price.Bid),
                       Offer = ud.GetAsDecimal(Indices.Price.Offer),
                       Direction = ud.GetAsInt(Indices.Price.Direction),
                       Change = ud.GetAsDecimal(Indices.Price.Change)
                       
                       //AuditId = ud.GetAsString(Indices.Price.AuditId),
                       //Delta = ud.GetAsDecimal(Indices.Price.Delta),
                       //ImpliedVolatility = ud.GetAsDecimal(Indices.Price.ImpliedVolatility),
                       //LastUpdateTime = ud.GetAsDateTime(Indices.Price.LastUpdateTime),
                       //Indicative = ud.GetAsBool(Indices.Price.Indicative)
                   };
            }
            catch (Exception exception)
            {
                update.Price = new NullPrice();
                Connection.OnStatusChanged(
                    new StatusEventArgs
                        {
                            Status = string.Format(
                            "Exception: Unable to convert StreamingUpdate to PriceUpdate: {0}: {1}\r\n{2}", 
                            exception.GetType(), 
                            exception.Message, 
                            exception.StackTrace)
                        });
            }

            return new LightstreamerEventArgs<PriceUpdate>{ Item = update };
        }


        protected override SimpleTableInfo GetTableInfo()
        {
            return new SimpleTableInfo(
                ItemPrefix + MarketId.ToString(),
                "MERGE", "MarketId TickDate Price Bid Offer Direction Change",
                true)
                       {
                           DataAdapter = "PRICES"
                       };
        }
    }
}