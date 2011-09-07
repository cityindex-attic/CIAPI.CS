namespace TradingRobot
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
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SellNUD = new System.Windows.Forms.NumericUpDown();
            this.BuyNUD = new System.Windows.Forms.NumericUpDown();
            this.CurrentBuyNUD = new System.Windows.Forms.NumericUpDown();
            this.CurrentSellNUD = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.MarketIdTextBox = new System.Windows.Forms.TextBox();
            this.QuantityNUD = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.SellNUD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BuyNUD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CurrentBuyNUD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CurrentSellNUD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.QuantityNUD)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 13);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Connect";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "MarketId";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 106);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(24, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Sell";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(25, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Buy";
            // 
            // SellNUD
            // 
            this.SellNUD.DecimalPlaces = 6;
            this.SellNUD.Location = new System.Drawing.Point(81, 106);
            this.SellNUD.Name = "SellNUD";
            this.SellNUD.Size = new System.Drawing.Size(120, 20);
            this.SellNUD.TabIndex = 10;
            this.SellNUD.Value = new decimal(new int[] {
            16003,
            0,
            0,
            262144});
            // 
            // BuyNUD
            // 
            this.BuyNUD.DecimalPlaces = 6;
            this.BuyNUD.Location = new System.Drawing.Point(81, 80);
            this.BuyNUD.Name = "BuyNUD";
            this.BuyNUD.Size = new System.Drawing.Size(120, 20);
            this.BuyNUD.TabIndex = 11;
            this.BuyNUD.Value = new decimal(new int[] {
            16002,
            0,
            0,
            262144});
            // 
            // CurrentBuyNUD
            // 
            this.CurrentBuyNUD.DecimalPlaces = 6;
            this.CurrentBuyNUD.Location = new System.Drawing.Point(81, 188);
            this.CurrentBuyNUD.Name = "CurrentBuyNUD";
            this.CurrentBuyNUD.ReadOnly = true;
            this.CurrentBuyNUD.Size = new System.Drawing.Size(120, 20);
            this.CurrentBuyNUD.TabIndex = 15;
            // 
            // CurrentSellNUD
            // 
            this.CurrentSellNUD.DecimalPlaces = 6;
            this.CurrentSellNUD.Location = new System.Drawing.Point(81, 214);
            this.CurrentSellNUD.Name = "CurrentSellNUD";
            this.CurrentSellNUD.ReadOnly = true;
            this.CurrentSellNUD.Size = new System.Drawing.Size(120, 20);
            this.CurrentSellNUD.TabIndex = 14;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 188);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Current Buy";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 214);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(61, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Current Sell";
            // 
            // StatusLabel
            // 
            this.StatusLabel.AutoSize = true;
            this.StatusLabel.Location = new System.Drawing.Point(18, 252);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(0, 13);
            this.StatusLabel.TabIndex = 16;
            // 
            // MarketIdTextBox
            // 
            this.MarketIdTextBox.Location = new System.Drawing.Point(81, 48);
            this.MarketIdTextBox.Name = "MarketIdTextBox";
            this.MarketIdTextBox.Size = new System.Drawing.Size(100, 20);
            this.MarketIdTextBox.TabIndex = 17;
            this.MarketIdTextBox.Text = "80905";
            // 
            // QuantityNUD
            // 
            this.QuantityNUD.DecimalPlaces = 6;
            this.QuantityNUD.Location = new System.Drawing.Point(81, 132);
            this.QuantityNUD.Name = "QuantityNUD";
            this.QuantityNUD.Size = new System.Drawing.Size(120, 20);
            this.QuantityNUD.TabIndex = 19;
            this.QuantityNUD.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(18, 132);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(46, 13);
            this.label6.TabIndex = 18;
            this.label6.Text = "Quantity";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(707, 364);
            this.Controls.Add(this.QuantityNUD);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.MarketIdTextBox);
            this.Controls.Add(this.StatusLabel);
            this.Controls.Add(this.CurrentBuyNUD);
            this.Controls.Add(this.CurrentSellNUD);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.BuyNUD);
            this.Controls.Add(this.SellNUD);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.SellNUD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BuyNUD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CurrentBuyNUD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CurrentSellNUD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.QuantityNUD)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown SellNUD;
        private System.Windows.Forms.NumericUpDown BuyNUD;
        private System.Windows.Forms.NumericUpDown CurrentBuyNUD;
        private System.Windows.Forms.NumericUpDown CurrentSellNUD;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label StatusLabel;
        private System.Windows.Forms.TextBox MarketIdTextBox;
        private System.Windows.Forms.NumericUpDown QuantityNUD;
        private System.Windows.Forms.Label label6;
    }
}

