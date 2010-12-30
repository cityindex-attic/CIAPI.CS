using System;
using Rhino.Mocks;
using TradingApi.Client.Core.Lightstreamer;
using TradingApi.Client.Core.Lightstreamer.LightstreamerDictionaryIndices;
using ClientAccountMargin = TradingApi.Client.Core.ClientDTO.ClientAccountMargin;
using Indices = TradingApi.Client.Core.Lightstreamer.LightstreamerDictionaryIndices;

namespace TradingApi.Client.Core.UnitTests
{
    public class ClientAccountMarginBuilder
    {
        public decimal MarginIndicator = 10.5m;
        public decimal Cash = 18.766m;
        public decimal CreditAllocation = 0.3m;
        public decimal WaivedMarginRequirement = 29876.27m;
        public decimal OpenTradeEquity = 12876.0m;
        public decimal TradingResource = 62566.33m;
        public decimal NetEquity = 625166.3643m;
        public decimal Margin = 253.2372735271m;
        public decimal TradableFunds = 37627.34m;
        public decimal TotalMarginRequirement = 18.12m;
        public string CurrencyISOCode = "GBP";

        public ClientAccountMargin Build()
        {
            return new ClientAccountMargin
            {
                MarginIndicator = MarginIndicator,
                Cash = Cash,
                CreditAllocation = CreditAllocation,
                WaivedMarginRequirement = WaivedMarginRequirement,
                OpenTradeEquity = OpenTradeEquity,
                TradingResource = TradingResource,
                NetEquity = NetEquity,
                Margin = Margin,
                TradableFunds = TradableFunds,
                TotalMarginRequirement = TotalMarginRequirement,
                CurrencyISOCode = CurrencyISOCode
            };
        }

        public LightstreamerEventArgs<StreamingUpdate> CreateMockStreamingUpdateForClientAccountMargin(ClientAccountMargin cam)
        {
            var args = new LightstreamerEventArgs<StreamingUpdate>();
            args.Item = new StreamingUpdate();
            args.Item.ItemName = string.Format("ClientAccountMargin.{0}", cam.Margin);
            args.Item.ItemPosition = 1;
            args.Item.Update = MockRepository.GenerateMock<UpdateDetails>();
            args.Item.Update.Expect(x => x.GetAsDecimal(Indices.ClientAccountMargin.MarginIndicator)).Return(cam.MarginIndicator);
            args.Item.Update.Expect(x => x.GetAsDecimal(Indices.ClientAccountMargin.Cash)).Return(cam.Cash);
            args.Item.Update.Expect(x => x.GetAsDecimal(Indices.ClientAccountMargin.CreditAllocation)).Return(cam.CreditAllocation);
            args.Item.Update.Expect(x => x.GetAsDecimal(Indices.ClientAccountMargin.WaivedMarginRequirement)).Return(cam.WaivedMarginRequirement);
            args.Item.Update.Expect(x => x.GetAsDecimal(Indices.ClientAccountMargin.OpenTradeEquity)).Return(cam.OpenTradeEquity);
            args.Item.Update.Expect(x => x.GetAsDecimal(Indices.ClientAccountMargin.TradingResource)).Return(cam.TradingResource);
            args.Item.Update.Expect(x => x.GetAsDecimal(Indices.ClientAccountMargin.NetEquity)).Return(cam.NetEquity);
            args.Item.Update.Expect(x => x.GetAsDecimal(Indices.ClientAccountMargin.Margin)).Return(cam.Margin);
            args.Item.Update.Expect(x => x.GetAsDecimal(Indices.ClientAccountMargin.TradableFunds)).Return(cam.TradableFunds);
            args.Item.Update.Expect(x => x.GetAsDecimal(Indices.ClientAccountMargin.TotalMarginRequirement)).Return(cam.TotalMarginRequirement);
            args.Item.Update.Expect(x => x.GetAsString(Indices.ClientAccountMargin.CurrencyISOCode)).Return(cam.CurrencyISOCode);
            return args;
        }
    }
}
