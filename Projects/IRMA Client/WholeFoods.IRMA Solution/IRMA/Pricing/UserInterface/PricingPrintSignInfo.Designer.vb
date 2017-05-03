<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPricingPrintSignInfo
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
        Dim Appearance14 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim UltraGridBand1 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1)
        Dim UltraGridColumn1 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("LabelType_ID")
        Dim UltraGridColumn2 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("LabelTypeDesc")
        Dim UltraGridColumn3 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Item_Count")
        Dim UltraGridColumn4 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("PriceBatchHeaderID", -1, Nothing, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, False)
        Dim ColScrollRegion1 As Infragistics.Win.UltraWinGrid.ColScrollRegion = New Infragistics.Win.UltraWinGrid.ColScrollRegion(428)
        Dim ColScrollRegion2 As Infragistics.Win.UltraWinGrid.ColScrollRegion = New Infragistics.Win.UltraWinGrid.ColScrollRegion(116)
        Dim Appearance15 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance16 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance17 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance18 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance19 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance20 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance21 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance22 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance23 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance24 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance25 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance26 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPricingPrintSignInfo))
        Me.ugrdList = New Infragistics.Win.UltraWinGrid.UltraGrid
        Me.cmdExit = New System.Windows.Forms.Button
        Me.cmdPrint = New System.Windows.Forms.Button
        Me.StartLabelTextBox = New System.Windows.Forms.TextBox
        Me.lblStartLabel = New System.Windows.Forms.Label
        Me.chkSortLabels = New System.Windows.Forms.CheckBox
        Me.lblSortLabels = New System.Windows.Forms.Label
        Me.cmbTagOverride = New System.Windows.Forms.ComboBox
        Me.lblTagType = New System.Windows.Forms.Label
        Me.optPriceType_New = New System.Windows.Forms.RadioButton
        Me.pnlPriceTypeOverrideOptions = New System.Windows.Forms.Panel
        Me.optPriceType_Promo = New System.Windows.Forms.RadioButton
        Me.optPriceType_Regular = New System.Windows.Forms.RadioButton
        CType(Me.ugrdList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlPriceTypeOverrideOptions.SuspendLayout()
        Me.SuspendLayout()
        '
        'ugrdList
        '
        Appearance14.BackColor = System.Drawing.SystemColors.ControlLight
        Appearance14.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.ugrdList.DisplayLayout.Appearance = Appearance14
        Me.ugrdList.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        UltraGridColumn1.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn1.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn1.Header.Caption = "Label Type ID"
        UltraGridColumn1.Header.VisiblePosition = 0
        UltraGridColumn1.Hidden = True
        UltraGridColumn2.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn2.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn2.Header.Caption = "Label Type"
        UltraGridColumn2.Header.VisiblePosition = 1
        UltraGridColumn2.Width = 117
        UltraGridColumn3.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn3.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn3.Header.Caption = "Item Count"
        UltraGridColumn3.Header.VisiblePosition = 2
        UltraGridColumn3.Width = 124
        UltraGridColumn4.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn4.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn4.Header.Caption = "Price Batch Header ID"
        UltraGridColumn4.Header.VisiblePosition = 3
        UltraGridColumn4.Width = 168
        UltraGridBand1.Columns.AddRange(New Object() {UltraGridColumn1, UltraGridColumn2, UltraGridColumn3, UltraGridColumn4})
        Me.ugrdList.DisplayLayout.BandsSerializer.Add(UltraGridBand1)
        Me.ugrdList.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.ugrdList.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Me.ugrdList.DisplayLayout.ColScrollRegions.Add(ColScrollRegion1)
        Me.ugrdList.DisplayLayout.ColScrollRegions.Add(ColScrollRegion2)
        Appearance15.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance15.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance15.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance15.BorderColor = System.Drawing.SystemColors.Window
        Me.ugrdList.DisplayLayout.GroupByBox.Appearance = Appearance15
        Appearance16.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugrdList.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance16
        Me.ugrdList.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.ugrdList.DisplayLayout.GroupByBox.Hidden = True
        Appearance17.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance17.BackColor2 = System.Drawing.SystemColors.Control
        Appearance17.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance17.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugrdList.DisplayLayout.GroupByBox.PromptAppearance = Appearance17
        Me.ugrdList.DisplayLayout.MaxColScrollRegions = 1
        Me.ugrdList.DisplayLayout.MaxRowScrollRegions = 1
        Appearance18.BackColor = System.Drawing.SystemColors.Window
        Appearance18.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ugrdList.DisplayLayout.Override.ActiveCellAppearance = Appearance18
        Appearance19.BackColor = System.Drawing.SystemColors.Highlight
        Appearance19.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.ugrdList.DisplayLayout.Override.ActiveRowAppearance = Appearance19
        Me.ugrdList.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.ugrdList.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance20.BackColor = System.Drawing.SystemColors.Window
        Me.ugrdList.DisplayLayout.Override.CardAreaAppearance = Appearance20
        Appearance21.BorderColor = System.Drawing.Color.Silver
        Appearance21.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.ugrdList.DisplayLayout.Override.CellAppearance = Appearance21
        Me.ugrdList.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.ugrdList.DisplayLayout.Override.CellPadding = 0
        Appearance22.BackColor = System.Drawing.SystemColors.Control
        Appearance22.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance22.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance22.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance22.BorderColor = System.Drawing.SystemColors.Window
        Me.ugrdList.DisplayLayout.Override.GroupByRowAppearance = Appearance22
        Appearance23.FontData.BoldAsString = "True"
        Appearance23.TextHAlignAsString = "Left"
        Me.ugrdList.DisplayLayout.Override.HeaderAppearance = Appearance23
        Me.ugrdList.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti
        Me.ugrdList.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance24.BackColor = System.Drawing.SystemColors.ControlLight
        Me.ugrdList.DisplayLayout.Override.RowAlternateAppearance = Appearance24
        Appearance25.BackColor = System.Drawing.SystemColors.Window
        Appearance25.BorderColor = System.Drawing.Color.Silver
        Me.ugrdList.DisplayLayout.Override.RowAppearance = Appearance25
        Me.ugrdList.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[True]
        Me.ugrdList.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Extended
        Appearance26.BackColor = System.Drawing.SystemColors.ControlLight
        Me.ugrdList.DisplayLayout.Override.TemplateAddRowAppearance = Appearance26
        Me.ugrdList.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.ugrdList.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.ugrdList.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        Me.ugrdList.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ugrdList.Location = New System.Drawing.Point(12, 12)
        Me.ugrdList.Name = "ugrdList"
        Me.ugrdList.Size = New System.Drawing.Size(430, 225)
        Me.ugrdList.TabIndex = 0
        Me.ugrdList.Text = "UltraGrid1"
        '
        'cmdExit
        '
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.Font = New System.Drawing.Font("Arial", 8.0!)
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Image = CType(resources.GetObject("cmdExit.Image"), System.Drawing.Image)
        Me.cmdExit.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.cmdExit.Location = New System.Drawing.Point(401, 260)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(41, 41)
        Me.cmdExit.TabIndex = 34
        Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'cmdPrint
        '
        Me.cmdPrint.BackColor = System.Drawing.SystemColors.Control
        Me.cmdPrint.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdPrint.Font = New System.Drawing.Font("Arial", 8.0!)
        Me.cmdPrint.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdPrint.Image = CType(resources.GetObject("cmdPrint.Image"), System.Drawing.Image)
        Me.cmdPrint.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.cmdPrint.Location = New System.Drawing.Point(344, 260)
        Me.cmdPrint.Name = "cmdPrint"
        Me.cmdPrint.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdPrint.Size = New System.Drawing.Size(41, 41)
        Me.cmdPrint.TabIndex = 35
        Me.cmdPrint.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.cmdPrint.UseVisualStyleBackColor = False
        '
        'StartLabelTextBox
        '
        Me.StartLabelTextBox.AcceptsReturn = True
        Me.StartLabelTextBox.BackColor = System.Drawing.SystemColors.Window
        Me.StartLabelTextBox.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.StartLabelTextBox.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.StartLabelTextBox.ForeColor = System.Drawing.SystemColors.WindowText
        Me.StartLabelTextBox.Location = New System.Drawing.Point(90, 250)
        Me.StartLabelTextBox.MaxLength = 2
        Me.StartLabelTextBox.Name = "StartLabelTextBox"
        Me.StartLabelTextBox.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartLabelTextBox.Size = New System.Drawing.Size(33, 20)
        Me.StartLabelTextBox.TabIndex = 40
        Me.StartLabelTextBox.Tag = "String"
        Me.StartLabelTextBox.Text = "1"
        Me.StartLabelTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblStartLabel
        '
        Me.lblStartLabel.BackColor = System.Drawing.Color.Transparent
        Me.lblStartLabel.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblStartLabel.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.lblStartLabel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblStartLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblStartLabel.Location = New System.Drawing.Point(3, 253)
        Me.lblStartLabel.Name = "lblStartLabel"
        Me.lblStartLabel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblStartLabel.Size = New System.Drawing.Size(81, 17)
        Me.lblStartLabel.TabIndex = 41
        Me.lblStartLabel.Text = "Start Label :"
        Me.lblStartLabel.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'chkSortLabels
        '
        Me.chkSortLabels.AutoSize = True
        Me.chkSortLabels.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.chkSortLabels.Location = New System.Drawing.Point(214, 250)
        Me.chkSortLabels.Name = "chkSortLabels"
        Me.chkSortLabels.Size = New System.Drawing.Size(15, 14)
        Me.chkSortLabels.TabIndex = 42
        Me.chkSortLabels.UseVisualStyleBackColor = True
        '
        'lblSortLabels
        '
        Me.lblSortLabels.BackColor = System.Drawing.Color.Transparent
        Me.lblSortLabels.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSortLabels.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.lblSortLabels.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSortLabels.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblSortLabels.Location = New System.Drawing.Point(130, 250)
        Me.lblSortLabels.Name = "lblSortLabels"
        Me.lblSortLabels.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSortLabels.Size = New System.Drawing.Size(81, 17)
        Me.lblSortLabels.TabIndex = 43
        Me.lblSortLabels.Text = "Sort Labels :"
        Me.lblSortLabels.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'cmbTagOverride
        '
        Me.cmbTagOverride.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbTagOverride.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbTagOverride.BackColor = System.Drawing.SystemColors.Window
        Me.cmbTagOverride.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbTagOverride.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTagOverride.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.cmbTagOverride.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbTagOverride.Items.AddRange(New Object() {"Grocery", "Nutrition"})
        Me.cmbTagOverride.Location = New System.Drawing.Point(91, 279)
        Me.cmbTagOverride.Name = "cmbTagOverride"
        Me.cmbTagOverride.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbTagOverride.Size = New System.Drawing.Size(137, 22)
        Me.cmbTagOverride.TabIndex = 53
        '
        'lblTagType
        '
        Me.lblTagType.BackColor = System.Drawing.Color.Transparent
        Me.lblTagType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTagType.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.lblTagType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTagType.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblTagType.Location = New System.Drawing.Point(13, 275)
        Me.lblTagType.Name = "lblTagType"
        Me.lblTagType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTagType.Size = New System.Drawing.Size(72, 31)
        Me.lblTagType.TabIndex = 54
        Me.lblTagType.Text = "Tag Size Override :"
        '
        'optPriceType_New
        '
        Me.optPriceType_New.AutoSize = True
        Me.optPriceType_New.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.optPriceType_New.Location = New System.Drawing.Point(1, 36)
        Me.optPriceType_New.Name = "optPriceType_New"
        Me.optPriceType_New.Size = New System.Drawing.Size(47, 17)
        Me.optPriceType_New.TabIndex = 49
        Me.optPriceType_New.TabStop = True
        Me.optPriceType_New.Text = "New"
        Me.optPriceType_New.UseVisualStyleBackColor = True
        '
        'pnlPriceTypeOverrideOptions
        '
        Me.pnlPriceTypeOverrideOptions.Controls.Add(Me.optPriceType_New)
        Me.pnlPriceTypeOverrideOptions.Controls.Add(Me.optPriceType_Promo)
        Me.pnlPriceTypeOverrideOptions.Controls.Add(Me.optPriceType_Regular)
        Me.pnlPriceTypeOverrideOptions.Location = New System.Drawing.Point(245, 247)
        Me.pnlPriceTypeOverrideOptions.Name = "pnlPriceTypeOverrideOptions"
        Me.pnlPriceTypeOverrideOptions.Size = New System.Drawing.Size(78, 56)
        Me.pnlPriceTypeOverrideOptions.TabIndex = 55
        Me.pnlPriceTypeOverrideOptions.Visible = False
        '
        'optPriceType_Promo
        '
        Me.optPriceType_Promo.AutoSize = True
        Me.optPriceType_Promo.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.optPriceType_Promo.Location = New System.Drawing.Point(1, 19)
        Me.optPriceType_Promo.Name = "optPriceType_Promo"
        Me.optPriceType_Promo.Size = New System.Drawing.Size(55, 17)
        Me.optPriceType_Promo.TabIndex = 48
        Me.optPriceType_Promo.TabStop = True
        Me.optPriceType_Promo.Text = "Promo"
        Me.optPriceType_Promo.UseVisualStyleBackColor = True
        '
        'optPriceType_Regular
        '
        Me.optPriceType_Regular.AutoSize = True
        Me.optPriceType_Regular.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.optPriceType_Regular.Location = New System.Drawing.Point(1, 1)
        Me.optPriceType_Regular.Name = "optPriceType_Regular"
        Me.optPriceType_Regular.Size = New System.Drawing.Size(62, 17)
        Me.optPriceType_Regular.TabIndex = 47
        Me.optPriceType_Regular.TabStop = True
        Me.optPriceType_Regular.Text = "Regular"
        Me.optPriceType_Regular.UseVisualStyleBackColor = True
        '
        'frmPricingPrintSignInfo
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(457, 319)
        Me.Controls.Add(Me.pnlPriceTypeOverrideOptions)
        Me.Controls.Add(Me.cmbTagOverride)
        Me.Controls.Add(Me.lblTagType)
        Me.Controls.Add(Me.lblSortLabels)
        Me.Controls.Add(Me.chkSortLabels)
        Me.Controls.Add(Me.StartLabelTextBox)
        Me.Controls.Add(Me.lblStartLabel)
        Me.Controls.Add(Me.cmdPrint)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.ugrdList)
        Me.Name = "frmPricingPrintSignInfo"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Print Signs Information"
        CType(Me.ugrdList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlPriceTypeOverrideOptions.ResumeLayout(False)
        Me.pnlPriceTypeOverrideOptions.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ugrdList As Infragistics.Win.UltraWinGrid.UltraGrid
    Public WithEvents cmdExit As System.Windows.Forms.Button
    Public WithEvents cmdPrint As System.Windows.Forms.Button
    Public WithEvents StartLabelTextBox As System.Windows.Forms.TextBox
    Public WithEvents lblStartLabel As System.Windows.Forms.Label
    Friend WithEvents chkSortLabels As System.Windows.Forms.CheckBox
    Public WithEvents lblSortLabels As System.Windows.Forms.Label
    Public WithEvents cmbTagOverride As System.Windows.Forms.ComboBox
    Public WithEvents lblTagType As System.Windows.Forms.Label
    Friend WithEvents optPriceType_New As System.Windows.Forms.RadioButton
    Friend WithEvents pnlPriceTypeOverrideOptions As System.Windows.Forms.Panel
    Friend WithEvents optPriceType_Promo As System.Windows.Forms.RadioButton
    Friend WithEvents optPriceType_Regular As System.Windows.Forms.RadioButton
End Class
