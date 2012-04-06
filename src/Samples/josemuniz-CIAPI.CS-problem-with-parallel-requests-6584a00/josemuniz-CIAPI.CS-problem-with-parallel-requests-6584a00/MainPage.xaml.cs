using System;
using System.Linq;
using System.Threading;
using System.Windows;
using CIAPI.DTO;
using CIAPI.Tests;
using Client = CIAPI.Rpc.Client;

namespace PhoneApp2
{
    public partial class MainPage
    {
        //private const string USERNAME = "0x160";
        //private const string PASSWORD = "password";
        private const int MarketId = 400494226;

        public static Client RpcClient;

        //private static readonly Uri RPC_URI = new Uri("https://ciapipreprod.cityindextest9.co.uk/TradingApi");

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            BuildClients();
        }

        private void BuildClients()
        {
            Dispatcher.BeginInvoke(() => listBox1.Items.Add("creating rpc client"));
            RpcClient = new Client(new Uri(StaticTestConfig.RpcUrl), new Uri(StaticTestConfig.StreamingUrl), StaticTestConfig.AppKey);
            RpcClient.BeginLogIn(StaticTestConfig.ApiUsername, StaticTestConfig.ApiPassword, ar =>
            {
                RpcClient.EndLogIn(ar);
                Dispatcher.BeginInvoke(() => button1.IsEnabled = true);
            }, null);
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(() => button1.IsEnabled = false);

            var evt = new ManualResetEvent(false);

            Dispatcher.BeginInvoke(() => listBox1.Items.Add("getting watchlists..."));

            RpcClient.Watchlist.BeginGetWatchlists(ar =>
            {
                var result = RpcClient.Watchlist.EndGetWatchlists(ar);

                Dispatcher.BeginInvoke(() => listBox1.Items.Add(String.Format("{0} watchlists found", result.ClientAccountWatchlists.Length)));

                Dispatcher.BeginInvoke(() => listBox1.Items.Add("removing the market from all sequentially..."));
                foreach (var watchlist in result.ClientAccountWatchlists)
                {
                    var detail = watchlist.Items.FirstOrDefault(i => i.MarketId == MarketId);

                    if (detail == null) continue;

                    watchlist.Items = watchlist.Items.Where(i => i != detail).ToArray();

                    evt.Reset();
                    RpcClient.Watchlist.BeginSaveWatchlist(new ApiSaveWatchlistRequestDTO
                    {
                        Watchlist = watchlist
                    }, ar2 =>
                    {
                        RpcClient.Watchlist.EndSaveWatchlist(ar2);
                        evt.Set();
                    }, null);
                    evt.WaitOne();
                }

                Dispatcher.BeginInvoke(() => listBox1.Items.Add("adding the market to all in parallel..."));
                foreach (var watchlist in result.ClientAccountWatchlists)
                {
                    watchlist.Items = watchlist.Items.Concat(new[]
                    {
                        new ApiClientAccountWatchlistItemDTO
                        {
                            MarketId = MarketId,
                            DisplayOrder = watchlist.Items.Count(),
                            WatchlistId = watchlist.WatchlistId
                        }
                    }).ToArray();

                    RpcClient.Watchlist.BeginSaveWatchlist(new ApiSaveWatchlistRequestDTO
                    {
                        Watchlist = watchlist
                    }, ar2 => RpcClient.Watchlist.EndSaveWatchlist(ar2), null);
                }

                Dispatcher.BeginInvoke(() => listBox1.Items.Add("waiting for completion..."));
                Thread.Sleep(5000);
                
                Dispatcher.BeginInvoke(() =>
                {
                    listBox1.Items.Add("getting watchlists again...");
                    RpcClient.Watchlist.BeginGetWatchlists(ar3 =>
                    {
                        var result2 = RpcClient.Watchlist.EndGetWatchlists(ar3);
                        int haveIt = result2.ClientAccountWatchlists.Count(wl => wl.Items.Any(i => i.MarketId == MarketId));

                        if (result2.ClientAccountWatchlists.Length == haveIt)
                        {
                            Dispatcher.BeginInvoke(() => listBox1.Items.Add("all the watchlists have the market :)"));
                        }
                        else
                        {
                            Dispatcher.BeginInvoke(() => listBox1.Items.Add(String.Format("only {0} of {1} watchlists have the market :(", haveIt, result2.ClientAccountWatchlists.Length)));
                        }
                        Dispatcher.BeginInvoke(() => button1.IsEnabled = true);
                    }, null);
                });
            }, null);
        }
    }
}
