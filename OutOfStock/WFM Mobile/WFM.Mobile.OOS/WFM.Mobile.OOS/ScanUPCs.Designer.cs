namespace WFM.Mobile.OOS
{
    partial class ScanUPCs
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu mainMenu1;

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
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem_Exit = new System.Windows.Forms.MenuItem();
            this.menuItem_Settings = new System.Windows.Forms.MenuItem();
            this.menuItem_Upload = new System.Windows.Forms.MenuItem();
            this.Label_Store = new System.Windows.Forms.Label();
            this.Panel1 = new System.Windows.Forms.Panel();
            this.backupPanel = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.label_backupcount = new System.Windows.Forms.Label();
            this.label_backupdate = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.button_backupremove = new System.Windows.Forms.Button();
            this.button_backupload = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.ManualUpcPanel = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ManualUpcSave = new System.Windows.Forms.Button();
            this.ManualUpcClear = new System.Windows.Forms.Button();
            this.ManualUpcTextBox = new System.Windows.Forms.TextBox();
            this.Label_Status = new System.Windows.Forms.Label();
            this.Label_TotalItems = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Button_Remove = new System.Windows.Forms.Button();
            this.ListBox_ScannedUPCs = new System.Windows.Forms.ListBox();
            this.Label_Region = new System.Windows.Forms.Label();
            this.label_backuptime = new System.Windows.Forms.Label();
            this.Panel1.SuspendLayout();
            this.backupPanel.SuspendLayout();
            this.ManualUpcPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.menuItem1);
            this.mainMenu1.MenuItems.Add(this.menuItem_Upload);
            // 
            // menuItem1
            // 
            this.menuItem1.MenuItems.Add(this.menuItem_Exit);
            this.menuItem1.MenuItems.Add(this.menuItem_Settings);
            this.menuItem1.Text = "Menu";
            // 
            // menuItem_Exit
            // 
            this.menuItem_Exit.Text = "Exit";
            this.menuItem_Exit.Click += new System.EventHandler(this.menuItem_Exit_Click);
            // 
            // menuItem_Settings
            // 
            this.menuItem_Settings.Text = "Settings";
            this.menuItem_Settings.Click += new System.EventHandler(this.menuItem_Settings_Click);
            // 
            // menuItem_Upload
            // 
            this.menuItem_Upload.Text = "Upload";
            this.menuItem_Upload.Click += new System.EventHandler(this.menuItem_Upload_Click);
            // 
            // Label_Store
            // 
            this.Label_Store.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold);
            this.Label_Store.ForeColor = System.Drawing.Color.Black;
            this.Label_Store.Location = new System.Drawing.Point(167, 0);
            this.Label_Store.Name = "Label_Store";
            this.Label_Store.Size = new System.Drawing.Size(63, 29);
            this.Label_Store.Text = "ARD";
            this.Label_Store.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // Panel1
            // 
            this.Panel1.BackColor = System.Drawing.Color.Wheat;
            this.Panel1.Controls.Add(this.backupPanel);
            this.Panel1.Controls.Add(this.ManualUpcPanel);
            this.Panel1.Controls.Add(this.Label_Status);
            this.Panel1.Controls.Add(this.Label_TotalItems);
            this.Panel1.Controls.Add(this.label1);
            this.Panel1.Controls.Add(this.Button_Remove);
            this.Panel1.Controls.Add(this.ListBox_ScannedUPCs);
            this.Panel1.Location = new System.Drawing.Point(7, 32);
            this.Panel1.Name = "Panel1";
            this.Panel1.Size = new System.Drawing.Size(226, 233);
            // 
            // backupPanel
            // 
            this.backupPanel.BackColor = System.Drawing.Color.Wheat;
            this.backupPanel.Controls.Add(this.label_backuptime);
            this.backupPanel.Controls.Add(this.label7);
            this.backupPanel.Controls.Add(this.label_backupcount);
            this.backupPanel.Controls.Add(this.label_backupdate);
            this.backupPanel.Controls.Add(this.label6);
            this.backupPanel.Controls.Add(this.label5);
            this.backupPanel.Controls.Add(this.button_backupremove);
            this.backupPanel.Controls.Add(this.button_backupload);
            this.backupPanel.Controls.Add(this.textBox1);
            this.backupPanel.Location = new System.Drawing.Point(0, 0);
            this.backupPanel.Name = "backupPanel";
            this.backupPanel.Size = new System.Drawing.Size(226, 231);
            this.backupPanel.Visible = false;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(7, 154);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(82, 20);
            this.label7.Text = "Backup Time";
            // 
            // label_backupcount
            // 
            this.label_backupcount.Location = new System.Drawing.Point(141, 174);
            this.label_backupcount.Name = "label_backupcount";
            this.label_backupcount.Size = new System.Drawing.Size(82, 20);
            this.label_backupcount.Text = "99";
            this.label_backupcount.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label_backupdate
            // 
            this.label_backupdate.Location = new System.Drawing.Point(125, 135);
            this.label_backupdate.Name = "label_backupdate";
            this.label_backupdate.Size = new System.Drawing.Size(98, 20);
            this.label_backupdate.Text = "mm/dd/yyy";
            this.label_backupdate.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(7, 174);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(100, 20);
            this.label6.Text = "Number of Items";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(7, 135);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 20);
            this.label5.Text = "Backup Date";
            // 
            // button_backupremove
            // 
            this.button_backupremove.Location = new System.Drawing.Point(147, 195);
            this.button_backupremove.Name = "button_backupremove";
            this.button_backupremove.Size = new System.Drawing.Size(72, 20);
            this.button_backupremove.TabIndex = 2;
            this.button_backupremove.Text = "Remove";
            this.button_backupremove.Click += new System.EventHandler(this.button_backupremove_Click);
            // 
            // button_backupload
            // 
            this.button_backupload.Location = new System.Drawing.Point(7, 195);
            this.button_backupload.Name = "button_backupload";
            this.button_backupload.Size = new System.Drawing.Size(72, 20);
            this.button_backupload.TabIndex = 1;
            this.button_backupload.Text = "Load";
            this.button_backupload.Click += new System.EventHandler(this.button_backupload_Click);
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular);
            this.textBox1.Location = new System.Drawing.Point(3, 6);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(220, 126);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "An error occured when the last batch was uploaded. Items were saved. Would you li" +
                "ke to reload these items? \r\n\r\nIf not, the backup list will be removed.";
            // 
            // ManualUpcPanel
            // 
            this.ManualUpcPanel.BackColor = System.Drawing.Color.Wheat;
            this.ManualUpcPanel.Controls.Add(this.label4);
            this.ManualUpcPanel.Controls.Add(this.label3);
            this.ManualUpcPanel.Controls.Add(this.label2);
            this.ManualUpcPanel.Controls.Add(this.ManualUpcSave);
            this.ManualUpcPanel.Controls.Add(this.ManualUpcClear);
            this.ManualUpcPanel.Controls.Add(this.ManualUpcTextBox);
            this.ManualUpcPanel.Location = new System.Drawing.Point(0, 3);
            this.ManualUpcPanel.Name = "ManualUpcPanel";
            this.ManualUpcPanel.Size = new System.Drawing.Size(226, 116);
            this.ManualUpcPanel.Visible = false;
            this.ManualUpcPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.ManualUpcPanel_Paint);
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.label4.Location = new System.Drawing.Point(125, 5);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(98, 18);
            this.label4.Text = "Enter = Save";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.label3.Location = new System.Drawing.Point(3, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 18);
            this.label3.Text = "ESC = Close";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(17, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(190, 18);
            this.label2.Text = "Manually Enter UPC";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // ManualUpcSave
            // 
            this.ManualUpcSave.Location = new System.Drawing.Point(135, 77);
            this.ManualUpcSave.Name = "ManualUpcSave";
            this.ManualUpcSave.Size = new System.Drawing.Size(73, 23);
            this.ManualUpcSave.TabIndex = 2;
            this.ManualUpcSave.Text = "Save";
            this.ManualUpcSave.Click += new System.EventHandler(this.ManualUpcSave_Click);
            // 
            // ManualUpcClear
            // 
            this.ManualUpcClear.Location = new System.Drawing.Point(16, 77);
            this.ManualUpcClear.Name = "ManualUpcClear";
            this.ManualUpcClear.Size = new System.Drawing.Size(73, 23);
            this.ManualUpcClear.TabIndex = 1;
            this.ManualUpcClear.Text = "Clear";
            this.ManualUpcClear.Click += new System.EventHandler(this.ManualUpcClear_Click);
            // 
            // ManualUpcTextBox
            // 
            this.ManualUpcTextBox.Location = new System.Drawing.Point(13, 45);
            this.ManualUpcTextBox.MaxLength = 13;
            this.ManualUpcTextBox.Name = "ManualUpcTextBox";
            this.ManualUpcTextBox.Size = new System.Drawing.Size(195, 21);
            this.ManualUpcTextBox.TabIndex = 0;
            this.ManualUpcTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ManualUpcTextBox_KeyPress);
            // 
            // Label_Status
            // 
            this.Label_Status.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.Label_Status.ForeColor = System.Drawing.Color.Green;
            this.Label_Status.Location = new System.Drawing.Point(3, 0);
            this.Label_Status.Name = "Label_Status";
            this.Label_Status.Size = new System.Drawing.Size(220, 20);
            this.Label_Status.Text = "Ready To Scan Items!";
            this.Label_Status.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // Label_TotalItems
            // 
            this.Label_TotalItems.Location = new System.Drawing.Point(147, 218);
            this.Label_TotalItems.Name = "Label_TotalItems";
            this.Label_TotalItems.Size = new System.Drawing.Size(76, 13);
            this.Label_TotalItems.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(3, 218);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 15);
            this.label1.Text = "Total Items:";
            // 
            // Button_Remove
            // 
            this.Button_Remove.BackColor = System.Drawing.Color.DarkBlue;
            this.Button_Remove.ForeColor = System.Drawing.Color.White;
            this.Button_Remove.Location = new System.Drawing.Point(3, 191);
            this.Button_Remove.Name = "Button_Remove";
            this.Button_Remove.Size = new System.Drawing.Size(220, 24);
            this.Button_Remove.TabIndex = 5;
            this.Button_Remove.Text = "Remove UPC from list";
            this.Button_Remove.Click += new System.EventHandler(this.Button_Remove_Click);
            // 
            // ListBox_ScannedUPCs
            // 
            this.ListBox_ScannedUPCs.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.ListBox_ScannedUPCs.Location = new System.Drawing.Point(3, 23);
            this.ListBox_ScannedUPCs.Name = "ListBox_ScannedUPCs";
            this.ListBox_ScannedUPCs.Size = new System.Drawing.Size(220, 162);
            this.ListBox_ScannedUPCs.TabIndex = 2;
            this.ListBox_ScannedUPCs.SelectedIndexChanged += new System.EventHandler(this.ListBox_ScannedUPCs_SelectedIndexChanged);
            // 
            // Label_Region
            // 
            this.Label_Region.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold);
            this.Label_Region.ForeColor = System.Drawing.Color.Black;
            this.Label_Region.Location = new System.Drawing.Point(7, 0);
            this.Label_Region.Name = "Label_Region";
            this.Label_Region.Size = new System.Drawing.Size(48, 31);
            this.Label_Region.Text = "NC";
            // 
            // label_backuptime
            // 
            this.label_backuptime.Location = new System.Drawing.Point(125, 154);
            this.label_backuptime.Name = "label_backuptime";
            this.label_backuptime.Size = new System.Drawing.Size(98, 20);
            this.label_backuptime.Text = "hh:mm:ss";
            this.label_backuptime.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // ScanUPCs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.ControlBox = false;
            this.Controls.Add(this.Label_Region);
            this.Controls.Add(this.Label_Store);
            this.Controls.Add(this.Panel1);
            this.KeyPreview = true;
            this.Menu = this.mainMenu1;
            this.Name = "ScanUPCs";
            this.Text = "OOS Scan UPCs";
            this.Load += new System.EventHandler(this.ScanUPCs_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ScanUPCs_KeyPress);
            this.Panel1.ResumeLayout(false);
            this.backupPanel.ResumeLayout(false);
            this.ManualUpcPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItem_Exit;
        internal System.Windows.Forms.Label Label_Store;
        internal System.Windows.Forms.Panel Panel1;
        internal System.Windows.Forms.Button Button_Remove;
        internal System.Windows.Forms.ListBox ListBox_ScannedUPCs;
        private System.Windows.Forms.MenuItem menuItem_Upload;
        private System.Windows.Forms.Label Label_TotalItems;
        private System.Windows.Forms.Label label1;
        internal System.Windows.Forms.Label Label_Region;
        internal System.Windows.Forms.Label Label_Status;
        private System.Windows.Forms.MenuItem menuItem_Settings;
        private System.Windows.Forms.Panel ManualUpcPanel;
        private System.Windows.Forms.Button ManualUpcSave;
        private System.Windows.Forms.Button ManualUpcClear;
        private System.Windows.Forms.TextBox ManualUpcTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel backupPanel;
        private System.Windows.Forms.Button button_backupremove;
        private System.Windows.Forms.Button button_backupload;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label_backupcount;
        private System.Windows.Forms.Label label_backupdate;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label_backuptime;
    }
}