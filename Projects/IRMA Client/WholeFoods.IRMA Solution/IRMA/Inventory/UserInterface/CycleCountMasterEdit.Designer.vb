<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmCycleCountMasterEdit
#Region "Windows Form Designer generated code "
	<System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
        MyBase.New()

        IsInitializing = True

		'This call is required by the Windows Form Designer.
        InitializeComponent()

        IsInitializing = False

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
	Public WithEvents cmdMasterReport As System.Windows.Forms.Button
	Public WithEvents cmdCloseMaster As System.Windows.Forms.Button
	Public WithEvents lblCountsLocked As System.Windows.Forms.Label
	Public WithEvents lblMasterStatus As System.Windows.Forms.Label
	Public WithEvents Frame1 As System.Windows.Forms.GroupBox
	Public WithEvents chkEndOfPeriod As System.Windows.Forms.CheckBox
	Public WithEvents cmdApply As System.Windows.Forms.Button
	Public WithEvents cmdExit As System.Windows.Forms.Button
	Public WithEvents cmdCycleCountReport As System.Windows.Forms.Button
	Public WithEvents cboStatus As System.Windows.Forms.ComboBox
	Public WithEvents cmdEdit As System.Windows.Forms.Button
	Public WithEvents cmdAdd As System.Windows.Forms.Button
	Public WithEvents cmdDelete As System.Windows.Forms.Button
	Public WithEvents cmdCloseCounts As System.Windows.Forms.Button
	Public WithEvents txtCountName As System.Windows.Forms.TextBox
    Public WithEvents cmdSearch As System.Windows.Forms.Button
    Public WithEvents Label2 As System.Windows.Forms.Label
	Public WithEvents Label1 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_0 As System.Windows.Forms.Label
	Public WithEvents fraLocations As System.Windows.Forms.GroupBox
	Public WithEvents cboStore As System.Windows.Forms.ComboBox
	Public WithEvents cboSubTeam As System.Windows.Forms.ComboBox
    Public WithEvents _lblLabel_1 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_4 As System.Windows.Forms.Label
	Public WithEvents _lblSubTeam_2 As System.Windows.Forms.Label
	Public WithEvents lblStore As System.Windows.Forms.Label
	Public WithEvents lblLabel As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	Public WithEvents lblSubTeam As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCycleCountMasterEdit))
        Dim Appearance1 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim UltraGridBand1 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1)
        Dim UltraGridColumn1 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("CountID")
        Dim UltraGridColumn2 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("External")
        Dim UltraGridColumn3 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("InvLocID")
        Dim UltraGridColumn4 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Type")
        Dim UltraGridColumn5 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Name")
        Dim UltraGridColumn6 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Start Date")
        Dim UltraGridColumn7 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Status")
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
        Dim Appearance12 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance13 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdMasterReport = New System.Windows.Forms.Button
        Me.cmdCloseMaster = New System.Windows.Forms.Button
        Me.cmdApply = New System.Windows.Forms.Button
        Me.cmdExit = New System.Windows.Forms.Button
        Me.cmdCycleCountReport = New System.Windows.Forms.Button
        Me.cmdEdit = New System.Windows.Forms.Button
        Me.cmdAdd = New System.Windows.Forms.Button
        Me.cmdDelete = New System.Windows.Forms.Button
        Me.cmdCloseCounts = New System.Windows.Forms.Button
        Me.cmdSearch = New System.Windows.Forms.Button
        Me._lblLabel_4 = New System.Windows.Forms.Label
        Me.Frame1 = New System.Windows.Forms.GroupBox
        Me.lblCountsLocked = New System.Windows.Forms.Label
        Me.lblMasterStatus = New System.Windows.Forms.Label
        Me.chkEndOfPeriod = New System.Windows.Forms.CheckBox
        Me.fraLocations = New System.Windows.Forms.GroupBox
        Me.ugrdCounts = New Infragistics.Win.UltraWinGrid.UltraGrid
        Me.cboStatus = New System.Windows.Forms.ComboBox
        Me.txtCountName = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me._lblLabel_0 = New System.Windows.Forms.Label
        Me.dtpStartScan = New System.Windows.Forms.DateTimePicker
        Me.dtpEndScan = New System.Windows.Forms.DateTimePicker
        Me.cboStore = New System.Windows.Forms.ComboBox
        Me.cboSubTeam = New System.Windows.Forms.ComboBox
        Me._lblLabel_1 = New System.Windows.Forms.Label
        Me._lblSubTeam_2 = New System.Windows.Forms.Label
        Me.lblStore = New System.Windows.Forms.Label
        Me.lblLabel = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(Me.components)
        Me.lblSubTeam = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(Me.components)
        Me.Frame1.SuspendLayout()
        Me.fraLocations.SuspendLayout()
        CType(Me.ugrdCounts, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblSubTeam, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdMasterReport
        '
        Me.cmdMasterReport.BackColor = System.Drawing.SystemColors.Control
        Me.cmdMasterReport.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdMasterReport, "cmdMasterReport")
        Me.cmdMasterReport.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdMasterReport.Name = "cmdMasterReport"
        Me.ToolTip1.SetToolTip(Me.cmdMasterReport, resources.GetString("cmdMasterReport.ToolTip"))
        Me.cmdMasterReport.UseVisualStyleBackColor = False
        '
        'cmdCloseMaster
        '
        Me.cmdCloseMaster.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCloseMaster.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdCloseMaster, "cmdCloseMaster")
        Me.cmdCloseMaster.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCloseMaster.Name = "cmdCloseMaster"
        Me.ToolTip1.SetToolTip(Me.cmdCloseMaster, resources.GetString("cmdCloseMaster.ToolTip"))
        Me.cmdCloseMaster.UseVisualStyleBackColor = False
        '
        'cmdApply
        '
        Me.cmdApply.BackColor = System.Drawing.SystemColors.Control
        Me.cmdApply.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdApply, "cmdApply")
        Me.cmdApply.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdApply.Name = "cmdApply"
        Me.ToolTip1.SetToolTip(Me.cmdApply, resources.GetString("cmdApply.ToolTip"))
        Me.cmdApply.UseVisualStyleBackColor = False
        '
        'cmdExit
        '
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        resources.ApplyResources(Me.cmdExit, "cmdExit")
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Name = "cmdExit"
        Me.ToolTip1.SetToolTip(Me.cmdExit, resources.GetString("cmdExit.ToolTip"))
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'cmdCycleCountReport
        '
        Me.cmdCycleCountReport.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCycleCountReport.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdCycleCountReport, "cmdCycleCountReport")
        Me.cmdCycleCountReport.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCycleCountReport.Name = "cmdCycleCountReport"
        Me.ToolTip1.SetToolTip(Me.cmdCycleCountReport, resources.GetString("cmdCycleCountReport.ToolTip"))
        Me.cmdCycleCountReport.UseVisualStyleBackColor = False
        '
        'cmdEdit
        '
        Me.cmdEdit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEdit.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdEdit, "cmdEdit")
        Me.cmdEdit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEdit.Name = "cmdEdit"
        Me.cmdEdit.Tag = "B"
        Me.ToolTip1.SetToolTip(Me.cmdEdit, resources.GetString("cmdEdit.ToolTip"))
        Me.cmdEdit.UseVisualStyleBackColor = False
        '
        'cmdAdd
        '
        Me.cmdAdd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAdd.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdAdd, "cmdAdd")
        Me.cmdAdd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAdd.Name = "cmdAdd"
        Me.ToolTip1.SetToolTip(Me.cmdAdd, resources.GetString("cmdAdd.ToolTip"))
        Me.cmdAdd.UseVisualStyleBackColor = False
        '
        'cmdDelete
        '
        Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDelete.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdDelete, "cmdDelete")
        Me.cmdDelete.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDelete.Name = "cmdDelete"
        Me.ToolTip1.SetToolTip(Me.cmdDelete, resources.GetString("cmdDelete.ToolTip"))
        Me.cmdDelete.UseVisualStyleBackColor = False
        '
        'cmdCloseCounts
        '
        Me.cmdCloseCounts.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCloseCounts.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdCloseCounts, "cmdCloseCounts")
        Me.cmdCloseCounts.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCloseCounts.Name = "cmdCloseCounts"
        Me.ToolTip1.SetToolTip(Me.cmdCloseCounts, resources.GetString("cmdCloseCounts.ToolTip"))
        Me.cmdCloseCounts.UseVisualStyleBackColor = False
        '
        'cmdSearch
        '
        Me.cmdSearch.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSearch.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdSearch, "cmdSearch")
        Me.cmdSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSearch.Name = "cmdSearch"
        Me.ToolTip1.SetToolTip(Me.cmdSearch, resources.GetString("cmdSearch.ToolTip"))
        Me.cmdSearch.UseVisualStyleBackColor = False
        '
        '_lblLabel_4
        '
        Me._lblLabel_4.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_4.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lblLabel_4, "_lblLabel_4")
        Me._lblLabel_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_4, CType(4, Short))
        Me._lblLabel_4.Name = "_lblLabel_4"
        Me.ToolTip1.SetToolTip(Me._lblLabel_4, resources.GetString("_lblLabel_4.ToolTip"))
        '
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me.cmdCloseMaster)
        Me.Frame1.Controls.Add(Me.lblCountsLocked)
        Me.Frame1.Controls.Add(Me.lblMasterStatus)
        resources.ApplyResources(Me.Frame1, "Frame1")
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Name = "Frame1"
        Me.Frame1.TabStop = False
        '
        'lblCountsLocked
        '
        Me.lblCountsLocked.BackColor = System.Drawing.SystemColors.Control
        Me.lblCountsLocked.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblCountsLocked, "lblCountsLocked")
        Me.lblCountsLocked.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCountsLocked.Name = "lblCountsLocked"
        '
        'lblMasterStatus
        '
        Me.lblMasterStatus.BackColor = System.Drawing.SystemColors.Control
        Me.lblMasterStatus.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblMasterStatus, "lblMasterStatus")
        Me.lblMasterStatus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMasterStatus.Name = "lblMasterStatus"
        '
        'chkEndOfPeriod
        '
        Me.chkEndOfPeriod.BackColor = System.Drawing.SystemColors.Control
        Me.chkEndOfPeriod.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.chkEndOfPeriod, "chkEndOfPeriod")
        Me.chkEndOfPeriod.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkEndOfPeriod.Name = "chkEndOfPeriod"
        Me.chkEndOfPeriod.UseVisualStyleBackColor = False
        '
        'fraLocations
        '
        Me.fraLocations.BackColor = System.Drawing.SystemColors.Control
        Me.fraLocations.Controls.Add(Me.ugrdCounts)
        Me.fraLocations.Controls.Add(Me.cmdCycleCountReport)
        Me.fraLocations.Controls.Add(Me.cboStatus)
        Me.fraLocations.Controls.Add(Me.cmdEdit)
        Me.fraLocations.Controls.Add(Me.cmdAdd)
        Me.fraLocations.Controls.Add(Me.cmdDelete)
        Me.fraLocations.Controls.Add(Me.cmdCloseCounts)
        Me.fraLocations.Controls.Add(Me.txtCountName)
        Me.fraLocations.Controls.Add(Me.cmdSearch)
        Me.fraLocations.Controls.Add(Me.Label2)
        Me.fraLocations.Controls.Add(Me.Label1)
        Me.fraLocations.Controls.Add(Me._lblLabel_0)
        Me.fraLocations.Controls.Add(Me.dtpStartScan)
        resources.ApplyResources(Me.fraLocations, "fraLocations")
        Me.fraLocations.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraLocations.Name = "fraLocations"
        Me.fraLocations.TabStop = False
        '
        'ugrdCounts
        '
        Appearance1.BackColor = System.Drawing.SystemColors.Window
        Appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption
        resources.ApplyResources(Appearance1.FontData, "Appearance1.FontData")
        resources.ApplyResources(Appearance1, "Appearance1")
        Appearance1.ForceApplyResources = "FontData|"
        Me.ugrdCounts.DisplayLayout.Appearance = Appearance1
        UltraGridColumn1.Header.VisiblePosition = 0
        UltraGridColumn1.Hidden = True
        UltraGridColumn2.Header.VisiblePosition = 1
        UltraGridColumn2.Hidden = True
        UltraGridColumn3.Header.VisiblePosition = 2
        UltraGridColumn3.Hidden = True
        UltraGridColumn4.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn4.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn4.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn4.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn4.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn4.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn4.Header.VisiblePosition = 3
        UltraGridColumn4.Width = 60
        UltraGridColumn5.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn5.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn5.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn5.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn5.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn5.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn5.Header.VisiblePosition = 4
        UltraGridColumn5.Width = 251
        UltraGridColumn6.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn6.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn6.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn6.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn6.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn6.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn6.Header.VisiblePosition = 5
        UltraGridColumn7.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn7.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn7.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn7.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn7.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn7.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn7.Header.VisiblePosition = 6
        UltraGridColumn7.Width = 60
        UltraGridBand1.Columns.AddRange(New Object() {UltraGridColumn1, UltraGridColumn2, UltraGridColumn3, UltraGridColumn4, UltraGridColumn5, UltraGridColumn6, UltraGridColumn7})
        UltraGridBand1.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[True]
        Me.ugrdCounts.DisplayLayout.BandsSerializer.Add(UltraGridBand1)
        Me.ugrdCounts.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.ugrdCounts.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[True]
        Me.ugrdCounts.DisplayLayout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.[False]
        Appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance2.BorderColor = System.Drawing.SystemColors.Window
        resources.ApplyResources(Appearance2.FontData, "Appearance2.FontData")
        resources.ApplyResources(Appearance2, "Appearance2")
        Appearance2.ForceApplyResources = "FontData|"
        Me.ugrdCounts.DisplayLayout.GroupByBox.Appearance = Appearance2
        Appearance3.ForeColor = System.Drawing.SystemColors.GrayText
        resources.ApplyResources(Appearance3.FontData, "Appearance3.FontData")
        resources.ApplyResources(Appearance3, "Appearance3")
        Appearance3.ForceApplyResources = "FontData|"
        Me.ugrdCounts.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance3
        Me.ugrdCounts.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance4.BackColor2 = System.Drawing.SystemColors.Control
        Appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance4.ForeColor = System.Drawing.SystemColors.GrayText
        resources.ApplyResources(Appearance4.FontData, "Appearance4.FontData")
        resources.ApplyResources(Appearance4, "Appearance4")
        Appearance4.ForceApplyResources = "FontData|"
        Me.ugrdCounts.DisplayLayout.GroupByBox.PromptAppearance = Appearance4
        Me.ugrdCounts.DisplayLayout.MaxBandDepth = 1
        Me.ugrdCounts.DisplayLayout.MaxColScrollRegions = 1
        Me.ugrdCounts.DisplayLayout.MaxRowScrollRegions = 1
        Appearance5.BackColor = System.Drawing.SystemColors.Window
        Appearance5.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Appearance5.FontData, "Appearance5.FontData")
        resources.ApplyResources(Appearance5, "Appearance5")
        Appearance5.ForceApplyResources = "FontData|"
        Me.ugrdCounts.DisplayLayout.Override.ActiveCellAppearance = Appearance5
        Appearance6.ForeColor = System.Drawing.SystemColors.GrayText
        resources.ApplyResources(Appearance6.FontData, "Appearance6.FontData")
        resources.ApplyResources(Appearance6, "Appearance6")
        Appearance6.ForceApplyResources = "FontData|"
        Me.ugrdCounts.DisplayLayout.Override.ActiveRowAppearance = Appearance6
        Me.ugrdCounts.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.ugrdCounts.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance7.BackColor = System.Drawing.SystemColors.Window
        resources.ApplyResources(Appearance7.FontData, "Appearance7.FontData")
        resources.ApplyResources(Appearance7, "Appearance7")
        Appearance7.ForceApplyResources = "FontData|"
        Me.ugrdCounts.DisplayLayout.Override.CardAreaAppearance = Appearance7
        Appearance8.BorderColor = System.Drawing.Color.Silver
        Appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        resources.ApplyResources(Appearance8.FontData, "Appearance8.FontData")
        resources.ApplyResources(Appearance8, "Appearance8")
        Appearance8.ForceApplyResources = "FontData|"
        Me.ugrdCounts.DisplayLayout.Override.CellAppearance = Appearance8
        Me.ugrdCounts.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        Me.ugrdCounts.DisplayLayout.Override.CellPadding = 0
        Appearance9.BackColor = System.Drawing.SystemColors.Control
        Appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance9.BorderColor = System.Drawing.SystemColors.Window
        resources.ApplyResources(Appearance9.FontData, "Appearance9.FontData")
        resources.ApplyResources(Appearance9, "Appearance9")
        Appearance9.ForceApplyResources = "FontData|"
        Me.ugrdCounts.DisplayLayout.Override.GroupByRowAppearance = Appearance9
        resources.ApplyResources(Appearance10, "Appearance10")
        resources.ApplyResources(Appearance10.FontData, "Appearance10.FontData")
        Appearance10.ForceApplyResources = "FontData|"
        Me.ugrdCounts.DisplayLayout.Override.HeaderAppearance = Appearance10
        Me.ugrdCounts.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti
        Me.ugrdCounts.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance11.BackColor = System.Drawing.SystemColors.ControlLight
        resources.ApplyResources(Appearance11.FontData, "Appearance11.FontData")
        resources.ApplyResources(Appearance11, "Appearance11")
        Appearance11.ForceApplyResources = "FontData|"
        Me.ugrdCounts.DisplayLayout.Override.RowAlternateAppearance = Appearance11
        Appearance12.BackColor = System.Drawing.SystemColors.Window
        Appearance12.BorderColor = System.Drawing.Color.Silver
        resources.ApplyResources(Appearance12.FontData, "Appearance12.FontData")
        resources.ApplyResources(Appearance12, "Appearance12")
        Appearance12.ForceApplyResources = "FontData|"
        Me.ugrdCounts.DisplayLayout.Override.RowAppearance = Appearance12
        Me.ugrdCounts.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[False]
        Appearance13.BackColor = System.Drawing.SystemColors.ControlLight
        resources.ApplyResources(Appearance13.FontData, "Appearance13.FontData")
        resources.ApplyResources(Appearance13, "Appearance13")
        Appearance13.ForceApplyResources = "FontData|"
        Me.ugrdCounts.DisplayLayout.Override.TemplateAddRowAppearance = Appearance13
        Me.ugrdCounts.DisplayLayout.Scrollbars = Infragistics.Win.UltraWinGrid.Scrollbars.Vertical
        Me.ugrdCounts.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.ugrdCounts.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.ugrdCounts.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand
        Me.ugrdCounts.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.Horizontal
        resources.ApplyResources(Me.ugrdCounts, "ugrdCounts")
        Me.ugrdCounts.Name = "ugrdCounts"
        '
        'cboStatus
        '
        Me.cboStatus.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cboStatus.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboStatus.BackColor = System.Drawing.SystemColors.Window
        Me.cboStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.cboStatus, "cboStatus")
        Me.cboStatus.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboStatus.Items.AddRange(New Object() {resources.GetString("cboStatus.Items"), resources.GetString("cboStatus.Items1"), resources.GetString("cboStatus.Items2")})
        Me.cboStatus.Name = "cboStatus"
        Me.cboStatus.Sorted = True
        '
        'txtCountName
        '
        Me.txtCountName.AcceptsReturn = True
        Me.txtCountName.BackColor = System.Drawing.SystemColors.Window
        Me.txtCountName.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.txtCountName, "txtCountName")
        Me.txtCountName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCountName.Name = "txtCountName"
        Me.txtCountName.Tag = "String"
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.Label2, "Label2")
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Name = "Label2"
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Name = "Label1"
        '
        '_lblLabel_0
        '
        Me._lblLabel_0.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_0.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lblLabel_0, "_lblLabel_0")
        Me._lblLabel_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_0, CType(0, Short))
        Me._lblLabel_0.Name = "_lblLabel_0"
        '
        'dtpStartScan
        '
        Me.dtpStartScan.Checked = False
        resources.ApplyResources(Me.dtpStartScan, "dtpStartScan")
        Me.dtpStartScan.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpStartScan.Name = "dtpStartScan"
        Me.dtpStartScan.ShowCheckBox = True
        '
        'dtpEndScan
        '
        resources.ApplyResources(Me.dtpEndScan, "dtpEndScan")
        Me.dtpEndScan.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpEndScan.Name = "dtpEndScan"
        '
        'cboStore
        '
        Me.cboStore.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cboStore.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboStore.BackColor = System.Drawing.SystemColors.Window
        Me.cboStore.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboStore.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.cboStore, "cboStore")
        Me.cboStore.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboStore.Name = "cboStore"
        Me.cboStore.Sorted = True
        '
        'cboSubTeam
        '
        Me.cboSubTeam.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cboSubTeam.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboSubTeam.BackColor = System.Drawing.SystemColors.Window
        Me.cboSubTeam.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboSubTeam.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.cboSubTeam, "cboSubTeam")
        Me.cboSubTeam.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboSubTeam.Name = "cboSubTeam"
        Me.cboSubTeam.Sorted = True
        '
        '_lblLabel_1
        '
        Me._lblLabel_1.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_1.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lblLabel_1, "_lblLabel_1")
        Me._lblLabel_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_1, CType(1, Short))
        Me._lblLabel_1.Name = "_lblLabel_1"
        '
        '_lblSubTeam_2
        '
        Me._lblSubTeam_2.BackColor = System.Drawing.Color.Transparent
        Me._lblSubTeam_2.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lblSubTeam_2, "_lblSubTeam_2")
        Me._lblSubTeam_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSubTeam.SetIndex(Me._lblSubTeam_2, CType(2, Short))
        Me._lblSubTeam_2.Name = "_lblSubTeam_2"
        '
        'lblStore
        '
        Me.lblStore.BackColor = System.Drawing.Color.Transparent
        Me.lblStore.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblStore, "lblStore")
        Me.lblStore.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblStore.Name = "lblStore"
        '
        'frmCycleCountMasterEdit
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdExit
        Me.Controls.Add(Me.cmdMasterReport)
        Me.Controls.Add(Me.Frame1)
        Me.Controls.Add(Me.chkEndOfPeriod)
        Me.Controls.Add(Me.cmdApply)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.fraLocations)
        Me.Controls.Add(Me.cboStore)
        Me.Controls.Add(Me.cboSubTeam)
        Me.Controls.Add(Me._lblLabel_1)
        Me.Controls.Add(Me._lblLabel_4)
        Me.Controls.Add(Me._lblSubTeam_2)
        Me.Controls.Add(Me.lblStore)
        Me.Controls.Add(Me.dtpEndScan)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmCycleCountMasterEdit"
        Me.Frame1.ResumeLayout(False)
        Me.fraLocations.ResumeLayout(False)
        Me.fraLocations.PerformLayout()
        CType(Me.ugrdCounts, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblSubTeam, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ugrdCounts As Infragistics.Win.UltraWinGrid.UltraGrid
    Friend WithEvents dtpEndScan As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtpStartScan As System.Windows.Forms.DateTimePicker
#End Region
End Class