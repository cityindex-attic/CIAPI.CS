namespace BasicFormats
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class UIFileExport : Form
    {
        private Button btnBrowse;
        private Button btnExport;
        internal CheckBox cbOpenFolder;
        internal CheckBox cbRecreateFolderStructure;
        private IContainer components;
        private GroupBox gbOptions;
        private Label lblTo;
        internal TextBox txtLocation;

        public UIFileExport()
        {
            this.InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog {
                ShowNewFolderButton = true,
                Description = "Select the folder in which files should be placed:"
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.txtLocation.Text = dialog.SelectedPath;
            }
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
            this.btnExport = new Button();
            this.btnBrowse = new Button();
            this.txtLocation = new TextBox();
            this.cbRecreateFolderStructure = new CheckBox();
            this.gbOptions = new GroupBox();
            this.cbOpenFolder = new CheckBox();
            this.lblTo = new Label();
            this.gbOptions.SuspendLayout();
            base.SuspendLayout();
            this.btnExport.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnExport.DialogResult = DialogResult.OK;
            this.btnExport.Location = new Point(0x177, 0x4b);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new Size(0x4d, 0x1d);
            this.btnExport.TabIndex = 0;
            this.btnExport.Text = "&Export >>";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnBrowse.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.btnBrowse.Location = new Point(0x177, 7);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new Size(0x4d, 0x1d);
            this.btnBrowse.TabIndex = 3;
            this.btnBrowse.Text = "&Browse...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new EventHandler(this.btnBrowse_Click);
            this.txtLocation.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.txtLocation.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            this.txtLocation.AutoCompleteSource = AutoCompleteSource.FileSystemDirectories;
            this.txtLocation.Location = new Point(0x3a, 12);
            this.txtLocation.Name = "txtLocation";
            this.txtLocation.Size = new Size(0x137, 0x15);
            this.txtLocation.TabIndex = 2;
            this.cbRecreateFolderStructure.AutoSize = true;
            this.cbRecreateFolderStructure.Checked = true;
            this.cbRecreateFolderStructure.CheckState = CheckState.Checked;
            this.cbRecreateFolderStructure.Location = new Point(0x10, 20);
            this.cbRecreateFolderStructure.Name = "cbRecreateFolderStructure";
            this.cbRecreateFolderStructure.Size = new Size(0x94, 0x11);
            this.cbRecreateFolderStructure.TabIndex = 0;
            this.cbRecreateFolderStructure.Text = "&Recreate folder structure";
            this.cbRecreateFolderStructure.UseVisualStyleBackColor = true;
            this.gbOptions.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gbOptions.Controls.Add(this.cbOpenFolder);
            this.gbOptions.Controls.Add(this.cbRecreateFolderStructure);
            this.gbOptions.Location = new Point(12, 0x27);
            this.gbOptions.Name = "gbOptions";
            this.gbOptions.Size = new Size(0xeb, 0x41);
            this.gbOptions.TabIndex = 4;
            this.gbOptions.TabStop = false;
            this.gbOptions.Text = "Options";
            this.cbOpenFolder.AutoSize = true;
            this.cbOpenFolder.Checked = true;
            this.cbOpenFolder.CheckState = CheckState.Checked;
            this.cbOpenFolder.Location = new Point(0x10, 0x24);
            this.cbOpenFolder.Name = "cbOpenFolder";
            this.cbOpenFolder.Size = new Size(0x9e, 0x11);
            this.cbOpenFolder.TabIndex = 1;
            this.cbOpenFolder.Text = "&Open folder when complete";
            this.cbOpenFolder.UseVisualStyleBackColor = true;
            this.lblTo.AutoSize = true;
            this.lblTo.Location = new Point(15, 15);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new Size(0x21, 13);
            this.lblTo.TabIndex = 1;
            this.lblTo.Text = "&Path:";
            base.AcceptButton = this.btnExport;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x1d0, 0x74);
            base.Controls.Add(this.lblTo);
            base.Controls.Add(this.gbOptions);
            base.Controls.Add(this.txtLocation);
            base.Controls.Add(this.btnBrowse);
            base.Controls.Add(this.btnExport);
            this.Font = new Font("Tahoma", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            base.FormBorderStyle = FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new Size(480, 150);
            base.Name = "UIFileExport";
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "File Exporter";
            this.gbOptions.ResumeLayout(false);
            this.gbOptions.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }
    }
}

