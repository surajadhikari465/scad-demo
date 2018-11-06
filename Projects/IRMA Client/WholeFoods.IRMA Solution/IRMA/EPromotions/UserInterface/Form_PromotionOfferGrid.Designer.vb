<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_PromotionOfferGrid
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
        Me.components = New System.ComponentModel.Container
        Dim Appearance1 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim UltraGridBand1 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("PromotionOfferBO", -1)
        Dim UltraGridColumn1 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("IsDirty")
        Dim UltraGridColumn2 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("ModifiedDate")
        Dim UltraGridColumn3 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("RewardGroupID")
        Dim UltraGridColumn4 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("StartDate")
        Dim UltraGridColumn5 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("EndDate")
        Dim UltraGridColumn6 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("PromotionOfferID")
        Dim UltraGridColumn7 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Loading", -1, Nothing, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, False)
        Dim UltraGridColumn8 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("EntityState")
        Dim UltraGridColumn9 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("UserID")
        Dim UltraGridColumn10 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("PriceMethodID")
        Dim UltraGridColumn11 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Desc")
        Dim UltraGridColumn12 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("IsNew")
        Dim UltraGridColumn13 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("RewardID")
        Dim UltraGridColumn14 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("RewardQty")
        Dim UltraGridColumn15 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("RewardAmount")
        Dim UltraGridColumn16 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("CreateDate")
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
        Me.Button_Ok = New System.Windows.Forms.Button
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.UltraGrid_PromotionOffers = New Infragistics.Win.UltraWinGrid.UltraGrid
        Me.Button_Delete = New System.Windows.Forms.Button
        Me.Button_Edit = New System.Windows.Forms.Button
        Me.Button_Add = New System.Windows.Forms.Button
        Me.BindingSource_PromotionOffer = New System.Windows.Forms.BindingSource(Me.components)
        Me.GroupBox1.SuspendLayout()
        CType(Me.UltraGrid_PromotionOffers, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BindingSource_PromotionOffer, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Button_Ok
        '
        Me.Button_Ok.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Button_Ok.Location = New System.Drawing.Point(664, 227)
        Me.Button_Ok.Name = "Button_Ok"
        Me.Button_Ok.Size = New System.Drawing.Size(75, 22)
        Me.Button_Ok.TabIndex = 24
        Me.Button_Ok.Text = "OK"
        Me.Button_Ok.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.UltraGrid_PromotionOffers)
        Me.GroupBox1.Controls.Add(Me.Button_Delete)
        Me.GroupBox1.Controls.Add(Me.Button_Edit)
        Me.GroupBox1.Controls.Add(Me.Button_Add)
        Me.GroupBox1.Location = New System.Drawing.Point(11, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(736, 209)
        Me.GroupBox1.TabIndex = 25
        Me.GroupBox1.TabStop = False
        '
        'UltraGrid_PromotionOffers
        '
        Appearance1.BackColor = System.Drawing.SystemColors.ControlLight
        Appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.UltraGrid_PromotionOffers.DisplayLayout.Appearance = Appearance1
        Me.UltraGrid_PromotionOffers.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        UltraGridColumn1.Header.VisiblePosition = 0
        UltraGridColumn1.Width = 23
        UltraGridColumn2.Header.VisiblePosition = 2
        UltraGridColumn2.Width = 46
        UltraGridColumn3.Header.VisiblePosition = 4
        UltraGridColumn3.Width = 34
        UltraGridColumn4.Header.VisiblePosition = 3
        UltraGridColumn4.Width = 35
        UltraGridColumn5.Header.VisiblePosition = 5
        UltraGridColumn5.Width = 41
        UltraGridColumn6.Header.VisiblePosition = 6
        UltraGridColumn6.Width = 44
        UltraGridColumn7.Header.VisiblePosition = 7
        UltraGridColumn7.Width = 30
        UltraGridColumn8.Header.VisiblePosition = 8
        UltraGridColumn8.Width = 30
        UltraGridColumn9.Header.VisiblePosition = 9
        UltraGridColumn9.Width = 30
        UltraGridColumn10.Header.VisiblePosition = 10
        UltraGridColumn10.Width = 30
        UltraGridColumn11.Header.VisiblePosition = 1
        UltraGridColumn11.Width = 46
        UltraGridColumn12.Header.VisiblePosition = 11
        UltraGridColumn12.Width = 30
        UltraGridColumn13.Header.VisiblePosition = 12
        UltraGridColumn13.Width = 30
        UltraGridColumn14.Header.VisiblePosition = 13
        UltraGridColumn14.Width = 30
        UltraGridColumn15.Header.VisiblePosition = 14
        UltraGridColumn15.Width = 30
        UltraGridColumn16.Header.VisiblePosition = 15
        UltraGridColumn16.Width = 30
        UltraGridBand1.Columns.AddRange(New Object() {UltraGridColumn1, UltraGridColumn2, UltraGridColumn3, UltraGridColumn4, UltraGridColumn5, UltraGridColumn6, UltraGridColumn7, UltraGridColumn8, UltraGridColumn9, UltraGridColumn10, UltraGridColumn11, UltraGridColumn12, UltraGridColumn13, UltraGridColumn14, UltraGridColumn15, UltraGridColumn16})
        Me.UltraGrid_PromotionOffers.DisplayLayout.BandsSerializer.Add(UltraGridBand1)
        Me.UltraGrid_PromotionOffers.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.UltraGrid_PromotionOffers.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance2.BorderColor = System.Drawing.SystemColors.Window
        Me.UltraGrid_PromotionOffers.DisplayLayout.GroupByBox.Appearance = Appearance2
        Me.UltraGrid_PromotionOffers.DisplayLayout.GroupByBox.Hidden = True
        Appearance3.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance3.BackColor2 = System.Drawing.SystemColors.Control
        Appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance3.ForeColor = System.Drawing.SystemColors.GrayText
        Me.UltraGrid_PromotionOffers.DisplayLayout.GroupByBox.PromptAppearance = Appearance3
        Me.UltraGrid_PromotionOffers.DisplayLayout.MaxColScrollRegions = 1
        Me.UltraGrid_PromotionOffers.DisplayLayout.MaxRowScrollRegions = 1
        Appearance4.BackColor = System.Drawing.SystemColors.Window
        Appearance4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.UltraGrid_PromotionOffers.DisplayLayout.Override.ActiveCellAppearance = Appearance4
        Appearance5.BackColor = System.Drawing.SystemColors.Highlight
        Appearance5.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.UltraGrid_PromotionOffers.DisplayLayout.Override.ActiveRowAppearance = Appearance5
        Me.UltraGrid_PromotionOffers.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.[False]
        Me.UltraGrid_PromotionOffers.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.UltraGrid_PromotionOffers.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance6.BorderColor = System.Drawing.Color.Silver
        Appearance6.FontData.BoldAsString = "True"
        Appearance6.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.UltraGrid_PromotionOffers.DisplayLayout.Override.CellAppearance = Appearance6
        Me.UltraGrid_PromotionOffers.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        Me.UltraGrid_PromotionOffers.DisplayLayout.Override.CellPadding = 0
        Appearance7.FontData.BoldAsString = "True"
        Me.UltraGrid_PromotionOffers.DisplayLayout.Override.FixedHeaderAppearance = Appearance7
        Appearance8.FontData.BoldAsString = "True"
        Appearance8.TextHAlign = Infragistics.Win.HAlign.Left
        Me.UltraGrid_PromotionOffers.DisplayLayout.Override.HeaderAppearance = Appearance8
        Me.UltraGrid_PromotionOffers.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle
        Me.UltraGrid_PromotionOffers.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance9.BackColor = System.Drawing.SystemColors.Control
        Me.UltraGrid_PromotionOffers.DisplayLayout.Override.RowAlternateAppearance = Appearance9
        Appearance10.BackColor = System.Drawing.SystemColors.Window
        Appearance10.BorderColor = System.Drawing.Color.Silver
        Me.UltraGrid_PromotionOffers.DisplayLayout.Override.RowAppearance = Appearance10
        Me.UltraGrid_PromotionOffers.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[True]
        Me.UltraGrid_PromotionOffers.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed
        Me.UltraGrid_PromotionOffers.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.UltraGrid_PromotionOffers.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.UltraGrid_PromotionOffers.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.[Single]
        Appearance11.BackColor = System.Drawing.SystemColors.ControlLight
        Me.UltraGrid_PromotionOffers.DisplayLayout.Override.TemplateAddRowAppearance = Appearance11
        Me.UltraGrid_PromotionOffers.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.UltraGrid_PromotionOffers.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.UltraGrid_PromotionOffers.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        Me.UltraGrid_PromotionOffers.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.UltraGrid_PromotionOffers.Location = New System.Drawing.Point(6, 19)
        Me.UltraGrid_PromotionOffers.Name = "UltraGrid_PromotionOffers"
        Me.UltraGrid_PromotionOffers.Size = New System.Drawing.Size(560, 184)
        Me.UltraGrid_PromotionOffers.TabIndex = 19
        Me.UltraGrid_PromotionOffers.Text = "Promotion Offers"
        '
        'Button_Delete
        '
        Me.Button_Delete.Enabled = False
        Me.Button_Delete.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Button_Delete.Location = New System.Drawing.Point(572, 77)
        Me.Button_Delete.Name = "Button_Delete"
        Me.Button_Delete.Size = New System.Drawing.Size(75, 23)
        Me.Button_Delete.TabIndex = 18
        Me.Button_Delete.Text = "Delete"
        Me.Button_Delete.UseVisualStyleBackColor = True
        '
        'Button_Edit
        '
        Me.Button_Edit.Enabled = False
        Me.Button_Edit.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Button_Edit.Location = New System.Drawing.Point(572, 48)
        Me.Button_Edit.Name = "Button_Edit"
        Me.Button_Edit.Size = New System.Drawing.Size(75, 23)
        Me.Button_Edit.TabIndex = 17
        Me.Button_Edit.Text = "Edit"
        Me.Button_Edit.UseVisualStyleBackColor = True
        '
        'Button_Add
        '
        Me.Button_Add.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Button_Add.Location = New System.Drawing.Point(572, 19)
        Me.Button_Add.Name = "Button_Add"
        Me.Button_Add.Size = New System.Drawing.Size(75, 23)
        Me.Button_Add.TabIndex = 16
        Me.Button_Add.Text = "Add"
        Me.Button_Add.UseVisualStyleBackColor = True
        '
        'Form_PromotionOfferGrid
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(751, 262)
        Me.Controls.Add(Me.Button_Ok)
        Me.Controls.Add(Me.GroupBox1)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Form_PromotionOfferGrid"
        Me.ShowInTaskbar = False
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Promotional Offers"
        Me.GroupBox1.ResumeLayout(False)
        CType(Me.UltraGrid_PromotionOffers, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BindingSource_PromotionOffer, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Button_Ok As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Button_Delete As System.Windows.Forms.Button
    Friend WithEvents Button_Edit As System.Windows.Forms.Button
    Friend WithEvents Button_Add As System.Windows.Forms.Button
    Friend WithEvents UltraGrid_PromotionOffers As Infragistics.Win.UltraWinGrid.UltraGrid
    Friend WithEvents BindingSource_PromotionOffer As System.Windows.Forms.BindingSource
End Class
