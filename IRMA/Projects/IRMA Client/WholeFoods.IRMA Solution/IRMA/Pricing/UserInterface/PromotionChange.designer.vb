<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmPromotionChange
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
	Public WithEvents cmbStates As System.Windows.Forms.ComboBox
	Public WithEvents _optSelection_4 As System.Windows.Forms.RadioButton
	Public WithEvents _optSelection_3 As System.Windows.Forms.RadioButton
	Public WithEvents cmbZones As System.Windows.Forms.ComboBox
	Public WithEvents _optSelection_2 As System.Windows.Forms.RadioButton
	Public WithEvents _optSelection_1 As System.Windows.Forms.RadioButton
	Public WithEvents _optSelection_0 As System.Windows.Forms.RadioButton
    Public WithEvents _Frame1_1 As System.Windows.Forms.GroupBox
    Public WithEvents txtMultiple As System.Windows.Forms.TextBox
	Public WithEvents _lblSlash_3 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_9 As System.Windows.Forms.Label
	Public WithEvents fraReg As System.Windows.Forms.GroupBox
    'Public WithEvents CrystalReport1 As AxCrystal.AxCrystalReport
	Public WithEvents txtMSRPMultiple As System.Windows.Forms.TextBox
    Public WithEvents cmbPricingMethod As System.Windows.Forms.ComboBox
	Public WithEvents _txtField_0 As System.Windows.Forms.TextBox
    Public WithEvents _txtField_4 As System.Windows.Forms.TextBox
	Public WithEvents _txtField_5 As System.Windows.Forms.TextBox
	Public WithEvents _txtField_6 As System.Windows.Forms.TextBox
	Public WithEvents _lblLabel_0 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_3 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_4 As System.Windows.Forms.Label
	Public WithEvents Frame2 As System.Windows.Forms.GroupBox
	Public WithEvents _lblLabel_7 As System.Windows.Forms.Label
	Public WithEvents _lblSlash_2 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_6 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_1 As System.Windows.Forms.Label
    Public WithEvents _lblLabel_2 As System.Windows.Forms.Label
    Public WithEvents _Frame1_0 As System.Windows.Forms.GroupBox
	Public WithEvents cmdExit As System.Windows.Forms.Button
	Public WithEvents cmdSelect As System.Windows.Forms.Button
	Public WithEvents Frame1 As Microsoft.VisualBasic.Compatibility.VB6.GroupBoxArray
	Public WithEvents lblLabel As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	Public WithEvents lblSlash As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	Public WithEvents optSelection As Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray
	Public WithEvents txtField As Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPromotionChange))
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
        Dim UltraGridColumn10 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Promo Price")
        Dim UltraGridColumn11 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("hasVendor")
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
        Dim UltraDataColumn10 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Promo Price")
        Dim UltraDataColumn11 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("hasVendor")
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me._optSelection_4 = New System.Windows.Forms.RadioButton
        Me._optSelection_3 = New System.Windows.Forms.RadioButton
        Me._optSelection_2 = New System.Windows.Forms.RadioButton
        Me._optSelection_1 = New System.Windows.Forms.RadioButton
        Me._optSelection_0 = New System.Windows.Forms.RadioButton
        Me.cmdExit = New System.Windows.Forms.Button
        Me.cmdSelect = New System.Windows.Forms.Button
        Me.cmdItemVendors = New System.Windows.Forms.Button
        Me.Button_MarginInfo = New System.Windows.Forms.Button
        Me._Frame1_1 = New System.Windows.Forms.GroupBox
        Me.checkboxPriceDisplay = New System.Windows.Forms.CheckBox
        Me.ugrdStoreList = New Infragistics.Win.UltraWinGrid.UltraGrid
        Me.StoresUltraDataSource = New Infragistics.Win.UltraWinDataSource.UltraDataSource(Me.components)
        Me.cmbStates = New System.Windows.Forms.ComboBox
        Me.cmbZones = New System.Windows.Forms.ComboBox
        Me.fraReg = New System.Windows.Forms.GroupBox
        Me.txtPOSPrice = New Infragistics.Win.UltraWinEditors.UltraNumericEditor
        Me.txtMultiple = New System.Windows.Forms.TextBox
        Me._lblSlash_3 = New System.Windows.Forms.Label
        Me._lblLabel_9 = New System.Windows.Forms.Label
        Me._Frame1_0 = New System.Windows.Forms.GroupBox
        Me.cmbPriceType = New System.Windows.Forms.ComboBox
        Me.dtpEndDate = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
        Me.dtpStartDate = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
        Me.txtMSRPPrice = New Infragistics.Win.UltraWinEditors.UltraNumericEditor
        Me.lblDash = New System.Windows.Forms.Label
        Me.txtPOSPromoPrice = New Infragistics.Win.UltraWinEditors.UltraNumericEditor
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtMSRPMultiple = New System.Windows.Forms.TextBox
        Me.cmbPricingMethod = New System.Windows.Forms.ComboBox
        Me._txtField_0 = New System.Windows.Forms.TextBox
        Me.Frame2 = New System.Windows.Forms.GroupBox
        Me._txtField_4 = New System.Windows.Forms.TextBox
        Me._txtField_5 = New System.Windows.Forms.TextBox
        Me._txtField_6 = New System.Windows.Forms.TextBox
        Me._lblLabel_0 = New System.Windows.Forms.Label
        Me._lblLabel_3 = New System.Windows.Forms.Label
        Me._lblLabel_4 = New System.Windows.Forms.Label
        Me._lblLabel_7 = New System.Windows.Forms.Label
        Me._lblSlash_2 = New System.Windows.Forms.Label
        Me._lblLabel_6 = New System.Windows.Forms.Label
        Me._lblLabel_1 = New System.Windows.Forms.Label
        Me._lblLabel_2 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Frame1 = New Microsoft.VisualBasic.Compatibility.VB6.GroupBoxArray(Me.components)
        Me.lblLabel = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(Me.components)
        Me.lblSlash = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(Me.components)
        Me.optSelection = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(Me.components)
        Me.txtField = New Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray(Me.components)
        Me._Frame1_1.SuspendLayout()
        CType(Me.ugrdStoreList, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.StoresUltraDataSource, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.fraReg.SuspendLayout()
        CType(Me.txtPOSPrice, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._Frame1_0.SuspendLayout()
        CType(Me.dtpEndDate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dtpStartDate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtMSRPPrice, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtPOSPromoPrice, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Frame2.SuspendLayout()
        CType(Me.Frame1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblSlash, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.optSelection, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtField, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
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
        '_Frame1_1
        '
        Me._Frame1_1.BackColor = System.Drawing.SystemColors.Control
        Me._Frame1_1.Controls.Add(Me.checkboxPriceDisplay)
        Me._Frame1_1.Controls.Add(Me.ugrdStoreList)
        Me._Frame1_1.Controls.Add(Me.cmbStates)
        Me._Frame1_1.Controls.Add(Me._optSelection_4)
        Me._Frame1_1.Controls.Add(Me._optSelection_3)
        Me._Frame1_1.Controls.Add(Me.cmbZones)
        Me._Frame1_1.Controls.Add(Me._optSelection_2)
        Me._Frame1_1.Controls.Add(Me._optSelection_1)
        Me._Frame1_1.Controls.Add(Me._optSelection_0)
        resources.ApplyResources(Me._Frame1_1, "_Frame1_1")
        Me._Frame1_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.SetIndex(Me._Frame1_1, CType(1, Short))
        Me._Frame1_1.Name = "_Frame1_1"
        Me._Frame1_1.TabStop = False
        '
        'checkboxPriceDisplay
        '
        resources.ApplyResources(Me.checkboxPriceDisplay, "checkboxPriceDisplay")
        Me.checkboxPriceDisplay.Name = "checkboxPriceDisplay"
        Me.checkboxPriceDisplay.UseVisualStyleBackColor = True
        '
        'ugrdStoreList
        '
        Me.ugrdStoreList.DataMember = "Band 0"
        Me.ugrdStoreList.DataSource = Me.StoresUltraDataSource
        Appearance1.BackColor = System.Drawing.SystemColors.ControlLight
        Appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.ugrdStoreList.DisplayLayout.Appearance = Appearance1
        Me.ugrdStoreList.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        UltraGridColumn1.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn1.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn1.Header.VisiblePosition = 0
        UltraGridColumn1.Width = 14
        UltraGridColumn2.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn2.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn2.Header.VisiblePosition = 1
        UltraGridColumn2.Width = 14
        UltraGridColumn3.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn3.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn3.Header.VisiblePosition = 2
        UltraGridColumn3.Width = 14
        UltraGridColumn4.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn4.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn4.Header.VisiblePosition = 3
        UltraGridColumn4.Width = 15
        UltraGridColumn5.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn5.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn5.Header.VisiblePosition = 4
        UltraGridColumn5.Width = 23
        UltraGridColumn6.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn6.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn6.Header.VisiblePosition = 5
        UltraGridColumn6.Width = 23
        UltraGridColumn7.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn7.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn7.Header.VisiblePosition = 6
        UltraGridColumn7.Width = 16
        UltraGridColumn8.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn8.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn8.Header.VisiblePosition = 7
        UltraGridColumn8.Width = 14
        UltraGridColumn9.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn9.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn9.Header.VisiblePosition = 8
        UltraGridColumn9.Width = 18
        UltraGridColumn10.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn10.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn10.Header.VisiblePosition = 9
        UltraGridColumn10.Width = 51
        UltraGridColumn11.Header.VisiblePosition = 10
        UltraGridColumn11.Width = 69
        UltraGridBand1.Columns.AddRange(New Object() {UltraGridColumn1, UltraGridColumn2, UltraGridColumn3, UltraGridColumn4, UltraGridColumn5, UltraGridColumn6, UltraGridColumn7, UltraGridColumn8, UltraGridColumn9, UltraGridColumn10, UltraGridColumn11})
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
        Me.ugrdStoreList.TabStop = False
        '
        'StoresUltraDataSource
        '
        UltraDataColumn1.DataType = GetType(Short)
        UltraDataColumn3.DataType = GetType(Short)
        UltraDataColumn5.DataType = GetType(Boolean)
        UltraDataColumn6.DataType = GetType(Boolean)
        UltraDataColumn8.DataType = GetType(Double)
        UltraDataColumn11.DataType = GetType(Boolean)
        Me.StoresUltraDataSource.Band.Columns.AddRange(New Object() {UltraDataColumn1, UltraDataColumn2, UltraDataColumn3, UltraDataColumn4, UltraDataColumn5, UltraDataColumn6, UltraDataColumn7, UltraDataColumn8, UltraDataColumn9, UltraDataColumn10, UltraDataColumn11})
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
        'fraReg
        '
        Me.fraReg.BackColor = System.Drawing.SystemColors.Control
        Me.fraReg.Controls.Add(Me.txtPOSPrice)
        Me.fraReg.Controls.Add(Me.txtMultiple)
        Me.fraReg.Controls.Add(Me._lblSlash_3)
        Me.fraReg.Controls.Add(Me._lblLabel_9)
        resources.ApplyResources(Me.fraReg, "fraReg")
        Me.fraReg.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraReg.Name = "fraReg"
        Me.fraReg.TabStop = False
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
        '_lblSlash_3
        '
        Me._lblSlash_3.BackColor = System.Drawing.SystemColors.Control
        Me._lblSlash_3.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lblSlash_3, "_lblSlash_3")
        Me._lblSlash_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSlash.SetIndex(Me._lblSlash_3, CType(3, Short))
        Me._lblSlash_3.Name = "_lblSlash_3"
        '
        '_lblLabel_9
        '
        Me._lblLabel_9.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_9.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lblLabel_9, "_lblLabel_9")
        Me._lblLabel_9.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_9, CType(9, Short))
        Me._lblLabel_9.Name = "_lblLabel_9"
        '
        '_Frame1_0
        '
        Me._Frame1_0.BackColor = System.Drawing.SystemColors.Control
        Me._Frame1_0.Controls.Add(Me.cmbPriceType)
        Me._Frame1_0.Controls.Add(Me.dtpEndDate)
        Me._Frame1_0.Controls.Add(Me.dtpStartDate)
        Me._Frame1_0.Controls.Add(Me.txtMSRPPrice)
        Me._Frame1_0.Controls.Add(Me.lblDash)
        Me._Frame1_0.Controls.Add(Me.txtPOSPromoPrice)
        Me._Frame1_0.Controls.Add(Me.Label1)
        Me._Frame1_0.Controls.Add(Me.txtMSRPMultiple)
        Me._Frame1_0.Controls.Add(Me.cmbPricingMethod)
        Me._Frame1_0.Controls.Add(Me._txtField_0)
        Me._Frame1_0.Controls.Add(Me.Frame2)
        Me._Frame1_0.Controls.Add(Me._lblLabel_7)
        Me._Frame1_0.Controls.Add(Me._lblSlash_2)
        Me._Frame1_0.Controls.Add(Me._lblLabel_6)
        Me._Frame1_0.Controls.Add(Me._lblLabel_1)
        Me._Frame1_0.Controls.Add(Me._lblLabel_2)
        Me._Frame1_0.Controls.Add(Me.Label2)
        resources.ApplyResources(Me._Frame1_0, "_Frame1_0")
        Me._Frame1_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.SetIndex(Me._Frame1_0, CType(0, Short))
        Me._Frame1_0.Name = "_Frame1_0"
        Me._Frame1_0.TabStop = False
        '
        'cmbPriceType
        '
        Me.cmbPriceType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbPriceType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbPriceType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.cmbPriceType, "cmbPriceType")
        Me.cmbPriceType.Name = "cmbPriceType"
        '
        'dtpEndDate
        '
        Me.dtpEndDate.DateTime = New Date(1950, 1, 1, 0, 0, 0, 0)
        resources.ApplyResources(Me.dtpEndDate, "dtpEndDate")
        Me.dtpEndDate.MaxDate = New Date(2999, 12, 31, 0, 0, 0, 0)
        Me.dtpEndDate.MinDate = New Date(1950, 1, 1, 0, 0, 0, 0)
        Me.dtpEndDate.Name = "dtpEndDate"
        Me.dtpEndDate.Value = Nothing
        '
        'dtpStartDate
        '
        Me.dtpStartDate.DateTime = New Date(1950, 1, 1, 0, 0, 0, 0)
        resources.ApplyResources(Me.dtpStartDate, "dtpStartDate")
        Me.dtpStartDate.MaxDate = New Date(2999, 12, 31, 0, 0, 0, 0)
        Me.dtpStartDate.MinDate = New Date(1950, 1, 1, 0, 0, 0, 0)
        Me.dtpStartDate.Name = "dtpStartDate"
        Me.dtpStartDate.Value = Nothing
        '
        'txtMSRPPrice
        '
        resources.ApplyResources(Me.txtMSRPPrice, "txtMSRPPrice")
        Me.txtMSRPPrice.MaskClipMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeBoth
        Me.txtMSRPPrice.MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeLiterals
        Me.txtMSRPPrice.MaskInput = "{double:6.2}"
        Me.txtMSRPPrice.MaxValue = 100000
        Me.txtMSRPPrice.MinValue = 0
        Me.txtMSRPPrice.Name = "txtMSRPPrice"
        Me.txtMSRPPrice.Nullable = True
        Me.txtMSRPPrice.NullText = "0.00"
        Me.txtMSRPPrice.NumericType = Infragistics.Win.UltraWinEditors.NumericType.[Double]
        Me.txtMSRPPrice.PromptChar = Global.Microsoft.VisualBasic.ChrW(32)
        '
        'lblDash
        '
        Me.lblDash.BackColor = System.Drawing.Color.Transparent
        Me.lblDash.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblDash, "lblDash")
        Me.lblDash.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDash.Name = "lblDash"
        '
        'txtPOSPromoPrice
        '
        resources.ApplyResources(Me.txtPOSPromoPrice, "txtPOSPromoPrice")
        Me.txtPOSPromoPrice.MaskClipMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeBoth
        Me.txtPOSPromoPrice.MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeLiterals
        Me.txtPOSPromoPrice.MaskInput = "{double:6.2}"
        Me.txtPOSPromoPrice.MaxValue = 100000
        Me.txtPOSPromoPrice.MinValue = 0
        Me.txtPOSPromoPrice.Name = "txtPOSPromoPrice"
        Me.txtPOSPromoPrice.Nullable = True
        Me.txtPOSPromoPrice.NullText = "0.00"
        Me.txtPOSPromoPrice.NumericType = Infragistics.Win.UltraWinEditors.NumericType.[Double]
        Me.txtPOSPromoPrice.PromptChar = Global.Microsoft.VisualBasic.ChrW(32)
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Name = "Label1"
        '
        'txtMSRPMultiple
        '
        Me.txtMSRPMultiple.AcceptsReturn = True
        Me.txtMSRPMultiple.BackColor = System.Drawing.SystemColors.Window
        Me.txtMSRPMultiple.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.txtMSRPMultiple, "txtMSRPMultiple")
        Me.txtMSRPMultiple.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMSRPMultiple.Name = "txtMSRPMultiple"
        Me.txtMSRPMultiple.Tag = "Number"
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
        '_txtField_0
        '
        Me._txtField_0.AcceptsReturn = True
        Me._txtField_0.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_0.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me._txtField_0, "_txtField_0")
        Me._txtField_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_0, CType(0, Short))
        Me._txtField_0.Name = "_txtField_0"
        Me._txtField_0.Tag = "Number"
        '
        'Frame2
        '
        Me.Frame2.BackColor = System.Drawing.SystemColors.Control
        Me.Frame2.Controls.Add(Me._txtField_4)
        Me.Frame2.Controls.Add(Me._txtField_5)
        Me.Frame2.Controls.Add(Me._txtField_6)
        Me.Frame2.Controls.Add(Me._lblLabel_0)
        Me.Frame2.Controls.Add(Me._lblLabel_3)
        Me.Frame2.Controls.Add(Me._lblLabel_4)
        resources.ApplyResources(Me.Frame2, "Frame2")
        Me.Frame2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame2.Name = "Frame2"
        Me.Frame2.TabStop = False
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
        '_lblLabel_0
        '
        Me._lblLabel_0.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_0.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lblLabel_0, "_lblLabel_0")
        Me._lblLabel_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_0, CType(0, Short))
        Me._lblLabel_0.Name = "_lblLabel_0"
        '
        '_lblLabel_3
        '
        Me._lblLabel_3.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_3.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lblLabel_3, "_lblLabel_3")
        Me._lblLabel_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_3, CType(3, Short))
        Me._lblLabel_3.Name = "_lblLabel_3"
        '
        '_lblLabel_4
        '
        Me._lblLabel_4.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_4.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lblLabel_4, "_lblLabel_4")
        Me._lblLabel_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_4, CType(4, Short))
        Me._lblLabel_4.Name = "_lblLabel_4"
        '
        '_lblLabel_7
        '
        Me._lblLabel_7.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_7.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lblLabel_7, "_lblLabel_7")
        Me._lblLabel_7.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_7, CType(7, Short))
        Me._lblLabel_7.Name = "_lblLabel_7"
        '
        '_lblSlash_2
        '
        Me._lblSlash_2.BackColor = System.Drawing.SystemColors.Control
        Me._lblSlash_2.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lblSlash_2, "_lblSlash_2")
        Me._lblSlash_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSlash.SetIndex(Me._lblSlash_2, CType(2, Short))
        Me._lblSlash_2.Name = "_lblSlash_2"
        '
        '_lblLabel_6
        '
        Me._lblLabel_6.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_6.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lblLabel_6, "_lblLabel_6")
        Me._lblLabel_6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_6, CType(6, Short))
        Me._lblLabel_6.Name = "_lblLabel_6"
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
        '_lblLabel_2
        '
        Me._lblLabel_2.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_2.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lblLabel_2, "_lblLabel_2")
        Me._lblLabel_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_2, CType(2, Short))
        Me._lblLabel_2.Name = "_lblLabel_2"
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.SystemColors.Control
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.Label2, "Label2")
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Name = "Label2"
        '
        'optSelection
        '
        '
        'txtField
        '
        '
        'frmPromotionChange
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.Controls.Add(Me.Button_MarginInfo)
        Me.Controls.Add(Me.cmdItemVendors)
        Me.Controls.Add(Me._Frame1_1)
        Me.Controls.Add(Me.fraReg)
        Me.Controls.Add(Me._Frame1_0)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.cmdSelect)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmPromotionChange"
        Me.ShowInTaskbar = False
        Me._Frame1_1.ResumeLayout(False)
        Me._Frame1_1.PerformLayout()
        CType(Me.ugrdStoreList, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.StoresUltraDataSource, System.ComponentModel.ISupportInitialize).EndInit()
        Me.fraReg.ResumeLayout(False)
        Me.fraReg.PerformLayout()
        CType(Me.txtPOSPrice, System.ComponentModel.ISupportInitialize).EndInit()
        Me._Frame1_0.ResumeLayout(False)
        Me._Frame1_0.PerformLayout()
        CType(Me.dtpEndDate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dtpStartDate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtMSRPPrice, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtPOSPromoPrice, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Frame2.ResumeLayout(False)
        Me.Frame2.PerformLayout()
        CType(Me.Frame1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblSlash, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.optSelection, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtField, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Public WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ugrdStoreList As Infragistics.Win.UltraWinGrid.UltraGrid
    Friend WithEvents StoresUltraDataSource As Infragistics.Win.UltraWinDataSource.UltraDataSource
    Friend WithEvents txtPOSPrice As Infragistics.Win.UltraWinEditors.UltraNumericEditor
    Friend WithEvents txtPOSPromoPrice As Infragistics.Win.UltraWinEditors.UltraNumericEditor
    Friend WithEvents txtMSRPPrice As Infragistics.Win.UltraWinEditors.UltraNumericEditor
    Public WithEvents lblDash As System.Windows.Forms.Label
    Friend WithEvents dtpStartDate As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
    Friend WithEvents dtpEndDate As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
    Friend WithEvents checkboxPriceDisplay As System.Windows.Forms.CheckBox
    Public WithEvents cmbPriceType As System.Windows.Forms.ComboBox
    Public WithEvents cmdItemVendors As System.Windows.Forms.Button
    Public WithEvents Button_MarginInfo As System.Windows.Forms.Button
#End Region
End Class