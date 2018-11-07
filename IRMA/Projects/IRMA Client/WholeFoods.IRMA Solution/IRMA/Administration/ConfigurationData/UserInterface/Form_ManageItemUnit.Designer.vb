<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_ManageItemUnit
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim Appearance13 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridBand1 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("ItemUnitBO", -1)
        Dim UltraGridColumn1 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("UnitId")
        Dim UltraGridColumn2 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("UnitName")
        Dim UltraGridColumn3 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("WeightUnit")
        Dim UltraGridColumn4 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("UserId")
        Dim UltraGridColumn5 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("UnitAbbreviation")
        Dim UltraGridColumn6 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("UnitSysCode")
        Dim UltraGridColumn7 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("IsPackageUnit")
        Dim UltraGridColumn8 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("PlumUnitAbbr")
        Dim UltraGridColumn9 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("EDISysCode")
        Dim UltraGridColumn10 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("ErrorMessage")
        Dim UltraGridColumn11 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Valid")
        Dim UltraGridColumn12 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("DataChanged")
        Dim Appearance14 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance15 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance16 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance17 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance18 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance19 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance20 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance21 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance22 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance23 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance24 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance49 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Me.UltraGrid1 = New Infragistics.Win.UltraWinGrid.UltraGrid()
        Me.ItemUnitBOBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.UltraPanel1 = New Infragistics.Win.Misc.UltraPanel()
        Me.RichTextBox1 = New System.Windows.Forms.RichTextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.TextBox_NewItemUnit_UnitSysCode = New System.Windows.Forms.TextBox()
        Me.Button_NewItemUnit_Close = New System.Windows.Forms.Button()
        Me.Button_NewItemUnit_Save = New System.Windows.Forms.Button()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.CheckBox_NewItemUnit_PackageUnit = New System.Windows.Forms.CheckBox()
        Me.CheckBox_NewItemUnit_WeightUnit = New System.Windows.Forms.CheckBox()
        Me.TextBox_NewItemUnit_PlumAbbrev = New System.Windows.Forms.TextBox()
        Me.TextBox_NewItemUnit_EDISysCode = New System.Windows.Forms.TextBox()
        Me.TextBox_NewItemUnit_Abbreviation = New System.Windows.Forms.TextBox()
        Me.TextBox_NewItemUnit_Name = New System.Windows.Forms.TextBox()
        Me.Button_AddNew = New System.Windows.Forms.Button()
        Me.Button_Save = New System.Windows.Forms.Button()
        Me.Button_Close = New System.Windows.Forms.Button()
        CType(Me.UltraGrid1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ItemUnitBOBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.UltraPanel1.ClientArea.SuspendLayout()
        Me.UltraPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'UltraGrid1
        '
        Me.UltraGrid1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.UltraGrid1.DataSource = Me.ItemUnitBOBindingSource
        Appearance13.BackColor = System.Drawing.SystemColors.Window
        Appearance13.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.UltraGrid1.DisplayLayout.Appearance = Appearance13
        Me.UltraGrid1.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        UltraGridColumn1.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn1.Header.VisiblePosition = 0
        UltraGridColumn1.Width = 36
        UltraGridColumn2.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn2.Header.VisiblePosition = 1
        UltraGridColumn2.Width = 142
        UltraGridColumn3.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn3.Header.VisiblePosition = 2
        UltraGridColumn3.Width = 104
        UltraGridColumn4.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn4.Header.VisiblePosition = 3
        UltraGridColumn4.Hidden = True
        UltraGridColumn4.Width = 65
        UltraGridColumn5.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn5.Header.VisiblePosition = 4
        UltraGridColumn5.Width = 139
        UltraGridColumn6.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn6.Header.VisiblePosition = 5
        UltraGridColumn6.Width = 125
        UltraGridColumn7.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn7.Header.Caption = "PackageUnit"
        UltraGridColumn7.Header.VisiblePosition = 7
        UltraGridColumn7.Width = 123
        UltraGridColumn8.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn8.Header.VisiblePosition = 8
        UltraGridColumn8.Width = 137
        UltraGridColumn9.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn9.Header.VisiblePosition = 9
        UltraGridColumn9.Width = 125
        UltraGridColumn10.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn10.Header.VisiblePosition = 10
        UltraGridColumn10.Hidden = True
        UltraGridColumn10.Width = 93
        UltraGridColumn11.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn11.Header.VisiblePosition = 11
        UltraGridColumn11.Hidden = True
        UltraGridColumn11.Width = 44
        UltraGridColumn12.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn12.Header.VisiblePosition = 6
        UltraGridColumn12.Hidden = True
        UltraGridColumn12.Width = 98
        UltraGridBand1.Columns.AddRange(New Object() {UltraGridColumn1, UltraGridColumn2, UltraGridColumn3, UltraGridColumn4, UltraGridColumn5, UltraGridColumn6, UltraGridColumn7, UltraGridColumn8, UltraGridColumn9, UltraGridColumn10, UltraGridColumn11, UltraGridColumn12})
        UltraGridBand1.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        Me.UltraGrid1.DisplayLayout.BandsSerializer.Add(UltraGridBand1)
        Me.UltraGrid1.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.UltraGrid1.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Appearance14.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance14.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance14.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance14.BorderColor = System.Drawing.SystemColors.Window
        Me.UltraGrid1.DisplayLayout.GroupByBox.Appearance = Appearance14
        Appearance15.ForeColor = System.Drawing.SystemColors.GrayText
        Me.UltraGrid1.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance15
        Me.UltraGrid1.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Appearance16.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance16.BackColor2 = System.Drawing.SystemColors.Control
        Appearance16.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance16.ForeColor = System.Drawing.SystemColors.GrayText
        Me.UltraGrid1.DisplayLayout.GroupByBox.PromptAppearance = Appearance16
        Me.UltraGrid1.DisplayLayout.MaxColScrollRegions = 1
        Me.UltraGrid1.DisplayLayout.MaxRowScrollRegions = 1
        Appearance17.BackColor = System.Drawing.SystemColors.Window
        Appearance17.ForeColor = System.Drawing.SystemColors.ControlText
        Me.UltraGrid1.DisplayLayout.Override.ActiveCellAppearance = Appearance17
        Appearance18.BackColor = System.Drawing.SystemColors.Highlight
        Appearance18.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.UltraGrid1.DisplayLayout.Override.ActiveRowAppearance = Appearance18
        Me.UltraGrid1.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No
        Me.UltraGrid1.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.[False]
        Me.UltraGrid1.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.[False]
        Me.UltraGrid1.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.UltraGrid1.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance19.BackColor = System.Drawing.SystemColors.Window
        Me.UltraGrid1.DisplayLayout.Override.CardAreaAppearance = Appearance19
        Appearance20.BorderColor = System.Drawing.Color.Silver
        Appearance20.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.UltraGrid1.DisplayLayout.Override.CellAppearance = Appearance20
        Me.UltraGrid1.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.UltraGrid1.DisplayLayout.Override.CellPadding = 0
        Appearance21.BackColor = System.Drawing.SystemColors.Control
        Appearance21.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance21.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance21.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance21.BorderColor = System.Drawing.SystemColors.Window
        Me.UltraGrid1.DisplayLayout.Override.GroupByRowAppearance = Appearance21
        Appearance22.TextHAlignAsString = "Left"
        Me.UltraGrid1.DisplayLayout.Override.HeaderAppearance = Appearance22
        Me.UltraGrid1.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti
        Me.UltraGrid1.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance23.BackColor = System.Drawing.SystemColors.Window
        Appearance23.BorderColor = System.Drawing.Color.Silver
        Me.UltraGrid1.DisplayLayout.Override.RowAppearance = Appearance23
        Me.UltraGrid1.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[False]
        Me.UltraGrid1.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.UltraGrid1.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.UltraGrid1.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Extended
        Appearance24.BackColor = System.Drawing.SystemColors.ControlLight
        Me.UltraGrid1.DisplayLayout.Override.TemplateAddRowAppearance = Appearance24
        Me.UltraGrid1.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.UltraGrid1.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.UltraGrid1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.UltraGrid1.Location = New System.Drawing.Point(12, 12)
        Me.UltraGrid1.Name = "UltraGrid1"
        Me.UltraGrid1.Size = New System.Drawing.Size(933, 613)
        Me.UltraGrid1.TabIndex = 0
        Me.UltraGrid1.Text = "UltraGrid1"
        '
        'ItemUnitBOBindingSource
        '
        Me.ItemUnitBOBindingSource.AllowNew = False
        Me.ItemUnitBOBindingSource.DataSource = GetType(WholeFoods.IRMA.ItemHosting.BusinessLogic.ItemUnitBO)
        '
        'UltraPanel1
        '
        Appearance49.BorderColor = System.Drawing.Color.Black
        Appearance49.BorderColor2 = System.Drawing.Color.Silver
        Me.UltraPanel1.Appearance = Appearance49
        Me.UltraPanel1.BorderStyle = Infragistics.Win.UIElementBorderStyle.Raised
        '
        'UltraPanel1.ClientArea
        '
        Me.UltraPanel1.ClientArea.Controls.Add(Me.RichTextBox1)
        Me.UltraPanel1.ClientArea.Controls.Add(Me.Label6)
        Me.UltraPanel1.ClientArea.Controls.Add(Me.TextBox_NewItemUnit_UnitSysCode)
        Me.UltraPanel1.ClientArea.Controls.Add(Me.Button_NewItemUnit_Close)
        Me.UltraPanel1.ClientArea.Controls.Add(Me.Button_NewItemUnit_Save)
        Me.UltraPanel1.ClientArea.Controls.Add(Me.Label5)
        Me.UltraPanel1.ClientArea.Controls.Add(Me.Label4)
        Me.UltraPanel1.ClientArea.Controls.Add(Me.Label3)
        Me.UltraPanel1.ClientArea.Controls.Add(Me.Label2)
        Me.UltraPanel1.ClientArea.Controls.Add(Me.Label1)
        Me.UltraPanel1.ClientArea.Controls.Add(Me.CheckBox_NewItemUnit_PackageUnit)
        Me.UltraPanel1.ClientArea.Controls.Add(Me.CheckBox_NewItemUnit_WeightUnit)
        Me.UltraPanel1.ClientArea.Controls.Add(Me.TextBox_NewItemUnit_PlumAbbrev)
        Me.UltraPanel1.ClientArea.Controls.Add(Me.TextBox_NewItemUnit_EDISysCode)
        Me.UltraPanel1.ClientArea.Controls.Add(Me.TextBox_NewItemUnit_Abbreviation)
        Me.UltraPanel1.ClientArea.Controls.Add(Me.TextBox_NewItemUnit_Name)
        Me.UltraPanel1.Location = New System.Drawing.Point(292, 246)
        Me.UltraPanel1.Name = "UltraPanel1"
        Me.UltraPanel1.Size = New System.Drawing.Size(363, 210)
        Me.UltraPanel1.TabIndex = 2
        '
        'RichTextBox1
        '
        Me.RichTextBox1.BackColor = System.Drawing.SystemColors.Menu
        Me.RichTextBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RichTextBox1.ForeColor = System.Drawing.Color.Red
        Me.RichTextBox1.Location = New System.Drawing.Point(57, 142)
        Me.RichTextBox1.Name = "RichTextBox1"
        Me.RichTextBox1.Size = New System.Drawing.Size(266, 63)
        Me.RichTextBox1.TabIndex = 14
        Me.RichTextBox1.Text = "Caution: Changing these values can break Ordering/Receiving functionality." & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(10) & "Use a" & _
    "t your own risk."
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(1, 149)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(0, 13)
        Me.Label6.TabIndex = 13
        '
        'TextBox_NewItemUnit_UnitSysCode
        '
        Me.TextBox_NewItemUnit_UnitSysCode.Location = New System.Drawing.Point(111, 63)
        Me.TextBox_NewItemUnit_UnitSysCode.Name = "TextBox_NewItemUnit_UnitSysCode"
        Me.TextBox_NewItemUnit_UnitSysCode.Size = New System.Drawing.Size(137, 20)
        Me.TextBox_NewItemUnit_UnitSysCode.TabIndex = 12
        '
        'Button_NewItemUnit_Close
        '
        Me.Button_NewItemUnit_Close.Location = New System.Drawing.Point(258, 112)
        Me.Button_NewItemUnit_Close.Name = "Button_NewItemUnit_Close"
        Me.Button_NewItemUnit_Close.Size = New System.Drawing.Size(78, 20)
        Me.Button_NewItemUnit_Close.TabIndex = 3
        Me.Button_NewItemUnit_Close.Text = "Close"
        Me.Button_NewItemUnit_Close.UseVisualStyleBackColor = True
        '
        'Button_NewItemUnit_Save
        '
        Me.Button_NewItemUnit_Save.Location = New System.Drawing.Point(258, 86)
        Me.Button_NewItemUnit_Save.Name = "Button_NewItemUnit_Save"
        Me.Button_NewItemUnit_Save.Size = New System.Drawing.Size(78, 20)
        Me.Button_NewItemUnit_Save.TabIndex = 3
        Me.Button_NewItemUnit_Save.Text = "Save"
        Me.Button_NewItemUnit_Save.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(24, 119)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(81, 13)
        Me.Label5.TabIndex = 11
        Me.Label5.Text = "EDI SysCode"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(23, 93)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(82, 13)
        Me.Label4.TabIndex = 10
        Me.Label4.Text = "Plum Abbrev."
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(22, 68)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(83, 13)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "Unit SysCode"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(54, 41)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(51, 13)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "Abbrev."
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(66, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(39, 13)
        Me.Label1.TabIndex = 7
        Me.Label1.Text = "Name"
        '
        'CheckBox_NewItemUnit_PackageUnit
        '
        Me.CheckBox_NewItemUnit_PackageUnit.AutoSize = True
        Me.CheckBox_NewItemUnit_PackageUnit.Location = New System.Drawing.Point(254, 40)
        Me.CheckBox_NewItemUnit_PackageUnit.Name = "CheckBox_NewItemUnit_PackageUnit"
        Me.CheckBox_NewItemUnit_PackageUnit.Size = New System.Drawing.Size(91, 17)
        Me.CheckBox_NewItemUnit_PackageUnit.TabIndex = 6
        Me.CheckBox_NewItemUnit_PackageUnit.Text = "Package Unit"
        Me.CheckBox_NewItemUnit_PackageUnit.UseVisualStyleBackColor = True
        '
        'CheckBox_NewItemUnit_WeightUnit
        '
        Me.CheckBox_NewItemUnit_WeightUnit.AutoSize = True
        Me.CheckBox_NewItemUnit_WeightUnit.Location = New System.Drawing.Point(254, 14)
        Me.CheckBox_NewItemUnit_WeightUnit.Name = "CheckBox_NewItemUnit_WeightUnit"
        Me.CheckBox_NewItemUnit_WeightUnit.Size = New System.Drawing.Size(82, 17)
        Me.CheckBox_NewItemUnit_WeightUnit.TabIndex = 5
        Me.CheckBox_NewItemUnit_WeightUnit.Text = "Weight Unit"
        Me.CheckBox_NewItemUnit_WeightUnit.UseVisualStyleBackColor = True
        '
        'TextBox_NewItemUnit_PlumAbbrev
        '
        Me.TextBox_NewItemUnit_PlumAbbrev.Location = New System.Drawing.Point(111, 90)
        Me.TextBox_NewItemUnit_PlumAbbrev.Name = "TextBox_NewItemUnit_PlumAbbrev"
        Me.TextBox_NewItemUnit_PlumAbbrev.Size = New System.Drawing.Size(137, 20)
        Me.TextBox_NewItemUnit_PlumAbbrev.TabIndex = 3
        '
        'TextBox_NewItemUnit_EDISysCode
        '
        Me.TextBox_NewItemUnit_EDISysCode.Location = New System.Drawing.Point(111, 116)
        Me.TextBox_NewItemUnit_EDISysCode.Name = "TextBox_NewItemUnit_EDISysCode"
        Me.TextBox_NewItemUnit_EDISysCode.Size = New System.Drawing.Size(137, 20)
        Me.TextBox_NewItemUnit_EDISysCode.TabIndex = 2
        '
        'TextBox_NewItemUnit_Abbreviation
        '
        Me.TextBox_NewItemUnit_Abbreviation.Location = New System.Drawing.Point(111, 38)
        Me.TextBox_NewItemUnit_Abbreviation.Name = "TextBox_NewItemUnit_Abbreviation"
        Me.TextBox_NewItemUnit_Abbreviation.Size = New System.Drawing.Size(137, 20)
        Me.TextBox_NewItemUnit_Abbreviation.TabIndex = 1
        '
        'TextBox_NewItemUnit_Name
        '
        Me.TextBox_NewItemUnit_Name.Location = New System.Drawing.Point(111, 12)
        Me.TextBox_NewItemUnit_Name.Name = "TextBox_NewItemUnit_Name"
        Me.TextBox_NewItemUnit_Name.Size = New System.Drawing.Size(137, 20)
        Me.TextBox_NewItemUnit_Name.TabIndex = 0
        '
        'Button_AddNew
        '
        Me.Button_AddNew.Location = New System.Drawing.Point(15, 636)
        Me.Button_AddNew.Name = "Button_AddNew"
        Me.Button_AddNew.Size = New System.Drawing.Size(78, 24)
        Me.Button_AddNew.TabIndex = 3
        Me.Button_AddNew.Text = "Add New"
        Me.Button_AddNew.UseVisualStyleBackColor = True
        '
        'Button_Save
        '
        Me.Button_Save.Location = New System.Drawing.Point(773, 636)
        Me.Button_Save.Name = "Button_Save"
        Me.Button_Save.Size = New System.Drawing.Size(78, 24)
        Me.Button_Save.TabIndex = 4
        Me.Button_Save.Text = "Save"
        Me.Button_Save.UseVisualStyleBackColor = True
        '
        'Button_Close
        '
        Me.Button_Close.Location = New System.Drawing.Point(867, 636)
        Me.Button_Close.Name = "Button_Close"
        Me.Button_Close.Size = New System.Drawing.Size(78, 24)
        Me.Button_Close.TabIndex = 5
        Me.Button_Close.Text = "Close"
        Me.Button_Close.UseVisualStyleBackColor = True
        '
        'Form_ManageItemUnit
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(957, 676)
        Me.Controls.Add(Me.Button_Close)
        Me.Controls.Add(Me.Button_Save)
        Me.Controls.Add(Me.Button_AddNew)
        Me.Controls.Add(Me.UltraPanel1)
        Me.Controls.Add(Me.UltraGrid1)
        Me.Name = "Form_ManageItemUnit"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Manage ItemUnit Data"
        CType(Me.UltraGrid1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ItemUnitBOBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        Me.UltraPanel1.ClientArea.ResumeLayout(False)
        Me.UltraPanel1.ClientArea.PerformLayout()
        Me.UltraPanel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents UltraGrid1 As Infragistics.Win.UltraWinGrid.UltraGrid
    Friend WithEvents ItemUnitBOBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents UltraPanel1 As Infragistics.Win.Misc.UltraPanel
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents CheckBox_NewItemUnit_PackageUnit As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_NewItemUnit_WeightUnit As System.Windows.Forms.CheckBox
    Friend WithEvents TextBox_NewItemUnit_PlumAbbrev As System.Windows.Forms.TextBox
    Friend WithEvents TextBox_NewItemUnit_EDISysCode As System.Windows.Forms.TextBox
    Friend WithEvents TextBox_NewItemUnit_Abbreviation As System.Windows.Forms.TextBox
    Friend WithEvents TextBox_NewItemUnit_Name As System.Windows.Forms.TextBox
    Friend WithEvents Button_NewItemUnit_Close As System.Windows.Forms.Button
    Friend WithEvents Button_NewItemUnit_Save As System.Windows.Forms.Button
    Friend WithEvents Button_AddNew As System.Windows.Forms.Button
    Friend WithEvents Button_Save As System.Windows.Forms.Button
    Friend WithEvents Button_Close As System.Windows.Forms.Button
    Friend WithEvents TextBox_NewItemUnit_UnitSysCode As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents RichTextBox1 As System.Windows.Forms.RichTextBox
End Class
