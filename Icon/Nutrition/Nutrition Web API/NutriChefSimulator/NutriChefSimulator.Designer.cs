namespace NutriChefSimulator
{
    partial class NutriChefSimulator
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
            this.txtBoxPlu = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtBoxRecipeName = new System.Windows.Forms.TextBox();
            this.txtBoxHshRating = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtBoxServingsPerPortion = new System.Windows.Forms.TextBox();
            this.btnSendToICon = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "PLU";
            // 
            // txtBoxPlu
            // 
            this.txtBoxPlu.Location = new System.Drawing.Point(131, 25);
            this.txtBoxPlu.Name = "txtBoxPlu";
            this.txtBoxPlu.Size = new System.Drawing.Size(162, 20);
            this.txtBoxPlu.TabIndex = 1;
            this.txtBoxPlu.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBoxPlu_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Recipe Name";
            // 
            // txtBoxRecipeName
            // 
            this.txtBoxRecipeName.Location = new System.Drawing.Point(131, 61);
            this.txtBoxRecipeName.Name = "txtBoxRecipeName";
            this.txtBoxRecipeName.Size = new System.Drawing.Size(162, 20);
            this.txtBoxRecipeName.TabIndex = 3;
            // 
            // txtBoxHshRating
            // 
            this.txtBoxHshRating.Location = new System.Drawing.Point(131, 100);
            this.txtBoxHshRating.Name = "txtBoxHshRating";
            this.txtBoxHshRating.Size = new System.Drawing.Size(162, 20);
            this.txtBoxHshRating.TabIndex = 4;
            this.txtBoxHshRating.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBoxHshRating_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 103);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Hsh Rating";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(26, 139);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(103, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Servings Per Portion";
            // 
            // txtBoxServingsPerPortion
            // 
            this.txtBoxServingsPerPortion.Location = new System.Drawing.Point(131, 139);
            this.txtBoxServingsPerPortion.Name = "txtBoxServingsPerPortion";
            this.txtBoxServingsPerPortion.Size = new System.Drawing.Size(162, 20);
            this.txtBoxServingsPerPortion.TabIndex = 7;
            this.txtBoxServingsPerPortion.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBoxServingsPerPortion_KeyPress);
            // 
            // btnSendToICon
            // 
            this.btnSendToICon.Location = new System.Drawing.Point(131, 205);
            this.btnSendToICon.Name = "btnSendToICon";
            this.btnSendToICon.Size = new System.Drawing.Size(75, 23);
            this.btnSendToICon.TabIndex = 8;
            this.btnSendToICon.Text = "Send To Icon";
            this.btnSendToICon.UseVisualStyleBackColor = true;
            this.btnSendToICon.Click += new System.EventHandler(this.btnSendToICon_Click);
            // 
            // NutriChefSimulator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(573, 459);
            this.Controls.Add(this.btnSendToICon);
            this.Controls.Add(this.txtBoxServingsPerPortion);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtBoxHshRating);
            this.Controls.Add(this.txtBoxRecipeName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtBoxPlu);
            this.Controls.Add(this.label1);
            this.Name = "NutriChefSimulator";
            this.Text = "NutriChefSimulator";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtBoxPlu;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtBoxRecipeName;
        private System.Windows.Forms.TextBox txtBoxHshRating;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtBoxServingsPerPortion;
        private System.Windows.Forms.Button btnSendToICon;
    }
}

