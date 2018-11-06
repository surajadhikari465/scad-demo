<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmVendorCostDetail
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
	Public WithEvents _optSelection_0 As System.Windows.Forms.RadioButton
	Public WithEvents _optSelection_1 As System.Windows.Forms.RadioButton
	Public WithEvents _optSelection_2 As System.Windows.Forms.RadioButton
	Public WithEvents cmbZones As System.Windows.Forms.ComboBox
	Public WithEvents _optSelection_3 As System.Windows.Forms.RadioButton
    Public WithEvents _optSelection_4 As System.Windows.Forms.RadioButton
    Public WithEvents _optSelection_5 As System.Windows.Forms.RadioButton
	Public WithEvents cmbStates As System.Windows.Forms.ComboBox
    Public WithEvents fraStoreSel As System.Windows.Forms.GroupBox
	Public WithEvents _optType_1 As System.Windows.Forms.RadioButton
	Public WithEvents _optType_0 As System.Windows.Forms.RadioButton
	Public WithEvents fraType As System.Windows.Forms.GroupBox
    Public WithEvents cmdSelect As System.Windows.Forms.Button
	Public WithEvents cmdExit As System.Windows.Forms.Button
    Public WithEvents optSelection As Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray
	Public WithEvents optType As Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray
	Public WithEvents txtDate As Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmVendorCostDetail))
        Dim Appearance29 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridBand1 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1)
        Dim UltraGridColumn1 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Store_No")
        Dim UltraGridColumn2 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Store_Name")
        Dim Appearance30 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridColumn3 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Zone_ID")
        Dim UltraGridColumn4 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("State")
        Dim UltraGridColumn5 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("WFM_Store")
        Dim UltraGridColumn6 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Mega_Store")
        Dim UltraGridColumn7 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("CustomerType")
        Dim UltraGridColumn8 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("StoreJurisdictionID", 0)
        Dim Appearance31 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance32 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance33 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
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
        Dim UltraGridBand2 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1)
        Dim UltraGridColumn9 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Item_Key")
        Dim UltraGridColumn10 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("StoreJurisdictionDesc", -1, Nothing, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Descending, False)
        Dim UltraGridColumn11 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("StoreJurisdictionID")
        Dim UltraGridColumn12 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Package_Desc1")
        Dim UltraGridColumn13 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Package_Desc2")
        Dim UltraGridColumn14 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Slash")
        Dim UltraGridColumn15 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Unit_Name")
        Dim Appearance45 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance46 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance47 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance48 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance49 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance50 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance51 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance52 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance53 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance54 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance55 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance56 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me._optSelection_0 = New System.Windows.Forms.RadioButton()
        Me._optSelection_1 = New System.Windows.Forms.RadioButton()
        Me._optSelection_2 = New System.Windows.Forms.RadioButton()
        Me._optSelection_3 = New System.Windows.Forms.RadioButton()
        Me._optSelection_4 = New System.Windows.Forms.RadioButton()
        Me._optSelection_5 = New System.Windows.Forms.RadioButton()
        Me.cmdSelect = New System.Windows.Forms.Button()
        Me.cmdExit = New System.Windows.Forms.Button()
        Me.Button_MarginInfo = New System.Windows.Forms.Button()
        Me.fraStoreSel = New System.Windows.Forms.GroupBox()
        Me.cmbJurisdiction = New System.Windows.Forms.ComboBox()
        Me.ugrdStoreList = New Infragistics.Win.UltraWinGrid.UltraGrid()
        Me.cmbZones = New System.Windows.Forms.ComboBox()
        Me.cmbStates = New System.Windows.Forms.ComboBox()
        Me.fraType = New System.Windows.Forms.GroupBox()
        Me._optType_1 = New System.Windows.Forms.RadioButton()
        Me._optType_0 = New System.Windows.Forms.RadioButton()
        Me.optSelection = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(Me.components)
        Me.optType = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(Me.components)
        Me.txtDate = New Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray(Me.components)
        Me.txtPkgDesc1 = New System.Windows.Forms.TextBox()
        Me.txtMSRP = New System.Windows.Forms.TextBox()
        Me.txtCost = New System.Windows.Forms.TextBox()
        Me.txtFreight = New System.Windows.Forms.TextBox()
        Me.lblVendorPack = New System.Windows.Forms.Label()
        Me._lblLabel_1 = New System.Windows.Forms.Label()
        Me._lblLabel_0 = New System.Windows.Forms.Label()
        Me.lblPkgDesc = New System.Windows.Forms.Label()
        Me.lblCost = New System.Windows.Forms.Label()
        Me.lblStart = New System.Windows.Forms.Label()
        Me.lblMSRP = New System.Windows.Forms.Label()
        Me.lblEnd = New System.Windows.Forms.Label()
        Me.lblFreight = New System.Windows.Forms.Label()
        Me.dtpStartDate = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor()
        Me.dtpEndDate = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor()
        Me.ComboBox_FreightUnit = New System.Windows.Forms.ComboBox()
        Me.ComboBox_CostUnit = New System.Windows.Forms.ComboBox()
        Me.CheckBox_CostedByWeight = New System.Windows.Forms.CheckBox()
        Me.ugrdRetailPackList = New Infragistics.Win.UltraWinGrid.UltraGrid()
        Me._labelVendorPackUOM = New System.Windows.Forms.Label()
        Me.CheckBox_CatchweightRequired = New System.Windows.Forms.CheckBox()
        Me.chkIgnorePackUpdates = New System.Windows.Forms.CheckBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmbCurrency = New System.Windows.Forms.ComboBox()
        Me.TextBox_RetailCasePack = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lblCurrency = New System.Windows.Forms.Label()
        Me.btnConversionCalculator = New System.Windows.Forms.Button()
        Me.fraStoreSel.SuspendLayout()
        CType(Me.ugrdStoreList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.fraType.SuspendLayout()
        CType(Me.optSelection, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.optType, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtDate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dtpStartDate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dtpEndDate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ugrdRetailPackList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
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
        Me.ToolTip1.SetToolTip(Me._optSelection_0, resources.GetString("_optSelection_0.ToolTip"))
        Me._optSelection_0.UseVisualStyleBackColor = False
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
        Me.ToolTip1.SetToolTip(Me._optSelection_1, resources.GetString("_optSelection_1.ToolTip"))
        Me._optSelection_1.UseVisualStyleBackColor = False
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
        Me.ToolTip1.SetToolTip(Me._optSelection_2, resources.GetString("_optSelection_2.ToolTip"))
        Me._optSelection_2.UseVisualStyleBackColor = False
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
        Me.ToolTip1.SetToolTip(Me._optSelection_3, resources.GetString("_optSelection_3.ToolTip"))
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
        Me.ToolTip1.SetToolTip(Me._optSelection_4, resources.GetString("_optSelection_4.ToolTip"))
        Me._optSelection_4.UseVisualStyleBackColor = False
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
        Me.ToolTip1.SetToolTip(Me._optSelection_5, resources.GetString("_optSelection_5.ToolTip"))
        Me._optSelection_5.UseVisualStyleBackColor = False
        '
        'cmdSelect
        '
        Me.cmdSelect.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSelect.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdSelect, "cmdSelect")
        Me.cmdSelect.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSelect.Name = "cmdSelect"
        Me.ToolTip1.SetToolTip(Me.cmdSelect, resources.GetString("cmdSelect.ToolTip"))
        Me.cmdSelect.UseVisualStyleBackColor = False
        '
        'cmdExit
        '
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdExit, "cmdExit")
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Name = "cmdExit"
        Me.ToolTip1.SetToolTip(Me.cmdExit, resources.GetString("cmdExit.ToolTip"))
        Me.cmdExit.UseVisualStyleBackColor = False
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
        'fraStoreSel
        '
        Me.fraStoreSel.BackColor = System.Drawing.SystemColors.Control
        Me.fraStoreSel.Controls.Add(Me.cmbJurisdiction)
        Me.fraStoreSel.Controls.Add(Me.ugrdStoreList)
        Me.fraStoreSel.Controls.Add(Me._optSelection_0)
        Me.fraStoreSel.Controls.Add(Me._optSelection_1)
        Me.fraStoreSel.Controls.Add(Me._optSelection_2)
        Me.fraStoreSel.Controls.Add(Me.cmbZones)
        Me.fraStoreSel.Controls.Add(Me._optSelection_3)
        Me.fraStoreSel.Controls.Add(Me._optSelection_4)
        Me.fraStoreSel.Controls.Add(Me._optSelection_5)
        Me.fraStoreSel.Controls.Add(Me.cmbStates)
        resources.ApplyResources(Me.fraStoreSel, "fraStoreSel")
        Me.fraStoreSel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraStoreSel.Name = "fraStoreSel"
        Me.fraStoreSel.TabStop = False
        '
        'cmbJurisdiction
        '
        Me.cmbJurisdiction.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbJurisdiction.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbJurisdiction.BackColor = System.Drawing.SystemColors.Window
        Me.cmbJurisdiction.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbJurisdiction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.cmbJurisdiction, "cmbJurisdiction")
        Me.cmbJurisdiction.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbJurisdiction.Name = "cmbJurisdiction"
        Me.cmbJurisdiction.Sorted = True
        '
        'ugrdStoreList
        '
        Appearance29.BackColor = System.Drawing.SystemColors.ControlLight
        Appearance29.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.ugrdStoreList.DisplayLayout.Appearance = Appearance29
        Me.ugrdStoreList.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        UltraGridColumn1.Header.VisiblePosition = 0
        UltraGridColumn1.Hidden = True
        UltraGridColumn2.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn2.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        resources.ApplyResources(Appearance30, "Appearance30")
        Appearance30.ForceApplyResources = ""
        UltraGridColumn2.Header.Appearance = Appearance30
        UltraGridColumn2.Header.Caption = resources.GetString("resource.Caption")
        UltraGridColumn2.Header.VisiblePosition = 1
        UltraGridColumn2.SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled
        UltraGridColumn3.Header.VisiblePosition = 2
        UltraGridColumn3.Hidden = True
        UltraGridColumn4.Header.VisiblePosition = 3
        UltraGridColumn4.Hidden = True
        UltraGridColumn5.Header.VisiblePosition = 4
        UltraGridColumn5.Hidden = True
        UltraGridColumn6.Header.VisiblePosition = 5
        UltraGridColumn6.Hidden = True
        UltraGridColumn7.Header.VisiblePosition = 6
        UltraGridColumn7.Hidden = True
        UltraGridColumn8.Header.VisiblePosition = 7
        UltraGridColumn8.Hidden = True
        UltraGridBand1.Columns.AddRange(New Object() {UltraGridColumn1, UltraGridColumn2, UltraGridColumn3, UltraGridColumn4, UltraGridColumn5, UltraGridColumn6, UltraGridColumn7, UltraGridColumn8})
        UltraGridBand1.RowLayoutStyle = Infragistics.Win.UltraWinGrid.RowLayoutStyle.ColumnLayout
        Me.ugrdStoreList.DisplayLayout.BandsSerializer.Add(UltraGridBand1)
        Me.ugrdStoreList.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Appearance31.FontData.BoldAsString = resources.GetString("resource.BoldAsString")
        Me.ugrdStoreList.DisplayLayout.CaptionAppearance = Appearance31
        Me.ugrdStoreList.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Appearance32.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance32.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance32.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance32.BorderColor = System.Drawing.SystemColors.Window
        Me.ugrdStoreList.DisplayLayout.GroupByBox.Appearance = Appearance32
        Appearance33.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugrdStoreList.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance33
        Me.ugrdStoreList.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.ugrdStoreList.DisplayLayout.GroupByBox.Hidden = True
        Appearance34.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance34.BackColor2 = System.Drawing.SystemColors.Control
        Appearance34.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance34.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugrdStoreList.DisplayLayout.GroupByBox.PromptAppearance = Appearance34
        Me.ugrdStoreList.DisplayLayout.MaxColScrollRegions = 1
        Me.ugrdStoreList.DisplayLayout.MaxRowScrollRegions = 1
        Appearance35.BackColor = System.Drawing.SystemColors.Window
        Appearance35.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ugrdStoreList.DisplayLayout.Override.ActiveCellAppearance = Appearance35
        Me.ugrdStoreList.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.ugrdStoreList.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance36.BackColor = System.Drawing.SystemColors.Window
        Me.ugrdStoreList.DisplayLayout.Override.CardAreaAppearance = Appearance36
        Appearance37.BorderColor = System.Drawing.Color.Silver
        Appearance37.FontData.BoldAsString = resources.GetString("resource.BoldAsString1")
        Appearance37.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.ugrdStoreList.DisplayLayout.Override.CellAppearance = Appearance37
        Me.ugrdStoreList.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.ugrdStoreList.DisplayLayout.Override.CellPadding = 0
        Appearance38.FontData.BoldAsString = resources.GetString("resource.BoldAsString2")
        Me.ugrdStoreList.DisplayLayout.Override.FixedHeaderAppearance = Appearance38
        Appearance39.BackColor = System.Drawing.SystemColors.Control
        Appearance39.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance39.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance39.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance39.BorderColor = System.Drawing.SystemColors.Window
        Me.ugrdStoreList.DisplayLayout.Override.GroupByRowAppearance = Appearance39
        Appearance40.FontData.BoldAsString = resources.GetString("resource.BoldAsString3")
        resources.ApplyResources(Appearance40, "Appearance40")
        Appearance40.ForceApplyResources = ""
        Me.ugrdStoreList.DisplayLayout.Override.HeaderAppearance = Appearance40
        Me.ugrdStoreList.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle
        Me.ugrdStoreList.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance41.BackColor = System.Drawing.SystemColors.ControlLight
        Me.ugrdStoreList.DisplayLayout.Override.RowAlternateAppearance = Appearance41
        Appearance42.BackColor = System.Drawing.SystemColors.Window
        Appearance42.BorderColor = System.Drawing.Color.Silver
        Me.ugrdStoreList.DisplayLayout.Override.RowAppearance = Appearance42
        Me.ugrdStoreList.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[True]
        Me.ugrdStoreList.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed
        Me.ugrdStoreList.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.ugrdStoreList.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.ugrdStoreList.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Extended
        Appearance43.BackColor = System.Drawing.SystemColors.ControlLight
        Me.ugrdStoreList.DisplayLayout.Override.TemplateAddRowAppearance = Appearance43
        Me.ugrdStoreList.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.ugrdStoreList.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.ugrdStoreList.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand
        Me.ugrdStoreList.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        resources.ApplyResources(Me.ugrdStoreList, "ugrdStoreList")
        Me.ugrdStoreList.Name = "ugrdStoreList"
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
        'cmbStates
        '
        Me.cmbStates.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbStates.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbStates.BackColor = System.Drawing.SystemColors.Window
        Me.cmbStates.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbStates.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.cmbStates, "cmbStates")
        Me.cmbStates.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbStates.Name = "cmbStates"
        Me.cmbStates.Sorted = True
        '
        'fraType
        '
        Me.fraType.BackColor = System.Drawing.SystemColors.Control
        Me.fraType.Controls.Add(Me._optType_1)
        Me.fraType.Controls.Add(Me._optType_0)
        resources.ApplyResources(Me.fraType, "fraType")
        Me.fraType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraType.Name = "fraType"
        Me.fraType.TabStop = False
        '
        '_optType_1
        '
        Me._optType_1.BackColor = System.Drawing.SystemColors.Control
        Me._optType_1.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._optType_1, "_optType_1")
        Me._optType_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optType.SetIndex(Me._optType_1, CType(1, Short))
        Me._optType_1.Name = "_optType_1"
        Me._optType_1.TabStop = True
        Me._optType_1.UseVisualStyleBackColor = False
        '
        '_optType_0
        '
        Me._optType_0.BackColor = System.Drawing.SystemColors.Control
        Me._optType_0.Checked = True
        Me._optType_0.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._optType_0, "_optType_0")
        Me._optType_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optType.SetIndex(Me._optType_0, CType(0, Short))
        Me._optType_0.Name = "_optType_0"
        Me._optType_0.TabStop = True
        Me._optType_0.UseVisualStyleBackColor = False
        '
        'optSelection
        '
        '
        'optType
        '
        '
        'txtPkgDesc1
        '
        Me.txtPkgDesc1.AcceptsReturn = True
        Me.txtPkgDesc1.BackColor = System.Drawing.SystemColors.Window
        Me.txtPkgDesc1.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.txtPkgDesc1, "txtPkgDesc1")
        Me.txtPkgDesc1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPkgDesc1.Name = "txtPkgDesc1"
        Me.txtPkgDesc1.Tag = "Currency"
        '
        'txtMSRP
        '
        Me.txtMSRP.AcceptsReturn = True
        Me.txtMSRP.BackColor = System.Drawing.SystemColors.Window
        Me.txtMSRP.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.txtMSRP, "txtMSRP")
        Me.txtMSRP.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMSRP.Name = "txtMSRP"
        Me.txtMSRP.Tag = "Currency"
        '
        'txtCost
        '
        Me.txtCost.AcceptsReturn = True
        Me.txtCost.BackColor = System.Drawing.SystemColors.Window
        Me.txtCost.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.txtCost, "txtCost")
        Me.txtCost.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCost.Name = "txtCost"
        Me.txtCost.Tag = "ExtCurrency"
        '
        'txtFreight
        '
        Me.txtFreight.AcceptsReturn = True
        Me.txtFreight.BackColor = System.Drawing.SystemColors.Window
        Me.txtFreight.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.txtFreight, "txtFreight")
        Me.txtFreight.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFreight.Name = "txtFreight"
        Me.txtFreight.Tag = "ExtCurrency"
        '
        'lblVendorPack
        '
        Me.lblVendorPack.BackColor = System.Drawing.Color.Transparent
        Me.lblVendorPack.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblVendorPack, "lblVendorPack")
        Me.lblVendorPack.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblVendorPack.Name = "lblVendorPack"
        '
        '_lblLabel_1
        '
        Me._lblLabel_1.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_1.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lblLabel_1, "_lblLabel_1")
        Me._lblLabel_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabel_1.Name = "_lblLabel_1"
        '
        '_lblLabel_0
        '
        Me._lblLabel_0.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_0.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lblLabel_0, "_lblLabel_0")
        Me._lblLabel_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabel_0.Name = "_lblLabel_0"
        '
        'lblPkgDesc
        '
        Me.lblPkgDesc.BackColor = System.Drawing.Color.Transparent
        Me.lblPkgDesc.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblPkgDesc, "lblPkgDesc")
        Me.lblPkgDesc.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPkgDesc.Name = "lblPkgDesc"
        '
        'lblCost
        '
        Me.lblCost.BackColor = System.Drawing.Color.Transparent
        Me.lblCost.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblCost, "lblCost")
        Me.lblCost.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCost.Name = "lblCost"
        '
        'lblStart
        '
        Me.lblStart.BackColor = System.Drawing.Color.Transparent
        Me.lblStart.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblStart, "lblStart")
        Me.lblStart.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblStart.Name = "lblStart"
        '
        'lblMSRP
        '
        Me.lblMSRP.BackColor = System.Drawing.Color.Transparent
        Me.lblMSRP.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblMSRP, "lblMSRP")
        Me.lblMSRP.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMSRP.Name = "lblMSRP"
        '
        'lblEnd
        '
        Me.lblEnd.BackColor = System.Drawing.Color.Transparent
        Me.lblEnd.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblEnd, "lblEnd")
        Me.lblEnd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEnd.Name = "lblEnd"
        '
        'lblFreight
        '
        Me.lblFreight.BackColor = System.Drawing.Color.Transparent
        Me.lblFreight.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblFreight, "lblFreight")
        Me.lblFreight.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFreight.Name = "lblFreight"
        '
        'dtpStartDate
        '
        resources.ApplyResources(Me.dtpStartDate, "dtpStartDate")
        Me.dtpStartDate.Name = "dtpStartDate"
        '
        'dtpEndDate
        '
        resources.ApplyResources(Me.dtpEndDate, "dtpEndDate")
        Me.dtpEndDate.Name = "dtpEndDate"
        '
        'ComboBox_FreightUnit
        '
        Me.ComboBox_FreightUnit.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.ComboBox_FreightUnit.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.ComboBox_FreightUnit.BackColor = System.Drawing.SystemColors.Window
        Me.ComboBox_FreightUnit.Cursor = System.Windows.Forms.Cursors.Default
        Me.ComboBox_FreightUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.ComboBox_FreightUnit, "ComboBox_FreightUnit")
        Me.ComboBox_FreightUnit.ForeColor = System.Drawing.SystemColors.WindowText
        Me.ComboBox_FreightUnit.Name = "ComboBox_FreightUnit"
        Me.ComboBox_FreightUnit.Sorted = True
        '
        'ComboBox_CostUnit
        '
        Me.ComboBox_CostUnit.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.ComboBox_CostUnit.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.ComboBox_CostUnit.BackColor = System.Drawing.SystemColors.Window
        Me.ComboBox_CostUnit.Cursor = System.Windows.Forms.Cursors.Default
        Me.ComboBox_CostUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.ComboBox_CostUnit, "ComboBox_CostUnit")
        Me.ComboBox_CostUnit.ForeColor = System.Drawing.SystemColors.WindowText
        Me.ComboBox_CostUnit.Name = "ComboBox_CostUnit"
        Me.ComboBox_CostUnit.Sorted = True
        '
        'CheckBox_CostedByWeight
        '
        resources.ApplyResources(Me.CheckBox_CostedByWeight, "CheckBox_CostedByWeight")
        Me.CheckBox_CostedByWeight.Name = "CheckBox_CostedByWeight"
        Me.CheckBox_CostedByWeight.UseVisualStyleBackColor = True
        '
        'ugrdRetailPackList
        '
        Appearance44.BackColor = System.Drawing.SystemColors.ControlLight
        Appearance44.BorderColor = System.Drawing.SystemColors.InactiveCaption
        resources.ApplyResources(Appearance44.FontData, "Appearance44.FontData")
        resources.ApplyResources(Appearance44, "Appearance44")
        Appearance44.ForceApplyResources = "FontData|"
        Me.ugrdRetailPackList.DisplayLayout.Appearance = Appearance44
        Me.ugrdRetailPackList.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        UltraGridColumn9.Header.VisiblePosition = 6
        UltraGridColumn9.Hidden = True
        UltraGridColumn10.Header.Caption = resources.GetString("resource.Caption1")
        UltraGridColumn10.Header.VisiblePosition = 0
        UltraGridColumn10.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(94, 0)
        UltraGridColumn11.Header.VisiblePosition = 2
        UltraGridColumn11.Hidden = True
        UltraGridColumn12.Header.Caption = resources.GetString("resource.Caption2")
        UltraGridColumn12.Header.VisiblePosition = 1
        UltraGridColumn12.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(65, 0)
        UltraGridColumn13.Header.Caption = resources.GetString("resource.Caption3")
        UltraGridColumn13.Header.VisiblePosition = 3
        UltraGridColumn13.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(40, 0)
        UltraGridColumn14.Header.Caption = resources.GetString("resource.Caption4")
        UltraGridColumn14.Header.VisiblePosition = 4
        UltraGridColumn14.MaxLength = 1
        UltraGridColumn14.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(22, 0)
        UltraGridColumn15.Header.Caption = resources.GetString("resource.Caption5")
        UltraGridColumn15.Header.VisiblePosition = 5
        UltraGridColumn15.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(78, 0)
        UltraGridColumn15.Width = 120
        UltraGridBand2.Columns.AddRange(New Object() {UltraGridColumn9, UltraGridColumn10, UltraGridColumn11, UltraGridColumn12, UltraGridColumn13, UltraGridColumn14, UltraGridColumn15})
        UltraGridBand2.Expandable = False
        UltraGridBand2.RowLayoutStyle = Infragistics.Win.UltraWinGrid.RowLayoutStyle.ColumnLayout
        Me.ugrdRetailPackList.DisplayLayout.BandsSerializer.Add(UltraGridBand2)
        Me.ugrdRetailPackList.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Appearance45.FontData.BoldAsString = resources.GetString("resource.BoldAsString4")
        resources.ApplyResources(Appearance45, "Appearance45")
        Appearance45.ForceApplyResources = ""
        Me.ugrdRetailPackList.DisplayLayout.CaptionAppearance = Appearance45
        Me.ugrdRetailPackList.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[True]
        Appearance46.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance46.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance46.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance46.BorderColor = System.Drawing.SystemColors.Window
        resources.ApplyResources(Appearance46.FontData, "Appearance46.FontData")
        resources.ApplyResources(Appearance46, "Appearance46")
        Appearance46.ForceApplyResources = "FontData|"
        Me.ugrdRetailPackList.DisplayLayout.GroupByBox.Appearance = Appearance46
        Appearance47.ForeColor = System.Drawing.SystemColors.GrayText
        resources.ApplyResources(Appearance47.FontData, "Appearance47.FontData")
        resources.ApplyResources(Appearance47, "Appearance47")
        Appearance47.ForceApplyResources = "FontData|"
        Me.ugrdRetailPackList.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance47
        Me.ugrdRetailPackList.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.ugrdRetailPackList.DisplayLayout.GroupByBox.Hidden = True
        Appearance48.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance48.BackColor2 = System.Drawing.SystemColors.Control
        Appearance48.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance48.ForeColor = System.Drawing.SystemColors.GrayText
        resources.ApplyResources(Appearance48.FontData, "Appearance48.FontData")
        resources.ApplyResources(Appearance48, "Appearance48")
        Appearance48.ForceApplyResources = "FontData|"
        Me.ugrdRetailPackList.DisplayLayout.GroupByBox.PromptAppearance = Appearance48
        Me.ugrdRetailPackList.DisplayLayout.MaxColScrollRegions = 1
        Me.ugrdRetailPackList.DisplayLayout.MaxRowScrollRegions = 1
        Me.ugrdRetailPackList.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.[False]
        Me.ugrdRetailPackList.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.[False]
        Me.ugrdRetailPackList.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.ugrdRetailPackList.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance49.BackColor = System.Drawing.SystemColors.Window
        resources.ApplyResources(Appearance49.FontData, "Appearance49.FontData")
        resources.ApplyResources(Appearance49, "Appearance49")
        Appearance49.ForceApplyResources = "FontData|"
        Me.ugrdRetailPackList.DisplayLayout.Override.CardAreaAppearance = Appearance49
        Appearance50.BorderColor = System.Drawing.Color.Silver
        Appearance50.FontData.BoldAsString = resources.GetString("resource.BoldAsString5")
        Appearance50.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        resources.ApplyResources(Appearance50, "Appearance50")
        Appearance50.ForceApplyResources = ""
        Me.ugrdRetailPackList.DisplayLayout.Override.CellAppearance = Appearance50
        Me.ugrdRetailPackList.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.ugrdRetailPackList.DisplayLayout.Override.CellPadding = 0
        Appearance51.FontData.BoldAsString = resources.GetString("resource.BoldAsString6")
        resources.ApplyResources(Appearance51, "Appearance51")
        Appearance51.ForceApplyResources = ""
        Me.ugrdRetailPackList.DisplayLayout.Override.FixedHeaderAppearance = Appearance51
        Appearance52.BackColor = System.Drawing.SystemColors.Control
        Appearance52.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance52.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance52.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance52.BorderColor = System.Drawing.SystemColors.Window
        resources.ApplyResources(Appearance52.FontData, "Appearance52.FontData")
        resources.ApplyResources(Appearance52, "Appearance52")
        Appearance52.ForceApplyResources = "FontData|"
        Me.ugrdRetailPackList.DisplayLayout.Override.GroupByRowAppearance = Appearance52
        Appearance53.FontData.BoldAsString = resources.GetString("resource.BoldAsString7")
        resources.ApplyResources(Appearance53, "Appearance53")
        Appearance53.ForceApplyResources = ""
        Me.ugrdRetailPackList.DisplayLayout.Override.HeaderAppearance = Appearance53
        Me.ugrdRetailPackList.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle
        Me.ugrdRetailPackList.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance54.BackColor = System.Drawing.SystemColors.ControlLight
        resources.ApplyResources(Appearance54.FontData, "Appearance54.FontData")
        resources.ApplyResources(Appearance54, "Appearance54")
        Appearance54.ForceApplyResources = "FontData|"
        Me.ugrdRetailPackList.DisplayLayout.Override.RowAlternateAppearance = Appearance54
        Appearance55.BackColor = System.Drawing.SystemColors.Window
        Appearance55.BorderColor = System.Drawing.Color.Silver
        resources.ApplyResources(Appearance55.FontData, "Appearance55.FontData")
        resources.ApplyResources(Appearance55, "Appearance55")
        Appearance55.ForceApplyResources = "FontData|"
        Me.ugrdRetailPackList.DisplayLayout.Override.RowAppearance = Appearance55
        Me.ugrdRetailPackList.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[True]
        Me.ugrdRetailPackList.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed
        Me.ugrdRetailPackList.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.ugrdRetailPackList.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.ugrdRetailPackList.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Extended
        Appearance56.BackColor = System.Drawing.SystemColors.ControlLight
        resources.ApplyResources(Appearance56.FontData, "Appearance56.FontData")
        resources.ApplyResources(Appearance56, "Appearance56")
        Appearance56.ForceApplyResources = "FontData|"
        Me.ugrdRetailPackList.DisplayLayout.Override.TemplateAddRowAppearance = Appearance56
        Me.ugrdRetailPackList.DisplayLayout.Scrollbars = Infragistics.Win.UltraWinGrid.Scrollbars.None
        Me.ugrdRetailPackList.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.ugrdRetailPackList.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.ugrdRetailPackList.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand
        Me.ugrdRetailPackList.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        resources.ApplyResources(Me.ugrdRetailPackList, "ugrdRetailPackList")
        Me.ugrdRetailPackList.Name = "ugrdRetailPackList"
        Me.ugrdRetailPackList.UseFlatMode = Infragistics.Win.DefaultableBoolean.[True]
        '
        '_labelVendorPackUOM
        '
        resources.ApplyResources(Me._labelVendorPackUOM, "_labelVendorPackUOM")
        Me._labelVendorPackUOM.Name = "_labelVendorPackUOM"
        '
        'CheckBox_CatchweightRequired
        '
        resources.ApplyResources(Me.CheckBox_CatchweightRequired, "CheckBox_CatchweightRequired")
        Me.CheckBox_CatchweightRequired.Name = "CheckBox_CatchweightRequired"
        Me.CheckBox_CatchweightRequired.UseVisualStyleBackColor = True
        '
        'chkIgnorePackUpdates
        '
        resources.ApplyResources(Me.chkIgnorePackUpdates, "chkIgnorePackUpdates")
        Me.chkIgnorePackUpdates.Name = "chkIgnorePackUpdates"
        Me.chkIgnorePackUpdates.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Name = "Label1"
        '
        'cmbCurrency
        '
        Me.cmbCurrency.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbCurrency.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbCurrency.BackColor = System.Drawing.SystemColors.Window
        Me.cmbCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbCurrency.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.cmbCurrency, "cmbCurrency")
        Me.cmbCurrency.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbCurrency.Name = "cmbCurrency"
        Me.cmbCurrency.Sorted = True
        '
        'TextBox_RetailCasePack
        '
        Me.TextBox_RetailCasePack.AcceptsReturn = True
        Me.TextBox_RetailCasePack.BackColor = System.Drawing.SystemColors.Window
        Me.TextBox_RetailCasePack.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.TextBox_RetailCasePack, "TextBox_RetailCasePack")
        Me.TextBox_RetailCasePack.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextBox_RetailCasePack.Name = "TextBox_RetailCasePack"
        Me.TextBox_RetailCasePack.Tag = "Currency"
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.Label2, "Label2")
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Name = "Label2"
        '
        'lblCurrency
        '
        resources.ApplyResources(Me.lblCurrency, "lblCurrency")
        Me.lblCurrency.Name = "lblCurrency"
        '
        'btnConversionCalculator
        '
        resources.ApplyResources(Me.btnConversionCalculator, "btnConversionCalculator")
        Me.btnConversionCalculator.Name = "btnConversionCalculator"
        Me.btnConversionCalculator.UseVisualStyleBackColor = True
        '
        'frmVendorCostDetail
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.Controls.Add(Me.btnConversionCalculator)
        Me.Controls.Add(Me.lblCurrency)
        Me.Controls.Add(Me.TextBox_RetailCasePack)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.cmbCurrency)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.chkIgnorePackUpdates)
        Me.Controls.Add(Me.CheckBox_CatchweightRequired)
        Me.Controls.Add(Me._labelVendorPackUOM)
        Me.Controls.Add(Me.ugrdRetailPackList)
        Me.Controls.Add(Me.Button_MarginInfo)
        Me.Controls.Add(Me.CheckBox_CostedByWeight)
        Me.Controls.Add(Me.ComboBox_FreightUnit)
        Me.Controls.Add(Me.ComboBox_CostUnit)
        Me.Controls.Add(Me.dtpEndDate)
        Me.Controls.Add(Me.dtpStartDate)
        Me.Controls.Add(Me.txtPkgDesc1)
        Me.Controls.Add(Me.txtMSRP)
        Me.Controls.Add(Me.txtCost)
        Me.Controls.Add(Me.txtFreight)
        Me.Controls.Add(Me.lblVendorPack)
        Me.Controls.Add(Me._lblLabel_1)
        Me.Controls.Add(Me._lblLabel_0)
        Me.Controls.Add(Me.lblPkgDesc)
        Me.Controls.Add(Me.lblCost)
        Me.Controls.Add(Me.lblStart)
        Me.Controls.Add(Me.lblMSRP)
        Me.Controls.Add(Me.lblEnd)
        Me.Controls.Add(Me.lblFreight)
        Me.Controls.Add(Me.fraStoreSel)
        Me.Controls.Add(Me.fraType)
        Me.Controls.Add(Me.cmdSelect)
        Me.Controls.Add(Me.cmdExit)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmVendorCostDetail"
        Me.ShowInTaskbar = False
        Me.fraStoreSel.ResumeLayout(False)
        CType(Me.ugrdStoreList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.fraType.ResumeLayout(False)
        CType(Me.optSelection, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.optType, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtDate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dtpStartDate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dtpEndDate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ugrdRetailPackList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Public WithEvents txtPkgDesc1 As System.Windows.Forms.TextBox
    Public WithEvents txtMSRP As System.Windows.Forms.TextBox
    Public WithEvents txtCost As System.Windows.Forms.TextBox
    Public WithEvents txtFreight As System.Windows.Forms.TextBox
    Public WithEvents lblVendorPack As System.Windows.Forms.Label
    Public WithEvents _lblLabel_1 As System.Windows.Forms.Label
    Public WithEvents _lblLabel_0 As System.Windows.Forms.Label
    Public WithEvents lblPkgDesc As System.Windows.Forms.Label
    Public WithEvents lblCost As System.Windows.Forms.Label
    Public WithEvents lblStart As System.Windows.Forms.Label
    Public WithEvents lblMSRP As System.Windows.Forms.Label
    Public WithEvents lblEnd As System.Windows.Forms.Label
    Public WithEvents lblFreight As System.Windows.Forms.Label
    Friend WithEvents ugrdStoreList As Infragistics.Win.UltraWinGrid.UltraGrid
    Friend WithEvents dtpStartDate As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
    Friend WithEvents dtpEndDate As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
    Public WithEvents ComboBox_FreightUnit As System.Windows.Forms.ComboBox
    Public WithEvents ComboBox_CostUnit As System.Windows.Forms.ComboBox
    Friend WithEvents CheckBox_CostedByWeight As System.Windows.Forms.CheckBox
    Public WithEvents Button_MarginInfo As System.Windows.Forms.Button
    Public WithEvents cmbJurisdiction As System.Windows.Forms.ComboBox
    'Public WithEvents _optSelection_5 As System.Windows.Forms.RadioButton
    Friend WithEvents ugrdRetailPackList As Infragistics.Win.UltraWinGrid.UltraGrid
    Friend WithEvents _labelVendorPackUOM As System.Windows.Forms.Label
    Friend WithEvents CheckBox_CatchweightRequired As System.Windows.Forms.CheckBox
    Friend WithEvents chkIgnorePackUpdates As System.Windows.Forms.CheckBox
    Public WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents cmbCurrency As System.Windows.Forms.ComboBox
    Public WithEvents TextBox_RetailCasePack As System.Windows.Forms.TextBox
    Public WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lblCurrency As System.Windows.Forms.Label
    Friend WithEvents btnConversionCalculator As System.Windows.Forms.Button
#End Region
End Class
