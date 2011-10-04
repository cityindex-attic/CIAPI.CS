using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIAPI.DTO;
using CIAPI.Rpc;
using NUnit.Framework;

namespace CIAPI.Tests
{
    [TestFixture]
    public class DTOTestFixture
    {

        [Test]
        public void EnsureApiStopLimitOrderDTOIsComposed()
        {

            var type = typeof (ApiStopLimitOrderDTO);
            var prop = type.GetProperty("TriggerPrice");
            Assert.IsNotNull(prop, "TriggerPrice is missing from ApiStopLimitOrderDTO");
            Assert.AreEqual(typeof(Decimal), prop.PropertyType, "ApiStopLimitOrderDTO.TriggerPrice is not decimal");
        }

        [Test]
        public void EnsureSaveMarketInformationHasParameter()
        {
            var type = typeof (Client);
            var member = type.GetProperty("Market");
            var method = member.PropertyType.GetMethod("SaveMarketInformation");
            Assert.IsNotNull(method, "SaveMarketInformation is missing");
            var parameters= method.GetParameters();

            Assert.AreEqual(1, parameters.Length, "SaveMarketInformation has no parameters");
            Assert.AreEqual(typeof(SaveMarketInformationRequestDTO), parameters[0].ParameterType, "SaveMarketInformation parameter type is incorrect");
        }
    }
}
