<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmPricingBatchItemSearch
#Region "Windows Form Designer generated code "
	<System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
        MyBase.New()

        isinitializing = True

		'This call is required by the Windows Form Designer.
		InitializeComponent()

        isinitializing = False

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
    Public WithEvents cmdSearch As System.Windows.Forms.Button
	Public WithEvents _optSelection_4 As System.Windows.Forms.RadioButton
	Public WithEvents _optSelection_3 As System.Windows.Forms.RadioButton
	Public WithEvents cmbZones As System.Windows.Forms.ComboBox
	Public WithEvents _optSelection_1 As System.Windows.Forms.RadioButton
	Public WithEvents _optSelection_5 As System.Windows.Forms.RadioButton
    Public WithEvents _optSelection_0 As System.Windows.Forms.RadioButton
	Public WithEvents _optSelection_2 As System.Windows.Forms.RadioButton
	Public WithEvents cmbState As System.Windows.Forms.ComboBox
	Public WithEvents fraStores As System.Windows.Forms.GroupBox
    Public WithEvents fraPriceType As System.Windows.Forms.GroupBox
	Public WithEvents _StatusBar1_Panel1 As System.Windows.Forms.ToolStripStatusLabel
	Public WithEvents _StatusBar1_Panel2 As System.Windows.Forms.ToolStripStatusLabel
	Public WithEvents StatusBar1 As System.Windows.Forms.StatusStrip
	Public WithEvents cmdExit As System.Windows.Forms.Button
	Public WithEvents txtIdentifier As System.Windows.Forms.TextBox
	Public WithEvents txtDescription As System.Windows.Forms.TextBox
	Public WithEvents _optType_0 As System.Windows.Forms.RadioButton
	Public WithEvents _optType_1 As System.Windows.Forms.RadioButton
	Public WithEvents _optType_2 As System.Windows.Forms.RadioButton
    Public WithEvents _optType_3 As System.Windows.Forms.RadioButton
    Public WithEvents _optType_4 As System.Windows.Forms.RadioButton
	Public WithEvents fraType As System.Windows.Forms.GroupBox
    Public WithEvents lblDates As System.Windows.Forms.Label
    Public WithEvents _lblLabel_1 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_5 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_2 As System.Windows.Forms.Label
	Public WithEvents lblLabel As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	Public WithEvents optSelection As Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray
    Public WithEvents optType As Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPricingBatchItemSearch))
		Dim Appearance15 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
		Dim Appearance16 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
		Dim Appearance18 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
		Dim Appearance17 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
		Dim Appearance21 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
		Dim Appearance24 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
		Dim Appearance26 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
		Dim Appearance22 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
		Dim Appearance20 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
		Dim Appearance19 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
		Dim Appearance25 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
		Dim Appearance23 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
		Dim Appearance1 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
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
		Dim Appearance14 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
		Dim UltraDataColumn10 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Store_name")
		Dim UltraDataColumn11 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Store_No")
		Dim UltraDataColumn12 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("PriceBatchDetailID")
		Dim UltraDataColumn13 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Identifier")
		Dim UltraDataColumn14 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Brand_Name")
		Dim UltraDataColumn15 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Item_Description")
		Dim UltraDataColumn16 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("StartDate")
		Dim UltraDataColumn17 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Sale_End_Date")
		Dim UltraDataColumn18 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Selected")
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cmdSearch = New System.Windows.Forms.Button()
		Me.cmdExit = New System.Windows.Forms.Button()
		Me.cmdSelect = New System.Windows.Forms.Button()
		Me.cmdBERTReport = New System.Windows.Forms.Button()
		Me.fraStores = New System.Windows.Forms.GroupBox()
		Me.ucmbStoreList = New Infragistics.Win.UltraWinGrid.UltraCombo()
		Me._optSelection_4 = New System.Windows.Forms.RadioButton()
		Me._optSelection_3 = New System.Windows.Forms.RadioButton()
		Me.cmbZones = New System.Windows.Forms.ComboBox()
		Me._optSelection_1 = New System.Windows.Forms.RadioButton()
		Me._optSelection_5 = New System.Windows.Forms.RadioButton()
		Me._optSelection_0 = New System.Windows.Forms.RadioButton()
		Me._optSelection_2 = New System.Windows.Forms.RadioButton()
		Me.cmbState = New System.Windows.Forms.ComboBox()
		Me.fraPriceType = New System.Windows.Forms.GroupBox()
		Me.cmbPriceType = New System.Windows.Forms.ComboBox()
		Me.StatusBar1 = New System.Windows.Forms.StatusStrip()
		Me._StatusBar1_Panel1 = New System.Windows.Forms.ToolStripStatusLabel()
		Me._StatusBar1_Panel2 = New System.Windows.Forms.ToolStripStatusLabel()
		Me.txtIdentifier = New System.Windows.Forms.TextBox()
		Me.txtDescription = New System.Windows.Forms.TextBox()
		Me.fraType = New System.Windows.Forms.GroupBox()
		Me._optType_5 = New System.Windows.Forms.RadioButton()
		Me._optType_4 = New System.Windows.Forms.RadioButton()
		Me._optType_0 = New System.Windows.Forms.RadioButton()
		Me._optType_1 = New System.Windows.Forms.RadioButton()
		Me._optType_2 = New System.Windows.Forms.RadioButton()
		Me._optType_3 = New System.Windows.Forms.RadioButton()
		Me.lblDates = New System.Windows.Forms.Label()
		Me._lblLabel_1 = New System.Windows.Forms.Label()
		Me._lblLabel_5 = New System.Windows.Forms.Label()
		Me._lblLabel_2 = New System.Windows.Forms.Label()
		Me.lblLabel = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(Me.components)
		Me.optSelection = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(Me.components)
		Me.optType = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(Me.components)
		Me.ugrdList = New Infragistics.Win.UltraWinGrid.UltraGrid()
		Me.UltraDataSource1 = New Infragistics.Win.UltraWinDataSource.UltraDataSource(Me.components)
		Me.HeaderFrame = New System.Windows.Forms.GroupBox()
		Me.BatchDescriptionTextBox = New System.Windows.Forms.TextBox()
		Me.BatchDescLabel = New System.Windows.Forms.Label()
		Me.AutoApplyDateUDTE = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor()
		Me.ApplyDateLabel = New System.Windows.Forms.Label()
		Me.AutoApplyCheckBox = New System.Windows.Forms.CheckBox()
		Me.txtNumBatch = New System.Windows.Forms.TextBox()
		Me._lblLabel_7 = New System.Windows.Forms.Label()
		Me.dtpStartDate = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor()
		Me.chkIncScaleItems = New System.Windows.Forms.CheckBox()
		Me.chkSelectAll = New System.Windows.Forms.CheckBox()
		Me.Label1 = New System.Windows.Forms.Label()
		Me.chkIncludeNonRetailItems = New System.Windows.Forms.CheckBox()
		Me.cmbSubTeam = New SubteamComboBox()
		Me.fraStores.SuspendLayout()
		CType(Me.ucmbStoreList, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.fraPriceType.SuspendLayout()
		Me.StatusBar1.SuspendLayout()
		Me.fraType.SuspendLayout()
		CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.optSelection, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.optType, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.ugrdList, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.UltraDataSource1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.HeaderFrame.SuspendLayout()
		CType(Me.AutoApplyDateUDTE, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.dtpStartDate, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'cmdSearch
		'
		resources.ApplyResources(Me.cmdSearch, "cmdSearch")
		Me.cmdSearch.BackColor = System.Drawing.SystemColors.Control
		Me.cmdSearch.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdSearch.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdSearch.Name = "cmdSearch"
		Me.ToolTip1.SetToolTip(Me.cmdSearch, resources.GetString("cmdSearch.ToolTip"))
		Me.cmdSearch.UseVisualStyleBackColor = False
		'
		'cmdExit
		'
		resources.ApplyResources(Me.cmdExit, "cmdExit")
		Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
		Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
		Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdExit.Name = "cmdExit"
		Me.ToolTip1.SetToolTip(Me.cmdExit, resources.GetString("cmdExit.ToolTip"))
		Me.cmdExit.UseVisualStyleBackColor = False
		'
		'cmdSelect
		'
		resources.ApplyResources(Me.cmdSelect, "cmdSelect")
		Me.cmdSelect.BackColor = System.Drawing.SystemColors.Control
		Me.cmdSelect.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdSelect.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdSelect.Name = "cmdSelect"
		Me.ToolTip1.SetToolTip(Me.cmdSelect, resources.GetString("cmdSelect.ToolTip"))
		Me.cmdSelect.UseVisualStyleBackColor = False
		'
		'cmdBERTReport
		'
		resources.ApplyResources(Me.cmdBERTReport, "cmdBERTReport")
		Me.cmdBERTReport.BackColor = System.Drawing.SystemColors.Control
		Me.cmdBERTReport.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdBERTReport.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdBERTReport.Name = "cmdBERTReport"
		Me.ToolTip1.SetToolTip(Me.cmdBERTReport, resources.GetString("cmdBERTReport.ToolTip"))
		Me.cmdBERTReport.UseVisualStyleBackColor = False
		'
		'fraStores
		'
		Me.fraStores.BackColor = System.Drawing.SystemColors.Control
		Me.fraStores.Controls.Add(Me.ucmbStoreList)
		Me.fraStores.Controls.Add(Me._optSelection_4)
		Me.fraStores.Controls.Add(Me._optSelection_3)
		Me.fraStores.Controls.Add(Me.cmbZones)
		Me.fraStores.Controls.Add(Me._optSelection_1)
		Me.fraStores.Controls.Add(Me._optSelection_5)
		Me.fraStores.Controls.Add(Me._optSelection_0)
		Me.fraStores.Controls.Add(Me._optSelection_2)
		Me.fraStores.Controls.Add(Me.cmbState)
		resources.ApplyResources(Me.fraStores, "fraStores")
		Me.fraStores.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraStores.Name = "fraStores"
		Me.fraStores.TabStop = False
		'
		'ucmbStoreList
		'
		Me.ucmbStoreList.CheckedListSettings.CheckStateMember = ""
		Appearance15.BackColor = System.Drawing.SystemColors.Window
		Appearance15.BorderColor = System.Drawing.SystemColors.InactiveCaption
		resources.ApplyResources(Appearance15.FontData, "Appearance15.FontData")
		resources.ApplyResources(Appearance15, "Appearance15")
		Appearance15.ForceApplyResources = "FontData|"
		Me.ucmbStoreList.DisplayLayout.Appearance = Appearance15
		Me.ucmbStoreList.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
		Me.ucmbStoreList.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
		Appearance16.BackColor = System.Drawing.SystemColors.ActiveBorder
		Appearance16.BackColor2 = System.Drawing.SystemColors.ControlDark
		Appearance16.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
		Appearance16.BorderColor = System.Drawing.SystemColors.Window
		resources.ApplyResources(Appearance16.FontData, "Appearance16.FontData")
		resources.ApplyResources(Appearance16, "Appearance16")
		Appearance16.ForceApplyResources = "FontData|"
		Me.ucmbStoreList.DisplayLayout.GroupByBox.Appearance = Appearance16
		Appearance18.ForeColor = System.Drawing.SystemColors.GrayText
		resources.ApplyResources(Appearance18.FontData, "Appearance18.FontData")
		resources.ApplyResources(Appearance18, "Appearance18")
		Appearance18.ForceApplyResources = "FontData|"
		Me.ucmbStoreList.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance18
		Me.ucmbStoreList.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
		Appearance17.BackColor = System.Drawing.SystemColors.ControlLightLight
		Appearance17.BackColor2 = System.Drawing.SystemColors.Control
		Appearance17.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
		Appearance17.ForeColor = System.Drawing.SystemColors.GrayText
		resources.ApplyResources(Appearance17.FontData, "Appearance17.FontData")
		resources.ApplyResources(Appearance17, "Appearance17")
		Appearance17.ForceApplyResources = "FontData|"
		Me.ucmbStoreList.DisplayLayout.GroupByBox.PromptAppearance = Appearance17
		Me.ucmbStoreList.DisplayLayout.MaxColScrollRegions = 1
		Me.ucmbStoreList.DisplayLayout.MaxRowScrollRegions = 1
		Appearance21.BackColor = System.Drawing.SystemColors.Window
		Appearance21.ForeColor = System.Drawing.SystemColors.ControlText
		resources.ApplyResources(Appearance21.FontData, "Appearance21.FontData")
		resources.ApplyResources(Appearance21, "Appearance21")
		Appearance21.ForceApplyResources = "FontData|"
		Me.ucmbStoreList.DisplayLayout.Override.ActiveCellAppearance = Appearance21
		Appearance24.BackColor = System.Drawing.SystemColors.Highlight
		Appearance24.ForeColor = System.Drawing.SystemColors.HighlightText
		resources.ApplyResources(Appearance24.FontData, "Appearance24.FontData")
		resources.ApplyResources(Appearance24, "Appearance24")
		Appearance24.ForceApplyResources = "FontData|"
		Me.ucmbStoreList.DisplayLayout.Override.ActiveRowAppearance = Appearance24
		Me.ucmbStoreList.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
		Me.ucmbStoreList.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
		Appearance26.BackColor = System.Drawing.SystemColors.Window
		resources.ApplyResources(Appearance26.FontData, "Appearance26.FontData")
		resources.ApplyResources(Appearance26, "Appearance26")
		Appearance26.ForceApplyResources = "FontData|"
		Me.ucmbStoreList.DisplayLayout.Override.CardAreaAppearance = Appearance26
		Appearance22.BorderColor = System.Drawing.Color.Silver
		Appearance22.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
		resources.ApplyResources(Appearance22.FontData, "Appearance22.FontData")
		resources.ApplyResources(Appearance22, "Appearance22")
		Appearance22.ForceApplyResources = "FontData|"
		Me.ucmbStoreList.DisplayLayout.Override.CellAppearance = Appearance22
		Me.ucmbStoreList.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
		Me.ucmbStoreList.DisplayLayout.Override.CellPadding = 0
		Appearance20.BackColor = System.Drawing.SystemColors.Control
		Appearance20.BackColor2 = System.Drawing.SystemColors.ControlDark
		Appearance20.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
		Appearance20.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
		Appearance20.BorderColor = System.Drawing.SystemColors.Window
		resources.ApplyResources(Appearance20.FontData, "Appearance20.FontData")
		resources.ApplyResources(Appearance20, "Appearance20")
		Appearance20.ForceApplyResources = "FontData|"
		Me.ucmbStoreList.DisplayLayout.Override.GroupByRowAppearance = Appearance20
		resources.ApplyResources(Appearance19, "Appearance19")
		resources.ApplyResources(Appearance19.FontData, "Appearance19.FontData")
		Appearance19.ForceApplyResources = "FontData|"
		Me.ucmbStoreList.DisplayLayout.Override.HeaderAppearance = Appearance19
		Me.ucmbStoreList.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti
		Me.ucmbStoreList.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
		Appearance25.BackColor = System.Drawing.SystemColors.Window
		Appearance25.BorderColor = System.Drawing.Color.Silver
		resources.ApplyResources(Appearance25.FontData, "Appearance25.FontData")
		resources.ApplyResources(Appearance25, "Appearance25")
		Appearance25.ForceApplyResources = "FontData|"
		Me.ucmbStoreList.DisplayLayout.Override.RowAppearance = Appearance25
		Me.ucmbStoreList.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[False]
		Appearance23.BackColor = System.Drawing.SystemColors.ControlLight
		resources.ApplyResources(Appearance23.FontData, "Appearance23.FontData")
		resources.ApplyResources(Appearance23, "Appearance23")
		Appearance23.ForceApplyResources = "FontData|"
		Me.ucmbStoreList.DisplayLayout.Override.TemplateAddRowAppearance = Appearance23
		Me.ucmbStoreList.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
		Me.ucmbStoreList.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
		Me.ucmbStoreList.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
		resources.ApplyResources(Me.ucmbStoreList, "ucmbStoreList")
		Me.ucmbStoreList.Name = "ucmbStoreList"
		'
		'_optSelection_4
		'
		Me._optSelection_4.BackColor = System.Drawing.SystemColors.Control
		Me._optSelection_4.Cursor = System.Windows.Forms.Cursors.Default
		resources.ApplyResources(Me._optSelection_4, "_optSelection_4")
		Me._optSelection_4.ForeColor = System.Drawing.SystemColors.ControlText
		Me.optSelection.SetIndex(Me._optSelection_4, CType(4, Short))
		Me._optSelection_4.Name = "_optSelection_4"
		Me._optSelection_4.UseVisualStyleBackColor = False
		'
		'_optSelection_3
		'
		Me._optSelection_3.BackColor = System.Drawing.SystemColors.Control
		Me._optSelection_3.Cursor = System.Windows.Forms.Cursors.Default
		resources.ApplyResources(Me._optSelection_3, "_optSelection_3")
		Me._optSelection_3.ForeColor = System.Drawing.SystemColors.ControlText
		Me.optSelection.SetIndex(Me._optSelection_3, CType(3, Short))
		Me._optSelection_3.Name = "_optSelection_3"
		Me._optSelection_3.UseVisualStyleBackColor = False
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
		'_optSelection_1
		'
		Me._optSelection_1.BackColor = System.Drawing.SystemColors.Control
		Me._optSelection_1.Cursor = System.Windows.Forms.Cursors.Default
		resources.ApplyResources(Me._optSelection_1, "_optSelection_1")
		Me._optSelection_1.ForeColor = System.Drawing.SystemColors.ControlText
		Me.optSelection.SetIndex(Me._optSelection_1, CType(1, Short))
		Me._optSelection_1.Name = "_optSelection_1"
		Me._optSelection_1.UseVisualStyleBackColor = False
		'
		'_optSelection_5
		'
		Me._optSelection_5.BackColor = System.Drawing.SystemColors.Control
		Me._optSelection_5.Checked = True
		Me._optSelection_5.Cursor = System.Windows.Forms.Cursors.Default
		resources.ApplyResources(Me._optSelection_5, "_optSelection_5")
		Me._optSelection_5.ForeColor = System.Drawing.SystemColors.ControlText
		Me.optSelection.SetIndex(Me._optSelection_5, CType(5, Short))
		Me._optSelection_5.Name = "_optSelection_5"
		Me._optSelection_5.TabStop = True
		Me._optSelection_5.UseVisualStyleBackColor = False
		'
		'_optSelection_0
		'
		Me._optSelection_0.BackColor = System.Drawing.SystemColors.Control
		Me._optSelection_0.Cursor = System.Windows.Forms.Cursors.Default
		resources.ApplyResources(Me._optSelection_0, "_optSelection_0")
		Me._optSelection_0.ForeColor = System.Drawing.SystemColors.ControlText
		Me.optSelection.SetIndex(Me._optSelection_0, CType(0, Short))
		Me._optSelection_0.Name = "_optSelection_0"
		Me._optSelection_0.UseVisualStyleBackColor = False
		'
		'_optSelection_2
		'
		Me._optSelection_2.BackColor = System.Drawing.SystemColors.Control
		Me._optSelection_2.Cursor = System.Windows.Forms.Cursors.Default
		resources.ApplyResources(Me._optSelection_2, "_optSelection_2")
		Me._optSelection_2.ForeColor = System.Drawing.SystemColors.ControlText
		Me.optSelection.SetIndex(Me._optSelection_2, CType(2, Short))
		Me._optSelection_2.Name = "_optSelection_2"
		Me._optSelection_2.UseVisualStyleBackColor = False
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
		'fraPriceType
		'
		resources.ApplyResources(Me.fraPriceType, "fraPriceType")
		Me.fraPriceType.BackColor = System.Drawing.SystemColors.Control
		Me.fraPriceType.Controls.Add(Me.cmbPriceType)
		Me.fraPriceType.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraPriceType.Name = "fraPriceType"
		Me.fraPriceType.TabStop = False
		'
		'cmbPriceType
		'
		Me.cmbPriceType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
		Me.cmbPriceType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
		Me.cmbPriceType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		resources.ApplyResources(Me.cmbPriceType, "cmbPriceType")
		Me.cmbPriceType.Name = "cmbPriceType"
		'
		'StatusBar1
		'
		resources.ApplyResources(Me.StatusBar1, "StatusBar1")
		Me.StatusBar1.ImageScalingSize = New System.Drawing.Size(20, 20)
		Me.StatusBar1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._StatusBar1_Panel1, Me._StatusBar1_Panel2})
		Me.StatusBar1.Name = "StatusBar1"
		'
		'_StatusBar1_Panel1
		'
		resources.ApplyResources(Me._StatusBar1_Panel1, "_StatusBar1_Panel1")
		Me._StatusBar1_Panel1.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
			Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
			Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
		Me._StatusBar1_Panel1.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
		Me._StatusBar1_Panel1.Margin = New System.Windows.Forms.Padding(0)
		Me._StatusBar1_Panel1.Name = "_StatusBar1_Panel1"
		'
		'_StatusBar1_Panel2
		'
		resources.ApplyResources(Me._StatusBar1_Panel2, "_StatusBar1_Panel2")
		Me._StatusBar1_Panel2.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
			Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
			Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
		Me._StatusBar1_Panel2.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
		Me._StatusBar1_Panel2.Margin = New System.Windows.Forms.Padding(0)
		Me._StatusBar1_Panel2.Name = "_StatusBar1_Panel2"
		'
		'txtIdentifier
		'
		Me.txtIdentifier.AcceptsReturn = True
		Me.txtIdentifier.BackColor = System.Drawing.SystemColors.Window
		Me.txtIdentifier.Cursor = System.Windows.Forms.Cursors.IBeam
		resources.ApplyResources(Me.txtIdentifier, "txtIdentifier")
		Me.txtIdentifier.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtIdentifier.Name = "txtIdentifier"
		Me.txtIdentifier.Tag = "String"
		'
		'txtDescription
		'
		Me.txtDescription.AcceptsReturn = True
		resources.ApplyResources(Me.txtDescription, "txtDescription")
		Me.txtDescription.BackColor = System.Drawing.SystemColors.Window
		Me.txtDescription.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtDescription.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtDescription.Name = "txtDescription"
		Me.txtDescription.Tag = "String"
		'
		'fraType
		'
		resources.ApplyResources(Me.fraType, "fraType")
		Me.fraType.BackColor = System.Drawing.SystemColors.Control
		Me.fraType.Controls.Add(Me._optType_5)
		Me.fraType.Controls.Add(Me._optType_4)
		Me.fraType.Controls.Add(Me._optType_0)
		Me.fraType.Controls.Add(Me._optType_1)
		Me.fraType.Controls.Add(Me._optType_2)
		Me.fraType.Controls.Add(Me._optType_3)
		Me.fraType.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraType.Name = "fraType"
		Me.fraType.TabStop = False
		'
		'_optType_5
		'
		Me._optType_5.BackColor = System.Drawing.SystemColors.Control
		Me._optType_5.Cursor = System.Windows.Forms.Cursors.Default
		resources.ApplyResources(Me._optType_5, "_optType_5")
		Me._optType_5.ForeColor = System.Drawing.SystemColors.ControlText
		Me.optType.SetIndex(Me._optType_5, CType(5, Short))
		Me._optType_5.Name = "_optType_5"
		Me._optType_5.UseVisualStyleBackColor = False
		'
		'_optType_4
		'
		Me._optType_4.BackColor = System.Drawing.SystemColors.Control
		Me._optType_4.Cursor = System.Windows.Forms.Cursors.Default
		resources.ApplyResources(Me._optType_4, "_optType_4")
		Me._optType_4.ForeColor = System.Drawing.SystemColors.ControlText
		Me.optType.SetIndex(Me._optType_4, CType(4, Short))
		Me._optType_4.Name = "_optType_4"
		Me._optType_4.UseVisualStyleBackColor = False
		'
		'_optType_0
		'
		Me._optType_0.BackColor = System.Drawing.SystemColors.Control
		Me._optType_0.Cursor = System.Windows.Forms.Cursors.Default
		resources.ApplyResources(Me._optType_0, "_optType_0")
		Me._optType_0.ForeColor = System.Drawing.SystemColors.ControlText
		Me.optType.SetIndex(Me._optType_0, CType(0, Short))
		Me._optType_0.Name = "_optType_0"
		Me._optType_0.UseVisualStyleBackColor = False
		'
		'_optType_1
		'
		Me._optType_1.BackColor = System.Drawing.SystemColors.Control
		Me._optType_1.Cursor = System.Windows.Forms.Cursors.Default
		resources.ApplyResources(Me._optType_1, "_optType_1")
		Me._optType_1.ForeColor = System.Drawing.SystemColors.ControlText
		Me.optType.SetIndex(Me._optType_1, CType(1, Short))
		Me._optType_1.Name = "_optType_1"
		Me._optType_1.UseVisualStyleBackColor = False
		'
		'_optType_2
		'
		Me._optType_2.BackColor = System.Drawing.SystemColors.Control
		Me._optType_2.Cursor = System.Windows.Forms.Cursors.Default
		resources.ApplyResources(Me._optType_2, "_optType_2")
		Me._optType_2.ForeColor = System.Drawing.SystemColors.ControlText
		Me.optType.SetIndex(Me._optType_2, CType(2, Short))
		Me._optType_2.Name = "_optType_2"
		Me._optType_2.UseVisualStyleBackColor = False
		'
		'_optType_3
		'
		Me._optType_3.BackColor = System.Drawing.SystemColors.Control
		Me._optType_3.Checked = True
		Me._optType_3.Cursor = System.Windows.Forms.Cursors.Default
		resources.ApplyResources(Me._optType_3, "_optType_3")
		Me._optType_3.ForeColor = System.Drawing.SystemColors.ControlText
		Me.optType.SetIndex(Me._optType_3, CType(3, Short))
		Me._optType_3.Name = "_optType_3"
		Me._optType_3.TabStop = True
		Me._optType_3.UseVisualStyleBackColor = False
		'
		'lblDates
		'
		resources.ApplyResources(Me.lblDates, "lblDates")
		Me.lblDates.BackColor = System.Drawing.Color.Transparent
		Me.lblDates.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblDates.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblDates.Name = "lblDates"
		'
		'_lblLabel_1
		'
		Me._lblLabel_1.BackColor = System.Drawing.Color.Transparent
		Me._lblLabel_1.Cursor = System.Windows.Forms.Cursors.Default
		resources.ApplyResources(Me._lblLabel_1, "_lblLabel_1")
		Me._lblLabel_1.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblLabel.SetIndex(Me._lblLabel_1, CType(1, Short))
		Me._lblLabel_1.Name = "_lblLabel_1"
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
		'_lblLabel_2
		'
		resources.ApplyResources(Me._lblLabel_2, "_lblLabel_2")
		Me._lblLabel_2.BackColor = System.Drawing.Color.Transparent
		Me._lblLabel_2.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblLabel_2.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblLabel.SetIndex(Me._lblLabel_2, CType(2, Short))
		Me._lblLabel_2.Name = "_lblLabel_2"
		'
		'optSelection
		'
		'
		'optType
		'
		'
		'ugrdList
		'
		resources.ApplyResources(Me.ugrdList, "ugrdList")
		Appearance1.BackColor = System.Drawing.SystemColors.ControlLight
		Appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption
		resources.ApplyResources(Appearance1.FontData, "Appearance1.FontData")
		resources.ApplyResources(Appearance1, "Appearance1")
		Appearance1.ForceApplyResources = "FontData|"
		Me.ugrdList.DisplayLayout.Appearance = Appearance1
		Me.ugrdList.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn
		Me.ugrdList.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
		Appearance2.FontData.BoldAsString = resources.GetString("resource.BoldAsString")
		resources.ApplyResources(Appearance2, "Appearance2")
		Appearance2.ForceApplyResources = ""
		Me.ugrdList.DisplayLayout.CaptionAppearance = Appearance2
		Me.ugrdList.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[True]
		Appearance3.BackColor = System.Drawing.SystemColors.ActiveBorder
		Appearance3.BackColor2 = System.Drawing.SystemColors.ControlDark
		Appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
		Appearance3.BorderColor = System.Drawing.SystemColors.Window
		resources.ApplyResources(Appearance3.FontData, "Appearance3.FontData")
		resources.ApplyResources(Appearance3, "Appearance3")
		Appearance3.ForceApplyResources = "FontData|"
		Me.ugrdList.DisplayLayout.GroupByBox.Appearance = Appearance3
		Appearance4.ForeColor = System.Drawing.SystemColors.GrayText
		resources.ApplyResources(Appearance4.FontData, "Appearance4.FontData")
		resources.ApplyResources(Appearance4, "Appearance4")
		Appearance4.ForceApplyResources = "FontData|"
		Me.ugrdList.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance4
		Me.ugrdList.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
		Me.ugrdList.DisplayLayout.GroupByBox.Hidden = True
		Appearance5.BackColor = System.Drawing.SystemColors.ControlLightLight
		Appearance5.BackColor2 = System.Drawing.SystemColors.Control
		Appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
		Appearance5.ForeColor = System.Drawing.SystemColors.GrayText
		resources.ApplyResources(Appearance5.FontData, "Appearance5.FontData")
		resources.ApplyResources(Appearance5, "Appearance5")
		Appearance5.ForceApplyResources = "FontData|"
		Me.ugrdList.DisplayLayout.GroupByBox.PromptAppearance = Appearance5
		Me.ugrdList.DisplayLayout.MaxColScrollRegions = 1
		Me.ugrdList.DisplayLayout.MaxRowScrollRegions = 1
		Appearance6.BackColor = System.Drawing.SystemColors.Window
		Appearance6.ForeColor = System.Drawing.SystemColors.ControlText
		resources.ApplyResources(Appearance6.FontData, "Appearance6.FontData")
		resources.ApplyResources(Appearance6, "Appearance6")
		Appearance6.ForceApplyResources = "FontData|"
		Me.ugrdList.DisplayLayout.Override.ActiveCellAppearance = Appearance6
		Me.ugrdList.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
		Me.ugrdList.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
		Appearance7.BackColor = System.Drawing.SystemColors.Window
		resources.ApplyResources(Appearance7.FontData, "Appearance7.FontData")
		resources.ApplyResources(Appearance7, "Appearance7")
		Appearance7.ForceApplyResources = "FontData|"
		Me.ugrdList.DisplayLayout.Override.CardAreaAppearance = Appearance7
		Appearance8.BorderColor = System.Drawing.Color.Silver
		Appearance8.FontData.BoldAsString = resources.GetString("resource.BoldAsString1")
		Appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
		resources.ApplyResources(Appearance8, "Appearance8")
		Appearance8.ForceApplyResources = ""
		Me.ugrdList.DisplayLayout.Override.CellAppearance = Appearance8
		Me.ugrdList.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
		Me.ugrdList.DisplayLayout.Override.CellPadding = 0
		Appearance9.FontData.BoldAsString = resources.GetString("resource.BoldAsString2")
		resources.ApplyResources(Appearance9, "Appearance9")
		Appearance9.ForceApplyResources = ""
		Me.ugrdList.DisplayLayout.Override.FixedHeaderAppearance = Appearance9
		Appearance10.BackColor = System.Drawing.SystemColors.Control
		Appearance10.BackColor2 = System.Drawing.SystemColors.ControlDark
		Appearance10.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
		Appearance10.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
		Appearance10.BorderColor = System.Drawing.SystemColors.Window
		resources.ApplyResources(Appearance10.FontData, "Appearance10.FontData")
		resources.ApplyResources(Appearance10, "Appearance10")
		Appearance10.ForceApplyResources = "FontData|"
		Me.ugrdList.DisplayLayout.Override.GroupByRowAppearance = Appearance10
		Appearance11.FontData.BoldAsString = resources.GetString("resource.BoldAsString3")
		resources.ApplyResources(Appearance11, "Appearance11")
		Appearance11.ForceApplyResources = ""
		Me.ugrdList.DisplayLayout.Override.HeaderAppearance = Appearance11
		Me.ugrdList.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle
		Me.ugrdList.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
		Me.ugrdList.DisplayLayout.Override.MinRowHeight = 1
		Appearance12.BackColor = System.Drawing.SystemColors.ControlLight
		resources.ApplyResources(Appearance12.FontData, "Appearance12.FontData")
		resources.ApplyResources(Appearance12, "Appearance12")
		Appearance12.ForceApplyResources = "FontData|"
		Me.ugrdList.DisplayLayout.Override.RowAlternateAppearance = Appearance12
		Appearance13.BackColor = System.Drawing.SystemColors.Window
		Appearance13.BorderColor = System.Drawing.Color.Silver
		resources.ApplyResources(Appearance13.FontData, "Appearance13.FontData")
		resources.ApplyResources(Appearance13, "Appearance13")
		Appearance13.ForceApplyResources = "FontData|"
		Me.ugrdList.DisplayLayout.Override.RowAppearance = Appearance13
		Me.ugrdList.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[True]
		Me.ugrdList.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed
		Me.ugrdList.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None
		Me.ugrdList.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None
		Me.ugrdList.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Extended
		Appearance14.BackColor = System.Drawing.SystemColors.ControlLight
		resources.ApplyResources(Appearance14.FontData, "Appearance14.FontData")
		resources.ApplyResources(Appearance14, "Appearance14")
		Appearance14.ForceApplyResources = "FontData|"
		Me.ugrdList.DisplayLayout.Override.TemplateAddRowAppearance = Appearance14
		Me.ugrdList.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
		Me.ugrdList.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
		Me.ugrdList.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand
		Me.ugrdList.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
		Me.ugrdList.Name = "ugrdList"
		'
		'UltraDataSource1
		'
		UltraDataColumn16.DataType = GetType(Date)
		UltraDataColumn17.DataType = GetType(Date)
		Me.UltraDataSource1.Band.Columns.AddRange(New Object() {UltraDataColumn10, UltraDataColumn11, UltraDataColumn12, UltraDataColumn13, UltraDataColumn14, UltraDataColumn15, UltraDataColumn16, UltraDataColumn17, UltraDataColumn18})
		'
		'HeaderFrame
		'
		resources.ApplyResources(Me.HeaderFrame, "HeaderFrame")
		Me.HeaderFrame.Controls.Add(Me.BatchDescriptionTextBox)
		Me.HeaderFrame.Controls.Add(Me.BatchDescLabel)
		Me.HeaderFrame.Controls.Add(Me.AutoApplyDateUDTE)
		Me.HeaderFrame.Controls.Add(Me.ApplyDateLabel)
		Me.HeaderFrame.Controls.Add(Me.AutoApplyCheckBox)
		Me.HeaderFrame.ForeColor = System.Drawing.SystemColors.ControlText
		Me.HeaderFrame.Name = "HeaderFrame"
		Me.HeaderFrame.TabStop = False
		'
		'BatchDescriptionTextBox
		'
		Me.BatchDescriptionTextBox.BackColor = System.Drawing.SystemColors.Window
		Me.BatchDescriptionTextBox.Cursor = System.Windows.Forms.Cursors.IBeam
		resources.ApplyResources(Me.BatchDescriptionTextBox, "BatchDescriptionTextBox")
		Me.BatchDescriptionTextBox.ForeColor = System.Drawing.SystemColors.WindowText
		Me.BatchDescriptionTextBox.Name = "BatchDescriptionTextBox"
		Me.BatchDescriptionTextBox.Tag = "String"
		'
		'BatchDescLabel
		'
		Me.BatchDescLabel.BackColor = System.Drawing.Color.Transparent
		Me.BatchDescLabel.Cursor = System.Windows.Forms.Cursors.Default
		resources.ApplyResources(Me.BatchDescLabel, "BatchDescLabel")
		Me.BatchDescLabel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.BatchDescLabel.Name = "BatchDescLabel"
		'
		'AutoApplyDateUDTE
		'
		resources.ApplyResources(Me.AutoApplyDateUDTE, "AutoApplyDateUDTE")
		Me.AutoApplyDateUDTE.MaskInput = ""
		Me.AutoApplyDateUDTE.MaxDate = New Date(2099, 12, 31, 0, 0, 0, 0)
		Me.AutoApplyDateUDTE.MinDate = New Date(1980, 1, 1, 0, 0, 0, 0)
		Me.AutoApplyDateUDTE.Name = "AutoApplyDateUDTE"
		'
		'ApplyDateLabel
		'
		Me.ApplyDateLabel.BackColor = System.Drawing.Color.Transparent
		Me.ApplyDateLabel.Cursor = System.Windows.Forms.Cursors.Default
		resources.ApplyResources(Me.ApplyDateLabel, "ApplyDateLabel")
		Me.ApplyDateLabel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.ApplyDateLabel.Name = "ApplyDateLabel"
		'
		'AutoApplyCheckBox
		'
		resources.ApplyResources(Me.AutoApplyCheckBox, "AutoApplyCheckBox")
		Me.AutoApplyCheckBox.Name = "AutoApplyCheckBox"
		Me.AutoApplyCheckBox.UseVisualStyleBackColor = True
		'
		'txtNumBatch
		'
		Me.txtNumBatch.AcceptsReturn = True
		resources.ApplyResources(Me.txtNumBatch, "txtNumBatch")
		Me.txtNumBatch.BackColor = System.Drawing.SystemColors.Window
		Me.txtNumBatch.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtNumBatch.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtNumBatch.Name = "txtNumBatch"
		Me.txtNumBatch.Tag = "Number"
		'
		'_lblLabel_7
		'
		resources.ApplyResources(Me._lblLabel_7, "_lblLabel_7")
		Me._lblLabel_7.BackColor = System.Drawing.Color.Transparent
		Me._lblLabel_7.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblLabel_7.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblLabel_7.Name = "_lblLabel_7"
		'
		'dtpStartDate
		'
		resources.ApplyResources(Me.dtpStartDate, "dtpStartDate")
		Me.dtpStartDate.Name = "dtpStartDate"
		'
		'chkIncScaleItems
		'
		resources.ApplyResources(Me.chkIncScaleItems, "chkIncScaleItems")
		Me.chkIncScaleItems.Checked = True
		Me.chkIncScaleItems.CheckState = System.Windows.Forms.CheckState.Checked
		Me.chkIncScaleItems.Name = "chkIncScaleItems"
		Me.chkIncScaleItems.UseVisualStyleBackColor = True
		'
		'chkSelectAll
		'
		resources.ApplyResources(Me.chkSelectAll, "chkSelectAll")
		Me.chkSelectAll.Name = "chkSelectAll"
		Me.chkSelectAll.UseVisualStyleBackColor = True
		'
		'Label1
		'
		resources.ApplyResources(Me.Label1, "Label1")
		Me.Label1.ForeColor = System.Drawing.Color.Red
		Me.Label1.Name = "Label1"
		'
		'chkIncludeNonRetailItems
		'
		resources.ApplyResources(Me.chkIncludeNonRetailItems, "chkIncludeNonRetailItems")
		Me.chkIncludeNonRetailItems.Name = "chkIncludeNonRetailItems"
		Me.chkIncludeNonRetailItems.UseVisualStyleBackColor = True
		'
		'cmbSubTeam
		'
		resources.ApplyResources(Me.cmbSubTeam, "cmbSubTeam")
		Me.cmbSubTeam.CheckboxFont = New System.Drawing.Font("Arial", 8.0!)
		Me.cmbSubTeam.CheckboxForeColor = System.Drawing.SystemColors.ControlText
		Me.cmbSubTeam.CheckboxText = "Show All"
		Me.cmbSubTeam.ClearSelectionVisisble = False
		Me.cmbSubTeam.DataSource = Nothing
		Me.cmbSubTeam.DisplayMember = "SubTeamName"
		Me.cmbSubTeam.DropDownWidth = 162
		Me.cmbSubTeam.HeaderFont = New System.Drawing.Font("Arial", 8.0!)
		Me.cmbSubTeam.HeaderForeColor = System.Drawing.SystemColors.ControlText
		Me.cmbSubTeam.HeaderText = "Caption"
		Me.cmbSubTeam.HeaderVisible = False
		Me.cmbSubTeam.IsShowAll = False
		Me.cmbSubTeam.Name = "cmbSubTeam"
		Me.cmbSubTeam.SelectedIndex = -1
		Me.cmbSubTeam.SelectedItem = Nothing
		Me.cmbSubTeam.SelectedText = ""
		Me.cmbSubTeam.SelectedValue = Nothing
		Me.cmbSubTeam.ValueMember = "SubTeamNo"
		'
		'frmPricingBatchItemSearch
		'
		Me.AcceptButton = Me.cmdSearch
		resources.ApplyResources(Me, "$this")
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.Controls.Add(Me.cmbSubTeam)
		Me.Controls.Add(Me.chkIncludeNonRetailItems)
		Me.Controls.Add(Me.Label1)
		Me.Controls.Add(Me.chkSelectAll)
		Me.Controls.Add(Me.chkIncScaleItems)
		Me.Controls.Add(Me.cmdBERTReport)
		Me.Controls.Add(Me.dtpStartDate)
		Me.Controls.Add(Me.txtNumBatch)
		Me.Controls.Add(Me.cmdSelect)
		Me.Controls.Add(Me._lblLabel_7)
		Me.Controls.Add(Me.HeaderFrame)
		Me.Controls.Add(Me.ugrdList)
		Me.Controls.Add(Me.cmdSearch)
		Me.Controls.Add(Me.fraStores)
		Me.Controls.Add(Me.fraPriceType)
		Me.Controls.Add(Me.StatusBar1)
		Me.Controls.Add(Me.cmdExit)
		Me.Controls.Add(Me.txtIdentifier)
		Me.Controls.Add(Me.txtDescription)
		Me.Controls.Add(Me.fraType)
		Me.Controls.Add(Me.lblDates)
		Me.Controls.Add(Me._lblLabel_1)
		Me.Controls.Add(Me._lblLabel_5)
		Me.Controls.Add(Me._lblLabel_2)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmPricingBatchItemSearch"
		Me.ShowInTaskbar = False
		Me.fraStores.ResumeLayout(False)
		Me.fraStores.PerformLayout()
		CType(Me.ucmbStoreList, System.ComponentModel.ISupportInitialize).EndInit()
		Me.fraPriceType.ResumeLayout(False)
		Me.StatusBar1.ResumeLayout(False)
		Me.StatusBar1.PerformLayout()
		Me.fraType.ResumeLayout(False)
		CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.optSelection, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.optType, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.ugrdList, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.UltraDataSource1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.HeaderFrame.ResumeLayout(False)
		Me.HeaderFrame.PerformLayout()
		CType(Me.AutoApplyDateUDTE, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.dtpStartDate, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	Friend WithEvents ugrdList As Infragistics.Win.UltraWinGrid.UltraGrid
	Friend WithEvents HeaderFrame As System.Windows.Forms.GroupBox
	Public WithEvents BatchDescriptionTextBox As System.Windows.Forms.TextBox
	Public WithEvents BatchDescLabel As System.Windows.Forms.Label
	Friend WithEvents AutoApplyDateUDTE As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
	Public WithEvents ApplyDateLabel As System.Windows.Forms.Label
	Friend WithEvents AutoApplyCheckBox As System.Windows.Forms.CheckBox
	Public WithEvents txtNumBatch As System.Windows.Forms.TextBox
	Public WithEvents cmdSelect As System.Windows.Forms.Button
	Public WithEvents _lblLabel_7 As System.Windows.Forms.Label
	Friend WithEvents UltraDataSource1 As Infragistics.Win.UltraWinDataSource.UltraDataSource
	Friend WithEvents dtpStartDate As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
	Public WithEvents cmbPriceType As System.Windows.Forms.ComboBox
	Public WithEvents cmdBERTReport As System.Windows.Forms.Button
	Public WithEvents _optType_5 As System.Windows.Forms.RadioButton
	Friend WithEvents chkIncScaleItems As System.Windows.Forms.CheckBox
	Friend WithEvents chkSelectAll As System.Windows.Forms.CheckBox
	Friend WithEvents ucmbStoreList As Infragistics.Win.UltraWinGrid.UltraCombo
	Friend WithEvents Label1 As System.Windows.Forms.Label
	Friend WithEvents chkIncludeNonRetailItems As System.Windows.Forms.CheckBox
	Friend WithEvents cmbSubTeam As SubteamComboBox
#End Region
End Class