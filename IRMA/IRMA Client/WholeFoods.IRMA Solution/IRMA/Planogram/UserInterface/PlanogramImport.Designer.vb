<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PlanogramImport
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
        Me.GroupBoxImport = New System.Windows.Forms.GroupBox()
        Me.LabelImportStatusValue = New System.Windows.Forms.Label()
        Me.LabelImportStatus = New System.Windows.Forms.Label()
        Me.ButtonImport = New System.Windows.Forms.Button()
        Me.ButtonSelectFile = New System.Windows.Forms.Button()
        Me.TextBoxFilePath = New System.Windows.Forms.TextBox()
        Me.OpenFileDialogSelectFile = New System.Windows.Forms.OpenFileDialog()
        Me.DateTimePickerEffectiveDate = New System.Windows.Forms.DateTimePicker()
        Me.LabelEffectiveDate = New System.Windows.Forms.Label()
        Me.ButtonCancel = New System.Windows.Forms.Button()
        Me.ButtonSend = New System.Windows.Forms.Button()
        Me.GroupBoxImport.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBoxImport
        '
        Me.GroupBoxImport.Controls.Add(Me.LabelImportStatusValue)
        Me.GroupBoxImport.Controls.Add(Me.LabelImportStatus)
        Me.GroupBoxImport.Controls.Add(Me.ButtonImport)
        Me.GroupBoxImport.Controls.Add(Me.ButtonSelectFile)
        Me.GroupBoxImport.Controls.Add(Me.TextBoxFilePath)
        Me.GroupBoxImport.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.GroupBoxImport.Location = New System.Drawing.Point(12, 12)
        Me.GroupBoxImport.Name = "GroupBoxImport"
        Me.GroupBoxImport.Size = New System.Drawing.Size(490, 93)
        Me.GroupBoxImport.TabIndex = 10
        Me.GroupBoxImport.TabStop = False
        Me.GroupBoxImport.Text = "Import Planogram File"
        '
        'LabelImportStatusValue
        '
        Me.LabelImportStatusValue.AutoSize = True
        Me.LabelImportStatusValue.Location = New System.Drawing.Point(94, 61)
        Me.LabelImportStatusValue.Name = "LabelImportStatusValue"
        Me.LabelImportStatusValue.Size = New System.Drawing.Size(76, 13)
        Me.LabelImportStatusValue.TabIndex = 11
        Me.LabelImportStatusValue.Text = "Text Text Text"
        '
        'LabelImportStatus
        '
        Me.LabelImportStatus.AutoSize = True
        Me.LabelImportStatus.Location = New System.Drawing.Point(16, 61)
        Me.LabelImportStatus.Name = "LabelImportStatus"
        Me.LabelImportStatus.Size = New System.Drawing.Size(72, 13)
        Me.LabelImportStatus.TabIndex = 10
        Me.LabelImportStatus.Text = "Import Status:"
        '
        'ButtonImport
        '
        Me.ButtonImport.Enabled = False
        Me.ButtonImport.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.ButtonImport.Location = New System.Drawing.Point(401, 26)
        Me.ButtonImport.Name = "ButtonImport"
        Me.ButtonImport.Size = New System.Drawing.Size(70, 23)
        Me.ButtonImport.TabIndex = 9
        Me.ButtonImport.Text = "Import"
        Me.ButtonImport.UseVisualStyleBackColor = True
        '
        'ButtonSelectFile
        '
        Me.ButtonSelectFile.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.ButtonSelectFile.Location = New System.Drawing.Point(325, 26)
        Me.ButtonSelectFile.Name = "ButtonSelectFile"
        Me.ButtonSelectFile.Size = New System.Drawing.Size(70, 23)
        Me.ButtonSelectFile.TabIndex = 1
        Me.ButtonSelectFile.Text = "Select File"
        Me.ButtonSelectFile.UseVisualStyleBackColor = True
        '
        'TextBoxFilePath
        '
        Me.TextBoxFilePath.Location = New System.Drawing.Point(16, 28)
        Me.TextBoxFilePath.Name = "TextBoxFilePath"
        Me.TextBoxFilePath.ReadOnly = True
        Me.TextBoxFilePath.Size = New System.Drawing.Size(303, 20)
        Me.TextBoxFilePath.TabIndex = 0
        '
        'OpenFileDialogSelectFile
        '
        Me.OpenFileDialogSelectFile.Filter = "Planogram Files|*.dat"
        '
        'DateTimePickerEffectiveDate
        '
        Me.DateTimePickerEffectiveDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.DateTimePickerEffectiveDate.Location = New System.Drawing.Point(112, 132)
        Me.DateTimePickerEffectiveDate.Name = "DateTimePickerEffectiveDate"
        Me.DateTimePickerEffectiveDate.Size = New System.Drawing.Size(103, 20)
        Me.DateTimePickerEffectiveDate.TabIndex = 11
        '
        'LabelEffectiveDate
        '
        Me.LabelEffectiveDate.AutoSize = True
        Me.LabelEffectiveDate.Location = New System.Drawing.Point(28, 134)
        Me.LabelEffectiveDate.Name = "LabelEffectiveDate"
        Me.LabelEffectiveDate.Size = New System.Drawing.Size(78, 13)
        Me.LabelEffectiveDate.TabIndex = 12
        Me.LabelEffectiveDate.Text = "Effective Date:"
        '
        'ButtonCancel
        '
        Me.ButtonCancel.Location = New System.Drawing.Point(413, 134)
        Me.ButtonCancel.Name = "ButtonCancel"
        Me.ButtonCancel.Size = New System.Drawing.Size(70, 23)
        Me.ButtonCancel.TabIndex = 13
        Me.ButtonCancel.Text = "Cancel"
        Me.ButtonCancel.UseVisualStyleBackColor = True
        '
        'ButtonSend
        '
        Me.ButtonSend.Enabled = False
        Me.ButtonSend.Location = New System.Drawing.Point(337, 134)
        Me.ButtonSend.Name = "ButtonSend"
        Me.ButtonSend.Size = New System.Drawing.Size(70, 23)
        Me.ButtonSend.TabIndex = 14
        Me.ButtonSend.Text = "Send"
        Me.ButtonSend.UseVisualStyleBackColor = True
        '
        'PlanogramImport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(515, 169)
        Me.Controls.Add(Me.ButtonSend)
        Me.Controls.Add(Me.ButtonCancel)
        Me.Controls.Add(Me.LabelEffectiveDate)
        Me.Controls.Add(Me.DateTimePickerEffectiveDate)
        Me.Controls.Add(Me.GroupBoxImport)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "PlanogramImport"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.Text = "Planogram Import"
        Me.GroupBoxImport.ResumeLayout(False)
        Me.GroupBoxImport.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents GroupBoxImport As GroupBox
    Friend WithEvents ButtonSelectFile As Button
    Friend WithEvents TextBoxFilePath As TextBox
    Friend WithEvents ButtonImport As Button
    Friend WithEvents OpenFileDialogSelectFile As OpenFileDialog
    Friend WithEvents DateTimePickerEffectiveDate As DateTimePicker
    Friend WithEvents LabelEffectiveDate As Label
    Friend WithEvents ButtonCancel As Button
    Friend WithEvents ButtonSend As Button
    Friend WithEvents LabelImportStatusValue As Label
    Friend WithEvents LabelImportStatus As Label
End Class
