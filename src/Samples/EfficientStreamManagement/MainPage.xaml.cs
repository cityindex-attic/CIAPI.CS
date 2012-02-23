using System;
using System.Linq;
using System.Threading;
using System.Windows;
using CIAPI;
using CIAPI.DTO;
using CIAPI.Streaming;
using Salient.JsonClient;
using Microsoft.Phone.Controls;
using Salient.ReflectiveLoggingAdapter;
using StreamingClient;
using Client = CIAPI.Rpc.Client;
using IStreamingClient = CIAPI.Streaming.IStreamingClient;

namespace EfficientStreamManagement
{
    public partial class MainPage : PhoneApplicationPage
    {
        //This is an IFX Markets Test account on live
        //Use FlexITP at : https://trade.loginandtrade.com/tp/fx/
        //Ensure the account has some open positions
        private const string USERNAME = "XX055187"; 
        private const string PASSWORD = "password";
        private const int ACCOUNTOPERATORID = 3321;  //IFX Markets (test)
        private const int PRICEMARKETID = 400481142; // GBP/USD
        public static Client RpcClient;
        public static IStreamingClient StreamingClient;

        private static readonly Uri RPC_URI = new Uri("https://ciapi.cityindex.com/tradingapi");
        private static readonly Uri STREAMING_URI = new Uri("https://push.cityindex.com");
        private IStreamingListener<ClientAccountMarginDTO> _clientAccountMarginsListener;
        private IStreamingListener<NewsDTO> _newsListener;
        private IStreamingListener<OrderDTO> _orderslistener;
        private IStreamingListener<PriceDTO> _priceListener;
        private IStreamingListener<PriceDTO> _defaultPriceListener;
        private IStreamingListener<QuoteDTO> _quotesListener;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            //Hook up a logger for the CIAPI.CS libraries
            LogManager.CreateInnerLogger = (logName, logLevel, showLevel, showDateTime, showLogName, dateTimeFormat)
                         => new SimpleDebugAppender(logName, logLevel, showLevel, showDateTime, showLogName, dateTimeFormat);

            BuildClients();
        }

        private void BuildClients()
        {
            Dispatcher.BeginInvoke(() => listBox1.Items.Add("creating rpc client"));
            RpcClient = new Client(RPC_URI);
            RpcClient.BeginLogIn(USERNAME, PASSWORD, InitializeStreamingClient, null);
        }

        public void InitializeStreamingClient(ApiAsyncResult<ApiLogOnResponseDTO> ar)
        {
            RpcClient.EndLogIn(ar);
            StreamingClient = StreamingClientFactory.CreateStreamingClient(STREAMING_URI, RpcClient.UserName,
                                                                           RpcClient.Session);
            LogToScreen("rpc client logged in");
        }

        private void LogToScreen(string message)
        {
            Dispatcher.BeginInvoke(() =>
                {
                    listBox1.Items.Add(message);
                    listBox1.ScrollIntoView(listBox1.Items[listBox1.Items.Count-1]);
                });

        }


        private void start_CITYINDEXSTREAMING_Click(object sender, RoutedEventArgs e)
        {
            new Thread(() =>
            {
                BuildListener_CITYINDEXSTREAMING_DataAdapterSet();
            }).Start();
        }

        private void start_STREAMINGCLIENTACCOUNT_Click(object sender, RoutedEventArgs e)
        {
            new Thread(() =>
            {
                BuildListener_STREAMINGCLIENTACCOUNT_DataAdapterSet();
            }).Start();
        }

        private void start_STREAMINGTRADINGACCOUNT_Click(object sender, RoutedEventArgs e)
        {
            new Thread(() =>
            {
                BuildListener_STREAMINGTRADINGACCOUNT_DataAdapterSet();
            }).Start();
        }

        private void start_CITYINDEXSTREAMINGDEFAULTPRICES_Click(object sender, RoutedEventArgs e)
        {
            new Thread(() =>
            {
                BuildListener_CITYINDEXSTREAMINGDEFAULTPRICES_DataAdapterSet();
            }).Start();
        }

        private void stop_CITYINDEXSTREAMING_Click(object sender, RoutedEventArgs e)
        {

        }

        private void stop_STREAMINGCLIENTACCOUNT_Click(object sender, RoutedEventArgs e)
        {

        }

        private void stop_CITYINDEXSTREAMINGDEFAULTPRICES_Click(object sender, RoutedEventArgs e)
        {

        }

        private void stop_STREAMINGTRADINGACCOUNT_Click(object sender, RoutedEventArgs e)
        {

        }

        private void stop_ALL_Click(object sender, RoutedEventArgs e)
        {
            TearDownListeners();
        }

        private void TearDownListeners()
        {
            LogToScreen("tearing down all listeners");

            if (_priceListener != null)
            {
                _priceListener.MessageReceived -= PriceListenerMessageReceived;
                StreamingClient.TearDownListener(_priceListener);
            }

            if (_defaultPriceListener != null)
            {
                _defaultPriceListener.MessageReceived -= _defaultPricesListener_MessageReceived;
                StreamingClient.TearDownListener(_defaultPriceListener);
            }

            if (_clientAccountMarginsListener != null)
            {
                _clientAccountMarginsListener.MessageReceived -= ClientAccountMarginsListenerMessageReceived;
                StreamingClient.TearDownListener(_clientAccountMarginsListener);
            }

            if (_newsListener != null)
            {
                _newsListener.MessageReceived -= _newsListener_MessageReceived;
                StreamingClient.TearDownListener(_newsListener);
            }

            if (_orderslistener != null)
            {
                _orderslistener.MessageReceived -= _orderslistener_MessageReceived;
                StreamingClient.TearDownListener(_orderslistener);
            }

            if (_quotesListener != null)
            {
                _quotesListener.MessageReceived -= _quotesListener_MessageReceived;
                StreamingClient.TearDownListener(_quotesListener);
            }

            LogToScreen("all listeners torn down");
        }

