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
using CityIndex.JsonClient;
using Microsoft.Phone.Controls;
using Client = CIAPI.Rpc.Client;

namespace SearchTestApplication
{
    public partial class MainPage : PhoneApplicationPage
    {
        const string ApiUrl = "https://ciapi.cityindex.com/tradingapi/";
        const string StreamingUrl = "https://push.cityindex.com/";
        const string UserName = "DM715257";
        const string Password = "password";



        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        public void FindMarkets()
        {
            var client = new Client(new Uri(ApiUrl));

            LogTextBox.Text = LogTextBox.Text + "Trying to connect.." + Environment.NewLine;

            client.BeginLogIn(UserName, Password,
                              response => Dispatcher.BeginInvoke(new Action(() =>
                              {
                                  client.EndLogIn(response);
                                  LogTextBox.Text = LogTextBox.Text + string.Format("Connected: {0}", !string.IsNullOrEmpty(client.Session)) + Environment.NewLine;
                                  LogTextBox.Text = LogTextBox.Text + string.Format("Call Search Method.")+ Environment.NewLine;

                                  try
                                  {
                                      client.Market.BeginListMarketInformationSearch(
                                          false, // market code not used for search
                                          true, // use market name
                                          false, // iFX markets should always be CFD according to Peter
                                          true,
                                          false,
                                          SearchTextBox.Text,
                                          10,
                                          searchCallBack => Dispatcher.BeginInvoke(() =>
                                                                                       {
                                                                                           {
                                                                                               try
                                                                                               {
                                                                                                   var findedMarkets =
                                                                                                       client.Market.EndListMarketInformationSearch(searchCallBack);

                                                                                                   foreach (var marketInfoDTO in findedMarkets.MarketInformation)
                                                                                                   {

                                                                                                       LogTextBox.Text = LogTextBox.Text + 
                                                                                                           string.Format(marketInfoDTO.MarketId.ToString()) + Environment.NewLine;
                                                                                                   }
                                                                                               }
                                                                                               catch (Exception ex)
                                                                                               {
                                                                                                   LogTextBox.Text = LogTextBox.Text + string.Format("Exception: {0}", ex.Message) + Environment.NewLine;
                                                                                               }

                                                                                           }
                                                                                       }),
                                        null);
                                  }
                                  catch (Exception ex)
                                  {
                                      LogTextBox.Text = LogTextBox.Text + string.Format("Exception: {0}", ex.Message) + Environment.NewLine;
                                  }

                              }), null), null);
        }

        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            FindMarkets();
        }
    }
}