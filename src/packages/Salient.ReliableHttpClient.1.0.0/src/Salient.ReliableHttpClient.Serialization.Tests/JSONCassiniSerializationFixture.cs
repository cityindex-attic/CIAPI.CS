using NUnit.Framework;

namespace Salient.ReliableHttpClient.Serialization.Tests
{
    [TestFixture]
    public class JSONCassiniSerializationFixture : CassiniSerializationFixture
    {
        public JSONCassiniSerializationFixture()
        {
            _serializer = new JSON.Serializer();
        }

        private readonly IJsonSerializer _serializer;

        public override IJsonSerializer Serializer
        {
            get { return _serializer; }
        }
    }
}