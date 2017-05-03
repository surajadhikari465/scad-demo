<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_ManageLabelType
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
        Dim Appearance1 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridBand1 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1)
        Dim UltraGridColumn1 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("LabelTypeID")
        Dim UltraGridColumn2 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("LabelTypeDesc")
        Dim UltraGridColumn3 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Delete", 0, Nothing, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, False)
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
        Me.UltraGrid_LabelType = New Infragistics.Win.UltraWinGrid.UltraGrid()
        Me.Button_Insert = New System.Windows.Forms.Button()
        Me.Button_Save = New System.Windows.Forms.Button()
        CType(Me.UltraGrid_LabelType, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'UltraGrid_LabelType
        '
        Me.UltraGrid_LabelType.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Appearance1.BackColor = System.Drawing.SystemColors.Window
        Appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.UltraGrid_LabelType.DisplayLayout.Appearance = Appearance1
        Me.UltraGrid_LabelType.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        UltraGridColumn1.Header.VisiblePosition = 0
        UltraGridColumn1.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Disallow
        UltraGridColumn1.TabStop = False
        UltraGridColumn1.Width = 343
        UltraGridColumn2.Header.VisiblePosition = 1
        UltraGridColumn2.MaxLength = 4
        UltraGridColumn2.Width = 487
        UltraGridColumn3.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always
        UltraGridColumn3.CellActivation = Infragistics.Win.UltraWinGrid.Activation.ActivateOnly
        UltraGridColumn3.Header.VisiblePosition = 2
        UltraGridColumn3.Hidden = True
        UltraGridColumn3.LockedWidth = True
        UltraGridColumn3.NullText = "Delete"
        UltraGridColumn3.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button
        UltraGridColumn3.Width = 102
        UltraGridBand1.Columns.AddRange(New Object() {UltraGridColumn1, UltraGridColumn2, UltraGridColumn3})
        Me.UltraGrid_LabelType.DisplayLayout.BandsSerializer.Add(UltraGridBand1)
        Me.UltraGrid_LabelType.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.UltraGrid_LabelType.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance2.BorderColor = System.Drawing.SystemColors.Window
        Me.UltraGrid_LabelType.DisplayLayout.GroupByBox.Appearance = Appearance2
        Appearance3.ForeColor = System.Drawing.SystemColors.GrayText
        Me.UltraGrid_LabelType.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance3
        Me.UltraGrid_LabelType.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.UltraGrid_LabelType.DisplayLayout.GroupByBox.Hidden = True
        Appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance4.BackColor2 = System.Drawing.SystemColors.Control
        Appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance4.ForeColor = System.Drawing.SystemColors.GrayText
        Me.UltraGrid_LabelType.DisplayLayout.GroupByBox.PromptAppearance = Appearance4
        Me.UltraGrid_LabelType.DisplayLayout.MaxColScrollRegions = 1
        Me.UltraGrid_LabelType.DisplayLayout.MaxRowScrollRegions = 1
        Appearance5.BackColor = System.Drawing.SystemColors.Window
        Appearance5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.UltraGrid_LabelType.DisplayLayout.Override.ActiveCellAppearance = Appearance5
        Appearance6.BackColor = System.Drawing.SystemColors.Highlight
        Appearance6.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.UltraGrid_LabelType.DisplayLayout.Override.ActiveRowAppearance = Appearance6
        Me.UltraGrid_LabelType.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.UltraGrid_LabelType.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance7.BackColor = System.Drawing.SystemColors.Window
        Me.UltraGrid_LabelType.DisplayLayout.Override.CardAreaAppearance = Appearance7
        Appearance8.BorderColor = System.Drawing.Color.Silver
        Appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.UltraGrid_LabelType.DisplayLayout.Override.CellAppearance = Appearance8
        Me.UltraGrid_LabelType.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.UltraGrid_LabelType.DisplayLayout.Override.CellPadding = 0
        Appearance9.BackColor = System.Drawing.SystemColors.Control
        Appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance9.BorderColor = System.Drawing.SystemColors.Window
        Me.UltraGrid_LabelType.DisplayLayout.Override.GroupByRowAppearance = Appearance9
        Appearance10.TextHAlignAsString = "Left"
        Me.UltraGrid_LabelType.DisplayLayout.Override.HeaderAppearance = Appearance10
        Me.UltraGrid_LabelType.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti
        Me.UltraGrid_LabelType.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance11.BackColor = System.Drawing.SystemColors.Window
        Appearance11.BorderColor = System.Drawing.Color.Silver
        Me.UltraGrid_LabelType.DisplayLayout.Override.RowAppearance = Appearance11
        Me.UltraGrid_LabelType.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[False]
        Me.UltraGrid_LabelType.DisplayLayout.Override.SummaryDisplayArea = Infragistics.Win.UltraWinGrid.SummaryDisplayAreas.None
        Appearance12.BackColor = System.Drawing.SystemColors.ControlLight
        Me.UltraGrid_LabelType.DisplayLayout.Override.TemplateAddRowAppearance = Appearance12
        Me.UltraGrid_LabelType.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.UltraGrid_LabelType.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.UltraGrid_LabelType.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        Me.UltraGrid_LabelType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.UltraGrid_LabelType.Location = New System.Drawing.Point(12, 12)
        Me.UltraGrid_LabelType.Name = "UltraGrid_LabelType"
        Me.UltraGrid_LabelType.Size = New System.Drawing.Size(832, 341)
        Me.UltraGrid_LabelType.TabIndex = 6
        Me.UltraGrid_LabelType.Text = "UltraGrid1"
        '
        'Button_Insert
        '
        Me.Button_Insert.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Insert.Location = New System.Drawing.Point(534, 359)
        Me.Button_Insert.Name = "Button_Insert"
        Me.Button_Insert.Size = New System.Drawing.Size(152, 23)
        Me.Button_Insert.TabIndex = 5
        Me.Button_Insert.Text = "Insert"
        Me.Button_Insert.UseVisualStyleBackColor = True
        '
        'Button_Save
        '
        Me.Button_Save.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Save.Location = New System.Drawing.Point(692, 359)
        Me.Button_Save.Name = "Button_Save"
        Me.Button_Save.Size = New System.Drawing.Size(152, 23)
        Me.Button_Save.TabIndex = 4
        Me.Button_Save.Text = "Save"
        Me.Button_Save.UseVisualStyleBackColor = True
        '
        'Form_ManageLabelType
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(856, 394)
        Me.Controls.Add(Me.UltraGrid_LabelType)
        Me.Controls.Add(Me.Button_Insert)
        Me.Controls.Add(Me.Button_Save)
        Me.Name = "Form_ManageLabelType"
        Me.Text = "Form_ManageLabelType"
        CType(Me.UltraGrid_LabelType, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents UltraGrid_LabelType As Infragistics.Win.UltraWinGrid.UltraGrid
    Friend WithEvents Button_Insert As System.Windows.Forms.Button
    Friend WithEvents Button_Save As System.Windows.Forms.Button
End Class
