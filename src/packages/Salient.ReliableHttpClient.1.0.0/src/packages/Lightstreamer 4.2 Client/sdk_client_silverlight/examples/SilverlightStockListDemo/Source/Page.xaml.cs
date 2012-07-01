using System.Windows;
using System.Windows.Controls;
using System.Threading;

using Lightstreamer.DotNet.Client;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System;
using System.Collections.Generic;

namespace SilverlightDemo
{
    public partial class Page : UserControl
    {
        public static ImageSource DISCONNECTED = new BitmapImage(new Uri("/SilverlightDemo;component/images/status_disconnected.png", UriKind.Relative));
        public static ImageSource STREAMING = new BitmapImage(new Uri("/SilverlightDemo;component/images/status_connected_streaming.png", UriKind.Relative));
        public static ImageSource POLLING = new BitmapImage(new Uri("/SilverlightDemo;component/images/status_connected_polling.png", UriKind.Relative));
        public static ImageSource STALLED = new BitmapImage(new Uri("/SilverlightDemo;component/images/status_stalled.png", UriKind.Relative));

        private LSClient client = new LSClient();
        private int phase = 0;

        private ConnectionInfo connInfo;

        private List<Stock> gridModel;

        SynchronizationContext syncContext;
        
        private int mexNum = 0;

        public Page(string LSHost)
        {
            InitializeComponent();

        
            connInfo = new ConnectionInfo();
            connInfo.PushServerUrl = LSHost;
            connInfo.Adapter = "DEMO";

            syncContext = SynchronizationContext.Current;

            this.Start();
       }

       private void cleanModel(object data)
       {
           gridModel = new List<Stock>();
           for (int i = 0; i < 30; i++)
           {
               Stock toAdd = new Stock();
               toAdd.StockName = "-";
               gridModel.Add(toAdd);
           }
           myStockList.SelectedIndex = -1;
           myStockList.ItemsSource = gridModel;
       }
        

        public void Start(int ph)
        {
            lock (this)
            {
                if (ph != this.phase)
                {
                    //we ignore old calls
                    return;
                }
                this.Start();
            }
        }

        private void Start()
        {
            lock (this)
            {
                int ph = ++this.phase;

                Thread t = new Thread(new ThreadStart(delegate()
                {
                    execute(ph);
                }));
                t.Start();
            }
        }

        private void execute(int ph)
        {
            lock (this)
            {
                if (ph != this.phase)
                {
                    return;
                }
                this.phase++;
                this.Connect();
                this.Subscribe();

            }
        }


        

        private void Connect()
        {
            lock (this)
            {

                bool connected = false;
                while (!connected)
                {
                    this.StatusChanged(this.phase, "Connecting to Lightstreamer Server @ " + connInfo.PushServerUrl, DISCONNECTED);

                    /// <exception cref="PushConnException">in case of connection problems.</exception>
                    /// <exception cref="PushServerException">in case of errors in the supplied
                    /// parameters or in Server answer.
                    /// In normal operation, this should not happen.</exception>
                    /// <exception cref="PushUserException">in case the connection was refused
                    /// after the checks in Lightstreamer Server Metadata Adapter.</exception>
                    try
                    {
                        this.phase++;
                        client.OpenConnection(this.connInfo, new StocklistConnectionListener(this, this.phase));
                        connected = true;
                    }
                    catch (PushConnException e)
                    {
                        this.StatusChanged(this.phase, "Connection to Server failed: " + e.Message, null);
                    }
                    catch (PushServerException e)
                    {
                        this.StatusChanged(this.phase, "Unexpected error: " + e.Message, null);
                    }
                    catch (PushUserException e)
                    {
                        this.StatusChanged(this.phase, "Request refused: " + e.Message, null);
                    }

                    if (!connected)
                    {
                        Thread.Sleep(5000);
                    }
                }

            }
        }

        private void Subscribe()
        {

            lock (this)
            {

                this.syncContext.Send(this.cleanModel,"");

                //this method will try just one subscription.
                //we know that when this method executes we should be already connected
                //If we're not or we disconnect while subscribing we don't have to do anything here as an
                //event will be (or was) sent to the ConnectionListener that will handle the case.
                //If we're connected but the subscription fails we can't do anything as the same subscription 
                //would fail again and again (btw this should never happen)

                try
                {
                    ExtendedTableInfo tableInfo = new ExtendedTableInfo(
                        new string[] { "item1", "item2", "item3", "item4", "item5", "item6", "item7", "item8", "item9", "item10", "item11", "item12", "item13", "item14", "item15", "item16", "item17", "item18", "item19", "item20", "item21", "item22", "item23", "item24", "item25", "item26", "item27", "item28", "item29", "item30" },
                        "MERGE",
                        new string[] { "stock_name", "last_price", "time", "pct_change", "bid_quantity", "bid", "ask", "ask_quantity", "min", "max", "ref_price", "open_price" },
                        true);
                    tableInfo.DataAdapter = "QUOTE_ADAPTER";

                    client.SubscribeTable(
                        tableInfo,
                        new StocklistHandyTableListener(this, this.phase, gridModel),
                        false);

                }
                catch (SubscrException e)
                {
                    this.StatusChanged(this.phase, "No session is active: " + e.Message, null);
                }
                catch (PushServerException e)
                {
                    this.StatusChanged(this.phase, "Unexpected error: " + e.Message, null);
                }
                catch (PushUserException e)
                {
                    this.StatusChanged(this.phase, "Request refused: " + e.Message, null);
                }
                catch (PushConnException e)
                {
                    this.StatusChanged(this.phase, "Control connection failed: " + e.Message, null);
                }
            }

        }
       

        

        public void StatusChanged(int ph, string status, ImageSource newImg)
        {
            lock (this)
            {
                if (ph != this.phase)
                {
                    return;
                }

                this.syncContext.Post(this.setComment, status);
                if (newImg != null)
                {
                    this.syncContext.Post(this.setWidget, newImg);
                }
            }
        }

        private void setWidget(object data)
        {
            statusImg.Source = (ImageSource) data;
        }
        private void setComment(object data)
        {
            myComment.Text = (++mexNum) + " | " + (string)data + "\n" + myComment.Text;
        }

        public void setData(int ph, Stock data)
        {
            lock (this)
            {
                if (ph != this.phase)
                {
                    return;
                }
                this.syncContext.Post(this.setGrid, data);
                
            }
        }

        private void setGrid(object data)
        {
            ((Stock) data).NotifyAllChanged();
        }


        private void myButton_Click(object sender, RoutedEventArgs e)
        {
            myComment.Text = "";
        }

    }
}
