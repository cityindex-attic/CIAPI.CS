using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using NUnit.Framework;
using Salient.ReflectiveLoggingAdapter;
using Salient.ReliableHttpClient.Serialization.Newtonsoft;

namespace Salient.ReliableHttpClient.Tests
{
    [TestFixture]
    public class RequestControllerFixture : LoggingFixtureBase
    {
        [Test]
        public void TestGet()
        {
            var logger = LogManager.GetLogger(typeof(RequestController));

            var controller = new RequestController(new Serializer());
            var gate = new AutoResetEvent(false);
            Exception exception = null;
            string responseText = null;
            controller.BeginRequest(new Uri("http://api.geonames.org/citiesJSON?north=44.1&south=-9.9&east=-22.4&west=55.2&lang=de&username=demo"), RequestMethod.GET, null, null, ContentType.TEXT, ContentType.TEXT, TimeSpan.FromSeconds(1), 3000, "http://api.geonames.org", "citiesJSON?north=44.1&south=-9.9&east=-22.4&west=55.2&lang=de&username=demo", 0, new Dictionary<string, object>(), ar =>
                                                                                                 {
                                                                                                     try
                                                                                                     {

                                                                                                         controller.EndRequest(ar);
                                                                                                         responseText = ar.ResponseText;
                                                                                                         Console.WriteLine(responseText);
                                                                                                     }
                                                                                                     catch (Exception ex)
                                                                                                     {
                                                                                                         exception = ex;
                                                                                                     }
                                                                                                     gate.Set();
                                                                                                 }, null);

            if (!gate.WaitOne(10000))
            {
                throw new Exception("timed out");
            }

            // verify cache has purged
            gate.WaitOne(3000);

            if (exception != null)
            {
                Assert.Fail(exception.Message);
            }

            var output = GetLogOutput();
        }

        [Test]
        public void TestPost()
        {
            var controller = new RequestController(new Serializer()) { UserAgent = "salient.ReliableHttpClient" };
            var gate = new AutoResetEvent(false);
            Exception exception = null;
            string responseText = null;
            var headers = new Dictionary<string, string> { { "TEST_HEADER", "VALUE" } };
            // http://www.henrycipolla.com/blog/2010/10/let-me-dump-your-post-free-http-post-test-server
            
            controller.BeginRequest(new Uri("http://posttestserver.com/post.php?dump"), RequestMethod.POST, "foo=bar", headers, ContentType.FORM, ContentType.TEXT, TimeSpan.Zero, 3000, "target", "uriTemplate", 0, new Dictionary<string, object>(), ar =>
            {
                try
                {

                    controller.EndRequest(ar);
                    responseText = ar.ResponseText;
                    Console.WriteLine(responseText);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
                gate.Set();
            }, null);

            if (!gate.WaitOne(10000))
            {
                throw new Exception("timed out");
            }
            if (exception != null)
            {
                Assert.Fail(exception.Message);
            }


        }

        //[Test]
        //public void TestRecorder()
        //{
        //    var controller = new RequestController(new Serializer()) { UserAgent = "salient.ReliableHttpClient" };
            
        //    var gate = new AutoResetEvent(false);
        //    Exception exception = null;
        //    string responseText = null;
        //    var headers = new Dictionary<string, object> { { "TEST_HEADER", "VALUE" } };
        //    // http://www.henrycipolla.com/blog/2010/10/let-me-dump-your-post-free-http-post-test-server
        //    controller.BeginRequest(new Uri("http://posttestserver.com/post.php?dump"), RequestMethod.POST, "foo=bar", headers, ContentType.FORM, ContentType.TEXT, TimeSpan.Zero, 3000, "target", "uriTemplate", 0, null, ar =>
        //    {
        //        try
        //        {

        //            controller.EndRequest(ar);
        //            responseText = ar.ResponseText;
        //            Console.WriteLine(responseText);
        //        }
        //        catch (Exception ex)
        //        {
        //            exception = ex;
        //        }
        //        gate.Set();
        //    }, null);

        //    if (!gate.WaitOne(10000))
        //    {
        //        throw new Exception("timed out");
        //    }
        //    if (exception != null)
        //    {
        //        Assert.Fail(exception.Message);
        //    }

            

            
        //}
    }
}
