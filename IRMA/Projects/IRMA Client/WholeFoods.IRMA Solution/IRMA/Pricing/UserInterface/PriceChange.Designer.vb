<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmPriceChange
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
    'Public WithEvents CrystalReport1 As AxCrystal.AxCrystalReport
	Public WithEvents cmdExit As System.Windows.Forms.Button
	Public WithEvents cmdSelect As System.Windows.Forms.Button
    Public WithEvents lblAvgCost As System.Windows.Forms.Label
    Public WithEvents lblCost As System.Windows.Forms.Label
    Public WithEvents Frame1 As Microsoft.VisualBasic.Compatibility.VB6.GroupBoxArray
    Public WithEvents optSelection As Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPriceChange))
        Dim Appearance1 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim UltraGridBand1 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1)
        Dim UltraGridColumn1 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Store_No")
        Dim UltraGridColumn2 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Store_Name")
        Dim UltraGridColumn3 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Zone_ID")
        Dim UltraGridColumn4 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("State")
        Dim UltraGridColumn5 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("WFM_Store")
        Dim UltraGridColumn6 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Mega_Store")
        Dim UltraGridColumn7 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("CustomerType")
        Dim UltraGridColumn8 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("TaxRate")
        Dim UltraGridColumn9 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Price")
        Dim UltraGridColumn10 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("hasVendor")
        Dim Appearance2 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance3 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance4 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance5 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance6 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance7 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance8 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance9 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance10 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance11 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance12 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance13 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance14 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim UltraDataColumn1 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Store_No")
        Dim UltraDataColumn2 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Store_Name")
        Dim UltraDataColumn3 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Zone_ID")
        Dim UltraDataColumn4 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("State")
        Dim UltraDataColumn5 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("WFM_Store")
        Dim UltraDataColumn6 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Mega_Store")
        Dim UltraDataColumn7 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("CustomerType")
        Dim UltraDataColumn8 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("TaxRate")
        Dim UltraDataColumn9 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Price")
        Dim UltraDataColumn10 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("hasVendor")
        Dim UltraDataColumn11 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Store_No")
        Dim UltraDataColumn12 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Store_Name")
        Dim UltraDataColumn13 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Zone_ID")
        Dim UltraDataColumn14 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("State")
        Dim UltraDataColumn15 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("WFM_Store")
        Dim UltraDataColumn16 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Mega_Store")
        Dim UltraDataColumn17 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("CustomerType")
        Dim UltraDataColumn18 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("TaxRate")
        Dim UltraDataColumn19 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Price")
        Dim UltraDataColumn20 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("hasVendor")
        Dim UltraDataColumn21 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Store_No")
        Dim UltraDataColumn22 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Store_Name")
        Dim UltraDataColumn23 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Zone_ID")
        Dim UltraDataColumn24 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("State")
        Dim UltraDataColumn25 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("WFM_Store")
        Dim UltraDataColumn26 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Mega_Store")
        Dim UltraDataColumn27 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("CustomerType")
        Dim UltraDataColumn28 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("TaxRate")
        Dim UltraDataColumn29 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Price")
        Dim UltraDataColumn30 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("hasVendor")
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdExit = New System.Windows.Forms.Button
        Me.cmdSelect = New System.Windows.Forms.Button
        Me.AllWFMRadioButton = New System.Windows.Forms.RadioButton
        Me.StateRadioButton = New System.Windows.Forms.RadioButton
        Me.ZoneRadioButton = New System.Windows.Forms.RadioButton
        Me.AllRadioButton = New System.Windows.Forms.RadioButton
        Me.ManualRadioButton = New System.Windows.Forms.RadioButton
        Me.cmdItemVendors = New System.Windows.Forms.Button
        Me.Button_MarginInfo = New System.Windows.Forms.Button
        Me.lblAvgCost = New System.Windows.Forms.Label
        Me.lblCost = New System.Windows.Forms.Label
        Me.Frame1 = New Microsoft.VisualBasic.Compatibility.VB6.GroupBoxArray(Me.components)
        Me.optSelection = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(Me.components)
        Me.ugrdStoreList = New Infragistics.Win.UltraWinGrid.UltraGrid
        Me.StoresUltraDataSource = New Infragistics.Win.UltraWinDataSource.UltraDataSource(Me.components)
        Me.UltraDataSource2 = New Infragistics.Win.UltraWinDataSource.UltraDataSource(Me.components)
        Me.UltraDataSource1 = New Infragistics.Win.UltraWinDataSource.UltraDataSource(Me.components)
        Me._Frame1_1 = New System.Windows.Forms.GroupBox
        Me.cmbStates = New System.Windows.Forms.ComboBox
        Me.cmbZones = New System.Windows.Forms.ComboBox
        Me.frmPriceInfo = New System.Windows.Forms.GroupBox
        Me.dtStartDate = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtPOSPrice = New Infragistics.Win.UltraWinEditors.UltraNumericEditor
        Me.txtMultiple = New System.Windows.Forms.TextBox
        Me.lblPrice = New System.Windows.Forms.Label
        Me.lblSlash = New System.Windows.Forms.Label
        Me.checkboxPriceDisplay = New System.Windows.Forms.CheckBox
        CType(Me.Frame1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.optSelection, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ugrdStoreList, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.StoresUltraDataSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.UltraDataSource2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.UltraDataSource1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._Frame1_1.SuspendLayout()
        Me.frmPriceInfo.SuspendLayout()
        CType(Me.dtStartDate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtPOSPrice, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
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
        'AllWFMRadioButton
        '
        Me.AllWFMRadioButton.BackColor = System.Drawing.SystemColors.Control
        Me.AllWFMRadioButton.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.AllWFMRadioButton, "AllWFMRadioButton")
        Me.AllWFMRadioButton.ForeColor = System.Drawing.SystemColors.ControlText
        Me.AllWFMRadioButton.Name = "AllWFMRadioButton"
        Me.AllWFMRadioButton.TabStop = True
        Me.ToolTip1.SetToolTip(Me.AllWFMRadioButton, resources.GetString("AllWFMRadioButton.ToolTip"))
        Me.AllWFMRadioButton.UseVisualStyleBackColor = False
        '
        'StateRadioButton
        '
        Me.StateRadioButton.BackColor = System.Drawing.SystemColors.Control
        Me.StateRadioButton.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.StateRadioButton, "StateRadioButton")
        Me.StateRadioButton.ForeColor = System.Drawing.SystemColors.ControlText
        Me.StateRadioButton.Name = "StateRadioButton"
        Me.StateRadioButton.TabStop = True
        Me.ToolTip1.SetToolTip(Me.StateRadioButton, resources.GetString("StateRadioButton.ToolTip"))
        Me.StateRadioButton.UseVisualStyleBackColor = False
        '
        'ZoneRadioButton
        '
        Me.ZoneRadioButton.BackColor = System.Drawing.SystemColors.Control
        Me.ZoneRadioButton.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.ZoneRadioButton, "ZoneRadioButton")
        Me.ZoneRadioButton.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ZoneRadioButton.Name = "ZoneRadioButton"
        Me.ZoneRadioButton.TabStop = True
        Me.ToolTip1.SetToolTip(Me.ZoneRadioButton, resources.GetString("ZoneRadioButton.ToolTip"))
        Me.ZoneRadioButton.UseVisualStyleBackColor = False
        '
        'AllRadioButton
        '
        Me.AllRadioButton.BackColor = System.Drawing.SystemColors.Control
        Me.AllRadioButton.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.AllRadioButton, "AllRadioButton")
        Me.AllRadioButton.ForeColor = System.Drawing.SystemColors.ControlText
        Me.AllRadioButton.Name = "AllRadioButton"
        Me.AllRadioButton.TabStop = True
        Me.ToolTip1.SetToolTip(Me.AllRadioButton, resources.GetString("AllRadioButton.ToolTip"))
        Me.AllRadioButton.UseVisualStyleBackColor = False
        '
        'ManualRadioButton
        '
        Me.ManualRadioButton.BackColor = System.Drawing.SystemColors.Control
        Me.ManualRadioButton.Checked = True
        Me.ManualRadioButton.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.ManualRadioButton, "ManualRadioButton")
        Me.ManualRadioButton.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ManualRadioButton.Name = "ManualRadioButton"
        Me.ManualRadioButton.TabStop = True
        Me.ToolTip1.SetToolTip(Me.ManualRadioButton, resources.GetString("ManualRadioButton.ToolTip"))
        Me.ManualRadioButton.UseVisualStyleBackColor = False
        '
        'cmdItemVendors
        '
        resources.ApplyResources(Me.cmdItemVendors, "cmdItemVendors")
        Me.cmdItemVendors.BackColor = System.Drawing.SystemColors.Control
        Me.cmdItemVendors.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdItemVendors.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdItemVendors.Name = "cmdItemVendors"
        Me.ToolTip1.SetToolTip(Me.cmdItemVendors, resources.GetString("cmdItemVendors.ToolTip"))
        Me.cmdItemVendors.UseVisualStyleBackColor = False
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
        'lblAvgCost
        '
        Me.lblAvgCost.BackColor = System.Drawing.Color.Transparent
        Me.lblAvgCost.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblAvgCost, "lblAvgCost")
        Me.lblAvgCost.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAvgCost.Name = "lblAvgCost"
        '
        'lblCost
        '
        Me.lblCost.BackColor = System.Drawing.SystemColors.Control
        Me.lblCost.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblCost, "lblCost")
        Me.lblCost.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCost.Name = "lblCost"
        '
        'ugrdStoreList
        '
        Me.ugrdStoreList.DataMember = "Band 0"
        Me.ugrdStoreList.DataSource = Me.StoresUltraDataSource
        Appearance1.BackColor = System.Drawing.SystemColors.ControlLight
        Appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.ugrdStoreList.DisplayLayout.Appearance = Appearance1
        Me.ugrdStoreList.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        UltraGridColumn1.Header.VisiblePosition = 0
        UltraGridColumn1.Width = 19
        UltraGridColumn2.Header.VisiblePosition = 1
        UltraGridColumn2.Width = 15
        UltraGridColumn3.Header.VisiblePosition = 2
        UltraGridColumn3.Width = 15
        UltraGridColumn4.Header.VisiblePosition = 3
        UltraGridColumn4.Width = 33
        UltraGridColumn5.Header.VisiblePosition = 4
        UltraGridColumn5.Width = 37
        UltraGridColumn6.Header.VisiblePosition = 5
        UltraGridColumn6.Width = 33
        UltraGridColumn7.Header.VisiblePosition = 6
        UltraGridColumn7.Width = 33
        UltraGridColumn8.Header.VisiblePosition = 7
        UltraGridColumn8.Width = 26
        UltraGridColumn9.Header.VisiblePosition = 8
        UltraGridColumn9.Width = 33
        UltraGridColumn10.Header.VisiblePosition = 9
        UltraGridColumn10.Hidden = True
        UltraGridColumn10.Width = 24
        UltraGridBand1.Columns.AddRange(New Object() {UltraGridColumn1, UltraGridColumn2, UltraGridColumn3, UltraGridColumn4, UltraGridColumn5, UltraGridColumn6, UltraGridColumn7, UltraGridColumn8, UltraGridColumn9, UltraGridColumn10})
        Me.ugrdStoreList.DisplayLayout.BandsSerializer.Add(UltraGridBand1)
        Me.ugrdStoreList.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Appearance2.FontData.BoldAsString = resources.GetString("resource.BoldAsString")
        Me.ugrdStoreList.DisplayLayout.CaptionAppearance = Appearance2
        Me.ugrdStoreList.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Appearance3.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance3.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance3.BorderColor = System.Drawing.SystemColors.Window
        Me.ugrdStoreList.DisplayLayout.GroupByBox.Appearance = Appearance3
        Appearance4.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugrdStoreList.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance4
        Me.ugrdStoreList.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.ugrdStoreList.DisplayLayout.GroupByBox.Hidden = True
        Appearance5.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance5.BackColor2 = System.Drawing.SystemColors.Control
        Appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance5.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugrdStoreList.DisplayLayout.GroupByBox.PromptAppearance = Appearance5
        Me.ugrdStoreList.DisplayLayout.MaxColScrollRegions = 1
        Me.ugrdStoreList.DisplayLayout.MaxRowScrollRegions = 1
        Appearance6.BackColor = System.Drawing.SystemColors.Window
        Appearance6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ugrdStoreList.DisplayLayout.Override.ActiveCellAppearance = Appearance6
        Me.ugrdStoreList.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No
        Me.ugrdStoreList.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.[False]
        Me.ugrdStoreList.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.[False]
        Me.ugrdStoreList.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.ugrdStoreList.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance7.BackColor = System.Drawing.SystemColors.Window
        Me.ugrdStoreList.DisplayLayout.Override.CardAreaAppearance = Appearance7
        Appearance8.BorderColor = System.Drawing.Color.Silver
        Appearance8.FontData.BoldAsString = resources.GetString("resource.BoldAsString1")
        Appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.ugrdStoreList.DisplayLayout.Override.CellAppearance = Appearance8
        Me.ugrdStoreList.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        Me.ugrdStoreList.DisplayLayout.Override.CellPadding = 0
        Appearance9.FontData.BoldAsString = resources.GetString("resource.BoldAsString2")
        Me.ugrdStoreList.DisplayLayout.Override.FixedHeaderAppearance = Appearance9
        Appearance10.BackColor = System.Drawing.SystemColors.Control
        Appearance10.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance10.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance10.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance10.BorderColor = System.Drawing.SystemColors.Window
        Me.ugrdStoreList.DisplayLayout.Override.GroupByRowAppearance = Appearance10
        Appearance11.FontData.BoldAsString = resources.GetString("resource.BoldAsString3")
        Appearance11.TextHAlign = Infragistics.Win.HAlign.Left
        Me.ugrdStoreList.DisplayLayout.Override.HeaderAppearance = Appearance11
        Me.ugrdStoreList.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle
        Me.ugrdStoreList.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance12.BackColor = System.Drawing.SystemColors.ControlLight
        Me.ugrdStoreList.DisplayLayout.Override.RowAlternateAppearance = Appearance12
        Appearance13.BackColor = System.Drawing.SystemColors.Window
        Appearance13.BorderColor = System.Drawing.Color.Silver
        Me.ugrdStoreList.DisplayLayout.Override.RowAppearance = Appearance13
        Me.ugrdStoreList.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[True]
        Me.ugrdStoreList.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed
        Me.ugrdStoreList.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.ugrdStoreList.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.ugrdStoreList.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Extended
        Appearance14.BackColor = System.Drawing.SystemColors.ControlLight
        Me.ugrdStoreList.DisplayLayout.Override.TemplateAddRowAppearance = Appearance14
        Me.ugrdStoreList.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.ugrdStoreList.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.ugrdStoreList.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand
        Me.ugrdStoreList.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        resources.ApplyResources(Me.ugrdStoreList, "ugrdStoreList")
        Me.ugrdStoreList.Name = "ugrdStoreList"
        '
        'StoresUltraDataSource
        '
        UltraDataColumn1.DataType = GetType(Short)
        UltraDataColumn3.DataType = GetType(Short)
        UltraDataColumn5.DataType = GetType(Boolean)
        UltraDataColumn6.DataType = GetType(Boolean)
        UltraDataColumn8.DataType = GetType(Double)
        UltraDataColumn10.DataType = GetType(Boolean)
        Me.StoresUltraDataSource.Band.Columns.AddRange(New Object() {UltraDataColumn1, UltraDataColumn2, UltraDataColumn3, UltraDataColumn4, UltraDataColumn5, UltraDataColumn6, UltraDataColumn7, UltraDataColumn8, UltraDataColumn9, UltraDataColumn10})
        '
        'UltraDataSource2
        '
        Me.UltraDataSource2.Band.Columns.AddRange(New Object() {UltraDataColumn11, UltraDataColumn12, UltraDataColumn13, UltraDataColumn14, UltraDataColumn15, UltraDataColumn16, UltraDataColumn17, UltraDataColumn18, UltraDataColumn19, UltraDataColumn20})
        '
        'UltraDataSource1
        '
        UltraDataColumn30.DataType = GetType(Boolean)
        Me.UltraDataSource1.Band.Columns.AddRange(New Object() {UltraDataColumn21, UltraDataColumn22, UltraDataColumn23, UltraDataColumn24, UltraDataColumn25, UltraDataColumn26, UltraDataColumn27, UltraDataColumn28, UltraDataColumn29, UltraDataColumn30})
        '
        '_Frame1_1
        '
        Me._Frame1_1.BackColor = System.Drawing.SystemColors.Control
        Me._Frame1_1.Controls.Add(Me.cmbStates)
        Me._Frame1_1.Controls.Add(Me.AllWFMRadioButton)
        Me._Frame1_1.Controls.Add(Me.StateRadioButton)
        Me._Frame1_1.Controls.Add(Me.cmbZones)
        Me._Frame1_1.Controls.Add(Me.ZoneRadioButton)
        Me._Frame1_1.Controls.Add(Me.AllRadioButton)
        Me._Frame1_1.Controls.Add(Me.ManualRadioButton)
        resources.ApplyResources(Me._Frame1_1, "_Frame1_1")
        Me._Frame1_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._Frame1_1.Name = "_Frame1_1"
        Me._Frame1_1.TabStop = False
        '
        'cmbStates
        '
        Me.cmbStates.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cmbStates.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbStates.BackColor = System.Drawing.SystemColors.Window
        Me.cmbStates.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbStates.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.cmbStates, "cmbStates")
        Me.cmbStates.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbStates.Name = "cmbStates"
        Me.cmbStates.Sorted = True
        '
        'cmbZones
        '
        Me.cmbZones.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cmbZones.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbZones.BackColor = System.Drawing.SystemColors.Window
        Me.cmbZones.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbZones.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.cmbZones, "cmbZones")
        Me.cmbZones.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbZones.Name = "cmbZones"
        Me.cmbZones.Sorted = True
        '
        'frmPriceInfo
        '
        Me.frmPriceInfo.BackColor = System.Drawing.SystemColors.Control
        Me.frmPriceInfo.Controls.Add(Me.dtStartDate)
        Me.frmPriceInfo.Controls.Add(Me.Label1)
        Me.frmPriceInfo.Controls.Add(Me.txtPOSPrice)
        Me.frmPriceInfo.Controls.Add(Me.txtMultiple)
        Me.frmPriceInfo.Controls.Add(Me.lblPrice)
        Me.frmPriceInfo.Controls.Add(Me.lblSlash)
        resources.ApplyResources(Me.frmPriceInfo, "frmPriceInfo")
        Me.frmPriceInfo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.frmPriceInfo.Name = "frmPriceInfo"
        Me.frmPriceInfo.TabStop = False
        '
        'dtStartDate
        '
        Me.dtStartDate.DateTime = New Date(1980, 1, 1, 0, 0, 0, 0)
        resources.ApplyResources(Me.dtStartDate, "dtStartDate")
        Me.dtStartDate.MinDate = New Date(1980, 1, 1, 0, 0, 0, 0)
        Me.dtStartDate.Name = "dtStartDate"
        Me.dtStartDate.Value = New Date(1980, 1, 1, 0, 0, 0, 0)
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Name = "Label1"
        '
        'txtPOSPrice
        '
        resources.ApplyResources(Me.txtPOSPrice, "txtPOSPrice")
        Me.txtPOSPrice.MaskClipMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeBoth
        Me.txtPOSPrice.MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeLiterals
        Me.txtPOSPrice.MaskInput = "{double:6.2}"
        Me.txtPOSPrice.MaxValue = 100000
        Me.txtPOSPrice.MinValue = 0
        Me.txtPOSPrice.Name = "txtPOSPrice"
        Me.txtPOSPrice.Nullable = True
        Me.txtPOSPrice.NullText = "0.00"
        Me.txtPOSPrice.NumericType = Infragistics.Win.UltraWinEditors.NumericType.[Double]
        Me.txtPOSPrice.PromptChar = Global.Microsoft.VisualBasic.ChrW(32)
        '
        'txtMultiple
        '
        Me.txtMultiple.AcceptsReturn = True
        Me.txtMultiple.BackColor = System.Drawing.SystemColors.Window
        Me.txtMultiple.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.txtMultiple, "txtMultiple")
        Me.txtMultiple.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMultiple.Name = "txtMultiple"
        Me.txtMultiple.Tag = "Number"
        '
        'lblPrice
        '
        Me.lblPrice.BackColor = System.Drawing.Color.Transparent
        Me.lblPrice.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblPrice, "lblPrice")
        Me.lblPrice.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPrice.Name = "lblPrice"
        '
        'lblSlash
        '
        Me.lblSlash.BackColor = System.Drawing.SystemColors.Control
        Me.lblSlash.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblSlash, "lblSlash")
        Me.lblSlash.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSlash.Name = "lblSlash"
        '
        'checkboxPriceDisplay
        '
        resources.ApplyResources(Me.checkboxPriceDisplay, "checkboxPriceDisplay")
        Me.checkboxPriceDisplay.Name = "checkboxPriceDisplay"
        Me.checkboxPriceDisplay.UseVisualStyleBackColor = True
        '
        'frmPriceChange
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.Controls.Add(Me.Button_MarginInfo)
        Me.Controls.Add(Me.cmdItemVendors)
        Me.Controls.Add(Me.checkboxPriceDisplay)
        Me.Controls.Add(Me.ugrdStoreList)
        Me.Controls.Add(Me._Frame1_1)
        Me.Controls.Add(Me.frmPriceInfo)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.cmdSelect)
        Me.Controls.Add(Me.lblAvgCost)
        Me.Controls.Add(Me.lblCost)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmPriceChange"
        Me.ShowInTaskbar = False
        CType(Me.Frame1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.optSelection, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ugrdStoreList, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.StoresUltraDataSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.UltraDataSource2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.UltraDataSource1, System.ComponentModel.ISupportInitialize).EndInit()
        Me._Frame1_1.ResumeLayout(False)
        Me.frmPriceInfo.ResumeLayout(False)
        Me.frmPriceInfo.PerformLayout()
        CType(Me.dtStartDate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtPOSPrice, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ugrdStoreList As Infragistics.Win.UltraWinGrid.UltraGrid
    Public WithEvents _Frame1_1 As System.Windows.Forms.GroupBox
    Public WithEvents cmbStates As System.Windows.Forms.ComboBox
    Public WithEvents AllWFMRadioButton As System.Windows.Forms.RadioButton
    Public WithEvents StateRadioButton As System.Windows.Forms.RadioButton
    Public WithEvents cmbZones As System.Windows.Forms.ComboBox
    Public WithEvents ZoneRadioButton As System.Windows.Forms.RadioButton
    Public WithEvents AllRadioButton As System.Windows.Forms.RadioButton
    Public WithEvents ManualRadioButton As System.Windows.Forms.RadioButton
    Public WithEvents frmPriceInfo As System.Windows.Forms.GroupBox
    Public WithEvents txtMultiple As System.Windows.Forms.TextBox
    Public WithEvents lblPrice As System.Windows.Forms.Label
    Public WithEvents lblSlash As System.Windows.Forms.Label
    Friend WithEvents StoresUltraDataSource As Infragistics.Win.UltraWinDataSource.UltraDataSource
    Friend WithEvents txtPOSPrice As Infragistics.Win.UltraWinEditors.UltraNumericEditor
    Friend WithEvents checkboxPriceDisplay As System.Windows.Forms.CheckBox
    Public WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents dtStartDate As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
    Friend WithEvents UltraDataSource1 As Infragistics.Win.UltraWinDataSource.UltraDataSource
    Friend WithEvents UltraDataSource2 As Infragistics.Win.UltraWinDataSource.UltraDataSource
    Public WithEvents cmdItemVendors As System.Windows.Forms.Button
    Public WithEvents Button_MarginInfo As System.Windows.Forms.Button
#End Region
End Class