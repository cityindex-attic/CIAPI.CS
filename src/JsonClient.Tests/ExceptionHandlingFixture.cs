using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using CassiniDev;
using NUnit.Framework;

namespace CityIndex.JsonClient.Tests
{
    /// <summary>
    /// </summary>
    [TestFixture,Ignore("hudson is choking on serving the website, not sure why but local runs prove that the exception pollution is not an issue in JsonClient so can ignore for now.")]
    public class ExceptionHandlingFixture : CassiniDevServer
    {
        [TestFixtureSetUp]
        public void Setup()
        {
            StartServer(new ContentLocator(@"src\JsonClient.Tests.Web").LocateContent());
        }

        #region expected behavior

        [Test, ExpectedExceptionAttribute(typeof(WebException), ExpectedMessage = "The remote server returned an error: (400) Bad Request."), Category("expected behavior")]
        public void SanityCheckServerIsRunningAndWillServeException()
        {
            var client = new WebClient();
            client.DownloadString(NormalizeUrl("ErrorEcho.aspx?code=400&message={%22foo%22}"));
        }


        [Test, Category("expected behavior")]
        public void AsyncExceptionReturnsResponseText()
        {
            var client = new Client(new Uri(RootUrl));
            var gate = new AutoResetEvent(false);
            string expected = "{\"Foo\": \"bar\"}"; ;
            string actual = null;

            client.BeginRequest<TestDTO>(ar =>
                                             {
                                                 try
                                                 {
                                                     client.EndRequest(ar);
                                                     Assert.Fail("expected exception");
                                                 }
                                                 catch (ApiException ex)
                                                 {

                                                     Assert.AreEqual("The remote server returned an error: (400) Bad Request.", ex.Message);

                                                     actual = ex.ResponseText;

                                                 }
                                                 finally
                                                 {
                                                     gate.Set();
                                                 }


                                             }, null, "ErrorEcho.aspx?code=400&message=" + HttpUtility.UrlEncode(expected), "GET");

            gate.WaitOne();

            Assert.AreEqual(expected, actual);




        }




        [Test, Category("expected behavior")]
        public void AsyncExceptionSurroundedByAsyncRequestsReturnsResponseText()
        {
            var client = new Client(new Uri(RootUrl));
            var gate = new AutoResetEvent(false);
            string expected = "{\"Foo\": \"bar\"}"; ;
            string actual = null;


            client.BeginRequest<TestDTO>(ar =>
            {
                try
                {
                    var dto = client.EndRequest(ar);
                    Assert.IsNotNull(dto);
                }
                finally
                {
                    gate.Set();
                }


            }, null, "ErrorEcho.aspx?code=200&message=" + HttpUtility.UrlEncode(expected), "GET");

            gate.WaitOne();




            client.BeginRequest<TestDTO>(ar =>
            {
                try
                {
                    client.EndRequest(ar);
                    Assert.Fail("expected exception");
                }
                catch (ApiException ex)
                {

                    Assert.AreEqual("The remote server returned an error: (400) Bad Request.", ex.Message);

                    actual = ex.ResponseText;

                }
                finally
                {
                    gate.Set();
                }


            }, null, "ErrorEcho.aspx?code=400&message=" + HttpUtility.UrlEncode(expected), "GET");

            gate.WaitOne();

            Assert.AreEqual(expected, actual);


            client.BeginRequest<TestDTO>(ar =>
            {
                try
                {
                    var dto = client.EndRequest(ar);
                    Assert.IsNotNull(dto);
                }
                finally
                {
                    gate.Set();
                }


            }, null, "ErrorEcho.aspx?code=200&message=" + HttpUtility.UrlEncode(expected), "GET");

            gate.WaitOne();


        }



        [Test, Category("expected behavior")]
        public void ConsucutiveAsyncExceptionReturnsResponseText()
        {
            var client = new Client(new Uri(RootUrl));
            var gate = new AutoResetEvent(false);
            string expected = "{\"Foo\": \"bar\"}"; ;
            string actual = null;
            client.BeginRequest<TestDTO>(ar =>
            {
                try
                {
                    client.EndRequest(ar);
                    Assert.Fail("expected exception");
                }
                catch (ApiException ex)
                {

                    Assert.AreEqual("The remote server returned an error: (400) Bad Request.", ex.Message);

                    actual = ex.ResponseText;

                }
                finally
                {
                    gate.Set();
                }


            }, null, "ErrorEcho.aspx?code=400&message=" + HttpUtility.UrlEncode(expected), "GET");

            gate.WaitOne();

            Assert.AreEqual(expected, actual);

            client.BeginRequest<TestDTO>(ar =>
            {
                try
                {
                    client.EndRequest(ar);
                    Assert.Fail("expected exception");
                }
                catch (ApiException ex)
                {

                    Assert.AreEqual("The remote server returned an error: (400) Bad Request.", ex.Message);

                    actual = ex.ResponseText;

                }
                finally
                {
                    gate.Set();
                }


            }, null, "ErrorEcho.aspx?code=400&message=" + HttpUtility.UrlEncode(expected), "GET");

            gate.WaitOne();

            Assert.AreEqual(expected, actual);

            client.BeginRequest<TestDTO>(ar =>
            {
                try
                {
                    client.EndRequest(ar);
                    Assert.Fail("expected exception");
                }
                catch (ApiException ex)
                {

                    Assert.AreEqual("The remote server returned an error: (400) Bad Request.", ex.Message);

                    actual = ex.ResponseText;

                }
                finally
                {
                    gate.Set();
                }


            }, null, "ErrorEcho.aspx?code=400&message=" + HttpUtility.UrlEncode(expected), "GET");

            gate.WaitOne();

            Assert.AreEqual(expected, actual);

        }

