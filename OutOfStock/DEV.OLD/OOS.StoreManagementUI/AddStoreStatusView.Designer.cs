namespace OOS.StoreManagementUI
{
    partial class AddStoreStatusView
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
            this.label1 = new System.Windows.Forms.Label();
            this.storeStatus = new System.Windows.Forms.TextBox();
            this.addStoreStatus = new System.Windows.Forms.LinkLabel();
            this.addStoreStatusResult = new System.Windows.Forms.Label();
            this.refresh = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Enter store status:";
            // 
            // storeStatus
            // 
            this.storeStatus.Location = new System.Drawing.Point(35, 76);
            this.storeStatus.Name = "storeStatus";
            this.storeStatus.Size = new System.Drawing.Size(100, 20);
            this.storeStatus.TabIndex = 1;
            // 
            // addStoreStatus
            // 
            this.addStoreStatus.AutoSize = true;
            this.addStoreStatus.Location = new System.Drawing.Point(176, 76);
            this.addStoreStatus.Name = "addStoreStatus";
            this.addStoreStatus.Size = new System.Drawing.Size(26, 13);
            this.addStoreStatus.TabIndex = 2;
            this.addStoreStatus.TabStop = true;
            this.addStoreStatus.Text = "Add";
            this.addStoreStatus.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.addStoreStatus_LinkClicked);
            // 
            // addStoreStatusResult
            // 
            this.addStoreStatusResult.AutoSize = true;
            this.addStoreStatusResult.Location = new System.Drawing.Point(32, 20);
            this.addStoreStatusResult.Name = "addStoreStatusResult";
            this.addStoreStatusResult.Size = new System.Drawing.Size(156, 13);
            this.addStoreStatusResult.TabIndex = 3;
            this.addStoreStatusResult.Text = "Add store status result message";
            // 
            // refresh
            // 
            this.refresh.AutoSize = true;
            this.refresh.Location = new System.Drawing.Point(176, 111);
            this.refresh.Name = "refresh";
            this.refresh.Size = new System.Drawing.Size(44, 13);
            this.refresh.TabIndex = 4;
            this.refresh.TabStop = true;
            this.refresh.Text = "Refresh";
            this.refresh.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.refresh_LinkClicked);
            // 
            // AddStoreStatusView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.refresh);
            this.Controls.Add(this.addStoreStatusResult);
            this.Controls.Add(this.addStoreStatus);
            this.Controls.Add(this.storeStatus);
            this.Controls.Add(this.label1);
            this.Name = "AddStoreStatusView";
            this.Text = "AddStoreStatusView";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox storeStatus;
        private System.Windows.Forms.LinkLabel addStoreStatus;
        private System.Windows.Forms.Label addStoreStatusResult;
        private System.Windows.Forms.LinkLabel refresh;
    }
}