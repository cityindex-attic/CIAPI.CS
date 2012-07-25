using System;
using System.Net;
using System.Text;
using NUnit.Framework;

namespace CassiniDev.Lib.Net35.Tests
{
    [TestFixture]
    public class ProcessingHooksFixture : CassiniDevServer
    {

        #region Plumbing
        [TestFixtureSetUp]
        public void Start()
        {
            string content = new ContentLocator("testContent").LocateContent();
            StartServer(content);



        }
        [TestFixtureTearDown]
        public void Stop()
        {

            StopServer();
        }
        #endregion

        [Test]
        public void CanFetchCustomExtension()
        {
            var url = NormalizeUrl("foofile.foo");

            var request = WebRequest.Create(url);

            var response = request.GetResponse();

            Assert.AreEqual("text/plain", response.ContentType);
            var x = Encoding.UTF8.GetString(response.GetResponseStream().StreamToBytes());

            StringAssert.Contains("﻿this is a foo file", x);


        }


        [Test]
        public void CanFetchUnknownExtensionOrContentType()
        {

            var url = NormalizeUrl("barfile.bar");

            var request = WebRequest.Create(url);

            WebResponse response = request.GetResponse();

            var x = Encoding.UTF8.GetString(response.GetResponseStream().StreamToBytes());

            StringAssert.Contains("﻿﻿this is a bar file", x);
        }

        [Test]
        public void CanInterceptRequest()
        {


            // this is where you would build out something like service mock logic

            EventHandler<RequestInfoArgs> mockingHandler = (i, e) =>
                                                               {
                                                                   e.Continue = false;
                                                                   e.Response = "all yur base are minez";
                                                                   e.ExtraHeaders = "x-foo-bar-header: value";
                                                               };



            var url = NormalizeUrl("foofile.foo");
            var request = HttpWebRequest.Create(url);

            Server.ProcessRequest += mockingHandler;

            WebResponse response = request.GetResponse();

            Server.ProcessRequest -= mockingHandler;


            Assert.IsNotNull(response.Headers["x-foo-bar-header"]);
            Assert.AreEqual("value", response.Headers["x-foo-bar-header"]);

            var x = Encoding.UTF8.GetString(response.GetResponseStream().StreamToBytes());
            Assert.AreEqual("all yur base are minez", x);


        }

    }
}