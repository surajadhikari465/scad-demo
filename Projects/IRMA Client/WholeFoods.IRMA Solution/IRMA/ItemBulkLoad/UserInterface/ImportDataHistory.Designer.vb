<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ImportDataHistory
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
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ImportDataHistory))
        Dim Appearance1 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim UltraGridBand1 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1)
        Dim UltraGridColumn1 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("ItemUploadHeaderID")
        Dim UltraGridColumn2 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("ItemsProcessedCount")
        Dim UltraGridColumn3 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("ItemsLoadedCount")
        Dim UltraGridColumn4 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("ErrorsCount")
        Dim UltraGridColumn5 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("UploadedDateTime")
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
        Me.SearchGroupBox = New System.Windows.Forms.GroupBox
        Me.ItemLoadHeaderIDUNE = New Infragistics.Win.UltraWinEditors.UltraNumericEditor
        Me.UserComboBox = New System.Windows.Forms.ComboBox
        Me.UserLabel = New System.Windows.Forms.Label
        Me.ItemLoadHeaderIDLabel = New System.Windows.Forms.Label
        Me.ImportTypeComboBox = New System.Windows.Forms.ComboBox
        Me.ImportTypeLabel = New System.Windows.Forms.Label
        Me.BatchDescriptionTextBox = New System.Windows.Forms.TextBox
        Me.BatchDescLabel = New System.Windows.Forms.Label
        Me.CreateDateUDTE = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
        Me.LoadDateLabel = New System.Windows.Forms.Label
        Me.cmdSearch = New System.Windows.Forms.Button
        Me.cmdExit = New System.Windows.Forms.Button
        Me.ugrdList = New Infragistics.Win.UltraWinGrid.UltraGrid
        Me.PriceBatchButton = New System.Windows.Forms.Button
        Me.ItemLoadDetailButton = New System.Windows.Forms.Button
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.SearchGroupBox.SuspendLayout()
        CType(Me.ItemLoadHeaderIDUNE, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CreateDateUDTE, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ugrdList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'SearchGroupBox
        '
        Me.SearchGroupBox.Controls.Add(Me.ItemLoadHeaderIDUNE)
        Me.SearchGroupBox.Controls.Add(Me.UserComboBox)
        Me.SearchGroupBox.Controls.Add(Me.UserLabel)
        Me.SearchGroupBox.Controls.Add(Me.ItemLoadHeaderIDLabel)
        Me.SearchGroupBox.Controls.Add(Me.ImportTypeComboBox)
        Me.SearchGroupBox.Controls.Add(Me.ImportTypeLabel)
        Me.SearchGroupBox.Controls.Add(Me.BatchDescriptionTextBox)
        Me.SearchGroupBox.Controls.Add(Me.BatchDescLabel)
        Me.SearchGroupBox.Controls.Add(Me.CreateDateUDTE)
        Me.SearchGroupBox.Controls.Add(Me.LoadDateLabel)
        Me.SearchGroupBox.Controls.Add(Me.cmdSearch)
        Me.SearchGroupBox.Location = New System.Drawing.Point(5, 2)
        Me.SearchGroupBox.Name = "SearchGroupBox"
        Me.SearchGroupBox.Size = New System.Drawing.Size(775, 80)
        Me.SearchGroupBox.TabIndex = 1
        Me.SearchGroupBox.TabStop = False
        Me.SearchGroupBox.Text = "Search"
        '
        'ItemLoadHeaderIDUNE
        '
        Me.ItemLoadHeaderIDUNE.Location = New System.Drawing.Point(96, 18)
        Me.ItemLoadHeaderIDUNE.MinValue = 1
        Me.ItemLoadHeaderIDUNE.Name = "ItemLoadHeaderIDUNE"
        Me.ItemLoadHeaderIDUNE.Nullable = True
        Me.ItemLoadHeaderIDUNE.PromptChar = Global.Microsoft.VisualBasic.ChrW(32)
        Me.ItemLoadHeaderIDUNE.Size = New System.Drawing.Size(85, 21)
        Me.ItemLoadHeaderIDUNE.TabIndex = 0
        Me.ItemLoadHeaderIDUNE.Value = Nothing
        '
        'UserComboBox
        '
        Me.UserComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.UserComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.UserComboBox.BackColor = System.Drawing.SystemColors.Window
        Me.UserComboBox.Cursor = System.Windows.Forms.Cursors.Default
        Me.UserComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.UserComboBox.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.UserComboBox.ForeColor = System.Drawing.SystemColors.WindowText
        Me.UserComboBox.Location = New System.Drawing.Point(295, 45)
        Me.UserComboBox.Name = "UserComboBox"
        Me.UserComboBox.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.UserComboBox.Size = New System.Drawing.Size(145, 22)
        Me.UserComboBox.Sorted = True
        Me.UserComboBox.TabIndex = 3
        '
        'UserLabel
        '
        Me.UserLabel.BackColor = System.Drawing.Color.Transparent
        Me.UserLabel.Cursor = System.Windows.Forms.Cursors.Default
        Me.UserLabel.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.UserLabel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.UserLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.UserLabel.Location = New System.Drawing.Point(204, 45)
        Me.UserLabel.Name = "UserLabel"
        Me.UserLabel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.UserLabel.Size = New System.Drawing.Size(85, 20)
        Me.UserLabel.TabIndex = 123
        Me.UserLabel.Text = "User :"
        Me.UserLabel.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'ItemLoadHeaderIDLabel
        '
        Me.ItemLoadHeaderIDLabel.BackColor = System.Drawing.Color.Transparent
        Me.ItemLoadHeaderIDLabel.Cursor = System.Windows.Forms.Cursors.Default
        Me.ItemLoadHeaderIDLabel.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.ItemLoadHeaderIDLabel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ItemLoadHeaderIDLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.ItemLoadHeaderIDLabel.Location = New System.Drawing.Point(9, 22)
        Me.ItemLoadHeaderIDLabel.Name = "ItemLoadHeaderIDLabel"
        Me.ItemLoadHeaderIDLabel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ItemLoadHeaderIDLabel.Size = New System.Drawing.Size(81, 17)
        Me.ItemLoadHeaderIDLabel.TabIndex = 121
        Me.ItemLoadHeaderIDLabel.Text = "Upload ID :"
        Me.ItemLoadHeaderIDLabel.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'ImportTypeComboBox
        '
        Me.ImportTypeComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.ImportTypeComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.ImportTypeComboBox.BackColor = System.Drawing.SystemColors.Window
        Me.ImportTypeComboBox.Cursor = System.Windows.Forms.Cursors.Default
        Me.ImportTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ImportTypeComboBox.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.ImportTypeComboBox.ForeColor = System.Drawing.SystemColors.WindowText
        Me.ImportTypeComboBox.Location = New System.Drawing.Point(295, 18)
        Me.ImportTypeComboBox.Name = "ImportTypeComboBox"
        Me.ImportTypeComboBox.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ImportTypeComboBox.Size = New System.Drawing.Size(145, 22)
        Me.ImportTypeComboBox.Sorted = True
        Me.ImportTypeComboBox.TabIndex = 2
        '
        'ImportTypeLabel
        '
        Me.ImportTypeLabel.BackColor = System.Drawing.Color.Transparent
        Me.ImportTypeLabel.Cursor = System.Windows.Forms.Cursors.Default
        Me.ImportTypeLabel.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.ImportTypeLabel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ImportTypeLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.ImportTypeLabel.Location = New System.Drawing.Point(204, 22)
        Me.ImportTypeLabel.Name = "ImportTypeLabel"
        Me.ImportTypeLabel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ImportTypeLabel.Size = New System.Drawing.Size(85, 20)
        Me.ImportTypeLabel.TabIndex = 119
        Me.ImportTypeLabel.Text = "Import Type :"
        Me.ImportTypeLabel.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'BatchDescriptionTextBox
        '
        Me.BatchDescriptionTextBox.AcceptsReturn = True
        Me.BatchDescriptionTextBox.BackColor = System.Drawing.SystemColors.Window
        Me.BatchDescriptionTextBox.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.BatchDescriptionTextBox.Enabled = False
        Me.BatchDescriptionTextBox.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.BatchDescriptionTextBox.ForeColor = System.Drawing.SystemColors.WindowText
        Me.BatchDescriptionTextBox.Location = New System.Drawing.Point(537, 19)
        Me.BatchDescriptionTextBox.MaxLength = 30
        Me.BatchDescriptionTextBox.Multiline = True
        Me.BatchDescriptionTextBox.Name = "BatchDescriptionTextBox"
        Me.BatchDescriptionTextBox.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.BatchDescriptionTextBox.Size = New System.Drawing.Size(185, 53)
        Me.BatchDescriptionTextBox.TabIndex = 4
        Me.BatchDescriptionTextBox.Tag = "String"
        '
        'BatchDescLabel
        '
        Me.BatchDescLabel.BackColor = System.Drawing.Color.Transparent
        Me.BatchDescLabel.Cursor = System.Windows.Forms.Cursors.Default
        Me.BatchDescLabel.Enabled = False
        Me.BatchDescLabel.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.BatchDescLabel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.BatchDescLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.BatchDescLabel.Location = New System.Drawing.Point(446, 20)
        Me.BatchDescLabel.Name = "BatchDescLabel"
        Me.BatchDescLabel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.BatchDescLabel.Size = New System.Drawing.Size(87, 18)
        Me.BatchDescLabel.TabIndex = 117
        Me.BatchDescLabel.Text = "Batch Desc : "
        Me.BatchDescLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'CreateDateUDTE
        '
        Me.CreateDateUDTE.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.CreateDateUDTE.Location = New System.Drawing.Point(96, 47)
        Me.CreateDateUDTE.MaskInput = ""
        Me.CreateDateUDTE.MaxDate = New Date(2099, 12, 31, 0, 0, 0, 0)
        Me.CreateDateUDTE.MinDate = New Date(1980, 1, 1, 0, 0, 0, 0)
        Me.CreateDateUDTE.Name = "CreateDateUDTE"
        Me.CreateDateUDTE.Size = New System.Drawing.Size(85, 21)
        Me.CreateDateUDTE.TabIndex = 1
        '
        'LoadDateLabel
        '
        Me.LoadDateLabel.BackColor = System.Drawing.Color.Transparent
        Me.LoadDateLabel.Cursor = System.Windows.Forms.Cursors.Default
        Me.LoadDateLabel.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.LoadDateLabel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LoadDateLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.LoadDateLabel.Location = New System.Drawing.Point(13, 51)
        Me.LoadDateLabel.Name = "LoadDateLabel"
        Me.LoadDateLabel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LoadDateLabel.Size = New System.Drawing.Size(77, 16)
        Me.LoadDateLabel.TabIndex = 113
        Me.LoadDateLabel.Text = "Create Date : "
        Me.LoadDateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmdSearch
        '
        Me.cmdSearch.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSearch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSearch.Font = New System.Drawing.Font("Arial", 8.0!)
        Me.cmdSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSearch.Image = CType(resources.GetObject("cmdSearch.Image"), System.Drawing.Image)
        Me.cmdSearch.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.cmdSearch.Location = New System.Drawing.Point(728, 24)
        Me.cmdSearch.Name = "cmdSearch"
        Me.cmdSearch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSearch.Size = New System.Drawing.Size(41, 41)
        Me.cmdSearch.TabIndex = 5
        Me.cmdSearch.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdSearch, "Search Uploads")
        Me.cmdSearch.UseVisualStyleBackColor = False
        '
        'cmdExit
        '
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdExit.Font = New System.Drawing.Font("Arial", 8.0!)
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Image = CType(resources.GetObject("cmdExit.Image"), System.Drawing.Image)
        Me.cmdExit.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.cmdExit.Location = New System.Drawing.Point(742, 453)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(41, 41)
        Me.cmdExit.TabIndex = 2
        Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdExit, "Exit")
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'ugrdList
        '
        Appearance1.BackColor = System.Drawing.SystemColors.ControlLight
        Appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.ugrdList.DisplayLayout.Appearance = Appearance1
        Me.ugrdList.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        UltraGridColumn1.Header.Caption = "Item Upload ID"
        UltraGridColumn1.Header.ToolTipText = "Unique ID noted in confirmation email."
        UltraGridColumn1.Header.VisiblePosition = 0
        UltraGridColumn2.Header.Caption = "# Items Processed"
        UltraGridColumn2.Header.VisiblePosition = 1
        UltraGridColumn3.Header.Caption = "# Items Loaded"
        UltraGridColumn3.Header.VisiblePosition = 2
        UltraGridColumn4.Header.Caption = "# Errors"
        UltraGridColumn4.Header.VisiblePosition = 3
        UltraGridColumn5.Header.Caption = "Upload Date"
        UltraGridColumn5.Header.VisiblePosition = 4
        UltraGridBand1.Columns.AddRange(New Object() {UltraGridColumn1, UltraGridColumn2, UltraGridColumn3, UltraGridColumn4, UltraGridColumn5})
        UltraGridBand1.RowLayoutStyle = Infragistics.Win.UltraWinGrid.RowLayoutStyle.None
        Me.ugrdList.DisplayLayout.BandsSerializer.Add(UltraGridBand1)
        Me.ugrdList.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Appearance2.FontData.BoldAsString = "True"
        Me.ugrdList.DisplayLayout.CaptionAppearance = Appearance2
        Me.ugrdList.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[True]
        Appearance3.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance3.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance3.BorderColor = System.Drawing.SystemColors.Window
        Me.ugrdList.DisplayLayout.GroupByBox.Appearance = Appearance3
        Appearance4.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugrdList.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance4
        Me.ugrdList.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.ugrdList.DisplayLayout.GroupByBox.Hidden = True
        Appearance5.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance5.BackColor2 = System.Drawing.SystemColors.Control
        Appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance5.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugrdList.DisplayLayout.GroupByBox.PromptAppearance = Appearance5
        Me.ugrdList.DisplayLayout.MaxColScrollRegions = 1
        Me.ugrdList.DisplayLayout.MaxRowScrollRegions = 1
        Appearance6.BackColor = System.Drawing.SystemColors.Window
        Appearance6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ugrdList.DisplayLayout.Override.ActiveCellAppearance = Appearance6
        Me.ugrdList.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No
        Me.ugrdList.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.[False]
        Me.ugrdList.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.[False]
        Me.ugrdList.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.ugrdList.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance7.BackColor = System.Drawing.SystemColors.Window
        Me.ugrdList.DisplayLayout.Override.CardAreaAppearance = Appearance7
        Appearance8.BorderColor = System.Drawing.Color.Silver
        Appearance8.FontData.BoldAsString = "True"
        Appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.ugrdList.DisplayLayout.Override.CellAppearance = Appearance8
        Me.ugrdList.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        Me.ugrdList.DisplayLayout.Override.CellPadding = 0
        Appearance9.FontData.BoldAsString = "True"
        Me.ugrdList.DisplayLayout.Override.FixedHeaderAppearance = Appearance9
        Appearance10.BackColor = System.Drawing.SystemColors.Control
        Appearance10.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance10.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance10.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance10.BorderColor = System.Drawing.SystemColors.Window
        Me.ugrdList.DisplayLayout.Override.GroupByRowAppearance = Appearance10
        Appearance11.FontData.BoldAsString = "True"
        Appearance11.TextHAlign = Infragistics.Win.HAlign.Left
        Me.ugrdList.DisplayLayout.Override.HeaderAppearance = Appearance11
        Me.ugrdList.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle
        Me.ugrdList.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance12.BackColor = System.Drawing.SystemColors.ControlLight
        Me.ugrdList.DisplayLayout.Override.RowAlternateAppearance = Appearance12
        Appearance13.BackColor = System.Drawing.SystemColors.Window
        Appearance13.BorderColor = System.Drawing.Color.Silver
        Me.ugrdList.DisplayLayout.Override.RowAppearance = Appearance13
        Me.ugrdList.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[True]
        Me.ugrdList.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed
        Me.ugrdList.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.ugrdList.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.ugrdList.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.[Single]
        Appearance14.BackColor = System.Drawing.SystemColors.ControlLight
        Me.ugrdList.DisplayLayout.Override.TemplateAddRowAppearance = Appearance14
        Me.ugrdList.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.ugrdList.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.ugrdList.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand
        Me.ugrdList.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        Me.ugrdList.Font = New System.Drawing.Font("Arial", 8.0!)
        Me.ugrdList.Location = New System.Drawing.Point(5, 88)
        Me.ugrdList.Name = "ugrdList"
        Me.ugrdList.Size = New System.Drawing.Size(778, 359)
        Me.ugrdList.TabIndex = 3
        Me.ugrdList.Text = "Search Results"
        '
        'PriceBatchButton
        '
        Me.PriceBatchButton.Enabled = False
        Me.PriceBatchButton.Location = New System.Drawing.Point(5, 453)
        Me.PriceBatchButton.Name = "PriceBatchButton"
        Me.PriceBatchButton.Size = New System.Drawing.Size(57, 41)
        Me.PriceBatchButton.TabIndex = 0
        Me.PriceBatchButton.Text = "Batches"
        Me.ToolTip1.SetToolTip(Me.PriceBatchButton, "View Batches for Upload")
        Me.PriceBatchButton.UseVisualStyleBackColor = True
        '
        'ItemLoadDetailButton
        '
        Me.ItemLoadDetailButton.BackColor = System.Drawing.SystemColors.Control
        Me.ItemLoadDetailButton.Cursor = System.Windows.Forms.Cursors.Default
        Me.ItemLoadDetailButton.Font = New System.Drawing.Font("Arial", 8.0!)
        Me.ItemLoadDetailButton.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ItemLoadDetailButton.Image = CType(resources.GetObject("ItemLoadDetailButton.Image"), System.Drawing.Image)
        Me.ItemLoadDetailButton.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.ItemLoadDetailButton.Location = New System.Drawing.Point(68, 453)
        Me.ItemLoadDetailButton.Name = "ItemLoadDetailButton"
        Me.ItemLoadDetailButton.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ItemLoadDetailButton.Size = New System.Drawing.Size(41, 41)
        Me.ItemLoadDetailButton.TabIndex = 1
        Me.ItemLoadDetailButton.Tag = "B"
        Me.ItemLoadDetailButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.ItemLoadDetailButton, "View Upload Details")
        Me.ItemLoadDetailButton.UseVisualStyleBackColor = False
        '
        'ImportDataHistory
        '
        Me.AcceptButton = Me.cmdSearch
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.cmdExit
        Me.ClientSize = New System.Drawing.Size(801, 498)
        Me.Controls.Add(Me.ItemLoadDetailButton)
        Me.Controls.Add(Me.PriceBatchButton)
        Me.Controls.Add(Me.ugrdList)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.SearchGroupBox)
        Me.Name = "ImportDataHistory"
        Me.ShowInTaskbar = False
        Me.Text = "Item Maintenance Load Audit / History"
        Me.SearchGroupBox.ResumeLayout(False)
        Me.SearchGroupBox.PerformLayout()
        CType(Me.ItemLoadHeaderIDUNE, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CreateDateUDTE, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ugrdList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents SearchGroupBox As System.Windows.Forms.GroupBox
    Public WithEvents cmdSearch As System.Windows.Forms.Button
    Friend WithEvents CreateDateUDTE As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
    Public WithEvents LoadDateLabel As System.Windows.Forms.Label
    Public WithEvents BatchDescriptionTextBox As System.Windows.Forms.TextBox
    Public WithEvents BatchDescLabel As System.Windows.Forms.Label
    Public WithEvents ImportTypeComboBox As System.Windows.Forms.ComboBox
    Public WithEvents ImportTypeLabel As System.Windows.Forms.Label
    Public WithEvents ItemLoadHeaderIDLabel As System.Windows.Forms.Label
    Public WithEvents cmdExit As System.Windows.Forms.Button
    Friend WithEvents ugrdList As Infragistics.Win.UltraWinGrid.UltraGrid
    Friend WithEvents PriceBatchButton As System.Windows.Forms.Button
    Public WithEvents ItemLoadDetailButton As System.Windows.Forms.Button
    Public WithEvents UserComboBox As System.Windows.Forms.ComboBox
    Public WithEvents UserLabel As System.Windows.Forms.Label
    Friend WithEvents ItemLoadHeaderIDUNE As Infragistics.Win.UltraWinEditors.UltraNumericEditor
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
End Class
