<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmShipperList
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
	Public WithEvents cmdExit As System.Windows.Forms.Button
	Public WithEvents cmdAddItem As System.Windows.Forms.Button
	Public WithEvents cmdDeleteItem As System.Windows.Forms.Button
	Public WithEvents cmdEditItem As System.Windows.Forms.Button
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmShipperList))
        Dim Appearance1 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim UltraGridBand1 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1)
        Dim UltraGridColumn1 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Item_Key")
        Dim UltraGridColumn2 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Identifier")
        Dim UltraGridColumn3 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Item_Description", -1, Nothing, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, False)
        Dim UltraGridColumn4 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Quantity")
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
        Me.cmdExit = New System.Windows.Forms.Button
        Me.cmdAddItem = New System.Windows.Forms.Button
        Me.cmdDeleteItem = New System.Windows.Forms.Button
        Me.cmdEditItem = New System.Windows.Forms.Button
        Me.ugrdShipper = New Infragistics.Win.UltraWinGrid.UltraGrid
        CType(Me.ugrdShipper, System.ComponentModel.ISupportInitialize).BeginInit()
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
        'cmdAddItem
        '
        Me.cmdAddItem.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddItem.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdAddItem, "cmdAddItem")
        Me.cmdAddItem.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddItem.Name = "cmdAddItem"
        Me.ToolTip1.SetToolTip(Me.cmdAddItem, resources.GetString("cmdAddItem.ToolTip"))
        Me.cmdAddItem.UseVisualStyleBackColor = False
        '
        'cmdDeleteItem
        '
        Me.cmdDeleteItem.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDeleteItem.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdDeleteItem, "cmdDeleteItem")
        Me.cmdDeleteItem.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDeleteItem.Name = "cmdDeleteItem"
        Me.ToolTip1.SetToolTip(Me.cmdDeleteItem, resources.GetString("cmdDeleteItem.ToolTip"))
        Me.cmdDeleteItem.UseVisualStyleBackColor = False
        '
        'cmdEditItem
        '
        Me.cmdEditItem.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEditItem.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdEditItem, "cmdEditItem")
        Me.cmdEditItem.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEditItem.Name = "cmdEditItem"
        Me.ToolTip1.SetToolTip(Me.cmdEditItem, resources.GetString("cmdEditItem.ToolTip"))
        Me.cmdEditItem.UseVisualStyleBackColor = False
        '
        'ugrdShipper
        '
        Appearance1.BackColor = System.Drawing.SystemColors.ControlLight
        Appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption
        resources.ApplyResources(Appearance1.FontData, "Appearance1.FontData")
        resources.ApplyResources(Appearance1, "Appearance1")
        Appearance1.ForceApplyResources = "FontData|"
        Me.ugrdShipper.DisplayLayout.Appearance = Appearance1
        Me.ugrdShipper.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        UltraGridColumn1.Header.VisiblePosition = 0
        UltraGridColumn1.Hidden = True
        UltraGridColumn2.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn2.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn2.Header.VisiblePosition = 1
        UltraGridColumn2.MinWidth = 116
        UltraGridColumn2.Width = 189
        UltraGridColumn3.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn3.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn3.Header.Caption = resources.GetString("resource.Caption")
        UltraGridColumn3.Header.VisiblePosition = 2
        UltraGridColumn3.Width = 169
        UltraGridColumn4.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn4.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn4.Header.VisiblePosition = 3
        UltraGridColumn4.MinWidth = 90
        UltraGridColumn4.Width = 166
        UltraGridBand1.Columns.AddRange(New Object() {UltraGridColumn1, UltraGridColumn2, UltraGridColumn3, UltraGridColumn4})
        Me.ugrdShipper.DisplayLayout.BandsSerializer.Add(UltraGridBand1)
        Me.ugrdShipper.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Appearance2.FontData.BoldAsString = resources.GetString("resource.BoldAsString")
        resources.ApplyResources(Appearance2, "Appearance2")
        Appearance2.ForceApplyResources = ""
        Me.ugrdShipper.DisplayLayout.CaptionAppearance = Appearance2
        Me.ugrdShipper.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Appearance3.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance3.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance3.BorderColor = System.Drawing.SystemColors.Window
        resources.ApplyResources(Appearance3.FontData, "Appearance3.FontData")
        resources.ApplyResources(Appearance3, "Appearance3")
        Appearance3.ForceApplyResources = "FontData|"
        Me.ugrdShipper.DisplayLayout.GroupByBox.Appearance = Appearance3
        Appearance4.ForeColor = System.Drawing.SystemColors.GrayText
        resources.ApplyResources(Appearance4.FontData, "Appearance4.FontData")
        resources.ApplyResources(Appearance4, "Appearance4")
        Appearance4.ForceApplyResources = "FontData|"
        Me.ugrdShipper.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance4
        Me.ugrdShipper.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.ugrdShipper.DisplayLayout.GroupByBox.Hidden = True
        Appearance5.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance5.BackColor2 = System.Drawing.SystemColors.Control
        Appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance5.ForeColor = System.Drawing.SystemColors.GrayText
        resources.ApplyResources(Appearance5.FontData, "Appearance5.FontData")
        resources.ApplyResources(Appearance5, "Appearance5")
        Appearance5.ForceApplyResources = "FontData|"
        Me.ugrdShipper.DisplayLayout.GroupByBox.PromptAppearance = Appearance5
        Me.ugrdShipper.DisplayLayout.MaxColScrollRegions = 1
        Me.ugrdShipper.DisplayLayout.MaxRowScrollRegions = 1
        Appearance6.BackColor = System.Drawing.SystemColors.Window
        Appearance6.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Appearance6.FontData, "Appearance6.FontData")
        resources.ApplyResources(Appearance6, "Appearance6")
        Appearance6.ForceApplyResources = "FontData|"
        Me.ugrdShipper.DisplayLayout.Override.ActiveCellAppearance = Appearance6
        Me.ugrdShipper.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.ugrdShipper.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance7.BackColor = System.Drawing.SystemColors.Window
        resources.ApplyResources(Appearance7.FontData, "Appearance7.FontData")
        resources.ApplyResources(Appearance7, "Appearance7")
        Appearance7.ForceApplyResources = "FontData|"
        Me.ugrdShipper.DisplayLayout.Override.CardAreaAppearance = Appearance7
        Appearance8.BorderColor = System.Drawing.Color.Silver
        Appearance8.FontData.BoldAsString = resources.GetString("resource.BoldAsString1")
        Appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        resources.ApplyResources(Appearance8, "Appearance8")
        Appearance8.ForceApplyResources = ""
        Me.ugrdShipper.DisplayLayout.Override.CellAppearance = Appearance8
        Me.ugrdShipper.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.ugrdShipper.DisplayLayout.Override.CellPadding = 0
        Appearance9.FontData.BoldAsString = resources.GetString("resource.BoldAsString2")
        resources.ApplyResources(Appearance9, "Appearance9")
        Appearance9.ForceApplyResources = ""
        Me.ugrdShipper.DisplayLayout.Override.FixedHeaderAppearance = Appearance9
        Appearance10.BackColor = System.Drawing.SystemColors.Control
        Appearance10.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance10.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance10.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance10.BorderColor = System.Drawing.SystemColors.Window
        resources.ApplyResources(Appearance10.FontData, "Appearance10.FontData")
        resources.ApplyResources(Appearance10, "Appearance10")
        Appearance10.ForceApplyResources = "FontData|"
        Me.ugrdShipper.DisplayLayout.Override.GroupByRowAppearance = Appearance10
        Appearance11.FontData.BoldAsString = resources.GetString("resource.BoldAsString3")
        resources.ApplyResources(Appearance11, "Appearance11")
        Appearance11.ForceApplyResources = ""
        Me.ugrdShipper.DisplayLayout.Override.HeaderAppearance = Appearance11
        Me.ugrdShipper.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle
        Me.ugrdShipper.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance12.BackColor = System.Drawing.SystemColors.ControlLight
        resources.ApplyResources(Appearance12.FontData, "Appearance12.FontData")
        resources.ApplyResources(Appearance12, "Appearance12")
        Appearance12.ForceApplyResources = "FontData|"
        Me.ugrdShipper.DisplayLayout.Override.RowAlternateAppearance = Appearance12
        Appearance13.BackColor = System.Drawing.SystemColors.Window
        Appearance13.BorderColor = System.Drawing.Color.Silver
        resources.ApplyResources(Appearance13.FontData, "Appearance13.FontData")
        resources.ApplyResources(Appearance13, "Appearance13")
        Appearance13.ForceApplyResources = "FontData|"
        Me.ugrdShipper.DisplayLayout.Override.RowAppearance = Appearance13
        Me.ugrdShipper.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[True]
        Me.ugrdShipper.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed
        Me.ugrdShipper.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.ugrdShipper.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.ugrdShipper.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.[Single]
        Appearance14.BackColor = System.Drawing.SystemColors.ControlLight
        resources.ApplyResources(Appearance14.FontData, "Appearance14.FontData")
        resources.ApplyResources(Appearance14, "Appearance14")
        Appearance14.ForceApplyResources = "FontData|"
        Me.ugrdShipper.DisplayLayout.Override.TemplateAddRowAppearance = Appearance14
        Me.ugrdShipper.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.ugrdShipper.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.ugrdShipper.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand
        Me.ugrdShipper.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        resources.ApplyResources(Me.ugrdShipper, "ugrdShipper")
        Me.ugrdShipper.Name = "ugrdShipper"
        '
        'frmShipperList
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.Controls.Add(Me.ugrdShipper)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.cmdAddItem)
        Me.Controls.Add(Me.cmdDeleteItem)
        Me.Controls.Add(Me.cmdEditItem)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmShipperList"
        Me.ShowInTaskbar = False
        CType(Me.ugrdShipper, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ugrdShipper As Infragistics.Win.UltraWinGrid.UltraGrid
#End Region 
End Class