<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class InvoiceControlGroup
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
        Me.components = New System.ComponentModel.Container
        Dim Appearance1 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
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
        Dim UltraDataColumn1 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Invoice Date")
        Dim UltraDataColumn2 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Invoice #")
        Dim UltraDataColumn3 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Invoice Amt.")
        Dim UltraDataColumn4 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("PO #")
        Dim UltraDataColumn5 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Vendor ID")
        Dim UltraDataColumn6 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Credit Inv.")
        Dim UltraDataColumn7 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("3rd Party Fr.")
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(InvoiceControlGroup))
        Me.Label_ControlGroupID = New System.Windows.Forms.Label
        Me.Label_ControlGroupStatus = New System.Windows.Forms.Label
        Me.TextBox_ControlGroupID = New System.Windows.Forms.TextBox
        Me.TextBox_ControlGroupStatus = New System.Windows.Forms.TextBox
        Me.Label_ExpectedNumInvoices = New System.Windows.Forms.Label
        Me.Label_ExpectedGrossAmt = New System.Windows.Forms.Label
        Me.GroupBox_Invoices = New System.Windows.Forms.GroupBox
        Me.TextBox_DiffGrossAmt = New System.Windows.Forms.TextBox
        Me.TextBox_DiffNumInvoices = New System.Windows.Forms.TextBox
        Me.Label_DiffGrossAmt = New System.Windows.Forms.Label
        Me.Label_DiffNumInvoices = New System.Windows.Forms.Label
        Me.TextBox_EnteredGrossAmt = New System.Windows.Forms.TextBox
        Me.TextBox_EnteredNumInvoices = New System.Windows.Forms.TextBox
        Me.Label_EnteredGrossAmt = New System.Windows.Forms.Label
        Me.Label_EnteredNumInvoices = New System.Windows.Forms.Label
        Me.Button_EditInvoice = New System.Windows.Forms.Button
        Me.Button_DeleteInvoice = New System.Windows.Forms.Button
        Me.UltraGrid_Invoices = New Infragistics.Win.UltraWinGrid.UltraGrid
        Me.Button_AddInvoice = New System.Windows.Forms.Button
        Me.UltraDataSource1 = New Infragistics.Win.UltraWinDataSource.UltraDataSource(Me.components)
        Me.Button_SearchControlGroup = New System.Windows.Forms.Button
        Me.Button_AddControlGroup = New System.Windows.Forms.Button
        Me.Button_Exit = New System.Windows.Forms.Button
        Me.Button_CloseControlGroup = New System.Windows.Forms.Button
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.UltraNumericEditor_ExpectedGrossAmt = New Infragistics.Win.UltraWinEditors.UltraNumericEditor
        Me.UltraNumericEditor_ExpectedNumInvoices = New Infragistics.Win.UltraWinEditors.UltraNumericEditor
        Me.GroupBox_Invoices.SuspendLayout()
        CType(Me.UltraGrid_Invoices, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.UltraDataSource1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.UltraNumericEditor_ExpectedGrossAmt, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.UltraNumericEditor_ExpectedNumInvoices, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label_ControlGroupID
        '
        Me.Label_ControlGroupID.AutoSize = True
        Me.Label_ControlGroupID.Location = New System.Drawing.Point(32, 9)
        Me.Label_ControlGroupID.Name = "Label_ControlGroupID"
        Me.Label_ControlGroupID.Size = New System.Drawing.Size(92, 13)
        Me.Label_ControlGroupID.TabIndex = 0
        Me.Label_ControlGroupID.Text = "Control Group ID :"
        '
        'Label_ControlGroupStatus
        '
        Me.Label_ControlGroupStatus.AutoSize = True
        Me.Label_ControlGroupStatus.Location = New System.Drawing.Point(284, 9)
        Me.Label_ControlGroupStatus.Name = "Label_ControlGroupStatus"
        Me.Label_ControlGroupStatus.Size = New System.Drawing.Size(111, 13)
        Me.Label_ControlGroupStatus.TabIndex = 1
        Me.Label_ControlGroupStatus.Text = "Control Group Status :"
        '
        'TextBox_ControlGroupID
        '
        Me.TextBox_ControlGroupID.Location = New System.Drawing.Point(130, 6)
        Me.TextBox_ControlGroupID.Name = "TextBox_ControlGroupID"
        Me.TextBox_ControlGroupID.ReadOnly = True
        Me.TextBox_ControlGroupID.Size = New System.Drawing.Size(100, 20)
        Me.TextBox_ControlGroupID.TabIndex = 2
        '
        'TextBox_ControlGroupStatus
        '
        Me.TextBox_ControlGroupStatus.Location = New System.Drawing.Point(401, 6)
        Me.TextBox_ControlGroupStatus.Name = "TextBox_ControlGroupStatus"
        Me.TextBox_ControlGroupStatus.ReadOnly = True
        Me.TextBox_ControlGroupStatus.Size = New System.Drawing.Size(100, 20)
        Me.TextBox_ControlGroupStatus.TabIndex = 3
        '
        'Label_ExpectedNumInvoices
        '
        Me.Label_ExpectedNumInvoices.AutoSize = True
        Me.Label_ExpectedNumInvoices.Location = New System.Drawing.Point(13, 36)
        Me.Label_ExpectedNumInvoices.Name = "Label_ExpectedNumInvoices"
        Me.Label_ExpectedNumInvoices.Size = New System.Drawing.Size(111, 13)
        Me.Label_ExpectedNumInvoices.TabIndex = 4
        Me.Label_ExpectedNumInvoices.Text = "# Invoices Expected :"
        '
        'Label_ExpectedGrossAmt
        '
        Me.Label_ExpectedGrossAmt.AutoSize = True
        Me.Label_ExpectedGrossAmt.Location = New System.Drawing.Point(286, 36)
        Me.Label_ExpectedGrossAmt.Name = "Label_ExpectedGrossAmt"
        Me.Label_ExpectedGrossAmt.Size = New System.Drawing.Size(109, 13)
        Me.Label_ExpectedGrossAmt.TabIndex = 5
        Me.Label_ExpectedGrossAmt.Text = "Gross Amt Expected :"
        '
        'GroupBox_Invoices
        '
        Me.GroupBox_Invoices.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox_Invoices.Controls.Add(Me.TextBox_DiffGrossAmt)
        Me.GroupBox_Invoices.Controls.Add(Me.TextBox_DiffNumInvoices)
        Me.GroupBox_Invoices.Controls.Add(Me.Label_DiffGrossAmt)
        Me.GroupBox_Invoices.Controls.Add(Me.Label_DiffNumInvoices)
        Me.GroupBox_Invoices.Controls.Add(Me.TextBox_EnteredGrossAmt)
        Me.GroupBox_Invoices.Controls.Add(Me.TextBox_EnteredNumInvoices)
        Me.GroupBox_Invoices.Controls.Add(Me.Label_EnteredGrossAmt)
        Me.GroupBox_Invoices.Controls.Add(Me.Label_EnteredNumInvoices)
        Me.GroupBox_Invoices.Controls.Add(Me.Button_EditInvoice)
        Me.GroupBox_Invoices.Controls.Add(Me.Button_DeleteInvoice)
        Me.GroupBox_Invoices.Controls.Add(Me.UltraGrid_Invoices)
        Me.GroupBox_Invoices.Controls.Add(Me.Button_AddInvoice)
        Me.GroupBox_Invoices.Location = New System.Drawing.Point(12, 58)
        Me.GroupBox_Invoices.Name = "GroupBox_Invoices"
        Me.GroupBox_Invoices.Size = New System.Drawing.Size(624, 486)
        Me.GroupBox_Invoices.TabIndex = 6
        Me.GroupBox_Invoices.TabStop = False
        Me.GroupBox_Invoices.Text = "Invoices"
        '
        'TextBox_DiffGrossAmt
        '
        Me.TextBox_DiffGrossAmt.Location = New System.Drawing.Point(389, 37)
        Me.TextBox_DiffGrossAmt.Name = "TextBox_DiffGrossAmt"
        Me.TextBox_DiffGrossAmt.ReadOnly = True
        Me.TextBox_DiffGrossAmt.Size = New System.Drawing.Size(100, 20)
        Me.TextBox_DiffGrossAmt.TabIndex = 11
        '
        'TextBox_DiffNumInvoices
        '
        Me.TextBox_DiffNumInvoices.Location = New System.Drawing.Point(118, 37)
        Me.TextBox_DiffNumInvoices.Name = "TextBox_DiffNumInvoices"
        Me.TextBox_DiffNumInvoices.ReadOnly = True
        Me.TextBox_DiffNumInvoices.Size = New System.Drawing.Size(47, 20)
        Me.TextBox_DiffNumInvoices.TabIndex = 10
        '
        'Label_DiffGrossAmt
        '
        Me.Label_DiffGrossAmt.AutoSize = True
        Me.Label_DiffGrossAmt.Location = New System.Drawing.Point(303, 40)
        Me.Label_DiffGrossAmt.Name = "Label_DiffGrossAmt"
        Me.Label_DiffGrossAmt.Size = New System.Drawing.Size(80, 13)
        Me.Label_DiffGrossAmt.TabIndex = 13
        Me.Label_DiffGrossAmt.Text = "Gross Amt Diff :"
        '
        'Label_DiffNumInvoices
        '
        Me.Label_DiffNumInvoices.AutoSize = True
        Me.Label_DiffNumInvoices.Location = New System.Drawing.Point(30, 40)
        Me.Label_DiffNumInvoices.Name = "Label_DiffNumInvoices"
        Me.Label_DiffNumInvoices.Size = New System.Drawing.Size(82, 13)
        Me.Label_DiffNumInvoices.TabIndex = 12
        Me.Label_DiffNumInvoices.Text = "# Invoices Diff :"
        '
        'TextBox_EnteredGrossAmt
        '
        Me.TextBox_EnteredGrossAmt.Location = New System.Drawing.Point(389, 11)
        Me.TextBox_EnteredGrossAmt.Name = "TextBox_EnteredGrossAmt"
        Me.TextBox_EnteredGrossAmt.ReadOnly = True
        Me.TextBox_EnteredGrossAmt.Size = New System.Drawing.Size(100, 20)
        Me.TextBox_EnteredGrossAmt.TabIndex = 7
        '
        'TextBox_EnteredNumInvoices
        '
        Me.TextBox_EnteredNumInvoices.Location = New System.Drawing.Point(118, 11)
        Me.TextBox_EnteredNumInvoices.Name = "TextBox_EnteredNumInvoices"
        Me.TextBox_EnteredNumInvoices.ReadOnly = True
        Me.TextBox_EnteredNumInvoices.Size = New System.Drawing.Size(47, 20)
        Me.TextBox_EnteredNumInvoices.TabIndex = 6
        '
        'Label_EnteredGrossAmt
        '
        Me.Label_EnteredGrossAmt.AutoSize = True
        Me.Label_EnteredGrossAmt.Location = New System.Drawing.Point(282, 14)
        Me.Label_EnteredGrossAmt.Name = "Label_EnteredGrossAmt"
        Me.Label_EnteredGrossAmt.Size = New System.Drawing.Size(101, 13)
        Me.Label_EnteredGrossAmt.TabIndex = 9
        Me.Label_EnteredGrossAmt.Text = "Gross Amt Entered :"
        '
        'Label_EnteredNumInvoices
        '
        Me.Label_EnteredNumInvoices.AutoSize = True
        Me.Label_EnteredNumInvoices.Location = New System.Drawing.Point(9, 14)
        Me.Label_EnteredNumInvoices.Name = "Label_EnteredNumInvoices"
        Me.Label_EnteredNumInvoices.Size = New System.Drawing.Size(103, 13)
        Me.Label_EnteredNumInvoices.TabIndex = 8
        Me.Label_EnteredNumInvoices.Text = "# Invoices Entered :"
        '
        'Button_EditInvoice
        '
        Me.Button_EditInvoice.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_EditInvoice.Location = New System.Drawing.Point(543, 92)
        Me.Button_EditInvoice.Name = "Button_EditInvoice"
        Me.Button_EditInvoice.Size = New System.Drawing.Size(75, 23)
        Me.Button_EditInvoice.TabIndex = 3
        Me.Button_EditInvoice.Text = "Edit"
        Me.ToolTip1.SetToolTip(Me.Button_EditInvoice, "Delete Invoice From Control Group")
        Me.Button_EditInvoice.UseVisualStyleBackColor = True
        '
        'Button_DeleteInvoice
        '
        Me.Button_DeleteInvoice.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_DeleteInvoice.Location = New System.Drawing.Point(543, 121)
        Me.Button_DeleteInvoice.Name = "Button_DeleteInvoice"
        Me.Button_DeleteInvoice.Size = New System.Drawing.Size(75, 23)
        Me.Button_DeleteInvoice.TabIndex = 2
        Me.Button_DeleteInvoice.Text = "Delete"
        Me.ToolTip1.SetToolTip(Me.Button_DeleteInvoice, "Delete Invoice From Control Group")
        Me.Button_DeleteInvoice.UseVisualStyleBackColor = True
        '
        'UltraGrid_Invoices
        '
        Me.UltraGrid_Invoices.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Appearance1.BackColor = System.Drawing.SystemColors.Window
        Appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.UltraGrid_Invoices.DisplayLayout.Appearance = Appearance1
        Me.UltraGrid_Invoices.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        Me.UltraGrid_Invoices.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.UltraGrid_Invoices.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance2.BorderColor = System.Drawing.SystemColors.Window
        Me.UltraGrid_Invoices.DisplayLayout.GroupByBox.Appearance = Appearance2
        Appearance3.ForeColor = System.Drawing.SystemColors.GrayText
        Me.UltraGrid_Invoices.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance3
        Me.UltraGrid_Invoices.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.UltraGrid_Invoices.DisplayLayout.GroupByBox.Hidden = True
        Appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance4.BackColor2 = System.Drawing.SystemColors.Control
        Appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance4.ForeColor = System.Drawing.SystemColors.GrayText
        Me.UltraGrid_Invoices.DisplayLayout.GroupByBox.PromptAppearance = Appearance4
        Me.UltraGrid_Invoices.DisplayLayout.MaxColScrollRegions = 1
        Me.UltraGrid_Invoices.DisplayLayout.MaxRowScrollRegions = 1
        Appearance5.BackColor = System.Drawing.SystemColors.Window
        Appearance5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.UltraGrid_Invoices.DisplayLayout.Override.ActiveCellAppearance = Appearance5
        Appearance6.BackColor = System.Drawing.SystemColors.Highlight
        Appearance6.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.UltraGrid_Invoices.DisplayLayout.Override.ActiveRowAppearance = Appearance6
        Me.UltraGrid_Invoices.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No
        Me.UltraGrid_Invoices.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.[False]
        Me.UltraGrid_Invoices.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.[False]
        Me.UltraGrid_Invoices.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.UltraGrid_Invoices.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance7.BackColor = System.Drawing.SystemColors.Window
        Me.UltraGrid_Invoices.DisplayLayout.Override.CardAreaAppearance = Appearance7
        Appearance8.BorderColor = System.Drawing.Color.Silver
        Appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.UltraGrid_Invoices.DisplayLayout.Override.CellAppearance = Appearance8
        Me.UltraGrid_Invoices.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.UltraGrid_Invoices.DisplayLayout.Override.CellPadding = 0
        Appearance9.BackColor = System.Drawing.SystemColors.Control
        Appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance9.BorderColor = System.Drawing.SystemColors.Window
        Me.UltraGrid_Invoices.DisplayLayout.Override.GroupByRowAppearance = Appearance9
        Appearance10.TextHAlign = Infragistics.Win.HAlign.Left
        Me.UltraGrid_Invoices.DisplayLayout.Override.HeaderAppearance = Appearance10
        Me.UltraGrid_Invoices.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti
        Me.UltraGrid_Invoices.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance11.BackColor = System.Drawing.SystemColors.Window
        Appearance11.BorderColor = System.Drawing.Color.Silver
        Me.UltraGrid_Invoices.DisplayLayout.Override.RowAppearance = Appearance11
        Me.UltraGrid_Invoices.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[True]
        Me.UltraGrid_Invoices.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.[Single]
        Appearance12.BackColor = System.Drawing.SystemColors.ControlLight
        Me.UltraGrid_Invoices.DisplayLayout.Override.TemplateAddRowAppearance = Appearance12
        Me.UltraGrid_Invoices.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.UltraGrid_Invoices.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.UltraGrid_Invoices.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        Me.UltraGrid_Invoices.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.UltraGrid_Invoices.Location = New System.Drawing.Point(6, 63)
        Me.UltraGrid_Invoices.Name = "UltraGrid_Invoices"
        Me.UltraGrid_Invoices.Size = New System.Drawing.Size(531, 417)
        Me.UltraGrid_Invoices.TabIndex = 1
        Me.UltraGrid_Invoices.Text = "UltraGrid1"
        '
        'Button_AddInvoice
        '
        Me.Button_AddInvoice.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_AddInvoice.Location = New System.Drawing.Point(543, 63)
        Me.Button_AddInvoice.Name = "Button_AddInvoice"
        Me.Button_AddInvoice.Size = New System.Drawing.Size(75, 23)
        Me.Button_AddInvoice.TabIndex = 0
        Me.Button_AddInvoice.Text = "Add"
        Me.ToolTip1.SetToolTip(Me.Button_AddInvoice, "Add Invoice To Control Group")
        Me.Button_AddInvoice.UseVisualStyleBackColor = True
        '
        'UltraDataSource1
        '
        UltraDataColumn1.DataType = GetType(Date)
        UltraDataColumn3.DataType = GetType(Decimal)
        UltraDataColumn6.DataType = GetType(Boolean)
        UltraDataColumn7.DataType = GetType(Boolean)
        Me.UltraDataSource1.Band.Columns.AddRange(New Object() {UltraDataColumn1, UltraDataColumn2, UltraDataColumn3, UltraDataColumn4, UltraDataColumn5, UltraDataColumn6, UltraDataColumn7})
        '
        'Button_SearchControlGroup
        '
        Me.Button_SearchControlGroup.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_SearchControlGroup.BackColor = System.Drawing.SystemColors.Control
        Me.Button_SearchControlGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button_SearchControlGroup.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button_SearchControlGroup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button_SearchControlGroup.Image = CType(resources.GetObject("Button_SearchControlGroup.Image"), System.Drawing.Image)
        Me.Button_SearchControlGroup.Location = New System.Drawing.Point(541, 550)
        Me.Button_SearchControlGroup.Name = "Button_SearchControlGroup"
        Me.Button_SearchControlGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button_SearchControlGroup.Size = New System.Drawing.Size(41, 41)
        Me.Button_SearchControlGroup.TabIndex = 31
        Me.Button_SearchControlGroup.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.Button_SearchControlGroup, "Search For Control Group")
        Me.Button_SearchControlGroup.UseVisualStyleBackColor = False
        '
        'Button_AddControlGroup
        '
        Me.Button_AddControlGroup.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_AddControlGroup.BackColor = System.Drawing.SystemColors.Control
        Me.Button_AddControlGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button_AddControlGroup.Font = New System.Drawing.Font("Arial", 8.0!)
        Me.Button_AddControlGroup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button_AddControlGroup.Image = CType(resources.GetObject("Button_AddControlGroup.Image"), System.Drawing.Image)
        Me.Button_AddControlGroup.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Button_AddControlGroup.Location = New System.Drawing.Point(446, 550)
        Me.Button_AddControlGroup.Name = "Button_AddControlGroup"
        Me.Button_AddControlGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button_AddControlGroup.Size = New System.Drawing.Size(42, 41)
        Me.Button_AddControlGroup.TabIndex = 85
        Me.Button_AddControlGroup.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.Button_AddControlGroup, "Add Control Group")
        Me.Button_AddControlGroup.UseVisualStyleBackColor = False
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
        Me.Button_Exit.Location = New System.Drawing.Point(594, 550)
        Me.Button_Exit.Name = "Button_Exit"
        Me.Button_Exit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button_Exit.Size = New System.Drawing.Size(42, 41)
        Me.Button_Exit.TabIndex = 95
        Me.Button_Exit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.Button_Exit, "Exit")
        Me.Button_Exit.UseVisualStyleBackColor = False
        '
        'Button_CloseControlGroup
        '
        Me.Button_CloseControlGroup.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_CloseControlGroup.BackColor = System.Drawing.SystemColors.Control
        Me.Button_CloseControlGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button_CloseControlGroup.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button_CloseControlGroup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button_CloseControlGroup.Image = CType(resources.GetObject("Button_CloseControlGroup.Image"), System.Drawing.Image)
        Me.Button_CloseControlGroup.Location = New System.Drawing.Point(494, 550)
        Me.Button_CloseControlGroup.Name = "Button_CloseControlGroup"
        Me.Button_CloseControlGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button_CloseControlGroup.Size = New System.Drawing.Size(41, 41)
        Me.Button_CloseControlGroup.TabIndex = 96
        Me.Button_CloseControlGroup.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.Button_CloseControlGroup, "Close Control Group")
        Me.Button_CloseControlGroup.UseVisualStyleBackColor = False
        '
        'UltraNumericEditor_ExpectedGrossAmt
        '
        Me.UltraNumericEditor_ExpectedGrossAmt.AlwaysInEditMode = True
        Me.UltraNumericEditor_ExpectedGrossAmt.Location = New System.Drawing.Point(401, 32)
        Me.UltraNumericEditor_ExpectedGrossAmt.MaskInput = "{LOC}-nnnnnnnnnn.nn"
        Me.UltraNumericEditor_ExpectedGrossAmt.MaxValue = 922337203685477.62
        Me.UltraNumericEditor_ExpectedGrossAmt.MinValue = -922337203685477.62
        Me.UltraNumericEditor_ExpectedGrossAmt.Name = "UltraNumericEditor_ExpectedGrossAmt"
        Me.UltraNumericEditor_ExpectedGrossAmt.Nullable = True
        Me.UltraNumericEditor_ExpectedGrossAmt.NullText = """"""
        Me.UltraNumericEditor_ExpectedGrossAmt.NumericType = Infragistics.Win.UltraWinEditors.NumericType.[Double]
        Me.UltraNumericEditor_ExpectedGrossAmt.PromptChar = Global.Microsoft.VisualBasic.ChrW(32)
        Me.UltraNumericEditor_ExpectedGrossAmt.Size = New System.Drawing.Size(100, 21)
        Me.UltraNumericEditor_ExpectedGrossAmt.TabIndex = 2
        '
        'UltraNumericEditor_ExpectedNumInvoices
        '
        Me.UltraNumericEditor_ExpectedNumInvoices.AlwaysInEditMode = True
        Me.UltraNumericEditor_ExpectedNumInvoices.Location = New System.Drawing.Point(130, 32)
        Me.UltraNumericEditor_ExpectedNumInvoices.MaskInput = "nnnnnnnnn"
        Me.UltraNumericEditor_ExpectedNumInvoices.MaxValue = 200
        Me.UltraNumericEditor_ExpectedNumInvoices.MinValue = 1
        Me.UltraNumericEditor_ExpectedNumInvoices.Name = "UltraNumericEditor_ExpectedNumInvoices"
        Me.UltraNumericEditor_ExpectedNumInvoices.Nullable = True
        Me.UltraNumericEditor_ExpectedNumInvoices.NullText = """"""
        Me.UltraNumericEditor_ExpectedNumInvoices.PromptChar = Global.Microsoft.VisualBasic.ChrW(32)
        Me.UltraNumericEditor_ExpectedNumInvoices.Size = New System.Drawing.Size(47, 21)
        Me.UltraNumericEditor_ExpectedNumInvoices.TabIndex = 1
        '
        'InvoiceControlGroup
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(648, 603)
        Me.Controls.Add(Me.UltraNumericEditor_ExpectedNumInvoices)
        Me.Controls.Add(Me.UltraNumericEditor_ExpectedGrossAmt)
        Me.Controls.Add(Me.Button_CloseControlGroup)
        Me.Controls.Add(Me.Button_Exit)
        Me.Controls.Add(Me.Button_AddControlGroup)
        Me.Controls.Add(Me.Button_SearchControlGroup)
        Me.Controls.Add(Me.GroupBox_Invoices)
        Me.Controls.Add(Me.Label_ExpectedGrossAmt)
        Me.Controls.Add(Me.Label_ExpectedNumInvoices)
        Me.Controls.Add(Me.TextBox_ControlGroupStatus)
        Me.Controls.Add(Me.TextBox_ControlGroupID)
        Me.Controls.Add(Me.Label_ControlGroupStatus)
        Me.Controls.Add(Me.Label_ControlGroupID)
        Me.Name = "InvoiceControlGroup"
        Me.Text = "Invoice Control Group"
        Me.GroupBox_Invoices.ResumeLayout(False)
        Me.GroupBox_Invoices.PerformLayout()
        CType(Me.UltraGrid_Invoices, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.UltraDataSource1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.UltraNumericEditor_ExpectedGrossAmt, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.UltraNumericEditor_ExpectedNumInvoices, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label_ControlGroupID As System.Windows.Forms.Label
    Friend WithEvents Label_ControlGroupStatus As System.Windows.Forms.Label
    Friend WithEvents TextBox_ControlGroupID As System.Windows.Forms.TextBox
    Friend WithEvents TextBox_ControlGroupStatus As System.Windows.Forms.TextBox
    Friend WithEvents Label_ExpectedNumInvoices As System.Windows.Forms.Label
    Friend WithEvents Label_ExpectedGrossAmt As System.Windows.Forms.Label
    Friend WithEvents GroupBox_Invoices As System.Windows.Forms.GroupBox
    Public WithEvents Button_SearchControlGroup As System.Windows.Forms.Button
    Public WithEvents Button_AddControlGroup As System.Windows.Forms.Button
    Public WithEvents Button_Exit As System.Windows.Forms.Button
    Public WithEvents Button_CloseControlGroup As System.Windows.Forms.Button
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents Button_AddInvoice As System.Windows.Forms.Button
    Friend WithEvents UltraGrid_Invoices As Infragistics.Win.UltraWinGrid.UltraGrid
    Friend WithEvents Button_DeleteInvoice As System.Windows.Forms.Button
    Friend WithEvents UltraDataSource1 As Infragistics.Win.UltraWinDataSource.UltraDataSource
    Friend WithEvents TextBox_EnteredGrossAmt As System.Windows.Forms.TextBox
    Friend WithEvents TextBox_EnteredNumInvoices As System.Windows.Forms.TextBox
    Friend WithEvents Label_EnteredGrossAmt As System.Windows.Forms.Label
    Friend WithEvents Label_EnteredNumInvoices As System.Windows.Forms.Label
    Friend WithEvents Button_EditInvoice As System.Windows.Forms.Button
    Friend WithEvents TextBox_DiffGrossAmt As System.Windows.Forms.TextBox
    Friend WithEvents TextBox_DiffNumInvoices As System.Windows.Forms.TextBox
    Friend WithEvents Label_DiffGrossAmt As System.Windows.Forms.Label
    Friend WithEvents Label_DiffNumInvoices As System.Windows.Forms.Label
    Friend WithEvents UltraNumericEditor_ExpectedGrossAmt As Infragistics.Win.UltraWinEditors.UltraNumericEditor
    Friend WithEvents UltraNumericEditor_ExpectedNumInvoices As Infragistics.Win.UltraWinEditors.UltraNumericEditor
End Class
