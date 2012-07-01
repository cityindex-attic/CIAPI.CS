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
using Microsoft.Phone.Controls;
using Lightstreamer.DotNet.Client;
using System.Diagnostics;
using System.Windows.Media.Imaging;

namespace WindowsPhone7Demo
{
    public partial class MainPage : PhoneApplicationPage, ILightstreamerListener
    {

        class UpdateCell
        {

            TextBlock tb;
            Storyboard sbUp, sbDown, sbSame;

            public UpdateCell(TextBlock tb, Storyboard sbUp, Storyboard sbDown, Storyboard sbSame)
            {
                this.tb = tb;
                this.sbUp = sbUp;
                this.sbDown = sbDown;
                this.sbSame = sbSame;
            }

            public void SetText(string message)
            {
                tb.Dispatcher.BeginInvoke(() =>
                {
                    tb.Text = message;
                });
            }

            public void AnimateUp()
            {
                sbUp.Dispatcher.BeginInvoke(() =>
                {
                    sbUp.Begin();
                });
            }

            public void AnimateDown()
            {
                sbDown.Dispatcher.BeginInvoke(() =>
                {
                    sbDown.Begin();
                });
            }

            public void AnimateSame()
            {
                sbSame.Dispatcher.BeginInvoke(() =>
                {
                    sbSame.Begin();
                });
            }

            public void Animate(IUpdateInfo update, string field, Dictionary<int, UpdateCell> TextMap)
            {
                string oldval = update.GetOldValue(field);
                string newval = update.GetNewValue(field);
                int action = 0;
                try
                {
                    if (Convert.ToDouble(oldval) > Convert.ToDouble(newval))
                        action = -1;
                    else
                        action = 1;
                } catch (FormatException) {
                    // ignore
                }
                foreach (UpdateCell cell in TextMap.Values)
                {
                    if (action > 0)
                        cell.AnimateUp();
                    else if (action < 0)
                        cell.AnimateDown();
                    else
                        cell.AnimateSame();
                }
            }

        }

        private Dictionary<int, Dictionary<int, UpdateCell>> RowMap = new Dictionary<int, Dictionary<int, UpdateCell>>();

        public static Boolean wantsConnection = true;

        public MainPage()
        {
            InitializeComponent();
            InitializeTable();
            App.SetListener(this);
            AnimateCellsStalling();
        }

        private void InitializeTable()
        {
            Color toUse = (Visibility)Application.Current.Resources["PhoneLightThemeVisibility"] == Visibility.Visible ? Colors.Black : Colors.White;

            Uri iconUri = new Uri("/WP7StockListDemo;component/res/" + (toUse == Colors.White ? "lslogo.png" : "lslogo_black.png"), 
                UriKind.Relative);
            BitmapImage iconSource = new BitmapImage(iconUri);
            logoImage.Source = iconSource;


            // Add TextBlocks to Cells
            for (int i = 0; i < ContentPanel.RowDefinitions.Count; i++)
            {
                if (i == 0)
                    // title row
                    continue;

                Dictionary<int, UpdateCell> TextMap = new Dictionary<int, UpdateCell>();
                RowMap.Add(i - 1, TextMap);

                // for each column
                for (int y = 0; y < ContentPanel.ColumnDefinitions.Count; y++)
                {
                    TextBlock tb = new TextBlock();
                    if (i > 1)
                        tb.Text = "--";
                    SolidColorBrush colorB = new SolidColorBrush(toUse);
                    tb.Foreground = colorB;
                    Grid.SetRow(tb, i - 1);
                    Grid.SetColumn(tb, y);
                    Storyboard sbUp = new Storyboard();
                    Storyboard sbDown = new Storyboard();
                    Storyboard sbSame = new Storyboard();

                    // highlight color animation (in case of positive update)
                    ColorAnimation colorUp = new ColorAnimation();
                    colorUp.From = toUse;
                    colorUp.To = Colors.Green;
                    colorUp.AutoReverse = true;
                    colorUp.Duration = new Duration(TimeSpan.FromSeconds(0.6));
                    Storyboard.SetTarget(colorUp, tb.Foreground);
                    Storyboard.SetTargetProperty(colorUp, new PropertyPath("Color"));
                    sbUp.Children.Add(colorUp);

                    // highlight color animation (in case of negative update)
                    ColorAnimation colorDown = new ColorAnimation();
                    colorDown.From = toUse;
                    colorDown.To = Colors.Red;
                    colorDown.AutoReverse = true;
                    colorDown.Duration = new Duration(TimeSpan.FromSeconds(0.6));
                    Storyboard.SetTarget(colorDown, tb.Foreground);
                    Storyboard.SetTargetProperty(colorDown, new PropertyPath("Color"));
                    sbDown.Children.Add(colorDown);

                    // highlight color animation (in case of stable)
                    ColorAnimation colorSame = new ColorAnimation();
                    colorSame.From = toUse;
                    colorSame.To = Colors.Orange;
                    colorSame.AutoReverse = true;
                    colorSame.Duration = new Duration(TimeSpan.FromSeconds(0.6));
                    Storyboard.SetTarget(colorSame, tb.Foreground);
                    Storyboard.SetTargetProperty(colorSame, new PropertyPath("Color"));
                    sbSame.Children.Add(colorSame);

                    UpdateCell cell = new UpdateCell(tb, sbUp, sbDown, sbSame);
                    TextMap.Add(y, cell);
                    ContentPanel.Children.Add(tb);
                    
                }
            }
        }

