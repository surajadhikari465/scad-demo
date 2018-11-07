<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmOutOfPeriodInvoice
#Region "Windows Form Designer generated code "
	<System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
	End Sub
	'Form overrides dispose to clean up the component list.
	<System.Diagnostics.DebuggerNonUserCode()> Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
		If Disposing Then
			If Not components Is Nothing Then
				components.Dispose()
			End If
		End If
		MyBase.Dispose(Disposing)
	End Sub
	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer
	Public ToolTip1 As System.Windows.Forms.ToolTip
	Public WithEvents cmdExit As System.Windows.Forms.Button
	Public WithEvents cmdReport As System.Windows.Forms.Button
	Public WithEvents _cmbField_1 As System.Windows.Forms.ComboBox
	Public WithEvents chkPrintOnly As System.Windows.Forms.CheckBox
	Public WithEvents _cmbField_2 As System.Windows.Forms.ComboBox
	Public WithEvents mskStartDate As System.Windows.Forms.MaskedTextBox
    Public WithEvents _lblLabel_0 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_5 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_2 As System.Windows.Forms.Label
	Public WithEvents cmbField As Microsoft.VisualBasic.Compatibility.VB6.ComboBoxArray
	Public WithEvents lblLabel As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmOutOfPeriodInvoice))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdExit = New System.Windows.Forms.Button
        Me.cmdReport = New System.Windows.Forms.Button
        Me._cmbField_1 = New System.Windows.Forms.ComboBox
        Me.chkPrintOnly = New System.Windows.Forms.CheckBox
        Me._cmbField_2 = New System.Windows.Forms.ComboBox
        Me.mskStartDate = New System.Windows.Forms.MaskedTextBox
        Me._lblLabel_0 = New System.Windows.Forms.Label
        Me._lblLabel_5 = New System.Windows.Forms.Label
        Me._lblLabel_2 = New System.Windows.Forms.Label
        Me.cmbField = New Microsoft.VisualBasic.Compatibility.VB6.ComboBoxArray(Me.components)
        Me.lblLabel = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(Me.components)
        Me.chkAllSubTeams = New System.Windows.Forms.CheckBox
        Me.chkAllStores = New System.Windows.Forms.CheckBox
        CType(Me.cmbField, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdExit
        '
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdExit.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Image = CType(resources.GetObject("cmdExit.Image"), System.Drawing.Image)
        Me.cmdExit.Location = New System.Drawing.Point(530, 132)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(41, 41)
        Me.cmdExit.TabIndex = 5
        Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdExit, "Exit")
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'cmdReport
        '
        Me.cmdReport.BackColor = System.Drawing.SystemColors.Control
        Me.cmdReport.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdReport.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdReport.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdReport.Image = CType(resources.GetObject("cmdReport.Image"), System.Drawing.Image)
        Me.cmdReport.Location = New System.Drawing.Point(483, 132)
        Me.cmdReport.Name = "cmdReport"
        Me.cmdReport.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdReport.Size = New System.Drawing.Size(41, 41)
        Me.cmdReport.TabIndex = 4
        Me.cmdReport.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdReport, "Print")
        Me.cmdReport.UseVisualStyleBackColor = False
        '
        '_cmbField_1
        '
        Me._cmbField_1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me._cmbField_1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me._cmbField_1.BackColor = System.Drawing.SystemColors.Window
        Me._cmbField_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmbField_1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me._cmbField_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmbField_1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbField.SetIndex(Me._cmbField_1, CType(1, Short))
        Me._cmbField_1.Location = New System.Drawing.Point(250, 18)
        Me._cmbField_1.Name = "_cmbField_1"
        Me._cmbField_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmbField_1.Size = New System.Drawing.Size(321, 22)
        Me._cmbField_1.Sorted = True
        Me._cmbField_1.TabIndex = 0
        '
        'chkPrintOnly
        '
        Me.chkPrintOnly.BackColor = System.Drawing.SystemColors.Control
        Me.chkPrintOnly.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkPrintOnly.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPrintOnly.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkPrintOnly.Location = New System.Drawing.Point(385, 155)
        Me.chkPrintOnly.Name = "chkPrintOnly"
        Me.chkPrintOnly.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkPrintOnly.Size = New System.Drawing.Size(81, 17)
        Me.chkPrintOnly.TabIndex = 3
        Me.chkPrintOnly.Text = "Print Only"
        Me.chkPrintOnly.UseVisualStyleBackColor = False
        '
        '_cmbField_2
        '
        Me._cmbField_2.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me._cmbField_2.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me._cmbField_2.BackColor = System.Drawing.SystemColors.Window
        Me._cmbField_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmbField_2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me._cmbField_2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmbField_2.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbField.SetIndex(Me._cmbField_2, CType(2, Short))
        Me._cmbField_2.Location = New System.Drawing.Point(250, 43)
        Me._cmbField_2.Name = "_cmbField_2"
        Me._cmbField_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmbField_2.Size = New System.Drawing.Size(321, 22)
        Me._cmbField_2.Sorted = True
        Me._cmbField_2.TabIndex = 1
        '
        'mskStartDate
        '
        Me.mskStartDate.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.mskStartDate.Location = New System.Drawing.Point(250, 88)
        Me.mskStartDate.Name = "mskStartDate"
        Me.mskStartDate.PromptChar = Global.Microsoft.VisualBasic.ChrW(48)
        Me.mskStartDate.Size = New System.Drawing.Size(73, 20)
        Me.mskStartDate.TabIndex = 2
        '
        '_lblLabel_0
        '
        Me._lblLabel_0.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_0.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_0, CType(0, Short))
        Me._lblLabel_0.Location = New System.Drawing.Point(147, 91)
        Me._lblLabel_0.Name = "_lblLabel_0"
        Me._lblLabel_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_0.Size = New System.Drawing.Size(97, 17)
        Me._lblLabel_0.TabIndex = 8
        Me._lblLabel_0.Text = "Period :"
        Me._lblLabel_0.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_5
        '
        Me._lblLabel_5.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_5.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_5, CType(5, Short))
        Me._lblLabel_5.Location = New System.Drawing.Point(147, 21)
        Me._lblLabel_5.Name = "_lblLabel_5"
        Me._lblLabel_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_5.Size = New System.Drawing.Size(97, 17)
        Me._lblLabel_5.TabIndex = 7
        Me._lblLabel_5.Text = "Store :"
        Me._lblLabel_5.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_2
        '
        Me._lblLabel_2.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_2, CType(2, Short))
        Me._lblLabel_2.Location = New System.Drawing.Point(147, 43)
        Me._lblLabel_2.Name = "_lblLabel_2"
        Me._lblLabel_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_2.Size = New System.Drawing.Size(97, 17)
        Me._lblLabel_2.TabIndex = 6
        Me._lblLabel_2.Text = "Sub-Team :"
        Me._lblLabel_2.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'cmbField
        '
        '
        'chkAllSubTeams
        '
        Me.chkAllSubTeams.BackColor = System.Drawing.SystemColors.Control
        Me.chkAllSubTeams.Checked = True
        Me.chkAllSubTeams.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkAllSubTeams.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkAllSubTeams.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkAllSubTeams.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkAllSubTeams.Location = New System.Drawing.Point(28, 43)
        Me.chkAllSubTeams.Name = "chkAllSubTeams"
        Me.chkAllSubTeams.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkAllSubTeams.Size = New System.Drawing.Size(108, 19)
        Me.chkAllSubTeams.TabIndex = 15
        Me.chkAllSubTeams.Text = "All Sub-Teams"
        Me.chkAllSubTeams.UseVisualStyleBackColor = False
        '
        'chkAllStores
        '
        Me.chkAllStores.BackColor = System.Drawing.SystemColors.Control
        Me.chkAllStores.Checked = True
        Me.chkAllStores.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkAllStores.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkAllStores.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkAllStores.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkAllStores.Location = New System.Drawing.Point(28, 19)
        Me.chkAllStores.Name = "chkAllStores"
        Me.chkAllStores.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkAllStores.Size = New System.Drawing.Size(92, 19)
        Me.chkAllStores.TabIndex = 14
        Me.chkAllStores.Text = "All Stores"
        Me.chkAllStores.UseVisualStyleBackColor = False
        '
        'frmOutOfPeriodInvoice
        '
        Me.AcceptButton = Me.cmdReport
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdExit
        Me.ClientSize = New System.Drawing.Size(594, 191)
        Me.Controls.Add(Me.chkAllSubTeams)
        Me.Controls.Add(Me.chkAllStores)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.cmdReport)
        Me.Controls.Add(Me._cmbField_1)
        Me.Controls.Add(Me.chkPrintOnly)
        Me.Controls.Add(Me._cmbField_2)
        Me.Controls.Add(Me.mskStartDate)
        Me.Controls.Add(Me._lblLabel_0)
        Me.Controls.Add(Me._lblLabel_5)
        Me.Controls.Add(Me._lblLabel_2)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Location = New System.Drawing.Point(82, 472)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmOutOfPeriodInvoice"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Out of Period Invoice Report"
        CType(Me.cmbField, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Public WithEvents chkAllSubTeams As System.Windows.Forms.CheckBox
    Public WithEvents chkAllStores As System.Windows.Forms.CheckBox
#End Region 
End Class