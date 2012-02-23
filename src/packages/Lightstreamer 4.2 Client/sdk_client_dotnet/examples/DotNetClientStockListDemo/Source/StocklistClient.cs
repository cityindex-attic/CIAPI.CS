using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using Lightstreamer.DotNet.Client;
using System.Threading;

namespace DotNetStockListDemo
{
    
    class StocklistClient
    {
        private DemoForm demoForm;
        private LightstreamerUpdateDelegate updateDelegate;
        private LightstreamerStatusChangedDelegate statusChangeDelegate;

        private LSClient client;
        private ConnectionInfo cInfo;

        public StocklistClient(
                string pushServerUrl,
                DemoForm form,
                LightstreamerUpdateDelegate lsUpdateDelegate,
                LightstreamerStatusChangedDelegate lsStatusChangeDelegate)
        {
            
            demoForm = form;
            updateDelegate = lsUpdateDelegate;
            statusChangeDelegate = lsStatusChangeDelegate;

            cInfo = new ConnectionInfo();
            cInfo.PushServerUrl = pushServerUrl;
            cInfo.Adapter = "DEMO";

            client = new LSClient();
        }


        private int phase = 0;

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

        public void Start()
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

        private void execute(int ph) {
            lock (this)
            {
                if (ph != this.phase)
                {
                    return;
                }
                this.phase++;
                this.connect();
                this.subscribe();
            }
        }

        public void StatusChanged(int ph, int cStatus, string status)
        {
            lock(this) {
                if (ph != this.phase)
                {
                    return;
                }

                demoForm.Invoke(statusChangeDelegate, new Object[] { cStatus, status });
            }
        }

        public void UpdateReceived(int ph, int itemPos, IUpdateInfo update)
        {
            lock (this)
            {
                if (ph != this.phase)
                {
                    return;
                }

                demoForm.Invoke(updateDelegate, new Object[] { itemPos, update });
            }
        }
    
        private void connect() {
            lock(this) {
                
                bool connected = false;
                //this method will not exit until the openConnection returns without throwing an exception
                while (!connected) {
                    demoForm.Invoke(statusChangeDelegate, new Object[] { StocklistConnectionListener.VOID, "Connecting to Lightstreamer Server @ " + cInfo.PushServerUrl });
                    try {
                        this.phase++;
                        client.OpenConnection(this.cInfo, new StocklistConnectionListener(this, this.phase));
                        connected = true;
                    } catch (PushConnException e) {
                        demoForm.Invoke(statusChangeDelegate, new Object[] { StocklistConnectionListener.VOID, e.Message });
                    } catch (PushServerException e) {
                        demoForm.Invoke(statusChangeDelegate, new Object[] { StocklistConnectionListener.VOID, e.Message });
                    } catch (PushUserException e) {
                        demoForm.Invoke(statusChangeDelegate, new Object[] { StocklistConnectionListener.VOID, e.Message });
                    }
            
                    if (!connected) {
                        Thread.Sleep(5000);
                    }
                 }
            }
        }

        private void subscribe() {
            lock (this)
            {
                //this method will try just one subscription.
                //we know that when this method executes we should be already connected
                //If we're not or we disconnect while subscribing we don't have to do anything here as an
                //event will be (or was) sent to the ConnectionListener that will handle the case.
                //If we're connected but the subscription fails we can't do anything as the same subscription 
                //would fail again and again (btw this should never happen)

                try
                {
                    SimpleTableInfo tableInfo = new SimpleTableInfo(
                        "item1 item2 item3 item4 item5 item6 item7 item8 item9 item10 item11 item12 item13 item14 item15 item16 item17 item18 item19 item20 item21 item22 item23 item24 item25 item26 item27 item28 item29 item30",
                        "MERGE",
                        "stock_name last_price time pct_change bid_quantity bid ask ask_quantity min max ref_price open_price",
                        true);
                    tableInfo.DataAdapter = "QUOTE_ADAPTER";

                    client.SubscribeTable(
                        tableInfo,
                        new StocklistHandyTableListener(this, this.phase),
                        false);

                }
                catch (SubscrException)
                {
                }
                catch (PushServerException)
                {
                }
                catch (PushUserException)
                {
                }
                catch (PushConnException)
                {
                }
            }
        }

    }

}
