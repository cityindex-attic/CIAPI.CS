namespace BasicFormats
{
    using Fiddler;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Text;
    using System.Windows.Forms;

    public class AppCacheOptions : Form
    {
        private Button btnCancel;
        private Button btnSave;
        public CheckBox cbNetworkFallback;
        private ColumnHeader colSize;
        private ColumnHeader colType;
        private ColumnHeader colURL;
        private IContainer components;
        private Label lblBase;
        private Label lblinstructions;
        private Label lblResources;
        public ListView lvItems;
        public TextBox txtBase;

        public AppCacheOptions()
        {
            this.InitializeComponent();
            Utilities.SetCueText(this.txtBase, "(Optional) Specify URL to use as a base, e.g. http://example.com");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            ListViewGroup group = new ListViewGroup("Markup", HorizontalAlignment.Left);
            ListViewGroup group2 = new ListViewGroup("Images", HorizontalAlignment.Left);
            ListViewGroup group3 = new ListViewGroup("CSS", HorizontalAlignment.Left);
            ListViewGroup group4 = new ListViewGroup("Script", HorizontalAlignment.Left);
            ListViewGroup group5 = new ListViewGroup("Other", HorizontalAlignment.Left);
            ComponentResourceManager manager = new ComponentResourceManager(typeof(AppCacheOptions));
            this.cbNetworkFallback = new CheckBox();
            this.txtBase = new TextBox();
            this.lblBase = new Label();
            this.btnSave = new Button();
            this.btnCancel = new Button();
            this.lvItems = new ListView();
            this.colURL = new ColumnHeader();
            this.colSize = new ColumnHeader();
            this.colType = new ColumnHeader();
            this.lblResources = new Label();
            this.lblinstructions = new Label();
            base.SuspendLayout();
            this.cbNetworkFallback.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.cbNetworkFallback.AutoSize = true;
            this.cbNetworkFallback.Checked = true;
            this.cbNetworkFallback.CheckState = CheckState.Checked;
            this.cbNetworkFallback.Location = new Point(12, 0x177);
            this.cbNetworkFallback.Name = "cbNetworkFallback";
            this.cbNetworkFallback.Size = new Size(0xaf, 0x11);
            this.cbNetworkFallback.TabIndex = 3;
            this.cbNetworkFallback.Text = "&Allow Network for unlisted items";
            this.cbNetworkFallback.UseVisualStyleBackColor = true;
            this.txtBase.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.txtBase.Location = new Point(0xe5, 0x188);
            this.txtBase.Name = "txtBase";
            this.txtBase.Size = new Size(0x1fc, 20);
            this.txtBase.TabIndex = 5;
            this.lblBase.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.lblBase.AutoSize = true;
            this.lblBase.Location = new Point(0xe2, 0x178);
            this.lblBase.Name = "lblBase";
            this.lblBase.Size = new Size(0x3b, 13);
            this.lblBase.TabIndex = 4;
            this.lblBase.Text = "&Base URL:";
            this.btnSave.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnSave.DialogResult = DialogResult.OK;
            this.btnSave.Location = new Point(0x2f3, 0x177);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new Size(0x4b, 0x2e);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnCancel.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new Point(12, 0x18e);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x4b, 0x17);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.lvItems.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.lvItems.CheckBoxes = true;
            this.lvItems.Columns.AddRange(new ColumnHeader[] { this.colURL, this.colSize, this.colType });
            this.lvItems.FullRowSelect = true;
            group.Header = "Markup";
            group.Name = "lvgMarkup";
            group2.Header = "Images";
            group2.Name = "lvgImages";
            group3.Header = "CSS";
            group3.Name = "lvgCSS";
            group4.Header = "Script";
            group4.Name = "lvgScript";
            group5.Header = "Other";
            group5.Name = "lvgOther";
            this.lvItems.Groups.AddRange(new ListViewGroup[] { group, group2, group3, group4, group5 });
            this.lvItems.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            this.lvItems.HideSelection = false;
            this.lvItems.LabelEdit = true;
            this.lvItems.Location = new Point(12, 0x3e);
            this.lvItems.Name = "lvItems";
            this.lvItems.Size = new Size(0x331, 0x133);
            this.lvItems.Sorting = SortOrder.Ascending;
            this.lvItems.TabIndex = 2;
            this.lvItems.UseCompatibleStateImageBehavior = false;
            this.lvItems.View = View.Details;
            this.lvItems.KeyDown += new KeyEventHandler(this.lvItems_KeyDown);
            this.colURL.Text = "URL";
            this.colURL.Width = 450;
            this.colSize.Text = "Size";
            this.colSize.TextAlign = HorizontalAlignment.Right;
            this.colSize.Width = 100;
            this.colType.Text = "Type";
            this.colType.Width = 150;
            this.lblResources.AutoSize = true;
            this.lblResources.Location = new Point(12, 0x2d);
            this.lblResources.Name = "lblResources";
            this.lblResources.Size = new Size(0x48, 13);
            this.lblResources.TabIndex = 1;
            this.lblResources.Text = "&Resource List";
            this.lblinstructions.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.lblinstructions.ForeColor = SystemColors.ControlDarkDark;
            this.lblinstructions.Location = new Point(12, 5);
            this.lblinstructions.Name = "lblinstructions";
            this.lblinstructions.Size = new Size(0x331, 0x29);
            this.lblinstructions.TabIndex = 0;
            this.lblinstructions.Text = manager.GetString("lblinstructions.Text");
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(0x34a, 0x1b1);
            base.ControlBox = false;
            base.Controls.Add(this.txtBase);
            base.Controls.Add(this.lblinstructions);
            base.Controls.Add(this.lblResources);
            base.Controls.Add(this.lvItems);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.btnSave);
            base.Controls.Add(this.lblBase);
            base.Controls.Add(this.cbNetworkFallback);
            base.Name = "AppCacheOptions";
            base.ShowIcon = false;
            this.Text = "Adjust AppCache Manifest";
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void lvItems_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                foreach (ListViewItem item in this.lvItems.SelectedItems)
                {
                    this.lvItems.Items.Remove(item);
                }
            }
            if ((e.Modifiers == Keys.Control) && (e.KeyCode == Keys.C))
            {
                StringBuilder builder = new StringBuilder();
                foreach (ListViewItem item2 in this.lvItems.SelectedItems)
                {
                    builder.AppendLine(item2.Text);
                }
                Clipboard.SetText(builder.ToString());
            }
        }
    }
}

