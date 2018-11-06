Namespace WholeFoods.IRMA.CompetitorStore.UserInterface
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class CompetitorPriceGrid
        Inherits CompetitorGridBase

        'UserControl overrides dispose to clean up the component list.
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
            Dim Appearance1 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
            Dim Appearance2 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
            Dim UltraGridBand1 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("CompetitorPrice", -1)
            Dim UltraGridColumn1 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("UpdateUserID")
            Dim UltraGridColumn2 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("UpdateDateTime")
            Dim UltraGridColumn3 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Item_Key")
            Dim UltraGridColumn4 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("WFMIdentifier")
            Dim UltraGridColumn5 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("CompetitorID")
            Dim UltraGridColumn6 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("CompetitorLocationID")
            Dim UltraGridColumn7 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("CompetitorStoreID")
            Dim UltraGridColumn8 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("FiscalYear")
            Dim UltraGridColumn9 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("FiscalPeriod")
            Dim UltraGridColumn10 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("PeriodWeek")
            Dim UltraGridColumn11 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Competitor", -1, Nothing, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, False)
            Dim UltraGridColumn12 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Location")
            Dim UltraGridColumn13 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("CompetitorStore")
            Dim UltraGridColumn14 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("UPCCode")
            Dim UltraGridColumn15 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Description")
            Dim UltraGridColumn16 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Size")
            Dim Appearance3 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
            Dim UltraGridColumn17 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Unit_ID")
            Dim UltraGridColumn18 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("PriceMultiple")
            Dim Appearance4 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
            Dim UltraGridColumn19 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Price")
            Dim Appearance5 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
            Dim UltraGridColumn20 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("SaleMultiple")
            Dim Appearance6 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
            Dim UltraGridColumn21 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Sale")
            Dim Appearance7 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
            Dim UltraGridColumn22 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("CompetitorPriceID")
            Dim UltraGridColumn23 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("FiscalWeek", 0)
            Dim Appearance8 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
            Dim Appearance9 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
            Dim Appearance10 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
            Me.ugCompetitorPrices = New Infragistics.Win.UltraWinGrid.UltraGrid
            CType(Me.ugCompetitorPrices, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'ugCompetitorPrices
            '
            Me.ugCompetitorPrices.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                        Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Appearance1.BackColor = System.Drawing.Color.Gainsboro
            Me.ugCompetitorPrices.DisplayLayout.AddNewBox.Appearance = Appearance1
            Me.ugCompetitorPrices.DisplayLayout.AddNewBox.Hidden = False
            Appearance2.BackColor = System.Drawing.Color.White
            Appearance2.BorderColor = System.Drawing.SystemColors.InactiveCaption
            Me.ugCompetitorPrices.DisplayLayout.Appearance = Appearance2
            UltraGridBand1.AddButtonCaption = "Competitor Price"
            UltraGridColumn1.Header.VisiblePosition = 0
            UltraGridColumn1.Hidden = True
            UltraGridColumn2.Header.VisiblePosition = 1
            UltraGridColumn2.Hidden = True
            UltraGridColumn3.Header.VisiblePosition = 2
            UltraGridColumn3.Hidden = True
            UltraGridColumn3.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Null
            UltraGridColumn4.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
            UltraGridColumn4.Header.Caption = "Identifier"
            UltraGridColumn4.Header.VisiblePosition = 13
            UltraGridColumn4.Width = 78
            UltraGridColumn5.Header.VisiblePosition = 3
            UltraGridColumn5.Hidden = True
            UltraGridColumn5.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Null
            UltraGridColumn6.Header.VisiblePosition = 4
            UltraGridColumn6.Hidden = True
            UltraGridColumn6.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Null
            UltraGridColumn7.Header.VisiblePosition = 5
            UltraGridColumn7.Hidden = True
            UltraGridColumn7.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Null
            UltraGridColumn8.Header.VisiblePosition = 6
            UltraGridColumn8.Hidden = True
            UltraGridColumn9.Header.VisiblePosition = 7
            UltraGridColumn9.Hidden = True
            UltraGridColumn10.Header.VisiblePosition = 8
            UltraGridColumn10.Hidden = True
            UltraGridColumn11.Header.VisiblePosition = 9
            UltraGridColumn11.MaxLength = 50
            UltraGridColumn11.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Null
            UltraGridColumn12.Header.Caption = "Comp Location"
            UltraGridColumn12.Header.VisiblePosition = 10
            UltraGridColumn12.MaxLength = 50
            UltraGridColumn12.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Null
            UltraGridColumn13.Header.Caption = "Comp Store"
            UltraGridColumn13.Header.VisiblePosition = 11
            UltraGridColumn13.MaxLength = 50
            UltraGridColumn13.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Null
            UltraGridColumn14.Header.Caption = "UPC"
            UltraGridColumn14.Header.VisiblePosition = 12
            UltraGridColumn14.MaxLength = 50
            UltraGridColumn14.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Null
            UltraGridColumn15.Header.VisiblePosition = 14
            UltraGridColumn15.MaxLength = 250
            Appearance3.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGridColumn16.CellAppearance = Appearance3
            UltraGridColumn16.Format = "#.00"
            UltraGridColumn16.Header.VisiblePosition = 15
            UltraGridColumn17.Header.Caption = "Unit Of Measure"
            UltraGridColumn17.Header.VisiblePosition = 16
            Appearance4.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGridColumn18.CellAppearance = Appearance4
            UltraGridColumn18.DefaultCellValue = "1"
            UltraGridColumn18.Header.Caption = "Price Multiple"
            UltraGridColumn18.Header.VisiblePosition = 17
            Appearance5.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGridColumn19.CellAppearance = Appearance5
            UltraGridColumn19.CellDisplayStyle = Infragistics.Win.UltraWinGrid.CellDisplayStyle.FormattedText
            UltraGridColumn19.Format = "C"
            UltraGridColumn19.Header.VisiblePosition = 18
            Appearance6.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGridColumn20.CellAppearance = Appearance6
            UltraGridColumn20.Header.Caption = "Sale Multiple"
            UltraGridColumn20.Header.VisiblePosition = 19
            Appearance7.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGridColumn21.CellAppearance = Appearance7
            UltraGridColumn21.CellDisplayStyle = Infragistics.Win.UltraWinGrid.CellDisplayStyle.FormattedText
            UltraGridColumn21.Format = "C"
            UltraGridColumn21.Header.Caption = "Sale Price"
            UltraGridColumn21.Header.VisiblePosition = 20
            UltraGridColumn22.Header.VisiblePosition = 22
            UltraGridColumn22.Hidden = True
            UltraGridColumn23.DataType = GetType(WholeFoods.IRMA.CompetitorStore.BusinessLogic.CompetitorStoreDataSet.FiscalWeekRow)
            UltraGridColumn23.Header.Caption = "Fiscal Week"
            UltraGridColumn23.Header.VisiblePosition = 21
            UltraGridColumn23.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Disallow
            UltraGridColumn23.Width = 125
            UltraGridBand1.Columns.AddRange(New Object() {UltraGridColumn1, UltraGridColumn2, UltraGridColumn3, UltraGridColumn4, UltraGridColumn5, UltraGridColumn6, UltraGridColumn7, UltraGridColumn8, UltraGridColumn9, UltraGridColumn10, UltraGridColumn11, UltraGridColumn12, UltraGridColumn13, UltraGridColumn14, UltraGridColumn15, UltraGridColumn16, UltraGridColumn17, UltraGridColumn18, UltraGridColumn19, UltraGridColumn20, UltraGridColumn21, UltraGridColumn22, UltraGridColumn23})
            Me.ugCompetitorPrices.DisplayLayout.BandsSerializer.Add(UltraGridBand1)
            Me.ugCompetitorPrices.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
            Me.ugCompetitorPrices.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
            Me.ugCompetitorPrices.DisplayLayout.MaxColScrollRegions = 1
            Me.ugCompetitorPrices.DisplayLayout.MaxRowScrollRegions = 1
            Me.ugCompetitorPrices.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.Yes
            Me.ugCompetitorPrices.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.NotAllowed
            Me.ugCompetitorPrices.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed
            Me.ugCompetitorPrices.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.[True]
            Me.ugCompetitorPrices.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.[True]
            Appearance8.BackColor = System.Drawing.Color.Yellow
            Me.ugCompetitorPrices.DisplayLayout.Override.DataErrorCellAppearance = Appearance8
            Appearance9.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
            Me.ugCompetitorPrices.DisplayLayout.Override.DataErrorRowAppearance = Appearance9
            Appearance10.BackColor = System.Drawing.Color.LightSteelBlue
            Appearance10.BackColor2 = System.Drawing.Color.White
            Me.ugCompetitorPrices.DisplayLayout.Override.HeaderAppearance = Appearance10
            Me.ugCompetitorPrices.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
            Me.ugCompetitorPrices.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None
            Me.ugCompetitorPrices.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Extended
            Me.ugCompetitorPrices.DisplayLayout.Override.SortComparisonType = Infragistics.Win.UltraWinGrid.SortComparisonType.CaseInsensitive
            Me.ugCompetitorPrices.DisplayLayout.Override.SupportDataErrorInfo = Infragistics.Win.UltraWinGrid.SupportDataErrorInfo.RowsAndCells
            Me.ugCompetitorPrices.DisplayLayout.Override.TipStyleScroll = Infragistics.Win.UltraWinGrid.TipStyle.Hide
            Me.ugCompetitorPrices.DisplayLayout.Override.WrapHeaderText = Infragistics.Win.DefaultableBoolean.[True]
            Me.ugCompetitorPrices.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
            Me.ugCompetitorPrices.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
            Me.ugCompetitorPrices.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand
            Me.ugCompetitorPrices.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.ugCompetitorPrices.Location = New System.Drawing.Point(0, 0)
            Me.ugCompetitorPrices.Name = "ugCompetitorPrices"
            Me.ugCompetitorPrices.Size = New System.Drawing.Size(718, 341)
            Me.ugCompetitorPrices.TabIndex = 0
            Me.ugCompetitorPrices.Text = "UltraGrid1"
            '
            'CompetitorPriceGrid
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Controls.Add(Me.ugCompetitorPrices)
            Me.MinimumSize = New System.Drawing.Size(600, 250)
            Me.Name = "CompetitorPriceGrid"
            Me.Size = New System.Drawing.Size(718, 341)
            CType(Me.ugCompetitorPrices, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents ugCompetitorPrices As Infragistics.Win.UltraWinGrid.UltraGrid
    End Class
End Namespace