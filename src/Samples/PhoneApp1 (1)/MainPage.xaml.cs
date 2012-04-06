using System;
using System.Threading;
using System.Windows;
using CIAPI.Tests;
using Salient.ReflectiveLoggingAdapter;
using Client = CIAPI.Rpc.Client;

namespace PhoneApp1
{
    public partial class MainPage
    {
        private const string MarketId = "400481121";
        private const string Period = "MINUTE";
        private const int Interval = 1;
        private const string NumBars = "1";

        private DateTime _start;
        public static Client RpcClient;

     
        private const string AppKey = "testkey-for-PhoneApp1";

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            BuildClients();
        }

        private void BuildClients()
        {
            Dispatcher.BeginInvoke(() => listBox1.Items.Add("creating rpc client"));

            //Hook up a logger for the CIAPI.CS libraries
            LogManager.CreateInnerLogger = (logName, logLevel, showLevel, showDateTime, showLogName, dateTimeFormat)
                                            => new SimpleDebugAppender(logName, logLevel, showLevel, showDateTime, showLogName, dateTimeFormat);

            RpcClient = new Client(new Uri(StaticTestConfig.RpcUrl), new Uri(StaticTestConfig.StreamingUrl),AppKey);
            RpcClient.BeginLogIn(StaticTestConfig.ApiUsername, StaticTestConfig.ApiPassword, ar =>
            {
                RpcClient.EndLogIn(ar);
                Dispatcher.BeginInvoke(() => button1.IsEnabled = true);
            }, null);
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                button1.IsEnabled = false;
                if (_start == DateTime.MinValue) _start = DateTime.Now;
            });
            ThreadPool.QueueUserWorkItem(_ =>
            {
                for (int i = 0; i < 5; ++i)
                {
                    ThreadPool.QueueUserWorkItem(__ =>
                    {
                        for (int j = 1; j <= 525; ++j) PerformRequest();
                    });
                }
                Thread.Sleep(10000);
                if (_start + TimeSpan.FromMinutes(1) < DateTime.Now)
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        listBox1.Items.Add("lucky you, it didn't happen... please try again!");
                        _start = DateTime.MinValue;
                        button1.IsEnabled = true;
                    });
                }
                else
                {
                    button1_Click(sender, e);
                }
            });
        }

        private void PerformRequest()
        {
            new Thread(() => RpcClient.PriceHistory.BeginGetPriceBars(MarketId, Period, Interval, NumBars, ar => RpcClient.PriceHistory.EndGetPriceBars(ar), null)).Start();
        }
    }
}
