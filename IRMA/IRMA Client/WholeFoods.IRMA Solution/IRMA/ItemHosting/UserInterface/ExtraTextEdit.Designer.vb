<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ExtraTextEdit
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
        Me.grpDetails = New System.Windows.Forms.GroupBox
        Me.txtExtraTextDescription = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.lblLabelType = New System.Windows.Forms.Label
        Me.cmbLabelType = New System.Windows.Forms.ComboBox
        Me.txtExtraText = New System.Windows.Forms.TextBox
        Me.cmdSave = New System.Windows.Forms.Button
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.btnExtraTxtLookup = New System.Windows.Forms.Button
        Me.btnExtraTxtNew = New System.Windows.Forms.Button
        Me.btnExtraTxtLink = New System.Windows.Forms.Button
        Me.formToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdClose = New System.Windows.Forms.Button
        Me.lblExtraTextItemIdentifier = New System.Windows.Forms.Label
        Me.lblExtraTextItemDesc = New System.Windows.Forms.Label
        Me.grpCurrentItem = New System.Windows.Forms.GroupBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.lblLinkedIdentifier = New System.Windows.Forms.Label
        Me.frmErrorProvider = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.grpCurrentRecordOptions = New System.Windows.Forms.GroupBox
        Me.GroupBox3 = New System.Windows.Forms.GroupBox
        Me.grpDetails.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.grpCurrentItem.SuspendLayout()
        CType(Me.frmErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpCurrentRecordOptions.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpDetails
        '
        Me.grpDetails.Controls.Add(Me.txtExtraTextDescription)
        Me.grpDetails.Controls.Add(Me.Label2)
        Me.grpDetails.Controls.Add(Me.lblLabelType)
        Me.grpDetails.Controls.Add(Me.cmbLabelType)
        Me.grpDetails.Controls.Add(Me.txtExtraText)
        Me.frmErrorProvider.SetIconAlignment(Me.grpDetails, System.Windows.Forms.ErrorIconAlignment.TopLeft)
        Me.grpDetails.Location = New System.Drawing.Point(14, 94)
        Me.grpDetails.Name = "grpDetails"
        Me.grpDetails.Size = New System.Drawing.Size(441, 227)
        Me.grpDetails.TabIndex = 0
        Me.grpDetails.TabStop = False
        Me.grpDetails.Text = "Extra Text Record Details"
        '
        'txtExtraTextDescription
        '
        Me.txtExtraTextDescription.Location = New System.Drawing.Point(129, 25)
        Me.txtExtraTextDescription.Name = "txtExtraTextDescription"
        Me.txtExtraTextDescription.Size = New System.Drawing.Size(282, 20)
        Me.txtExtraTextDescription.TabIndex = 2
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.frmErrorProvider.SetIconAlignment(Me.Label2, System.Windows.Forms.ErrorIconAlignment.MiddleLeft)
        Me.Label2.Location = New System.Drawing.Point(9, 28)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(114, 13)
        Me.Label2.TabIndex = 9
        Me.Label2.Text = "Extra Text Description:"
        '
        'lblLabelType
        '
        Me.lblLabelType.AutoSize = True
        Me.frmErrorProvider.SetIconAlignment(Me.lblLabelType, System.Windows.Forms.ErrorIconAlignment.MiddleLeft)
        Me.lblLabelType.Location = New System.Drawing.Point(60, 54)
        Me.lblLabelType.Name = "lblLabelType"
        Me.lblLabelType.Size = New System.Drawing.Size(63, 13)
        Me.lblLabelType.TabIndex = 2
        Me.lblLabelType.Text = "Label Type:"
        '
        'cmbLabelType
        '
        Me.cmbLabelType.FormattingEnabled = True
        Me.cmbLabelType.Location = New System.Drawing.Point(129, 51)
        Me.cmbLabelType.Name = "cmbLabelType"
        Me.cmbLabelType.Size = New System.Drawing.Size(282, 21)
        Me.cmbLabelType.TabIndex = 3
        '
        'txtExtraText
        '
        Me.txtExtraText.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtExtraText.Location = New System.Drawing.Point(12, 78)
        Me.txtExtraText.MaxLength = 4200
        Me.txtExtraText.Multiline = True
        Me.txtExtraText.Name = "txtExtraText"
        Me.txtExtraText.Size = New System.Drawing.Size(399, 143)
        Me.txtExtraText.TabIndex = 4
        '
        'cmdSave
        '
        Me.cmdSave.Enabled = False
        Me.cmdSave.Location = New System.Drawing.Point(12, 45)
        Me.cmdSave.Name = "cmdSave"
        Me.cmdSave.Size = New System.Drawing.Size(125, 30)
        Me.cmdSave.TabIndex = 6
        Me.cmdSave.Text = "Save Details Changes"
        Me.formToolTip.SetToolTip(Me.cmdSave, "Save the extra text change to the extra text record.")
        Me.cmdSave.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.btnExtraTxtLookup)
        Me.GroupBox2.Controls.Add(Me.btnExtraTxtNew)
        Me.GroupBox2.Location = New System.Drawing.Point(461, 9)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(145, 79)
        Me.GroupBox2.TabIndex = 1
        Me.GroupBox2.TabStop = False
        '
        'btnExtraTxtLookup
        '
        Me.btnExtraTxtLookup.Location = New System.Drawing.Point(12, 11)
        Me.btnExtraTxtLookup.Name = "btnExtraTxtLookup"
        Me.btnExtraTxtLookup.Size = New System.Drawing.Size(125, 31)
        Me.btnExtraTxtLookup.TabIndex = 0
        Me.btnExtraTxtLookup.Text = "Lookup Extra Text"
        Me.formToolTip.SetToolTip(Me.btnExtraTxtLookup, "Choose a different extra text record to view.")
        Me.btnExtraTxtLookup.UseVisualStyleBackColor = True
        '
        'btnExtraTxtNew
        '
        Me.btnExtraTxtNew.Location = New System.Drawing.Point(12, 43)
        Me.btnExtraTxtNew.Name = "btnExtraTxtNew"
        Me.btnExtraTxtNew.Size = New System.Drawing.Size(125, 30)
        Me.btnExtraTxtNew.TabIndex = 1
        Me.btnExtraTxtNew.Text = "Create New"
        Me.btnExtraTxtNew.UseVisualStyleBackColor = True
        '
        'btnExtraTxtLink
        '
        Me.btnExtraTxtLink.Enabled = False
        Me.btnExtraTxtLink.Location = New System.Drawing.Point(12, 11)
        Me.btnExtraTxtLink.Name = "btnExtraTxtLink"
        Me.btnExtraTxtLink.Size = New System.Drawing.Size(125, 31)
        Me.btnExtraTxtLink.TabIndex = 5
        Me.btnExtraTxtLink.Text = "Select This Record"
        Me.formToolTip.SetToolTip(Me.btnExtraTxtLink, "Link the current Scale Extra Text record to the currently selected item.")
        Me.btnExtraTxtLink.UseVisualStyleBackColor = True
        '
        'cmdClose
        '
        Me.cmdClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdClose.Location = New System.Drawing.Point(12, 11)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New System.Drawing.Size(125, 30)
        Me.cmdClose.TabIndex = 7
        Me.cmdClose.Text = "Close"
        Me.formToolTip.SetToolTip(Me.cmdClose, "Save the extra text change to the extra text record.")
        Me.cmdClose.UseVisualStyleBackColor = True
        '
        'lblExtraTextItemIdentifier
        '
        Me.lblExtraTextItemIdentifier.AutoSize = True
        Me.lblExtraTextItemIdentifier.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblExtraTextItemIdentifier.Location = New System.Drawing.Point(83, 27)
        Me.lblExtraTextItemIdentifier.Name = "lblExtraTextItemIdentifier"
        Me.lblExtraTextItemIdentifier.Size = New System.Drawing.Size(89, 13)
        Me.lblExtraTextItemIdentifier.TabIndex = 2
        Me.lblExtraTextItemIdentifier.Text = "[ItemIdentifier]"
        '
        'lblExtraTextItemDesc
        '
        Me.lblExtraTextItemDesc.AutoSize = True
        Me.lblExtraTextItemDesc.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblExtraTextItemDesc.Location = New System.Drawing.Point(83, 48)
        Me.lblExtraTextItemDesc.Name = "lblExtraTextItemDesc"
        Me.lblExtraTextItemDesc.Size = New System.Drawing.Size(103, 13)
        Me.lblExtraTextItemDesc.TabIndex = 3
        Me.lblExtraTextItemDesc.Text = "[ItemDescription]"
        '
        'grpCurrentItem
        '
        Me.grpCurrentItem.Controls.Add(Me.Label1)
        Me.grpCurrentItem.Controls.Add(Me.lblLinkedIdentifier)
        Me.grpCurrentItem.Controls.Add(Me.lblExtraTextItemIdentifier)
        Me.grpCurrentItem.Controls.Add(Me.lblExtraTextItemDesc)
        Me.grpCurrentItem.Location = New System.Drawing.Point(14, 9)
        Me.grpCurrentItem.Name = "grpCurrentItem"
        Me.grpCurrentItem.Size = New System.Drawing.Size(441, 79)
        Me.grpCurrentItem.TabIndex = 8
        Me.grpCurrentItem.TabStop = False
        Me.grpCurrentItem.Text = "Current Identifier"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(14, 48)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(63, 13)
        Me.Label1.TabIndex = 8
        Me.Label1.Text = "Description:"
        '
        'lblLinkedIdentifier
        '
        Me.lblLinkedIdentifier.AutoSize = True
        Me.lblLinkedIdentifier.Location = New System.Drawing.Point(27, 27)
        Me.lblLinkedIdentifier.Name = "lblLinkedIdentifier"
        Me.lblLinkedIdentifier.Size = New System.Drawing.Size(50, 13)
        Me.lblLinkedIdentifier.TabIndex = 7
        Me.lblLinkedIdentifier.Text = "Identifier:"
        '
        'frmErrorProvider
        '
        Me.frmErrorProvider.ContainerControl = Me
        '
        'grpCurrentRecordOptions
        '
        Me.grpCurrentRecordOptions.Controls.Add(Me.btnExtraTxtLink)
        Me.grpCurrentRecordOptions.Controls.Add(Me.cmdSave)
        Me.grpCurrentRecordOptions.Location = New System.Drawing.Point(461, 94)
        Me.grpCurrentRecordOptions.Name = "grpCurrentRecordOptions"
        Me.grpCurrentRecordOptions.Size = New System.Drawing.Size(145, 82)
        Me.grpCurrentRecordOptions.TabIndex = 9
        Me.grpCurrentRecordOptions.TabStop = False
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.cmdClose)
        Me.GroupBox3.Location = New System.Drawing.Point(461, 274)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(145, 47)
        Me.GroupBox3.TabIndex = 10
        Me.GroupBox3.TabStop = False
        '
        'ExtraTextEdit
        '
        Me.AcceptButton = Me.cmdSave
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.cmdClose
        Me.ClientSize = New System.Drawing.Size(614, 331)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.grpCurrentRecordOptions)
        Me.Controls.Add(Me.grpCurrentItem)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.grpDetails)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ExtraTextEdit"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Edit Scale Item Extra Text"
        Me.grpDetails.ResumeLayout(False)
        Me.grpDetails.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.grpCurrentItem.ResumeLayout(False)
        Me.grpCurrentItem.PerformLayout()
        CType(Me.frmErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpCurrentRecordOptions.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpDetails As System.Windows.Forms.GroupBox
    Friend WithEvents txtExtraText As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents btnExtraTxtLookup As System.Windows.Forms.Button
    Friend WithEvents btnExtraTxtLink As System.Windows.Forms.Button
    Friend WithEvents formToolTip As System.Windows.Forms.ToolTip
    Friend WithEvents lblExtraTextItemIdentifier As System.Windows.Forms.Label
    Friend WithEvents lblExtraTextItemDesc As System.Windows.Forms.Label
    Friend WithEvents cmdSave As System.Windows.Forms.Button
    Friend WithEvents cmdClose As System.Windows.Forms.Button
    Friend WithEvents btnExtraTxtNew As System.Windows.Forms.Button
    Friend WithEvents cmbLabelType As System.Windows.Forms.ComboBox
    Friend WithEvents grpCurrentItem As System.Windows.Forms.GroupBox
    Friend WithEvents lblLabelType As System.Windows.Forms.Label
    Friend WithEvents frmErrorProvider As System.Windows.Forms.ErrorProvider
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblLinkedIdentifier As System.Windows.Forms.Label
    Friend WithEvents txtExtraTextDescription As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents grpCurrentRecordOptions As System.Windows.Forms.GroupBox
End Class
