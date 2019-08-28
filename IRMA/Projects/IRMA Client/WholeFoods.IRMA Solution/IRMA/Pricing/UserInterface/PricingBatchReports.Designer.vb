<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmPricingBatchReports
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
	Public WithEvents cmdExit As System.Windows.Forms.Button
	Public WithEvents cmdReport As System.Windows.Forms.Button
	Public WithEvents _optReport_2 As System.Windows.Forms.RadioButton
	Public WithEvents _optReport_1 As System.Windows.Forms.RadioButton
	Public WithEvents _optReport_0 As System.Windows.Forms.RadioButton
	Public WithEvents fraReport As System.Windows.Forms.GroupBox
	Public WithEvents cmbStatus As System.Windows.Forms.ComboBox
	Public WithEvents fraPriceType As System.Windows.Forms.GroupBox
	Public WithEvents _optType_0 As System.Windows.Forms.RadioButton
	Public WithEvents _optType_1 As System.Windows.Forms.RadioButton
	Public WithEvents _optType_2 As System.Windows.Forms.RadioButton
	Public WithEvents _optType_3 As System.Windows.Forms.RadioButton
	Public WithEvents _optType_4 As System.Windows.Forms.RadioButton
	Public WithEvents Frame3 As System.Windows.Forms.GroupBox
	Public WithEvents _optSelection_4 As System.Windows.Forms.RadioButton
	Public WithEvents _optSelection_3 As System.Windows.Forms.RadioButton
	Public WithEvents cmbZones As System.Windows.Forms.ComboBox
	Public WithEvents _optSelection_1 As System.Windows.Forms.RadioButton
	Public WithEvents _optSelection_5 As System.Windows.Forms.RadioButton
	Public WithEvents _optSelection_0 As System.Windows.Forms.RadioButton
	Public WithEvents _optSelection_2 As System.Windows.Forms.RadioButton
	Public WithEvents cmbState As System.Windows.Forms.ComboBox
	Public WithEvents fraStores As System.Windows.Forms.GroupBox
	Public WithEvents lblDates As System.Windows.Forms.Label
	Public WithEvents lblSubTeam As System.Windows.Forms.Label
	Public WithEvents lblStatus As System.Windows.Forms.Label
	Public WithEvents lblDash As System.Windows.Forms.Label
	Public WithEvents fraSelection As System.Windows.Forms.GroupBox
	Public WithEvents optReport As Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray
	Public WithEvents optSelection As Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray
	Public WithEvents optTagType As Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray
	Public WithEvents optType As Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray
	Public WithEvents txtDate As Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPricingBatchReports))
		Dim Appearance1 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
		Dim Appearance2 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
		Dim Appearance4 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
		Dim Appearance3 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
		Dim Appearance7 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
		Dim Appearance10 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
		Dim Appearance12 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
		Dim Appearance8 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
		Dim Appearance6 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
		Dim Appearance5 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
		Dim Appearance11 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
		Dim Appearance9 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cmdExit = New System.Windows.Forms.Button()
		Me.cmdReport = New System.Windows.Forms.Button()
		Me.fraReport = New System.Windows.Forms.GroupBox()
		Me._optReport_3 = New System.Windows.Forms.RadioButton()
		Me._optReport_2 = New System.Windows.Forms.RadioButton()
		Me._optReport_1 = New System.Windows.Forms.RadioButton()
		Me._optReport_0 = New System.Windows.Forms.RadioButton()
		Me.fraSelection = New System.Windows.Forms.GroupBox()
		Me.cmbSubTeam = New SubteamComboBox()
		Me.dtpEndDate = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor()
		Me.dtpStartDate = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor()
		Me.cmbStatus = New System.Windows.Forms.ComboBox()
		Me.fraPriceType = New System.Windows.Forms.GroupBox()
		Me.cmbPriceType = New System.Windows.Forms.ComboBox()
		Me.Frame3 = New System.Windows.Forms.GroupBox()
		Me._optType_0 = New System.Windows.Forms.RadioButton()
		Me._optType_1 = New System.Windows.Forms.RadioButton()
		Me._optType_2 = New System.Windows.Forms.RadioButton()
		Me._optType_3 = New System.Windows.Forms.RadioButton()
		Me._optType_4 = New System.Windows.Forms.RadioButton()
		Me.fraStores = New System.Windows.Forms.GroupBox()
		Me.ucmbStoreList = New Infragistics.Win.UltraWinGrid.UltraCombo()
		Me._optSelection_4 = New System.Windows.Forms.RadioButton()
		Me._optSelection_3 = New System.Windows.Forms.RadioButton()
		Me.cmbZones = New System.Windows.Forms.ComboBox()
		Me._optSelection_1 = New System.Windows.Forms.RadioButton()
		Me._optSelection_5 = New System.Windows.Forms.RadioButton()
		Me._optSelection_0 = New System.Windows.Forms.RadioButton()
		Me._optSelection_2 = New System.Windows.Forms.RadioButton()
		Me.cmbState = New System.Windows.Forms.ComboBox()
		Me.lblDates = New System.Windows.Forms.Label()
		Me.lblSubTeam = New System.Windows.Forms.Label()
		Me.lblStatus = New System.Windows.Forms.Label()
		Me.lblDash = New System.Windows.Forms.Label()
		Me.optReport = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(Me.components)
		Me.optSelection = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(Me.components)
		Me.optTagType = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(Me.components)
		Me.optType = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(Me.components)
		Me.txtDate = New Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray(Me.components)
		Me.fraReport.SuspendLayout()
		Me.fraSelection.SuspendLayout()
		CType(Me.dtpEndDate, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.dtpStartDate, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.fraPriceType.SuspendLayout()
		Me.Frame3.SuspendLayout()
		Me.fraStores.SuspendLayout()
		CType(Me.ucmbStoreList, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.optReport, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.optSelection, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.optTagType, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.optType, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtDate, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
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
		'fraReport
		'
		Me.fraReport.BackColor = System.Drawing.SystemColors.Control
		Me.fraReport.Controls.Add(Me._optReport_3)
		Me.fraReport.Controls.Add(Me._optReport_2)
		Me.fraReport.Controls.Add(Me._optReport_1)
		Me.fraReport.Controls.Add(Me._optReport_0)
		resources.ApplyResources(Me.fraReport, "fraReport")
		Me.fraReport.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraReport.Name = "fraReport"
		Me.fraReport.TabStop = False
		'
		'_optReport_3
		'
		Me._optReport_3.BackColor = System.Drawing.SystemColors.Control
		Me._optReport_3.Cursor = System.Windows.Forms.Cursors.Default
		resources.ApplyResources(Me._optReport_3, "_optReport_3")
		Me._optReport_3.ForeColor = System.Drawing.SystemColors.ControlText
		Me.optReport.SetIndex(Me._optReport_3, CType(3, Short))
		Me._optReport_3.Name = "_optReport_3"
		Me._optReport_3.TabStop = True
		Me._optReport_3.UseVisualStyleBackColor = False
		'
		'_optReport_2
		'
		Me._optReport_2.BackColor = System.Drawing.SystemColors.Control
		Me._optReport_2.Cursor = System.Windows.Forms.Cursors.Default
		resources.ApplyResources(Me._optReport_2, "_optReport_2")
		Me._optReport_2.ForeColor = System.Drawing.SystemColors.ControlText
		Me.optReport.SetIndex(Me._optReport_2, CType(2, Short))
		Me._optReport_2.Name = "_optReport_2"
		Me._optReport_2.TabStop = True
		Me._optReport_2.UseVisualStyleBackColor = False
		'
		'_optReport_1
		'
		Me._optReport_1.BackColor = System.Drawing.SystemColors.Control
		Me._optReport_1.Cursor = System.Windows.Forms.Cursors.Default
		resources.ApplyResources(Me._optReport_1, "_optReport_1")
		Me._optReport_1.ForeColor = System.Drawing.SystemColors.ControlText
		Me.optReport.SetIndex(Me._optReport_1, CType(1, Short))
		Me._optReport_1.Name = "_optReport_1"
		Me._optReport_1.TabStop = True
		Me._optReport_1.UseVisualStyleBackColor = False
		'
		'_optReport_0
		'
		Me._optReport_0.BackColor = System.Drawing.SystemColors.Control
		Me._optReport_0.Cursor = System.Windows.Forms.Cursors.Default
		resources.ApplyResources(Me._optReport_0, "_optReport_0")
		Me._optReport_0.ForeColor = System.Drawing.SystemColors.ControlText
		Me.optReport.SetIndex(Me._optReport_0, CType(0, Short))
		Me._optReport_0.Name = "_optReport_0"
		Me._optReport_0.TabStop = True
		Me._optReport_0.UseVisualStyleBackColor = False
		'
		'fraSelection
		'
		Me.fraSelection.BackColor = System.Drawing.SystemColors.Control
		Me.fraSelection.Controls.Add(Me.cmbSubTeam)
		Me.fraSelection.Controls.Add(Me.dtpEndDate)
		Me.fraSelection.Controls.Add(Me.dtpStartDate)
		Me.fraSelection.Controls.Add(Me.cmbStatus)
		Me.fraSelection.Controls.Add(Me.fraPriceType)
		Me.fraSelection.Controls.Add(Me.Frame3)
		Me.fraSelection.Controls.Add(Me.fraStores)
		Me.fraSelection.Controls.Add(Me.lblDates)
		Me.fraSelection.Controls.Add(Me.lblSubTeam)
		Me.fraSelection.Controls.Add(Me.lblStatus)
		Me.fraSelection.Controls.Add(Me.lblDash)
		resources.ApplyResources(Me.fraSelection, "fraSelection")
		Me.fraSelection.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraSelection.Name = "fraSelection"
		Me.fraSelection.TabStop = False
		'
		'cmbSubTeam
		'
		resources.ApplyResources(Me.cmbSubTeam, "cmbSubTeam")
		Me.cmbSubTeam.CheckboxFont = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
		Me.cmbSubTeam.CheckboxForeColor = System.Drawing.SystemColors.ControlText
		Me.cmbSubTeam.CheckboxText = "Show All"
		Me.cmbSubTeam.ClearSelectionVisisble = False
		Me.cmbSubTeam.DataSource = Nothing
		Me.cmbSubTeam.DisplayMember = "SubTeamName"
		Me.cmbSubTeam.DropDownWidth = 178
		Me.cmbSubTeam.HeaderFont = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
		Me.cmbSubTeam.HeaderForeColor = System.Drawing.SystemColors.ControlText
		Me.cmbSubTeam.HeaderText = "Caption"
		Me.cmbSubTeam.HeaderVisible = False
		Me.cmbSubTeam.IsShowAll = False
		Me.cmbSubTeam.Name = "cmbSubTeam"
		Me.cmbSubTeam.SelectedIndex = -1
		Me.cmbSubTeam.SelectedItem = Nothing
		Me.cmbSubTeam.SelectedText = ""
		Me.cmbSubTeam.SelectedValue = Nothing
		Me.cmbSubTeam.ValueMember = "SubTeamNo"
		'
		'dtpEndDate
		'
		resources.ApplyResources(Me.dtpEndDate, "dtpEndDate")
		Me.dtpEndDate.Name = "dtpEndDate"
		'
		'dtpStartDate
		'
		resources.ApplyResources(Me.dtpStartDate, "dtpStartDate")
		Me.dtpStartDate.Name = "dtpStartDate"
		'
		'cmbStatus
		'
		Me.cmbStatus.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
		Me.cmbStatus.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
		Me.cmbStatus.BackColor = System.Drawing.SystemColors.Window
		Me.cmbStatus.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		resources.ApplyResources(Me.cmbStatus, "cmbStatus")
		Me.cmbStatus.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cmbStatus.Name = "cmbStatus"
		Me.cmbStatus.Sorted = True
		'
		'fraPriceType
		'
		Me.fraPriceType.BackColor = System.Drawing.SystemColors.Control
		Me.fraPriceType.Controls.Add(Me.cmbPriceType)
		resources.ApplyResources(Me.fraPriceType, "fraPriceType")
		Me.fraPriceType.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraPriceType.Name = "fraPriceType"
		Me.fraPriceType.TabStop = False
		'
		'cmbPriceType
		'
		Me.cmbPriceType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
		Me.cmbPriceType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
		Me.cmbPriceType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		resources.ApplyResources(Me.cmbPriceType, "cmbPriceType")
		Me.cmbPriceType.Name = "cmbPriceType"
		'
		'Frame3
		'
		Me.Frame3.BackColor = System.Drawing.SystemColors.Control
		Me.Frame3.Controls.Add(Me._optType_0)
		Me.Frame3.Controls.Add(Me._optType_1)
		Me.Frame3.Controls.Add(Me._optType_2)
		Me.Frame3.Controls.Add(Me._optType_3)
		Me.Frame3.Controls.Add(Me._optType_4)
		resources.ApplyResources(Me.Frame3, "Frame3")
		Me.Frame3.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Frame3.Name = "Frame3"
		Me.Frame3.TabStop = False
		'
		'_optType_0
		'
		Me._optType_0.BackColor = System.Drawing.SystemColors.Control
		Me._optType_0.Cursor = System.Windows.Forms.Cursors.Default
		resources.ApplyResources(Me._optType_0, "_optType_0")
		Me._optType_0.ForeColor = System.Drawing.SystemColors.ControlText
		Me.optType.SetIndex(Me._optType_0, CType(0, Short))
		Me._optType_0.Name = "_optType_0"
		Me._optType_0.TabStop = True
		Me._optType_0.UseVisualStyleBackColor = False
		'
		'_optType_1
		'
		Me._optType_1.BackColor = System.Drawing.SystemColors.Control
		Me._optType_1.Cursor = System.Windows.Forms.Cursors.Default
		resources.ApplyResources(Me._optType_1, "_optType_1")
		Me._optType_1.ForeColor = System.Drawing.SystemColors.ControlText
		Me.optType.SetIndex(Me._optType_1, CType(1, Short))
		Me._optType_1.Name = "_optType_1"
		Me._optType_1.TabStop = True
		Me._optType_1.UseVisualStyleBackColor = False
		'
		'_optType_2
		'
		Me._optType_2.BackColor = System.Drawing.SystemColors.Control
		Me._optType_2.Cursor = System.Windows.Forms.Cursors.Default
		resources.ApplyResources(Me._optType_2, "_optType_2")
		Me._optType_2.ForeColor = System.Drawing.SystemColors.ControlText
		Me.optType.SetIndex(Me._optType_2, CType(2, Short))
		Me._optType_2.Name = "_optType_2"
		Me._optType_2.TabStop = True
		Me._optType_2.UseVisualStyleBackColor = False
		'
		'_optType_3
		'
		Me._optType_3.BackColor = System.Drawing.SystemColors.Control
		Me._optType_3.Cursor = System.Windows.Forms.Cursors.Default
		resources.ApplyResources(Me._optType_3, "_optType_3")
		Me._optType_3.ForeColor = System.Drawing.SystemColors.ControlText
		Me.optType.SetIndex(Me._optType_3, CType(3, Short))
		Me._optType_3.Name = "_optType_3"
		Me._optType_3.TabStop = True
		Me._optType_3.UseVisualStyleBackColor = False
		'
		'_optType_4
		'
		Me._optType_4.BackColor = System.Drawing.SystemColors.Control
		Me._optType_4.Checked = True
		Me._optType_4.Cursor = System.Windows.Forms.Cursors.Default
		resources.ApplyResources(Me._optType_4, "_optType_4")
		Me._optType_4.ForeColor = System.Drawing.SystemColors.ControlText
		Me.optType.SetIndex(Me._optType_4, CType(4, Short))
		Me._optType_4.Name = "_optType_4"
		Me._optType_4.TabStop = True
		Me._optType_4.UseVisualStyleBackColor = False
		'
		'fraStores
		'
		Me.fraStores.BackColor = System.Drawing.SystemColors.Control
		Me.fraStores.Controls.Add(Me.ucmbStoreList)
		Me.fraStores.Controls.Add(Me._optSelection_4)
		Me.fraStores.Controls.Add(Me._optSelection_3)
		Me.fraStores.Controls.Add(Me.cmbZones)
		Me.fraStores.Controls.Add(Me._optSelection_1)
		Me.fraStores.Controls.Add(Me._optSelection_5)
		Me.fraStores.Controls.Add(Me._optSelection_0)
		Me.fraStores.Controls.Add(Me._optSelection_2)
		Me.fraStores.Controls.Add(Me.cmbState)
		resources.ApplyResources(Me.fraStores, "fraStores")
		Me.fraStores.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraStores.Name = "fraStores"
		Me.fraStores.TabStop = False
		'
		'ucmbStoreList
		'
		Me.ucmbStoreList.CheckedListSettings.CheckStateMember = ""
		Appearance1.BackColor = System.Drawing.SystemColors.Window
		Appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption
		resources.ApplyResources(Appearance1.FontData, "Appearance1.FontData")
		resources.ApplyResources(Appearance1, "Appearance1")
		Appearance1.ForceApplyResources = "FontData|"
		Me.ucmbStoreList.DisplayLayout.Appearance = Appearance1
		Me.ucmbStoreList.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
		Me.ucmbStoreList.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
		Appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder
		Appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark
		Appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
		Appearance2.BorderColor = System.Drawing.SystemColors.Window
		resources.ApplyResources(Appearance2.FontData, "Appearance2.FontData")
		resources.ApplyResources(Appearance2, "Appearance2")
		Appearance2.ForceApplyResources = "FontData|"
		Me.ucmbStoreList.DisplayLayout.GroupByBox.Appearance = Appearance2
		Appearance4.ForeColor = System.Drawing.SystemColors.GrayText
		resources.ApplyResources(Appearance4.FontData, "Appearance4.FontData")
		resources.ApplyResources(Appearance4, "Appearance4")
		Appearance4.ForceApplyResources = "FontData|"
		Me.ucmbStoreList.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance4
		Me.ucmbStoreList.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
		Appearance3.BackColor = System.Drawing.SystemColors.ControlLightLight
		Appearance3.BackColor2 = System.Drawing.SystemColors.Control
		Appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
		Appearance3.ForeColor = System.Drawing.SystemColors.GrayText
		resources.ApplyResources(Appearance3.FontData, "Appearance3.FontData")
		resources.ApplyResources(Appearance3, "Appearance3")
		Appearance3.ForceApplyResources = "FontData|"
		Me.ucmbStoreList.DisplayLayout.GroupByBox.PromptAppearance = Appearance3
		Me.ucmbStoreList.DisplayLayout.MaxColScrollRegions = 1
		Me.ucmbStoreList.DisplayLayout.MaxRowScrollRegions = 1
		Appearance7.BackColor = System.Drawing.SystemColors.Window
		Appearance7.ForeColor = System.Drawing.SystemColors.ControlText
		resources.ApplyResources(Appearance7.FontData, "Appearance7.FontData")
		resources.ApplyResources(Appearance7, "Appearance7")
		Appearance7.ForceApplyResources = "FontData|"
		Me.ucmbStoreList.DisplayLayout.Override.ActiveCellAppearance = Appearance7
		Appearance10.BackColor = System.Drawing.SystemColors.Highlight
		Appearance10.ForeColor = System.Drawing.SystemColors.HighlightText
		resources.ApplyResources(Appearance10.FontData, "Appearance10.FontData")
		resources.ApplyResources(Appearance10, "Appearance10")
		Appearance10.ForceApplyResources = "FontData|"
		Me.ucmbStoreList.DisplayLayout.Override.ActiveRowAppearance = Appearance10
		Me.ucmbStoreList.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
		Me.ucmbStoreList.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
		Appearance12.BackColor = System.Drawing.SystemColors.Window
		resources.ApplyResources(Appearance12.FontData, "Appearance12.FontData")
		resources.ApplyResources(Appearance12, "Appearance12")
		Appearance12.ForceApplyResources = "FontData|"
		Me.ucmbStoreList.DisplayLayout.Override.CardAreaAppearance = Appearance12
		Appearance8.BorderColor = System.Drawing.Color.Silver
		Appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
		resources.ApplyResources(Appearance8.FontData, "Appearance8.FontData")
		resources.ApplyResources(Appearance8, "Appearance8")
		Appearance8.ForceApplyResources = "FontData|"
		Me.ucmbStoreList.DisplayLayout.Override.CellAppearance = Appearance8
		Me.ucmbStoreList.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
		Me.ucmbStoreList.DisplayLayout.Override.CellPadding = 0
		Appearance6.BackColor = System.Drawing.SystemColors.Control
		Appearance6.BackColor2 = System.Drawing.SystemColors.ControlDark
		Appearance6.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
		Appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
		Appearance6.BorderColor = System.Drawing.SystemColors.Window
		resources.ApplyResources(Appearance6.FontData, "Appearance6.FontData")
		resources.ApplyResources(Appearance6, "Appearance6")
		Appearance6.ForceApplyResources = "FontData|"
		Me.ucmbStoreList.DisplayLayout.Override.GroupByRowAppearance = Appearance6
		resources.ApplyResources(Appearance5, "Appearance5")
		resources.ApplyResources(Appearance5.FontData, "Appearance5.FontData")
		Appearance5.ForceApplyResources = "FontData|"
		Me.ucmbStoreList.DisplayLayout.Override.HeaderAppearance = Appearance5
		Me.ucmbStoreList.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti
		Me.ucmbStoreList.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
		Appearance11.BackColor = System.Drawing.SystemColors.Window
		Appearance11.BorderColor = System.Drawing.Color.Silver
		resources.ApplyResources(Appearance11.FontData, "Appearance11.FontData")
		resources.ApplyResources(Appearance11, "Appearance11")
		Appearance11.ForceApplyResources = "FontData|"
		Me.ucmbStoreList.DisplayLayout.Override.RowAppearance = Appearance11
		Me.ucmbStoreList.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[False]
		Appearance9.BackColor = System.Drawing.SystemColors.ControlLight
		resources.ApplyResources(Appearance9.FontData, "Appearance9.FontData")
		resources.ApplyResources(Appearance9, "Appearance9")
		Appearance9.ForceApplyResources = "FontData|"
		Me.ucmbStoreList.DisplayLayout.Override.TemplateAddRowAppearance = Appearance9
		Me.ucmbStoreList.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
		Me.ucmbStoreList.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
		Me.ucmbStoreList.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
		resources.ApplyResources(Me.ucmbStoreList, "ucmbStoreList")
		Me.ucmbStoreList.Name = "ucmbStoreList"
		'
		'_optSelection_4
		'
		Me._optSelection_4.BackColor = System.Drawing.SystemColors.Control
		Me._optSelection_4.Cursor = System.Windows.Forms.Cursors.Default
		resources.ApplyResources(Me._optSelection_4, "_optSelection_4")
		Me._optSelection_4.ForeColor = System.Drawing.SystemColors.ControlText
		Me.optSelection.SetIndex(Me._optSelection_4, CType(4, Short))
		Me._optSelection_4.Name = "_optSelection_4"
		Me._optSelection_4.TabStop = True
		Me._optSelection_4.UseVisualStyleBackColor = False
		'
		'_optSelection_3
		'
		Me._optSelection_3.BackColor = System.Drawing.SystemColors.Control
		Me._optSelection_3.Cursor = System.Windows.Forms.Cursors.Default
		resources.ApplyResources(Me._optSelection_3, "_optSelection_3")
		Me._optSelection_3.ForeColor = System.Drawing.SystemColors.ControlText
		Me.optSelection.SetIndex(Me._optSelection_3, CType(3, Short))
		Me._optSelection_3.Name = "_optSelection_3"
		Me._optSelection_3.TabStop = True
		Me._optSelection_3.UseVisualStyleBackColor = False
		'
		'cmbZones
		'
		Me.cmbZones.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
		Me.cmbZones.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
		Me.cmbZones.BackColor = System.Drawing.SystemColors.Window
		Me.cmbZones.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmbZones.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		resources.ApplyResources(Me.cmbZones, "cmbZones")
		Me.cmbZones.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cmbZones.Name = "cmbZones"
		Me.cmbZones.Sorted = True
		'
		'_optSelection_1
		'
		Me._optSelection_1.BackColor = System.Drawing.SystemColors.Control
		Me._optSelection_1.Cursor = System.Windows.Forms.Cursors.Default
		resources.ApplyResources(Me._optSelection_1, "_optSelection_1")
		Me._optSelection_1.ForeColor = System.Drawing.SystemColors.ControlText
		Me.optSelection.SetIndex(Me._optSelection_1, CType(1, Short))
		Me._optSelection_1.Name = "_optSelection_1"
		Me._optSelection_1.TabStop = True
		Me._optSelection_1.UseVisualStyleBackColor = False
		'
		'_optSelection_5
		'
		Me._optSelection_5.BackColor = System.Drawing.SystemColors.Control
		Me._optSelection_5.Cursor = System.Windows.Forms.Cursors.Default
		resources.ApplyResources(Me._optSelection_5, "_optSelection_5")
		Me._optSelection_5.ForeColor = System.Drawing.SystemColors.ControlText
		Me.optSelection.SetIndex(Me._optSelection_5, CType(5, Short))
		Me._optSelection_5.Name = "_optSelection_5"
		Me._optSelection_5.TabStop = True
		Me._optSelection_5.UseVisualStyleBackColor = False
		'
		'_optSelection_0
		'
		Me._optSelection_0.BackColor = System.Drawing.SystemColors.Control
		Me._optSelection_0.Checked = True
		Me._optSelection_0.Cursor = System.Windows.Forms.Cursors.Default
		resources.ApplyResources(Me._optSelection_0, "_optSelection_0")
		Me._optSelection_0.ForeColor = System.Drawing.SystemColors.ControlText
		Me.optSelection.SetIndex(Me._optSelection_0, CType(0, Short))
		Me._optSelection_0.Name = "_optSelection_0"
		Me._optSelection_0.TabStop = True
		Me._optSelection_0.UseVisualStyleBackColor = False
		'
		'_optSelection_2
		'
		Me._optSelection_2.BackColor = System.Drawing.SystemColors.Control
		Me._optSelection_2.Cursor = System.Windows.Forms.Cursors.Default
		resources.ApplyResources(Me._optSelection_2, "_optSelection_2")
		Me._optSelection_2.ForeColor = System.Drawing.SystemColors.ControlText
		Me.optSelection.SetIndex(Me._optSelection_2, CType(2, Short))
		Me._optSelection_2.Name = "_optSelection_2"
		Me._optSelection_2.TabStop = True
		Me._optSelection_2.UseVisualStyleBackColor = False
		'
		'cmbState
		'
		Me.cmbState.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
		Me.cmbState.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
		Me.cmbState.BackColor = System.Drawing.SystemColors.Window
		Me.cmbState.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmbState.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		resources.ApplyResources(Me.cmbState, "cmbState")
		Me.cmbState.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cmbState.Name = "cmbState"
		Me.cmbState.Sorted = True
		'
		'lblDates
		'
		Me.lblDates.BackColor = System.Drawing.Color.Transparent
		Me.lblDates.Cursor = System.Windows.Forms.Cursors.Default
		resources.ApplyResources(Me.lblDates, "lblDates")
		Me.lblDates.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblDates.Name = "lblDates"
		'
		'lblSubTeam
		'
		Me.lblSubTeam.BackColor = System.Drawing.Color.Transparent
		Me.lblSubTeam.Cursor = System.Windows.Forms.Cursors.Default
		resources.ApplyResources(Me.lblSubTeam, "lblSubTeam")
		Me.lblSubTeam.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblSubTeam.Name = "lblSubTeam"
		'
		'lblStatus
		'
		Me.lblStatus.BackColor = System.Drawing.Color.Transparent
		Me.lblStatus.Cursor = System.Windows.Forms.Cursors.Default
		resources.ApplyResources(Me.lblStatus, "lblStatus")
		Me.lblStatus.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblStatus.Name = "lblStatus"
		'
		'lblDash
		'
		Me.lblDash.BackColor = System.Drawing.Color.Transparent
		Me.lblDash.Cursor = System.Windows.Forms.Cursors.Default
		resources.ApplyResources(Me.lblDash, "lblDash")
		Me.lblDash.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblDash.Name = "lblDash"
		'
		'optReport
		'
		'
		'optSelection
		'
		'
		'frmPricingBatchReports
		'
		Me.AcceptButton = Me.cmdReport
		resources.ApplyResources(Me, "$this")
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.CancelButton = Me.cmdExit
		Me.Controls.Add(Me.cmdExit)
		Me.Controls.Add(Me.cmdReport)
		Me.Controls.Add(Me.fraReport)
		Me.Controls.Add(Me.fraSelection)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmPricingBatchReports"
		Me.ShowInTaskbar = False
		Me.fraReport.ResumeLayout(False)
		Me.fraSelection.ResumeLayout(False)
		Me.fraSelection.PerformLayout()
		CType(Me.dtpEndDate, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.dtpStartDate, System.ComponentModel.ISupportInitialize).EndInit()
		Me.fraPriceType.ResumeLayout(False)
		Me.Frame3.ResumeLayout(False)
		Me.fraStores.ResumeLayout(False)
		Me.fraStores.PerformLayout()
		CType(Me.ucmbStoreList, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.optReport, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.optSelection, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.optTagType, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.optType, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtDate, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)

	End Sub
	Friend WithEvents dtpStartDate As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
	Friend WithEvents dtpEndDate As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
	Public WithEvents cmbPriceType As System.Windows.Forms.ComboBox
	Public WithEvents _optReport_3 As System.Windows.Forms.RadioButton
	Friend WithEvents ucmbStoreList As Infragistics.Win.UltraWinGrid.UltraCombo
	Friend WithEvents cmbSubTeam As SubteamComboBox
#End Region
End Class