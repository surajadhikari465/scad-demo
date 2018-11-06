<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmAvgCostMarginReports
#Region "Windows Form Designer generated code "
	<System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
        MyBase.New()
        Me.IsInitializing = True
		'This call is required by the Windows Form Designer.
        InitializeComponent()
        Me.IsInitializing = False

	End Sub
	'Form overrides dispose to clean up the component list.
	<System.Diagnostics.DebuggerNonUserCode()> Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
		If Disposing Then
			If Not components Is Nothing Then
				components.Dispose()
			End If
		End If
		MyBase.Dispose(Disposing)
	End Sub
	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer
	Public ToolTip1 As System.Windows.Forms.ToolTip
	Public WithEvents _optSelection_6 As System.Windows.Forms.RadioButton
	Public WithEvents _optSelection_5 As System.Windows.Forms.RadioButton
	Public WithEvents cmbStates As System.Windows.Forms.ComboBox
	Public WithEvents _optSelection_4 As System.Windows.Forms.RadioButton
	Public WithEvents _optSelection_3 As System.Windows.Forms.RadioButton
	Public WithEvents cmbZones As System.Windows.Forms.ComboBox
	Public WithEvents _optSelection_2 As System.Windows.Forms.RadioButton
	Public WithEvents _optSelection_1 As System.Windows.Forms.RadioButton
	Public WithEvents _optSelection_0 As System.Windows.Forms.RadioButton
    Public WithEvents fraStores As System.Windows.Forms.GroupBox
	Public WithEvents cmbVendor As System.Windows.Forms.ComboBox
	Public WithEvents cmbSubTeam As System.Windows.Forms.ComboBox
    Public WithEvents _lblLabel_1 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_2 As System.Windows.Forms.Label
    Public WithEvents lblDates As System.Windows.Forms.Label
	Public WithEvents Frame1 As System.Windows.Forms.GroupBox
	Public WithEvents _optLevel_0 As System.Windows.Forms.RadioButton
	Public WithEvents _optLevel_1 As System.Windows.Forms.RadioButton
	Public WithEvents fraLevel As System.Windows.Forms.GroupBox
	Public WithEvents _optReport_0 As System.Windows.Forms.RadioButton
	Public WithEvents _optReport_1 As System.Windows.Forms.RadioButton
	Public WithEvents fraReport As System.Windows.Forms.GroupBox
	Public WithEvents cmdExit As System.Windows.Forms.Button
	Public WithEvents cmdReport As System.Windows.Forms.Button
    Public WithEvents lblLabel As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	Public WithEvents optLevel As Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray
	Public WithEvents optReport As Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray
	Public WithEvents optSelection As Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAvgCostMarginReports))
        Dim Appearance1 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim UltraGridBand1 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1)
        Dim UltraGridColumn1 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Vendor_ID")
        Dim UltraGridColumn2 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyName")
        Dim Appearance2 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim UltraGridColumn3 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Zone_ID")
        Dim UltraGridColumn4 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("State")
        Dim UltraGridColumn5 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("WFM_Store")
        Dim UltraGridColumn6 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Mega_Store")
        Dim UltraGridColumn7 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("CustomerType")
        Dim Appearance3 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance4 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance5 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance6 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance7 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance8 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance9 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance10 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance11 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance12 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance13 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance14 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance15 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me._optSelection_6 = New System.Windows.Forms.RadioButton
        Me._optSelection_5 = New System.Windows.Forms.RadioButton
        Me._optSelection_4 = New System.Windows.Forms.RadioButton
        Me._optSelection_3 = New System.Windows.Forms.RadioButton
        Me._optSelection_2 = New System.Windows.Forms.RadioButton
        Me._optSelection_1 = New System.Windows.Forms.RadioButton
        Me._optSelection_0 = New System.Windows.Forms.RadioButton
        Me.cmdExit = New System.Windows.Forms.Button
        Me.cmdReport = New System.Windows.Forms.Button
        Me.dtpEndDate = New System.Windows.Forms.DateTimePicker
        Me.dtpStartDate = New System.Windows.Forms.DateTimePicker
        Me.fraStores = New System.Windows.Forms.GroupBox
        Me.ugrdStoreList = New Infragistics.Win.UltraWinGrid.UltraGrid
        Me.cmbStates = New System.Windows.Forms.ComboBox
        Me.cmbZones = New System.Windows.Forms.ComboBox
        Me.Frame1 = New System.Windows.Forms.GroupBox
        Me.cmbVendor = New System.Windows.Forms.ComboBox
        Me.cmbSubTeam = New System.Windows.Forms.ComboBox
        Me._lblLabel_1 = New System.Windows.Forms.Label
        Me._lblLabel_2 = New System.Windows.Forms.Label
        Me.lblDates = New System.Windows.Forms.Label
        Me.lblDash = New System.Windows.Forms.Label
        Me.fraLevel = New System.Windows.Forms.GroupBox
        Me._optLevel_0 = New System.Windows.Forms.RadioButton
        Me._optLevel_1 = New System.Windows.Forms.RadioButton
        Me.fraReport = New System.Windows.Forms.GroupBox
        Me._optReport_0 = New System.Windows.Forms.RadioButton
        Me._optReport_1 = New System.Windows.Forms.RadioButton
        Me.lblLabel = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(Me.components)
        Me.optLevel = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(Me.components)
        Me.optReport = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(Me.components)
        Me.optSelection = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(Me.components)
        Me.fraStores.SuspendLayout()
        CType(Me.ugrdStoreList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Frame1.SuspendLayout()
        Me.fraLevel.SuspendLayout()
        Me.fraReport.SuspendLayout()
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.optLevel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.optReport, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.optSelection, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        '_optSelection_6
        '
        Me._optSelection_6.BackColor = System.Drawing.SystemColors.Control
        Me._optSelection_6.Cursor = System.Windows.Forms.Cursors.Default
        Me._optSelection_6.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optSelection_6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optSelection.SetIndex(Me._optSelection_6, CType(6, Short))
        Me._optSelection_6.Location = New System.Drawing.Point(92, 43)
        Me._optSelection_6.Name = "_optSelection_6"
        Me._optSelection_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optSelection_6.Size = New System.Drawing.Size(156, 17)
        Me._optSelection_6.TabIndex = 12
        Me._optSelection_6.TabStop = True
        Me._optSelection_6.Text = "All Region - Retail Only"
        Me.ToolTip1.SetToolTip(Me._optSelection_6, "Select all retail stores within the region.")
        Me._optSelection_6.UseVisualStyleBackColor = False
        '
        '_optSelection_5
        '
        Me._optSelection_5.BackColor = System.Drawing.SystemColors.Control
        Me._optSelection_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._optSelection_5.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optSelection_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optSelection.SetIndex(Me._optSelection_5, CType(5, Short))
        Me._optSelection_5.Location = New System.Drawing.Point(92, 21)
        Me._optSelection_5.Name = "_optSelection_5"
        Me._optSelection_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optSelection_5.Size = New System.Drawing.Size(81, 17)
        Me._optSelection_5.TabIndex = 9
        Me._optSelection_5.TabStop = True
        Me._optSelection_5.Text = "All Region"
        Me.ToolTip1.SetToolTip(Me._optSelection_5, "Select all stores in the region.")
        Me._optSelection_5.UseVisualStyleBackColor = False
        '
        '_optSelection_4
        '
        Me._optSelection_4.BackColor = System.Drawing.SystemColors.Control
        Me._optSelection_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._optSelection_4.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optSelection_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optSelection.SetIndex(Me._optSelection_4, CType(4, Short))
        Me._optSelection_4.Location = New System.Drawing.Point(182, 21)
        Me._optSelection_4.Name = "_optSelection_4"
        Me._optSelection_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optSelection_4.Size = New System.Drawing.Size(81, 17)
        Me._optSelection_4.TabIndex = 10
        Me._optSelection_4.TabStop = True
        Me._optSelection_4.Text = "All WFM"
        Me.ToolTip1.SetToolTip(Me._optSelection_4, "Select all WFM stores within the region.")
        Me._optSelection_4.UseVisualStyleBackColor = False
        '
        '_optSelection_3
        '
        Me._optSelection_3.BackColor = System.Drawing.SystemColors.Control
        Me._optSelection_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._optSelection_3.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optSelection_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optSelection.SetIndex(Me._optSelection_3, CType(3, Short))
        Me._optSelection_3.Location = New System.Drawing.Point(15, 93)
        Me._optSelection_3.Name = "_optSelection_3"
        Me._optSelection_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optSelection_3.Size = New System.Drawing.Size(70, 17)
        Me._optSelection_3.TabIndex = 15
        Me._optSelection_3.TabStop = True
        Me._optSelection_3.Text = "By State"
        Me.ToolTip1.SetToolTip(Me._optSelection_3, "Select all stores from a states within the region.")
        Me._optSelection_3.UseVisualStyleBackColor = False
        '
        '_optSelection_2
        '
        Me._optSelection_2.BackColor = System.Drawing.SystemColors.Control
        Me._optSelection_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._optSelection_2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optSelection_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optSelection.SetIndex(Me._optSelection_2, CType(2, Short))
        Me._optSelection_2.Location = New System.Drawing.Point(15, 68)
        Me._optSelection_2.Name = "_optSelection_2"
        Me._optSelection_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optSelection_2.Size = New System.Drawing.Size(71, 17)
        Me._optSelection_2.TabIndex = 13
        Me._optSelection_2.TabStop = True
        Me._optSelection_2.Text = "By Zone"
        Me.ToolTip1.SetToolTip(Me._optSelection_2, "Select all stores from a zone within the region.")
        Me._optSelection_2.UseVisualStyleBackColor = False
        '
        '_optSelection_1
        '
        Me._optSelection_1.BackColor = System.Drawing.SystemColors.Control
        Me._optSelection_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._optSelection_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optSelection_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optSelection.SetIndex(Me._optSelection_1, CType(1, Short))
        Me._optSelection_1.Location = New System.Drawing.Point(15, 44)
        Me._optSelection_1.Name = "_optSelection_1"
        Me._optSelection_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optSelection_1.Size = New System.Drawing.Size(78, 17)
        Me._optSelection_1.TabIndex = 11
        Me._optSelection_1.TabStop = True
        Me._optSelection_1.Text = "All Stores"
        Me.ToolTip1.SetToolTip(Me._optSelection_1, "Select all stores in region.")
        Me._optSelection_1.UseVisualStyleBackColor = False
        '
        '_optSelection_0
        '
        Me._optSelection_0.BackColor = System.Drawing.SystemColors.Control
        Me._optSelection_0.Checked = True
        Me._optSelection_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._optSelection_0.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optSelection_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optSelection.SetIndex(Me._optSelection_0, CType(0, Short))
        Me._optSelection_0.Location = New System.Drawing.Point(15, 21)
        Me._optSelection_0.Name = "_optSelection_0"
        Me._optSelection_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optSelection_0.Size = New System.Drawing.Size(66, 17)
        Me._optSelection_0.TabIndex = 8
        Me._optSelection_0.TabStop = True
        Me._optSelection_0.Text = "Manual"
        Me.ToolTip1.SetToolTip(Me._optSelection_0, "Manually select from all store in region.")
        Me._optSelection_0.UseVisualStyleBackColor = False
        '
        'cmdExit
        '
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdExit.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Image = CType(resources.GetObject("cmdExit.Image"), System.Drawing.Image)
        Me.cmdExit.Location = New System.Drawing.Point(540, 297)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(41, 41)
        Me.cmdExit.TabIndex = 20
        Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdExit, "Exit")
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'cmdReport
        '
        Me.cmdReport.BackColor = System.Drawing.SystemColors.Control
        Me.cmdReport.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdReport.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdReport.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdReport.Image = CType(resources.GetObject("cmdReport.Image"), System.Drawing.Image)
        Me.cmdReport.Location = New System.Drawing.Point(489, 297)
        Me.cmdReport.Name = "cmdReport"
        Me.cmdReport.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdReport.Size = New System.Drawing.Size(41, 41)
        Me.cmdReport.TabIndex = 19
        Me.cmdReport.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdReport, "Print")
        Me.cmdReport.UseVisualStyleBackColor = False
        '
        'dtpEndDate
        '
        Me.dtpEndDate.CustomFormat = "M/d/yyyy"
        Me.dtpEndDate.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpEndDate.Location = New System.Drawing.Point(204, 115)
        Me.dtpEndDate.Name = "dtpEndDate"
        Me.dtpEndDate.Size = New System.Drawing.Size(80, 20)
        Me.dtpEndDate.TabIndex = 41
        Me.ToolTip1.SetToolTip(Me.dtpEndDate, "End Date")
        Me.dtpEndDate.Value = New Date(2006, 12, 27, 0, 0, 0, 0)
        '
        'dtpStartDate
        '
        Me.dtpStartDate.CustomFormat = "M/d/yyyy"
        Me.dtpStartDate.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpStartDate.Location = New System.Drawing.Point(107, 115)
        Me.dtpStartDate.Name = "dtpStartDate"
        Me.dtpStartDate.Size = New System.Drawing.Size(80, 20)
        Me.dtpStartDate.TabIndex = 40
        Me.ToolTip1.SetToolTip(Me.dtpStartDate, "Start Date")
        Me.dtpStartDate.Value = New Date(2006, 6, 27, 0, 0, 0, 0)
        '
        'fraStores
        '
        Me.fraStores.BackColor = System.Drawing.SystemColors.Control
        Me.fraStores.Controls.Add(Me.ugrdStoreList)
        Me.fraStores.Controls.Add(Me._optSelection_6)
        Me.fraStores.Controls.Add(Me._optSelection_5)
        Me.fraStores.Controls.Add(Me.cmbStates)
        Me.fraStores.Controls.Add(Me._optSelection_4)
        Me.fraStores.Controls.Add(Me._optSelection_3)
        Me.fraStores.Controls.Add(Me.cmbZones)
        Me.fraStores.Controls.Add(Me._optSelection_2)
        Me.fraStores.Controls.Add(Me._optSelection_1)
        Me.fraStores.Controls.Add(Me._optSelection_0)
        Me.fraStores.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraStores.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraStores.Location = New System.Drawing.Point(315, 3)
        Me.fraStores.Name = "fraStores"
        Me.fraStores.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraStores.Size = New System.Drawing.Size(264, 288)
        Me.fraStores.TabIndex = 28
        Me.fraStores.TabStop = False
        Me.fraStores.Text = "Store Selection"
        '
        'ugrdStoreList
        '
        Appearance1.BackColor = System.Drawing.SystemColors.ControlLight
        Appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.ugrdStoreList.DisplayLayout.Appearance = Appearance1
        Me.ugrdStoreList.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        UltraGridColumn1.Header.VisiblePosition = 0
        UltraGridColumn1.Hidden = True
        UltraGridColumn2.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn2.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        Appearance2.TextHAlignAsString = "Center"
        UltraGridColumn2.Header.Appearance = Appearance2
        UltraGridColumn2.Header.Caption = "Purchasing Store Selection"
        UltraGridColumn2.Header.VisiblePosition = 1
        UltraGridColumn3.Header.VisiblePosition = 2
        UltraGridColumn3.Hidden = True
        UltraGridColumn4.Header.VisiblePosition = 3
        UltraGridColumn4.Hidden = True
        UltraGridColumn5.Header.VisiblePosition = 4
        UltraGridColumn5.Hidden = True
        UltraGridColumn6.Header.VisiblePosition = 5
        UltraGridColumn6.Hidden = True
        UltraGridColumn7.Header.VisiblePosition = 6
        UltraGridColumn7.Hidden = True
        UltraGridBand1.Columns.AddRange(New Object() {UltraGridColumn1, UltraGridColumn2, UltraGridColumn3, UltraGridColumn4, UltraGridColumn5, UltraGridColumn6, UltraGridColumn7})
        UltraGridBand1.RowLayoutStyle = Infragistics.Win.UltraWinGrid.RowLayoutStyle.ColumnLayout
        Me.ugrdStoreList.DisplayLayout.BandsSerializer.Add(UltraGridBand1)
        Me.ugrdStoreList.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Appearance3.FontData.BoldAsString = "True"
        Me.ugrdStoreList.DisplayLayout.CaptionAppearance = Appearance3
        Me.ugrdStoreList.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Appearance4.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance4.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance4.BorderColor = System.Drawing.SystemColors.Window
        Me.ugrdStoreList.DisplayLayout.GroupByBox.Appearance = Appearance4
        Appearance5.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugrdStoreList.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance5
        Me.ugrdStoreList.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.ugrdStoreList.DisplayLayout.GroupByBox.Hidden = True
        Appearance6.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance6.BackColor2 = System.Drawing.SystemColors.Control
        Appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance6.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugrdStoreList.DisplayLayout.GroupByBox.PromptAppearance = Appearance6
        Me.ugrdStoreList.DisplayLayout.MaxColScrollRegions = 1
        Me.ugrdStoreList.DisplayLayout.MaxRowScrollRegions = 1
        Appearance7.BackColor = System.Drawing.SystemColors.Window
        Appearance7.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ugrdStoreList.DisplayLayout.Override.ActiveCellAppearance = Appearance7
        Me.ugrdStoreList.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.ugrdStoreList.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance8.BackColor = System.Drawing.SystemColors.Window
        Me.ugrdStoreList.DisplayLayout.Override.CardAreaAppearance = Appearance8
        Appearance9.BorderColor = System.Drawing.Color.Silver
        Appearance9.FontData.BoldAsString = "True"
        Appearance9.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.ugrdStoreList.DisplayLayout.Override.CellAppearance = Appearance9
        Me.ugrdStoreList.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.ugrdStoreList.DisplayLayout.Override.CellPadding = 0
        Appearance10.FontData.BoldAsString = "True"
        Me.ugrdStoreList.DisplayLayout.Override.FixedHeaderAppearance = Appearance10
        Appearance11.BackColor = System.Drawing.SystemColors.Control
        Appearance11.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance11.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance11.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance11.BorderColor = System.Drawing.SystemColors.Window
        Me.ugrdStoreList.DisplayLayout.Override.GroupByRowAppearance = Appearance11
        Appearance12.FontData.BoldAsString = "True"
        Appearance12.TextHAlignAsString = "Left"
        Me.ugrdStoreList.DisplayLayout.Override.HeaderAppearance = Appearance12
        Me.ugrdStoreList.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle
        Me.ugrdStoreList.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance13.BackColor = System.Drawing.SystemColors.Control
        Me.ugrdStoreList.DisplayLayout.Override.RowAlternateAppearance = Appearance13
        Appearance14.BackColor = System.Drawing.SystemColors.Window
        Appearance14.BorderColor = System.Drawing.Color.Silver
        Me.ugrdStoreList.DisplayLayout.Override.RowAppearance = Appearance14
        Me.ugrdStoreList.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[True]
        Me.ugrdStoreList.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed
        Me.ugrdStoreList.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.ugrdStoreList.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.ugrdStoreList.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Extended
        Appearance15.BackColor = System.Drawing.SystemColors.ControlLight
        Me.ugrdStoreList.DisplayLayout.Override.TemplateAddRowAppearance = Appearance15
        Me.ugrdStoreList.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.ugrdStoreList.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.ugrdStoreList.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand
        Me.ugrdStoreList.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        Me.ugrdStoreList.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ugrdStoreList.Location = New System.Drawing.Point(12, 125)
        Me.ugrdStoreList.Name = "ugrdStoreList"
        Me.ugrdStoreList.Size = New System.Drawing.Size(239, 157)
        Me.ugrdStoreList.TabIndex = 18
        '
        'cmbStates
        '
        Me.cmbStates.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbStates.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbStates.BackColor = System.Drawing.SystemColors.Window
        Me.cmbStates.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbStates.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbStates.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbStates.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbStates.Location = New System.Drawing.Point(90, 91)
        Me.cmbStates.Name = "cmbStates"
        Me.cmbStates.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbStates.Size = New System.Drawing.Size(161, 22)
        Me.cmbStates.Sorted = True
        Me.cmbStates.TabIndex = 16
        '
        'cmbZones
        '
        Me.cmbZones.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbZones.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbZones.BackColor = System.Drawing.SystemColors.Window
        Me.cmbZones.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbZones.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbZones.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbZones.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbZones.Location = New System.Drawing.Point(90, 64)
        Me.cmbZones.Name = "cmbZones"
        Me.cmbZones.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbZones.Size = New System.Drawing.Size(161, 22)
        Me.cmbZones.Sorted = True
        Me.cmbZones.TabIndex = 14
        '
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me.cmbVendor)
        Me.Frame1.Controls.Add(Me.cmbSubTeam)
        Me.Frame1.Controls.Add(Me._lblLabel_1)
        Me.Frame1.Controls.Add(Me._lblLabel_2)
        Me.Frame1.Controls.Add(Me.lblDates)
        Me.Frame1.Controls.Add(Me.lblDash)
        Me.Frame1.Controls.Add(Me.dtpEndDate)
        Me.Frame1.Controls.Add(Me.dtpStartDate)
        Me.Frame1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Location = New System.Drawing.Point(7, 129)
        Me.Frame1.Name = "Frame1"
        Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame1.Size = New System.Drawing.Size(298, 162)
        Me.Frame1.TabIndex = 23
        Me.Frame1.TabStop = False
        '
        'cmbVendor
        '
        Me.cmbVendor.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbVendor.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbVendor.BackColor = System.Drawing.SystemColors.Window
        Me.cmbVendor.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbVendor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbVendor.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbVendor.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbVendor.Location = New System.Drawing.Point(107, 23)
        Me.cmbVendor.Name = "cmbVendor"
        Me.cmbVendor.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbVendor.Size = New System.Drawing.Size(177, 22)
        Me.cmbVendor.Sorted = True
        Me.cmbVendor.TabIndex = 4
        '
        'cmbSubTeam
        '
        Me.cmbSubTeam.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbSubTeam.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbSubTeam.BackColor = System.Drawing.SystemColors.Window
        Me.cmbSubTeam.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbSubTeam.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbSubTeam.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbSubTeam.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbSubTeam.Location = New System.Drawing.Point(107, 67)
        Me.cmbSubTeam.Name = "cmbSubTeam"
        Me.cmbSubTeam.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbSubTeam.Size = New System.Drawing.Size(177, 22)
        Me.cmbSubTeam.Sorted = True
        Me.cmbSubTeam.TabIndex = 5
        '
        '_lblLabel_1
        '
        Me._lblLabel_1.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_1, CType(1, Short))
        Me._lblLabel_1.Location = New System.Drawing.Point(13, 26)
        Me._lblLabel_1.Name = "_lblLabel_1"
        Me._lblLabel_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_1.Size = New System.Drawing.Size(89, 17)
        Me._lblLabel_1.TabIndex = 27
        Me._lblLabel_1.Text = "Vendor :"
        Me._lblLabel_1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_2
        '
        Me._lblLabel_2.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_2, CType(2, Short))
        Me._lblLabel_2.Location = New System.Drawing.Point(9, 70)
        Me._lblLabel_2.Name = "_lblLabel_2"
        Me._lblLabel_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_2.Size = New System.Drawing.Size(93, 34)
        Me._lblLabel_2.TabIndex = 26
        Me._lblLabel_2.Text = "Transfer Sub-Team :"
        Me._lblLabel_2.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblDates
        '
        Me.lblDates.BackColor = System.Drawing.Color.Transparent
        Me.lblDates.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDates.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDates.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDates.Location = New System.Drawing.Point(5, 117)
        Me.lblDates.Name = "lblDates"
        Me.lblDates.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDates.Size = New System.Drawing.Size(97, 17)
        Me.lblDates.TabIndex = 24
        Me.lblDates.Text = "Receiving Date :"
        Me.lblDates.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblDash
        '
        Me.lblDash.BackColor = System.Drawing.Color.Transparent
        Me.lblDash.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDash.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold)
        Me.lblDash.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDash.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblDash.Location = New System.Drawing.Point(187, 114)
        Me.lblDash.Name = "lblDash"
        Me.lblDash.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDash.Size = New System.Drawing.Size(17, 17)
        Me.lblDash.TabIndex = 39
        Me.lblDash.Text = "-"
        Me.lblDash.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'fraLevel
        '
        Me.fraLevel.BackColor = System.Drawing.SystemColors.Control
        Me.fraLevel.Controls.Add(Me._optLevel_0)
        Me.fraLevel.Controls.Add(Me._optLevel_1)
        Me.fraLevel.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraLevel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraLevel.Location = New System.Drawing.Point(7, 67)
        Me.fraLevel.Name = "fraLevel"
        Me.fraLevel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraLevel.Size = New System.Drawing.Size(298, 56)
        Me.fraLevel.TabIndex = 22
        Me.fraLevel.TabStop = False
        Me.fraLevel.Text = "Level"
        '
        '_optLevel_0
        '
        Me._optLevel_0.BackColor = System.Drawing.SystemColors.Control
        Me._optLevel_0.Checked = True
        Me._optLevel_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._optLevel_0.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optLevel_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optLevel.SetIndex(Me._optLevel_0, CType(0, Short))
        Me._optLevel_0.Location = New System.Drawing.Point(98, 22)
        Me._optLevel_0.Name = "_optLevel_0"
        Me._optLevel_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optLevel_0.Size = New System.Drawing.Size(65, 17)
        Me._optLevel_0.TabIndex = 2
        Me._optLevel_0.TabStop = True
        Me._optLevel_0.Text = "Detail"
        Me._optLevel_0.UseVisualStyleBackColor = False
        '
        '_optLevel_1
        '
        Me._optLevel_1.BackColor = System.Drawing.SystemColors.Control
        Me._optLevel_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._optLevel_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optLevel_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optLevel.SetIndex(Me._optLevel_1, CType(1, Short))
        Me._optLevel_1.Location = New System.Drawing.Point(180, 22)
        Me._optLevel_1.Name = "_optLevel_1"
        Me._optLevel_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optLevel_1.Size = New System.Drawing.Size(81, 17)
        Me._optLevel_1.TabIndex = 3
        Me._optLevel_1.TabStop = True
        Me._optLevel_1.Text = "Summary"
        Me._optLevel_1.UseVisualStyleBackColor = False
        '
        'fraReport
        '
        Me.fraReport.BackColor = System.Drawing.SystemColors.Control
        Me.fraReport.Controls.Add(Me._optReport_0)
        Me.fraReport.Controls.Add(Me._optReport_1)
        Me.fraReport.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraReport.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraReport.Location = New System.Drawing.Point(7, 6)
        Me.fraReport.Name = "fraReport"
        Me.fraReport.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraReport.Size = New System.Drawing.Size(298, 56)
        Me.fraReport.TabIndex = 21
        Me.fraReport.TabStop = False
        Me.fraReport.Text = "Report"
        '
        '_optReport_0
        '
        Me._optReport_0.BackColor = System.Drawing.SystemColors.Control
        Me._optReport_0.Checked = True
        Me._optReport_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._optReport_0.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optReport_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optReport.SetIndex(Me._optReport_0, CType(0, Short))
        Me._optReport_0.Location = New System.Drawing.Point(98, 21)
        Me._optReport_0.Name = "_optReport_0"
        Me._optReport_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optReport_0.Size = New System.Drawing.Size(49, 17)
        Me._optReport_0.TabIndex = 0
        Me._optReport_0.TabStop = True
        Me._optReport_0.Text = "PO"
        Me._optReport_0.UseVisualStyleBackColor = False
        '
        '_optReport_1
        '
        Me._optReport_1.BackColor = System.Drawing.SystemColors.Control
        Me._optReport_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._optReport_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optReport_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optReport.SetIndex(Me._optReport_1, CType(1, Short))
        Me._optReport_1.Location = New System.Drawing.Point(179, 21)
        Me._optReport_1.Name = "_optReport_1"
        Me._optReport_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optReport_1.Size = New System.Drawing.Size(73, 17)
        Me._optReport_1.TabIndex = 1
        Me._optReport_1.TabStop = True
        Me._optReport_1.Text = "Item"
        Me._optReport_1.UseVisualStyleBackColor = False
        '
        'optReport
        '
        '
        'optSelection
        '
        '
        'frmAvgCostMarginReports
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(591, 346)
        Me.Controls.Add(Me.fraStores)
        Me.Controls.Add(Me.Frame1)
        Me.Controls.Add(Me.fraLevel)
        Me.Controls.Add(Me.fraReport)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.cmdReport)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Location = New System.Drawing.Point(25, 44)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmAvgCostMarginReports"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Avg Cost Distribution Margin Reports"
        Me.fraStores.ResumeLayout(False)
        CType(Me.ugrdStoreList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Frame1.ResumeLayout(False)
        Me.fraLevel.ResumeLayout(False)
        Me.fraReport.ResumeLayout(False)
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.optLevel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.optReport, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.optSelection, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ugrdStoreList As Infragistics.Win.UltraWinGrid.UltraGrid
    Public WithEvents lblDash As System.Windows.Forms.Label
    Friend WithEvents dtpEndDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtpStartDate As System.Windows.Forms.DateTimePicker
#End Region 
End Class