<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ExtraTextLookup
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
        Me.txtExtraText = New System.Windows.Forms.TextBox
        Me.ExtraTextLabel = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.cmbDescription = New System.Windows.Forms.ComboBox
        Me.btnChooseRecord = New System.Windows.Forms.Button
        Me.LabelTypeLabel = New System.Windows.Forms.Label
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmbLabelType = New System.Windows.Forms.ComboBox
        Me._chkLinkBack = New System.Windows.Forms.CheckBox
        Me.SuspendLayout()
        '
        'txtExtraText
        '
        Me.txtExtraText.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtExtraText.Location = New System.Drawing.Point(21, 103)
        Me.txtExtraText.MaxLength = 4200
        Me.txtExtraText.Multiline = True
        Me.txtExtraText.Name = "txtExtraText"
        Me.txtExtraText.ReadOnly = True
        Me.txtExtraText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtExtraText.Size = New System.Drawing.Size(393, 209)
        Me.txtExtraText.TabIndex = 3
        '
        'ExtraTextLabel
        '
        Me.ExtraTextLabel.BackColor = System.Drawing.Color.Transparent
        Me.ExtraTextLabel.Cursor = System.Windows.Forms.Cursors.Default
        Me.ExtraTextLabel.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ExtraTextLabel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ExtraTextLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.ExtraTextLabel.Location = New System.Drawing.Point(18, 77)
        Me.ExtraTextLabel.Name = "ExtraTextLabel"
        Me.ExtraTextLabel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ExtraTextLabel.Size = New System.Drawing.Size(73, 20)
        Me.ExtraTextLabel.TabIndex = 43
        Me.ExtraTextLabel.Text = "Extra Text :"
        Me.ExtraTextLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(9, 15)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(80, 17)
        Me.Label2.TabIndex = 76
        Me.Label2.Text = "Description :"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'cmbDescription
        '
        Me.cmbDescription.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cmbDescription.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbDescription.FormattingEnabled = True
        Me.cmbDescription.Location = New System.Drawing.Point(91, 12)
        Me.cmbDescription.Name = "cmbDescription"
        Me.cmbDescription.Size = New System.Drawing.Size(323, 21)
        Me.cmbDescription.TabIndex = 0
        '
        'btnChooseRecord
        '
        Me.btnChooseRecord.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnChooseRecord.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnChooseRecord.Location = New System.Drawing.Point(295, 67)
        Me.btnChooseRecord.Name = "btnChooseRecord"
        Me.btnChooseRecord.Size = New System.Drawing.Size(119, 30)
        Me.btnChooseRecord.TabIndex = 7
        Me.btnChooseRecord.Text = "Choose This Record"
        Me.btnChooseRecord.UseVisualStyleBackColor = True
        '
        'LabelTypeLabel
        '
        Me.LabelTypeLabel.BackColor = System.Drawing.Color.Transparent
        Me.LabelTypeLabel.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelTypeLabel.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelTypeLabel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelTypeLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.LabelTypeLabel.Location = New System.Drawing.Point(8, 44)
        Me.LabelTypeLabel.Name = "LabelTypeLabel"
        Me.LabelTypeLabel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelTypeLabel.Size = New System.Drawing.Size(81, 17)
        Me.LabelTypeLabel.TabIndex = 126
        Me.LabelTypeLabel.Text = "Label Type :"
        Me.LabelTypeLabel.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'cmdCancel
        '
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.cmdCancel.Location = New System.Drawing.Point(344, 318)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(70, 21)
        Me.cmdCancel.TabIndex = 128
        Me.cmdCancel.Text = "Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = True
        '
        'cmbLabelType
        '
        Me.cmbLabelType.BackColor = System.Drawing.SystemColors.Control
        Me.cmbLabelType.Enabled = False
        Me.cmbLabelType.FormattingEnabled = True
        Me.cmbLabelType.Location = New System.Drawing.Point(91, 40)
        Me.cmbLabelType.Name = "cmbLabelType"
        Me.cmbLabelType.Size = New System.Drawing.Size(323, 21)
        Me.cmbLabelType.TabIndex = 129
        '
        '_chkLinkBack
        '
        Me._chkLinkBack.AutoSize = True
        Me._chkLinkBack.Checked = Global.MySettings.Default._autoChooseExtraTextLookup
        Me._chkLinkBack.CheckState = System.Windows.Forms.CheckState.Checked
        Me._chkLinkBack.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Global.MySettings.Default, "_autoChooseExtraTextLookup", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me._chkLinkBack.Location = New System.Drawing.Point(21, 321)
        Me._chkLinkBack.Name = "_chkLinkBack"
        Me._chkLinkBack.Size = New System.Drawing.Size(136, 17)
        Me._chkLinkBack.TabIndex = 130
        Me._chkLinkBack.Text = "Link to current identifier"
        Me._chkLinkBack.UseVisualStyleBackColor = True
        '
        'ExtraTextLookup
        '
        Me.AcceptButton = Me.btnChooseRecord
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(426, 347)
        Me.Controls.Add(Me._chkLinkBack)
        Me.Controls.Add(Me.cmbLabelType)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.LabelTypeLabel)
        Me.Controls.Add(Me.btnChooseRecord)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.cmbDescription)
        Me.Controls.Add(Me.ExtraTextLabel)
        Me.Controls.Add(Me.txtExtraText)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ExtraTextLookup"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Extra Text Lookup"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtExtraText As System.Windows.Forms.TextBox
    Public WithEvents ExtraTextLabel As System.Windows.Forms.Label
    Public WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents cmbDescription As System.Windows.Forms.ComboBox
    Friend WithEvents btnChooseRecord As System.Windows.Forms.Button
    Public WithEvents LabelTypeLabel As System.Windows.Forms.Label
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents cmbLabelType As System.Windows.Forms.ComboBox
    Friend WithEvents _chkLinkBack As System.Windows.Forms.CheckBox
End Class
