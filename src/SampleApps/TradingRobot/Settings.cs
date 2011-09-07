using System;
using System.Windows.Forms;
using CIAPI.Rpc;
using CIAPI.Streaming;

namespace TradingRobot
{
    public partial class Settings : Form
    {
        private Client _rpcClient;
        private IStreamingClient _streamingClient;

        public Settings()
        {
            InitializeComponent();
        }

        public Client RpcClient
        {
            get { return _rpcClient; }
        }

        public IStreamingClient StreamingClient
        {
            get { return _streamingClient; }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _rpcClient = new Client(new Uri(RpcUriTextBox.Text));

            RpcClient.LogIn(UserNameTextBox.Text, PasswordTextBox.Text);
            _streamingClient = StreamingClientFactory.CreateStreamingClient(new Uri(StreamingUriTextBox.Text),
                                                                            UserNameTextBox.Text,
                                                                            RpcClient.Session);
            _streamingClient.Connect();
            DialogResult = DialogResult.OK;
        }
    }
}