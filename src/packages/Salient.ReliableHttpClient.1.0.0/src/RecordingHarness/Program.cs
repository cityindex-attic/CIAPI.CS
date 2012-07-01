using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using CassiniDev;
using Salient.ReliableHttpClient;
using Salient.ReliableHttpClient.ReferenceImplementation;


namespace RecordingHarness
{
    class Program
    {
        private static void Wait(Exception e, WaitHandle g)
        {
            if (!g.WaitOne(10000))
            {
                throw new Exception("timed out");
            }
            if (e != null)
            {
                throw e;
            }
        }
        static void Main(string[] args)
        {

            var gate = new AutoResetEvent(false);
            Exception exception = null;
            
            var server = new CassiniDevServer();
            var path = new ContentLocator("WcfRestService1").LocateContent();
            server.StartServer(path);

            var client = new SampleClient(server.NormalizeUrl("").TrimEnd('/'));
            var recorder = new Recorder(client);
            recorder .Start();



            client.BeginListService1(ar =>
                                         {
                                             try
                                             {
                                                 List<SampleItem> result = client.EndListService1(ar);
                                                 Console.WriteLine(DateTime.Now + " " + result.Count);
                                             }
                                             catch (Exception ex)
                                             {

                                                 exception = ex;
                                             }
                                             finally
                                             {
                                                 gate.Set();
                                             }


                                         }, null);


            Wait(exception, gate);


            server.StopServer();
            server.Dispose();


            var recording = recorder.GetRequests();
            recorder.Dispose();

            var serializedRecording = client.Serializer.SerializeObject(recording);

            client.Dispose();

            File.WriteAllText("output.txt", serializedRecording);
        }
    }
}