        public void UpdateStatus(int status, string message)
        {
            string icon = null;
            if (status == LightstreamerConnectionHandler.DISCONNECTED)
            {
                icon = "status_disconnected.png";
            }
            else if (status == LightstreamerConnectionHandler.STALLED)
            {
                icon = "status_stalled.png";
            }
            else if (status == LightstreamerConnectionHandler.STREAMING)
            {
                icon = "status_connected_streaming.png";
            }
            else if (status == LightstreamerConnectionHandler.POLLING)
            {
                icon = "status_connected_polling.png";
            }
            else if (status == LightstreamerConnectionHandler.CONNECTING)
            {
                icon = "status_disconnected.png";
            }
            else if (status == LightstreamerConnectionHandler.ERROR) {
                //we may show it somewhere
            }
            if (icon != null)
            {
                StatusImage.Dispatcher.BeginInvoke(() =>
                {
                    Uri iconUri = new Uri("/WP7StockListDemo;component/res/" + icon,
                        UriKind.Relative);
                    BitmapImage iconSource = new BitmapImage(iconUri);
                    StatusImage.Source = iconSource;
                });
            }

            if (message != null)
            {
                StatusLabel.Dispatcher.BeginInvoke(() =>
                {
                    StatusLabel.Text = message;
                });
            }
        }

        public void OnStatusChange(int phase, int status, string message)
        {
            if (!App.checkPhase(phase))
            {
                return;
            }
            UpdateStatus(status, message);
            // Debug.WriteLine("OnStatusChange: " + status + ", " + message);
        }

        public void OnItemUpdate(int phase, int itemPos, string itemName, IUpdateInfo update)
        {
            if (!App.checkPhase(phase))
            {
                return;
            }
            Dictionary<int, UpdateCell> TextMap;
            if (RowMap.TryGetValue(itemPos, out TextMap))
            {

                for (int i = 0; i < App.fields.Length; i++)
                {
                    string field = App.fields[i];
                    if (update.IsValueChanged(field) || update.Snapshot)
                    {
                        UpdateCell cell;
                        if (TextMap.TryGetValue(i, out cell))
                        {
                            cell.SetText(update.GetNewValue(field));
                            if (field.Equals("last_price"))
                                cell.Animate(update, field, TextMap);
                        }
                    }
                }

            }

            Debug.WriteLine("OnItemUpdate: " + itemName + ", " + update + ", pos: " + itemPos);
        }

        public void OnLostUpdate(int phase, int itemPos, string itemName, int lostUpdates)
        {
            if (!App.checkPhase(phase))
            {
                return;
            }
            // Debug.WriteLine("OnLostUpdate: " + itemName + ", lost updates: " + lostUpdates);
        }

        public void OnReconnectRequest(int phase)
        {
            if (!App.checkPhase(phase))
            {
                return;
            }
            Debug.WriteLine("OnReconnectRequest called");
            App.SpawnLightstreamerClientStart();
        }

        private void AnimateCellsStalling()
        {
            foreach (Dictionary<int, UpdateCell> map in RowMap.Values)
            {
                foreach (UpdateCell cell in map.Values)
                {
                    cell.AnimateSame();
                }
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            wantsConnection = !wantsConnection;
            App.UserClick(wantsConnection);
            if (wantsConnection)
            {
                button1.Content = "Stop";
            }
            else
            {
                button1.Content = "Start";
            }
            AnimateCellsStalling();
        }

    }
}