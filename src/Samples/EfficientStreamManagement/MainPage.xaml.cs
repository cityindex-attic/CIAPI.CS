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
        private const string USERNAME = "XX055187"; 
        private const string PASSWORD = "password";
        private const string AppKey = "testkey-for-EfficientStreamManagement";
        private const int ACCOUNTOPERATORID = 3321;  //IFX Markets (test)
        private int[] PRICEMARKETIDS = new[]{ 400481136, 400481137, 400481138, 400481139, 400481140, 400481141, 400481142 }; //GBP/AUD, GBP/CAD, GBP/CHF, GBP/JPY, GBP/NZD, GBP/SGD, GBP/USD
        private static readonly Uri RPC_URI = new Uri("https://ciapi.cityindex.com/tradingapi");
        private static readonly Uri STREAMING_URI = new Uri("https://push.cityindex.com");

        public static Client RpcClient;
        public static IStreamingClient StreamingClient;
        private IStreamingListener<ClientAccountMarginDTO> _clientAccountMarginsListener;
        private IStreamingListener<PriceDTO> _priceListener;
        private IStreamingListener<PriceDTO> _defaultPriceListener;
        private IStreamingListener<QuoteDTO> _quotesListener;

        #region Setup and helper functions

        public MainPage()
        {
            InitializeComponent();
            //Hook up a logger for the CIAPI.CS libraries
            LogManager.CreateInnerLogger = (logName, logLevel, showLevel, showDateTime, showLogName, dateTimeFormat)
                                           =>
                                           new SimpleDebugAppender(logName, logLevel, showLevel, showDateTime,
                                                                   showLogName, dateTimeFormat);

            BuildClients();
        }

        private void BuildClients()
        {
            Dispatcher.BeginInvoke(() => statusUpdatesListBox.Items.Add("creating rpc client"));
            RpcClient = new Client(RPC_URI, AppKey);
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
                                           statusUpdatesListBox.Items.Add(message);
                                           statusUpdatesListBox.ScrollIntoView(
                                               statusUpdatesListBox.Items[statusUpdatesListBox.Items.Count - 1]);
                                       });
        }

        #endregion

        #region Button Click handlers

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

        private void startAllButton_Click(object sender, RoutedEventArgs e)
        {
            new Thread(() =>
            {
                LogToScreen("building all listeners");
                BuildListener_CITYINDEXSTREAMING_DataAdapterSet();
                BuildListener_CITYINDEXSTREAMINGDEFAULTPRICES_DataAdapterSet();
                BuildListener_STREAMINGCLIENTACCOUNT_DataAdapterSet();
                BuildListener_STREAMINGTRADINGACCOUNT_DataAdapterSet();
                GetMarketInfo();
                LogToScreen("done building all listeners");
            }).Start();
        }

        private void stop_ALL_Click(object sender, RoutedEventArgs e)
        {
            TearDownListeners();
        }

        private void getMarketInfo_Click(object sender, RoutedEventArgs e)
        {
            GetMarketInfo();
        }

        #endregion

        private void BuildListener_CITYINDEXSTREAMING_DataAdapterSet()
        {
            LogToScreen("building listener on CITYINDEXSTREAMING data adapter");
            _priceListener = StreamingClient.BuildPricesListener(PRICEMARKETIDS);
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
            Dispatcher.BeginInvoke(() => statusUpdatesListBox.Items.Add("listener on CITYINDEXCLIENTACCOUNT built"));
        }

        private void BuildListener_STREAMINGTRADINGACCOUNT_DataAdapterSet()
        {
            LogToScreen("building listener on CITYINDEXTRADINGACCOUNT data adapter");
            _quotesListener = StreamingClient.BuildQuotesListener();
            _quotesListener.MessageReceived += _quotesListener_MessageReceived;
            Dispatcher.BeginInvoke(() => statusUpdatesListBox.Items.Add("listener on CITYINDEXTRADINGACCOUNT built"));
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

            if (_quotesListener != null)
            {
                _quotesListener.MessageReceived -= _quotesListener_MessageReceived;
                StreamingClient.TearDownListener(_quotesListener);
            }

            LogToScreen("all listeners torn down");
        }

        private void GetMarketInfo()
        {
            LogToScreen("Getting marketinfo for 7 markets");
            foreach (var marketId in PRICEMARKETIDS)
            {
                RpcClient.Market.BeginGetMarketInformation(marketId.ToString(), OnGetMarketInfoResponse, null);
            }
        }

        private void _defaultPricesListener_MessageReceived(object sender, MessageEventArgs<PriceDTO> e) 
        {
            Dispatcher.BeginInvoke(() => lastDefaultPriceTextbox.Text = e.Data.Price.ToString());
        }

        private void _quotesListener_MessageReceived(object sender, MessageEventArgs<QuoteDTO> e)
        {
            Dispatcher.BeginInvoke(() => lastOrderTextbox.Text = e.Data.ToStringWithValues());
        }

        private void ClientAccountMarginsListenerMessageReceived(object sender,
                                                                 MessageEventArgs<ClientAccountMarginDTO> e)
        {
            Dispatcher.BeginInvoke(() => lastMarginTextbox.Text = e.Data.Margin.ToString());
        }

        private void PriceListenerMessageReceived(object sender, MessageEventArgs<PriceDTO> e)
        {
            Dispatcher.BeginInvoke(() =>
                                       {
                                           if (PRICEMARKETIDS[1] == e.Data.MarketId)
                                           {
                                               lastPrice1TextBox.Text = e.Data.Price.ToString();
                                           }
                                           if (PRICEMARKETIDS[2] == e.Data.MarketId)
                                           {
                                               lastPrice2TextBox.Text = e.Data.Price.ToString();
                                           }
                                           if (PRICEMARKETIDS[3] == e.Data.MarketId)
                                           {
                                               lastPrice3TextBox.Text = e.Data.Price.ToString();
                                           }
                                           if (PRICEMARKETIDS[4] == e.Data.MarketId)
                                           {
                                               lastPrice4TextBox.Text = e.Data.Price.ToString();
                                           }
                                           if (PRICEMARKETIDS[5] == e.Data.MarketId)
                                           {
                                               lastPrice5TextBox.Text = e.Data.Price.ToString();
                                           }
                                       });
        }
        
        private void OnGetMarketInfoResponse(ApiAsyncResult<GetMarketInformationResponseDTO> asyncresult)
        {
            var result = RpcClient.Market.EndGetMarketInformation(asyncresult);
            LogToScreen(string.Format("{0}->{1}", result.MarketInformation.MarketId, result.MarketInformation.Name));
        }
    }
}