<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmItemSearch
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
	Public WithEvents cmdSearch As System.Windows.Forms.Button
	Public WithEvents cmbDistSubTeam As System.Windows.Forms.ComboBox
	Public WithEvents cmbBrand As System.Windows.Forms.ComboBox
    Public WithEvents chkIncludeDeletedItems As System.Windows.Forms.CheckBox
	Public WithEvents chkHFM As System.Windows.Forms.CheckBox
	Public WithEvents chkNotAvailable As System.Windows.Forms.CheckBox
	Public WithEvents _txtField_5 As System.Windows.Forms.TextBox
	Public WithEvents chkWFMItems As System.Windows.Forms.CheckBox
	Public WithEvents chkDiscontinued As System.Windows.Forms.CheckBox
	Public WithEvents _txtField_4 As System.Windows.Forms.TextBox
	Public WithEvents _txtField_0 As System.Windows.Forms.TextBox
	Public WithEvents _txtField_1 As System.Windows.Forms.TextBox
	Public WithEvents cmdSelect As System.Windows.Forms.Button
	Public WithEvents cmdExit As System.Windows.Forms.Button
    Public WithEvents _lblLabel_5 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_3 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_0 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_7 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_2 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_1 As System.Windows.Forms.Label
	Public WithEvents lblLabel As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	Public WithEvents txtField As Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmItemSearch))
        Dim Appearance15 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridBand1 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1)
        Dim UltraGridColumn1 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Item_Key")
        Dim UltraGridColumn2 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Description", -1, Nothing, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, False)
        Dim UltraGridColumn3 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Identifier")
        Dim UltraGridColumn4 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("SubTeam_No")
        Dim UltraGridColumn5 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Pre_Order")
        Dim UltraGridColumn6 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("EXEDistributed")
        Dim UltraGridColumn7 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Brand")
        Dim Appearance16 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance17 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance18 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance19 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance20 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance21 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance22 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance23 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance24 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance25 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance26 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance27 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance28 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridBand2 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1)
        Dim UltraGridColumn8 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Jurisdiction")
        Dim UltraGridColumn9 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Selected")
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdSearch = New System.Windows.Forms.Button()
        Me.cmdSelect = New System.Windows.Forms.Button()
        Me.cmdExit = New System.Windows.Forms.Button()
        Me.cmdClearCriteria = New System.Windows.Forms.Button()
        Me.cmbDistSubTeam = New System.Windows.Forms.ComboBox()
        Me.cmbBrand = New System.Windows.Forms.ComboBox()
        Me.chkIncludeDeletedItems = New System.Windows.Forms.CheckBox()
        Me.chkHFM = New System.Windows.Forms.CheckBox()
        Me.chkNotAvailable = New System.Windows.Forms.CheckBox()
        Me._txtField_5 = New System.Windows.Forms.TextBox()
        Me.chkWFMItems = New System.Windows.Forms.CheckBox()
        Me.chkDiscontinued = New System.Windows.Forms.CheckBox()
        Me._txtField_4 = New System.Windows.Forms.TextBox()
        Me._txtField_0 = New System.Windows.Forms.TextBox()
        Me._txtField_1 = New System.Windows.Forms.TextBox()
        Me._lblLabel_5 = New System.Windows.Forms.Label()
        Me._lblLabel_3 = New System.Windows.Forms.Label()
        Me._lblLabel_0 = New System.Windows.Forms.Label()
        Me._lblLabel_7 = New System.Windows.Forms.Label()
        Me._lblLabel_2 = New System.Windows.Forms.Label()
        Me._lblLabel_1 = New System.Windows.Forms.Label()
        Me.lblLabel = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(Me.components)
        Me.txtField = New Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray(Me.components)
        Me.ugrdSearchResults = New Infragistics.Win.UltraWinGrid.UltraGrid()
        Me.HierarchySelector1 = New HierarchySelector()
        Me.chkDefaultIdentifiers = New System.Windows.Forms.CheckBox()
        Me.txtPOSDesc = New System.Windows.Forms.TextBox()
        Me.lblPOSDesc = New System.Windows.Forms.Label()
        Me.txtVendorDesc = New System.Windows.Forms.TextBox()
        Me.lblVendorDesc = New System.Windows.Forms.Label()
        Me.UltraComboJurisdiction = New Infragistics.Win.UltraWinGrid.UltraCombo()
        Me.LabelJurisdiction = New System.Windows.Forms.Label()
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtField, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ugrdSearchResults, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.UltraComboJurisdiction, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdSearch
        '
        Me.cmdSearch.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSearch.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdSearch, "cmdSearch")
        Me.cmdSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSearch.Name = "cmdSearch"
        Me.cmdSearch.TabStop = False
        Me.ToolTip1.SetToolTip(Me.cmdSearch, resources.GetString("cmdSearch.ToolTip"))
        Me.cmdSearch.UseVisualStyleBackColor = False
        '
        'cmdSelect
        '
        Me.cmdSelect.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSelect.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdSelect, "cmdSelect")
        Me.cmdSelect.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSelect.Name = "cmdSelect"
        Me.cmdSelect.TabStop = False
        Me.ToolTip1.SetToolTip(Me.cmdSelect, resources.GetString("cmdSelect.ToolTip"))
        Me.cmdSelect.UseVisualStyleBackColor = False
        '
        'cmdExit
        '
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        resources.ApplyResources(Me.cmdExit, "cmdExit")
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.TabStop = False
        Me.ToolTip1.SetToolTip(Me.cmdExit, resources.GetString("cmdExit.ToolTip"))
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'cmdClearCriteria
        '
        Me.cmdClearCriteria.BackColor = System.Drawing.SystemColors.Control
        Me.cmdClearCriteria.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdClearCriteria, "cmdClearCriteria")
        Me.cmdClearCriteria.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdClearCriteria.Name = "cmdClearCriteria"
        Me.cmdClearCriteria.TabStop = False
        Me.ToolTip1.SetToolTip(Me.cmdClearCriteria, resources.GetString("cmdClearCriteria.ToolTip"))
        Me.cmdClearCriteria.UseVisualStyleBackColor = False
        '
        'cmbDistSubTeam
        '
        Me.cmbDistSubTeam.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbDistSubTeam.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbDistSubTeam.BackColor = System.Drawing.SystemColors.Window
        Me.cmbDistSubTeam.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbDistSubTeam.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.cmbDistSubTeam, "cmbDistSubTeam")
        Me.cmbDistSubTeam.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbDistSubTeam.Name = "cmbDistSubTeam"
        Me.cmbDistSubTeam.Sorted = True
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
        '
        'chkIncludeDeletedItems
        '
        Me.chkIncludeDeletedItems.BackColor = System.Drawing.SystemColors.Control
        resources.ApplyResources(Me.chkIncludeDeletedItems, "chkIncludeDeletedItems")
        Me.chkIncludeDeletedItems.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIncludeDeletedItems.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIncludeDeletedItems.Name = "chkIncludeDeletedItems"
        Me.chkIncludeDeletedItems.TabStop = False
        Me.chkIncludeDeletedItems.UseVisualStyleBackColor = False
        '
        'chkHFM
        '
        Me.chkHFM.BackColor = System.Drawing.SystemColors.Control
        resources.ApplyResources(Me.chkHFM, "chkHFM")
        Me.chkHFM.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkHFM.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkHFM.Name = "chkHFM"
        Me.chkHFM.TabStop = False
        Me.chkHFM.UseVisualStyleBackColor = False
        '
        'chkNotAvailable
        '
        Me.chkNotAvailable.BackColor = System.Drawing.SystemColors.Control
        resources.ApplyResources(Me.chkNotAvailable, "chkNotAvailable")
        Me.chkNotAvailable.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkNotAvailable.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkNotAvailable.Name = "chkNotAvailable"
        Me.chkNotAvailable.TabStop = False
        Me.chkNotAvailable.UseVisualStyleBackColor = False
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
        Me._txtField_5.Tag = "Integer"
        '
        'chkWFMItems
        '
        Me.chkWFMItems.BackColor = System.Drawing.SystemColors.Control
        resources.ApplyResources(Me.chkWFMItems, "chkWFMItems")
        Me.chkWFMItems.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkWFMItems.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkWFMItems.Name = "chkWFMItems"
        Me.chkWFMItems.TabStop = False
        Me.chkWFMItems.UseVisualStyleBackColor = False
        '
        'chkDiscontinued
        '
        Me.chkDiscontinued.BackColor = System.Drawing.SystemColors.Control
        resources.ApplyResources(Me.chkDiscontinued, "chkDiscontinued")
        Me.chkDiscontinued.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkDiscontinued.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkDiscontinued.Name = "chkDiscontinued"
        Me.chkDiscontinued.TabStop = False
        Me.chkDiscontinued.UseVisualStyleBackColor = False
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
        Me._txtField_4.Tag = "String"
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
        Me._txtField_0.Tag = "String"
        '
        '_txtField_1
        '
        Me._txtField_1.AcceptsReturn = True
        Me._txtField_1.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_1.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me._txtField_1, "_txtField_1")
        Me._txtField_1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_1, CType(1, Short))
        Me._txtField_1.Name = "_txtField_1"
        Me._txtField_1.Tag = "String"
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
        '_lblLabel_3
        '
        Me._lblLabel_3.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_3.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lblLabel_3, "_lblLabel_3")
        Me._lblLabel_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_3, CType(3, Short))
        Me._lblLabel_3.Name = "_lblLabel_3"
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
        '_lblLabel_7
        '
        Me._lblLabel_7.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_7.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lblLabel_7, "_lblLabel_7")
        Me._lblLabel_7.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_7, CType(7, Short))
        Me._lblLabel_7.Name = "_lblLabel_7"
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
        '_lblLabel_1
        '
        Me._lblLabel_1.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_1.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lblLabel_1, "_lblLabel_1")
        Me._lblLabel_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_1, CType(1, Short))
        Me._lblLabel_1.Name = "_lblLabel_1"
        '
        'txtField
        '
        '
        'ugrdSearchResults
        '
        Appearance15.BackColor = System.Drawing.SystemColors.ControlLight
        Appearance15.BorderColor = System.Drawing.SystemColors.InactiveCaption
        resources.ApplyResources(Appearance15.FontData, "Appearance15.FontData")
        resources.ApplyResources(Appearance15, "Appearance15")
        Appearance15.ForceApplyResources = "FontData|"
        Me.ugrdSearchResults.DisplayLayout.Appearance = Appearance15
        Me.ugrdSearchResults.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        UltraGridColumn1.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn1.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn1.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn1.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn1.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn1.Header.VisiblePosition = 0
        UltraGridColumn1.Hidden = True
        UltraGridColumn1.Width = 14
        UltraGridColumn2.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn2.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn2.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn2.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn2.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn2.Header.VisiblePosition = 1
        UltraGridColumn2.RowLayoutColumnInfo.OriginX = 2
        UltraGridColumn2.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn2.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(260, 17)
        UltraGridColumn2.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn2.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn2.Width = 431
        UltraGridColumn3.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn3.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn3.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn3.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn3.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn3.Header.VisiblePosition = 2
        UltraGridColumn3.RowLayoutColumnInfo.OriginX = 4
        UltraGridColumn3.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn3.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(184, 17)
        UltraGridColumn3.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn3.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn4.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn4.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn4.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn4.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn4.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn4.Header.VisiblePosition = 3
        UltraGridColumn4.Hidden = True
        UltraGridColumn4.Width = 14
        UltraGridColumn5.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn5.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn5.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn5.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn5.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn5.Header.VisiblePosition = 4
        UltraGridColumn5.Hidden = True
        UltraGridColumn5.Width = 14
        UltraGridColumn6.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn6.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn6.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn6.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn6.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn6.Header.VisiblePosition = 5
        UltraGridColumn6.Hidden = True
        UltraGridColumn6.Width = 14
        UltraGridColumn7.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn7.Header.VisiblePosition = 6
        UltraGridColumn7.RowLayoutColumnInfo.OriginX = 0
        UltraGridColumn7.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn7.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(202, 0)
        UltraGridColumn7.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn7.RowLayoutColumnInfo.SpanY = 2
        UltraGridBand1.Columns.AddRange(New Object() {UltraGridColumn1, UltraGridColumn2, UltraGridColumn3, UltraGridColumn4, UltraGridColumn5, UltraGridColumn6, UltraGridColumn7})
        UltraGridBand1.RowLayoutStyle = Infragistics.Win.UltraWinGrid.RowLayoutStyle.ColumnLayout
        Me.ugrdSearchResults.DisplayLayout.BandsSerializer.Add(UltraGridBand1)
        Me.ugrdSearchResults.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        resources.ApplyResources(Appearance16.FontData, "Appearance16.FontData")
        resources.ApplyResources(Appearance16, "Appearance16")
        Appearance16.ForceApplyResources = "FontData|"
        Me.ugrdSearchResults.DisplayLayout.CaptionAppearance = Appearance16
        Me.ugrdSearchResults.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[True]
        Appearance17.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance17.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance17.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance17.BorderColor = System.Drawing.SystemColors.Window
        resources.ApplyResources(Appearance17.FontData, "Appearance17.FontData")
        resources.ApplyResources(Appearance17, "Appearance17")
        Appearance17.ForceApplyResources = "FontData|"
        Me.ugrdSearchResults.DisplayLayout.GroupByBox.Appearance = Appearance17
        Appearance18.ForeColor = System.Drawing.SystemColors.GrayText
        resources.ApplyResources(Appearance18.FontData, "Appearance18.FontData")
        resources.ApplyResources(Appearance18, "Appearance18")
        Appearance18.ForceApplyResources = "FontData|"
        Me.ugrdSearchResults.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance18
        Me.ugrdSearchResults.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.ugrdSearchResults.DisplayLayout.GroupByBox.Hidden = True
        Appearance19.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance19.BackColor2 = System.Drawing.SystemColors.Control
        Appearance19.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance19.ForeColor = System.Drawing.SystemColors.GrayText
        resources.ApplyResources(Appearance19.FontData, "Appearance19.FontData")
        resources.ApplyResources(Appearance19, "Appearance19")
        Appearance19.ForceApplyResources = "FontData|"
        Me.ugrdSearchResults.DisplayLayout.GroupByBox.PromptAppearance = Appearance19
        Me.ugrdSearchResults.DisplayLayout.MaxColScrollRegions = 1
        Me.ugrdSearchResults.DisplayLayout.MaxRowScrollRegions = 1
        Appearance20.BackColor = System.Drawing.SystemColors.Window
        Appearance20.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Appearance20.FontData, "Appearance20.FontData")
        resources.ApplyResources(Appearance20, "Appearance20")
        Appearance20.ForceApplyResources = "FontData|"
        Me.ugrdSearchResults.DisplayLayout.Override.ActiveCellAppearance = Appearance20
        Me.ugrdSearchResults.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.ugrdSearchResults.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance21.BackColor = System.Drawing.SystemColors.Window
        resources.ApplyResources(Appearance21.FontData, "Appearance21.FontData")
        resources.ApplyResources(Appearance21, "Appearance21")
        Appearance21.ForceApplyResources = "FontData|"
        Me.ugrdSearchResults.DisplayLayout.Override.CardAreaAppearance = Appearance21
        Appearance22.BorderColor = System.Drawing.Color.Silver
        resources.ApplyResources(Appearance22.FontData, "Appearance22.FontData")
        Appearance22.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        resources.ApplyResources(Appearance22, "Appearance22")
        Appearance22.ForceApplyResources = "FontData|"
        Me.ugrdSearchResults.DisplayLayout.Override.CellAppearance = Appearance22
        Me.ugrdSearchResults.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        Me.ugrdSearchResults.DisplayLayout.Override.CellPadding = 0
        resources.ApplyResources(Appearance23.FontData, "Appearance23.FontData")
        resources.ApplyResources(Appearance23, "Appearance23")
        Appearance23.ForceApplyResources = "FontData|"
        Me.ugrdSearchResults.DisplayLayout.Override.FixedHeaderAppearance = Appearance23
        Appearance24.BackColor = System.Drawing.SystemColors.Control
        Appearance24.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance24.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance24.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance24.BorderColor = System.Drawing.SystemColors.Window
        resources.ApplyResources(Appearance24.FontData, "Appearance24.FontData")
        resources.ApplyResources(Appearance24, "Appearance24")
        Appearance24.ForceApplyResources = "FontData|"
        Me.ugrdSearchResults.DisplayLayout.Override.GroupByRowAppearance = Appearance24
        resources.ApplyResources(Appearance25.FontData, "Appearance25.FontData")
        resources.ApplyResources(Appearance25, "Appearance25")
        Appearance25.ForceApplyResources = "FontData|"
        Me.ugrdSearchResults.DisplayLayout.Override.HeaderAppearance = Appearance25
        Me.ugrdSearchResults.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle
        Me.ugrdSearchResults.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance26.BackColor = System.Drawing.SystemColors.ControlLight
        resources.ApplyResources(Appearance26.FontData, "Appearance26.FontData")
        resources.ApplyResources(Appearance26, "Appearance26")
        Appearance26.ForceApplyResources = "FontData|"
        Me.ugrdSearchResults.DisplayLayout.Override.RowAlternateAppearance = Appearance26
        Appearance27.BackColor = System.Drawing.SystemColors.Window
        Appearance27.BorderColor = System.Drawing.Color.Silver
        resources.ApplyResources(Appearance27.FontData, "Appearance27.FontData")
        resources.ApplyResources(Appearance27, "Appearance27")
        Appearance27.ForceApplyResources = "FontData|"
        Me.ugrdSearchResults.DisplayLayout.Override.RowAppearance = Appearance27
        Me.ugrdSearchResults.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[True]
        Me.ugrdSearchResults.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed
        Appearance28.BackColor = System.Drawing.SystemColors.ControlLight
        resources.ApplyResources(Appearance28.FontData, "Appearance28.FontData")
        resources.ApplyResources(Appearance28, "Appearance28")
        Appearance28.ForceApplyResources = "FontData|"
        Me.ugrdSearchResults.DisplayLayout.Override.TemplateAddRowAppearance = Appearance28
        Me.ugrdSearchResults.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.ugrdSearchResults.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.ugrdSearchResults.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand
        Me.ugrdSearchResults.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        resources.ApplyResources(Me.ugrdSearchResults, "ugrdSearchResults")
        Me.ugrdSearchResults.Name = "ugrdSearchResults"
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
        'chkDefaultIdentifiers
        '
        Me.chkDefaultIdentifiers.BackColor = System.Drawing.SystemColors.Control
        resources.ApplyResources(Me.chkDefaultIdentifiers, "chkDefaultIdentifiers")
        Me.chkDefaultIdentifiers.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkDefaultIdentifiers.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkDefaultIdentifiers.Name = "chkDefaultIdentifiers"
        Me.chkDefaultIdentifiers.TabStop = False
        Me.chkDefaultIdentifiers.UseVisualStyleBackColor = False
        '
        'txtPOSDesc
        '
        Me.txtPOSDesc.AcceptsReturn = True
        Me.txtPOSDesc.BackColor = System.Drawing.SystemColors.Window
        Me.txtPOSDesc.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.txtPOSDesc, "txtPOSDesc")
        Me.txtPOSDesc.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPOSDesc.Name = "txtPOSDesc"
        Me.txtPOSDesc.Tag = "String"
        '
        'lblPOSDesc
        '
        Me.lblPOSDesc.BackColor = System.Drawing.Color.Transparent
        Me.lblPOSDesc.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblPOSDesc, "lblPOSDesc")
        Me.lblPOSDesc.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPOSDesc.Name = "lblPOSDesc"
        '
        'txtVendorDesc
        '
        Me.txtVendorDesc.AcceptsReturn = True
        Me.txtVendorDesc.BackColor = System.Drawing.SystemColors.Window
        Me.txtVendorDesc.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.txtVendorDesc, "txtVendorDesc")
        Me.txtVendorDesc.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtVendorDesc.Name = "txtVendorDesc"
        Me.txtVendorDesc.Tag = "String"
        '
        'lblVendorDesc
        '
        Me.lblVendorDesc.BackColor = System.Drawing.Color.Transparent
        Me.lblVendorDesc.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblVendorDesc, "lblVendorDesc")
        Me.lblVendorDesc.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblVendorDesc.Name = "lblVendorDesc"
        '
        'UltraComboJurisdiction
        '
        Me.UltraComboJurisdiction.CheckedListSettings.CheckStateMember = "Selected"
        UltraGridColumn8.Header.VisiblePosition = 0
        UltraGridColumn9.Header.VisiblePosition = 1
        UltraGridBand2.Columns.AddRange(New Object() {UltraGridColumn8, UltraGridColumn9})
        Me.UltraComboJurisdiction.DisplayLayout.BandsSerializer.Add(UltraGridBand2)
        resources.ApplyResources(Me.UltraComboJurisdiction, "UltraComboJurisdiction")
        Me.UltraComboJurisdiction.Name = "UltraComboJurisdiction"
        '
        'LabelJurisdiction
        '
        Me.LabelJurisdiction.BackColor = System.Drawing.Color.Transparent
        Me.LabelJurisdiction.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.LabelJurisdiction, "LabelJurisdiction")
        Me.LabelJurisdiction.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelJurisdiction.Name = "LabelJurisdiction"
        '
        'frmItemSearch
        '
        Me.AcceptButton = Me.cmdSearch
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdExit
        Me.Controls.Add(Me.LabelJurisdiction)
        Me.Controls.Add(Me.UltraComboJurisdiction)
        Me.Controls.Add(Me.txtVendorDesc)
        Me.Controls.Add(Me.lblVendorDesc)
        Me.Controls.Add(Me.txtPOSDesc)
        Me.Controls.Add(Me.lblPOSDesc)
        Me.Controls.Add(Me.chkDefaultIdentifiers)
        Me.Controls.Add(Me.HierarchySelector1)
        Me.Controls.Add(Me.cmdClearCriteria)
        Me.Controls.Add(Me.ugrdSearchResults)
        Me.Controls.Add(Me.cmdSearch)
        Me.Controls.Add(Me.cmbDistSubTeam)
        Me.Controls.Add(Me.cmbBrand)
        Me.Controls.Add(Me.chkIncludeDeletedItems)
        Me.Controls.Add(Me.chkHFM)
        Me.Controls.Add(Me.chkNotAvailable)
        Me.Controls.Add(Me._txtField_5)
        Me.Controls.Add(Me.chkWFMItems)
        Me.Controls.Add(Me.chkDiscontinued)
        Me.Controls.Add(Me._txtField_4)
        Me.Controls.Add(Me._txtField_0)
        Me.Controls.Add(Me._txtField_1)
        Me.Controls.Add(Me.cmdSelect)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me._lblLabel_5)
        Me.Controls.Add(Me._lblLabel_3)
        Me.Controls.Add(Me._lblLabel_0)
        Me.Controls.Add(Me._lblLabel_7)
        Me.Controls.Add(Me._lblLabel_2)
        Me.Controls.Add(Me._lblLabel_1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmItemSearch"
        Me.ShowInTaskbar = False
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtField, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ugrdSearchResults, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.UltraComboJurisdiction, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ugrdSearchResults As Infragistics.Win.UltraWinGrid.UltraGrid
    Friend WithEvents HierarchySelector1 As HierarchySelector
    Public WithEvents cmdClearCriteria As System.Windows.Forms.Button
    Public WithEvents chkDefaultIdentifiers As System.Windows.Forms.CheckBox
    Public WithEvents txtPOSDesc As System.Windows.Forms.TextBox
    Public WithEvents lblPOSDesc As System.Windows.Forms.Label
    Public WithEvents txtVendorDesc As System.Windows.Forms.TextBox
    Public WithEvents lblVendorDesc As System.Windows.Forms.Label
    Friend WithEvents UltraComboJurisdiction As Infragistics.Win.UltraWinGrid.UltraCombo
    Public WithEvents LabelJurisdiction As System.Windows.Forms.Label
#End Region
End Class
