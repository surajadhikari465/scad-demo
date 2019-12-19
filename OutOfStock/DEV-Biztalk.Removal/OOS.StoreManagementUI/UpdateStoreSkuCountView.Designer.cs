namespace OOS.StoreManagementUI
{
    partial class UpdateStoreSkuCountView
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
            this.regionComboBox = new System.Windows.Forms.ComboBox();
            this.storeComboBox = new System.Windows.Forms.ComboBox();
            this.teamComboBox = new System.Windows.Forms.ComboBox();
            this.skuCountTexBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.updateStoreSku = new System.Windows.Forms.LinkLabel();
            this.refresh = new System.Windows.Forms.LinkLabel();
            this.updateSkuResultLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // regionComboBox
            // 
            this.regionComboBox.FormattingEnabled = true;
            this.regionComboBox.Location = new System.Drawing.Point(27, 54);
            this.regionComboBox.Name = "regionComboBox";
            this.regionComboBox.Size = new System.Drawing.Size(121, 21);
            this.regionComboBox.TabIndex = 0;
            this.regionComboBox.Text = "---Select a region---";
            this.regionComboBox.SelectedIndexChanged += new System.EventHandler(this.regionComboBox_SelectedIndexChanged);
            // 
            // storeComboBox
            // 
            this.storeComboBox.FormattingEnabled = true;
            this.storeComboBox.Location = new System.Drawing.Point(27, 102);
            this.storeComboBox.Name = "storeComboBox";
            this.storeComboBox.Size = new System.Drawing.Size(121, 21);
            this.storeComboBox.TabIndex = 1;
            this.storeComboBox.Text = "---Select a store---";
            this.storeComboBox.SelectedIndexChanged += new System.EventHandler(this.storeComboBox_SelectedIndexChanged);
            // 
            // teamComboBox
            // 
            this.teamComboBox.FormattingEnabled = true;
            this.teamComboBox.Location = new System.Drawing.Point(27, 149);
            this.teamComboBox.Name = "teamComboBox";
            this.teamComboBox.Size = new System.Drawing.Size(121, 21);
            this.teamComboBox.TabIndex = 2;
            this.teamComboBox.Text = "---Select a team---";
            // 
            // skuCountTexBox
            // 
            this.skuCountTexBox.Location = new System.Drawing.Point(27, 212);
            this.skuCountTexBox.Name = "skuCountTexBox";
            this.skuCountTexBox.Size = new System.Drawing.Size(100, 20);
            this.skuCountTexBox.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 196);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Enter SKU Count:";
            // 
            // updateStoreSku
            // 
            this.updateStoreSku.AutoSize = true;
            this.updateStoreSku.Location = new System.Drawing.Point(207, 54);
            this.updateStoreSku.Name = "updateStoreSku";
            this.updateStoreSku.Size = new System.Drawing.Size(42, 13);
            this.updateStoreSku.TabIndex = 5;
            this.updateStoreSku.TabStop = true;
            this.updateStoreSku.Text = "Update";
            this.updateStoreSku.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.updateStoreSku_LinkClicked);
            // 
            // refresh
            // 
            this.refresh.AutoSize = true;
            this.refresh.Location = new System.Drawing.Point(207, 102);
            this.refresh.Name = "refresh";
            this.refresh.Size = new System.Drawing.Size(44, 13);
            this.refresh.TabIndex = 6;
            this.refresh.TabStop = true;
            this.refresh.Text = "Refresh";
            this.refresh.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.refresh_LinkClicked);
            // 
            // updateSkuResultLabel
            // 
            this.updateSkuResultLabel.AutoSize = true;
            this.updateSkuResultLabel.Location = new System.Drawing.Point(27, 18);
            this.updateSkuResultLabel.Name = "updateSkuResultLabel";
            this.updateSkuResultLabel.Size = new System.Drawing.Size(140, 13);
            this.updateSkuResultLabel.TabIndex = 7;
            this.updateSkuResultLabel.Text = "Update SKU result message";
            // 
            // UpdateStoreSkuCountView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.updateSkuResultLabel);
            this.Controls.Add(this.refresh);
            this.Controls.Add(this.updateStoreSku);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.skuCountTexBox);
            this.Controls.Add(this.teamComboBox);
            this.Controls.Add(this.storeComboBox);
            this.Controls.Add(this.regionComboBox);
            this.Name = "UpdateStoreSkuCountView";
            this.Text = "UpdateStoreSkuCountView";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox regionComboBox;
        private System.Windows.Forms.ComboBox storeComboBox;
        private System.Windows.Forms.ComboBox teamComboBox;
        private System.Windows.Forms.TextBox skuCountTexBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel updateStoreSku;
        private System.Windows.Forms.LinkLabel refresh;
        private System.Windows.Forms.Label updateSkuResultLabel;
    }
}