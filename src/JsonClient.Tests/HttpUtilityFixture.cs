using NUnit.Framework;

namespace CityIndex.JsonClient.Tests
{
    [TestFixture]
    public class HttpUtilityFixture
    {
        [Test]
        public void UrlEncodeBehavesLikeBaseClassImplementation()
        {
            const string aString = "http://server.com/a/path/with a space/?param=1&arg1=true";
            Assert.AreEqual(System.Web.HttpUtility.UrlEncode(aString), CityIndex.JsonClient.HttpUtility.UrlEncode(aString));
        }
    }
}
