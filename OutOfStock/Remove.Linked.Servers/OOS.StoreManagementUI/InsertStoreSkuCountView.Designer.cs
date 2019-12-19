namespace OOS.StoreManagementUI
{
    partial class InsertStoreSkuCountView
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
            this.skuCount = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.insertSkuCountResult = new System.Windows.Forms.Label();
            this.insertSku = new System.Windows.Forms.LinkLabel();
            this.refresh = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // regionComboBox
            // 
            this.regionComboBox.FormattingEnabled = true;
            this.regionComboBox.Location = new System.Drawing.Point(27, 48);
            this.regionComboBox.Name = "regionComboBox";
            this.regionComboBox.Size = new System.Drawing.Size(121, 21);
            this.regionComboBox.TabIndex = 0;
            this.regionComboBox.Text = "---Select a region---";
            this.regionComboBox.SelectedIndexChanged += new System.EventHandler(this.regionComboBox_SelectedIndexChanged);
            // 
            // storeComboBox
            // 
            this.storeComboBox.FormattingEnabled = true;
            this.storeComboBox.Location = new System.Drawing.Point(27, 92);
            this.storeComboBox.Name = "storeComboBox";
            this.storeComboBox.Size = new System.Drawing.Size(121, 21);
            this.storeComboBox.TabIndex = 1;
            this.storeComboBox.Text = "---Select a store---";
            this.storeComboBox.SelectedIndexChanged += new System.EventHandler(this.storeComboBox_SelectedIndexChanged);
            // 
            // teamComboBox
            // 
            this.teamComboBox.FormattingEnabled = true;
            this.teamComboBox.Location = new System.Drawing.Point(27, 137);
            this.teamComboBox.Name = "teamComboBox";
            this.teamComboBox.Size = new System.Drawing.Size(121, 21);
            this.teamComboBox.TabIndex = 2;
            this.teamComboBox.Text = "---Select a team---";
            // 
            // skuCount
            // 
            this.skuCount.Location = new System.Drawing.Point(27, 190);
            this.skuCount.Name = "skuCount";
            this.skuCount.Size = new System.Drawing.Size(100, 20);
            this.skuCount.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 171);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Enter SKU Count:";
            // 
            // insertSkuCountResult
            // 
            this.insertSkuCountResult.AutoSize = true;
            this.insertSkuCountResult.Location = new System.Drawing.Point(27, 18);
            this.insertSkuCountResult.Name = "insertSkuCountResult";
            this.insertSkuCountResult.Size = new System.Drawing.Size(161, 13);
            this.insertSkuCountResult.TabIndex = 5;
            this.insertSkuCountResult.Text = "Insert SKU count result message";
            // 
            // insertSku
            // 
            this.insertSku.AutoSize = true;
            this.insertSku.Location = new System.Drawing.Point(195, 48);
            this.insertSku.Name = "insertSku";
            this.insertSku.Size = new System.Drawing.Size(33, 13);
            this.insertSku.TabIndex = 6;
            this.insertSku.TabStop = true;
            this.insertSku.Text = "Insert";
            this.insertSku.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.insertSku_LinkClicked);
            // 
            // refresh
            // 
            this.refresh.AutoSize = true;
            this.refresh.Location = new System.Drawing.Point(195, 92);
            this.refresh.Name = "refresh";
            this.refresh.Size = new System.Drawing.Size(44, 13);
            this.refresh.TabIndex = 7;
            this.refresh.TabStop = true;
            this.refresh.Text = "Refresh";
            this.refresh.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.refresh_LinkClicked);
            // 
            // InsertStoreSkuCountView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.refresh);
            this.Controls.Add(this.insertSku);
            this.Controls.Add(this.insertSkuCountResult);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.skuCount);
            this.Controls.Add(this.teamComboBox);
            this.Controls.Add(this.storeComboBox);
            this.Controls.Add(this.regionComboBox);
            this.Name = "InsertStoreSkuCountView";
            this.Text = "InsertStoreSkuCountView";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox regionComboBox;
        private System.Windows.Forms.ComboBox storeComboBox;
        private System.Windows.Forms.ComboBox teamComboBox;
        private System.Windows.Forms.TextBox skuCount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label insertSkuCountResult;
        private System.Windows.Forms.LinkLabel insertSku;
        private System.Windows.Forms.LinkLabel refresh;
    }
}