Namespace WholeFoods.IRMA.CompetitorStore.UserInterface
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class CompetitorImportInfoGrid
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
            Dim UltraGridBand1 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("CompetitorImportInfo", -1)
            Dim UltraGridColumn1 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("CompetitorImportInfoID")
            Dim UltraGridColumn2 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("CompetitorImportSessionID")
            Dim UltraGridColumn3 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Item_Key")
            Dim UltraGridColumn4 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("WFMIdentifier")
            Dim UltraGridColumn5 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("CompetitorID")
            Dim UltraGridColumn6 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("CompetitorLocationID")
            Dim UltraGridColumn7 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("CompetitorStoreID")
            Dim UltraGridColumn8 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("FiscalYear")
            Dim UltraGridColumn9 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("FiscalPeriod")
            Dim UltraGridColumn10 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("PeriodWeek")
            Dim UltraGridColumn11 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Competitor")
            Dim UltraGridColumn12 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Location")
            Dim UltraGridColumn13 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("CompetitorStore")
            Dim UltraGridColumn14 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("UPCCode")
            Dim UltraGridColumn15 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Description", -1, Nothing, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, False)
            Dim UltraGridColumn16 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Size")
            Dim Appearance2 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
            Dim UltraGridColumn17 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Unit_ID")
            Dim UltraGridColumn18 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("PriceMultiple")
            Dim Appearance3 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
            Dim UltraGridColumn19 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Price")
            Dim Appearance4 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
            Dim UltraGridColumn20 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("SaleMultiple")
            Dim Appearance5 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
            Dim UltraGridColumn21 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Sale")
            Dim Appearance6 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
            Dim UltraGridColumn22 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("CheckDate")
            Dim UltraGridColumn23 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("UnitOfMeasure")
            Dim UltraGridColumn24 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("FiscalWeek", 0)
            Dim Appearance7 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
            Dim Appearance8 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
            Dim Appearance9 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
            Me.ugPreview = New Infragistics.Win.UltraWinGrid.UltraGrid
            CType(Me.ugPreview, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'ugPreview
            '
            Me.ugPreview.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                        Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Appearance1.BackColor = System.Drawing.Color.White
            Appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption
            Me.ugPreview.DisplayLayout.Appearance = Appearance1
            UltraGridColumn1.Header.VisiblePosition = 0
            UltraGridColumn1.Hidden = True
            UltraGridColumn2.Header.VisiblePosition = 1
            UltraGridColumn2.Hidden = True
            UltraGridColumn3.Header.VisiblePosition = 2
            UltraGridColumn3.Hidden = True
            UltraGridColumn4.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
            UltraGridColumn4.Header.Caption = "Identifier"
            UltraGridColumn4.Header.VisiblePosition = 14
            UltraGridColumn4.Width = 78
            UltraGridColumn5.Header.VisiblePosition = 3
            UltraGridColumn5.Hidden = True
            UltraGridColumn6.Header.VisiblePosition = 4
            UltraGridColumn6.Hidden = True
            UltraGridColumn7.Header.VisiblePosition = 5
            UltraGridColumn7.Hidden = True
            UltraGridColumn8.Header.VisiblePosition = 6
            UltraGridColumn8.Hidden = True
            UltraGridColumn9.Header.VisiblePosition = 7
            UltraGridColumn9.Hidden = True
            UltraGridColumn10.Header.VisiblePosition = 8
            UltraGridColumn10.Hidden = True
            UltraGridColumn11.Header.VisiblePosition = 10
            UltraGridColumn11.MaxLength = 50
            UltraGridColumn12.Header.Caption = "Comp Location"
            UltraGridColumn12.Header.VisiblePosition = 11
            UltraGridColumn12.MaxLength = 50
            UltraGridColumn12.Width = 93
            UltraGridColumn13.Header.Caption = "Comp Store"
            UltraGridColumn13.Header.VisiblePosition = 12
            UltraGridColumn13.MaxLength = 50
            UltraGridColumn13.Width = 91
            UltraGridColumn14.Header.Caption = "UPC"
            UltraGridColumn14.Header.VisiblePosition = 13
            UltraGridColumn14.MaxLength = 50
            UltraGridColumn14.Width = 81
            UltraGridColumn15.Header.VisiblePosition = 15
            UltraGridColumn15.MaxLength = 250
            Appearance2.TextHAlignAsString = "Right"
            UltraGridColumn16.CellAppearance = Appearance2
            UltraGridColumn16.Format = "#.00"
            UltraGridColumn16.Header.VisiblePosition = 16
            UltraGridColumn16.Width = 51
            UltraGridColumn17.Header.Caption = "Unit Of Measure"
            UltraGridColumn17.Header.VisiblePosition = 17
            UltraGridColumn17.Width = 59
            Appearance3.TextHAlignAsString = "Right"
            UltraGridColumn18.CellAppearance = Appearance3
            UltraGridColumn18.Header.Caption = "Price Multiple"
            UltraGridColumn18.Header.VisiblePosition = 18
            UltraGridColumn18.Width = 52
            Appearance4.TextHAlignAsString = "Right"
            UltraGridColumn19.CellAppearance = Appearance4
            UltraGridColumn19.CellDisplayStyle = Infragistics.Win.UltraWinGrid.CellDisplayStyle.FormattedText
            UltraGridColumn19.Format = "C"
            UltraGridColumn19.Header.VisiblePosition = 19
            UltraGridColumn19.Width = 57
            Appearance5.TextHAlignAsString = "Right"
            UltraGridColumn20.CellAppearance = Appearance5
            UltraGridColumn20.Header.Caption = "Sale Multiple"
            UltraGridColumn20.Header.VisiblePosition = 20
            UltraGridColumn20.Width = 49
            Appearance6.TextHAlignAsString = "Right"
            UltraGridColumn21.CellAppearance = Appearance6
            UltraGridColumn21.CellDisplayStyle = Infragistics.Win.UltraWinGrid.CellDisplayStyle.FormattedText
            UltraGridColumn21.Format = "C"
            UltraGridColumn21.Header.Caption = "Sale Price"
            UltraGridColumn21.Header.VisiblePosition = 21
            UltraGridColumn21.Width = 39
            UltraGridColumn22.Header.Caption = "Date Checked"
            UltraGridColumn22.Header.VisiblePosition = 9
            UltraGridColumn22.Width = 73
            UltraGridColumn23.Header.VisiblePosition = 22
            UltraGridColumn23.Hidden = True
            UltraGridColumn24.DataType = GetType(WholeFoods.IRMA.CompetitorStore.BusinessLogic.CompetitorStoreDataSet.FiscalWeekRow)
            UltraGridColumn24.Header.Caption = "Fiscal Week"
            UltraGridColumn24.Header.VisiblePosition = 23
            UltraGridColumn24.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Disallow
            UltraGridColumn24.Width = 125
            UltraGridBand1.Columns.AddRange(New Object() {UltraGridColumn1, UltraGridColumn2, UltraGridColumn3, UltraGridColumn4, UltraGridColumn5, UltraGridColumn6, UltraGridColumn7, UltraGridColumn8, UltraGridColumn9, UltraGridColumn10, UltraGridColumn11, UltraGridColumn12, UltraGridColumn13, UltraGridColumn14, UltraGridColumn15, UltraGridColumn16, UltraGridColumn17, UltraGridColumn18, UltraGridColumn19, UltraGridColumn20, UltraGridColumn21, UltraGridColumn22, UltraGridColumn23, UltraGridColumn24})
            Me.ugPreview.DisplayLayout.BandsSerializer.Add(UltraGridBand1)
            Me.ugPreview.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
            Me.ugPreview.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
            Me.ugPreview.DisplayLayout.MaxColScrollRegions = 1
            Me.ugPreview.DisplayLayout.MaxRowScrollRegions = 1
            Me.ugPreview.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.NotAllowed
            Me.ugPreview.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed
            Me.ugPreview.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.[True]
            Me.ugPreview.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.[True]
            Appearance7.BackColor = System.Drawing.Color.Yellow
            Me.ugPreview.DisplayLayout.Override.DataErrorCellAppearance = Appearance7
            Appearance8.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
            Me.ugPreview.DisplayLayout.Override.DataErrorRowAppearance = Appearance8
            Appearance9.BackColor = System.Drawing.Color.LightSteelBlue
            Appearance9.BackColor2 = System.Drawing.Color.White
            Me.ugPreview.DisplayLayout.Override.HeaderAppearance = Appearance9
            Me.ugPreview.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
            Me.ugPreview.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None
            Me.ugPreview.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Extended
            Me.ugPreview.DisplayLayout.Override.SortComparisonType = Infragistics.Win.UltraWinGrid.SortComparisonType.CaseInsensitive
            Me.ugPreview.DisplayLayout.Override.SupportDataErrorInfo = Infragistics.Win.UltraWinGrid.SupportDataErrorInfo.RowsAndCells
            Me.ugPreview.DisplayLayout.Override.TipStyleScroll = Infragistics.Win.UltraWinGrid.TipStyle.Hide
            Me.ugPreview.DisplayLayout.Override.WrapHeaderText = Infragistics.Win.DefaultableBoolean.[True]
            Me.ugPreview.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
            Me.ugPreview.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
            Me.ugPreview.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand
            Me.ugPreview.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.ugPreview.Location = New System.Drawing.Point(0, 0)
            Me.ugPreview.Name = "ugPreview"
            Me.ugPreview.Size = New System.Drawing.Size(718, 341)
            Me.ugPreview.TabIndex = 0
            Me.ugPreview.Text = "UltraGrid1"
            '
            'CompetitorImportInfoGrid
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Controls.Add(Me.ugPreview)
            Me.MinimumSize = New System.Drawing.Size(600, 250)
            Me.Name = "CompetitorImportInfoGrid"
            Me.Size = New System.Drawing.Size(718, 341)
            CType(Me.ugPreview, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents ugPreview As Infragistics.Win.UltraWinGrid.UltraGrid
    End Class
End Namespace