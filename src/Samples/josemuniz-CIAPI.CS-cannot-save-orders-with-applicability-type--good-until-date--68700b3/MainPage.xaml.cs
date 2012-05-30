using System;
using System.Threading;
using System.Windows;
using CIAPI.DTO;
using CIAPI.Streaming;
using Salient.ReflectiveLoggingAdapter;
using StreamingClient;
using Client = CIAPI.Rpc.Client;

namespace PhoneApp5
{
    public partial class MainPage
    {
        private const string UerName = "xx794680";
        private const string Password = "password";

        // 400494226 EUR/USD
        // 400494234 GBP/USD
        // 154290 EUR/USD
        private const int MarketId = 154290;

        private static readonly Uri RpcUri = new Uri("https://ciapi.cityindex.com/tradingapi");
        private static readonly Uri StreamingUri = new Uri("https://push.cityindex.com");

        public Client RpcClient;
        public IStreamingClient StreamingClient;
        public IStreamingListener<PriceDTO> PriceListener;
        public ApiMarketInformationDTO Market;
        public AccountInformationResponseDTO Account;

        private bool _ordered;
        private bool _listening;

        public MainPage()
        {
            InitializeComponent();
            Unloaded += OnMainPageUnloaded;
            BuildClients();
        }

        private void BuildClients()
        {
            //Hook up a logger for the CIAPI.CS libraries
            LogManager.CreateInnerLogger = (logName, logLevel, showLevel, showDateTime, showLogName, dateTimeFormat)
                         => new SimpleDebugAppender(logName, logLevel, showLevel, showDateTime, showLogName, dateTimeFormat);

            Dispatcher.BeginInvoke(() => listBox1.Items.Add("creating rpc client"));
            RpcClient = new Client(RpcUri, StreamingUri, "CI-WP7");
            RpcClient.BeginLogIn(UerName, Password, ar =>
            {
                try
                {
                    RpcClient.EndLogIn(ar);
                    //RpcClient.MagicNumberResolver.PreloadMagicNumbersAsync();
                    RpcClient.MagicNumberResolver.PreloadMagicNumbers();
                    ThreadPool.QueueUserWorkItem(_ =>
                    {
                        Dispatcher.BeginInvoke(() => listBox1.Items.Add("creating listeners"));
                        StreamingClient = RpcClient.CreateStreamingClient();
                        PriceListener = StreamingClient.BuildPricesListener(MarketId);
                        PriceListener.MessageReceived += OnMessageReceived;

                        Dispatcher.BeginInvoke(() => listBox1.Items.Add("getting account info"));
                        RpcClient.AccountInformation.BeginGetClientAndTradingAccount(ar2 =>
                        {
                            Account = RpcClient.AccountInformation.EndGetClientAndTradingAccount(ar2);

                            Dispatcher.BeginInvoke(() => listBox1.Items.Add("getting market info"));
                            RpcClient.Market.BeginGetMarketInformation(MarketId.ToString(), ar3 =>
                            {
                                Market = RpcClient.Market.EndGetMarketInformation(ar3).MarketInformation;
                                Dispatcher.BeginInvoke(() => Button1.IsEnabled = true);
                            }, null);
                        }, null);
                    });
                }
                catch (Exception ex)
                {
                    Dispatcher.BeginInvoke(() => listBox1.Items.Add("exception caught: " + ex));
                }
            }, null);
        }

        private void OnButton1Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                Button1.IsEnabled = false;
                _listening = true;
                _ordered = false;
            });
        }

        private void OnMessageReceived(object sender, MessageEventArgs<PriceDTO> e)
        {
            if (!_listening || _ordered || Market == null) return;
            _ordered = true;

            var order = new NewStopLimitOrderRequestDTO
            {
                MarketId = e.Data.MarketId,
                AuditId = e.Data.AuditId,
                BidPrice = e.Data.Bid,
                OfferPrice = e.Data.Offer,
                Quantity = Market.WebMinSize.GetValueOrDefault() + 1,
                TradingAccountId = Account.TradingAccounts[0].TradingAccountId,
                Direction = "buy",
                Applicability = "GFD",
                ExpiryDateTimeUTC = DateTime.UtcNow + TimeSpan.FromDays(1)
            };

            Dispatcher.BeginInvoke(() => listBox1.Items.Add("price update arrived, making a new trade"));

            RpcClient.TradesAndOrders.BeginOrder(order, ar =>
            {
                try
                {
                    var result = RpcClient.TradesAndOrders.EndTrade(ar);

                    Dispatcher.BeginInvoke(() =>
                                               {

                                                   RpcClient.MagicNumberResolver.ResolveMagicNumbers(result);
                                                   listBox1.Items.Add(String.Format("trading complete\n\tstatus = {0} {2}\n\tstatus reason = {1} {3}", result.Status, result.StatusReason, result.Status_Resolved, result.StatusReason_Resolved));
                                                   if (result.OrderId > 0)
                                                   {
                                                       listBox1.Items.Add(String.Format("created order {0}", result.OrderId));
                                                   }
                                               });
                }
                catch (Exception ex)
                {
                    Dispatcher.BeginInvoke(() => listBox1.Items.Add("trading failed!"));
                    Dispatcher.BeginInvoke(() => listBox1.Items.Add("exception caught: " + ex));
                }
                finally
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        Button1.IsEnabled = true;
                        _listening = false;
                    });
                }
            }, null);
        }

        private void OnMainPageUnloaded(object sender, RoutedEventArgs e)
        {
            if (StreamingClient == null) return;
            if (PriceListener != null) StreamingClient.TearDownListener(PriceListener);
        }
    }
}