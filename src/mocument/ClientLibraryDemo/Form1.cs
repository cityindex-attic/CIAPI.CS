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

namespace ClientLibraryDemo
{
    public partial class ClientDemo : Form
    {
        private Client _client;
        AccountInformationResponseDTO _accounts;
        ListOpenPositionsResponseDTO _positions;
        public ClientDemo()
        {
            InitializeComponent();
            OpenPositionsGroupBox.Enabled = false;
            ClientAccountsGroupBox.Enabled = false;
            LogInGroupBox.Enabled = true;
            LogOutButton.Enabled = false;

        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            _client = new Client(new Uri(EndpointTextBox.Text), new Uri("http://foo.com"), "mocument client demo");
            _client.LogIn(UsernameTextBox.Text, PasswordTextBox.Text);
            SessionIDLabel.Text = _client.Session;
            OpenPositionsGroupBox.Enabled = false;
            ClientAccountsGroupBox.Enabled = true;
            LogInGroupBox.Enabled = false;
            LogOutButton.Enabled = true;

            
            
           
          

        }

        private void GetAccountsButton_Click(object sender, EventArgs e)
        {
            
            
            _accounts = _client.AccountInformation.GetClientAndTradingAccount();
            apiTradingAccountDTOBindingSource.DataSource = _accounts.TradingAccounts;
            OpenPositionsGroupBox.Enabled = true;
            ClientAccountsGroupBox.Enabled = true;
            LogInGroupBox.Enabled = false;
            LogOutButton.Enabled = true;

        }

        private void ListOpenPositionsButton_Click(object sender, EventArgs e)
        {
 
            _positions = _client.TradesAndOrders.ListOpenPositions(_accounts.TradingAccounts[0].TradingAccountId);
            apiOpenPositionDTOBindingSource.DataSource = _positions.OpenPositions;
        }

        private void LogOutButton_Click(object sender, EventArgs e)
        {
            apiTradingAccountDTOBindingSource.DataSource = null;
            apiOpenPositionDTOBindingSource.DataSource = null;
            _client.LogOut();
            _client.Dispose();

            OpenPositionsGroupBox.Enabled = false;
            ClientAccountsGroupBox.Enabled = false;
            LogInGroupBox.Enabled = true;
            LogOutButton.Enabled = false;
            SessionIDLabel.Text = "";

        }
    }
}
