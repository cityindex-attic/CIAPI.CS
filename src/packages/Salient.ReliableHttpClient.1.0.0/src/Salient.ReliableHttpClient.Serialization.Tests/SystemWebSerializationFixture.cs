using NUnit.Framework;

namespace Salient.ReliableHttpClient.Serialization.Tests
{
    [TestFixture]
    public class SystemWebCassiniSerializationFixture : CassiniSerializationFixture
    {
        public SystemWebCassiniSerializationFixture()
        {
            _serializer = new SystemWeb.Serializer();
        }

        private readonly IJsonSerializer _serializer;

        public override IJsonSerializer Serializer
        {
            get { return _serializer; }
        }
    }
}