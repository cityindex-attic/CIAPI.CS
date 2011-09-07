using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CIAPI.DTO;
using CIAPI.Rpc;
using StreamingClient;
using TradingRobot.Logic;
using IStreamingClient = CIAPI.Streaming.IStreamingClient;

namespace TradingRobot
{
    public partial class Form1 : Form
    {
        private IStreamingListener<PriceDTO> _listener;
        private SimpleBuyLowSellHigh _logic;
        private Client _rpcClient;
        private IStreamingClient _streamingClient;
        public Client AuthenticatedClient
        {
            get { return _rpcClient; }
        }

        public IStreamingClient StreamingClient
        {
            get { return _streamingClient; }
        }
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (_streamingClient != null)
            {
                try
                {
                    _listener.Stop();

                    _streamingClient.Disconnect();
                }
                catch
                {

                    // swallow
                }
                _listener = null;
                _streamingClient = null;
            }
            if (_rpcClient != null)
            {
                try
                {
                    _rpcClient.LogOut();
                }
                catch
                {

                    // swallow
                }

                _streamingClient = null;

            }
            var settingsForm = new Settings();
            settingsForm.ShowDialog();
            if (settingsForm.StreamingClient == null)
            {
                return;
                MessageBox.Show("Not connected");
            }
            _streamingClient = settingsForm.StreamingClient;
            _listener = _streamingClient.BuildPricesListener(int.Parse(MarketIdTextBox.Text));
            _listener.MessageReceived += new EventHandler<MessageEventArgs<PriceDTO>>(ListenerMessageReceived);
            _listener.Start();
            _rpcClient = settingsForm.RpcClient;
            _logic = new TradingRobot.Logic.SimpleBuyLowSellHigh(_rpcClient);
            _logic.BidPrice = SellNUD.Value;
            _logic.OfferPrice = BuyNUD.Value;
            _logic.Quantity = QuantityNUD.Value;
            MessageBox.Show("Connected");

        }

        
        private void ListenerMessageReceived(object sender, MessageEventArgs<PriceDTO> e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdateUi(e.Data)));
            }
            else
            {
                UpdateUi(e.Data);
            }
            

        }

        private void UpdateUi(PriceDTO data)
        {
            _logic.ProcessTick(data);

            if (_logic.PositionIsOpen)
            {
                StatusLabel.Text = "position is open.";
            }
            else
            {
                StatusLabel.Text = "no position.";
            }
            if (_logic.BidPending)
            {
                StatusLabel.Text = StatusLabel.Text + " sell is pending...";
            }
            else if (_logic.OfferPending)
            {
                StatusLabel.Text = StatusLabel.Text + " buy is pending...";
            }
        }
    }
}
