<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmOrderList
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
	Public WithEvents chkDiscontinued As System.Windows.Forms.CheckBox
	Public WithEvents cmbDistSubTeam As System.Windows.Forms.ComboBox
	Public WithEvents _optEXE_1 As System.Windows.Forms.RadioButton
	Public WithEvents _optEXE_0 As System.Windows.Forms.RadioButton
	Public WithEvents fraEXE As System.Windows.Forms.GroupBox
    Public WithEvents _optPreOrder_2 As System.Windows.Forms.RadioButton
	Public WithEvents _optPreOrder_1 As System.Windows.Forms.RadioButton
	Public WithEvents fraDisplay As System.Windows.Forms.GroupBox
	Public WithEvents _optOrder_0 As System.Windows.Forms.RadioButton
	Public WithEvents _optOrder_1 As System.Windows.Forms.RadioButton
	Public WithEvents _optOrder_2 As System.Windows.Forms.RadioButton
	Public WithEvents Frame2 As System.Windows.Forms.GroupBox
	Public WithEvents chkOrdered As System.Windows.Forms.CheckBox
	Public WithEvents cmdSubmit As System.Windows.Forms.Button
	Public WithEvents cmdExit As System.Windows.Forms.Button
	Public WithEvents _txtField_1 As System.Windows.Forms.TextBox
	Public WithEvents _txtField_0 As System.Windows.Forms.TextBox
    Public WithEvents _lblLabel_5 As System.Windows.Forms.Label
	Public WithEvents lblStatus As System.Windows.Forms.Label
	Public WithEvents _lblLabel_1 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_2 As System.Windows.Forms.Label
	Public WithEvents lblLabel As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	Public WithEvents optEXE As Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray
	Public WithEvents optOrder As Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray
	Public WithEvents optPreOrder As Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray
	Public WithEvents txtField As Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmOrderList))
        Dim UltraGridBand1 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("Recordset", -1)
        Dim UltraGridColumn1 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Identifier")
        Dim UltraGridColumn2 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("NA")
        Dim UltraGridColumn3 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Item_Description")
        Dim UltraGridColumn4 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("QuantityOrdered")
        Dim Appearance13 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance14 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridColumn5 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("QuantityUnit", -1, 213751391)
        Dim UltraGridColumn6 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("PkgDesc")
        Dim UltraGridColumn7 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("CreditReason_ID", -1, 213704360, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, False)
        Dim UltraGridColumn8 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Item_Key")
        Dim UltraGridColumn9 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("OrderItem_ID")
        Dim UltraGridColumn10 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Category_Name")
        Dim UltraGridColumn11 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Category_ID")
        Dim UltraGridColumn12 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Pre_Order")
        Dim UltraGridColumn13 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("SubTeam_No")
        Dim UltraGridColumn14 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("EXEDistributed")
        Dim UltraGridColumn15 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("DistSubTeam_No")
        Dim UltraGridColumn16 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Discontinue_Item")
        Dim UltraGridColumn17 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("VendorItemID")
        Dim UltraGridColumn18 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Brand")
        Dim UltraGridColumn19 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Cost")
        Dim UltraGridColumn20 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("VendorCostHistoryId", 0)
        Dim UltraGridColumn21 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("VendorItemStatus", 1)
        Dim UltraGridColumn22 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("VendorItemStatusFull", 2)
        Dim ColScrollRegion1 As Infragistics.Win.UltraWinGrid.ColScrollRegion = New Infragistics.Win.UltraWinGrid.ColScrollRegion(885)
        Dim ColScrollRegion2 As Infragistics.Win.UltraWinGrid.ColScrollRegion = New Infragistics.Win.UltraWinGrid.ColScrollRegion(500)
        Dim ColScrollRegion3 As Infragistics.Win.UltraWinGrid.ColScrollRegion = New Infragistics.Win.UltraWinGrid.ColScrollRegion(-507)
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
        Dim ValueList1 As Infragistics.Win.ValueList = New Infragistics.Win.ValueList(213704360)
        Dim ValueList2 As Infragistics.Win.ValueList = New Infragistics.Win.ValueList(213751391)
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdSubmit = New System.Windows.Forms.Button()
        Me.cmdExit = New System.Windows.Forms.Button()
        Me.btnConversionCalculator = New System.Windows.Forms.Button()
        Me.chkDiscontinued = New System.Windows.Forms.CheckBox()
        Me.cmbDistSubTeam = New System.Windows.Forms.ComboBox()
        Me.fraEXE = New System.Windows.Forms.GroupBox()
        Me._optEXE_1 = New System.Windows.Forms.RadioButton()
        Me._optEXE_0 = New System.Windows.Forms.RadioButton()
        Me.fraDisplay = New System.Windows.Forms.GroupBox()
        Me._optPreOrder_2 = New System.Windows.Forms.RadioButton()
        Me._optPreOrder_1 = New System.Windows.Forms.RadioButton()
        Me.Frame2 = New System.Windows.Forms.GroupBox()
        Me.RadioButton_SortByStatus = New System.Windows.Forms.RadioButton()
        Me._optOrder_4 = New System.Windows.Forms.RadioButton()
        Me._optOrder_3 = New System.Windows.Forms.RadioButton()
        Me._optOrder_0 = New System.Windows.Forms.RadioButton()
        Me._optOrder_1 = New System.Windows.Forms.RadioButton()
        Me._optOrder_2 = New System.Windows.Forms.RadioButton()
        Me.chkOrdered = New System.Windows.Forms.CheckBox()
        Me._txtField_1 = New System.Windows.Forms.TextBox()
        Me._txtField_0 = New System.Windows.Forms.TextBox()
        Me._lblLabel_5 = New System.Windows.Forms.Label()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me._lblLabel_1 = New System.Windows.Forms.Label()
        Me._lblLabel_2 = New System.Windows.Forms.Label()
        Me.lblLabel = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(Me.components)
        Me.optEXE = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(Me.components)
        Me.optOrder = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(Me.components)
        Me.optPreOrder = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(Me.components)
        Me.txtField = New Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray(Me.components)
        Me.TextBoxVIN = New System.Windows.Forms.TextBox()
        Me.cmbCategory = New System.Windows.Forms.ComboBox()
        Me.cmbSubTeam = New System.Windows.Forms.ComboBox()
        Me.lblCategory = New System.Windows.Forms.Label()
        Me.lblSubTeam = New System.Windows.Forms.Label()
        Me.ugrdOrderList = New Infragistics.Win.UltraWinGrid.UltraGrid()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cmbBrand = New System.Windows.Forms.ComboBox()
        Me.chkIncludeNotAvailable = New System.Windows.Forms.CheckBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.chkVendorItemStatus_Seasonal = New System.Windows.Forms.CheckBox()
        Me.chkVendorItemStatus_VendorDiscontinued = New System.Windows.Forms.CheckBox()
        Me.chkVendorItemStatus_MfgDiscontinued = New System.Windows.Forms.CheckBox()
        Me.chkVendorItemStatus_NotAvailable = New System.Windows.Forms.CheckBox()
        Me.btnApplyAllCreditReason = New System.Windows.Forms.Button()
        Me.fraEXE.SuspendLayout()
        Me.fraDisplay.SuspendLayout()
        Me.Frame2.SuspendLayout()
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.optEXE, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.optOrder, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.optPreOrder, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtField, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ugrdOrderList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdSubmit
        '
        Me.cmdSubmit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSubmit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSubmit.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSubmit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSubmit.Image = CType(resources.GetObject("cmdSubmit.Image"), System.Drawing.Image)
        Me.cmdSubmit.Location = New System.Drawing.Point(808, 530)
        Me.cmdSubmit.Name = "cmdSubmit"
        Me.cmdSubmit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSubmit.Size = New System.Drawing.Size(41, 41)
        Me.cmdSubmit.TabIndex = 22
        Me.cmdSubmit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdSubmit, "Commit Changes")
        Me.cmdSubmit.UseVisualStyleBackColor = False
        '
        'cmdExit
        '
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdExit.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Image = CType(resources.GetObject("cmdExit.Image"), System.Drawing.Image)
        Me.cmdExit.Location = New System.Drawing.Point(855, 530)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(41, 41)
        Me.cmdExit.TabIndex = 23
        Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdExit, "Exit")
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'btnConversionCalculator
        '
        Me.btnConversionCalculator.Image = CType(resources.GetObject("btnConversionCalculator.Image"), System.Drawing.Image)
        Me.btnConversionCalculator.Location = New System.Drawing.Point(761, 531)
        Me.btnConversionCalculator.Name = "btnConversionCalculator"
        Me.btnConversionCalculator.Size = New System.Drawing.Size(41, 41)
        Me.btnConversionCalculator.TabIndex = 58
        Me.btnConversionCalculator.UseVisualStyleBackColor = True
        Me.btnConversionCalculator.Visible = False
        '
        'chkDiscontinued
        '
        Me.chkDiscontinued.BackColor = System.Drawing.SystemColors.Control
        Me.chkDiscontinued.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkDiscontinued.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkDiscontinued.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDiscontinued.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkDiscontinued.Location = New System.Drawing.Point(450, 137)
        Me.chkDiscontinued.Name = "chkDiscontinued"
        Me.chkDiscontinued.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkDiscontinued.Size = New System.Drawing.Size(153, 17)
        Me.chkDiscontinued.TabIndex = 19
        Me.chkDiscontinued.Text = "Include Discontinued :"
        Me.chkDiscontinued.UseVisualStyleBackColor = False
        '
        'cmbDistSubTeam
        '
        Me.cmbDistSubTeam.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbDistSubTeam.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbDistSubTeam.BackColor = System.Drawing.SystemColors.Window
        Me.cmbDistSubTeam.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbDistSubTeam.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbDistSubTeam.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbDistSubTeam.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbDistSubTeam.Location = New System.Drawing.Point(96, 162)
        Me.cmbDistSubTeam.Name = "cmbDistSubTeam"
        Me.cmbDistSubTeam.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbDistSubTeam.Size = New System.Drawing.Size(225, 22)
        Me.cmbDistSubTeam.Sorted = True
        Me.cmbDistSubTeam.TabIndex = 6
        '
        'fraEXE
        '
        Me.fraEXE.BackColor = System.Drawing.SystemColors.Control
        Me.fraEXE.Controls.Add(Me._optEXE_1)
        Me.fraEXE.Controls.Add(Me._optEXE_0)
        Me.fraEXE.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraEXE.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraEXE.Location = New System.Drawing.Point(344, 66)
        Me.fraEXE.Name = "fraEXE"
        Me.fraEXE.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraEXE.Size = New System.Drawing.Size(121, 52)
        Me.fraEXE.TabIndex = 10
        Me.fraEXE.TabStop = False
        '
        '_optEXE_1
        '
        Me._optEXE_1.BackColor = System.Drawing.SystemColors.Control
        Me._optEXE_1.Checked = True
        Me._optEXE_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._optEXE_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optEXE_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optEXE.SetIndex(Me._optEXE_1, CType(1, Short))
        Me._optEXE_1.Location = New System.Drawing.Point(8, 25)
        Me._optEXE_1.Name = "_optEXE_1"
        Me._optEXE_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optEXE_1.Size = New System.Drawing.Size(105, 17)
        Me._optEXE_1.TabIndex = 12
        Me._optEXE_1.TabStop = True
        Me._optEXE_1.Text = "Non EXE Dist"
        Me._optEXE_1.UseVisualStyleBackColor = False
        '
        '_optEXE_0
        '
        Me._optEXE_0.BackColor = System.Drawing.SystemColors.Control
        Me._optEXE_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._optEXE_0.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optEXE_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optEXE.SetIndex(Me._optEXE_0, CType(0, Short))
        Me._optEXE_0.Location = New System.Drawing.Point(8, 10)
        Me._optEXE_0.Name = "_optEXE_0"
        Me._optEXE_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optEXE_0.Size = New System.Drawing.Size(105, 17)
        Me._optEXE_0.TabIndex = 11
        Me._optEXE_0.Text = "EXE Dist"
        Me._optEXE_0.UseVisualStyleBackColor = False
        '
        'fraDisplay
        '
        Me.fraDisplay.BackColor = System.Drawing.SystemColors.Control
        Me.fraDisplay.Controls.Add(Me._optPreOrder_2)
        Me.fraDisplay.Controls.Add(Me._optPreOrder_1)
        Me.fraDisplay.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraDisplay.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraDisplay.Location = New System.Drawing.Point(344, 8)
        Me.fraDisplay.Name = "fraDisplay"
        Me.fraDisplay.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraDisplay.Size = New System.Drawing.Size(121, 55)
        Me.fraDisplay.TabIndex = 7
        Me.fraDisplay.TabStop = False
        '
        '_optPreOrder_2
        '
        Me._optPreOrder_2.BackColor = System.Drawing.SystemColors.Control
        Me._optPreOrder_2.Checked = True
        Me._optPreOrder_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._optPreOrder_2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optPreOrder_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optPreOrder.SetIndex(Me._optPreOrder_2, CType(2, Short))
        Me._optPreOrder_2.Location = New System.Drawing.Point(8, 33)
        Me._optPreOrder_2.Name = "_optPreOrder_2"
        Me._optPreOrder_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optPreOrder_2.Size = New System.Drawing.Size(105, 17)
        Me._optPreOrder_2.TabIndex = 9
        Me._optPreOrder_2.TabStop = True
        Me._optPreOrder_2.Text = "Non Pre-Order"
        Me._optPreOrder_2.UseVisualStyleBackColor = False
        '
        '_optPreOrder_1
        '
        Me._optPreOrder_1.BackColor = System.Drawing.SystemColors.Control
        Me._optPreOrder_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._optPreOrder_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optPreOrder_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optPreOrder.SetIndex(Me._optPreOrder_1, CType(1, Short))
        Me._optPreOrder_1.Location = New System.Drawing.Point(8, 16)
        Me._optPreOrder_1.Name = "_optPreOrder_1"
        Me._optPreOrder_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optPreOrder_1.Size = New System.Drawing.Size(105, 17)
        Me._optPreOrder_1.TabIndex = 8
        Me._optPreOrder_1.Text = "Pre-Order"
        Me._optPreOrder_1.UseVisualStyleBackColor = False
        '
        'Frame2
        '
        Me.Frame2.BackColor = System.Drawing.SystemColors.Control
        Me.Frame2.Controls.Add(Me.RadioButton_SortByStatus)
        Me.Frame2.Controls.Add(Me._optOrder_4)
        Me.Frame2.Controls.Add(Me._optOrder_3)
        Me.Frame2.Controls.Add(Me._optOrder_0)
        Me.Frame2.Controls.Add(Me._optOrder_1)
        Me.Frame2.Controls.Add(Me._optOrder_2)
        Me.Frame2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame2.Location = New System.Drawing.Point(473, 8)
        Me.Frame2.Name = "Frame2"
        Me.Frame2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame2.Size = New System.Drawing.Size(130, 126)
        Me.Frame2.TabIndex = 13
        Me.Frame2.TabStop = False
        Me.Frame2.Text = "Sort By"
        '
        'RadioButton_SortByStatus
        '
        Me.RadioButton_SortByStatus.BackColor = System.Drawing.SystemColors.Control
        Me.RadioButton_SortByStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.RadioButton_SortByStatus.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioButton_SortByStatus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.RadioButton_SortByStatus.Location = New System.Drawing.Point(8, 100)
        Me.RadioButton_SortByStatus.Name = "RadioButton_SortByStatus"
        Me.RadioButton_SortByStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.RadioButton_SortByStatus.Size = New System.Drawing.Size(111, 20)
        Me.RadioButton_SortByStatus.TabIndex = 19
        Me.RadioButton_SortByStatus.TabStop = True
        Me.RadioButton_SortByStatus.Text = "Vendor Status"
        Me.RadioButton_SortByStatus.UseVisualStyleBackColor = False
        '
        '_optOrder_4
        '
        Me._optOrder_4.BackColor = System.Drawing.SystemColors.Control
        Me._optOrder_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._optOrder_4.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optOrder_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optOrder.SetIndex(Me._optOrder_4, CType(4, Short))
        Me._optOrder_4.Location = New System.Drawing.Point(8, 83)
        Me._optOrder_4.Name = "_optOrder_4"
        Me._optOrder_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optOrder_4.Size = New System.Drawing.Size(97, 17)
        Me._optOrder_4.TabIndex = 18
        Me._optOrder_4.TabStop = True
        Me._optOrder_4.Text = "Brand"
        Me._optOrder_4.UseVisualStyleBackColor = False
        '
        '_optOrder_3
        '
        Me._optOrder_3.BackColor = System.Drawing.SystemColors.Control
        Me._optOrder_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._optOrder_3.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optOrder_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optOrder.SetIndex(Me._optOrder_3, CType(3, Short))
        Me._optOrder_3.Location = New System.Drawing.Point(8, 67)
        Me._optOrder_3.Name = "_optOrder_3"
        Me._optOrder_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optOrder_3.Size = New System.Drawing.Size(111, 19)
        Me._optOrder_3.TabIndex = 17
        Me._optOrder_3.TabStop = True
        Me._optOrder_3.Text = "Vendor Item ID"
        Me._optOrder_3.UseVisualStyleBackColor = False
        '
        '_optOrder_0
        '
        Me._optOrder_0.BackColor = System.Drawing.SystemColors.Control
        Me._optOrder_0.Checked = True
        Me._optOrder_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._optOrder_0.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optOrder_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optOrder.SetIndex(Me._optOrder_0, CType(0, Short))
        Me._optOrder_0.Location = New System.Drawing.Point(8, 16)
        Me._optOrder_0.Name = "_optOrder_0"
        Me._optOrder_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optOrder_0.Size = New System.Drawing.Size(97, 17)
        Me._optOrder_0.TabIndex = 14
        Me._optOrder_0.TabStop = True
        Me._optOrder_0.Text = "Category"
        Me._optOrder_0.UseVisualStyleBackColor = False
        '
        '_optOrder_1
        '
        Me._optOrder_1.BackColor = System.Drawing.SystemColors.Control
        Me._optOrder_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._optOrder_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optOrder_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optOrder.SetIndex(Me._optOrder_1, CType(1, Short))
        Me._optOrder_1.Location = New System.Drawing.Point(8, 33)
        Me._optOrder_1.Name = "_optOrder_1"
        Me._optOrder_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optOrder_1.Size = New System.Drawing.Size(97, 17)
        Me._optOrder_1.TabIndex = 15
        Me._optOrder_1.TabStop = True
        Me._optOrder_1.Text = "Description"
        Me._optOrder_1.UseVisualStyleBackColor = False
        '
        '_optOrder_2
        '
        Me._optOrder_2.BackColor = System.Drawing.SystemColors.Control
        Me._optOrder_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._optOrder_2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optOrder_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optOrder.SetIndex(Me._optOrder_2, CType(2, Short))
        Me._optOrder_2.Location = New System.Drawing.Point(8, 50)
        Me._optOrder_2.Name = "_optOrder_2"
        Me._optOrder_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optOrder_2.Size = New System.Drawing.Size(97, 17)
        Me._optOrder_2.TabIndex = 16
        Me._optOrder_2.TabStop = True
        Me._optOrder_2.Text = "Identifier"
        Me._optOrder_2.UseVisualStyleBackColor = False
        '
        'chkOrdered
        '
        Me.chkOrdered.BackColor = System.Drawing.SystemColors.Control
        Me.chkOrdered.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkOrdered.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkOrdered.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkOrdered.Location = New System.Drawing.Point(9, 541)
        Me.chkOrdered.Name = "chkOrdered"
        Me.chkOrdered.Size = New System.Drawing.Size(139, 22)
        Me.chkOrdered.TabIndex = 21
        Me.chkOrdered.Text = "Show Ordered Only"
        Me.chkOrdered.UseVisualStyleBackColor = False
        '
        '_txtField_1
        '
        Me._txtField_1.AcceptsReturn = True
        Me._txtField_1.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtField_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtField_1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_1, CType(1, Short))
        Me._txtField_1.Location = New System.Drawing.Point(96, 8)
        Me._txtField_1.MaxLength = 13
        Me._txtField_1.Name = "_txtField_1"
        Me._txtField_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtField_1.Size = New System.Drawing.Size(225, 20)
        Me._txtField_1.TabIndex = 0
        Me._txtField_1.Tag = "String"
        '
        '_txtField_0
        '
        Me._txtField_0.AcceptsReturn = True
        Me._txtField_0.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_0.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtField_0.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtField_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_0, CType(0, Short))
        Me._txtField_0.Location = New System.Drawing.Point(96, 87)
        Me._txtField_0.MaxLength = 60
        Me._txtField_0.Name = "_txtField_0"
        Me._txtField_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtField_0.Size = New System.Drawing.Size(225, 20)
        Me._txtField_0.TabIndex = 3
        Me._txtField_0.Tag = "String"
        '
        '_lblLabel_5
        '
        Me._lblLabel_5.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_5.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_5, CType(5, Short))
        Me._lblLabel_5.Location = New System.Drawing.Point(17, 156)
        Me._lblLabel_5.Name = "_lblLabel_5"
        Me._lblLabel_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_5.Size = New System.Drawing.Size(73, 33)
        Me._lblLabel_5.TabIndex = 6
        Me._lblLabel_5.Text = "Distribution  Sub-Team :"
        Me._lblLabel_5.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblStatus
        '
        Me.lblStatus.BackColor = System.Drawing.Color.Transparent
        Me.lblStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblStatus.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblStatus.Location = New System.Drawing.Point(144, 530)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblStatus.Size = New System.Drawing.Size(607, 43)
        Me.lblStatus.TabIndex = 20
        Me.lblStatus.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblLabel_1
        '
        Me._lblLabel_1.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_1, CType(1, Short))
        Me._lblLabel_1.Location = New System.Drawing.Point(11, 10)
        Me._lblLabel_1.Name = "_lblLabel_1"
        Me._lblLabel_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_1.Size = New System.Drawing.Size(81, 17)
        Me._lblLabel_1.TabIndex = 0
        Me._lblLabel_1.Text = "Identifier :"
        Me._lblLabel_1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_2
        '
        Me._lblLabel_2.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_2, CType(2, Short))
        Me._lblLabel_2.Location = New System.Drawing.Point(19, 88)
        Me._lblLabel_2.Name = "_lblLabel_2"
        Me._lblLabel_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_2.Size = New System.Drawing.Size(73, 17)
        Me._lblLabel_2.TabIndex = 3
        Me._lblLabel_2.Text = "Desc :"
        Me._lblLabel_2.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'optEXE
        '
        '
        'optOrder
        '
        '
        'optPreOrder
        '
        '
        'txtField
        '
        '
        'TextBoxVIN
        '
        Me.TextBoxVIN.AcceptsReturn = True
        Me.TextBoxVIN.BackColor = System.Drawing.SystemColors.Window
        Me.TextBoxVIN.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextBoxVIN.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBoxVIN.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me.TextBoxVIN, CType(3, Short))
        Me.TextBoxVIN.Location = New System.Drawing.Point(96, 34)
        Me.TextBoxVIN.MaxLength = 13
        Me.TextBoxVIN.Name = "TextBoxVIN"
        Me.TextBoxVIN.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextBoxVIN.Size = New System.Drawing.Size(225, 20)
        Me.TextBoxVIN.TabIndex = 1
        Me.TextBoxVIN.Tag = "String"
        '
        'cmbCategory
        '
        Me.cmbCategory.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbCategory.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCategory.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Bold)
        Me.cmbCategory.FormattingEnabled = True
        Me.cmbCategory.Location = New System.Drawing.Point(96, 137)
        Me.cmbCategory.Name = "cmbCategory"
        Me.cmbCategory.Size = New System.Drawing.Size(225, 22)
        Me.cmbCategory.Sorted = True
        Me.cmbCategory.TabIndex = 5
        '
        'cmbSubTeam
        '
        Me.cmbSubTeam.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbSubTeam.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbSubTeam.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbSubTeam.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Bold)
        Me.cmbSubTeam.FormattingEnabled = True
        Me.cmbSubTeam.Location = New System.Drawing.Point(96, 112)
        Me.cmbSubTeam.Name = "cmbSubTeam"
        Me.cmbSubTeam.Size = New System.Drawing.Size(225, 22)
        Me.cmbSubTeam.Sorted = True
        Me.cmbSubTeam.TabIndex = 4
        '
        'lblCategory
        '
        Me.lblCategory.AutoSize = True
        Me.lblCategory.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.lblCategory.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblCategory.Location = New System.Drawing.Point(45, 137)
        Me.lblCategory.Name = "lblCategory"
        Me.lblCategory.Size = New System.Drawing.Size(44, 14)
        Me.lblCategory.TabIndex = 5
        Me.lblCategory.Text = "Class :"
        '
        'lblSubTeam
        '
        Me.lblSubTeam.AutoSize = True
        Me.lblSubTeam.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.lblSubTeam.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblSubTeam.Location = New System.Drawing.Point(23, 114)
        Me.lblSubTeam.Name = "lblSubTeam"
        Me.lblSubTeam.Size = New System.Drawing.Size(68, 14)
        Me.lblSubTeam.TabIndex = 4
        Me.lblSubTeam.Text = "Sub-Team :"
        '
        'ugrdOrderList
        '
        Me.ugrdOrderList.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        UltraGridColumn1.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn1.Header.VisiblePosition = 1
        UltraGridColumn1.MinWidth = 1
        UltraGridColumn1.TabStop = False
        UltraGridColumn1.Width = 109
        UltraGridColumn2.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn2.Header.VisiblePosition = 5
        UltraGridColumn2.MinWidth = 1
        UltraGridColumn2.TabStop = False
        UltraGridColumn2.Width = 60
        UltraGridColumn3.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn3.Header.Caption = "Description"
        UltraGridColumn3.Header.VisiblePosition = 3
        UltraGridColumn3.MinWidth = 1
        UltraGridColumn3.TabStop = False
        UltraGridColumn3.Width = 148
        Appearance13.TextHAlignAsString = "Right"
        UltraGridColumn4.CellAppearance = Appearance13
        Appearance14.TextHAlignAsString = "Right"
        UltraGridColumn4.Header.Appearance = Appearance14
        UltraGridColumn4.Header.Caption = "Ordered"
        UltraGridColumn4.Header.VisiblePosition = 6
        UltraGridColumn4.MinWidth = 1
        UltraGridColumn4.Width = 64
        UltraGridColumn5.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn5.Header.Caption = "Unit"
        UltraGridColumn5.Header.VisiblePosition = 7
        UltraGridColumn5.MinWidth = 1
        UltraGridColumn5.TabStop = False
        UltraGridColumn5.Width = 51
        UltraGridColumn6.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn6.Header.Caption = "Vendor Pack"
        UltraGridColumn6.Header.VisiblePosition = 9
        UltraGridColumn6.MinWidth = 1
        UltraGridColumn6.TabStop = False
        UltraGridColumn6.Width = 80
        UltraGridColumn7.Header.Caption = "Credit Reason"
        UltraGridColumn7.Header.VisiblePosition = 10
        UltraGridColumn7.MinWidth = 1
        UltraGridColumn7.Width = 111
        UltraGridColumn8.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn8.Header.VisiblePosition = 11
        UltraGridColumn8.Hidden = True
        UltraGridColumn8.TabStop = False
        UltraGridColumn9.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn9.Header.VisiblePosition = 12
        UltraGridColumn9.Hidden = True
        UltraGridColumn9.TabStop = False
        UltraGridColumn10.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn10.Header.VisiblePosition = 13
        UltraGridColumn10.Hidden = True
        UltraGridColumn10.TabStop = False
        UltraGridColumn11.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn11.Header.VisiblePosition = 14
        UltraGridColumn11.Hidden = True
        UltraGridColumn11.TabStop = False
        UltraGridColumn12.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn12.Header.VisiblePosition = 15
        UltraGridColumn12.Hidden = True
        UltraGridColumn12.TabStop = False
        UltraGridColumn13.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn13.Header.VisiblePosition = 16
        UltraGridColumn13.Hidden = True
        UltraGridColumn13.TabStop = False
        UltraGridColumn14.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn14.Header.VisiblePosition = 17
        UltraGridColumn14.Hidden = True
        UltraGridColumn14.TabStop = False
        UltraGridColumn15.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn15.Header.VisiblePosition = 18
        UltraGridColumn15.Hidden = True
        UltraGridColumn15.TabStop = False
        UltraGridColumn16.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn16.Header.VisiblePosition = 19
        UltraGridColumn16.Hidden = True
        UltraGridColumn16.TabStop = False
        UltraGridColumn17.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn17.Header.VisiblePosition = 0
        UltraGridColumn17.MinWidth = 1
        UltraGridColumn17.Width = 52
        UltraGridColumn18.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn18.Header.VisiblePosition = 2
        UltraGridColumn18.MinWidth = 1
        UltraGridColumn18.Width = 72
        UltraGridColumn19.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn19.Header.VisiblePosition = 8
        UltraGridColumn19.MinWidth = 1
        UltraGridColumn19.Width = 64
        UltraGridColumn20.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn20.DataType = GetType(Integer)
        UltraGridColumn20.Header.VisiblePosition = 20
        UltraGridColumn20.Hidden = True
        UltraGridColumn20.Width = 56
        UltraGridColumn21.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn21.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn21.Header.Caption = "Status"
        UltraGridColumn21.Header.VisiblePosition = 4
        UltraGridColumn21.MinWidth = 1
        UltraGridColumn21.Width = 55
        UltraGridColumn22.Header.VisiblePosition = 21
        UltraGridColumn22.Hidden = True
        UltraGridColumn22.Width = 102
        UltraGridBand1.Columns.AddRange(New Object() {UltraGridColumn1, UltraGridColumn2, UltraGridColumn3, UltraGridColumn4, UltraGridColumn5, UltraGridColumn6, UltraGridColumn7, UltraGridColumn8, UltraGridColumn9, UltraGridColumn10, UltraGridColumn11, UltraGridColumn12, UltraGridColumn13, UltraGridColumn14, UltraGridColumn15, UltraGridColumn16, UltraGridColumn17, UltraGridColumn18, UltraGridColumn19, UltraGridColumn20, UltraGridColumn21, UltraGridColumn22})
        UltraGridBand1.ExcludeFromColumnChooser = Infragistics.Win.UltraWinGrid.ExcludeFromColumnChooser.[True]
        UltraGridBand1.Expandable = False
        UltraGridBand1.GroupHeadersVisible = False
        UltraGridBand1.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[True]
        Me.ugrdOrderList.DisplayLayout.BandsSerializer.Add(UltraGridBand1)
        Me.ugrdOrderList.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.ugrdOrderList.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Me.ugrdOrderList.DisplayLayout.ColScrollRegions.Add(ColScrollRegion1)
        Me.ugrdOrderList.DisplayLayout.ColScrollRegions.Add(ColScrollRegion2)
        Me.ugrdOrderList.DisplayLayout.ColScrollRegions.Add(ColScrollRegion3)
        Me.ugrdOrderList.DisplayLayout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.[False]
        Appearance3.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance3.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance3.BorderColor = System.Drawing.SystemColors.Window
        Me.ugrdOrderList.DisplayLayout.GroupByBox.Appearance = Appearance3
        Appearance4.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugrdOrderList.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance4
        Me.ugrdOrderList.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.ugrdOrderList.DisplayLayout.GroupByBox.Hidden = True
        Appearance5.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance5.BackColor2 = System.Drawing.SystemColors.Control
        Appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance5.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugrdOrderList.DisplayLayout.GroupByBox.PromptAppearance = Appearance5
        Me.ugrdOrderList.DisplayLayout.MaxBandDepth = 1
        Me.ugrdOrderList.DisplayLayout.MaxColScrollRegions = 1
        Me.ugrdOrderList.DisplayLayout.MaxRowScrollRegions = 1
        Appearance6.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugrdOrderList.DisplayLayout.Override.ActiveRowAppearance = Appearance6
        Me.ugrdOrderList.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No
        Me.ugrdOrderList.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.NotAllowed
        Me.ugrdOrderList.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed
        Me.ugrdOrderList.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.[False]
        Me.ugrdOrderList.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        Me.ugrdOrderList.DisplayLayout.Override.AllowMultiCellOperations = Infragistics.Win.UltraWinGrid.AllowMultiCellOperation.None
        Me.ugrdOrderList.DisplayLayout.Override.AllowRowSummaries = Infragistics.Win.UltraWinGrid.AllowRowSummaries.[False]
        Me.ugrdOrderList.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.[True]
        Me.ugrdOrderList.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.ugrdOrderList.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance7.BackColor = System.Drawing.SystemColors.Window
        Me.ugrdOrderList.DisplayLayout.Override.CardAreaAppearance = Appearance7
        Appearance8.BorderColor = System.Drawing.Color.Silver
        Appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.ugrdOrderList.DisplayLayout.Override.CellAppearance = Appearance8
        Me.ugrdOrderList.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.ugrdOrderList.DisplayLayout.Override.CellPadding = 0
        Appearance9.BackColor = System.Drawing.SystemColors.Control
        Appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance9.BorderColor = System.Drawing.SystemColors.Window
        Me.ugrdOrderList.DisplayLayout.Override.GroupByRowAppearance = Appearance9
        Appearance10.TextHAlignAsString = "Left"
        Me.ugrdOrderList.DisplayLayout.Override.HeaderAppearance = Appearance10
        Me.ugrdOrderList.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti
        Me.ugrdOrderList.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Me.ugrdOrderList.DisplayLayout.Override.MaxSelectedCells = 1
        Appearance11.BackColor = System.Drawing.SystemColors.ControlLight
        Me.ugrdOrderList.DisplayLayout.Override.RowAlternateAppearance = Appearance11
        Me.ugrdOrderList.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[True]
        Me.ugrdOrderList.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Extended
        Appearance12.BackColor = System.Drawing.SystemColors.ControlLight
        Me.ugrdOrderList.DisplayLayout.Override.TemplateAddRowAppearance = Appearance12
        Me.ugrdOrderList.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.ugrdOrderList.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        ValueList1.Key = "CreditReasons"
        ValueList2.Key = "QuantityUnits"
        Me.ugrdOrderList.DisplayLayout.ValueLists.AddRange(New Infragistics.Win.ValueList() {ValueList1, ValueList2})
        Me.ugrdOrderList.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand
        Me.ugrdOrderList.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.Horizontal
        Me.ugrdOrderList.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ugrdOrderList.Location = New System.Drawing.Point(9, 205)
        Me.ugrdOrderList.Name = "ugrdOrderList"
        Me.ugrdOrderList.Size = New System.Drawing.Size(887, 319)
        Me.ugrdOrderList.TabIndex = 20
        Me.ugrdOrderList.Text = "Order - List View"
        Me.ugrdOrderList.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnCellChangeOrLostFocus
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(19, 27)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(72, 36)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Vendor Item ID:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(16, 63)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(73, 17)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Brand :"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'cmbBrand
        '
        Me.cmbBrand.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbBrand.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbBrand.BackColor = System.Drawing.SystemColors.Window
        Me.cmbBrand.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbBrand.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbBrand.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.cmbBrand.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbBrand.Location = New System.Drawing.Point(97, 60)
        Me.cmbBrand.Name = "cmbBrand"
        Me.cmbBrand.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbBrand.Size = New System.Drawing.Size(224, 22)
        Me.cmbBrand.Sorted = True
        Me.cmbBrand.TabIndex = 2
        '
        'chkIncludeNotAvailable
        '
        Me.chkIncludeNotAvailable.BackColor = System.Drawing.SystemColors.Control
        Me.chkIncludeNotAvailable.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkIncludeNotAvailable.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIncludeNotAvailable.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIncludeNotAvailable.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIncludeNotAvailable.Location = New System.Drawing.Point(450, 157)
        Me.chkIncludeNotAvailable.Name = "chkIncludeNotAvailable"
        Me.chkIncludeNotAvailable.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIncludeNotAvailable.Size = New System.Drawing.Size(153, 17)
        Me.chkIncludeNotAvailable.TabIndex = 24
        Me.chkIncludeNotAvailable.Text = "Include Not Available :"
        Me.chkIncludeNotAvailable.UseVisualStyleBackColor = False
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.chkVendorItemStatus_Seasonal)
        Me.GroupBox1.Controls.Add(Me.chkVendorItemStatus_VendorDiscontinued)
        Me.GroupBox1.Controls.Add(Me.chkVendorItemStatus_MfgDiscontinued)
        Me.GroupBox1.Controls.Add(Me.chkVendorItemStatus_NotAvailable)
        Me.GroupBox1.Location = New System.Drawing.Point(609, 8)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(142, 111)
        Me.GroupBox1.TabIndex = 57
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Vendor Status"
        '
        'chkVendorItemStatus_Seasonal
        '
        Me.chkVendorItemStatus_Seasonal.AutoSize = True
        Me.chkVendorItemStatus_Seasonal.Location = New System.Drawing.Point(6, 91)
        Me.chkVendorItemStatus_Seasonal.Name = "chkVendorItemStatus_Seasonal"
        Me.chkVendorItemStatus_Seasonal.Size = New System.Drawing.Size(108, 18)
        Me.chkVendorItemStatus_Seasonal.TabIndex = 3
        Me.chkVendorItemStatus_Seasonal.Text = "Include Seasonal"
        Me.chkVendorItemStatus_Seasonal.UseVisualStyleBackColor = True
        '
        'chkVendorItemStatus_VendorDiscontinued
        '
        Me.chkVendorItemStatus_VendorDiscontinued.AutoSize = True
        Me.chkVendorItemStatus_VendorDiscontinued.Location = New System.Drawing.Point(6, 67)
        Me.chkVendorItemStatus_VendorDiscontinued.Name = "chkVendorItemStatus_VendorDiscontinued"
        Me.chkVendorItemStatus_VendorDiscontinued.Size = New System.Drawing.Size(128, 18)
        Me.chkVendorItemStatus_VendorDiscontinued.TabIndex = 2
        Me.chkVendorItemStatus_VendorDiscontinued.Text = "Include Vendor Disco"
        Me.chkVendorItemStatus_VendorDiscontinued.UseVisualStyleBackColor = True
        '
        'chkVendorItemStatus_MfgDiscontinued
        '
        Me.chkVendorItemStatus_MfgDiscontinued.AutoSize = True
        Me.chkVendorItemStatus_MfgDiscontinued.Location = New System.Drawing.Point(6, 43)
        Me.chkVendorItemStatus_MfgDiscontinued.Name = "chkVendorItemStatus_MfgDiscontinued"
        Me.chkVendorItemStatus_MfgDiscontinued.Size = New System.Drawing.Size(111, 18)
        Me.chkVendorItemStatus_MfgDiscontinued.TabIndex = 1
        Me.chkVendorItemStatus_MfgDiscontinued.Text = "Include Mfg Disco"
        Me.chkVendorItemStatus_MfgDiscontinued.UseVisualStyleBackColor = True
        '
        'chkVendorItemStatus_NotAvailable
        '
        Me.chkVendorItemStatus_NotAvailable.AutoSize = True
        Me.chkVendorItemStatus_NotAvailable.Location = New System.Drawing.Point(6, 19)
        Me.chkVendorItemStatus_NotAvailable.Name = "chkVendorItemStatus_NotAvailable"
        Me.chkVendorItemStatus_NotAvailable.Size = New System.Drawing.Size(125, 18)
        Me.chkVendorItemStatus_NotAvailable.TabIndex = 0
        Me.chkVendorItemStatus_NotAvailable.Text = "Include Not Available"
        Me.chkVendorItemStatus_NotAvailable.UseVisualStyleBackColor = True
        '
        'btnApplyAllCreditReason
        '
        Me.btnApplyAllCreditReason.Image = CType(resources.GetObject("btnApplyAllCreditReason.Image"), System.Drawing.Image)
        Me.btnApplyAllCreditReason.Location = New System.Drawing.Point(711, 531)
        Me.btnApplyAllCreditReason.Name = "btnApplyAllCreditReason"
        Me.btnApplyAllCreditReason.Size = New System.Drawing.Size(44, 39)
        Me.btnApplyAllCreditReason.TabIndex = 59
        Me.ToolTip1.SetToolTip(Me.btnApplyAllCreditReason, "Apply first credit reason to all line items")
        Me.btnApplyAllCreditReason.UseVisualStyleBackColor = True
        '
        'frmOrderList
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdExit
        Me.ClientSize = New System.Drawing.Size(908, 582)
        Me.ControlBox = False
        Me.Controls.Add(Me.btnApplyAllCreditReason)
        Me.Controls.Add(Me.btnConversionCalculator)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.fraEXE)
        Me.Controls.Add(Me.chkIncludeNotAvailable)
        Me.Controls.Add(Me.cmbBrand)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.TextBoxVIN)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ugrdOrderList)
        Me.Controls.Add(Me.cmbCategory)
        Me.Controls.Add(Me.cmbSubTeam)
        Me.Controls.Add(Me.lblCategory)
        Me.Controls.Add(Me.lblSubTeam)
        Me.Controls.Add(Me.chkDiscontinued)
        Me.Controls.Add(Me.cmbDistSubTeam)
        Me.Controls.Add(Me.fraDisplay)
        Me.Controls.Add(Me.Frame2)
        Me.Controls.Add(Me.chkOrdered)
        Me.Controls.Add(Me.cmdSubmit)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me._txtField_1)
        Me.Controls.Add(Me._txtField_0)
        Me.Controls.Add(Me._lblLabel_5)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me._lblLabel_1)
        Me.Controls.Add(Me._lblLabel_2)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(12, 29)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmOrderList"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Order - List View"
        Me.fraEXE.ResumeLayout(False)
        Me.fraDisplay.ResumeLayout(False)
        Me.Frame2.ResumeLayout(False)
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.optEXE, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.optOrder, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.optPreOrder, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtField, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ugrdOrderList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
	Friend WithEvents cmbCategory As System.Windows.Forms.ComboBox
	Friend WithEvents cmbSubTeam As System.Windows.Forms.ComboBox
	Friend WithEvents lblCategory As System.Windows.Forms.Label
	Friend WithEvents lblSubTeam As System.Windows.Forms.Label
	Friend WithEvents ugrdOrderList As Infragistics.Win.UltraWinGrid.UltraGrid
	Public WithEvents TextBoxVIN As System.Windows.Forms.TextBox
	Public WithEvents Label1 As System.Windows.Forms.Label
	Public WithEvents Label2 As System.Windows.Forms.Label
	Public WithEvents _optOrder_4 As System.Windows.Forms.RadioButton
	Public WithEvents _optOrder_3 As System.Windows.Forms.RadioButton
	Public WithEvents cmbBrand As System.Windows.Forms.ComboBox
	Public WithEvents chkIncludeNotAvailable As System.Windows.Forms.CheckBox
	Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
	Friend WithEvents chkVendorItemStatus_Seasonal As System.Windows.Forms.CheckBox
	Friend WithEvents chkVendorItemStatus_VendorDiscontinued As System.Windows.Forms.CheckBox
	Friend WithEvents chkVendorItemStatus_MfgDiscontinued As System.Windows.Forms.CheckBox
	Friend WithEvents chkVendorItemStatus_NotAvailable As System.Windows.Forms.CheckBox
    Public WithEvents RadioButton_SortByStatus As System.Windows.Forms.RadioButton
    Friend WithEvents btnConversionCalculator As System.Windows.Forms.Button
    Friend WithEvents btnApplyAllCreditReason As System.Windows.Forms.Button
#End Region
End Class
