<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_ManagePriceChgType
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim Appearance13 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridBand1 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1)
        Dim UltraGridColumn1 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("PriceChgTypeID")
        Dim UltraGridColumn2 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("PriceChgTypeDesc")
        Dim UltraGridColumn3 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Priority")
        Dim UltraGridColumn4 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("IsOnSale")
        Dim UltraGridColumn5 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("IsMSRPRequired")
        Dim UltraGridColumn6 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("IsLineDrive")
        Dim UltraGridColumn7 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("IsCompetitive", 0)
        Dim UltraGridColumn8 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("LastUpdateTimestamp", 1)
        Dim Appearance14 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance15 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance16 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance17 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance18 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance19 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance20 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance21 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance22 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance23 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance24 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Me.Button_Save = New System.Windows.Forms.Button()
        Me.Button_Insert = New System.Windows.Forms.Button()
        Me.UltraGrid_PriceChgType = New Infragistics.Win.UltraWinGrid.UltraGrid()
        CType(Me.UltraGrid_PriceChgType, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Button_Save
        '
        Me.Button_Save.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Save.Location = New System.Drawing.Point(595, 343)
        Me.Button_Save.Name = "Button_Save"
        Me.Button_Save.Size = New System.Drawing.Size(152, 23)
        Me.Button_Save.TabIndex = 1
        Me.Button_Save.Text = "Save"
        Me.Button_Save.UseVisualStyleBackColor = True
        '
        'Button_Insert
        '
        Me.Button_Insert.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Insert.Location = New System.Drawing.Point(437, 343)
        Me.Button_Insert.Name = "Button_Insert"
        Me.Button_Insert.Size = New System.Drawing.Size(152, 23)
        Me.Button_Insert.TabIndex = 2
        Me.Button_Insert.Text = "Insert"
        Me.Button_Insert.UseVisualStyleBackColor = True
        '
        'UltraGrid_PriceChgType
        '
        Me.UltraGrid_PriceChgType.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Appearance13.BackColor = System.Drawing.SystemColors.Window
        Appearance13.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.UltraGrid_PriceChgType.DisplayLayout.Appearance = Appearance13
        Me.UltraGrid_PriceChgType.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        UltraGridColumn1.Header.VisiblePosition = 0
        UltraGridColumn1.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Disallow
        UltraGridColumn1.TabStop = False
        UltraGridColumn1.Width = 92
        UltraGridColumn2.Header.VisiblePosition = 1
        UltraGridColumn2.MaxLength = 3
        UltraGridColumn2.Width = 131
        UltraGridColumn3.Header.VisiblePosition = 2
        UltraGridColumn3.MaxLength = 2
        UltraGridColumn3.MaxValue = "99"
        UltraGridColumn3.MaxValueExclusive = "99"
        UltraGridColumn3.MinValue = CType(0, Short)
        UltraGridColumn3.MinValueExclusive = CType(0, Short)
        UltraGridColumn3.Width = 59
        UltraGridColumn4.Header.VisiblePosition = 3
        UltraGridColumn4.Width = 71
        UltraGridColumn5.Header.VisiblePosition = 4
        UltraGridColumn5.Width = 111
        UltraGridColumn6.Header.VisiblePosition = 5
        UltraGridColumn6.Width = 81
        UltraGridColumn7.Header.VisiblePosition = 6
        UltraGridColumn7.Width = 87
        UltraGridColumn8.DataType = GetType(Date)
        UltraGridColumn8.Format = "MM/dd/yyyy hh:m:ss"
        UltraGridColumn8.Header.VisiblePosition = 7
        UltraGridColumn8.MaskInput = "{date} {time}"
        UltraGridColumn8.TabStop = False
        UltraGridColumn8.Width = 100
        UltraGridBand1.Columns.AddRange(New Object() {UltraGridColumn1, UltraGridColumn2, UltraGridColumn3, UltraGridColumn4, UltraGridColumn5, UltraGridColumn6, UltraGridColumn7, UltraGridColumn8})
        Me.UltraGrid_PriceChgType.DisplayLayout.BandsSerializer.Add(UltraGridBand1)
        Me.UltraGrid_PriceChgType.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.UltraGrid_PriceChgType.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Appearance14.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance14.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance14.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance14.BorderColor = System.Drawing.SystemColors.Window
        Me.UltraGrid_PriceChgType.DisplayLayout.GroupByBox.Appearance = Appearance14
        Appearance15.ForeColor = System.Drawing.SystemColors.GrayText
        Me.UltraGrid_PriceChgType.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance15
        Me.UltraGrid_PriceChgType.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.UltraGrid_PriceChgType.DisplayLayout.GroupByBox.Hidden = True
        Appearance16.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance16.BackColor2 = System.Drawing.SystemColors.Control
        Appearance16.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance16.ForeColor = System.Drawing.SystemColors.GrayText
        Me.UltraGrid_PriceChgType.DisplayLayout.GroupByBox.PromptAppearance = Appearance16
        Me.UltraGrid_PriceChgType.DisplayLayout.MaxColScrollRegions = 1
        Me.UltraGrid_PriceChgType.DisplayLayout.MaxRowScrollRegions = 1
        Appearance17.BackColor = System.Drawing.SystemColors.Window
        Appearance17.ForeColor = System.Drawing.SystemColors.ControlText
        Me.UltraGrid_PriceChgType.DisplayLayout.Override.ActiveCellAppearance = Appearance17
        Appearance18.BackColor = System.Drawing.SystemColors.Highlight
        Appearance18.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.UltraGrid_PriceChgType.DisplayLayout.Override.ActiveRowAppearance = Appearance18
        Me.UltraGrid_PriceChgType.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.UltraGrid_PriceChgType.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance19.BackColor = System.Drawing.SystemColors.Window
        Me.UltraGrid_PriceChgType.DisplayLayout.Override.CardAreaAppearance = Appearance19
        Appearance20.BorderColor = System.Drawing.Color.Silver
        Appearance20.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.UltraGrid_PriceChgType.DisplayLayout.Override.CellAppearance = Appearance20
        Me.UltraGrid_PriceChgType.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.UltraGrid_PriceChgType.DisplayLayout.Override.CellPadding = 0
        Appearance21.BackColor = System.Drawing.SystemColors.Control
        Appearance21.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance21.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance21.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance21.BorderColor = System.Drawing.SystemColors.Window
        Me.UltraGrid_PriceChgType.DisplayLayout.Override.GroupByRowAppearance = Appearance21
        Appearance22.TextHAlignAsString = "Left"
        Me.UltraGrid_PriceChgType.DisplayLayout.Override.HeaderAppearance = Appearance22
        Me.UltraGrid_PriceChgType.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti
        Me.UltraGrid_PriceChgType.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance23.BackColor = System.Drawing.SystemColors.Window
        Appearance23.BorderColor = System.Drawing.Color.Silver
        Me.UltraGrid_PriceChgType.DisplayLayout.Override.RowAppearance = Appearance23
        Me.UltraGrid_PriceChgType.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[False]
        Me.UltraGrid_PriceChgType.DisplayLayout.Override.SummaryDisplayArea = Infragistics.Win.UltraWinGrid.SummaryDisplayAreas.None
        Appearance24.BackColor = System.Drawing.SystemColors.ControlLight
        Me.UltraGrid_PriceChgType.DisplayLayout.Override.TemplateAddRowAppearance = Appearance24
        Me.UltraGrid_PriceChgType.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.UltraGrid_PriceChgType.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.UltraGrid_PriceChgType.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        Me.UltraGrid_PriceChgType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.UltraGrid_PriceChgType.Location = New System.Drawing.Point(13, 12)
        Me.UltraGrid_PriceChgType.Name = "UltraGrid_PriceChgType"
        Me.UltraGrid_PriceChgType.Size = New System.Drawing.Size(734, 325)
        Me.UltraGrid_PriceChgType.TabIndex = 3
        Me.UltraGrid_PriceChgType.Text = "UltraGrid1"
        '
        'Form_ManagePriceChgType
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(760, 378)
        Me.Controls.Add(Me.UltraGrid_PriceChgType)
        Me.Controls.Add(Me.Button_Insert)
        Me.Controls.Add(Me.Button_Save)
        Me.Name = "Form_ManagePriceChgType"
        Me.Text = "Manage PriceChgTypes"
        CType(Me.UltraGrid_PriceChgType, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Button_Save As System.Windows.Forms.Button
    Friend WithEvents Button_Insert As System.Windows.Forms.Button
    Friend WithEvents UltraGrid_PriceChgType As Infragistics.Win.UltraWinGrid.UltraGrid
End Class
