using System;
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
        //private const string PushServerHost = "https://pushpreprod.cityindextest9.co.uk/";
        //private const string RpcServerHost = "https://ciapipreprod.cityindextest9.co.uk/TradingApi/";
        //private const string UserName = "xx189949";
        //private const string Password = "password";
        private const string PushServerHost = "https://push.cityindex.com/";
        private const string RpcServerHost = "https://ciapi.cityindex.com/tradingapi/";
        private const string UserName = "DM715257";
        private const string Password = "password";
        IStreamingClient _streamingClient;
        IStreamingListener<PriceDTO> _listener;
        private IStreamingListener<PriceDTO> _listener2;
        private Thread _backgroundRpc;
        private Client rpcClient;
        private bool _backgroundRpcStop = false;
        public MainPage()
        {
            InitializeComponent();

            Dispatcher.BeginInvoke(() =>
            {
                StartButton.IsEnabled = false;
                StopButton.IsEnabled = false;
            });


            // build an rpc client and log it in.
            rpcClient = new Client(new Uri(RpcServerHost));

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
            Dispatcher.BeginInvoke(() => { listBox2.Items.Add("Starting"); });

            Dispatcher.BeginInvoke(() =>
            {
                listBox1.Items.Clear();
                listBox2.Items.Clear();
                listBox3.Items.Clear();

                StartButton.IsEnabled = false;
                StopButton.IsEnabled = false;
                 

                _backgroundRpcStop = false;
                _backgroundRpc = new Thread(() =>
                                                {
                                                    while (_backgroundRpcStop==false)
                                                    {
                                                        new AutoResetEvent(false).WaitOne(100000);
                                                        var marketInfo = rpcClient.Market.GetMarketInformation("400535967");
                                                        Dispatcher.BeginInvoke(() => listBox3.Items.Add(marketInfo.MarketInformation.MarketId));
                                                    }
                                                    return;
                                                });
                _backgroundRpc.Start();
                new Thread(() =>
                {

                    Debug.WriteLine("building listener2");
                    _listener = _streamingClient.BuildPricesListener(400535967, 81136, 400509294, 400535971, 80902, 400509295, 400193864, 400525367, 80926, 400498641, 400193866, 91047, 400194551, 121766, 400172033, 139144);
                    _listener.MessageReceived += ListenerMessageReceived;
                    Debug.WriteLine("building listener");

                    _listener2 = _streamingClient.BuildDefaultPricesListener(2347);
                    _listener2.MessageReceived += Listener2MessageReceived;
                    Debug.WriteLine("listener started");
                    Debug.WriteLine("listener2 started");

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
            Dispatcher.BeginInvoke(() => { listBox2.Items.Add("Stopping"); });

            _backgroundRpcStop = true;

            // tearing down a listener should be done on a new thread.
            // you will want to start a listener in a new thread, doing so on the UI thread is forbidden

            new Thread(() =>
                {

                    Debug.WriteLine("tearing down listeners");

                    // unwire the listener 
                    _listener.MessageReceived -= ListenerMessageReceived;
                    _listener2.MessageReceived -= Listener2MessageReceived;
                    // tear down the listener
                    _streamingClient.TearDownListener(_listener);
                    _streamingClient.TearDownListener(_listener2);

                }).Start();


            // enable the user to start a new listener, the current one is not
            // long for this world. ;-)

            StartButton.IsEnabled = true;
            StopButton.IsEnabled = false;



        }
        void Listener2MessageReceived(object sender, MessageEventArgs<PriceDTO> e)
        {
            Dispatcher.BeginInvoke(() => listBox2.Items.Add(e.Data.MarketId));
        }
        void ListenerMessageReceived(object sender, MessageEventArgs<PriceDTO> e)
        {

            Dispatcher.BeginInvoke(() => listBox1.Items.Add(e.Data.MarketId));
        }
    }
}