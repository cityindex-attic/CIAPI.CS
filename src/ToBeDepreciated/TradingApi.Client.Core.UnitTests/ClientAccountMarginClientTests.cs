using NUnit.Framework;

using Rhino.Mocks;
using TradingApi.Client.Core.ClientDTO;
using TradingApi.Client.Core.Domain;
using TradingApi.Client.Core.UnitTests.Domain;

namespace TradingApi.Client.Core.UnitTests
{
    [TestFixture]
    [TestFixture]
    public class ClientAccountMarginClientTests
    {
        [Test]
        public void StreamingUpdateTriggersUpdateEvent()
        {
            var marginFromEvent = new ClientAccountMargin();
            var expectedMargin = new ClientAccountMarginBuilder().Build();

            //Setup a PricingClient listening to a dummy market Id
            var clientAccountMarginClient = new ClientAccountMarginListener(12345,MockRepository.GenerateMock<Lightstreamer.ILightstreamerConnection>());

            //Trap the Price given by the update event for checking
            clientAccountMarginClient.Update += (s, e) =>
                                               {
                                                   marginFromEvent = e.Item.ClientAccountMargin;
                                               };

            //Trigger StreamingUpdateEvent using a mock streaming update
            //var mockStreamingUpdate = new ClientAccountMarginBuilder().CreateMockStreamingUpdateForClientAccountMargin(expectedMargin);
            //clientAccountMarginClient.OnUpdate(mockStreamingUpdate);

            //Ensure the correct price was fetched from the Update
            //mockStreamingUpdate.Item.Update.VerifyAllExpectations();
            //Assert.AreEqual(expectedMargin.ToString(), marginFromEvent.ToString());
        }
    }
}