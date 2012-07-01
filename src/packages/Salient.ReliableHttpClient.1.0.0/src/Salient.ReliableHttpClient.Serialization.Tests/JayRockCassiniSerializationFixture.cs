using NUnit.Framework;

namespace Salient.ReliableHttpClient.Serialization.Tests
{
    [TestFixture]
    public class JayRockCassiniSerializationFixture : CassiniSerializationFixture
    {
        public JayRockCassiniSerializationFixture()
        {
            _serializer = new Salient.ReliableHttpClient.Serialization.JayRock.Serializer();
        }

        private readonly IJsonSerializer _serializer;

        public override IJsonSerializer Serializer
        {
            get { return _serializer; }
        }
    }
}