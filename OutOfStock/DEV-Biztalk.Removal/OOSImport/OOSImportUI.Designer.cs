namespace OOSImport
{
    partial class OOSImportUI
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
            this.textBox_Log = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.listView_FilesToImport = new System.Windows.Forms.ListView();
            this.columnHeader_Guid = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_Format = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_Date = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_Store = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_File = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox_Region = new System.Windows.Forms.ComboBox();
            this.comboBox_Store = new System.Windows.Forms.ComboBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.button_ChooseFiles = new System.Windows.Forms.Button();
            this.groupBox_Select = new System.Windows.Forms.GroupBox();
            this.checkBox_Validation = new System.Windows.Forms.CheckBox();
            this.dateTimePicker_ScanDate = new System.Windows.Forms.DateTimePicker();
            this.comboBox_Format = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.button_ClearQueue = new System.Windows.Forms.Button();
            this.groupBox_ImportQueue = new System.Windows.Forms.GroupBox();
            this.groupBox_Log = new System.Windows.Forms.GroupBox();
            this.button_Import = new System.Windows.Forms.Button();
            this.splitContainer_Outer = new System.Windows.Forms.SplitContainer();
            this.splitContainer_TopInner = new System.Windows.Forms.SplitContainer();
            this.groupBox_Select.SuspendLayout();
            this.groupBox_ImportQueue.SuspendLayout();
            this.groupBox_Log.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_Outer)).BeginInit();
            this.splitContainer_Outer.Panel1.SuspendLayout();
            this.splitContainer_Outer.Panel2.SuspendLayout();
            this.splitContainer_Outer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_TopInner)).BeginInit();
            this.splitContainer_TopInner.Panel1.SuspendLayout();
            this.splitContainer_TopInner.Panel2.SuspendLayout();
            this.splitContainer_TopInner.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox_Log
            // 
            this.textBox_Log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Log.HideSelection = false;
            this.textBox_Log.Location = new System.Drawing.Point(2, 15);
            this.textBox_Log.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textBox_Log.Multiline = true;
            this.textBox_Log.Name = "textBox_Log";
            this.textBox_Log.ReadOnly = true;
            this.textBox_Log.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_Log.Size = new System.Drawing.Size(498, 76);
            this.textBox_Log.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(276, 15);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Store";
            // 
            // listView_FilesToImport
            // 
            this.listView_FilesToImport.AutoArrange = false;
            this.listView_FilesToImport.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader_Guid,
            this.columnHeader_Format,
            this.columnHeader_Date,
            this.columnHeader_Store,
            this.columnHeader_File});
            this.listView_FilesToImport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView_FilesToImport.FullRowSelect = true;
            this.listView_FilesToImport.GridLines = true;
            this.listView_FilesToImport.HideSelection = false;
            this.listView_FilesToImport.LabelEdit = true;
            this.listView_FilesToImport.LabelWrap = false;
            this.listView_FilesToImport.Location = new System.Drawing.Point(2, 15);
            this.listView_FilesToImport.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.listView_FilesToImport.Name = "listView_FilesToImport";
            this.listView_FilesToImport.ShowGroups = false;
            this.listView_FilesToImport.ShowItemToolTips = true;
            this.listView_FilesToImport.Size = new System.Drawing.Size(498, 115);
            this.listView_FilesToImport.TabIndex = 0;
            this.listView_FilesToImport.UseCompatibleStateImageBehavior = false;
            this.listView_FilesToImport.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader_Guid
            // 
            this.columnHeader_Guid.Text = "";
            this.columnHeader_Guid.Width = 0;
            // 
            // columnHeader_Format
            // 
            this.columnHeader_Format.Text = "Format";
            // 
            // columnHeader_Date
            // 
            this.columnHeader_Date.Text = "Scanned";
            this.columnHeader_Date.Width = 66;
            // 
            // columnHeader_Store
            // 
            this.columnHeader_Store.Tag = "store";
            this.columnHeader_Store.Text = "Store";
            // 
            // columnHeader_File
            // 
            this.columnHeader_File.Tag = "file";
            this.columnHeader_File.Text = "File";
            this.columnHeader_File.Width = 471;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(182, 15);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Region";
            // 
            // comboBox_Region
            // 
            this.comboBox_Region.FormattingEnabled = true;
            this.comboBox_Region.Location = new System.Drawing.Point(184, 31);
            this.comboBox_Region.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.comboBox_Region.Name = "comboBox_Region";
            this.comboBox_Region.Size = new System.Drawing.Size(91, 21);
            this.comboBox_Region.TabIndex = 2;
            this.comboBox_Region.SelectedIndexChanged += new System.EventHandler(this.comboBox_Region_SelectedIndexChanged);
            // 
            // comboBox_Store
            // 
            this.comboBox_Store.FormattingEnabled = true;
            this.comboBox_Store.Location = new System.Drawing.Point(278, 31);
            this.comboBox_Store.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.comboBox_Store.Name = "comboBox_Store";
            this.comboBox_Store.Size = new System.Drawing.Size(106, 21);
            this.comboBox_Store.TabIndex = 3;
            this.comboBox_Store.SelectedIndexChanged += new System.EventHandler(this.comboBox_Store_SelectedIndexChanged);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Multiselect = true;
            this.openFileDialog1.Title = "Select WIMP files for store";
            // 
            // button_ChooseFiles
            // 
            this.button_ChooseFiles.Location = new System.Drawing.Point(7, 56);
            this.button_ChooseFiles.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button_ChooseFiles.Name = "button_ChooseFiles";
            this.button_ChooseFiles.Size = new System.Drawing.Size(83, 19);
            this.button_ChooseFiles.TabIndex = 4;
            this.button_ChooseFiles.Text = "Files to Import";
            this.button_ChooseFiles.UseVisualStyleBackColor = true;
            this.button_ChooseFiles.Click += new System.EventHandler(this.button_ChooseFiles_Click);
            // 
            // groupBox_Select
            // 
            this.groupBox_Select.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox_Select.Controls.Add(this.checkBox_Validation);
            this.groupBox_Select.Controls.Add(this.dateTimePicker_ScanDate);
            this.groupBox_Select.Controls.Add(this.comboBox_Format);
            this.groupBox_Select.Controls.Add(this.label1);
            this.groupBox_Select.Controls.Add(this.label3);
            this.groupBox_Select.Controls.Add(this.label4);
            this.groupBox_Select.Controls.Add(this.comboBox_Region);
            this.groupBox_Select.Controls.Add(this.button_ChooseFiles);
            this.groupBox_Select.Controls.Add(this.label2);
            this.groupBox_Select.Controls.Add(this.comboBox_Store);
            this.groupBox_Select.Location = new System.Drawing.Point(1, 1);
            this.groupBox_Select.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox_Select.Name = "groupBox_Select";
            this.groupBox_Select.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox_Select.Size = new System.Drawing.Size(393, 80);
            this.groupBox_Select.TabIndex = 0;
            this.groupBox_Select.TabStop = false;
            this.groupBox_Select.Text = "Select";
            // 
            // checkBox_Validation
            // 
            this.checkBox_Validation.AutoSize = true;
            this.checkBox_Validation.Location = new System.Drawing.Point(94, 58);
            this.checkBox_Validation.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.checkBox_Validation.Name = "checkBox_Validation";
            this.checkBox_Validation.Size = new System.Drawing.Size(124, 14);
            this.checkBox_Validation.TabIndex = 5;
            this.checkBox_Validation.Text = "Validate but do not store data";
            this.checkBox_Validation.UseVisualStyleBackColor = true;
            // 
            // dateTimePicker_ScanDate
            // 
            this.dateTimePicker_ScanDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker_ScanDate.Location = new System.Drawing.Point(94, 32);
            this.dateTimePicker_ScanDate.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dateTimePicker_ScanDate.Name = "dateTimePicker_ScanDate";
            this.dateTimePicker_ScanDate.Size = new System.Drawing.Size(86, 20);
            this.dateTimePicker_ScanDate.TabIndex = 1;
            // 
            // comboBox_Format
            // 
            this.comboBox_Format.FormattingEnabled = true;
            this.comboBox_Format.Location = new System.Drawing.Point(7, 31);
            this.comboBox_Format.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.comboBox_Format.Name = "comboBox_Format";
            this.comboBox_Format.Size = new System.Drawing.Size(84, 21);
            this.comboBox_Format.TabIndex = 0;
            this.comboBox_Format.SelectedIndexChanged += new System.EventHandler(this.comboBox_Format_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(92, 15);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Scan Date";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 15);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Format";
            // 
            // button_ClearQueue
            // 
            this.button_ClearQueue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_ClearQueue.Location = new System.Drawing.Point(419, 50);
            this.button_ClearQueue.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button_ClearQueue.Name = "button_ClearQueue";
            this.button_ClearQueue.Size = new System.Drawing.Size(74, 19);
            this.button_ClearQueue.TabIndex = 2;
            this.button_ClearQueue.Text = "Clear Queue";
            this.button_ClearQueue.UseVisualStyleBackColor = true;
            this.button_ClearQueue.Click += new System.EventHandler(this.button_ClearQueue_Click);
            // 
            // groupBox_ImportQueue
            // 
            this.groupBox_ImportQueue.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox_ImportQueue.Controls.Add(this.listView_FilesToImport);
            this.groupBox_ImportQueue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_ImportQueue.Location = new System.Drawing.Point(0, 0);
            this.groupBox_ImportQueue.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox_ImportQueue.Name = "groupBox_ImportQueue";
            this.groupBox_ImportQueue.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox_ImportQueue.Size = new System.Drawing.Size(502, 132);
            this.groupBox_ImportQueue.TabIndex = 0;
            this.groupBox_ImportQueue.TabStop = false;
            this.groupBox_ImportQueue.Text = "Import Queue";
            // 
            // groupBox_Log
            // 
            this.groupBox_Log.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox_Log.Controls.Add(this.textBox_Log);
            this.groupBox_Log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_Log.Location = new System.Drawing.Point(0, 0);
            this.groupBox_Log.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox_Log.Name = "groupBox_Log";
            this.groupBox_Log.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox_Log.Size = new System.Drawing.Size(502, 93);
            this.groupBox_Log.TabIndex = 0;
            this.groupBox_Log.TabStop = false;
            this.groupBox_Log.Text = "Log";
            // 
            // button_Import
            // 
            this.button_Import.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Import.Location = new System.Drawing.Point(419, 10);
            this.button_Import.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button_Import.Name = "button_Import";
            this.button_Import.Size = new System.Drawing.Size(74, 19);
            this.button_Import.TabIndex = 1;
            this.button_Import.Text = "Import";
            this.button_Import.UseVisualStyleBackColor = true;
            this.button_Import.Click += new System.EventHandler(this.button_Import_Click);
            // 
            // splitContainer_Outer
            // 
            this.splitContainer_Outer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_Outer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_Outer.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.splitContainer_Outer.Name = "splitContainer_Outer";
            this.splitContainer_Outer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer_Outer.Panel1
            // 
            this.splitContainer_Outer.Panel1.Controls.Add(this.splitContainer_TopInner);
            // 
            // splitContainer_Outer.Panel2
            // 
            this.splitContainer_Outer.Panel2.Controls.Add(this.groupBox_Log);
            this.splitContainer_Outer.Size = new System.Drawing.Size(502, 329);
            this.splitContainer_Outer.SplitterDistance = 233;
            this.splitContainer_Outer.SplitterWidth = 3;
            this.splitContainer_Outer.TabIndex = 14;
            // 
            // splitContainer_TopInner
            // 
            this.splitContainer_TopInner.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_TopInner.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer_TopInner.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_TopInner.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.splitContainer_TopInner.Name = "splitContainer_TopInner";
            this.splitContainer_TopInner.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer_TopInner.Panel1
            // 
            this.splitContainer_TopInner.Panel1.Controls.Add(this.button_ClearQueue);
            this.splitContainer_TopInner.Panel1.Controls.Add(this.groupBox_Select);
            this.splitContainer_TopInner.Panel1.Controls.Add(this.button_Import);
            // 
            // splitContainer_TopInner.Panel2
            // 
            this.splitContainer_TopInner.Panel2.Controls.Add(this.groupBox_ImportQueue);
            this.splitContainer_TopInner.Size = new System.Drawing.Size(502, 233);
            this.splitContainer_TopInner.SplitterDistance = 98;
            this.splitContainer_TopInner.SplitterWidth = 3;
            this.splitContainer_TopInner.TabIndex = 12;
            // 
            // OOSImportUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(502, 329);
            this.Controls.Add(this.splitContainer_Outer);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "OOSImportUI";
            this.Text = "OOS Import";
            this.groupBox_Select.ResumeLayout(false);
            this.groupBox_Select.PerformLayout();
            this.groupBox_ImportQueue.ResumeLayout(false);
            this.groupBox_Log.ResumeLayout(false);
            this.groupBox_Log.PerformLayout();
            this.splitContainer_Outer.Panel1.ResumeLayout(false);
            this.splitContainer_Outer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_Outer)).EndInit();
            this.splitContainer_Outer.ResumeLayout(false);
            this.splitContainer_TopInner.Panel1.ResumeLayout(false);
            this.splitContainer_TopInner.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_TopInner)).EndInit();
            this.splitContainer_TopInner.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.TextBox textBox_Log;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.ListView listView_FilesToImport;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBox_Region;
        private System.Windows.Forms.ComboBox comboBox_Store;
        public System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button button_ChooseFiles;
        public System.Windows.Forms.ColumnHeader columnHeader_Store;
        public System.Windows.Forms.ColumnHeader columnHeader_File;
        private System.Windows.Forms.GroupBox groupBox_Select;
        private System.Windows.Forms.GroupBox groupBox_ImportQueue;
        private System.Windows.Forms.GroupBox groupBox_Log;
        private System.Windows.Forms.Button button_Import;
        private System.Windows.Forms.SplitContainer splitContainer_Outer;
        private System.Windows.Forms.SplitContainer splitContainer_TopInner;
        private System.Windows.Forms.Button button_ClearQueue;
        private System.Windows.Forms.ComboBox comboBox_Format;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dateTimePicker_ScanDate;
        private System.Windows.Forms.CheckBox checkBox_Validation;
        private System.Windows.Forms.ColumnHeader columnHeader_Format;
        private System.Windows.Forms.ColumnHeader columnHeader_Date;
        private System.Windows.Forms.ColumnHeader columnHeader_Guid;
    }
}

