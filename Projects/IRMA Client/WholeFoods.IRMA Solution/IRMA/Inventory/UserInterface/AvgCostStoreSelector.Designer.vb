<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class AvgCostStoreSelector
#Region "Windows Form Designer generated code "
    <System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
        MyBase.New()
        'This call is required by the Windows Form Designer.
        Me.IsInitializing = True
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
    'Public WithEvents CrystalReport1 As AxCrystal.AxCrystalReport
    Public WithEvents cmdExit As System.Windows.Forms.Button
    Public WithEvents cmdSelect As System.Windows.Forms.Button
    Public WithEvents lblCost As System.Windows.Forms.Label
    Public WithEvents Frame1 As Microsoft.VisualBasic.Compatibility.VB6.GroupBoxArray
    Public WithEvents optSelection As Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AvgCostStoreSelector))
        Dim Appearance1 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridBand1 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1)
        Dim UltraGridColumn1 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Zone_Id")
        Dim UltraGridColumn2 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Zone_Name")
        Dim UltraGridColumn3 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Store_Name")
        Dim UltraGridColumn4 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Mega_Store")
        Dim UltraGridColumn5 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("WFM_Store")
        Dim UltraGridColumn6 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Store_No")
        Dim UltraGridColumn7 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Region_Id")
        Dim UltraGridColumn8 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("State", 0)
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
        Dim Appearance14 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdExit = New System.Windows.Forms.Button()
        Me.cmdSelect = New System.Windows.Forms.Button()
        Me.AllWFMRadioButton = New System.Windows.Forms.RadioButton()
        Me.StateRadioButton = New System.Windows.Forms.RadioButton()
        Me.ZoneRadioButton = New System.Windows.Forms.RadioButton()
        Me.AllRadioButton = New System.Windows.Forms.RadioButton()
        Me.ManualRadioButton = New System.Windows.Forms.RadioButton()
        Me.lblCost = New System.Windows.Forms.Label()
        Me.Frame1 = New Microsoft.VisualBasic.Compatibility.VB6.GroupBoxArray(Me.components)
        Me.optSelection = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(Me.components)
        Me.ugrdStoreList = New Infragistics.Win.UltraWinGrid.UltraGrid()
        Me._Frame1_1 = New System.Windows.Forms.GroupBox()
        Me.cmbStates = New System.Windows.Forms.ComboBox()
        Me.cmbZones = New System.Windows.Forms.ComboBox()
        CType(Me.Frame1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.optSelection, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ugrdStoreList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._Frame1_1.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdExit
        '
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdExit, "cmdExit")
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Name = "cmdExit"
        Me.ToolTip1.SetToolTip(Me.cmdExit, resources.GetString("cmdExit.ToolTip"))
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'cmdSelect
        '
        Me.cmdSelect.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSelect.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdSelect, "cmdSelect")
        Me.cmdSelect.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSelect.Name = "cmdSelect"
        Me.ToolTip1.SetToolTip(Me.cmdSelect, resources.GetString("cmdSelect.ToolTip"))
        Me.cmdSelect.UseVisualStyleBackColor = False
        '
        'AllWFMRadioButton
        '
        Me.AllWFMRadioButton.BackColor = System.Drawing.SystemColors.Control
        Me.AllWFMRadioButton.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.AllWFMRadioButton, "AllWFMRadioButton")
        Me.AllWFMRadioButton.ForeColor = System.Drawing.SystemColors.ControlText
        Me.AllWFMRadioButton.Name = "AllWFMRadioButton"
        Me.AllWFMRadioButton.TabStop = True
        Me.ToolTip1.SetToolTip(Me.AllWFMRadioButton, resources.GetString("AllWFMRadioButton.ToolTip"))
        Me.AllWFMRadioButton.UseVisualStyleBackColor = False
        '
        'StateRadioButton
        '
        Me.StateRadioButton.BackColor = System.Drawing.SystemColors.Control
        Me.StateRadioButton.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.StateRadioButton, "StateRadioButton")
        Me.StateRadioButton.ForeColor = System.Drawing.SystemColors.ControlText
        Me.StateRadioButton.Name = "StateRadioButton"
        Me.StateRadioButton.TabStop = True
        Me.ToolTip1.SetToolTip(Me.StateRadioButton, resources.GetString("StateRadioButton.ToolTip"))
        Me.StateRadioButton.UseVisualStyleBackColor = False
        '
        'ZoneRadioButton
        '
        Me.ZoneRadioButton.BackColor = System.Drawing.SystemColors.Control
        Me.ZoneRadioButton.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.ZoneRadioButton, "ZoneRadioButton")
        Me.ZoneRadioButton.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ZoneRadioButton.Name = "ZoneRadioButton"
        Me.ZoneRadioButton.TabStop = True
        Me.ToolTip1.SetToolTip(Me.ZoneRadioButton, resources.GetString("ZoneRadioButton.ToolTip"))
        Me.ZoneRadioButton.UseVisualStyleBackColor = False
        '
        'AllRadioButton
        '
        Me.AllRadioButton.BackColor = System.Drawing.SystemColors.Control
        Me.AllRadioButton.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.AllRadioButton, "AllRadioButton")
        Me.AllRadioButton.ForeColor = System.Drawing.SystemColors.ControlText
        Me.AllRadioButton.Name = "AllRadioButton"
        Me.AllRadioButton.TabStop = True
        Me.ToolTip1.SetToolTip(Me.AllRadioButton, resources.GetString("AllRadioButton.ToolTip"))
        Me.AllRadioButton.UseVisualStyleBackColor = False
        '
        'ManualRadioButton
        '
        Me.ManualRadioButton.BackColor = System.Drawing.SystemColors.Control
        Me.ManualRadioButton.Checked = True
        Me.ManualRadioButton.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.ManualRadioButton, "ManualRadioButton")
        Me.ManualRadioButton.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ManualRadioButton.Name = "ManualRadioButton"
        Me.ManualRadioButton.TabStop = True
        Me.ToolTip1.SetToolTip(Me.ManualRadioButton, resources.GetString("ManualRadioButton.ToolTip"))
        Me.ManualRadioButton.UseVisualStyleBackColor = False
        '
        'lblCost
        '
        Me.lblCost.BackColor = System.Drawing.SystemColors.Control
        Me.lblCost.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblCost, "lblCost")
        Me.lblCost.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCost.Name = "lblCost"
        '
        'ugrdStoreList
        '
        Appearance1.BackColor = System.Drawing.SystemColors.ControlLight
        Appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption
        resources.ApplyResources(Appearance1.FontData, "Appearance1.FontData")
        resources.ApplyResources(Appearance1, "Appearance1")
        Appearance1.ForceApplyResources = "FontData|"
        Me.ugrdStoreList.DisplayLayout.Appearance = Appearance1
        Me.ugrdStoreList.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        UltraGridColumn1.Header.VisiblePosition = 0
        UltraGridColumn1.Hidden = True
        UltraGridColumn1.Width = 24
        UltraGridColumn2.Header.VisiblePosition = 1
        UltraGridColumn2.Hidden = True
        UltraGridColumn2.Width = 17
        UltraGridColumn3.Header.Caption = resources.GetString("resource.Caption")
        UltraGridColumn3.Header.VisiblePosition = 2
        UltraGridColumn3.Width = 373
        UltraGridColumn4.Header.VisiblePosition = 3
        UltraGridColumn4.Hidden = True
        UltraGridColumn4.Width = 28
        UltraGridColumn5.Header.VisiblePosition = 4
        UltraGridColumn5.Hidden = True
        UltraGridColumn5.Width = 47
        UltraGridColumn6.Header.VisiblePosition = 5
        UltraGridColumn6.Hidden = True
        UltraGridColumn6.Width = 70
        UltraGridColumn7.Header.VisiblePosition = 6
        UltraGridColumn7.Hidden = True
        UltraGridColumn7.Width = 103
        UltraGridColumn8.Header.VisiblePosition = 7
        UltraGridColumn8.Hidden = True
        UltraGridColumn8.Width = 116
        UltraGridBand1.Columns.AddRange(New Object() {UltraGridColumn1, UltraGridColumn2, UltraGridColumn3, UltraGridColumn4, UltraGridColumn5, UltraGridColumn6, UltraGridColumn7, UltraGridColumn8})
        Me.ugrdStoreList.DisplayLayout.BandsSerializer.Add(UltraGridBand1)
        Me.ugrdStoreList.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Appearance2.FontData.BoldAsString = resources.GetString("resource.BoldAsString")
        resources.ApplyResources(Appearance2, "Appearance2")
        Appearance2.ForceApplyResources = ""
        Me.ugrdStoreList.DisplayLayout.CaptionAppearance = Appearance2
        Me.ugrdStoreList.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Appearance3.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance3.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance3.BorderColor = System.Drawing.SystemColors.Window
        resources.ApplyResources(Appearance3.FontData, "Appearance3.FontData")
        resources.ApplyResources(Appearance3, "Appearance3")
        Appearance3.ForceApplyResources = "FontData|"
        Me.ugrdStoreList.DisplayLayout.GroupByBox.Appearance = Appearance3
        Appearance4.ForeColor = System.Drawing.SystemColors.GrayText
        resources.ApplyResources(Appearance4.FontData, "Appearance4.FontData")
        resources.ApplyResources(Appearance4, "Appearance4")
        Appearance4.ForceApplyResources = "FontData|"
        Me.ugrdStoreList.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance4
        Me.ugrdStoreList.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.ugrdStoreList.DisplayLayout.GroupByBox.Hidden = True
        Appearance5.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance5.BackColor2 = System.Drawing.SystemColors.Control
        Appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance5.ForeColor = System.Drawing.SystemColors.GrayText
        resources.ApplyResources(Appearance5.FontData, "Appearance5.FontData")
        resources.ApplyResources(Appearance5, "Appearance5")
        Appearance5.ForceApplyResources = "FontData|"
        Me.ugrdStoreList.DisplayLayout.GroupByBox.PromptAppearance = Appearance5
        Me.ugrdStoreList.DisplayLayout.MaxColScrollRegions = 1
        Me.ugrdStoreList.DisplayLayout.MaxRowScrollRegions = 1
        Appearance6.BackColor = System.Drawing.SystemColors.Window
        Appearance6.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Appearance6.FontData, "Appearance6.FontData")
        resources.ApplyResources(Appearance6, "Appearance6")
        Appearance6.ForceApplyResources = "FontData|"
        Me.ugrdStoreList.DisplayLayout.Override.ActiveCellAppearance = Appearance6
        Me.ugrdStoreList.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No
        Me.ugrdStoreList.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.[False]
        Me.ugrdStoreList.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.[False]
        Me.ugrdStoreList.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.ugrdStoreList.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance7.BackColor = System.Drawing.SystemColors.Window
        resources.ApplyResources(Appearance7.FontData, "Appearance7.FontData")
        resources.ApplyResources(Appearance7, "Appearance7")
        Appearance7.ForceApplyResources = "FontData|"
        Me.ugrdStoreList.DisplayLayout.Override.CardAreaAppearance = Appearance7
        Appearance8.BorderColor = System.Drawing.Color.Silver
        Appearance8.FontData.BoldAsString = resources.GetString("resource.BoldAsString1")
        Appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        resources.ApplyResources(Appearance8, "Appearance8")
        Appearance8.ForceApplyResources = ""
        Me.ugrdStoreList.DisplayLayout.Override.CellAppearance = Appearance8
        Me.ugrdStoreList.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        Me.ugrdStoreList.DisplayLayout.Override.CellPadding = 0
        Appearance9.FontData.BoldAsString = resources.GetString("resource.BoldAsString2")
        resources.ApplyResources(Appearance9, "Appearance9")
        Appearance9.ForceApplyResources = ""
        Me.ugrdStoreList.DisplayLayout.Override.FixedHeaderAppearance = Appearance9
        Appearance10.BackColor = System.Drawing.SystemColors.Control
        Appearance10.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance10.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance10.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance10.BorderColor = System.Drawing.SystemColors.Window
        resources.ApplyResources(Appearance10.FontData, "Appearance10.FontData")
        resources.ApplyResources(Appearance10, "Appearance10")
        Appearance10.ForceApplyResources = "FontData|"
        Me.ugrdStoreList.DisplayLayout.Override.GroupByRowAppearance = Appearance10
        Appearance11.FontData.BoldAsString = resources.GetString("resource.BoldAsString3")
        resources.ApplyResources(Appearance11, "Appearance11")
        Appearance11.ForceApplyResources = ""
        Me.ugrdStoreList.DisplayLayout.Override.HeaderAppearance = Appearance11
        Me.ugrdStoreList.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle
        Me.ugrdStoreList.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance12.BackColor = System.Drawing.SystemColors.ControlLight
        resources.ApplyResources(Appearance12.FontData, "Appearance12.FontData")
        resources.ApplyResources(Appearance12, "Appearance12")
        Appearance12.ForceApplyResources = "FontData|"
        Me.ugrdStoreList.DisplayLayout.Override.RowAlternateAppearance = Appearance12
        Appearance13.BackColor = System.Drawing.SystemColors.Window
        Appearance13.BorderColor = System.Drawing.Color.Silver
        resources.ApplyResources(Appearance13.FontData, "Appearance13.FontData")
        resources.ApplyResources(Appearance13, "Appearance13")
        Appearance13.ForceApplyResources = "FontData|"
        Me.ugrdStoreList.DisplayLayout.Override.RowAppearance = Appearance13
        Me.ugrdStoreList.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[True]
        Me.ugrdStoreList.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed
        Me.ugrdStoreList.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.ugrdStoreList.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.ugrdStoreList.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Extended
        Appearance14.BackColor = System.Drawing.SystemColors.ControlLight
        resources.ApplyResources(Appearance14.FontData, "Appearance14.FontData")
        resources.ApplyResources(Appearance14, "Appearance14")
        Appearance14.ForceApplyResources = "FontData|"
        Me.ugrdStoreList.DisplayLayout.Override.TemplateAddRowAppearance = Appearance14
        Me.ugrdStoreList.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.ugrdStoreList.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.ugrdStoreList.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand
        Me.ugrdStoreList.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        resources.ApplyResources(Me.ugrdStoreList, "ugrdStoreList")
        Me.ugrdStoreList.Name = "ugrdStoreList"
        '
        '_Frame1_1
        '
        Me._Frame1_1.BackColor = System.Drawing.SystemColors.Control
        Me._Frame1_1.Controls.Add(Me.cmbStates)
        Me._Frame1_1.Controls.Add(Me.AllWFMRadioButton)
        Me._Frame1_1.Controls.Add(Me.StateRadioButton)
        Me._Frame1_1.Controls.Add(Me.cmbZones)
        Me._Frame1_1.Controls.Add(Me.ZoneRadioButton)
        Me._Frame1_1.Controls.Add(Me.AllRadioButton)
        Me._Frame1_1.Controls.Add(Me.ManualRadioButton)
        resources.ApplyResources(Me._Frame1_1, "_Frame1_1")
        Me._Frame1_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._Frame1_1.Name = "_Frame1_1"
        Me._Frame1_1.TabStop = False
        '
        'cmbStates
        '
        Me.cmbStates.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cmbStates.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbStates.BackColor = System.Drawing.SystemColors.Window
        Me.cmbStates.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbStates.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.cmbStates, "cmbStates")
        Me.cmbStates.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbStates.Name = "cmbStates"
        Me.cmbStates.Sorted = True
        '
        'cmbZones
        '
        Me.cmbZones.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cmbZones.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbZones.BackColor = System.Drawing.SystemColors.Window
        Me.cmbZones.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbZones.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.cmbZones, "cmbZones")
        Me.cmbZones.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbZones.Name = "cmbZones"
        Me.cmbZones.Sorted = True
        '
        'AvgCostStoreSelector
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.Controls.Add(Me.ugrdStoreList)
        Me.Controls.Add(Me._Frame1_1)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.cmdSelect)
        Me.Controls.Add(Me.lblCost)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "AvgCostStoreSelector"
        Me.ShowInTaskbar = False
        CType(Me.Frame1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.optSelection, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ugrdStoreList, System.ComponentModel.ISupportInitialize).EndInit()
        Me._Frame1_1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ugrdStoreList As Infragistics.Win.UltraWinGrid.UltraGrid
    Public WithEvents _Frame1_1 As System.Windows.Forms.GroupBox
    Public WithEvents cmbStates As System.Windows.Forms.ComboBox
    Public WithEvents AllWFMRadioButton As System.Windows.Forms.RadioButton
    Public WithEvents StateRadioButton As System.Windows.Forms.RadioButton
    Public WithEvents cmbZones As System.Windows.Forms.ComboBox
    Public WithEvents ZoneRadioButton As System.Windows.Forms.RadioButton
    Public WithEvents AllRadioButton As System.Windows.Forms.RadioButton
    Public WithEvents ManualRadioButton As System.Windows.Forms.RadioButton
#End Region
End Class