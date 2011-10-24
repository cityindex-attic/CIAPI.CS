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
using CIAPI.DTO;
using CIAPI.Streaming.Lightstreamer;
using Microsoft.Phone.Controls;

namespace NewStreamingClientTestApp
{
    public partial class MainPage : PhoneApplicationPage
    {
        const string apiUrl = "https://ciapipreprod.cityindextest9.co.uk/TradingApi/";
        const string streamingUrl = "https://pushpreprod.cityindextest9.co.uk";
        const string userName = "xx189949";
        const string password = "password";
        const string topic = "PRICES.PRICE.71442";
        const string dataAdapter = "CITYINDEXSTREAMING";

        // Constructor
        public MainPage()
        {
            InitializeComponent();


            var authenticatedClient = new CIAPI.Rpc.Client(new Uri(apiUrl));
            authenticatedClient.BeginLogIn(userName, password, (ar) =>
                {
                    authenticatedClient.EndLogIn(ar);

                    var client = new LightstreamerClient(new Uri(streamingUrl), authenticatedClient.UserName, authenticatedClient.Session);

                    client.StatusUpdate += client_StatusUpdate;

                    var listener = client.BuildListener<PriceDTO>(dataAdapter, topic);
                    listener.MessageReceived += listener_MessageReceived;
                }, null);


            
        }

        void listener_MessageReceived(object sender, StreamingClient.MessageEventArgs<PriceDTO> e)
        {

        }

        void client_StatusUpdate(object sender, StreamingClient.ConnectionStatusEventArgs e)
        {

        }
    }
}