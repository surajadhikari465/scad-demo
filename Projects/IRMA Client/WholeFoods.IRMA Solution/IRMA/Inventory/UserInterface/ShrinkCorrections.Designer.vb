<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmShrinkCorrections
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
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
        Dim UltraGridColumn10 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("UserName", -1, Nothing, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, False)
        Dim UltraGridColumn11 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("CostedbyWeight")
        Dim UltraGridColumn12 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Cost")
        Dim UltraGridColumn13 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("OriginalDateStamp")
        Dim UltraGridColumn14 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("ShrinkSubtype_ID")
        Dim UltraGridColumn15 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("InventoryAdjustment_ID")
        Dim UltraGridColumn16 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("ShrinkSubtype_Desc")
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
        Me.lblDateDash = New System.Windows.Forms.Label()
        Me.lblShrinkSubtype = New System.Windows.Forms.Label()
        Me.cmbShrinkSubtype = New System.Windows.Forms.ComboBox()
        Me.lblSpoilageType = New System.Windows.Forms.Label()
        Me.cmbShrinkType = New System.Windows.Forms.ComboBox()
        Me.cmdSearch = New System.Windows.Forms.Button()
        Me.mskEndDate = New System.Windows.Forms.DateTimePicker()
        Me.mskStartDate = New System.Windows.Forms.DateTimePicker()
        Me.lblDates = New System.Windows.Forms.Label()
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
        Me.searchGroupBox.Controls.Add(Me.lblDateDash)
        Me.searchGroupBox.Controls.Add(Me.lblShrinkSubtype)
        Me.searchGroupBox.Controls.Add(Me.cmbShrinkSubtype)
        Me.searchGroupBox.Controls.Add(Me.lblSpoilageType)
        Me.searchGroupBox.Controls.Add(Me.cmbShrinkType)
        Me.searchGroupBox.Controls.Add(Me.cmdSearch)
        Me.searchGroupBox.Controls.Add(Me.mskEndDate)
        Me.searchGroupBox.Controls.Add(Me.mskStartDate)
        Me.searchGroupBox.Controls.Add(Me.lblDates)
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
        'lblDateDash
        '
        Me.lblDateDash.BackColor = System.Drawing.Color.Transparent
        Me.lblDateDash.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDateDash.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDateDash.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDateDash.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblDateDash.Location = New System.Drawing.Point(803, 49)
        Me.lblDateDash.Name = "lblDateDash"
        Me.lblDateDash.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDateDash.Size = New System.Drawing.Size(10, 17)
        Me.lblDateDash.TabIndex = 20
        Me.lblDateDash.Text = "-"
        Me.lblDateDash.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblShrinkSubtype
        '
        Me.lblShrinkSubtype.BackColor = System.Drawing.Color.Transparent
        Me.lblShrinkSubtype.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblShrinkSubtype.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShrinkSubtype.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblShrinkSubtype.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblShrinkSubtype.Location = New System.Drawing.Point(305, 71)
        Me.lblShrinkSubtype.Name = "lblShrinkSubtype"
        Me.lblShrinkSubtype.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblShrinkSubtype.Size = New System.Drawing.Size(98, 17)
        Me.lblShrinkSubtype.TabIndex = 19
        Me.lblShrinkSubtype.Text = "Shrink Subtype :"
        Me.lblShrinkSubtype.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'cmbShrinkSubtype
        '
        Me.cmbShrinkSubtype.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbShrinkSubtype.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbShrinkSubtype.BackColor = System.Drawing.SystemColors.Window
        Me.cmbShrinkSubtype.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbShrinkSubtype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbShrinkSubtype.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.cmbShrinkSubtype.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbShrinkSubtype.Location = New System.Drawing.Point(404, 68)
        Me.cmbShrinkSubtype.Name = "cmbShrinkSubtype"
        Me.cmbShrinkSubtype.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbShrinkSubtype.Size = New System.Drawing.Size(210, 24)
        Me.cmbShrinkSubtype.Sorted = True
        Me.cmbShrinkSubtype.TabIndex = 18
        '
        'lblSpoilageType
        '
        Me.lblSpoilageType.BackColor = System.Drawing.Color.Transparent
        Me.lblSpoilageType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSpoilageType.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSpoilageType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSpoilageType.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblSpoilageType.Location = New System.Drawing.Point(305, 27)
        Me.lblSpoilageType.Name = "lblSpoilageType"
        Me.lblSpoilageType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSpoilageType.Size = New System.Drawing.Size(95, 17)
        Me.lblSpoilageType.TabIndex = 17
        Me.lblSpoilageType.Text = "Shrink Type :"
        Me.lblSpoilageType.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'cmbShrinkType
        '
        Me.cmbShrinkType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbShrinkType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbShrinkType.BackColor = System.Drawing.SystemColors.Window
        Me.cmbShrinkType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbShrinkType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbShrinkType.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.cmbShrinkType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbShrinkType.Items.AddRange(New Object() {"ALL", "Foodbank", "Sampling", "Spoilage"})
        Me.cmbShrinkType.Location = New System.Drawing.Point(404, 24)
        Me.cmbShrinkType.Name = "cmbShrinkType"
        Me.cmbShrinkType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbShrinkType.Size = New System.Drawing.Size(210, 24)
        Me.cmbShrinkType.Sorted = True
        Me.cmbShrinkType.TabIndex = 16
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
        Me.mskEndDate.Location = New System.Drawing.Point(817, 46)
        Me.mskEndDate.MinDate = New Date(1977, 1, 1, 0, 0, 0, 0)
        Me.mskEndDate.Name = "mskEndDate"
        Me.mskEndDate.Size = New System.Drawing.Size(89, 26)
        Me.mskEndDate.TabIndex = 14
        '
        'mskStartDate
        '
        Me.mskStartDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.mskStartDate.Location = New System.Drawing.Point(709, 46)
        Me.mskStartDate.Name = "mskStartDate"
        Me.mskStartDate.Size = New System.Drawing.Size(90, 26)
        Me.mskStartDate.TabIndex = 13
        '
        'lblDates
        '
        Me.lblDates.BackColor = System.Drawing.Color.Transparent
        Me.lblDates.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDates.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDates.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDates.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblDates.Location = New System.Drawing.Point(638, 49)
        Me.lblDates.Name = "lblDates"
        Me.lblDates.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDates.Size = New System.Drawing.Size(65, 17)
        Me.lblDates.TabIndex = 12
        Me.lblDates.Text = "Dates : "
        Me.lblDates.TextAlign = System.Drawing.ContentAlignment.TopRight
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
        Me.cmbStore.Size = New System.Drawing.Size(201, 24)
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
        Me.cmbSubTeam.Location = New System.Drawing.Point(92, 68)
        Me.cmbSubTeam.Name = "cmbSubTeam"
        Me.cmbSubTeam.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbSubTeam.Size = New System.Drawing.Size(201, 24)
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
        Me._lblSubTeam_2.Location = New System.Drawing.Point(16, 71)
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
        UltraGridColumn1.Width = 68
        UltraGridColumn2.Header.Caption = "Item" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Description"
        UltraGridColumn2.Header.VisiblePosition = 1
        UltraGridColumn2.Width = 152
        UltraGridColumn3.Header.Caption = "Item" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Subteam"
        UltraGridColumn3.Header.VisiblePosition = 2
        UltraGridColumn3.Width = 62
        UltraGridColumn4.Header.Caption = "Category" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Name"
        UltraGridColumn4.Header.VisiblePosition = 3
        UltraGridColumn4.Width = 82
        UltraGridColumn5.Header.Caption = "Brand" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Name"
        UltraGridColumn5.Header.VisiblePosition = 4
        UltraGridColumn5.Width = 82
        UltraGridColumn6.Header.Caption = "Date" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Stamp"
        UltraGridColumn6.Header.VisiblePosition = 5
        UltraGridColumn6.Width = 124
        UltraGridColumn7.Header.Caption = "Shrink" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "SubTeam"
        UltraGridColumn7.Header.VisiblePosition = 6
        UltraGridColumn7.Width = 62
        UltraGridColumn8.Header.Caption = "Qty."
        UltraGridColumn8.Header.VisiblePosition = 7
        UltraGridColumn8.Width = 40
        UltraGridColumn9.Header.Caption = "Shrink" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Type"
        UltraGridColumn9.Header.VisiblePosition = 9
        UltraGridColumn9.Width = 40
        UltraGridColumn10.Header.Caption = "Created" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "By"
        UltraGridColumn10.Header.VisiblePosition = 11
        UltraGridColumn10.Width = 104
        UltraGridColumn11.Header.Caption = "Costed by Weight"
        UltraGridColumn11.Header.VisiblePosition = 13
        UltraGridColumn11.Hidden = True
        UltraGridColumn11.Width = 40
        UltraGridColumn12.Header.VisiblePosition = 8
        UltraGridColumn12.Width = 82
        UltraGridColumn13.Header.Caption = "Original" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Date Stamp"
        UltraGridColumn13.Header.VisiblePosition = 12
        UltraGridColumn13.Hidden = True
        UltraGridColumn13.Width = 40
        UltraGridColumn14.Header.Caption = "Shrink" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Subtype"
        UltraGridColumn14.Header.VisiblePosition = 10
        UltraGridColumn14.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownValidate
        UltraGridColumn14.Width = 66
        UltraGridColumn15.Header.VisiblePosition = 14
        UltraGridColumn15.Hidden = True
        UltraGridColumn15.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList
        UltraGridColumn15.Width = 14
        UltraGridColumn16.Header.VisiblePosition = 15
        UltraGridColumn16.Hidden = True
        UltraGridColumn16.Width = 14
        UltraGridBand1.Columns.AddRange(New Object() {UltraGridColumn1, UltraGridColumn2, UltraGridColumn3, UltraGridColumn4, UltraGridColumn5, UltraGridColumn6, UltraGridColumn7, UltraGridColumn8, UltraGridColumn9, UltraGridColumn10, UltraGridColumn11, UltraGridColumn12, UltraGridColumn13, UltraGridColumn14, UltraGridColumn15, UltraGridColumn16})
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
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 19.0!)
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
    Public WithEvents lblDates As System.Windows.Forms.Label
    Public WithEvents cmbStore As System.Windows.Forms.ComboBox
    Public WithEvents cmbSubTeam As System.Windows.Forms.ComboBox
    Public WithEvents lblStore As System.Windows.Forms.Label
    Public WithEvents _lblSubTeam_2 As System.Windows.Forms.Label
    Public WithEvents cmdSearch As System.Windows.Forms.Button
    Friend WithEvents gridShrink As Infragistics.Win.UltraWinGrid.UltraGrid
    Public WithEvents cmdExit As System.Windows.Forms.Button
    Public WithEvents cmdSave As System.Windows.Forms.Button
    Public WithEvents lblSpoilageType As System.Windows.Forms.Label
    Public WithEvents cmbShrinkType As System.Windows.Forms.ComboBox
    Public WithEvents cmdDelete As System.Windows.Forms.Button
    Public WithEvents cmdExcelExport As System.Windows.Forms.Button
    Friend WithEvents UltraGridExcelExporter1 As Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Public WithEvents lblShrinkSubtype As Label
    Public WithEvents cmbShrinkSubtype As ComboBox
    Public WithEvents lblDateDash As Label
End Class
