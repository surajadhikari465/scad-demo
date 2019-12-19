namespace OOS.StoreManagementUI
{
    partial class AddStoreView
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
            this.addStoreInRegionComboBox = new System.Windows.Forms.ComboBox();
            this.storeAbbreviationTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.storeNameTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.storeStatusComboBox = new System.Windows.Forms.ComboBox();
            this.addStoreResultLabel = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // addStoreInRegionComboBox
            // 
            this.addStoreInRegionComboBox.FormattingEnabled = true;
            this.addStoreInRegionComboBox.Location = new System.Drawing.Point(28, 70);
            this.addStoreInRegionComboBox.Name = "addStoreInRegionComboBox";
            this.addStoreInRegionComboBox.Size = new System.Drawing.Size(121, 21);
            this.addStoreInRegionComboBox.TabIndex = 0;
            this.addStoreInRegionComboBox.Text = "---Select a region---";
            // 
            // storeAbbreviationTextBox
            // 
            this.storeAbbreviationTextBox.Location = new System.Drawing.Point(187, 71);
            this.storeAbbreviationTextBox.Name = "storeAbbreviationTextBox";
            this.storeAbbreviationTextBox.Size = new System.Drawing.Size(121, 20);
            this.storeAbbreviationTextBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(184, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Abbreviation:";
            // 
            // storeNameTextBox
            // 
            this.storeNameTextBox.Location = new System.Drawing.Point(187, 117);
            this.storeNameTextBox.Name = "storeNameTextBox";
            this.storeNameTextBox.Size = new System.Drawing.Size(121, 20);
            this.storeNameTextBox.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(184, 101);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Name:";
            // 
            // storeStatusComboBox
            // 
            this.storeStatusComboBox.FormattingEnabled = true;
            this.storeStatusComboBox.Location = new System.Drawing.Point(28, 116);
            this.storeStatusComboBox.Name = "storeStatusComboBox";
            this.storeStatusComboBox.Size = new System.Drawing.Size(121, 21);
            this.storeStatusComboBox.TabIndex = 6;
            this.storeStatusComboBox.Text = "---Select a status---";
            // 
            // addStoreResultLabel
            // 
            this.addStoreResultLabel.AutoSize = true;
            this.addStoreResultLabel.Location = new System.Drawing.Point(25, 18);
            this.addStoreResultLabel.Name = "addStoreResultLabel";
            this.addStoreResultLabel.Size = new System.Drawing.Size(125, 13);
            this.addStoreResultLabel.TabIndex = 7;
            this.addStoreResultLabel.Text = "Add store result message";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(353, 70);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(39, 13);
            this.linkLabel1.TabIndex = 8;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Submit";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // linkLabel2
            // 
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.Location = new System.Drawing.Point(353, 116);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(44, 13);
            this.linkLabel2.TabIndex = 9;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "Refresh";
            this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
            // 
            // AddStoreView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(422, 224);
            this.Controls.Add(this.linkLabel2);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.addStoreResultLabel);
            this.Controls.Add(this.storeStatusComboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.storeNameTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.storeAbbreviationTextBox);
            this.Controls.Add(this.addStoreInRegionComboBox);
            this.Name = "AddStoreView";
            this.Text = "Add Store View";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox addStoreInRegionComboBox;
        private System.Windows.Forms.TextBox storeAbbreviationTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox storeNameTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox storeStatusComboBox;
        private System.Windows.Forms.Label addStoreResultLabel;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.LinkLabel linkLabel2;
    }
}