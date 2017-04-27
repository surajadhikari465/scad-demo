<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmCycleCountEdit
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
	Public WithEvents txtName As System.Windows.Forms.TextBox
	Public WithEvents txtType As System.Windows.Forms.TextBox
	Public WithEvents cmdClose As System.Windows.Forms.Button
	Public WithEvents lblCountsLocked As System.Windows.Forms.Label
	Public WithEvents lblCountStatus As System.Windows.Forms.Label
	Public WithEvents Frame2 As System.Windows.Forms.GroupBox
	Public WithEvents cmdReport As System.Windows.Forms.Button
	Public WithEvents cmdAdd As System.Windows.Forms.Button
	Public WithEvents cmdDelete As System.Windows.Forms.Button
	Public WithEvents cmdEdit As System.Windows.Forms.Button
	Public WithEvents cmdSearch As System.Windows.Forms.Button
	Public WithEvents txtDesc As System.Windows.Forms.TextBox
    Public WithEvents txtVendor As System.Windows.Forms.TextBox
	Public WithEvents txtVendorID As System.Windows.Forms.TextBox
	Public WithEvents txtIdentifier As System.Windows.Forms.TextBox
    Public WithEvents _lblLabel_3 As System.Windows.Forms.Label
    Public WithEvents _lblLabel_8 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_9 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_1 As System.Windows.Forms.Label
	Public WithEvents Frame1 As System.Windows.Forms.GroupBox
	Public WithEvents cmdExit As System.Windows.Forms.Button
	Public WithEvents txtStartScan As System.Windows.Forms.TextBox
	Public WithEvents lblStore As System.Windows.Forms.Label
	Public WithEvents _lblSubTeam_2 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_4 As System.Windows.Forms.Label
	Public WithEvents lblLabel As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	Public WithEvents lblSubTeam As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCycleCountEdit))
        Dim Appearance1 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim UltraGridBand1 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1)
        Dim UltraGridColumn1 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("CycleCountItemID")
        Dim UltraGridColumn2 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Item_Key")
        Dim UltraGridColumn3 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("CostedByWeight")
        Dim UltraGridColumn4 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Identifier")
        Dim UltraGridColumn5 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Item_Description", -1, Nothing, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, False)
        Dim UltraGridColumn6 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("CountTotal")
        Dim UltraGridColumn7 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("WeightTotal")
        Dim UltraGridColumn8 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Package_Desc1")
        Dim UltraGridColumn9 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Package_Desc2")
        Dim UltraGridColumn10 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Package_Unit_ID")
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
        Dim Appearance14 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance15 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdClose = New System.Windows.Forms.Button
        Me.cmdReport = New System.Windows.Forms.Button
        Me.cmdAdd = New System.Windows.Forms.Button
        Me.cmdDelete = New System.Windows.Forms.Button
        Me.cmdEdit = New System.Windows.Forms.Button
        Me.cmdSearch = New System.Windows.Forms.Button
        Me.cmdExit = New System.Windows.Forms.Button
        Me.txtName = New System.Windows.Forms.TextBox
        Me.txtType = New System.Windows.Forms.TextBox
        Me.Frame2 = New System.Windows.Forms.GroupBox
        Me.lblCountsLocked = New System.Windows.Forms.Label
        Me.lblCountStatus = New System.Windows.Forms.Label
        Me.Frame1 = New System.Windows.Forms.GroupBox
        Me.ugrdItems = New Infragistics.Win.UltraWinGrid.UltraGrid
        Me.txtDesc = New System.Windows.Forms.TextBox
        Me.txtVendor = New System.Windows.Forms.TextBox
        Me.txtVendorID = New System.Windows.Forms.TextBox
        Me.txtIdentifier = New System.Windows.Forms.TextBox
        Me._lblLabel_3 = New System.Windows.Forms.Label
        Me._lblLabel_8 = New System.Windows.Forms.Label
        Me._lblLabel_9 = New System.Windows.Forms.Label
        Me._lblLabel_1 = New System.Windows.Forms.Label
        Me.txtStartScan = New System.Windows.Forms.TextBox
        Me.lblStore = New System.Windows.Forms.Label
        Me._lblSubTeam_2 = New System.Windows.Forms.Label
        Me._lblLabel_4 = New System.Windows.Forms.Label
        Me.lblLabel = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(Me.components)
        Me.lblSubTeam = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(Me.components)
        Me.Frame2.SuspendLayout()
        Me.Frame1.SuspendLayout()
        CType(Me.ugrdItems, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblSubTeam, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdClose
        '
        Me.cmdClose.BackColor = System.Drawing.SystemColors.Control
        Me.cmdClose.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdClose, "cmdClose")
        Me.cmdClose.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdClose.Name = "cmdClose"
        Me.ToolTip1.SetToolTip(Me.cmdClose, resources.GetString("cmdClose.ToolTip"))
        Me.cmdClose.UseVisualStyleBackColor = False
        '
        'cmdReport
        '
        Me.cmdReport.BackColor = System.Drawing.SystemColors.Control
        Me.cmdReport.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdReport, "cmdReport")
        Me.cmdReport.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdReport.Name = "cmdReport"
        Me.ToolTip1.SetToolTip(Me.cmdReport, resources.GetString("cmdReport.ToolTip"))
        Me.cmdReport.UseVisualStyleBackColor = False
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
        'cmdEdit
        '
        Me.cmdEdit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEdit.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdEdit, "cmdEdit")
        Me.cmdEdit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEdit.Name = "cmdEdit"
        Me.ToolTip1.SetToolTip(Me.cmdEdit, resources.GetString("cmdEdit.ToolTip"))
        Me.cmdEdit.UseVisualStyleBackColor = False
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
        'txtName
        '
        Me.txtName.AcceptsReturn = True
        Me.txtName.BackColor = System.Drawing.SystemColors.Window
        Me.txtName.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.txtName, "txtName")
        Me.txtName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtName.Name = "txtName"
        Me.txtName.ReadOnly = True
        Me.txtName.Tag = "DateTime"
        '
        'txtType
        '
        Me.txtType.AcceptsReturn = True
        Me.txtType.BackColor = System.Drawing.SystemColors.Window
        Me.txtType.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.txtType, "txtType")
        Me.txtType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtType.Name = "txtType"
        Me.txtType.ReadOnly = True
        Me.txtType.Tag = "DateTime"
        '
        'Frame2
        '
        Me.Frame2.BackColor = System.Drawing.SystemColors.Control
        Me.Frame2.Controls.Add(Me.cmdClose)
        Me.Frame2.Controls.Add(Me.lblCountsLocked)
        Me.Frame2.Controls.Add(Me.lblCountStatus)
        resources.ApplyResources(Me.Frame2, "Frame2")
        Me.Frame2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame2.Name = "Frame2"
        Me.Frame2.TabStop = False
        '
        'lblCountsLocked
        '
        Me.lblCountsLocked.BackColor = System.Drawing.SystemColors.Control
        Me.lblCountsLocked.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblCountsLocked, "lblCountsLocked")
        Me.lblCountsLocked.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCountsLocked.Name = "lblCountsLocked"
        '
        'lblCountStatus
        '
        Me.lblCountStatus.BackColor = System.Drawing.SystemColors.Control
        Me.lblCountStatus.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblCountStatus, "lblCountStatus")
        Me.lblCountStatus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCountStatus.Name = "lblCountStatus"
        '
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me.ugrdItems)
        Me.Frame1.Controls.Add(Me.cmdAdd)
        Me.Frame1.Controls.Add(Me.cmdDelete)
        Me.Frame1.Controls.Add(Me.cmdEdit)
        Me.Frame1.Controls.Add(Me.cmdSearch)
        Me.Frame1.Controls.Add(Me.txtDesc)
        Me.Frame1.Controls.Add(Me.txtVendor)
        Me.Frame1.Controls.Add(Me.txtVendorID)
        Me.Frame1.Controls.Add(Me.txtIdentifier)
        Me.Frame1.Controls.Add(Me._lblLabel_3)
        Me.Frame1.Controls.Add(Me._lblLabel_8)
        Me.Frame1.Controls.Add(Me._lblLabel_9)
        Me.Frame1.Controls.Add(Me._lblLabel_1)
        resources.ApplyResources(Me.Frame1, "Frame1")
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Name = "Frame1"
        Me.Frame1.TabStop = False
        '
        'ugrdItems
        '
        Appearance1.BackColor = System.Drawing.SystemColors.ControlLight
        Appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.ugrdItems.DisplayLayout.Appearance = Appearance1
        Me.ugrdItems.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn
        UltraGridColumn1.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn1.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn1.AllowRowSummaries = Infragistics.Win.UltraWinGrid.AllowRowSummaries.[False]
        UltraGridColumn1.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn1.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn1.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn1.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn1.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn1.Header.VisiblePosition = 0
        UltraGridColumn1.Hidden = True
        UltraGridColumn2.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn2.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn2.AllowRowSummaries = Infragistics.Win.UltraWinGrid.AllowRowSummaries.[False]
        UltraGridColumn2.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn2.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn2.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn2.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn2.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn2.Header.VisiblePosition = 1
        UltraGridColumn2.Hidden = True
        UltraGridColumn2.Width = 14
        UltraGridColumn3.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn3.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn3.AllowRowSummaries = Infragistics.Win.UltraWinGrid.AllowRowSummaries.[False]
        UltraGridColumn3.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn3.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn3.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn3.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn3.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn3.Header.VisiblePosition = 2
        UltraGridColumn3.Hidden = True
        UltraGridColumn4.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn4.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn4.AllowRowSummaries = Infragistics.Win.UltraWinGrid.AllowRowSummaries.[False]
        UltraGridColumn4.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn4.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn4.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn4.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn4.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn4.Header.VisiblePosition = 3
        UltraGridColumn4.RowLayoutColumnInfo.OriginX = 2
        UltraGridColumn4.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn4.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(0, 17)
        UltraGridColumn4.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn4.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn5.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn5.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn5.AllowRowSummaries = Infragistics.Win.UltraWinGrid.AllowRowSummaries.[False]
        UltraGridColumn5.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn5.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn5.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn5.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn5.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn5.Header.Caption = resources.GetString("resource.Caption")
        UltraGridColumn5.Header.VisiblePosition = 4
        UltraGridColumn5.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(201, 0)
        UltraGridColumn6.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn6.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn6.AllowRowSummaries = Infragistics.Win.UltraWinGrid.AllowRowSummaries.[False]
        UltraGridColumn6.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn6.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn6.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn6.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn6.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn6.Header.Caption = resources.GetString("resource.Caption1")
        UltraGridColumn6.Header.VisiblePosition = 5
        UltraGridColumn6.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(94, 0)
        UltraGridColumn7.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn7.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn7.AllowRowSummaries = Infragistics.Win.UltraWinGrid.AllowRowSummaries.[False]
        UltraGridColumn7.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn7.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn7.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn7.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn7.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn7.Header.Caption = resources.GetString("resource.Caption2")
        UltraGridColumn7.Header.VisiblePosition = 6
        UltraGridColumn7.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(71, 0)
        UltraGridColumn8.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn8.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn8.AllowRowSummaries = Infragistics.Win.UltraWinGrid.AllowRowSummaries.[False]
        UltraGridColumn8.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn8.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn8.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn8.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn8.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn8.Header.VisiblePosition = 7
        UltraGridColumn8.Hidden = True
        UltraGridColumn9.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn9.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn9.AllowRowSummaries = Infragistics.Win.UltraWinGrid.AllowRowSummaries.[False]
        UltraGridColumn9.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn9.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn9.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn9.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn9.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn9.Header.VisiblePosition = 8
        UltraGridColumn9.Hidden = True
        UltraGridColumn10.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn10.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn10.AllowRowSummaries = Infragistics.Win.UltraWinGrid.AllowRowSummaries.[False]
        UltraGridColumn10.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn10.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn10.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn10.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn10.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn10.Header.VisiblePosition = 9
        UltraGridColumn10.Hidden = True
        UltraGridBand1.Columns.AddRange(New Object() {UltraGridColumn1, UltraGridColumn2, UltraGridColumn3, UltraGridColumn4, UltraGridColumn5, UltraGridColumn6, UltraGridColumn7, UltraGridColumn8, UltraGridColumn9, UltraGridColumn10})
        UltraGridBand1.RowLayoutStyle = Infragistics.Win.UltraWinGrid.RowLayoutStyle.None
        Me.ugrdItems.DisplayLayout.BandsSerializer.Add(UltraGridBand1)
        Me.ugrdItems.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Appearance2.FontData.BoldAsString = resources.GetString("resource.BoldAsString")
        Me.ugrdItems.DisplayLayout.CaptionAppearance = Appearance2
        Me.ugrdItems.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[True]
        Appearance3.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance3.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance3.BorderColor = System.Drawing.SystemColors.Window
        Me.ugrdItems.DisplayLayout.GroupByBox.Appearance = Appearance3
        Appearance4.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugrdItems.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance4
        Me.ugrdItems.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.ugrdItems.DisplayLayout.GroupByBox.Hidden = True
        Appearance5.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance5.BackColor2 = System.Drawing.SystemColors.Control
        Appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance5.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugrdItems.DisplayLayout.GroupByBox.PromptAppearance = Appearance5
        Me.ugrdItems.DisplayLayout.MaxColScrollRegions = 1
        Me.ugrdItems.DisplayLayout.MaxRowScrollRegions = 1
        Appearance6.BackColor = System.Drawing.SystemColors.Window
        Appearance6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ugrdItems.DisplayLayout.Override.ActiveCellAppearance = Appearance6
        Appearance7.BackColor = System.Drawing.SystemColors.HighlightText
        Appearance7.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugrdItems.DisplayLayout.Override.ActiveRowAppearance = Appearance7
        Me.ugrdItems.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.ugrdItems.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance8.BackColor = System.Drawing.SystemColors.Window
        Me.ugrdItems.DisplayLayout.Override.CardAreaAppearance = Appearance8
        Appearance9.BorderColor = System.Drawing.Color.Silver
        Appearance9.FontData.BoldAsString = resources.GetString("resource.BoldAsString1")
        Appearance9.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.ugrdItems.DisplayLayout.Override.CellAppearance = Appearance9
        Me.ugrdItems.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        Me.ugrdItems.DisplayLayout.Override.CellPadding = 0
        Appearance10.FontData.BoldAsString = resources.GetString("resource.BoldAsString2")
        Me.ugrdItems.DisplayLayout.Override.FixedHeaderAppearance = Appearance10
        Appearance11.BackColor = System.Drawing.SystemColors.Control
        Appearance11.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance11.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance11.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance11.BorderColor = System.Drawing.SystemColors.Window
        Me.ugrdItems.DisplayLayout.Override.GroupByRowAppearance = Appearance11
        Appearance12.FontData.BoldAsString = resources.GetString("resource.BoldAsString3")
        Appearance12.TextHAlign = Infragistics.Win.HAlign.Left
        Me.ugrdItems.DisplayLayout.Override.HeaderAppearance = Appearance12
        Me.ugrdItems.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle
        Me.ugrdItems.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance13.BackColor = System.Drawing.SystemColors.Control
        Me.ugrdItems.DisplayLayout.Override.RowAlternateAppearance = Appearance13
        Appearance14.BackColor = System.Drawing.SystemColors.Window
        Appearance14.BorderColor = System.Drawing.Color.Silver
        Me.ugrdItems.DisplayLayout.Override.RowAppearance = Appearance14
        Me.ugrdItems.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[True]
        Me.ugrdItems.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed
        Me.ugrdItems.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.ugrdItems.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.ugrdItems.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Extended
        Appearance15.BackColor = System.Drawing.SystemColors.ControlLight
        Me.ugrdItems.DisplayLayout.Override.TemplateAddRowAppearance = Appearance15
        Me.ugrdItems.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.ugrdItems.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.ugrdItems.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand
        Me.ugrdItems.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        resources.ApplyResources(Me.ugrdItems, "ugrdItems")
        Me.ugrdItems.Name = "ugrdItems"
        '
        'txtDesc
        '
        Me.txtDesc.AcceptsReturn = True
        Me.txtDesc.BackColor = System.Drawing.SystemColors.Window
        Me.txtDesc.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.txtDesc, "txtDesc")
        Me.txtDesc.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDesc.Name = "txtDesc"
        Me.txtDesc.Tag = "String"
        '
        'txtVendor
        '
        Me.txtVendor.AcceptsReturn = True
        Me.txtVendor.BackColor = System.Drawing.SystemColors.Window
        Me.txtVendor.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.txtVendor, "txtVendor")
        Me.txtVendor.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtVendor.Name = "txtVendor"
        Me.txtVendor.Tag = "String"
        '
        'txtVendorID
        '
        Me.txtVendorID.AcceptsReturn = True
        Me.txtVendorID.BackColor = System.Drawing.SystemColors.Window
        Me.txtVendorID.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.txtVendorID, "txtVendorID")
        Me.txtVendorID.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtVendorID.Name = "txtVendorID"
        Me.txtVendorID.Tag = "String"
        '
        'txtIdentifier
        '
        Me.txtIdentifier.AcceptsReturn = True
        Me.txtIdentifier.BackColor = System.Drawing.SystemColors.Window
        Me.txtIdentifier.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.txtIdentifier, "txtIdentifier")
        Me.txtIdentifier.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtIdentifier.Name = "txtIdentifier"
        Me.txtIdentifier.Tag = "String"
        '
        '_lblLabel_3
        '
        Me._lblLabel_3.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_3.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lblLabel_3, "_lblLabel_3")
        Me._lblLabel_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_3, CType(3, Short))
        Me._lblLabel_3.Name = "_lblLabel_3"
        '
        '_lblLabel_8
        '
        Me._lblLabel_8.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_8.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lblLabel_8, "_lblLabel_8")
        Me._lblLabel_8.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_8, CType(8, Short))
        Me._lblLabel_8.Name = "_lblLabel_8"
        '
        '_lblLabel_9
        '
        Me._lblLabel_9.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_9.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lblLabel_9, "_lblLabel_9")
        Me._lblLabel_9.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_9, CType(9, Short))
        Me._lblLabel_9.Name = "_lblLabel_9"
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
        'txtStartScan
        '
        Me.txtStartScan.AcceptsReturn = True
        Me.txtStartScan.BackColor = System.Drawing.SystemColors.Window
        Me.txtStartScan.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.txtStartScan, "txtStartScan")
        Me.txtStartScan.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtStartScan.Name = "txtStartScan"
        Me.txtStartScan.Tag = "DateTime"
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
        '_lblLabel_4
        '
        Me._lblLabel_4.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_4.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lblLabel_4, "_lblLabel_4")
        Me._lblLabel_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_4, CType(4, Short))
        Me._lblLabel_4.Name = "_lblLabel_4"
        '
        'frmCycleCountEdit
        '
        Me.AcceptButton = Me.cmdSearch
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdExit
        Me.Controls.Add(Me.txtName)
        Me.Controls.Add(Me.txtType)
        Me.Controls.Add(Me.Frame2)
        Me.Controls.Add(Me.cmdReport)
        Me.Controls.Add(Me.Frame1)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.txtStartScan)
        Me.Controls.Add(Me.lblStore)
        Me.Controls.Add(Me._lblSubTeam_2)
        Me.Controls.Add(Me._lblLabel_4)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmCycleCountEdit"
        Me.Frame2.ResumeLayout(False)
        Me.Frame1.ResumeLayout(False)
        Me.Frame1.PerformLayout()
        CType(Me.ugrdItems, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblSubTeam, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ugrdItems As Infragistics.Win.UltraWinGrid.UltraGrid
#End Region 
End Class