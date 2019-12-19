namespace OOS.StoreManagementUI
{
    partial class StoreManagementTasks
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
            this.ViewStoreSkuCountLink = new System.Windows.Forms.LinkLabel();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.linkLabel3 = new System.Windows.Forms.LinkLabel();
            this.addStoreLinkLabel = new System.Windows.Forms.LinkLabel();
            this.changeStoreStatusLinkLabel = new System.Windows.Forms.LinkLabel();
            this.addStoreStatus = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // ViewStoreSkuCountLink
            // 
            this.ViewStoreSkuCountLink.AutoSize = true;
            this.ViewStoreSkuCountLink.Location = new System.Drawing.Point(214, 50);
            this.ViewStoreSkuCountLink.Name = "ViewStoreSkuCountLink";
            this.ViewStoreSkuCountLink.Size = new System.Drawing.Size(114, 13);
            this.ViewStoreSkuCountLink.TabIndex = 0;
            this.ViewStoreSkuCountLink.TabStop = true;
            this.ViewStoreSkuCountLink.Text = "View Store SKU Count";
            this.ViewStoreSkuCountLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // linkLabel2
            // 
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.Location = new System.Drawing.Point(214, 96);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(117, 13);
            this.linkLabel2.TabIndex = 1;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "Insert Store SKU Count";
            this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
            // 
            // linkLabel3
            // 
            this.linkLabel3.AutoSize = true;
            this.linkLabel3.Location = new System.Drawing.Point(214, 145);
            this.linkLabel3.Name = "linkLabel3";
            this.linkLabel3.Size = new System.Drawing.Size(122, 13);
            this.linkLabel3.TabIndex = 2;
            this.linkLabel3.TabStop = true;
            this.linkLabel3.Text = "Modify Store SKU Count";
            this.linkLabel3.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel3_LinkClicked);
            // 
            // addStoreLinkLabel
            // 
            this.addStoreLinkLabel.AutoSize = true;
            this.addStoreLinkLabel.Location = new System.Drawing.Point(54, 50);
            this.addStoreLinkLabel.Name = "addStoreLinkLabel";
            this.addStoreLinkLabel.Size = new System.Drawing.Size(61, 13);
            this.addStoreLinkLabel.TabIndex = 3;
            this.addStoreLinkLabel.TabStop = true;
            this.addStoreLinkLabel.Text = "Add a store";
            this.addStoreLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.addStoreLinkLabel_LinkClicked);
            // 
            // changeStoreStatusLinkLabel
            // 
            this.changeStoreStatusLinkLabel.AutoSize = true;
            this.changeStoreStatusLinkLabel.Location = new System.Drawing.Point(54, 96);
            this.changeStoreStatusLinkLabel.Name = "changeStoreStatusLinkLabel";
            this.changeStoreStatusLinkLabel.Size = new System.Drawing.Size(101, 13);
            this.changeStoreStatusLinkLabel.TabIndex = 4;
            this.changeStoreStatusLinkLabel.TabStop = true;
            this.changeStoreStatusLinkLabel.Text = "Change store status";
            this.changeStoreStatusLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.changeStoreStatusLinkLabel_LinkClicked);
            // 
            // addStoreStatus
            // 
            this.addStoreStatus.AutoSize = true;
            this.addStoreStatus.Location = new System.Drawing.Point(54, 145);
            this.addStoreStatus.Name = "addStoreStatus";
            this.addStoreStatus.Size = new System.Drawing.Size(83, 13);
            this.addStoreStatus.TabIndex = 5;
            this.addStoreStatus.TabStop = true;
            this.addStoreStatus.Text = "Add store status";
            this.addStoreStatus.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.addStoreStatus_LinkClicked);
            // 
            // StoreManagementTasks
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(490, 260);
            this.Controls.Add(this.addStoreStatus);
            this.Controls.Add(this.changeStoreStatusLinkLabel);
            this.Controls.Add(this.addStoreLinkLabel);
            this.Controls.Add(this.linkLabel3);
            this.Controls.Add(this.linkLabel2);
            this.Controls.Add(this.ViewStoreSkuCountLink);
            this.Name = "StoreManagementTasks";
            this.Text = "Store Manager Tasks";
            this.Load += new System.EventHandler(this.StoreManagement_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel ViewStoreSkuCountLink;
        private System.Windows.Forms.LinkLabel linkLabel2;
        private System.Windows.Forms.LinkLabel linkLabel3;
        private System.Windows.Forms.LinkLabel addStoreLinkLabel;
        private System.Windows.Forms.LinkLabel changeStoreStatusLinkLabel;
        private System.Windows.Forms.LinkLabel addStoreStatus;
    }
}

