<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MarginInfo
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim Appearance18 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridBand1 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1)
        Dim UltraGridColumn1 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("StoreNo")
        Dim UltraGridColumn2 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("StoreName")
        Dim UltraGridColumn3 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyName")
        Dim UltraGridColumn4 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("PackageDesc1")
        Dim UltraGridColumn5 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("CurrentPrice")
        Dim Appearance19 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridColumn6 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("CurrentMarginAvgCost")
        Dim UltraGridColumn7 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("RegMarginAvgCost")
        Dim UltraGridColumn8 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("AvgCost")
        Dim UltraGridColumn9 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("CurrentMarginCurrCost")
        Dim UltraGridColumn10 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("RegMarginCurrCost")
        Dim UltraGridColumn11 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("RegCost")
        Dim UltraGridColumn12 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("NetCost")
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MarginInfo))
        Me.UltraGrid_MarginInfo = New Infragistics.Win.UltraWinGrid.UltraGrid()
        Me.Label_ItemDesc = New System.Windows.Forms.Label()
        Me.Label_ItemDescValue = New System.Windows.Forms.Label()
        Me.Button_Exit = New System.Windows.Forms.Button()
        Me.Label_PkgDescValue = New System.Windows.Forms.Label()
        Me.Label_PkgDesc = New System.Windows.Forms.Label()
        Me.Label_AvgCostConfigMessage = New System.Windows.Forms.Label()
        CType(Me.UltraGrid_MarginInfo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'UltraGrid_MarginInfo
        '
        Appearance18.BackColor = System.Drawing.SystemColors.Window
        Appearance18.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.UltraGrid_MarginInfo.DisplayLayout.Appearance = Appearance18
        Me.UltraGrid_MarginInfo.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        UltraGridColumn1.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn1.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn1.Header.VisiblePosition = 0
        UltraGridColumn1.Hidden = True
        UltraGridColumn1.RowLayoutColumnInfo.OriginX = 0
        UltraGridColumn1.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn1.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn1.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn2.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn2.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn2.Header.Caption = "Store"
        UltraGridColumn2.Header.VisiblePosition = 2
        UltraGridColumn2.RowLayoutColumnInfo.OriginX = 0
        UltraGridColumn2.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn2.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(147, 0)
        UltraGridColumn2.RowLayoutColumnInfo.SpanX = 3
        UltraGridColumn2.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn2.Width = 88
        UltraGridColumn3.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn3.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn3.Header.Caption = "Vendor"
        UltraGridColumn3.Header.VisiblePosition = 3
        UltraGridColumn3.RowLayoutColumnInfo.OriginX = 3
        UltraGridColumn3.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn3.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(147, 0)
        UltraGridColumn3.RowLayoutColumnInfo.SpanX = 3
        UltraGridColumn3.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn3.Width = 60
        UltraGridColumn4.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn4.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn4.Header.Caption = "Vendor Pack"
        UltraGridColumn4.Header.VisiblePosition = 4
        UltraGridColumn4.Hidden = True
        UltraGridColumn4.RowLayoutColumnInfo.OriginX = 8
        UltraGridColumn4.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn4.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn4.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn5.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        Appearance19.TextHAlignAsString = "Right"
        UltraGridColumn5.CellAppearance = Appearance19
        UltraGridColumn5.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn5.Header.Caption = "Cur Price"
        UltraGridColumn5.Header.VisiblePosition = 1
        UltraGridColumn5.RowLayoutColumnInfo.OriginX = 10
        UltraGridColumn5.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn5.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(85, 0)
        UltraGridColumn5.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn5.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn5.Width = 28
        UltraGridColumn6.Header.Caption = "Cur Margin% (AvgCost)"
        UltraGridColumn6.Header.VisiblePosition = 7
        UltraGridColumn6.Width = 87
        UltraGridColumn7.Header.Caption = "Reg Margin% (AvgCost)"
        UltraGridColumn7.Header.VisiblePosition = 6
        UltraGridColumn7.Width = 89
        UltraGridColumn8.Header.Caption = "Avg Cost"
        UltraGridColumn8.Header.VisiblePosition = 5
        UltraGridColumn8.Width = 62
        UltraGridColumn9.Header.Caption = "Cur Margin% (CurCost)"
        UltraGridColumn9.Header.VisiblePosition = 11
        UltraGridColumn9.Width = 89
        UltraGridColumn10.Header.Caption = "Reg Margin% (CurCost)"
        UltraGridColumn10.Header.VisiblePosition = 10
        UltraGridColumn10.Width = 77
        UltraGridColumn11.Header.Caption = "Reg Unit Cost"
        UltraGridColumn11.Header.VisiblePosition = 8
        UltraGridColumn11.Width = 62
        UltraGridColumn12.Header.Caption = "Net Unit Cost"
        UltraGridColumn12.Header.VisiblePosition = 9
        UltraGridColumn12.Width = 62
        UltraGridBand1.Columns.AddRange(New Object() {UltraGridColumn1, UltraGridColumn2, UltraGridColumn3, UltraGridColumn4, UltraGridColumn5, UltraGridColumn6, UltraGridColumn7, UltraGridColumn8, UltraGridColumn9, UltraGridColumn10, UltraGridColumn11, UltraGridColumn12})
        UltraGridBand1.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[True]
        Me.UltraGrid_MarginInfo.DisplayLayout.BandsSerializer.Add(UltraGridBand1)
        Me.UltraGrid_MarginInfo.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.UltraGrid_MarginInfo.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Appearance20.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance20.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance20.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance20.BorderColor = System.Drawing.SystemColors.Window
        Me.UltraGrid_MarginInfo.DisplayLayout.GroupByBox.Appearance = Appearance20
        Appearance21.ForeColor = System.Drawing.SystemColors.GrayText
        Me.UltraGrid_MarginInfo.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance21
        Me.UltraGrid_MarginInfo.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Appearance22.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance22.BackColor2 = System.Drawing.SystemColors.Control
        Appearance22.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance22.ForeColor = System.Drawing.SystemColors.GrayText
        Me.UltraGrid_MarginInfo.DisplayLayout.GroupByBox.PromptAppearance = Appearance22
        Me.UltraGrid_MarginInfo.DisplayLayout.MaxColScrollRegions = 1
        Me.UltraGrid_MarginInfo.DisplayLayout.MaxRowScrollRegions = 1
        Appearance23.BackColor = System.Drawing.SystemColors.Window
        Appearance23.ForeColor = System.Drawing.SystemColors.ControlText
        Me.UltraGrid_MarginInfo.DisplayLayout.Override.ActiveCellAppearance = Appearance23
        Appearance24.BackColor = System.Drawing.SystemColors.Highlight
        Appearance24.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.UltraGrid_MarginInfo.DisplayLayout.Override.ActiveRowAppearance = Appearance24
        Me.UltraGrid_MarginInfo.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.UltraGrid_MarginInfo.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance25.BackColor = System.Drawing.SystemColors.Window
        Me.UltraGrid_MarginInfo.DisplayLayout.Override.CardAreaAppearance = Appearance25
        Appearance26.BorderColor = System.Drawing.Color.Silver
        Appearance26.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.UltraGrid_MarginInfo.DisplayLayout.Override.CellAppearance = Appearance26
        Me.UltraGrid_MarginInfo.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.UltraGrid_MarginInfo.DisplayLayout.Override.CellPadding = 0
        Appearance27.BackColor = System.Drawing.SystemColors.Control
        Appearance27.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance27.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance27.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance27.BorderColor = System.Drawing.SystemColors.Window
        Me.UltraGrid_MarginInfo.DisplayLayout.Override.GroupByRowAppearance = Appearance27
        Appearance28.TextHAlignAsString = "Left"
        Me.UltraGrid_MarginInfo.DisplayLayout.Override.HeaderAppearance = Appearance28
        Me.UltraGrid_MarginInfo.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti
        Me.UltraGrid_MarginInfo.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance29.BackColor = System.Drawing.SystemColors.Window
        Appearance29.BorderColor = System.Drawing.Color.Silver
        Me.UltraGrid_MarginInfo.DisplayLayout.Override.RowAppearance = Appearance29
        Me.UltraGrid_MarginInfo.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[False]
        Me.UltraGrid_MarginInfo.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.None
        Appearance30.BackColor = System.Drawing.SystemColors.ControlLight
        Me.UltraGrid_MarginInfo.DisplayLayout.Override.TemplateAddRowAppearance = Appearance30
        Me.UltraGrid_MarginInfo.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.UltraGrid_MarginInfo.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.UltraGrid_MarginInfo.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Bold)
        Me.UltraGrid_MarginInfo.Location = New System.Drawing.Point(12, 77)
        Me.UltraGrid_MarginInfo.Name = "UltraGrid_MarginInfo"
        Me.UltraGrid_MarginInfo.Size = New System.Drawing.Size(725, 278)
        Me.UltraGrid_MarginInfo.TabIndex = 0
        Me.UltraGrid_MarginInfo.Text = "UltraGrid1"
        '
        'Label_ItemDesc
        '
        Me.Label_ItemDesc.AutoSize = True
        Me.Label_ItemDesc.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.Label_ItemDesc.Location = New System.Drawing.Point(78, 19)
        Me.Label_ItemDesc.Name = "Label_ItemDesc"
        Me.Label_ItemDesc.Size = New System.Drawing.Size(38, 14)
        Me.Label_ItemDesc.TabIndex = 1
        Me.Label_ItemDesc.Text = "Item :"
        '
        'Label_ItemDescValue
        '
        Me.Label_ItemDescValue.AutoSize = True
        Me.Label_ItemDescValue.Location = New System.Drawing.Point(122, 19)
        Me.Label_ItemDescValue.Name = "Label_ItemDescValue"
        Me.Label_ItemDescValue.Size = New System.Drawing.Size(39, 13)
        Me.Label_ItemDescValue.TabIndex = 2
        Me.Label_ItemDescValue.Text = "Label1"
        '
        'Button_Exit
        '
        Me.Button_Exit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Exit.BackColor = System.Drawing.SystemColors.Control
        Me.Button_Exit.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button_Exit.Font = New System.Drawing.Font("Arial", 8.0!)
        Me.Button_Exit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button_Exit.Image = CType(resources.GetObject("Button_Exit.Image"), System.Drawing.Image)
        Me.Button_Exit.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Button_Exit.Location = New System.Drawing.Point(696, 365)
        Me.Button_Exit.Name = "Button_Exit"
        Me.Button_Exit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button_Exit.Size = New System.Drawing.Size(41, 41)
        Me.Button_Exit.TabIndex = 34
        Me.Button_Exit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.Button_Exit.UseVisualStyleBackColor = False
        '
        'Label_PkgDescValue
        '
        Me.Label_PkgDescValue.AutoSize = True
        Me.Label_PkgDescValue.Location = New System.Drawing.Point(122, 44)
        Me.Label_PkgDescValue.Name = "Label_PkgDescValue"
        Me.Label_PkgDescValue.Size = New System.Drawing.Size(39, 13)
        Me.Label_PkgDescValue.TabIndex = 36
        Me.Label_PkgDescValue.Text = "Label1"
        '
        'Label_PkgDesc
        '
        Me.Label_PkgDesc.BackColor = System.Drawing.Color.Transparent
        Me.Label_PkgDesc.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label_PkgDesc.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.Label_PkgDesc.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label_PkgDesc.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label_PkgDesc.Location = New System.Drawing.Point(-59, 44)
        Me.Label_PkgDesc.Name = "Label_PkgDesc"
        Me.Label_PkgDesc.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label_PkgDesc.Size = New System.Drawing.Size(175, 17)
        Me.Label_PkgDesc.TabIndex = 35
        Me.Label_PkgDesc.Text = "Retail Package :"
        Me.Label_PkgDesc.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label_AvgCostConfigMessage
        '
        Me.Label_AvgCostConfigMessage.AutoSize = True
        Me.Label_AvgCostConfigMessage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_AvgCostConfigMessage.ForeColor = System.Drawing.Color.Red
        Me.Label_AvgCostConfigMessage.Location = New System.Drawing.Point(12, 61)
        Me.Label_AvgCostConfigMessage.Name = "Label_AvgCostConfigMessage"
        Me.Label_AvgCostConfigMessage.Size = New System.Drawing.Size(613, 13)
        Me.Label_AvgCostConfigMessage.TabIndex = 37
        Me.Label_AvgCostConfigMessage.Text = "! The UseAverageCostforCostandMargin key has not been configured and will affect " & _
    "values on this screen."
        Me.Label_AvgCostConfigMessage.Visible = False
        '
        'MarginInfo
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(750, 418)
        Me.Controls.Add(Me.Label_AvgCostConfigMessage)
        Me.Controls.Add(Me.Label_PkgDescValue)
        Me.Controls.Add(Me.Label_PkgDesc)
        Me.Controls.Add(Me.Button_Exit)
        Me.Controls.Add(Me.Label_ItemDescValue)
        Me.Controls.Add(Me.Label_ItemDesc)
        Me.Controls.Add(Me.UltraGrid_MarginInfo)
        Me.Name = "MarginInfo"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Item Margin Info"
        CType(Me.UltraGrid_MarginInfo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents UltraGrid_MarginInfo As Infragistics.Win.UltraWinGrid.UltraGrid
    Friend WithEvents Label_ItemDesc As System.Windows.Forms.Label
    Friend WithEvents Label_ItemDescValue As System.Windows.Forms.Label
    Public WithEvents Button_Exit As System.Windows.Forms.Button
    Friend WithEvents Label_PkgDescValue As System.Windows.Forms.Label
    Public WithEvents Label_PkgDesc As System.Windows.Forms.Label
    Friend WithEvents Label_AvgCostConfigMessage As System.Windows.Forms.Label
End Class
