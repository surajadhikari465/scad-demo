namespace OOSCollector
{
    partial class OOSCollectorUI
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
            this.button_Start = new System.Windows.Forms.Button();
            this.textBox_Uploaded = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_Imported = new System.Windows.Forms.TextBox();
            this.textBox_Log = new System.Windows.Forms.TextBox();
            this.splitContainer_Outer = new System.Windows.Forms.SplitContainer();
            this.checkBox_Known = new System.Windows.Forms.CheckBox();
            this.checkBox_Reported = new System.Windows.Forms.CheckBox();
            this.checkBox_Autorun = new System.Windows.Forms.CheckBox();
            this.checkBox_Validate = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_Outer)).BeginInit();
            this.splitContainer_Outer.Panel1.SuspendLayout();
            this.splitContainer_Outer.Panel2.SuspendLayout();
            this.splitContainer_Outer.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_Start
            // 
            this.button_Start.Location = new System.Drawing.Point(10, 13);
            this.button_Start.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_Start.Name = "button_Start";
            this.button_Start.Size = new System.Drawing.Size(57, 18);
            this.button_Start.TabIndex = 0;
            this.button_Start.Text = "Start";
            this.button_Start.UseVisualStyleBackColor = true;
            this.button_Start.Click += new System.EventHandler(this.button_Start_Click);
            // 
            // textBox_Uploaded
            // 
            this.textBox_Uploaded.Location = new System.Drawing.Point(134, 13);
            this.textBox_Uploaded.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox_Uploaded.Name = "textBox_Uploaded";
            this.textBox_Uploaded.ReadOnly = true;
            this.textBox_Uploaded.Size = new System.Drawing.Size(355, 20);
            this.textBox_Uploaded.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(77, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Uploaded";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(81, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Imported";
            // 
            // textBox_Imported
            // 
            this.textBox_Imported.Location = new System.Drawing.Point(134, 36);
            this.textBox_Imported.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox_Imported.Name = "textBox_Imported";
            this.textBox_Imported.ReadOnly = true;
            this.textBox_Imported.Size = new System.Drawing.Size(355, 20);
            this.textBox_Imported.TabIndex = 3;
            // 
            // textBox_Log
            // 
            this.textBox_Log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Log.Location = new System.Drawing.Point(0, 0);
            this.textBox_Log.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox_Log.Multiline = true;
            this.textBox_Log.Name = "textBox_Log";
            this.textBox_Log.ReadOnly = true;
            this.textBox_Log.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_Log.Size = new System.Drawing.Size(549, 238);
            this.textBox_Log.TabIndex = 9;
            // 
            // splitContainer_Outer
            // 
            this.splitContainer_Outer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_Outer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer_Outer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_Outer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.splitContainer_Outer.Name = "splitContainer_Outer";
            this.splitContainer_Outer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer_Outer.Panel1
            // 
            this.splitContainer_Outer.Panel1.Controls.Add(this.checkBox_Known);
            this.splitContainer_Outer.Panel1.Controls.Add(this.checkBox_Reported);
            this.splitContainer_Outer.Panel1.Controls.Add(this.checkBox_Autorun);
            this.splitContainer_Outer.Panel1.Controls.Add(this.checkBox_Validate);
            this.splitContainer_Outer.Panel1.Controls.Add(this.button_Start);
            this.splitContainer_Outer.Panel1.Controls.Add(this.textBox_Uploaded);
            this.splitContainer_Outer.Panel1.Controls.Add(this.label1);
            this.splitContainer_Outer.Panel1.Controls.Add(this.textBox_Imported);
            this.splitContainer_Outer.Panel1.Controls.Add(this.label2);
            // 
            // splitContainer_Outer.Panel2
            // 
            this.splitContainer_Outer.Panel2.Controls.Add(this.textBox_Log);
            this.splitContainer_Outer.Size = new System.Drawing.Size(549, 343);
            this.splitContainer_Outer.SplitterDistance = 102;
            this.splitContainer_Outer.SplitterWidth = 3;
            this.splitContainer_Outer.TabIndex = 12;
            // 
            // checkBox_Known
            // 
            this.checkBox_Known.AutoSize = true;
            this.checkBox_Known.Checked = true;
            this.checkBox_Known.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_Known.Location = new System.Drawing.Point(334, 58);
            this.checkBox_Known.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBox_Known.Name = "checkBox_Known";
            this.checkBox_Known.Size = new System.Drawing.Size(59, 17);
            this.checkBox_Known.TabIndex = 15;
            this.checkBox_Known.Text = "Known";
            this.checkBox_Known.UseVisualStyleBackColor = true;
            // 
            // checkBox_Reported
            // 
            this.checkBox_Reported.AutoSize = true;
            this.checkBox_Reported.Checked = true;
            this.checkBox_Reported.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_Reported.Location = new System.Drawing.Point(263, 58);
            this.checkBox_Reported.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBox_Reported.Name = "checkBox_Reported";
            this.checkBox_Reported.Size = new System.Drawing.Size(70, 17);
            this.checkBox_Reported.TabIndex = 14;
            this.checkBox_Reported.Text = "Reported";
            this.checkBox_Reported.UseVisualStyleBackColor = true;
            // 
            // checkBox_Autorun
            // 
            this.checkBox_Autorun.AutoSize = true;
            this.checkBox_Autorun.Enabled = false;
            this.checkBox_Autorun.Location = new System.Drawing.Point(199, 58);
            this.checkBox_Autorun.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBox_Autorun.Name = "checkBox_Autorun";
            this.checkBox_Autorun.Size = new System.Drawing.Size(63, 17);
            this.checkBox_Autorun.TabIndex = 13;
            this.checkBox_Autorun.Text = "Autorun";
            this.checkBox_Autorun.UseVisualStyleBackColor = true;
            // 
            // checkBox_Validate
            // 
            this.checkBox_Validate.AutoSize = true;
            this.checkBox_Validate.Location = new System.Drawing.Point(134, 58);
            this.checkBox_Validate.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBox_Validate.Name = "checkBox_Validate";
            this.checkBox_Validate.Size = new System.Drawing.Size(64, 17);
            this.checkBox_Validate.TabIndex = 12;
            this.checkBox_Validate.Text = "Validate";
            this.checkBox_Validate.UseVisualStyleBackColor = true;
            // 
            // OOSCollectorUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(549, 343);
            this.Controls.Add(this.splitContainer_Outer);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MinimumSize = new System.Drawing.Size(559, 194);
            this.Name = "OOSCollectorUI";
            this.Text = "OOSCollector";
            this.Shown += new System.EventHandler(this.OOSCollectorUI_Shown);
            this.splitContainer_Outer.Panel1.ResumeLayout(false);
            this.splitContainer_Outer.Panel1.PerformLayout();
            this.splitContainer_Outer.Panel2.ResumeLayout(false);
            this.splitContainer_Outer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_Outer)).EndInit();
            this.splitContainer_Outer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_Start;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.SplitContainer splitContainer_Outer;
        public System.Windows.Forms.TextBox textBox_Uploaded;
        public System.Windows.Forms.TextBox textBox_Imported;
        public System.Windows.Forms.TextBox textBox_Log;
        public System.Windows.Forms.CheckBox checkBox_Autorun;
        public System.Windows.Forms.CheckBox checkBox_Validate;
        private System.Windows.Forms.CheckBox checkBox_Known;
        private System.Windows.Forms.CheckBox checkBox_Reported;
    }
}

