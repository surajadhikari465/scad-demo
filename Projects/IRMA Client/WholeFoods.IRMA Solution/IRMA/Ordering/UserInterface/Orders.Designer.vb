<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmOrders
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
	Public WithEvents _txtField_15 As System.Windows.Forms.TextBox
	Public WithEvents cmdReceive As System.Windows.Forms.Button
    Public WithEvents Timer1 As System.Windows.Forms.Timer
	Public WithEvents cmdChgVendor As System.Windows.Forms.Button
    Public WithEvents cmdWarehouseSend As System.Windows.Forms.Button
    Public WithEvents _txtField_13 As System.Windows.Forms.TextBox
    Public WithEvents _txtField_9 As System.Windows.Forms.TextBox
    Public WithEvents cmdCloseOrder As System.Windows.Forms.Button
    Public WithEvents cmdDistributionCreditOrder As System.Windows.Forms.Button
    Public WithEvents cmdDeleteItem As System.Windows.Forms.Button
    Public WithEvents cmdEditItem As System.Windows.Forms.Button
    Public WithEvents cmdAddItem As System.Windows.Forms.Button
    Public WithEvents cmdExit As System.Windows.Forms.Button
    Public WithEvents cmdUnlock As System.Windows.Forms.Button
    Public WithEvents cmdReports As System.Windows.Forms.Button
    Public WithEvents cmdSearch As System.Windows.Forms.Button
    Public WithEvents cmdSendOrder As System.Windows.Forms.Button
    Public WithEvents _txtField_10 As System.Windows.Forms.TextBox
    Public WithEvents _chkField_1 As System.Windows.Forms.CheckBox
    Public WithEvents cmdOrderList As System.Windows.Forms.Button
    Public WithEvents _txtField_12 As System.Windows.Forms.TextBox
    Public WithEvents _txtField_11 As System.Windows.Forms.TextBox
    Public WithEvents _cmbField_2 As System.Windows.Forms.ComboBox
    Public WithEvents _txtField_7 As System.Windows.Forms.TextBox
    Public WithEvents cmdOrderNotes As System.Windows.Forms.Button
    Public WithEvents _cmbField_0 As System.Windows.Forms.ComboBox
    Public WithEvents _cmbField_1 As System.Windows.Forms.ComboBox
    Public WithEvents _txtField_5 As System.Windows.Forms.TextBox
    Public WithEvents _txtField_4 As System.Windows.Forms.TextBox
    Public WithEvents cmdDeleteOrder As System.Windows.Forms.Button
    Public WithEvents cmdAddOrder As System.Windows.Forms.Button
    Public WithEvents _txtField_2 As System.Windows.Forms.TextBox
    Public WithEvents _txtField_3 As System.Windows.Forms.TextBox
    Public WithEvents _txtField_1 As System.Windows.Forms.TextBox
    Public WithEvents _txtField_0 As System.Windows.Forms.TextBox
    Public WithEvents _lblLabel_16 As System.Windows.Forms.Label
    Public WithEvents _lblLabel_11 As System.Windows.Forms.Label
    Public WithEvents lblOrgPO As System.Windows.Forms.Label
    Public WithEvents lblCredit As System.Windows.Forms.Label
    Public WithEvents _lblLabel_14 As System.Windows.Forms.Label
    Public WithEvents _lblLabel_13 As System.Windows.Forms.Label
    Public WithEvents _lblLabel_9 As System.Windows.Forms.Label
    Public WithEvents lblReadOnly As System.Windows.Forms.Label
    Public WithEvents _lblLabel_31 As System.Windows.Forms.Label
    Public WithEvents _lblLabel_8 As System.Windows.Forms.Label
    Public WithEvents _lblLabel_7 As System.Windows.Forms.Label
    Public WithEvents _lblLabel_6 As System.Windows.Forms.Label
    Public WithEvents _lblLabel_5 As System.Windows.Forms.Label
    Public WithEvents _lblLabel_4 As System.Windows.Forms.Label
    Public WithEvents lblStatus As System.Windows.Forms.Label
    Public WithEvents _lblLabel_3 As System.Windows.Forms.Label
    Public WithEvents _lblLabel_2 As System.Windows.Forms.Label
    Public WithEvents _lblLabel_1 As System.Windows.Forms.Label
    Public WithEvents _lblLabel_0 As System.Windows.Forms.Label
    Public WithEvents chkField As Microsoft.VisualBasic.Compatibility.VB6.CheckBoxArray
    Public WithEvents cmbField As Microsoft.VisualBasic.Compatibility.VB6.ComboBoxArray
    Public WithEvents lblLabel As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
    Public WithEvents optCost As Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray
    Public WithEvents txtField As Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray
    Public WithEvents optPOTrans As Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmOrders))
        Dim Appearance24 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridBand1 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1)
        Dim UltraGridColumn1 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("OrderItem_ID")
        Dim UltraGridColumn2 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Line")
        Dim Appearance25 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance26 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridColumn3 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Identifier")
        Dim Appearance27 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance28 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridColumn4 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Brand_Name")
        Dim UltraGridColumn5 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Item_Description", -1, Nothing, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, False)
        Dim UltraGridColumn6 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("QuantityOrdered")
        Dim Appearance29 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance30 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridColumn7 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("QuantityReceived")
        Dim Appearance31 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance32 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridColumn8 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("ActualCost", 0)
        Dim Appearance13 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance15 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridColumn9 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("eInvoice", 1)
        Dim Appearance14 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance16 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridColumn10 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Vendor_Item_Description", 2)
        Dim UltraGridColumn11 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Item_Key", 3)
        Dim Appearance33 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim ColScrollRegion1 As Infragistics.Win.UltraWinGrid.ColScrollRegion = New Infragistics.Win.UltraWinGrid.ColScrollRegion(764)
        Dim ColScrollRegion2 As Infragistics.Win.UltraWinGrid.ColScrollRegion = New Infragistics.Win.UltraWinGrid.ColScrollRegion(0)
        Dim Appearance34 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance35 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance36 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance37 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance38 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance39 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance40 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance41 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance42 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance43 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance44 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance45 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance46 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance1 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance2 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance4 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance3 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance9 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance5 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance12 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance8 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance6 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance7 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance10 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance11 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdReceive = New System.Windows.Forms.Button()
        Me.cmdChgVendor = New System.Windows.Forms.Button()
        Me.cmdWarehouseSend = New System.Windows.Forms.Button()
        Me.cmdCloseOrder = New System.Windows.Forms.Button()
        Me.cmdDistributionCreditOrder = New System.Windows.Forms.Button()
        Me.cmdDeleteItem = New System.Windows.Forms.Button()
        Me.cmdEditItem = New System.Windows.Forms.Button()
        Me.cmdAddItem = New System.Windows.Forms.Button()
        Me.cmdExit = New System.Windows.Forms.Button()
        Me.cmdUnlock = New System.Windows.Forms.Button()
        Me.cmdReports = New System.Windows.Forms.Button()
        Me.cmdSearch = New System.Windows.Forms.Button()
        Me.cmdSendOrder = New System.Windows.Forms.Button()
        Me.cmdOrderList = New System.Windows.Forms.Button()
        Me.cmdOrderNotes = New System.Windows.Forms.Button()
        Me.cmdDeleteOrder = New System.Windows.Forms.Button()
        Me.cmdAddOrder = New System.Windows.Forms.Button()
        Me.cmd3rdPartyInvoice = New System.Windows.Forms.Button()
        Me.cmdDisplayEInvoice = New System.Windows.Forms.Button()
        Me.cmdCopyPO = New System.Windows.Forms.Button()
        Me.cmdUndoWarehouseSend = New System.Windows.Forms.Button()
        Me.UploadedCostTextBox = New System.Windows.Forms.TextBox()
        Me.AdjustedReceivedCostTextBox = New System.Windows.Forms.TextBox()
        Me.OrderedCostTextBox = New System.Windows.Forms.TextBox()
        Me.UploadedCostLabel = New System.Windows.Forms.Label()
        Me.AdjustedReceivedCostLabel = New System.Windows.Forms.Label()
        Me.OrderedCostLabel = New System.Windows.Forms.Label()
        Me.OrigianalReceivedCostLabel = New System.Windows.Forms.Label()
        Me.OriginalReceivedCostTextBox = New System.Windows.Forms.TextBox()
        Me.txtPOCostDate = New System.Windows.Forms.TextBox()
        Me.lblPOCostDate = New System.Windows.Forms.Label()
        Me._cmbField_2 = New System.Windows.Forms.ComboBox()
        Me.cmdItemsRefused = New System.Windows.Forms.Button()
        Me._txtField_15 = New System.Windows.Forms.TextBox()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me._txtField_13 = New System.Windows.Forms.TextBox()
        Me._txtField_9 = New System.Windows.Forms.TextBox()
        Me._txtField_10 = New System.Windows.Forms.TextBox()
        Me._chkField_1 = New System.Windows.Forms.CheckBox()
        Me._txtField_12 = New System.Windows.Forms.TextBox()
        Me._txtField_11 = New System.Windows.Forms.TextBox()
        Me._txtField_7 = New System.Windows.Forms.TextBox()
        Me._cmbField_0 = New System.Windows.Forms.ComboBox()
        Me._cmbField_1 = New System.Windows.Forms.ComboBox()
        Me._txtField_5 = New System.Windows.Forms.TextBox()
        Me._txtField_4 = New System.Windows.Forms.TextBox()
        Me._txtField_2 = New System.Windows.Forms.TextBox()
        Me._txtField_3 = New System.Windows.Forms.TextBox()
        Me._txtField_1 = New System.Windows.Forms.TextBox()
        Me._txtField_0 = New System.Windows.Forms.TextBox()
        Me._lblLabel_16 = New System.Windows.Forms.Label()
        Me._lblLabel_11 = New System.Windows.Forms.Label()
        Me.lblOrgPO = New System.Windows.Forms.Label()
        Me.lblCredit = New System.Windows.Forms.Label()
        Me._lblLabel_14 = New System.Windows.Forms.Label()
        Me._lblLabel_13 = New System.Windows.Forms.Label()
        Me._lblLabel_9 = New System.Windows.Forms.Label()
        Me.lblReadOnly = New System.Windows.Forms.Label()
        Me._lblLabel_31 = New System.Windows.Forms.Label()
        Me._lblLabel_8 = New System.Windows.Forms.Label()
        Me._lblLabel_7 = New System.Windows.Forms.Label()
        Me._lblLabel_6 = New System.Windows.Forms.Label()
        Me._lblLabel_5 = New System.Windows.Forms.Label()
        Me._lblLabel_4 = New System.Windows.Forms.Label()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me._lblLabel_3 = New System.Windows.Forms.Label()
        Me._lblLabel_2 = New System.Windows.Forms.Label()
        Me._lblLabel_1 = New System.Windows.Forms.Label()
        Me._lblLabel_0 = New System.Windows.Forms.Label()
        Me.chkField = New Microsoft.VisualBasic.Compatibility.VB6.CheckBoxArray(Me.components)
        Me.cmbField = New Microsoft.VisualBasic.Compatibility.VB6.ComboBoxArray(Me.components)
        Me.lblLabel = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(Me.components)
        Me.optCost = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(Me.components)
        Me.txtField = New Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray(Me.components)
        Me._txtField_16 = New System.Windows.Forms.TextBox()
        Me.optPOTrans = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(Me.components)
        Me.ugrdItems = New Infragistics.Win.UltraWinGrid.UltraGrid()
        Me.sbMsg = New System.Windows.Forms.StatusStrip()
        Me.tssl1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtClosedBy = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lblTransmissionType = New System.Windows.Forms.Label()
        Me.CheckBox_DropShip = New System.Windows.Forms.CheckBox()
        Me._lblLabel_12 = New System.Windows.Forms.Label()
        Me.txtApprover = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtApproved = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.LabelCancelled = New System.Windows.Forms.Label()
        Me.chkShowVendorDescription = New System.Windows.Forms.CheckBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.FormValidator = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.ucCostAdjReasonCode = New Infragistics.Win.UltraWinGrid.UltraCombo()
        Me.txtRecvDate = New System.Windows.Forms.TextBox()
        Me.lblRecvDate = New System.Windows.Forms.Label()
        Me.txtDeletedby = New System.Windows.Forms.TextBox()
        Me.lblDeletedBy = New System.Windows.Forms.Label()
        Me.txtDeletedDate = New System.Windows.Forms.TextBox()
        Me.lblDeletedDate = New System.Windows.Forms.Label()
        Me.txtDeletedReason = New System.Windows.Forms.TextBox()
        Me.lblDeletedReason = New System.Windows.Forms.Label()
        Me.txtRefusedTotal = New System.Windows.Forms.TextBox()
        Me.lblRefusedTotal = New System.Windows.Forms.Label()
        Me.dtpExpectedDate = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor()
        Me.chkApplyAllCreditReason = New System.Windows.Forms.CheckBox()
        Me.lblCreditReasonCode = New System.Windows.Forms.Label()
        CType(Me.chkField, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cmbField, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.optCost, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtField, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.optPOTrans, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ugrdItems, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.sbMsg.SuspendLayout()
        CType(Me.FormValidator, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ucCostAdjReasonCode, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dtpExpectedDate, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdReceive
        '
        Me.cmdReceive.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdReceive.BackColor = System.Drawing.SystemColors.Control
        Me.cmdReceive.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdReceive.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdReceive.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdReceive.Image = CType(resources.GetObject("cmdReceive.Image"), System.Drawing.Image)
        Me.cmdReceive.Location = New System.Drawing.Point(531, 280)
        Me.cmdReceive.Name = "cmdReceive"
        Me.cmdReceive.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdReceive.Size = New System.Drawing.Size(41, 41)
        Me.cmdReceive.TabIndex = 33
        Me.cmdReceive.Tag = "BDA"
        Me.cmdReceive.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdReceive, "Receiving List")
        Me.cmdReceive.UseVisualStyleBackColor = False
        '
        'cmdChgVendor
        '
        Me.cmdChgVendor.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdChgVendor.BackColor = System.Drawing.SystemColors.Control
        Me.cmdChgVendor.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdChgVendor.Enabled = False
        Me.cmdChgVendor.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdChgVendor.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdChgVendor.Image = CType(resources.GetObject("cmdChgVendor.Image"), System.Drawing.Image)
        Me.cmdChgVendor.Location = New System.Drawing.Point(571, 280)
        Me.cmdChgVendor.Name = "cmdChgVendor"
        Me.cmdChgVendor.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdChgVendor.Size = New System.Drawing.Size(41, 41)
        Me.cmdChgVendor.TabIndex = 34
        Me.cmdChgVendor.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdChgVendor, "Change Vendor")
        Me.cmdChgVendor.UseVisualStyleBackColor = False
        Me.cmdChgVendor.Visible = False
        '
        'cmdWarehouseSend
        '
        Me.cmdWarehouseSend.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdWarehouseSend.BackColor = System.Drawing.SystemColors.Control
        Me.cmdWarehouseSend.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdWarehouseSend.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdWarehouseSend.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdWarehouseSend.Image = CType(resources.GetObject("cmdWarehouseSend.Image"), System.Drawing.Image)
        Me.cmdWarehouseSend.Location = New System.Drawing.Point(611, 240)
        Me.cmdWarehouseSend.Name = "cmdWarehouseSend"
        Me.cmdWarehouseSend.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdWarehouseSend.Size = New System.Drawing.Size(41, 41)
        Me.cmdWarehouseSend.TabIndex = 27
        Me.cmdWarehouseSend.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdWarehouseSend, "Warehouse Send Now")
        Me.cmdWarehouseSend.UseVisualStyleBackColor = False
        '
        'cmdCloseOrder
        '
        Me.cmdCloseOrder.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdCloseOrder.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCloseOrder.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCloseOrder.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCloseOrder.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCloseOrder.Image = CType(resources.GetObject("cmdCloseOrder.Image"), System.Drawing.Image)
        Me.cmdCloseOrder.Location = New System.Drawing.Point(691, 240)
        Me.cmdCloseOrder.Name = "cmdCloseOrder"
        Me.cmdCloseOrder.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCloseOrder.Size = New System.Drawing.Size(41, 41)
        Me.cmdCloseOrder.TabIndex = 29
        Me.cmdCloseOrder.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdCloseOrder, "Close Order")
        Me.cmdCloseOrder.UseVisualStyleBackColor = False
        '
        'cmdDistributionCreditOrder
        '
        Me.cmdDistributionCreditOrder.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdDistributionCreditOrder.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDistributionCreditOrder.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDistributionCreditOrder.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDistributionCreditOrder.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDistributionCreditOrder.Image = CType(resources.GetObject("cmdDistributionCreditOrder.Image"), System.Drawing.Image)
        Me.cmdDistributionCreditOrder.Location = New System.Drawing.Point(651, 240)
        Me.cmdDistributionCreditOrder.Name = "cmdDistributionCreditOrder"
        Me.cmdDistributionCreditOrder.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDistributionCreditOrder.Size = New System.Drawing.Size(41, 41)
        Me.cmdDistributionCreditOrder.TabIndex = 28
        Me.cmdDistributionCreditOrder.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdDistributionCreditOrder, "Distribution Credit Order")
        Me.cmdDistributionCreditOrder.UseVisualStyleBackColor = False
        '
        'cmdDeleteItem
        '
        Me.cmdDeleteItem.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdDeleteItem.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDeleteItem.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDeleteItem.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDeleteItem.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDeleteItem.Image = CType(resources.GetObject("cmdDeleteItem.Image"), System.Drawing.Image)
        Me.cmdDeleteItem.Location = New System.Drawing.Point(88, 522)
        Me.cmdDeleteItem.Name = "cmdDeleteItem"
        Me.cmdDeleteItem.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDeleteItem.Size = New System.Drawing.Size(41, 41)
        Me.cmdDeleteItem.TabIndex = 38
        Me.cmdDeleteItem.Tag = "B"
        Me.cmdDeleteItem.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdDeleteItem, "Remove Item")
        Me.cmdDeleteItem.UseVisualStyleBackColor = False
        '
        'cmdEditItem
        '
        Me.cmdEditItem.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdEditItem.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEditItem.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEditItem.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEditItem.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEditItem.Image = CType(resources.GetObject("cmdEditItem.Image"), System.Drawing.Image)
        Me.cmdEditItem.Location = New System.Drawing.Point(48, 522)
        Me.cmdEditItem.Name = "cmdEditItem"
        Me.cmdEditItem.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEditItem.Size = New System.Drawing.Size(41, 41)
        Me.cmdEditItem.TabIndex = 37
        Me.cmdEditItem.Tag = "B"
        Me.cmdEditItem.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdEditItem, "Edit Item")
        Me.cmdEditItem.UseVisualStyleBackColor = False
        '
        'cmdAddItem
        '
        Me.cmdAddItem.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdAddItem.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddItem.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddItem.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddItem.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddItem.Image = CType(resources.GetObject("cmdAddItem.Image"), System.Drawing.Image)
        Me.cmdAddItem.Location = New System.Drawing.Point(8, 522)
        Me.cmdAddItem.Name = "cmdAddItem"
        Me.cmdAddItem.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddItem.Size = New System.Drawing.Size(41, 41)
        Me.cmdAddItem.TabIndex = 36
        Me.cmdAddItem.Tag = "B"
        Me.cmdAddItem.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdAddItem, "Add Item")
        Me.cmdAddItem.UseVisualStyleBackColor = False
        '
        'cmdExit
        '
        Me.cmdExit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.cmdExit.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Image = CType(resources.GetObject("cmdExit.Image"), System.Drawing.Image)
        Me.cmdExit.Location = New System.Drawing.Point(733, 522)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(41, 41)
        Me.cmdExit.TabIndex = 39
        Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdExit, "Exit")
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'cmdUnlock
        '
        Me.cmdUnlock.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdUnlock.BackColor = System.Drawing.SystemColors.Control
        Me.cmdUnlock.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdUnlock.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdUnlock.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdUnlock.Image = CType(resources.GetObject("cmdUnlock.Image"), System.Drawing.Image)
        Me.cmdUnlock.Location = New System.Drawing.Point(491, 280)
        Me.cmdUnlock.Name = "cmdUnlock"
        Me.cmdUnlock.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdUnlock.Size = New System.Drawing.Size(41, 41)
        Me.cmdUnlock.TabIndex = 32
        Me.cmdUnlock.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdUnlock, "Unlock Order")
        Me.cmdUnlock.UseVisualStyleBackColor = False
        '
        'cmdReports
        '
        Me.cmdReports.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdReports.BackColor = System.Drawing.SystemColors.Control
        Me.cmdReports.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdReports.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdReports.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdReports.Image = CType(resources.GetObject("cmdReports.Image"), System.Drawing.Image)
        Me.cmdReports.Location = New System.Drawing.Point(451, 280)
        Me.cmdReports.Name = "cmdReports"
        Me.cmdReports.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdReports.Size = New System.Drawing.Size(41, 41)
        Me.cmdReports.TabIndex = 31
        Me.cmdReports.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdReports, "Order Reports")
        Me.cmdReports.UseVisualStyleBackColor = False
        '
        'cmdSearch
        '
        Me.cmdSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdSearch.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSearch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSearch.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSearch.Image = CType(resources.GetObject("cmdSearch.Image"), System.Drawing.Image)
        Me.cmdSearch.Location = New System.Drawing.Point(411, 280)
        Me.cmdSearch.Name = "cmdSearch"
        Me.cmdSearch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSearch.Size = New System.Drawing.Size(41, 41)
        Me.cmdSearch.TabIndex = 30
        Me.cmdSearch.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdSearch, "Search For Order")
        Me.cmdSearch.UseVisualStyleBackColor = False
        '
        'cmdSendOrder
        '
        Me.cmdSendOrder.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdSendOrder.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSendOrder.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSendOrder.Enabled = False
        Me.cmdSendOrder.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSendOrder.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSendOrder.Image = CType(resources.GetObject("cmdSendOrder.Image"), System.Drawing.Image)
        Me.cmdSendOrder.Location = New System.Drawing.Point(571, 240)
        Me.cmdSendOrder.Name = "cmdSendOrder"
        Me.cmdSendOrder.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSendOrder.Size = New System.Drawing.Size(41, 41)
        Me.cmdSendOrder.TabIndex = 26
        Me.cmdSendOrder.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdSendOrder, "Send Now")
        Me.cmdSendOrder.UseVisualStyleBackColor = False
        '
        'cmdOrderList
        '
        Me.cmdOrderList.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdOrderList.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOrderList.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOrderList.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOrderList.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOrderList.Image = CType(resources.GetObject("cmdOrderList.Image"), System.Drawing.Image)
        Me.cmdOrderList.Location = New System.Drawing.Point(531, 240)
        Me.cmdOrderList.Name = "cmdOrderList"
        Me.cmdOrderList.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOrderList.Size = New System.Drawing.Size(41, 41)
        Me.cmdOrderList.TabIndex = 25
        Me.cmdOrderList.Tag = "BDA"
        Me.cmdOrderList.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdOrderList, "List View")
        Me.cmdOrderList.UseVisualStyleBackColor = False
        '
        'cmdOrderNotes
        '
        Me.cmdOrderNotes.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdOrderNotes.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOrderNotes.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOrderNotes.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOrderNotes.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOrderNotes.Image = CType(resources.GetObject("cmdOrderNotes.Image"), System.Drawing.Image)
        Me.cmdOrderNotes.Location = New System.Drawing.Point(491, 240)
        Me.cmdOrderNotes.Name = "cmdOrderNotes"
        Me.cmdOrderNotes.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOrderNotes.Size = New System.Drawing.Size(41, 41)
        Me.cmdOrderNotes.TabIndex = 24
        Me.cmdOrderNotes.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdOrderNotes, "Order Notes")
        Me.cmdOrderNotes.UseVisualStyleBackColor = False
        '
        'cmdDeleteOrder
        '
        Me.cmdDeleteOrder.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdDeleteOrder.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDeleteOrder.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDeleteOrder.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDeleteOrder.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDeleteOrder.Image = CType(resources.GetObject("cmdDeleteOrder.Image"), System.Drawing.Image)
        Me.cmdDeleteOrder.Location = New System.Drawing.Point(451, 240)
        Me.cmdDeleteOrder.Name = "cmdDeleteOrder"
        Me.cmdDeleteOrder.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDeleteOrder.Size = New System.Drawing.Size(41, 41)
        Me.cmdDeleteOrder.TabIndex = 23
        Me.cmdDeleteOrder.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdDeleteOrder, "Remove Order")
        Me.cmdDeleteOrder.UseVisualStyleBackColor = False
        '
        'cmdAddOrder
        '
        Me.cmdAddOrder.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdAddOrder.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddOrder.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddOrder.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddOrder.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddOrder.Image = CType(resources.GetObject("cmdAddOrder.Image"), System.Drawing.Image)
        Me.cmdAddOrder.Location = New System.Drawing.Point(411, 240)
        Me.cmdAddOrder.Name = "cmdAddOrder"
        Me.cmdAddOrder.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddOrder.Size = New System.Drawing.Size(41, 41)
        Me.cmdAddOrder.TabIndex = 22
        Me.cmdAddOrder.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdAddOrder, "Add Order")
        Me.cmdAddOrder.UseVisualStyleBackColor = False
        '
        'cmd3rdPartyInvoice
        '
        Me.cmd3rdPartyInvoice.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmd3rdPartyInvoice.BackColor = System.Drawing.SystemColors.Control
        Me.cmd3rdPartyInvoice.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmd3rdPartyInvoice.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmd3rdPartyInvoice.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmd3rdPartyInvoice.Image = Global.My.Resources.Resources.ThirdPartyFreight
        Me.cmd3rdPartyInvoice.Location = New System.Drawing.Point(691, 280)
        Me.cmd3rdPartyInvoice.Name = "cmd3rdPartyInvoice"
        Me.cmd3rdPartyInvoice.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmd3rdPartyInvoice.Size = New System.Drawing.Size(41, 41)
        Me.cmd3rdPartyInvoice.TabIndex = 72
        Me.cmd3rdPartyInvoice.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmd3rdPartyInvoice, "Enter Third Party Freight invoice")
        Me.cmd3rdPartyInvoice.UseVisualStyleBackColor = False
        '
        'cmdDisplayEInvoice
        '
        Me.cmdDisplayEInvoice.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdDisplayEInvoice.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDisplayEInvoice.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDisplayEInvoice.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDisplayEInvoice.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDisplayEInvoice.Image = CType(resources.GetObject("cmdDisplayEInvoice.Image"), System.Drawing.Image)
        Me.cmdDisplayEInvoice.Location = New System.Drawing.Point(651, 280)
        Me.cmdDisplayEInvoice.Name = "cmdDisplayEInvoice"
        Me.cmdDisplayEInvoice.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDisplayEInvoice.Size = New System.Drawing.Size(41, 41)
        Me.cmdDisplayEInvoice.TabIndex = 35
        Me.cmdDisplayEInvoice.Tag = ""
        Me.cmdDisplayEInvoice.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdDisplayEInvoice, "Display E-Invoice")
        Me.cmdDisplayEInvoice.UseVisualStyleBackColor = False
        '
        'cmdCopyPO
        '
        Me.cmdCopyPO.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdCopyPO.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCopyPO.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCopyPO.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCopyPO.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCopyPO.Image = Global.My.Resources.Resources.copy
        Me.cmdCopyPO.Location = New System.Drawing.Point(731, 240)
        Me.cmdCopyPO.Name = "cmdCopyPO"
        Me.cmdCopyPO.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCopyPO.Size = New System.Drawing.Size(41, 41)
        Me.cmdCopyPO.TabIndex = 73
        Me.cmdCopyPO.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdCopyPO, "Copy Purchase Order")
        Me.cmdCopyPO.UseVisualStyleBackColor = False
        '
        'cmdUndoWarehouseSend
        '
        Me.cmdUndoWarehouseSend.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdUndoWarehouseSend.BackColor = System.Drawing.SystemColors.Control
        Me.cmdUndoWarehouseSend.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdUndoWarehouseSend.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdUndoWarehouseSend.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdUndoWarehouseSend.Image = CType(resources.GetObject("cmdUndoWarehouseSend.Image"), System.Drawing.Image)
        Me.cmdUndoWarehouseSend.Location = New System.Drawing.Point(611, 280)
        Me.cmdUndoWarehouseSend.Name = "cmdUndoWarehouseSend"
        Me.cmdUndoWarehouseSend.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdUndoWarehouseSend.Size = New System.Drawing.Size(41, 41)
        Me.cmdUndoWarehouseSend.TabIndex = 73
        Me.cmdUndoWarehouseSend.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdUndoWarehouseSend, "Undo Warehouse Send")
        Me.cmdUndoWarehouseSend.UseVisualStyleBackColor = False
        '
        'UploadedCostTextBox
        '
        Me.UploadedCostTextBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.UploadedCostTextBox.Enabled = False
        Me.UploadedCostTextBox.Location = New System.Drawing.Point(672, 86)
        Me.UploadedCostTextBox.Name = "UploadedCostTextBox"
        Me.UploadedCostTextBox.Size = New System.Drawing.Size(100, 23)
        Me.UploadedCostTextBox.TabIndex = 13
        Me.UploadedCostTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ToolTip1.SetToolTip(Me.UploadedCostTextBox, "This is the cost uploaded to PeopleSoft.")
        '
        'AdjustedReceivedCostTextBox
        '
        Me.AdjustedReceivedCostTextBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AdjustedReceivedCostTextBox.Enabled = False
        Me.AdjustedReceivedCostTextBox.Location = New System.Drawing.Point(672, 60)
        Me.AdjustedReceivedCostTextBox.Name = "AdjustedReceivedCostTextBox"
        Me.AdjustedReceivedCostTextBox.Size = New System.Drawing.Size(100, 23)
        Me.AdjustedReceivedCostTextBox.TabIndex = 13
        Me.AdjustedReceivedCostTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ToolTip1.SetToolTip(Me.AdjustedReceivedCostTextBox, "This is the cost of the items as received, taking into account quantities and dif" & _
        "ferences.")
        '
        'OrderedCostTextBox
        '
        Me.OrderedCostTextBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.OrderedCostTextBox.Enabled = False
        Me.OrderedCostTextBox.Location = New System.Drawing.Point(672, 8)
        Me.OrderedCostTextBox.Name = "OrderedCostTextBox"
        Me.OrderedCostTextBox.Size = New System.Drawing.Size(100, 23)
        Me.OrderedCostTextBox.TabIndex = 13
        Me.OrderedCostTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ToolTip1.SetToolTip(Me.OrderedCostTextBox, "This is the cost, as ordered, using the current vendor cost file.")
        Me.OrderedCostTextBox.Visible = False
        '
        'UploadedCostLabel
        '
        Me.UploadedCostLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.UploadedCostLabel.BackColor = System.Drawing.Color.Transparent
        Me.UploadedCostLabel.Cursor = System.Windows.Forms.Cursors.Default
        Me.UploadedCostLabel.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.UploadedCostLabel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.UploadedCostLabel.Location = New System.Drawing.Point(545, 88)
        Me.UploadedCostLabel.Name = "UploadedCostLabel"
        Me.UploadedCostLabel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.UploadedCostLabel.Size = New System.Drawing.Size(121, 17)
        Me.UploadedCostLabel.TabIndex = 83
        Me.UploadedCostLabel.Text = "Uploaded Cost :"
        Me.UploadedCostLabel.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me.ToolTip1.SetToolTip(Me.UploadedCostLabel, "This is the cost uploaded to PeopleSoft.")
        '
        'AdjustedReceivedCostLabel
        '
        Me.AdjustedReceivedCostLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AdjustedReceivedCostLabel.BackColor = System.Drawing.Color.Transparent
        Me.AdjustedReceivedCostLabel.Cursor = System.Windows.Forms.Cursors.Default
        Me.AdjustedReceivedCostLabel.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.AdjustedReceivedCostLabel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.AdjustedReceivedCostLabel.Location = New System.Drawing.Point(516, 63)
        Me.AdjustedReceivedCostLabel.Name = "AdjustedReceivedCostLabel"
        Me.AdjustedReceivedCostLabel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.AdjustedReceivedCostLabel.Size = New System.Drawing.Size(150, 17)
        Me.AdjustedReceivedCostLabel.TabIndex = 85
        Me.AdjustedReceivedCostLabel.Text = "Adjusted Received Cost :"
        Me.AdjustedReceivedCostLabel.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me.ToolTip1.SetToolTip(Me.AdjustedReceivedCostLabel, "This is the cost of the items as received, taking into account quantities and dif" & _
        "ferences.")
        '
        'OrderedCostLabel
        '
        Me.OrderedCostLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.OrderedCostLabel.BackColor = System.Drawing.Color.Transparent
        Me.OrderedCostLabel.Cursor = System.Windows.Forms.Cursors.Default
        Me.OrderedCostLabel.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OrderedCostLabel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.OrderedCostLabel.Location = New System.Drawing.Point(545, 11)
        Me.OrderedCostLabel.Name = "OrderedCostLabel"
        Me.OrderedCostLabel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.OrderedCostLabel.Size = New System.Drawing.Size(121, 17)
        Me.OrderedCostLabel.TabIndex = 86
        Me.OrderedCostLabel.Text = "Ordered Cost :"
        Me.OrderedCostLabel.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me.ToolTip1.SetToolTip(Me.OrderedCostLabel, "This is the cost, as ordered, using the current vendor cost file.")
        '
        'OrigianalReceivedCostLabel
        '
        Me.OrigianalReceivedCostLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.OrigianalReceivedCostLabel.BackColor = System.Drawing.Color.Transparent
        Me.OrigianalReceivedCostLabel.Cursor = System.Windows.Forms.Cursors.Default
        Me.OrigianalReceivedCostLabel.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OrigianalReceivedCostLabel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.OrigianalReceivedCostLabel.Location = New System.Drawing.Point(516, 37)
        Me.OrigianalReceivedCostLabel.Name = "OrigianalReceivedCostLabel"
        Me.OrigianalReceivedCostLabel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.OrigianalReceivedCostLabel.Size = New System.Drawing.Size(150, 17)
        Me.OrigianalReceivedCostLabel.TabIndex = 92
        Me.OrigianalReceivedCostLabel.Text = "Original Received Cost :"
        Me.OrigianalReceivedCostLabel.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me.ToolTip1.SetToolTip(Me.OrigianalReceivedCostLabel, "This is the cost, as ordered, using the vendor cost file as of the time of the or" & _
        "der.")
        '
        'OriginalReceivedCostTextBox
        '
        Me.OriginalReceivedCostTextBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.OriginalReceivedCostTextBox.Enabled = False
        Me.OriginalReceivedCostTextBox.Location = New System.Drawing.Point(672, 35)
        Me.OriginalReceivedCostTextBox.Name = "OriginalReceivedCostTextBox"
        Me.OriginalReceivedCostTextBox.Size = New System.Drawing.Size(100, 23)
        Me.OriginalReceivedCostTextBox.TabIndex = 93
        Me.OriginalReceivedCostTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ToolTip1.SetToolTip(Me.OriginalReceivedCostTextBox, "This is the cost, as ordered, using the vendor cost file as of the time of the or" & _
        "der.")
        '
        'txtPOCostDate
        '
        Me.txtPOCostDate.AcceptsReturn = True
        Me.txtPOCostDate.BackColor = System.Drawing.SystemColors.ControlLight
        Me.txtPOCostDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPOCostDate.Enabled = False
        Me.txtPOCostDate.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPOCostDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPOCostDate.Location = New System.Drawing.Point(89, 273)
        Me.txtPOCostDate.MaxLength = 0
        Me.txtPOCostDate.Name = "txtPOCostDate"
        Me.txtPOCostDate.ReadOnly = True
        Me.txtPOCostDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPOCostDate.Size = New System.Drawing.Size(111, 23)
        Me.txtPOCostDate.TabIndex = 94
        Me.txtPOCostDate.TabStop = False
        Me.ToolTip1.SetToolTip(Me.txtPOCostDate, "If PO Cost Date is after Sent Date, this vendor uses lead-time costs.")
        '
        'lblPOCostDate
        '
        Me.lblPOCostDate.BackColor = System.Drawing.Color.Transparent
        Me.lblPOCostDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPOCostDate.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPOCostDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPOCostDate.Location = New System.Drawing.Point(1, 276)
        Me.lblPOCostDate.Name = "lblPOCostDate"
        Me.lblPOCostDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPOCostDate.Size = New System.Drawing.Size(85, 17)
        Me.lblPOCostDate.TabIndex = 95
        Me.lblPOCostDate.Text = "PO Cost Date :"
        Me.lblPOCostDate.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me.ToolTip1.SetToolTip(Me.lblPOCostDate, "This is the date used to retrieve vendor's cost.")
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
        Me._cmbField_2.Location = New System.Drawing.Point(168, 168)
        Me._cmbField_2.Name = "_cmbField_2"
        Me._cmbField_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmbField_2.Size = New System.Drawing.Size(145, 24)
        Me._cmbField_2.TabIndex = 17
        Me.ToolTip1.SetToolTip(Me._cmbField_2, "Discount Type. Please note, a Percent Discount is applied using the Reg Cost.")
        '
        'cmdItemsRefused
        '
        Me.cmdItemsRefused.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdItemsRefused.BackColor = System.Drawing.SystemColors.Control
        Me.cmdItemsRefused.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdItemsRefused.Enabled = False
        Me.cmdItemsRefused.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdItemsRefused.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdItemsRefused.Image = Global.My.Resources.Resources.Refusal32
        Me.cmdItemsRefused.Location = New System.Drawing.Point(731, 280)
        Me.cmdItemsRefused.Name = "cmdItemsRefused"
        Me.cmdItemsRefused.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdItemsRefused.Size = New System.Drawing.Size(41, 41)
        Me.cmdItemsRefused.TabIndex = 108
        Me.cmdItemsRefused.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdItemsRefused, "Refused Items")
        Me.cmdItemsRefused.UseVisualStyleBackColor = False
        '
        '_txtField_15
        '
        Me._txtField_15.AcceptsReturn = True
        Me._txtField_15.BackColor = System.Drawing.SystemColors.ControlLight
        Me._txtField_15.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtField_15.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtField_15.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_15, CType(15, Short))
        Me._txtField_15.Location = New System.Drawing.Point(436, 8)
        Me._txtField_15.MaxLength = 16
        Me._txtField_15.Name = "_txtField_15"
        Me._txtField_15.ReadOnly = True
        Me._txtField_15.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtField_15.Size = New System.Drawing.Size(97, 23)
        Me._txtField_15.TabIndex = 63
        Me._txtField_15.Tag = "String"
        '
        'Timer1
        '
        Me.Timer1.Interval = 60000
        '
        '_txtField_13
        '
        Me._txtField_13.AcceptsReturn = True
        Me._txtField_13.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_13.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtField_13.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtField_13.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_13, CType(13, Short))
        Me._txtField_13.Location = New System.Drawing.Point(239, 196)
        Me._txtField_13.MaxLength = 16
        Me._txtField_13.Name = "_txtField_13"
        Me._txtField_13.ReadOnly = True
        Me._txtField_13.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtField_13.Size = New System.Drawing.Size(74, 23)
        Me._txtField_13.TabIndex = 19
        Me._txtField_13.TabStop = False
        Me._txtField_13.Tag = "String"
        '
        '_txtField_9
        '
        Me._txtField_9.AcceptsReturn = True
        Me._txtField_9.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_9.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtField_9.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtField_9.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_9, CType(9, Short))
        Me._txtField_9.Location = New System.Drawing.Point(89, 196)
        Me._txtField_9.MaxLength = 16
        Me._txtField_9.Name = "_txtField_9"
        Me._txtField_9.ReadOnly = True
        Me._txtField_9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtField_9.Size = New System.Drawing.Size(73, 23)
        Me._txtField_9.TabIndex = 18
        Me._txtField_9.Tag = "String"
        '
        '_txtField_10
        '
        Me._txtField_10.AcceptsReturn = True
        Me._txtField_10.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._txtField_10.BackColor = System.Drawing.SystemColors.ControlLight
        Me._txtField_10.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtField_10.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtField_10.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_10, CType(10, Short))
        Me._txtField_10.Location = New System.Drawing.Point(672, 112)
        Me._txtField_10.MaxLength = 0
        Me._txtField_10.Name = "_txtField_10"
        Me._txtField_10.ReadOnly = True
        Me._txtField_10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtField_10.Size = New System.Drawing.Size(100, 23)
        Me._txtField_10.TabIndex = 13
        Me._txtField_10.TabStop = False
        Me._txtField_10.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._txtField_10.Visible = False
        '
        '_chkField_1
        '
        Me._chkField_1.BackColor = System.Drawing.SystemColors.Control
        Me._chkField_1.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me._chkField_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkField_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkField_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkField.SetIndex(Me._chkField_1, CType(1, Short))
        Me._chkField_1.Location = New System.Drawing.Point(375, 88)
        Me._chkField_1.Name = "_chkField_1"
        Me._chkField_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkField_1.Size = New System.Drawing.Size(76, 17)
        Me._chkField_1.TabIndex = 21
        Me._chkField_1.Text = "Return Order"
        Me._chkField_1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me._chkField_1.UseVisualStyleBackColor = False
        Me._chkField_1.Visible = False
        '
        '_txtField_12
        '
        Me._txtField_12.AcceptsReturn = True
        Me._txtField_12.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_12.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtField_12.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtField_12.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_12, CType(12, Short))
        Me._txtField_12.Location = New System.Drawing.Point(231, 8)
        Me._txtField_12.MaxLength = 16
        Me._txtField_12.Name = "_txtField_12"
        Me._txtField_12.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtField_12.Size = New System.Drawing.Size(137, 23)
        Me._txtField_12.TabIndex = 0
        Me._txtField_12.Tag = "String"
        '
        '_txtField_11
        '
        Me._txtField_11.AcceptsReturn = True
        Me._txtField_11.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_11.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtField_11.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtField_11.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_11, CType(11, Short))
        Me._txtField_11.Location = New System.Drawing.Point(436, 35)
        Me._txtField_11.MaxLength = 2
        Me._txtField_11.Name = "_txtField_11"
        Me._txtField_11.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtField_11.Size = New System.Drawing.Size(45, 23)
        Me._txtField_11.TabIndex = 6
        Me._txtField_11.Tag = "Number"
        '
        '_txtField_7
        '
        Me._txtField_7.AcceptsReturn = True
        Me._txtField_7.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_7.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtField_7.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtField_7.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_7, CType(7, Short))
        Me._txtField_7.Location = New System.Drawing.Point(88, 168)
        Me._txtField_7.MaxLength = 8
        Me._txtField_7.Name = "_txtField_7"
        Me._txtField_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtField_7.Size = New System.Drawing.Size(73, 23)
        Me._txtField_7.TabIndex = 16
        Me._txtField_7.Tag = "-ExtCurrency"
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
        Me._cmbField_0.Location = New System.Drawing.Point(88, 140)
        Me._cmbField_0.Name = "_cmbField_0"
        Me._cmbField_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmbField_0.Size = New System.Drawing.Size(225, 24)
        Me._cmbField_0.Sorted = True
        Me._cmbField_0.TabIndex = 15
        Me._cmbField_0.Tag = "B"
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
        Me._cmbField_1.Location = New System.Drawing.Point(88, 112)
        Me._cmbField_1.Name = "_cmbField_1"
        Me._cmbField_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmbField_1.Size = New System.Drawing.Size(225, 24)
        Me._cmbField_1.Sorted = True
        Me._cmbField_1.TabIndex = 14
        Me._cmbField_1.Tag = "B"
        '
        '_txtField_5
        '
        Me._txtField_5.AcceptsReturn = True
        Me._txtField_5.BackColor = System.Drawing.SystemColors.ControlLight
        Me._txtField_5.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtField_5.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtField_5.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_5, CType(5, Short))
        Me._txtField_5.Location = New System.Drawing.Point(436, 60)
        Me._txtField_5.MaxLength = 0
        Me._txtField_5.Name = "_txtField_5"
        Me._txtField_5.ReadOnly = True
        Me._txtField_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtField_5.Size = New System.Drawing.Size(77, 23)
        Me._txtField_5.TabIndex = 9
        Me._txtField_5.TabStop = False
        '
        '_txtField_4
        '
        Me._txtField_4.AcceptsReturn = True
        Me._txtField_4.BackColor = System.Drawing.SystemColors.ControlLight
        Me._txtField_4.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtField_4.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtField_4.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_4, CType(4, Short))
        Me._txtField_4.Location = New System.Drawing.Point(88, 86)
        Me._txtField_4.MaxLength = 0
        Me._txtField_4.Name = "_txtField_4"
        Me._txtField_4.ReadOnly = True
        Me._txtField_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtField_4.Size = New System.Drawing.Size(112, 23)
        Me._txtField_4.TabIndex = 11
        Me._txtField_4.TabStop = False
        '
        '_txtField_2
        '
        Me._txtField_2.AcceptsReturn = True
        Me._txtField_2.BackColor = System.Drawing.SystemColors.ControlLight
        Me._txtField_2.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtField_2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtField_2.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_2, CType(2, Short))
        Me._txtField_2.Location = New System.Drawing.Point(268, 222)
        Me._txtField_2.MaxLength = 0
        Me._txtField_2.Name = "_txtField_2"
        Me._txtField_2.ReadOnly = True
        Me._txtField_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtField_2.Size = New System.Drawing.Size(100, 23)
        Me._txtField_2.TabIndex = 2
        Me._txtField_2.TabStop = False
        '
        '_txtField_3
        '
        Me._txtField_3.AcceptsReturn = True
        Me._txtField_3.BackColor = System.Drawing.SystemColors.ControlLight
        Me._txtField_3.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtField_3.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtField_3.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_3, CType(3, Short))
        Me._txtField_3.Location = New System.Drawing.Point(88, 60)
        Me._txtField_3.MaxLength = 10
        Me._txtField_3.Name = "_txtField_3"
        Me._txtField_3.ReadOnly = True
        Me._txtField_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtField_3.Size = New System.Drawing.Size(78, 23)
        Me._txtField_3.TabIndex = 7
        Me._txtField_3.TabStop = False
        Me._txtField_3.Tag = "Date"
        '
        '_txtField_1
        '
        Me._txtField_1.AcceptsReturn = True
        Me._txtField_1.BackColor = System.Drawing.SystemColors.ControlLight
        Me._txtField_1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtField_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtField_1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_1, CType(1, Short))
        Me._txtField_1.Location = New System.Drawing.Point(88, 34)
        Me._txtField_1.MaxLength = 0
        Me._txtField_1.Name = "_txtField_1"
        Me._txtField_1.ReadOnly = True
        Me._txtField_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtField_1.Size = New System.Drawing.Size(281, 23)
        Me._txtField_1.TabIndex = 5
        Me._txtField_1.TabStop = False
        '
        '_txtField_0
        '
        Me._txtField_0.AcceptsReturn = True
        Me._txtField_0.BackColor = System.Drawing.SystemColors.ControlLight
        Me._txtField_0.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtField_0.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtField_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_0, CType(0, Short))
        Me._txtField_0.Location = New System.Drawing.Point(88, 8)
        Me._txtField_0.MaxLength = 0
        Me._txtField_0.Name = "_txtField_0"
        Me._txtField_0.ReadOnly = True
        Me._txtField_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtField_0.Size = New System.Drawing.Size(73, 23)
        Me._txtField_0.TabIndex = 0
        Me._txtField_0.TabStop = False
        '
        '_lblLabel_16
        '
        Me._lblLabel_16.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_16.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_16.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_16.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_16, CType(16, Short))
        Me._lblLabel_16.Location = New System.Drawing.Point(377, 12)
        Me._lblLabel_16.Name = "_lblLabel_16"
        Me._lblLabel_16.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_16.Size = New System.Drawing.Size(53, 17)
        Me._lblLabel_16.TabIndex = 64
        Me._lblLabel_16.Text = "Inv Amt :"
        Me._lblLabel_16.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_11
        '
        Me._lblLabel_11.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_11.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_11.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_11.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_11, CType(11, Short))
        Me._lblLabel_11.Location = New System.Drawing.Point(176, 199)
        Me._lblLabel_11.Name = "_lblLabel_11"
        Me._lblLabel_11.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_11.Size = New System.Drawing.Size(57, 17)
        Me._lblLabel_11.TabIndex = 60
        Me._lblLabel_11.Text = "Rec Log :"
        '
        'lblOrgPO
        '
        Me.lblOrgPO.BackColor = System.Drawing.SystemColors.Control
        Me.lblOrgPO.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblOrgPO.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOrgPO.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblOrgPO.Location = New System.Drawing.Point(13, 199)
        Me.lblOrgPO.Name = "lblOrgPO"
        Me.lblOrgPO.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblOrgPO.Size = New System.Drawing.Size(73, 17)
        Me.lblOrgPO.TabIndex = 59
        Me.lblOrgPO.Text = "Original PO :"
        '
        'lblCredit
        '
        Me.lblCredit.BackColor = System.Drawing.SystemColors.Control
        Me.lblCredit.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCredit.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCredit.ForeColor = System.Drawing.Color.Red
        Me.lblCredit.Location = New System.Drawing.Point(12, 297)
        Me.lblCredit.Name = "lblCredit"
        Me.lblCredit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCredit.Size = New System.Drawing.Size(350, 17)
        Me.lblCredit.TabIndex = 58
        '
        '_lblLabel_14
        '
        Me._lblLabel_14.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_14.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_14.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_14.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_14, CType(14, Short))
        Me._lblLabel_14.Location = New System.Drawing.Point(176, 11)
        Me._lblLabel_14.Name = "_lblLabel_14"
        Me._lblLabel_14.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_14.Size = New System.Drawing.Size(49, 17)
        Me._lblLabel_14.TabIndex = 41
        Me._lblLabel_14.Text = "Inv No :"
        Me._lblLabel_14.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_13
        '
        Me._lblLabel_13.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_13.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_13.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_13.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_13, CType(13, Short))
        Me._lblLabel_13.Location = New System.Drawing.Point(481, 37)
        Me._lblLabel_13.Name = "_lblLabel_13"
        Me._lblLabel_13.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_13.Size = New System.Drawing.Size(25, 17)
        Me._lblLabel_13.TabIndex = 52
        Me._lblLabel_13.Text = "°F"
        '
        '_lblLabel_9
        '
        Me._lblLabel_9.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_9.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_9.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_9.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_9, CType(9, Short))
        Me._lblLabel_9.Location = New System.Drawing.Point(377, 38)
        Me._lblLabel_9.Name = "_lblLabel_9"
        Me._lblLabel_9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_9.Size = New System.Drawing.Size(53, 17)
        Me._lblLabel_9.TabIndex = 51
        Me._lblLabel_9.Text = "Temp :"
        Me._lblLabel_9.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblReadOnly
        '
        Me.lblReadOnly.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblReadOnly.BackColor = System.Drawing.Color.Transparent
        Me.lblReadOnly.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReadOnly.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReadOnly.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.lblReadOnly.Location = New System.Drawing.Point(135, 546)
        Me.lblReadOnly.Name = "lblReadOnly"
        Me.lblReadOnly.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblReadOnly.Size = New System.Drawing.Size(592, 18)
        Me.lblReadOnly.TabIndex = 55
        Me.lblReadOnly.Text = "Read Only"
        Me.lblReadOnly.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.lblReadOnly.Visible = False
        '
        '_lblLabel_31
        '
        Me._lblLabel_31.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_31.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_31.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_31.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_31, CType(31, Short))
        Me._lblLabel_31.Location = New System.Drawing.Point(13, 171)
        Me._lblLabel_31.Name = "_lblLabel_31"
        Me._lblLabel_31.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_31.Size = New System.Drawing.Size(73, 17)
        Me._lblLabel_31.TabIndex = 50
        Me._lblLabel_31.Text = "Discount :"
        Me._lblLabel_31.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_8
        '
        Me._lblLabel_8.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_8.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_8.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_8.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_8, CType(8, Short))
        Me._lblLabel_8.Location = New System.Drawing.Point(176, 63)
        Me._lblLabel_8.Name = "_lblLabel_8"
        Me._lblLabel_8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_8.Size = New System.Drawing.Size(96, 17)
        Me._lblLabel_8.TabIndex = 45
        Me._lblLabel_8.Text = "Expected Date :"
        Me._lblLabel_8.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_7
        '
        Me._lblLabel_7.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_7.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_7.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_7.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_7, CType(7, Short))
        Me._lblLabel_7.Location = New System.Drawing.Point(13, 143)
        Me._lblLabel_7.Name = "_lblLabel_7"
        Me._lblLabel_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_7.Size = New System.Drawing.Size(73, 17)
        Me._lblLabel_7.TabIndex = 49
        Me._lblLabel_7.Text = "Ship To :"
        Me._lblLabel_7.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_6
        '
        Me._lblLabel_6.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_6.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_6.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_6, CType(6, Short))
        Me._lblLabel_6.Location = New System.Drawing.Point(4, 114)
        Me._lblLabel_6.Name = "_lblLabel_6"
        Me._lblLabel_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_6.Size = New System.Drawing.Size(82, 16)
        Me._lblLabel_6.TabIndex = 48
        Me._lblLabel_6.Text = "Purchasing :"
        Me._lblLabel_6.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_5
        '
        Me._lblLabel_5.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_5.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_5, CType(5, Short))
        Me._lblLabel_5.Location = New System.Drawing.Point(377, 63)
        Me._lblLabel_5.Name = "_lblLabel_5"
        Me._lblLabel_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_5.Size = New System.Drawing.Size(53, 17)
        Me._lblLabel_5.TabIndex = 47
        Me._lblLabel_5.Text = "Closed :"
        Me._lblLabel_5.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_4
        '
        Me._lblLabel_4.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_4.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_4, CType(4, Short))
        Me._lblLabel_4.Location = New System.Drawing.Point(13, 88)
        Me._lblLabel_4.Name = "_lblLabel_4"
        Me._lblLabel_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_4.Size = New System.Drawing.Size(73, 17)
        Me._lblLabel_4.TabIndex = 46
        Me._lblLabel_4.Text = "Sent :"
        Me._lblLabel_4.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblStatus
        '
        Me.lblStatus.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblStatus.BackColor = System.Drawing.Color.Transparent
        Me.lblStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblStatus.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblStatus.Location = New System.Drawing.Point(135, 517)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblStatus.Size = New System.Drawing.Size(592, 27)
        Me.lblStatus.TabIndex = 53
        Me.lblStatus.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblLabel_3
        '
        Me._lblLabel_3.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_3.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_3, CType(3, Short))
        Me._lblLabel_3.Location = New System.Drawing.Point(207, 225)
        Me._lblLabel_3.Name = "_lblLabel_3"
        Me._lblLabel_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_3.Size = New System.Drawing.Size(55, 17)
        Me._lblLabel_3.TabIndex = 42
        Me._lblLabel_3.Text = "Creator :"
        Me._lblLabel_3.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_2
        '
        Me._lblLabel_2.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_2, CType(2, Short))
        Me._lblLabel_2.Location = New System.Drawing.Point(13, 63)
        Me._lblLabel_2.Name = "_lblLabel_2"
        Me._lblLabel_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_2.Size = New System.Drawing.Size(73, 17)
        Me._lblLabel_2.TabIndex = 44
        Me._lblLabel_2.Text = "Order Date :"
        Me._lblLabel_2.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_1
        '
        Me._lblLabel_1.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_1, CType(1, Short))
        Me._lblLabel_1.Location = New System.Drawing.Point(13, 38)
        Me._lblLabel_1.Name = "_lblLabel_1"
        Me._lblLabel_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_1.Size = New System.Drawing.Size(73, 17)
        Me._lblLabel_1.TabIndex = 43
        Me._lblLabel_1.Text = "Vendor :"
        Me._lblLabel_1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_0
        '
        Me._lblLabel_0.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_0.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_0, CType(0, Short))
        Me._lblLabel_0.Location = New System.Drawing.Point(5, 12)
        Me._lblLabel_0.Name = "_lblLabel_0"
        Me._lblLabel_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_0.Size = New System.Drawing.Size(81, 17)
        Me._lblLabel_0.TabIndex = 40
        Me._lblLabel_0.Text = "PO Number :"
        Me._lblLabel_0.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'chkField
        '
        '
        'cmbField
        '
        '
        'txtField
        '
        '
        '_txtField_16
        '
        Me._txtField_16.AcceptsReturn = True
        Me._txtField_16.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._txtField_16.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_16.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtField_16.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtField_16.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_16, CType(16, Short))
        Me._txtField_16.Location = New System.Drawing.Point(672, 138)
        Me._txtField_16.MaxLength = 0
        Me._txtField_16.Name = "_txtField_16"
        Me._txtField_16.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtField_16.Size = New System.Drawing.Size(100, 23)
        Me._txtField_16.TabIndex = 70
        Me._txtField_16.TabStop = False
        Me._txtField_16.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._txtField_16.Visible = False
        '
        'ugrdItems
        '
        Me.ugrdItems.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Appearance24.BackColor = System.Drawing.SystemColors.ControlLight
        Appearance24.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.ugrdItems.DisplayLayout.Appearance = Appearance24
        Me.ugrdItems.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        UltraGridColumn1.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn1.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn1.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn1.Header.Caption = "ID"
        UltraGridColumn1.Header.VisiblePosition = 0
        UltraGridColumn1.Hidden = True
        UltraGridColumn2.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn2.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        Appearance25.TextHAlignAsString = "Right"
        UltraGridColumn2.CellAppearance = Appearance25
        UltraGridColumn2.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        Appearance26.TextHAlignAsString = "Right"
        UltraGridColumn2.Header.Appearance = Appearance26
        UltraGridColumn2.Header.VisiblePosition = 1
        UltraGridColumn2.MinWidth = 10
        UltraGridColumn2.RowLayoutColumnInfo.OriginX = 0
        UltraGridColumn2.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn2.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn2.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn2.Width = 10
        UltraGridColumn3.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn3.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        Appearance27.TextHAlignAsString = "Right"
        UltraGridColumn3.CellAppearance = Appearance27
        UltraGridColumn3.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        Appearance28.TextHAlignAsString = "Right"
        UltraGridColumn3.Header.Appearance = Appearance28
        UltraGridColumn3.Header.VisiblePosition = 2
        UltraGridColumn3.MinWidth = 60
        UltraGridColumn3.RowLayoutColumnInfo.OriginX = 2
        UltraGridColumn3.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn3.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn3.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn3.Width = 61
        UltraGridColumn4.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn4.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn4.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn4.Header.Caption = "Brand"
        UltraGridColumn4.Header.VisiblePosition = 3
        UltraGridColumn4.MinWidth = 100
        UltraGridColumn4.RowLayoutColumnInfo.OriginX = 4
        UltraGridColumn4.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn4.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn4.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn4.Width = 102
        UltraGridColumn5.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn5.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn5.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn5.Header.Caption = "Description"
        UltraGridColumn5.Header.VisiblePosition = 4
        UltraGridColumn5.MinWidth = 200
        UltraGridColumn5.RowLayoutColumnInfo.OriginX = 6
        UltraGridColumn5.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn5.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn5.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn5.Width = 286
        UltraGridColumn6.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn6.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        Appearance29.TextHAlignAsString = "Right"
        UltraGridColumn6.CellAppearance = Appearance29
        UltraGridColumn6.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        Appearance30.TextHAlignAsString = "Right"
        UltraGridColumn6.Header.Appearance = Appearance30
        UltraGridColumn6.Header.Caption = "Ordered"
        UltraGridColumn6.Header.VisiblePosition = 6
        UltraGridColumn6.MinWidth = 40
        UltraGridColumn6.RowLayoutColumnInfo.OriginX = 8
        UltraGridColumn6.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn6.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn6.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn6.Width = 79
        UltraGridColumn7.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn7.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        Appearance31.TextHAlignAsString = "Right"
        UltraGridColumn7.CellAppearance = Appearance31
        UltraGridColumn7.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn7.Format = "####0.0#"
        Appearance32.TextHAlignAsString = "Right"
        UltraGridColumn7.Header.Appearance = Appearance32
        UltraGridColumn7.Header.Caption = "Recvd"
        UltraGridColumn7.Header.VisiblePosition = 8
        UltraGridColumn7.MinWidth = 40
        UltraGridColumn7.RowLayoutColumnInfo.OriginX = 12
        UltraGridColumn7.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn7.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn7.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn7.Width = 81
        Appearance13.TextHAlignAsString = "Right"
        UltraGridColumn8.CellAppearance = Appearance13
        UltraGridColumn8.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.Edit
        UltraGridColumn8.DataType = GetType(Decimal)
        UltraGridColumn8.Format = "#0.00##"
        Appearance15.TextHAlignAsString = "Right"
        UltraGridColumn8.Header.Appearance = Appearance15
        UltraGridColumn8.Header.Caption = "Cost"
        UltraGridColumn8.Header.VisiblePosition = 7
        UltraGridColumn8.MinWidth = 24
        UltraGridColumn8.RowLayoutColumnInfo.OriginX = 10
        UltraGridColumn8.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn8.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(182, 0)
        UltraGridColumn8.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn8.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn8.Width = 53
        Appearance14.TextHAlignAsString = "Right"
        UltraGridColumn9.CellAppearance = Appearance14
        UltraGridColumn9.Format = "####0.###"
        Appearance16.TextHAlignAsString = "Right"
        UltraGridColumn9.Header.Appearance = Appearance16
        UltraGridColumn9.Header.VisiblePosition = 9
        UltraGridColumn9.MinWidth = 35
        UltraGridColumn9.RowLayoutColumnInfo.OriginX = 14
        UltraGridColumn9.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn9.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(118, 0)
        UltraGridColumn9.Width = 73
        UltraGridColumn10.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn10.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn10.Header.Caption = "Vendor Item Description"
        UltraGridColumn10.Header.VisiblePosition = 5
        UltraGridColumn10.Hidden = True
        UltraGridColumn10.Width = 102
        UltraGridColumn11.Header.VisiblePosition = 10
        UltraGridColumn11.Hidden = True
        UltraGridColumn11.Width = 102
        UltraGridBand1.Columns.AddRange(New Object() {UltraGridColumn1, UltraGridColumn2, UltraGridColumn3, UltraGridColumn4, UltraGridColumn5, UltraGridColumn6, UltraGridColumn7, UltraGridColumn8, UltraGridColumn9, UltraGridColumn10, UltraGridColumn11})
        Me.ugrdItems.DisplayLayout.BandsSerializer.Add(UltraGridBand1)
        Me.ugrdItems.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Appearance33.FontData.BoldAsString = "True"
        Me.ugrdItems.DisplayLayout.CaptionAppearance = Appearance33
        Me.ugrdItems.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[True]
        Me.ugrdItems.DisplayLayout.ColScrollRegions.Add(ColScrollRegion1)
        Me.ugrdItems.DisplayLayout.ColScrollRegions.Add(ColScrollRegion2)
        Appearance34.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance34.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance34.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance34.BorderColor = System.Drawing.SystemColors.Window
        Me.ugrdItems.DisplayLayout.GroupByBox.Appearance = Appearance34
        Appearance35.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugrdItems.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance35
        Me.ugrdItems.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.ugrdItems.DisplayLayout.GroupByBox.Hidden = True
        Appearance36.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance36.BackColor2 = System.Drawing.SystemColors.Control
        Appearance36.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance36.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugrdItems.DisplayLayout.GroupByBox.PromptAppearance = Appearance36
        Me.ugrdItems.DisplayLayout.MaxColScrollRegions = 1
        Me.ugrdItems.DisplayLayout.MaxRowScrollRegions = 1
        Appearance37.BackColor = System.Drawing.SystemColors.Window
        Appearance37.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ugrdItems.DisplayLayout.Override.ActiveCellAppearance = Appearance37
        Appearance38.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugrdItems.DisplayLayout.Override.ActiveRowAppearance = Appearance38
        Me.ugrdItems.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No
        Me.ugrdItems.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.[False]
        Me.ugrdItems.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.[True]
        Me.ugrdItems.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.ugrdItems.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance39.BackColor = System.Drawing.SystemColors.Window
        Me.ugrdItems.DisplayLayout.Override.CardAreaAppearance = Appearance39
        Appearance40.BorderColor = System.Drawing.Color.Silver
        Appearance40.FontData.BoldAsString = "True"
        Appearance40.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.ugrdItems.DisplayLayout.Override.CellAppearance = Appearance40
        Me.ugrdItems.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.ugrdItems.DisplayLayout.Override.CellPadding = 0
        Appearance41.FontData.BoldAsString = "True"
        Me.ugrdItems.DisplayLayout.Override.FixedHeaderAppearance = Appearance41
        Appearance42.BackColor = System.Drawing.SystemColors.Control
        Appearance42.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance42.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance42.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance42.BorderColor = System.Drawing.SystemColors.Window
        Me.ugrdItems.DisplayLayout.Override.GroupByRowAppearance = Appearance42
        Appearance43.FontData.BoldAsString = "True"
        Appearance43.TextHAlignAsString = "Left"
        Me.ugrdItems.DisplayLayout.Override.HeaderAppearance = Appearance43
        Me.ugrdItems.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle
        Me.ugrdItems.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance44.BackColor = System.Drawing.SystemColors.Control
        Me.ugrdItems.DisplayLayout.Override.RowAlternateAppearance = Appearance44
        Appearance45.BackColor = System.Drawing.SystemColors.Window
        Appearance45.BorderColor = System.Drawing.Color.Silver
        Me.ugrdItems.DisplayLayout.Override.RowAppearance = Appearance45
        Me.ugrdItems.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[True]
        Me.ugrdItems.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed
        Me.ugrdItems.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.ugrdItems.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.ugrdItems.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.[Single]
        Appearance46.BackColor = System.Drawing.SystemColors.ControlLight
        Me.ugrdItems.DisplayLayout.Override.TemplateAddRowAppearance = Appearance46
        Me.ugrdItems.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.ugrdItems.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.ugrdItems.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand
        Me.ugrdItems.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        Me.ugrdItems.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ugrdItems.Location = New System.Drawing.Point(8, 325)
        Me.ugrdItems.Name = "ugrdItems"
        Me.ugrdItems.Size = New System.Drawing.Size(766, 189)
        Me.ugrdItems.TabIndex = 65
        Me.ugrdItems.Text = "Order Items"
        '
        'sbMsg
        '
        Me.sbMsg.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tssl1})
        Me.sbMsg.Location = New System.Drawing.Point(0, 573)
        Me.sbMsg.Name = "sbMsg"
        Me.sbMsg.Size = New System.Drawing.Size(784, 22)
        Me.sbMsg.SizingGrip = False
        Me.sbMsg.TabIndex = 66
        '
        'tssl1
        '
        Me.tssl1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tssl1.Name = "tssl1"
        Me.tssl1.Size = New System.Drawing.Size(769, 17)
        Me.tssl1.Spring = True
        Me.tssl1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(210, 251)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(52, 17)
        Me.Label1.TabIndex = 67
        Me.Label1.Text = "Closer :"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'txtClosedBy
        '
        Me.txtClosedBy.AcceptsReturn = True
        Me.txtClosedBy.BackColor = System.Drawing.SystemColors.ControlLight
        Me.txtClosedBy.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtClosedBy.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtClosedBy.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtClosedBy.Location = New System.Drawing.Point(268, 248)
        Me.txtClosedBy.MaxLength = 0
        Me.txtClosedBy.Name = "txtClosedBy"
        Me.txtClosedBy.ReadOnly = True
        Me.txtClosedBy.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtClosedBy.Size = New System.Drawing.Size(100, 23)
        Me.txtClosedBy.TabIndex = 68
        Me.txtClosedBy.TabStop = False
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(519, 141)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(147, 17)
        Me.Label2.TabIndex = 71
        Me.Label2.Text = "3rd Party Freight :"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblTransmissionType
        '
        Me.lblTransmissionType.BackColor = System.Drawing.SystemColors.Control
        Me.lblTransmissionType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTransmissionType.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTransmissionType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTransmissionType.Location = New System.Drawing.Point(319, 141)
        Me.lblTransmissionType.Name = "lblTransmissionType"
        Me.lblTransmissionType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTransmissionType.Size = New System.Drawing.Size(224, 21)
        Me.lblTransmissionType.TabIndex = 77
        Me.lblTransmissionType.Text = "Transmission Type :"
        '
        'CheckBox_DropShip
        '
        Me.CheckBox_DropShip.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox_DropShip.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckBox_DropShip.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox_DropShip.Enabled = False
        Me.CheckBox_DropShip.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox_DropShip.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox_DropShip.Location = New System.Drawing.Point(457, 88)
        Me.CheckBox_DropShip.Name = "CheckBox_DropShip"
        Me.CheckBox_DropShip.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox_DropShip.Size = New System.Drawing.Size(79, 17)
        Me.CheckBox_DropShip.TabIndex = 78
        Me.CheckBox_DropShip.Text = "Drop Ship"
        Me.CheckBox_DropShip.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.CheckBox_DropShip.UseVisualStyleBackColor = False
        Me.CheckBox_DropShip.Visible = False
        '
        '_lblLabel_12
        '
        Me._lblLabel_12.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._lblLabel_12.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_12.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_12.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_12.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabel_12.Location = New System.Drawing.Point(545, 115)
        Me._lblLabel_12.Name = "_lblLabel_12"
        Me._lblLabel_12.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_12.Size = New System.Drawing.Size(121, 17)
        Me._lblLabel_12.TabIndex = 57
        Me._lblLabel_12.Text = "Vendor Freight :"
        Me._lblLabel_12.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'txtApprover
        '
        Me.txtApprover.AcceptsReturn = True
        Me.txtApprover.BackColor = System.Drawing.SystemColors.ControlLight
        Me.txtApprover.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtApprover.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtApprover.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtApprover.Location = New System.Drawing.Point(89, 222)
        Me.txtApprover.MaxLength = 0
        Me.txtApprover.Name = "txtApprover"
        Me.txtApprover.ReadOnly = True
        Me.txtApprover.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtApprover.Size = New System.Drawing.Size(111, 23)
        Me.txtApprover.TabIndex = 88
        Me.txtApprover.TabStop = False
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label3.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label3.Location = New System.Drawing.Point(19, 225)
        Me.Label3.Name = "Label3"
        Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label3.Size = New System.Drawing.Size(67, 17)
        Me.Label3.TabIndex = 87
        Me.Label3.Text = "Approver :"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'txtApproved
        '
        Me.txtApproved.AcceptsReturn = True
        Me.txtApproved.BackColor = System.Drawing.SystemColors.ControlLight
        Me.txtApproved.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtApproved.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtApproved.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtApproved.Location = New System.Drawing.Point(89, 248)
        Me.txtApproved.MaxLength = 0
        Me.txtApproved.Name = "txtApproved"
        Me.txtApproved.ReadOnly = True
        Me.txtApproved.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtApproved.Size = New System.Drawing.Size(111, 23)
        Me.txtApproved.TabIndex = 89
        Me.txtApproved.TabStop = False
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label4.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label4.Location = New System.Drawing.Point(19, 251)
        Me.Label4.Name = "Label4"
        Me.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label4.Size = New System.Drawing.Size(67, 17)
        Me.Label4.TabIndex = 90
        Me.Label4.Text = "Approved :"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'LabelCancelled
        '
        Me.LabelCancelled.BackColor = System.Drawing.SystemColors.Control
        Me.LabelCancelled.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelCancelled.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelCancelled.ForeColor = System.Drawing.Color.Red
        Me.LabelCancelled.Location = New System.Drawing.Point(135, 544)
        Me.LabelCancelled.Name = "LabelCancelled"
        Me.LabelCancelled.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelCancelled.Size = New System.Drawing.Size(127, 17)
        Me.LabelCancelled.TabIndex = 91
        '
        'chkShowVendorDescription
        '
        Me.chkShowVendorDescription.AutoSize = True
        Me.chkShowVendorDescription.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkShowVendorDescription.Location = New System.Drawing.Point(203, 276)
        Me.chkShowVendorDescription.Name = "chkShowVendorDescription"
        Me.chkShowVendorDescription.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.chkShowVendorDescription.Size = New System.Drawing.Size(207, 20)
        Me.chkShowVendorDescription.TabIndex = 96
        Me.chkShowVendorDescription.Text = "Show Vendor Description"
        Me.chkShowVendorDescription.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(319, 171)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(110, 16)
        Me.Label5.TabIndex = 98
        Me.Label5.Text = "Reason Code :"
        '
        'FormValidator
        '
        Me.FormValidator.ContainerControl = Me
        '
        'ucCostAdjReasonCode
        '
        Me.ucCostAdjReasonCode.CheckedListSettings.CheckStateMember = ""
        Appearance1.BackColor = System.Drawing.SystemColors.Window
        Appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.ucCostAdjReasonCode.DisplayLayout.Appearance = Appearance1
        Me.ucCostAdjReasonCode.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.ucCostAdjReasonCode.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance2.BorderColor = System.Drawing.SystemColors.Window
        Me.ucCostAdjReasonCode.DisplayLayout.GroupByBox.Appearance = Appearance2
        Appearance4.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ucCostAdjReasonCode.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance4
        Me.ucCostAdjReasonCode.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Appearance3.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance3.BackColor2 = System.Drawing.SystemColors.Control
        Appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance3.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ucCostAdjReasonCode.DisplayLayout.GroupByBox.PromptAppearance = Appearance3
        Me.ucCostAdjReasonCode.DisplayLayout.MaxColScrollRegions = 1
        Me.ucCostAdjReasonCode.DisplayLayout.MaxRowScrollRegions = 1
        Appearance9.BackColor = System.Drawing.SystemColors.Window
        Appearance9.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ucCostAdjReasonCode.DisplayLayout.Override.ActiveCellAppearance = Appearance9
        Appearance5.BackColor = System.Drawing.SystemColors.Highlight
        Appearance5.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.ucCostAdjReasonCode.DisplayLayout.Override.ActiveRowAppearance = Appearance5
        Me.ucCostAdjReasonCode.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.ucCostAdjReasonCode.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance12.BackColor = System.Drawing.SystemColors.Window
        Me.ucCostAdjReasonCode.DisplayLayout.Override.CardAreaAppearance = Appearance12
        Appearance8.BorderColor = System.Drawing.Color.Silver
        Appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.ucCostAdjReasonCode.DisplayLayout.Override.CellAppearance = Appearance8
        Me.ucCostAdjReasonCode.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.ucCostAdjReasonCode.DisplayLayout.Override.CellPadding = 0
        Appearance6.BackColor = System.Drawing.SystemColors.Control
        Appearance6.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance6.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance6.BorderColor = System.Drawing.SystemColors.Window
        Me.ucCostAdjReasonCode.DisplayLayout.Override.GroupByRowAppearance = Appearance6
        Appearance7.TextHAlignAsString = "Left"
        Me.ucCostAdjReasonCode.DisplayLayout.Override.HeaderAppearance = Appearance7
        Me.ucCostAdjReasonCode.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti
        Me.ucCostAdjReasonCode.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance10.BackColor = System.Drawing.SystemColors.Window
        Appearance10.BorderColor = System.Drawing.Color.Silver
        Me.ucCostAdjReasonCode.DisplayLayout.Override.RowAppearance = Appearance10
        Me.ucCostAdjReasonCode.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[False]
        Appearance11.BackColor = System.Drawing.SystemColors.ControlLight
        Me.ucCostAdjReasonCode.DisplayLayout.Override.TemplateAddRowAppearance = Appearance11
        Me.ucCostAdjReasonCode.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.ucCostAdjReasonCode.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.ucCostAdjReasonCode.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        Me.ucCostAdjReasonCode.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList
        Me.ucCostAdjReasonCode.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ucCostAdjReasonCode.Location = New System.Drawing.Point(411, 167)
        Me.ucCostAdjReasonCode.Name = "ucCostAdjReasonCode"
        Me.ucCostAdjReasonCode.Size = New System.Drawing.Size(61, 25)
        Me.ucCostAdjReasonCode.TabIndex = 99
        '
        'txtRecvDate
        '
        Me.txtRecvDate.AcceptsReturn = True
        Me.txtRecvDate.BackColor = System.Drawing.SystemColors.ControlLight
        Me.txtRecvDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRecvDate.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRecvDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtRecvDate.Location = New System.Drawing.Point(278, 85)
        Me.txtRecvDate.MaxLength = 0
        Me.txtRecvDate.Name = "txtRecvDate"
        Me.txtRecvDate.ReadOnly = True
        Me.txtRecvDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRecvDate.Size = New System.Drawing.Size(91, 23)
        Me.txtRecvDate.TabIndex = 100
        Me.txtRecvDate.TabStop = False
        '
        'lblRecvDate
        '
        Me.lblRecvDate.BackColor = System.Drawing.Color.Transparent
        Me.lblRecvDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRecvDate.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRecvDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRecvDate.Location = New System.Drawing.Point(205, 88)
        Me.lblRecvDate.Name = "lblRecvDate"
        Me.lblRecvDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRecvDate.Size = New System.Drawing.Size(67, 19)
        Me.lblRecvDate.TabIndex = 101
        Me.lblRecvDate.Text = "Recv Date :"
        Me.lblRecvDate.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'txtDeletedby
        '
        Me.txtDeletedby.AcceptsReturn = True
        Me.txtDeletedby.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDeletedby.BackColor = System.Drawing.SystemColors.ControlLight
        Me.txtDeletedby.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDeletedby.Enabled = False
        Me.txtDeletedby.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDeletedby.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDeletedby.Location = New System.Drawing.Point(672, 188)
        Me.txtDeletedby.MaxLength = 0
        Me.txtDeletedby.Name = "txtDeletedby"
        Me.txtDeletedby.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDeletedby.Size = New System.Drawing.Size(100, 23)
        Me.txtDeletedby.TabIndex = 102
        Me.txtDeletedby.TabStop = False
        Me.txtDeletedby.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtDeletedby.Visible = False
        '
        'lblDeletedBy
        '
        Me.lblDeletedBy.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblDeletedBy.BackColor = System.Drawing.Color.Transparent
        Me.lblDeletedBy.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDeletedBy.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDeletedBy.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDeletedBy.Location = New System.Drawing.Point(571, 191)
        Me.lblDeletedBy.Name = "lblDeletedBy"
        Me.lblDeletedBy.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDeletedBy.Size = New System.Drawing.Size(95, 17)
        Me.lblDeletedBy.TabIndex = 103
        Me.lblDeletedBy.Text = "Deleted By :"
        Me.lblDeletedBy.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'txtDeletedDate
        '
        Me.txtDeletedDate.AcceptsReturn = True
        Me.txtDeletedDate.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDeletedDate.BackColor = System.Drawing.SystemColors.ControlLight
        Me.txtDeletedDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDeletedDate.Enabled = False
        Me.txtDeletedDate.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDeletedDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDeletedDate.Location = New System.Drawing.Point(672, 215)
        Me.txtDeletedDate.MaxLength = 0
        Me.txtDeletedDate.Name = "txtDeletedDate"
        Me.txtDeletedDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDeletedDate.Size = New System.Drawing.Size(100, 23)
        Me.txtDeletedDate.TabIndex = 104
        Me.txtDeletedDate.TabStop = False
        Me.txtDeletedDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtDeletedDate.Visible = False
        '
        'lblDeletedDate
        '
        Me.lblDeletedDate.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblDeletedDate.BackColor = System.Drawing.Color.Transparent
        Me.lblDeletedDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDeletedDate.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDeletedDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDeletedDate.Location = New System.Drawing.Point(548, 218)
        Me.lblDeletedDate.Name = "lblDeletedDate"
        Me.lblDeletedDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDeletedDate.Size = New System.Drawing.Size(118, 17)
        Me.lblDeletedDate.TabIndex = 105
        Me.lblDeletedDate.Text = "Deleted Date :"
        Me.lblDeletedDate.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'txtDeletedReason
        '
        Me.txtDeletedReason.AcceptsReturn = True
        Me.txtDeletedReason.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDeletedReason.BackColor = System.Drawing.SystemColors.ControlLight
        Me.txtDeletedReason.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDeletedReason.Enabled = False
        Me.txtDeletedReason.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDeletedReason.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDeletedReason.Location = New System.Drawing.Point(420, 197)
        Me.txtDeletedReason.MaxLength = 0
        Me.txtDeletedReason.Name = "txtDeletedReason"
        Me.txtDeletedReason.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDeletedReason.Size = New System.Drawing.Size(143, 23)
        Me.txtDeletedReason.TabIndex = 106
        Me.txtDeletedReason.TabStop = False
        Me.txtDeletedReason.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtDeletedReason.Visible = False
        '
        'lblDeletedReason
        '
        Me.lblDeletedReason.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblDeletedReason.BackColor = System.Drawing.Color.Transparent
        Me.lblDeletedReason.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDeletedReason.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDeletedReason.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDeletedReason.Location = New System.Drawing.Point(319, 199)
        Me.lblDeletedReason.Name = "lblDeletedReason"
        Me.lblDeletedReason.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDeletedReason.Size = New System.Drawing.Size(106, 17)
        Me.lblDeletedReason.TabIndex = 107
        Me.lblDeletedReason.Text = "Deleted Reason :"
        Me.lblDeletedReason.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtRefusedTotal
        '
        Me.txtRefusedTotal.AcceptsReturn = True
        Me.txtRefusedTotal.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtRefusedTotal.BackColor = System.Drawing.SystemColors.ControlLight
        Me.txtRefusedTotal.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRefusedTotal.Enabled = False
        Me.txtRefusedTotal.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRefusedTotal.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtRefusedTotal.Location = New System.Drawing.Point(672, 164)
        Me.txtRefusedTotal.MaxLength = 0
        Me.txtRefusedTotal.Name = "txtRefusedTotal"
        Me.txtRefusedTotal.ReadOnly = True
        Me.txtRefusedTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRefusedTotal.Size = New System.Drawing.Size(100, 23)
        Me.txtRefusedTotal.TabIndex = 109
        Me.txtRefusedTotal.TabStop = False
        Me.txtRefusedTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblRefusedTotal
        '
        Me.lblRefusedTotal.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblRefusedTotal.BackColor = System.Drawing.Color.Transparent
        Me.lblRefusedTotal.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRefusedTotal.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRefusedTotal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRefusedTotal.Location = New System.Drawing.Point(519, 167)
        Me.lblRefusedTotal.Name = "lblRefusedTotal"
        Me.lblRefusedTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRefusedTotal.Size = New System.Drawing.Size(147, 17)
        Me.lblRefusedTotal.TabIndex = 110
        Me.lblRefusedTotal.Text = "Refused Total :"
        Me.lblRefusedTotal.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'dtpExpectedDate
        '
        Me.dtpExpectedDate.DateTime = New Date(2015, 2, 18, 0, 0, 0, 0)
        Me.dtpExpectedDate.Location = New System.Drawing.Point(278, 60)
        Me.dtpExpectedDate.Name = "dtpExpectedDate"
        Me.dtpExpectedDate.Size = New System.Drawing.Size(90, 24)
        Me.dtpExpectedDate.TabIndex = 111
        Me.dtpExpectedDate.Value = New Date(2015, 2, 18, 0, 0, 0, 0)
        '
        'chkApplyAllCreditReason
        '
        Me.chkApplyAllCreditReason.AutoSize = True
        Me.chkApplyAllCreditReason.Font = New System.Drawing.Font("Arial", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkApplyAllCreditReason.Location = New System.Drawing.Point(570, 327)
        Me.chkApplyAllCreditReason.Name = "chkApplyAllCreditReason"
        Me.chkApplyAllCreditReason.Size = New System.Drawing.Size(230, 19)
        Me.chkApplyAllCreditReason.TabIndex = 112
        Me.chkApplyAllCreditReason.Text = "Apply Credit Reason to All Line Items"
        Me.chkApplyAllCreditReason.UseVisualStyleBackColor = True
        Me.chkApplyAllCreditReason.Visible = False
        '
        'lblCreditReasonCode
        '
        Me.lblCreditReasonCode.AutoSize = True
        Me.lblCreditReasonCode.Location = New System.Drawing.Point(491, 328)
        Me.lblCreditReasonCode.Name = "lblCreditReasonCode"
        Me.lblCreditReasonCode.Size = New System.Drawing.Size(0, 16)
        Me.lblCreditReasonCode.TabIndex = 113
        Me.lblCreditReasonCode.Visible = False
        '
        'frmOrders
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(784, 595)
        Me.Controls.Add(Me.lblCreditReasonCode)
        Me.Controls.Add(Me.chkApplyAllCreditReason)
        Me.Controls.Add(Me.dtpExpectedDate)
        Me.Controls.Add(Me.txtRefusedTotal)
        Me.Controls.Add(Me.lblRefusedTotal)
        Me.Controls.Add(Me.cmdItemsRefused)
        Me.Controls.Add(Me.txtDeletedReason)
        Me.Controls.Add(Me.lblDeletedReason)
        Me.Controls.Add(Me.txtDeletedDate)
        Me.Controls.Add(Me.lblDeletedDate)
        Me.Controls.Add(Me.txtDeletedby)
        Me.Controls.Add(Me.lblDeletedBy)
        Me.Controls.Add(Me.txtRecvDate)
        Me.Controls.Add(Me.lblRecvDate)
        Me.Controls.Add(Me.ucCostAdjReasonCode)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.chkShowVendorDescription)
        Me.Controls.Add(Me.txtPOCostDate)
        Me.Controls.Add(Me.lblPOCostDate)
        Me.Controls.Add(Me.OriginalReceivedCostTextBox)
        Me.Controls.Add(Me.OrigianalReceivedCostLabel)
        Me.Controls.Add(Me.LabelCancelled)
        Me.Controls.Add(Me.txtApproved)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.txtApprover)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.OrderedCostLabel)
        Me.Controls.Add(Me.AdjustedReceivedCostLabel)
        Me.Controls.Add(Me.UploadedCostLabel)
        Me.Controls.Add(Me.OrderedCostTextBox)
        Me.Controls.Add(Me.AdjustedReceivedCostTextBox)
        Me.Controls.Add(Me.UploadedCostTextBox)
        Me.Controls.Add(Me.CheckBox_DropShip)
        Me.Controls.Add(Me.lblTransmissionType)
        Me.Controls.Add(Me.cmdCopyPO)
        Me.Controls.Add(Me.cmdUndoWarehouseSend)
        Me.Controls.Add(Me.cmdDisplayEInvoice)
        Me.Controls.Add(Me.cmd3rdPartyInvoice)
        Me.Controls.Add(Me._txtField_16)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtClosedBy)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.sbMsg)
        Me.Controls.Add(Me.ugrdItems)
        Me.Controls.Add(Me._txtField_15)
        Me.Controls.Add(Me.cmdReceive)
        Me.Controls.Add(Me.cmdChgVendor)
        Me.Controls.Add(Me.cmdWarehouseSend)
        Me.Controls.Add(Me._txtField_13)
        Me.Controls.Add(Me._txtField_9)
        Me.Controls.Add(Me.cmdCloseOrder)
        Me.Controls.Add(Me.cmdDistributionCreditOrder)
        Me.Controls.Add(Me.cmdDeleteItem)
        Me.Controls.Add(Me.cmdEditItem)
        Me.Controls.Add(Me.cmdAddItem)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.cmdUnlock)
        Me.Controls.Add(Me.cmdReports)
        Me.Controls.Add(Me.cmdSearch)
        Me.Controls.Add(Me.cmdSendOrder)
        Me.Controls.Add(Me._txtField_10)
        Me.Controls.Add(Me._chkField_1)
        Me.Controls.Add(Me.cmdOrderList)
        Me.Controls.Add(Me._txtField_12)
        Me.Controls.Add(Me._txtField_11)
        Me.Controls.Add(Me._cmbField_2)
        Me.Controls.Add(Me._txtField_7)
        Me.Controls.Add(Me.cmdOrderNotes)
        Me.Controls.Add(Me._cmbField_0)
        Me.Controls.Add(Me._cmbField_1)
        Me.Controls.Add(Me._txtField_5)
        Me.Controls.Add(Me._txtField_4)
        Me.Controls.Add(Me.cmdDeleteOrder)
        Me.Controls.Add(Me.cmdAddOrder)
        Me.Controls.Add(Me._txtField_2)
        Me.Controls.Add(Me._txtField_3)
        Me.Controls.Add(Me._txtField_1)
        Me.Controls.Add(Me._txtField_0)
        Me.Controls.Add(Me._lblLabel_16)
        Me.Controls.Add(Me._lblLabel_11)
        Me.Controls.Add(Me.lblOrgPO)
        Me.Controls.Add(Me.lblCredit)
        Me.Controls.Add(Me._lblLabel_12)
        Me.Controls.Add(Me._lblLabel_14)
        Me.Controls.Add(Me._lblLabel_13)
        Me.Controls.Add(Me._lblLabel_9)
        Me.Controls.Add(Me.lblReadOnly)
        Me.Controls.Add(Me._lblLabel_31)
        Me.Controls.Add(Me._lblLabel_8)
        Me.Controls.Add(Me._lblLabel_7)
        Me.Controls.Add(Me._lblLabel_6)
        Me.Controls.Add(Me._lblLabel_5)
        Me.Controls.Add(Me._lblLabel_4)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me._lblLabel_3)
        Me.Controls.Add(Me._lblLabel_2)
        Me.Controls.Add(Me._lblLabel_1)
        Me.Controls.Add(Me._lblLabel_0)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(264, 349)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(790, 590)
        Me.Name = "frmOrders"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Purchase Product Order Information"
        CType(Me.chkField, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cmbField, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.optCost, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtField, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.optPOTrans, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ugrdItems, System.ComponentModel.ISupportInitialize).EndInit()
        Me.sbMsg.ResumeLayout(False)
        Me.sbMsg.PerformLayout()
        CType(Me.FormValidator, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ucCostAdjReasonCode, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dtpExpectedDate, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ugrdItems As Infragistics.Win.UltraWinGrid.UltraGrid
    Friend WithEvents sbMsg As System.Windows.Forms.StatusStrip
    Friend WithEvents tssl1 As System.Windows.Forms.ToolStripStatusLabel
    Public WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents txtClosedBy As System.Windows.Forms.TextBox
    Public WithEvents _txtField_16 As System.Windows.Forms.TextBox
    Public WithEvents Label2 As System.Windows.Forms.Label
    Public WithEvents cmd3rdPartyInvoice As System.Windows.Forms.Button
    Public WithEvents cmdDisplayEInvoice As System.Windows.Forms.Button
    Public WithEvents cmdUndoWarehouseSend As System.Windows.Forms.Button
    Public WithEvents cmdCopyPO As System.Windows.Forms.Button
    Public WithEvents lblTransmissionType As System.Windows.Forms.Label
    Public WithEvents CheckBox_DropShip As System.Windows.Forms.CheckBox
    Friend WithEvents UploadedCostTextBox As System.Windows.Forms.TextBox
    Friend WithEvents AdjustedReceivedCostTextBox As System.Windows.Forms.TextBox
    Friend WithEvents OrderedCostTextBox As System.Windows.Forms.TextBox
    Public WithEvents _lblLabel_12 As System.Windows.Forms.Label
    Public WithEvents UploadedCostLabel As System.Windows.Forms.Label
    Public WithEvents AdjustedReceivedCostLabel As System.Windows.Forms.Label
    Public WithEvents OrderedCostLabel As System.Windows.Forms.Label
    Public WithEvents txtApprover As System.Windows.Forms.TextBox
    Public WithEvents Label3 As System.Windows.Forms.Label
    Public WithEvents txtApproved As System.Windows.Forms.TextBox
    Public WithEvents Label4 As System.Windows.Forms.Label
    Public WithEvents LabelCancelled As System.Windows.Forms.Label
    Public WithEvents OrigianalReceivedCostLabel As System.Windows.Forms.Label
    Friend WithEvents OriginalReceivedCostTextBox As System.Windows.Forms.TextBox
    Public WithEvents txtPOCostDate As System.Windows.Forms.TextBox
    Public WithEvents lblPOCostDate As System.Windows.Forms.Label
    Friend WithEvents chkShowVendorDescription As System.Windows.Forms.CheckBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents FormValidator As System.Windows.Forms.ErrorProvider
    Friend WithEvents ucCostAdjReasonCode As Infragistics.Win.UltraWinGrid.UltraCombo
    Public WithEvents txtRecvDate As System.Windows.Forms.TextBox
    Public WithEvents lblRecvDate As System.Windows.Forms.Label
    Public WithEvents txtDeletedReason As System.Windows.Forms.TextBox
    Public WithEvents lblDeletedReason As System.Windows.Forms.Label
    Public WithEvents txtDeletedDate As System.Windows.Forms.TextBox
    Public WithEvents lblDeletedDate As System.Windows.Forms.Label
    Public WithEvents txtDeletedby As System.Windows.Forms.TextBox
    Public WithEvents lblDeletedBy As System.Windows.Forms.Label
    Public WithEvents cmdItemsRefused As System.Windows.Forms.Button
    Public WithEvents txtRefusedTotal As System.Windows.Forms.TextBox
    Public WithEvents lblRefusedTotal As System.Windows.Forms.Label
    Friend WithEvents dtpExpectedDate As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
    Friend WithEvents chkApplyAllCreditReason As System.Windows.Forms.CheckBox
    Friend WithEvents lblCreditReasonCode As System.Windows.Forms.Label
#End Region
End Class
