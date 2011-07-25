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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SilverlightSpike
{
    public partial class Home : Page
    {
        public Home()
        {
            InitializeComponent();
   

            
            
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            List<string> datasource;

            App.ctx.BeginLogIn("CC735158", "password", loginResult =>
            {
                App.ctx.EndLogIn(loginResult);

                App.ctx.News.BeginListNewsHeadlines("UK", 20, headlinesResult =>
                {
                    var response = App.ctx.News.EndListNewsHeadlines(headlinesResult);
                    datasource = response.Headlines.Select(item => item.Headline).ToList();

                    Dispatcher.BeginInvoke(() =>
                    {

                        listBox1.ItemsSource = datasource;
                    });

                }, null);
            }, null);
        }
    }
}