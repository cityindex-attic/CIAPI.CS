using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using CIAPI;
using CityIndex.JsonClient;
using CIAPI.DTO;


namespace WinFormsSpike
{
    public partial class Form1 : Form
    {
        ApiClient _ctx;

        public Form1()
        {
            InitializeComponent();
        }


        private void Form1Load(object sender, EventArgs e)
        {
            ApiEndpointTextBox.Text = TestConfig.RpcUrl;
            UidTextBox.Text = TestConfig.ApiUsername;
            PwdTextBox.Text = TestConfig.ApiPassword;
            NewsHeadlinesGridView.SelectionChanged += NewsHeadlinesGridViewSelectionChanged;
            MainTabControl.Enabled = false;
            LogOutButton.Enabled = false;
            LoginButton.Enabled = true;
        }


        #region Authentication
        private void LoginButtonClick(object sender, EventArgs e)
        {
            DisableUi();
            _ctx = new ApiClient(new Uri(ApiEndpointTextBox.Text));
            _ctx.BeginLogIn(UidTextBox.Text, PwdTextBox.Text, result =>
                {
                    try
                    {

                        _ctx.EndLogIn(result);
                        Invoke(() =>
                        {
                            MainTabControl.Enabled = true;
                            LogOutButton.Enabled = true;
                            LoginButton.Enabled = false;
                            Application.DoEvents();

                        });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        Cursor = Cursors.Default;
                    }
                }, null);

        }

        private void LogOutButtonClick(object sender, EventArgs e)
        {
            DisableUi();
            _ctx.BeginLogOut(result =>
                {
                    try
                    {
                        _ctx.EndLogOut(result);
                        Invoke(() =>
                        {
                            MainTabControl.Enabled = false;
                            LogOutButton.Enabled = false;
                            LoginButton.Enabled = true;
                            Application.DoEvents();
                        });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        Cursor = Cursors.Default;
                    }

                }, null);
        }

        #endregion

        #region News

        private void ListNewsHeadlinesButtonClick(object sender, EventArgs e)
        {
            new Thread(() =>
            {
                var response = _ctx.ListNewsHeadlines(CategoryTextBox.Text, (int)MaxResultsNumericUpDown.Value);
                Invoke(() =>
                {
                    NewsDTOBindingSource.DataSource = response.Headlines;

                });

            }).Start();

        }


        void NewsHeadlinesGridViewSelectionChanged(object sender, EventArgs e)
        {
            var item = (NewsDTO)NewsDTOBindingSource.Current;

            _ctx.BeginGetNewsDetail(item.StoryId.ToString(), r =>
            {
                var response = _ctx.EndGetNewsDetail(r);
                Invoke(() => DisplayHtml(response.NewsDetail.Story, NewsDetailWebBrowser));

            }, null);
        }

        #endregion

        #region Plumbing
        /// <summary>
        /// Simple helper method that cleans up the calling of Control.Invoke
        /// </summary>
        /// <param name="action"></param>
        private void Invoke(Action action)
        {
            base.Invoke(action);
        }


        /// <summary>
        /// Typically call during authentication methods
        /// </summary>
        private void DisableUi()
        {

            Invoke(() =>
                {
                    Cursor = Cursors.WaitCursor;
                    MainTabControl.Enabled = false;
                    LogOutButton.Enabled = false;
                    LoginButton.Enabled = false;
                    Application.DoEvents();
                });
        }

        /// <summary>
        /// this is a quick hack to get data in a WebBrowser control. 
        /// You may write directly to the document but the code is more involved 
        /// and is beyond the scope of this demo
        /// </summary>
        /// <param name="html"></param>
        /// <param name="browser"></param>
        private static void DisplayHtml(string html, WebBrowser browser)
        {


            string filePath = Path.GetFullPath("data.html");
            File.WriteAllText(filePath, html);

            browser.Navigate(filePath);
        }

        #endregion


    }
}
