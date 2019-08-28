<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmOrdersAllocate
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
	Public WithEvents cmdOrders As System.Windows.Forms.Button
	Public WithEvents cmdCloseOrderingWindow As System.Windows.Forms.Button
	Public WithEvents optNonRetail As System.Windows.Forms.RadioButton
	Public WithEvents optRetail As System.Windows.Forms.RadioButton
	Public WithEvents fraTransfers As System.Windows.Forms.GroupBox
	Public WithEvents _optPreOrder_0 As System.Windows.Forms.RadioButton
	Public WithEvents _optPreOrder_1 As System.Windows.Forms.RadioButton
	Public WithEvents _optPreOrder_2 As System.Windows.Forms.RadioButton
	Public WithEvents fraPreOrder As System.Windows.Forms.GroupBox
	Public WithEvents cmdRefresh As System.Windows.Forms.Button
	Public WithEvents cmdWarehouseSend As System.Windows.Forms.Button
	Public WithEvents _optBOH_2 As System.Windows.Forms.RadioButton
	Public WithEvents _optBOH_1 As System.Windows.Forms.RadioButton
	Public WithEvents _optBOH_0 As System.Windows.Forms.RadioButton
	Public WithEvents fraBOH As System.Windows.Forms.GroupBox
	Public WithEvents cmdReport As System.Windows.Forms.Button
	Public WithEvents cmdExit As System.Windows.Forms.Button
	Public WithEvents cmdSearch As System.Windows.Forms.Button
	Public WithEvents cmdNext As System.Windows.Forms.Button
	Public WithEvents cmdPrevious As System.Windows.Forms.Button
    Public WithEvents _lblLabel_3 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_2 As System.Windows.Forms.Label
	Public WithEvents lblItemDesc As System.Windows.Forms.Label
	Public WithEvents lblIdentifier As System.Windows.Forms.Label
	Public WithEvents fraAlloc As System.Windows.Forms.GroupBox
	Public WithEvents cmbSubTeam As System.Windows.Forms.ComboBox
	Public WithEvents cmbStore As System.Windows.Forms.ComboBox
    Public WithEvents _lblLabel_1 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_0 As System.Windows.Forms.Label
	Public WithEvents lblLabel As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	Public WithEvents optBOH As Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray
	Public WithEvents optPreOrder As Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmOrdersAllocate))
		Dim Appearance1 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
		Dim UltraGridBand1 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1)
		Dim UltraGridColumn1 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("PackSize")
		Dim Appearance2 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
		Dim Appearance3 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
		Dim UltraGridColumn2 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("BOH")
		Dim Appearance4 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
		Dim Appearance5 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
		Dim UltraGridColumn3 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("WOO")
		Dim Appearance6 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
		Dim Appearance7 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
		Dim UltraGridColumn4 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("SOO")
		Dim Appearance8 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
		Dim Appearance9 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
		Dim UltraGridColumn5 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Alloc")
		Dim Appearance10 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
		Dim Appearance11 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
		Dim UltraGridColumn6 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("EOH")
		Dim Appearance12 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
		Dim Appearance13 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
		Dim Appearance14 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
		Dim Appearance15 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
		Dim Appearance16 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
		Dim Appearance17 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
		Dim Appearance18 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
		Dim Appearance19 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
		Dim Appearance20 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
		Dim Appearance21 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
		Dim Appearance22 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
		Dim Appearance23 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
		Dim Appearance24 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
		Dim Appearance25 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
		Dim Appearance26 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
		Dim Appearance27 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
		Dim UltraGridBand2 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("Recordset", -1)
		Dim UltraGridColumn7 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("OrderItem_ID")
		Dim UltraGridColumn8 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Item_Key")
		Dim UltraGridColumn9 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyName")
		Dim UltraGridColumn10 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("OrderHeader_ID")
		Dim Appearance28 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
		Dim Appearance29 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
		Dim UltraGridColumn11 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("QuantityOrdered")
		Dim Appearance30 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
		Dim Appearance31 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
		Dim UltraGridColumn12 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Package_Desc1", -1, 81762766)
		Dim Appearance32 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
		Dim Appearance33 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
		Dim UltraGridColumn13 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("QuantityAllocated")
		Dim Appearance34 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
		Dim Appearance35 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
		Dim UltraGridColumn14 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Identifier", 0)
		Dim UltraGridColumn15 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Item_Description", 1)
		Dim UltraGridColumn16 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("SubTeam_Name", 2, Nothing, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, False)
		Dim ColScrollRegion1 As Infragistics.Win.UltraWinGrid.ColScrollRegion = New Infragistics.Win.UltraWinGrid.ColScrollRegion(776)
		Dim ColScrollRegion2 As Infragistics.Win.UltraWinGrid.ColScrollRegion = New Infragistics.Win.UltraWinGrid.ColScrollRegion(638)
		Dim ColScrollRegion3 As Infragistics.Win.UltraWinGrid.ColScrollRegion = New Infragistics.Win.UltraWinGrid.ColScrollRegion(500)
		Dim ColScrollRegion4 As Infragistics.Win.UltraWinGrid.ColScrollRegion = New Infragistics.Win.UltraWinGrid.ColScrollRegion(-1159)
		Dim Appearance36 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
		Dim Appearance37 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
		Dim Appearance38 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
		Dim Appearance40 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
		Dim Appearance41 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
		Dim Appearance42 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
		Dim Appearance43 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
		Dim Appearance44 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
		Dim Appearance48 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
		Dim Appearance46 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
		Dim Appearance45 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
		Dim ValueList1 As Infragistics.Win.ValueList = New Infragistics.Win.ValueList(81762766)
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cmdOrders = New System.Windows.Forms.Button()
		Me.cmdCloseOrderingWindow = New System.Windows.Forms.Button()
		Me.cmdRefresh = New System.Windows.Forms.Button()
		Me.cmdWarehouseSend = New System.Windows.Forms.Button()
		Me.cmdReport = New System.Windows.Forms.Button()
		Me.cmdExit = New System.Windows.Forms.Button()
		Me.cmdSearch = New System.Windows.Forms.Button()
		Me.cmdNext = New System.Windows.Forms.Button()
		Me.cmdPrevious = New System.Windows.Forms.Button()
		Me.cmdAutoAllocate = New System.Windows.Forms.Button()
		Me.cmdSubItemSearch = New System.Windows.Forms.Button()
		Me.cmdSubstitute = New System.Windows.Forms.Button()
		Me.cmdShowNotAvailable = New System.Windows.Forms.Button()
		Me.checkIncludeWOO = New System.Windows.Forms.CheckBox()
		Me.chkSubteam = New System.Windows.Forms.CheckBox()
		Me.fraTransfers = New System.Windows.Forms.GroupBox()
		Me.optAllOrders = New System.Windows.Forms.RadioButton()
		Me.optNonRetail = New System.Windows.Forms.RadioButton()
		Me.optRetail = New System.Windows.Forms.RadioButton()
		Me.fraPreOrder = New System.Windows.Forms.GroupBox()
		Me._optPreOrder_0 = New System.Windows.Forms.RadioButton()
		Me._optPreOrder_1 = New System.Windows.Forms.RadioButton()
		Me._optPreOrder_2 = New System.Windows.Forms.RadioButton()
		Me.fraBOH = New System.Windows.Forms.GroupBox()
		Me.checkMultiPackOnly = New System.Windows.Forms.CheckBox()
		Me._optBOH_4 = New System.Windows.Forms.RadioButton()
		Me._optBOH_3 = New System.Windows.Forms.RadioButton()
		Me._optBOH_2 = New System.Windows.Forms.RadioButton()
		Me._optBOH_1 = New System.Windows.Forms.RadioButton()
		Me._optBOH_0 = New System.Windows.Forms.RadioButton()
		Me.fraAlloc = New System.Windows.Forms.GroupBox()
		Me.grpSubstitute = New System.Windows.Forms.GroupBox()
		Me.txtSubstituteIdentifier = New System.Windows.Forms.TextBox()
		Me._lblLabel_5 = New System.Windows.Forms.Label()
		Me._lblLabel_4 = New System.Windows.Forms.Label()
		Me.lblSubIdentifierDesc = New System.Windows.Forms.Label()
		Me.ugrdItem = New Infragistics.Win.UltraWinGrid.UltraGrid()
		Me.ugrdAlloc = New Infragistics.Win.UltraWinGrid.UltraGrid()
		Me._lblLabel_3 = New System.Windows.Forms.Label()
		Me._lblLabel_2 = New System.Windows.Forms.Label()
		Me.lblItemDesc = New System.Windows.Forms.Label()
		Me.lblIdentifier = New System.Windows.Forms.Label()
		Me.cmbSubTeam = New System.Windows.Forms.ComboBox()
		Me.cmbStore = New System.Windows.Forms.ComboBox()
		Me._lblLabel_1 = New System.Windows.Forms.Label()
		Me._lblLabel_0 = New System.Windows.Forms.Label()
		Me.lblLabel = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(Me.components)
		Me.optBOH = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(Me.components)
		Me.optPreOrder = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(Me.components)
		Me.frmErrorProvider = New System.Windows.Forms.ErrorProvider(Me.components)
		Me.grpPOExpectedDate = New System.Windows.Forms.GroupBox()
		Me.Label2 = New System.Windows.Forms.Label()
		Me.Label1 = New System.Windows.Forms.Label()
		Me.dtWOOEnd = New System.Windows.Forms.DateTimePicker()
		Me.dtWOOStart = New System.Windows.Forms.DateTimePicker()
		Me.fraTransfers.SuspendLayout()
		Me.fraPreOrder.SuspendLayout()
		Me.fraBOH.SuspendLayout()
		Me.fraAlloc.SuspendLayout()
		Me.grpSubstitute.SuspendLayout()
		CType(Me.ugrdItem, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.ugrdAlloc, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.optBOH, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.optPreOrder, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.frmErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.grpPOExpectedDate.SuspendLayout()
		Me.SuspendLayout()
		'
		'cmdOrders
		'
		Me.cmdOrders.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.cmdOrders.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOrders.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOrders.Enabled = False
		Me.cmdOrders.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmdOrders.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOrders.Image = CType(resources.GetObject("cmdOrders.Image"), System.Drawing.Image)
		Me.cmdOrders.Location = New System.Drawing.Point(790, 102)
		Me.cmdOrders.Name = "cmdOrders"
		Me.cmdOrders.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOrders.Size = New System.Drawing.Size(41, 41)
		Me.cmdOrders.TabIndex = 16
		Me.cmdOrders.TextAlign = System.Drawing.ContentAlignment.BottomCenter
		Me.ToolTip1.SetToolTip(Me.cmdOrders, "Edit Orders")
		Me.cmdOrders.UseVisualStyleBackColor = True
		'
		'cmdCloseOrderingWindow
		'
		Me.cmdCloseOrderingWindow.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCloseOrderingWindow.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCloseOrderingWindow.Enabled = False
		Me.cmdCloseOrderingWindow.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmdCloseOrderingWindow.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCloseOrderingWindow.Image = CType(resources.GetObject("cmdCloseOrderingWindow.Image"), System.Drawing.Image)
		Me.cmdCloseOrderingWindow.Location = New System.Drawing.Point(700, 95)
		Me.cmdCloseOrderingWindow.Name = "cmdCloseOrderingWindow"
		Me.cmdCloseOrderingWindow.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCloseOrderingWindow.Size = New System.Drawing.Size(41, 41)
		Me.cmdCloseOrderingWindow.TabIndex = 15
		Me.cmdCloseOrderingWindow.TextAlign = System.Drawing.ContentAlignment.BottomCenter
		Me.ToolTip1.SetToolTip(Me.cmdCloseOrderingWindow, "Close Ordering Window")
		Me.cmdCloseOrderingWindow.UseVisualStyleBackColor = True
		'
		'cmdRefresh
		'
		Me.cmdRefresh.BackColor = System.Drawing.SystemColors.Control
		Me.cmdRefresh.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdRefresh.Enabled = False
		Me.cmdRefresh.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmdRefresh.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdRefresh.Image = CType(resources.GetObject("cmdRefresh.Image"), System.Drawing.Image)
		Me.cmdRefresh.Location = New System.Drawing.Point(700, 9)
		Me.cmdRefresh.Name = "cmdRefresh"
		Me.cmdRefresh.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdRefresh.Size = New System.Drawing.Size(41, 41)
		Me.cmdRefresh.TabIndex = 14
		Me.cmdRefresh.TextAlign = System.Drawing.ContentAlignment.BottomCenter
		Me.ToolTip1.SetToolTip(Me.cmdRefresh, "Refresh Item Section")
		Me.cmdRefresh.UseVisualStyleBackColor = True
		'
		'cmdWarehouseSend
		'
		Me.cmdWarehouseSend.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.cmdWarehouseSend.BackColor = System.Drawing.SystemColors.Control
		Me.cmdWarehouseSend.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdWarehouseSend.Enabled = False
		Me.cmdWarehouseSend.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmdWarehouseSend.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdWarehouseSend.Image = CType(resources.GetObject("cmdWarehouseSend.Image"), System.Drawing.Image)
		Me.cmdWarehouseSend.Location = New System.Drawing.Point(790, 149)
		Me.cmdWarehouseSend.Name = "cmdWarehouseSend"
		Me.cmdWarehouseSend.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdWarehouseSend.Size = New System.Drawing.Size(41, 41)
		Me.cmdWarehouseSend.TabIndex = 23
		Me.cmdWarehouseSend.TextAlign = System.Drawing.ContentAlignment.BottomCenter
		Me.ToolTip1.SetToolTip(Me.cmdWarehouseSend, "Warehouse Send Now")
		Me.cmdWarehouseSend.UseVisualStyleBackColor = True
		'
		'cmdReport
		'
		Me.cmdReport.BackColor = System.Drawing.SystemColors.Control
		Me.cmdReport.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdReport.Enabled = False
		Me.cmdReport.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmdReport.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdReport.Image = CType(resources.GetObject("cmdReport.Image"), System.Drawing.Image)
		Me.cmdReport.Location = New System.Drawing.Point(700, 52)
		Me.cmdReport.Name = "cmdReport"
		Me.cmdReport.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdReport.Size = New System.Drawing.Size(41, 41)
		Me.cmdReport.TabIndex = 13
		Me.cmdReport.TextAlign = System.Drawing.ContentAlignment.BottomCenter
		Me.ToolTip1.SetToolTip(Me.cmdReport, "Allocation Report")
		Me.cmdReport.UseVisualStyleBackColor = True
		'
		'cmdExit
		'
		Me.cmdExit.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
		Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
		Me.cmdExit.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdExit.Image = CType(resources.GetObject("cmdExit.Image"), System.Drawing.Image)
		Me.cmdExit.Location = New System.Drawing.Point(798, 9)
		Me.cmdExit.Name = "cmdExit"
		Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdExit.Size = New System.Drawing.Size(41, 41)
		Me.cmdExit.TabIndex = 24
		Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
		Me.ToolTip1.SetToolTip(Me.cmdExit, "Exit")
		Me.cmdExit.UseVisualStyleBackColor = True
		'
		'cmdSearch
		'
		Me.cmdSearch.BackColor = System.Drawing.SystemColors.Control
		Me.cmdSearch.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdSearch.Enabled = False
		Me.cmdSearch.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmdSearch.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdSearch.Image = CType(resources.GetObject("cmdSearch.Image"), System.Drawing.Image)
		Me.cmdSearch.Location = New System.Drawing.Point(743, 19)
		Me.cmdSearch.Name = "cmdSearch"
		Me.cmdSearch.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdSearch.Size = New System.Drawing.Size(41, 33)
		Me.cmdSearch.TabIndex = 20
		Me.cmdSearch.TextAlign = System.Drawing.ContentAlignment.BottomCenter
		Me.ToolTip1.SetToolTip(Me.cmdSearch, "Search For Item")
		Me.cmdSearch.UseVisualStyleBackColor = True
		'
		'cmdNext
		'
		Me.cmdNext.BackColor = System.Drawing.SystemColors.Control
		Me.cmdNext.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdNext.Enabled = False
		Me.cmdNext.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmdNext.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdNext.Image = CType(resources.GetObject("cmdNext.Image"), System.Drawing.Image)
		Me.cmdNext.Location = New System.Drawing.Point(692, 19)
		Me.cmdNext.Name = "cmdNext"
		Me.cmdNext.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdNext.Size = New System.Drawing.Size(41, 33)
		Me.cmdNext.TabIndex = 22
		Me.cmdNext.TextAlign = System.Drawing.ContentAlignment.BottomCenter
		Me.ToolTip1.SetToolTip(Me.cmdNext, "Next Item")
		Me.cmdNext.UseVisualStyleBackColor = True
		'
		'cmdPrevious
		'
		Me.cmdPrevious.BackColor = System.Drawing.SystemColors.Control
		Me.cmdPrevious.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdPrevious.Enabled = False
		Me.cmdPrevious.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmdPrevious.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdPrevious.Image = CType(resources.GetObject("cmdPrevious.Image"), System.Drawing.Image)
		Me.cmdPrevious.Location = New System.Drawing.Point(645, 19)
		Me.cmdPrevious.Name = "cmdPrevious"
		Me.cmdPrevious.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdPrevious.Size = New System.Drawing.Size(41, 33)
		Me.cmdPrevious.TabIndex = 21
		Me.cmdPrevious.TextAlign = System.Drawing.ContentAlignment.BottomCenter
		Me.ToolTip1.SetToolTip(Me.cmdPrevious, "Previous Item")
		Me.cmdPrevious.UseVisualStyleBackColor = True
		'
		'cmdAutoAllocate
		'
		Me.cmdAutoAllocate.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.cmdAutoAllocate.Enabled = False
		Me.cmdAutoAllocate.Image = CType(resources.GetObject("cmdAutoAllocate.Image"), System.Drawing.Image)
		Me.cmdAutoAllocate.Location = New System.Drawing.Point(790, 55)
		Me.cmdAutoAllocate.Name = "cmdAutoAllocate"
		Me.cmdAutoAllocate.Size = New System.Drawing.Size(41, 41)
		Me.cmdAutoAllocate.TabIndex = 27
		Me.cmdAutoAllocate.TextAlign = System.Drawing.ContentAlignment.BottomCenter
		Me.ToolTip1.SetToolTip(Me.cmdAutoAllocate, "Auto-Allocate all Items")
		Me.cmdAutoAllocate.UseVisualStyleBackColor = True
		'
		'cmdSubItemSearch
		'
		Me.cmdSubItemSearch.BackColor = System.Drawing.SystemColors.Control
		Me.cmdSubItemSearch.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdSubItemSearch.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmdSubItemSearch.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdSubItemSearch.Image = CType(resources.GetObject("cmdSubItemSearch.Image"), System.Drawing.Image)
		Me.cmdSubItemSearch.Location = New System.Drawing.Point(222, 20)
		Me.cmdSubItemSearch.Name = "cmdSubItemSearch"
		Me.cmdSubItemSearch.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdSubItemSearch.Size = New System.Drawing.Size(29, 27)
		Me.cmdSubItemSearch.TabIndex = 2
		Me.cmdSubItemSearch.TextAlign = System.Drawing.ContentAlignment.BottomCenter
		Me.ToolTip1.SetToolTip(Me.cmdSubItemSearch, "Search for Substitution Item")
		Me.cmdSubItemSearch.UseVisualStyleBackColor = True
		'
		'cmdSubstitute
		'
		Me.cmdSubstitute.BackColor = System.Drawing.SystemColors.Control
		Me.cmdSubstitute.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdSubstitute.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmdSubstitute.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdSubstitute.Image = CType(resources.GetObject("cmdSubstitute.Image"), System.Drawing.Image)
		Me.cmdSubstitute.Location = New System.Drawing.Point(729, 16)
		Me.cmdSubstitute.Name = "cmdSubstitute"
		Me.cmdSubstitute.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdSubstitute.Size = New System.Drawing.Size(41, 41)
		Me.cmdSubstitute.TabIndex = 4
		Me.cmdSubstitute.TextAlign = System.Drawing.ContentAlignment.BottomCenter
		Me.ToolTip1.SetToolTip(Me.cmdSubstitute, "Make the Substitution")
		Me.cmdSubstitute.UseVisualStyleBackColor = True
		'
		'cmdShowNotAvailable
		'
		Me.cmdShowNotAvailable.BackColor = System.Drawing.SystemColors.Control
		Me.cmdShowNotAvailable.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdShowNotAvailable.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmdShowNotAvailable.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdShowNotAvailable.Image = CType(resources.GetObject("cmdShowNotAvailable.Image"), System.Drawing.Image)
		Me.cmdShowNotAvailable.Location = New System.Drawing.Point(682, 16)
		Me.cmdShowNotAvailable.Name = "cmdShowNotAvailable"
		Me.cmdShowNotAvailable.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdShowNotAvailable.Size = New System.Drawing.Size(41, 41)
		Me.cmdShowNotAvailable.TabIndex = 3
		Me.cmdShowNotAvailable.TextAlign = System.Drawing.ContentAlignment.BottomCenter
		Me.ToolTip1.SetToolTip(Me.cmdShowNotAvailable, "Search for Orders Missing Substitution Item")
		Me.cmdShowNotAvailable.UseVisualStyleBackColor = True
		'
		'checkIncludeWOO
		'
		Me.checkIncludeWOO.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold)
		Me.checkIncludeWOO.Location = New System.Drawing.Point(12, 104)
		Me.checkIncludeWOO.Name = "checkIncludeWOO"
		Me.checkIncludeWOO.Size = New System.Drawing.Size(178, 17)
		Me.checkIncludeWOO.TabIndex = 4
		Me.checkIncludeWOO.Text = "Include Warehouse On Order"
		Me.ToolTip1.SetToolTip(Me.checkIncludeWOO, resources.GetString("checkIncludeWOO.ToolTip"))
		Me.checkIncludeWOO.UseVisualStyleBackColor = True
		'
		'chkSubteam
		'
		Me.chkSubteam.AutoSize = True
		Me.chkSubteam.Location = New System.Drawing.Point(90, 75)
		Me.chkSubteam.Name = "chkSubteam"
		Me.chkSubteam.Size = New System.Drawing.Size(120, 18)
		Me.chkSubteam.TabIndex = 28
		Me.chkSubteam.Text = "Show All Subteams"
		Me.ToolTip1.SetToolTip(Me.chkSubteam, "Show All Subteams including inactive")
		Me.chkSubteam.UseVisualStyleBackColor = True
		'
		'fraTransfers
		'
		Me.fraTransfers.BackColor = System.Drawing.SystemColors.Control
		Me.fraTransfers.Controls.Add(Me.optAllOrders)
		Me.fraTransfers.Controls.Add(Me.optNonRetail)
		Me.fraTransfers.Controls.Add(Me.optRetail)
		Me.fraTransfers.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.fraTransfers.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraTransfers.Location = New System.Drawing.Point(323, 9)
		Me.fraTransfers.Name = "fraTransfers"
		Me.fraTransfers.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fraTransfers.Size = New System.Drawing.Size(90, 73)
		Me.fraTransfers.TabIndex = 2
		Me.fraTransfers.TabStop = False
		'
		'optAllOrders
		'
		Me.optAllOrders.AutoSize = True
		Me.optAllOrders.BackColor = System.Drawing.SystemColors.Control
		Me.optAllOrders.Cursor = System.Windows.Forms.Cursors.Default
		Me.optAllOrders.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.optAllOrders.ForeColor = System.Drawing.SystemColors.ControlText
		Me.optAllOrders.Location = New System.Drawing.Point(6, 16)
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
		Me.optNonRetail.Location = New System.Drawing.Point(6, 32)
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
		Me.optRetail.Location = New System.Drawing.Point(6, 48)
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
		Me.fraPreOrder.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.fraPreOrder.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraPreOrder.Location = New System.Drawing.Point(419, 9)
		Me.fraPreOrder.Name = "fraPreOrder"
		Me.fraPreOrder.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fraPreOrder.Size = New System.Drawing.Size(120, 73)
		Me.fraPreOrder.TabIndex = 5
		Me.fraPreOrder.TabStop = False
		'
		'_optPreOrder_0
		'
		Me._optPreOrder_0.AutoSize = True
		Me._optPreOrder_0.BackColor = System.Drawing.SystemColors.Control
		Me._optPreOrder_0.Cursor = System.Windows.Forms.Cursors.Default
		Me._optPreOrder_0.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me._optPreOrder_0.ForeColor = System.Drawing.SystemColors.ControlText
		Me.optPreOrder.SetIndex(Me._optPreOrder_0, CType(0, Short))
		Me._optPreOrder_0.Location = New System.Drawing.Point(8, 16)
		Me._optPreOrder_0.Name = "_optPreOrder_0"
		Me._optPreOrder_0.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._optPreOrder_0.Size = New System.Drawing.Size(39, 18)
		Me._optPreOrder_0.TabIndex = 6
		Me._optPreOrder_0.TabStop = True
		Me._optPreOrder_0.Text = "All"
		Me._optPreOrder_0.UseVisualStyleBackColor = False
		'
		'_optPreOrder_1
		'
		Me._optPreOrder_1.AutoSize = True
		Me._optPreOrder_1.BackColor = System.Drawing.SystemColors.Control
		Me._optPreOrder_1.Cursor = System.Windows.Forms.Cursors.Default
		Me._optPreOrder_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me._optPreOrder_1.ForeColor = System.Drawing.SystemColors.ControlText
		Me.optPreOrder.SetIndex(Me._optPreOrder_1, CType(1, Short))
		Me._optPreOrder_1.Location = New System.Drawing.Point(8, 32)
		Me._optPreOrder_1.Name = "_optPreOrder_1"
		Me._optPreOrder_1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._optPreOrder_1.Size = New System.Drawing.Size(80, 18)
		Me._optPreOrder_1.TabIndex = 7
		Me._optPreOrder_1.TabStop = True
		Me._optPreOrder_1.Text = "Pre-Order"
		Me._optPreOrder_1.UseVisualStyleBackColor = False
		'
		'_optPreOrder_2
		'
		Me._optPreOrder_2.AutoSize = True
		Me._optPreOrder_2.BackColor = System.Drawing.SystemColors.Control
		Me._optPreOrder_2.Checked = True
		Me._optPreOrder_2.Cursor = System.Windows.Forms.Cursors.Default
		Me._optPreOrder_2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me._optPreOrder_2.ForeColor = System.Drawing.SystemColors.ControlText
		Me.optPreOrder.SetIndex(Me._optPreOrder_2, CType(2, Short))
		Me._optPreOrder_2.Location = New System.Drawing.Point(8, 48)
		Me._optPreOrder_2.Name = "_optPreOrder_2"
		Me._optPreOrder_2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._optPreOrder_2.Size = New System.Drawing.Size(104, 18)
		Me._optPreOrder_2.TabIndex = 8
		Me._optPreOrder_2.TabStop = True
		Me._optPreOrder_2.Text = "Non Pre-Order"
		Me._optPreOrder_2.UseVisualStyleBackColor = False
		'
		'fraBOH
		'
		Me.fraBOH.BackColor = System.Drawing.SystemColors.Control
		Me.fraBOH.Controls.Add(Me.checkMultiPackOnly)
		Me.fraBOH.Controls.Add(Me._optBOH_4)
		Me.fraBOH.Controls.Add(Me._optBOH_3)
		Me.fraBOH.Controls.Add(Me._optBOH_2)
		Me.fraBOH.Controls.Add(Me._optBOH_1)
		Me.fraBOH.Controls.Add(Me._optBOH_0)
		Me.fraBOH.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.fraBOH.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraBOH.Location = New System.Drawing.Point(545, 9)
		Me.fraBOH.Name = "fraBOH"
		Me.fraBOH.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fraBOH.Size = New System.Drawing.Size(149, 73)
		Me.fraBOH.TabIndex = 9
		Me.fraBOH.TabStop = False
		'
		'checkMultiPackOnly
		'
		Me.checkMultiPackOnly.AutoSize = True
		Me.checkMultiPackOnly.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.checkMultiPackOnly.Location = New System.Drawing.Point(73, 16)
		Me.checkMultiPackOnly.Name = "checkMultiPackOnly"
		Me.checkMultiPackOnly.Size = New System.Drawing.Size(70, 17)
		Me.checkMultiPackOnly.TabIndex = 28
		Me.checkMultiPackOnly.Text = "Pack > 1"
		Me.checkMultiPackOnly.UseVisualStyleBackColor = True
		'
		'_optBOH_4
		'
		Me._optBOH_4.AutoSize = True
		Me._optBOH_4.BackColor = System.Drawing.SystemColors.Control
		Me._optBOH_4.Cursor = System.Windows.Forms.Cursors.Default
		Me._optBOH_4.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me._optBOH_4.ForeColor = System.Drawing.SystemColors.ControlText
		Me.optBOH.SetIndex(Me._optBOH_4, CType(4, Short))
		Me._optBOH_4.Location = New System.Drawing.Point(73, 48)
		Me._optBOH_4.Name = "_optBOH_4"
		Me._optBOH_4.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._optBOH_4.Size = New System.Drawing.Size(67, 18)
		Me._optBOH_4.TabIndex = 14
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
		Me._optBOH_3.Location = New System.Drawing.Point(74, 32)
		Me._optBOH_3.Name = "_optBOH_3"
		Me._optBOH_3.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._optBOH_3.Size = New System.Drawing.Size(61, 18)
		Me._optBOH_3.TabIndex = 13
		Me._optBOH_3.TabStop = True
		Me._optBOH_3.Text = "Diff < 0"
		Me._optBOH_3.UseVisualStyleBackColor = False
		'
		'_optBOH_2
		'
		Me._optBOH_2.AutoSize = True
		Me._optBOH_2.BackColor = System.Drawing.SystemColors.Control
		Me._optBOH_2.Cursor = System.Windows.Forms.Cursors.Default
		Me._optBOH_2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me._optBOH_2.ForeColor = System.Drawing.SystemColors.ControlText
		Me.optBOH.SetIndex(Me._optBOH_2, CType(2, Short))
		Me._optBOH_2.Location = New System.Drawing.Point(6, 48)
		Me._optBOH_2.Name = "_optBOH_2"
		Me._optBOH_2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._optBOH_2.Size = New System.Drawing.Size(67, 18)
		Me._optBOH_2.TabIndex = 12
		Me._optBOH_2.TabStop = True
		Me._optBOH_2.Text = "Diff <= 0"
		Me._optBOH_2.UseVisualStyleBackColor = False
		'
		'_optBOH_1
		'
		Me._optBOH_1.AutoSize = True
		Me._optBOH_1.BackColor = System.Drawing.SystemColors.Control
		Me._optBOH_1.Cursor = System.Windows.Forms.Cursors.Default
		Me._optBOH_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me._optBOH_1.ForeColor = System.Drawing.SystemColors.ControlText
		Me.optBOH.SetIndex(Me._optBOH_1, CType(1, Short))
		Me._optBOH_1.Location = New System.Drawing.Point(6, 32)
		Me._optBOH_1.Name = "_optBOH_1"
		Me._optBOH_1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._optBOH_1.Size = New System.Drawing.Size(61, 18)
		Me._optBOH_1.TabIndex = 11
		Me._optBOH_1.TabStop = True
		Me._optBOH_1.Text = "Diff > 0"
		Me._optBOH_1.UseVisualStyleBackColor = False
		'
		'_optBOH_0
		'
		Me._optBOH_0.AutoSize = True
		Me._optBOH_0.BackColor = System.Drawing.SystemColors.Control
		Me._optBOH_0.Checked = True
		Me._optBOH_0.Cursor = System.Windows.Forms.Cursors.Default
		Me._optBOH_0.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me._optBOH_0.ForeColor = System.Drawing.SystemColors.ControlText
		Me.optBOH.SetIndex(Me._optBOH_0, CType(0, Short))
		Me._optBOH_0.Location = New System.Drawing.Point(6, 16)
		Me._optBOH_0.Name = "_optBOH_0"
		Me._optBOH_0.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._optBOH_0.Size = New System.Drawing.Size(39, 18)
		Me._optBOH_0.TabIndex = 10
		Me._optBOH_0.TabStop = True
		Me._optBOH_0.Text = "All"
		Me._optBOH_0.UseVisualStyleBackColor = False
		'
		'fraAlloc
		'
		Me.fraAlloc.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.fraAlloc.BackColor = System.Drawing.SystemColors.Control
		Me.fraAlloc.Controls.Add(Me.grpSubstitute)
		Me.fraAlloc.Controls.Add(Me.ugrdItem)
		Me.fraAlloc.Controls.Add(Me.cmdOrders)
		Me.fraAlloc.Controls.Add(Me.cmdAutoAllocate)
		Me.fraAlloc.Controls.Add(Me.cmdWarehouseSend)
		Me.fraAlloc.Controls.Add(Me.ugrdAlloc)
		Me.fraAlloc.Controls.Add(Me.cmdSearch)
		Me.fraAlloc.Controls.Add(Me.cmdNext)
		Me.fraAlloc.Controls.Add(Me.cmdPrevious)
		Me.fraAlloc.Controls.Add(Me._lblLabel_3)
		Me.fraAlloc.Controls.Add(Me._lblLabel_2)
		Me.fraAlloc.Controls.Add(Me.lblItemDesc)
		Me.fraAlloc.Controls.Add(Me.lblIdentifier)
		Me.fraAlloc.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.fraAlloc.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraAlloc.Location = New System.Drawing.Point(8, 142)
		Me.fraAlloc.Name = "fraAlloc"
		Me.fraAlloc.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fraAlloc.Size = New System.Drawing.Size(837, 560)
		Me.fraAlloc.TabIndex = 17
		Me.fraAlloc.TabStop = False
		Me.fraAlloc.Text = "Item"
		'
		'grpSubstitute
		'
		Me.grpSubstitute.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
		Me.grpSubstitute.Controls.Add(Me.txtSubstituteIdentifier)
		Me.grpSubstitute.Controls.Add(Me.cmdShowNotAvailable)
		Me.grpSubstitute.Controls.Add(Me.cmdSubstitute)
		Me.grpSubstitute.Controls.Add(Me.cmdSubItemSearch)
		Me.grpSubstitute.Controls.Add(Me._lblLabel_5)
		Me.grpSubstitute.Controls.Add(Me._lblLabel_4)
		Me.grpSubstitute.Controls.Add(Me.lblSubIdentifierDesc)
		Me.grpSubstitute.Enabled = False
		Me.grpSubstitute.Location = New System.Drawing.Point(6, 488)
		Me.grpSubstitute.Name = "grpSubstitute"
		Me.grpSubstitute.Size = New System.Drawing.Size(776, 63)
		Me.grpSubstitute.TabIndex = 34
		Me.grpSubstitute.TabStop = False
		Me.grpSubstitute.Text = "Substitution"
		'
		'txtSubstituteIdentifier
		'
		Me.txtSubstituteIdentifier.Location = New System.Drawing.Point(76, 25)
		Me.txtSubstituteIdentifier.MaxLength = 13
		Me.txtSubstituteIdentifier.Name = "txtSubstituteIdentifier"
		Me.txtSubstituteIdentifier.Size = New System.Drawing.Size(141, 20)
		Me.txtSubstituteIdentifier.TabIndex = 1
		'
		'_lblLabel_5
		'
		Me._lblLabel_5.AutoSize = True
		Me._lblLabel_5.BackColor = System.Drawing.Color.Transparent
		Me._lblLabel_5.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblLabel_5.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me._lblLabel_5.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblLabel_5.Location = New System.Drawing.Point(281, 28)
		Me._lblLabel_5.Name = "_lblLabel_5"
		Me._lblLabel_5.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblLabel_5.Size = New System.Drawing.Size(40, 14)
		Me._lblLabel_5.TabIndex = 34
		Me._lblLabel_5.Text = "Desc :"
		Me._lblLabel_5.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'_lblLabel_4
		'
		Me._lblLabel_4.AutoSize = True
		Me._lblLabel_4.BackColor = System.Drawing.Color.Transparent
		Me._lblLabel_4.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblLabel_4.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me._lblLabel_4.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblLabel_4.Location = New System.Drawing.Point(7, 27)
		Me._lblLabel_4.Name = "_lblLabel_4"
		Me._lblLabel_4.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblLabel_4.Size = New System.Drawing.Size(63, 14)
		Me._lblLabel_4.TabIndex = 33
		Me._lblLabel_4.Text = "Identifier :"
		Me._lblLabel_4.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lblSubIdentifierDesc
		'
		Me.lblSubIdentifierDesc.BackColor = System.Drawing.Color.Transparent
		Me.lblSubIdentifierDesc.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.lblSubIdentifierDesc.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblSubIdentifierDesc.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblSubIdentifierDesc.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblSubIdentifierDesc.Location = New System.Drawing.Point(327, 25)
		Me.lblSubIdentifierDesc.Name = "lblSubIdentifierDesc"
		Me.lblSubIdentifierDesc.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblSubIdentifierDesc.Size = New System.Drawing.Size(242, 21)
		Me.lblSubIdentifierDesc.TabIndex = 32
		'
		'ugrdItem
		'
		Me.ugrdItem.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption
		Me.ugrdItem.DisplayLayout.Appearance = Appearance1
		Me.ugrdItem.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
		UltraGridColumn1.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
		Appearance2.TextHAlignAsString = "Right"
		UltraGridColumn1.CellAppearance = Appearance2
		UltraGridColumn1.Format = "####0.##"
		Appearance3.TextHAlignAsString = "Right"
		UltraGridColumn1.Header.Appearance = Appearance3
		UltraGridColumn1.Header.Caption = "Pack Size"
		UltraGridColumn1.Header.VisiblePosition = 0
		UltraGridColumn1.SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled
		UltraGridColumn2.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
		Appearance4.TextHAlignAsString = "Right"
		UltraGridColumn2.CellAppearance = Appearance4
		UltraGridColumn2.Format = "####0.##"
		Appearance5.TextHAlignAsString = "Right"
		UltraGridColumn2.Header.Appearance = Appearance5
		UltraGridColumn2.Header.VisiblePosition = 1
		UltraGridColumn2.SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled
		UltraGridColumn3.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
		Appearance6.TextHAlignAsString = "Right"
		UltraGridColumn3.CellAppearance = Appearance6
		UltraGridColumn3.Format = "####0.##"
		Appearance7.TextHAlignAsString = "Right"
		UltraGridColumn3.Header.Appearance = Appearance7
		UltraGridColumn3.Header.VisiblePosition = 2
		UltraGridColumn3.SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled
		UltraGridColumn4.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
		Appearance8.TextHAlignAsString = "Right"
		UltraGridColumn4.CellAppearance = Appearance8
		UltraGridColumn4.Format = "####0.##"
		Appearance9.TextHAlignAsString = "Right"
		UltraGridColumn4.Header.Appearance = Appearance9
		UltraGridColumn4.Header.VisiblePosition = 3
		UltraGridColumn4.SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled
		UltraGridColumn5.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
		Appearance10.TextHAlignAsString = "Right"
		UltraGridColumn5.CellAppearance = Appearance10
		UltraGridColumn5.Format = "####0.##"
		Appearance11.TextHAlignAsString = "Right"
		UltraGridColumn5.Header.Appearance = Appearance11
		UltraGridColumn5.Header.VisiblePosition = 4
		UltraGridColumn5.SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled
		UltraGridColumn6.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
		Appearance12.TextHAlignAsString = "Right"
		UltraGridColumn6.CellAppearance = Appearance12
		UltraGridColumn6.Format = "####0.##"
		Appearance13.TextHAlignAsString = "Right"
		UltraGridColumn6.Header.Appearance = Appearance13
		UltraGridColumn6.Header.VisiblePosition = 5
		UltraGridColumn6.SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled
		UltraGridBand1.Columns.AddRange(New Object() {UltraGridColumn1, UltraGridColumn2, UltraGridColumn3, UltraGridColumn4, UltraGridColumn5, UltraGridColumn6})
		UltraGridBand1.RowLayoutStyle = Infragistics.Win.UltraWinGrid.RowLayoutStyle.ColumnLayout
		Me.ugrdItem.DisplayLayout.BandsSerializer.Add(UltraGridBand1)
		Me.ugrdItem.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
		Appearance14.FontData.BoldAsString = "True"
		Me.ugrdItem.DisplayLayout.CaptionAppearance = Appearance14
		Me.ugrdItem.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
		Appearance15.BackColor = System.Drawing.SystemColors.ActiveBorder
		Appearance15.BackColor2 = System.Drawing.SystemColors.ControlDark
		Appearance15.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
		Appearance15.BorderColor = System.Drawing.SystemColors.Window
		Me.ugrdItem.DisplayLayout.GroupByBox.Appearance = Appearance15
		Appearance16.ForeColor = System.Drawing.SystemColors.GrayText
		Me.ugrdItem.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance16
		Me.ugrdItem.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
		Me.ugrdItem.DisplayLayout.GroupByBox.Hidden = True
		Appearance17.BackColor = System.Drawing.SystemColors.ControlLightLight
		Appearance17.BackColor2 = System.Drawing.SystemColors.Control
		Appearance17.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
		Appearance17.ForeColor = System.Drawing.SystemColors.GrayText
		Me.ugrdItem.DisplayLayout.GroupByBox.PromptAppearance = Appearance17
		Me.ugrdItem.DisplayLayout.MaxColScrollRegions = 1
		Me.ugrdItem.DisplayLayout.MaxRowScrollRegions = 1
		Appearance18.BackColor = System.Drawing.SystemColors.Window
		Appearance18.ForeColor = System.Drawing.SystemColors.ControlText
		Me.ugrdItem.DisplayLayout.Override.ActiveCellAppearance = Appearance18
		Appearance19.ForeColor = System.Drawing.SystemColors.GrayText
		Me.ugrdItem.DisplayLayout.Override.ActiveRowAppearance = Appearance19
		Me.ugrdItem.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
		Me.ugrdItem.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
		Appearance20.BackColor = System.Drawing.SystemColors.Window
		Me.ugrdItem.DisplayLayout.Override.CardAreaAppearance = Appearance20
		Appearance21.BorderColor = System.Drawing.Color.Silver
		Appearance21.FontData.BoldAsString = "True"
		Appearance21.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
		Me.ugrdItem.DisplayLayout.Override.CellAppearance = Appearance21
		Me.ugrdItem.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
		Me.ugrdItem.DisplayLayout.Override.CellPadding = 0
		Appearance22.FontData.BoldAsString = "True"
		Me.ugrdItem.DisplayLayout.Override.FixedHeaderAppearance = Appearance22
		Appearance23.BackColor = System.Drawing.SystemColors.Control
		Appearance23.BackColor2 = System.Drawing.SystemColors.ControlDark
		Appearance23.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
		Appearance23.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
		Appearance23.BorderColor = System.Drawing.SystemColors.Window
		Me.ugrdItem.DisplayLayout.Override.GroupByRowAppearance = Appearance23
		Appearance24.FontData.BoldAsString = "True"
		Appearance24.TextHAlignAsString = "Left"
		Me.ugrdItem.DisplayLayout.Override.HeaderAppearance = Appearance24
		Me.ugrdItem.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle
		Me.ugrdItem.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
		Appearance25.BackColor = System.Drawing.SystemColors.Control
		Me.ugrdItem.DisplayLayout.Override.RowAlternateAppearance = Appearance25
		Appearance26.BackColor = System.Drawing.SystemColors.Window
		Appearance26.BorderColor = System.Drawing.Color.Silver
		Me.ugrdItem.DisplayLayout.Override.RowAppearance = Appearance26
		Me.ugrdItem.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[True]
		Me.ugrdItem.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed
		Me.ugrdItem.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None
		Me.ugrdItem.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None
		Me.ugrdItem.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.[Single]
		Appearance27.BackColor = System.Drawing.SystemColors.ControlLight
		Me.ugrdItem.DisplayLayout.Override.TemplateAddRowAppearance = Appearance27
		Me.ugrdItem.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
		Me.ugrdItem.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
		Me.ugrdItem.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand
		Me.ugrdItem.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
		Me.ugrdItem.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.ugrdItem.Location = New System.Drawing.Point(6, 55)
		Me.ugrdItem.Name = "ugrdItem"
		Me.ugrdItem.Size = New System.Drawing.Size(778, 99)
		Me.ugrdItem.TabIndex = 32
		'
		'ugrdAlloc
		'
		Me.ugrdAlloc.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.ugrdAlloc.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
		UltraGridColumn7.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.VisibleRows
		UltraGridColumn7.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
		UltraGridColumn7.Header.VisiblePosition = 5
		UltraGridColumn7.Hidden = True
		UltraGridColumn7.Width = 99
		UltraGridColumn8.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
		UltraGridColumn8.Header.VisiblePosition = 1
		UltraGridColumn8.Hidden = True
		UltraGridColumn8.Width = 90
		UltraGridColumn9.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.VisibleRows
		UltraGridColumn9.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
		UltraGridColumn9.Header.Caption = "Receiving Loc"
		UltraGridColumn9.Header.VisiblePosition = 0
		UltraGridColumn9.TabStop = False
		UltraGridColumn9.Width = 96
		UltraGridColumn10.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
		Appearance28.TextHAlignAsString = "Right"
		UltraGridColumn10.CellAppearance = Appearance28
		UltraGridColumn10.Format = "###.#"
		Appearance29.TextHAlignAsString = "Right"
		UltraGridColumn10.Header.Appearance = Appearance29
		UltraGridColumn10.Header.Caption = "Order Number"
		UltraGridColumn10.Header.VisiblePosition = 2
		UltraGridColumn10.TabStop = False
		UltraGridColumn10.Width = 219
		UltraGridColumn11.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
		Appearance30.TextHAlignAsString = "Right"
		UltraGridColumn11.CellAppearance = Appearance30
		UltraGridColumn11.Format = "##0.#"
		Appearance31.TextHAlignAsString = "Right"
		UltraGridColumn11.Header.Appearance = Appearance31
		UltraGridColumn11.Header.Caption = "OO"
		UltraGridColumn11.Header.VisiblePosition = 4
		UltraGridColumn11.TabStop = False
		UltraGridColumn11.Width = 56
		Appearance32.TextHAlignAsString = "Right"
		UltraGridColumn12.CellAppearance = Appearance32
		UltraGridColumn12.Format = "###.#"
		Appearance33.TextHAlignAsString = "Right"
		UltraGridColumn12.Header.Appearance = Appearance33
		UltraGridColumn12.Header.Caption = "Pack Size"
		UltraGridColumn12.Header.VisiblePosition = 6
		UltraGridColumn12.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList
		UltraGridColumn12.Width = 88
		Appearance34.TextHAlignAsString = "Right"
		UltraGridColumn13.CellAppearance = Appearance34
		UltraGridColumn13.Format = "##0.#"
		Appearance35.TextHAlignAsString = "Right"
		UltraGridColumn13.Header.Appearance = Appearance35
		UltraGridColumn13.Header.Caption = "Allocation"
		UltraGridColumn13.Header.VisiblePosition = 7
		UltraGridColumn13.MaskInput = "nnn"
		UltraGridColumn13.Width = 79
		UltraGridColumn14.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
		UltraGridColumn14.Header.VisiblePosition = 8
		UltraGridColumn14.Hidden = True
		UltraGridColumn14.Width = 82
		UltraGridColumn15.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
		UltraGridColumn15.Header.VisiblePosition = 9
		UltraGridColumn15.Hidden = True
		UltraGridColumn15.Width = 104
		UltraGridColumn16.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.VisibleRows
		UltraGridColumn16.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
		UltraGridColumn16.Header.Caption = "Receiving Subteam"
		UltraGridColumn16.Header.VisiblePosition = 3
		UltraGridColumn16.Width = 219
		UltraGridBand2.Columns.AddRange(New Object() {UltraGridColumn7, UltraGridColumn8, UltraGridColumn9, UltraGridColumn10, UltraGridColumn11, UltraGridColumn12, UltraGridColumn13, UltraGridColumn14, UltraGridColumn15, UltraGridColumn16})
		UltraGridBand2.ExcludeFromColumnChooser = Infragistics.Win.UltraWinGrid.ExcludeFromColumnChooser.[True]
		UltraGridBand2.Expandable = False
		UltraGridBand2.GroupHeadersVisible = False
		UltraGridBand2.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[True]
		Me.ugrdAlloc.DisplayLayout.BandsSerializer.Add(UltraGridBand2)
		Me.ugrdAlloc.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
		Me.ugrdAlloc.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
		Me.ugrdAlloc.DisplayLayout.ColScrollRegions.Add(ColScrollRegion1)
		Me.ugrdAlloc.DisplayLayout.ColScrollRegions.Add(ColScrollRegion2)
		Me.ugrdAlloc.DisplayLayout.ColScrollRegions.Add(ColScrollRegion3)
		Me.ugrdAlloc.DisplayLayout.ColScrollRegions.Add(ColScrollRegion4)
		Me.ugrdAlloc.DisplayLayout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.[False]
		Appearance36.BackColor = System.Drawing.SystemColors.ActiveBorder
		Appearance36.BackColor2 = System.Drawing.SystemColors.ControlDark
		Appearance36.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
		Appearance36.BorderColor = System.Drawing.SystemColors.Window
		Me.ugrdAlloc.DisplayLayout.GroupByBox.Appearance = Appearance36
		Appearance37.ForeColor = System.Drawing.SystemColors.GrayText
		Me.ugrdAlloc.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance37
		Me.ugrdAlloc.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
		Me.ugrdAlloc.DisplayLayout.GroupByBox.Hidden = True
		Appearance38.BackColor = System.Drawing.SystemColors.ControlLightLight
		Appearance38.BackColor2 = System.Drawing.SystemColors.Control
		Appearance38.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
		Appearance38.ForeColor = System.Drawing.SystemColors.GrayText
		Me.ugrdAlloc.DisplayLayout.GroupByBox.PromptAppearance = Appearance38
		Me.ugrdAlloc.DisplayLayout.MaxBandDepth = 1
		Me.ugrdAlloc.DisplayLayout.MaxColScrollRegions = 1
		Me.ugrdAlloc.DisplayLayout.MaxRowScrollRegions = 1
		Me.ugrdAlloc.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No
		Me.ugrdAlloc.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.NotAllowed
		Me.ugrdAlloc.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed
		Me.ugrdAlloc.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.[False]
		Me.ugrdAlloc.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
		Me.ugrdAlloc.DisplayLayout.Override.AllowMultiCellOperations = Infragistics.Win.UltraWinGrid.AllowMultiCellOperation.None
		Me.ugrdAlloc.DisplayLayout.Override.AllowRowSummaries = Infragistics.Win.UltraWinGrid.AllowRowSummaries.[False]
		Me.ugrdAlloc.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.[True]
		Me.ugrdAlloc.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
		Me.ugrdAlloc.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
		Appearance40.BackColor = System.Drawing.SystemColors.Window
		Me.ugrdAlloc.DisplayLayout.Override.CardAreaAppearance = Appearance40
		Appearance41.BorderColor = System.Drawing.Color.Silver
		Appearance41.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
		Me.ugrdAlloc.DisplayLayout.Override.CellAppearance = Appearance41
		Me.ugrdAlloc.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
		Me.ugrdAlloc.DisplayLayout.Override.CellPadding = 0
		Appearance42.BackColor = System.Drawing.SystemColors.Control
		Appearance42.BackColor2 = System.Drawing.SystemColors.ControlDark
		Appearance42.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
		Appearance42.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
		Appearance42.BorderColor = System.Drawing.SystemColors.Window
		Me.ugrdAlloc.DisplayLayout.Override.GroupByRowAppearance = Appearance42
		Appearance43.TextHAlignAsString = "Left"
		Me.ugrdAlloc.DisplayLayout.Override.HeaderAppearance = Appearance43
		Me.ugrdAlloc.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti
		Me.ugrdAlloc.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
		Me.ugrdAlloc.DisplayLayout.Override.MaxSelectedCells = 1
		Appearance44.BackColor = System.Drawing.SystemColors.Control
		Me.ugrdAlloc.DisplayLayout.Override.RowAlternateAppearance = Appearance44
		Me.ugrdAlloc.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[True]
		Appearance48.ForeColor = System.Drawing.Color.White
		Me.ugrdAlloc.DisplayLayout.Override.SelectedCellAppearance = Appearance48
		Appearance46.ForeColor = System.Drawing.Color.White
		Me.ugrdAlloc.DisplayLayout.Override.SelectedRowAppearance = Appearance46
		Me.ugrdAlloc.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Extended
		Appearance45.BackColor = System.Drawing.SystemColors.ControlLight
		Me.ugrdAlloc.DisplayLayout.Override.TemplateAddRowAppearance = Appearance45
		Me.ugrdAlloc.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
		Me.ugrdAlloc.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
		ValueList1.Key = "PackSize"
		Me.ugrdAlloc.DisplayLayout.ValueLists.AddRange(New Infragistics.Win.ValueList() {ValueList1})
		Me.ugrdAlloc.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand
		Me.ugrdAlloc.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.ugrdAlloc.Location = New System.Drawing.Point(6, 160)
		Me.ugrdAlloc.Name = "ugrdAlloc"
		Me.ugrdAlloc.Size = New System.Drawing.Size(778, 322)
		Me.ugrdAlloc.TabIndex = 31
		Me.ugrdAlloc.Text = "Order - List View"
		Me.ugrdAlloc.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnCellChangeOrLostFocus
		'
		'_lblLabel_3
		'
		Me._lblLabel_3.AutoSize = True
		Me._lblLabel_3.BackColor = System.Drawing.Color.Transparent
		Me._lblLabel_3.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblLabel_3.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me._lblLabel_3.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblLabel.SetIndex(Me._lblLabel_3, CType(3, Short))
		Me._lblLabel_3.Location = New System.Drawing.Point(269, 24)
		Me._lblLabel_3.Name = "_lblLabel_3"
		Me._lblLabel_3.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblLabel_3.Size = New System.Drawing.Size(40, 14)
		Me._lblLabel_3.TabIndex = 30
		Me._lblLabel_3.Text = "Desc :"
		Me._lblLabel_3.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'_lblLabel_2
		'
		Me._lblLabel_2.AutoSize = True
		Me._lblLabel_2.BackColor = System.Drawing.Color.Transparent
		Me._lblLabel_2.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblLabel_2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me._lblLabel_2.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblLabel.SetIndex(Me._lblLabel_2, CType(2, Short))
		Me._lblLabel_2.Location = New System.Drawing.Point(16, 24)
		Me._lblLabel_2.Name = "_lblLabel_2"
		Me._lblLabel_2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblLabel_2.Size = New System.Drawing.Size(63, 14)
		Me._lblLabel_2.TabIndex = 29
		Me._lblLabel_2.Text = "Identifier :"
		Me._lblLabel_2.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'lblItemDesc
		'
		Me.lblItemDesc.BackColor = System.Drawing.Color.Transparent
		Me.lblItemDesc.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.lblItemDesc.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblItemDesc.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblItemDesc.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblItemDesc.Location = New System.Drawing.Point(312, 24)
		Me.lblItemDesc.Name = "lblItemDesc"
		Me.lblItemDesc.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblItemDesc.Size = New System.Drawing.Size(325, 21)
		Me.lblItemDesc.TabIndex = 28
		'
		'lblIdentifier
		'
		Me.lblIdentifier.BackColor = System.Drawing.Color.Transparent
		Me.lblIdentifier.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.lblIdentifier.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblIdentifier.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblIdentifier.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblIdentifier.Location = New System.Drawing.Point(80, 24)
		Me.lblIdentifier.Name = "lblIdentifier"
		Me.lblIdentifier.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblIdentifier.Size = New System.Drawing.Size(177, 21)
		Me.lblIdentifier.TabIndex = 27
		'
		'cmbSubTeam
		'
		Me.cmbSubTeam.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
		Me.cmbSubTeam.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
		Me.cmbSubTeam.BackColor = System.Drawing.SystemColors.Window
		Me.cmbSubTeam.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmbSubTeam.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cmbSubTeam.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmbSubTeam.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cmbSubTeam.Location = New System.Drawing.Point(88, 47)
		Me.cmbSubTeam.Name = "cmbSubTeam"
		Me.cmbSubTeam.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmbSubTeam.Size = New System.Drawing.Size(229, 22)
		Me.cmbSubTeam.Sorted = True
		Me.cmbSubTeam.TabIndex = 1
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
		Me.cmbStore.Location = New System.Drawing.Point(88, 23)
		Me.cmbStore.Name = "cmbStore"
		Me.cmbStore.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmbStore.Size = New System.Drawing.Size(229, 22)
		Me.cmbStore.Sorted = True
		Me.cmbStore.TabIndex = 0
		'
		'_lblLabel_1
		'
		Me._lblLabel_1.AutoSize = True
		Me._lblLabel_1.BackColor = System.Drawing.Color.Transparent
		Me._lblLabel_1.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblLabel_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me._lblLabel_1.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblLabel.SetIndex(Me._lblLabel_1, CType(1, Short))
		Me._lblLabel_1.Location = New System.Drawing.Point(15, 50)
		Me._lblLabel_1.Name = "_lblLabel_1"
		Me._lblLabel_1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblLabel_1.Size = New System.Drawing.Size(68, 14)
		Me._lblLabel_1.TabIndex = 26
		Me._lblLabel_1.Text = "Sub-Team :"
		Me._lblLabel_1.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'_lblLabel_0
		'
		Me._lblLabel_0.AutoSize = True
		Me._lblLabel_0.BackColor = System.Drawing.Color.Transparent
		Me._lblLabel_0.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblLabel_0.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me._lblLabel_0.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblLabel.SetIndex(Me._lblLabel_0, CType(0, Short))
		Me._lblLabel_0.Location = New System.Drawing.Point(8, 26)
		Me._lblLabel_0.Name = "_lblLabel_0"
		Me._lblLabel_0.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblLabel_0.Size = New System.Drawing.Size(76, 14)
		Me._lblLabel_0.TabIndex = 25
		Me._lblLabel_0.Text = "Warehouse :"
		Me._lblLabel_0.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'optBOH
		'
		'
		'optPreOrder
		'
		'
		'frmErrorProvider
		'
		Me.frmErrorProvider.ContainerControl = Me
		'
		'grpPOExpectedDate
		'
		Me.grpPOExpectedDate.Controls.Add(Me.Label2)
		Me.grpPOExpectedDate.Controls.Add(Me.Label1)
		Me.grpPOExpectedDate.Controls.Add(Me.dtWOOEnd)
		Me.grpPOExpectedDate.Controls.Add(Me.dtWOOStart)
		Me.grpPOExpectedDate.Enabled = False
		Me.grpPOExpectedDate.Font = New System.Drawing.Font("Segoe UI", 8.25!)
		Me.grpPOExpectedDate.Location = New System.Drawing.Point(202, 80)
		Me.grpPOExpectedDate.Name = "grpPOExpectedDate"
		Me.grpPOExpectedDate.Size = New System.Drawing.Size(492, 54)
		Me.grpPOExpectedDate.TabIndex = 27
		Me.grpPOExpectedDate.TabStop = False
		'
		'Label2
		'
		Me.Label2.AutoSize = True
		Me.Label2.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold)
		Me.Label2.Location = New System.Drawing.Point(257, 25)
		Me.Label2.Name = "Label2"
		Me.Label2.Size = New System.Drawing.Size(107, 13)
		Me.Label2.TabIndex = 3
		Me.Label2.Text = "Expected Date End:"
		'
		'Label1
		'
		Me.Label1.AutoSize = True
		Me.Label1.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold)
		Me.Label1.Location = New System.Drawing.Point(18, 25)
		Me.Label1.Name = "Label1"
		Me.Label1.Size = New System.Drawing.Size(111, 13)
		Me.Label1.TabIndex = 2
		Me.Label1.Text = "Expected Date Start:"
		'
		'dtWOOEnd
		'
		Me.dtWOOEnd.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
		Me.dtWOOEnd.Location = New System.Drawing.Point(369, 21)
		Me.dtWOOEnd.Name = "dtWOOEnd"
		Me.dtWOOEnd.Size = New System.Drawing.Size(109, 22)
		Me.dtWOOEnd.TabIndex = 1
		'
		'dtWOOStart
		'
		Me.dtWOOStart.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
		Me.dtWOOStart.Location = New System.Drawing.Point(134, 21)
		Me.dtWOOStart.Name = "dtWOOStart"
		Me.dtWOOStart.Size = New System.Drawing.Size(109, 22)
		Me.dtWOOStart.TabIndex = 0
		'
		'frmOrdersAllocate
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(859, 714)
		Me.Controls.Add(Me.grpPOExpectedDate)
		Me.Controls.Add(Me.chkSubteam)
		Me.Controls.Add(Me.checkIncludeWOO)
		Me.Controls.Add(Me.cmbSubTeam)
		Me.Controls.Add(Me.cmdCloseOrderingWindow)
		Me.Controls.Add(Me.fraTransfers)
		Me.Controls.Add(Me.fraPreOrder)
		Me.Controls.Add(Me.cmdRefresh)
		Me.Controls.Add(Me.fraBOH)
		Me.Controls.Add(Me.cmdReport)
		Me.Controls.Add(Me.cmdExit)
		Me.Controls.Add(Me.cmbStore)
		Me.Controls.Add(Me._lblLabel_1)
		Me.Controls.Add(Me._lblLabel_0)
		Me.Controls.Add(Me.fraAlloc)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.Location = New System.Drawing.Point(3, 22)
		Me.MinimizeBox = False
		Me.MinimumSize = New System.Drawing.Size(875, 735)
		Me.Name = "frmOrdersAllocate"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "FSA (Fair-Share Allocations)"
		Me.fraTransfers.ResumeLayout(False)
		Me.fraTransfers.PerformLayout()
		Me.fraPreOrder.ResumeLayout(False)
		Me.fraPreOrder.PerformLayout()
		Me.fraBOH.ResumeLayout(False)
		Me.fraBOH.PerformLayout()
		Me.fraAlloc.ResumeLayout(False)
		Me.fraAlloc.PerformLayout()
		Me.grpSubstitute.ResumeLayout(False)
		Me.grpSubstitute.PerformLayout()
		CType(Me.ugrdItem, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.ugrdAlloc, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.optBOH, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.optPreOrder, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.frmErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
		Me.grpPOExpectedDate.ResumeLayout(False)
		Me.grpPOExpectedDate.PerformLayout()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	Friend WithEvents ugrdAlloc As Infragistics.Win.UltraWinGrid.UltraGrid
    Friend WithEvents ugrdItem As Infragistics.Win.UltraWinGrid.UltraGrid
    Friend WithEvents cmdAutoAllocate As System.Windows.Forms.Button
    Friend WithEvents grpSubstitute As System.Windows.Forms.GroupBox
    Public WithEvents _lblLabel_5 As System.Windows.Forms.Label
    Public WithEvents _lblLabel_4 As System.Windows.Forms.Label
    Public WithEvents lblSubIdentifierDesc As System.Windows.Forms.Label
    Public WithEvents cmdSubstitute As System.Windows.Forms.Button
    Public WithEvents cmdSubItemSearch As System.Windows.Forms.Button
    Public WithEvents cmdShowNotAvailable As System.Windows.Forms.Button
    Friend WithEvents txtSubstituteIdentifier As System.Windows.Forms.TextBox
    Friend WithEvents frmErrorProvider As System.Windows.Forms.ErrorProvider
    Friend WithEvents grpPOExpectedDate As System.Windows.Forms.GroupBox
    Friend WithEvents dtWOOStart As System.Windows.Forms.DateTimePicker
    Friend WithEvents checkIncludeWOO As System.Windows.Forms.CheckBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents dtWOOEnd As System.Windows.Forms.DateTimePicker
    Public WithEvents optAllOrders As System.Windows.Forms.RadioButton
    Public WithEvents _optBOH_4 As System.Windows.Forms.RadioButton
    Public WithEvents _optBOH_3 As System.Windows.Forms.RadioButton
    Friend WithEvents checkMultiPackOnly As System.Windows.Forms.CheckBox
	Friend WithEvents chkSubteam As CheckBox
#End Region
End Class