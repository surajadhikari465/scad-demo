<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ExtraTextNew
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
        Me.components = New System.ComponentModel.Container
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.LabelTypeLabel = New System.Windows.Forms.Label
        Me.btnAddRecord = New System.Windows.Forms.Button
        Me.ExtraTextLabel = New System.Windows.Forms.Label
        Me.txtExtraText = New System.Windows.Forms.TextBox
        Me.DescriptionLabel = New System.Windows.Forms.Label
        Me.txtDescription = New System.Windows.Forms.TextBox
        Me.cmbLabelType = New System.Windows.Forms.ComboBox
        Me.btnClearForm = New System.Windows.Forms.Button
        Me.grpNewRecord = New System.Windows.Forms.GroupBox
        Me.frmErrorProvider = New System.Windows.Forms.ErrorProvider(Me.components)
        Me._chkLinkBack = New System.Windows.Forms.CheckBox
        Me.grpNewRecord.SuspendLayout()
        CType(Me.frmErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdCancel
        '
        Me.cmdCancel.CausesValidation = False
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.cmdCancel.Location = New System.Drawing.Point(264, 281)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(70, 30)
        Me.cmdCancel.TabIndex = 4
        Me.cmdCancel.Text = "Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = True
        '
        'LabelTypeLabel
        '
        Me.LabelTypeLabel.BackColor = System.Drawing.Color.Transparent
        Me.LabelTypeLabel.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelTypeLabel.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelTypeLabel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelTypeLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.LabelTypeLabel.Location = New System.Drawing.Point(16, 51)
        Me.LabelTypeLabel.Name = "LabelTypeLabel"
        Me.LabelTypeLabel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelTypeLabel.Size = New System.Drawing.Size(81, 17)
        Me.LabelTypeLabel.TabIndex = 134
        Me.LabelTypeLabel.Text = "Label Type :"
        Me.LabelTypeLabel.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'btnAddRecord
        '
        Me.btnAddRecord.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnAddRecord.Location = New System.Drawing.Point(341, 281)
        Me.btnAddRecord.Name = "btnAddRecord"
        Me.btnAddRecord.Size = New System.Drawing.Size(112, 30)
        Me.btnAddRecord.TabIndex = 3
        Me.btnAddRecord.Text = "Add This Record"
        Me.btnAddRecord.UseVisualStyleBackColor = True
        '
        'ExtraTextLabel
        '
        Me.ExtraTextLabel.BackColor = System.Drawing.Color.Transparent
        Me.ExtraTextLabel.Cursor = System.Windows.Forms.Cursors.Default
        Me.ExtraTextLabel.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ExtraTextLabel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ExtraTextLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.ExtraTextLabel.Location = New System.Drawing.Point(21, 75)
        Me.ExtraTextLabel.Name = "ExtraTextLabel"
        Me.ExtraTextLabel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ExtraTextLabel.Size = New System.Drawing.Size(73, 20)
        Me.ExtraTextLabel.TabIndex = 132
        Me.ExtraTextLabel.Text = "Extra Text :"
        Me.ExtraTextLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'txtExtraText
        '
        Me.txtExtraText.Location = New System.Drawing.Point(24, 98)
        Me.txtExtraText.MaxLength = 4200
        Me.txtExtraText.Multiline = True
        Me.txtExtraText.Name = "txtExtraText"
        Me.txtExtraText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtExtraText.Size = New System.Drawing.Size(389, 166)
        Me.txtExtraText.TabIndex = 2
        '
        'DescriptionLabel
        '
        Me.DescriptionLabel.BackColor = System.Drawing.Color.Transparent
        Me.DescriptionLabel.Cursor = System.Windows.Forms.Cursors.Default
        Me.DescriptionLabel.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DescriptionLabel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.DescriptionLabel.Location = New System.Drawing.Point(17, 24)
        Me.DescriptionLabel.Name = "DescriptionLabel"
        Me.DescriptionLabel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.DescriptionLabel.Size = New System.Drawing.Size(80, 17)
        Me.DescriptionLabel.TabIndex = 133
        Me.DescriptionLabel.Text = "Description :"
        Me.DescriptionLabel.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'txtDescription
        '
        Me.txtDescription.BackColor = System.Drawing.SystemColors.Window
        Me.txtDescription.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDescription.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDescription.Location = New System.Drawing.Point(103, 21)
        Me.txtDescription.MaxLength = 50
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDescription.Size = New System.Drawing.Size(310, 20)
        Me.txtDescription.TabIndex = 0
        Me.txtDescription.Tag = "String"
        '
        'cmbLabelType
        '
        Me.cmbLabelType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cmbLabelType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbLabelType.FormattingEnabled = True
        Me.cmbLabelType.Location = New System.Drawing.Point(103, 48)
        Me.cmbLabelType.Name = "cmbLabelType"
        Me.cmbLabelType.Size = New System.Drawing.Size(310, 21)
        Me.cmbLabelType.TabIndex = 1
        '
        'btnClearForm
        '
        Me.btnClearForm.CausesValidation = False
        Me.btnClearForm.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnClearForm.Location = New System.Drawing.Point(188, 281)
        Me.btnClearForm.Name = "btnClearForm"
        Me.btnClearForm.Size = New System.Drawing.Size(70, 30)
        Me.btnClearForm.TabIndex = 5
        Me.btnClearForm.Text = "Clear Form"
        Me.btnClearForm.UseVisualStyleBackColor = True
        '
        'grpNewRecord
        '
        Me.grpNewRecord.CausesValidation = False
        Me.grpNewRecord.Controls.Add(Me.txtDescription)
        Me.grpNewRecord.Controls.Add(Me.txtExtraText)
        Me.grpNewRecord.Controls.Add(Me.cmbLabelType)
        Me.grpNewRecord.Controls.Add(Me.ExtraTextLabel)
        Me.grpNewRecord.Controls.Add(Me.DescriptionLabel)
        Me.grpNewRecord.Controls.Add(Me.LabelTypeLabel)
        Me.grpNewRecord.Location = New System.Drawing.Point(9, 5)
        Me.grpNewRecord.Name = "grpNewRecord"
        Me.grpNewRecord.Size = New System.Drawing.Size(444, 270)
        Me.grpNewRecord.TabIndex = 0
        Me.grpNewRecord.TabStop = False
        '
        'frmErrorProvider
        '
        Me.frmErrorProvider.ContainerControl = Me
        '
        '_chkLinkBack
        '
        Me._chkLinkBack.AutoSize = True
        Me._chkLinkBack.CausesValidation = False
        Me._chkLinkBack.Checked = Global.MySettings.Default._autoChooseExtraTextNew
        Me._chkLinkBack.CheckState = System.Windows.Forms.CheckState.Checked
        Me._chkLinkBack.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Global.MySettings.Default, "_autoChooseExtraTextNew", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me._chkLinkBack.Location = New System.Drawing.Point(12, 289)
        Me._chkLinkBack.Name = "_chkLinkBack"
        Me._chkLinkBack.Size = New System.Drawing.Size(136, 17)
        Me._chkLinkBack.TabIndex = 6
        Me._chkLinkBack.Text = "Link to current identifier"
        Me._chkLinkBack.UseVisualStyleBackColor = True
        Me._chkLinkBack.Visible = False
        '
        'ExtraTextNew
        '
        Me.AcceptButton = Me.btnAddRecord
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(461, 318)
        Me.Controls.Add(Me._chkLinkBack)
        Me.Controls.Add(Me.grpNewRecord)
        Me.Controls.Add(Me.btnClearForm)
        Me.Controls.Add(Me.btnAddRecord)
        Me.Controls.Add(Me.cmdCancel)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ExtraTextNew"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "New Extra Text Record"
        Me.grpNewRecord.ResumeLayout(False)
        Me.grpNewRecord.PerformLayout()
        CType(Me.frmErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents LabelTypeLabel As System.Windows.Forms.Label
    Friend WithEvents btnAddRecord As System.Windows.Forms.Button
    Public WithEvents ExtraTextLabel As System.Windows.Forms.Label
    Friend WithEvents txtExtraText As System.Windows.Forms.TextBox
    Public WithEvents DescriptionLabel As System.Windows.Forms.Label
    Public WithEvents txtDescription As System.Windows.Forms.TextBox
    Friend WithEvents cmbLabelType As System.Windows.Forms.ComboBox
    Friend WithEvents btnClearForm As System.Windows.Forms.Button
    Friend WithEvents grpNewRecord As System.Windows.Forms.GroupBox
    Friend WithEvents _chkLinkBack As System.Windows.Forms.CheckBox
    Friend WithEvents frmErrorProvider As System.Windows.Forms.ErrorProvider
End Class
