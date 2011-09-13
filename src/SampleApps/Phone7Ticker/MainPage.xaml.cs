using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using CIAPI.Rpc;
using CIAPI.Streaming;
using Microsoft.Phone.Controls;

namespace Phone7Ticker
{
    public partial class MainPage : PhoneApplicationPage
    {
        private const string pushServerHost = "https://push.cityindex.com";
        private const string rpcServerHost = "https://ciapi.cityindex.com/tradingapi";
        private const string userName = "DM925308";
        private const string password = "password";
        private string sessionId = "";
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            var rpcClient = new Client(new Uri(rpcServerHost));
            rpcClient.BeginLogIn(userName, password, ar =>
                {
                    rpcClient.EndLogIn(ar);
                    sessionId = rpcClient.Session;
                    Dispatcher.BeginInvoke(() =>
                        {
                            listBox1.Items.Add("logged in");
                        });
                    var streamingClient = StreamingClientFactory.CreateStreamingClient(new Uri(pushServerHost), userName, sessionId);
                    streamingClient.Connect();
                    var listener = streamingClient.BuildPricesListener(400535967 ,81136, 400509294, 400535971, 80902, 400509295, 400193864 ,400525367, 80926, 400498641, 400193866 ,91047 ,400194551, 121766, 400172033 ,139144);
                    listener.MessageReceived += listener_MessageReceived;
                    listener.Start();

                }, null);

        }

        void listener_MessageReceived(object sender, StreamingClient.MessageEventArgs<CIAPI.DTO.PriceDTO> e)
        {

            Dispatcher.BeginInvoke(() =>
            {
                listBox1.Items.Add(e.Data.MarketId);
            });
        }

        
    }
}