        private void BuildListener_CITYINDEXSTREAMING_DataAdapterSet()
        {
            LogToScreen("building listener on CITYINDEXSTREAMING data adapter");
            _priceListener = StreamingClient.BuildPricesListener(PRICEMARKETID);
            _priceListener.MessageReceived += PriceListenerMessageReceived;
            LogToScreen("listener on CITYINDEXSTREAMING built");
        }

        private void BuildListener_CITYINDEXSTREAMINGDEFAULTPRICES_DataAdapterSet()
        {
            LogToScreen("building listener on CITYINDEXSTREAMINGDEFAULTPRICES data adapter");
            _defaultPriceListener = StreamingClient.BuildDefaultPricesListener(ACCOUNTOPERATORID);
            _defaultPriceListener.MessageReceived += _defaultPricesListener_MessageReceived;
            LogToScreen("listener on CITYINDEXSTREAMING built");
        }

        private void BuildListener_STREAMINGCLIENTACCOUNT_DataAdapterSet()
        {
            LogToScreen("building listener on CITYINDEXCLIENTACCOUNT data adapter");
            _clientAccountMarginsListener = StreamingClient.BuildClientAccountMarginListener();
            _clientAccountMarginsListener.MessageReceived += ClientAccountMarginsListenerMessageReceived;
            Dispatcher.BeginInvoke(() => listBox1.Items.Add("listener on CITYINDEXCLIENTACCOUNT built"));
        }

        private void BuildListener_STREAMINGTRADINGACCOUNT_DataAdapterSet()
        {
            LogToScreen("building listener on CITYINDEXTRADINGACCOUNT data adapter");
            _quotesListener = StreamingClient.BuildQuotesListener();
            _quotesListener.MessageReceived += _quotesListener_MessageReceived;
            Dispatcher.BeginInvoke(() => listBox1.Items.Add("listener on CITYINDEXTRADINGACCOUNT built"));
        }

        private void startAllButton_Click(object sender, RoutedEventArgs e)
        {
            new Thread(() =>
            {
                LogToScreen("building all listeners");
                _priceListener = StreamingClient.BuildPricesListener(PRICEMARKETID);
                _priceListener.MessageReceived += PriceListenerMessageReceived;
                _defaultPriceListener = StreamingClient.BuildDefaultPricesListener(ACCOUNTOPERATORID);
                _defaultPriceListener.MessageReceived += _defaultPricesListener_MessageReceived;
                _clientAccountMarginsListener = StreamingClient.BuildClientAccountMarginListener();
                _clientAccountMarginsListener.MessageReceived += ClientAccountMarginsListenerMessageReceived;
                _quotesListener = StreamingClient.BuildQuotesListener();
                _quotesListener.MessageReceived += _quotesListener_MessageReceived;
                LogToScreen("done building all listeners");
            }).Start();
        }


        private void _defaultPricesListener_MessageReceived(object sender, MessageEventArgs<PriceDTO> e) 
        {
            Dispatcher.BeginInvoke(() => lastDefaultPriceTextbox.Text = e.Data.Price.ToString());
        }

        private void _quotesListener_MessageReceived(object sender, MessageEventArgs<QuoteDTO> e)
        {
            Dispatcher.BeginInvoke(() => lastOrderTextbox.Text = e.Data.ToStringWithValues());
        }

        private void _orderslistener_MessageReceived(object sender, MessageEventArgs<OrderDTO> e)
        {
            Dispatcher.BeginInvoke(() => lastOrderTextbox.Text = e.Data.ToStringWithValues());

        }

        private void _newsListener_MessageReceived(object sender, MessageEventArgs<NewsDTO> e)
        {
        }

        private void ClientAccountMarginsListenerMessageReceived(object sender,
                                                                 MessageEventArgs<ClientAccountMarginDTO> e)
        {
            Dispatcher.BeginInvoke(() => lastMarginTextbox.Text = e.Data.Margin.ToString());
        }

        private void PriceListenerMessageReceived(object sender, MessageEventArgs<PriceDTO> e)
        {
            Dispatcher.BeginInvoke(() => lastPrice1TextBox.Text = e.Data.Price.ToString());
        }

        private void marketSearchButton_Click(object sender, RoutedEventArgs e)
        {
            RpcClient.Market.BeginListMarketInformationSearch(false, true, false, true, false, marketSearchTextBox.Text, 10, false, OnMarketSearchResponse, null);
        }

        private void OnMarketSearchResponse(ApiAsyncResult<ListMarketInformationSearchResponseDTO> asyncresult)
        {
            var result = RpcClient.Market.EndListMarketInformationSearch(asyncresult);
            LogToScreen(String.Join(", ", result.MarketInformation.Select(m => m.Name).ToArray()));
        }

    }
}