using NUnit.Framework;

namespace Salient.ReliableHttpClient.Serialization.Tests
{
    [TestFixture]
    public class NewtonsoftCassiniSerializationFixture : CassiniSerializationFixture
    {
        public NewtonsoftCassiniSerializationFixture()
        {
            _serializer = new Newtonsoft.Serializer();
        }

        private readonly IJsonSerializer _serializer;

        public override IJsonSerializer Serializer
        {
            get { return _serializer; }
        }
    }
}