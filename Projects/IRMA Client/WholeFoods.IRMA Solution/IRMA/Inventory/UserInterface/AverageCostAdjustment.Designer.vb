<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAvgCostAdjustment
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
        Me.components = New System.ComponentModel.Container()
        Dim Appearance1 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridBand1 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1)
        Dim UltraGridColumn1 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Effective_Date", -1, Nothing, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, False)
        Dim Appearance2 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance3 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridColumn2 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("AvgCost")
        Dim Appearance4 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance5 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridColumn3 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Store_Name")
        Dim Appearance6 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance7 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridColumn4 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Source")
        Dim Appearance8 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance9 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridColumn5 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Reason")
        Dim Appearance10 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance11 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance12 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim ColScrollRegion1 As Infragistics.Win.UltraWinGrid.ColScrollRegion = New Infragistics.Win.UltraWinGrid.ColScrollRegion(779)
        Dim ColScrollRegion2 As Infragistics.Win.UltraWinGrid.ColScrollRegion = New Infragistics.Win.UltraWinGrid.ColScrollRegion(726)
        Dim ColScrollRegion3 As Infragistics.Win.UltraWinGrid.ColScrollRegion = New Infragistics.Win.UltraWinGrid.ColScrollRegion(0)
        Dim Appearance13 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
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
        Dim Appearance50 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAvgCostAdjustment))
        Me.grpHistory = New System.Windows.Forms.GroupBox()
        Me.gridHistory = New Infragistics.Win.UltraWinGrid.UltraGrid()
        Me.txtComments = New System.Windows.Forms.TextBox()
        Me.grpAdjustment = New System.Windows.Forms.GroupBox()
        Me.lblOutOfTolerance = New System.Windows.Forms.Label()
        Me.cmdSaveAdjustment = New System.Windows.Forms.Button()
        Me.cmbReason = New System.Windows.Forms.ComboBox()
        Me.lblReason = New System.Windows.Forms.Label()
        Me.txtAmount = New System.Windows.Forms.TextBox()
        Me.lblNewAvgCost = New System.Windows.Forms.Label()
        Me.lblComments = New System.Windows.Forms.Label()
        Me.frmToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.frmErrorProvider = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.fraStoreSelection = New System.Windows.Forms.GroupBox()
        Me.txtIdentifier = New System.Windows.Forms.TextBox()
        Me.lblIdentifier = New System.Windows.Forms.Label()
        Me.dtpEndDate = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor()
        Me.dtpStartDate = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor()
        Me.cmdItemSearch = New System.Windows.Forms.Button()
        Me.cmbSubTeam = New System.Windows.Forms.ComboBox()
        Me.txtItemDesc = New System.Windows.Forms.TextBox()
        Me.lblItemDesc = New System.Windows.Forms.Label()
        Me.lblDash = New System.Windows.Forms.Label()
        Me.chkCurrentCost = New System.Windows.Forms.CheckBox()
        Me.lblSubTeam = New System.Windows.Forms.Label()
        Me.cmbState = New System.Windows.Forms.ComboBox()
        Me.optSelectState = New System.Windows.Forms.RadioButton()
        Me.optSelectStore = New System.Windows.Forms.RadioButton()
        Me.cmbStoreSelection = New System.Windows.Forms.ComboBox()
        Me.optSelectALLStores = New System.Windows.Forms.RadioButton()
        Me.optSelectZone = New System.Windows.Forms.RadioButton()
        Me.cmbZones = New System.Windows.Forms.ComboBox()
        Me.optSelectHFM = New System.Windows.Forms.RadioButton()
        Me.optSelectWFM = New System.Windows.Forms.RadioButton()
        Me.lblDates = New System.Windows.Forms.Label()
        Me.grpHistory.SuspendLayout()
        CType(Me.gridHistory, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpAdjustment.SuspendLayout()
        CType(Me.frmErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.fraStoreSelection.SuspendLayout()
        CType(Me.dtpEndDate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dtpStartDate, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'grpHistory
        '
        Me.grpHistory.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpHistory.Controls.Add(Me.gridHistory)
        Me.grpHistory.Enabled = False
        Me.grpHistory.Location = New System.Drawing.Point(9, 252)
        Me.grpHistory.Name = "grpHistory"
        Me.grpHistory.Size = New System.Drawing.Size(800, 444)
        Me.grpHistory.TabIndex = 5
        Me.grpHistory.TabStop = False
        Me.grpHistory.Text = "History"
        '
        'gridHistory
        '
        Me.gridHistory.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Appearance1.BackColor = System.Drawing.SystemColors.ControlLight
        Appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.gridHistory.DisplayLayout.Appearance = Appearance1
        Me.gridHistory.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        Appearance2.TextHAlignAsString = "Right"
        UltraGridColumn1.CellAppearance = Appearance2
        Appearance3.TextHAlignAsString = "Center"
        UltraGridColumn1.Header.Appearance = Appearance3
        UltraGridColumn1.Header.Caption = "Effective Date"
        UltraGridColumn1.Header.Fixed = True
        UltraGridColumn1.Header.VisiblePosition = 1
        UltraGridColumn1.RowLayoutColumnInfo.OriginX = 0
        UltraGridColumn1.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn1.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn1.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn1.Width = 186
        Appearance4.TextHAlignAsString = "Right"
        UltraGridColumn2.CellAppearance = Appearance4
        Appearance5.TextHAlignAsString = "Center"
        UltraGridColumn2.Header.Appearance = Appearance5
        UltraGridColumn2.Header.Fixed = True
        UltraGridColumn2.Header.VisiblePosition = 2
        UltraGridColumn2.RowLayoutColumnInfo.OriginX = 2
        UltraGridColumn2.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn2.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn2.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn2.Width = 160
        Appearance6.TextHAlignAsString = "Right"
        UltraGridColumn3.CellAppearance = Appearance6
        Appearance7.TextHAlignAsString = "Center"
        UltraGridColumn3.Header.Appearance = Appearance7
        UltraGridColumn3.Header.Caption = "StoreName"
        UltraGridColumn3.Header.Fixed = True
        UltraGridColumn3.Header.VisiblePosition = 0
        UltraGridColumn3.RowLayoutColumnInfo.OriginX = 4
        UltraGridColumn3.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn3.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn3.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn3.Width = 113
        Appearance8.TextHAlignAsString = "Right"
        UltraGridColumn4.CellAppearance = Appearance8
        Appearance9.TextHAlignAsString = "Center"
        UltraGridColumn4.Header.Appearance = Appearance9
        UltraGridColumn4.Header.Fixed = True
        UltraGridColumn4.Header.VisiblePosition = 4
        UltraGridColumn4.RowLayoutColumnInfo.OriginX = 6
        UltraGridColumn4.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn4.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn4.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn4.Width = 152
        Appearance10.TextHAlignAsString = "Right"
        UltraGridColumn5.CellAppearance = Appearance10
        Appearance11.TextHAlignAsString = "Center"
        UltraGridColumn5.Header.Appearance = Appearance11
        UltraGridColumn5.Header.Fixed = True
        UltraGridColumn5.Header.VisiblePosition = 3
        UltraGridColumn5.RowLayoutColumnInfo.OriginX = 8
        UltraGridColumn5.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn5.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn5.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn5.Width = 149
        UltraGridBand1.Columns.AddRange(New Object() {UltraGridColumn1, UltraGridColumn2, UltraGridColumn3, UltraGridColumn4, UltraGridColumn5})
        Me.gridHistory.DisplayLayout.BandsSerializer.Add(UltraGridBand1)
        Me.gridHistory.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Appearance12.FontData.BoldAsString = "True"
        Me.gridHistory.DisplayLayout.CaptionAppearance = Appearance12
        Me.gridHistory.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Me.gridHistory.DisplayLayout.ColScrollRegions.Add(ColScrollRegion1)
        Me.gridHistory.DisplayLayout.ColScrollRegions.Add(ColScrollRegion2)
        Me.gridHistory.DisplayLayout.ColScrollRegions.Add(ColScrollRegion3)
        Appearance13.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance13.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance13.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance13.BorderColor = System.Drawing.SystemColors.Window
        Me.gridHistory.DisplayLayout.GroupByBox.Appearance = Appearance13
        Appearance14.ForeColor = System.Drawing.SystemColors.GrayText
        Me.gridHistory.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance14
        Me.gridHistory.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.gridHistory.DisplayLayout.GroupByBox.Hidden = True
        Appearance15.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance15.BackColor2 = System.Drawing.SystemColors.Control
        Appearance15.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance15.ForeColor = System.Drawing.SystemColors.GrayText
        Me.gridHistory.DisplayLayout.GroupByBox.PromptAppearance = Appearance15
        Me.gridHistory.DisplayLayout.MaxColScrollRegions = 1
        Me.gridHistory.DisplayLayout.MaxRowScrollRegions = 1
        Appearance16.BackColor = System.Drawing.SystemColors.Window
        Appearance16.ForeColor = System.Drawing.SystemColors.ControlText
        Me.gridHistory.DisplayLayout.Override.ActiveCellAppearance = Appearance16
        Appearance17.FontData.BoldAsString = "True"
        Appearance17.ForeColor = System.Drawing.Color.White
        Me.gridHistory.DisplayLayout.Override.ActiveRowAppearance = Appearance17
        Me.gridHistory.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No
        Me.gridHistory.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.NotAllowed
        Me.gridHistory.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.[False]
        Me.gridHistory.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.[False]
        Me.gridHistory.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.[False]
        Me.gridHistory.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.gridHistory.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance18.BackColor = System.Drawing.SystemColors.Window
        Me.gridHistory.DisplayLayout.Override.CardAreaAppearance = Appearance18
        Appearance19.BorderColor = System.Drawing.Color.Silver
        Appearance19.FontData.BoldAsString = "True"
        Appearance19.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.gridHistory.DisplayLayout.Override.CellAppearance = Appearance19
        Me.gridHistory.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        Me.gridHistory.DisplayLayout.Override.CellPadding = 0
        Appearance20.FontData.BoldAsString = "True"
        Me.gridHistory.DisplayLayout.Override.FixedHeaderAppearance = Appearance20
        Appearance21.BackColor = System.Drawing.SystemColors.Control
        Appearance21.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance21.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance21.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance21.BorderColor = System.Drawing.SystemColors.Window
        Me.gridHistory.DisplayLayout.Override.GroupByRowAppearance = Appearance21
        Appearance22.FontData.BoldAsString = "True"
        Appearance22.TextHAlignAsString = "Left"
        Me.gridHistory.DisplayLayout.Override.HeaderAppearance = Appearance22
        Me.gridHistory.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti
        Me.gridHistory.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.XPThemed
        Appearance23.BackColor = System.Drawing.SystemColors.Control
        Me.gridHistory.DisplayLayout.Override.RowAlternateAppearance = Appearance23
        Appearance24.BackColor = System.Drawing.SystemColors.Window
        Appearance24.BorderColor = System.Drawing.Color.Silver
        Me.gridHistory.DisplayLayout.Override.RowAppearance = Appearance24
        Me.gridHistory.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[True]
        Me.gridHistory.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed
        Me.gridHistory.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.gridHistory.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.gridHistory.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.[Single]
        Appearance50.BackColor = System.Drawing.SystemColors.ControlLight
        Me.gridHistory.DisplayLayout.Override.TemplateAddRowAppearance = Appearance50
        Me.gridHistory.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.gridHistory.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.gridHistory.DisplayLayout.UseFixedHeaders = True
        Me.gridHistory.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand
        Me.gridHistory.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        Me.gridHistory.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gridHistory.Location = New System.Drawing.Point(9, 21)
        Me.gridHistory.Name = "gridHistory"
        Me.gridHistory.Size = New System.Drawing.Size(781, 417)
        Me.gridHistory.TabIndex = 66
        '
        'txtComments
        '
        Me.txtComments.Location = New System.Drawing.Point(430, 27)
        Me.txtComments.Multiline = True
        Me.txtComments.Name = "txtComments"
        Me.txtComments.Size = New System.Drawing.Size(200, 47)
        Me.txtComments.TabIndex = 5
        '
        'grpAdjustment
        '
        Me.grpAdjustment.Controls.Add(Me.lblOutOfTolerance)
        Me.grpAdjustment.Controls.Add(Me.cmdSaveAdjustment)
        Me.grpAdjustment.Controls.Add(Me.txtComments)
        Me.grpAdjustment.Controls.Add(Me.cmbReason)
        Me.grpAdjustment.Controls.Add(Me.lblReason)
        Me.grpAdjustment.Controls.Add(Me.txtAmount)
        Me.grpAdjustment.Controls.Add(Me.lblNewAvgCost)
        Me.grpAdjustment.Controls.Add(Me.lblComments)
        Me.grpAdjustment.Enabled = False
        Me.grpAdjustment.Location = New System.Drawing.Point(10, 166)
        Me.grpAdjustment.Name = "grpAdjustment"
        Me.grpAdjustment.Size = New System.Drawing.Size(800, 81)
        Me.grpAdjustment.TabIndex = 2
        Me.grpAdjustment.TabStop = False
        Me.grpAdjustment.Text = "Adjustment"
        '
        'lblOutOfTolerance
        '
        Me.lblOutOfTolerance.AutoSize = True
        Me.lblOutOfTolerance.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOutOfTolerance.ForeColor = System.Drawing.Color.Red
        Me.lblOutOfTolerance.Location = New System.Drawing.Point(198, 20)
        Me.lblOutOfTolerance.Name = "lblOutOfTolerance"
        Me.lblOutOfTolerance.Size = New System.Drawing.Size(280, 19)
        Me.lblOutOfTolerance.TabIndex = 6
        Me.lblOutOfTolerance.Text = "Entry exceeds {0}% adjustment tolerance"
        Me.lblOutOfTolerance.Visible = False
        '
        'cmdSaveAdjustment
        '
        Me.cmdSaveAdjustment.Image = CType(resources.GetObject("cmdSaveAdjustment.Image"), System.Drawing.Image)
        Me.cmdSaveAdjustment.Location = New System.Drawing.Point(652, 22)
        Me.cmdSaveAdjustment.Name = "cmdSaveAdjustment"
        Me.cmdSaveAdjustment.Size = New System.Drawing.Size(75, 42)
        Me.cmdSaveAdjustment.TabIndex = 3
        Me.cmdSaveAdjustment.Text = "Save"
        Me.cmdSaveAdjustment.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cmdSaveAdjustment.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.frmToolTip.SetToolTip(Me.cmdSaveAdjustment, "Save Adjustment")
        Me.cmdSaveAdjustment.UseVisualStyleBackColor = True
        '
        'cmbReason
        '
        Me.cmbReason.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbReason.FormattingEnabled = True
        Me.cmbReason.Location = New System.Drawing.Point(103, 44)
        Me.cmbReason.Name = "cmbReason"
        Me.cmbReason.Size = New System.Drawing.Size(222, 27)
        Me.cmbReason.TabIndex = 3
        '
        'lblReason
        '
        Me.lblReason.AutoSize = True
        Me.lblReason.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReason.Location = New System.Drawing.Point(49, 47)
        Me.lblReason.Name = "lblReason"
        Me.lblReason.Size = New System.Drawing.Size(61, 19)
        Me.lblReason.TabIndex = 2
        Me.lblReason.Text = "Reason:"
        '
        'txtAmount
        '
        Me.txtAmount.Location = New System.Drawing.Point(103, 17)
        Me.txtAmount.Name = "txtAmount"
        Me.txtAmount.Size = New System.Drawing.Size(89, 26)
        Me.txtAmount.TabIndex = 1
        '
        'lblNewAvgCost
        '
        Me.lblNewAvgCost.AutoSize = True
        Me.lblNewAvgCost.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNewAvgCost.Location = New System.Drawing.Point(10, 20)
        Me.lblNewAvgCost.Name = "lblNewAvgCost"
        Me.lblNewAvgCost.Size = New System.Drawing.Size(111, 19)
        Me.lblNewAvgCost.TabIndex = 0
        Me.lblNewAvgCost.Text = "New Avg. Cost:"
        '
        'lblComments
        '
        Me.lblComments.AutoSize = True
        Me.lblComments.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblComments.Location = New System.Drawing.Point(427, 11)
        Me.lblComments.Name = "lblComments"
        Me.lblComments.Size = New System.Drawing.Size(84, 19)
        Me.lblComments.TabIndex = 4
        Me.lblComments.Text = "Comments:"
        '
        'frmErrorProvider
        '
        Me.frmErrorProvider.ContainerControl = Me
        '
        'fraStoreSelection
        '
        Me.fraStoreSelection.BackColor = System.Drawing.SystemColors.Control
        Me.fraStoreSelection.Controls.Add(Me.txtIdentifier)
        Me.fraStoreSelection.Controls.Add(Me.lblIdentifier)
        Me.fraStoreSelection.Controls.Add(Me.dtpEndDate)
        Me.fraStoreSelection.Controls.Add(Me.dtpStartDate)
        Me.fraStoreSelection.Controls.Add(Me.cmdItemSearch)
        Me.fraStoreSelection.Controls.Add(Me.cmbSubTeam)
        Me.fraStoreSelection.Controls.Add(Me.txtItemDesc)
        Me.fraStoreSelection.Controls.Add(Me.lblItemDesc)
        Me.fraStoreSelection.Controls.Add(Me.lblDash)
        Me.fraStoreSelection.Controls.Add(Me.chkCurrentCost)
        Me.fraStoreSelection.Controls.Add(Me.lblSubTeam)
        Me.fraStoreSelection.Controls.Add(Me.cmbState)
        Me.fraStoreSelection.Controls.Add(Me.optSelectState)
        Me.fraStoreSelection.Controls.Add(Me.optSelectStore)
        Me.fraStoreSelection.Controls.Add(Me.cmbStoreSelection)
        Me.fraStoreSelection.Controls.Add(Me.optSelectALLStores)
        Me.fraStoreSelection.Controls.Add(Me.optSelectZone)
        Me.fraStoreSelection.Controls.Add(Me.cmbZones)
        Me.fraStoreSelection.Controls.Add(Me.optSelectHFM)
        Me.fraStoreSelection.Controls.Add(Me.optSelectWFM)
        Me.fraStoreSelection.Controls.Add(Me.lblDates)
        Me.fraStoreSelection.Font = New System.Drawing.Font("Arial", 8.25!)
        Me.fraStoreSelection.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraStoreSelection.Location = New System.Drawing.Point(9, 6)
        Me.fraStoreSelection.Name = "fraStoreSelection"
        Me.fraStoreSelection.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraStoreSelection.Size = New System.Drawing.Size(800, 154)
        Me.fraStoreSelection.TabIndex = 9
        Me.fraStoreSelection.TabStop = False
        Me.fraStoreSelection.Text = "Avg Cost History Selection"
        '
        'txtIdentifier
        '
        Me.txtIdentifier.AcceptsReturn = True
        Me.txtIdentifier.BackColor = System.Drawing.SystemColors.ControlLight
        Me.txtIdentifier.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtIdentifier.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.txtIdentifier.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtIdentifier.Location = New System.Drawing.Point(430, 125)
        Me.txtIdentifier.MaxLength = 18
        Me.txtIdentifier.Name = "txtIdentifier"
        Me.txtIdentifier.ReadOnly = True
        Me.txtIdentifier.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtIdentifier.Size = New System.Drawing.Size(200, 23)
        Me.txtIdentifier.TabIndex = 7
        Me.txtIdentifier.TabStop = False
        Me.txtIdentifier.Tag = "Integer"
        '
        'lblIdentifier
        '
        Me.lblIdentifier.AutoSize = True
        Me.lblIdentifier.BackColor = System.Drawing.Color.Transparent
        Me.lblIdentifier.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblIdentifier.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIdentifier.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblIdentifier.Location = New System.Drawing.Point(347, 125)
        Me.lblIdentifier.Name = "lblIdentifier"
        Me.lblIdentifier.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblIdentifier.Size = New System.Drawing.Size(74, 19)
        Me.lblIdentifier.TabIndex = 6
        Me.lblIdentifier.Text = "Identifier:"
        '
        'dtpEndDate
        '
        Me.dtpEndDate.DateTime = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpEndDate.Location = New System.Drawing.Point(545, 96)
        Me.dtpEndDate.Name = "dtpEndDate"
        Me.dtpEndDate.Size = New System.Drawing.Size(85, 25)
        Me.dtpEndDate.TabIndex = 23
        Me.dtpEndDate.Value = Nothing
        '
        'dtpStartDate
        '
        Me.dtpStartDate.Location = New System.Drawing.Point(430, 96)
        Me.dtpStartDate.Name = "dtpStartDate"
        Me.dtpStartDate.Size = New System.Drawing.Size(85, 25)
        Me.dtpStartDate.TabIndex = 21
        '
        'cmdItemSearch
        '
        Me.cmdItemSearch.BackColor = System.Drawing.SystemColors.Control
        Me.cmdItemSearch.CausesValidation = False
        Me.cmdItemSearch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdItemSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdItemSearch.Image = CType(resources.GetObject("cmdItemSearch.Image"), System.Drawing.Image)
        Me.cmdItemSearch.Location = New System.Drawing.Point(555, 44)
        Me.cmdItemSearch.Name = "cmdItemSearch"
        Me.cmdItemSearch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdItemSearch.Size = New System.Drawing.Size(75, 44)
        Me.cmdItemSearch.TabIndex = 3
        Me.cmdItemSearch.TabStop = False
        Me.cmdItemSearch.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.cmdItemSearch.UseVisualStyleBackColor = False
        '
        'cmbSubTeam
        '
        Me.cmbSubTeam.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbSubTeam.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbSubTeam.BackColor = System.Drawing.SystemColors.Window
        Me.cmbSubTeam.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbSubTeam.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbSubTeam.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbSubTeam.Location = New System.Drawing.Point(79, 95)
        Me.cmbSubTeam.Name = "cmbSubTeam"
        Me.cmbSubTeam.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbSubTeam.Size = New System.Drawing.Size(246, 24)
        Me.cmbSubTeam.Sorted = True
        Me.cmbSubTeam.TabIndex = 2
        '
        'txtItemDesc
        '
        Me.txtItemDesc.AcceptsReturn = True
        Me.txtItemDesc.BackColor = System.Drawing.SystemColors.ControlLight
        Me.txtItemDesc.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtItemDesc.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.txtItemDesc.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtItemDesc.Location = New System.Drawing.Point(78, 126)
        Me.txtItemDesc.MaxLength = 26
        Me.txtItemDesc.Name = "txtItemDesc"
        Me.txtItemDesc.ReadOnly = True
        Me.txtItemDesc.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtItemDesc.Size = New System.Drawing.Size(247, 23)
        Me.txtItemDesc.TabIndex = 5
        Me.txtItemDesc.TabStop = False
        Me.txtItemDesc.Tag = "String"
        '
        'lblItemDesc
        '
        Me.lblItemDesc.AutoSize = True
        Me.lblItemDesc.BackColor = System.Drawing.Color.Transparent
        Me.lblItemDesc.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblItemDesc.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblItemDesc.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblItemDesc.Location = New System.Drawing.Point(12, 129)
        Me.lblItemDesc.Name = "lblItemDesc"
        Me.lblItemDesc.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblItemDesc.Size = New System.Drawing.Size(78, 19)
        Me.lblItemDesc.TabIndex = 4
        Me.lblItemDesc.Text = "Item Desc:"
        '
        'lblDash
        '
        Me.lblDash.BackColor = System.Drawing.Color.Transparent
        Me.lblDash.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDash.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold)
        Me.lblDash.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDash.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblDash.Location = New System.Drawing.Point(522, 97)
        Me.lblDash.Name = "lblDash"
        Me.lblDash.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDash.Size = New System.Drawing.Size(17, 17)
        Me.lblDash.TabIndex = 22
        Me.lblDash.Text = "-"
        Me.lblDash.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'chkCurrentCost
        '
        Me.chkCurrentCost.BackColor = System.Drawing.SystemColors.Control
        Me.chkCurrentCost.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkCurrentCost.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkCurrentCost.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.chkCurrentCost.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkCurrentCost.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.chkCurrentCost.Location = New System.Drawing.Point(345, 69)
        Me.chkCurrentCost.Name = "chkCurrentCost"
        Me.chkCurrentCost.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkCurrentCost.Size = New System.Drawing.Size(127, 22)
        Me.chkCurrentCost.TabIndex = 17
        Me.chkCurrentCost.Text = "Current Avg Cost:"
        Me.chkCurrentCost.UseVisualStyleBackColor = False
        '
        'lblSubTeam
        '
        Me.lblSubTeam.AutoSize = True
        Me.lblSubTeam.BackColor = System.Drawing.Color.Transparent
        Me.lblSubTeam.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSubTeam.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSubTeam.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSubTeam.Location = New System.Drawing.Point(10, 98)
        Me.lblSubTeam.Name = "lblSubTeam"
        Me.lblSubTeam.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSubTeam.Size = New System.Drawing.Size(81, 19)
        Me.lblSubTeam.TabIndex = 2
        Me.lblSubTeam.Text = "Sub-Team:"
        '
        'cmbState
        '
        Me.cmbState.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbState.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbState.BackColor = System.Drawing.SystemColors.Window
        Me.cmbState.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbState.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbState.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.cmbState.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbState.Location = New System.Drawing.Point(78, 69)
        Me.cmbState.Name = "cmbState"
        Me.cmbState.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbState.Size = New System.Drawing.Size(57, 24)
        Me.cmbState.Sorted = True
        Me.cmbState.TabIndex = 19
        '
        'optSelectState
        '
        Me.optSelectState.BackColor = System.Drawing.SystemColors.Control
        Me.optSelectState.Cursor = System.Windows.Forms.Cursors.Default
        Me.optSelectState.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.optSelectState.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optSelectState.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.optSelectState.Location = New System.Drawing.Point(13, 68)
        Me.optSelectState.Name = "optSelectState"
        Me.optSelectState.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optSelectState.Size = New System.Drawing.Size(64, 17)
        Me.optSelectState.TabIndex = 18
        Me.optSelectState.TabStop = True
        Me.optSelectState.Text = "State"
        Me.optSelectState.UseVisualStyleBackColor = False
        '
        'optSelectStore
        '
        Me.optSelectStore.BackColor = System.Drawing.SystemColors.Control
        Me.optSelectStore.Checked = True
        Me.optSelectStore.Cursor = System.Windows.Forms.Cursors.Default
        Me.optSelectStore.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.optSelectStore.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optSelectStore.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.optSelectStore.Location = New System.Drawing.Point(13, 20)
        Me.optSelectStore.Name = "optSelectStore"
        Me.optSelectStore.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optSelectStore.Size = New System.Drawing.Size(64, 17)
        Me.optSelectStore.TabIndex = 9
        Me.optSelectStore.TabStop = True
        Me.optSelectStore.Text = "Store"
        Me.optSelectStore.UseVisualStyleBackColor = False
        '
        'cmbStoreSelection
        '
        Me.cmbStoreSelection.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbStoreSelection.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbStoreSelection.BackColor = System.Drawing.SystemColors.Window
        Me.cmbStoreSelection.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbStoreSelection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbStoreSelection.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.cmbStoreSelection.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbStoreSelection.Location = New System.Drawing.Point(78, 19)
        Me.cmbStoreSelection.Name = "cmbStoreSelection"
        Me.cmbStoreSelection.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbStoreSelection.Size = New System.Drawing.Size(247, 24)
        Me.cmbStoreSelection.Sorted = True
        Me.cmbStoreSelection.TabIndex = 10
        '
        'optSelectALLStores
        '
        Me.optSelectALLStores.BackColor = System.Drawing.SystemColors.Control
        Me.optSelectALLStores.Cursor = System.Windows.Forms.Cursors.Default
        Me.optSelectALLStores.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.optSelectALLStores.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optSelectALLStores.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.optSelectALLStores.Location = New System.Drawing.Point(469, 21)
        Me.optSelectALLStores.Name = "optSelectALLStores"
        Me.optSelectALLStores.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optSelectALLStores.Size = New System.Drawing.Size(89, 17)
        Me.optSelectALLStores.TabIndex = 13
        Me.optSelectALLStores.TabStop = True
        Me.optSelectALLStores.Text = "All Stores"
        Me.optSelectALLStores.UseVisualStyleBackColor = False
        '
        'optSelectZone
        '
        Me.optSelectZone.BackColor = System.Drawing.SystemColors.Control
        Me.optSelectZone.Cursor = System.Windows.Forms.Cursors.Default
        Me.optSelectZone.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.optSelectZone.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optSelectZone.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.optSelectZone.Location = New System.Drawing.Point(13, 44)
        Me.optSelectZone.Name = "optSelectZone"
        Me.optSelectZone.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optSelectZone.Size = New System.Drawing.Size(64, 17)
        Me.optSelectZone.TabIndex = 15
        Me.optSelectZone.TabStop = True
        Me.optSelectZone.Text = "Zone"
        Me.optSelectZone.UseVisualStyleBackColor = False
        '
        'cmbZones
        '
        Me.cmbZones.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbZones.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbZones.BackColor = System.Drawing.SystemColors.Window
        Me.cmbZones.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbZones.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbZones.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.cmbZones.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbZones.Location = New System.Drawing.Point(78, 43)
        Me.cmbZones.Name = "cmbZones"
        Me.cmbZones.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbZones.Size = New System.Drawing.Size(247, 24)
        Me.cmbZones.Sorted = True
        Me.cmbZones.TabIndex = 16
        '
        'optSelectHFM
        '
        Me.optSelectHFM.BackColor = System.Drawing.SystemColors.Control
        Me.optSelectHFM.Cursor = System.Windows.Forms.Cursors.Default
        Me.optSelectHFM.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.optSelectHFM.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optSelectHFM.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.optSelectHFM.Location = New System.Drawing.Point(335, 20)
        Me.optSelectHFM.Name = "optSelectHFM"
        Me.optSelectHFM.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optSelectHFM.Size = New System.Drawing.Size(66, 19)
        Me.optSelectHFM.TabIndex = 11
        Me.optSelectHFM.TabStop = True
        Me.optSelectHFM.Text = "All 365"
        Me.optSelectHFM.UseVisualStyleBackColor = False
        '
        'optSelectWFM
        '
        Me.optSelectWFM.BackColor = System.Drawing.SystemColors.Control
        Me.optSelectWFM.Cursor = System.Windows.Forms.Cursors.Default
        Me.optSelectWFM.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.optSelectWFM.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optSelectWFM.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.optSelectWFM.Location = New System.Drawing.Point(401, 21)
        Me.optSelectWFM.Name = "optSelectWFM"
        Me.optSelectWFM.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optSelectWFM.Size = New System.Drawing.Size(81, 17)
        Me.optSelectWFM.TabIndex = 12
        Me.optSelectWFM.TabStop = True
        Me.optSelectWFM.Text = "All WFM"
        Me.optSelectWFM.UseVisualStyleBackColor = False
        '
        'lblDates
        '
        Me.lblDates.BackColor = System.Drawing.Color.Transparent
        Me.lblDates.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDates.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.lblDates.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDates.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblDates.Location = New System.Drawing.Point(331, 98)
        Me.lblDates.Name = "lblDates"
        Me.lblDates.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDates.Size = New System.Drawing.Size(89, 17)
        Me.lblDates.TabIndex = 20
        Me.lblDates.Text = "Date Range :"
        Me.lblDates.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'frmAvgCostAdjustment
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 19.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange
        Me.ClientSize = New System.Drawing.Size(824, 708)
        Me.Controls.Add(Me.fraStoreSelection)
        Me.Controls.Add(Me.grpAdjustment)
        Me.Controls.Add(Me.grpHistory)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(840, 610)
        Me.Name = "frmAvgCostAdjustment"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Inventory Costing"
        Me.grpHistory.ResumeLayout(False)
        CType(Me.gridHistory, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpAdjustment.ResumeLayout(False)
        Me.grpAdjustment.PerformLayout()
        CType(Me.frmErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me.fraStoreSelection.ResumeLayout(False)
        Me.fraStoreSelection.PerformLayout()
        CType(Me.dtpEndDate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dtpStartDate, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpHistory As System.Windows.Forms.GroupBox
    Friend WithEvents grpAdjustment As System.Windows.Forms.GroupBox
    Friend WithEvents txtAmount As System.Windows.Forms.TextBox
    Friend WithEvents lblNewAvgCost As System.Windows.Forms.Label
    Friend WithEvents cmbReason As System.Windows.Forms.ComboBox
    Friend WithEvents txtComments As System.Windows.Forms.TextBox
    Friend WithEvents lblComments As System.Windows.Forms.Label
    Friend WithEvents lblReason As System.Windows.Forms.Label
    Friend WithEvents cmdSaveAdjustment As System.Windows.Forms.Button
    Friend WithEvents frmToolTip As System.Windows.Forms.ToolTip
    Friend WithEvents frmErrorProvider As System.Windows.Forms.ErrorProvider
    Friend WithEvents lblOutOfTolerance As System.Windows.Forms.Label
    Friend WithEvents gridHistory As Infragistics.Win.UltraWinGrid.UltraGrid
    Public WithEvents fraStoreSelection As System.Windows.Forms.GroupBox
    Friend WithEvents dtpEndDate As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
    Friend WithEvents dtpStartDate As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
    Public WithEvents lblDash As System.Windows.Forms.Label
    Public WithEvents chkCurrentCost As System.Windows.Forms.CheckBox
    Public WithEvents cmbState As System.Windows.Forms.ComboBox
    Public WithEvents optSelectState As System.Windows.Forms.RadioButton
    Public WithEvents optSelectStore As System.Windows.Forms.RadioButton
    Public WithEvents cmbStoreSelection As System.Windows.Forms.ComboBox
    Public WithEvents optSelectALLStores As System.Windows.Forms.RadioButton
    Public WithEvents optSelectZone As System.Windows.Forms.RadioButton
    Public WithEvents cmbZones As System.Windows.Forms.ComboBox
    Public WithEvents optSelectHFM As System.Windows.Forms.RadioButton
    Public WithEvents optSelectWFM As System.Windows.Forms.RadioButton
    Public WithEvents lblDates As System.Windows.Forms.Label
    Public WithEvents txtIdentifier As System.Windows.Forms.TextBox
    Public WithEvents lblIdentifier As System.Windows.Forms.Label
    Public WithEvents cmbSubTeam As System.Windows.Forms.ComboBox
    Public WithEvents cmdItemSearch As System.Windows.Forms.Button
    Public WithEvents txtItemDesc As System.Windows.Forms.TextBox
    Public WithEvents lblSubTeam As System.Windows.Forms.Label
    Public WithEvents lblItemDesc As System.Windows.Forms.Label
End Class
