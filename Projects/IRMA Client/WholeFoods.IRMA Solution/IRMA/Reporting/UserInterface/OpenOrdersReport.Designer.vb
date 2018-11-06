<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmOpenOrdersReport
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
	Public WithEvents cmbUser As System.Windows.Forms.ComboBox
	Public WithEvents _optRptType_2 As System.Windows.Forms.RadioButton
	Public WithEvents _optRptType_1 As System.Windows.Forms.RadioButton
	Public WithEvents _optRptType_0 As System.Windows.Forms.RadioButton
    Public WithEvents chkDetail As System.Windows.Forms.CheckBox
	Public WithEvents cmbSubTeam As System.Windows.Forms.ComboBox
	Public WithEvents cmdReport As System.Windows.Forms.Button
	Public WithEvents cmdExit As System.Windows.Forms.Button
    Public WithEvents cmbStore As System.Windows.Forms.ComboBox
    Public WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents _lblLabel_16 As System.Windows.Forms.Label
    Public WithEvents _lblLabel_2 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_0 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_5 As System.Windows.Forms.Label
	Public WithEvents lblLabel As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	Public WithEvents optRptType As Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray
	Public WithEvents txtField As Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmOpenOrdersReport))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdReport = New System.Windows.Forms.Button()
        Me.cmdExit = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.dtpOrderEndDate = New System.Windows.Forms.DateTimePicker()
        Me.dtpOrderStartDate = New System.Windows.Forms.DateTimePicker()
        Me.dtpExpectedEndDate = New System.Windows.Forms.DateTimePicker()
        Me.dtpExpectedStartDate = New System.Windows.Forms.DateTimePicker()
        Me.cmbUser = New System.Windows.Forms.ComboBox()
        Me._optRptType_2 = New System.Windows.Forms.RadioButton()
        Me._optRptType_1 = New System.Windows.Forms.RadioButton()
        Me._optRptType_0 = New System.Windows.Forms.RadioButton()
        Me.chkDetail = New System.Windows.Forms.CheckBox()
        Me.cmbSubTeam = New System.Windows.Forms.ComboBox()
        Me.cmbStore = New System.Windows.Forms.ComboBox()
        Me._lblLabel_16 = New System.Windows.Forms.Label()
        Me._lblLabel_2 = New System.Windows.Forms.Label()
        Me._lblLabel_0 = New System.Windows.Forms.Label()
        Me._lblLabel_5 = New System.Windows.Forms.Label()
        Me.lblLabel = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(Me.components)
        Me.optRptType = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(Me.components)
        Me.txtField = New Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray(Me.components)
        Me.lblDash = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cmbVendor = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.chkPreOrder = New System.Windows.Forms.CheckBox()
        Me.chkIncludeBlankPOs = New System.Windows.Forms.CheckBox()
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.optRptType, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtField, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdReport
        '
        Me.cmdReport.BackColor = System.Drawing.SystemColors.Control
        Me.cmdReport.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdReport, "cmdReport")
        Me.cmdReport.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdReport.Name = "cmdReport"
        Me.ToolTip1.SetToolTip(Me.cmdReport, resources.GetString("cmdReport.ToolTip"))
        Me.cmdReport.UseVisualStyleBackColor = False
        '
        'cmdExit
        '
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        resources.ApplyResources(Me.cmdExit, "cmdExit")
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Name = "cmdExit"
        Me.ToolTip1.SetToolTip(Me.cmdExit, resources.GetString("cmdExit.ToolTip"))
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Name = "Label1"
        Me.ToolTip1.SetToolTip(Me.Label1, resources.GetString("Label1.ToolTip"))
        '
        'dtpOrderEndDate
        '
        Me.dtpOrderEndDate.Checked = False
        resources.ApplyResources(Me.dtpOrderEndDate, "dtpOrderEndDate")
        Me.dtpOrderEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpOrderEndDate.Name = "dtpOrderEndDate"
        Me.dtpOrderEndDate.ShowCheckBox = True
        Me.ToolTip1.SetToolTip(Me.dtpOrderEndDate, resources.GetString("dtpOrderEndDate.ToolTip"))
        Me.dtpOrderEndDate.Value = New Date(2006, 12, 27, 0, 0, 0, 0)
        '
        'dtpOrderStartDate
        '
        Me.dtpOrderStartDate.Checked = False
        resources.ApplyResources(Me.dtpOrderStartDate, "dtpOrderStartDate")
        Me.dtpOrderStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpOrderStartDate.Name = "dtpOrderStartDate"
        Me.dtpOrderStartDate.ShowCheckBox = True
        Me.ToolTip1.SetToolTip(Me.dtpOrderStartDate, resources.GetString("dtpOrderStartDate.ToolTip"))
        Me.dtpOrderStartDate.Value = New Date(2006, 6, 27, 0, 0, 0, 0)
        '
        'dtpExpectedEndDate
        '
        Me.dtpExpectedEndDate.Checked = False
        resources.ApplyResources(Me.dtpExpectedEndDate, "dtpExpectedEndDate")
        Me.dtpExpectedEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpExpectedEndDate.Name = "dtpExpectedEndDate"
        Me.dtpExpectedEndDate.ShowCheckBox = True
        Me.ToolTip1.SetToolTip(Me.dtpExpectedEndDate, resources.GetString("dtpExpectedEndDate.ToolTip"))
        Me.dtpExpectedEndDate.Value = New Date(2006, 12, 27, 0, 0, 0, 0)
        '
        'dtpExpectedStartDate
        '
        Me.dtpExpectedStartDate.Checked = False
        resources.ApplyResources(Me.dtpExpectedStartDate, "dtpExpectedStartDate")
        Me.dtpExpectedStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpExpectedStartDate.Name = "dtpExpectedStartDate"
        Me.dtpExpectedStartDate.ShowCheckBox = True
        Me.ToolTip1.SetToolTip(Me.dtpExpectedStartDate, resources.GetString("dtpExpectedStartDate.ToolTip"))
        Me.dtpExpectedStartDate.Value = New Date(2006, 6, 27, 0, 0, 0, 0)
        '
        'cmbUser
        '
        Me.cmbUser.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbUser.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbUser.BackColor = System.Drawing.SystemColors.Window
        Me.cmbUser.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbUser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.cmbUser, "cmbUser")
        Me.cmbUser.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbUser.Name = "cmbUser"
        Me.cmbUser.Sorted = True
        '
        '_optRptType_2
        '
        Me._optRptType_2.BackColor = System.Drawing.SystemColors.Control
        Me._optRptType_2.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._optRptType_2, "_optRptType_2")
        Me._optRptType_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optRptType.SetIndex(Me._optRptType_2, CType(2, Short))
        Me._optRptType_2.Name = "_optRptType_2"
        Me._optRptType_2.TabStop = True
        Me._optRptType_2.UseVisualStyleBackColor = False
        '
        '_optRptType_1
        '
        Me._optRptType_1.BackColor = System.Drawing.SystemColors.Control
        Me._optRptType_1.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._optRptType_1, "_optRptType_1")
        Me._optRptType_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optRptType.SetIndex(Me._optRptType_1, CType(1, Short))
        Me._optRptType_1.Name = "_optRptType_1"
        Me._optRptType_1.TabStop = True
        Me._optRptType_1.UseVisualStyleBackColor = False
        '
        '_optRptType_0
        '
        Me._optRptType_0.BackColor = System.Drawing.SystemColors.Control
        Me._optRptType_0.Checked = True
        Me._optRptType_0.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._optRptType_0, "_optRptType_0")
        Me._optRptType_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optRptType.SetIndex(Me._optRptType_0, CType(0, Short))
        Me._optRptType_0.Name = "_optRptType_0"
        Me._optRptType_0.TabStop = True
        Me._optRptType_0.UseVisualStyleBackColor = False
        '
        'chkDetail
        '
        Me.chkDetail.BackColor = System.Drawing.SystemColors.Control
        Me.chkDetail.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.chkDetail, "chkDetail")
        Me.chkDetail.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkDetail.Name = "chkDetail"
        Me.chkDetail.UseVisualStyleBackColor = False
        '
        'cmbSubTeam
        '
        Me.cmbSubTeam.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbSubTeam.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbSubTeam.BackColor = System.Drawing.SystemColors.Window
        Me.cmbSubTeam.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbSubTeam.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.cmbSubTeam, "cmbSubTeam")
        Me.cmbSubTeam.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbSubTeam.Name = "cmbSubTeam"
        Me.cmbSubTeam.Sorted = True
        '
        'cmbStore
        '
        Me.cmbStore.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbStore.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbStore.BackColor = System.Drawing.SystemColors.Window
        Me.cmbStore.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbStore.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.cmbStore, "cmbStore")
        Me.cmbStore.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbStore.Name = "cmbStore"
        Me.cmbStore.Sorted = True
        '
        '_lblLabel_16
        '
        Me._lblLabel_16.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_16.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lblLabel_16, "_lblLabel_16")
        Me._lblLabel_16.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_16, CType(16, Short))
        Me._lblLabel_16.Name = "_lblLabel_16"
        '
        '_lblLabel_2
        '
        Me._lblLabel_2.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_2.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lblLabel_2, "_lblLabel_2")
        Me._lblLabel_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_2, CType(2, Short))
        Me._lblLabel_2.Name = "_lblLabel_2"
        '
        '_lblLabel_0
        '
        Me._lblLabel_0.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_0.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lblLabel_0, "_lblLabel_0")
        Me._lblLabel_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_0, CType(0, Short))
        Me._lblLabel_0.Name = "_lblLabel_0"
        '
        '_lblLabel_5
        '
        Me._lblLabel_5.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_5.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lblLabel_5, "_lblLabel_5")
        Me._lblLabel_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_5, CType(5, Short))
        Me._lblLabel_5.Name = "_lblLabel_5"
        '
        'lblDash
        '
        Me.lblDash.BackColor = System.Drawing.Color.Transparent
        Me.lblDash.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblDash, "lblDash")
        Me.lblDash.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDash.Name = "lblDash"
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.Label2, "Label2")
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Name = "Label2"
        '
        'cmbVendor
        '
        Me.cmbVendor.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbVendor.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbVendor.BackColor = System.Drawing.SystemColors.Window
        Me.cmbVendor.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbVendor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.cmbVendor, "cmbVendor")
        Me.cmbVendor.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbVendor.Name = "cmbVendor"
        Me.cmbVendor.Sorted = True
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.Label3, "Label3")
        Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label3.Name = "Label3"
        '
        'chkPreOrder
        '
        Me.chkPreOrder.BackColor = System.Drawing.SystemColors.Control
        Me.chkPreOrder.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.chkPreOrder, "chkPreOrder")
        Me.chkPreOrder.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkPreOrder.Name = "chkPreOrder"
        Me.chkPreOrder.UseVisualStyleBackColor = False
        '
        'chkIncludeBlankPOs
        '
        Me.chkIncludeBlankPOs.BackColor = System.Drawing.SystemColors.Control
        Me.chkIncludeBlankPOs.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.chkIncludeBlankPOs, "chkIncludeBlankPOs")
        Me.chkIncludeBlankPOs.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIncludeBlankPOs.Name = "chkIncludeBlankPOs"
        Me.chkIncludeBlankPOs.UseVisualStyleBackColor = False
        '
        'frmOpenOrdersReport
        '
        Me.AcceptButton = Me.cmdReport
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdExit
        Me.Controls.Add(Me.chkIncludeBlankPOs)
        Me.Controls.Add(Me.chkPreOrder)
        Me.Controls.Add(Me.cmbVendor)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.cmdReport)
        Me.Controls.Add(Me._optRptType_1)
        Me.Controls.Add(Me.cmbUser)
        Me.Controls.Add(Me._optRptType_2)
        Me.Controls.Add(Me._optRptType_0)
        Me.Controls.Add(Me.chkDetail)
        Me.Controls.Add(Me.cmbSubTeam)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.cmbStore)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me._lblLabel_16)
        Me.Controls.Add(Me._lblLabel_2)
        Me.Controls.Add(Me._lblLabel_0)
        Me.Controls.Add(Me._lblLabel_5)
        Me.Controls.Add(Me.lblDash)
        Me.Controls.Add(Me.dtpOrderEndDate)
        Me.Controls.Add(Me.dtpOrderStartDate)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.dtpExpectedEndDate)
        Me.Controls.Add(Me.dtpExpectedStartDate)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmOpenOrdersReport"
        Me.ShowInTaskbar = False
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.optRptType, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtField, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Public WithEvents lblDash As System.Windows.Forms.Label
    Friend WithEvents dtpOrderEndDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtpOrderStartDate As System.Windows.Forms.DateTimePicker
    Public WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents dtpExpectedEndDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtpExpectedStartDate As System.Windows.Forms.DateTimePicker
    Public WithEvents cmbVendor As System.Windows.Forms.ComboBox
    Public WithEvents Label3 As System.Windows.Forms.Label
    Public WithEvents chkPreOrder As System.Windows.Forms.CheckBox
    Public WithEvents chkIncludeBlankPOs As System.Windows.Forms.CheckBox
#End Region 
End Class