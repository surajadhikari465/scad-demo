<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmItemVendorStore
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
	Public WithEvents _optSelection_0 As System.Windows.Forms.RadioButton
	Public WithEvents _optSelection_1 As System.Windows.Forms.RadioButton
	Public WithEvents _optSelection_2 As System.Windows.Forms.RadioButton
	Public WithEvents cmbZones As System.Windows.Forms.ComboBox
	Public WithEvents _optSelection_3 As System.Windows.Forms.RadioButton
	Public WithEvents _optSelection_4 As System.Windows.Forms.RadioButton
	Public WithEvents cmbStates As System.Windows.Forms.ComboBox
	Public WithEvents _optSelection_5 As System.Windows.Forms.RadioButton
	Public WithEvents _optSelection_6 As System.Windows.Forms.RadioButton
	Public WithEvents Frame1 As System.Windows.Forms.GroupBox
	Public WithEvents chkPrimary As System.Windows.Forms.CheckBox
	Public WithEvents Frame2 As System.Windows.Forms.GroupBox
	Public WithEvents cmdExit As System.Windows.Forms.Button
	Public WithEvents cmdApply As System.Windows.Forms.Button
    Public WithEvents optSelection As Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmItemVendorStore))
        Dim Appearance1 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim UltraGridBand1 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1)
        Dim UltraGridColumn1 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Store_No")
        Dim UltraGridColumn2 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Store_Name")
        Dim UltraGridColumn3 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Zone_ID")
        Dim UltraGridColumn4 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("State")
        Dim UltraGridColumn5 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("WFM_Store")
        Dim UltraGridColumn6 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Mega_Store")
        Dim UltraGridColumn7 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("CustomerType")
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
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me._optSelection_0 = New System.Windows.Forms.RadioButton
        Me._optSelection_1 = New System.Windows.Forms.RadioButton
        Me._optSelection_2 = New System.Windows.Forms.RadioButton
        Me._optSelection_3 = New System.Windows.Forms.RadioButton
        Me._optSelection_4 = New System.Windows.Forms.RadioButton
        Me._optSelection_5 = New System.Windows.Forms.RadioButton
        Me._optSelection_6 = New System.Windows.Forms.RadioButton
        Me.chkPrimary = New System.Windows.Forms.CheckBox
        Me.cmdExit = New System.Windows.Forms.Button
        Me.cmdApply = New System.Windows.Forms.Button
        Me.Frame1 = New System.Windows.Forms.GroupBox
        Me.cmbZones = New System.Windows.Forms.ComboBox
        Me.cmbStates = New System.Windows.Forms.ComboBox
        Me.Frame2 = New System.Windows.Forms.GroupBox
        Me.optSelection = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(Me.components)
        Me.ugrdStoreList = New Infragistics.Win.UltraWinGrid.UltraGrid
        Me.Frame1.SuspendLayout()
        Me.Frame2.SuspendLayout()
        CType(Me.optSelection, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ugrdStoreList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
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
        Me.ToolTip1.SetToolTip(Me._optSelection_0, resources.GetString("_optSelection_0.ToolTip"))
        Me._optSelection_0.UseVisualStyleBackColor = False
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
        Me.ToolTip1.SetToolTip(Me._optSelection_1, resources.GetString("_optSelection_1.ToolTip"))
        Me._optSelection_1.UseVisualStyleBackColor = False
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
        Me.ToolTip1.SetToolTip(Me._optSelection_2, resources.GetString("_optSelection_2.ToolTip"))
        Me._optSelection_2.UseVisualStyleBackColor = False
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
        Me.ToolTip1.SetToolTip(Me._optSelection_3, resources.GetString("_optSelection_3.ToolTip"))
        Me._optSelection_3.UseVisualStyleBackColor = False
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
        Me.ToolTip1.SetToolTip(Me._optSelection_4, resources.GetString("_optSelection_4.ToolTip"))
        Me._optSelection_4.UseVisualStyleBackColor = False
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
        Me.ToolTip1.SetToolTip(Me._optSelection_5, resources.GetString("_optSelection_5.ToolTip"))
        Me._optSelection_5.UseVisualStyleBackColor = False
        '
        '_optSelection_6
        '
        Me._optSelection_6.BackColor = System.Drawing.SystemColors.Control
        Me._optSelection_6.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._optSelection_6, "_optSelection_6")
        Me._optSelection_6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optSelection.SetIndex(Me._optSelection_6, CType(6, Short))
        Me._optSelection_6.Name = "_optSelection_6"
        Me._optSelection_6.TabStop = True
        Me.ToolTip1.SetToolTip(Me._optSelection_6, resources.GetString("_optSelection_6.ToolTip"))
        Me._optSelection_6.UseVisualStyleBackColor = False
        '
        'chkPrimary
        '
        Me.chkPrimary.BackColor = System.Drawing.SystemColors.Control
        resources.ApplyResources(Me.chkPrimary, "chkPrimary")
        Me.chkPrimary.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkPrimary.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkPrimary.Name = "chkPrimary"
        Me.ToolTip1.SetToolTip(Me.chkPrimary, resources.GetString("chkPrimary.ToolTip"))
        Me.chkPrimary.UseVisualStyleBackColor = False
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
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me._optSelection_0)
        Me.Frame1.Controls.Add(Me._optSelection_1)
        Me.Frame1.Controls.Add(Me._optSelection_2)
        Me.Frame1.Controls.Add(Me.cmbZones)
        Me.Frame1.Controls.Add(Me._optSelection_3)
        Me.Frame1.Controls.Add(Me._optSelection_4)
        Me.Frame1.Controls.Add(Me.cmbStates)
        Me.Frame1.Controls.Add(Me._optSelection_5)
        Me.Frame1.Controls.Add(Me._optSelection_6)
        resources.ApplyResources(Me.Frame1, "Frame1")
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Name = "Frame1"
        Me.Frame1.TabStop = False
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
        'cmbStates
        '
        Me.cmbStates.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbStates.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbStates.BackColor = System.Drawing.SystemColors.Window
        Me.cmbStates.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbStates.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.cmbStates, "cmbStates")
        Me.cmbStates.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbStates.Name = "cmbStates"
        Me.cmbStates.Sorted = True
        '
        'Frame2
        '
        Me.Frame2.BackColor = System.Drawing.SystemColors.Control
        Me.Frame2.Controls.Add(Me.chkPrimary)
        resources.ApplyResources(Me.Frame2, "Frame2")
        Me.Frame2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame2.Name = "Frame2"
        Me.Frame2.TabStop = False
        '
        'optSelection
        '
        '
        'ugrdStoreList
        '
        Appearance1.BackColor = System.Drawing.SystemColors.ControlLight
        Appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.ugrdStoreList.DisplayLayout.Appearance = Appearance1
        Me.ugrdStoreList.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn
        UltraGridColumn1.Header.VisiblePosition = 0
        UltraGridColumn1.Hidden = True
        UltraGridColumn2.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn2.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn2.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn2.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn2.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn2.Header.Caption = resources.GetString("resource.Caption")
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
        UltraGridBand1.RowLayoutStyle = Infragistics.Win.UltraWinGrid.RowLayoutStyle.None
        Me.ugrdStoreList.DisplayLayout.BandsSerializer.Add(UltraGridBand1)
        Me.ugrdStoreList.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Appearance2.FontData.BoldAsString = resources.GetString("resource.BoldAsString")
        Me.ugrdStoreList.DisplayLayout.CaptionAppearance = Appearance2
        Me.ugrdStoreList.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[True]
        Appearance3.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance3.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance3.BorderColor = System.Drawing.SystemColors.Window
        Me.ugrdStoreList.DisplayLayout.GroupByBox.Appearance = Appearance3
        Appearance4.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugrdStoreList.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance4
        Me.ugrdStoreList.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.ugrdStoreList.DisplayLayout.GroupByBox.Hidden = True
        Appearance5.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance5.BackColor2 = System.Drawing.SystemColors.Control
        Appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance5.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugrdStoreList.DisplayLayout.GroupByBox.PromptAppearance = Appearance5
        Me.ugrdStoreList.DisplayLayout.MaxColScrollRegions = 1
        Me.ugrdStoreList.DisplayLayout.MaxRowScrollRegions = 1
        Appearance6.BackColor = System.Drawing.SystemColors.Window
        Appearance6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ugrdStoreList.DisplayLayout.Override.ActiveCellAppearance = Appearance6
        Me.ugrdStoreList.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.ugrdStoreList.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance7.BackColor = System.Drawing.SystemColors.Window
        Me.ugrdStoreList.DisplayLayout.Override.CardAreaAppearance = Appearance7
        Appearance8.BorderColor = System.Drawing.Color.Silver
        Appearance8.FontData.BoldAsString = resources.GetString("resource.BoldAsString1")
        Appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.ugrdStoreList.DisplayLayout.Override.CellAppearance = Appearance8
        Me.ugrdStoreList.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.ugrdStoreList.DisplayLayout.Override.CellPadding = 0
        Appearance9.FontData.BoldAsString = resources.GetString("resource.BoldAsString2")
        Me.ugrdStoreList.DisplayLayout.Override.FixedHeaderAppearance = Appearance9
        Appearance10.BackColor = System.Drawing.SystemColors.Control
        Appearance10.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance10.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance10.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance10.BorderColor = System.Drawing.SystemColors.Window
        Me.ugrdStoreList.DisplayLayout.Override.GroupByRowAppearance = Appearance10
        Appearance11.FontData.BoldAsString = resources.GetString("resource.BoldAsString3")
        Appearance11.TextHAlign = Infragistics.Win.HAlign.Left
        Me.ugrdStoreList.DisplayLayout.Override.HeaderAppearance = Appearance11
        Me.ugrdStoreList.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle
        Me.ugrdStoreList.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance12.BackColor = System.Drawing.SystemColors.ControlLight
        Me.ugrdStoreList.DisplayLayout.Override.RowAlternateAppearance = Appearance12
        Appearance13.BackColor = System.Drawing.SystemColors.Window
        Appearance13.BorderColor = System.Drawing.Color.Silver
        Me.ugrdStoreList.DisplayLayout.Override.RowAppearance = Appearance13
        Me.ugrdStoreList.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[True]
        Me.ugrdStoreList.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed
        Me.ugrdStoreList.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.ugrdStoreList.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.ugrdStoreList.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Extended
        Appearance14.BackColor = System.Drawing.SystemColors.ControlLight
        Me.ugrdStoreList.DisplayLayout.Override.TemplateAddRowAppearance = Appearance14
        Me.ugrdStoreList.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.ugrdStoreList.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.ugrdStoreList.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand
        Me.ugrdStoreList.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        resources.ApplyResources(Me.ugrdStoreList, "ugrdStoreList")
        Me.ugrdStoreList.Name = "ugrdStoreList"
        '
        'frmItemVendorStore
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.Controls.Add(Me.ugrdStoreList)
        Me.Controls.Add(Me.Frame1)
        Me.Controls.Add(Me.Frame2)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.cmdApply)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmItemVendorStore"
        Me.ShowInTaskbar = False
        Me.Frame1.ResumeLayout(False)
        Me.Frame2.ResumeLayout(False)
        CType(Me.optSelection, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ugrdStoreList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ugrdStoreList As Infragistics.Win.UltraWinGrid.UltraGrid
#End Region 
End Class