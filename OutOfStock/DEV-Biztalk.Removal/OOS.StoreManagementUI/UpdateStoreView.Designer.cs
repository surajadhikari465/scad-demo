namespace OOS.StoreManagementUI
{
    partial class UpdateStoreView
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
            this.statusComboBox = new System.Windows.Forms.ComboBox();
            this.updateStoreLinkLabel = new System.Windows.Forms.LinkLabel();
            this.refreshView = new System.Windows.Forms.LinkLabel();
            this.updateStoreResultLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // regionComboBox
            // 
            this.regionComboBox.FormattingEnabled = true;
            this.regionComboBox.Location = new System.Drawing.Point(40, 57);
            this.regionComboBox.Name = "regionComboBox";
            this.regionComboBox.Size = new System.Drawing.Size(121, 21);
            this.regionComboBox.TabIndex = 0;
            this.regionComboBox.Text = "---Select a region---";
            this.regionComboBox.SelectedIndexChanged += new System.EventHandler(this.regionComboBox_SelectedIndexChanged);
            // 
            // storeComboBox
            // 
            this.storeComboBox.FormattingEnabled = true;
            this.storeComboBox.Location = new System.Drawing.Point(40, 114);
            this.storeComboBox.Name = "storeComboBox";
            this.storeComboBox.Size = new System.Drawing.Size(121, 21);
            this.storeComboBox.TabIndex = 1;
            this.storeComboBox.Text = "---Select a store---";
            // 
            // statusComboBox
            // 
            this.statusComboBox.FormattingEnabled = true;
            this.statusComboBox.Location = new System.Drawing.Point(40, 166);
            this.statusComboBox.Name = "statusComboBox";
            this.statusComboBox.Size = new System.Drawing.Size(121, 21);
            this.statusComboBox.TabIndex = 2;
            this.statusComboBox.Text = "---Select a status---";
            // 
            // updateStoreLinkLabel
            // 
            this.updateStoreLinkLabel.AutoSize = true;
            this.updateStoreLinkLabel.Location = new System.Drawing.Point(296, 64);
            this.updateStoreLinkLabel.Name = "updateStoreLinkLabel";
            this.updateStoreLinkLabel.Size = new System.Drawing.Size(42, 13);
            this.updateStoreLinkLabel.TabIndex = 3;
            this.updateStoreLinkLabel.TabStop = true;
            this.updateStoreLinkLabel.Text = "Update";
            this.updateStoreLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.updateStoreLinkLabel_LinkClicked);
            // 
            // refreshView
            // 
            this.refreshView.AutoSize = true;
            this.refreshView.Location = new System.Drawing.Point(296, 122);
            this.refreshView.Name = "refreshView";
            this.refreshView.Size = new System.Drawing.Size(44, 13);
            this.refreshView.TabIndex = 4;
            this.refreshView.TabStop = true;
            this.refreshView.Text = "Refresh";
            this.refreshView.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.refreshView_LinkClicked);
            // 
            // updateStoreResultLabel
            // 
            this.updateStoreResultLabel.AutoSize = true;
            this.updateStoreResultLabel.Location = new System.Drawing.Point(40, 19);
            this.updateStoreResultLabel.Name = "updateStoreResultLabel";
            this.updateStoreResultLabel.Size = new System.Drawing.Size(141, 13);
            this.updateStoreResultLabel.TabIndex = 5;
            this.updateStoreResultLabel.Text = "Update store result message";
            // 
            // UpdateStoreView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(424, 262);
            this.Controls.Add(this.updateStoreResultLabel);
            this.Controls.Add(this.refreshView);
            this.Controls.Add(this.updateStoreLinkLabel);
            this.Controls.Add(this.statusComboBox);
            this.Controls.Add(this.storeComboBox);
            this.Controls.Add(this.regionComboBox);
            this.Name = "UpdateStoreView";
            this.Text = "UpdateStoreView";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox regionComboBox;
        private System.Windows.Forms.ComboBox storeComboBox;
        private System.Windows.Forms.ComboBox statusComboBox;
        private System.Windows.Forms.LinkLabel updateStoreLinkLabel;
        private System.Windows.Forms.LinkLabel refreshView;
        private System.Windows.Forms.Label updateStoreResultLabel;
    }
}