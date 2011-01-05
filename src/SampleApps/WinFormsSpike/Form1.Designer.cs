

namespace WinFormsSpike
{
    partial class Form1
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
            this.MainTabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.MaxResultsNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.CategoryTextBox = new System.Windows.Forms.TextBox();
            this.ListNewsHeadlinesButton = new System.Windows.Forms.Button();
            this.NewsDetailWebBrowser = new System.Windows.Forms.WebBrowser();
            this.NewsHeadlinesGridView = new System.Windows.Forms.DataGridView();
            this.NewsDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.LoginButton = new System.Windows.Forms.Button();
            this.UidTextBox = new System.Windows.Forms.TextBox();
            this.PwdTextBox = new System.Windows.Forms.TextBox();
            this.LogOutButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ApiEndpointTextBox = new System.Windows.Forms.TextBox();
            this.storyIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.publishDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.headlineDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MainTabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MaxResultsNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NewsHeadlinesGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NewsDTOBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // MainTabControl
            // 
            this.MainTabControl.Controls.Add(this.tabPage1);
            this.MainTabControl.Controls.Add(this.tabPage2);
            this.MainTabControl.Location = new System.Drawing.Point(12, 54);
            this.MainTabControl.Name = "MainTabControl";
            this.MainTabControl.SelectedIndex = 0;
            this.MainTabControl.Size = new System.Drawing.Size(579, 422);
            this.MainTabControl.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.MaxResultsNumericUpDown);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.CategoryTextBox);
            this.tabPage1.Controls.Add(this.ListNewsHeadlinesButton);
            this.tabPage1.Controls.Add(this.NewsDetailWebBrowser);
            this.tabPage1.Controls.Add(this.NewsHeadlinesGridView);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(571, 396);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "News";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // MaxResultsNumericUpDown
            // 
            this.MaxResultsNumericUpDown.Location = new System.Drawing.Point(115, 29);
            this.MaxResultsNumericUpDown.Name = "MaxResultsNumericUpDown";
            this.MaxResultsNumericUpDown.Size = new System.Drawing.Size(120, 20);
            this.MaxResultsNumericUpDown.TabIndex = 11;
            this.MaxResultsNumericUpDown.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(112, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Max Results";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 11);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Category";
            // 
            // CategoryTextBox
            // 
            this.CategoryTextBox.Location = new System.Drawing.Point(6, 30);
            this.CategoryTextBox.Name = "CategoryTextBox";
            this.CategoryTextBox.Size = new System.Drawing.Size(100, 20);
            this.CategoryTextBox.TabIndex = 7;
            this.CategoryTextBox.Text = "UK";
            // 
            // ListNewsHeadlinesButton
            // 
            this.ListNewsHeadlinesButton.Location = new System.Drawing.Point(241, 28);
            this.ListNewsHeadlinesButton.Name = "ListNewsHeadlinesButton";
            this.ListNewsHeadlinesButton.Size = new System.Drawing.Size(129, 23);
            this.ListNewsHeadlinesButton.TabIndex = 2;
            this.ListNewsHeadlinesButton.Text = "Get Headlines";
            this.ListNewsHeadlinesButton.UseVisualStyleBackColor = true;
            this.ListNewsHeadlinesButton.Click += new System.EventHandler(this.ListNewsHeadlinesButtonClick);
            // 
            // NewsDetailWebBrowser
            // 
            this.NewsDetailWebBrowser.Location = new System.Drawing.Point(3, 207);
            this.NewsDetailWebBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.NewsDetailWebBrowser.Name = "NewsDetailWebBrowser";
            this.NewsDetailWebBrowser.Size = new System.Drawing.Size(559, 183);
            this.NewsDetailWebBrowser.TabIndex = 1;
            // 
            // NewsHeadlinesGridView
            // 
            this.NewsHeadlinesGridView.AllowUserToAddRows = false;
            this.NewsHeadlinesGridView.AllowUserToDeleteRows = false;
            this.NewsHeadlinesGridView.AllowUserToResizeRows = false;
            this.NewsHeadlinesGridView.AutoGenerateColumns = false;
            this.NewsHeadlinesGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.NewsHeadlinesGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.storyIdDataGridViewTextBoxColumn,
            this.publishDateDataGridViewTextBoxColumn,
            this.headlineDataGridViewTextBoxColumn});
            this.NewsHeadlinesGridView.DataSource = this.NewsDTOBindingSource;
            this.NewsHeadlinesGridView.Location = new System.Drawing.Point(3, 57);
            this.NewsHeadlinesGridView.Name = "NewsHeadlinesGridView";
            this.NewsHeadlinesGridView.ReadOnly = true;
            this.NewsHeadlinesGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.NewsHeadlinesGridView.Size = new System.Drawing.Size(559, 144);
            this.NewsHeadlinesGridView.TabIndex = 0;
            // 
            // NewsDTOBindingSource
            // 
            this.NewsDTOBindingSource.DataSource = typeof(CIAPI.DTO.NewsDTO);
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(572, 299);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // LoginButton
            // 
            this.LoginButton.Location = new System.Drawing.Point(435, 24);
            this.LoginButton.Name = "LoginButton";
            this.LoginButton.Size = new System.Drawing.Size(75, 23);
            this.LoginButton.TabIndex = 1;
            this.LoginButton.Text = "Log In";
            this.LoginButton.UseVisualStyleBackColor = true;
            this.LoginButton.Click += new System.EventHandler(this.LoginButtonClick);
            // 
            // UidTextBox
            // 
            this.UidTextBox.Location = new System.Drawing.Point(223, 25);
            this.UidTextBox.Name = "UidTextBox";
            this.UidTextBox.Size = new System.Drawing.Size(100, 20);
            this.UidTextBox.TabIndex = 2;
            // 
            // PwdTextBox
            // 
            this.PwdTextBox.Location = new System.Drawing.Point(329, 26);
            this.PwdTextBox.Name = "PwdTextBox";
            this.PwdTextBox.PasswordChar = '*';
            this.PwdTextBox.Size = new System.Drawing.Size(100, 20);
            this.PwdTextBox.TabIndex = 3;
            this.PwdTextBox.Text = "password";
            // 
            // LogOutButton
            // 
            this.LogOutButton.Location = new System.Drawing.Point(516, 25);
            this.LogOutButton.Name = "LogOutButton";
            this.LogOutButton.Size = new System.Drawing.Size(75, 23);
            this.LogOutButton.TabIndex = 4;
            this.LogOutButton.Text = "Log Out";
            this.LogOutButton.UseVisualStyleBackColor = true;
            this.LogOutButton.Click += new System.EventHandler(this.LogOutButtonClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(223, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "UserName";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(329, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Password";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "API Endpoint";
            // 
            // ApiEndpointTextBox
            // 
            this.ApiEndpointTextBox.Location = new System.Drawing.Point(12, 25);
            this.ApiEndpointTextBox.Name = "ApiEndpointTextBox";
            this.ApiEndpointTextBox.Size = new System.Drawing.Size(205, 20);
            this.ApiEndpointTextBox.TabIndex = 7;
            // 
            // storyIdDataGridViewTextBoxColumn
            // 
            this.storyIdDataGridViewTextBoxColumn.DataPropertyName = "StoryId";
            this.storyIdDataGridViewTextBoxColumn.HeaderText = "StoryId";
            this.storyIdDataGridViewTextBoxColumn.Name = "storyIdDataGridViewTextBoxColumn";
            this.storyIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // publishDateDataGridViewTextBoxColumn
            // 
            this.publishDateDataGridViewTextBoxColumn.DataPropertyName = "PublishDate";
            this.publishDateDataGridViewTextBoxColumn.HeaderText = "PublishDate";
            this.publishDateDataGridViewTextBoxColumn.Name = "publishDateDataGridViewTextBoxColumn";
            this.publishDateDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // headlineDataGridViewTextBoxColumn
            // 
            this.headlineDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.headlineDataGridViewTextBoxColumn.DataPropertyName = "Headline";
            this.headlineDataGridViewTextBoxColumn.HeaderText = "Headline";
            this.headlineDataGridViewTextBoxColumn.Name = "headlineDataGridViewTextBoxColumn";
            this.headlineDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(601, 488);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ApiEndpointTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LogOutButton);
            this.Controls.Add(this.PwdTextBox);
            this.Controls.Add(this.UidTextBox);
            this.Controls.Add(this.LoginButton);
            this.Controls.Add(this.MainTabControl);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1Load);
            this.MainTabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MaxResultsNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NewsHeadlinesGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NewsDTOBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl MainTabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button LoginButton;
        private System.Windows.Forms.TextBox UidTextBox;
        private System.Windows.Forms.TextBox PwdTextBox;
        private System.Windows.Forms.Button LogOutButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox ApiEndpointTextBox;
        private System.Windows.Forms.Button ListNewsHeadlinesButton;
        private System.Windows.Forms.WebBrowser NewsDetailWebBrowser;
        private System.Windows.Forms.DataGridView NewsHeadlinesGridView;
        private System.Windows.Forms.BindingSource NewsDTOBindingSource;
        private System.Windows.Forms.NumericUpDown MaxResultsNumericUpDown;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox CategoryTextBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn storyIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn publishDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn headlineDataGridViewTextBoxColumn;
    }
}

