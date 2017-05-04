<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmAPUploadReports
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
	Public WithEvents cmbStore As System.Windows.Forms.ComboBox
    Public WithEvents _optReport_4 As System.Windows.Forms.RadioButton
	Public WithEvents _optReport_3 As System.Windows.Forms.RadioButton
	Public WithEvents _optReport_2 As System.Windows.Forms.RadioButton
	Public WithEvents _optReport_5 As System.Windows.Forms.RadioButton
	Public WithEvents _optReport_1 As System.Windows.Forms.RadioButton
	Public WithEvents _optReport_0 As System.Windows.Forms.RadioButton
	Public WithEvents fraReport As System.Windows.Forms.GroupBox
	Public WithEvents chkPrintOnly As System.Windows.Forms.CheckBox
	Public WithEvents cmdReport As System.Windows.Forms.Button
	Public WithEvents cmdExit As System.Windows.Forms.Button
    Public WithEvents _lblLabel_1 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_0 As System.Windows.Forms.Label
	Public WithEvents lblDates As System.Windows.Forms.Label
	Public WithEvents lblLabel As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	Public WithEvents optReport As Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray
	Public WithEvents txtDate As Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAPUploadReports))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdReport = New System.Windows.Forms.Button()
        Me.cmdExit = New System.Windows.Forms.Button()
        Me.dtpEndDate = New System.Windows.Forms.DateTimePicker()
        Me.dtpStartDate = New System.Windows.Forms.DateTimePicker()
        Me.cmbStore = New System.Windows.Forms.ComboBox()
        Me.fraReport = New System.Windows.Forms.GroupBox()
        Me._optReport_4 = New System.Windows.Forms.RadioButton()
        Me._optReport_3 = New System.Windows.Forms.RadioButton()
        Me._optReport_2 = New System.Windows.Forms.RadioButton()
        Me._optReport_5 = New System.Windows.Forms.RadioButton()
        Me._optReport_1 = New System.Windows.Forms.RadioButton()
        Me._optReport_0 = New System.Windows.Forms.RadioButton()
        Me.chkPrintOnly = New System.Windows.Forms.CheckBox()
        Me._lblLabel_1 = New System.Windows.Forms.Label()
        Me._lblLabel_0 = New System.Windows.Forms.Label()
        Me.lblDates = New System.Windows.Forms.Label()
        Me.lblLabel = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(Me.components)
        Me.optReport = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(Me.components)
        Me.txtDate = New Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray(Me.components)
        Me.cdbSave = New System.Windows.Forms.SaveFileDialog()
        Me.lblDash = New System.Windows.Forms.Label()
        Me.fraReport.SuspendLayout()
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.optReport, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtDate, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdReport
        '
        Me.cmdReport.BackColor = System.Drawing.SystemColors.Control
        Me.cmdReport.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdReport.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdReport.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdReport.Image = CType(resources.GetObject("cmdReport.Image"), System.Drawing.Image)
        Me.cmdReport.Location = New System.Drawing.Point(188, 284)
        Me.cmdReport.Name = "cmdReport"
        Me.cmdReport.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdReport.Size = New System.Drawing.Size(41, 41)
        Me.cmdReport.TabIndex = 12
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
        Me.cmdExit.Location = New System.Drawing.Point(236, 284)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(41, 41)
        Me.cmdExit.TabIndex = 13
        Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdExit, "Exit")
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'dtpEndDate
        '
        Me.dtpEndDate.CustomFormat = ""
        Me.dtpEndDate.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpEndDate.Location = New System.Drawing.Point(201, 193)
        Me.dtpEndDate.Name = "dtpEndDate"
        Me.dtpEndDate.Size = New System.Drawing.Size(80, 20)
        Me.dtpEndDate.TabIndex = 53
        Me.ToolTip1.SetToolTip(Me.dtpEndDate, "End Date")
        Me.dtpEndDate.Value = New Date(2009, 8, 6, 0, 0, 0, 0)
        '
        'dtpStartDate
        '
        Me.dtpStartDate.CustomFormat = ""
        Me.dtpStartDate.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpStartDate.Location = New System.Drawing.Point(100, 193)
        Me.dtpStartDate.Name = "dtpStartDate"
        Me.dtpStartDate.Size = New System.Drawing.Size(80, 20)
        Me.dtpStartDate.TabIndex = 52
        Me.ToolTip1.SetToolTip(Me.dtpStartDate, "Start Date")
        Me.dtpStartDate.Value = New Date(2009, 8, 6, 0, 0, 0, 0)
        '
        'cmbStore
        '
        Me.cmbStore.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbStore.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbStore.BackColor = System.Drawing.SystemColors.Window
        Me.cmbStore.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbStore.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbStore.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbStore.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbStore.Location = New System.Drawing.Point(100, 228)
        Me.cmbStore.Name = "cmbStore"
        Me.cmbStore.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbStore.Size = New System.Drawing.Size(177, 22)
        Me.cmbStore.Sorted = True
        Me.cmbStore.TabIndex = 10
        '
        'fraReport
        '
        Me.fraReport.BackColor = System.Drawing.SystemColors.Control
        Me.fraReport.Controls.Add(Me._optReport_4)
        Me.fraReport.Controls.Add(Me._optReport_3)
        Me.fraReport.Controls.Add(Me._optReport_2)
        Me.fraReport.Controls.Add(Me._optReport_5)
        Me.fraReport.Controls.Add(Me._optReport_1)
        Me.fraReport.Controls.Add(Me._optReport_0)
        Me.fraReport.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraReport.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraReport.Location = New System.Drawing.Point(8, 8)
        Me.fraReport.Name = "fraReport"
        Me.fraReport.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraReport.Size = New System.Drawing.Size(273, 173)
        Me.fraReport.TabIndex = 0
        Me.fraReport.TabStop = False
        Me.fraReport.Text = "Report"
        '
        '_optReport_4
        '
        Me._optReport_4.BackColor = System.Drawing.SystemColors.Control
        Me._optReport_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._optReport_4.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optReport_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optReport.SetIndex(Me._optReport_4, CType(4, Short))
        Me._optReport_4.Location = New System.Drawing.Point(24, 112)
        Me._optReport_4.Name = "_optReport_4"
        Me._optReport_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optReport_4.Size = New System.Drawing.Size(169, 18)
        Me._optReport_4.TabIndex = 5
        Me._optReport_4.TabStop = True
        Me._optReport_4.Text = "No Invoice Cost"
        Me._optReport_4.UseVisualStyleBackColor = False
        '
        '_optReport_3
        '
        Me._optReport_3.BackColor = System.Drawing.SystemColors.Control
        Me._optReport_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._optReport_3.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optReport_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optReport.SetIndex(Me._optReport_3, CType(3, Short))
        Me._optReport_3.Location = New System.Drawing.Point(24, 88)
        Me._optReport_3.Name = "_optReport_3"
        Me._optReport_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optReport_3.Size = New System.Drawing.Size(169, 18)
        Me._optReport_3.TabIndex = 4
        Me._optReport_3.TabStop = True
        Me._optReport_3.Text = "Missing PS Info"
        Me._optReport_3.UseVisualStyleBackColor = False
        '
        '_optReport_2
        '
        Me._optReport_2.BackColor = System.Drawing.SystemColors.Control
        Me._optReport_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._optReport_2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optReport_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optReport.SetIndex(Me._optReport_2, CType(2, Short))
        Me._optReport_2.Location = New System.Drawing.Point(24, 64)
        Me._optReport_2.Name = "_optReport_2"
        Me._optReport_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optReport_2.Size = New System.Drawing.Size(169, 18)
        Me._optReport_2.TabIndex = 3
        Me._optReport_2.TabStop = True
        Me._optReport_2.Text = "Cost Mismatch"
        Me._optReport_2.UseVisualStyleBackColor = False
        '
        '_optReport_5
        '
        Me._optReport_5.BackColor = System.Drawing.SystemColors.Control
        Me._optReport_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._optReport_5.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optReport_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optReport.SetIndex(Me._optReport_5, CType(5, Short))
        Me._optReport_5.Location = New System.Drawing.Point(24, 136)
        Me._optReport_5.Name = "_optReport_5"
        Me._optReport_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optReport_5.Size = New System.Drawing.Size(169, 18)
        Me._optReport_5.TabIndex = 6
        Me._optReport_5.TabStop = True
        Me._optReport_5.Text = "Uploaded"
        Me._optReport_5.UseVisualStyleBackColor = False
        '
        '_optReport_1
        '
        Me._optReport_1.BackColor = System.Drawing.SystemColors.Control
        Me._optReport_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._optReport_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optReport_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optReport.SetIndex(Me._optReport_1, CType(1, Short))
        Me._optReport_1.Location = New System.Drawing.Point(24, 40)
        Me._optReport_1.Name = "_optReport_1"
        Me._optReport_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optReport_1.Size = New System.Drawing.Size(169, 18)
        Me._optReport_1.TabIndex = 2
        Me._optReport_1.TabStop = True
        Me._optReport_1.Text = "All Exceptions"
        Me._optReport_1.UseVisualStyleBackColor = False
        '
        '_optReport_0
        '
        Me._optReport_0.BackColor = System.Drawing.SystemColors.Control
        Me._optReport_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._optReport_0.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optReport_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optReport.SetIndex(Me._optReport_0, CType(0, Short))
        Me._optReport_0.Location = New System.Drawing.Point(24, 16)
        Me._optReport_0.Name = "_optReport_0"
        Me._optReport_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optReport_0.Size = New System.Drawing.Size(169, 18)
        Me._optReport_0.TabIndex = 1
        Me._optReport_0.TabStop = True
        Me._optReport_0.Text = "Approved for Upload"
        Me._optReport_0.UseVisualStyleBackColor = False
        '
        'chkPrintOnly
        '
        Me.chkPrintOnly.BackColor = System.Drawing.SystemColors.Control
        Me.chkPrintOnly.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkPrintOnly.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPrintOnly.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkPrintOnly.Location = New System.Drawing.Point(188, 260)
        Me.chkPrintOnly.Name = "chkPrintOnly"
        Me.chkPrintOnly.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkPrintOnly.Size = New System.Drawing.Size(81, 17)
        Me.chkPrintOnly.TabIndex = 11
        Me.chkPrintOnly.Text = "Print Only"
        Me.chkPrintOnly.UseVisualStyleBackColor = False
        '
        '_lblLabel_1
        '
        Me._lblLabel_1.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_1, CType(1, Short))
        Me._lblLabel_1.Location = New System.Drawing.Point(4, 228)
        Me._lblLabel_1.Name = "_lblLabel_1"
        Me._lblLabel_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_1.Size = New System.Drawing.Size(89, 17)
        Me._lblLabel_1.TabIndex = 16
        Me._lblLabel_1.Text = "Store :"
        Me._lblLabel_1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_0
        '
        Me._lblLabel_0.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_0.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_0, CType(0, Short))
        Me._lblLabel_0.Location = New System.Drawing.Point(184, 224)
        Me._lblLabel_0.Name = "_lblLabel_0"
        Me._lblLabel_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_0.Size = New System.Drawing.Size(17, 17)
        Me._lblLabel_0.TabIndex = 15
        Me._lblLabel_0.Text = "-"
        Me._lblLabel_0.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblDates
        '
        Me.lblDates.BackColor = System.Drawing.Color.Transparent
        Me.lblDates.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDates.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDates.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDates.Location = New System.Drawing.Point(4, 196)
        Me.lblDates.Name = "lblDates"
        Me.lblDates.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDates.Size = New System.Drawing.Size(89, 33)
        Me.lblDates.TabIndex = 14
        Me.lblDates.Text = "Date Range :"
        Me.lblDates.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'txtDate
        '
        '
        'lblDash
        '
        Me.lblDash.BackColor = System.Drawing.Color.Transparent
        Me.lblDash.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDash.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold)
        Me.lblDash.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDash.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblDash.Location = New System.Drawing.Point(184, 193)
        Me.lblDash.Name = "lblDash"
        Me.lblDash.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDash.Size = New System.Drawing.Size(17, 17)
        Me.lblDash.TabIndex = 51
        Me.lblDash.Text = "-"
        Me.lblDash.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'frmAPUploadReports
        '
        Me.AcceptButton = Me.cmdReport
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdExit
        Me.ClientSize = New System.Drawing.Size(291, 341)
        Me.Controls.Add(Me.lblDash)
        Me.Controls.Add(Me.dtpEndDate)
        Me.Controls.Add(Me.dtpStartDate)
        Me.Controls.Add(Me.cmbStore)
        Me.Controls.Add(Me.fraReport)
        Me.Controls.Add(Me.chkPrintOnly)
        Me.Controls.Add(Me.cmdReport)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me._lblLabel_1)
        Me.Controls.Add(Me._lblLabel_0)
        Me.Controls.Add(Me.lblDates)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Location = New System.Drawing.Point(3, 29)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmAPUploadReports"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Order AP Upload Reports"
        Me.fraReport.ResumeLayout(False)
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.optReport, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtDate, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents cdbSave As System.Windows.Forms.SaveFileDialog
    Public WithEvents lblDash As System.Windows.Forms.Label
    Friend WithEvents dtpEndDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtpStartDate As System.Windows.Forms.DateTimePicker
#End Region 
End Class