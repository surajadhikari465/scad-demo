<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmCreditReason
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
	Public WithEvents chkShowStoreCost As System.Windows.Forms.CheckBox
	Public WithEvents chkDetail As System.Windows.Forms.CheckBox
	Public WithEvents chkPrintOnly As System.Windows.Forms.CheckBox
    Public WithEvents cmbVendor As System.Windows.Forms.ComboBox
	Public WithEvents cmdExit As System.Windows.Forms.Button
	Public WithEvents cmdReport As System.Windows.Forms.Button
    Public WithEvents Label2 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_16 As System.Windows.Forms.Label
    Public WithEvents lblLabel As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCreditReason))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdExit = New System.Windows.Forms.Button
        Me.cmdReport = New System.Windows.Forms.Button
        Me.dtpEndDate = New System.Windows.Forms.DateTimePicker
        Me.dtpStartDate = New System.Windows.Forms.DateTimePicker
        Me.chkShowStoreCost = New System.Windows.Forms.CheckBox
        Me.chkDetail = New System.Windows.Forms.CheckBox
        Me.chkPrintOnly = New System.Windows.Forms.CheckBox
        Me.cmbVendor = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me._lblLabel_16 = New System.Windows.Forms.Label
        Me.lblLabel = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(Me.components)
        Me.lblDash = New System.Windows.Forms.Label
        Me.fraStores = New System.Windows.Forms.GroupBox
        Me.cmbZone = New System.Windows.Forms.ComboBox
        Me.optZone = New System.Windows.Forms.RadioButton
        Me.cmbStore = New System.Windows.Forms.ComboBox
        Me.optStore = New System.Windows.Forms.RadioButton
        Me.optRegion = New System.Windows.Forms.RadioButton
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.fraStores.SuspendLayout()
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
        Me.cmdExit.Location = New System.Drawing.Point(258, 230)
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
        Me.cmdReport.Location = New System.Drawing.Point(210, 230)
        Me.cmdReport.Name = "cmdReport"
        Me.cmdReport.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdReport.Size = New System.Drawing.Size(41, 41)
        Me.cmdReport.TabIndex = 4
        Me.cmdReport.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdReport, "Print")
        Me.cmdReport.UseVisualStyleBackColor = False
        '
        'dtpEndDate
        '
        Me.dtpEndDate.CustomFormat = "M/d/yyyy"
        Me.dtpEndDate.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpEndDate.Location = New System.Drawing.Point(199, 170)
        Me.dtpEndDate.Name = "dtpEndDate"
        Me.dtpEndDate.Size = New System.Drawing.Size(80, 20)
        Me.dtpEndDate.TabIndex = 41
        Me.ToolTip1.SetToolTip(Me.dtpEndDate, "End Date")
        Me.dtpEndDate.Value = New Date(2006, 12, 27, 0, 0, 0, 0)
        '
        'dtpStartDate
        '
        Me.dtpStartDate.CustomFormat = "M/d/yyyy"
        Me.dtpStartDate.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpStartDate.Location = New System.Drawing.Point(102, 170)
        Me.dtpStartDate.Name = "dtpStartDate"
        Me.dtpStartDate.Size = New System.Drawing.Size(80, 20)
        Me.dtpStartDate.TabIndex = 40
        Me.ToolTip1.SetToolTip(Me.dtpStartDate, "Start Date")
        Me.dtpStartDate.Value = New Date(2006, 6, 27, 0, 0, 0, 0)
        '
        'chkShowStoreCost
        '
        Me.chkShowStoreCost.BackColor = System.Drawing.SystemColors.Control
        Me.chkShowStoreCost.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkShowStoreCost.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkShowStoreCost.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkShowStoreCost.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkShowStoreCost.Location = New System.Drawing.Point(25, 230)
        Me.chkShowStoreCost.Name = "chkShowStoreCost"
        Me.chkShowStoreCost.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkShowStoreCost.Size = New System.Drawing.Size(157, 25)
        Me.chkShowStoreCost.TabIndex = 16
        Me.chkShowStoreCost.Text = "Show Dist Center Cost"
        Me.chkShowStoreCost.UseVisualStyleBackColor = False
        '
        'chkDetail
        '
        Me.chkDetail.BackColor = System.Drawing.SystemColors.Control
        Me.chkDetail.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkDetail.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkDetail.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDetail.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkDetail.Location = New System.Drawing.Point(85, 211)
        Me.chkDetail.Name = "chkDetail"
        Me.chkDetail.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkDetail.Size = New System.Drawing.Size(97, 17)
        Me.chkDetail.TabIndex = 15
        Me.chkDetail.Text = "Show Detail"
        Me.chkDetail.UseVisualStyleBackColor = False
        '
        'chkPrintOnly
        '
        Me.chkPrintOnly.BackColor = System.Drawing.SystemColors.Control
        Me.chkPrintOnly.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkPrintOnly.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkPrintOnly.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPrintOnly.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkPrintOnly.Location = New System.Drawing.Point(91, 257)
        Me.chkPrintOnly.Name = "chkPrintOnly"
        Me.chkPrintOnly.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkPrintOnly.Size = New System.Drawing.Size(91, 17)
        Me.chkPrintOnly.TabIndex = 11
        Me.chkPrintOnly.Text = "Print Only"
        Me.chkPrintOnly.UseVisualStyleBackColor = False
        '
        'cmbVendor
        '
        Me.cmbVendor.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbVendor.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbVendor.BackColor = System.Drawing.SystemColors.Window
        Me.cmbVendor.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbVendor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbVendor.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbVendor.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbVendor.Location = New System.Drawing.Point(100, 12)
        Me.cmbVendor.Name = "cmbVendor"
        Me.cmbVendor.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbVendor.Size = New System.Drawing.Size(177, 22)
        Me.cmbVendor.Sorted = True
        Me.cmbVendor.TabIndex = 0
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.SystemColors.Control
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(44, 12)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(52, 17)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "Vendor:"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_16
        '
        Me._lblLabel_16.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_16.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_16.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_16.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_16, CType(16, Short))
        Me._lblLabel_16.Location = New System.Drawing.Point(14, 173)
        Me._lblLabel_16.Name = "_lblLabel_16"
        Me._lblLabel_16.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_16.Size = New System.Drawing.Size(81, 17)
        Me._lblLabel_16.TabIndex = 7
        Me._lblLabel_16.Text = "Date Range :"
        Me._lblLabel_16.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblDash
        '
        Me.lblDash.BackColor = System.Drawing.Color.Transparent
        Me.lblDash.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDash.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold)
        Me.lblDash.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDash.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblDash.Location = New System.Drawing.Point(182, 169)
        Me.lblDash.Name = "lblDash"
        Me.lblDash.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDash.Size = New System.Drawing.Size(17, 17)
        Me.lblDash.TabIndex = 39
        Me.lblDash.Text = "-"
        Me.lblDash.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'fraStores
        '
        Me.fraStores.BackColor = System.Drawing.SystemColors.Control
        Me.fraStores.Controls.Add(Me.cmbZone)
        Me.fraStores.Controls.Add(Me.optZone)
        Me.fraStores.Controls.Add(Me.cmbStore)
        Me.fraStores.Controls.Add(Me.optStore)
        Me.fraStores.Controls.Add(Me.optRegion)
        Me.fraStores.Font = New System.Drawing.Font("Arial", 8.0!)
        Me.fraStores.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraStores.Location = New System.Drawing.Point(11, 48)
        Me.fraStores.Name = "fraStores"
        Me.fraStores.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraStores.Size = New System.Drawing.Size(288, 101)
        Me.fraStores.TabIndex = 42
        Me.fraStores.TabStop = False
        '
        'cmbZone
        '
        Me.cmbZone.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbZone.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbZone.BackColor = System.Drawing.SystemColors.Window
        Me.cmbZone.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbZone.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbZone.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.cmbZone.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbZone.Location = New System.Drawing.Point(91, 44)
        Me.cmbZone.Name = "cmbZone"
        Me.cmbZone.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbZone.Size = New System.Drawing.Size(177, 22)
        Me.cmbZone.Sorted = True
        Me.cmbZone.TabIndex = 9
        '
        'optZone
        '
        Me.optZone.BackColor = System.Drawing.SystemColors.Control
        Me.optZone.Cursor = System.Windows.Forms.Cursors.Default
        Me.optZone.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.optZone.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optZone.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.optZone.Location = New System.Drawing.Point(8, 46)
        Me.optZone.Name = "optZone"
        Me.optZone.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optZone.Size = New System.Drawing.Size(65, 17)
        Me.optZone.TabIndex = 8
        Me.optZone.TabStop = True
        Me.optZone.Text = "Zone"
        Me.optZone.UseVisualStyleBackColor = False
        '
        'cmbStore
        '
        Me.cmbStore.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbStore.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbStore.BackColor = System.Drawing.SystemColors.Window
        Me.cmbStore.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbStore.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbStore.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.cmbStore.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbStore.Location = New System.Drawing.Point(91, 16)
        Me.cmbStore.Name = "cmbStore"
        Me.cmbStore.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbStore.Size = New System.Drawing.Size(177, 22)
        Me.cmbStore.Sorted = True
        Me.cmbStore.TabIndex = 7
        '
        'optStore
        '
        Me.optStore.BackColor = System.Drawing.SystemColors.Control
        Me.optStore.Checked = True
        Me.optStore.Cursor = System.Windows.Forms.Cursors.Default
        Me.optStore.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.optStore.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optStore.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.optStore.Location = New System.Drawing.Point(8, 10)
        Me.optStore.Name = "optStore"
        Me.optStore.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optStore.Size = New System.Drawing.Size(78, 32)
        Me.optStore.TabIndex = 6
        Me.optStore.TabStop = True
        Me.optStore.Text = "Receiving Store"
        Me.optStore.UseVisualStyleBackColor = False
        '
        'optRegion
        '
        Me.optRegion.BackColor = System.Drawing.SystemColors.Control
        Me.optRegion.Cursor = System.Windows.Forms.Cursors.Default
        Me.optRegion.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.optRegion.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optRegion.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.optRegion.Location = New System.Drawing.Point(8, 75)
        Me.optRegion.Name = "optRegion"
        Me.optRegion.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optRegion.Size = New System.Drawing.Size(65, 17)
        Me.optRegion.TabIndex = 10
        Me.optRegion.TabStop = True
        Me.optRegion.Text = "Region"
        Me.optRegion.UseVisualStyleBackColor = False
        '
        'frmCreditReason
        '
        Me.AcceptButton = Me.cmdReport
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdExit
        Me.ClientSize = New System.Drawing.Size(309, 283)
        Me.Controls.Add(Me.fraStores)
        Me.Controls.Add(Me.lblDash)
        Me.Controls.Add(Me.dtpEndDate)
        Me.Controls.Add(Me.dtpStartDate)
        Me.Controls.Add(Me.chkShowStoreCost)
        Me.Controls.Add(Me.chkDetail)
        Me.Controls.Add(Me.chkPrintOnly)
        Me.Controls.Add(Me.cmbVendor)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.cmdReport)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me._lblLabel_16)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Location = New System.Drawing.Point(3, 29)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmCreditReason"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.Text = "Credit Reason"
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).EndInit()
        Me.fraStores.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Public WithEvents lblDash As System.Windows.Forms.Label
    Friend WithEvents dtpEndDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtpStartDate As System.Windows.Forms.DateTimePicker
    Public WithEvents fraStores As System.Windows.Forms.GroupBox
    Public WithEvents cmbZone As System.Windows.Forms.ComboBox
    Public WithEvents optZone As System.Windows.Forms.RadioButton
    Public WithEvents cmbStore As System.Windows.Forms.ComboBox
    Public WithEvents optStore As System.Windows.Forms.RadioButton
    Public WithEvents optRegion As System.Windows.Forms.RadioButton
#End Region 
End Class