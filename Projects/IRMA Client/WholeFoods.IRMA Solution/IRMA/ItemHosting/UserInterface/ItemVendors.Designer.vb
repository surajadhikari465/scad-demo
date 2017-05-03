<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmItemVendors
#Region "Windows Form Designer generated code "
    <System.Diagnostics.DebuggerNonUserCode()> Private Sub New()
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
	Public WithEvents cmdExit As System.Windows.Forms.Button
    'Public WithEvents gridVendorList As AxSSDataWidgets_B.AxSSDBGrid
	Public WithEvents cmdCost As System.Windows.Forms.Button
	Public WithEvents cmdDelete As System.Windows.Forms.Button
	Public WithEvents cmdEditItem As System.Windows.Forms.Button
	Public WithEvents cmdAdd As System.Windows.Forms.Button
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmItemVendors))
        Dim Appearance17 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridBand1 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1)
        Dim UltraGridColumn1 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Vendor_ID")
        Dim Appearance18 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance19 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridColumn2 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyName")
        Dim UltraGridColumn3 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Location")
        Dim UltraGridColumn4 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Item_ID")
        Dim UltraGridColumn5 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("RetailCasePack", 0)
        Dim UltraGridColumn6 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("VendorItemStatus", 1)
        Dim UltraGridColumn7 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("VendorItemDescription", 2)
        Dim UltraGridColumn8 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("VendorItemStatusFull", 3)
        Dim UltraGridColumn9 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("CurrencyCode", 4, Nothing, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, False)
        Dim UltraGridColumn10 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("CurrencyID", 5)
        Dim Appearance20 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance21 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance22 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance23 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance24 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance25 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance26 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance27 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance28 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance29 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance30 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance31 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance32 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdExit = New System.Windows.Forms.Button()
        Me.cmdCost = New System.Windows.Forms.Button()
        Me.cmdDelete = New System.Windows.Forms.Button()
        Me.cmdEditItem = New System.Windows.Forms.Button()
        Me.cmdAdd = New System.Windows.Forms.Button()
        Me.ugrdVend = New Infragistics.Win.UltraWinGrid.UltraGrid()
        CType(Me.ugrdVend, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
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
        'cmdCost
        '
        resources.ApplyResources(Me.cmdCost, "cmdCost")
        Me.cmdCost.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCost.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCost.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCost.Name = "cmdCost"
        Me.ToolTip1.SetToolTip(Me.cmdCost, resources.GetString("cmdCost.ToolTip"))
        Me.cmdCost.UseVisualStyleBackColor = False
        '
        'cmdDelete
        '
        resources.ApplyResources(Me.cmdDelete, "cmdDelete")
        Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDelete.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDelete.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDelete.Name = "cmdDelete"
        Me.ToolTip1.SetToolTip(Me.cmdDelete, resources.GetString("cmdDelete.ToolTip"))
        Me.cmdDelete.UseVisualStyleBackColor = False
        '
        'cmdEditItem
        '
        resources.ApplyResources(Me.cmdEditItem, "cmdEditItem")
        Me.cmdEditItem.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEditItem.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEditItem.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEditItem.Name = "cmdEditItem"
        Me.cmdEditItem.Tag = "B"
        Me.ToolTip1.SetToolTip(Me.cmdEditItem, resources.GetString("cmdEditItem.ToolTip"))
        Me.cmdEditItem.UseVisualStyleBackColor = False
        '
        'cmdAdd
        '
        resources.ApplyResources(Me.cmdAdd, "cmdAdd")
        Me.cmdAdd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAdd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAdd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAdd.Name = "cmdAdd"
        Me.ToolTip1.SetToolTip(Me.cmdAdd, resources.GetString("cmdAdd.ToolTip"))
        Me.cmdAdd.UseVisualStyleBackColor = False
        '
        'ugrdVend
        '
        resources.ApplyResources(Me.ugrdVend, "ugrdVend")
        Appearance17.BackColor = System.Drawing.SystemColors.ControlLight
        Appearance17.BorderColor = System.Drawing.SystemColors.InactiveCaption
        resources.ApplyResources(Appearance17.FontData, "Appearance17.FontData")
        resources.ApplyResources(Appearance17, "Appearance17")
        Appearance17.ForceApplyResources = "FontData|"
        Me.ugrdVend.DisplayLayout.Appearance = Appearance17
        Me.ugrdVend.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        UltraGridColumn1.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.VisibleRows
        UltraGridColumn1.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        resources.ApplyResources(Appearance18, "Appearance18")
        resources.ApplyResources(Appearance18.FontData, "Appearance18.FontData")
        Appearance18.ForceApplyResources = "FontData|"
        UltraGridColumn1.CellAppearance = Appearance18
        UltraGridColumn1.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        resources.ApplyResources(Appearance19, "Appearance19")
        resources.ApplyResources(Appearance19.FontData, "Appearance19.FontData")
        Appearance19.ForceApplyResources = "FontData|"
        UltraGridColumn1.Header.Appearance = Appearance19
        UltraGridColumn1.Header.Caption = resources.GetString("resource.Caption")
        UltraGridColumn1.Header.VisiblePosition = 0
        UltraGridColumn1.MinWidth = 50
        UltraGridColumn1.Width = 57
        UltraGridColumn2.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.VisibleRows
        UltraGridColumn2.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn2.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn2.Header.Caption = resources.GetString("resource.Caption1")
        UltraGridColumn2.Header.VisiblePosition = 1
        UltraGridColumn2.MinLength = 50
        UltraGridColumn2.MinWidth = 185
        UltraGridColumn2.Width = 213
        UltraGridColumn3.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.VisibleRows
        UltraGridColumn3.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn3.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn3.Header.VisiblePosition = 3
        UltraGridColumn3.MinWidth = 50
        UltraGridColumn3.Width = 79
        UltraGridColumn4.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.VisibleRows
        UltraGridColumn4.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn4.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn4.Header.Caption = resources.GetString("resource.Caption2")
        UltraGridColumn4.Header.VisiblePosition = 6
        UltraGridColumn4.MinWidth = 50
        UltraGridColumn4.Width = 108
        UltraGridColumn5.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.VisibleRows
        UltraGridColumn5.Header.Caption = resources.GetString("resource.Caption3")
        UltraGridColumn5.Header.VisiblePosition = 5
        UltraGridColumn5.MinWidth = 50
        UltraGridColumn5.Width = 65
        UltraGridColumn6.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.VisibleRows
        UltraGridColumn6.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn6.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn6.Header.Caption = resources.GetString("resource.Caption4")
        UltraGridColumn6.Header.VisiblePosition = 7
        UltraGridColumn6.MinWidth = 50
        UltraGridColumn6.Width = 57
        UltraGridColumn7.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.VisibleRows
        UltraGridColumn7.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn7.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn7.Header.Caption = resources.GetString("resource.Caption5")
        UltraGridColumn7.Header.VisiblePosition = 4
        UltraGridColumn7.MinWidth = 50
        UltraGridColumn7.Width = 100
        UltraGridColumn8.Header.VisiblePosition = 8
        UltraGridColumn8.Hidden = True
        UltraGridColumn8.Width = 102
        UltraGridColumn9.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.VisibleRows
        UltraGridColumn9.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn9.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn9.Header.Caption = resources.GetString("resource.Caption6")
        UltraGridColumn9.Header.VisiblePosition = 2
        UltraGridColumn9.Width = 100
        UltraGridColumn10.Header.VisiblePosition = 9
        UltraGridColumn10.Hidden = True
        UltraGridColumn10.Width = 103
        UltraGridBand1.Columns.AddRange(New Object() {UltraGridColumn1, UltraGridColumn2, UltraGridColumn3, UltraGridColumn4, UltraGridColumn5, UltraGridColumn6, UltraGridColumn7, UltraGridColumn8, UltraGridColumn9, UltraGridColumn10})
        UltraGridBand1.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinBand
        UltraGridBand1.Override.ColumnAutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.VisibleRows
        Me.ugrdVend.DisplayLayout.BandsSerializer.Add(UltraGridBand1)
        Me.ugrdVend.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        resources.ApplyResources(Appearance20.FontData, "Appearance20.FontData")
        resources.ApplyResources(Appearance20, "Appearance20")
        Appearance20.ForceApplyResources = "FontData|"
        Me.ugrdVend.DisplayLayout.CaptionAppearance = Appearance20
        Me.ugrdVend.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Appearance21.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance21.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance21.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance21.BorderColor = System.Drawing.SystemColors.Window
        resources.ApplyResources(Appearance21.FontData, "Appearance21.FontData")
        resources.ApplyResources(Appearance21, "Appearance21")
        Appearance21.ForceApplyResources = "FontData|"
        Me.ugrdVend.DisplayLayout.GroupByBox.Appearance = Appearance21
        Appearance22.ForeColor = System.Drawing.SystemColors.GrayText
        resources.ApplyResources(Appearance22.FontData, "Appearance22.FontData")
        resources.ApplyResources(Appearance22, "Appearance22")
        Appearance22.ForceApplyResources = "FontData|"
        Me.ugrdVend.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance22
        Me.ugrdVend.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.ugrdVend.DisplayLayout.GroupByBox.Hidden = True
        Appearance23.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance23.BackColor2 = System.Drawing.SystemColors.Control
        Appearance23.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance23.ForeColor = System.Drawing.SystemColors.GrayText
        resources.ApplyResources(Appearance23.FontData, "Appearance23.FontData")
        resources.ApplyResources(Appearance23, "Appearance23")
        Appearance23.ForceApplyResources = "FontData|"
        Me.ugrdVend.DisplayLayout.GroupByBox.PromptAppearance = Appearance23
        Me.ugrdVend.DisplayLayout.MaxColScrollRegions = 1
        Me.ugrdVend.DisplayLayout.MaxRowScrollRegions = 1
        Appearance24.BackColor = System.Drawing.SystemColors.Window
        Appearance24.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Appearance24.FontData, "Appearance24.FontData")
        resources.ApplyResources(Appearance24, "Appearance24")
        Appearance24.ForceApplyResources = "FontData|"
        Me.ugrdVend.DisplayLayout.Override.ActiveCellAppearance = Appearance24
        Me.ugrdVend.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.ugrdVend.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance25.BackColor = System.Drawing.SystemColors.Window
        resources.ApplyResources(Appearance25.FontData, "Appearance25.FontData")
        resources.ApplyResources(Appearance25, "Appearance25")
        Appearance25.ForceApplyResources = "FontData|"
        Me.ugrdVend.DisplayLayout.Override.CardAreaAppearance = Appearance25
        Appearance26.BorderColor = System.Drawing.Color.Silver
        resources.ApplyResources(Appearance26.FontData, "Appearance26.FontData")
        Appearance26.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        resources.ApplyResources(Appearance26, "Appearance26")
        Appearance26.ForceApplyResources = "FontData|"
        Me.ugrdVend.DisplayLayout.Override.CellAppearance = Appearance26
        Me.ugrdVend.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.ugrdVend.DisplayLayout.Override.CellPadding = 0
        resources.ApplyResources(Appearance27.FontData, "Appearance27.FontData")
        resources.ApplyResources(Appearance27, "Appearance27")
        Appearance27.ForceApplyResources = "FontData|"
        Me.ugrdVend.DisplayLayout.Override.FixedHeaderAppearance = Appearance27
        Appearance28.BackColor = System.Drawing.SystemColors.Control
        Appearance28.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance28.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance28.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance28.BorderColor = System.Drawing.SystemColors.Window
        resources.ApplyResources(Appearance28.FontData, "Appearance28.FontData")
        resources.ApplyResources(Appearance28, "Appearance28")
        Appearance28.ForceApplyResources = "FontData|"
        Me.ugrdVend.DisplayLayout.Override.GroupByRowAppearance = Appearance28
        resources.ApplyResources(Appearance29.FontData, "Appearance29.FontData")
        resources.ApplyResources(Appearance29, "Appearance29")
        Appearance29.ForceApplyResources = "FontData|"
        Me.ugrdVend.DisplayLayout.Override.HeaderAppearance = Appearance29
        Me.ugrdVend.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle
        Me.ugrdVend.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance30.BackColor = System.Drawing.SystemColors.ControlLight
        resources.ApplyResources(Appearance30.FontData, "Appearance30.FontData")
        resources.ApplyResources(Appearance30, "Appearance30")
        Appearance30.ForceApplyResources = "FontData|"
        Me.ugrdVend.DisplayLayout.Override.RowAlternateAppearance = Appearance30
        Appearance31.BackColor = System.Drawing.SystemColors.Window
        Appearance31.BorderColor = System.Drawing.Color.Silver
        resources.ApplyResources(Appearance31.FontData, "Appearance31.FontData")
        resources.ApplyResources(Appearance31, "Appearance31")
        Appearance31.ForceApplyResources = "FontData|"
        Me.ugrdVend.DisplayLayout.Override.RowAppearance = Appearance31
        Me.ugrdVend.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[True]
        Me.ugrdVend.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed
        Me.ugrdVend.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.ugrdVend.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.ugrdVend.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.[Single]
        Appearance32.BackColor = System.Drawing.SystemColors.ControlLight
        resources.ApplyResources(Appearance32.FontData, "Appearance32.FontData")
        resources.ApplyResources(Appearance32, "Appearance32")
        Appearance32.ForceApplyResources = "FontData|"
        Me.ugrdVend.DisplayLayout.Override.TemplateAddRowAppearance = Appearance32
        Me.ugrdVend.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.ugrdVend.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.ugrdVend.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand
        Me.ugrdVend.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        Me.ugrdVend.Name = "ugrdVend"
        '
        'frmItemVendors
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.ugrdVend)
        Me.Controls.Add(Me.cmdCost)
        Me.Controls.Add(Me.cmdDelete)
        Me.Controls.Add(Me.cmdEditItem)
        Me.Controls.Add(Me.cmdAdd)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmItemVendors"
        Me.ShowInTaskbar = False
        CType(Me.ugrdVend, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ugrdVend As Infragistics.Win.UltraWinGrid.UltraGrid
#End Region
End Class