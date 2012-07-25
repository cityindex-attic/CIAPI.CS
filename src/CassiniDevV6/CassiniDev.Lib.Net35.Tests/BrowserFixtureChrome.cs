using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using CassiniDev;


namespace CassiniDev.Lib.Net35.Tests
{

    [TestFixture]
    public class BrowserFixtureChrome : CassiniDevBrowserTest
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
        public void CanGetQUnitTestResults()
        {
            var uri = NormalizeUrl("qunit-callback.htm");

            RequestEventArgs result = RunTest(uri, WebBrowser.Chrome);
            var body = Encoding.UTF8.GetString(result.RequestLog.Body);


            Console.WriteLine(body);

            StringAssert.Contains("Module Done: module; failures = 0; total = 1", body);
        }
    }
}
