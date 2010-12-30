using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Script.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace TradingApi.Client.Core.UnitTests
{
    [TestClass]
    public class OrderPlacerTests
    {
        private Settings _settings;
        private IWebClient _mockWebClient;

        [TestInitialize]
        public void SetUp()
        {
            _settings = new Settings { TradingAPIUrlBase = "http://myserver.com/TradingAPI/" };
            _mockWebClient = MockRepository.GenerateMock<IWebClient>();
        }

        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void WithInvalidCredentialsPlaceStopLimitOrderThrowsUnauthorizedAccessException()
        {
            var stopLimitOrderUrl = _settings.TradingAPIUrlBase + "/OrderService/PlaceStopLimitOrder";

            _mockWebClient.Expect(x => x.BeginGetRequest(stopLimitOrderUrl, new Dictionary<string, string>()))
                .Throw(new UnauthorizedAccessException());

            var orderPlacer = new OrderPlacer(_settings, _mockWebClient);

            orderPlacer.PlaceStopLimitOrder(new StopLimitOrderDTO());
        }

        [TestMethod]
        public void PlaceStopLimitOrderReturnsOrderConfirmationDto()
        {
            var responseData = "[OrderId=1001]";
            var stopLimitOrderUrl = _settings.TradingAPIUrlBase + "/OrderService/PlaceStopLimitOrder";

            _mockWebClient.Expect(x => x.BeginGetRequest(stopLimitOrderUrl, new Dictionary<string, string>()))
                       .WhenCalled(x => _mockWebClient.Raise(t => t.Response += null, this,
                       new WebResponseEventArgs { ResponseData = responseData }));

            var orderPlacer = new OrderPlacer(_settings, _mockWebClient);

            OrderConfirmationDTO recievedOrderConfirmationDto = null;

            orderPlacer.PlaceStopLimitOrderResponse += (object sender, OrderPlacerEventArgs orderConfirmationDto) =>
                                                           {
                                                               recievedOrderConfirmationDto = orderConfirmationDto.StopLimitOrder;
                                                           };

            orderPlacer.PlaceStopLimitOrder(new StopLimitOrderDTO());

            while (recievedOrderConfirmationDto==null)
            {
                System.Threading.Thread.Sleep(500);
            }

            Assert.AreEqual(recievedOrderConfirmationDto.OrderId, 1001);
        }       
    }

}
