using System;
using System.Windows.Forms;
using CIAPI.Rpc;
using CIAPI.Streaming;

namespace TradingRobot
{
    public partial class Settings : Form
    {
        private Client _authenticatedClient;
        private IStreamingClient _streamingClient;

        public Settings()
        {
            InitializeComponent();
        }

        public Client AuthenticatedClient
        {
            get { return _authenticatedClient; }
        }

        public IStreamingClient StreamingClient
        {
            get { return _streamingClient; }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _authenticatedClient = new Client(new Uri(RpcUriTextBox.Text));

            AuthenticatedClient.LogIn(UserNameTextBox.Text, PasswordTextBox.Text);
            _streamingClient = StreamingClientFactory.CreateStreamingClient(new Uri(StreamingUriTextBox.Text),
                                                                            UserNameTextBox.Text,
                                                                            AuthenticatedClient.Session);
        }
    }
}