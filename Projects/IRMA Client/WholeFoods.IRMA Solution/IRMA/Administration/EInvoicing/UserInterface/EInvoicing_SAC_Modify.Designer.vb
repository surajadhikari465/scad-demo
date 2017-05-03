<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class EInvoicing_SAC_Modify
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.TextBox_ElementName = New System.Windows.Forms.TextBox
        Me.TextBox_Label = New System.Windows.Forms.TextBox
        Me.rdoAllocated = New System.Windows.Forms.RadioButton
        Me.rdoNotAllocated = New System.Windows.Forms.RadioButton
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Button_Save = New System.Windows.Forms.Button
        Me.Button_Cancel = New System.Windows.Forms.Button
        Me.Label3 = New System.Windows.Forms.Label
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.rdoLineItem = New System.Windows.Forms.RadioButton
        Me.ComboBox_SubTeam = New System.Windows.Forms.ComboBox
        Me.chkAllowance = New System.Windows.Forms.CheckBox
        Me.rdoHeaderElement = New System.Windows.Forms.RadioButton
        Me.rdoItemElement = New System.Windows.Forms.RadioButton
        Me.chkSACCode = New System.Windows.Forms.CheckBox
        Me.rdoSummaryElement = New System.Windows.Forms.RadioButton
        Me.CheckBox_Disabled = New System.Windows.Forms.CheckBox
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TextBox_ElementName
        '
        Me.TextBox_ElementName.Location = New System.Drawing.Point(12, 45)
        Me.TextBox_ElementName.Name = "TextBox_ElementName"
        Me.TextBox_ElementName.Size = New System.Drawing.Size(156, 20)
        Me.TextBox_ElementName.TabIndex = 0
        '
        'TextBox_Label
        '
        Me.TextBox_Label.Location = New System.Drawing.Point(15, 130)
        Me.TextBox_Label.MaxLength = 30
        Me.TextBox_Label.Name = "TextBox_Label"
        Me.TextBox_Label.Size = New System.Drawing.Size(376, 20)
        Me.TextBox_Label.TabIndex = 2
        '
        'rdoAllocated
        '
        Me.rdoAllocated.AutoSize = True
        Me.rdoAllocated.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rdoAllocated.Location = New System.Drawing.Point(6, 19)
        Me.rdoAllocated.Name = "rdoAllocated"
        Me.rdoAllocated.Size = New System.Drawing.Size(78, 17)
        Me.rdoAllocated.TabIndex = 0
        Me.rdoAllocated.Text = "Allocated"
        Me.rdoAllocated.UseVisualStyleBackColor = True
        '
        'rdoNotAllocated
        '
        Me.rdoNotAllocated.AutoSize = True
        Me.rdoNotAllocated.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rdoNotAllocated.Location = New System.Drawing.Point(6, 39)
        Me.rdoNotAllocated.Name = "rdoNotAllocated"
        Me.rdoNotAllocated.Size = New System.Drawing.Size(102, 17)
        Me.rdoNotAllocated.TabIndex = 1
        Me.rdoNotAllocated.Text = "Not Allocated"
        Me.rdoNotAllocated.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(9, 27)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(88, 13)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Element Name"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(12, 114)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(38, 13)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "Label"
        '
        'Button_Save
        '
        Me.Button_Save.Location = New System.Drawing.Point(243, 189)
        Me.Button_Save.Name = "Button_Save"
        Me.Button_Save.Size = New System.Drawing.Size(71, 25)
        Me.Button_Save.TabIndex = 8
        Me.Button_Save.Text = "&Save"
        Me.Button_Save.UseVisualStyleBackColor = True
        '
        'Button_Cancel
        '
        Me.Button_Cancel.Location = New System.Drawing.Point(320, 189)
        Me.Button_Cancel.Name = "Button_Cancel"
        Me.Button_Cancel.Size = New System.Drawing.Size(71, 25)
        Me.Button_Cancel.TabIndex = 9
        Me.Button_Cancel.Text = "&Cancel"
        Me.Button_Cancel.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(12, 68)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(56, 13)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "Subteam"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.rdoLineItem)
        Me.GroupBox1.Controls.Add(Me.rdoNotAllocated)
        Me.GroupBox1.Controls.Add(Me.rdoAllocated)
        Me.GroupBox1.Location = New System.Drawing.Point(271, 23)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(120, 82)
        Me.GroupBox1.TabIndex = 5
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Type:"
        '
        'rdoLineItem
        '
        Me.rdoLineItem.AutoSize = True
        Me.rdoLineItem.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rdoLineItem.Location = New System.Drawing.Point(6, 59)
        Me.rdoLineItem.Name = "rdoLineItem"
        Me.rdoLineItem.Size = New System.Drawing.Size(77, 17)
        Me.rdoLineItem.TabIndex = 2
        Me.rdoLineItem.Text = "Line Item"
        Me.rdoLineItem.UseVisualStyleBackColor = True
        '
        'ComboBox_SubTeam
        '
        Me.ComboBox_SubTeam.FormattingEnabled = True
        Me.ComboBox_SubTeam.Location = New System.Drawing.Point(12, 84)
        Me.ComboBox_SubTeam.Name = "ComboBox_SubTeam"
        Me.ComboBox_SubTeam.Size = New System.Drawing.Size(247, 21)
        Me.ComboBox_SubTeam.TabIndex = 1
        '
        'chkAllowance
        '
        Me.chkAllowance.AutoSize = True
        Me.chkAllowance.Location = New System.Drawing.Point(184, 61)
        Me.chkAllowance.Name = "chkAllowance"
        Me.chkAllowance.Size = New System.Drawing.Size(75, 17)
        Me.chkAllowance.TabIndex = 4
        Me.chkAllowance.Text = "Allowance"
        Me.chkAllowance.UseVisualStyleBackColor = True
        '
        'rdoHeaderElement
        '
        Me.rdoHeaderElement.AutoSize = True
        Me.rdoHeaderElement.Location = New System.Drawing.Point(15, 167)
        Me.rdoHeaderElement.Name = "rdoHeaderElement"
        Me.rdoHeaderElement.Size = New System.Drawing.Size(101, 17)
        Me.rdoHeaderElement.TabIndex = 6
        Me.rdoHeaderElement.TabStop = True
        Me.rdoHeaderElement.Text = "Header Element"
        Me.rdoHeaderElement.UseVisualStyleBackColor = True
        '
        'rdoItemElement
        '
        Me.rdoItemElement.AutoSize = True
        Me.rdoItemElement.Location = New System.Drawing.Point(122, 167)
        Me.rdoItemElement.Name = "rdoItemElement"
        Me.rdoItemElement.Size = New System.Drawing.Size(86, 17)
        Me.rdoItemElement.TabIndex = 7
        Me.rdoItemElement.TabStop = True
        Me.rdoItemElement.Text = "Item Element"
        Me.rdoItemElement.UseVisualStyleBackColor = True
        '
        'chkSACCode
        '
        Me.chkSACCode.AutoSize = True
        Me.chkSACCode.Location = New System.Drawing.Point(184, 42)
        Me.chkSACCode.Name = "chkSACCode"
        Me.chkSACCode.Size = New System.Drawing.Size(75, 17)
        Me.chkSACCode.TabIndex = 3
        Me.chkSACCode.Text = "SAC Code"
        Me.chkSACCode.UseVisualStyleBackColor = True
        '
        'rdoSummaryElement
        '
        Me.rdoSummaryElement.AutoSize = True
        Me.rdoSummaryElement.Location = New System.Drawing.Point(214, 166)
        Me.rdoSummaryElement.Name = "rdoSummaryElement"
        Me.rdoSummaryElement.Size = New System.Drawing.Size(109, 17)
        Me.rdoSummaryElement.TabIndex = 10
        Me.rdoSummaryElement.TabStop = True
        Me.rdoSummaryElement.Text = "Summary Element"
        Me.rdoSummaryElement.UseVisualStyleBackColor = True
        '
        'CheckBox_Disabled
        '
        Me.CheckBox_Disabled.AutoSize = True
        Me.CheckBox_Disabled.Location = New System.Drawing.Point(15, 189)
        Me.CheckBox_Disabled.Name = "CheckBox_Disabled"
        Me.CheckBox_Disabled.Size = New System.Drawing.Size(67, 17)
        Me.CheckBox_Disabled.TabIndex = 11
        Me.CheckBox_Disabled.Text = "&Disabled"
        Me.CheckBox_Disabled.UseVisualStyleBackColor = True
        '
        'EInvoicing_SAC_Modify
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(403, 223)
        Me.Controls.Add(Me.CheckBox_Disabled)
        Me.Controls.Add(Me.rdoSummaryElement)
        Me.Controls.Add(Me.chkSACCode)
        Me.Controls.Add(Me.rdoItemElement)
        Me.Controls.Add(Me.rdoHeaderElement)
        Me.Controls.Add(Me.ComboBox_SubTeam)
        Me.Controls.Add(Me.chkAllowance)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Button_Cancel)
        Me.Controls.Add(Me.Button_Save)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TextBox_Label)
        Me.Controls.Add(Me.TextBox_ElementName)
        Me.Name = "EInvoicing_SAC_Modify"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "E-Invoice Integration - Special Allowances and Charges"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TextBox_ElementName As System.Windows.Forms.TextBox
    Friend WithEvents TextBox_Label As System.Windows.Forms.TextBox
    Friend WithEvents rdoAllocated As System.Windows.Forms.RadioButton
    Friend WithEvents rdoNotAllocated As System.Windows.Forms.RadioButton
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Button_Save As System.Windows.Forms.Button
    Friend WithEvents Button_Cancel As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents rdoLineItem As System.Windows.Forms.RadioButton
    Friend WithEvents ComboBox_SubTeam As System.Windows.Forms.ComboBox
    Friend WithEvents chkAllowance As System.Windows.Forms.CheckBox
    Friend WithEvents rdoHeaderElement As System.Windows.Forms.RadioButton
    Friend WithEvents rdoItemElement As System.Windows.Forms.RadioButton
    Friend WithEvents chkSACCode As System.Windows.Forms.CheckBox
    Friend WithEvents rdoSummaryElement As System.Windows.Forms.RadioButton
    Friend WithEvents CheckBox_Disabled As System.Windows.Forms.CheckBox
End Class
