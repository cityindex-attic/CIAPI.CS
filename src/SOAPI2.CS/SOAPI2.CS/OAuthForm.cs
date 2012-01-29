using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SOAPI2
{
    public partial class OAuthForm : Form
    {
        public OAuthForm()
        {
            InitializeComponent();
            webBrowser1.Navigated += new WebBrowserNavigatedEventHandler(webBrowser1_Navigated);
            webBrowser1.Navigate(new Uri("https://stackexchange.com/oauth/dialog?client_id=66&redirect_uri=https://stackexchange.com/oauth/login_success"));

        }

        void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            Console.WriteLine(e.Url.ToString());
            var match = Regex.Match(e.Url.ToString(), "https://stackexchange.com/oauth/login_success#access_token=.?&expires=\\d+");
            if(match.Success)
            {
                
            }
        }
    }
}
