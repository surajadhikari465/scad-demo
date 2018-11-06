<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmAllocationReport
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
    Public WithEvents _optWarehouseSent_2 As System.Windows.Forms.RadioButton
	Public WithEvents _optWarehouseSent_1 As System.Windows.Forms.RadioButton
	Public WithEvents _optWarehouseSent_0 As System.Windows.Forms.RadioButton
	Public WithEvents fraWarehouseSent As System.Windows.Forms.GroupBox
    Public WithEvents cmdRunReport As System.Windows.Forms.Button
    Public WithEvents lblLabel As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
    Public WithEvents optWarehouseSent As Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray
    Public WithEvents txtField As Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAllocationReport))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdRunReport = New System.Windows.Forms.Button
        Me.checkIncludeWOO = New System.Windows.Forms.CheckBox
        Me.fraWarehouseSent = New System.Windows.Forms.GroupBox
        Me._optWarehouseSent_2 = New System.Windows.Forms.RadioButton
        Me._optWarehouseSent_1 = New System.Windows.Forms.RadioButton
        Me._optWarehouseSent_0 = New System.Windows.Forms.RadioButton
        Me.lblLabel = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(Me.components)
        Me.optWarehouseSent = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(Me.components)
        Me.txtField = New Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray(Me.components)
        Me.grpPOExpectedDate = New System.Windows.Forms.GroupBox
        Me.lblExpectedEnd = New System.Windows.Forms.Label
        Me.lblExpectedStart = New System.Windows.Forms.Label
        Me.dtWOOEnd = New System.Windows.Forms.DateTimePicker
        Me.dtWOOStart = New System.Windows.Forms.DateTimePicker
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.optSummaryReport = New System.Windows.Forms.RadioButton
        Me.optDetailReport = New System.Windows.Forms.RadioButton
        Me.grpSubTeamOrderType = New System.Windows.Forms.GroupBox
        Me.optAllOrders = New System.Windows.Forms.RadioButton
        Me.optNonRetail = New System.Windows.Forms.RadioButton
        Me.optRetail = New System.Windows.Forms.RadioButton
        Me.fraPreOrder = New System.Windows.Forms.GroupBox
        Me._optPreOrder_0 = New System.Windows.Forms.RadioButton
        Me._optPreOrder_1 = New System.Windows.Forms.RadioButton
        Me._optPreOrder_2 = New System.Windows.Forms.RadioButton
        Me.fraBOH = New System.Windows.Forms.GroupBox
        Me._optBOH_4 = New System.Windows.Forms.RadioButton
        Me._optBOH_3 = New System.Windows.Forms.RadioButton
        Me._optBOH_2 = New System.Windows.Forms.RadioButton
        Me._optBOH_1 = New System.Windows.Forms.RadioButton
        Me._optBOH_0 = New System.Windows.Forms.RadioButton
        Me.optPreOrder = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(Me.components)
        Me.optBOH = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(Me.components)
        Me.grpPOShipDate = New System.Windows.Forms.GroupBox
        Me.lblShipStart = New System.Windows.Forms.Label
        Me.dtShipStart = New System.Windows.Forms.DateTimePicker
        Me.fraWarehouseSent.SuspendLayout()
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.optWarehouseSent, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtField, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpPOExpectedDate.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.grpSubTeamOrderType.SuspendLayout()
        Me.fraPreOrder.SuspendLayout()
        Me.fraBOH.SuspendLayout()
        CType(Me.optPreOrder, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.optBOH, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpPOShipDate.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdRunReport
        '
        Me.cmdRunReport.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdRunReport.BackColor = System.Drawing.SystemColors.Control
        Me.cmdRunReport.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdRunReport.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.cmdRunReport.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRunReport.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdRunReport.Image = CType(resources.GetObject("cmdRunReport.Image"), System.Drawing.Image)
        Me.cmdRunReport.Location = New System.Drawing.Point(380, 123)
        Me.cmdRunReport.Name = "cmdRunReport"
        Me.cmdRunReport.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdRunReport.Size = New System.Drawing.Size(54, 46)
        Me.cmdRunReport.TabIndex = 4
        Me.cmdRunReport.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdRunReport, "Exit")
        Me.cmdRunReport.UseVisualStyleBackColor = False
        '
        'checkIncludeWOO
        '
        Me.checkIncludeWOO.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.checkIncludeWOO.Location = New System.Drawing.Point(12, 78)
        Me.checkIncludeWOO.Name = "checkIncludeWOO"
        Me.checkIncludeWOO.Size = New System.Drawing.Size(178, 19)
        Me.checkIncludeWOO.TabIndex = 38
        Me.checkIncludeWOO.Text = "Include Warehouse On Order"
        Me.ToolTip1.SetToolTip(Me.checkIncludeWOO, resources.GetString("checkIncludeWOO.ToolTip"))
        Me.checkIncludeWOO.UseVisualStyleBackColor = True
        '
        'fraWarehouseSent
        '
        Me.fraWarehouseSent.BackColor = System.Drawing.SystemColors.Control
        Me.fraWarehouseSent.Controls.Add(Me._optWarehouseSent_2)
        Me.fraWarehouseSent.Controls.Add(Me._optWarehouseSent_1)
        Me.fraWarehouseSent.Controls.Add(Me._optWarehouseSent_0)
        Me.fraWarehouseSent.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraWarehouseSent.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraWarehouseSent.Location = New System.Drawing.Point(178, 270)
        Me.fraWarehouseSent.Name = "fraWarehouseSent"
        Me.fraWarehouseSent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraWarehouseSent.Size = New System.Drawing.Size(160, 78)
        Me.fraWarehouseSent.TabIndex = 0
        Me.fraWarehouseSent.TabStop = False
        Me.fraWarehouseSent.Text = "Warehouse Sent"
        '
        '_optWarehouseSent_2
        '
        Me._optWarehouseSent_2.BackColor = System.Drawing.SystemColors.Control
        Me._optWarehouseSent_2.Checked = True
        Me._optWarehouseSent_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._optWarehouseSent_2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optWarehouseSent_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optWarehouseSent.SetIndex(Me._optWarehouseSent_2, CType(2, Short))
        Me._optWarehouseSent_2.Location = New System.Drawing.Point(6, 56)
        Me._optWarehouseSent_2.Name = "_optWarehouseSent_2"
        Me._optWarehouseSent_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optWarehouseSent_2.Size = New System.Drawing.Size(73, 16)
        Me._optWarehouseSent_2.TabIndex = 3
        Me._optWarehouseSent_2.TabStop = True
        Me._optWarehouseSent_2.Text = "Not Sent"
        Me._optWarehouseSent_2.UseVisualStyleBackColor = False
        '
        '_optWarehouseSent_1
        '
        Me._optWarehouseSent_1.BackColor = System.Drawing.SystemColors.Control
        Me._optWarehouseSent_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._optWarehouseSent_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optWarehouseSent_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optWarehouseSent.SetIndex(Me._optWarehouseSent_1, CType(1, Short))
        Me._optWarehouseSent_1.Location = New System.Drawing.Point(6, 37)
        Me._optWarehouseSent_1.Name = "_optWarehouseSent_1"
        Me._optWarehouseSent_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optWarehouseSent_1.Size = New System.Drawing.Size(73, 16)
        Me._optWarehouseSent_1.TabIndex = 2
        Me._optWarehouseSent_1.TabStop = True
        Me._optWarehouseSent_1.Text = "Sent"
        Me._optWarehouseSent_1.UseVisualStyleBackColor = False
        '
        '_optWarehouseSent_0
        '
        Me._optWarehouseSent_0.BackColor = System.Drawing.SystemColors.Control
        Me._optWarehouseSent_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._optWarehouseSent_0.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optWarehouseSent_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optWarehouseSent.SetIndex(Me._optWarehouseSent_0, CType(0, Short))
        Me._optWarehouseSent_0.Location = New System.Drawing.Point(6, 18)
        Me._optWarehouseSent_0.Name = "_optWarehouseSent_0"
        Me._optWarehouseSent_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optWarehouseSent_0.Size = New System.Drawing.Size(73, 16)
        Me._optWarehouseSent_0.TabIndex = 1
        Me._optWarehouseSent_0.TabStop = True
        Me._optWarehouseSent_0.Text = "All"
        Me._optWarehouseSent_0.UseVisualStyleBackColor = False
        '
        'grpPOExpectedDate
        '
        Me.grpPOExpectedDate.Controls.Add(Me.lblExpectedEnd)
        Me.grpPOExpectedDate.Controls.Add(Me.lblExpectedStart)
        Me.grpPOExpectedDate.Controls.Add(Me.dtWOOEnd)
        Me.grpPOExpectedDate.Controls.Add(Me.dtWOOStart)
        Me.grpPOExpectedDate.Enabled = False
        Me.grpPOExpectedDate.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpPOExpectedDate.Location = New System.Drawing.Point(12, 103)
        Me.grpPOExpectedDate.Name = "grpPOExpectedDate"
        Me.grpPOExpectedDate.Size = New System.Drawing.Size(256, 77)
        Me.grpPOExpectedDate.TabIndex = 39
        Me.grpPOExpectedDate.TabStop = False
        Me.grpPOExpectedDate.Text = "WOO Date Range"
        '
        'lblExpectedEnd
        '
        Me.lblExpectedEnd.AutoSize = True
        Me.lblExpectedEnd.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold)
        Me.lblExpectedEnd.Location = New System.Drawing.Point(15, 48)
        Me.lblExpectedEnd.Name = "lblExpectedEnd"
        Me.lblExpectedEnd.Size = New System.Drawing.Size(107, 13)
        Me.lblExpectedEnd.TabIndex = 3
        Me.lblExpectedEnd.Text = "Expected Date End:"
        '
        'lblExpectedStart
        '
        Me.lblExpectedStart.AutoSize = True
        Me.lblExpectedStart.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold)
        Me.lblExpectedStart.Location = New System.Drawing.Point(11, 23)
        Me.lblExpectedStart.Name = "lblExpectedStart"
        Me.lblExpectedStart.Size = New System.Drawing.Size(111, 13)
        Me.lblExpectedStart.TabIndex = 2
        Me.lblExpectedStart.Text = "Expected Date Start:"
        '
        'dtWOOEnd
        '
        Me.dtWOOEnd.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtWOOEnd.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtWOOEnd.Location = New System.Drawing.Point(127, 44)
        Me.dtWOOEnd.Name = "dtWOOEnd"
        Me.dtWOOEnd.Size = New System.Drawing.Size(109, 22)
        Me.dtWOOEnd.TabIndex = 1
        '
        'dtWOOStart
        '
        Me.dtWOOStart.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtWOOStart.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtWOOStart.Location = New System.Drawing.Point(127, 19)
        Me.dtWOOStart.Name = "dtWOOStart"
        Me.dtWOOStart.Size = New System.Drawing.Size(109, 22)
        Me.dtWOOStart.TabIndex = 0
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.optSummaryReport)
        Me.GroupBox1.Controls.Add(Me.optDetailReport)
        Me.GroupBox1.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(160, 60)
        Me.GroupBox1.TabIndex = 40
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Report Type"
        '
        'optSummaryReport
        '
        Me.optSummaryReport.AutoSize = True
        Me.optSummaryReport.Location = New System.Drawing.Point(6, 36)
        Me.optSummaryReport.Name = "optSummaryReport"
        Me.optSummaryReport.Size = New System.Drawing.Size(74, 17)
        Me.optSummaryReport.TabIndex = 1
        Me.optSummaryReport.Text = "Summary"
        Me.optSummaryReport.UseVisualStyleBackColor = True
        '
        'optDetailReport
        '
        Me.optDetailReport.AutoSize = True
        Me.optDetailReport.Checked = True
        Me.optDetailReport.Location = New System.Drawing.Point(6, 17)
        Me.optDetailReport.Name = "optDetailReport"
        Me.optDetailReport.Size = New System.Drawing.Size(55, 17)
        Me.optDetailReport.TabIndex = 0
        Me.optDetailReport.TabStop = True
        Me.optDetailReport.Text = "Detail"
        Me.optDetailReport.UseVisualStyleBackColor = True
        '
        'grpSubTeamOrderType
        '
        Me.grpSubTeamOrderType.BackColor = System.Drawing.SystemColors.Control
        Me.grpSubTeamOrderType.Controls.Add(Me.optAllOrders)
        Me.grpSubTeamOrderType.Controls.Add(Me.optNonRetail)
        Me.grpSubTeamOrderType.Controls.Add(Me.optRetail)
        Me.grpSubTeamOrderType.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold)
        Me.grpSubTeamOrderType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.grpSubTeamOrderType.Location = New System.Drawing.Point(12, 186)
        Me.grpSubTeamOrderType.Name = "grpSubTeamOrderType"
        Me.grpSubTeamOrderType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.grpSubTeamOrderType.Size = New System.Drawing.Size(160, 78)
        Me.grpSubTeamOrderType.TabIndex = 41
        Me.grpSubTeamOrderType.TabStop = False
        Me.grpSubTeamOrderType.Text = "Subteam Order Type"
        '
        'optAllOrders
        '
        Me.optAllOrders.AutoSize = True
        Me.optAllOrders.BackColor = System.Drawing.SystemColors.Control
        Me.optAllOrders.Cursor = System.Windows.Forms.Cursors.Default
        Me.optAllOrders.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optAllOrders.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optAllOrders.Location = New System.Drawing.Point(6, 17)
        Me.optAllOrders.Name = "optAllOrders"
        Me.optAllOrders.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optAllOrders.Size = New System.Drawing.Size(39, 18)
        Me.optAllOrders.TabIndex = 7
        Me.optAllOrders.TabStop = True
        Me.optAllOrders.Text = "All"
        Me.optAllOrders.UseVisualStyleBackColor = False
        '
        'optNonRetail
        '
        Me.optNonRetail.AutoSize = True
        Me.optNonRetail.BackColor = System.Drawing.SystemColors.Control
        Me.optNonRetail.Cursor = System.Windows.Forms.Cursors.Default
        Me.optNonRetail.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optNonRetail.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optNonRetail.Location = New System.Drawing.Point(6, 36)
        Me.optNonRetail.Name = "optNonRetail"
        Me.optNonRetail.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optNonRetail.Size = New System.Drawing.Size(80, 18)
        Me.optNonRetail.TabIndex = 3
        Me.optNonRetail.TabStop = True
        Me.optNonRetail.Text = "Non-Retail"
        Me.optNonRetail.UseVisualStyleBackColor = False
        '
        'optRetail
        '
        Me.optRetail.AutoSize = True
        Me.optRetail.BackColor = System.Drawing.SystemColors.Control
        Me.optRetail.Checked = True
        Me.optRetail.Cursor = System.Windows.Forms.Cursors.Default
        Me.optRetail.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optRetail.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optRetail.Location = New System.Drawing.Point(6, 55)
        Me.optRetail.Name = "optRetail"
        Me.optRetail.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optRetail.Size = New System.Drawing.Size(55, 18)
        Me.optRetail.TabIndex = 4
        Me.optRetail.TabStop = True
        Me.optRetail.Text = "Retail"
        Me.optRetail.UseVisualStyleBackColor = False
        '
        'fraPreOrder
        '
        Me.fraPreOrder.BackColor = System.Drawing.SystemColors.Control
        Me.fraPreOrder.Controls.Add(Me._optPreOrder_0)
        Me.fraPreOrder.Controls.Add(Me._optPreOrder_1)
        Me.fraPreOrder.Controls.Add(Me._optPreOrder_2)
        Me.fraPreOrder.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold)
        Me.fraPreOrder.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraPreOrder.Location = New System.Drawing.Point(178, 186)
        Me.fraPreOrder.Name = "fraPreOrder"
        Me.fraPreOrder.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraPreOrder.Size = New System.Drawing.Size(160, 78)
        Me.fraPreOrder.TabIndex = 42
        Me.fraPreOrder.TabStop = False
        Me.fraPreOrder.Text = "PreOrder Type"
        '
        '_optPreOrder_0
        '
        Me._optPreOrder_0.BackColor = System.Drawing.SystemColors.Control
        Me._optPreOrder_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._optPreOrder_0.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optPreOrder_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optPreOrder.SetIndex(Me._optPreOrder_0, CType(0, Short))
        Me._optPreOrder_0.Location = New System.Drawing.Point(10, 18)
        Me._optPreOrder_0.Name = "_optPreOrder_0"
        Me._optPreOrder_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optPreOrder_0.Size = New System.Drawing.Size(105, 17)
        Me._optPreOrder_0.TabIndex = 6
        Me._optPreOrder_0.TabStop = True
        Me._optPreOrder_0.Text = "All"
        Me._optPreOrder_0.UseVisualStyleBackColor = False
        '
        '_optPreOrder_1
        '
        Me._optPreOrder_1.BackColor = System.Drawing.SystemColors.Control
        Me._optPreOrder_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._optPreOrder_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optPreOrder_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optPreOrder.SetIndex(Me._optPreOrder_1, CType(1, Short))
        Me._optPreOrder_1.Location = New System.Drawing.Point(10, 37)
        Me._optPreOrder_1.Name = "_optPreOrder_1"
        Me._optPreOrder_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optPreOrder_1.Size = New System.Drawing.Size(105, 17)
        Me._optPreOrder_1.TabIndex = 7
        Me._optPreOrder_1.TabStop = True
        Me._optPreOrder_1.Text = "Pre-Order"
        Me._optPreOrder_1.UseVisualStyleBackColor = False
        '
        '_optPreOrder_2
        '
        Me._optPreOrder_2.BackColor = System.Drawing.SystemColors.Control
        Me._optPreOrder_2.Checked = True
        Me._optPreOrder_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._optPreOrder_2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optPreOrder_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optPreOrder.SetIndex(Me._optPreOrder_2, CType(2, Short))
        Me._optPreOrder_2.Location = New System.Drawing.Point(10, 56)
        Me._optPreOrder_2.Name = "_optPreOrder_2"
        Me._optPreOrder_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optPreOrder_2.Size = New System.Drawing.Size(105, 17)
        Me._optPreOrder_2.TabIndex = 8
        Me._optPreOrder_2.TabStop = True
        Me._optPreOrder_2.Text = "Non Pre-Order"
        Me._optPreOrder_2.UseVisualStyleBackColor = False
        '
        'fraBOH
        '
        Me.fraBOH.BackColor = System.Drawing.SystemColors.Control
        Me.fraBOH.Controls.Add(Me._optBOH_4)
        Me.fraBOH.Controls.Add(Me._optBOH_3)
        Me.fraBOH.Controls.Add(Me._optBOH_2)
        Me.fraBOH.Controls.Add(Me._optBOH_1)
        Me.fraBOH.Controls.Add(Me._optBOH_0)
        Me.fraBOH.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold)
        Me.fraBOH.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraBOH.Location = New System.Drawing.Point(12, 270)
        Me.fraBOH.Name = "fraBOH"
        Me.fraBOH.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraBOH.Size = New System.Drawing.Size(160, 78)
        Me.fraBOH.TabIndex = 43
        Me.fraBOH.TabStop = False
        Me.fraBOH.Text = "BOH Filter"
        '
        '_optBOH_4
        '
        Me._optBOH_4.AutoSize = True
        Me._optBOH_4.BackColor = System.Drawing.SystemColors.Control
        Me._optBOH_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._optBOH_4.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optBOH_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optBOH.SetIndex(Me._optBOH_4, CType(4, Short))
        Me._optBOH_4.Location = New System.Drawing.Point(79, 56)
        Me._optBOH_4.Name = "_optBOH_4"
        Me._optBOH_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optBOH_4.Size = New System.Drawing.Size(67, 18)
        Me._optBOH_4.TabIndex = 16
        Me._optBOH_4.TabStop = True
        Me._optBOH_4.Text = "Diff >= 0"
        Me._optBOH_4.UseVisualStyleBackColor = False
        '
        '_optBOH_3
        '
        Me._optBOH_3.AutoSize = True
        Me._optBOH_3.BackColor = System.Drawing.SystemColors.Control
        Me._optBOH_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._optBOH_3.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optBOH_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optBOH.SetIndex(Me._optBOH_3, CType(3, Short))
        Me._optBOH_3.Location = New System.Drawing.Point(79, 37)
        Me._optBOH_3.Name = "_optBOH_3"
        Me._optBOH_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optBOH_3.Size = New System.Drawing.Size(61, 18)
        Me._optBOH_3.TabIndex = 15
        Me._optBOH_3.TabStop = True
        Me._optBOH_3.Text = "Diff < 0"
        Me._optBOH_3.UseVisualStyleBackColor = False
        '
        '_optBOH_2
        '
        Me._optBOH_2.BackColor = System.Drawing.SystemColors.Control
        Me._optBOH_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._optBOH_2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optBOH_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optBOH.SetIndex(Me._optBOH_2, CType(2, Short))
        Me._optBOH_2.Location = New System.Drawing.Point(6, 56)
        Me._optBOH_2.Name = "_optBOH_2"
        Me._optBOH_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optBOH_2.Size = New System.Drawing.Size(81, 17)
        Me._optBOH_2.TabIndex = 12
        Me._optBOH_2.TabStop = True
        Me._optBOH_2.Text = "Diff <= 0"
        Me._optBOH_2.UseVisualStyleBackColor = False
        '
        '_optBOH_1
        '
        Me._optBOH_1.BackColor = System.Drawing.SystemColors.Control
        Me._optBOH_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._optBOH_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optBOH_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optBOH.SetIndex(Me._optBOH_1, CType(1, Short))
        Me._optBOH_1.Location = New System.Drawing.Point(6, 37)
        Me._optBOH_1.Name = "_optBOH_1"
        Me._optBOH_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optBOH_1.Size = New System.Drawing.Size(81, 17)
        Me._optBOH_1.TabIndex = 11
        Me._optBOH_1.TabStop = True
        Me._optBOH_1.Text = "Diff > 0"
        Me._optBOH_1.UseVisualStyleBackColor = False
        '
        '_optBOH_0
        '
        Me._optBOH_0.BackColor = System.Drawing.SystemColors.Control
        Me._optBOH_0.Checked = True
        Me._optBOH_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._optBOH_0.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optBOH_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optBOH.SetIndex(Me._optBOH_0, CType(0, Short))
        Me._optBOH_0.Location = New System.Drawing.Point(6, 18)
        Me._optBOH_0.Name = "_optBOH_0"
        Me._optBOH_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optBOH_0.Size = New System.Drawing.Size(73, 17)
        Me._optBOH_0.TabIndex = 10
        Me._optBOH_0.TabStop = True
        Me._optBOH_0.Text = "All"
        Me._optBOH_0.UseVisualStyleBackColor = False
        '
        'grpPOShipDate
        '
        Me.grpPOShipDate.Controls.Add(Me.lblShipStart)
        Me.grpPOShipDate.Controls.Add(Me.dtShipStart)
        Me.grpPOShipDate.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpPOShipDate.Location = New System.Drawing.Point(178, 12)
        Me.grpPOShipDate.Name = "grpPOShipDate"
        Me.grpPOShipDate.Size = New System.Drawing.Size(256, 60)
        Me.grpPOShipDate.TabIndex = 44
        Me.grpPOShipDate.TabStop = False
        Me.grpPOShipDate.Text = "Ship Date"
        '
        'lblShipStart
        '
        Me.lblShipStart.AutoSize = True
        Me.lblShipStart.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold)
        Me.lblShipStart.Location = New System.Drawing.Point(38, 28)
        Me.lblShipStart.Name = "lblShipStart"
        Me.lblShipStart.Size = New System.Drawing.Size(84, 13)
        Me.lblShipStart.TabIndex = 4
        Me.lblShipStart.Text = "Expected Date:"
        '
        'dtShipStart
        '
        Me.dtShipStart.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtShipStart.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtShipStart.Location = New System.Drawing.Point(127, 24)
        Me.dtShipStart.Name = "dtShipStart"
        Me.dtShipStart.Size = New System.Drawing.Size(109, 22)
        Me.dtShipStart.TabIndex = 3
        '
        'frmAllocationReport
        '
        Me.AcceptButton = Me.cmdRunReport
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdRunReport
        Me.ClientSize = New System.Drawing.Size(446, 359)
        Me.Controls.Add(Me.grpPOShipDate)
        Me.Controls.Add(Me.grpSubTeamOrderType)
        Me.Controls.Add(Me.fraPreOrder)
        Me.Controls.Add(Me.fraBOH)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.checkIncludeWOO)
        Me.Controls.Add(Me.grpPOExpectedDate)
        Me.Controls.Add(Me.fraWarehouseSent)
        Me.Controls.Add(Me.cmdRunReport)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Location = New System.Drawing.Point(341, 203)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmAllocationReport"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Allocation Report"
        Me.fraWarehouseSent.ResumeLayout(False)
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.optWarehouseSent, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtField, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpPOExpectedDate.ResumeLayout(False)
        Me.grpPOExpectedDate.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.grpSubTeamOrderType.ResumeLayout(False)
        Me.grpSubTeamOrderType.PerformLayout()
        Me.fraPreOrder.ResumeLayout(False)
        Me.fraBOH.ResumeLayout(False)
        Me.fraBOH.PerformLayout()
        CType(Me.optPreOrder, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.optBOH, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpPOShipDate.ResumeLayout(False)
        Me.grpPOShipDate.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents checkIncludeWOO As System.Windows.Forms.CheckBox
    Friend WithEvents grpPOExpectedDate As System.Windows.Forms.GroupBox
    Friend WithEvents lblExpectedEnd As System.Windows.Forms.Label
    Friend WithEvents lblExpectedStart As System.Windows.Forms.Label
    Friend WithEvents dtWOOEnd As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtWOOStart As System.Windows.Forms.DateTimePicker
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents optSummaryReport As System.Windows.Forms.RadioButton
    Friend WithEvents optDetailReport As System.Windows.Forms.RadioButton
    Public WithEvents grpSubTeamOrderType As System.Windows.Forms.GroupBox
    Public WithEvents optAllOrders As System.Windows.Forms.RadioButton
    Public WithEvents optNonRetail As System.Windows.Forms.RadioButton
    Public WithEvents optRetail As System.Windows.Forms.RadioButton
    Public WithEvents fraPreOrder As System.Windows.Forms.GroupBox
    Public WithEvents _optPreOrder_0 As System.Windows.Forms.RadioButton
    Public WithEvents _optPreOrder_1 As System.Windows.Forms.RadioButton
    Public WithEvents _optPreOrder_2 As System.Windows.Forms.RadioButton
    Public WithEvents fraBOH As System.Windows.Forms.GroupBox
    Public WithEvents _optBOH_2 As System.Windows.Forms.RadioButton
    Public WithEvents _optBOH_1 As System.Windows.Forms.RadioButton
    Public WithEvents _optBOH_0 As System.Windows.Forms.RadioButton
    Public WithEvents optPreOrder As Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray
    Public WithEvents optBOH As Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray
    Public WithEvents _optBOH_4 As System.Windows.Forms.RadioButton
    Public WithEvents _optBOH_3 As System.Windows.Forms.RadioButton
    Friend WithEvents grpPOShipDate As System.Windows.Forms.GroupBox
    Friend WithEvents lblShipStart As System.Windows.Forms.Label
    Friend WithEvents dtShipStart As System.Windows.Forms.DateTimePicker
#End Region
End Class