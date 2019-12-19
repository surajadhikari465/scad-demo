namespace WFM.Mobile.OOS
{
    partial class Settings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Settings));
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.menuItem_Empty = new System.Windows.Forms.MenuItem();
            this.MenuItem_ExitOOS = new System.Windows.Forms.MenuItem();
            this.menuItem_Save = new System.Windows.Forms.MenuItem();
            this.Panel1 = new System.Windows.Forms.Panel();
            this.panel_Info = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label_Info = new System.Windows.Forms.Label();
            this.textBox_BizTalkURI = new System.Windows.Forms.TextBox();
            this.textBox_OOSURI = new System.Windows.Forms.TextBox();
            this.picNetworkTest = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Label4 = new System.Windows.Forms.Label();
            this.ComboBox_Regions = new System.Windows.Forms.ComboBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.ComboBox_Stores = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.imageList1 = new System.Windows.Forms.ImageList();
            this.Panel1.SuspendLayout();
            this.panel_Info.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.menuItem_Empty);
            this.mainMenu1.MenuItems.Add(this.menuItem_Save);
            // 
            // menuItem_Empty
            // 
            this.menuItem_Empty.MenuItems.Add(this.MenuItem_ExitOOS);
            this.menuItem_Empty.Text = "Menu";
            // 
            // MenuItem_ExitOOS
            // 
            this.MenuItem_ExitOOS.Text = "Exit Out Of Stock";
            this.MenuItem_ExitOOS.Click += new System.EventHandler(this.MenuItem_ExitOOS_Click);
            // 
            // menuItem_Save
            // 
            this.menuItem_Save.Text = "Save";
            this.menuItem_Save.Click += new System.EventHandler(this.menuItem_Save_Click);
            // 
            // Panel1
            // 
            this.Panel1.BackColor = System.Drawing.Color.Wheat;
            this.Panel1.Controls.Add(this.panel_Info);
            this.Panel1.Controls.Add(this.textBox_BizTalkURI);
            this.Panel1.Controls.Add(this.textBox_OOSURI);
            this.Panel1.Controls.Add(this.picNetworkTest);
            this.Panel1.Controls.Add(this.label2);
            this.Panel1.Controls.Add(this.Label4);
            this.Panel1.Controls.Add(this.ComboBox_Regions);
            this.Panel1.Controls.Add(this.Label1);
            this.Panel1.Controls.Add(this.ComboBox_Stores);
            this.Panel1.Controls.Add(this.label5);
            this.Panel1.Controls.Add(this.label3);
            this.Panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Panel1.Location = new System.Drawing.Point(0, 0);
            this.Panel1.Name = "Panel1";
            this.Panel1.Size = new System.Drawing.Size(240, 268);
            
            // 
            // panel_Info
            // 
            this.panel_Info.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel_Info.Controls.Add(this.panel3);
            this.panel_Info.Location = new System.Drawing.Point(7, 125);
            this.panel_Info.Name = "panel_Info";
            this.panel_Info.Size = new System.Drawing.Size(224, 40);
            this.panel_Info.Visible = false;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label_Info);
            this.panel3.Location = new System.Drawing.Point(3, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(218, 33);
            // 
            // label_Info
            // 
            this.label_Info.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.label_Info.Location = new System.Drawing.Point(5, 5);
            this.label_Info.Name = "label_Info";
            this.label_Info.Size = new System.Drawing.Size(202, 27);
            this.label_Info.Text = "label_Info";
            this.label_Info.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // textBox_BizTalkURI
            // 
            this.textBox_BizTalkURI.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.textBox_BizTalkURI.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.textBox_BizTalkURI.Location = new System.Drawing.Point(7, 242);
            this.textBox_BizTalkURI.Name = "textBox_BizTalkURI";
            this.textBox_BizTalkURI.ReadOnly = true;
            this.textBox_BizTalkURI.Size = new System.Drawing.Size(224, 19);
            this.textBox_BizTalkURI.TabIndex = 8;
            // 
            // textBox_OOSURI
            // 
            this.textBox_OOSURI.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.textBox_OOSURI.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.textBox_OOSURI.Location = new System.Drawing.Point(7, 201);
            this.textBox_OOSURI.Name = "textBox_OOSURI";
            this.textBox_OOSURI.ReadOnly = true;
            this.textBox_OOSURI.Size = new System.Drawing.Size(224, 19);
            this.textBox_OOSURI.TabIndex = 5;
            // 
            // picNetworkTest
            // 
            this.picNetworkTest.BackColor = System.Drawing.Color.Transparent;
            this.picNetworkTest.Location = new System.Drawing.Point(215, 9);
            this.picNetworkTest.Name = "picNetworkTest";
            this.picNetworkTest.Size = new System.Drawing.Size(16, 16);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.label2.Location = new System.Drawing.Point(7, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 22);
            this.label2.Text = "Network Connection";
            // 
            // Label4
            // 
            this.Label4.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.Label4.ForeColor = System.Drawing.Color.Black;
            this.Label4.Location = new System.Drawing.Point(3, 34);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(50, 20);
            this.Label4.Text = "Region:";
            this.Label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // ComboBox_Regions
            // 
            this.ComboBox_Regions.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.ComboBox_Regions.Location = new System.Drawing.Point(59, 34);
            this.ComboBox_Regions.Name = "ComboBox_Regions";
            this.ComboBox_Regions.Size = new System.Drawing.Size(60, 20);
            this.ComboBox_Regions.TabIndex = 3;
            this.ComboBox_Regions.SelectedIndexChanged += new System.EventHandler(this.ComboBox_Regions_SelectedIndexChanged);
            // 
            // Label1
            // 
            this.Label1.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.Label1.ForeColor = System.Drawing.Color.Black;
            this.Label1.Location = new System.Drawing.Point(125, 34);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(40, 20);
            this.Label1.Text = "Store:";
            this.Label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // ComboBox_Stores
            // 
            this.ComboBox_Stores.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.ComboBox_Stores.Location = new System.Drawing.Point(171, 34);
            this.ComboBox_Stores.Name = "ComboBox_Stores";
            this.ComboBox_Stores.Size = new System.Drawing.Size(60, 20);
            this.ComboBox_Stores.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.label5.Location = new System.Drawing.Point(7, 225);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(224, 20);
            this.label5.Text = "Service Bus URI";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.label3.Location = new System.Drawing.Point(7, 183);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(224, 20);
            this.label3.Text = "Out Of Stock Web Service";
            this.imageList1.Images.Clear();
            this.imageList1.Images.Add(((System.Drawing.Image)(resources.GetObject("resource"))));
            this.imageList1.Images.Add(((System.Drawing.Image)(resources.GetObject("resource1"))));
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.ControlBox = false;
            this.Controls.Add(this.Panel1);
            this.Menu = this.mainMenu1;
            this.Name = "Settings";
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.Settings_Load);
            this.Panel1.ResumeLayout(false);
            this.panel_Info.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Panel Panel1;
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.ComboBox ComboBox_Regions;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.ComboBox ComboBox_Stores;
        private System.Windows.Forms.MenuItem menuItem_Empty;
        private System.Windows.Forms.PictureBox picNetworkTest;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox_BizTalkURI;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_OOSURI;
        private System.Windows.Forms.Panel panel_Info;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label_Info;
        private System.Windows.Forms.MenuItem menuItem_Save;
        private System.Windows.Forms.MenuItem MenuItem_ExitOOS;
    }
}