<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmVendorReports
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
	Public WithEvents cmdExit As System.Windows.Forms.Button
	Public WithEvents cmdReport As System.Windows.Forms.Button
	Public WithEvents fraButtons As System.Windows.Forms.Panel
	Public WithEvents optEachStore As System.Windows.Forms.RadioButton
	Public WithEvents cmbZoneSingleRpt As System.Windows.Forms.ComboBox
	Public WithEvents optZoneSingleRpt As System.Windows.Forms.RadioButton
	Public WithEvents optRegionSingleRpt As System.Windows.Forms.RadioButton
	Public WithEvents Frame2 As System.Windows.Forms.GroupBox
	Public WithEvents cmbTeam As System.Windows.Forms.ComboBox
	Public WithEvents txtUPC As System.Windows.Forms.TextBox
	Public WithEvents cmbBrand As System.Windows.Forms.ComboBox
    Public WithEvents Label1 As System.Windows.Forms.Label
	Public WithEvents Label2 As System.Windows.Forms.Label
	Public WithEvents Label3 As System.Windows.Forms.Label
	Public WithEvents Frame1 As System.Windows.Forms.GroupBox
	Public WithEvents _optSelection_0 As System.Windows.Forms.RadioButton
	Public WithEvents _optSelection_1 As System.Windows.Forms.RadioButton
	Public WithEvents _optSelection_2 As System.Windows.Forms.RadioButton
	Public WithEvents cmbZones As System.Windows.Forms.ComboBox
	Public WithEvents _optSelection_3 As System.Windows.Forms.RadioButton
	Public WithEvents _optSelection_4 As System.Windows.Forms.RadioButton
	Public WithEvents cmbStates As System.Windows.Forms.ComboBox
    Public WithEvents fraStores As System.Windows.Forms.GroupBox
	Public WithEvents txtVendorName As System.Windows.Forms.TextBox
	Public WithEvents cmdCompanySearch As System.Windows.Forms.Button
    Public WithEvents lblVendor As System.Windows.Forms.Label
    Public WithEvents optSelection As Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmVendorReports))
        Dim Appearance1 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim UltraGridBand1 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1)
        Dim UltraGridColumn1 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Store_No")
        Dim UltraGridColumn2 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Store_Name")
        Dim Appearance2 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim UltraGridColumn3 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Zone_ID")
        Dim UltraGridColumn4 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("State")
        Dim UltraGridColumn5 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("WFM_Store")
        Dim UltraGridColumn6 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Mega_Store")
        Dim UltraGridColumn7 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("CustomerType")
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
        Dim Appearance15 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance16 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdExit = New System.Windows.Forms.Button
        Me.cmdReport = New System.Windows.Forms.Button
        Me.optZoneSingleRpt = New System.Windows.Forms.RadioButton
        Me._optSelection_0 = New System.Windows.Forms.RadioButton
        Me._optSelection_1 = New System.Windows.Forms.RadioButton
        Me._optSelection_2 = New System.Windows.Forms.RadioButton
        Me._optSelection_3 = New System.Windows.Forms.RadioButton
        Me._optSelection_4 = New System.Windows.Forms.RadioButton
        Me.cmdCompanySearch = New System.Windows.Forms.Button
        Me.fraButtons = New System.Windows.Forms.Panel
        Me.Frame2 = New System.Windows.Forms.GroupBox
        Me.optEachStore = New System.Windows.Forms.RadioButton
        Me.cmbZoneSingleRpt = New System.Windows.Forms.ComboBox
        Me.optRegionSingleRpt = New System.Windows.Forms.RadioButton
        Me.Frame1 = New System.Windows.Forms.GroupBox
        Me.HierarchySelector1 = New HierarchySelector
        Me.cmbTeam = New System.Windows.Forms.ComboBox
        Me.txtUPC = New System.Windows.Forms.TextBox
        Me.cmbBrand = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.fraStores = New System.Windows.Forms.GroupBox
        Me.ugrdStoreList = New Infragistics.Win.UltraWinGrid.UltraGrid
        Me.cmbZones = New System.Windows.Forms.ComboBox
        Me.cmbStates = New System.Windows.Forms.ComboBox
        Me.txtVendorName = New System.Windows.Forms.TextBox
        Me.lblVendor = New System.Windows.Forms.Label
        Me.optSelection = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(Me.components)
        Me.fraButtons.SuspendLayout()
        Me.Frame2.SuspendLayout()
        Me.Frame1.SuspendLayout()
        Me.fraStores.SuspendLayout()
        CType(Me.ugrdStoreList, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.optSelection, System.ComponentModel.ISupportInitialize).BeginInit()
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
        'optZoneSingleRpt
        '
        Me.optZoneSingleRpt.BackColor = System.Drawing.SystemColors.Control
        Me.optZoneSingleRpt.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.optZoneSingleRpt, "optZoneSingleRpt")
        Me.optZoneSingleRpt.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optZoneSingleRpt.Name = "optZoneSingleRpt"
        Me.optZoneSingleRpt.TabStop = True
        Me.ToolTip1.SetToolTip(Me.optZoneSingleRpt, resources.GetString("optZoneSingleRpt.ToolTip"))
        Me.optZoneSingleRpt.UseVisualStyleBackColor = False
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
        'cmdCompanySearch
        '
        Me.cmdCompanySearch.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCompanySearch.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdCompanySearch, "cmdCompanySearch")
        Me.cmdCompanySearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCompanySearch.Name = "cmdCompanySearch"
        Me.ToolTip1.SetToolTip(Me.cmdCompanySearch, resources.GetString("cmdCompanySearch.ToolTip"))
        Me.cmdCompanySearch.UseVisualStyleBackColor = False
        '
        'fraButtons
        '
        Me.fraButtons.BackColor = System.Drawing.SystemColors.Control
        Me.fraButtons.Controls.Add(Me.cmdExit)
        Me.fraButtons.Controls.Add(Me.cmdReport)
        Me.fraButtons.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.fraButtons, "fraButtons")
        Me.fraButtons.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraButtons.Name = "fraButtons"
        '
        'Frame2
        '
        Me.Frame2.BackColor = System.Drawing.SystemColors.Control
        Me.Frame2.Controls.Add(Me.optEachStore)
        Me.Frame2.Controls.Add(Me.cmbZoneSingleRpt)
        Me.Frame2.Controls.Add(Me.optZoneSingleRpt)
        Me.Frame2.Controls.Add(Me.optRegionSingleRpt)
        resources.ApplyResources(Me.Frame2, "Frame2")
        Me.Frame2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame2.Name = "Frame2"
        Me.Frame2.TabStop = False
        '
        'optEachStore
        '
        Me.optEachStore.BackColor = System.Drawing.SystemColors.Control
        Me.optEachStore.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.optEachStore, "optEachStore")
        Me.optEachStore.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optEachStore.Name = "optEachStore"
        Me.optEachStore.TabStop = True
        Me.optEachStore.UseVisualStyleBackColor = False
        '
        'cmbZoneSingleRpt
        '
        Me.cmbZoneSingleRpt.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbZoneSingleRpt.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbZoneSingleRpt.BackColor = System.Drawing.SystemColors.Window
        Me.cmbZoneSingleRpt.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbZoneSingleRpt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.cmbZoneSingleRpt, "cmbZoneSingleRpt")
        Me.cmbZoneSingleRpt.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbZoneSingleRpt.Name = "cmbZoneSingleRpt"
        Me.cmbZoneSingleRpt.Sorted = True
        '
        'optRegionSingleRpt
        '
        Me.optRegionSingleRpt.BackColor = System.Drawing.SystemColors.Control
        Me.optRegionSingleRpt.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.optRegionSingleRpt, "optRegionSingleRpt")
        Me.optRegionSingleRpt.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optRegionSingleRpt.Name = "optRegionSingleRpt"
        Me.optRegionSingleRpt.TabStop = True
        Me.optRegionSingleRpt.UseVisualStyleBackColor = False
        '
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me.HierarchySelector1)
        resources.ApplyResources(Me.Frame1, "Frame1")
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Name = "Frame1"
        Me.Frame1.TabStop = False
        '
        'HierarchySelector1
        '
        resources.ApplyResources(Me.HierarchySelector1, "HierarchySelector1")
        Me.HierarchySelector1.ItemIdentifier = Nothing
        Me.HierarchySelector1.Name = "HierarchySelector1"
        Me.HierarchySelector1.SelectedCategoryId = 0
        Me.HierarchySelector1.SelectedLevel3Id = 0
        Me.HierarchySelector1.SelectedLevel4Id = 0
        Me.HierarchySelector1.SelectedSubTeamId = 0
        '
        'cmbTeam
        '
        Me.cmbTeam.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbTeam.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbTeam.BackColor = System.Drawing.SystemColors.Window
        Me.cmbTeam.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbTeam.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.cmbTeam, "cmbTeam")
        Me.cmbTeam.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbTeam.Name = "cmbTeam"
        Me.cmbTeam.Sorted = True
        '
        'txtUPC
        '
        Me.txtUPC.AcceptsReturn = True
        Me.txtUPC.BackColor = System.Drawing.SystemColors.Window
        Me.txtUPC.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.txtUPC, "txtUPC")
        Me.txtUPC.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtUPC.Name = "txtUPC"
        '
        'cmbBrand
        '
        Me.cmbBrand.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbBrand.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbBrand.BackColor = System.Drawing.SystemColors.Window
        Me.cmbBrand.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmbBrand, "cmbBrand")
        Me.cmbBrand.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbBrand.Name = "cmbBrand"
        Me.cmbBrand.Sorted = True
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Name = "Label1"
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.SystemColors.Control
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.Label2, "Label2")
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Name = "Label2"
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.SystemColors.Control
        Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.Label3, "Label3")
        Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label3.Name = "Label3"
        '
        'fraStores
        '
        Me.fraStores.BackColor = System.Drawing.SystemColors.Control
        Me.fraStores.Controls.Add(Me.ugrdStoreList)
        Me.fraStores.Controls.Add(Me._optSelection_0)
        Me.fraStores.Controls.Add(Me._optSelection_1)
        Me.fraStores.Controls.Add(Me._optSelection_2)
        Me.fraStores.Controls.Add(Me.cmbZones)
        Me.fraStores.Controls.Add(Me._optSelection_3)
        Me.fraStores.Controls.Add(Me._optSelection_4)
        Me.fraStores.Controls.Add(Me.cmbStates)
        resources.ApplyResources(Me.fraStores, "fraStores")
        Me.fraStores.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraStores.Name = "fraStores"
        Me.fraStores.TabStop = False
        '
        'ugrdStoreList
        '
        Appearance1.BackColor = System.Drawing.SystemColors.ControlLight
        Appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.ugrdStoreList.DisplayLayout.Appearance = Appearance1
        Me.ugrdStoreList.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        UltraGridColumn1.Header.VisiblePosition = 0
        UltraGridColumn1.Hidden = True
        UltraGridColumn2.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn2.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        Appearance2.TextHAlign = Infragistics.Win.HAlign.Center
        UltraGridColumn2.Header.Appearance = Appearance2
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
        UltraGridBand1.Columns.AddRange(New Object() {UltraGridColumn1, UltraGridColumn2, UltraGridColumn3, UltraGridColumn4, UltraGridColumn5, UltraGridColumn6, UltraGridColumn7})
        UltraGridBand1.RowLayoutStyle = Infragistics.Win.UltraWinGrid.RowLayoutStyle.None
        Me.ugrdStoreList.DisplayLayout.BandsSerializer.Add(UltraGridBand1)
        Me.ugrdStoreList.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Appearance3.FontData.BoldAsString = resources.GetString("resource.BoldAsString")
        Me.ugrdStoreList.DisplayLayout.CaptionAppearance = Appearance3
        Me.ugrdStoreList.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Appearance4.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance4.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance4.BorderColor = System.Drawing.SystemColors.Window
        Me.ugrdStoreList.DisplayLayout.GroupByBox.Appearance = Appearance4
        Appearance5.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugrdStoreList.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance5
        Me.ugrdStoreList.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.ugrdStoreList.DisplayLayout.GroupByBox.Hidden = True
        Appearance6.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance6.BackColor2 = System.Drawing.SystemColors.Control
        Appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance6.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugrdStoreList.DisplayLayout.GroupByBox.PromptAppearance = Appearance6
        Me.ugrdStoreList.DisplayLayout.MaxColScrollRegions = 1
        Me.ugrdStoreList.DisplayLayout.MaxRowScrollRegions = 1
        Appearance7.BackColor = System.Drawing.SystemColors.Window
        Appearance7.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ugrdStoreList.DisplayLayout.Override.ActiveCellAppearance = Appearance7
        Appearance8.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugrdStoreList.DisplayLayout.Override.AddRowAppearance = Appearance8
        Me.ugrdStoreList.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.ugrdStoreList.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance9.BackColor = System.Drawing.SystemColors.Window
        Me.ugrdStoreList.DisplayLayout.Override.CardAreaAppearance = Appearance9
        Appearance10.BorderColor = System.Drawing.Color.Silver
        Appearance10.FontData.BoldAsString = resources.GetString("resource.BoldAsString1")
        Appearance10.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.ugrdStoreList.DisplayLayout.Override.CellAppearance = Appearance10
        Me.ugrdStoreList.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.ugrdStoreList.DisplayLayout.Override.CellPadding = 0
        Appearance11.FontData.BoldAsString = resources.GetString("resource.BoldAsString2")
        Me.ugrdStoreList.DisplayLayout.Override.FixedHeaderAppearance = Appearance11
        Appearance12.BackColor = System.Drawing.SystemColors.Control
        Appearance12.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance12.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance12.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance12.BorderColor = System.Drawing.SystemColors.Window
        Me.ugrdStoreList.DisplayLayout.Override.GroupByRowAppearance = Appearance12
        Appearance13.FontData.BoldAsString = resources.GetString("resource.BoldAsString3")
        Appearance13.TextHAlign = Infragistics.Win.HAlign.Left
        Me.ugrdStoreList.DisplayLayout.Override.HeaderAppearance = Appearance13
        Me.ugrdStoreList.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle
        Me.ugrdStoreList.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance14.BackColor = System.Drawing.SystemColors.Control
        Me.ugrdStoreList.DisplayLayout.Override.RowAlternateAppearance = Appearance14
        Appearance15.BackColor = System.Drawing.SystemColors.Window
        Appearance15.BorderColor = System.Drawing.Color.Silver
        Me.ugrdStoreList.DisplayLayout.Override.RowAppearance = Appearance15
        Me.ugrdStoreList.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[True]
        Me.ugrdStoreList.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed
        Me.ugrdStoreList.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.ugrdStoreList.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.ugrdStoreList.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Extended
        Appearance16.BackColor = System.Drawing.SystemColors.ControlLight
        Me.ugrdStoreList.DisplayLayout.Override.TemplateAddRowAppearance = Appearance16
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
        'txtVendorName
        '
        Me.txtVendorName.AcceptsReturn = True
        Me.txtVendorName.BackColor = System.Drawing.SystemColors.Window
        Me.txtVendorName.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.txtVendorName, "txtVendorName")
        Me.txtVendorName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtVendorName.Name = "txtVendorName"
        '
        'lblVendor
        '
        Me.lblVendor.BackColor = System.Drawing.SystemColors.Control
        Me.lblVendor.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblVendor, "lblVendor")
        Me.lblVendor.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblVendor.Name = "lblVendor"
        '
        'optSelection
        '
        '
        'frmVendorReports
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.Controls.Add(Me.fraButtons)
        Me.Controls.Add(Me.Frame2)
        Me.Controls.Add(Me.Frame1)
        Me.Controls.Add(Me.fraStores)
        Me.Controls.Add(Me.cmbTeam)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtVendorName)
        Me.Controls.Add(Me.txtUPC)
        Me.Controls.Add(Me.cmdCompanySearch)
        Me.Controls.Add(Me.cmbBrand)
        Me.Controls.Add(Me.lblVendor)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label3)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmVendorReports"
        Me.ShowInTaskbar = False
        Me.fraButtons.ResumeLayout(False)
        Me.Frame2.ResumeLayout(False)
        Me.Frame1.ResumeLayout(False)
        Me.fraStores.ResumeLayout(False)
        CType(Me.ugrdStoreList, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.optSelection, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
#End Region

    Private Sub frmVendorReports_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

    End Sub
    Friend WithEvents ugrdStoreList As Infragistics.Win.UltraWinGrid.UltraGrid
    Friend WithEvents HierarchySelector1 As HierarchySelector
End Class