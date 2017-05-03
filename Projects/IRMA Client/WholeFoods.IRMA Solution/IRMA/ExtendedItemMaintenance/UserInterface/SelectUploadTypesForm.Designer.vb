<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SelectUploadTypesForm
    Inherits System.Windows.Forms.Form

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
        Me.ButtonCancel = New System.Windows.Forms.Button
        Me.ButtonOK = New System.Windows.Forms.Button
        Me.CheckBoxItemMaintenance = New System.Windows.Forms.CheckBox
        Me.CheckBoxPriceUpload = New System.Windows.Forms.CheckBox
        Me.CheckBoxCostUpload = New System.Windows.Forms.CheckBox
        Me.GroupBoxUploadTypesAndTemplates = New System.Windows.Forms.GroupBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.ComboBoxSubteam = New System.Windows.Forms.ComboBox
        Me.LabelAttributeTemplateLabel = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.ComboBoxCostUploadTemplates = New System.Windows.Forms.ComboBox
        Me.ComboBoxPriceUploadTemplates = New System.Windows.Forms.ComboBox
        Me.ComboBoxItemMaintenanceTemplates = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.TextBoxSessionName = New System.Windows.Forms.TextBox
        Me.ie = New System.Windows.Forms.GroupBox
        Me.TextBoxNotes = New System.Windows.Forms.TextBox
        Me.CheckBoxNewItemSession = New System.Windows.Forms.CheckBox
        Me.CheckBoxDeleteItemSession = New System.Windows.Forms.CheckBox
        Me.GroupBoxUploadTypesAndTemplates.SuspendLayout()
        Me.ie.SuspendLayout()
        Me.SuspendLayout()
        '
        'ButtonCancel
        '
        Me.ButtonCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.ButtonCancel.Location = New System.Drawing.Point(307, 355)
        Me.ButtonCancel.Name = "ButtonCancel"
        Me.ButtonCancel.Size = New System.Drawing.Size(75, 23)
        Me.ButtonCancel.TabIndex = 1
        Me.ButtonCancel.Text = "Cancel"
        Me.ButtonCancel.UseVisualStyleBackColor = True
        '
        'ButtonOK
        '
        Me.ButtonOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.ButtonOK.Location = New System.Drawing.Point(226, 355)
        Me.ButtonOK.Name = "ButtonOK"
        Me.ButtonOK.Size = New System.Drawing.Size(75, 23)
        Me.ButtonOK.TabIndex = 0
        Me.ButtonOK.Text = "Save"
        Me.ButtonOK.UseVisualStyleBackColor = True
        '
        'CheckBoxItemMaintenance
        '
        Me.CheckBoxItemMaintenance.AutoSize = True
        Me.CheckBoxItemMaintenance.Checked = True
        Me.CheckBoxItemMaintenance.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBoxItemMaintenance.Location = New System.Drawing.Point(29, 83)
        Me.CheckBoxItemMaintenance.Name = "CheckBoxItemMaintenance"
        Me.CheckBoxItemMaintenance.Size = New System.Drawing.Size(111, 17)
        Me.CheckBoxItemMaintenance.TabIndex = 0
        Me.CheckBoxItemMaintenance.Text = "Item Maintenance"
        Me.CheckBoxItemMaintenance.UseVisualStyleBackColor = True
        '
        'CheckBoxPriceUpload
        '
        Me.CheckBoxPriceUpload.AutoSize = True
        Me.CheckBoxPriceUpload.Checked = True
        Me.CheckBoxPriceUpload.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBoxPriceUpload.Location = New System.Drawing.Point(29, 107)
        Me.CheckBoxPriceUpload.Name = "CheckBoxPriceUpload"
        Me.CheckBoxPriceUpload.Size = New System.Drawing.Size(87, 17)
        Me.CheckBoxPriceUpload.TabIndex = 2
        Me.CheckBoxPriceUpload.Text = "Price Upload"
        Me.CheckBoxPriceUpload.UseVisualStyleBackColor = True
        '
        'CheckBoxCostUpload
        '
        Me.CheckBoxCostUpload.AutoSize = True
        Me.CheckBoxCostUpload.Checked = True
        Me.CheckBoxCostUpload.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBoxCostUpload.Location = New System.Drawing.Point(29, 131)
        Me.CheckBoxCostUpload.Name = "CheckBoxCostUpload"
        Me.CheckBoxCostUpload.Size = New System.Drawing.Size(84, 17)
        Me.CheckBoxCostUpload.TabIndex = 4
        Me.CheckBoxCostUpload.Text = "Cost Upload"
        Me.CheckBoxCostUpload.UseVisualStyleBackColor = True
        '
        'GroupBoxUploadTypesAndTemplates
        '
        Me.GroupBoxUploadTypesAndTemplates.Controls.Add(Me.Label3)
        Me.GroupBoxUploadTypesAndTemplates.Controls.Add(Me.ComboBoxSubteam)
        Me.GroupBoxUploadTypesAndTemplates.Controls.Add(Me.LabelAttributeTemplateLabel)
        Me.GroupBoxUploadTypesAndTemplates.Controls.Add(Me.Label1)
        Me.GroupBoxUploadTypesAndTemplates.Controls.Add(Me.ComboBoxCostUploadTemplates)
        Me.GroupBoxUploadTypesAndTemplates.Controls.Add(Me.ComboBoxPriceUploadTemplates)
        Me.GroupBoxUploadTypesAndTemplates.Controls.Add(Me.ComboBoxItemMaintenanceTemplates)
        Me.GroupBoxUploadTypesAndTemplates.Controls.Add(Me.CheckBoxItemMaintenance)
        Me.GroupBoxUploadTypesAndTemplates.Controls.Add(Me.CheckBoxCostUpload)
        Me.GroupBoxUploadTypesAndTemplates.Controls.Add(Me.CheckBoxPriceUpload)
        Me.GroupBoxUploadTypesAndTemplates.Location = New System.Drawing.Point(8, 176)
        Me.GroupBoxUploadTypesAndTemplates.Name = "GroupBoxUploadTypesAndTemplates"
        Me.GroupBoxUploadTypesAndTemplates.Size = New System.Drawing.Size(371, 152)
        Me.GroupBoxUploadTypesAndTemplates.TabIndex = 5
        Me.GroupBoxUploadTypesAndTemplates.TabStop = False
        Me.GroupBoxUploadTypesAndTemplates.Text = "Upload Types and Templates"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(85, 31)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(55, 13)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "Sub-team:"
        '
        'ComboBoxSubteam
        '
        Me.ComboBoxSubteam.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxSubteam.FormattingEnabled = True
        Me.ComboBoxSubteam.Items.AddRange(New Object() {"- All Templates -"})
        Me.ComboBoxSubteam.Location = New System.Drawing.Point(149, 28)
        Me.ComboBoxSubteam.Name = "ComboBoxSubteam"
        Me.ComboBoxSubteam.Size = New System.Drawing.Size(211, 21)
        Me.ComboBoxSubteam.TabIndex = 9
        '
        'LabelAttributeTemplateLabel
        '
        Me.LabelAttributeTemplateLabel.AutoSize = True
        Me.LabelAttributeTemplateLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelAttributeTemplateLabel.Location = New System.Drawing.Point(143, 65)
        Me.LabelAttributeTemplateLabel.Name = "LabelAttributeTemplateLabel"
        Me.LabelAttributeTemplateLabel.Size = New System.Drawing.Size(117, 13)
        Me.LabelAttributeTemplateLabel.TabIndex = 8
        Me.LabelAttributeTemplateLabel.Text = "Attribute Templates"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(26, 65)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(85, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Upload Types"
        '
        'ComboBoxCostUploadTemplates
        '
        Me.ComboBoxCostUploadTemplates.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxCostUploadTemplates.FormattingEnabled = True
        Me.ComboBoxCostUploadTemplates.Items.AddRange(New Object() {"All Attributes"})
        Me.ComboBoxCostUploadTemplates.Location = New System.Drawing.Point(146, 129)
        Me.ComboBoxCostUploadTemplates.Name = "ComboBoxCostUploadTemplates"
        Me.ComboBoxCostUploadTemplates.Size = New System.Drawing.Size(214, 21)
        Me.ComboBoxCostUploadTemplates.TabIndex = 5
        '
        'ComboBoxPriceUploadTemplates
        '
        Me.ComboBoxPriceUploadTemplates.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxPriceUploadTemplates.FormattingEnabled = True
        Me.ComboBoxPriceUploadTemplates.Items.AddRange(New Object() {"All Attributes"})
        Me.ComboBoxPriceUploadTemplates.Location = New System.Drawing.Point(146, 105)
        Me.ComboBoxPriceUploadTemplates.Name = "ComboBoxPriceUploadTemplates"
        Me.ComboBoxPriceUploadTemplates.Size = New System.Drawing.Size(214, 21)
        Me.ComboBoxPriceUploadTemplates.TabIndex = 3
        '
        'ComboBoxItemMaintenanceTemplates
        '
        Me.ComboBoxItemMaintenanceTemplates.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxItemMaintenanceTemplates.FormattingEnabled = True
        Me.ComboBoxItemMaintenanceTemplates.Items.AddRange(New Object() {"All Attributes"})
        Me.ComboBoxItemMaintenanceTemplates.Location = New System.Drawing.Point(146, 81)
        Me.ComboBoxItemMaintenanceTemplates.Name = "ComboBoxItemMaintenanceTemplates"
        Me.ComboBoxItemMaintenanceTemplates.Size = New System.Drawing.Size(214, 21)
        Me.ComboBoxItemMaintenanceTemplates.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(12, 119)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(91, 13)
        Me.Label2.TabIndex = 9
        Me.Label2.Text = "Session Name:"
        '
        'TextBoxSessionName
        '
        Me.TextBoxSessionName.Location = New System.Drawing.Point(109, 116)
        Me.TextBoxSessionName.MaxLength = 100
        Me.TextBoxSessionName.Name = "TextBoxSessionName"
        Me.TextBoxSessionName.Size = New System.Drawing.Size(259, 20)
        Me.TextBoxSessionName.TabIndex = 0
        '
        'ie
        '
        Me.ie.Controls.Add(Me.TextBoxNotes)
        Me.ie.Location = New System.Drawing.Point(12, 4)
        Me.ie.Name = "ie"
        Me.ie.Size = New System.Drawing.Size(371, 106)
        Me.ie.TabIndex = 10
        Me.ie.TabStop = False
        Me.ie.Text = "Notes"
        '
        'TextBoxNotes
        '
        Me.TextBoxNotes.BackColor = System.Drawing.SystemColors.Control
        Me.TextBoxNotes.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBoxNotes.Location = New System.Drawing.Point(28, 19)
        Me.TextBoxNotes.Multiline = True
        Me.TextBoxNotes.Name = "TextBoxNotes"
        Me.TextBoxNotes.ReadOnly = True
        Me.TextBoxNotes.Size = New System.Drawing.Size(328, 81)
        Me.TextBoxNotes.TabIndex = 0
        '
        'CheckBoxNewItemSession
        '
        Me.CheckBoxNewItemSession.AutoSize = True
        Me.CheckBoxNewItemSession.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBoxNewItemSession.Location = New System.Drawing.Point(24, 145)
        Me.CheckBoxNewItemSession.Name = "CheckBoxNewItemSession"
        Me.CheckBoxNewItemSession.Size = New System.Drawing.Size(127, 17)
        Me.CheckBoxNewItemSession.TabIndex = 11
        Me.CheckBoxNewItemSession.Text = "New Item Session"
        Me.CheckBoxNewItemSession.UseVisualStyleBackColor = True
        '
        'CheckBoxDeleteItemSession
        '
        Me.CheckBoxDeleteItemSession.AutoSize = True
        Me.CheckBoxDeleteItemSession.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBoxDeleteItemSession.Location = New System.Drawing.Point(216, 144)
        Me.CheckBoxDeleteItemSession.Name = "CheckBoxDeleteItemSession"
        Me.CheckBoxDeleteItemSession.Size = New System.Drawing.Size(139, 17)
        Me.CheckBoxDeleteItemSession.TabIndex = 12
        Me.CheckBoxDeleteItemSession.Text = "Delete Item Session"
        Me.CheckBoxDeleteItemSession.UseVisualStyleBackColor = True
        '
        'SelectUploadTypesForm
        '
        Me.AcceptButton = Me.ButtonOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.ButtonCancel
        Me.ClientSize = New System.Drawing.Size(394, 385)
        Me.ControlBox = False
        Me.Controls.Add(Me.CheckBoxDeleteItemSession)
        Me.Controls.Add(Me.CheckBoxNewItemSession)
        Me.Controls.Add(Me.ie)
        Me.Controls.Add(Me.TextBoxSessionName)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.GroupBoxUploadTypesAndTemplates)
        Me.Controls.Add(Me.ButtonOK)
        Me.Controls.Add(Me.ButtonCancel)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "SelectUploadTypesForm"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Save your Session"
        Me.GroupBoxUploadTypesAndTemplates.ResumeLayout(False)
        Me.GroupBoxUploadTypesAndTemplates.PerformLayout()
        Me.ie.ResumeLayout(False)
        Me.ie.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ButtonCancel As System.Windows.Forms.Button
    Friend WithEvents ButtonOK As System.Windows.Forms.Button
    Friend WithEvents CheckBoxItemMaintenance As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxPriceUpload As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxCostUpload As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBoxUploadTypesAndTemplates As System.Windows.Forms.GroupBox
    Friend WithEvents ComboBoxCostUploadTemplates As System.Windows.Forms.ComboBox
    Friend WithEvents ComboBoxPriceUploadTemplates As System.Windows.Forms.ComboBox
    Friend WithEvents ComboBoxItemMaintenanceTemplates As System.Windows.Forms.ComboBox
    Friend WithEvents LabelAttributeTemplateLabel As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TextBoxSessionName As System.Windows.Forms.TextBox
    Friend WithEvents ie As System.Windows.Forms.GroupBox
    Friend WithEvents TextBoxNotes As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents ComboBoxSubteam As System.Windows.Forms.ComboBox
    Friend WithEvents CheckBoxNewItemSession As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxDeleteItemSession As System.Windows.Forms.CheckBox
End Class
