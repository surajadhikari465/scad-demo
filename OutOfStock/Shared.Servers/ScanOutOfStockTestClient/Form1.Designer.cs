namespace ScanOutOfStockTestClient
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
            this.pingCommand = new System.Windows.Forms.Button();
            this.getRegionsCommand = new System.Windows.Forms.Button();
            this.getStoresCommand = new System.Windows.Forms.Button();
            this.regionListView = new System.Windows.Forms.ListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.storeListView = new System.Windows.Forms.ListBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.PingBox = new System.Windows.Forms.TextBox();
            this.clearCommand = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.validationMsg = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pingCommand
            // 
            this.pingCommand.Location = new System.Drawing.Point(7, 4);
            this.pingCommand.Name = "pingCommand";
            this.pingCommand.Size = new System.Drawing.Size(75, 23);
            this.pingCommand.TabIndex = 0;
            this.pingCommand.Text = "Ping";
            this.pingCommand.UseVisualStyleBackColor = true;
            this.pingCommand.Click += new System.EventHandler(this.button1_Click);
            // 
            // getRegionsCommand
            // 
            this.getRegionsCommand.Location = new System.Drawing.Point(224, 89);
            this.getRegionsCommand.Name = "getRegionsCommand";
            this.getRegionsCommand.Size = new System.Drawing.Size(82, 23);
            this.getRegionsCommand.TabIndex = 1;
            this.getRegionsCommand.Text = "Get Regions";
            this.getRegionsCommand.UseVisualStyleBackColor = true;
            this.getRegionsCommand.Click += new System.EventHandler(this.button2_Click);
            // 
            // getStoresCommand
            // 
            this.getStoresCommand.Location = new System.Drawing.Point(224, 124);
            this.getStoresCommand.Name = "getStoresCommand";
            this.getStoresCommand.Size = new System.Drawing.Size(84, 23);
            this.getStoresCommand.TabIndex = 2;
            this.getStoresCommand.Text = "Get Stores";
            this.getStoresCommand.UseVisualStyleBackColor = true;
            this.getStoresCommand.Click += new System.EventHandler(this.getStoresCommand_Click);
            // 
            // regionListView
            // 
            this.regionListView.FormattingEnabled = true;
            this.regionListView.ItemHeight = 17;
            this.regionListView.Location = new System.Drawing.Point(7, 8);
            this.regionListView.Name = "regionListView";
            this.regionListView.Size = new System.Drawing.Size(195, 242);
            this.regionListView.TabIndex = 3;
            this.regionListView.SelectedIndexChanged += new System.EventHandler(this.regionListView_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.regionListView);
            this.panel1.Controls.Add(this.getRegionsCommand);
            this.panel1.Controls.Add(this.storeListView);
            this.panel1.Controls.Add(this.getStoresCommand);
            this.panel1.Location = new System.Drawing.Point(18, 119);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(533, 261);
            this.panel1.TabIndex = 4;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // storeListView
            // 
            this.storeListView.FormattingEnabled = true;
            this.storeListView.ItemHeight = 17;
            this.storeListView.Location = new System.Drawing.Point(326, 7);
            this.storeListView.Name = "storeListView";
            this.storeListView.Size = new System.Drawing.Size(194, 242);
            this.storeListView.TabIndex = 5;
            this.storeListView.SelectedIndexChanged += new System.EventHandler(this.storeListView_SelectedIndexChanged);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.pingCommand);
            this.panel2.Controls.Add(this.PingBox);
            this.panel2.Location = new System.Drawing.Point(18, 50);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(339, 39);
            this.panel2.TabIndex = 5;
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 103);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Regions:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // PingBox
            // 
            this.PingBox.Location = new System.Drawing.Point(88, 7);
            this.PingBox.Name = "PingBox";
            this.PingBox.Size = new System.Drawing.Size(100, 20);
            this.PingBox.TabIndex = 0;
            // 
            // clearCommand
            // 
            this.clearCommand.Location = new System.Drawing.Point(476, 50);
            this.clearCommand.Name = "clearCommand";
            this.clearCommand.Size = new System.Drawing.Size(75, 23);
            this.clearCommand.TabIndex = 1;
            this.clearCommand.Text = "Clear All";
            this.clearCommand.UseVisualStyleBackColor = true;
            this.clearCommand.Click += new System.EventHandler(this.ClearCommand_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 17);
            this.label2.TabIndex = 6;
            this.label2.Text = "Ping Service:";
            // 
            // validationMsg
            // 
            this.validationMsg.AutoSize = true;
            this.validationMsg.Location = new System.Drawing.Point(21, 9);
            this.validationMsg.Name = "validationMsg";
            this.validationMsg.Size = new System.Drawing.Size(0, 17);
            this.validationMsg.TabIndex = 7;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(583, 413);
            this.Controls.Add(this.clearCommand);
            this.Controls.Add(this.validationMsg);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Scan Out Of Stock Client";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button pingCommand;
        private System.Windows.Forms.Button getRegionsCommand;
        private System.Windows.Forms.Button getStoresCommand;
        private System.Windows.Forms.ListBox regionListView;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListBox storeListView;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox PingBox;
        private System.Windows.Forms.Button clearCommand;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label validationMsg;
    }
}

