//using NUnit.Framework;

//namespace CIAPI.Tests.Rpc
//{
//    [TestFixture]
//    public class ExceptionHandling:FixtureBase
//    {






//        //    //[Test, Ignore, ExpectedException(typeof(ApiTimeoutException))]
//        //    public void ReproAbortedRequest()
//        //    {
//        //        TestRequestFactory factory = new TestRequestFactory();
//        //        var requestController = new RequestController(TimeSpan.FromSeconds(0), 2, factory, new ErrorResponseDTOJsonExceptionFactory(), new ThrottledRequestQueue(TimeSpan.FromSeconds(5), 30, 10, "data"), new ThrottledRequestQueue(TimeSpan.FromSeconds(3), 1, 3, "trading"));

//        //        var ctx = new CIAPI.Rpc.Client(new Uri(TestConfig.RpcUrl), "mockAppKey", requestController);
//        //        ctx.UserName = TestConfig.ApiUsername;
//        //        ctx.Session = TestConfig.ApiTestSessionId;

//        //        factory.CreateTestRequest("{}", TimeSpan.FromMinutes(1));

//        //        ctx.Market.GetMarketInformation("FOO");

//        //    }

//    }
//}