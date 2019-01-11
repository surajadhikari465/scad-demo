<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmPricingPrintSigns
#Region "Windows Form Designer generated code "
	<System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
        MyBase.New()

        isInitializing = True

		'This call is required by the Windows Form Designer.
        InitializeComponent()

        isInitializing = False

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
    Public WithEvents cmbBrand As System.Windows.Forms.ComboBox
  Public WithEvents cmdItemEdit As System.Windows.Forms.Button
  Public WithEvents _txtField_0 As System.Windows.Forms.TextBox
  Public WithEvents _txtField_1 As System.Windows.Forms.TextBox
  'Public WithEvents crwReport As AxCrystal.AxCrystalReport
  Public WithEvents cmdExit As System.Windows.Forms.Button
  Public WithEvents lblBrand As System.Windows.Forms.Label
  Public WithEvents lblIdentifier As System.Windows.Forms.Label
  Public WithEvents lblDesc As System.Windows.Forms.Label
  Public WithEvents txtField As Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray
  'NOTE: The following procedure is required by the Windows Form Designer
  'It can be modified using the Windows Form Designer.
  'Do not modify it using the code editor.
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Me.components = New System.ComponentModel.Container()
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPricingPrintSigns))
    Dim Appearance15 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
    Dim UltraGridBand1 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1)
    Dim UltraGridColumn1 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Item_Key")
    Dim UltraGridColumn2 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Sign_Description")
    Dim UltraGridColumn3 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Brand_Name", -1, Nothing, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Descending, False)
    Dim UltraGridColumn4 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Identifier")
    Dim UltraGridColumn5 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("SubTeam_Name")
    Dim UltraGridColumn6 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Price")
    Dim UltraGridColumn7 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("PriceChgTypeDesc", 0)
    Dim UltraGridColumn8 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("SubTeam_No", 1)
    Dim UltraGridColumn9 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("RequestedOrder", 2)
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
    Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
    Me.cmdSearch = New System.Windows.Forms.Button()
    Me.cmdItemEdit = New System.Windows.Forms.Button()
    Me.cmdExit = New System.Windows.Forms.Button()
    Me.cmdPrint = New System.Windows.Forms.Button()
    Me.lblBrand = New System.Windows.Forms.Label()
    Me.lblIdentifier = New System.Windows.Forms.Label()
    Me.lblDesc = New System.Windows.Forms.Label()
    Me.lblCategory = New System.Windows.Forms.Label()
    Me.lblSubTeam = New System.Windows.Forms.Label()
    Me.cmbBrand = New System.Windows.Forms.ComboBox()
    Me._txtField_0 = New System.Windows.Forms.TextBox()
    Me._txtField_1 = New System.Windows.Forms.TextBox()
    Me.txtField = New Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray(Me.components)
    Me.cmbCategory = New System.Windows.Forms.ComboBox()
    Me.cmbSubTeam = New System.Windows.Forms.ComboBox()
    Me.ugrdSearchResults = New Infragistics.Win.UltraWinGrid.UltraGrid()
    Me.btnIdentifiers = New System.Windows.Forms.Button()
    Me.split = New System.Windows.Forms.SplitContainer()
    Me.lblStores = New System.Windows.Forms.Label()
    Me.lblInfo2 = New System.Windows.Forms.Label()
    Me.lvStore = New System.Windows.Forms.ListView()
    Me.hdrStore = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
    Me.grpPrint = New System.Windows.Forms.GroupBox()
    Me.chkApplyNoTagLogic = New System.Windows.Forms.CheckBox()
    Me.txtBatchName = New System.Windows.Forms.TextBox()
    Me.lblInfo = New System.Windows.Forms.Label()
    Me.lbl = New System.Windows.Forms.Label()
    CType(Me.txtField, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.ugrdSearchResults, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.split, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.split.Panel1.SuspendLayout()
    Me.split.Panel2.SuspendLayout()
    Me.split.SuspendLayout()
    Me.grpPrint.SuspendLayout()
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
    'cmdItemEdit
    '
    resources.ApplyResources(Me.cmdItemEdit, "cmdItemEdit")
    Me.cmdItemEdit.BackColor = System.Drawing.SystemColors.Control
    Me.cmdItemEdit.Cursor = System.Windows.Forms.Cursors.Default
    Me.cmdItemEdit.ForeColor = System.Drawing.SystemColors.ControlText
    Me.cmdItemEdit.Name = "cmdItemEdit"
    Me.ToolTip1.SetToolTip(Me.cmdItemEdit, resources.GetString("cmdItemEdit.ToolTip"))
    Me.cmdItemEdit.UseVisualStyleBackColor = False
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
    'cmdPrint
    '
    resources.ApplyResources(Me.cmdPrint, "cmdPrint")
    Me.cmdPrint.BackColor = System.Drawing.SystemColors.Control
    Me.cmdPrint.Cursor = System.Windows.Forms.Cursors.Default
    Me.cmdPrint.ForeColor = System.Drawing.SystemColors.ControlText
    Me.cmdPrint.Image = Global.My.Resources.Resources.PrintIcon
    Me.cmdPrint.Name = "cmdPrint"
    Me.ToolTip1.SetToolTip(Me.cmdPrint, resources.GetString("cmdPrint.ToolTip"))
    Me.cmdPrint.UseVisualStyleBackColor = False
    '
    'lblBrand
    '
    resources.ApplyResources(Me.lblBrand, "lblBrand")
    Me.lblBrand.BackColor = System.Drawing.Color.Transparent
    Me.lblBrand.Cursor = System.Windows.Forms.Cursors.Default
    Me.lblBrand.ForeColor = System.Drawing.SystemColors.ControlText
    Me.lblBrand.Name = "lblBrand"
    '
    'lblIdentifier
    '
    resources.ApplyResources(Me.lblIdentifier, "lblIdentifier")
    Me.lblIdentifier.BackColor = System.Drawing.Color.Transparent
    Me.lblIdentifier.Cursor = System.Windows.Forms.Cursors.Default
    Me.lblIdentifier.ForeColor = System.Drawing.SystemColors.ControlText
    Me.lblIdentifier.Name = "lblIdentifier"
    '
    'lblDesc
    '
    resources.ApplyResources(Me.lblDesc, "lblDesc")
    Me.lblDesc.BackColor = System.Drawing.Color.Transparent
    Me.lblDesc.Cursor = System.Windows.Forms.Cursors.Default
    Me.lblDesc.ForeColor = System.Drawing.SystemColors.ControlText
    Me.lblDesc.Name = "lblDesc"
    '
    'lblCategory
    '
    resources.ApplyResources(Me.lblCategory, "lblCategory")
    Me.lblCategory.ForeColor = System.Drawing.SystemColors.ControlText
    Me.lblCategory.Name = "lblCategory"
    '
    'lblSubTeam
    '
    resources.ApplyResources(Me.lblSubTeam, "lblSubTeam")
    Me.lblSubTeam.ForeColor = System.Drawing.SystemColors.ControlText
    Me.lblSubTeam.Name = "lblSubTeam"
    '
    'cmbBrand
    '
    resources.ApplyResources(Me.cmbBrand, "cmbBrand")
    Me.cmbBrand.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
    Me.cmbBrand.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
    Me.cmbBrand.BackColor = System.Drawing.SystemColors.Window
    Me.cmbBrand.Cursor = System.Windows.Forms.Cursors.Default
    Me.cmbBrand.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
    Me.cmbBrand.ForeColor = System.Drawing.SystemColors.WindowText
    Me.cmbBrand.Name = "cmbBrand"
    Me.cmbBrand.Sorted = True
    '
    '_txtField_0
    '
    Me._txtField_0.AcceptsReturn = True
    resources.ApplyResources(Me._txtField_0, "_txtField_0")
    Me._txtField_0.BackColor = System.Drawing.SystemColors.Window
    Me._txtField_0.Cursor = System.Windows.Forms.Cursors.IBeam
    Me._txtField_0.ForeColor = System.Drawing.SystemColors.WindowText
    Me.txtField.SetIndex(Me._txtField_0, CType(0, Short))
    Me._txtField_0.Name = "_txtField_0"
    Me._txtField_0.Tag = "String"
    '
    '_txtField_1
    '
    Me._txtField_1.AcceptsReturn = True
    resources.ApplyResources(Me._txtField_1, "_txtField_1")
    Me._txtField_1.BackColor = System.Drawing.SystemColors.Window
    Me._txtField_1.Cursor = System.Windows.Forms.Cursors.IBeam
    Me._txtField_1.ForeColor = System.Drawing.SystemColors.WindowText
    Me.txtField.SetIndex(Me._txtField_1, CType(1, Short))
    Me._txtField_1.Name = "_txtField_1"
    Me._txtField_1.Tag = "String"
    '
    'txtField
    '
    '
    'cmbCategory
    '
    resources.ApplyResources(Me.cmbCategory, "cmbCategory")
    Me.cmbCategory.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
    Me.cmbCategory.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
    Me.cmbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
    Me.cmbCategory.FormattingEnabled = True
    Me.cmbCategory.Name = "cmbCategory"
    Me.cmbCategory.Sorted = True
    '
    'cmbSubTeam
    '
    resources.ApplyResources(Me.cmbSubTeam, "cmbSubTeam")
    Me.cmbSubTeam.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
    Me.cmbSubTeam.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
    Me.cmbSubTeam.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
    Me.cmbSubTeam.FormattingEnabled = True
    Me.cmbSubTeam.Name = "cmbSubTeam"
    Me.cmbSubTeam.Sorted = True
    '
    'ugrdSearchResults
    '
    resources.ApplyResources(Me.ugrdSearchResults, "ugrdSearchResults")
    Appearance15.BackColor = System.Drawing.SystemColors.ControlLight
    Appearance15.BorderColor = System.Drawing.SystemColors.InactiveCaption
    resources.ApplyResources(Appearance15.FontData, "Appearance15.FontData")
    resources.ApplyResources(Appearance15, "Appearance15")
    Appearance15.ForceApplyResources = "FontData|"
    Me.ugrdSearchResults.DisplayLayout.Appearance = Appearance15
    Me.ugrdSearchResults.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn
    UltraGridColumn1.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
    UltraGridColumn1.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
    UltraGridColumn1.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
    UltraGridColumn1.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
    UltraGridColumn1.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
    UltraGridColumn1.Header.VisiblePosition = 0
    UltraGridColumn1.Hidden = True
    UltraGridColumn1.RowLayoutColumnInfo.OriginX = 0
    UltraGridColumn1.RowLayoutColumnInfo.OriginY = 0
    UltraGridColumn1.RowLayoutColumnInfo.SpanX = 2
    UltraGridColumn1.RowLayoutColumnInfo.SpanY = 2
    UltraGridColumn1.Width = 14
    UltraGridColumn2.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
    UltraGridColumn2.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
    UltraGridColumn2.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
    UltraGridColumn2.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
    UltraGridColumn2.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
    UltraGridColumn2.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
    UltraGridColumn2.Header.Caption = resources.GetString("resource.Caption")
    UltraGridColumn2.Header.VisiblePosition = 1
    UltraGridColumn2.RowLayoutColumnInfo.OriginX = 2
    UltraGridColumn2.RowLayoutColumnInfo.OriginY = 0
    UltraGridColumn2.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(214, 0)
    UltraGridColumn2.RowLayoutColumnInfo.SpanX = 2
    UltraGridColumn2.RowLayoutColumnInfo.SpanY = 2
    UltraGridColumn3.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
    UltraGridColumn3.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
    UltraGridColumn3.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
    UltraGridColumn3.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
    UltraGridColumn3.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
    UltraGridColumn3.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
    UltraGridColumn3.Header.Caption = resources.GetString("resource.Caption1")
    UltraGridColumn3.Header.VisiblePosition = 2
    UltraGridColumn3.RowLayoutColumnInfo.OriginX = 4
    UltraGridColumn3.RowLayoutColumnInfo.OriginY = 0
    UltraGridColumn3.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(165, 0)
    UltraGridColumn3.RowLayoutColumnInfo.SpanX = 2
    UltraGridColumn3.RowLayoutColumnInfo.SpanY = 2
    UltraGridColumn4.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
    UltraGridColumn4.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
    UltraGridColumn4.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
    UltraGridColumn4.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
    UltraGridColumn4.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
    UltraGridColumn4.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
    UltraGridColumn4.Header.VisiblePosition = 3
    UltraGridColumn4.RowLayoutColumnInfo.OriginX = 6
    UltraGridColumn4.RowLayoutColumnInfo.OriginY = 0
    UltraGridColumn4.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(151, 17)
    UltraGridColumn4.RowLayoutColumnInfo.SpanX = 2
    UltraGridColumn4.RowLayoutColumnInfo.SpanY = 2
    UltraGridColumn5.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
    UltraGridColumn5.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
    UltraGridColumn5.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
    UltraGridColumn5.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
    UltraGridColumn5.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
    UltraGridColumn5.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
    UltraGridColumn5.Header.Caption = resources.GetString("resource.Caption2")
    UltraGridColumn5.Header.VisiblePosition = 4
    UltraGridColumn5.RowLayoutColumnInfo.OriginX = 8
    UltraGridColumn5.RowLayoutColumnInfo.OriginY = 0
    UltraGridColumn5.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(202, 0)
    UltraGridColumn5.RowLayoutColumnInfo.SpanX = 2
    UltraGridColumn5.RowLayoutColumnInfo.SpanY = 2
    UltraGridColumn6.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
    UltraGridColumn6.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
    UltraGridColumn6.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
    UltraGridColumn6.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
    UltraGridColumn6.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
    UltraGridColumn6.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
    UltraGridColumn6.Header.VisiblePosition = 5
    UltraGridColumn6.RowLayoutColumnInfo.OriginX = 10
    UltraGridColumn6.RowLayoutColumnInfo.OriginY = 0
    UltraGridColumn6.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(106, 0)
    UltraGridColumn6.RowLayoutColumnInfo.SpanX = 2
    UltraGridColumn6.RowLayoutColumnInfo.SpanY = 2
    UltraGridColumn7.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
    UltraGridColumn7.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
    UltraGridColumn7.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
    UltraGridColumn7.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
    UltraGridColumn7.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
    UltraGridColumn7.Header.Caption = resources.GetString("resource.Caption3")
    UltraGridColumn7.Header.VisiblePosition = 6
    UltraGridColumn7.RowLayoutColumnInfo.OriginX = 12
    UltraGridColumn7.RowLayoutColumnInfo.OriginY = 0
    UltraGridColumn7.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(25, 0)
    UltraGridColumn7.RowLayoutColumnInfo.SpanX = 2
    UltraGridColumn7.RowLayoutColumnInfo.SpanY = 2
    UltraGridColumn7.Width = 25
    UltraGridColumn8.Header.VisiblePosition = 7
    UltraGridColumn8.Hidden = True
    UltraGridColumn9.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
    UltraGridColumn9.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
    UltraGridColumn9.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
    UltraGridColumn9.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
    UltraGridColumn9.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
    UltraGridColumn9.Header.VisiblePosition = 8
    UltraGridColumn9.Hidden = True
    UltraGridColumn9.Width = 14
    UltraGridBand1.Columns.AddRange(New Object() {UltraGridColumn1, UltraGridColumn2, UltraGridColumn3, UltraGridColumn4, UltraGridColumn5, UltraGridColumn6, UltraGridColumn7, UltraGridColumn8, UltraGridColumn9})
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
    Me.ugrdSearchResults.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
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
    Me.ugrdSearchResults.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None
    Me.ugrdSearchResults.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None
    Me.ugrdSearchResults.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Extended
    Appearance28.BackColor = System.Drawing.SystemColors.ControlLight
    resources.ApplyResources(Appearance28.FontData, "Appearance28.FontData")
    resources.ApplyResources(Appearance28, "Appearance28")
    Appearance28.ForceApplyResources = "FontData|"
    Me.ugrdSearchResults.DisplayLayout.Override.TemplateAddRowAppearance = Appearance28
    Me.ugrdSearchResults.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
    Me.ugrdSearchResults.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
    Me.ugrdSearchResults.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand
    Me.ugrdSearchResults.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
    Me.ugrdSearchResults.Name = "ugrdSearchResults"
    '
    'btnIdentifiers
    '
    resources.ApplyResources(Me.btnIdentifiers, "btnIdentifiers")
    Me.btnIdentifiers.Name = "btnIdentifiers"
    Me.btnIdentifiers.UseVisualStyleBackColor = True
    '
    'split
    '
    Me.split.BackColor = System.Drawing.SystemColors.ControlDark
    resources.ApplyResources(Me.split, "split")
    Me.split.Name = "split"
    '
    'split.Panel1
    '
    Me.split.Panel1.BackColor = System.Drawing.SystemColors.Control
    Me.split.Panel1.Controls.Add(Me.lblStores)
    Me.split.Panel1.Controls.Add(Me.cmdSearch)
    Me.split.Panel1.Controls.Add(Me.lblInfo2)
    Me.split.Panel1.Controls.Add(Me.lvStore)
    Me.split.Panel1.Controls.Add(Me.cmbCategory)
    Me.split.Panel1.Controls.Add(Me.cmbBrand)
    Me.split.Panel1.Controls.Add(Me.btnIdentifiers)
    Me.split.Panel1.Controls.Add(Me.lblBrand)
    Me.split.Panel1.Controls.Add(Me.lblCategory)
    Me.split.Panel1.Controls.Add(Me.cmbSubTeam)
    Me.split.Panel1.Controls.Add(Me.lblIdentifier)
    Me.split.Panel1.Controls.Add(Me._txtField_1)
    Me.split.Panel1.Controls.Add(Me.lblSubTeam)
    Me.split.Panel1.Controls.Add(Me.lblDesc)
    Me.split.Panel1.Controls.Add(Me._txtField_0)
    '
    'split.Panel2
    '
    Me.split.Panel2.BackColor = System.Drawing.SystemColors.Control
    Me.split.Panel2.Controls.Add(Me.grpPrint)
    Me.split.Panel2.Controls.Add(Me.ugrdSearchResults)
    Me.split.Panel2.Controls.Add(Me.cmdItemEdit)
    Me.split.Panel2.Controls.Add(Me.cmdExit)
    '
    'lblStores
    '
    resources.ApplyResources(Me.lblStores, "lblStores")
    Me.lblStores.BackColor = System.Drawing.Color.Transparent
    Me.lblStores.Cursor = System.Windows.Forms.Cursors.Default
    Me.lblStores.ForeColor = System.Drawing.SystemColors.ControlText
    Me.lblStores.Name = "lblStores"
    '
    'lblInfo2
    '
    Me.lblInfo2.BackColor = System.Drawing.Color.Transparent
    Me.lblInfo2.Cursor = System.Windows.Forms.Cursors.Default
    resources.ApplyResources(Me.lblInfo2, "lblInfo2")
    Me.lblInfo2.ForeColor = System.Drawing.SystemColors.ControlDark
    Me.lblInfo2.Name = "lblInfo2"
    '
    'lvStore
    '
    resources.ApplyResources(Me.lvStore, "lvStore")
    Me.lvStore.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.hdrStore})
    Me.lvStore.FullRowSelect = True
    Me.lvStore.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
    Me.lvStore.HideSelection = False
    Me.lvStore.Name = "lvStore"
    Me.lvStore.UseCompatibleStateImageBehavior = False
    Me.lvStore.View = System.Windows.Forms.View.Details
    '
    'hdrStore
    '
    resources.ApplyResources(Me.hdrStore, "hdrStore")
    '
    'grpPrint
    '
    resources.ApplyResources(Me.grpPrint, "grpPrint")
    Me.grpPrint.Controls.Add(Me.cmdPrint)
    Me.grpPrint.Controls.Add(Me.chkApplyNoTagLogic)
    Me.grpPrint.Controls.Add(Me.txtBatchName)
    Me.grpPrint.Controls.Add(Me.lblInfo)
    Me.grpPrint.Controls.Add(Me.lbl)
    Me.grpPrint.ForeColor = System.Drawing.Color.Gray
    Me.grpPrint.Name = "grpPrint"
    Me.grpPrint.TabStop = False
    '
    'chkApplyNoTagLogic
    '
    resources.ApplyResources(Me.chkApplyNoTagLogic, "chkApplyNoTagLogic")
    Me.chkApplyNoTagLogic.ForeColor = System.Drawing.Color.Black
    Me.chkApplyNoTagLogic.Name = "chkApplyNoTagLogic"
    Me.chkApplyNoTagLogic.UseVisualStyleBackColor = True
    '
    'txtBatchName
    '
    resources.ApplyResources(Me.txtBatchName, "txtBatchName")
    Me.txtBatchName.ForeColor = System.Drawing.Color.Black
    Me.txtBatchName.Name = "txtBatchName"
    '
    'lblInfo
    '
    Me.lblInfo.BackColor = System.Drawing.SystemColors.Control
    resources.ApplyResources(Me.lblInfo, "lblInfo")
    Me.lblInfo.ForeColor = System.Drawing.Color.DarkRed
    Me.lblInfo.Name = "lblInfo"
    '
    'lbl
    '
    resources.ApplyResources(Me.lbl, "lbl")
    Me.lbl.ForeColor = System.Drawing.Color.Black
    Me.lbl.Name = "lbl"
    '
    'frmPricingPrintSigns
    '
    Me.AcceptButton = Me.cmdSearch
    resources.ApplyResources(Me, "$this")
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.BackColor = System.Drawing.SystemColors.Control
    Me.CancelButton = Me.cmdExit
    Me.Controls.Add(Me.split)
    Me.Cursor = System.Windows.Forms.Cursors.Default
    Me.MaximizeBox = False
    Me.MinimizeBox = False
    Me.Name = "frmPricingPrintSigns"
    Me.ShowIcon = False
    Me.ShowInTaskbar = False
    CType(Me.txtField, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.ugrdSearchResults, System.ComponentModel.ISupportInitialize).EndInit()
    Me.split.Panel1.ResumeLayout(False)
    Me.split.Panel1.PerformLayout()
    Me.split.Panel2.ResumeLayout(False)
    CType(Me.split, System.ComponentModel.ISupportInitialize).EndInit()
    Me.split.ResumeLayout(False)
    Me.grpPrint.ResumeLayout(False)
    Me.grpPrint.PerformLayout()
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents cmbCategory As System.Windows.Forms.ComboBox
    Friend WithEvents cmbSubTeam As System.Windows.Forms.ComboBox
    Friend WithEvents lblCategory As System.Windows.Forms.Label
    Friend WithEvents lblSubTeam As System.Windows.Forms.Label
  Private WithEvents ugrdSearchResults As Infragistics.Win.UltraWinGrid.UltraGrid
  Friend WithEvents btnIdentifiers As Button
  Friend WithEvents split As SplitContainer
  Friend WithEvents lvStore As ListView
  Friend WithEvents hdrStore As ColumnHeader
  Friend WithEvents txtBatchName As TextBox
  Friend WithEvents lbl As Label
  Friend WithEvents grpPrint As GroupBox
  Friend WithEvents lblInfo As Label
  Friend WithEvents chkApplyNoTagLogic As CheckBox
  Public WithEvents lblInfo2 As Label
  Public WithEvents lblStores As Label
  Public WithEvents cmdPrint As Button
#End Region
End Class