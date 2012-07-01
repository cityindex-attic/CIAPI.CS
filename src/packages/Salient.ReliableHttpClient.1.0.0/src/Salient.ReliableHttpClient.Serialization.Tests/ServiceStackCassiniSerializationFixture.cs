using NUnit.Framework;

namespace Salient.ReliableHttpClient.Serialization.Tests
{
    [TestFixture]
    public class ServiceStackCassiniSerializationFixture : CassiniSerializationFixture
    {
        public ServiceStackCassiniSerializationFixture()
        {
            _serializer = new ServiceStack.Serializer();
        }

        private readonly IJsonSerializer _serializer;

        public override IJsonSerializer Serializer
        {
            get { return _serializer; }
        }
    }
}