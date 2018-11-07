<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmMarginReport
#Region "Windows Form Designer generated code "
	<System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
        MyBase.New()
        Me.IsInitializing = True
		'This call is required by the Windows Form Designer.
        InitializeComponent()
        Me.IsInitializing = False
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
	Public WithEvents optOutRange As System.Windows.Forms.RadioButton
	Public WithEvents optInRange As System.Windows.Forms.RadioButton
	Public WithEvents Frame2 As System.Windows.Forms.GroupBox
	Public WithEvents _txtField_1 As System.Windows.Forms.TextBox
	Public WithEvents optVendor As System.Windows.Forms.RadioButton
	Public WithEvents optSubTeam As System.Windows.Forms.RadioButton
	Public WithEvents Frame1 As System.Windows.Forms.GroupBox
	Public WithEvents cmdVendorSearch As System.Windows.Forms.Button
	Public WithEvents txtVendor As System.Windows.Forms.TextBox
    Public WithEvents _cmbField_0 As System.Windows.Forms.ComboBox
	Public WithEvents _cmbField_1 As System.Windows.Forms.ComboBox
	Public WithEvents _txtField_0 As System.Windows.Forms.TextBox
	Public WithEvents cmdReport As System.Windows.Forms.Button
	Public WithEvents cmdExit As System.Windows.Forms.Button
    'Public WithEvents crwReport As AxCrystal.AxCrystalReport
	Public WithEvents _lblLabel_2 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_0 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_1 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_5 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_16 As System.Windows.Forms.Label
	Public WithEvents cmbField As Microsoft.VisualBasic.Compatibility.VB6.ComboBoxArray
	Public WithEvents lblLabel As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	Public WithEvents txtField As Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMarginReport))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdVendorSearch = New System.Windows.Forms.Button
        Me.cmdReport = New System.Windows.Forms.Button
        Me.cmdExit = New System.Windows.Forms.Button
        Me.Frame2 = New System.Windows.Forms.GroupBox
        Me.optOutRange = New System.Windows.Forms.RadioButton
        Me.optInRange = New System.Windows.Forms.RadioButton
        Me._txtField_1 = New System.Windows.Forms.TextBox
        Me.Frame1 = New System.Windows.Forms.GroupBox
        Me.optVendor = New System.Windows.Forms.RadioButton
        Me.optSubTeam = New System.Windows.Forms.RadioButton
        Me.txtVendor = New System.Windows.Forms.TextBox
        Me._cmbField_0 = New System.Windows.Forms.ComboBox
        Me._cmbField_1 = New System.Windows.Forms.ComboBox
        Me._txtField_0 = New System.Windows.Forms.TextBox
        Me._lblLabel_2 = New System.Windows.Forms.Label
        Me._lblLabel_0 = New System.Windows.Forms.Label
        Me._lblLabel_1 = New System.Windows.Forms.Label
        Me._lblLabel_5 = New System.Windows.Forms.Label
        Me._lblLabel_16 = New System.Windows.Forms.Label
        Me.cmbField = New Microsoft.VisualBasic.Compatibility.VB6.ComboBoxArray(Me.components)
        Me.lblLabel = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(Me.components)
        Me.txtField = New Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray(Me.components)
        Me.Frame2.SuspendLayout()
        Me.Frame1.SuspendLayout()
        CType(Me.cmbField, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtField, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdVendorSearch
        '
        Me.cmdVendorSearch.BackColor = System.Drawing.SystemColors.Control
        Me.cmdVendorSearch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdVendorSearch.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdVendorSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdVendorSearch.Image = CType(resources.GetObject("cmdVendorSearch.Image"), System.Drawing.Image)
        Me.cmdVendorSearch.Location = New System.Drawing.Point(280, 8)
        Me.cmdVendorSearch.Name = "cmdVendorSearch"
        Me.cmdVendorSearch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdVendorSearch.Size = New System.Drawing.Size(25, 20)
        Me.cmdVendorSearch.TabIndex = 2
        Me.cmdVendorSearch.TabStop = False
        Me.cmdVendorSearch.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdVendorSearch, "Search for Vendor")
        Me.cmdVendorSearch.UseVisualStyleBackColor = False
        '
        'cmdReport
        '
        Me.cmdReport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdReport.BackColor = System.Drawing.SystemColors.Control
        Me.cmdReport.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdReport.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdReport.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdReport.Image = CType(resources.GetObject("cmdReport.Image"), System.Drawing.Image)
        Me.cmdReport.Location = New System.Drawing.Point(216, 197)
        Me.cmdReport.Name = "cmdReport"
        Me.cmdReport.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdReport.Size = New System.Drawing.Size(41, 41)
        Me.cmdReport.TabIndex = 17
        Me.cmdReport.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdReport, "Print")
        Me.cmdReport.UseVisualStyleBackColor = False
        '
        'cmdExit
        '
        Me.cmdExit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdExit.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Image = CType(resources.GetObject("cmdExit.Image"), System.Drawing.Image)
        Me.cmdExit.Location = New System.Drawing.Point(264, 197)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(41, 41)
        Me.cmdExit.TabIndex = 18
        Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdExit, "Exit")
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'Frame2
        '
        Me.Frame2.BackColor = System.Drawing.SystemColors.Control
        Me.Frame2.Controls.Add(Me.optOutRange)
        Me.Frame2.Controls.Add(Me.optInRange)
        Me.Frame2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame2.Location = New System.Drawing.Point(25, 102)
        Me.Frame2.Name = "Frame2"
        Me.Frame2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame2.Size = New System.Drawing.Size(225, 41)
        Me.Frame2.TabIndex = 11
        Me.Frame2.TabStop = False
        Me.Frame2.Text = "Margins Only"
        '
        'optOutRange
        '
        Me.optOutRange.BackColor = System.Drawing.SystemColors.Control
        Me.optOutRange.Cursor = System.Windows.Forms.Cursors.Default
        Me.optOutRange.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optOutRange.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optOutRange.Location = New System.Drawing.Point(96, 16)
        Me.optOutRange.Name = "optOutRange"
        Me.optOutRange.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optOutRange.Size = New System.Drawing.Size(121, 17)
        Me.optOutRange.TabIndex = 13
        Me.optOutRange.TabStop = True
        Me.optOutRange.Text = "Out of Range"
        Me.optOutRange.UseVisualStyleBackColor = False
        '
        'optInRange
        '
        Me.optInRange.BackColor = System.Drawing.SystemColors.Control
        Me.optInRange.Checked = True
        Me.optInRange.Cursor = System.Windows.Forms.Cursors.Default
        Me.optInRange.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optInRange.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optInRange.Location = New System.Drawing.Point(8, 16)
        Me.optInRange.Name = "optInRange"
        Me.optInRange.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optInRange.Size = New System.Drawing.Size(89, 17)
        Me.optInRange.TabIndex = 12
        Me.optInRange.TabStop = True
        Me.optInRange.Text = "In Range"
        Me.optInRange.UseVisualStyleBackColor = False
        '
        '_txtField_1
        '
        Me._txtField_1.AcceptsReturn = True
        Me._txtField_1.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtField_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtField_1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_1, CType(1, Short))
        Me._txtField_1.Location = New System.Drawing.Point(152, 64)
        Me._txtField_1.MaxLength = 2
        Me._txtField_1.Name = "_txtField_1"
        Me._txtField_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtField_1.Size = New System.Drawing.Size(33, 20)
        Me._txtField_1.TabIndex = 9
        Me._txtField_1.Tag = "Integer"
        '
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me.optVendor)
        Me.Frame1.Controls.Add(Me.optSubTeam)
        Me.Frame1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Location = New System.Drawing.Point(25, 159)
        Me.Frame1.Name = "Frame1"
        Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame1.Size = New System.Drawing.Size(185, 41)
        Me.Frame1.TabIndex = 14
        Me.Frame1.TabStop = False
        Me.Frame1.Text = "Report Type"
        '
        'optVendor
        '
        Me.optVendor.BackColor = System.Drawing.SystemColors.Control
        Me.optVendor.Checked = True
        Me.optVendor.Cursor = System.Windows.Forms.Cursors.Default
        Me.optVendor.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optVendor.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optVendor.Location = New System.Drawing.Point(8, 16)
        Me.optVendor.Name = "optVendor"
        Me.optVendor.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optVendor.Size = New System.Drawing.Size(89, 17)
        Me.optVendor.TabIndex = 15
        Me.optVendor.TabStop = True
        Me.optVendor.Text = "By Vendor"
        Me.optVendor.UseVisualStyleBackColor = False
        '
        'optSubTeam
        '
        Me.optSubTeam.BackColor = System.Drawing.SystemColors.Control
        Me.optSubTeam.Cursor = System.Windows.Forms.Cursors.Default
        Me.optSubTeam.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optSubTeam.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optSubTeam.Location = New System.Drawing.Point(96, 16)
        Me.optSubTeam.Name = "optSubTeam"
        Me.optSubTeam.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optSubTeam.Size = New System.Drawing.Size(73, 17)
        Me.optSubTeam.TabIndex = 16
        Me.optSubTeam.TabStop = True
        Me.optSubTeam.Text = "By Dept"
        Me.optSubTeam.UseVisualStyleBackColor = False
        '
        'txtVendor
        '
        Me.txtVendor.AcceptsReturn = True
        Me.txtVendor.BackColor = System.Drawing.SystemColors.Window
        Me.txtVendor.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtVendor.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtVendor.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtVendor.Location = New System.Drawing.Point(104, 8)
        Me.txtVendor.MaxLength = 50
        Me.txtVendor.Name = "txtVendor"
        Me.txtVendor.ReadOnly = True
        Me.txtVendor.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtVendor.Size = New System.Drawing.Size(177, 20)
        Me.txtVendor.TabIndex = 1
        Me.txtVendor.Tag = "-1"
        '
        '_cmbField_0
        '
        Me._cmbField_0.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me._cmbField_0.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me._cmbField_0.BackColor = System.Drawing.SystemColors.Window
        Me._cmbField_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmbField_0.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me._cmbField_0.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmbField_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbField.SetIndex(Me._cmbField_0, CType(0, Short))
        Me._cmbField_0.Location = New System.Drawing.Point(104, 36)
        Me._cmbField_0.Name = "_cmbField_0"
        Me._cmbField_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmbField_0.Size = New System.Drawing.Size(201, 22)
        Me._cmbField_0.Sorted = True
        Me._cmbField_0.TabIndex = 4
        '
        '_cmbField_1
        '
        Me._cmbField_1.BackColor = System.Drawing.SystemColors.Window
        Me._cmbField_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmbField_1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me._cmbField_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmbField_1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbField.SetIndex(Me._cmbField_1, CType(1, Short))
        Me._cmbField_1.Location = New System.Drawing.Point(104, 8)
        Me._cmbField_1.Name = "_cmbField_1"
        Me._cmbField_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmbField_1.Size = New System.Drawing.Size(201, 22)
        Me._cmbField_1.Sorted = True
        Me._cmbField_1.TabIndex = 6
        '
        '_txtField_0
        '
        Me._txtField_0.AcceptsReturn = True
        Me._txtField_0.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_0.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtField_0.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtField_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_0, CType(0, Short))
        Me._txtField_0.Location = New System.Drawing.Point(104, 64)
        Me._txtField_0.MaxLength = 2
        Me._txtField_0.Name = "_txtField_0"
        Me._txtField_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtField_0.Size = New System.Drawing.Size(33, 20)
        Me._txtField_0.TabIndex = 8
        Me._txtField_0.Tag = "Integer"
        '
        '_lblLabel_2
        '
        Me._lblLabel_2.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_2, CType(2, Short))
        Me._lblLabel_2.Location = New System.Drawing.Point(136, 64)
        Me._lblLabel_2.Name = "_lblLabel_2"
        Me._lblLabel_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_2.Size = New System.Drawing.Size(17, 17)
        Me._lblLabel_2.TabIndex = 19
        Me._lblLabel_2.Text = "-"
        Me._lblLabel_2.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblLabel_0
        '
        Me._lblLabel_0.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_0.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_0, CType(0, Short))
        Me._lblLabel_0.Location = New System.Drawing.Point(8, 11)
        Me._lblLabel_0.Name = "_lblLabel_0"
        Me._lblLabel_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_0.Size = New System.Drawing.Size(89, 17)
        Me._lblLabel_0.TabIndex = 0
        Me._lblLabel_0.Text = "Vendor :"
        Me._lblLabel_0.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_1
        '
        Me._lblLabel_1.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_1, CType(1, Short))
        Me._lblLabel_1.Location = New System.Drawing.Point(9, 39)
        Me._lblLabel_1.Name = "_lblLabel_1"
        Me._lblLabel_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_1.Size = New System.Drawing.Size(89, 17)
        Me._lblLabel_1.TabIndex = 3
        Me._lblLabel_1.Text = "Store :"
        Me._lblLabel_1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_5
        '
        Me._lblLabel_5.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_5.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_5, CType(5, Short))
        Me._lblLabel_5.Location = New System.Drawing.Point(8, 11)
        Me._lblLabel_5.Name = "_lblLabel_5"
        Me._lblLabel_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_5.Size = New System.Drawing.Size(89, 17)
        Me._lblLabel_5.TabIndex = 5
        Me._lblLabel_5.Text = "Sub-Team :"
        Me._lblLabel_5.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_16
        '
        Me._lblLabel_16.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_16.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_16.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_16.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_16, CType(16, Short))
        Me._lblLabel_16.Location = New System.Drawing.Point(9, 67)
        Me._lblLabel_16.Name = "_lblLabel_16"
        Me._lblLabel_16.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_16.Size = New System.Drawing.Size(89, 17)
        Me._lblLabel_16.TabIndex = 7
        Me._lblLabel_16.Text = "Margin :"
        Me._lblLabel_16.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'cmbField
        '
        '
        'txtField
        '
        '
        'frmMarginReport
        '
        Me.AcceptButton = Me.cmdReport
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdExit
        Me.ClientSize = New System.Drawing.Size(315, 249)
        Me.Controls.Add(Me.Frame2)
        Me.Controls.Add(Me._txtField_1)
        Me.Controls.Add(Me.Frame1)
        Me.Controls.Add(Me.cmdVendorSearch)
        Me.Controls.Add(Me.txtVendor)
        Me.Controls.Add(Me._cmbField_0)
        Me.Controls.Add(Me._cmbField_1)
        Me.Controls.Add(Me._txtField_0)
        Me.Controls.Add(Me.cmdReport)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me._lblLabel_2)
        Me.Controls.Add(Me._lblLabel_0)
        Me.Controls.Add(Me._lblLabel_1)
        Me.Controls.Add(Me._lblLabel_5)
        Me.Controls.Add(Me._lblLabel_16)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Location = New System.Drawing.Point(402, 296)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmMarginReport"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Margin Report"
        Me.Frame2.ResumeLayout(False)
        Me.Frame1.ResumeLayout(False)
        CType(Me.cmbField, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtField, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
#End Region 
End Class