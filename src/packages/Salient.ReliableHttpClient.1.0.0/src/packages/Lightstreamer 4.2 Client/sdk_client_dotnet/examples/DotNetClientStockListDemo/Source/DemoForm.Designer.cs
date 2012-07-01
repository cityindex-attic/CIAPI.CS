namespace DotNetStockListDemo {
    partial class DemoForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Generated code

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle21 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle22 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle23 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle24 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DemoForm));
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.dataGridview = new System.Windows.Forms.DataGridView();
            this.StockName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LastPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PctChange = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BidQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Bid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Ask = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AskQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Min = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Max = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RefPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OpenPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.logoImg = new System.Windows.Forms.PictureBox();
            this.statusImg = new System.Windows.Forms.PictureBox();
            this.title = new System.Windows.Forms.Label();
            this.statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.logoImg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusImg)).BeginInit();
            this.SuspendLayout();
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 533);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(902, 22);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(754, 17);
            this.statusLabel.Spring = true;
            this.statusLabel.Text = "Connecting to Lightstreamer Server...";
            this.statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dataGridview
            // 
            this.dataGridview.AllowUserToAddRows = false;
            this.dataGridview.AllowUserToDeleteRows = false;
            this.dataGridview.AllowUserToOrderColumns = true;
            this.dataGridview.AllowUserToResizeRows = false;
            this.dataGridview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridview.CausesValidation = false;
            this.dataGridview.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridview.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.StockName,
            this.LastPrice,
            this.Time,
            this.PctChange,
            this.BidQuantity,
            this.Bid,
            this.Ask,
            this.AskQuantity,
            this.Min,
            this.Max,
            this.RefPrice,
            this.OpenPrice});
            this.dataGridview.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridview.Location = new System.Drawing.Point(0, 90);
            this.dataGridview.Name = "dataGridview";
            this.dataGridview.ReadOnly = true;
            this.dataGridview.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dataGridview.Size = new System.Drawing.Size(902, 443);
            this.dataGridview.TabIndex = 3;
            // 
            // StockName
            // 
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.StockName.DefaultCellStyle = dataGridViewCellStyle13;
            this.StockName.HeaderText = "Name";
            this.StockName.Name = "StockName";
            this.StockName.ReadOnly = true;
            this.StockName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.StockName.Width = 90;
            // 
            // LastPrice
            // 
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle14.Format = "N2";
            dataGridViewCellStyle14.NullValue = null;
            this.LastPrice.DefaultCellStyle = dataGridViewCellStyle14;
            this.LastPrice.HeaderText = "Last";
            this.LastPrice.Name = "LastPrice";
            this.LastPrice.ReadOnly = true;
            this.LastPrice.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.LastPrice.Width = 77;
            // 
            // Time
            // 
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle15.Format = "T";
            dataGridViewCellStyle15.NullValue = null;
            this.Time.DefaultCellStyle = dataGridViewCellStyle15;
            this.Time.HeaderText = "Time";
            this.Time.Name = "Time";
            this.Time.ReadOnly = true;
            this.Time.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Time.Width = 51;
            // 
            // PctChange
            // 
            dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle16.Format = "N2";
            dataGridViewCellStyle16.NullValue = null;
            this.PctChange.DefaultCellStyle = dataGridViewCellStyle16;
            this.PctChange.HeaderText = "Change %";
            this.PctChange.Name = "PctChange";
            this.PctChange.ReadOnly = true;
            this.PctChange.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.PctChange.Width = 89;
            // 
            // BidQuantity
            // 
            dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.BidQuantity.DefaultCellStyle = dataGridViewCellStyle17;
            this.BidQuantity.HeaderText = "Bid Size";
            this.BidQuantity.Name = "BidQuantity";
            this.BidQuantity.ReadOnly = true;
            this.BidQuantity.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.BidQuantity.Width = 89;
            // 
            // Bid
            // 
            dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle18.Format = "N2";
            dataGridViewCellStyle18.NullValue = null;
            this.Bid.DefaultCellStyle = dataGridViewCellStyle18;
            this.Bid.HeaderText = "Bid";
            this.Bid.Name = "Bid";
            this.Bid.ReadOnly = true;
            this.Bid.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Bid.Width = 46;
            // 
            // Ask
            // 
            dataGridViewCellStyle19.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle19.Format = "N2";
            dataGridViewCellStyle19.NullValue = null;
            this.Ask.DefaultCellStyle = dataGridViewCellStyle19;
            this.Ask.HeaderText = "Ask";
            this.Ask.Name = "Ask";
            this.Ask.ReadOnly = true;
            this.Ask.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Ask.Width = 49;
            // 
            // AskQuantity
            // 
            dataGridViewCellStyle20.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.AskQuantity.DefaultCellStyle = dataGridViewCellStyle20;
            this.AskQuantity.HeaderText = "Ask Size";
            this.AskQuantity.Name = "AskQuantity";
            this.AskQuantity.ReadOnly = true;
            this.AskQuantity.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.AskQuantity.Width = 92;
            // 
            // Min
            // 
            dataGridViewCellStyle21.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle21.Format = "N2";
            dataGridViewCellStyle21.NullValue = null;
            this.Min.DefaultCellStyle = dataGridViewCellStyle21;
            this.Min.HeaderText = "Min";
            this.Min.Name = "Min";
            this.Min.ReadOnly = true;
            this.Min.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Min.Width = 48;
            // 
            // Max
            // 
            dataGridViewCellStyle22.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle22.Format = "N2";
            dataGridViewCellStyle22.NullValue = null;
            this.Max.DefaultCellStyle = dataGridViewCellStyle22;
            this.Max.HeaderText = "Max";
            this.Max.Name = "Max";
            this.Max.ReadOnly = true;
            this.Max.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Max.Width = 51;
            // 
            // RefPrice
            // 
            dataGridViewCellStyle23.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle23.Format = "N2";
            dataGridViewCellStyle23.NullValue = null;
            this.RefPrice.DefaultCellStyle = dataGridViewCellStyle23;
            this.RefPrice.HeaderText = "Ref.";
            this.RefPrice.Name = "RefPrice";
            this.RefPrice.ReadOnly = true;
            this.RefPrice.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.RefPrice.Width = 73;
            // 
            // OpenPrice
            // 
            dataGridViewCellStyle24.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle24.Format = "N2";
            dataGridViewCellStyle24.NullValue = null;
            this.OpenPrice.DefaultCellStyle = dataGridViewCellStyle24;
            this.OpenPrice.HeaderText = "Open";
            this.OpenPrice.Name = "OpenPrice";
            this.OpenPrice.ReadOnly = true;
            this.OpenPrice.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.OpenPrice.Width = 85;
            // 
            // logoImg
            // 
            this.logoImg.Image = ((System.Drawing.Image)(resources.GetObject("logoImg.Image")));
            this.logoImg.Location = new System.Drawing.Point(28, 4);
            this.logoImg.Name = "logoImg";
            this.logoImg.Size = new System.Drawing.Size(300, 80);
            this.logoImg.TabIndex = 4;
            this.logoImg.TabStop = false;
            // 
            // statusImg
            // 
            this.statusImg.Image = global::DotNetStockListDemo.Properties.Resources.status_disconnected;
            this.statusImg.Location = new System.Drawing.Point(1, 12);
            this.statusImg.Name = "statusImg";
            this.statusImg.Size = new System.Drawing.Size(21, 51);
            this.statusImg.TabIndex = 5;
            this.statusImg.TabStop = false;
            // 
            // title
            // 
            this.title.AutoSize = true;
            this.title.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.title.Location = new System.Drawing.Point(359, 37);
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(385, 17);
            this.title.TabIndex = 6;
            this.title.Text = "Lightstreamer :: .NET Framework :: Stock-List Demo";
            // 
            // DemoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(902, 555);
            this.Controls.Add(this.title);
            this.Controls.Add(this.statusImg);
            this.Controls.Add(this.logoImg);
            this.Controls.Add(this.dataGridview);
            this.Controls.Add(this.statusStrip);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DemoForm";
            this.Text = "Lightstreamer .NET Client Demo";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormClosed);
            this.Load += new System.EventHandler(this.OnFormLoaded);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.logoImg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusImg)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.DataGridView dataGridview;
        private System.Windows.Forms.DataGridViewTextBoxColumn StockName;
        private System.Windows.Forms.DataGridViewTextBoxColumn LastPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn Time;
        private System.Windows.Forms.DataGridViewTextBoxColumn PctChange;
        private System.Windows.Forms.DataGridViewTextBoxColumn BidQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn Bid;
        private System.Windows.Forms.DataGridViewTextBoxColumn Ask;
        private System.Windows.Forms.DataGridViewTextBoxColumn AskQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn Min;
        private System.Windows.Forms.DataGridViewTextBoxColumn Max;
        private System.Windows.Forms.DataGridViewTextBoxColumn RefPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn OpenPrice;
        private System.Windows.Forms.PictureBox logoImg;
        private System.Windows.Forms.PictureBox statusImg;
        private System.Windows.Forms.Label title;
	}
}