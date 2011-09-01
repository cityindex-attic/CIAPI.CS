namespace TradingRobot
{
    partial class Settings
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.StreamingUriTextBox = new System.Windows.Forms.TextBox();
            this.RpcUriTextBox = new System.Windows.Forms.TextBox();
            this.PasswordTextBox = new System.Windows.Forms.TextBox();
            this.UserNameTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(426, 172);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(345, 172);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "OK";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(38, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Username";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(40, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Password";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(50, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Rpc Uri";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 107);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Streaming Uri";
            // 
            // StreamingUriTextBox
            // 
            this.StreamingUriTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::TradingRobot.Properties.Settings.Default, "StreamingUri", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.StreamingUriTextBox.Location = new System.Drawing.Point(101, 105);
            this.StreamingUriTextBox.Name = "StreamingUriTextBox";
            this.StreamingUriTextBox.Size = new System.Drawing.Size(400, 20);
            this.StreamingUriTextBox.TabIndex = 8;
            this.StreamingUriTextBox.Text = global::TradingRobot.Properties.Settings.Default.StreamingUri;
            // 
            // RpcUriTextBox
            // 
            this.RpcUriTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::TradingRobot.Properties.Settings.Default, "RpcUri", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.RpcUriTextBox.Location = new System.Drawing.Point(101, 79);
            this.RpcUriTextBox.Name = "RpcUriTextBox";
            this.RpcUriTextBox.Size = new System.Drawing.Size(400, 20);
            this.RpcUriTextBox.TabIndex = 6;
            this.RpcUriTextBox.Text = global::TradingRobot.Properties.Settings.Default.RpcUri;
            // 
            // PasswordTextBox
            // 
            this.PasswordTextBox.Location = new System.Drawing.Point(101, 53);
            this.PasswordTextBox.Name = "PasswordTextBox";
            this.PasswordTextBox.PasswordChar = '*';
            this.PasswordTextBox.Size = new System.Drawing.Size(400, 20);
            this.PasswordTextBox.TabIndex = 4;
            // 
            // UserNameTextBox
            // 
            this.UserNameTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::TradingRobot.Properties.Settings.Default, "UserName", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.UserNameTextBox.Location = new System.Drawing.Point(101, 27);
            this.UserNameTextBox.Name = "UserNameTextBox";
            this.UserNameTextBox.Size = new System.Drawing.Size(400, 20);
            this.UserNameTextBox.TabIndex = 2;
            this.UserNameTextBox.Text = global::TradingRobot.Properties.Settings.Default.UserName;
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(512, 222);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.StreamingUriTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.RpcUriTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.PasswordTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.UserNameTextBox);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "Settings";
            this.Text = "Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox UserNameTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox PasswordTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox RpcUriTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox StreamingUriTextBox;
    }
}