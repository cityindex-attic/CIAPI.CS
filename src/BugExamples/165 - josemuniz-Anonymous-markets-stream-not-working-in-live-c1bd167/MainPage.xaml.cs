using System;
using System.Threading;
using System.Windows;
using CIAPI.DTO;
using StreamingClient;
using Client = CIAPI.Rpc.Client;
using IStreamingClient = CIAPI.Streaming.IStreamingClient;

namespace PhoneApp6
{
    public partial class MainPage
    {
        private const string USERNAME = "xx794680";
        private const string PASSWORD = "password";
        private const int AccountOperatorId = 2347;

        public Client RpcClient;
        public IStreamingClient StreamingClient;
        public IStreamingListener<PriceDTO> MarketPricesStream;
        private bool _listening;

        private static readonly Uri RPC_URI = new Uri("https://ciapi.cityindex.com/TradingApi");
        private static readonly Uri STREAM_URI = new Uri("https://push.cityindex.com/");

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            Unloaded += OnMainPageUnloaded;
            BuildClients();
        }

        private void OnMainPageUnloaded(object sender, RoutedEventArgs e)
        {
            if (StreamingClient == null) return;
            if (MarketPricesStream != null) StreamingClient.TearDownListener(MarketPricesStream);
        }        

        private void BuildClients()
        {
            Dispatcher.BeginInvoke(() => listBox1.Items.Add("creating rpc client"));
            RpcClient = new Client(RPC_URI, STREAM_URI, "CI-WP7");
            RpcClient.BeginLogIn(USERNAME, PASSWORD, ar =>
            {
                var session = RpcClient.EndLogIn(ar);

                ThreadPool.QueueUserWorkItem(_ =>
                {
                    Dispatcher.BeginInvoke(() => listBox1.Items.Add("creating listeners"));
                    StreamingClient = RpcClient.CreateStreamingClient();
                    MarketPricesStream = StreamingClient.BuildDefaultPricesListener(AccountOperatorId);
                    MarketPricesStream.MessageReceived += OnMarketPricesStreamMessageReceived;
                    Dispatcher.BeginInvoke(() => button1.IsEnabled = true);
                });
            }, null);
        }

        private void OnMarketPricesStreamMessageReceived(object sender, MessageEventArgs<PriceDTO> e)
        {
            if (!_listening) return;
            Dispatcher.BeginInvoke(() => listBox1.Items.Add("anonymous price arrived!"));
            Dispatcher.BeginInvoke(() => button1.IsEnabled = true);
            _listening = false;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                listBox1.Items.Add("waiting for anonymous prices...");
                button1.IsEnabled = false;
                _listening = true;
            });
        }
    }
}
