using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace CityIndex.JsonClient.Tests
{
    public class DummyJsonExceptionFactory : IJsonExceptionFactory
    {
        public Exception ParseException(string json)
        {
            if (json.Contains("error"))
            {
                return new Exception("there was an error");
            }
            return null;
        }

        public Exception ParseException(string extraInfo, string json, Exception inner)
        {
            if (json.Contains("error"))
            {
                return new Exception("there was an error", inner);
            }
            return null;
        }
    }

    public class TestDto
    { }
    [TestFixture]
    public class JsonExceptionFactoryFixture
    {

        [Test, ExpectedException(typeof(ApiException), ExpectedMessage = "there was an error")]
        public void ErrorJsonIsRecognizedAndThrown()
        {


            var ctx = BuildClientAndSetupResponse("{\"error\":\"foo\"}");

            ctx.Request<TestDto>("foo", "GET");

        }


        private Client BuildClientAndSetupResponse(string expectedJson)
        {

            TestRequestFactory factory = new TestRequestFactory();
            var requestController = new RequestController(TimeSpan.FromSeconds(0), 2, factory, new DummyJsonExceptionFactory(), new ThrottedRequestQueue(TimeSpan.FromSeconds(5), 30, 10, "default"));

            var ctx = new Client(new Uri("http://foo.bar"), requestController);
            factory.CreateTestRequest(expectedJson);
            return ctx;
        }
    }
}
