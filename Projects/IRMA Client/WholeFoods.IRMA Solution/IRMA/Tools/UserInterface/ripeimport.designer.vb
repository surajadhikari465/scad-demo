<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmRIPEImportSelection
#Region "Windows Form Designer generated code "
    <System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
        MyBase.New()
        'This call is required by the Windows Form Designer.
        IsInitializing = True
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
    Public WithEvents cmbRipeZone As System.Windows.Forms.ComboBox
    Public WithEvents _optSelection_2 As System.Windows.Forms.RadioButton
    Public WithEvents _optSelection_1 As System.Windows.Forms.RadioButton
    Public WithEvents _optSelection_0 As System.Windows.Forms.RadioButton
    Public WithEvents Frame1 As System.Windows.Forms.GroupBox
    Public WithEvents cmdImport As System.Windows.Forms.Button
    Public WithEvents cmdExit As System.Windows.Forms.Button
    Public WithEvents txtDistribution As System.Windows.Forms.TextBox
    Public WithEvents cmbFromLocation As System.Windows.Forms.ComboBox
    'Public WithEvents rptSummary As AxCrystal.AxCrystalReport
    'Public WithEvents rptInvoice As AxCrystal.AxCrystalReport
    'Public WithEvents rptUnmappedItems As AxCrystal.AxCrystalReport
    Public WithEvents lblTotInv As System.Windows.Forms.Label
    Public WithEvents lblCurInv As System.Windows.Forms.Label
    Public WithEvents _lblLabel_16 As System.Windows.Forms.Label
    Public WithEvents _lblLabel_1 As System.Windows.Forms.Label
    Public WithEvents lblLabel As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
    Public WithEvents optSelection As Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmRIPEImportSelection))
        Dim Appearance1 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim UltraGridBand1 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1)
        Dim UltraGridColumn1 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyName")
        Dim UltraGridColumn2 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("CustomerID")
        Dim UltraGridColumn3 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("StoreNo")
        Dim UltraGridColumn4 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("ZoneID")
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
        Me.cmdImport = New System.Windows.Forms.Button
        Me.cmdExit = New System.Windows.Forms.Button
        Me.Frame1 = New System.Windows.Forms.GroupBox
        Me.cmbRipeZone = New System.Windows.Forms.ComboBox
        Me._optSelection_2 = New System.Windows.Forms.RadioButton
        Me._optSelection_1 = New System.Windows.Forms.RadioButton
        Me._optSelection_0 = New System.Windows.Forms.RadioButton
        Me.txtDistribution = New System.Windows.Forms.TextBox
        Me.cmbFromLocation = New System.Windows.Forms.ComboBox
        Me.lblTotInv = New System.Windows.Forms.Label
        Me.lblCurInv = New System.Windows.Forms.Label
        Me._lblLabel_16 = New System.Windows.Forms.Label
        Me._lblLabel_1 = New System.Windows.Forms.Label
        Me.lblLabel = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(Me.components)
        Me.optSelection = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(Me.components)
        Me.ugrdRecipeCustomer = New Infragistics.Win.UltraWinGrid.UltraGrid
        Me.Frame1.SuspendLayout()
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.optSelection, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ugrdRecipeCustomer, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdImport
        '
        Me.cmdImport.BackColor = System.Drawing.SystemColors.Control
        Me.cmdImport.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdImport.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdImport.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdImport.Image = CType(resources.GetObject("cmdImport.Image"), System.Drawing.Image)
        Me.cmdImport.Location = New System.Drawing.Point(262, 354)
        Me.cmdImport.Name = "cmdImport"
        Me.cmdImport.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdImport.Size = New System.Drawing.Size(41, 41)
        Me.cmdImport.TabIndex = 4
        Me.cmdImport.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdImport, "Import")
        Me.cmdImport.UseVisualStyleBackColor = False
        '
        'cmdExit
        '
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Image = CType(resources.GetObject("cmdExit.Image"), System.Drawing.Image)
        Me.cmdExit.Location = New System.Drawing.Point(310, 354)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(41, 41)
        Me.cmdExit.TabIndex = 5
        Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdExit, "Exit")
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me.cmbRipeZone)
        Me.Frame1.Controls.Add(Me._optSelection_2)
        Me.Frame1.Controls.Add(Me._optSelection_1)
        Me.Frame1.Controls.Add(Me._optSelection_0)
        Me.Frame1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Location = New System.Drawing.Point(7, 64)
        Me.Frame1.Name = "Frame1"
        Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame1.Size = New System.Drawing.Size(353, 44)
        Me.Frame1.TabIndex = 6
        Me.Frame1.TabStop = False
        Me.Frame1.Text = "Store"
        '
        'cmbRipeZone
        '
        Me.cmbRipeZone.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbRipeZone.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbRipeZone.BackColor = System.Drawing.SystemColors.Window
        Me.cmbRipeZone.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbRipeZone.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbRipeZone.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbRipeZone.Location = New System.Drawing.Point(221, 15)
        Me.cmbRipeZone.Name = "cmbRipeZone"
        Me.cmbRipeZone.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbRipeZone.Size = New System.Drawing.Size(125, 22)
        Me.cmbRipeZone.Sorted = True
        Me.cmbRipeZone.TabIndex = 10
        '
        '_optSelection_2
        '
        Me._optSelection_2.BackColor = System.Drawing.SystemColors.Control
        Me._optSelection_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._optSelection_2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optSelection_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optSelection.SetIndex(Me._optSelection_2, CType(2, Short))
        Me._optSelection_2.Location = New System.Drawing.Point(139, 18)
        Me._optSelection_2.Name = "_optSelection_2"
        Me._optSelection_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optSelection_2.Size = New System.Drawing.Size(83, 17)
        Me._optSelection_2.TabIndex = 9
        Me._optSelection_2.TabStop = True
        Me._optSelection_2.Text = "Ripe Zone"
        Me._optSelection_2.UseVisualStyleBackColor = False
        '
        '_optSelection_1
        '
        Me._optSelection_1.BackColor = System.Drawing.SystemColors.Control
        Me._optSelection_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._optSelection_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optSelection_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optSelection.SetIndex(Me._optSelection_1, CType(1, Short))
        Me._optSelection_1.Location = New System.Drawing.Point(86, 19)
        Me._optSelection_1.Name = "_optSelection_1"
        Me._optSelection_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optSelection_1.Size = New System.Drawing.Size(44, 17)
        Me._optSelection_1.TabIndex = 8
        Me._optSelection_1.TabStop = True
        Me._optSelection_1.Text = "All"
        Me._optSelection_1.UseVisualStyleBackColor = False
        '
        '_optSelection_0
        '
        Me._optSelection_0.BackColor = System.Drawing.SystemColors.Control
        Me._optSelection_0.Checked = True
        Me._optSelection_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._optSelection_0.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optSelection_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optSelection.SetIndex(Me._optSelection_0, CType(0, Short))
        Me._optSelection_0.Location = New System.Drawing.Point(9, 19)
        Me._optSelection_0.Name = "_optSelection_0"
        Me._optSelection_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optSelection_0.Size = New System.Drawing.Size(65, 17)
        Me._optSelection_0.TabIndex = 7
        Me._optSelection_0.TabStop = True
        Me._optSelection_0.Text = "Manual"
        Me._optSelection_0.UseVisualStyleBackColor = False
        '
        'txtDistribution
        '
        Me.txtDistribution.AcceptsReturn = True
        Me.txtDistribution.BackColor = System.Drawing.SystemColors.Window
        Me.txtDistribution.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDistribution.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDistribution.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDistribution.Location = New System.Drawing.Point(185, 6)
        Me.txtDistribution.MaxLength = 10
        Me.txtDistribution.Name = "txtDistribution"
        Me.txtDistribution.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDistribution.Size = New System.Drawing.Size(81, 20)
        Me.txtDistribution.TabIndex = 1
        Me.txtDistribution.Tag = "Date"
        '
        'cmbFromLocation
        '
        Me.cmbFromLocation.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbFromLocation.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbFromLocation.BackColor = System.Drawing.SystemColors.Window
        Me.cmbFromLocation.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbFromLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbFromLocation.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbFromLocation.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbFromLocation.Location = New System.Drawing.Point(118, 38)
        Me.cmbFromLocation.Name = "cmbFromLocation"
        Me.cmbFromLocation.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbFromLocation.Size = New System.Drawing.Size(207, 22)
        Me.cmbFromLocation.Sorted = True
        Me.cmbFromLocation.TabIndex = 3
        '
        'lblTotInv
        '
        Me.lblTotInv.AutoSize = True
        Me.lblTotInv.BackColor = System.Drawing.SystemColors.Control
        Me.lblTotInv.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTotInv.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotInv.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTotInv.Location = New System.Drawing.Point(161, 370)
        Me.lblTotInv.Name = "lblTotInv"
        Me.lblTotInv.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTotInv.Size = New System.Drawing.Size(80, 14)
        Me.lblTotInv.TabIndex = 13
        Me.lblTotInv.Text = " of Y Invoices"
        Me.lblTotInv.Visible = False
        '
        'lblCurInv
        '
        Me.lblCurInv.AutoSize = True
        Me.lblCurInv.BackColor = System.Drawing.SystemColors.Control
        Me.lblCurInv.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCurInv.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurInv.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCurInv.Location = New System.Drawing.Point(77, 370)
        Me.lblCurInv.Name = "lblCurInv"
        Me.lblCurInv.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCurInv.Size = New System.Drawing.Size(89, 14)
        Me.lblCurInv.TabIndex = 12
        Me.lblCurInv.Text = "Now Printing x "
        Me.lblCurInv.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me.lblCurInv.Visible = False
        '
        '_lblLabel_16
        '
        Me._lblLabel_16.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_16.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_16.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_16.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_16, CType(16, Short))
        Me._lblLabel_16.Location = New System.Drawing.Point(73, 6)
        Me._lblLabel_16.Name = "_lblLabel_16"
        Me._lblLabel_16.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_16.Size = New System.Drawing.Size(105, 17)
        Me._lblLabel_16.TabIndex = 0
        Me._lblLabel_16.Text = "Distribution Date :"
        Me._lblLabel_16.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_1
        '
        Me._lblLabel_1.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_1, CType(1, Short))
        Me._lblLabel_1.Location = New System.Drawing.Point(25, 40)
        Me._lblLabel_1.Name = "_lblLabel_1"
        Me._lblLabel_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_1.Size = New System.Drawing.Size(83, 17)
        Me._lblLabel_1.TabIndex = 2
        Me._lblLabel_1.Text = "From Facility:"
        Me._lblLabel_1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'optSelection
        '
        '
        'ugrdRecipeCustomer
        '
        Appearance1.BackColor = System.Drawing.SystemColors.ControlLight
        Appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.ugrdRecipeCustomer.DisplayLayout.Appearance = Appearance1
        Me.ugrdRecipeCustomer.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn
        UltraGridColumn1.Header.VisiblePosition = 0
        UltraGridColumn2.Header.VisiblePosition = 1
        UltraGridColumn3.Header.VisiblePosition = 2
        UltraGridColumn4.Header.VisiblePosition = 3
        UltraGridBand1.Columns.AddRange(New Object() {UltraGridColumn1, UltraGridColumn2, UltraGridColumn3, UltraGridColumn4})
        Me.ugrdRecipeCustomer.DisplayLayout.BandsSerializer.Add(UltraGridBand1)
        Me.ugrdRecipeCustomer.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Appearance2.FontData.BoldAsString = "True"
        Me.ugrdRecipeCustomer.DisplayLayout.CaptionAppearance = Appearance2
        Me.ugrdRecipeCustomer.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[True]
        Appearance3.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance3.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance3.BorderColor = System.Drawing.SystemColors.Window
        Me.ugrdRecipeCustomer.DisplayLayout.GroupByBox.Appearance = Appearance3
        Appearance4.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugrdRecipeCustomer.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance4
        Me.ugrdRecipeCustomer.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.ugrdRecipeCustomer.DisplayLayout.GroupByBox.Hidden = True
        Appearance5.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance5.BackColor2 = System.Drawing.SystemColors.Control
        Appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance5.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugrdRecipeCustomer.DisplayLayout.GroupByBox.PromptAppearance = Appearance5
        Me.ugrdRecipeCustomer.DisplayLayout.MaxColScrollRegions = 1
        Me.ugrdRecipeCustomer.DisplayLayout.MaxRowScrollRegions = 1
        Appearance6.BackColor = System.Drawing.SystemColors.Window
        Appearance6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ugrdRecipeCustomer.DisplayLayout.Override.ActiveCellAppearance = Appearance6
        Appearance7.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugrdRecipeCustomer.DisplayLayout.Override.ActiveRowAppearance = Appearance7
        Me.ugrdRecipeCustomer.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.ugrdRecipeCustomer.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance8.BackColor = System.Drawing.SystemColors.Window
        Me.ugrdRecipeCustomer.DisplayLayout.Override.CardAreaAppearance = Appearance8
        Appearance9.BorderColor = System.Drawing.Color.Silver
        Appearance9.FontData.BoldAsString = "True"
        Appearance9.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.ugrdRecipeCustomer.DisplayLayout.Override.CellAppearance = Appearance9
        Me.ugrdRecipeCustomer.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.ugrdRecipeCustomer.DisplayLayout.Override.CellPadding = 0
        Appearance10.FontData.BoldAsString = "True"
        Me.ugrdRecipeCustomer.DisplayLayout.Override.FixedHeaderAppearance = Appearance10
        Appearance11.BackColor = System.Drawing.SystemColors.Control
        Appearance11.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance11.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance11.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance11.BorderColor = System.Drawing.SystemColors.Window
        Me.ugrdRecipeCustomer.DisplayLayout.Override.GroupByRowAppearance = Appearance11
        Appearance12.FontData.BoldAsString = "True"
        Appearance12.TextHAlignAsString = "Left"
        Me.ugrdRecipeCustomer.DisplayLayout.Override.HeaderAppearance = Appearance12
        Me.ugrdRecipeCustomer.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle
        Me.ugrdRecipeCustomer.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance13.BackColor = System.Drawing.SystemColors.Control
        Me.ugrdRecipeCustomer.DisplayLayout.Override.RowAlternateAppearance = Appearance13
        Appearance14.BackColor = System.Drawing.SystemColors.Window
        Appearance14.BorderColor = System.Drawing.Color.Silver
        Me.ugrdRecipeCustomer.DisplayLayout.Override.RowAppearance = Appearance14
        Me.ugrdRecipeCustomer.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[True]
        Me.ugrdRecipeCustomer.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed
        Me.ugrdRecipeCustomer.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.ugrdRecipeCustomer.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.ugrdRecipeCustomer.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Extended
        Appearance15.BackColor = System.Drawing.SystemColors.ControlLight
        Me.ugrdRecipeCustomer.DisplayLayout.Override.TemplateAddRowAppearance = Appearance15
        Me.ugrdRecipeCustomer.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.ugrdRecipeCustomer.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.ugrdRecipeCustomer.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand
        Me.ugrdRecipeCustomer.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        Me.ugrdRecipeCustomer.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ugrdRecipeCustomer.Location = New System.Drawing.Point(9, 117)
        Me.ugrdRecipeCustomer.Name = "ugrdRecipeCustomer"
        Me.ugrdRecipeCustomer.Size = New System.Drawing.Size(349, 229)
        Me.ugrdRecipeCustomer.TabIndex = 26
        Me.ugrdRecipeCustomer.Text = "Search Results"
        '
        'frmRIPEImportSelection
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(368, 407)
        Me.Controls.Add(Me.ugrdRecipeCustomer)
        Me.Controls.Add(Me.Frame1)
        Me.Controls.Add(Me.cmdImport)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.txtDistribution)
        Me.Controls.Add(Me.cmbFromLocation)
        Me.Controls.Add(Me.lblTotInv)
        Me.Controls.Add(Me.lblCurInv)
        Me.Controls.Add(Me._lblLabel_16)
        Me.Controls.Add(Me._lblLabel_1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Location = New System.Drawing.Point(397, 233)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmRIPEImportSelection"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "RIPE Orders Import"
        Me.Frame1.ResumeLayout(False)
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.optSelection, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ugrdRecipeCustomer, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ugrdRecipeCustomer As Infragistics.Win.UltraWinGrid.UltraGrid
#End Region
End Class