using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CIAPI.Rpc;
using CIAPI.Streaming;

namespace TradingRobot
{
    public partial class Form1 : Form
    {
        private Client _authenticatedClient;
        private IStreamingClient _streamingClient;
        public Client AuthenticatedClient
        {
            get { return _authenticatedClient; }
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
                _streamingClient.Disconnect();
            }
        }
    }
}
