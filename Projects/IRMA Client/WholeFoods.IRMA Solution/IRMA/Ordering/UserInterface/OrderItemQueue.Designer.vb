<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmOrderItemQueue
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
	Public WithEvents cmdItemEdit As System.Windows.Forms.Button
	Public WithEvents imgIcons As System.Windows.Forms.ImageList
	Public WithEvents cmdSelectAll As System.Windows.Forms.Button
	Public WithEvents cmdViewQueue As System.Windows.Forms.Button
	Public WithEvents cmdSubmit As System.Windows.Forms.Button
	Public WithEvents cmdDelete As System.Windows.Forms.Button
	Public WithEvents fraAdmin As System.Windows.Forms.GroupBox
	Public WithEvents chkDiscontinued As System.Windows.Forms.CheckBox
	Public WithEvents cmbTransferFromSubteam As System.Windows.Forms.ComboBox
    Public WithEvents chkCredit As System.Windows.Forms.CheckBox
	Public WithEvents cmdSearch As System.Windows.Forms.Button
	Public WithEvents cmdReset As System.Windows.Forms.Button
	Public WithEvents cmbTransferToSubteam As System.Windows.Forms.ComboBox
	Public WithEvents cmbProductType As System.Windows.Forms.ComboBox
	Public WithEvents cmbPurchasing As System.Windows.Forms.ComboBox
	Public WithEvents optTransfer As System.Windows.Forms.RadioButton
	Public WithEvents optDistribution As System.Windows.Forms.RadioButton
	Public WithEvents optPurchase As System.Windows.Forms.RadioButton
	Public WithEvents fraOrderType As System.Windows.Forms.Panel
	Public WithEvents txtVendor As System.Windows.Forms.TextBox
	Public WithEvents cmdVendorSearch As System.Windows.Forms.Button
	Public WithEvents _lblLabel_2 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_1 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_5 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_4 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_0 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_3 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_9 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_8 As System.Windows.Forms.Label
	Public WithEvents fraOrderHeader As System.Windows.Forms.GroupBox
	Public WithEvents cmdCreateOrder As System.Windows.Forms.Button
	Public WithEvents cmdSelect As System.Windows.Forms.Button
	Public WithEvents cmdExit As System.Windows.Forms.Button
    Public WithEvents lblDisplayCount As System.Windows.Forms.Label
	Public WithEvents lblLabel As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmOrderItemQueue))
        Dim Appearance1 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridBand1 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1)
        Dim UltraGridColumn1 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("OrderItemQueue_ID")
        Dim UltraGridColumn2 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Selected")
        Dim UltraGridColumn3 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Identifier")
        Dim UltraGridColumn4 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Item_Description")
        Dim UltraGridColumn5 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("SubTeamName")
        Dim UltraGridColumn6 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("NotAvailable")
        Dim UltraGridColumn7 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Quantity")
        Dim UltraGridColumn8 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("QuantityUnitName", -1, Nothing, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, False)
        Dim UltraGridColumn9 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("PrimaryVendor")
        Dim UltraGridColumn10 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("UserName")
        Dim UltraGridColumn11 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("CreditReason_ID", -1, 1907985860)
        Dim UltraGridColumn12 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Insert_Date")
        Dim UltraGridColumn13 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Discontinued", 0)
        Dim UltraGridColumn14 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Cost", 1)
        Dim Appearance2 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance3 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance4 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance5 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance6 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance7 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance8 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance9 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance10 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance11 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance12 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance13 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim ValueList1 As Infragistics.Win.ValueList = New Infragistics.Win.ValueList(1907985860)
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdItemEdit = New System.Windows.Forms.Button()
        Me.cmdSelectAll = New System.Windows.Forms.Button()
        Me.cmdViewQueue = New System.Windows.Forms.Button()
        Me.cmdSubmit = New System.Windows.Forms.Button()
        Me.cmdDelete = New System.Windows.Forms.Button()
        Me.cmdSearch = New System.Windows.Forms.Button()
        Me.cmdReset = New System.Windows.Forms.Button()
        Me.cmdVendorSearch = New System.Windows.Forms.Button()
        Me.cmdCreateOrder = New System.Windows.Forms.Button()
        Me.cmdSelect = New System.Windows.Forms.Button()
        Me.cmdExit = New System.Windows.Forms.Button()
        Me.dtpStartDate = New System.Windows.Forms.DateTimePicker()
        Me.btnApplyAllCreditReason = New System.Windows.Forms.Button()
        Me.imgIcons = New System.Windows.Forms.ImageList(Me.components)
        Me.fraAdmin = New System.Windows.Forms.GroupBox()
        Me.fraOrderHeader = New System.Windows.Forms.GroupBox()
        Me.cmbPurchasing = New System.Windows.Forms.ComboBox()
        Me.chkDiscontinued = New System.Windows.Forms.CheckBox()
        Me.cmbTransferFromSubteam = New System.Windows.Forms.ComboBox()
        Me.chkCredit = New System.Windows.Forms.CheckBox()
        Me.cmbTransferToSubteam = New System.Windows.Forms.ComboBox()
        Me.cmbProductType = New System.Windows.Forms.ComboBox()
        Me.fraOrderType = New System.Windows.Forms.Panel()
        Me.optTransfer = New System.Windows.Forms.RadioButton()
        Me.optDistribution = New System.Windows.Forms.RadioButton()
        Me.optPurchase = New System.Windows.Forms.RadioButton()
        Me.txtVendor = New System.Windows.Forms.TextBox()
        Me._lblLabel_2 = New System.Windows.Forms.Label()
        Me._lblLabel_1 = New System.Windows.Forms.Label()
        Me._lblLabel_5 = New System.Windows.Forms.Label()
        Me._lblLabel_4 = New System.Windows.Forms.Label()
        Me._lblLabel_0 = New System.Windows.Forms.Label()
        Me._lblLabel_3 = New System.Windows.Forms.Label()
        Me._lblLabel_9 = New System.Windows.Forms.Label()
        Me._lblLabel_8 = New System.Windows.Forms.Label()
        Me.lblDisplayCount = New System.Windows.Forms.Label()
        Me.lblLabel = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(Me.components)
        Me.grdList = New Infragistics.Win.UltraWinGrid.UltraGrid()
        Me.lblOrderedTotal = New System.Windows.Forms.Label()
        Me.lblOrderedTotalAmount = New System.Windows.Forms.Label()
        Me.fraAdmin.SuspendLayout()
        Me.fraOrderHeader.SuspendLayout()
        Me.fraOrderType.SuspendLayout()
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.grdList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdItemEdit
        '
        Me.cmdItemEdit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdItemEdit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdItemEdit.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdItemEdit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdItemEdit.Image = CType(resources.GetObject("cmdItemEdit.Image"), System.Drawing.Image)
        Me.cmdItemEdit.Location = New System.Drawing.Point(547, 478)
        Me.cmdItemEdit.Name = "cmdItemEdit"
        Me.cmdItemEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdItemEdit.Size = New System.Drawing.Size(43, 41)
        Me.cmdItemEdit.TabIndex = 17
        Me.cmdItemEdit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdItemEdit, "View Item")
        Me.cmdItemEdit.UseVisualStyleBackColor = False
        '
        'cmdSelectAll
        '
        Me.cmdSelectAll.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSelectAll.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSelectAll.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSelectAll.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSelectAll.Location = New System.Drawing.Point(595, 478)
        Me.cmdSelectAll.Name = "cmdSelectAll"
        Me.cmdSelectAll.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSelectAll.Size = New System.Drawing.Size(45, 41)
        Me.cmdSelectAll.TabIndex = 18
        Me.cmdSelectAll.Tag = "Select"
        Me.ToolTip1.SetToolTip(Me.cmdSelectAll, "Select All")
        Me.cmdSelectAll.UseVisualStyleBackColor = False
        '
        'cmdViewQueue
        '
        Me.cmdViewQueue.BackColor = System.Drawing.SystemColors.Control
        Me.cmdViewQueue.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdViewQueue.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdViewQueue.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdViewQueue.Image = CType(resources.GetObject("cmdViewQueue.Image"), System.Drawing.Image)
        Me.cmdViewQueue.Location = New System.Drawing.Point(53, 17)
        Me.cmdViewQueue.Name = "cmdViewQueue"
        Me.cmdViewQueue.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdViewQueue.Size = New System.Drawing.Size(41, 41)
        Me.cmdViewQueue.TabIndex = 15
        Me.cmdViewQueue.Tag = "B"
        Me.cmdViewQueue.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdViewQueue, "View Queue")
        Me.cmdViewQueue.UseVisualStyleBackColor = False
        '
        'cmdSubmit
        '
        Me.cmdSubmit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSubmit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSubmit.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSubmit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSubmit.Image = CType(resources.GetObject("cmdSubmit.Image"), System.Drawing.Image)
        Me.cmdSubmit.Location = New System.Drawing.Point(100, 17)
        Me.cmdSubmit.Name = "cmdSubmit"
        Me.cmdSubmit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSubmit.Size = New System.Drawing.Size(41, 41)
        Me.cmdSubmit.TabIndex = 16
        Me.cmdSubmit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdSubmit, "Commit Changes to Queue")
        Me.cmdSubmit.UseVisualStyleBackColor = False
        '
        'cmdDelete
        '
        Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDelete.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDelete.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDelete.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDelete.Image = CType(resources.GetObject("cmdDelete.Image"), System.Drawing.Image)
        Me.cmdDelete.Location = New System.Drawing.Point(6, 16)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDelete.Size = New System.Drawing.Size(41, 41)
        Me.cmdDelete.TabIndex = 14
        Me.cmdDelete.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdDelete, "Delete Item from Queue")
        Me.cmdDelete.UseVisualStyleBackColor = False
        '
        'cmdSearch
        '
        Me.cmdSearch.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSearch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSearch.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSearch.Image = CType(resources.GetObject("cmdSearch.Image"), System.Drawing.Image)
        Me.cmdSearch.Location = New System.Drawing.Point(720, 104)
        Me.cmdSearch.Name = "cmdSearch"
        Me.cmdSearch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSearch.Size = New System.Drawing.Size(41, 41)
        Me.cmdSearch.TabIndex = 11
        Me.cmdSearch.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdSearch, "Search for items in queue")
        Me.cmdSearch.UseVisualStyleBackColor = False
        '
        'cmdReset
        '
        Me.cmdReset.BackColor = System.Drawing.SystemColors.Control
        Me.cmdReset.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdReset.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdReset.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdReset.Image = CType(resources.GetObject("cmdReset.Image"), System.Drawing.Image)
        Me.cmdReset.Location = New System.Drawing.Point(720, 56)
        Me.cmdReset.Name = "cmdReset"
        Me.cmdReset.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdReset.Size = New System.Drawing.Size(41, 41)
        Me.cmdReset.TabIndex = 12
        Me.cmdReset.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdReset, "Clear search criteria")
        Me.cmdReset.UseVisualStyleBackColor = False
        '
        'cmdVendorSearch
        '
        Me.cmdVendorSearch.BackColor = System.Drawing.SystemColors.Control
        Me.cmdVendorSearch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdVendorSearch.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdVendorSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdVendorSearch.Image = CType(resources.GetObject("cmdVendorSearch.Image"), System.Drawing.Image)
        Me.cmdVendorSearch.Location = New System.Drawing.Point(456, 24)
        Me.cmdVendorSearch.Name = "cmdVendorSearch"
        Me.cmdVendorSearch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdVendorSearch.Size = New System.Drawing.Size(21, 21)
        Me.cmdVendorSearch.TabIndex = 0
        Me.cmdVendorSearch.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdVendorSearch, "Vendor search")
        Me.cmdVendorSearch.UseVisualStyleBackColor = False
        '
        'cmdCreateOrder
        '
        Me.cmdCreateOrder.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCreateOrder.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCreateOrder.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCreateOrder.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCreateOrder.Image = CType(resources.GetObject("cmdCreateOrder.Image"), System.Drawing.Image)
        Me.cmdCreateOrder.Location = New System.Drawing.Point(689, 478)
        Me.cmdCreateOrder.Name = "cmdCreateOrder"
        Me.cmdCreateOrder.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCreateOrder.Size = New System.Drawing.Size(44, 41)
        Me.cmdCreateOrder.TabIndex = 20
        Me.cmdCreateOrder.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdCreateOrder, "Create Order")
        Me.cmdCreateOrder.UseVisualStyleBackColor = False
        '
        'cmdSelect
        '
        Me.cmdSelect.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSelect.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSelect.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSelect.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSelect.Image = CType(resources.GetObject("cmdSelect.Image"), System.Drawing.Image)
        Me.cmdSelect.Location = New System.Drawing.Point(642, 478)
        Me.cmdSelect.Name = "cmdSelect"
        Me.cmdSelect.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSelect.Size = New System.Drawing.Size(43, 41)
        Me.cmdSelect.TabIndex = 19
        Me.cmdSelect.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdSelect, "Select/Deselect")
        Me.cmdSelect.UseVisualStyleBackColor = False
        '
        'cmdExit
        '
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdExit.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Image = CType(resources.GetObject("cmdExit.Image"), System.Drawing.Image)
        Me.cmdExit.Location = New System.Drawing.Point(736, 478)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(41, 41)
        Me.cmdExit.TabIndex = 21
        Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdExit, "Exit")
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'dtpStartDate
        '
        Me.dtpStartDate.Checked = False
        Me.dtpStartDate.CustomFormat = ""
        Me.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpStartDate.Location = New System.Drawing.Point(112, 128)
        Me.dtpStartDate.Name = "dtpStartDate"
        Me.dtpStartDate.ShowCheckBox = True
        Me.dtpStartDate.Size = New System.Drawing.Size(95, 20)
        Me.dtpStartDate.TabIndex = 36
        Me.ToolTip1.SetToolTip(Me.dtpStartDate, "Expected Date")
        Me.dtpStartDate.Value = New Date(2006, 6, 27, 0, 0, 0, 0)
        '
        'btnApplyAllCreditReason
        '
        Me.btnApplyAllCreditReason.Image = Global.My.Resources.Resources.ApplyAll
        Me.btnApplyAllCreditReason.Location = New System.Drawing.Point(502, 479)
        Me.btnApplyAllCreditReason.Name = "btnApplyAllCreditReason"
        Me.btnApplyAllCreditReason.Size = New System.Drawing.Size(42, 40)
        Me.btnApplyAllCreditReason.TabIndex = 34
        Me.ToolTip1.SetToolTip(Me.btnApplyAllCreditReason, "Apply first credit reason to all line items")
        Me.btnApplyAllCreditReason.UseVisualStyleBackColor = True
        Me.btnApplyAllCreditReason.Visible = False
        '
        'imgIcons
        '
        Me.imgIcons.ImageStream = CType(resources.GetObject("imgIcons.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imgIcons.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.imgIcons.Images.SetKeyName(0, "All")
        Me.imgIcons.Images.SetKeyName(1, "None")
        '
        'fraAdmin
        '
        Me.fraAdmin.BackColor = System.Drawing.SystemColors.Control
        Me.fraAdmin.Controls.Add(Me.cmdViewQueue)
        Me.fraAdmin.Controls.Add(Me.cmdSubmit)
        Me.fraAdmin.Controls.Add(Me.cmdDelete)
        Me.fraAdmin.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraAdmin.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraAdmin.Location = New System.Drawing.Point(8, 464)
        Me.fraAdmin.Name = "fraAdmin"
        Me.fraAdmin.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraAdmin.Size = New System.Drawing.Size(146, 66)
        Me.fraAdmin.TabIndex = 27
        Me.fraAdmin.TabStop = False
        Me.fraAdmin.Text = "Queue Maintenance"
        '
        'fraOrderHeader
        '
        Me.fraOrderHeader.BackColor = System.Drawing.SystemColors.Control
        Me.fraOrderHeader.Controls.Add(Me.dtpStartDate)
        Me.fraOrderHeader.Controls.Add(Me.cmbPurchasing)
        Me.fraOrderHeader.Controls.Add(Me.chkDiscontinued)
        Me.fraOrderHeader.Controls.Add(Me.cmbTransferFromSubteam)
        Me.fraOrderHeader.Controls.Add(Me.chkCredit)
        Me.fraOrderHeader.Controls.Add(Me.cmdSearch)
        Me.fraOrderHeader.Controls.Add(Me.cmdReset)
        Me.fraOrderHeader.Controls.Add(Me.cmbTransferToSubteam)
        Me.fraOrderHeader.Controls.Add(Me.cmbProductType)
        Me.fraOrderHeader.Controls.Add(Me.fraOrderType)
        Me.fraOrderHeader.Controls.Add(Me.txtVendor)
        Me.fraOrderHeader.Controls.Add(Me.cmdVendorSearch)
        Me.fraOrderHeader.Controls.Add(Me._lblLabel_2)
        Me.fraOrderHeader.Controls.Add(Me._lblLabel_1)
        Me.fraOrderHeader.Controls.Add(Me._lblLabel_5)
        Me.fraOrderHeader.Controls.Add(Me._lblLabel_4)
        Me.fraOrderHeader.Controls.Add(Me._lblLabel_0)
        Me.fraOrderHeader.Controls.Add(Me._lblLabel_3)
        Me.fraOrderHeader.Controls.Add(Me._lblLabel_9)
        Me.fraOrderHeader.Controls.Add(Me._lblLabel_8)
        Me.fraOrderHeader.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraOrderHeader.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraOrderHeader.Location = New System.Drawing.Point(8, 8)
        Me.fraOrderHeader.Name = "fraOrderHeader"
        Me.fraOrderHeader.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraOrderHeader.Size = New System.Drawing.Size(769, 153)
        Me.fraOrderHeader.TabIndex = 16
        Me.fraOrderHeader.TabStop = False
        Me.fraOrderHeader.Text = "Order Header: "
        '
        'cmbPurchasing
        '
        Me.cmbPurchasing.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbPurchasing.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbPurchasing.BackColor = System.Drawing.SystemColors.Window
        Me.cmbPurchasing.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbPurchasing.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbPurchasing.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbPurchasing.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbPurchasing.Location = New System.Drawing.Point(112, 80)
        Me.cmbPurchasing.Name = "cmbPurchasing"
        Me.cmbPurchasing.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbPurchasing.Size = New System.Drawing.Size(241, 22)
        Me.cmbPurchasing.Sorted = True
        Me.cmbPurchasing.TabIndex = 4
        '
        'chkDiscontinued
        '
        Me.chkDiscontinued.BackColor = System.Drawing.SystemColors.Control
        Me.chkDiscontinued.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkDiscontinued.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkDiscontinued.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDiscontinued.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkDiscontinued.Location = New System.Drawing.Point(558, 128)
        Me.chkDiscontinued.Name = "chkDiscontinued"
        Me.chkDiscontinued.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkDiscontinued.Size = New System.Drawing.Size(150, 17)
        Me.chkDiscontinued.TabIndex = 10
        Me.chkDiscontinued.Text = "Include Discontinued :"
        Me.chkDiscontinued.UseVisualStyleBackColor = False
        '
        'cmbTransferFromSubteam
        '
        Me.cmbTransferFromSubteam.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbTransferFromSubteam.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbTransferFromSubteam.BackColor = System.Drawing.SystemColors.Window
        Me.cmbTransferFromSubteam.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbTransferFromSubteam.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTransferFromSubteam.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbTransferFromSubteam.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbTransferFromSubteam.Location = New System.Drawing.Point(468, 80)
        Me.cmbTransferFromSubteam.Name = "cmbTransferFromSubteam"
        Me.cmbTransferFromSubteam.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbTransferFromSubteam.Size = New System.Drawing.Size(241, 22)
        Me.cmbTransferFromSubteam.Sorted = True
        Me.cmbTransferFromSubteam.TabIndex = 7
        '
        'chkCredit
        '
        Me.chkCredit.BackColor = System.Drawing.SystemColors.Control
        Me.chkCredit.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkCredit.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkCredit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkCredit.Location = New System.Drawing.Point(468, 131)
        Me.chkCredit.Name = "chkCredit"
        Me.chkCredit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkCredit.Size = New System.Drawing.Size(41, 13)
        Me.chkCredit.TabIndex = 9
        Me.chkCredit.UseVisualStyleBackColor = False
        '
        'cmbTransferToSubteam
        '
        Me.cmbTransferToSubteam.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbTransferToSubteam.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbTransferToSubteam.BackColor = System.Drawing.SystemColors.Window
        Me.cmbTransferToSubteam.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbTransferToSubteam.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTransferToSubteam.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbTransferToSubteam.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbTransferToSubteam.Location = New System.Drawing.Point(468, 104)
        Me.cmbTransferToSubteam.Name = "cmbTransferToSubteam"
        Me.cmbTransferToSubteam.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbTransferToSubteam.Size = New System.Drawing.Size(241, 22)
        Me.cmbTransferToSubteam.Sorted = True
        Me.cmbTransferToSubteam.TabIndex = 8
        '
        'cmbProductType
        '
        Me.cmbProductType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbProductType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbProductType.BackColor = System.Drawing.SystemColors.Window
        Me.cmbProductType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbProductType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbProductType.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbProductType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbProductType.Location = New System.Drawing.Point(112, 104)
        Me.cmbProductType.Name = "cmbProductType"
        Me.cmbProductType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbProductType.Size = New System.Drawing.Size(241, 22)
        Me.cmbProductType.TabIndex = 5
        '
        'fraOrderType
        '
        Me.fraOrderType.BackColor = System.Drawing.SystemColors.Control
        Me.fraOrderType.Controls.Add(Me.optTransfer)
        Me.fraOrderType.Controls.Add(Me.optDistribution)
        Me.fraOrderType.Controls.Add(Me.optPurchase)
        Me.fraOrderType.Cursor = System.Windows.Forms.Cursors.Default
        Me.fraOrderType.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraOrderType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraOrderType.Location = New System.Drawing.Point(112, 46)
        Me.fraOrderType.Name = "fraOrderType"
        Me.fraOrderType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraOrderType.Size = New System.Drawing.Size(345, 36)
        Me.fraOrderType.TabIndex = 19
        '
        'optTransfer
        '
        Me.optTransfer.BackColor = System.Drawing.SystemColors.Control
        Me.optTransfer.Cursor = System.Windows.Forms.Cursors.Default
        Me.optTransfer.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optTransfer.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optTransfer.Location = New System.Drawing.Point(216, 8)
        Me.optTransfer.Name = "optTransfer"
        Me.optTransfer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optTransfer.Size = New System.Drawing.Size(73, 17)
        Me.optTransfer.TabIndex = 3
        Me.optTransfer.TabStop = True
        Me.optTransfer.Text = "Transfer"
        Me.optTransfer.UseVisualStyleBackColor = False
        '
        'optDistribution
        '
        Me.optDistribution.BackColor = System.Drawing.SystemColors.Control
        Me.optDistribution.Cursor = System.Windows.Forms.Cursors.Default
        Me.optDistribution.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optDistribution.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optDistribution.Location = New System.Drawing.Point(120, 8)
        Me.optDistribution.Name = "optDistribution"
        Me.optDistribution.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optDistribution.Size = New System.Drawing.Size(89, 17)
        Me.optDistribution.TabIndex = 2
        Me.optDistribution.TabStop = True
        Me.optDistribution.Text = "Distribution"
        Me.optDistribution.UseVisualStyleBackColor = False
        '
        'optPurchase
        '
        Me.optPurchase.BackColor = System.Drawing.SystemColors.Control
        Me.optPurchase.Cursor = System.Windows.Forms.Cursors.Default
        Me.optPurchase.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optPurchase.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optPurchase.Location = New System.Drawing.Point(32, 8)
        Me.optPurchase.Name = "optPurchase"
        Me.optPurchase.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optPurchase.Size = New System.Drawing.Size(81, 17)
        Me.optPurchase.TabIndex = 1
        Me.optPurchase.TabStop = True
        Me.optPurchase.Text = "Purchase"
        Me.optPurchase.UseVisualStyleBackColor = False
        '
        'txtVendor
        '
        Me.txtVendor.AcceptsReturn = True
        Me.txtVendor.BackColor = System.Drawing.SystemColors.ControlLight
        Me.txtVendor.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtVendor.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtVendor.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtVendor.Location = New System.Drawing.Point(112, 24)
        Me.txtVendor.MaxLength = 50
        Me.txtVendor.Name = "txtVendor"
        Me.txtVendor.ReadOnly = True
        Me.txtVendor.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtVendor.Size = New System.Drawing.Size(345, 20)
        Me.txtVendor.TabIndex = 17
        Me.txtVendor.Tag = "String"
        '
        '_lblLabel_2
        '
        Me._lblLabel_2.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_2, CType(2, Short))
        Me._lblLabel_2.Location = New System.Drawing.Point(373, 128)
        Me._lblLabel_2.Name = "_lblLabel_2"
        Me._lblLabel_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_2.Size = New System.Drawing.Size(89, 17)
        Me._lblLabel_2.TabIndex = 26
        Me._lblLabel_2.Text = "Credit :"
        Me._lblLabel_2.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_1
        '
        Me._lblLabel_1.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_1, CType(1, Short))
        Me._lblLabel_1.Location = New System.Drawing.Point(32, 128)
        Me._lblLabel_1.Name = "_lblLabel_1"
        Me._lblLabel_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_1.Size = New System.Drawing.Size(73, 17)
        Me._lblLabel_1.TabIndex = 25
        Me._lblLabel_1.Text = "Expected :"
        Me._lblLabel_1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_5
        '
        Me._lblLabel_5.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_5.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_5, CType(5, Short))
        Me._lblLabel_5.Location = New System.Drawing.Point(359, 80)
        Me._lblLabel_5.Name = "_lblLabel_5"
        Me._lblLabel_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_5.Size = New System.Drawing.Size(103, 17)
        Me._lblLabel_5.TabIndex = 24
        Me._lblLabel_5.Text = "Transfer From :"
        Me._lblLabel_5.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_4
        '
        Me._lblLabel_4.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_4.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_4, CType(4, Short))
        Me._lblLabel_4.Location = New System.Drawing.Point(373, 104)
        Me._lblLabel_4.Name = "_lblLabel_4"
        Me._lblLabel_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_4.Size = New System.Drawing.Size(89, 17)
        Me._lblLabel_4.TabIndex = 23
        Me._lblLabel_4.Text = "Transfer To :"
        Me._lblLabel_4.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_0
        '
        Me._lblLabel_0.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_0.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_0, CType(0, Short))
        Me._lblLabel_0.Location = New System.Drawing.Point(16, 104)
        Me._lblLabel_0.Name = "_lblLabel_0"
        Me._lblLabel_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_0.Size = New System.Drawing.Size(89, 17)
        Me._lblLabel_0.TabIndex = 22
        Me._lblLabel_0.Text = "Product Type :"
        Me._lblLabel_0.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_3
        '
        Me._lblLabel_3.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_3.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_3, CType(3, Short))
        Me._lblLabel_3.Location = New System.Drawing.Point(16, 80)
        Me._lblLabel_3.Name = "_lblLabel_3"
        Me._lblLabel_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_3.Size = New System.Drawing.Size(89, 17)
        Me._lblLabel_3.TabIndex = 21
        Me._lblLabel_3.Text = "Purchasing :"
        Me._lblLabel_3.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_9
        '
        Me._lblLabel_9.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_9.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_9.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_9.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_9, CType(9, Short))
        Me._lblLabel_9.Location = New System.Drawing.Point(8, 56)
        Me._lblLabel_9.Name = "_lblLabel_9"
        Me._lblLabel_9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_9.Size = New System.Drawing.Size(97, 17)
        Me._lblLabel_9.TabIndex = 20
        Me._lblLabel_9.Text = "Order Type :"
        Me._lblLabel_9.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_8
        '
        Me._lblLabel_8.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_8.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_8.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_8.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_8, CType(8, Short))
        Me._lblLabel_8.Location = New System.Drawing.Point(32, 24)
        Me._lblLabel_8.Name = "_lblLabel_8"
        Me._lblLabel_8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_8.Size = New System.Drawing.Size(73, 17)
        Me._lblLabel_8.TabIndex = 18
        Me._lblLabel_8.Text = "Vendor :"
        Me._lblLabel_8.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblDisplayCount
        '
        Me.lblDisplayCount.BackColor = System.Drawing.SystemColors.Control
        Me.lblDisplayCount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDisplayCount.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDisplayCount.ForeColor = System.Drawing.Color.Red
        Me.lblDisplayCount.Location = New System.Drawing.Point(162, 483)
        Me.lblDisplayCount.Name = "lblDisplayCount"
        Me.lblDisplayCount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDisplayCount.Size = New System.Drawing.Size(334, 47)
        Me.lblDisplayCount.TabIndex = 33
        '
        'grdList
        '
        Appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.grdList.DisplayLayout.Appearance = Appearance1
        UltraGridColumn1.Header.VisiblePosition = 0
        UltraGridColumn1.Hidden = True
        UltraGridColumn2.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn2.Header.Caption = "Sel"
        UltraGridColumn2.Header.VisiblePosition = 1
        UltraGridColumn2.TabStop = False
        UltraGridColumn2.Width = 25
        UltraGridColumn3.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn3.Header.VisiblePosition = 2
        UltraGridColumn3.TabStop = False
        UltraGridColumn3.Width = 89
        UltraGridColumn4.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn4.Header.Caption = "Description"
        UltraGridColumn4.Header.VisiblePosition = 3
        UltraGridColumn4.TabStop = False
        UltraGridColumn4.Width = 191
        UltraGridColumn5.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn5.Header.Caption = "Subteam"
        UltraGridColumn5.Header.VisiblePosition = 4
        UltraGridColumn5.TabStop = False
        UltraGridColumn5.Width = 79
        UltraGridColumn6.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn6.Header.Caption = "NA"
        UltraGridColumn6.Header.VisiblePosition = 5
        UltraGridColumn6.Hidden = True
        UltraGridColumn6.TabStop = False
        UltraGridColumn6.Width = 28
        UltraGridColumn7.Format = "#####.##"
        UltraGridColumn7.Header.VisiblePosition = 6
        UltraGridColumn7.Width = 58
        UltraGridColumn8.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn8.Header.Caption = "Unit"
        UltraGridColumn8.Header.VisiblePosition = 7
        UltraGridColumn8.TabStop = False
        UltraGridColumn8.Width = 43
        UltraGridColumn9.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn9.Header.Caption = "PV"
        UltraGridColumn9.Header.VisiblePosition = 9
        UltraGridColumn9.TabStop = False
        UltraGridColumn9.Width = 28
        UltraGridColumn10.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn10.Header.Caption = "User"
        UltraGridColumn10.Header.VisiblePosition = 10
        UltraGridColumn10.TabStop = False
        UltraGridColumn10.Width = 55
        UltraGridColumn11.Header.Caption = "Credit Reason"
        UltraGridColumn11.Header.VisiblePosition = 11
        UltraGridColumn11.Hidden = True
        UltraGridColumn11.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList
        UltraGridColumn11.Width = 100
        UltraGridColumn12.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn12.Header.Caption = "Inserted"
        UltraGridColumn12.Header.VisiblePosition = 12
        UltraGridColumn12.Hidden = True
        UltraGridColumn12.Width = 82
        UltraGridColumn13.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn13.DataType = GetType(Boolean)
        UltraGridColumn13.Header.VisiblePosition = 13
        UltraGridColumn14.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn14.DataType = GetType(Decimal)
        UltraGridColumn14.Format = "###,###.##"
        UltraGridColumn14.Header.VisiblePosition = 8
        UltraGridColumn14.Width = 58
        UltraGridBand1.Columns.AddRange(New Object() {UltraGridColumn1, UltraGridColumn2, UltraGridColumn3, UltraGridColumn4, UltraGridColumn5, UltraGridColumn6, UltraGridColumn7, UltraGridColumn8, UltraGridColumn9, UltraGridColumn10, UltraGridColumn11, UltraGridColumn12, UltraGridColumn13, UltraGridColumn14})
        Me.grdList.DisplayLayout.BandsSerializer.Add(UltraGridBand1)
        Me.grdList.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.grdList.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Me.grdList.DisplayLayout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.[False]
        Appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance2.BorderColor = System.Drawing.SystemColors.Window
        Me.grdList.DisplayLayout.GroupByBox.Appearance = Appearance2
        Appearance3.ForeColor = System.Drawing.SystemColors.GrayText
        Me.grdList.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance3
        Me.grdList.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance4.BackColor2 = System.Drawing.SystemColors.Control
        Appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance4.ForeColor = System.Drawing.SystemColors.GrayText
        Me.grdList.DisplayLayout.GroupByBox.PromptAppearance = Appearance4
        Me.grdList.DisplayLayout.MaxBandDepth = 1
        Me.grdList.DisplayLayout.MaxColScrollRegions = 1
        Me.grdList.DisplayLayout.MaxRowScrollRegions = 1
        Me.grdList.DisplayLayout.NewBandLoadStyle = Infragistics.Win.UltraWinGrid.NewBandLoadStyle.Hide
        Me.grdList.DisplayLayout.NewColumnLoadStyle = Infragistics.Win.UltraWinGrid.NewColumnLoadStyle.Hide
        Appearance5.BackColor = System.Drawing.SystemColors.Window
        Appearance5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.grdList.DisplayLayout.Override.ActiveCellAppearance = Appearance5
        Appearance6.ForeColor = System.Drawing.SystemColors.GrayText
        Me.grdList.DisplayLayout.Override.ActiveRowAppearance = Appearance6
        Me.grdList.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.grdList.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance7.BackColor = System.Drawing.SystemColors.Window
        Me.grdList.DisplayLayout.Override.CardAreaAppearance = Appearance7
        Appearance8.BorderColor = System.Drawing.Color.Silver
        Appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.grdList.DisplayLayout.Override.CellAppearance = Appearance8
        Me.grdList.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.grdList.DisplayLayout.Override.CellPadding = 0
        Appearance9.BackColor = System.Drawing.SystemColors.Control
        Appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance9.BorderColor = System.Drawing.SystemColors.Window
        Me.grdList.DisplayLayout.Override.GroupByRowAppearance = Appearance9
        Appearance10.TextHAlignAsString = "Left"
        Me.grdList.DisplayLayout.Override.HeaderAppearance = Appearance10
        Me.grdList.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti
        Me.grdList.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance11.BackColor = System.Drawing.SystemColors.ControlLight
        Me.grdList.DisplayLayout.Override.RowAlternateAppearance = Appearance11
        Appearance12.BackColor = System.Drawing.SystemColors.Window
        Appearance12.BorderColor = System.Drawing.Color.Silver
        Me.grdList.DisplayLayout.Override.RowAppearance = Appearance12
        Me.grdList.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[True]
        Appearance13.BackColor = System.Drawing.SystemColors.ControlLight
        Me.grdList.DisplayLayout.Override.TemplateAddRowAppearance = Appearance13
        Me.grdList.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.grdList.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        ValueList1.SortStyle = Infragistics.Win.ValueListSortStyle.Ascending
        Me.grdList.DisplayLayout.ValueLists.AddRange(New Infragistics.Win.ValueList() {ValueList1})
        Me.grdList.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand
        Me.grdList.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.Horizontal
        Me.grdList.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grdList.Location = New System.Drawing.Point(8, 167)
        Me.grdList.Name = "grdList"
        Me.grdList.Size = New System.Drawing.Size(769, 295)
        Me.grdList.TabIndex = 13
        Me.grdList.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnCellChangeOrLostFocus
        '
        'lblOrderedTotal
        '
        Me.lblOrderedTotal.AutoSize = True
        Me.lblOrderedTotal.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOrderedTotal.Location = New System.Drawing.Point(162, 469)
        Me.lblOrderedTotal.Name = "lblOrderedTotal"
        Me.lblOrderedTotal.Size = New System.Drawing.Size(114, 14)
        Me.lblOrderedTotal.TabIndex = 35
        Me.lblOrderedTotal.Text = "Total Ordered Cost:"
        '
        'lblOrderedTotalAmount
        '
        Me.lblOrderedTotalAmount.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOrderedTotalAmount.Location = New System.Drawing.Point(282, 469)
        Me.lblOrderedTotalAmount.Name = "lblOrderedTotalAmount"
        Me.lblOrderedTotalAmount.Size = New System.Drawing.Size(203, 14)
        Me.lblOrderedTotalAmount.TabIndex = 36
        '
        'frmOrderItemQueue
        '
        Me.AcceptButton = Me.cmdVendorSearch
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdExit
        Me.ClientSize = New System.Drawing.Size(785, 534)
        Me.Controls.Add(Me.lblOrderedTotalAmount)
        Me.Controls.Add(Me.lblOrderedTotal)
        Me.Controls.Add(Me.btnApplyAllCreditReason)
        Me.Controls.Add(Me.grdList)
        Me.Controls.Add(Me.cmdItemEdit)
        Me.Controls.Add(Me.cmdSelectAll)
        Me.Controls.Add(Me.fraAdmin)
        Me.Controls.Add(Me.fraOrderHeader)
        Me.Controls.Add(Me.cmdCreateOrder)
        Me.Controls.Add(Me.cmdSelect)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.lblDisplayCount)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(3, 29)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmOrderItemQueue"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Order Item Queue"
        Me.fraAdmin.ResumeLayout(False)
        Me.fraOrderHeader.ResumeLayout(False)
        Me.fraOrderHeader.PerformLayout()
        Me.fraOrderType.ResumeLayout(False)
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.grdList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents grdList As Infragistics.Win.UltraWinGrid.UltraGrid
    Friend WithEvents dtpStartDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents btnApplyAllCreditReason As System.Windows.Forms.Button
    Friend WithEvents lblOrderedTotal As System.Windows.Forms.Label
    Friend WithEvents lblOrderedTotalAmount As System.Windows.Forms.Label
#End Region
End Class