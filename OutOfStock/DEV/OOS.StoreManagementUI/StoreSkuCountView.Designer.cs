namespace OOS.StoreManagementUI
{
    partial class StoreSkuCountView
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
            this.skuCountDataGridView = new System.Windows.Forms.DataGridView();
            this.regionComboBox = new System.Windows.Forms.ComboBox();
            this.refreshLink = new System.Windows.Forms.LinkLabel();
            this.skuCountViewResult = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.skuCountDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // skuCountDataGridView
            // 
            this.skuCountDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.skuCountDataGridView.Location = new System.Drawing.Point(25, 91);
            this.skuCountDataGridView.Name = "skuCountDataGridView";
            this.skuCountDataGridView.Size = new System.Drawing.Size(338, 274);
            this.skuCountDataGridView.TabIndex = 0;
            // 
            // regionComboBox
            // 
            this.regionComboBox.FormattingEnabled = true;
            this.regionComboBox.Location = new System.Drawing.Point(25, 55);
            this.regionComboBox.Name = "regionComboBox";
            this.regionComboBox.Size = new System.Drawing.Size(144, 21);
            this.regionComboBox.TabIndex = 1;
            this.regionComboBox.Text = "---Select a region---";
            this.regionComboBox.SelectedIndexChanged += new System.EventHandler(this.regionComboBox_SelectedIndexChanged);
            // 
            // refreshLink
            // 
            this.refreshLink.AutoSize = true;
            this.refreshLink.Location = new System.Drawing.Point(451, 91);
            this.refreshLink.Name = "refreshLink";
            this.refreshLink.Size = new System.Drawing.Size(44, 13);
            this.refreshLink.TabIndex = 3;
            this.refreshLink.TabStop = true;
            this.refreshLink.Text = "Refresh";
            this.refreshLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.refreshLink_LinkClicked);
            // 
            // skuCountViewResult
            // 
            this.skuCountViewResult.AutoSize = true;
            this.skuCountViewResult.Location = new System.Drawing.Point(25, 18);
            this.skuCountViewResult.Name = "skuCountViewResult";
            this.skuCountViewResult.Size = new System.Drawing.Size(157, 13);
            this.skuCountViewResult.TabIndex = 4;
            this.skuCountViewResult.Text = "SKU count view result message";
            // 
            // StoreSkuCountView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(531, 400);
            this.Controls.Add(this.skuCountViewResult);
            this.Controls.Add(this.refreshLink);
            this.Controls.Add(this.regionComboBox);
            this.Controls.Add(this.skuCountDataGridView);
            this.Name = "StoreSkuCountView";
            this.Text = "Store SKU Count View";
            ((System.ComponentModel.ISupportInitialize)(this.skuCountDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView skuCountDataGridView;
        private System.Windows.Forms.ComboBox regionComboBox;
        private System.Windows.Forms.LinkLabel refreshLink;
        private System.Windows.Forms.Label skuCountViewResult;
    }
}