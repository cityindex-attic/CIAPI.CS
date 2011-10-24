using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using CIAPI.DTO;
using NUnit.Framework;
namespace StreamingClient
{
    [TestFixture]
    public class ConnectionAdapterFixture
    {
        const string apiUrl = "https://ciapipreprod.cityindextest9.co.uk/TradingApi/";
        const string streamingUrl = "https://pushpreprod.cityindextest9.co.uk";
        const string userName = "xx189949";
        const string password = "password";
        const string topic = "PRICES.PRICE.71442";
        const string dataAdapter = "CITYINDEXSTREAMING";
        [Test]
        public void Test()
        {


            var authenticatedClient = new CIAPI.Rpc.Client(new Uri(apiUrl));
            authenticatedClient.LogIn(userName, password);


            var adapter = new FaultTolerantLsClientAdapter(streamingUrl, authenticatedClient.UserName, authenticatedClient.Session, dataAdapter);

            adapter.StatusUpdate += AdapterStatusUpdate;
            adapter.Start();
            var listener = adapter.BuildListener<PriceDTO>(topic);
            listener.MessageReceived += ListenerMessageReceived;

            new ManualResetEvent(false).WaitOne(10000);

            adapter.Stop();
            adapter.Start();
            new ManualResetEvent(false).WaitOne(10000);
            
            adapter.Stop();
        
        }
        [Test]
        public void Test2()
        {
            
            var authenticatedClient = new CIAPI.Rpc.Client(new Uri(apiUrl));
            authenticatedClient.LogIn(userName, password);


            var client = new LightstreamerClient(streamingUrl, authenticatedClient.UserName, authenticatedClient.Session);

            client.StatusUpdate += AdapterStatusUpdate;

            var listener = client.BuildListener<PriceDTO>(dataAdapter, topic);
            listener.MessageReceived += ListenerMessageReceived;

            new ManualResetEvent(false).WaitOne(20000);

            client.TearDownListener(listener);

            client.Dispose();


        }
        static void ListenerMessageReceived(object sender, MessageEventArgs<PriceDTO> e)
        {
            Debug.WriteLine(e.Data.Price);
        }

        static void AdapterStatusUpdate(object sender, ConnectionStatusEventArgs e)
        {
            Debug.WriteLine(e);
        }
    }
}
