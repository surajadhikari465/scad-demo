<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmLineDrive
#Region "Windows Form Designer generated code "
	<System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        IsInitializing = True
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
	Public WithEvents _optSelection_0 As System.Windows.Forms.RadioButton
	Public WithEvents _optSelection_1 As System.Windows.Forms.RadioButton
	Public WithEvents _optSelection_2 As System.Windows.Forms.RadioButton
	Public WithEvents cmbZones As System.Windows.Forms.ComboBox
	Public WithEvents _optSelection_3 As System.Windows.Forms.RadioButton
	Public WithEvents _optSelection_4 As System.Windows.Forms.RadioButton
	Public WithEvents cmbStates As System.Windows.Forms.ComboBox
    Public WithEvents _Frame1_1 As System.Windows.Forms.GroupBox
	Public WithEvents txtPct As System.Windows.Forms.TextBox
    Public WithEvents _txtField_6 As System.Windows.Forms.TextBox
	Public WithEvents _txtField_5 As System.Windows.Forms.TextBox
	Public WithEvents _txtField_4 As System.Windows.Forms.TextBox
    Public WithEvents lblLimitSalePrice As System.Windows.Forms.Label
    Public WithEvents lblBuySalePrice As System.Windows.Forms.Label
    Public WithEvents lblBuyRegularPrice As System.Windows.Forms.Label
    Public WithEvents Frame2 As System.Windows.Forms.GroupBox
    Public WithEvents cmbPricingMethod As System.Windows.Forms.ComboBox
    Public WithEvents lblPercent As System.Windows.Forms.Label
    Public WithEvents _lblLabel_2 As System.Windows.Forms.Label
    Public WithEvents lblDates As System.Windows.Forms.Label
    Public WithEvents lblMethod As System.Windows.Forms.Label
    Public WithEvents _Frame1_0 As System.Windows.Forms.GroupBox
    Public WithEvents chkPrintOnly As System.Windows.Forms.CheckBox
    Public WithEvents cmdApply As System.Windows.Forms.Button
    Public WithEvents cmdExit As System.Windows.Forms.Button
    Public WithEvents cmdReport As System.Windows.Forms.Button
    Public WithEvents cmbBrand As System.Windows.Forms.ComboBox
    Public WithEvents txtFamily As System.Windows.Forms.TextBox
    Public WithEvents lblBrand As System.Windows.Forms.Label
    Public WithEvents lblFamily As System.Windows.Forms.Label
    Public WithEvents Frame1 As Microsoft.VisualBasic.Compatibility.VB6.GroupBoxArray
    Public WithEvents optSelection As Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray
    Public WithEvents txtDate As Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray
    Public WithEvents txtField As Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmLineDrive))
        Dim Appearance16 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim UltraGridBand2 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1)
        Dim UltraGridColumn8 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Store_No")
        Dim UltraGridColumn9 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Store_Name")
        Dim Appearance17 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim UltraGridColumn10 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Zone_ID")
        Dim UltraGridColumn11 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("State")
        Dim UltraGridColumn12 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("WFM_Store")
        Dim UltraGridColumn13 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Mega_Store")
        Dim UltraGridColumn14 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("CustomerType")
        Dim Appearance18 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance19 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance20 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance21 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance22 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance23 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance24 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance25 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance26 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance27 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance28 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance29 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance30 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me._optSelection_0 = New System.Windows.Forms.RadioButton
        Me._optSelection_1 = New System.Windows.Forms.RadioButton
        Me._optSelection_2 = New System.Windows.Forms.RadioButton
        Me._optSelection_3 = New System.Windows.Forms.RadioButton
        Me._optSelection_4 = New System.Windows.Forms.RadioButton
        Me.cmdApply = New System.Windows.Forms.Button
        Me.cmdExit = New System.Windows.Forms.Button
        Me.cmdReport = New System.Windows.Forms.Button
        Me._Frame1_1 = New System.Windows.Forms.GroupBox
        Me.ugrdStoreList = New Infragistics.Win.UltraWinGrid.UltraGrid
        Me.cmbZones = New System.Windows.Forms.ComboBox
        Me.cmbStates = New System.Windows.Forms.ComboBox
        Me._Frame1_0 = New System.Windows.Forms.GroupBox
        Me.dtpEndDate = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
        Me.dtpStartDate = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
        Me.txtPct = New System.Windows.Forms.TextBox
        Me.Frame2 = New System.Windows.Forms.GroupBox
        Me._txtField_6 = New System.Windows.Forms.TextBox
        Me._txtField_5 = New System.Windows.Forms.TextBox
        Me._txtField_4 = New System.Windows.Forms.TextBox
        Me.lblLimitSalePrice = New System.Windows.Forms.Label
        Me.lblBuySalePrice = New System.Windows.Forms.Label
        Me.lblBuyRegularPrice = New System.Windows.Forms.Label
        Me.cmbPricingMethod = New System.Windows.Forms.ComboBox
        Me.lblPercent = New System.Windows.Forms.Label
        Me.lblDates = New System.Windows.Forms.Label
        Me.lblMethod = New System.Windows.Forms.Label
        Me._lblLabel_2 = New System.Windows.Forms.Label
        Me.chkPrintOnly = New System.Windows.Forms.CheckBox
        Me.cmbBrand = New System.Windows.Forms.ComboBox
        Me.txtFamily = New System.Windows.Forms.TextBox
        Me.lblBrand = New System.Windows.Forms.Label
        Me.lblFamily = New System.Windows.Forms.Label
        Me.Frame1 = New Microsoft.VisualBasic.Compatibility.VB6.GroupBoxArray(Me.components)
        Me.optSelection = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(Me.components)
        Me.txtDate = New Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray(Me.components)
        Me.txtField = New Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray(Me.components)
        Me._Frame1_1.SuspendLayout()
        CType(Me.ugrdStoreList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._Frame1_0.SuspendLayout()
        CType(Me.dtpEndDate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dtpStartDate, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Frame2.SuspendLayout()
        CType(Me.Frame1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.optSelection, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtDate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtField, System.ComponentModel.ISupportInitialize).BeginInit()
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
        'cmdApply
        '
        Me.cmdApply.BackColor = System.Drawing.SystemColors.Control
        Me.cmdApply.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdApply, "cmdApply")
        Me.cmdApply.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdApply.Name = "cmdApply"
        Me.ToolTip1.SetToolTip(Me.cmdApply, resources.GetString("cmdApply.ToolTip"))
        Me.cmdApply.UseVisualStyleBackColor = False
        '
        'cmdExit
        '
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
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
        '_Frame1_1
        '
        Me._Frame1_1.BackColor = System.Drawing.SystemColors.Control
        Me._Frame1_1.Controls.Add(Me.ugrdStoreList)
        Me._Frame1_1.Controls.Add(Me._optSelection_0)
        Me._Frame1_1.Controls.Add(Me._optSelection_1)
        Me._Frame1_1.Controls.Add(Me._optSelection_2)
        Me._Frame1_1.Controls.Add(Me.cmbZones)
        Me._Frame1_1.Controls.Add(Me._optSelection_3)
        Me._Frame1_1.Controls.Add(Me._optSelection_4)
        Me._Frame1_1.Controls.Add(Me.cmbStates)
        resources.ApplyResources(Me._Frame1_1, "_Frame1_1")
        Me._Frame1_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.SetIndex(Me._Frame1_1, CType(1, Short))
        Me._Frame1_1.Name = "_Frame1_1"
        Me._Frame1_1.TabStop = False
        '
        'ugrdStoreList
        '
        Appearance16.BackColor = System.Drawing.SystemColors.ControlLight
        Appearance16.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.ugrdStoreList.DisplayLayout.Appearance = Appearance16
        Me.ugrdStoreList.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        UltraGridColumn8.Header.VisiblePosition = 0
        UltraGridColumn8.Hidden = True
        UltraGridColumn9.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn9.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        Appearance17.TextHAlign = Infragistics.Win.HAlign.Center
        UltraGridColumn9.Header.Appearance = Appearance17
        UltraGridColumn9.Header.Caption = resources.GetString("resource.Caption")
        UltraGridColumn9.Header.VisiblePosition = 1
        UltraGridColumn9.SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled
        UltraGridColumn10.Header.VisiblePosition = 2
        UltraGridColumn10.Hidden = True
        UltraGridColumn11.Header.VisiblePosition = 3
        UltraGridColumn11.Hidden = True
        UltraGridColumn12.Header.VisiblePosition = 4
        UltraGridColumn12.Hidden = True
        UltraGridColumn13.Header.VisiblePosition = 5
        UltraGridColumn13.Hidden = True
        UltraGridColumn14.Header.VisiblePosition = 6
        UltraGridColumn14.Hidden = True
        UltraGridBand2.Columns.AddRange(New Object() {UltraGridColumn8, UltraGridColumn9, UltraGridColumn10, UltraGridColumn11, UltraGridColumn12, UltraGridColumn13, UltraGridColumn14})
        UltraGridBand2.RowLayoutStyle = Infragistics.Win.UltraWinGrid.RowLayoutStyle.None
        Me.ugrdStoreList.DisplayLayout.BandsSerializer.Add(UltraGridBand2)
        Me.ugrdStoreList.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Appearance18.FontData.BoldAsString = resources.GetString("resource.BoldAsString")
        Me.ugrdStoreList.DisplayLayout.CaptionAppearance = Appearance18
        Me.ugrdStoreList.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Appearance19.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance19.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance19.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance19.BorderColor = System.Drawing.SystemColors.Window
        Me.ugrdStoreList.DisplayLayout.GroupByBox.Appearance = Appearance19
        Appearance20.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugrdStoreList.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance20
        Me.ugrdStoreList.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.ugrdStoreList.DisplayLayout.GroupByBox.Hidden = True
        Appearance21.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance21.BackColor2 = System.Drawing.SystemColors.Control
        Appearance21.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance21.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugrdStoreList.DisplayLayout.GroupByBox.PromptAppearance = Appearance21
        Me.ugrdStoreList.DisplayLayout.MaxColScrollRegions = 1
        Me.ugrdStoreList.DisplayLayout.MaxRowScrollRegions = 1
        Appearance22.BackColor = System.Drawing.SystemColors.Window
        Appearance22.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ugrdStoreList.DisplayLayout.Override.ActiveCellAppearance = Appearance22
        Me.ugrdStoreList.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.ugrdStoreList.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance23.BackColor = System.Drawing.SystemColors.Window
        Me.ugrdStoreList.DisplayLayout.Override.CardAreaAppearance = Appearance23
        Appearance24.BorderColor = System.Drawing.Color.Silver
        Appearance24.FontData.BoldAsString = resources.GetString("resource.BoldAsString1")
        Appearance24.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.ugrdStoreList.DisplayLayout.Override.CellAppearance = Appearance24
        Me.ugrdStoreList.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.ugrdStoreList.DisplayLayout.Override.CellPadding = 0
        Appearance25.FontData.BoldAsString = resources.GetString("resource.BoldAsString2")
        Me.ugrdStoreList.DisplayLayout.Override.FixedHeaderAppearance = Appearance25
        Appearance26.BackColor = System.Drawing.SystemColors.Control
        Appearance26.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance26.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance26.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance26.BorderColor = System.Drawing.SystemColors.Window
        Me.ugrdStoreList.DisplayLayout.Override.GroupByRowAppearance = Appearance26
        Appearance27.FontData.BoldAsString = resources.GetString("resource.BoldAsString3")
        Appearance27.TextHAlign = Infragistics.Win.HAlign.Left
        Me.ugrdStoreList.DisplayLayout.Override.HeaderAppearance = Appearance27
        Me.ugrdStoreList.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle
        Me.ugrdStoreList.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance28.BackColor = System.Drawing.SystemColors.ControlLight
        Me.ugrdStoreList.DisplayLayout.Override.RowAlternateAppearance = Appearance28
        Appearance29.BackColor = System.Drawing.SystemColors.Window
        Appearance29.BorderColor = System.Drawing.Color.Silver
        Me.ugrdStoreList.DisplayLayout.Override.RowAppearance = Appearance29
        Me.ugrdStoreList.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[True]
        Me.ugrdStoreList.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed
        Me.ugrdStoreList.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.ugrdStoreList.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.ugrdStoreList.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Extended
        Appearance30.BackColor = System.Drawing.SystemColors.ControlLight
        Me.ugrdStoreList.DisplayLayout.Override.TemplateAddRowAppearance = Appearance30
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
        '_Frame1_0
        '
        Me._Frame1_0.BackColor = System.Drawing.SystemColors.Control
        Me._Frame1_0.Controls.Add(Me.dtpEndDate)
        Me._Frame1_0.Controls.Add(Me.dtpStartDate)
        Me._Frame1_0.Controls.Add(Me.txtPct)
        Me._Frame1_0.Controls.Add(Me.Frame2)
        Me._Frame1_0.Controls.Add(Me.cmbPricingMethod)
        Me._Frame1_0.Controls.Add(Me.lblPercent)
        Me._Frame1_0.Controls.Add(Me.lblDates)
        Me._Frame1_0.Controls.Add(Me.lblMethod)
        Me._Frame1_0.Controls.Add(Me._lblLabel_2)
        resources.ApplyResources(Me._Frame1_0, "_Frame1_0")
        Me._Frame1_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.SetIndex(Me._Frame1_0, CType(0, Short))
        Me._Frame1_0.Name = "_Frame1_0"
        Me._Frame1_0.TabStop = False
        '
        'dtpEndDate
        '
        resources.ApplyResources(Me.dtpEndDate, "dtpEndDate")
        Me.dtpEndDate.Name = "dtpEndDate"
        '
        'dtpStartDate
        '
        resources.ApplyResources(Me.dtpStartDate, "dtpStartDate")
        Me.dtpStartDate.Name = "dtpStartDate"
        '
        'txtPct
        '
        Me.txtPct.AcceptsReturn = True
        Me.txtPct.BackColor = System.Drawing.SystemColors.Window
        Me.txtPct.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.txtPct, "txtPct")
        Me.txtPct.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPct.Name = "txtPct"
        Me.txtPct.Tag = "Currency"
        '
        'Frame2
        '
        Me.Frame2.BackColor = System.Drawing.SystemColors.Control
        Me.Frame2.Controls.Add(Me._txtField_6)
        Me.Frame2.Controls.Add(Me._txtField_5)
        Me.Frame2.Controls.Add(Me._txtField_4)
        Me.Frame2.Controls.Add(Me.lblLimitSalePrice)
        Me.Frame2.Controls.Add(Me.lblBuySalePrice)
        Me.Frame2.Controls.Add(Me.lblBuyRegularPrice)
        resources.ApplyResources(Me.Frame2, "Frame2")
        Me.Frame2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame2.Name = "Frame2"
        Me.Frame2.TabStop = False
        '
        '_txtField_6
        '
        Me._txtField_6.AcceptsReturn = True
        Me._txtField_6.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_6.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me._txtField_6, "_txtField_6")
        Me._txtField_6.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_6, CType(6, Short))
        Me._txtField_6.Name = "_txtField_6"
        Me._txtField_6.Tag = "Number"
        '
        '_txtField_5
        '
        Me._txtField_5.AcceptsReturn = True
        Me._txtField_5.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_5.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me._txtField_5, "_txtField_5")
        Me._txtField_5.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_5, CType(5, Short))
        Me._txtField_5.Name = "_txtField_5"
        Me._txtField_5.ReadOnly = True
        Me._txtField_5.Tag = "Number"
        '
        '_txtField_4
        '
        Me._txtField_4.AcceptsReturn = True
        Me._txtField_4.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_4.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me._txtField_4, "_txtField_4")
        Me._txtField_4.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_4, CType(4, Short))
        Me._txtField_4.Name = "_txtField_4"
        Me._txtField_4.Tag = "Number"
        '
        'lblLimitSalePrice
        '
        Me.lblLimitSalePrice.BackColor = System.Drawing.Color.Transparent
        Me.lblLimitSalePrice.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblLimitSalePrice, "lblLimitSalePrice")
        Me.lblLimitSalePrice.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLimitSalePrice.Name = "lblLimitSalePrice"
        '
        'lblBuySalePrice
        '
        Me.lblBuySalePrice.BackColor = System.Drawing.Color.Transparent
        Me.lblBuySalePrice.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblBuySalePrice, "lblBuySalePrice")
        Me.lblBuySalePrice.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBuySalePrice.Name = "lblBuySalePrice"
        '
        'lblBuyRegularPrice
        '
        Me.lblBuyRegularPrice.BackColor = System.Drawing.Color.Transparent
        Me.lblBuyRegularPrice.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblBuyRegularPrice, "lblBuyRegularPrice")
        Me.lblBuyRegularPrice.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBuyRegularPrice.Name = "lblBuyRegularPrice"
        '
        'cmbPricingMethod
        '
        Me.cmbPricingMethod.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbPricingMethod.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbPricingMethod.BackColor = System.Drawing.SystemColors.Window
        Me.cmbPricingMethod.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbPricingMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.cmbPricingMethod, "cmbPricingMethod")
        Me.cmbPricingMethod.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbPricingMethod.Name = "cmbPricingMethod"
        '
        'lblPercent
        '
        Me.lblPercent.BackColor = System.Drawing.Color.Transparent
        Me.lblPercent.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblPercent, "lblPercent")
        Me.lblPercent.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPercent.Name = "lblPercent"
        '
        'lblDates
        '
        Me.lblDates.BackColor = System.Drawing.Color.Transparent
        Me.lblDates.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblDates, "lblDates")
        Me.lblDates.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDates.Name = "lblDates"
        '
        'lblMethod
        '
        Me.lblMethod.BackColor = System.Drawing.Color.Transparent
        Me.lblMethod.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblMethod, "lblMethod")
        Me.lblMethod.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMethod.Name = "lblMethod"
        '
        '_lblLabel_2
        '
        Me._lblLabel_2.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_2.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lblLabel_2, "_lblLabel_2")
        Me._lblLabel_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabel_2.Name = "_lblLabel_2"
        '
        'chkPrintOnly
        '
        Me.chkPrintOnly.BackColor = System.Drawing.SystemColors.Control
        Me.chkPrintOnly.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.chkPrintOnly, "chkPrintOnly")
        Me.chkPrintOnly.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkPrintOnly.Name = "chkPrintOnly"
        Me.chkPrintOnly.UseVisualStyleBackColor = False
        '
        'cmbBrand
        '
        Me.cmbBrand.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbBrand.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbBrand.BackColor = System.Drawing.SystemColors.Window
        Me.cmbBrand.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbBrand.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.cmbBrand, "cmbBrand")
        Me.cmbBrand.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbBrand.Name = "cmbBrand"
        Me.cmbBrand.Sorted = True
        '
        'txtFamily
        '
        Me.txtFamily.AcceptsReturn = True
        Me.txtFamily.BackColor = System.Drawing.SystemColors.Window
        Me.txtFamily.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.txtFamily, "txtFamily")
        Me.txtFamily.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFamily.Name = "txtFamily"
        Me.txtFamily.Tag = "Number"
        '
        'lblBrand
        '
        Me.lblBrand.BackColor = System.Drawing.Color.Transparent
        Me.lblBrand.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblBrand, "lblBrand")
        Me.lblBrand.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBrand.Name = "lblBrand"
        '
        'lblFamily
        '
        Me.lblFamily.BackColor = System.Drawing.Color.Transparent
        Me.lblFamily.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblFamily, "lblFamily")
        Me.lblFamily.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFamily.Name = "lblFamily"
        '
        'optSelection
        '
        '
        'txtField
        '
        '
        'frmLineDrive
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.Controls.Add(Me._Frame1_1)
        Me.Controls.Add(Me._Frame1_0)
        Me.Controls.Add(Me.chkPrintOnly)
        Me.Controls.Add(Me.cmdApply)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.cmdReport)
        Me.Controls.Add(Me.cmbBrand)
        Me.Controls.Add(Me.txtFamily)
        Me.Controls.Add(Me.lblBrand)
        Me.Controls.Add(Me.lblFamily)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmLineDrive"
        Me.ShowInTaskbar = False
        Me._Frame1_1.ResumeLayout(False)
        CType(Me.ugrdStoreList, System.ComponentModel.ISupportInitialize).EndInit()
        Me._Frame1_0.ResumeLayout(False)
        Me._Frame1_0.PerformLayout()
        CType(Me.dtpEndDate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dtpStartDate, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Frame2.ResumeLayout(False)
        Me.Frame2.PerformLayout()
        CType(Me.Frame1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.optSelection, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtDate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtField, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ugrdStoreList As Infragistics.Win.UltraWinGrid.UltraGrid
    Friend WithEvents dtpEndDate As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
    Friend WithEvents dtpStartDate As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
#End Region
End Class