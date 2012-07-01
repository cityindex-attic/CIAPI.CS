using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Globalization;
using System.Runtime.InteropServices;

using Lightstreamer.DotNet.Client;

using DotNetStockListDemo.Properties;

namespace DotNetStockListDemo
{

    public delegate void LightstreamerUpdateDelegate(int item, IUpdateInfo values);
    public delegate void LightstreamerStatusChangedDelegate(int cStatus, string status);

    public delegate void StopDelegate();
    public delegate void BlinkOffDelegate(DataGridViewCell[] cells);

    public partial class DemoForm : Form, IMessageFilter {
        private static readonly Color blinkOnColor = Color.Yellow;
        private static readonly Color blinkOffColor = Color.White;
        private const long blinkTime = 300; // millisecs

        private StocklistClient stocklistClient;
        private ArrayList blinkingCells;
        private bool blinkEnabled;
        private bool blinkMenu;
        private bool isDirty = false;

        private string pushServerUrl;
#region API Declarations

        [DllImport("user32.dll")]
        private static extern int GetSystemMenu(int hwnd, bool bRevert);

        [DllImport("user32.dll")]
        private static extern long AppendMenuA(int hMenu, int wFlags, int wIDNewItem, string lpNewItem);

        [DllImport("user32.dll")]
        private static extern long RemoveMenu(int hMenu, int nPosition, int wFlags);

        private const int MF_BYPOSITION = 1024;
        private const int MF_SEPERATOR = 2048;
        private const int MF_REMOVE = 4096;
        private const int WM_SYSCOMMAND = 274;
        private const int WM_KEYDOWN = 256;

#endregion // API Declarations

        public DemoForm(string pushServerHost, int pushServerPort ) {
            stocklistClient = null;
            blinkingCells = new ArrayList();
            blinkEnabled = true;
            blinkMenu = false;

            pushServerUrl = "http://" + pushServerHost + ":" + pushServerPort;

            Thread t = new Thread(new ThreadStart(DeblinkingThread));
            t.Start();

            InitializeComponent();
        }

        private void OnFormLoaded(object sender, EventArgs e) {
            for (int i = 0; i < 30; i++) {
                dataGridview.Rows.Add();
            }

            dataGridview.Refresh();

            Thread t = new Thread(new ThreadStart(StartLightstreamer));
            t.Start();
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e) {
            Environment.Exit(0);
        }
        
        private void AddBlinkMenu() {
            if (blinkMenu) return;
            blinkMenu = true;

            int sysMenuHandle = GetSysMenuHandle((int)Handle);
            AppendSeperator(sysMenuHandle);
            AppendSysMenu(sysMenuHandle, 54321, "Toggle cell blink effect");
        }

        private void StartLightstreamer() {
            stocklistClient = new StocklistClient(
                pushServerUrl,
                this,
                new LightstreamerUpdateDelegate(OnLightstreamerUpdate),
                new LightstreamerStatusChangedDelegate(OnLightstreamerStatusChanged));

           
            stocklistClient.Start();
        }

        private void OnLightstreamerUpdate(int item, IUpdateInfo values)
        {
            dataGridview.SuspendLayout();

            if (this.isDirty)
            {
                this.isDirty = false;
                CleanGrid();
            }

            DataGridViewRow row = dataGridview.Rows[item - 1];

            ArrayList cells = new ArrayList();
            int len = values.NumFields;
            for (int i = 0; i < len; i++) {
                if (values.IsValueChanged(i + 1)) {
                    string val = values.GetNewValue(i + 1);
                    DataGridViewCell cell = row.Cells[i];

                    switch (i) {
                        case 1: // last_price          
                        case 5: // bid
                        case 6: // ask
                        case 8: // min
                        case 9: // max
                        case 10: // ref_price
                        case 11: // open_price
                            double dVal = Double.Parse(val, CultureInfo.GetCultureInfo("en-US").NumberFormat);
                            cell.Value = dVal;
                            break;

                        case 3: // pct_change
                            double dVal2 = Double.Parse(val, CultureInfo.GetCultureInfo("en-US").NumberFormat);
                            cell.Value = dVal2;
                            cell.Style.ForeColor = ((dVal2 < 0.0) ? Color.Red : Color.Green);
                            break;

                        case 4: // bid_quantity
                        case 7: // ask_quantity
                            int lVal = Int32.Parse(val);
                            cell.Value = lVal;
                            break;

                        case 2: // time
                            DateTime dtVal = DateTime.Parse(val);
                            cell.Value = dtVal;
                            break;

                        default: // stock_name, ...
                            cell.Value = val;
                            break;
                    }

                    if (blinkEnabled) {
                        cell.Style.BackColor = blinkOnColor;
                        cells.Add(cell);
                    }
                }
            }

            dataGridview.ResumeLayout();

            if (blinkEnabled) {
                long now = DateTime.Now.Ticks;
                ScheduleBlinkOff(cells, now);
            }
        }

