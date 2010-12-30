using NUnit.Framework;
using TradingApi.Client.Core.Lightstreamer;

namespace TradingApi.Client.Core.UnitTests
{
    [TestFixture]
    public class ConnectionTest
    {
        [Test, Ignore]
        public void UsernamePasswordAndLightstreamerServerUrlUsedToInitialiseLightstreamerClient()
        {
            var connection = new Connection("joeblow", "mustard", "", @"http://lightstreamerserver:8080/", "CITYINDEXPRICING");
            Assert.IsNotNull(connection.LightstreamerConnection, "LightstreamerConnection not initialised");
            Assert.AreEqual(@"http://lightstreamerserver:8080/", connection.LightstreamerConnection.ServerUrl);
        }
    }
}
