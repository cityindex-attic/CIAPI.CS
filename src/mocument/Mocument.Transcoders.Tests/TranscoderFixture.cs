using System;
using System.Globalization;
using System.Net;
using System.Text;
using System.Threading;
using Fiddler;
using Mocument.Model;
using NUnit.Framework;
using Newtonsoft.Json;
using Salient.FiddlerTestingInfrastructure;

namespace Mocument.Transcoders.Tests
{
    [TestFixture]
    public class TranscoderFixture
    {
        [Test]
        public void TranscoderCanRoundTrip()
        {
            for (int i = 0; i < 5; i++)
            {
                new Thread(io=>
                               {
                                   Console.WriteLine(io + "start ");
                                   Noid((string) io);
                                   Console.WriteLine(io + " end");
                               }).Start(i.ToString(CultureInfo.InvariantCulture));
            }
            Thread.Sleep(15000);
        }
        public void Noid(string param)
        {
            var p = TestProxyFactory.Create();
            Session sessionToExamine = null;
            p.BeforeRequest += session =>
                                   {
                                       sessionToExamine = session;
                                       string pathAndQuery;
                                       string host;
                                       string q = session.PathAndQuery.TrimStart('/');

                                       host = Utilities.TrimAfter(q, "/");
                                       if (q.IndexOf('/') > -1)
                                       {
                                           pathAndQuery = "/" + Utilities.TrimBefore(q, "/");
                                       }
                                       else
                                       {
                                           pathAndQuery = "/";
                                       }
                                       session.host = host;
                                       session.PathAndQuery = pathAndQuery;
                                   };

            p.Start();

            try
            {
                new WebClient().UploadString("http://localhost.:" + TestProxyFactory.Port + "/httpbin.org/post?foo=" + param, "bar=" + param);

                // lets try to round-trip the captured session

                Tape tape = HttpArchiveTranscoder.Export(new[] { sessionToExamine });
                var json = JsonConvert.SerializeObject(tape.log.entries[0],Formatting.Indented);
                var sessionOut = HttpArchiveTranscoder.Import(tape)[0];
                bool fail = false;
                if (sessionToExamine.PathAndQuery != sessionOut.PathAndQuery)
                {
                    fail = true; 
                    Console.WriteLine("PathAndQuery");
                }
                if (!CompareByteArrays(sessionToExamine.RequestBody, sessionOut.RequestBody, "RequestBody"))
                {
                    fail = true;
                    Console.WriteLine("RequestBody");

                    Console.WriteLine("--------------------------------------------------------");
                    Console.WriteLine(Encoding.UTF8.GetString(sessionToExamine.RequestBody));
                    Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");

                    Console.WriteLine(Encoding.UTF8.GetString(sessionOut.RequestBody));
                    Console.WriteLine("--------------------------------------------------------");

                }
                if (!CompareByteArrays(sessionToExamine.ResponseBody, sessionOut.ResponseBody, "ResponseBody"))
                {
                    fail = true;
                    Console.WriteLine("ResponseBody");
                    Console.WriteLine("--------------------------------------------------------");
                    Console.WriteLine(Encoding.UTF8.GetString(sessionToExamine.ResponseBody));
                    Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");

                    Console.WriteLine(Encoding.UTF8.GetString(sessionOut.ResponseBody));
                    Console.WriteLine("--------------------------------------------------------");

                }
                // fiddler static inc if (sessionToExamine.SuggestedFilename != sessionOut.SuggestedFilename) Console.WriteLine("SuggestedFilename");
                if (sessionToExamine.TunnelEgressByteCount != sessionOut.TunnelEgressByteCount)
                {
                    fail = true; 
                    Console.WriteLine("TunnelEgressByteCount");
                }
                if (sessionToExamine.TunnelIngressByteCount != sessionOut.TunnelIngressByteCount)
                {
                    fail = true; 
                    Console.WriteLine("TunnelIngressByteCount");
                }
                if (sessionToExamine.TunnelIsOpen != sessionOut.TunnelIsOpen)
                {
                    fail = true; 
                    Console.WriteLine("TunnelIsOpen");
                }
                if (sessionToExamine.bHasResponse != sessionOut.bHasResponse)
                {
                    fail = true; 
                    Console.WriteLine("bHasResponse");
                }
                if (sessionToExamine.bypassGateway != sessionOut.bypassGateway)
                {
                    fail = true; 
                    Console.WriteLine("bypassGateway");
                }
                if (sessionToExamine.fullUrl != sessionOut.fullUrl)
                {
                    fail = true; 
                    Console.WriteLine("fullUrl");
                }
                if (sessionToExamine.host != sessionOut.host)
                {
                    fail = true; 
                    Console.WriteLine("host");
                }
                if (sessionToExamine.hostname != sessionOut.hostname)
                {
                    fail = true; 
                    Console.WriteLine("hostname");
                }
                // fiddler static inc if (sessionToExamine.id != sessionOut.id) Console.WriteLine("id");
                if (sessionToExamine.isFTP != sessionOut.isFTP)
                {
                    fail = true; 
                    Console.WriteLine("isFTP");
                }
                if (sessionToExamine.isHTTPS != sessionOut.isHTTPS)
                {
                    fail = true; 
                    Console.WriteLine("isHTTPS");
                }
                if (sessionToExamine.isTunnel != sessionOut.isTunnel)
                {
                    fail = true; 
                    Console.WriteLine("isTunnel");
                }
               
                
                if (sessionToExamine.port != sessionOut.port)
                {
                    fail = true; 
                    Console.WriteLine("port");
                }
                if (!CompareByteArrays(sessionToExamine.requestBodyBytes, sessionOut.requestBodyBytes, "requestBodyBytes"))
                {
                    fail = true;
                    Console.WriteLine("requestBodyBytes");
                    Console.WriteLine("--------------------------------------------------------");
                    Console.WriteLine(Encoding.UTF8.GetString(sessionToExamine.requestBodyBytes));
                    Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");

                    Console.WriteLine(Encoding.UTF8.GetString(sessionOut.requestBodyBytes));
                    Console.WriteLine("--------------------------------------------------------");

                }
                if (!CompareByteArrays(sessionToExamine.responseBodyBytes, sessionOut.responseBodyBytes, "responseBodyBytes"))
                {
                    fail = true;
                    Console.WriteLine("responseBodyBytes");
                    Console.WriteLine("--------------------------------------------------------");
                    Console.WriteLine(Encoding.UTF8.GetString(sessionToExamine.responseBodyBytes));
                    Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");

                    Console.WriteLine(Encoding.UTF8.GetString(sessionOut.responseBodyBytes));
                    Console.WriteLine("--------------------------------------------------------");
                }
                if (sessionToExamine.responseCode != sessionOut.responseCode)
                {
                    fail = true;
                    Console.WriteLine("responseCode");
                }
                if (sessionToExamine.state != sessionOut.state)
                {
                    fail = true; 
                    Console.WriteLine("state");
                }
                if (sessionToExamine.url != sessionOut.url)
                {
                    fail = true; 
                    Console.WriteLine("url");
                }

                // not tx if (sessionToExamine.clientIP != sessionOut.clientIP) Console.WriteLine("clientIP");
                // not tx if (sessionToExamine.clientPort != sessionOut.clientPort) Console.WriteLine("clientPort");
                // not tx  if (sessionToExamine.m_clientIP != sessionOut.m_clientIP) Console.WriteLine("m_clientIP");
                // not tx if (sessionToExamine.m_clientPort != sessionOut.m_clientPort) Console.WriteLine("m_clientPort");
                // not tx ?? if (sessionToExamine.m_hostIP != sessionOut.m_hostIP) Console.WriteLine("m_hostIP");
                // #TODO: compare hashtables
                //if (sessionToExamine.oFlags != sessionOut.oFlags) Console.WriteLine("oFlags");
                // object comparison if (sessionToExamine.oRequest != sessionOut.oRequest) Console.WriteLine("oRequest");
                // object comparison if (sessionToExamine.oResponse != sessionOut.oResponse) Console.WriteLine("oResponse");

                //#TODO check export "!string.IsNullOrEmpty(session["ui-comments"])
                //if (sessionToExamine.BitFlags != sessionOut.BitFlags) Console.WriteLine("BitFlags");
                //if (sessionToExamine.LocalProcessID != sessionOut.LocalProcessID) Console.WriteLine("LocalProcessID");

                Assert.False(fail, "round trip failed");
            }
                catch(Exception ex)
                {
                    Assert.Fail(ex.Message);    
                }
            finally
            {
                p.Stop();
            }
        }

        public static bool CompareByteArrays(byte[] a, byte[] b, string tag)
        {
            if (a.Length != b.Length)
            {
                //Console.WriteLine("length");
                return false;
            }
            for (int i = 0; i < a.Length; i++)
            {
                var aa = a[i];
                var bb = b[i];
                if (aa != bb)
                {
                    // Console.WriteLine("{0} {1} {2} {3}", tag, i, aa, bb);
                    return false;
                }
            }

            return true;
        }
    }
}
