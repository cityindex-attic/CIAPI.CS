namespace ClientLibraryDemo
{
    partial class ClientDemo
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.UsernameTextBox = new System.Windows.Forms.TextBox();
            this.PasswordTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.LoginButton = new System.Windows.Forms.Button();
            this.LogInGroupBox = new System.Windows.Forms.GroupBox();
            this.EndpointTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.GetAccountsButton = new System.Windows.Forms.Button();
            this.ListOpenPositionsButton = new System.Windows.Forms.Button();
            this.LogOutButton = new System.Windows.Forms.Button();
            this.ClientAccountsGroupBox = new System.Windows.Forms.GroupBox();
            this.apiTradingAccountDTODataGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.apiTradingAccountDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.OpenPositionsGroupBox = new System.Windows.Forms.GroupBox();
            this.apiOpenPositionDTODataGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn14 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn15 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn16 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn17 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.apiOpenPositionDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label4 = new System.Windows.Forms.Label();
            this.SessionIDLabel = new System.Windows.Forms.Label();
            this.LogInGroupBox.SuspendLayout();
            this.ClientAccountsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.apiTradingAccountDTODataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.apiTradingAccountDTOBindingSource)).BeginInit();
            this.OpenPositionsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.apiOpenPositionDTODataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.apiOpenPositionDTOBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Username";
            // 
            // UsernameTextBox
            // 
            this.UsernameTextBox.Location = new System.Drawing.Point(6, 28);
            this.UsernameTextBox.Name = "UsernameTextBox";
            this.UsernameTextBox.Size = new System.Drawing.Size(100, 20);
            this.UsernameTextBox.TabIndex = 1;
            this.UsernameTextBox.Text = "DM813766";
            // 
            // PasswordTextBox
            // 
            this.PasswordTextBox.Location = new System.Drawing.Point(124, 28);
            this.PasswordTextBox.Name = "PasswordTextBox";
            this.PasswordTextBox.Size = new System.Drawing.Size(100, 20);
            this.PasswordTextBox.TabIndex = 3;
            this.PasswordTextBox.Text = "password";
            this.PasswordTextBox.UseSystemPasswordChar = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(124, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "password";
            // 
            // LoginButton
            // 
            this.LoginButton.Location = new System.Drawing.Point(242, 26);
            this.LoginButton.Name = "LoginButton";
            this.LoginButton.Size = new System.Drawing.Size(75, 23);
            this.LoginButton.TabIndex = 4;
            this.LoginButton.Text = "Login";
            this.LoginButton.UseVisualStyleBackColor = true;
            this.LoginButton.Click += new System.EventHandler(this.LoginButton_Click);
            // 
            // LogInGroupBox
            // 
            this.LogInGroupBox.Controls.Add(this.SessionIDLabel);
            this.LogInGroupBox.Controls.Add(this.label4);
            this.LogInGroupBox.Controls.Add(this.UsernameTextBox);
            this.LogInGroupBox.Controls.Add(this.LoginButton);
            this.LogInGroupBox.Controls.Add(this.label1);
            this.LogInGroupBox.Controls.Add(this.PasswordTextBox);
            this.LogInGroupBox.Controls.Add(this.label2);
            this.LogInGroupBox.Location = new System.Drawing.Point(12, 62);
            this.LogInGroupBox.Name = "LogInGroupBox";
            this.LogInGroupBox.Size = new System.Drawing.Size(553, 64);
            this.LogInGroupBox.TabIndex = 5;
            this.LogInGroupBox.TabStop = false;
            this.LogInGroupBox.Text = "Login";
            // 
            // EndpointTextBox
            // 
            this.EndpointTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EndpointTextBox.Location = new System.Drawing.Point(12, 25);
            this.EndpointTextBox.Name = "EndpointTextBox";
            this.EndpointTextBox.Size = new System.Drawing.Size(823, 22);
            this.EndpointTextBox.TabIndex = 7;
            this.EndpointTextBox.Text = "http://ciapipreprod.cityindextest9.co.uk/TradingApi";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Endpoint URL";
            // 
            // GetAccountsButton
            // 
            this.GetAccountsButton.Location = new System.Drawing.Point(6, 19);
            this.GetAccountsButton.Name = "GetAccountsButton";
            this.GetAccountsButton.Size = new System.Drawing.Size(147, 23);
            this.GetAccountsButton.TabIndex = 8;
            this.GetAccountsButton.Text = "Get Accounts";
            this.GetAccountsButton.UseVisualStyleBackColor = true;
            this.GetAccountsButton.Click += new System.EventHandler(this.GetAccountsButton_Click);
            // 
            // ListOpenPositionsButton
            // 
            this.ListOpenPositionsButton.Location = new System.Drawing.Point(9, 19);
            this.ListOpenPositionsButton.Name = "ListOpenPositionsButton";
            this.ListOpenPositionsButton.Size = new System.Drawing.Size(147, 23);
            this.ListOpenPositionsButton.TabIndex = 9;
            this.ListOpenPositionsButton.Text = "List Open Positions";
            this.ListOpenPositionsButton.UseVisualStyleBackColor = true;
            this.ListOpenPositionsButton.Click += new System.EventHandler(this.ListOpenPositionsButton_Click);
            // 
            // LogOutButton
            // 
            this.LogOutButton.Location = new System.Drawing.Point(679, 565);
            this.LogOutButton.Name = "LogOutButton";
            this.LogOutButton.Size = new System.Drawing.Size(147, 23);
            this.LogOutButton.TabIndex = 10;
            this.LogOutButton.Text = "Log out";
            this.LogOutButton.UseVisualStyleBackColor = true;
            this.LogOutButton.Click += new System.EventHandler(this.LogOutButton_Click);
            // 
            // ClientAccountsGroupBox
            // 
            this.ClientAccountsGroupBox.Controls.Add(this.apiTradingAccountDTODataGridView);
            this.ClientAccountsGroupBox.Controls.Add(this.GetAccountsButton);
            this.ClientAccountsGroupBox.Location = new System.Drawing.Point(12, 132);
            this.ClientAccountsGroupBox.Name = "ClientAccountsGroupBox";
            this.ClientAccountsGroupBox.Size = new System.Drawing.Size(823, 200);
            this.ClientAccountsGroupBox.TabIndex = 11;
            this.ClientAccountsGroupBox.TabStop = false;
            this.ClientAccountsGroupBox.Text = "Client Accounts";
            // 
            // apiTradingAccountDTODataGridView
            // 
            this.apiTradingAccountDTODataGridView.AutoGenerateColumns = false;
            this.apiTradingAccountDTODataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.apiTradingAccountDTODataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4});
            this.apiTradingAccountDTODataGridView.DataSource = this.apiTradingAccountDTOBindingSource;
            this.apiTradingAccountDTODataGridView.Location = new System.Drawing.Point(16, 57);
            this.apiTradingAccountDTODataGridView.Name = "apiTradingAccountDTODataGridView";
            this.apiTradingAccountDTODataGridView.Size = new System.Drawing.Size(798, 134);
            this.apiTradingAccountDTODataGridView.TabIndex = 8;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "TradingAccountId";
            this.dataGridViewTextBoxColumn1.HeaderText = "TradingAccountId";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "TradingAccountCode";
            this.dataGridViewTextBoxColumn2.HeaderText = "TradingAccountCode";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "TradingAccountStatus";
            this.dataGridViewTextBoxColumn3.HeaderText = "TradingAccountStatus";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "TradingAccountType";
            this.dataGridViewTextBoxColumn4.HeaderText = "TradingAccountType";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            // 
            // apiTradingAccountDTOBindingSource
            // 
            this.apiTradingAccountDTOBindingSource.DataSource = typeof(CIAPI.DTO.ApiTradingAccountDTO);
            // 
            // OpenPositionsGroupBox
            // 
            this.OpenPositionsGroupBox.Controls.Add(this.apiOpenPositionDTODataGridView);
            this.OpenPositionsGroupBox.Controls.Add(this.ListOpenPositionsButton);
            this.OpenPositionsGroupBox.Location = new System.Drawing.Point(15, 338);
            this.OpenPositionsGroupBox.Name = "OpenPositionsGroupBox";
            this.OpenPositionsGroupBox.Size = new System.Drawing.Size(820, 201);
            this.OpenPositionsGroupBox.TabIndex = 12;
            this.OpenPositionsGroupBox.TabStop = false;
            this.OpenPositionsGroupBox.Text = "Open Positions";
            // 
            // apiOpenPositionDTODataGridView
            // 
            this.apiOpenPositionDTODataGridView.AutoGenerateColumns = false;
            this.apiOpenPositionDTODataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.apiOpenPositionDTODataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewCheckBoxColumn1,
            this.dataGridViewTextBoxColumn6,
            this.dataGridViewTextBoxColumn7,
            this.dataGridViewTextBoxColumn8,
            this.dataGridViewTextBoxColumn9,
            this.dataGridViewTextBoxColumn10,
            this.dataGridViewTextBoxColumn11,
            this.dataGridViewTextBoxColumn12,
            this.dataGridViewTextBoxColumn13,
            this.dataGridViewTextBoxColumn14,
            this.dataGridViewTextBoxColumn15,
            this.dataGridViewTextBoxColumn16,
            this.dataGridViewTextBoxColumn17});
            this.apiOpenPositionDTODataGridView.DataSource = this.apiOpenPositionDTOBindingSource;
            this.apiOpenPositionDTODataGridView.Location = new System.Drawing.Point(14, 48);
            this.apiOpenPositionDTODataGridView.Name = "apiOpenPositionDTODataGridView";
            this.apiOpenPositionDTODataGridView.Size = new System.Drawing.Size(780, 128);
            this.apiOpenPositionDTODataGridView.TabIndex = 9;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "OrderId";
            this.dataGridViewTextBoxColumn5.HeaderText = "OrderId";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            // 
            // dataGridViewCheckBoxColumn1
            // 
            this.dataGridViewCheckBoxColumn1.DataPropertyName = "AutoRollover";
            this.dataGridViewCheckBoxColumn1.HeaderText = "AutoRollover";
            this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DataPropertyName = "MarketId";
            this.dataGridViewTextBoxColumn6.HeaderText = "MarketId";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.DataPropertyName = "MarketName";
            this.dataGridViewTextBoxColumn7.HeaderText = "MarketName";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.DataPropertyName = "Direction";
            this.dataGridViewTextBoxColumn8.HeaderText = "Direction";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.DataPropertyName = "Quantity";
            this.dataGridViewTextBoxColumn9.HeaderText = "Quantity";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.DataPropertyName = "Price";
            this.dataGridViewTextBoxColumn10.HeaderText = "Price";
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            // 
            // dataGridViewTextBoxColumn11
            // 
            this.dataGridViewTextBoxColumn11.DataPropertyName = "TradingAccountId";
            this.dataGridViewTextBoxColumn11.HeaderText = "TradingAccountId";
            this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            // 
            // dataGridViewTextBoxColumn12
            // 
            this.dataGridViewTextBoxColumn12.DataPropertyName = "Currency";
            this.dataGridViewTextBoxColumn12.HeaderText = "Currency";
            this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
            // 
            // dataGridViewTextBoxColumn13
            // 
            this.dataGridViewTextBoxColumn13.DataPropertyName = "Status";
            this.dataGridViewTextBoxColumn13.HeaderText = "Status";
            this.dataGridViewTextBoxColumn13.Name = "dataGridViewTextBoxColumn13";
            // 
            // dataGridViewTextBoxColumn14
            // 
            this.dataGridViewTextBoxColumn14.DataPropertyName = "StopOrder";
            this.dataGridViewTextBoxColumn14.HeaderText = "StopOrder";
            this.dataGridViewTextBoxColumn14.Name = "dataGridViewTextBoxColumn14";
            // 
            // dataGridViewTextBoxColumn15
            // 
            this.dataGridViewTextBoxColumn15.DataPropertyName = "LimitOrder";
            this.dataGridViewTextBoxColumn15.HeaderText = "LimitOrder";
            this.dataGridViewTextBoxColumn15.Name = "dataGridViewTextBoxColumn15";
            // 
            // dataGridViewTextBoxColumn16
            // 
            this.dataGridViewTextBoxColumn16.DataPropertyName = "LastChangedDateTimeUTC";
            this.dataGridViewTextBoxColumn16.HeaderText = "LastChangedDateTimeUTC";
            this.dataGridViewTextBoxColumn16.Name = "dataGridViewTextBoxColumn16";
            // 
            // dataGridViewTextBoxColumn17
            // 
            this.dataGridViewTextBoxColumn17.DataPropertyName = "Status_Resolved";
            this.dataGridViewTextBoxColumn17.HeaderText = "Status_Resolved";
            this.dataGridViewTextBoxColumn17.Name = "dataGridViewTextBoxColumn17";
            // 
            // apiOpenPositionDTOBindingSource
            // 
            this.apiOpenPositionDTOBindingSource.DataSource = typeof(CIAPI.DTO.ApiOpenPositionDTO);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(339, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Session ID";
            // 
            // SessionIDLabel
            // 
            this.SessionIDLabel.AutoSize = true;
            this.SessionIDLabel.Location = new System.Drawing.Point(339, 31);
            this.SessionIDLabel.Name = "SessionIDLabel";
            this.SessionIDLabel.Size = new System.Drawing.Size(0, 13);
            this.SessionIDLabel.TabIndex = 6;
            // 
            // ClientDemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(847, 615);
            this.Controls.Add(this.ClientAccountsGroupBox);
            this.Controls.Add(this.LogOutButton);
            this.Controls.Add(this.EndpointTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.LogInGroupBox);
            this.Controls.Add(this.OpenPositionsGroupBox);
            this.Name = "ClientDemo";
            this.Text = "Form1";
            this.LogInGroupBox.ResumeLayout(false);
            this.LogInGroupBox.PerformLayout();
            this.ClientAccountsGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.apiTradingAccountDTODataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.apiTradingAccountDTOBindingSource)).EndInit();
            this.OpenPositionsGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.apiOpenPositionDTODataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.apiOpenPositionDTOBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox UsernameTextBox;
        private System.Windows.Forms.TextBox PasswordTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button LoginButton;
        private System.Windows.Forms.GroupBox LogInGroupBox;
        private System.Windows.Forms.TextBox EndpointTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button GetAccountsButton;
        private System.Windows.Forms.Button ListOpenPositionsButton;
        private System.Windows.Forms.Button LogOutButton;
        private System.Windows.Forms.GroupBox ClientAccountsGroupBox;
        private System.Windows.Forms.GroupBox OpenPositionsGroupBox;
        private System.Windows.Forms.DataGridView apiTradingAccountDTODataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.BindingSource apiTradingAccountDTOBindingSource;
        private System.Windows.Forms.DataGridView apiOpenPositionDTODataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn13;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn14;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn15;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn16;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn17;
        private System.Windows.Forms.BindingSource apiOpenPositionDTOBindingSource;
        private System.Windows.Forms.Label SessionIDLabel;
        private System.Windows.Forms.Label label4;
    }
}

