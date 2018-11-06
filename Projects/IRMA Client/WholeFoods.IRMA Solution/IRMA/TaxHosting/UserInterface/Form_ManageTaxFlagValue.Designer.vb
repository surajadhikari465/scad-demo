<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_ManageTaxFlagValue
    Inherits Form_IRMABase

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.TextBox_TaxFlagKey = New System.Windows.Forms.TextBox()
        Me.Label_TaxFlagKey = New System.Windows.Forms.Label()
        Me.RadioButton_TaxFlagValueYes = New System.Windows.Forms.RadioButton()
        Me.RadioButton_TaxFlagValueNo = New System.Windows.Forms.RadioButton()
        Me.GroupBox_TaxFlagValue = New System.Windows.Forms.GroupBox()
        Me.TextBox_TaxPercent = New System.Windows.Forms.TextBox()
        Me.Label_TaxPercent = New System.Windows.Forms.Label()
        Me.TextBox_POSID = New System.Windows.Forms.TextBox()
        Me.Label_POSID = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.lblGroupItemCode = New System.Windows.Forms.Label()
        Me.txtExternalTaxGroupCode = New System.Windows.Forms.TextBox()
        Me.Button_OK = New System.Windows.Forms.Button()
        Me.Button_Cancel = New System.Windows.Forms.Button()
        Me.GroupBox_TaxFlagValue.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TextBox_TaxFlagKey
        '
        Me.TextBox_TaxFlagKey.Location = New System.Drawing.Point(27, 108)
        Me.TextBox_TaxFlagKey.Name = "TextBox_TaxFlagKey"
        Me.TextBox_TaxFlagKey.Size = New System.Drawing.Size(29, 20)
        Me.TextBox_TaxFlagKey.TabIndex = 2
        '
        'Label_TaxFlagKey
        '
        Me.Label_TaxFlagKey.AutoSize = True
        Me.Label_TaxFlagKey.Location = New System.Drawing.Point(24, 92)
        Me.Label_TaxFlagKey.Name = "Label_TaxFlagKey"
        Me.Label_TaxFlagKey.Size = New System.Drawing.Size(48, 13)
        Me.Label_TaxFlagKey.TabIndex = 1
        Me.Label_TaxFlagKey.Text = "Tax Flag"
        '
        'RadioButton_TaxFlagValueYes
        '
        Me.RadioButton_TaxFlagValueYes.AutoSize = True
        Me.RadioButton_TaxFlagValueYes.Location = New System.Drawing.Point(16, 19)
        Me.RadioButton_TaxFlagValueYes.Name = "RadioButton_TaxFlagValueYes"
        Me.RadioButton_TaxFlagValueYes.Size = New System.Drawing.Size(43, 17)
        Me.RadioButton_TaxFlagValueYes.TabIndex = 0
        Me.RadioButton_TaxFlagValueYes.Text = "Yes"
        Me.RadioButton_TaxFlagValueYes.UseVisualStyleBackColor = True
        '
        'RadioButton_TaxFlagValueNo
        '
        Me.RadioButton_TaxFlagValueNo.AutoSize = True
        Me.RadioButton_TaxFlagValueNo.Checked = True
        Me.RadioButton_TaxFlagValueNo.Location = New System.Drawing.Point(65, 19)
        Me.RadioButton_TaxFlagValueNo.Name = "RadioButton_TaxFlagValueNo"
        Me.RadioButton_TaxFlagValueNo.Size = New System.Drawing.Size(39, 17)
        Me.RadioButton_TaxFlagValueNo.TabIndex = 1
        Me.RadioButton_TaxFlagValueNo.TabStop = True
        Me.RadioButton_TaxFlagValueNo.Text = "No"
        Me.RadioButton_TaxFlagValueNo.UseVisualStyleBackColor = True
        '
        'GroupBox_TaxFlagValue
        '
        Me.GroupBox_TaxFlagValue.Controls.Add(Me.RadioButton_TaxFlagValueNo)
        Me.GroupBox_TaxFlagValue.Controls.Add(Me.RadioButton_TaxFlagValueYes)
        Me.GroupBox_TaxFlagValue.Location = New System.Drawing.Point(27, 24)
        Me.GroupBox_TaxFlagValue.Name = "GroupBox_TaxFlagValue"
        Me.GroupBox_TaxFlagValue.Size = New System.Drawing.Size(119, 48)
        Me.GroupBox_TaxFlagValue.TabIndex = 1
        Me.GroupBox_TaxFlagValue.TabStop = False
        Me.GroupBox_TaxFlagValue.Text = "Active?"
        '
        'TextBox_TaxPercent
        '
        Me.TextBox_TaxPercent.Location = New System.Drawing.Point(27, 147)
        Me.TextBox_TaxPercent.MaxLength = 30
        Me.TextBox_TaxPercent.Name = "TextBox_TaxPercent"
        Me.TextBox_TaxPercent.Size = New System.Drawing.Size(100, 20)
        Me.TextBox_TaxPercent.TabIndex = 3
        '
        'Label_TaxPercent
        '
        Me.Label_TaxPercent.AutoSize = True
        Me.Label_TaxPercent.Location = New System.Drawing.Point(24, 131)
        Me.Label_TaxPercent.Name = "Label_TaxPercent"
        Me.Label_TaxPercent.Size = New System.Drawing.Size(62, 13)
        Me.Label_TaxPercent.TabIndex = 7
        Me.Label_TaxPercent.Text = "Percentage"
        '
        'TextBox_POSID
        '
        Me.TextBox_POSID.Location = New System.Drawing.Point(27, 186)
        Me.TextBox_POSID.MaxLength = 4
        Me.TextBox_POSID.Name = "TextBox_POSID"
        Me.TextBox_POSID.Size = New System.Drawing.Size(100, 20)
        Me.TextBox_POSID.TabIndex = 4
        '
        'Label_POSID
        '
        Me.Label_POSID.AutoSize = True
        Me.Label_POSID.Location = New System.Drawing.Point(24, 170)
        Me.Label_POSID.Name = "Label_POSID"
        Me.Label_POSID.Size = New System.Drawing.Size(57, 13)
        Me.Label_POSID.TabIndex = 9
        Me.Label_POSID.Text = "POS Code"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.lblGroupItemCode)
        Me.GroupBox1.Controls.Add(Me.txtExternalTaxGroupCode)
        Me.GroupBox1.Controls.Add(Me.Label_POSID)
        Me.GroupBox1.Controls.Add(Me.TextBox_TaxPercent)
        Me.GroupBox1.Controls.Add(Me.TextBox_POSID)
        Me.GroupBox1.Controls.Add(Me.GroupBox_TaxFlagValue)
        Me.GroupBox1.Controls.Add(Me.Label_TaxPercent)
        Me.GroupBox1.Controls.Add(Me.TextBox_TaxFlagKey)
        Me.GroupBox1.Controls.Add(Me.Label_TaxFlagKey)
        Me.GroupBox1.Location = New System.Drawing.Point(21, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(171, 259)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        '
        'lblGroupItemCode
        '
        Me.lblGroupItemCode.AutoSize = True
        Me.lblGroupItemCode.Location = New System.Drawing.Point(24, 209)
        Me.lblGroupItemCode.Name = "lblGroupItemCode"
        Me.lblGroupItemCode.Size = New System.Drawing.Size(112, 13)
        Me.lblGroupItemCode.TabIndex = 11
        Me.lblGroupItemCode.Text = "CCH Group Item Code"
        '
        'txtExternalTaxGroupCode
        '
        Me.txtExternalTaxGroupCode.Enabled = False
        Me.txtExternalTaxGroupCode.Location = New System.Drawing.Point(27, 225)
        Me.txtExternalTaxGroupCode.MaxLength = 7
        Me.txtExternalTaxGroupCode.Name = "txtExternalTaxGroupCode"
        Me.txtExternalTaxGroupCode.Size = New System.Drawing.Size(100, 20)
        Me.txtExternalTaxGroupCode.TabIndex = 5
        '
        'Button_OK
        '
        Me.Button_OK.Location = New System.Drawing.Point(117, 277)
        Me.Button_OK.Name = "Button_OK"
        Me.Button_OK.Size = New System.Drawing.Size(75, 23)
        Me.Button_OK.TabIndex = 7
        Me.Button_OK.Text = "OK"
        Me.Button_OK.UseVisualStyleBackColor = True
        '
        'Button_Cancel
        '
        Me.Button_Cancel.Location = New System.Drawing.Point(21, 277)
        Me.Button_Cancel.Name = "Button_Cancel"
        Me.Button_Cancel.Size = New System.Drawing.Size(75, 23)
        Me.Button_Cancel.TabIndex = 6
        Me.Button_Cancel.Text = "Cancel"
        Me.Button_Cancel.UseVisualStyleBackColor = True
        '
        'Form_ManageTaxFlagValue
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(211, 307)
        Me.Controls.Add(Me.Button_Cancel)
        Me.Controls.Add(Me.Button_OK)
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "Form_ManageTaxFlagValue"
        Me.ShowInTaskbar = False
        Me.Text = "Edit Tax Flag"
        Me.GroupBox_TaxFlagValue.ResumeLayout(False)
        Me.GroupBox_TaxFlagValue.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TextBox_TaxFlagKey As System.Windows.Forms.TextBox
    Friend WithEvents Label_TaxFlagKey As System.Windows.Forms.Label
    Friend WithEvents RadioButton_TaxFlagValueYes As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton_TaxFlagValueNo As System.Windows.Forms.RadioButton
    Friend WithEvents GroupBox_TaxFlagValue As System.Windows.Forms.GroupBox
    Friend WithEvents TextBox_TaxPercent As System.Windows.Forms.TextBox
    Friend WithEvents Label_TaxPercent As System.Windows.Forms.Label
    Friend WithEvents TextBox_POSID As System.Windows.Forms.TextBox
    Friend WithEvents Label_POSID As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Button_OK As System.Windows.Forms.Button
    Friend WithEvents Button_Cancel As System.Windows.Forms.Button
    Friend WithEvents lblGroupItemCode As System.Windows.Forms.Label
    Friend WithEvents txtExternalTaxGroupCode As System.Windows.Forms.TextBox
End Class
