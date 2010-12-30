using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradingApi.Client.Core.Lightstreamer;

namespace TradingApi.Client.Core.UnitTests
{
    [TestClass()]
    public class ConnectionTest
    {
        [TestMethod, Ignore]
        public void UsernamePasswordAndLightstreamerServerUrlUsedToInitialiseLightstreamerClient()
        {
            var connection = new Connection("joeblow", "mustard", "", @"http://lightstreamerserver:8080/", "CITYINDEXPRICING");
            Assert.IsNotNull(connection.LightstreamerConnection, "LightstreamerConnection not initialised");
            Assert.AreEqual(@"http://lightstreamerserver:8080/", connection.LightstreamerConnection.ServerUrl);
        }
    }
}
