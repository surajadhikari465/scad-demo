<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmCycleCountMasterList
#Region "Windows Form Designer generated code "
	<System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
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
	Public WithEvents cmdReport As System.Windows.Forms.Button
	Public WithEvents cmdCloseMaster As System.Windows.Forms.Button
	Public WithEvents cmdEditMaster As System.Windows.Forms.Button
	Public WithEvents cmdExit As System.Windows.Forms.Button
	Public WithEvents cmdAddMaster As System.Windows.Forms.Button
	Public WithEvents cmdDeleteMaster As System.Windows.Forms.Button
	Public WithEvents cboType As System.Windows.Forms.ComboBox
	Public WithEvents cboStore As System.Windows.Forms.ComboBox
	Public WithEvents cboStatus As System.Windows.Forms.ComboBox
    Public WithEvents cboSubTeam As System.Windows.Forms.ComboBox
	Public WithEvents cmdSearch As System.Windows.Forms.Button
	Public WithEvents Label4 As System.Windows.Forms.Label
	Public WithEvents Label1 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_4 As System.Windows.Forms.Label
	Public WithEvents lblStore As System.Windows.Forms.Label
	Public WithEvents _lblSubTeam_2 As System.Windows.Forms.Label
	Public WithEvents Frame1 As System.Windows.Forms.GroupBox
    Public WithEvents Label3 As System.Windows.Forms.Label
	Public WithEvents Label2 As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	Public WithEvents lblLabel As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	Public WithEvents lblSubTeam As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCycleCountMasterList))
        Dim Appearance1 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridBand1 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1)
        Dim UltraGridColumn1 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("MasterCountID")
        Dim UltraGridColumn2 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("ReqExternalCount")
        Dim UltraGridColumn3 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("SubCounts")
        Dim UltraGridColumn4 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Store Name")
        Dim UltraGridColumn5 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Sub Team Name")
        Dim UltraGridColumn6 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("End Scan")
        Dim UltraGridColumn7 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Status")
        Dim UltraGridColumn8 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Type")
        Dim Appearance2 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
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
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdReport = New System.Windows.Forms.Button()
        Me.cmdCloseMaster = New System.Windows.Forms.Button()
        Me.cmdEditMaster = New System.Windows.Forms.Button()
        Me.cmdExit = New System.Windows.Forms.Button()
        Me.cmdAddMaster = New System.Windows.Forms.Button()
        Me.cmdDeleteMaster = New System.Windows.Forms.Button()
        Me.cmdSearch = New System.Windows.Forms.Button()
        Me.cmdGenerateRecs = New System.Windows.Forms.Button()
        Me.Frame1 = New System.Windows.Forms.GroupBox()
        Me.cboType = New System.Windows.Forms.ComboBox()
        Me.cboStore = New System.Windows.Forms.ComboBox()
        Me.cboStatus = New System.Windows.Forms.ComboBox()
        Me.cboSubTeam = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me._lblLabel_4 = New System.Windows.Forms.Label()
        Me.lblStore = New System.Windows.Forms.Label()
        Me._lblSubTeam_2 = New System.Windows.Forms.Label()
        Me.dtpEndScan = New System.Windows.Forms.DateTimePicker()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(Me.components)
        Me.lblLabel = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(Me.components)
        Me.lblSubTeam = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(Me.components)
        Me.ugrdMaster = New Infragistics.Win.UltraWinGrid.UltraGrid()
        Me.Label_BOHFileDate = New System.Windows.Forms.Label()
        Me.Frame1.SuspendLayout()
        CType(Me.Label2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblSubTeam, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ugrdMaster, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdReport
        '
        resources.ApplyResources(Me.cmdReport, "cmdReport")
        Me.cmdReport.BackColor = System.Drawing.SystemColors.Control
        Me.cmdReport.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdReport.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdReport.Name = "cmdReport"
        Me.ToolTip1.SetToolTip(Me.cmdReport, resources.GetString("cmdReport.ToolTip"))
        Me.cmdReport.UseVisualStyleBackColor = False
        '
        'cmdCloseMaster
        '
        resources.ApplyResources(Me.cmdCloseMaster, "cmdCloseMaster")
        Me.cmdCloseMaster.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCloseMaster.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCloseMaster.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCloseMaster.Name = "cmdCloseMaster"
        Me.ToolTip1.SetToolTip(Me.cmdCloseMaster, resources.GetString("cmdCloseMaster.ToolTip"))
        Me.cmdCloseMaster.UseVisualStyleBackColor = False
        '
        'cmdEditMaster
        '
        resources.ApplyResources(Me.cmdEditMaster, "cmdEditMaster")
        Me.cmdEditMaster.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEditMaster.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEditMaster.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEditMaster.Name = "cmdEditMaster"
        Me.cmdEditMaster.Tag = "B"
        Me.ToolTip1.SetToolTip(Me.cmdEditMaster, resources.GetString("cmdEditMaster.ToolTip"))
        Me.cmdEditMaster.UseVisualStyleBackColor = False
        '
        'cmdExit
        '
        resources.ApplyResources(Me.cmdExit, "cmdExit")
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Name = "cmdExit"
        Me.ToolTip1.SetToolTip(Me.cmdExit, resources.GetString("cmdExit.ToolTip"))
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'cmdAddMaster
        '
        resources.ApplyResources(Me.cmdAddMaster, "cmdAddMaster")
        Me.cmdAddMaster.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddMaster.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddMaster.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddMaster.Name = "cmdAddMaster"
        Me.ToolTip1.SetToolTip(Me.cmdAddMaster, resources.GetString("cmdAddMaster.ToolTip"))
        Me.cmdAddMaster.UseVisualStyleBackColor = False
        '
        'cmdDeleteMaster
        '
        resources.ApplyResources(Me.cmdDeleteMaster, "cmdDeleteMaster")
        Me.cmdDeleteMaster.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDeleteMaster.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDeleteMaster.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDeleteMaster.Name = "cmdDeleteMaster"
        Me.ToolTip1.SetToolTip(Me.cmdDeleteMaster, resources.GetString("cmdDeleteMaster.ToolTip"))
        Me.cmdDeleteMaster.UseVisualStyleBackColor = False
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
        'cmdGenerateRecs
        '
        resources.ApplyResources(Me.cmdGenerateRecs, "cmdGenerateRecs")
        Me.cmdGenerateRecs.BackColor = System.Drawing.SystemColors.Control
        Me.cmdGenerateRecs.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdGenerateRecs.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdGenerateRecs.Image = Global.My.Resources.Resources.WFM_forklift2
        Me.cmdGenerateRecs.Name = "cmdGenerateRecs"
        Me.ToolTip1.SetToolTip(Me.cmdGenerateRecs, resources.GetString("cmdGenerateRecs.ToolTip"))
        Me.cmdGenerateRecs.UseVisualStyleBackColor = False
        '
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me.cboType)
        Me.Frame1.Controls.Add(Me.cboStore)
        Me.Frame1.Controls.Add(Me.cboStatus)
        Me.Frame1.Controls.Add(Me.cboSubTeam)
        Me.Frame1.Controls.Add(Me.cmdSearch)
        Me.Frame1.Controls.Add(Me.Label4)
        Me.Frame1.Controls.Add(Me.Label1)
        Me.Frame1.Controls.Add(Me._lblLabel_4)
        Me.Frame1.Controls.Add(Me.lblStore)
        Me.Frame1.Controls.Add(Me._lblSubTeam_2)
        Me.Frame1.Controls.Add(Me.dtpEndScan)
        resources.ApplyResources(Me.Frame1, "Frame1")
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Name = "Frame1"
        Me.Frame1.TabStop = False
        '
        'cboType
        '
        Me.cboType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cboType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboType.BackColor = System.Drawing.SystemColors.Window
        Me.cboType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.cboType, "cboType")
        Me.cboType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboType.Items.AddRange(New Object() {resources.GetString("cboType.Items"), resources.GetString("cboType.Items1"), resources.GetString("cboType.Items2")})
        Me.cboType.Name = "cboType"
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
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.Label4, "Label4")
        Me.Label4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label4.Name = "Label4"
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Name = "Label1"
        '
        '_lblLabel_4
        '
        Me._lblLabel_4.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_4.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lblLabel_4, "_lblLabel_4")
        Me._lblLabel_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_4, CType(4, Short))
        Me._lblLabel_4.Name = "_lblLabel_4"
        Me._lblLabel_4.Tag = "English:EndScan||French:La Scan Fini"
        '
        'lblStore
        '
        Me.lblStore.BackColor = System.Drawing.Color.Transparent
        Me.lblStore.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblStore, "lblStore")
        Me.lblStore.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblStore.Name = "lblStore"
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
        'dtpEndScan
        '
        Me.dtpEndScan.Checked = False
        resources.ApplyResources(Me.dtpEndScan, "dtpEndScan")
        Me.dtpEndScan.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpEndScan.Name = "dtpEndScan"
        Me.dtpEndScan.ShowCheckBox = True
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.SystemColors.Control
        Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.Label3, "Label3")
        Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label3.Name = "Label3"
        '
        'ugrdMaster
        '
        resources.ApplyResources(Me.ugrdMaster, "ugrdMaster")
        Appearance1.BackColor = System.Drawing.SystemColors.Window
        Appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.ugrdMaster.DisplayLayout.Appearance = Appearance1
        UltraGridColumn1.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn1.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn1.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn1.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn1.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn1.Header.VisiblePosition = 0
        UltraGridColumn1.Hidden = True
        UltraGridColumn2.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn2.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn2.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn2.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn2.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn2.Header.VisiblePosition = 1
        UltraGridColumn2.Hidden = True
        UltraGridColumn3.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn3.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn3.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn3.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn3.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn3.Header.VisiblePosition = 2
        UltraGridColumn3.Hidden = True
        UltraGridColumn4.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn4.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn4.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn4.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn4.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn4.Header.VisiblePosition = 3
        UltraGridColumn4.Width = 204
        UltraGridColumn5.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn5.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn5.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn5.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn5.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn5.Header.VisiblePosition = 4
        UltraGridColumn5.Width = 172
        UltraGridColumn6.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn6.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn6.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn6.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn6.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn6.Header.VisiblePosition = 5
        UltraGridColumn6.Width = 91
        UltraGridColumn7.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn7.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn7.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn7.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn7.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn7.Header.VisiblePosition = 6
        UltraGridColumn7.Width = 60
        UltraGridColumn8.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn8.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn8.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn8.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn8.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn8.Header.VisiblePosition = 7
        UltraGridColumn8.Width = 60
        UltraGridBand1.Columns.AddRange(New Object() {UltraGridColumn1, UltraGridColumn2, UltraGridColumn3, UltraGridColumn4, UltraGridColumn5, UltraGridColumn6, UltraGridColumn7, UltraGridColumn8})
        UltraGridBand1.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[True]
        Me.ugrdMaster.DisplayLayout.BandsSerializer.Add(UltraGridBand1)
        Me.ugrdMaster.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.ugrdMaster.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[True]
        Me.ugrdMaster.DisplayLayout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.[False]
        Appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance2.BorderColor = System.Drawing.SystemColors.Window
        Me.ugrdMaster.DisplayLayout.GroupByBox.Appearance = Appearance2
        Appearance3.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugrdMaster.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance3
        Me.ugrdMaster.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance4.BackColor2 = System.Drawing.SystemColors.Control
        Appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance4.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugrdMaster.DisplayLayout.GroupByBox.PromptAppearance = Appearance4
        Me.ugrdMaster.DisplayLayout.MaxBandDepth = 1
        Me.ugrdMaster.DisplayLayout.MaxColScrollRegions = 1
        Me.ugrdMaster.DisplayLayout.MaxRowScrollRegions = 1
        Appearance5.BackColor = System.Drawing.SystemColors.Window
        Appearance5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ugrdMaster.DisplayLayout.Override.ActiveCellAppearance = Appearance5
        Appearance6.BackColor = System.Drawing.SystemColors.HighlightText
        Appearance6.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugrdMaster.DisplayLayout.Override.ActiveRowAppearance = Appearance6
        Me.ugrdMaster.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.ugrdMaster.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance7.BackColor = System.Drawing.SystemColors.Window
        Me.ugrdMaster.DisplayLayout.Override.CardAreaAppearance = Appearance7
        Appearance8.BorderColor = System.Drawing.Color.Silver
        Appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.ugrdMaster.DisplayLayout.Override.CellAppearance = Appearance8
        Me.ugrdMaster.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.ugrdMaster.DisplayLayout.Override.CellPadding = 0
        Appearance9.BackColor = System.Drawing.SystemColors.Control
        Appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance9.BorderColor = System.Drawing.SystemColors.Window
        Me.ugrdMaster.DisplayLayout.Override.GroupByRowAppearance = Appearance9
        resources.ApplyResources(Appearance10, "Appearance10")
        Appearance10.ForceApplyResources = ""
        Me.ugrdMaster.DisplayLayout.Override.HeaderAppearance = Appearance10
        Me.ugrdMaster.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti
        Me.ugrdMaster.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance11.BackColor = System.Drawing.SystemColors.ControlLight
        Me.ugrdMaster.DisplayLayout.Override.RowAlternateAppearance = Appearance11
        Appearance12.BackColor = System.Drawing.SystemColors.Window
        Appearance12.BorderColor = System.Drawing.Color.Silver
        Me.ugrdMaster.DisplayLayout.Override.RowAppearance = Appearance12
        Me.ugrdMaster.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[False]
        Appearance13.BackColor = System.Drawing.SystemColors.ControlLight
        Me.ugrdMaster.DisplayLayout.Override.TemplateAddRowAppearance = Appearance13
        Me.ugrdMaster.DisplayLayout.Scrollbars = Infragistics.Win.UltraWinGrid.Scrollbars.Vertical
        Me.ugrdMaster.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.ugrdMaster.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.ugrdMaster.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand
        Me.ugrdMaster.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.Horizontal
        Me.ugrdMaster.Name = "ugrdMaster"
        '
        'Label_BOHFileDate
        '
        Me.Label_BOHFileDate.BackColor = System.Drawing.Color.Transparent
        Me.Label_BOHFileDate.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.Label_BOHFileDate, "Label_BOHFileDate")
        Me.Label_BOHFileDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label_BOHFileDate.Name = "Label_BOHFileDate"
        '
        'frmCycleCountMasterList
        '
        Me.AcceptButton = Me.cmdSearch
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdExit
        Me.Controls.Add(Me.Label_BOHFileDate)
        Me.Controls.Add(Me.cmdGenerateRecs)
        Me.Controls.Add(Me.ugrdMaster)
        Me.Controls.Add(Me.cmdReport)
        Me.Controls.Add(Me.cmdCloseMaster)
        Me.Controls.Add(Me.cmdEditMaster)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.cmdAddMaster)
        Me.Controls.Add(Me.cmdDeleteMaster)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Frame1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmCycleCountMasterList"
        Me.ShowInTaskbar = False
        Me.Frame1.ResumeLayout(False)
        CType(Me.Label2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblSubTeam, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ugrdMaster, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ugrdMaster As Infragistics.Win.UltraWinGrid.UltraGrid
    Friend WithEvents dtpEndScan As System.Windows.Forms.DateTimePicker
    Public WithEvents cmdGenerateRecs As System.Windows.Forms.Button
    Public WithEvents Label_BOHFileDate As System.Windows.Forms.Label
#End Region 
End Class