<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmVendorCost
#Region "Windows Form Designer generated code "
	<System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
		MyBase.New()
        'This call is required by the Windows Form Designer.
        Me.IsInitializing = True
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
	Public WithEvents chkEDLP As System.Windows.Forms.CheckBox
	Public WithEvents chkCurrentCost As System.Windows.Forms.CheckBox
	Public WithEvents cmbState As System.Windows.Forms.ComboBox
	Public WithEvents _optSelection_2 As System.Windows.Forms.RadioButton
	Public WithEvents cmdSearch As System.Windows.Forms.Button
    Public WithEvents _optSelection_0 As System.Windows.Forms.RadioButton
	Public WithEvents cmbStore As System.Windows.Forms.ComboBox
	Public WithEvents _optSelection_5 As System.Windows.Forms.RadioButton
	Public WithEvents _optSelection_1 As System.Windows.Forms.RadioButton
	Public WithEvents cmbZones As System.Windows.Forms.ComboBox
	Public WithEvents _optSelection_3 As System.Windows.Forms.RadioButton
	Public WithEvents _optSelection_4 As System.Windows.Forms.RadioButton
	Public WithEvents lblDates As System.Windows.Forms.Label
    Public WithEvents fraStoreSelection As System.Windows.Forms.GroupBox
	Public WithEvents cmdAdd As System.Windows.Forms.Button
	Public WithEvents cmdDelete As System.Windows.Forms.Button
    Public WithEvents cmdExit As System.Windows.Forms.Button
	Public WithEvents txtItemID As System.Windows.Forms.TextBox
	Public WithEvents txtIdentifier As System.Windows.Forms.TextBox
	Public WithEvents txtItem_Description As System.Windows.Forms.TextBox
    Public WithEvents lblEDLP As System.Windows.Forms.Label
    Public WithEvents lblVendorItemID As System.Windows.Forms.Label
    Public WithEvents lblIdentifier As System.Windows.Forms.Label
    Public WithEvents lblItemDesc As System.Windows.Forms.Label
    Public WithEvents optSelection As Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray
    Public WithEvents txtDate As Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmVendorCost))
        Dim Appearance23 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridBand1 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1)
        Dim UltraGridColumn1 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Store_Name")
        Dim Appearance1 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridColumn2 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("FromVendor")
        Dim UltraGridColumn3 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Type")
        Dim UltraGridColumn4 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("UnitCost")
        Dim Appearance2 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridColumn5 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("UnitFreight")
        Dim Appearance3 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridColumn6 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("StartDate")
        Dim Appearance4 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridColumn7 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("EndDate")
        Dim Appearance5 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridColumn8 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("PrimaryVendor", -1, Nothing, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Descending, False)
        Dim Appearance6 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridColumn9 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("VendorCostHistoryID")
        Dim UltraGridColumn10 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Store_No")
        Dim UltraGridColumn11 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("NetDiscount")
        Dim Appearance7 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridColumn12 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("NetCost")
        Dim Appearance8 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridColumn13 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("VendorPack")
        Dim Appearance9 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridColumn14 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Package_Desc1")
        Dim Appearance10 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridColumn15 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("CostUnit_Name")
        Dim Appearance11 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridColumn16 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("FreightUnit_Name")
        Dim UltraGridColumn17 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Currency")
        Dim UltraGridColumn18 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Ignore_Pack_Updates")
        Dim UltraGridColumn19 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Insert_Date")
        Dim Appearance49 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance50 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance51 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance52 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance53 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance54 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance55 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance56 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance57 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance58 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance59 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance60 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance61 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraDataColumn1 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Store_Name")
        Dim UltraDataColumn2 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("FromVendor")
        Dim UltraDataColumn3 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Type")
        Dim UltraDataColumn4 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("UnitCost")
        Dim UltraDataColumn5 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("UnitFreight")
        Dim UltraDataColumn6 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("StartDate")
        Dim UltraDataColumn7 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("EndDate")
        Dim UltraDataColumn8 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("PrimaryVendor")
        Dim UltraDataColumn9 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("VendorCostHistoryID")
        Dim UltraDataColumn10 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Store_No")
        Dim UltraDataColumn11 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("NetDiscount")
        Dim UltraDataColumn12 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("NetCost")
        Dim UltraDataColumn13 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("VendorPack")
        Dim UltraDataColumn14 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Package_Desc1")
        Dim UltraDataColumn15 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("CostUnit_Name")
        Dim UltraDataColumn16 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("FreightUnit_Name")
        Dim UltraDataColumn17 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Currency")
        Dim UltraDataColumn18 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Ignore_Pack_Updates")
        Dim UltraDataColumn19 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Insert_Date")
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdSearch = New System.Windows.Forms.Button()
        Me.cmdAdd = New System.Windows.Forms.Button()
        Me.cmdDelete = New System.Windows.Forms.Button()
        Me.cmdExit = New System.Windows.Forms.Button()
        Me.cmdNetDiscountDetails = New System.Windows.Forms.Button()
        Me.Button_AddCostPromo = New System.Windows.Forms.Button()
        Me.Button_MarginInfo = New System.Windows.Forms.Button()
        Me.chkEDLP = New System.Windows.Forms.CheckBox()
        Me.fraStoreSelection = New System.Windows.Forms.GroupBox()
        Me.dtpEndDate = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor()
        Me.dtpStartDate = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor()
        Me.lblDash = New System.Windows.Forms.Label()
        Me.chkCurrentCost = New System.Windows.Forms.CheckBox()
        Me.cmbState = New System.Windows.Forms.ComboBox()
        Me._optSelection_2 = New System.Windows.Forms.RadioButton()
        Me._optSelection_0 = New System.Windows.Forms.RadioButton()
        Me.cmbStore = New System.Windows.Forms.ComboBox()
        Me._optSelection_5 = New System.Windows.Forms.RadioButton()
        Me._optSelection_1 = New System.Windows.Forms.RadioButton()
        Me.cmbZones = New System.Windows.Forms.ComboBox()
        Me._optSelection_3 = New System.Windows.Forms.RadioButton()
        Me._optSelection_4 = New System.Windows.Forms.RadioButton()
        Me.lblDates = New System.Windows.Forms.Label()
        Me.txtItemID = New System.Windows.Forms.TextBox()
        Me.txtIdentifier = New System.Windows.Forms.TextBox()
        Me.txtItem_Description = New System.Windows.Forms.TextBox()
        Me.lblEDLP = New System.Windows.Forms.Label()
        Me.lblVendorItemID = New System.Windows.Forms.Label()
        Me.lblIdentifier = New System.Windows.Forms.Label()
        Me.lblItemDesc = New System.Windows.Forms.Label()
        Me.optSelection = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(Me.components)
        Me.txtDate = New Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray(Me.components)
        Me.ugrdCostHistory = New Infragistics.Win.UltraWinGrid.UltraGrid()
        Me.UltraDataSource1 = New Infragistics.Win.UltraWinDataSource.UltraDataSource(Me.components)
        Me.cmbVendItemStats = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnConversionCalculator = New System.Windows.Forms.Button()
        Me.fraStoreSelection.SuspendLayout()
        CType(Me.dtpEndDate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dtpStartDate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.optSelection, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtDate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ugrdCostHistory, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.UltraDataSource1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdSearch
        '
        Me.cmdSearch.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSearch.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdSearch, "cmdSearch")
        Me.cmdSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSearch.Name = "cmdSearch"
        Me.ToolTip1.SetToolTip(Me.cmdSearch, resources.GetString("cmdSearch.ToolTip"))
        Me.cmdSearch.UseVisualStyleBackColor = False
        '
        'cmdAdd
        '
        Me.cmdAdd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAdd.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdAdd, "cmdAdd")
        Me.cmdAdd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAdd.Name = "cmdAdd"
        Me.ToolTip1.SetToolTip(Me.cmdAdd, resources.GetString("cmdAdd.ToolTip"))
        Me.cmdAdd.UseVisualStyleBackColor = False
        '
        'cmdDelete
        '
        Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDelete.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdDelete, "cmdDelete")
        Me.cmdDelete.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDelete.Name = "cmdDelete"
        Me.ToolTip1.SetToolTip(Me.cmdDelete, resources.GetString("cmdDelete.ToolTip"))
        Me.cmdDelete.UseVisualStyleBackColor = False
        '
        'cmdExit
        '
        resources.ApplyResources(Me.cmdExit, "cmdExit")
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Name = "cmdExit"
        Me.ToolTip1.SetToolTip(Me.cmdExit, resources.GetString("cmdExit.ToolTip"))
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'cmdNetDiscountDetails
        '
        Me.cmdNetDiscountDetails.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNetDiscountDetails.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdNetDiscountDetails, "cmdNetDiscountDetails")
        Me.cmdNetDiscountDetails.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNetDiscountDetails.Name = "cmdNetDiscountDetails"
        Me.ToolTip1.SetToolTip(Me.cmdNetDiscountDetails, resources.GetString("cmdNetDiscountDetails.ToolTip"))
        Me.cmdNetDiscountDetails.UseVisualStyleBackColor = False
        '
        'Button_AddCostPromo
        '
        Me.Button_AddCostPromo.BackColor = System.Drawing.SystemColors.Control
        Me.Button_AddCostPromo.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.Button_AddCostPromo, "Button_AddCostPromo")
        Me.Button_AddCostPromo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button_AddCostPromo.Name = "Button_AddCostPromo"
        Me.ToolTip1.SetToolTip(Me.Button_AddCostPromo, resources.GetString("Button_AddCostPromo.ToolTip"))
        Me.Button_AddCostPromo.UseVisualStyleBackColor = False
        '
        'Button_MarginInfo
        '
        Me.Button_MarginInfo.BackColor = System.Drawing.SystemColors.Control
        Me.Button_MarginInfo.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.Button_MarginInfo, "Button_MarginInfo")
        Me.Button_MarginInfo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button_MarginInfo.Name = "Button_MarginInfo"
        Me.ToolTip1.SetToolTip(Me.Button_MarginInfo, resources.GetString("Button_MarginInfo.ToolTip"))
        Me.Button_MarginInfo.UseVisualStyleBackColor = False
        '
        'chkEDLP
        '
        Me.chkEDLP.BackColor = System.Drawing.SystemColors.Control
        resources.ApplyResources(Me.chkEDLP, "chkEDLP")
        Me.chkEDLP.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkEDLP.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkEDLP.Name = "chkEDLP"
        Me.chkEDLP.TabStop = False
        Me.chkEDLP.UseVisualStyleBackColor = False
        '
        'fraStoreSelection
        '
        Me.fraStoreSelection.BackColor = System.Drawing.SystemColors.Control
        Me.fraStoreSelection.Controls.Add(Me.dtpEndDate)
        Me.fraStoreSelection.Controls.Add(Me.dtpStartDate)
        Me.fraStoreSelection.Controls.Add(Me.lblDash)
        Me.fraStoreSelection.Controls.Add(Me.chkCurrentCost)
        Me.fraStoreSelection.Controls.Add(Me.cmbState)
        Me.fraStoreSelection.Controls.Add(Me._optSelection_2)
        Me.fraStoreSelection.Controls.Add(Me.cmdSearch)
        Me.fraStoreSelection.Controls.Add(Me._optSelection_0)
        Me.fraStoreSelection.Controls.Add(Me.cmbStore)
        Me.fraStoreSelection.Controls.Add(Me._optSelection_5)
        Me.fraStoreSelection.Controls.Add(Me._optSelection_1)
        Me.fraStoreSelection.Controls.Add(Me.cmbZones)
        Me.fraStoreSelection.Controls.Add(Me._optSelection_3)
        Me.fraStoreSelection.Controls.Add(Me._optSelection_4)
        Me.fraStoreSelection.Controls.Add(Me.lblDates)
        resources.ApplyResources(Me.fraStoreSelection, "fraStoreSelection")
        Me.fraStoreSelection.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraStoreSelection.Name = "fraStoreSelection"
        Me.fraStoreSelection.TabStop = False
        '
        'dtpEndDate
        '
        Me.dtpEndDate.DateTime = New Date(2011, 1, 20, 0, 0, 0, 0)
        resources.ApplyResources(Me.dtpEndDate, "dtpEndDate")
        Me.dtpEndDate.Name = "dtpEndDate"
        Me.dtpEndDate.Value = New Date(2011, 1, 20, 0, 0, 0, 0)
        '
        'dtpStartDate
        '
        Me.dtpStartDate.DateTime = New Date(2011, 1, 20, 0, 0, 0, 0)
        resources.ApplyResources(Me.dtpStartDate, "dtpStartDate")
        Me.dtpStartDate.Name = "dtpStartDate"
        Me.dtpStartDate.Value = New Date(2011, 1, 20, 0, 0, 0, 0)
        '
        'lblDash
        '
        Me.lblDash.BackColor = System.Drawing.Color.Transparent
        Me.lblDash.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblDash, "lblDash")
        Me.lblDash.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDash.Name = "lblDash"
        '
        'chkCurrentCost
        '
        Me.chkCurrentCost.BackColor = System.Drawing.SystemColors.Control
        resources.ApplyResources(Me.chkCurrentCost, "chkCurrentCost")
        Me.chkCurrentCost.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkCurrentCost.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkCurrentCost.Name = "chkCurrentCost"
        Me.chkCurrentCost.UseVisualStyleBackColor = False
        '
        'cmbState
        '
        Me.cmbState.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbState.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbState.BackColor = System.Drawing.SystemColors.Window
        Me.cmbState.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbState.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.cmbState, "cmbState")
        Me.cmbState.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbState.Name = "cmbState"
        Me.cmbState.Sorted = True
        '
        '_optSelection_2
        '
        Me._optSelection_2.BackColor = System.Drawing.SystemColors.Control
        Me._optSelection_2.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._optSelection_2, "_optSelection_2")
        Me._optSelection_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optSelection.SetIndex(Me._optSelection_2, CType(2, Short))
        Me._optSelection_2.Name = "_optSelection_2"
        Me._optSelection_2.TabStop = True
        Me._optSelection_2.UseVisualStyleBackColor = False
        '
        '_optSelection_0
        '
        Me._optSelection_0.BackColor = System.Drawing.SystemColors.Control
        Me._optSelection_0.Checked = True
        Me._optSelection_0.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._optSelection_0, "_optSelection_0")
        Me._optSelection_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optSelection.SetIndex(Me._optSelection_0, CType(0, Short))
        Me._optSelection_0.Name = "_optSelection_0"
        Me._optSelection_0.TabStop = True
        Me._optSelection_0.UseVisualStyleBackColor = False
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
        '_optSelection_5
        '
        Me._optSelection_5.BackColor = System.Drawing.SystemColors.Control
        Me._optSelection_5.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._optSelection_5, "_optSelection_5")
        Me._optSelection_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optSelection.SetIndex(Me._optSelection_5, CType(5, Short))
        Me._optSelection_5.Name = "_optSelection_5"
        Me._optSelection_5.TabStop = True
        Me._optSelection_5.UseVisualStyleBackColor = False
        '
        '_optSelection_1
        '
        Me._optSelection_1.BackColor = System.Drawing.SystemColors.Control
        Me._optSelection_1.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._optSelection_1, "_optSelection_1")
        Me._optSelection_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optSelection.SetIndex(Me._optSelection_1, CType(1, Short))
        Me._optSelection_1.Name = "_optSelection_1"
        Me._optSelection_1.TabStop = True
        Me._optSelection_1.UseVisualStyleBackColor = False
        '
        'cmbZones
        '
        Me.cmbZones.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbZones.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbZones.BackColor = System.Drawing.SystemColors.Window
        Me.cmbZones.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbZones.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.cmbZones, "cmbZones")
        Me.cmbZones.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbZones.Name = "cmbZones"
        Me.cmbZones.Sorted = True
        '
        '_optSelection_3
        '
        Me._optSelection_3.BackColor = System.Drawing.SystemColors.Control
        Me._optSelection_3.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._optSelection_3, "_optSelection_3")
        Me._optSelection_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optSelection.SetIndex(Me._optSelection_3, CType(3, Short))
        Me._optSelection_3.Name = "_optSelection_3"
        Me._optSelection_3.TabStop = True
        Me._optSelection_3.UseVisualStyleBackColor = False
        '
        '_optSelection_4
        '
        Me._optSelection_4.BackColor = System.Drawing.SystemColors.Control
        Me._optSelection_4.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._optSelection_4, "_optSelection_4")
        Me._optSelection_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optSelection.SetIndex(Me._optSelection_4, CType(4, Short))
        Me._optSelection_4.Name = "_optSelection_4"
        Me._optSelection_4.TabStop = True
        Me._optSelection_4.UseVisualStyleBackColor = False
        '
        'lblDates
        '
        Me.lblDates.BackColor = System.Drawing.Color.Transparent
        Me.lblDates.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblDates, "lblDates")
        Me.lblDates.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDates.Name = "lblDates"
        '
        'txtItemID
        '
        Me.txtItemID.AcceptsReturn = True
        Me.txtItemID.BackColor = System.Drawing.SystemColors.Window
        Me.txtItemID.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.txtItemID, "txtItemID")
        Me.txtItemID.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtItemID.Name = "txtItemID"
        Me.txtItemID.Tag = "String"
        '
        'txtIdentifier
        '
        Me.txtIdentifier.AcceptsReturn = True
        Me.txtIdentifier.BackColor = System.Drawing.SystemColors.ControlLight
        Me.txtIdentifier.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.txtIdentifier, "txtIdentifier")
        Me.txtIdentifier.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtIdentifier.Name = "txtIdentifier"
        Me.txtIdentifier.ReadOnly = True
        Me.txtIdentifier.TabStop = False
        Me.txtIdentifier.Tag = "Integer"
        '
        'txtItem_Description
        '
        Me.txtItem_Description.AcceptsReturn = True
        Me.txtItem_Description.BackColor = System.Drawing.SystemColors.ControlLight
        Me.txtItem_Description.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.txtItem_Description, "txtItem_Description")
        Me.txtItem_Description.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtItem_Description.Name = "txtItem_Description"
        Me.txtItem_Description.ReadOnly = True
        Me.txtItem_Description.TabStop = False
        Me.txtItem_Description.Tag = "String"
        '
        'lblEDLP
        '
        Me.lblEDLP.BackColor = System.Drawing.Color.Transparent
        Me.lblEDLP.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblEDLP, "lblEDLP")
        Me.lblEDLP.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEDLP.Name = "lblEDLP"
        '
        'lblVendorItemID
        '
        Me.lblVendorItemID.BackColor = System.Drawing.Color.Transparent
        Me.lblVendorItemID.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblVendorItemID, "lblVendorItemID")
        Me.lblVendorItemID.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblVendorItemID.Name = "lblVendorItemID"
        '
        'lblIdentifier
        '
        Me.lblIdentifier.BackColor = System.Drawing.Color.Transparent
        Me.lblIdentifier.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblIdentifier, "lblIdentifier")
        Me.lblIdentifier.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblIdentifier.Name = "lblIdentifier"
        '
        'lblItemDesc
        '
        Me.lblItemDesc.BackColor = System.Drawing.Color.Transparent
        Me.lblItemDesc.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblItemDesc, "lblItemDesc")
        Me.lblItemDesc.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblItemDesc.Name = "lblItemDesc"
        '
        'optSelection
        '
        '
        'ugrdCostHistory
        '
        Me.ugrdCostHistory.DataSource = Me.UltraDataSource1
        Appearance23.BackColor = System.Drawing.SystemColors.ControlLight
        Appearance23.BorderColor = System.Drawing.SystemColors.InactiveCaption
        resources.ApplyResources(Appearance23.FontData, "Appearance23.FontData")
        resources.ApplyResources(Appearance23, "Appearance23")
        Appearance23.ForceApplyResources = "FontData|"
        Me.ugrdCostHistory.DisplayLayout.Appearance = Appearance23
        Me.ugrdCostHistory.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn
        UltraGridColumn1.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn1.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn1.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn1.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn1.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        resources.ApplyResources(Appearance1, "Appearance1")
        Appearance1.ForceApplyResources = ""
        UltraGridColumn1.CellAppearance = Appearance1
        UltraGridColumn1.Header.Caption = resources.GetString("resource.Caption")
        UltraGridColumn1.Header.VisiblePosition = 0
        UltraGridColumn1.RowLayoutColumnInfo.OriginX = 0
        UltraGridColumn1.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn1.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(135, 0)
        UltraGridColumn1.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn1.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn2.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn2.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn2.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn2.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn2.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn2.Header.Caption = resources.GetString("resource.Caption1")
        UltraGridColumn2.Header.VisiblePosition = 1
        UltraGridColumn2.RowLayoutColumnInfo.OriginX = 2
        UltraGridColumn2.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn2.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(82, 0)
        UltraGridColumn2.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn2.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn2.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox
        UltraGridColumn3.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn3.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn3.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn3.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn3.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn3.Header.VisiblePosition = 2
        UltraGridColumn3.Hidden = True
        UltraGridColumn3.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(50, 0)
        UltraGridColumn4.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn4.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn4.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn4.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn4.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        resources.ApplyResources(Appearance2, "Appearance2")
        Appearance2.ForceApplyResources = ""
        UltraGridColumn4.CellAppearance = Appearance2
        UltraGridColumn4.Header.Caption = resources.GetString("resource.Caption2")
        UltraGridColumn4.Header.VisiblePosition = 3
        UltraGridColumn4.RowLayoutColumnInfo.OriginX = 4
        UltraGridColumn4.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn4.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(71, 0)
        UltraGridColumn4.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn4.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn5.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn5.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn5.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn5.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn5.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        resources.ApplyResources(Appearance3, "Appearance3")
        Appearance3.ForceApplyResources = ""
        UltraGridColumn5.CellAppearance = Appearance3
        UltraGridColumn5.Header.Caption = resources.GetString("resource.Caption3")
        UltraGridColumn5.Header.VisiblePosition = 5
        UltraGridColumn5.RowLayoutColumnInfo.OriginX = 8
        UltraGridColumn5.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn5.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(71, 0)
        UltraGridColumn5.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn5.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn6.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn6.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn6.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn6.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn6.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        resources.ApplyResources(Appearance4, "Appearance4")
        Appearance4.ForceApplyResources = ""
        UltraGridColumn6.CellAppearance = Appearance4
        UltraGridColumn6.Header.Caption = resources.GetString("resource.Caption4")
        UltraGridColumn6.Header.VisiblePosition = 10
        UltraGridColumn6.RowLayoutColumnInfo.OriginX = 24
        UltraGridColumn6.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn6.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(75, 0)
        UltraGridColumn6.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn6.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn7.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn7.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn7.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn7.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn7.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        resources.ApplyResources(Appearance5, "Appearance5")
        Appearance5.ForceApplyResources = ""
        UltraGridColumn7.CellAppearance = Appearance5
        UltraGridColumn7.Header.Caption = resources.GetString("resource.Caption5")
        UltraGridColumn7.Header.VisiblePosition = 11
        UltraGridColumn7.RowLayoutColumnInfo.OriginX = 26
        UltraGridColumn7.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn7.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(75, 0)
        UltraGridColumn7.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn7.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn8.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn8.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn8.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn8.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn8.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        resources.ApplyResources(Appearance6, "Appearance6")
        Appearance6.ForceApplyResources = ""
        UltraGridColumn8.CellAppearance = Appearance6
        UltraGridColumn8.Header.Caption = resources.GetString("resource.Caption6")
        UltraGridColumn8.Header.VisiblePosition = 12
        UltraGridColumn8.RowLayoutColumnInfo.OriginX = 14
        UltraGridColumn8.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn8.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(75, 0)
        UltraGridColumn8.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn8.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn9.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn9.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn9.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn9.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn9.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn9.Header.Caption = resources.GetString("resource.Caption7")
        UltraGridColumn9.Header.VisiblePosition = 13
        UltraGridColumn9.Hidden = True
        UltraGridColumn9.RowLayoutColumnInfo.OriginX = 30
        UltraGridColumn9.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn9.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(53, 0)
        UltraGridColumn9.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn9.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn9.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox
        UltraGridColumn10.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn10.Header.VisiblePosition = 14
        UltraGridColumn10.Hidden = True
        UltraGridColumn11.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        resources.ApplyResources(Appearance7, "Appearance7")
        Appearance7.ForceApplyResources = ""
        UltraGridColumn11.CellAppearance = Appearance7
        UltraGridColumn11.Header.Caption = resources.GetString("resource.Caption8")
        UltraGridColumn11.Header.VisiblePosition = 4
        UltraGridColumn11.RowLayoutColumnInfo.OriginX = 6
        UltraGridColumn11.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn11.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(84, 0)
        UltraGridColumn11.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn11.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn12.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        resources.ApplyResources(Appearance8, "Appearance8")
        Appearance8.ForceApplyResources = ""
        UltraGridColumn12.CellAppearance = Appearance8
        UltraGridColumn12.Header.Caption = resources.GetString("resource.Caption9")
        UltraGridColumn12.Header.VisiblePosition = 6
        UltraGridColumn12.RowLayoutColumnInfo.OriginX = 10
        UltraGridColumn12.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn12.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(71, 0)
        UltraGridColumn12.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn12.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn13.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        resources.ApplyResources(Appearance9, "Appearance9")
        Appearance9.ForceApplyResources = ""
        UltraGridColumn13.CellAppearance = Appearance9
        UltraGridColumn13.Header.Caption = resources.GetString("resource.Caption10")
        UltraGridColumn13.Header.VisiblePosition = 18
        UltraGridColumn13.RowLayoutColumnInfo.OriginX = 20
        UltraGridColumn13.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn13.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn13.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn14.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        resources.ApplyResources(Appearance10, "Appearance10")
        Appearance10.ForceApplyResources = ""
        UltraGridColumn14.CellAppearance = Appearance10
        UltraGridColumn14.Header.Caption = resources.GetString("resource.Caption11")
        UltraGridColumn14.Header.VisiblePosition = 8
        UltraGridColumn14.RowLayoutColumnInfo.OriginX = 18
        UltraGridColumn14.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn14.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(99, 0)
        UltraGridColumn14.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn14.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn15.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        resources.ApplyResources(Appearance11, "Appearance11")
        Appearance11.ForceApplyResources = ""
        UltraGridColumn15.CellAppearance = Appearance11
        UltraGridColumn15.Header.Caption = resources.GetString("resource.Caption12")
        UltraGridColumn15.Header.VisiblePosition = 9
        UltraGridColumn15.RowLayoutColumnInfo.OriginX = 22
        UltraGridColumn15.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn15.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(85, 0)
        UltraGridColumn15.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn15.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn16.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn16.Header.Caption = resources.GetString("resource.Caption13")
        UltraGridColumn16.Header.VisiblePosition = 7
        UltraGridColumn16.Hidden = True
        UltraGridColumn16.RowLayoutColumnInfo.OriginX = 14
        UltraGridColumn16.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn16.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(70, 0)
        UltraGridColumn16.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn16.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn17.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn17.Header.VisiblePosition = 15
        UltraGridColumn17.RowLayoutColumnInfo.OriginX = 12
        UltraGridColumn17.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn17.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(73, 0)
        UltraGridColumn17.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn17.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn18.CellActivation = Infragistics.Win.UltraWinGrid.Activation.Disabled
        UltraGridColumn18.Header.Caption = resources.GetString("resource.Caption14")
        UltraGridColumn18.Header.ToolTipText = "Ignore Vendor Pack"
        UltraGridColumn18.Header.VisiblePosition = 16
        UltraGridColumn18.RowLayoutColumnInfo.OriginX = 16
        UltraGridColumn18.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn18.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn18.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn19.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        resources.ApplyResources(UltraGridColumn19, "UltraGridColumn19")
        UltraGridColumn19.Header.Caption = resources.GetString("resource.Caption15")
        UltraGridColumn19.Header.VisiblePosition = 17
        UltraGridColumn19.RowLayoutColumnInfo.OriginX = 28
        UltraGridColumn19.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn19.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn19.RowLayoutColumnInfo.SpanY = 2
        UltraGridBand1.Columns.AddRange(New Object() {UltraGridColumn1, UltraGridColumn2, UltraGridColumn3, UltraGridColumn4, UltraGridColumn5, UltraGridColumn6, UltraGridColumn7, UltraGridColumn8, UltraGridColumn9, UltraGridColumn10, UltraGridColumn11, UltraGridColumn12, UltraGridColumn13, UltraGridColumn14, UltraGridColumn15, UltraGridColumn16, UltraGridColumn17, UltraGridColumn18, UltraGridColumn19})
        UltraGridBand1.RowLayoutStyle = Infragistics.Win.UltraWinGrid.RowLayoutStyle.ColumnLayout
        Me.ugrdCostHistory.DisplayLayout.BandsSerializer.Add(UltraGridBand1)
        Me.ugrdCostHistory.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        resources.ApplyResources(Appearance49.FontData, "Appearance49.FontData")
        resources.ApplyResources(Appearance49, "Appearance49")
        Appearance49.ForceApplyResources = "FontData|"
        Me.ugrdCostHistory.DisplayLayout.CaptionAppearance = Appearance49
        Me.ugrdCostHistory.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[True]
        Appearance50.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance50.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance50.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance50.BorderColor = System.Drawing.SystemColors.Window
        resources.ApplyResources(Appearance50.FontData, "Appearance50.FontData")
        resources.ApplyResources(Appearance50, "Appearance50")
        Appearance50.ForceApplyResources = "FontData|"
        Me.ugrdCostHistory.DisplayLayout.GroupByBox.Appearance = Appearance50
        Appearance51.ForeColor = System.Drawing.SystemColors.GrayText
        resources.ApplyResources(Appearance51.FontData, "Appearance51.FontData")
        resources.ApplyResources(Appearance51, "Appearance51")
        Appearance51.ForceApplyResources = "FontData|"
        Me.ugrdCostHistory.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance51
        Me.ugrdCostHistory.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.ugrdCostHistory.DisplayLayout.GroupByBox.Hidden = True
        Appearance52.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance52.BackColor2 = System.Drawing.SystemColors.Control
        Appearance52.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance52.ForeColor = System.Drawing.SystemColors.GrayText
        resources.ApplyResources(Appearance52.FontData, "Appearance52.FontData")
        resources.ApplyResources(Appearance52, "Appearance52")
        Appearance52.ForceApplyResources = "FontData|"
        Me.ugrdCostHistory.DisplayLayout.GroupByBox.PromptAppearance = Appearance52
        Me.ugrdCostHistory.DisplayLayout.MaxColScrollRegions = 1
        Me.ugrdCostHistory.DisplayLayout.MaxRowScrollRegions = 1
        Appearance53.BackColor = System.Drawing.SystemColors.Window
        Appearance53.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Appearance53.FontData, "Appearance53.FontData")
        resources.ApplyResources(Appearance53, "Appearance53")
        Appearance53.ForceApplyResources = "FontData|"
        Me.ugrdCostHistory.DisplayLayout.Override.ActiveCellAppearance = Appearance53
        Me.ugrdCostHistory.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.ugrdCostHistory.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance54.BackColor = System.Drawing.SystemColors.Window
        resources.ApplyResources(Appearance54.FontData, "Appearance54.FontData")
        resources.ApplyResources(Appearance54, "Appearance54")
        Appearance54.ForceApplyResources = "FontData|"
        Me.ugrdCostHistory.DisplayLayout.Override.CardAreaAppearance = Appearance54
        Appearance55.BorderColor = System.Drawing.Color.Silver
        resources.ApplyResources(Appearance55.FontData, "Appearance55.FontData")
        Appearance55.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        resources.ApplyResources(Appearance55, "Appearance55")
        Appearance55.ForceApplyResources = "FontData|"
        Me.ugrdCostHistory.DisplayLayout.Override.CellAppearance = Appearance55
        Me.ugrdCostHistory.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.ugrdCostHistory.DisplayLayout.Override.CellPadding = 0
        resources.ApplyResources(Appearance56.FontData, "Appearance56.FontData")
        resources.ApplyResources(Appearance56, "Appearance56")
        Appearance56.ForceApplyResources = "FontData|"
        Me.ugrdCostHistory.DisplayLayout.Override.FixedHeaderAppearance = Appearance56
        Appearance57.BackColor = System.Drawing.SystemColors.Control
        Appearance57.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance57.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance57.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance57.BorderColor = System.Drawing.SystemColors.Window
        resources.ApplyResources(Appearance57.FontData, "Appearance57.FontData")
        resources.ApplyResources(Appearance57, "Appearance57")
        Appearance57.ForceApplyResources = "FontData|"
        Me.ugrdCostHistory.DisplayLayout.Override.GroupByRowAppearance = Appearance57
        resources.ApplyResources(Appearance58.FontData, "Appearance58.FontData")
        resources.ApplyResources(Appearance58, "Appearance58")
        Appearance58.ForceApplyResources = "FontData|"
        Me.ugrdCostHistory.DisplayLayout.Override.HeaderAppearance = Appearance58
        Me.ugrdCostHistory.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle
        Me.ugrdCostHistory.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance59.BackColor = System.Drawing.SystemColors.ControlLight
        resources.ApplyResources(Appearance59.FontData, "Appearance59.FontData")
        resources.ApplyResources(Appearance59, "Appearance59")
        Appearance59.ForceApplyResources = "FontData|"
        Me.ugrdCostHistory.DisplayLayout.Override.RowAlternateAppearance = Appearance59
        Appearance60.BackColor = System.Drawing.SystemColors.Window
        Appearance60.BorderColor = System.Drawing.Color.Silver
        resources.ApplyResources(Appearance60.FontData, "Appearance60.FontData")
        resources.ApplyResources(Appearance60, "Appearance60")
        Appearance60.ForceApplyResources = "FontData|"
        Me.ugrdCostHistory.DisplayLayout.Override.RowAppearance = Appearance60
        Me.ugrdCostHistory.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[True]
        Me.ugrdCostHistory.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed
        Me.ugrdCostHistory.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.ugrdCostHistory.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.ugrdCostHistory.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Extended
        Appearance61.BackColor = System.Drawing.SystemColors.ControlLight
        resources.ApplyResources(Appearance61.FontData, "Appearance61.FontData")
        resources.ApplyResources(Appearance61, "Appearance61")
        Appearance61.ForceApplyResources = "FontData|"
        Me.ugrdCostHistory.DisplayLayout.Override.TemplateAddRowAppearance = Appearance61
        Me.ugrdCostHistory.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.ugrdCostHistory.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.ugrdCostHistory.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand
        Me.ugrdCostHistory.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        resources.ApplyResources(Me.ugrdCostHistory, "ugrdCostHistory")
        Me.ugrdCostHistory.Name = "ugrdCostHistory"
        '
        'UltraDataSource1
        '
        UltraDataColumn6.DataType = GetType(Date)
        UltraDataColumn7.DataType = GetType(Date)
        UltraDataColumn17.ReadOnly = Infragistics.Win.DefaultableBoolean.[True]
        UltraDataColumn18.AllowDBNull = Infragistics.Win.DefaultableBoolean.[False]
        UltraDataColumn18.DataType = GetType(Boolean)
        UltraDataColumn18.ReadOnly = Infragistics.Win.DefaultableBoolean.[True]
        UltraDataColumn19.DataType = GetType(Date)
        UltraDataColumn19.ReadOnly = Infragistics.Win.DefaultableBoolean.[True]
        Me.UltraDataSource1.Band.Columns.AddRange(New Object() {UltraDataColumn1, UltraDataColumn2, UltraDataColumn3, UltraDataColumn4, UltraDataColumn5, UltraDataColumn6, UltraDataColumn7, UltraDataColumn8, UltraDataColumn9, UltraDataColumn10, UltraDataColumn11, UltraDataColumn12, UltraDataColumn13, UltraDataColumn14, UltraDataColumn15, UltraDataColumn16, UltraDataColumn17, UltraDataColumn18, UltraDataColumn19})
        '
        'cmbVendItemStats
        '
        Me.cmbVendItemStats.FormattingEnabled = True
        resources.ApplyResources(Me.cmbVendItemStats, "cmbVendItemStats")
        Me.cmbVendItemStats.Name = "cmbVendItemStats"
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Name = "Label1"
        '
        'btnConversionCalculator
        '
        resources.ApplyResources(Me.btnConversionCalculator, "btnConversionCalculator")
        Me.btnConversionCalculator.Name = "btnConversionCalculator"
        Me.btnConversionCalculator.UseVisualStyleBackColor = True
        '
        'frmVendorCost
        '
        Me.AcceptButton = Me.cmdSearch
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.Controls.Add(Me.btnConversionCalculator)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cmbVendItemStats)
        Me.Controls.Add(Me.Button_MarginInfo)
        Me.Controls.Add(Me.Button_AddCostPromo)
        Me.Controls.Add(Me.cmdNetDiscountDetails)
        Me.Controls.Add(Me.ugrdCostHistory)
        Me.Controls.Add(Me.chkEDLP)
        Me.Controls.Add(Me.fraStoreSelection)
        Me.Controls.Add(Me.cmdAdd)
        Me.Controls.Add(Me.cmdDelete)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.txtItemID)
        Me.Controls.Add(Me.txtIdentifier)
        Me.Controls.Add(Me.txtItem_Description)
        Me.Controls.Add(Me.lblEDLP)
        Me.Controls.Add(Me.lblVendorItemID)
        Me.Controls.Add(Me.lblIdentifier)
        Me.Controls.Add(Me.lblItemDesc)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmVendorCost"
        Me.ShowInTaskbar = False
        Me.fraStoreSelection.ResumeLayout(False)
        Me.fraStoreSelection.PerformLayout()
        CType(Me.dtpEndDate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dtpStartDate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.optSelection, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtDate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ugrdCostHistory, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.UltraDataSource1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ugrdCostHistory As Infragistics.Win.UltraWinGrid.UltraGrid
    Public WithEvents lblDash As System.Windows.Forms.Label
    Friend WithEvents dtpEndDate As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
    Friend WithEvents dtpStartDate As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
    Friend WithEvents UltraDataSource1 As Infragistics.Win.UltraWinDataSource.UltraDataSource
    Public WithEvents cmdNetDiscountDetails As System.Windows.Forms.Button
    Public WithEvents Button_AddCostPromo As System.Windows.Forms.Button
    Public WithEvents Button_MarginInfo As System.Windows.Forms.Button
    Friend WithEvents cmbVendItemStats As System.Windows.Forms.ComboBox
    Public WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnConversionCalculator As System.Windows.Forms.Button
#End Region
End Class
