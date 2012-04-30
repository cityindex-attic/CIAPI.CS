using System;
using System.Linq;
using System.Threading;
using System.Windows;
using CIAPI.DTO;
using CIAPI.Streaming;
using Salient.ReflectiveLoggingAdapter;
using StreamingClient;
using Client = CIAPI.Rpc.Client;
using IStreamingClient = CIAPI.Streaming.IStreamingClient;

namespace PhoneApp3
{
    public partial class MainPage
    {
        private ILog _logger = LogManager.GetLogger(typeof (MainPage));
        private const string USERNAME = "DM813766";
        private const string PASSWORD = "password";
        private const int MarketId = 154297;

        public Client RpcClient;
        public IStreamingClient StreamingClient;
        public IStreamingListener<PriceDTO> MarketPricesStream;
        public IStreamingListener<OrderDTO> OrdersStream;
        public ApiMarketInformationDTO Market;
        public AccountInformationResponseDTO Account;
        private bool _ordered;
        private bool _listening;

        private static readonly Uri RPC_URI = new Uri("http://ciapipreprod.cityindextest9.co.uk/tradingapi");
        private static readonly Uri STREAM_URI = new Uri("http://pushpreprod.cityindextest9.co.uk/");
        //private static readonly Uri RPC_URI = new Uri("https://ciapi.cityindex.com/tradingapi");
        //private static readonly Uri STREAM_URI = new Uri("https://push.cityindex.com");

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
            if (OrdersStream != null) StreamingClient.TearDownListener(OrdersStream);
        }        

        private void BuildClients()
        {
            //Hook up a logger for the CIAPI.CS libraries
            LogManager.CreateInnerLogger = (logName, logLevel, showLevel, showDateTime, showLogName, dateTimeFormat)
                         => new SimpleDebugAppender(logName, logLevel, showLevel, showDateTime, showLogName, dateTimeFormat);

            Dispatcher.BeginInvoke(() => listBox1.Items.Add("creating rpc client"));
            RpcClient = new Client(RPC_URI, STREAM_URI, "CI-WP7");
            RpcClient.BeginLogIn(USERNAME, PASSWORD, ar =>
            {
                _logger .Info("ending login");
                var session = RpcClient.EndLogIn(ar);

                ThreadPool.QueueUserWorkItem(_ =>
                {
                    Dispatcher.BeginInvoke(() => listBox1.Items.Add("creating listeners"));
                    StreamingClient = RpcClient.CreateStreamingClient();
                    MarketPricesStream = StreamingClient.BuildPricesListener(new[] { MarketId });
                    MarketPricesStream.MessageReceived += OnMarketPricesStreamMessageReceived;
                    OrdersStream = StreamingClient.BuildOrdersListener();
                    OrdersStream.MessageReceived += OnOrdersStreamMessageReceived;

                    Dispatcher.BeginInvoke(() => listBox1.Items.Add("getting account info"));
                    RpcClient.AccountInformation.BeginGetClientAndTradingAccount(ar2 =>
                    {
                        Account = RpcClient.AccountInformation.EndGetClientAndTradingAccount(ar2);

                        Dispatcher.BeginInvoke(() => listBox1.Items.Add("getting market info"));
                        RpcClient.Market.BeginGetMarketInformation(MarketId.ToString(), ar3 =>
                        {
                            Market = RpcClient.Market.EndGetMarketInformation(ar3).MarketInformation;
                            Dispatcher.BeginInvoke(() => button1.IsEnabled = true);
                        }, null);
                    }, null);
                });
            }, null);
        }

        private void OnMarketPricesStreamMessageReceived(object sender, MessageEventArgs<PriceDTO> e)
        {
            if (!_listening || _ordered || Market == null) return;
            _ordered = true;

            var order = new NewTradeOrderRequestDTO
            {
                MarketId = e.Data.MarketId,
                BidPrice = e.Data.Bid,
                OfferPrice = e.Data.Offer,
                AuditId = e.Data.AuditId,
                Quantity = Market.WebMinSize.GetValueOrDefault() + 1,
                TradingAccountId = Account.TradingAccounts[0].TradingAccountId,
                Direction = "buy"
            };

            Dispatcher.BeginInvoke(() => listBox1.Items.Add("price update arrived, making a new trade"));
            RpcClient.TradesAndOrders.BeginTrade(order, ar =>
            {
                var result = RpcClient.TradesAndOrders.EndTrade(ar);
                var newOrder = result.Orders.Length > 0 && result.Orders[0].OrderId == result.OrderId
                    ? result.Orders[0]
                    : null;

                if (newOrder != null && newOrder.Status == 8 && newOrder.StatusReason == 140)
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        listBox1.Items.Add("the account is on a dealer watchlist!");
                        listBox1.Items.Add("waiting for the order approval...");
                    });
                }
            }, null);
        }

        private void OnOrdersStreamMessageReceived(object sender, MessageEventArgs<OrderDTO> e)
        {
            Dispatcher.BeginInvoke(() => listBox1.Items.Add("got the response from the approval console!"));
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                button1.IsEnabled = false;
                _listening = true;
            });
        }
    }
}
