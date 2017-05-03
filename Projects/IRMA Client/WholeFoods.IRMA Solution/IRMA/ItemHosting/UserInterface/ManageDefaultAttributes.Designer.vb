<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ManageDefaultAttributes
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ManageDefaultAttributes))
        Dim Appearance13 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridBand1 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1)
        Dim UltraGridColumn1 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Active")
        Dim UltraGridColumn2 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("ItemDefaultAttribute_ID")
        Dim UltraGridColumn3 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("AttributeName")
        Dim UltraGridColumn4 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("AttributeField")
        Dim UltraGridColumn5 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("ControlTypeName")
        Dim UltraGridColumn6 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("ControlOrder", 0)
        Dim UltraGridColumn7 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("ControlType", 1)
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
        Me.cmdExit = New System.Windows.Forms.Button()
        Me.lblInstructions = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.ugridAttributes = New Infragistics.Win.UltraWinGrid.UltraGrid()
        Me.cmdApplyChanges = New System.Windows.Forms.Button()
        CType(Me.ugridAttributes, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdExit
        '
        Me.cmdExit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.Font = New System.Drawing.Font("Arial", 8.0!)
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Image = CType(resources.GetObject("cmdExit.Image"), System.Drawing.Image)
        Me.cmdExit.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.cmdExit.Location = New System.Drawing.Point(273, 591)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(41, 41)
        Me.cmdExit.TabIndex = 98
        Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'lblInstructions
        '
        Me.lblInstructions.AutoSize = True
        Me.lblInstructions.Location = New System.Drawing.Point(12, 9)
        Me.lblInstructions.Name = "lblInstructions"
        Me.lblInstructions.Size = New System.Drawing.Size(377, 13)
        Me.lblInstructions.TabIndex = 100
        Me.lblInstructions.Text = "Check each field you would like to display on the Item Default Attribute screen."
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 54)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(302, 13)
        Me.Label1.TabIndex = 101
        Me.Label1.Text = "You may edit the Attribute Name to change the label for a field."
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 32)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(315, 13)
        Me.Label2.TabIndex = 102
        Me.Label2.Text = "You may drag the rows to change the order in which they appear."
        '
        'ugridAttributes
        '
        Me.ugridAttributes.AllowDrop = True
        Appearance13.BackColor = System.Drawing.SystemColors.Window
        Appearance13.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.ugridAttributes.DisplayLayout.Appearance = Appearance13
        UltraGridColumn1.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn1.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn1.Header.VisiblePosition = 0
        UltraGridColumn1.SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled
        UltraGridColumn2.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn2.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn2.Header.VisiblePosition = 1
        UltraGridColumn2.Hidden = True
        UltraGridColumn2.SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled
        UltraGridColumn3.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn3.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn3.Header.Caption = "Attribute Name"
        UltraGridColumn3.Header.VisiblePosition = 2
        UltraGridColumn3.MaskInput = ""
        UltraGridColumn3.SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled
        UltraGridColumn4.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn4.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn4.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn4.Header.Caption = "Attribute Field"
        UltraGridColumn4.Header.VisiblePosition = 3
        UltraGridColumn4.SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled
        UltraGridColumn5.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn5.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn5.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn5.Header.Caption = "Type"
        UltraGridColumn5.Header.VisiblePosition = 4
        UltraGridColumn5.SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled
        UltraGridColumn6.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn6.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn6.Header.VisiblePosition = 5
        UltraGridColumn6.Hidden = True
        UltraGridColumn6.SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled
        UltraGridColumn7.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn7.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn7.Header.VisiblePosition = 6
        UltraGridColumn7.Hidden = True
        UltraGridColumn7.SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled
        UltraGridBand1.Columns.AddRange(New Object() {UltraGridColumn1, UltraGridColumn2, UltraGridColumn3, UltraGridColumn4, UltraGridColumn5, UltraGridColumn6, UltraGridColumn7})
        Me.ugridAttributes.DisplayLayout.BandsSerializer.Add(UltraGridBand1)
        Me.ugridAttributes.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.ugridAttributes.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Me.ugridAttributes.DisplayLayout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.[False]
        Appearance14.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance14.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance14.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance14.BorderColor = System.Drawing.SystemColors.Window
        Me.ugridAttributes.DisplayLayout.GroupByBox.Appearance = Appearance14
        Appearance15.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugridAttributes.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance15
        Me.ugridAttributes.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.ugridAttributes.DisplayLayout.GroupByBox.Hidden = True
        Appearance16.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance16.BackColor2 = System.Drawing.SystemColors.Control
        Appearance16.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance16.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugridAttributes.DisplayLayout.GroupByBox.PromptAppearance = Appearance16
        Me.ugridAttributes.DisplayLayout.MaxColScrollRegions = 1
        Me.ugridAttributes.DisplayLayout.MaxRowScrollRegions = 1
        Appearance17.BackColor = System.Drawing.SystemColors.Window
        Appearance17.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ugridAttributes.DisplayLayout.Override.ActiveCellAppearance = Appearance17
        Appearance18.BackColor = System.Drawing.SystemColors.Highlight
        Appearance18.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.ugridAttributes.DisplayLayout.Override.ActiveRowAppearance = Appearance18
        Me.ugridAttributes.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.ugridAttributes.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance19.BackColor = System.Drawing.SystemColors.Window
        Me.ugridAttributes.DisplayLayout.Override.CardAreaAppearance = Appearance19
        Appearance20.BorderColor = System.Drawing.Color.Silver
        Appearance20.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.ugridAttributes.DisplayLayout.Override.CellAppearance = Appearance20
        Me.ugridAttributes.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.ugridAttributes.DisplayLayout.Override.CellPadding = 0
        Appearance21.BackColor = System.Drawing.SystemColors.Control
        Appearance21.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance21.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance21.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance21.BorderColor = System.Drawing.SystemColors.Window
        Me.ugridAttributes.DisplayLayout.Override.GroupByRowAppearance = Appearance21
        Appearance22.TextHAlignAsString = "Left"
        Me.ugridAttributes.DisplayLayout.Override.HeaderAppearance = Appearance22
        Me.ugridAttributes.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti
        Me.ugridAttributes.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance23.BackColor = System.Drawing.SystemColors.Window
        Appearance23.BorderColor = System.Drawing.Color.Silver
        Me.ugridAttributes.DisplayLayout.Override.RowAppearance = Appearance23
        Me.ugridAttributes.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[True]
        Me.ugridAttributes.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.SingleAutoDrag
        Appearance24.BackColor = System.Drawing.SystemColors.ControlLight
        Me.ugridAttributes.DisplayLayout.Override.TemplateAddRowAppearance = Appearance24
        Me.ugridAttributes.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.ugridAttributes.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.ugridAttributes.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        Me.ugridAttributes.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ugridAttributes.Location = New System.Drawing.Point(12, 83)
        Me.ugridAttributes.Name = "ugridAttributes"
        Me.ugridAttributes.Size = New System.Drawing.Size(489, 502)
        Me.ugridAttributes.TabIndex = 103
        Me.ugridAttributes.Text = "UltraGrid1"
        Me.ugridAttributes.UseOsThemes = Infragistics.Win.DefaultableBoolean.[False]
        '
        'cmdApplyChanges
        '
        Me.cmdApplyChanges.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdApplyChanges.BackColor = System.Drawing.SystemColors.Control
        Me.cmdApplyChanges.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdApplyChanges.Enabled = False
        Me.cmdApplyChanges.Font = New System.Drawing.Font("Arial", 8.0!)
        Me.cmdApplyChanges.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdApplyChanges.Image = CType(resources.GetObject("cmdApplyChanges.Image"), System.Drawing.Image)
        Me.cmdApplyChanges.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.cmdApplyChanges.Location = New System.Drawing.Point(205, 591)
        Me.cmdApplyChanges.Name = "cmdApplyChanges"
        Me.cmdApplyChanges.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdApplyChanges.Size = New System.Drawing.Size(41, 41)
        Me.cmdApplyChanges.TabIndex = 115
        Me.cmdApplyChanges.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.cmdApplyChanges.UseVisualStyleBackColor = False
        '
        'ManageDefaultAttributes
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(513, 644)
        Me.Controls.Add(Me.cmdApplyChanges)
        Me.Controls.Add(Me.ugridAttributes)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lblInstructions)
        Me.Controls.Add(Me.cmdExit)
        Me.Name = "ManageDefaultAttributes"
        Me.Text = "Manage Default Attributes"
        CType(Me.ugridAttributes, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Public WithEvents cmdExit As System.Windows.Forms.Button
    Friend WithEvents lblInstructions As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ugridAttributes As Infragistics.Win.UltraWinGrid.UltraGrid
    Public WithEvents cmdApplyChanges As System.Windows.Forms.Button
End Class
