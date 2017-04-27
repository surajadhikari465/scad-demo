<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmShrinkCorrections
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmShrinkCorrections))
        Dim Appearance2 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridBand1 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1)
        Dim UltraGridColumn1 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Identifier")
        Dim UltraGridColumn2 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Item_Description")
        Dim UltraGridColumn3 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("ItemSubteam")
        Dim UltraGridColumn4 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Category_Name")
        Dim UltraGridColumn5 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Brand_Name")
        Dim UltraGridColumn6 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("DateStamp")
        Dim UltraGridColumn7 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("SubTeam_No")
        Dim UltraGridColumn8 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Qty")
        Dim UltraGridColumn9 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("wType")
        Dim UltraGridColumn10 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("UserName")
        Dim UltraGridColumn11 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("CostedbyWeight")
        Dim UltraGridColumn12 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Cost")
        Dim UltraGridColumn13 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("OriginalDateStamp")
        Dim Appearance1 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim ColScrollRegion1 As Infragistics.Win.UltraWinGrid.ColScrollRegion = New Infragistics.Win.UltraWinGrid.ColScrollRegion(983)
        Dim ColScrollRegion2 As Infragistics.Win.UltraWinGrid.ColScrollRegion = New Infragistics.Win.UltraWinGrid.ColScrollRegion(-7)
        Dim Appearance6 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance7 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance8 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance9 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance10 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance11 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance12 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance21 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance22 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance23 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance24 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Me.searchGroupBox = New System.Windows.Forms.GroupBox()
        Me.lblSpoilageType = New System.Windows.Forms.Label()
        Me.cmbType = New System.Windows.Forms.ComboBox()
        Me.cmdSearch = New System.Windows.Forms.Button()
        Me.mskEndDate = New System.Windows.Forms.DateTimePicker()
        Me.mskStartDate = New System.Windows.Forms.DateTimePicker()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmbStore = New System.Windows.Forms.ComboBox()
        Me.cmbSubTeam = New System.Windows.Forms.ComboBox()
        Me.lblStore = New System.Windows.Forms.Label()
        Me._lblSubTeam_2 = New System.Windows.Forms.Label()
        Me.gridShrink = New Infragistics.Win.UltraWinGrid.UltraGrid()
        Me.cmdExit = New System.Windows.Forms.Button()
        Me.cmdSave = New System.Windows.Forms.Button()
        Me.cmdDelete = New System.Windows.Forms.Button()
        Me.cmdExcelExport = New System.Windows.Forms.Button()
        Me.UltraGridExcelExporter1 = New Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter(Me.components)
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.searchGroupBox.SuspendLayout()
        CType(Me.gridShrink, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'searchGroupBox
        '
        Me.searchGroupBox.Controls.Add(Me.lblSpoilageType)
        Me.searchGroupBox.Controls.Add(Me.cmbType)
        Me.searchGroupBox.Controls.Add(Me.cmdSearch)
        Me.searchGroupBox.Controls.Add(Me.mskEndDate)
        Me.searchGroupBox.Controls.Add(Me.mskStartDate)
        Me.searchGroupBox.Controls.Add(Me.Label1)
        Me.searchGroupBox.Controls.Add(Me.cmbStore)
        Me.searchGroupBox.Controls.Add(Me.cmbSubTeam)
        Me.searchGroupBox.Controls.Add(Me.lblStore)
        Me.searchGroupBox.Controls.Add(Me._lblSubTeam_2)
        Me.searchGroupBox.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.searchGroupBox.Location = New System.Drawing.Point(19, 17)
        Me.searchGroupBox.Name = "searchGroupBox"
        Me.searchGroupBox.Size = New System.Drawing.Size(985, 102)
        Me.searchGroupBox.TabIndex = 0
        Me.searchGroupBox.TabStop = False
        Me.searchGroupBox.Text = "Search Criteria"
        '
        'lblSpoilageType
        '
        Me.lblSpoilageType.BackColor = System.Drawing.Color.Transparent
        Me.lblSpoilageType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSpoilageType.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSpoilageType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSpoilageType.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblSpoilageType.Location = New System.Drawing.Point(344, 27)
        Me.lblSpoilageType.Name = "lblSpoilageType"
        Me.lblSpoilageType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSpoilageType.Size = New System.Drawing.Size(128, 17)
        Me.lblSpoilageType.TabIndex = 17
        Me.lblSpoilageType.Text = "Spoilage/Shrink Type :"
        Me.lblSpoilageType.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'cmbType
        '
        Me.cmbType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbType.BackColor = System.Drawing.SystemColors.Window
        Me.cmbType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbType.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.cmbType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbType.Items.AddRange(New Object() {"ALL", "Foodbank", "Sampling", "Spoilage"})
        Me.cmbType.Location = New System.Drawing.Point(476, 24)
        Me.cmbType.Name = "cmbType"
        Me.cmbType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbType.Size = New System.Drawing.Size(210, 22)
        Me.cmbType.Sorted = True
        Me.cmbType.TabIndex = 16
        '
        'cmdSearch
        '
        Me.cmdSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdSearch.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSearch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSearch.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.cmdSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSearch.Image = CType(resources.GetObject("cmdSearch.Image"), System.Drawing.Image)
        Me.cmdSearch.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.cmdSearch.Location = New System.Drawing.Point(931, 19)
        Me.cmdSearch.Name = "cmdSearch"
        Me.cmdSearch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSearch.Size = New System.Drawing.Size(48, 48)
        Me.cmdSearch.TabIndex = 15
        Me.cmdSearch.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdSearch, "Start search")
        Me.cmdSearch.UseVisualStyleBackColor = False
        '
        'mskEndDate
        '
        Me.mskEndDate.Checked = False
        Me.mskEndDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.mskEndDate.Location = New System.Drawing.Point(596, 65)
        Me.mskEndDate.MinDate = New Date(1977, 1, 1, 0, 0, 0, 0)
        Me.mskEndDate.Name = "mskEndDate"
        Me.mskEndDate.Size = New System.Drawing.Size(89, 22)
        Me.mskEndDate.TabIndex = 14
        '
        'mskStartDate
        '
        Me.mskStartDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.mskStartDate.Location = New System.Drawing.Point(476, 65)
        Me.mskStartDate.Name = "mskStartDate"
        Me.mskStartDate.Size = New System.Drawing.Size(90, 22)
        Me.mskStartDate.TabIndex = 13
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label1.Location = New System.Drawing.Point(407, 66)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(65, 17)
        Me.Label1.TabIndex = 12
        Me.Label1.Text = "Dates : "
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'cmbStore
        '
        Me.cmbStore.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbStore.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbStore.BackColor = System.Drawing.SystemColors.Window
        Me.cmbStore.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbStore.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbStore.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.cmbStore.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbStore.Location = New System.Drawing.Point(92, 24)
        Me.cmbStore.Name = "cmbStore"
        Me.cmbStore.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbStore.Size = New System.Drawing.Size(201, 22)
        Me.cmbStore.Sorted = True
        Me.cmbStore.TabIndex = 9
        '
        'cmbSubTeam
        '
        Me.cmbSubTeam.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbSubTeam.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbSubTeam.BackColor = System.Drawing.SystemColors.Window
        Me.cmbSubTeam.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbSubTeam.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbSubTeam.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.cmbSubTeam.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbSubTeam.Location = New System.Drawing.Point(91, 63)
        Me.cmbSubTeam.Name = "cmbSubTeam"
        Me.cmbSubTeam.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbSubTeam.Size = New System.Drawing.Size(202, 22)
        Me.cmbSubTeam.Sorted = True
        Me.cmbSubTeam.TabIndex = 11
        '
        'lblStore
        '
        Me.lblStore.BackColor = System.Drawing.Color.Transparent
        Me.lblStore.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblStore.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStore.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblStore.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblStore.Location = New System.Drawing.Point(37, 27)
        Me.lblStore.Name = "lblStore"
        Me.lblStore.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblStore.Size = New System.Drawing.Size(49, 17)
        Me.lblStore.TabIndex = 8
        Me.lblStore.Text = "Store :"
        Me.lblStore.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblSubTeam_2
        '
        Me._lblSubTeam_2.BackColor = System.Drawing.Color.Transparent
        Me._lblSubTeam_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblSubTeam_2.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblSubTeam_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblSubTeam_2.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me._lblSubTeam_2.Location = New System.Drawing.Point(16, 66)
        Me._lblSubTeam_2.Name = "_lblSubTeam_2"
        Me._lblSubTeam_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblSubTeam_2.Size = New System.Drawing.Size(70, 17)
        Me._lblSubTeam_2.TabIndex = 10
        Me._lblSubTeam_2.Text = "Sub-Team :"
        Me._lblSubTeam_2.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'gridShrink
        '
        Me.gridShrink.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Appearance2.BackColor = System.Drawing.SystemColors.Window
        Appearance2.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Appearance2.TextHAlignAsString = "Center"
        Me.gridShrink.DisplayLayout.Appearance = Appearance2
        Me.gridShrink.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        UltraGridBand1.ColHeaderLines = 2
        UltraGridColumn1.Header.VisiblePosition = 0
        UltraGridColumn1.Width = 14
        UltraGridColumn2.Header.Caption = "Item Description"
        UltraGridColumn2.Header.VisiblePosition = 1
        UltraGridColumn2.Width = 98
        UltraGridColumn3.Header.Caption = "Item Subteam"
        UltraGridColumn3.Header.VisiblePosition = 2
        UltraGridColumn3.Width = 155
        UltraGridColumn4.Header.Caption = "Category Name"
        UltraGridColumn4.Header.VisiblePosition = 3
        UltraGridColumn4.Width = 80
        UltraGridColumn5.Header.Caption = "Brand Name"
        UltraGridColumn5.Header.VisiblePosition = 4
        UltraGridColumn5.Width = 84
        UltraGridColumn6.Header.Caption = "Date Stamp"
        UltraGridColumn6.Header.VisiblePosition = 5
        UltraGridColumn6.Width = 90
        UltraGridColumn7.Header.Caption = "Shrink SubTeam No"
        UltraGridColumn7.Header.VisiblePosition = 6
        UltraGridColumn7.Width = 108
        UltraGridColumn8.Header.Caption = "Quantity"
        UltraGridColumn8.Header.VisiblePosition = 7
        UltraGridColumn8.Width = 78
        UltraGridColumn9.Header.Caption = "Shrink Type"
        UltraGridColumn9.Header.VisiblePosition = 9
        UltraGridColumn9.Width = 78
        UltraGridColumn10.Header.Caption = "Created By"
        UltraGridColumn10.Header.VisiblePosition = 10
        UltraGridColumn10.Width = 78
        UltraGridColumn11.Header.Caption = "Costed by Weight"
        UltraGridColumn11.Header.VisiblePosition = 11
        UltraGridColumn11.Hidden = True
        UltraGridColumn11.Width = 94
        UltraGridColumn12.Header.VisiblePosition = 8
        UltraGridColumn12.Width = 87
        UltraGridColumn13.Header.Caption = "Original Date Stamp"
        UltraGridColumn13.Header.VisiblePosition = 12
        UltraGridColumn13.Width = 14
        UltraGridBand1.Columns.AddRange(New Object() {UltraGridColumn1, UltraGridColumn2, UltraGridColumn3, UltraGridColumn4, UltraGridColumn5, UltraGridColumn6, UltraGridColumn7, UltraGridColumn8, UltraGridColumn9, UltraGridColumn10, UltraGridColumn11, UltraGridColumn12, UltraGridColumn13})
        Appearance1.TextHAlignAsString = "Center"
        UltraGridBand1.Header.Appearance = Appearance1
        UltraGridBand1.Header.TextOrientation = New Infragistics.Win.TextOrientationInfo(0, Infragistics.Win.TextFlowDirection.Horizontal)
        UltraGridBand1.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed
        Me.gridShrink.DisplayLayout.BandsSerializer.Add(UltraGridBand1)
        Me.gridShrink.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.gridShrink.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Me.gridShrink.DisplayLayout.ColScrollRegions.Add(ColScrollRegion1)
        Me.gridShrink.DisplayLayout.ColScrollRegions.Add(ColScrollRegion2)
        Appearance6.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance6.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance6.BorderColor = System.Drawing.SystemColors.Window
        Me.gridShrink.DisplayLayout.GroupByBox.Appearance = Appearance6
        Appearance7.ForeColor = System.Drawing.SystemColors.GrayText
        Me.gridShrink.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance7
        Me.gridShrink.DisplayLayout.GroupByBox.Hidden = True
        Appearance8.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance8.BackColor2 = System.Drawing.SystemColors.Control
        Appearance8.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance8.ForeColor = System.Drawing.SystemColors.GrayText
        Me.gridShrink.DisplayLayout.GroupByBox.PromptAppearance = Appearance8
        Me.gridShrink.DisplayLayout.MaxColScrollRegions = 1
        Me.gridShrink.DisplayLayout.MaxRowScrollRegions = 1
        Appearance9.BackColor = System.Drawing.SystemColors.Window
        Appearance9.BorderColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
        Appearance9.ForeColor = System.Drawing.SystemColors.ControlText
        Me.gridShrink.DisplayLayout.Override.ActiveCellAppearance = Appearance9
        Appearance10.BackColor = System.Drawing.SystemColors.Highlight
        Appearance10.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.gridShrink.DisplayLayout.Override.ActiveRowAppearance = Appearance10
        Me.gridShrink.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.NotAllowed
        Me.gridShrink.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.None
        Me.gridShrink.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed
        Appearance11.BackColor = System.Drawing.SystemColors.Window
        Me.gridShrink.DisplayLayout.Override.CardAreaAppearance = Appearance11
        Appearance12.BorderColor = System.Drawing.Color.Silver
        Appearance12.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.gridShrink.DisplayLayout.Override.CellAppearance = Appearance12
        Me.gridShrink.DisplayLayout.Override.CellPadding = 0
        Appearance21.BackColor = System.Drawing.SystemColors.Control
        Appearance21.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance21.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance21.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance21.BorderColor = System.Drawing.SystemColors.Window
        Me.gridShrink.DisplayLayout.Override.GroupByRowAppearance = Appearance21
        Appearance22.FontData.BoldAsString = "True"
        Appearance22.TextHAlignAsString = "Left"
        Me.gridShrink.DisplayLayout.Override.HeaderAppearance = Appearance22
        Me.gridShrink.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle
        Me.gridShrink.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance23.BackColor = System.Drawing.SystemColors.Window
        Appearance23.BorderColor = System.Drawing.Color.SeaGreen
        Me.gridShrink.DisplayLayout.Override.RowAppearance = Appearance23
        Me.gridShrink.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[True]
        Me.gridShrink.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.[Single]
        Appearance24.BackColor = System.Drawing.SystemColors.ControlLight
        Me.gridShrink.DisplayLayout.Override.TemplateAddRowAppearance = Appearance24
        Me.gridShrink.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.gridShrink.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        Me.gridShrink.Location = New System.Drawing.Point(19, 134)
        Me.gridShrink.Name = "gridShrink"
        Me.gridShrink.Size = New System.Drawing.Size(985, 388)
        Me.gridShrink.TabIndex = 1
        '
        'cmdExit
        '
        Me.cmdExit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdExit.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Image = CType(resources.GetObject("cmdExit.Image"), System.Drawing.Image)
        Me.cmdExit.Location = New System.Drawing.Point(962, 531)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(42, 42)
        Me.cmdExit.TabIndex = 13
        Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdExit, "Exit")
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'cmdSave
        '
        Me.cmdSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdSave.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSave.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSave.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSave.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSave.Image = CType(resources.GetObject("cmdSave.Image"), System.Drawing.Image)
        Me.cmdSave.Location = New System.Drawing.Point(893, 531)
        Me.cmdSave.Name = "cmdSave"
        Me.cmdSave.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSave.Size = New System.Drawing.Size(42, 42)
        Me.cmdSave.TabIndex = 14
        Me.cmdSave.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdSave, "Save changes")
        Me.cmdSave.UseVisualStyleBackColor = False
        '
        'cmdDelete
        '
        Me.cmdDelete.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDelete.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDelete.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDelete.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDelete.Image = CType(resources.GetObject("cmdDelete.Image"), System.Drawing.Image)
        Me.cmdDelete.Location = New System.Drawing.Point(818, 531)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDelete.Size = New System.Drawing.Size(42, 42)
        Me.cmdDelete.TabIndex = 15
        Me.cmdDelete.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdDelete, "Delete")
        Me.cmdDelete.UseVisualStyleBackColor = False
        '
        'cmdExcelExport
        '
        Me.cmdExcelExport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdExcelExport.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExcelExport.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExcelExport.Enabled = False
        Me.cmdExcelExport.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExcelExport.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExcelExport.Image = CType(resources.GetObject("cmdExcelExport.Image"), System.Drawing.Image)
        Me.cmdExcelExport.Location = New System.Drawing.Point(639, 531)
        Me.cmdExcelExport.Name = "cmdExcelExport"
        Me.cmdExcelExport.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExcelExport.Size = New System.Drawing.Size(42, 42)
        Me.cmdExcelExport.TabIndex = 16
        Me.cmdExcelExport.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdExcelExport, "Export to an Excel file")
        Me.cmdExcelExport.UseVisualStyleBackColor = False
        '
        'frmShrinkCorrections
        '
        Me.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuBar
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1016, 585)
        Me.Controls.Add(Me.cmdExcelExport)
        Me.Controls.Add(Me.cmdDelete)
        Me.Controls.Add(Me.cmdSave)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.gridShrink)
        Me.Controls.Add(Me.searchGroupBox)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MinimizeBox = False
        Me.Name = "frmShrinkCorrections"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Shrink Corrections"
        Me.searchGroupBox.ResumeLayout(False)
        CType(Me.gridShrink, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents searchGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents mskEndDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents mskStartDate As System.Windows.Forms.DateTimePicker
    Public WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents cmbStore As System.Windows.Forms.ComboBox
    Public WithEvents cmbSubTeam As System.Windows.Forms.ComboBox
    Public WithEvents lblStore As System.Windows.Forms.Label
    Public WithEvents _lblSubTeam_2 As System.Windows.Forms.Label
    Public WithEvents cmdSearch As System.Windows.Forms.Button
    Friend WithEvents gridShrink As Infragistics.Win.UltraWinGrid.UltraGrid
    Public WithEvents cmdExit As System.Windows.Forms.Button
    Public WithEvents cmdSave As System.Windows.Forms.Button
    Public WithEvents lblSpoilageType As System.Windows.Forms.Label
    Public WithEvents cmbType As System.Windows.Forms.ComboBox
    Public WithEvents cmdDelete As System.Windows.Forms.Button
    Public WithEvents cmdExcelExport As System.Windows.Forms.Button
    Friend WithEvents UltraGridExcelExporter1 As Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
End Class
