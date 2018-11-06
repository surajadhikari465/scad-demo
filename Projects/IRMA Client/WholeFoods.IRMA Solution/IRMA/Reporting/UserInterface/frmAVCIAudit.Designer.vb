<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmAVCIAudit
#Region "Windows Form Designer generated code "
	<System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
        MyBase.New()
        IsInitializing = True
		'This call is required by the Windows Form Designer.
        InitializeComponent()
        IsInitializing = False
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
	Public WithEvents cmdFind As System.Windows.Forms.Button
    Public WithEvents txtVend As System.Windows.Forms.TextBox
	Public WithEvents optSubTeam As System.Windows.Forms.RadioButton
	Public WithEvents optTeam As System.Windows.Forms.RadioButton
	Public WithEvents cmbTeam As System.Windows.Forms.ComboBox
	Public WithEvents cmbExType As System.Windows.Forms.ComboBox
	Public WithEvents cmbSubTeam As System.Windows.Forms.ComboBox
	Public WithEvents cmbStatus As System.Windows.Forms.ComboBox
	Public WithEvents cmdReport As System.Windows.Forms.Button
	Public WithEvents cmdExit As System.Windows.Forms.Button
	Public WithEvents chkPrintOnly As System.Windows.Forms.CheckBox
    Public WithEvents lblVend As System.Windows.Forms.Label
	Public WithEvents lblBetween As System.Windows.Forms.Label
	Public WithEvents lblDateRange As System.Windows.Forms.Label
	Public WithEvents Label2 As System.Windows.Forms.Label
	Public WithEvents Label4 As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAVCIAudit))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdReport = New System.Windows.Forms.Button
        Me.cmdExit = New System.Windows.Forms.Button
        Me.dtpEndDate = New System.Windows.Forms.DateTimePicker
        Me.dtpStartDate = New System.Windows.Forms.DateTimePicker
        Me.cmdFind = New System.Windows.Forms.Button
        Me.txtVend = New System.Windows.Forms.TextBox
        Me.optSubTeam = New System.Windows.Forms.RadioButton
        Me.optTeam = New System.Windows.Forms.RadioButton
        Me.cmbTeam = New System.Windows.Forms.ComboBox
        Me.cmbExType = New System.Windows.Forms.ComboBox
        Me.cmbSubTeam = New System.Windows.Forms.ComboBox
        Me.cmbStatus = New System.Windows.Forms.ComboBox
        Me.chkPrintOnly = New System.Windows.Forms.CheckBox
        Me.lblVend = New System.Windows.Forms.Label
        Me.lblBetween = New System.Windows.Forms.Label
        Me.lblDateRange = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.lblDash = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'cmdReport
        '
        Me.cmdReport.BackColor = System.Drawing.SystemColors.Control
        Me.cmdReport.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdReport.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdReport.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdReport.Image = CType(resources.GetObject("cmdReport.Image"), System.Drawing.Image)
        Me.cmdReport.Location = New System.Drawing.Point(299, 156)
        Me.cmdReport.Name = "cmdReport"
        Me.cmdReport.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdReport.Size = New System.Drawing.Size(41, 41)
        Me.cmdReport.TabIndex = 2
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
        Me.cmdExit.Location = New System.Drawing.Point(348, 156)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(41, 41)
        Me.cmdExit.TabIndex = 1
        Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdExit, "Exit")
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'dtpEndDate
        '
        Me.dtpEndDate.Checked = False
        Me.dtpEndDate.CustomFormat = "M/d/yyyy"
        Me.dtpEndDate.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpEndDate.Location = New System.Drawing.Point(196, 103)
        Me.dtpEndDate.Name = "dtpEndDate"
        Me.dtpEndDate.ShowCheckBox = True
        Me.dtpEndDate.Size = New System.Drawing.Size(90, 20)
        Me.dtpEndDate.TabIndex = 56
        Me.ToolTip1.SetToolTip(Me.dtpEndDate, "End Date")
        Me.dtpEndDate.Value = New Date(2006, 12, 27, 0, 0, 0, 0)
        '
        'dtpStartDate
        '
        Me.dtpStartDate.Checked = False
        Me.dtpStartDate.CustomFormat = "M/d/yyyy"
        Me.dtpStartDate.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpStartDate.Location = New System.Drawing.Point(89, 103)
        Me.dtpStartDate.Name = "dtpStartDate"
        Me.dtpStartDate.ShowCheckBox = True
        Me.dtpStartDate.Size = New System.Drawing.Size(90, 20)
        Me.dtpStartDate.TabIndex = 55
        Me.ToolTip1.SetToolTip(Me.dtpStartDate, "Start Date")
        Me.dtpStartDate.Value = New Date(2006, 6, 27, 0, 0, 0, 0)
        '
        'cmdFind
        '
        Me.cmdFind.BackColor = System.Drawing.SystemColors.Control
        Me.cmdFind.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdFind.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdFind.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdFind.Image = CType(resources.GetObject("cmdFind.Image"), System.Drawing.Image)
        Me.cmdFind.Location = New System.Drawing.Point(364, 8)
        Me.cmdFind.Name = "cmdFind"
        Me.cmdFind.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdFind.Size = New System.Drawing.Size(26, 22)
        Me.cmdFind.TabIndex = 16
        Me.cmdFind.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.cmdFind.UseVisualStyleBackColor = False
        '
        'txtVend
        '
        Me.txtVend.AcceptsReturn = True
        Me.txtVend.BackColor = System.Drawing.SystemColors.Window
        Me.txtVend.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtVend.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtVend.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtVend.Location = New System.Drawing.Point(89, 9)
        Me.txtVend.MaxLength = 0
        Me.txtVend.Name = "txtVend"
        Me.txtVend.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtVend.Size = New System.Drawing.Size(275, 20)
        Me.txtVend.TabIndex = 11
        '
        'optSubTeam
        '
        Me.optSubTeam.BackColor = System.Drawing.SystemColors.Control
        Me.optSubTeam.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.optSubTeam.Cursor = System.Windows.Forms.Cursors.Default
        Me.optSubTeam.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optSubTeam.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optSubTeam.Location = New System.Drawing.Point(6, 51)
        Me.optSubTeam.Name = "optSubTeam"
        Me.optSubTeam.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optSubTeam.Size = New System.Drawing.Size(77, 18)
        Me.optSubTeam.TabIndex = 10
        Me.optSubTeam.TabStop = True
        Me.optSubTeam.Text = "SubTeam"
        Me.optSubTeam.UseVisualStyleBackColor = False
        '
        'optTeam
        '
        Me.optTeam.BackColor = System.Drawing.SystemColors.Control
        Me.optTeam.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.optTeam.Checked = True
        Me.optTeam.Cursor = System.Windows.Forms.Cursors.Default
        Me.optTeam.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optTeam.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optTeam.Location = New System.Drawing.Point(27, 34)
        Me.optTeam.Name = "optTeam"
        Me.optTeam.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optTeam.Size = New System.Drawing.Size(56, 21)
        Me.optTeam.TabIndex = 9
        Me.optTeam.TabStop = True
        Me.optTeam.Text = "Team"
        Me.optTeam.UseVisualStyleBackColor = False
        '
        'cmbTeam
        '
        Me.cmbTeam.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbTeam.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbTeam.BackColor = System.Drawing.SystemColors.Window
        Me.cmbTeam.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbTeam.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTeam.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbTeam.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbTeam.Location = New System.Drawing.Point(89, 42)
        Me.cmbTeam.Name = "cmbTeam"
        Me.cmbTeam.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbTeam.Size = New System.Drawing.Size(197, 22)
        Me.cmbTeam.Sorted = True
        Me.cmbTeam.TabIndex = 8
        '
        'cmbExType
        '
        Me.cmbExType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbExType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbExType.BackColor = System.Drawing.SystemColors.Window
        Me.cmbExType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbExType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbExType.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbExType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbExType.Location = New System.Drawing.Point(89, 73)
        Me.cmbExType.Name = "cmbExType"
        Me.cmbExType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbExType.Size = New System.Drawing.Size(197, 22)
        Me.cmbExType.Sorted = True
        Me.cmbExType.TabIndex = 5
        '
        'cmbSubTeam
        '
        Me.cmbSubTeam.BackColor = System.Drawing.SystemColors.Window
        Me.cmbSubTeam.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbSubTeam.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbSubTeam.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbSubTeam.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbSubTeam.Location = New System.Drawing.Point(89, 42)
        Me.cmbSubTeam.Name = "cmbSubTeam"
        Me.cmbSubTeam.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbSubTeam.Size = New System.Drawing.Size(167, 22)
        Me.cmbSubTeam.Sorted = True
        Me.cmbSubTeam.TabIndex = 4
        '
        'cmbStatus
        '
        Me.cmbStatus.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbStatus.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbStatus.BackColor = System.Drawing.SystemColors.Window
        Me.cmbStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbStatus.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbStatus.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbStatus.Location = New System.Drawing.Point(89, 130)
        Me.cmbStatus.Name = "cmbStatus"
        Me.cmbStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbStatus.Size = New System.Drawing.Size(197, 22)
        Me.cmbStatus.TabIndex = 3
        '
        'chkPrintOnly
        '
        Me.chkPrintOnly.BackColor = System.Drawing.SystemColors.Control
        Me.chkPrintOnly.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkPrintOnly.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkPrintOnly.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPrintOnly.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkPrintOnly.Location = New System.Drawing.Point(203, 174)
        Me.chkPrintOnly.Name = "chkPrintOnly"
        Me.chkPrintOnly.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkPrintOnly.Size = New System.Drawing.Size(83, 17)
        Me.chkPrintOnly.TabIndex = 0
        Me.chkPrintOnly.Text = "Print Only"
        Me.chkPrintOnly.UseVisualStyleBackColor = False
        '
        'lblVend
        '
        Me.lblVend.BackColor = System.Drawing.SystemColors.Control
        Me.lblVend.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblVend.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblVend.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblVend.Location = New System.Drawing.Point(32, 13)
        Me.lblVend.Name = "lblVend"
        Me.lblVend.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblVend.Size = New System.Drawing.Size(55, 16)
        Me.lblVend.TabIndex = 17
        Me.lblVend.Text = "Vendor :"
        '
        'lblBetween
        '
        Me.lblBetween.BackColor = System.Drawing.SystemColors.Control
        Me.lblBetween.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBetween.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBetween.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBetween.Location = New System.Drawing.Point(430, 44)
        Me.lblBetween.Name = "lblBetween"
        Me.lblBetween.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBetween.Size = New System.Drawing.Size(7, 16)
        Me.lblBetween.TabIndex = 15
        Me.lblBetween.Text = "-"
        '
        'lblDateRange
        '
        Me.lblDateRange.BackColor = System.Drawing.SystemColors.Control
        Me.lblDateRange.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDateRange.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDateRange.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDateRange.Location = New System.Drawing.Point(-1, 106)
        Me.lblDateRange.Name = "lblDateRange"
        Me.lblDateRange.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDateRange.Size = New System.Drawing.Size(88, 18)
        Me.lblDateRange.TabIndex = 14
        Me.lblDateRange.Text = "Insert Date :"
        Me.lblDateRange.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.SystemColors.Control
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(-2, 71)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(87, 28)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "Exception Type :"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.SystemColors.Control
        Me.Label4.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label4.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label4.Location = New System.Drawing.Point(21, 134)
        Me.Label4.Name = "Label4"
        Me.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label4.Size = New System.Drawing.Size(66, 18)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Status :"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblDash
        '
        Me.lblDash.BackColor = System.Drawing.Color.Transparent
        Me.lblDash.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDash.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold)
        Me.lblDash.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDash.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblDash.Location = New System.Drawing.Point(179, 102)
        Me.lblDash.Name = "lblDash"
        Me.lblDash.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDash.Size = New System.Drawing.Size(17, 17)
        Me.lblDash.TabIndex = 54
        Me.lblDash.Text = "-"
        Me.lblDash.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'frmAVCIAudit
        '
        Me.AcceptButton = Me.cmdFind
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdExit
        Me.ClientSize = New System.Drawing.Size(398, 205)
        Me.Controls.Add(Me.lblDash)
        Me.Controls.Add(Me.dtpEndDate)
        Me.Controls.Add(Me.dtpStartDate)
        Me.Controls.Add(Me.cmdFind)
        Me.Controls.Add(Me.txtVend)
        Me.Controls.Add(Me.optSubTeam)
        Me.Controls.Add(Me.optTeam)
        Me.Controls.Add(Me.cmbTeam)
        Me.Controls.Add(Me.cmbExType)
        Me.Controls.Add(Me.cmbSubTeam)
        Me.Controls.Add(Me.cmbStatus)
        Me.Controls.Add(Me.cmdReport)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.chkPrintOnly)
        Me.Controls.Add(Me.lblVend)
        Me.Controls.Add(Me.lblBetween)
        Me.Controls.Add(Me.lblDateRange)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label4)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Location = New System.Drawing.Point(135, 332)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmAVCIAudit"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "AVCI Exception Auditing"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Public WithEvents lblDash As System.Windows.Forms.Label
    Friend WithEvents dtpEndDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtpStartDate As System.Windows.Forms.DateTimePicker
#End Region
End Class