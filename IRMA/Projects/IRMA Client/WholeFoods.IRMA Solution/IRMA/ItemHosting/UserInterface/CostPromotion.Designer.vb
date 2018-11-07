<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CostPromotion
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
        Me.components = New System.ComponentModel.Container()
        Dim Appearance1 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridBand1 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1)
        Dim UltraGridColumn1 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("CostPromoDesc")
        Dim UltraGridColumn2 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("VendorDealTypeDesc ")
        Dim UltraGridColumn3 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("CaseAmt")
        Dim UltraGridColumn4 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("CaseAmtType")
        Dim UltraGridColumn5 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("StartDate")
        Dim UltraGridColumn6 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("EndDate")
        Dim UltraGridColumn7 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("StoreItemVendorID")
        Dim UltraGridColumn8 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("ItemKey")
        Dim UltraGridColumn9 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("VendorID")
        Dim UltraGridColumn10 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("CaseQty")
        Dim UltraGridColumn11 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("PackageDesc1")
        Dim UltraGridColumn12 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("VendorDealTypeID")
        Dim UltraGridColumn13 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("VendorDealTypeCode")
        Dim UltraGridColumn14 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("CostPromoCodeTypeID ")
        Dim UltraGridColumn15 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("CostPromoCode")
        Dim UltraGridColumn16 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("VendorDealHistoryID")
        Dim UltraGridColumn17 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("NotStackable")
        Dim Appearance2 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridColumn18 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("InsertDate", 0)
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
        Dim Appearance13 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(CostPromotion))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label_StoreName = New System.Windows.Forms.Label()
        Me.Label_StoreNameValue = New System.Windows.Forms.Label()
        Me.Label_PkgDescValue = New System.Windows.Forms.Label()
        Me.Label_ItemValue = New System.Windows.Forms.Label()
        Me.Label_VendorValue = New System.Windows.Forms.Label()
        Me.Label_PkgDesc = New System.Windows.Forms.Label()
        Me.Label_Item = New System.Windows.Forms.Label()
        Me.Label_Vendor = New System.Windows.Forms.Label()
        Me.UltraGrid_VendorDeals = New Infragistics.Win.UltraWinGrid.UltraGrid()
        Me.Button_Exit = New System.Windows.Forms.Button()
        Me.Button_Edit = New System.Windows.Forms.Button()
        Me.Button_Delete = New System.Windows.Forms.Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.GroupBox1.SuspendLayout()
        CType(Me.UltraGrid_VendorDeals, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label_StoreName)
        Me.GroupBox1.Controls.Add(Me.Label_StoreNameValue)
        Me.GroupBox1.Controls.Add(Me.Label_PkgDescValue)
        Me.GroupBox1.Controls.Add(Me.Label_ItemValue)
        Me.GroupBox1.Controls.Add(Me.Label_VendorValue)
        Me.GroupBox1.Controls.Add(Me.Label_PkgDesc)
        Me.GroupBox1.Controls.Add(Me.Label_Item)
        Me.GroupBox1.Controls.Add(Me.Label_Vendor)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(549, 118)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        '
        'Label_StoreName
        '
        Me.Label_StoreName.BackColor = System.Drawing.Color.Transparent
        Me.Label_StoreName.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label_StoreName.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.Label_StoreName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label_StoreName.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label_StoreName.Location = New System.Drawing.Point(34, 16)
        Me.Label_StoreName.Name = "Label_StoreName"
        Me.Label_StoreName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label_StoreName.Size = New System.Drawing.Size(76, 17)
        Me.Label_StoreName.TabIndex = 9
        Me.Label_StoreName.Text = "Store :"
        Me.Label_StoreName.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label_StoreNameValue
        '
        Me.Label_StoreNameValue.AutoSize = True
        Me.Label_StoreNameValue.Location = New System.Drawing.Point(116, 16)
        Me.Label_StoreNameValue.Name = "Label_StoreNameValue"
        Me.Label_StoreNameValue.Size = New System.Drawing.Size(39, 13)
        Me.Label_StoreNameValue.TabIndex = 8
        Me.Label_StoreNameValue.Text = "Label1"
        '
        'Label_PkgDescValue
        '
        Me.Label_PkgDescValue.AutoSize = True
        Me.Label_PkgDescValue.Location = New System.Drawing.Point(116, 93)
        Me.Label_PkgDescValue.Name = "Label_PkgDescValue"
        Me.Label_PkgDescValue.Size = New System.Drawing.Size(39, 13)
        Me.Label_PkgDescValue.TabIndex = 7
        Me.Label_PkgDescValue.Text = "Label1"
        '
        'Label_ItemValue
        '
        Me.Label_ItemValue.AutoSize = True
        Me.Label_ItemValue.Location = New System.Drawing.Point(116, 67)
        Me.Label_ItemValue.Name = "Label_ItemValue"
        Me.Label_ItemValue.Size = New System.Drawing.Size(39, 13)
        Me.Label_ItemValue.TabIndex = 6
        Me.Label_ItemValue.Text = "Label1"
        '
        'Label_VendorValue
        '
        Me.Label_VendorValue.AutoSize = True
        Me.Label_VendorValue.Location = New System.Drawing.Point(116, 41)
        Me.Label_VendorValue.Name = "Label_VendorValue"
        Me.Label_VendorValue.Size = New System.Drawing.Size(39, 13)
        Me.Label_VendorValue.TabIndex = 5
        Me.Label_VendorValue.Text = "Label1"
        '
        'Label_PkgDesc
        '
        Me.Label_PkgDesc.BackColor = System.Drawing.Color.Transparent
        Me.Label_PkgDesc.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label_PkgDesc.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.Label_PkgDesc.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label_PkgDesc.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label_PkgDesc.Location = New System.Drawing.Point(18, 93)
        Me.Label_PkgDesc.Name = "Label_PkgDesc"
        Me.Label_PkgDesc.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label_PkgDesc.Size = New System.Drawing.Size(92, 17)
        Me.Label_PkgDesc.TabIndex = 3
        Me.Label_PkgDesc.Text = "Retail Package :"
        Me.Label_PkgDesc.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label_Item
        '
        Me.Label_Item.BackColor = System.Drawing.Color.Transparent
        Me.Label_Item.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label_Item.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.Label_Item.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label_Item.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label_Item.Location = New System.Drawing.Point(34, 67)
        Me.Label_Item.Name = "Label_Item"
        Me.Label_Item.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label_Item.Size = New System.Drawing.Size(76, 17)
        Me.Label_Item.TabIndex = 2
        Me.Label_Item.Text = "Item :"
        Me.Label_Item.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label_Vendor
        '
        Me.Label_Vendor.BackColor = System.Drawing.Color.Transparent
        Me.Label_Vendor.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label_Vendor.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.Label_Vendor.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label_Vendor.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label_Vendor.Location = New System.Drawing.Point(34, 41)
        Me.Label_Vendor.Name = "Label_Vendor"
        Me.Label_Vendor.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label_Vendor.Size = New System.Drawing.Size(76, 17)
        Me.Label_Vendor.TabIndex = 1
        Me.Label_Vendor.Text = "Vendor :"
        Me.Label_Vendor.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'UltraGrid_VendorDeals
        '
        Appearance1.BackColor = System.Drawing.SystemColors.Window
        Appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.UltraGrid_VendorDeals.DisplayLayout.Appearance = Appearance1
        Me.UltraGrid_VendorDeals.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        UltraGridColumn1.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn1.Header.Caption = "Cost Promotion Type"
        UltraGridColumn1.Header.VisiblePosition = 5
        UltraGridColumn1.RowLayoutColumnInfo.OriginX = 0
        UltraGridColumn1.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn1.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn1.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn1.Width = 84
        UltraGridColumn2.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn2.Header.Caption = "Deal Type"
        UltraGridColumn2.Header.VisiblePosition = 6
        UltraGridColumn2.RowLayoutColumnInfo.OriginX = 2
        UltraGridColumn2.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn2.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn2.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn2.Width = 83
        UltraGridColumn3.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn3.Header.Caption = "Amt"
        UltraGridColumn3.Header.VisiblePosition = 7
        UltraGridColumn3.RowLayoutColumnInfo.OriginX = 4
        UltraGridColumn3.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn3.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn3.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn3.Width = 66
        UltraGridColumn4.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn4.Header.Caption = ""
        UltraGridColumn4.Header.VisiblePosition = 8
        UltraGridColumn4.MaxLength = 1
        UltraGridColumn4.MinWidth = 1
        UltraGridColumn4.RowLayoutColumnInfo.OriginX = 6
        UltraGridColumn4.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn4.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(20, 0)
        UltraGridColumn4.RowLayoutColumnInfo.SpanX = 1
        UltraGridColumn4.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn4.Width = 15
        UltraGridColumn5.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn5.Header.Caption = "Start Date"
        UltraGridColumn5.Header.VisiblePosition = 9
        UltraGridColumn5.RowLayoutColumnInfo.OriginX = 7
        UltraGridColumn5.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn5.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn5.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn5.Width = 71
        UltraGridColumn6.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn6.Header.Caption = "End Date"
        UltraGridColumn6.Header.VisiblePosition = 10
        UltraGridColumn6.RowLayoutColumnInfo.OriginX = 9
        UltraGridColumn6.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn6.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn6.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn6.Width = 71
        UltraGridColumn7.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn7.Header.VisiblePosition = 0
        UltraGridColumn7.Hidden = True
        UltraGridColumn8.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn8.Header.VisiblePosition = 1
        UltraGridColumn8.Hidden = True
        UltraGridColumn9.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn9.Header.VisiblePosition = 2
        UltraGridColumn9.Hidden = True
        UltraGridColumn10.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn10.Header.VisiblePosition = 3
        UltraGridColumn10.Hidden = True
        UltraGridColumn11.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn11.Header.VisiblePosition = 4
        UltraGridColumn11.Hidden = True
        UltraGridColumn12.Header.VisiblePosition = 11
        UltraGridColumn12.Hidden = True
        UltraGridColumn13.Header.VisiblePosition = 12
        UltraGridColumn13.Hidden = True
        UltraGridColumn14.Header.VisiblePosition = 13
        UltraGridColumn14.Hidden = True
        UltraGridColumn15.Header.VisiblePosition = 14
        UltraGridColumn15.Hidden = True
        UltraGridColumn16.Header.VisiblePosition = 15
        UltraGridColumn16.Hidden = True
        Appearance2.TextHAlignAsString = "Center"
        UltraGridColumn17.CellAppearance = Appearance2
        UltraGridColumn17.Header.Caption = "No Stack"
        UltraGridColumn17.Header.VisiblePosition = 17
        UltraGridColumn17.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(65, 0)
        UltraGridColumn17.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox
        UltraGridColumn17.Width = 71
        UltraGridColumn18.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn18.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn18.Format = "MM/dd/yyyy"
        UltraGridColumn18.Header.Caption = "Insert Date"
        UltraGridColumn18.Header.VisiblePosition = 16
        UltraGridColumn18.Width = 96
        UltraGridBand1.Columns.AddRange(New Object() {UltraGridColumn1, UltraGridColumn2, UltraGridColumn3, UltraGridColumn4, UltraGridColumn5, UltraGridColumn6, UltraGridColumn7, UltraGridColumn8, UltraGridColumn9, UltraGridColumn10, UltraGridColumn11, UltraGridColumn12, UltraGridColumn13, UltraGridColumn14, UltraGridColumn15, UltraGridColumn16, UltraGridColumn17, UltraGridColumn18})
        Me.UltraGrid_VendorDeals.DisplayLayout.BandsSerializer.Add(UltraGridBand1)
        Me.UltraGrid_VendorDeals.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.UltraGrid_VendorDeals.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Appearance3.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance3.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance3.BorderColor = System.Drawing.SystemColors.Window
        Me.UltraGrid_VendorDeals.DisplayLayout.GroupByBox.Appearance = Appearance3
        Appearance4.ForeColor = System.Drawing.SystemColors.GrayText
        Me.UltraGrid_VendorDeals.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance4
        Me.UltraGrid_VendorDeals.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Appearance5.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance5.BackColor2 = System.Drawing.SystemColors.Control
        Appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance5.ForeColor = System.Drawing.SystemColors.GrayText
        Me.UltraGrid_VendorDeals.DisplayLayout.GroupByBox.PromptAppearance = Appearance5
        Me.UltraGrid_VendorDeals.DisplayLayout.MaxColScrollRegions = 1
        Me.UltraGrid_VendorDeals.DisplayLayout.MaxRowScrollRegions = 1
        Appearance6.BackColor = System.Drawing.SystemColors.Window
        Appearance6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.UltraGrid_VendorDeals.DisplayLayout.Override.ActiveCellAppearance = Appearance6
        Appearance7.BackColor = System.Drawing.SystemColors.Highlight
        Appearance7.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.UltraGrid_VendorDeals.DisplayLayout.Override.ActiveRowAppearance = Appearance7
        Me.UltraGrid_VendorDeals.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.UltraGrid_VendorDeals.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance8.BackColor = System.Drawing.SystemColors.Window
        Me.UltraGrid_VendorDeals.DisplayLayout.Override.CardAreaAppearance = Appearance8
        Appearance9.BorderColor = System.Drawing.Color.Silver
        Appearance9.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.UltraGrid_VendorDeals.DisplayLayout.Override.CellAppearance = Appearance9
        Me.UltraGrid_VendorDeals.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.UltraGrid_VendorDeals.DisplayLayout.Override.CellPadding = 0
        Appearance10.BackColor = System.Drawing.SystemColors.Control
        Appearance10.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance10.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance10.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance10.BorderColor = System.Drawing.SystemColors.Window
        Me.UltraGrid_VendorDeals.DisplayLayout.Override.GroupByRowAppearance = Appearance10
        Appearance11.TextHAlignAsString = "Left"
        Me.UltraGrid_VendorDeals.DisplayLayout.Override.HeaderAppearance = Appearance11
        Me.UltraGrid_VendorDeals.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti
        Me.UltraGrid_VendorDeals.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance12.BackColor = System.Drawing.SystemColors.Window
        Appearance12.BorderColor = System.Drawing.Color.Silver
        Me.UltraGrid_VendorDeals.DisplayLayout.Override.RowAppearance = Appearance12
        Me.UltraGrid_VendorDeals.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[True]
        Me.UltraGrid_VendorDeals.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.UltraGrid_VendorDeals.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.UltraGrid_VendorDeals.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.[Single]
        Appearance13.BackColor = System.Drawing.SystemColors.ControlLight
        Me.UltraGrid_VendorDeals.DisplayLayout.Override.TemplateAddRowAppearance = Appearance13
        Me.UltraGrid_VendorDeals.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.UltraGrid_VendorDeals.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.UltraGrid_VendorDeals.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.UltraGrid_VendorDeals.Location = New System.Drawing.Point(12, 136)
        Me.UltraGrid_VendorDeals.Name = "UltraGrid_VendorDeals"
        Me.UltraGrid_VendorDeals.Size = New System.Drawing.Size(578, 275)
        Me.UltraGrid_VendorDeals.TabIndex = 1
        Me.UltraGrid_VendorDeals.Text = "UltraGrid1"
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
        Me.Button_Exit.Location = New System.Drawing.Point(549, 418)
        Me.Button_Exit.Name = "Button_Exit"
        Me.Button_Exit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button_Exit.Size = New System.Drawing.Size(41, 41)
        Me.Button_Exit.TabIndex = 96
        Me.Button_Exit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.Button_Exit, "Exit")
        Me.Button_Exit.UseVisualStyleBackColor = False
        '
        'Button_Edit
        '
        Me.Button_Edit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Button_Edit.BackColor = System.Drawing.SystemColors.Control
        Me.Button_Edit.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button_Edit.Font = New System.Drawing.Font("Arial", 8.0!)
        Me.Button_Edit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button_Edit.Image = CType(resources.GetObject("Button_Edit.Image"), System.Drawing.Image)
        Me.Button_Edit.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Button_Edit.Location = New System.Drawing.Point(59, 418)
        Me.Button_Edit.Name = "Button_Edit"
        Me.Button_Edit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button_Edit.Size = New System.Drawing.Size(41, 41)
        Me.Button_Edit.TabIndex = 97
        Me.Button_Edit.Tag = "B"
        Me.Button_Edit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.Button_Edit, "Edit Cost Promotion")
        Me.Button_Edit.UseVisualStyleBackColor = False
        '
        'Button_Delete
        '
        Me.Button_Delete.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Button_Delete.BackColor = System.Drawing.SystemColors.Control
        Me.Button_Delete.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button_Delete.Font = New System.Drawing.Font("Arial", 8.0!)
        Me.Button_Delete.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button_Delete.Image = CType(resources.GetObject("Button_Delete.Image"), System.Drawing.Image)
        Me.Button_Delete.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Button_Delete.Location = New System.Drawing.Point(12, 418)
        Me.Button_Delete.Name = "Button_Delete"
        Me.Button_Delete.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button_Delete.Size = New System.Drawing.Size(41, 41)
        Me.Button_Delete.TabIndex = 98
        Me.Button_Delete.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.Button_Delete, "Delete Cost Promotion")
        Me.Button_Delete.UseVisualStyleBackColor = False
        '
        'CostPromotion
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(602, 471)
        Me.Controls.Add(Me.Button_Delete)
        Me.Controls.Add(Me.Button_Edit)
        Me.Controls.Add(Me.Button_Exit)
        Me.Controls.Add(Me.UltraGrid_VendorDeals)
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "CostPromotion"
        Me.Text = "Cost Promotion Detail"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.UltraGrid_VendorDeals, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Public WithEvents Label_PkgDesc As System.Windows.Forms.Label
    Public WithEvents Label_Item As System.Windows.Forms.Label
    Public WithEvents Label_Vendor As System.Windows.Forms.Label
    Friend WithEvents Label_PkgDescValue As System.Windows.Forms.Label
    Friend WithEvents Label_ItemValue As System.Windows.Forms.Label
    Friend WithEvents Label_VendorValue As System.Windows.Forms.Label
    Friend WithEvents UltraGrid_VendorDeals As Infragistics.Win.UltraWinGrid.UltraGrid
    Public WithEvents Button_Exit As System.Windows.Forms.Button
    Public WithEvents Button_Edit As System.Windows.Forms.Button
    Public WithEvents Button_Delete As System.Windows.Forms.Button
    Public WithEvents Label_StoreName As System.Windows.Forms.Label
    Friend WithEvents Label_StoreNameValue As System.Windows.Forms.Label
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
End Class
