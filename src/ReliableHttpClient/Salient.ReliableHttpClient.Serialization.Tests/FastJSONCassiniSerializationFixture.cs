using NUnit.Framework;

namespace Salient.ReliableHttpClient.Serialization.Tests
{
    [TestFixture,Ignore("opaque json implementation")]
    public class FastJSONCassiniSerializationFixture : CassiniSerializationFixture
    {
        public FastJSONCassiniSerializationFixture()
        {
            _serializer = new Salient.ReliableHttpClient.Serialization.fastJSON.Serializer();
        }

        private readonly IJsonSerializer _serializer;

        public override IJsonSerializer Serializer
        {
            get { return _serializer; }
        }
    }
}