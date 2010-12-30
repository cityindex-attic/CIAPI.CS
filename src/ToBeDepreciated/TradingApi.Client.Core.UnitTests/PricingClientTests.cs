using NUnit.Framework;
using Rhino.Mocks;
using TradingApi.Client.Core.ClientDTO;
using TradingApi.Client.Core.Domain;
using TradingApi.Client.Core.Lightstreamer;
using TradingApi.Client.Core.UnitTests.Domain;

namespace TradingApi.Client.Core.UnitTests
{
    [TestFixture]
    public class PricingClientTests
    {
        [Test]
        public void StreamingUpdateTriggersUpdateEvent()
        {
            var priceFromEvent = new Price();
            var expectedPrice = new PriceBuilder { MarketId = 12322211 }.Build();

            //Setup a PricingClient listening to a dummy market Id
            var pricingAdapterClient = new PriceListener(expectedPrice.MarketId, MockRepository.GenerateMock<ILightstreamerConnection>());

            //Trap the Price given by the update event for checking
            pricingAdapterClient.Update += (s, e) =>
            {
                priceFromEvent = e.Item.Price;
            };

            //Trigger StreamingUpdateEvent using a mock streaming update
            //var mockStreamingUpdate = new PriceBuilder().CreateMockStreamingUpdateForPrice(expectedPrice);
            //pricingAdapterClient.OnUpdate(mockStreamingUpdate);

            //Ensure the correct price was fetched from the Update
            //mockStreamingUpdate.Item.Update.VerifyAllExpectations();
            //Assert.AreEqual(expectedPrice.ToString(), priceFromEvent.ToString());
        }
    }
}