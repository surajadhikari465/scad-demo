<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_ManageTaxOverrideValue
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
        Me.Button_Cancel = New System.Windows.Forms.Button
        Me.Button_OK = New System.Windows.Forms.Button
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.ComboBox_TaxFlag = New System.Windows.Forms.ComboBox
        Me.GroupBox_TaxFlagValue = New System.Windows.Forms.GroupBox
        Me.RadioButton_TaxFlagValueNo = New System.Windows.Forms.RadioButton
        Me.RadioButton_TaxFlagValueYes = New System.Windows.Forms.RadioButton
        Me.Label_TaxFlagKey = New System.Windows.Forms.Label
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox_TaxFlagValue.SuspendLayout()
        Me.SuspendLayout()
        '
        'Button_Cancel
        '
        Me.Button_Cancel.Location = New System.Drawing.Point(29, 166)
        Me.Button_Cancel.Name = "Button_Cancel"
        Me.Button_Cancel.Size = New System.Drawing.Size(75, 23)
        Me.Button_Cancel.TabIndex = 4
        Me.Button_Cancel.Text = "Cancel"
        Me.Button_Cancel.UseVisualStyleBackColor = True
        '
        'Button_OK
        '
        Me.Button_OK.Location = New System.Drawing.Point(119, 166)
        Me.Button_OK.Name = "Button_OK"
        Me.Button_OK.Size = New System.Drawing.Size(75, 23)
        Me.Button_OK.TabIndex = 3
        Me.Button_OK.Text = "OK"
        Me.Button_OK.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.ComboBox_TaxFlag)
        Me.GroupBox1.Controls.Add(Me.GroupBox_TaxFlagValue)
        Me.GroupBox1.Controls.Add(Me.Label_TaxFlagKey)
        Me.GroupBox1.Location = New System.Drawing.Point(23, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(171, 148)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        '
        'ComboBox_TaxFlag
        '
        Me.ComboBox_TaxFlag.FormattingEnabled = True
        Me.ComboBox_TaxFlag.Location = New System.Drawing.Point(27, 103)
        Me.ComboBox_TaxFlag.Name = "ComboBox_TaxFlag"
        Me.ComboBox_TaxFlag.Size = New System.Drawing.Size(64, 21)
        Me.ComboBox_TaxFlag.TabIndex = 3
        Me.ComboBox_TaxFlag.TabStop = False
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
        'Label_TaxFlagKey
        '
        Me.Label_TaxFlagKey.AutoSize = True
        Me.Label_TaxFlagKey.Location = New System.Drawing.Point(24, 87)
        Me.Label_TaxFlagKey.Name = "Label_TaxFlagKey"
        Me.Label_TaxFlagKey.Size = New System.Drawing.Size(48, 13)
        Me.Label_TaxFlagKey.TabIndex = 1
        Me.Label_TaxFlagKey.Text = "Tax Flag"
        '
        'Form_ManageTaxOverrideValue
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(220, 200)
        Me.Controls.Add(Me.Button_Cancel)
        Me.Controls.Add(Me.Button_OK)
        Me.Controls.Add(Me.GroupBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Form_ManageTaxOverrideValue"
        Me.ShowInTaskbar = False
        Me.Text = "Manage Tax Override"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox_TaxFlagValue.ResumeLayout(False)
        Me.GroupBox_TaxFlagValue.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Button_Cancel As System.Windows.Forms.Button
    Friend WithEvents Button_OK As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox_TaxFlagValue As System.Windows.Forms.GroupBox
    Friend WithEvents RadioButton_TaxFlagValueNo As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton_TaxFlagValueYes As System.Windows.Forms.RadioButton
    Friend WithEvents Label_TaxFlagKey As System.Windows.Forms.Label
    Friend WithEvents ComboBox_TaxFlag As System.Windows.Forms.ComboBox
End Class
