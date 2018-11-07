Namespace WholeFoods.IRMA.CompetitorStore.UserInterface
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class frmNationalPurchasingValue
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
            Dim Appearance1 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
            Dim UltraGridBand1 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("CompetitivePriceInfo", -1)
            Dim UltraGridColumn1 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Item_Key")
            Dim UltraGridColumn2 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Store_No")
            Dim UltraGridColumn3 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Store_Name", -1, Nothing, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, False)
            Dim Appearance2 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
            Dim UltraGridColumn4 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("SubTeam_Name")
            Dim UltraGridColumn5 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Category_Name")
            Dim UltraGridColumn6 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("PriceAndMultiple")
            Dim Appearance3 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
            Dim UltraGridColumn7 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Brand_Name")
            Dim UltraGridColumn8 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Identifier")
            Dim UltraGridColumn9 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Item_Description")
            Dim UltraGridColumn10 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("CompetitivePriceTypeID")
            Dim UltraGridColumn11 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("BandwidthPercentageHigh")
            Dim Appearance4 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
            Dim UltraGridColumn12 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("BandwidthPercentageLow")
            Dim Appearance5 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
            Dim UltraGridColumn13 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Price")
            Dim UltraGridColumn14 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Multiple")
            Dim UltraGridColumn15 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("FIACategory")
            Dim Appearance6 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
            Dim Appearance7 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
            Dim Appearance8 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
            Me.ugNPV = New Infragistics.Win.UltraWinGrid.UltraGrid
            Me.isSearch = New WholeFoods.IRMA.ItemChaining.UserInterface.ItemSearchControl
            Me.lblStore = New System.Windows.Forms.Label
            Me.lblCompetitivePriceType = New System.Windows.Forms.Label
            Me.cmbStore = New System.Windows.Forms.ComboBox
            Me.cmbCompetitivePriceType = New System.Windows.Forms.ComboBox
            Me.btnClear = New System.Windows.Forms.Button
            Me.btnSearch = New System.Windows.Forms.Button
            Me.btnExit = New System.Windows.Forms.Button
            Me.btnSave = New System.Windows.Forms.Button
            Me.isoItemOptions = New WholeFoods.IRMA.ItemChaining.UserInterface.ItemSearchItemOptions
            Me.gbSearchCriteria = New System.Windows.Forms.GroupBox
            Me.gbResults = New System.Windows.Forms.GroupBox
            CType(Me.ugNPV, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.gbSearchCriteria.SuspendLayout()
            Me.gbResults.SuspendLayout()
            Me.SuspendLayout()
            '
            'ugNPV
            '
            Appearance1.BackColor = System.Drawing.Color.White
            Appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption
            Me.ugNPV.DisplayLayout.Appearance = Appearance1
            UltraGridColumn1.Header.VisiblePosition = 0
            UltraGridColumn1.Hidden = True
            UltraGridColumn2.Header.VisiblePosition = 1
            UltraGridColumn2.Hidden = True
            UltraGridColumn3.CellActivation = Infragistics.Win.UltraWinGrid.Activation.ActivateOnly
            Appearance2.BackColor = System.Drawing.Color.LightSteelBlue
            UltraGridColumn3.Header.Appearance = Appearance2
            UltraGridColumn3.Header.Caption = "WFM Store"
            UltraGridColumn3.Header.VisiblePosition = 2
            UltraGridColumn4.CellActivation = Infragistics.Win.UltraWinGrid.Activation.ActivateOnly
            UltraGridColumn4.Header.Caption = "SubTeam"
            UltraGridColumn4.Header.VisiblePosition = 3
            UltraGridColumn5.CellActivation = Infragistics.Win.UltraWinGrid.Activation.ActivateOnly
            UltraGridColumn5.Header.Caption = "Class"
            UltraGridColumn5.Header.VisiblePosition = 5
            UltraGridColumn6.CellActivation = Infragistics.Win.UltraWinGrid.Activation.ActivateOnly
            Appearance3.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGridColumn6.CellAppearance = Appearance3
            UltraGridColumn6.Header.Caption = "Price"
            UltraGridColumn6.Header.VisiblePosition = 9
            UltraGridColumn7.CellActivation = Infragistics.Win.UltraWinGrid.Activation.ActivateOnly
            UltraGridColumn7.Header.Caption = "Brand"
            UltraGridColumn7.Header.VisiblePosition = 6
            UltraGridColumn8.CellActivation = Infragistics.Win.UltraWinGrid.Activation.ActivateOnly
            UltraGridColumn8.Header.VisiblePosition = 7
            UltraGridColumn9.CellActivation = Infragistics.Win.UltraWinGrid.Activation.ActivateOnly
            UltraGridColumn9.Header.Caption = "Description"
            UltraGridColumn9.Header.VisiblePosition = 8
            UltraGridColumn10.Header.Caption = "Comp Price Type"
            UltraGridColumn10.Header.VisiblePosition = 10
            Appearance4.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGridColumn11.CellAppearance = Appearance4
            UltraGridColumn11.Header.Caption = "% Range High"
            UltraGridColumn11.Header.VisiblePosition = 11
            Appearance5.TextHAlign = Infragistics.Win.HAlign.Right
            UltraGridColumn12.CellAppearance = Appearance5
            UltraGridColumn12.Header.Caption = "% Range Low"
            UltraGridColumn12.Header.VisiblePosition = 12
            UltraGridColumn13.Header.VisiblePosition = 13
            UltraGridColumn13.Hidden = True
            UltraGridColumn14.Header.VisiblePosition = 14
            UltraGridColumn14.Hidden = True
            UltraGridColumn15.CellActivation = Infragistics.Win.UltraWinGrid.Activation.ActivateOnly
            UltraGridColumn15.Header.Caption = "Category"
            UltraGridColumn15.Header.VisiblePosition = 4
            UltraGridBand1.Columns.AddRange(New Object() {UltraGridColumn1, UltraGridColumn2, UltraGridColumn3, UltraGridColumn4, UltraGridColumn5, UltraGridColumn6, UltraGridColumn7, UltraGridColumn8, UltraGridColumn9, UltraGridColumn10, UltraGridColumn11, UltraGridColumn12, UltraGridColumn13, UltraGridColumn14, UltraGridColumn15})
            Appearance6.BackColor = System.Drawing.Color.LightSteelBlue
            UltraGridBand1.Header.Appearance = Appearance6
            Me.ugNPV.DisplayLayout.BandsSerializer.Add(UltraGridBand1)
            Me.ugNPV.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
            Me.ugNPV.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
            Me.ugNPV.DisplayLayout.MaxColScrollRegions = 1
            Me.ugNPV.DisplayLayout.MaxRowScrollRegions = 1
            Me.ugNPV.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.NotAllowed
            Me.ugNPV.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed
            Appearance7.BackColor = System.Drawing.Color.Yellow
            Me.ugNPV.DisplayLayout.Override.DataErrorCellAppearance = Appearance7
            Appearance8.BackColor = System.Drawing.Color.LightSteelBlue
            Appearance8.BackColor2 = System.Drawing.Color.White
            Appearance8.ForeColor = System.Drawing.Color.Black
            Appearance8.TextHAlign = Infragistics.Win.HAlign.Center
            Me.ugNPV.DisplayLayout.Override.HeaderAppearance = Appearance8
            Me.ugNPV.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
            Me.ugNPV.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None
            Me.ugNPV.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Extended
            Me.ugNPV.DisplayLayout.Override.SortComparisonType = Infragistics.Win.UltraWinGrid.SortComparisonType.CaseInsensitive
            Me.ugNPV.DisplayLayout.Override.SupportDataErrorInfo = Infragistics.Win.UltraWinGrid.SupportDataErrorInfo.CellsOnly
            Me.ugNPV.DisplayLayout.Override.TipStyleScroll = Infragistics.Win.UltraWinGrid.TipStyle.Hide
            Me.ugNPV.DisplayLayout.Override.WrapHeaderText = Infragistics.Win.DefaultableBoolean.[True]
            Me.ugNPV.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
            Me.ugNPV.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
            Me.ugNPV.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand
            Me.ugNPV.Dock = System.Windows.Forms.DockStyle.Fill
            Me.ugNPV.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.ugNPV.Location = New System.Drawing.Point(3, 16)
            Me.ugNPV.Name = "ugNPV"
            Me.ugNPV.Size = New System.Drawing.Size(761, 232)
            Me.ugNPV.TabIndex = 0
            Me.ugNPV.Text = "UltraGrid1"
            '
            'isSearch
            '
            Me.isSearch.BackColor = System.Drawing.Color.Transparent
            Me.isSearch.Location = New System.Drawing.Point(6, 19)
            Me.isSearch.Name = "isSearch"
            Me.isSearch.ShowClearButton = False
            Me.isSearch.ShowHFM = False
            Me.isSearch.ShowItemCheckBoxes = False
            Me.isSearch.ShowSearchButton = False
            Me.isSearch.ShowWFM = False
            Me.isSearch.Size = New System.Drawing.Size(485, 198)
            Me.isSearch.TabIndex = 0
            '
            'lblStore
            '
            Me.lblStore.AutoSize = True
            Me.lblStore.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(178, Byte))
            Me.lblStore.Location = New System.Drawing.Point(553, 22)
            Me.lblStore.Name = "lblStore"
            Me.lblStore.Size = New System.Drawing.Size(43, 14)
            Me.lblStore.TabIndex = 1
            Me.lblStore.Text = "Store :"
            '
            'lblCompetitivePriceType
            '
            Me.lblCompetitivePriceType.AutoSize = True
            Me.lblCompetitivePriceType.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblCompetitivePriceType.Location = New System.Drawing.Point(490, 46)
            Me.lblCompetitivePriceType.Name = "lblCompetitivePriceType"
            Me.lblCompetitivePriceType.Size = New System.Drawing.Size(107, 14)
            Me.lblCompetitivePriceType.TabIndex = 2
            Me.lblCompetitivePriceType.Text = "Comp Price Type :"
            '
            'cmbStore
            '
            Me.cmbStore.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbStore.FormattingEnabled = True
            Me.cmbStore.Location = New System.Drawing.Point(601, 19)
            Me.cmbStore.Name = "cmbStore"
            Me.cmbStore.Size = New System.Drawing.Size(160, 21)
            Me.cmbStore.TabIndex = 3
            '
            'cmbCompetitivePriceType
            '
            Me.cmbCompetitivePriceType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbCompetitivePriceType.FormattingEnabled = True
            Me.cmbCompetitivePriceType.Location = New System.Drawing.Point(601, 43)
            Me.cmbCompetitivePriceType.Name = "cmbCompetitivePriceType"
            Me.cmbCompetitivePriceType.Size = New System.Drawing.Size(160, 21)
            Me.cmbCompetitivePriceType.TabIndex = 4
            '
            'btnClear
            '
            Me.btnClear.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnClear.Location = New System.Drawing.Point(605, 135)
            Me.btnClear.Name = "btnClear"
            Me.btnClear.Size = New System.Drawing.Size(75, 23)
            Me.btnClear.TabIndex = 5
            Me.btnClear.Text = "Clear"
            Me.btnClear.UseVisualStyleBackColor = True
            '
            'btnSearch
            '
            Me.btnSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnSearch.Location = New System.Drawing.Point(686, 135)
            Me.btnSearch.Name = "btnSearch"
            Me.btnSearch.Size = New System.Drawing.Size(75, 23)
            Me.btnSearch.TabIndex = 6
            Me.btnSearch.Text = "Search"
            Me.btnSearch.UseVisualStyleBackColor = True
            '
            'btnExit
            '
            Me.btnExit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnExit.Location = New System.Drawing.Point(13, 442)
            Me.btnExit.Name = "btnExit"
            Me.btnExit.Size = New System.Drawing.Size(75, 23)
            Me.btnExit.TabIndex = 7
            Me.btnExit.Text = "Exit"
            Me.btnExit.UseVisualStyleBackColor = True
            '
            'btnSave
            '
            Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnSave.Location = New System.Drawing.Point(705, 442)
            Me.btnSave.Name = "btnSave"
            Me.btnSave.Size = New System.Drawing.Size(75, 23)
            Me.btnSave.TabIndex = 8
            Me.btnSave.Text = "Save"
            Me.btnSave.UseVisualStyleBackColor = True
            '
            'isoItemOptions
            '
            Me.isoItemOptions.BackColor = System.Drawing.Color.Transparent
            Me.isoItemOptions.Location = New System.Drawing.Point(598, 68)
            Me.isoItemOptions.Name = "isoItemOptions"
            Me.isoItemOptions.ShowHFM = False
            Me.isoItemOptions.ShowWFM = False
            Me.isoItemOptions.Size = New System.Drawing.Size(305, 62)
            Me.isoItemOptions.TabIndex = 125
            '
            'gbSearchCriteria
            '
            Me.gbSearchCriteria.BackColor = System.Drawing.Color.Transparent
            Me.gbSearchCriteria.Controls.Add(Me.btnSearch)
            Me.gbSearchCriteria.Controls.Add(Me.btnClear)
            Me.gbSearchCriteria.Controls.Add(Me.lblStore)
            Me.gbSearchCriteria.Controls.Add(Me.lblCompetitivePriceType)
            Me.gbSearchCriteria.Controls.Add(Me.isSearch)
            Me.gbSearchCriteria.Controls.Add(Me.isoItemOptions)
            Me.gbSearchCriteria.Controls.Add(Me.cmbCompetitivePriceType)
            Me.gbSearchCriteria.Controls.Add(Me.cmbStore)
            Me.gbSearchCriteria.Location = New System.Drawing.Point(13, 12)
            Me.gbSearchCriteria.Name = "gbSearchCriteria"
            Me.gbSearchCriteria.Size = New System.Drawing.Size(767, 172)
            Me.gbSearchCriteria.TabIndex = 9
            Me.gbSearchCriteria.TabStop = False
            Me.gbSearchCriteria.Text = "Search Criteria"
            '
            'gbResults
            '
            Me.gbResults.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                        Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.gbResults.BackColor = System.Drawing.Color.Transparent
            Me.gbResults.Controls.Add(Me.ugNPV)
            Me.gbResults.Location = New System.Drawing.Point(13, 189)
            Me.gbResults.Name = "gbResults"
            Me.gbResults.Size = New System.Drawing.Size(767, 251)
            Me.gbResults.TabIndex = 10
            Me.gbResults.TabStop = False
            Me.gbResults.Text = "Results"
            '
            'frmNationalPurchasingValue
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.Color.White
            Me.ClientSize = New System.Drawing.Size(792, 477)
            Me.Controls.Add(Me.gbResults)
            Me.Controls.Add(Me.gbSearchCriteria)
            Me.Controls.Add(Me.btnSave)
            Me.Controls.Add(Me.btnExit)
            Me.MinimizeBox = False
            Me.MinimumSize = New System.Drawing.Size(800, 500)
            Me.Name = "frmNationalPurchasingValue"
            Me.ShowIcon = False
            Me.ShowInTaskbar = False
            Me.Text = "National Purchasing Value Maintenance"
            CType(Me.ugNPV, System.ComponentModel.ISupportInitialize).EndInit()
            Me.gbSearchCriteria.ResumeLayout(False)
            Me.gbSearchCriteria.PerformLayout()
            Me.gbResults.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents isSearch As WholeFoods.IRMA.ItemChaining.UserInterface.ItemSearchControl
        Friend WithEvents lblStore As System.Windows.Forms.Label
        Friend WithEvents lblCompetitivePriceType As System.Windows.Forms.Label
        Friend WithEvents cmbStore As System.Windows.Forms.ComboBox
        Friend WithEvents cmbCompetitivePriceType As System.Windows.Forms.ComboBox
        Friend WithEvents btnClear As System.Windows.Forms.Button
        Friend WithEvents btnSearch As System.Windows.Forms.Button
        Friend WithEvents btnExit As System.Windows.Forms.Button
        Friend WithEvents btnSave As System.Windows.Forms.Button
        Friend WithEvents gbSearchCriteria As System.Windows.Forms.GroupBox
        Friend WithEvents gbResults As System.Windows.Forms.GroupBox
        Friend WithEvents ugNPV As Infragistics.Win.UltraWinGrid.UltraGrid
        Friend WithEvents isoItemOptions As WholeFoods.IRMA.ItemChaining.UserInterface.ItemSearchItemOptions
    End Class
End Namespace