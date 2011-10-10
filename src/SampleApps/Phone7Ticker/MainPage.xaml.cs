﻿using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using CIAPI.DTO;
using CIAPI.Rpc;
using CIAPI.Streaming;
using Microsoft.Phone.Controls;
using StreamingClient;
using IStreamingClient = CIAPI.Streaming.IStreamingClient;

namespace Phone7Ticker
{
    public partial class MainPage : PhoneApplicationPage
    {
        private const string PushServerHost = "https://pushpreprod.cityindextest9.co.uk/";
        private const string RpcServerHost = "https://ciapipreprod.cityindextest9.co.uk/TradingApi/";
        private const string UserName = "xx189949";
        private const string Password = "password";
        
        IStreamingClient _streamingClient;
        IStreamingListener<PriceDTO> _listener;


        public MainPage()
        {
            InitializeComponent();

            Dispatcher.BeginInvoke(() =>
            {
                StartButton.IsEnabled = false;
                StopButton.IsEnabled = false;
            });


            // build an rpc client and log it in.
            var rpcClient = new Client(new Uri(RpcServerHost));

            // get a session from the rpc client
            rpcClient.BeginLogIn(UserName, Password, ar =>
                {
                    rpcClient.EndLogIn(ar);

                    


                    Debug.WriteLine("creating client");

                    // build a streaming client.
                    _streamingClient = StreamingClientFactory.CreateStreamingClient(new Uri(PushServerHost), UserName, rpcClient.Session);

                    Debug.WriteLine("connecting client");

                    // note: due to internal changes the 'connect' method
                    // name is a misnomer: no actual network activity is occuring,
                    // only the building of the necessary client connections for 
                    // each of the published data adapters. Actual connection is 
                    // performed on demand for each adapter. This minimizes startup time.

                    // the upside to this is that there is no need to run .Connect in a separate thread.

                    _streamingClient.Connect();
                    Debug.WriteLine("client connected");


                    // from this point there should be no need to stop, disconnect or dispose of the 
                    // client instance. But if you choose to disconnect a StreamingClient, it should
                    // be disposed and reinstantiated. it is a one use object at this point as this is 
                    // the only usage pattern presented in the sample code.

                    Dispatcher.BeginInvoke(() =>
                        {
                            listBox1.Items.Add("logged in");


                            StartButton.IsEnabled = true;
                            StopButton.IsEnabled = false;
                        });

                }, null);

        }





        private void StartButtonClick(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                listBox1.Items.Clear();

                StartButton.IsEnabled = false;
                StopButton.IsEnabled = false;

                // you will want to start a listener in a new thread, doing so on the UI thread will deadlock the app.
                // the referenced lightstreamer assembly does not check which thread you are on as the Rpc client does.
                new Thread(() =>
                {


                    Debug.WriteLine("building listener");
                    _listener = _streamingClient.BuildPricesListener(400535967, 81136, 400509294, 400535971, 80902, 400509295, 400193864, 400525367, 80926, 400498641, 400193866, 91047, 400194551, 121766, 400172033, 139144);
                    _listener.MessageReceived += ListenerMessageReceived;
                    Debug.WriteLine("starting listener");
                    _listener.Start();
                    Debug.WriteLine("listener started");

                    Dispatcher.BeginInvoke(() =>
                    {
                        StartButton.IsEnabled = false;
                        StopButton.IsEnabled = true;
                    });

                }).Start();
            });

        }

        private void StopButtonClick(object sender, RoutedEventArgs e)
        {




            // stopping a listener should be done on a new thread.


            new Thread(() =>
                {
                    // stopping a listener takes a while and at the end of the process it will throw
                    // an exception. from the looks of the LightStreamer sample code this is to be expected
                    // and should be ignored.

                    // it is for this reason that a listener should not be reused. 
                    // simply call stop in a separate thread and move on. the exception on disconnect
                    // will simply go into the ether and the instance will go out of scope and be collected.

                    // or, if you don't like seeing phantom exceptions in your logs, wrap it up in
                    // a swallow as shown.

                    Debug.WriteLine("stopping listener");

                    // unwire the listener 
                    _listener.MessageReceived -= ListenerMessageReceived;

                    try
                    {
                        _listener.Stop();
                    }
                    catch
                    {
                    }
                }).Start();


            // enable the user to start a new listener, the current one is not
            // long for this world. ;-)

            StartButton.IsEnabled = true;
            StopButton.IsEnabled = false;



        }

        void ListenerMessageReceived(object sender, MessageEventArgs<PriceDTO> e)
        {

            Dispatcher.BeginInvoke(() => listBox1.Items.Add(e.Data.MarketId));
        }
    }
}