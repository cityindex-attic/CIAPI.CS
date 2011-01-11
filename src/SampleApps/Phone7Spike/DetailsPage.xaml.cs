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
using System.Windows.Navigation;
using Microsoft.Phone.Controls;

namespace Phone7Spike
{
    public partial class DetailsPage : PhoneApplicationPage
    {
        // Constructor
        public DetailsPage()
        {
            InitializeComponent();
        }
        private string FetchBackgroundColor()
        {
            return IsBackgroundBlack() ? "#000;" : "#fff";
        }

        private string FetchFontColor()
        {
            return IsBackgroundBlack() ? "#fff;" : "#000";
        }

        private static bool IsBackgroundBlack()
        {
            return FetchBackGroundColor() == "#FF000000";
        }

        private static string FetchBackGroundColor()
        {
            string color;
            Color mc =
              (Color)Application.Current.Resources["PhoneBackgroundColor"];
            color = mc.ToString();
            return color;
        }
        private void SetBackground()
        {
            Color mc =
              (Color)Application.Current.Resources["PhoneBackgroundColor"];
            webBrowser1.Background = new SolidColorBrush(mc);
        }
        // When page is navigated to set data context to selected item in list
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            string selectedIndex = "";
            if (NavigationContext.QueryString.TryGetValue("selectedItem", out selectedIndex))
            {
                int index = int.Parse(selectedIndex);
                ItemViewModel item = App.ViewModel.Items[index];
                DataContext = item;


            }
        }

        /// <summary>
        /// from http://blogs.msdn.com/b/mikeormond/archive/2010/12/16/displaying-html-content-in-windows-phone-7.aspx
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void webBrowser1_Loaded(object sender, RoutedEventArgs e)
        {
            string fontColor = FetchFontColor();
            string backgroundColor = FetchBackgroundColor();

            SetBackground();

            App.ctx.BeginGetNewsDetail(((ItemViewModel)DataContext).StoryId.ToString(), result =>
            {
                var response = App.ctx.EndGetNewsDetail(result);

                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    ((ItemViewModel)DataContext).Story = response.NewsDetail.Story;
  

                    var html = response.NewsDetail.Story;



                    var htmlConcat = string.Format("<html><head>{0}</head>" +
                      "<body style=\"margin:0px;padding:0px;background-color:{3};\" " +
                      "onLoad=\"SendDataToPhoneApp()\">" +
                      "<div id=\"pageWrapper\" style=\"width:100%; color:{2}; " +
                      "background-color:{3}\">{1}</div></body></html>",
                      "",
                      html,
                      fontColor,
                      backgroundColor);
                    webBrowser1.NavigateToString(htmlConcat);

                });

            }, null);



        }


    }
}