        [Test, Category("expected behavior")]
        public void SycnExceptionReturnsResponseText()
        {
            var client = new Client(new Uri(RootUrl));
            string expected = "{\"Foo\": \"bar\"}"; ;
            string actual = null;

            try
            {
                client.Request<TestDTO>("ErrorEcho.aspx?code=400&message=" + HttpUtility.UrlEncode(expected), "GET");
            }
            catch (ApiException ex)
            {

                Assert.AreEqual("The remote server returned an error: (400) Bad Request.", ex.Message);

                actual = ex.ResponseText;

            }
            Assert.AreEqual(expected, actual);
        }


        [Test, Category("expected behavior")]
        public void MultipleSycnExceptionReturnsResponseText()
        {
            var client = new Client(new Uri(RootUrl));
            string expected = "{\"Foo\": \"bar\"}"; ;
            string actual = null;

            try
            {
                client.Request<TestDTO>("ErrorEcho.aspx?code=400&message=" + HttpUtility.UrlEncode(expected), "GET");
            }
            catch (ApiException ex)
            {

                Assert.AreEqual("The remote server returned an error: (400) Bad Request.", ex.Message);

                actual = ex.ResponseText;

            }
            Assert.AreEqual(expected, actual);

            expected = "{\"Foo\": \"bar\"}"; ;
            try
            {
                client.Request<TestDTO>("ErrorEcho.aspx?code=400&message=" + HttpUtility.UrlEncode(expected), "GET");
            }
            catch (ApiException ex)
            {

                Assert.AreEqual("The remote server returned an error: (400) Bad Request.", ex.Message);

                actual = ex.ResponseText;

            }
            Assert.AreEqual(expected, actual);
        }



        [Test, Category("expected behavior")]
        public void SyncThenAsyncExceptionReturnsResponseText()
        {
            var client = new Client(new Uri(RootUrl));
            string expected = "{\"Foo\": \"bar\"}"; ;
            string actual = null;

            try
            {
                client.Request<TestDTO>("ErrorEcho.aspx?code=400&message=" + HttpUtility.UrlEncode(expected), "GET");
            }
            catch (ApiException ex)
            {

                Assert.AreEqual("The remote server returned an error: (400) Bad Request.", ex.Message);

                actual = ex.ResponseText;

            }
            Assert.AreEqual(expected, actual);

            expected = "{\"Foo\": \"bar\"}"; ;
            var gate = new AutoResetEvent(false);

            client.BeginRequest<TestDTO>(ar =>
            {
                try
                {
                    client.EndRequest(ar);
                    Assert.Fail("expected exception");
                }
                catch (ApiException ex)
                {

                    Assert.AreEqual("The remote server returned an error: (400) Bad Request.", ex.Message);

                    actual = ex.ResponseText;

                }
                finally
                {
                    gate.Set();
                }


            }, null, "ErrorEcho.aspx?code=400&message=" + HttpUtility.UrlEncode(expected), "GET");

            gate.WaitOne();

            Assert.AreEqual(expected, actual);

        }



        #endregion


        [Test,Category("Bug case")]
        public void SyncThenAsyncExceptionThenSyncRequestReturnsResponseText ()
        {
            var client = new Client(new Uri(RootUrl));
            string expected = "{\"Foo\": \"bar\"}"; 
            string actual = null;

            try
            {
                client.Request<TestDTO>("ErrorEcho.aspx?code=400&message=" + HttpUtility.UrlEncode(expected), "GET");
            }
            catch (ApiException ex)
            {

                Assert.AreEqual("The remote server returned an error: (400) Bad Request.", ex.Message);

                actual = ex.ResponseText;

            }
            Assert.AreEqual(expected, actual);

            expected = "{\"Foo\": \"bar\"}";
            var gate = new AutoResetEvent(false);

            client.BeginRequest<TestDTO>(ar =>
            {
                try
                {
                    client.EndRequest(ar);
                    Assert.Fail("expected exception");
                }
                catch (ApiException ex)
                {

                    Assert.AreEqual("The remote server returned an error: (400) Bad Request.", ex.Message);

                    actual = ex.ResponseText;

                }
                finally
                {
                    gate.Set();
                }


            }, null, "ErrorEcho.aspx?code=400&message=" + HttpUtility.UrlEncode(expected), "GET");

            gate.WaitOne();

            Assert.AreEqual(expected, actual);

            var tdto = client.Request<TestDTO>("ErrorEcho.aspx?code=200&message=" + HttpUtility.UrlEncode(expected), "GET");

        }

        [Test,Category("Bug case")]
        public void SyncExceptionSurroundedBySyncRequestsReturnsResponseText()
        {
            var client = new Client(new Uri(RootUrl));
            string expected = "{\"Foo\": \"bar\"}"; ;
            string actual = null;

            var tdto = client.Request<TestDTO>("ErrorEcho.aspx?code=200&message=" + HttpUtility.UrlEncode(expected), "GET");
            Assert.IsNotNull(tdto);

            try
            {
                client.Request<TestDTO>("ErrorEcho.aspx?code=400&message=" + HttpUtility.UrlEncode(expected), "GET");
            }
            catch (ApiException ex)
            {

                Assert.AreEqual("The remote server returned an error: (400) Bad Request.", ex.Message);

                actual = ex.ResponseText;

            }
            Assert.AreEqual(expected, actual);

            tdto = client.Request<TestDTO>("ErrorEcho.aspx?code=200&message=" + HttpUtility.UrlEncode(expected), "GET");
            Assert.IsNotNull(tdto);


            
        }
    }

    public class TestDTO
    {
        public string Foo { get; set; }
    }
}
