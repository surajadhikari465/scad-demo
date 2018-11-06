<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmVendorItems
#Region "Windows Form Designer generated code "
    <System.Diagnostics.DebuggerNonUserCode()> Private Sub New()
        MyBase.New()

        Me.IsInitializing = True

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        SetupDataTable()

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
	Public WithEvents cmdReports As System.Windows.Forms.Button
	Public WithEvents cmdFilter As System.Windows.Forms.Button
	Public WithEvents txtIdentifier As System.Windows.Forms.TextBox
	Public WithEvents txtItemDescription As System.Windows.Forms.TextBox
	Public WithEvents txtVendorItemID As System.Windows.Forms.TextBox
	Public WithEvents cmbBrand As System.Windows.Forms.ComboBox
	Public WithEvents cmbStore As System.Windows.Forms.ComboBox
	Public WithEvents cmdItemEdit As System.Windows.Forms.Button
    Public WithEvents cmdCost As System.Windows.Forms.Button
	Public WithEvents cmdEditItem As System.Windows.Forms.Button
    Public WithEvents cmdDelete As System.Windows.Forms.Button
	Public WithEvents cmdAdd As System.Windows.Forms.Button
	Public WithEvents cmdSetPrimaryVend As System.Windows.Forms.Button
	Public WithEvents cmdExit As System.Windows.Forms.Button
	Public WithEvents lblStatus As System.Windows.Forms.Label
    Public WithEvents lblIdentifier As System.Windows.Forms.Label
    Public WithEvents lblDesc As System.Windows.Forms.Label
    Public WithEvents lblVendorItemID As System.Windows.Forms.Label
    Public WithEvents lblBrand As System.Windows.Forms.Label
    Public WithEvents lblStore As System.Windows.Forms.Label
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmVendorItems))
        Dim Appearance1 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim UltraGridBand1 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1)
        Dim UltraGridColumn1 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Item_Key")
        Dim UltraGridColumn2 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Identifier")
        Dim UltraGridColumn3 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Category_Name")
        Dim UltraGridColumn4 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Item_Description")
        Dim UltraGridColumn5 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Item_ID")
        Dim UltraGridColumn6 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("SubTeam_No")
        Dim UltraGridColumn7 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Category_ID")
        Dim UltraGridColumn8 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Brand_ID")
        Dim UltraGridColumn9 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Store_No")
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
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdReports = New System.Windows.Forms.Button
        Me.cmdFilter = New System.Windows.Forms.Button
        Me.cmdItemEdit = New System.Windows.Forms.Button
        Me.cmdCost = New System.Windows.Forms.Button
        Me.cmdEditItem = New System.Windows.Forms.Button
        Me.cmdDelete = New System.Windows.Forms.Button
        Me.cmdAdd = New System.Windows.Forms.Button
        Me.cmdSetPrimaryVend = New System.Windows.Forms.Button
        Me.cmdExit = New System.Windows.Forms.Button
        Me.txtIdentifier = New System.Windows.Forms.TextBox
        Me.txtItemDescription = New System.Windows.Forms.TextBox
        Me.txtVendorItemID = New System.Windows.Forms.TextBox
        Me.cmbBrand = New System.Windows.Forms.ComboBox
        Me.cmbStore = New System.Windows.Forms.ComboBox
        Me.lblStatus = New System.Windows.Forms.Label
        Me.lblIdentifier = New System.Windows.Forms.Label
        Me.lblDesc = New System.Windows.Forms.Label
        Me.lblVendorItemID = New System.Windows.Forms.Label
        Me.lblBrand = New System.Windows.Forms.Label
        Me.lblStore = New System.Windows.Forms.Label
        Me.cmbCategory = New System.Windows.Forms.ComboBox
        Me.cmbSubTeam = New System.Windows.Forms.ComboBox
        Me.lblCategory = New System.Windows.Forms.Label
        Me.lblSubTeam = New System.Windows.Forms.Label
        Me.ugrdItemList = New Infragistics.Win.UltraWinGrid.UltraGrid
        CType(Me.ugrdItemList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdReports
        '
        Me.cmdReports.BackColor = System.Drawing.SystemColors.Control
        Me.cmdReports.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdReports, "cmdReports")
        Me.cmdReports.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdReports.Name = "cmdReports"
        Me.ToolTip1.SetToolTip(Me.cmdReports, resources.GetString("cmdReports.ToolTip"))
        Me.cmdReports.UseVisualStyleBackColor = False
        '
        'cmdFilter
        '
        Me.cmdFilter.BackColor = System.Drawing.SystemColors.Control
        Me.cmdFilter.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdFilter, "cmdFilter")
        Me.cmdFilter.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdFilter.Name = "cmdFilter"
        Me.ToolTip1.SetToolTip(Me.cmdFilter, resources.GetString("cmdFilter.ToolTip"))
        Me.cmdFilter.UseVisualStyleBackColor = False
        '
        'cmdItemEdit
        '
        Me.cmdItemEdit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdItemEdit.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdItemEdit, "cmdItemEdit")
        Me.cmdItemEdit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdItemEdit.Name = "cmdItemEdit"
        Me.ToolTip1.SetToolTip(Me.cmdItemEdit, resources.GetString("cmdItemEdit.ToolTip"))
        Me.cmdItemEdit.UseVisualStyleBackColor = False
        '
        'cmdCost
        '
        Me.cmdCost.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCost.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdCost, "cmdCost")
        Me.cmdCost.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCost.Name = "cmdCost"
        Me.ToolTip1.SetToolTip(Me.cmdCost, resources.GetString("cmdCost.ToolTip"))
        Me.cmdCost.UseVisualStyleBackColor = False
        '
        'cmdEditItem
        '
        Me.cmdEditItem.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEditItem.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdEditItem, "cmdEditItem")
        Me.cmdEditItem.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEditItem.Name = "cmdEditItem"
        Me.cmdEditItem.Tag = "B"
        Me.ToolTip1.SetToolTip(Me.cmdEditItem, resources.GetString("cmdEditItem.ToolTip"))
        Me.cmdEditItem.UseVisualStyleBackColor = False
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
        'cmdSetPrimaryVend
        '
        Me.cmdSetPrimaryVend.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSetPrimaryVend.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdSetPrimaryVend, "cmdSetPrimaryVend")
        Me.cmdSetPrimaryVend.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSetPrimaryVend.Name = "cmdSetPrimaryVend"
        Me.ToolTip1.SetToolTip(Me.cmdSetPrimaryVend, resources.GetString("cmdSetPrimaryVend.ToolTip"))
        Me.cmdSetPrimaryVend.UseVisualStyleBackColor = False
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
        'txtItemDescription
        '
        Me.txtItemDescription.AcceptsReturn = True
        Me.txtItemDescription.BackColor = System.Drawing.SystemColors.Window
        Me.txtItemDescription.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.txtItemDescription, "txtItemDescription")
        Me.txtItemDescription.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtItemDescription.Name = "txtItemDescription"
        Me.txtItemDescription.Tag = "String"
        '
        'txtVendorItemID
        '
        Me.txtVendorItemID.AcceptsReturn = True
        Me.txtVendorItemID.BackColor = System.Drawing.SystemColors.Window
        Me.txtVendorItemID.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.txtVendorItemID, "txtVendorItemID")
        Me.txtVendorItemID.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtVendorItemID.Name = "txtVendorItemID"
        Me.txtVendorItemID.Tag = "Integer"
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
        'lblStatus
        '
        Me.lblStatus.BackColor = System.Drawing.Color.Transparent
        Me.lblStatus.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblStatus, "lblStatus")
        Me.lblStatus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblStatus.Name = "lblStatus"
        '
        'lblIdentifier
        '
        Me.lblIdentifier.BackColor = System.Drawing.Color.Transparent
        Me.lblIdentifier.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblIdentifier, "lblIdentifier")
        Me.lblIdentifier.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblIdentifier.Name = "lblIdentifier"
        '
        'lblDesc
        '
        Me.lblDesc.BackColor = System.Drawing.Color.Transparent
        Me.lblDesc.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblDesc, "lblDesc")
        Me.lblDesc.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDesc.Name = "lblDesc"
        '
        'lblVendorItemID
        '
        Me.lblVendorItemID.BackColor = System.Drawing.Color.Transparent
        Me.lblVendorItemID.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblVendorItemID, "lblVendorItemID")
        Me.lblVendorItemID.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblVendorItemID.Name = "lblVendorItemID"
        '
        'lblBrand
        '
        Me.lblBrand.BackColor = System.Drawing.Color.Transparent
        Me.lblBrand.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblBrand, "lblBrand")
        Me.lblBrand.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBrand.Name = "lblBrand"
        '
        'lblStore
        '
        Me.lblStore.BackColor = System.Drawing.Color.Transparent
        Me.lblStore.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblStore, "lblStore")
        Me.lblStore.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblStore.Name = "lblStore"
        '
        'cmbCategory
        '
        Me.cmbCategory.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbCategory.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.cmbCategory, "cmbCategory")
        Me.cmbCategory.FormattingEnabled = True
        Me.cmbCategory.Name = "cmbCategory"
        Me.cmbCategory.Sorted = True
        '
        'cmbSubTeam
        '
        Me.cmbSubTeam.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbSubTeam.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbSubTeam.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.cmbSubTeam, "cmbSubTeam")
        Me.cmbSubTeam.FormattingEnabled = True
        Me.cmbSubTeam.Name = "cmbSubTeam"
        Me.cmbSubTeam.Sorted = True
        '
        'lblCategory
        '
        resources.ApplyResources(Me.lblCategory, "lblCategory")
        Me.lblCategory.Name = "lblCategory"
        '
        'lblSubTeam
        '
        resources.ApplyResources(Me.lblSubTeam, "lblSubTeam")
        Me.lblSubTeam.Name = "lblSubTeam"
        '
        'ugrdItemList
        '
        Appearance1.BackColor = System.Drawing.SystemColors.ControlLight
        Appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.ugrdItemList.DisplayLayout.Appearance = Appearance1
        Me.ugrdItemList.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        UltraGridColumn1.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn1.Header.VisiblePosition = 0
        UltraGridColumn1.Hidden = True
        UltraGridColumn2.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn2.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn2.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn2.Header.VisiblePosition = 1
        UltraGridColumn3.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn3.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn3.Header.Caption = resources.GetString("resource.Caption")
        UltraGridColumn3.Header.VisiblePosition = 2
        UltraGridColumn4.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn4.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn4.Header.Caption = resources.GetString("resource.Caption1")
        UltraGridColumn4.Header.VisiblePosition = 3
        UltraGridColumn5.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn5.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn5.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn5.Header.Caption = resources.GetString("resource.Caption2")
        UltraGridColumn5.Header.VisiblePosition = 4
        UltraGridColumn5.MinWidth = 14
        UltraGridColumn6.Header.VisiblePosition = 5
        UltraGridColumn6.Hidden = True
        UltraGridColumn7.Header.VisiblePosition = 6
        UltraGridColumn7.Hidden = True
        UltraGridColumn8.Header.VisiblePosition = 7
        UltraGridColumn8.Hidden = True
        UltraGridColumn9.Header.VisiblePosition = 8
        UltraGridColumn9.Hidden = True
        UltraGridBand1.Columns.AddRange(New Object() {UltraGridColumn1, UltraGridColumn2, UltraGridColumn3, UltraGridColumn4, UltraGridColumn5, UltraGridColumn6, UltraGridColumn7, UltraGridColumn8, UltraGridColumn9})
        UltraGridBand1.RowLayoutStyle = Infragistics.Win.UltraWinGrid.RowLayoutStyle.None
        Me.ugrdItemList.DisplayLayout.BandsSerializer.Add(UltraGridBand1)
        Me.ugrdItemList.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Appearance2.FontData.BoldAsString = resources.GetString("resource.BoldAsString")
        Me.ugrdItemList.DisplayLayout.CaptionAppearance = Appearance2
        Me.ugrdItemList.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Appearance3.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance3.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance3.BorderColor = System.Drawing.SystemColors.Window
        Me.ugrdItemList.DisplayLayout.GroupByBox.Appearance = Appearance3
        Appearance4.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugrdItemList.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance4
        Me.ugrdItemList.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.ugrdItemList.DisplayLayout.GroupByBox.Hidden = True
        Appearance5.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance5.BackColor2 = System.Drawing.SystemColors.Control
        Appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance5.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugrdItemList.DisplayLayout.GroupByBox.PromptAppearance = Appearance5
        Me.ugrdItemList.DisplayLayout.MaxColScrollRegions = 1
        Me.ugrdItemList.DisplayLayout.MaxRowScrollRegions = 1
        Appearance6.BackColor = System.Drawing.SystemColors.Window
        Appearance6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ugrdItemList.DisplayLayout.Override.ActiveCellAppearance = Appearance6
        Me.ugrdItemList.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.ugrdItemList.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance7.BackColor = System.Drawing.SystemColors.Window
        Me.ugrdItemList.DisplayLayout.Override.CardAreaAppearance = Appearance7
        Appearance8.BorderColor = System.Drawing.Color.Silver
        Appearance8.FontData.BoldAsString = resources.GetString("resource.BoldAsString1")
        Appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.ugrdItemList.DisplayLayout.Override.CellAppearance = Appearance8
        Me.ugrdItemList.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.ugrdItemList.DisplayLayout.Override.CellPadding = 0
        Appearance9.FontData.BoldAsString = resources.GetString("resource.BoldAsString2")
        Me.ugrdItemList.DisplayLayout.Override.FixedHeaderAppearance = Appearance9
        Appearance10.BackColor = System.Drawing.SystemColors.Control
        Appearance10.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance10.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance10.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance10.BorderColor = System.Drawing.SystemColors.Window
        Me.ugrdItemList.DisplayLayout.Override.GroupByRowAppearance = Appearance10
        Appearance11.FontData.BoldAsString = resources.GetString("resource.BoldAsString3")
        Appearance11.TextHAlign = Infragistics.Win.HAlign.Left
        Me.ugrdItemList.DisplayLayout.Override.HeaderAppearance = Appearance11
        Me.ugrdItemList.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle
        Me.ugrdItemList.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance12.BackColor = System.Drawing.SystemColors.ControlLight
        Me.ugrdItemList.DisplayLayout.Override.RowAlternateAppearance = Appearance12
        Appearance13.BackColor = System.Drawing.SystemColors.Window
        Appearance13.BorderColor = System.Drawing.Color.Silver
        Me.ugrdItemList.DisplayLayout.Override.RowAppearance = Appearance13
        Me.ugrdItemList.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[True]
        Me.ugrdItemList.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed
        Me.ugrdItemList.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.ugrdItemList.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.ugrdItemList.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.[Single]
        Appearance14.BackColor = System.Drawing.SystemColors.ControlLight
        Me.ugrdItemList.DisplayLayout.Override.TemplateAddRowAppearance = Appearance14
        Me.ugrdItemList.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.ugrdItemList.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.ugrdItemList.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand
        Me.ugrdItemList.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        resources.ApplyResources(Me.ugrdItemList, "ugrdItemList")
        Me.ugrdItemList.Name = "ugrdItemList"
        '
        'frmVendorItems
        '
        Me.AcceptButton = Me.cmdFilter
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.Controls.Add(Me.ugrdItemList)
        Me.Controls.Add(Me.cmbCategory)
        Me.Controls.Add(Me.cmbSubTeam)
        Me.Controls.Add(Me.lblCategory)
        Me.Controls.Add(Me.lblSubTeam)
        Me.Controls.Add(Me.cmdReports)
        Me.Controls.Add(Me.cmdFilter)
        Me.Controls.Add(Me.txtIdentifier)
        Me.Controls.Add(Me.txtItemDescription)
        Me.Controls.Add(Me.txtVendorItemID)
        Me.Controls.Add(Me.cmbBrand)
        Me.Controls.Add(Me.cmbStore)
        Me.Controls.Add(Me.cmdItemEdit)
        Me.Controls.Add(Me.cmdCost)
        Me.Controls.Add(Me.cmdEditItem)
        Me.Controls.Add(Me.cmdDelete)
        Me.Controls.Add(Me.cmdAdd)
        Me.Controls.Add(Me.cmdSetPrimaryVend)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.lblIdentifier)
        Me.Controls.Add(Me.lblDesc)
        Me.Controls.Add(Me.lblVendorItemID)
        Me.Controls.Add(Me.lblBrand)
        Me.Controls.Add(Me.lblStore)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmVendorItems"
        Me.ShowInTaskbar = False
        CType(Me.ugrdItemList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cmbCategory As System.Windows.Forms.ComboBox
    Friend WithEvents cmbSubTeam As System.Windows.Forms.ComboBox
    Friend WithEvents lblCategory As System.Windows.Forms.Label
    Friend WithEvents lblSubTeam As System.Windows.Forms.Label
    Friend WithEvents ugrdItemList As Infragistics.Win.UltraWinGrid.UltraGrid
#End Region 
End Class