<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class EIMStoreSelector
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.UltraGrid1 = New Infragistics.Win.UltraWinGrid.UltraGrid()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.cmbStates = New System.Windows.Forms.ComboBox()
        Me.AllWFMRadioButton = New System.Windows.Forms.RadioButton()
        Me.StateRadioButton = New System.Windows.Forms.RadioButton()
        Me.cmbZones = New System.Windows.Forms.ComboBox()
        Me.ZoneRadioButton = New System.Windows.Forms.RadioButton()
        Me.AllRadioButton = New System.Windows.Forms.RadioButton()
        Me.ManualRadioButton = New System.Windows.Forms.RadioButton()
        Me.ButtonCopyFrom = New System.Windows.Forms.Button()
        Me.ButtonCopyTo = New System.Windows.Forms.Button()
        CType(Me.UltraGrid1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'UltraGrid1
        '
        Appearance16.BackColor = System.Drawing.SystemColors.ControlLight
        Appearance16.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.UltraGrid1.DisplayLayout.Appearance = Appearance16
        Me.UltraGrid1.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        UltraGridBand1.ColHeadersVisible = False
        UltraGridColumn1.Header.VisiblePosition = 0
        UltraGridColumn1.Hidden = True
        UltraGridColumn2.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn2.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        Appearance17.TextHAlignAsString = "Center"
        UltraGridColumn2.Header.Appearance = Appearance17
        UltraGridColumn2.Header.Caption = "Stores"
        UltraGridColumn2.Header.VisiblePosition = 1
        UltraGridColumn2.SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled
        UltraGridColumn2.Width = 210
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
        UltraGridColumn8.Width = 86
        UltraGridBand1.Columns.AddRange(New Object() {UltraGridColumn1, UltraGridColumn2, UltraGridColumn3, UltraGridColumn4, UltraGridColumn5, UltraGridColumn6, UltraGridColumn7, UltraGridColumn8})
        Me.UltraGrid1.DisplayLayout.BandsSerializer.Add(UltraGridBand1)
        Me.UltraGrid1.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Appearance18.FontData.BoldAsString = "True"
        Me.UltraGrid1.DisplayLayout.CaptionAppearance = Appearance18
        Me.UltraGrid1.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Appearance19.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance19.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance19.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance19.BorderColor = System.Drawing.SystemColors.Window
        Me.UltraGrid1.DisplayLayout.GroupByBox.Appearance = Appearance19
        Appearance20.ForeColor = System.Drawing.SystemColors.GrayText
        Me.UltraGrid1.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance20
        Me.UltraGrid1.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.UltraGrid1.DisplayLayout.GroupByBox.Hidden = True
        Appearance21.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance21.BackColor2 = System.Drawing.SystemColors.Control
        Appearance21.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance21.ForeColor = System.Drawing.SystemColors.GrayText
        Me.UltraGrid1.DisplayLayout.GroupByBox.PromptAppearance = Appearance21
        Me.UltraGrid1.DisplayLayout.MaxColScrollRegions = 1
        Me.UltraGrid1.DisplayLayout.MaxRowScrollRegions = 1
        Appearance22.BackColor = System.Drawing.SystemColors.Window
        Appearance22.ForeColor = System.Drawing.SystemColors.ControlText
        Me.UltraGrid1.DisplayLayout.Override.ActiveCellAppearance = Appearance22
        Me.UltraGrid1.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.UltraGrid1.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance23.BackColor = System.Drawing.SystemColors.Window
        Me.UltraGrid1.DisplayLayout.Override.CardAreaAppearance = Appearance23
        Appearance24.BorderColor = System.Drawing.Color.Silver
        Appearance24.FontData.BoldAsString = "True"
        Appearance24.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.UltraGrid1.DisplayLayout.Override.CellAppearance = Appearance24
        Me.UltraGrid1.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.UltraGrid1.DisplayLayout.Override.CellPadding = 0
        Appearance25.FontData.BoldAsString = "True"
        Me.UltraGrid1.DisplayLayout.Override.FixedHeaderAppearance = Appearance25
        Appearance26.BackColor = System.Drawing.SystemColors.Control
        Appearance26.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance26.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance26.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance26.BorderColor = System.Drawing.SystemColors.Window
        Me.UltraGrid1.DisplayLayout.Override.GroupByRowAppearance = Appearance26
        Appearance27.FontData.BoldAsString = "True"
        Appearance27.TextHAlignAsString = "Left"
        Me.UltraGrid1.DisplayLayout.Override.HeaderAppearance = Appearance27
        Me.UltraGrid1.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle
        Me.UltraGrid1.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance28.BackColor = System.Drawing.SystemColors.ControlLight
        Me.UltraGrid1.DisplayLayout.Override.RowAlternateAppearance = Appearance28
        Appearance29.BackColor = System.Drawing.SystemColors.Window
        Appearance29.BorderColor = System.Drawing.Color.Silver
        Me.UltraGrid1.DisplayLayout.Override.RowAppearance = Appearance29
        Me.UltraGrid1.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[True]
        Me.UltraGrid1.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed
        Me.UltraGrid1.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.UltraGrid1.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.UltraGrid1.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Extended
        Appearance30.BackColor = System.Drawing.SystemColors.ControlLight
        Me.UltraGrid1.DisplayLayout.Override.TemplateAddRowAppearance = Appearance30
        Me.UltraGrid1.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.UltraGrid1.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.UltraGrid1.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand
        Me.UltraGrid1.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        Me.UltraGrid1.Font = New System.Drawing.Font("Arial", 8.0!)
        Me.UltraGrid1.Location = New System.Drawing.Point(304, 4)
        Me.UltraGrid1.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.UltraGrid1.Name = "UltraGrid1"
        Me.UltraGrid1.Size = New System.Drawing.Size(231, 118)
        Me.UltraGrid1.TabIndex = 47
        '
        'GroupBox2
        '
        Me.GroupBox2.BackColor = System.Drawing.Color.Transparent
        Me.GroupBox2.Controls.Add(Me.cmbStates)
        Me.GroupBox2.Controls.Add(Me.AllWFMRadioButton)
        Me.GroupBox2.Controls.Add(Me.StateRadioButton)
        Me.GroupBox2.Controls.Add(Me.cmbZones)
        Me.GroupBox2.Controls.Add(Me.ZoneRadioButton)
        Me.GroupBox2.Controls.Add(Me.AllRadioButton)
        Me.GroupBox2.Controls.Add(Me.ManualRadioButton)
        Me.GroupBox2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.GroupBox2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBox2.Location = New System.Drawing.Point(4, 4)
        Me.GroupBox2.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.GroupBox2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBox2.Size = New System.Drawing.Size(292, 118)
        Me.GroupBox2.TabIndex = 46
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Store Selection"
        '
        'cmbStates
        '
        Me.cmbStates.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cmbStates.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbStates.BackColor = System.Drawing.SystemColors.Window
        Me.cmbStates.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbStates.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbStates.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.cmbStates.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbStates.Location = New System.Drawing.Point(120, 79)
        Me.cmbStates.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cmbStates.Name = "cmbStates"
        Me.cmbStates.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbStates.Size = New System.Drawing.Size(156, 24)
        Me.cmbStates.Sorted = True
        Me.cmbStates.TabIndex = 6
        '
        'AllWFMRadioButton
        '
        Me.AllWFMRadioButton.BackColor = System.Drawing.Color.Transparent
        Me.AllWFMRadioButton.Cursor = System.Windows.Forms.Cursors.Default
        Me.AllWFMRadioButton.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.AllWFMRadioButton.ForeColor = System.Drawing.SystemColors.ControlText
        Me.AllWFMRadioButton.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.AllWFMRadioButton.Location = New System.Drawing.Point(191, 20)
        Me.AllWFMRadioButton.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.AllWFMRadioButton.Name = "AllWFMRadioButton"
        Me.AllWFMRadioButton.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.AllWFMRadioButton.Size = New System.Drawing.Size(93, 21)
        Me.AllWFMRadioButton.TabIndex = 2
        Me.AllWFMRadioButton.TabStop = True
        Me.AllWFMRadioButton.Text = "All WFM"
        Me.AllWFMRadioButton.UseVisualStyleBackColor = False
        '
        'StateRadioButton
        '
        Me.StateRadioButton.BackColor = System.Drawing.Color.Transparent
        Me.StateRadioButton.Cursor = System.Windows.Forms.Cursors.Default
        Me.StateRadioButton.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.StateRadioButton.ForeColor = System.Drawing.SystemColors.ControlText
        Me.StateRadioButton.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.StateRadioButton.Location = New System.Drawing.Point(20, 81)
        Me.StateRadioButton.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.StateRadioButton.Name = "StateRadioButton"
        Me.StateRadioButton.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StateRadioButton.Size = New System.Drawing.Size(93, 21)
        Me.StateRadioButton.TabIndex = 5
        Me.StateRadioButton.TabStop = True
        Me.StateRadioButton.Text = "By State"
        Me.StateRadioButton.UseVisualStyleBackColor = False
        '
        'cmbZones
        '
        Me.cmbZones.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cmbZones.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbZones.BackColor = System.Drawing.SystemColors.Window
        Me.cmbZones.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbZones.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbZones.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.cmbZones.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbZones.Location = New System.Drawing.Point(120, 46)
        Me.cmbZones.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cmbZones.Name = "cmbZones"
        Me.cmbZones.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbZones.Size = New System.Drawing.Size(156, 24)
        Me.cmbZones.Sorted = True
        Me.cmbZones.TabIndex = 4
        '
        'ZoneRadioButton
        '
        Me.ZoneRadioButton.BackColor = System.Drawing.Color.Transparent
        Me.ZoneRadioButton.Cursor = System.Windows.Forms.Cursors.Default
        Me.ZoneRadioButton.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.ZoneRadioButton.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ZoneRadioButton.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.ZoneRadioButton.Location = New System.Drawing.Point(20, 50)
        Me.ZoneRadioButton.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.ZoneRadioButton.Name = "ZoneRadioButton"
        Me.ZoneRadioButton.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ZoneRadioButton.Size = New System.Drawing.Size(95, 21)
        Me.ZoneRadioButton.TabIndex = 3
        Me.ZoneRadioButton.TabStop = True
        Me.ZoneRadioButton.Text = "By Zone"
        Me.ZoneRadioButton.UseVisualStyleBackColor = False
        '
        'AllRadioButton
        '
        Me.AllRadioButton.BackColor = System.Drawing.Color.Transparent
        Me.AllRadioButton.Cursor = System.Windows.Forms.Cursors.Default
        Me.AllRadioButton.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.AllRadioButton.ForeColor = System.Drawing.SystemColors.ControlText
        Me.AllRadioButton.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.AllRadioButton.Location = New System.Drawing.Point(103, 20)
        Me.AllRadioButton.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.AllRadioButton.Name = "AllRadioButton"
        Me.AllRadioButton.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.AllRadioButton.Size = New System.Drawing.Size(104, 21)
        Me.AllRadioButton.TabIndex = 1
        Me.AllRadioButton.TabStop = True
        Me.AllRadioButton.Text = "All Stores"
        Me.AllRadioButton.UseVisualStyleBackColor = False
        '
        'ManualRadioButton
        '
        Me.ManualRadioButton.BackColor = System.Drawing.Color.Transparent
        Me.ManualRadioButton.Checked = True
        Me.ManualRadioButton.Cursor = System.Windows.Forms.Cursors.Default
        Me.ManualRadioButton.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.ManualRadioButton.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ManualRadioButton.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.ManualRadioButton.Location = New System.Drawing.Point(8, 20)
        Me.ManualRadioButton.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.ManualRadioButton.Name = "ManualRadioButton"
        Me.ManualRadioButton.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ManualRadioButton.Size = New System.Drawing.Size(88, 21)
        Me.ManualRadioButton.TabIndex = 0
        Me.ManualRadioButton.TabStop = True
        Me.ManualRadioButton.Text = "Manual"
        Me.ManualRadioButton.UseVisualStyleBackColor = False
        '
        'ButtonCopyFrom
        '
        Me.ButtonCopyFrom.Location = New System.Drawing.Point(543, 60)
        Me.ButtonCopyFrom.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.ButtonCopyFrom.Name = "ButtonCopyFrom"
        Me.ButtonCopyFrom.Size = New System.Drawing.Size(199, 28)
        Me.ButtonCopyFrom.TabIndex = 48
        Me.ButtonCopyFrom.Text = "Copy from Cost Upload"
        Me.ButtonCopyFrom.UseVisualStyleBackColor = True
        '
        'ButtonCopyTo
        '
        Me.ButtonCopyTo.Location = New System.Drawing.Point(543, 25)
        Me.ButtonCopyTo.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.ButtonCopyTo.Name = "ButtonCopyTo"
        Me.ButtonCopyTo.Size = New System.Drawing.Size(199, 28)
        Me.ButtonCopyTo.TabIndex = 49
        Me.ButtonCopyTo.Text = "Copy to Cost Upload"
        Me.ButtonCopyTo.UseVisualStyleBackColor = True
        '
        'EIMStoreSelector
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Transparent
        Me.Controls.Add(Me.ButtonCopyTo)
        Me.Controls.Add(Me.ButtonCopyFrom)
        Me.Controls.Add(Me.UltraGrid1)
        Me.Controls.Add(Me.GroupBox2)
        Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Name = "EIMStoreSelector"
        Me.Size = New System.Drawing.Size(747, 130)
        CType(Me.UltraGrid1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents UltraGrid1 As Infragistics.Win.UltraWinGrid.UltraGrid
    Public WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Public WithEvents cmbStates As System.Windows.Forms.ComboBox
    Public WithEvents AllWFMRadioButton As System.Windows.Forms.RadioButton
    Public WithEvents StateRadioButton As System.Windows.Forms.RadioButton
    Public WithEvents cmbZones As System.Windows.Forms.ComboBox
    Public WithEvents ZoneRadioButton As System.Windows.Forms.RadioButton
    Public WithEvents AllRadioButton As System.Windows.Forms.RadioButton
    Public WithEvents ManualRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents ButtonCopyFrom As System.Windows.Forms.Button
    Friend WithEvents ButtonCopyTo As System.Windows.Forms.Button

End Class
