using System;
using System.Windows;
using CIAPI.Streaming;
using CIAPI.Tests;
using Microsoft.Phone.Controls;
using Salient.ReliableHttpClient;
using Client = CIAPI.Rpc.Client;

namespace WindowsPhoneTestApplication
{
    public partial class MainPage : PhoneApplicationPage
    {
 

        private Client client;
        string[] popularMarketIds = new[]
                                       {
                                           "400481134"
                                           , "400481153", "400481142", "400481148", "400481117", "400481146",
                                           "400481147", "400481128",     "400481134"
                                       };

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            CIAPIButton.IsEnabled = false;


            // creating a single rpc client for an application is a better
            // approximation of the intended usage.

            client = new Client(new Uri(StaticTestConfig.RpcUrl), new Uri(StaticTestConfig.StreamingUrl), StaticTestConfig.AppKey);
            listBox1.Items.Add("Trying to connect..");


            client.BeginLogIn(StaticTestConfig.ApiUsername, StaticTestConfig.ApiPassword,
                              response => Dispatcher.BeginInvoke(new Action(() =>
                              {
                                  client.EndLogIn(response);

                                  listBox1.Items.Add(string.Format("Connected: {0}", !string.IsNullOrEmpty(client.Session)));
                                  CIAPIButton.IsEnabled = true;
                              }), null), null);

        }

        public void GetPopularMarketsByCIAPI()
        {
            


            foreach (var popularMarketId in popularMarketIds)
            {
                
                string id = popularMarketId;
                Dispatcher.BeginInvoke(
                    () =>
                        {
                            listBox1.Items.Add(string.Format("BeginGetMarketInformation(MarketId: {0})", id));
                            client.Market.BeginGetMarketInformation(id, asyncResponse =>
                            {
                                try
                                {
                                    var result = client.Market.EndGetMarketInformation(asyncResponse);
                                    var message = string.Format("Market {0} found (Name: {1})", result.MarketInformation.Name,
                                                                result.MarketInformation.MarketId);
                                    Dispatcher.BeginInvoke(() => listBox1.Items.Add(message));

                                }
                                catch (ReliableHttpException ex)
                                {
                                    Dispatcher.BeginInvoke(() => listBox1.Items.Add(string.Format("ApiException: {0}", ex.ResponseText)));
                                }
                            }, null);
                        });
            }
        }

        private void CIAPIButton_Click(object sender, RoutedEventArgs e)
        {
            listBox1.Items.Clear();
            GetPopularMarketsByCIAPI();
        }
    }
}