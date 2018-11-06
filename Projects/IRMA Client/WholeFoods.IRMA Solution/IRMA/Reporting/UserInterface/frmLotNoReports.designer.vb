<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmLotNoReports
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
	Public WithEvents cmdReport As System.Windows.Forms.Button
	Public WithEvents cmdExit As System.Windows.Forms.Button
	Public WithEvents txtIdentifier As System.Windows.Forms.TextBox
	Public WithEvents txtEndDate As System.Windows.Forms.TextBox
	Public WithEvents txtBeginDate As System.Windows.Forms.TextBox
	Public WithEvents lblIdentifier As System.Windows.Forms.Label
	Public WithEvents Label2 As System.Windows.Forms.Label
	Public WithEvents Label1 As System.Windows.Forms.Label
	Public WithEvents frmIdentifier As System.Windows.Forms.Panel
	Public WithEvents cmbVendors As System.Windows.Forms.ComboBox
	Public WithEvents txtLotNumber As System.Windows.Forms.TextBox
	Public WithEvents lblLotNo As System.Windows.Forms.Label
	Public WithEvents frmLotNo As System.Windows.Forms.Panel
	Public WithEvents _optRptBy_1 As System.Windows.Forms.RadioButton
	Public WithEvents _optRptBy_0 As System.Windows.Forms.RadioButton
	Public WithEvents frmRptBy As System.Windows.Forms.GroupBox
	Public WithEvents lblVendor As System.Windows.Forms.Label
	Public WithEvents optRptBy As Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmLotNoReports))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdReport = New System.Windows.Forms.Button
        Me.cmdExit = New System.Windows.Forms.Button
        Me.frmIdentifier = New System.Windows.Forms.Panel
        Me.txtIdentifier = New System.Windows.Forms.TextBox
        Me.txtEndDate = New System.Windows.Forms.TextBox
        Me.txtBeginDate = New System.Windows.Forms.TextBox
        Me.lblIdentifier = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.cmbVendors = New System.Windows.Forms.ComboBox
        Me.frmLotNo = New System.Windows.Forms.Panel
        Me.txtLotNumber = New System.Windows.Forms.TextBox
        Me.lblLotNo = New System.Windows.Forms.Label
        Me.frmRptBy = New System.Windows.Forms.GroupBox
        Me._optRptBy_1 = New System.Windows.Forms.RadioButton
        Me._optRptBy_0 = New System.Windows.Forms.RadioButton
        Me.lblVendor = New System.Windows.Forms.Label
        Me.optRptBy = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(Me.components)
        Me.frmIdentifier.SuspendLayout()
        Me.frmLotNo.SuspendLayout()
        Me.frmRptBy.SuspendLayout()
        CType(Me.optRptBy, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdReport
        '
        Me.cmdReport.BackColor = System.Drawing.SystemColors.Control
        Me.cmdReport.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdReport.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdReport.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdReport.Image = CType(resources.GetObject("cmdReport.Image"), System.Drawing.Image)
        Me.cmdReport.Location = New System.Drawing.Point(325, 76)
        Me.cmdReport.Name = "cmdReport"
        Me.cmdReport.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdReport.Size = New System.Drawing.Size(41, 41)
        Me.cmdReport.TabIndex = 16
        Me.cmdReport.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdReport, "Print")
        Me.cmdReport.UseVisualStyleBackColor = False
        '
        'cmdExit
        '
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdExit.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Image = CType(resources.GetObject("cmdExit.Image"), System.Drawing.Image)
        Me.cmdExit.Location = New System.Drawing.Point(373, 76)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(41, 41)
        Me.cmdExit.TabIndex = 15
        Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdExit, "Exit")
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'frmIdentifier
        '
        Me.frmIdentifier.BackColor = System.Drawing.SystemColors.Control
        Me.frmIdentifier.Controls.Add(Me.txtIdentifier)
        Me.frmIdentifier.Controls.Add(Me.txtEndDate)
        Me.frmIdentifier.Controls.Add(Me.txtBeginDate)
        Me.frmIdentifier.Controls.Add(Me.lblIdentifier)
        Me.frmIdentifier.Controls.Add(Me.Label2)
        Me.frmIdentifier.Controls.Add(Me.Label1)
        Me.frmIdentifier.Cursor = System.Windows.Forms.Cursors.Default
        Me.frmIdentifier.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.frmIdentifier.ForeColor = System.Drawing.SystemColors.ControlText
        Me.frmIdentifier.Location = New System.Drawing.Point(-1, 38)
        Me.frmIdentifier.Name = "frmIdentifier"
        Me.frmIdentifier.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.frmIdentifier.Size = New System.Drawing.Size(295, 62)
        Me.frmIdentifier.TabIndex = 8
        Me.frmIdentifier.Visible = False
        '
        'txtIdentifier
        '
        Me.txtIdentifier.AcceptsReturn = True
        Me.txtIdentifier.BackColor = System.Drawing.SystemColors.Window
        Me.txtIdentifier.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtIdentifier.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtIdentifier.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtIdentifier.Location = New System.Drawing.Point(78, 27)
        Me.txtIdentifier.MaxLength = 13
        Me.txtIdentifier.Name = "txtIdentifier"
        Me.txtIdentifier.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtIdentifier.Size = New System.Drawing.Size(88, 20)
        Me.txtIdentifier.TabIndex = 14
        Me.txtIdentifier.Tag = "Number"
        '
        'txtEndDate
        '
        Me.txtEndDate.AcceptsReturn = True
        Me.txtEndDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtEndDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtEndDate.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEndDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtEndDate.Location = New System.Drawing.Point(223, 3)
        Me.txtEndDate.MaxLength = 10
        Me.txtEndDate.Name = "txtEndDate"
        Me.txtEndDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtEndDate.Size = New System.Drawing.Size(70, 20)
        Me.txtEndDate.TabIndex = 12
        Me.txtEndDate.Text = "12/12/2005"
        '
        'txtBeginDate
        '
        Me.txtBeginDate.AcceptsReturn = True
        Me.txtBeginDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtBeginDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBeginDate.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBeginDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBeginDate.Location = New System.Drawing.Point(79, 3)
        Me.txtBeginDate.MaxLength = 10
        Me.txtBeginDate.Name = "txtBeginDate"
        Me.txtBeginDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBeginDate.Size = New System.Drawing.Size(73, 20)
        Me.txtBeginDate.TabIndex = 10
        Me.txtBeginDate.Text = "12/12/2005"
        '
        'lblIdentifier
        '
        Me.lblIdentifier.BackColor = System.Drawing.SystemColors.Control
        Me.lblIdentifier.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblIdentifier.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIdentifier.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblIdentifier.Location = New System.Drawing.Point(15, 26)
        Me.lblIdentifier.Name = "lblIdentifier"
        Me.lblIdentifier.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblIdentifier.Size = New System.Drawing.Size(57, 17)
        Me.lblIdentifier.TabIndex = 13
        Me.lblIdentifier.Text = "Identifier:  "
        Me.lblIdentifier.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.SystemColors.Control
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(158, 5)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(61, 19)
        Me.Label2.TabIndex = 11
        Me.Label2.Text = "End Date:  "
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(3, 5)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(74, 19)
        Me.Label1.TabIndex = 9
        Me.Label1.Text = "Begin Date:  "
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'cmbVendors
        '
        Me.cmbVendors.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbVendors.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbVendors.BackColor = System.Drawing.SystemColors.Window
        Me.cmbVendors.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbVendors.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbVendors.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbVendors.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbVendors.Location = New System.Drawing.Point(79, 12)
        Me.cmbVendors.Name = "cmbVendors"
        Me.cmbVendors.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbVendors.Size = New System.Drawing.Size(221, 22)
        Me.cmbVendors.Sorted = True
        Me.cmbVendors.TabIndex = 6
        '
        'frmLotNo
        '
        Me.frmLotNo.BackColor = System.Drawing.SystemColors.Control
        Me.frmLotNo.Controls.Add(Me.txtLotNumber)
        Me.frmLotNo.Controls.Add(Me.lblLotNo)
        Me.frmLotNo.Cursor = System.Windows.Forms.Cursors.Default
        Me.frmLotNo.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.frmLotNo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.frmLotNo.Location = New System.Drawing.Point(28, 37)
        Me.frmLotNo.Name = "frmLotNo"
        Me.frmLotNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.frmLotNo.Size = New System.Drawing.Size(277, 57)
        Me.frmLotNo.TabIndex = 3
        '
        'txtLotNumber
        '
        Me.txtLotNumber.AcceptsReturn = True
        Me.txtLotNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtLotNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtLotNumber.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLotNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtLotNumber.Location = New System.Drawing.Point(50, 2)
        Me.txtLotNumber.MaxLength = 12
        Me.txtLotNumber.Name = "txtLotNumber"
        Me.txtLotNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtLotNumber.Size = New System.Drawing.Size(128, 20)
        Me.txtLotNumber.TabIndex = 5
        Me.txtLotNumber.Tag = "STRING"
        '
        'lblLotNo
        '
        Me.lblLotNo.BackColor = System.Drawing.SystemColors.Control
        Me.lblLotNo.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLotNo.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLotNo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLotNo.Location = New System.Drawing.Point(4, 7)
        Me.lblLotNo.Name = "lblLotNo"
        Me.lblLotNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLotNo.Size = New System.Drawing.Size(44, 18)
        Me.lblLotNo.TabIndex = 4
        Me.lblLotNo.Text = "Lot No:  "
        Me.lblLotNo.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'frmRptBy
        '
        Me.frmRptBy.BackColor = System.Drawing.SystemColors.Control
        Me.frmRptBy.Controls.Add(Me._optRptBy_1)
        Me.frmRptBy.Controls.Add(Me._optRptBy_0)
        Me.frmRptBy.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.frmRptBy.ForeColor = System.Drawing.SystemColors.ControlText
        Me.frmRptBy.Location = New System.Drawing.Point(321, 6)
        Me.frmRptBy.Name = "frmRptBy"
        Me.frmRptBy.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.frmRptBy.Size = New System.Drawing.Size(93, 62)
        Me.frmRptBy.TabIndex = 0
        Me.frmRptBy.TabStop = False
        Me.frmRptBy.Text = "Report By"
        '
        '_optRptBy_1
        '
        Me._optRptBy_1.BackColor = System.Drawing.SystemColors.Control
        Me._optRptBy_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._optRptBy_1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optRptBy_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optRptBy.SetIndex(Me._optRptBy_1, CType(1, Short))
        Me._optRptBy_1.Location = New System.Drawing.Point(6, 37)
        Me._optRptBy_1.Name = "_optRptBy_1"
        Me._optRptBy_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optRptBy_1.Size = New System.Drawing.Size(81, 16)
        Me._optRptBy_1.TabIndex = 2
        Me._optRptBy_1.TabStop = True
        Me._optRptBy_1.Text = "Identifier"
        Me._optRptBy_1.UseVisualStyleBackColor = False
        '
        '_optRptBy_0
        '
        Me._optRptBy_0.BackColor = System.Drawing.SystemColors.Control
        Me._optRptBy_0.Checked = True
        Me._optRptBy_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._optRptBy_0.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optRptBy_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optRptBy.SetIndex(Me._optRptBy_0, CType(0, Short))
        Me._optRptBy_0.Location = New System.Drawing.Point(6, 18)
        Me._optRptBy_0.Name = "_optRptBy_0"
        Me._optRptBy_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optRptBy_0.Size = New System.Drawing.Size(81, 21)
        Me._optRptBy_0.TabIndex = 1
        Me._optRptBy_0.TabStop = True
        Me._optRptBy_0.Text = "Lot Number"
        Me._optRptBy_0.UseVisualStyleBackColor = False
        '
        'lblVendor
        '
        Me.lblVendor.BackColor = System.Drawing.SystemColors.Control
        Me.lblVendor.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblVendor.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblVendor.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblVendor.Location = New System.Drawing.Point(23, 13)
        Me.lblVendor.Name = "lblVendor"
        Me.lblVendor.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblVendor.Size = New System.Drawing.Size(52, 17)
        Me.lblVendor.TabIndex = 7
        Me.lblVendor.Text = "Vendor:  "
        Me.lblVendor.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'optRptBy
        '
        '
        'frmLotNoReports
        '
        Me.AcceptButton = Me.cmdReport
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdExit
        Me.ClientSize = New System.Drawing.Size(424, 123)
        Me.Controls.Add(Me.frmLotNo)
        Me.Controls.Add(Me.cmdReport)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.frmIdentifier)
        Me.Controls.Add(Me.cmbVendors)
        Me.Controls.Add(Me.frmRptBy)
        Me.Controls.Add(Me.lblVendor)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Location = New System.Drawing.Point(274, 425)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmLotNoReports"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Lot Number Reports"
        Me.frmIdentifier.ResumeLayout(False)
        Me.frmIdentifier.PerformLayout()
        Me.frmLotNo.ResumeLayout(False)
        Me.frmLotNo.PerformLayout()
        Me.frmRptBy.ResumeLayout(False)
        CType(Me.optRptBy, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class