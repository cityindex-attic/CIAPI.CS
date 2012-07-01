using NUnit.Framework;

namespace Salient.ReliableHttpClient.Serialization.Tests
{
    [TestFixture]
    public class SystemRuntimeCassiniSerializationFixture : CassiniSerializationFixture
    {
        public SystemRuntimeCassiniSerializationFixture()
        {
            _serializer = new SystemRuntime.Serializer();
        }

        private readonly IJsonSerializer _serializer;

        public override IJsonSerializer Serializer
        {
            get { return _serializer; }
        }
    }
}