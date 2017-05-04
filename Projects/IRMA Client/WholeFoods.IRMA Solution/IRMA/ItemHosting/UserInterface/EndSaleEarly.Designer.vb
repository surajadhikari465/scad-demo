<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class EndSaleEarly
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
        Me.components = New System.ComponentModel.Container()
        Dim Appearance15 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridBand1 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1)
        Dim UltraGridColumn1 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("ID")
        Dim UltraGridColumn2 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Store Name", -1, Nothing, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, False)
        Dim UltraGridColumn3 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Start Date")
        Dim UltraGridColumn4 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Type")
        Dim UltraGridColumn5 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("POS Price")
        Dim UltraGridColumn6 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Price")
        Dim UltraGridColumn7 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Sale End")
        Dim UltraGridColumn8 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Priority")
        Dim Appearance16 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance17 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
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
        Dim UltraDataColumn1 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("ID")
        Dim UltraDataColumn2 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Store Name")
        Dim UltraDataColumn3 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Start Date")
        Dim UltraDataColumn4 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Type")
        Dim UltraDataColumn5 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("POS Price")
        Dim UltraDataColumn6 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Price")
        Dim UltraDataColumn7 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Sale End")
        Dim UltraDataColumn8 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Batch Status")
        Dim UltraDataColumn9 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Priority")
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(EndSaleEarly))
        Me.dtpStartDate = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor()
        Me.Label_StartDate = New System.Windows.Forms.Label()
        Me.GroupBox_RegularPrice = New System.Windows.Forms.GroupBox()
        Me.txtPOSPrice = New Infragistics.Win.UltraWinEditors.UltraNumericEditor()
        Me.txtMultiple = New System.Windows.Forms.TextBox()
        Me._lblSlash_3 = New System.Windows.Forms.Label()
        Me.Label_POSPrice = New System.Windows.Forms.Label()
        Me.UltraGrid_SaleData = New Infragistics.Win.UltraWinGrid.UltraGrid()
        Me.UltraDataSource1 = New Infragistics.Win.UltraWinDataSource.UltraDataSource(Me.components)
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.GroupBox_Step1 = New System.Windows.Forms.GroupBox()
        Me.Button_Exit = New System.Windows.Forms.Button()
        Me.Button_ApplyChanges = New System.Windows.Forms.Button()
        CType(Me.dtpStartDate, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox_RegularPrice.SuspendLayout()
        CType(Me.txtPOSPrice, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.UltraGrid_SaleData, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.UltraDataSource1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox_Step1.SuspendLayout()
        Me.SuspendLayout()
        '
        'dtpStartDate
        '
        Me.dtpStartDate.Location = New System.Drawing.Point(123, 27)
        Me.dtpStartDate.Name = "dtpStartDate"
        Me.dtpStartDate.Size = New System.Drawing.Size(85, 21)
        Me.dtpStartDate.TabIndex = 2
        '
        'Label_StartDate
        '
        Me.Label_StartDate.BackColor = System.Drawing.Color.Transparent
        Me.Label_StartDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label_StartDate.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.Label_StartDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label_StartDate.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label_StartDate.Location = New System.Drawing.Point(12, 31)
        Me.Label_StartDate.Name = "Label_StartDate"
        Me.Label_StartDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label_StartDate.Size = New System.Drawing.Size(95, 17)
        Me.Label_StartDate.TabIndex = 121
        Me.Label_StartDate.Text = "Effective Date :"
        Me.Label_StartDate.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'GroupBox_RegularPrice
        '
        Me.GroupBox_RegularPrice.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox_RegularPrice.Controls.Add(Me.txtPOSPrice)
        Me.GroupBox_RegularPrice.Controls.Add(Me.txtMultiple)
        Me.GroupBox_RegularPrice.Controls.Add(Me._lblSlash_3)
        Me.GroupBox_RegularPrice.Controls.Add(Me.Label_POSPrice)
        Me.GroupBox_RegularPrice.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.GroupBox_RegularPrice.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBox_RegularPrice.Location = New System.Drawing.Point(439, 155)
        Me.GroupBox_RegularPrice.Name = "GroupBox_RegularPrice"
        Me.GroupBox_RegularPrice.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBox_RegularPrice.Size = New System.Drawing.Size(271, 60)
        Me.GroupBox_RegularPrice.TabIndex = 3
        Me.GroupBox_RegularPrice.TabStop = False
        Me.GroupBox_RegularPrice.Text = "[Optional] Adjust Regular Price"
        '
        'txtPOSPrice
        '
        Me.txtPOSPrice.Location = New System.Drawing.Point(141, 27)
        Me.txtPOSPrice.MaskClipMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeBoth
        Me.txtPOSPrice.MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeLiterals
        Me.txtPOSPrice.MaskInput = "{double:6.2}"
        Me.txtPOSPrice.MaxValue = 100000
        Me.txtPOSPrice.MinValue = 0
        Me.txtPOSPrice.Name = "txtPOSPrice"
        Me.txtPOSPrice.Nullable = True
        Me.txtPOSPrice.NullText = "0.00"
        Me.txtPOSPrice.NumericType = Infragistics.Win.UltraWinEditors.NumericType.[Double]
        Me.txtPOSPrice.PromptChar = Global.Microsoft.VisualBasic.ChrW(32)
        Me.txtPOSPrice.Size = New System.Drawing.Size(70, 21)
        Me.txtPOSPrice.TabIndex = 4
        '
        'txtMultiple
        '
        Me.txtMultiple.AcceptsReturn = True
        Me.txtMultiple.BackColor = System.Drawing.SystemColors.Window
        Me.txtMultiple.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtMultiple.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.txtMultiple.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMultiple.Location = New System.Drawing.Point(101, 27)
        Me.txtMultiple.MaxLength = 2
        Me.txtMultiple.Name = "txtMultiple"
        Me.txtMultiple.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMultiple.Size = New System.Drawing.Size(25, 20)
        Me.txtMultiple.TabIndex = 1
        Me.txtMultiple.TabStop = False
        Me.txtMultiple.Tag = "Number"
        Me.txtMultiple.Text = "1"
        Me.txtMultiple.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        '_lblSlash_3
        '
        Me._lblSlash_3.BackColor = System.Drawing.SystemColors.Control
        Me._lblSlash_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblSlash_3.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me._lblSlash_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblSlash_3.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me._lblSlash_3.Location = New System.Drawing.Point(122, 32)
        Me._lblSlash_3.Name = "_lblSlash_3"
        Me._lblSlash_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblSlash_3.Size = New System.Drawing.Size(25, 17)
        Me._lblSlash_3.TabIndex = 2
        Me._lblSlash_3.Text = "/"
        Me._lblSlash_3.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Label_POSPrice
        '
        Me.Label_POSPrice.BackColor = System.Drawing.Color.Transparent
        Me.Label_POSPrice.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label_POSPrice.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.Label_POSPrice.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label_POSPrice.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label_POSPrice.Location = New System.Drawing.Point(19, 30)
        Me.Label_POSPrice.Name = "Label_POSPrice"
        Me.Label_POSPrice.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label_POSPrice.Size = New System.Drawing.Size(75, 17)
        Me.Label_POSPrice.TabIndex = 0
        Me.Label_POSPrice.Text = "POS Price :"
        Me.Label_POSPrice.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'UltraGrid_SaleData
        '
        Me.UltraGrid_SaleData.DataMember = Nothing
        Appearance15.BackColor = System.Drawing.SystemColors.ControlLight
        Appearance15.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.UltraGrid_SaleData.DisplayLayout.Appearance = Appearance15
        Me.UltraGrid_SaleData.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn
        UltraGridColumn1.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn1.Header.VisiblePosition = 0
        UltraGridColumn1.Hidden = True
        UltraGridColumn2.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn2.Header.VisiblePosition = 1
        UltraGridColumn2.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(159, 0)
        UltraGridColumn3.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn3.Header.VisiblePosition = 2
        UltraGridColumn3.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(80, 0)
        UltraGridColumn4.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn4.Header.VisiblePosition = 3
        UltraGridColumn4.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(82, 0)
        UltraGridColumn5.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn5.Header.VisiblePosition = 4
        UltraGridColumn6.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn6.Header.VisiblePosition = 5
        UltraGridColumn6.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(80, 0)
        UltraGridColumn7.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn7.Header.VisiblePosition = 7
        UltraGridColumn7.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(72, 0)
        UltraGridColumn8.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        Appearance16.TextHAlignAsString = "Center"
        UltraGridColumn8.CellAppearance = Appearance16
        UltraGridColumn8.Header.VisiblePosition = 6
        UltraGridColumn8.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(60, 0)
        UltraGridBand1.Columns.AddRange(New Object() {UltraGridColumn1, UltraGridColumn2, UltraGridColumn3, UltraGridColumn4, UltraGridColumn5, UltraGridColumn6, UltraGridColumn7, UltraGridColumn8})
        Me.UltraGrid_SaleData.DisplayLayout.BandsSerializer.Add(UltraGridBand1)
        Me.UltraGrid_SaleData.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Appearance17.FontData.BoldAsString = "True"
        Me.UltraGrid_SaleData.DisplayLayout.CaptionAppearance = Appearance17
        Me.UltraGrid_SaleData.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Appearance18.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance18.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance18.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance18.BorderColor = System.Drawing.SystemColors.Window
        Me.UltraGrid_SaleData.DisplayLayout.GroupByBox.Appearance = Appearance18
        Appearance19.ForeColor = System.Drawing.SystemColors.GrayText
        Me.UltraGrid_SaleData.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance19
        Me.UltraGrid_SaleData.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.UltraGrid_SaleData.DisplayLayout.GroupByBox.Hidden = True
        Appearance20.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance20.BackColor2 = System.Drawing.SystemColors.Control
        Appearance20.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance20.ForeColor = System.Drawing.SystemColors.GrayText
        Me.UltraGrid_SaleData.DisplayLayout.GroupByBox.PromptAppearance = Appearance20
        Me.UltraGrid_SaleData.DisplayLayout.MaxColScrollRegions = 1
        Me.UltraGrid_SaleData.DisplayLayout.MaxRowScrollRegions = 1
        Me.UltraGrid_SaleData.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.UltraGrid_SaleData.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance21.BackColor = System.Drawing.SystemColors.Window
        Me.UltraGrid_SaleData.DisplayLayout.Override.CardAreaAppearance = Appearance21
        Appearance22.BorderColor = System.Drawing.Color.Silver
        Appearance22.FontData.BoldAsString = "True"
        Appearance22.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.UltraGrid_SaleData.DisplayLayout.Override.CellAppearance = Appearance22
        Me.UltraGrid_SaleData.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.UltraGrid_SaleData.DisplayLayout.Override.CellPadding = 0
        Appearance23.FontData.BoldAsString = "True"
        Me.UltraGrid_SaleData.DisplayLayout.Override.FixedHeaderAppearance = Appearance23
        Appearance24.BackColor = System.Drawing.SystemColors.Control
        Appearance24.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance24.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance24.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance24.BorderColor = System.Drawing.SystemColors.Window
        Me.UltraGrid_SaleData.DisplayLayout.Override.GroupByRowAppearance = Appearance24
        Appearance25.FontData.BoldAsString = "True"
        Appearance25.TextHAlignAsString = "Left"
        Me.UltraGrid_SaleData.DisplayLayout.Override.HeaderAppearance = Appearance25
        Me.UltraGrid_SaleData.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle
        Me.UltraGrid_SaleData.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance26.BackColor = System.Drawing.SystemColors.Control
        Me.UltraGrid_SaleData.DisplayLayout.Override.RowAlternateAppearance = Appearance26
        Appearance27.BackColor = System.Drawing.SystemColors.Window
        Appearance27.BorderColor = System.Drawing.Color.Silver
        Me.UltraGrid_SaleData.DisplayLayout.Override.RowAppearance = Appearance27
        Me.UltraGrid_SaleData.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[False]
        Me.UltraGrid_SaleData.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed
        Me.UltraGrid_SaleData.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.UltraGrid_SaleData.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.UltraGrid_SaleData.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.None
        Appearance28.BackColor = System.Drawing.SystemColors.ControlLight
        Me.UltraGrid_SaleData.DisplayLayout.Override.TemplateAddRowAppearance = Appearance28
        Me.UltraGrid_SaleData.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.UltraGrid_SaleData.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.UltraGrid_SaleData.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand
        Me.UltraGrid_SaleData.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        Me.UltraGrid_SaleData.Font = New System.Drawing.Font("Arial", 8.0!)
        Me.UltraGrid_SaleData.Location = New System.Drawing.Point(13, 19)
        Me.UltraGrid_SaleData.Name = "UltraGrid_SaleData"
        Me.UltraGrid_SaleData.Size = New System.Drawing.Size(668, 97)
        Me.UltraGrid_SaleData.TabIndex = 124
        Me.UltraGrid_SaleData.Text = "History"
        '
        'UltraDataSource1
        '
        UltraDataColumn3.DataType = GetType(Date)
        UltraDataColumn7.DataType = GetType(Date)
        Me.UltraDataSource1.Band.Columns.AddRange(New Object() {UltraDataColumn1, UltraDataColumn2, UltraDataColumn3, UltraDataColumn4, UltraDataColumn5, UltraDataColumn6, UltraDataColumn7, UltraDataColumn8, UltraDataColumn9})
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.dtpStartDate)
        Me.GroupBox1.Controls.Add(Me.Label_StartDate)
        Me.GroupBox1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.GroupBox1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBox1.Location = New System.Drawing.Point(18, 155)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(265, 60)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Provide the sale's NEW end date :"
        '
        'GroupBox_Step1
        '
        Me.GroupBox_Step1.Controls.Add(Me.UltraGrid_SaleData)
        Me.GroupBox_Step1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.GroupBox_Step1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBox_Step1.Location = New System.Drawing.Point(18, 21)
        Me.GroupBox_Step1.Name = "GroupBox_Step1"
        Me.GroupBox_Step1.Size = New System.Drawing.Size(692, 128)
        Me.GroupBox_Step1.TabIndex = 0
        Me.GroupBox_Step1.TabStop = False
        Me.GroupBox_Step1.Text = "You may ONLY end the current sale on the POS early [selected] :"
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
        Me.Button_Exit.Location = New System.Drawing.Point(670, 224)
        Me.Button_Exit.Name = "Button_Exit"
        Me.Button_Exit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button_Exit.Size = New System.Drawing.Size(41, 41)
        Me.Button_Exit.TabIndex = 6
        Me.Button_Exit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.Button_Exit.UseVisualStyleBackColor = False
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
        Me.Button_ApplyChanges.Location = New System.Drawing.Point(623, 224)
        Me.Button_ApplyChanges.Name = "Button_ApplyChanges"
        Me.Button_ApplyChanges.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button_ApplyChanges.Size = New System.Drawing.Size(41, 41)
        Me.Button_ApplyChanges.TabIndex = 5
        Me.Button_ApplyChanges.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.Button_ApplyChanges.UseVisualStyleBackColor = False
        '
        'EndSaleEarly
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(723, 277)
        Me.Controls.Add(Me.Button_ApplyChanges)
        Me.Controls.Add(Me.Button_Exit)
        Me.Controls.Add(Me.GroupBox_Step1)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.GroupBox_RegularPrice)
        Me.Name = "EndSaleEarly"
        Me.Text = "End Sale Early"
        CType(Me.dtpStartDate, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox_RegularPrice.ResumeLayout(False)
        Me.GroupBox_RegularPrice.PerformLayout()
        CType(Me.txtPOSPrice, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.UltraGrid_SaleData, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.UltraDataSource1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox_Step1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents dtpStartDate As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
    Public WithEvents Label_StartDate As System.Windows.Forms.Label
    Public WithEvents GroupBox_RegularPrice As System.Windows.Forms.GroupBox
    Friend WithEvents txtPOSPrice As Infragistics.Win.UltraWinEditors.UltraNumericEditor
    Public WithEvents txtMultiple As System.Windows.Forms.TextBox
    Public WithEvents _lblSlash_3 As System.Windows.Forms.Label
    Public WithEvents Label_POSPrice As System.Windows.Forms.Label
    Friend WithEvents UltraGrid_SaleData As Infragistics.Win.UltraWinGrid.UltraGrid
    Friend WithEvents UltraDataSource1 As Infragistics.Win.UltraWinDataSource.UltraDataSource
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox_Step1 As System.Windows.Forms.GroupBox
    Public WithEvents Button_Exit As System.Windows.Forms.Button
    Public WithEvents Button_ApplyChanges As System.Windows.Forms.Button
End Class