        private void OnLightstreamerStatusChanged(int cStatus, string status) {
            statusLabel.Text = status;
            

            switch (cStatus)
            {
                case StocklistConnectionListener.STREAMING:
                    statusImg.Image = Resources.status_connected_streaming;
                    break;
                case StocklistConnectionListener.POLLING:
                    statusImg.Image = Resources.status_connected_polling;
                    break;
                case StocklistConnectionListener.STALLED:
                    statusImg.Image = Resources.status_stalled;
                    break;
                case StocklistConnectionListener.DISCONNECTED:
                    this.isDirty = true;
                    statusImg.Image = Resources.status_disconnected;
                    break;
                default:
                    break;
            }

            this.Refresh();
        }

        private void CleanGrid() {
            for (int row = 0; row < dataGridview.Rows.Count; row++)
            {
                for (int col = 0; col < dataGridview.Rows[row].Cells.Count; col++)
                {
                    dataGridview.Rows[row].Cells[col].Value = "";
                }
            }
        }

        private class BlinkingCell {
            public readonly DataGridViewCell cell;
            public readonly long timestamp;

            public BlinkingCell(DataGridViewCell dataGridviewCell, long blinkOnTimestamp) {
                cell = dataGridviewCell;
                timestamp = blinkOnTimestamp;
            }
        }

        private void ScheduleBlinkOff(ArrayList cells, long blinkOnTimestamp) {
            lock (blinkingCells) {
                for (int j = 0; j < cells.Count; j++) {
                    DataGridViewCell cell = (DataGridViewCell)cells[j];

                    blinkingCells.Add(new BlinkingCell(cell, blinkOnTimestamp));
                }

                Monitor.Pulse(blinkingCells);
            }
        }

        private void BlinkOff(DataGridViewCell[] cells) {
            dataGridview.SuspendLayout();

            for (int i = 0; i < cells.Length; i++) {
                cells[i].Style.BackColor = blinkOffColor;
            }

            dataGridview.ResumeLayout();
        }

        private void DeblinkingThread() {
            ArrayList expiredBlinkingCells = new ArrayList();

            do {
                expiredBlinkingCells.Clear();

                lock (blinkingCells) {
                    if (blinkingCells.Count == 0)
                        Monitor.Wait(blinkingCells);

                    expiredBlinkingCells.AddRange(blinkingCells);
                    blinkingCells.Clear();
                }

                DataGridViewCell[] cells = new DataGridViewCell[expiredBlinkingCells.Count];
                for (int i = 0; i < expiredBlinkingCells.Count; i++) {
                    cells[i] = ((BlinkingCell)expiredBlinkingCells[i]).cell;
                }

                BlinkingCell lastBlinkingCell = (BlinkingCell)expiredBlinkingCells[expiredBlinkingCells.Count - 1];

                long now = DateTime.Now.Ticks;
                long diff = (now - lastBlinkingCell.timestamp) / 10000; // millisecs
                if (diff < blinkTime)
                    Thread.Sleep((int)(blinkTime - diff));

                Invoke(new BlinkOffDelegate(BlinkOff), new object[] { cells });

            } while (true);
        }

#region System Menu API

        public int GetSysMenuHandle(int frmHandle) {
            return GetSystemMenu(frmHandle, false);
        }
        
        private long RemoveSysMenu(int mnuHandle, int mnuPosition) {
            return RemoveMenu(mnuHandle, mnuPosition, MF_REMOVE);
        }
        
        private long AppendSysMenu(int mnuHandle, int MenuID, string mnuText) {
            return AppendMenuA(mnuHandle, 0, MenuID, mnuText);
        }
        
        private long AppendSeperator(int mnuHandle) {
            return AppendMenuA(mnuHandle, MF_SEPERATOR, 0, null);
        }

        public bool PreFilterMessage(ref Message msg) {
            switch (msg.Msg) {
                case WM_KEYDOWN:
                    Keys key = ((Keys)msg.WParam.ToInt32()) & Keys.KeyCode;
                    switch (key) {
                        case Keys.F12:
                            AddBlinkMenu();
                            break;
                    }
                    break;
            }

            return false;
        } 
        
        protected override void WndProc(ref Message messg) {
            switch (messg.Msg) {
                case WM_SYSCOMMAND:
                    switch (messg.WParam.ToInt32()) {
                        case 54321:
                            blinkEnabled = !blinkEnabled;
                            break;
                    }
                    break;
            }

            base.WndProc(ref messg);
        }

#endregion // System Menu API

    }

}