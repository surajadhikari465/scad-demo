<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CancelAllSales
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim Appearance16 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridBand1 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1)
        Dim UltraGridColumn1 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Store_No")
        Dim UltraGridColumn2 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Store_Name")
        Dim Appearance17 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridColumn3 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Zone_ID")
        Dim UltraGridColumn4 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("State")
        Dim UltraGridColumn5 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("WFM_Store")
        Dim UltraGridColumn6 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Mega_Store")
        Dim UltraGridColumn7 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("CustomerType")
        Dim UltraGridColumn8 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("IsGPMStore", 0)
        Dim Appearance18 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance19 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance20 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance21 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance22 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance23 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance24 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance25 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance26 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance27 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance28 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance29 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance30 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(CancelAllSales))
        Me.GroupBox_StoreSel = New System.Windows.Forms.GroupBox()
        Me.ugrdStoreList = New Infragistics.Win.UltraWinGrid.UltraGrid()
        Me.RadioButton_Manual = New System.Windows.Forms.RadioButton()
        Me.RadioButton_All = New System.Windows.Forms.RadioButton()
        Me.RadioButton_Zone = New System.Windows.Forms.RadioButton()
        Me.cmbZones = New System.Windows.Forms.ComboBox()
        Me.RadioButton_State = New System.Windows.Forms.RadioButton()
        Me.RadioButton_AllWFM = New System.Windows.Forms.RadioButton()
        Me.cmbStates = New System.Windows.Forms.ComboBox()
        Me.Button_ApplyChanges = New System.Windows.Forms.Button()
        Me.dtpStartDate = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor()
        Me.Label_StartDate = New System.Windows.Forms.Label()
        Me.Button_Exit = New System.Windows.Forms.Button()
        Me.GroupBox_StoreSel.SuspendLayout()
        CType(Me.ugrdStoreList, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dtpStartDate, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GroupBox_StoreSel
        '
        Me.GroupBox_StoreSel.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox_StoreSel.Controls.Add(Me.ugrdStoreList)
        Me.GroupBox_StoreSel.Controls.Add(Me.RadioButton_Manual)
        Me.GroupBox_StoreSel.Controls.Add(Me.RadioButton_All)
        Me.GroupBox_StoreSel.Controls.Add(Me.RadioButton_Zone)
        Me.GroupBox_StoreSel.Controls.Add(Me.cmbZones)
        Me.GroupBox_StoreSel.Controls.Add(Me.RadioButton_State)
        Me.GroupBox_StoreSel.Controls.Add(Me.RadioButton_AllWFM)
        Me.GroupBox_StoreSel.Controls.Add(Me.cmbStates)
        Me.GroupBox_StoreSel.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.GroupBox_StoreSel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBox_StoreSel.Location = New System.Drawing.Point(16, 58)
        Me.GroupBox_StoreSel.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.GroupBox_StoreSel.Name = "GroupBox_StoreSel"
        Me.GroupBox_StoreSel.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.GroupBox_StoreSel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBox_StoreSel.Size = New System.Drawing.Size(404, 385)
        Me.GroupBox_StoreSel.TabIndex = 116
        Me.GroupBox_StoreSel.TabStop = False
        Me.GroupBox_StoreSel.Text = "Store Selection"
        '
        'ugrdStoreList
        '
        Appearance16.BackColor = System.Drawing.SystemColors.ControlLight
        Appearance16.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.ugrdStoreList.DisplayLayout.Appearance = Appearance16
        Me.ugrdStoreList.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        UltraGridColumn1.Header.VisiblePosition = 0
        UltraGridColumn1.Hidden = True
        UltraGridColumn2.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn2.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        Appearance17.TextHAlignAsString = "Center"
        UltraGridColumn2.Header.Appearance = Appearance17
        UltraGridColumn2.Header.Caption = "Stores"
        UltraGridColumn2.Header.VisiblePosition = 1
        UltraGridColumn2.SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled
        UltraGridColumn2.Width = 300
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
        UltraGridColumn8.Header.VisiblePosition = 7
        UltraGridColumn8.Hidden = True
        UltraGridColumn8.Width = 97
        UltraGridBand1.Columns.AddRange(New Object() {UltraGridColumn1, UltraGridColumn2, UltraGridColumn3, UltraGridColumn4, UltraGridColumn5, UltraGridColumn6, UltraGridColumn7, UltraGridColumn8})
        Me.ugrdStoreList.DisplayLayout.BandsSerializer.Add(UltraGridBand1)
        Me.ugrdStoreList.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Appearance18.FontData.BoldAsString = "True"
        Me.ugrdStoreList.DisplayLayout.CaptionAppearance = Appearance18
        Me.ugrdStoreList.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Appearance19.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance19.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance19.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance19.BorderColor = System.Drawing.SystemColors.Window
        Me.ugrdStoreList.DisplayLayout.GroupByBox.Appearance = Appearance19
        Appearance20.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugrdStoreList.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance20
        Me.ugrdStoreList.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.ugrdStoreList.DisplayLayout.GroupByBox.Hidden = True
        Appearance21.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance21.BackColor2 = System.Drawing.SystemColors.Control
        Appearance21.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance21.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugrdStoreList.DisplayLayout.GroupByBox.PromptAppearance = Appearance21
        Me.ugrdStoreList.DisplayLayout.MaxColScrollRegions = 1
        Me.ugrdStoreList.DisplayLayout.MaxRowScrollRegions = 1
        Appearance22.BackColor = System.Drawing.SystemColors.Window
        Appearance22.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ugrdStoreList.DisplayLayout.Override.ActiveCellAppearance = Appearance22
        Me.ugrdStoreList.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.ugrdStoreList.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance23.BackColor = System.Drawing.SystemColors.Window
        Me.ugrdStoreList.DisplayLayout.Override.CardAreaAppearance = Appearance23
        Appearance24.BorderColor = System.Drawing.Color.Silver
        Appearance24.FontData.BoldAsString = "True"
        Appearance24.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.ugrdStoreList.DisplayLayout.Override.CellAppearance = Appearance24
        Me.ugrdStoreList.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.ugrdStoreList.DisplayLayout.Override.CellPadding = 0
        Appearance25.FontData.BoldAsString = "True"
        Me.ugrdStoreList.DisplayLayout.Override.FixedHeaderAppearance = Appearance25
        Appearance26.BackColor = System.Drawing.SystemColors.Control
        Appearance26.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance26.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance26.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance26.BorderColor = System.Drawing.SystemColors.Window
        Me.ugrdStoreList.DisplayLayout.Override.GroupByRowAppearance = Appearance26
        Appearance27.FontData.BoldAsString = "True"
        Appearance27.TextHAlignAsString = "Left"
        Me.ugrdStoreList.DisplayLayout.Override.HeaderAppearance = Appearance27
        Me.ugrdStoreList.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle
        Me.ugrdStoreList.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance28.BackColor = System.Drawing.SystemColors.ControlLight
        Me.ugrdStoreList.DisplayLayout.Override.RowAlternateAppearance = Appearance28
        Appearance29.BackColor = System.Drawing.SystemColors.Window
        Appearance29.BorderColor = System.Drawing.Color.Silver
        Me.ugrdStoreList.DisplayLayout.Override.RowAppearance = Appearance29
        Me.ugrdStoreList.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[True]
        Me.ugrdStoreList.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed
        Me.ugrdStoreList.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.ugrdStoreList.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.ugrdStoreList.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Extended
        Appearance30.BackColor = System.Drawing.SystemColors.ControlLight
        Me.ugrdStoreList.DisplayLayout.Override.TemplateAddRowAppearance = Appearance30
        Me.ugrdStoreList.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.ugrdStoreList.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.ugrdStoreList.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand
        Me.ugrdStoreList.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        Me.ugrdStoreList.Font = New System.Drawing.Font("Arial", 8.0!)
        Me.ugrdStoreList.Location = New System.Drawing.Point(43, 123)
        Me.ugrdStoreList.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.ugrdStoreList.Name = "ugrdStoreList"
        Me.ugrdStoreList.Size = New System.Drawing.Size(321, 241)
        Me.ugrdStoreList.TabIndex = 9
        '
        'RadioButton_Manual
        '
        Me.RadioButton_Manual.BackColor = System.Drawing.SystemColors.Control
        Me.RadioButton_Manual.Checked = True
        Me.RadioButton_Manual.Cursor = System.Windows.Forms.Cursors.Default
        Me.RadioButton_Manual.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.RadioButton_Manual.ForeColor = System.Drawing.SystemColors.ControlText
        Me.RadioButton_Manual.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.RadioButton_Manual.Location = New System.Drawing.Point(49, 26)
        Me.RadioButton_Manual.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.RadioButton_Manual.Name = "RadioButton_Manual"
        Me.RadioButton_Manual.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.RadioButton_Manual.Size = New System.Drawing.Size(88, 21)
        Me.RadioButton_Manual.TabIndex = 0
        Me.RadioButton_Manual.TabStop = True
        Me.RadioButton_Manual.Text = "Manual"
        Me.RadioButton_Manual.UseVisualStyleBackColor = False
        '
        'RadioButton_All
        '
        Me.RadioButton_All.BackColor = System.Drawing.SystemColors.Control
        Me.RadioButton_All.Cursor = System.Windows.Forms.Cursors.Default
        Me.RadioButton_All.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.RadioButton_All.ForeColor = System.Drawing.SystemColors.ControlText
        Me.RadioButton_All.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.RadioButton_All.Location = New System.Drawing.Point(153, 26)
        Me.RadioButton_All.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.RadioButton_All.Name = "RadioButton_All"
        Me.RadioButton_All.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.RadioButton_All.Size = New System.Drawing.Size(104, 21)
        Me.RadioButton_All.TabIndex = 1
        Me.RadioButton_All.TabStop = True
        Me.RadioButton_All.Text = "All Stores"
        Me.RadioButton_All.UseVisualStyleBackColor = False
        '
        'RadioButton_Zone
        '
        Me.RadioButton_Zone.BackColor = System.Drawing.SystemColors.Control
        Me.RadioButton_Zone.Cursor = System.Windows.Forms.Cursors.Default
        Me.RadioButton_Zone.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.RadioButton_Zone.ForeColor = System.Drawing.SystemColors.ControlText
        Me.RadioButton_Zone.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.RadioButton_Zone.Location = New System.Drawing.Point(49, 57)
        Me.RadioButton_Zone.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.RadioButton_Zone.Name = "RadioButton_Zone"
        Me.RadioButton_Zone.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.RadioButton_Zone.Size = New System.Drawing.Size(95, 21)
        Me.RadioButton_Zone.TabIndex = 3
        Me.RadioButton_Zone.TabStop = True
        Me.RadioButton_Zone.Text = "By Zone"
        Me.RadioButton_Zone.UseVisualStyleBackColor = False
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
        Me.cmbZones.Location = New System.Drawing.Point(149, 52)
        Me.cmbZones.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cmbZones.Name = "cmbZones"
        Me.cmbZones.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbZones.Size = New System.Drawing.Size(213, 24)
        Me.cmbZones.Sorted = True
        Me.cmbZones.TabIndex = 4
        '
        'RadioButton_State
        '
        Me.RadioButton_State.BackColor = System.Drawing.SystemColors.Control
        Me.RadioButton_State.Cursor = System.Windows.Forms.Cursors.Default
        Me.RadioButton_State.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.RadioButton_State.ForeColor = System.Drawing.SystemColors.ControlText
        Me.RadioButton_State.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.RadioButton_State.Location = New System.Drawing.Point(49, 87)
        Me.RadioButton_State.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.RadioButton_State.Name = "RadioButton_State"
        Me.RadioButton_State.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.RadioButton_State.Size = New System.Drawing.Size(93, 21)
        Me.RadioButton_State.TabIndex = 5
        Me.RadioButton_State.TabStop = True
        Me.RadioButton_State.Text = "By State"
        Me.RadioButton_State.UseVisualStyleBackColor = False
        '
        'RadioButton_AllWFM
        '
        Me.RadioButton_AllWFM.BackColor = System.Drawing.SystemColors.Control
        Me.RadioButton_AllWFM.Cursor = System.Windows.Forms.Cursors.Default
        Me.RadioButton_AllWFM.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.RadioButton_AllWFM.ForeColor = System.Drawing.SystemColors.ControlText
        Me.RadioButton_AllWFM.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.RadioButton_AllWFM.Location = New System.Drawing.Point(268, 26)
        Me.RadioButton_AllWFM.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.RadioButton_AllWFM.Name = "RadioButton_AllWFM"
        Me.RadioButton_AllWFM.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.RadioButton_AllWFM.Size = New System.Drawing.Size(108, 21)
        Me.RadioButton_AllWFM.TabIndex = 2
        Me.RadioButton_AllWFM.TabStop = True
        Me.RadioButton_AllWFM.Text = "All WFM"
        Me.RadioButton_AllWFM.UseVisualStyleBackColor = False
        '
        'cmbStates
        '
        Me.cmbStates.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbStates.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbStates.BackColor = System.Drawing.SystemColors.Window
        Me.cmbStates.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbStates.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbStates.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.cmbStates.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbStates.Location = New System.Drawing.Point(149, 85)
        Me.cmbStates.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cmbStates.Name = "cmbStates"
        Me.cmbStates.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbStates.Size = New System.Drawing.Size(213, 24)
        Me.cmbStates.Sorted = True
        Me.cmbStates.TabIndex = 6
        '
        'Button_ApplyChanges
        '
        Me.Button_ApplyChanges.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_ApplyChanges.BackColor = System.Drawing.SystemColors.Control
        Me.Button_ApplyChanges.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button_ApplyChanges.Font = New System.Drawing.Font("Arial", 8.0!)
        Me.Button_ApplyChanges.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button_ApplyChanges.Image = CType(resources.GetObject("Button_ApplyChanges.Image"), System.Drawing.Image)
        Me.Button_ApplyChanges.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Button_ApplyChanges.Location = New System.Drawing.Point(304, 452)
        Me.Button_ApplyChanges.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Button_ApplyChanges.Name = "Button_ApplyChanges"
        Me.Button_ApplyChanges.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button_ApplyChanges.Size = New System.Drawing.Size(55, 50)
        Me.Button_ApplyChanges.TabIndex = 118
        Me.Button_ApplyChanges.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.Button_ApplyChanges.UseVisualStyleBackColor = False
        '
        'dtpStartDate
        '
        Me.dtpStartDate.Location = New System.Drawing.Point(169, 15)
        Me.dtpStartDate.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.dtpStartDate.Name = "dtpStartDate"
        Me.dtpStartDate.Size = New System.Drawing.Size(113, 24)
        Me.dtpStartDate.TabIndex = 120
        '
        'Label_StartDate
        '
        Me.Label_StartDate.BackColor = System.Drawing.Color.Transparent
        Me.Label_StartDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label_StartDate.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.Label_StartDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label_StartDate.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label_StartDate.Location = New System.Drawing.Point(21, 20)
        Me.Label_StartDate.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label_StartDate.Name = "Label_StartDate"
        Me.Label_StartDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label_StartDate.Size = New System.Drawing.Size(127, 21)
        Me.Label_StartDate.TabIndex = 119
        Me.Label_StartDate.Text = "Effective Date :"
        Me.Label_StartDate.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Button_Exit
        '
        Me.Button_Exit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Exit.BackColor = System.Drawing.SystemColors.Control
        Me.Button_Exit.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button_Exit.Font = New System.Drawing.Font("Arial", 8.0!)
        Me.Button_Exit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button_Exit.Image = CType(resources.GetObject("Button_Exit.Image"), System.Drawing.Image)
        Me.Button_Exit.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Button_Exit.Location = New System.Drawing.Point(365, 452)
        Me.Button_Exit.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Button_Exit.Name = "Button_Exit"
        Me.Button_Exit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button_Exit.Size = New System.Drawing.Size(55, 50)
        Me.Button_Exit.TabIndex = 121
        Me.Button_Exit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.Button_Exit.UseVisualStyleBackColor = False
        '
        'CancelAllSales
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(437, 517)
        Me.Controls.Add(Me.Button_Exit)
        Me.Controls.Add(Me.dtpStartDate)
        Me.Controls.Add(Me.Label_StartDate)
        Me.Controls.Add(Me.Button_ApplyChanges)
        Me.Controls.Add(Me.GroupBox_StoreSel)
        Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Name = "CancelAllSales"
        Me.Text = "Cancel All Sales"
        Me.GroupBox_StoreSel.ResumeLayout(False)
        CType(Me.ugrdStoreList, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dtpStartDate, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Public WithEvents GroupBox_StoreSel As System.Windows.Forms.GroupBox
    Friend WithEvents ugrdStoreList As Infragistics.Win.UltraWinGrid.UltraGrid
    Public WithEvents RadioButton_Manual As System.Windows.Forms.RadioButton
    Public WithEvents RadioButton_All As System.Windows.Forms.RadioButton
    Public WithEvents RadioButton_Zone As System.Windows.Forms.RadioButton
    Public WithEvents cmbZones As System.Windows.Forms.ComboBox
    Public WithEvents RadioButton_State As System.Windows.Forms.RadioButton
    Public WithEvents RadioButton_AllWFM As System.Windows.Forms.RadioButton
    Public WithEvents cmbStates As System.Windows.Forms.ComboBox
    Public WithEvents Button_ApplyChanges As System.Windows.Forms.Button
    Friend WithEvents dtpStartDate As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
    Public WithEvents Label_StartDate As System.Windows.Forms.Label
    Public WithEvents Button_Exit As System.Windows.Forms.Button
End Class
