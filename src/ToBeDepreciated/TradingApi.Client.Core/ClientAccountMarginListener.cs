using System;
using Lightstreamer.DotNet.Client;
using TradingApi.Client.Core.ClientDTO;
using TradingApi.Client.Core.Domain;
using TradingApi.Client.Core.Lightstreamer;

using Indices = TradingApi.Client.Core.Lightstreamer.LightstreamerDictionaryIndices;

namespace TradingApi.Client.Core
{
    public class ClientAccountMarginListener : LightstreamerListener
    {
        public ClientAccountMarginListener(int clientAccountId,ILightstreamerConnection connection) : base(connection)
        {
            ClientAccountId = clientAccountId;
        }

        protected int ClientAccountId { get; private set; }

        public event EventHandler<LightstreamerEventArgs<ClientAccountMarginUpdate>> Update;

        protected internal override void OnUpdate(LightstreamerEventArgs<StreamingUpdate> e)
        {
            if (Update != null && !e.Item.Update.IsNull())
            {
                Update(this, ConvertStreamingUpdateToClientAccountMarginUpdate(e));
            }
        }

        private LightstreamerEventArgs<ClientAccountMarginUpdate> ConvertStreamingUpdateToClientAccountMarginUpdate(LightstreamerEventArgs<StreamingUpdate> args)
        {
            var update = new ClientAccountMarginUpdate();
            try
            {
                //update.ItemName = args.Item.ItemName;
                update.ItemName = "Testing";
                update.ItemPosition = args.Item.ItemPosition;
                update.Update = args.Item.Update;

                var ud = args.Item.Update;

                update.ClientAccountMargin = new ClientAccountMargin
                 {
                     MarginIndicator = Decimal.Round(ud.GetAsDecimal(Indices.ClientAccountMargin.MarginIndicator),4),
                     Cash = ud.GetAsDecimal(Indices.ClientAccountMargin.Cash)
                     //CreditAllocation = ud.GetAsDecimal(Indices.ClientAccountMargin.CreditAllocation),
                     //WaivedMarginRequirement = ud.GetAsDecimal(Indices.ClientAccountMargin.WaivedMarginRequirement),
                     //OpenTradeEquity = ud.GetAsDecimal(Indices.ClientAccountMargin.OpenTradeEquity),
                     //TradingResource = ud.GetAsDecimal(Indices.ClientAccountMargin.TradingResource),
                     //NetEquity = ud.GetAsDecimal(Indices.ClientAccountMargin.NetEquity),
                     //Margin = ud.GetAsDecimal(Indices.ClientAccountMargin.Margin),
                     //TradableFunds = ud.GetAsDecimal(Indices.ClientAccountMargin.TradableFunds),
                     //TotalMarginRequirement = ud.GetAsDecimal(Indices.ClientAccountMargin.TotalMarginRequirement),
                     //CurrencyISOCode = ud.GetAsString(Indices.ClientAccountMargin.CurrencyISOCode)
                 };
            }
            catch (Exception exception)
            {
                update.ClientAccountMargin = new NullClientAccountMargin();
                Connection.OnStatusChanged(new StatusEventArgs { Status = string.Format("Exception: Unable to convert StreamingUpdate to ClientAccountMarginUpdate: {0}: {1}\r\n{2}", exception.GetType(), exception.Message, exception.StackTrace) });
            }

            return new LightstreamerEventArgs<ClientAccountMarginUpdate> { Item = update };
        }

        protected override SimpleTableInfo GetTableInfo()
        {
            return new SimpleTableInfo(
                ClientAccountId.ToString(),
                "DISTINCT",
                "MarginIndicator Cash",
                true)
                       {
                           DataAdapter = "CLIENTACCOUNTMARGIN"
                       };

        }
    }
}