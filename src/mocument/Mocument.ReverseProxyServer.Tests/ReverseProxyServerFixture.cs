using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using Mocument.DataAccess;
using NUnit.Framework;

namespace Mocument.ReverseProxyServer.Tests
{
    [TestFixture]
    public class ReverseProxyServerFixture
    {
        [Test]
        public void CanLoadAndReplaySession()
        {
            var sessionText = File.ReadAllText("CIAPISESSION.txt");

        }

        [Test]
        public void ProxyCanRecordAndPlaybackOpen()
        {
            // fire up a proxy server, record a request, check the store and then replay it

            int port = 81;
            string contextName = "Mocument";
            var path = Path.GetTempFileName();
            var store = new JsonFileStore(path);
            Server server = null;
            try
            {
                server = new Server(port,82,"localhost.", false, store);
                server.Start();
                const string postData = "bar=foo";
                var recordAddress = "http://localhost.:" + port + "/record/sky/httpbin/httpbin.org/post?foo=bar";
                string response1 = new WebClient().UploadString(recordAddress, postData);

                // introduce some delay to let the server store the tape. this is not contrived....
                Thread.Sleep(1000);

                var tapes = store.List();
                Assert.AreEqual(1, tapes.Count);
                Assert.AreEqual("sky.httpbin", tapes[0].Id);
                Assert.AreEqual(1, tapes[0].log.entries.Count);

                // #TODO: we need to add something unobtrusive to the replayed response to make it clear it was canned
                var playAddress = "http://localhost.:" + port + "/play/sky/httpbin/httpbin.org/post?foo=bar";
                string response2 = new WebClient().UploadString(playAddress, postData);
                Thread.Sleep(1000);
                Assert.AreEqual(response1, response2);

            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }
            finally
            {
                File.Delete(path);
                if (server != null)
                {
                    server.Stop();
                }
            }


        }


    }
}
