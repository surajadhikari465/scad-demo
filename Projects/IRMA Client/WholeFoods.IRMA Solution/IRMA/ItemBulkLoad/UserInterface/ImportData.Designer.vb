<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ImportData
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
        Dim Appearance1 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim UltraGridBand1 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1)
        Dim UltraGridColumn1 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Identifier")
        Dim UltraGridColumn2 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("POS Description")
        Dim UltraGridColumn3 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Description")
        Dim UltraGridColumn4 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Tax Class", -1, 1279235)
        Dim UltraGridColumn5 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Food Stamps")
        Dim UltraGridColumn6 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Restricted Hours")
        Dim UltraGridColumn7 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Employee Discountable")
        Dim UltraGridColumn8 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Discontinued")
        Dim UltraGridColumn9 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("National Class", -1, 21052594)
        Dim UltraGridColumn10 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("ItemUploadDetail_ID")
        Dim UltraGridColumn11 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Sub Team No")
        Dim UltraGridColumn12 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Item Identifier Valid")
        Dim UltraGridColumn13 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Sub Team Allowed")
        Dim UltraGridColumn14 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Sub Team")
        Dim ColScrollRegion1 As Infragistics.Win.UltraWinGrid.ColScrollRegion = New Infragistics.Win.UltraWinGrid.ColScrollRegion(959)
        Dim ColScrollRegion2 As Infragistics.Win.UltraWinGrid.ColScrollRegion = New Infragistics.Win.UltraWinGrid.ColScrollRegion(959)
        Dim Appearance2 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance3 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance4 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance5 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance6 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance7 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance8 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance9 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance10 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim ValueList1 As Infragistics.Win.ValueList = New Infragistics.Win.ValueList(1279235)
        Dim ValueList2 As Infragistics.Win.ValueList = New Infragistics.Win.ValueList(21052594)
        Me.btnSelectFile = New System.Windows.Forms.Button
        Me.selectFileDialog = New System.Windows.Forms.OpenFileDialog
        Me.txtFile = New System.Windows.Forms.TextBox
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnUpload = New System.Windows.Forms.Button
        Me.ugrdBulkLoad = New Infragistics.Win.UltraWinGrid.UltraGrid
        Me.btnValidate = New System.Windows.Forms.Button
        Me.FileGroupBox = New System.Windows.Forms.GroupBox
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        CType(Me.ugrdBulkLoad, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.FileGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnSelectFile
        '
        Me.btnSelectFile.Location = New System.Drawing.Point(256, 24)
        Me.btnSelectFile.Name = "btnSelectFile"
        Me.btnSelectFile.Size = New System.Drawing.Size(75, 23)
        Me.btnSelectFile.TabIndex = 1
        Me.btnSelectFile.Text = "Select File"
        Me.ToolTip1.SetToolTip(Me.btnSelectFile, "Select spreadsheet to upload")
        Me.btnSelectFile.UseVisualStyleBackColor = True
        '
        'txtFile
        '
        Me.txtFile.Location = New System.Drawing.Point(6, 26)
        Me.txtFile.Name = "txtFile"
        Me.txtFile.Size = New System.Drawing.Size(235, 20)
        Me.txtFile.TabIndex = 0
        '
        'btnExit
        '
        Me.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnExit.Location = New System.Drawing.Point(898, 601)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(75, 23)
        Me.btnExit.TabIndex = 2
        Me.btnExit.Text = "Exit"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnUpload
        '
        Me.btnUpload.Enabled = False
        Me.btnUpload.Location = New System.Drawing.Point(807, 601)
        Me.btnUpload.Name = "btnUpload"
        Me.btnUpload.Size = New System.Drawing.Size(75, 23)
        Me.btnUpload.TabIndex = 1
        Me.btnUpload.Text = "Upload"
        Me.ToolTip1.SetToolTip(Me.btnUpload, "Upload item changes to the database")
        Me.btnUpload.UseVisualStyleBackColor = True
        '
        'ugrdBulkLoad
        '
        Appearance1.BackColor = System.Drawing.SystemColors.Window
        Appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.ugrdBulkLoad.DisplayLayout.Appearance = Appearance1
        Me.ugrdBulkLoad.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        UltraGridColumn1.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn1.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn1.Header.VisiblePosition = 0
        UltraGridColumn1.Width = 92
        UltraGridColumn2.Header.VisiblePosition = 1
        UltraGridColumn2.MaxLength = 26
        UltraGridColumn2.Width = 110
        UltraGridColumn3.Header.VisiblePosition = 2
        UltraGridColumn3.MaxLength = 60
        UltraGridColumn3.Width = 108
        UltraGridColumn4.Header.VisiblePosition = 3
        UltraGridColumn4.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList
        UltraGridColumn4.Width = 55
        UltraGridColumn5.Header.VisiblePosition = 4
        UltraGridColumn5.Width = 65
        UltraGridColumn6.CellMultiLine = Infragistics.Win.DefaultableBoolean.[True]
        UltraGridColumn6.Header.VisiblePosition = 5
        UltraGridColumn6.Width = 76
        UltraGridColumn7.CellMultiLine = Infragistics.Win.DefaultableBoolean.[True]
        UltraGridColumn7.Header.VisiblePosition = 6
        UltraGridColumn7.Width = 81
        UltraGridColumn8.Header.VisiblePosition = 7
        UltraGridColumn8.Width = 90
        UltraGridColumn9.Header.VisiblePosition = 8
        UltraGridColumn9.MaxLength = 195
        UltraGridColumn9.MinWidth = 90
        UltraGridColumn9.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList
        UltraGridColumn9.Width = 184
        UltraGridColumn10.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn10.Header.VisiblePosition = 9
        UltraGridColumn10.Hidden = True
        UltraGridColumn11.Header.VisiblePosition = 10
        UltraGridColumn11.Hidden = True
        UltraGridColumn12.Header.VisiblePosition = 11
        UltraGridColumn12.Hidden = True
        UltraGridColumn13.Header.VisiblePosition = 12
        UltraGridColumn13.Hidden = True
        UltraGridColumn14.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn14.Header.VisiblePosition = 13
        UltraGridColumn14.Width = 98
        UltraGridBand1.Columns.AddRange(New Object() {UltraGridColumn1, UltraGridColumn2, UltraGridColumn3, UltraGridColumn4, UltraGridColumn5, UltraGridColumn6, UltraGridColumn7, UltraGridColumn8, UltraGridColumn9, UltraGridColumn10, UltraGridColumn11, UltraGridColumn12, UltraGridColumn13, UltraGridColumn14})
        Me.ugrdBulkLoad.DisplayLayout.BandsSerializer.Add(UltraGridBand1)
        Me.ugrdBulkLoad.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.ugrdBulkLoad.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Me.ugrdBulkLoad.DisplayLayout.ColScrollRegions.Add(ColScrollRegion1)
        Me.ugrdBulkLoad.DisplayLayout.ColScrollRegions.Add(ColScrollRegion2)
        Appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance2.BorderColor = System.Drawing.SystemColors.Window
        Me.ugrdBulkLoad.DisplayLayout.GroupByBox.Appearance = Appearance2
        Appearance3.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugrdBulkLoad.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance3
        Me.ugrdBulkLoad.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.ugrdBulkLoad.DisplayLayout.GroupByBox.Hidden = True
        Appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance4.BackColor2 = System.Drawing.SystemColors.Control
        Appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance4.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugrdBulkLoad.DisplayLayout.GroupByBox.PromptAppearance = Appearance4
        Me.ugrdBulkLoad.DisplayLayout.MaxColScrollRegions = 1
        Me.ugrdBulkLoad.DisplayLayout.MaxRowScrollRegions = 1
        Me.ugrdBulkLoad.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.ugrdBulkLoad.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance5.BackColor = System.Drawing.SystemColors.Window
        Me.ugrdBulkLoad.DisplayLayout.Override.CardAreaAppearance = Appearance5
        Appearance6.BorderColor = System.Drawing.Color.Silver
        Appearance6.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.ugrdBulkLoad.DisplayLayout.Override.CellAppearance = Appearance6
        Me.ugrdBulkLoad.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.ugrdBulkLoad.DisplayLayout.Override.CellPadding = 0
        Appearance7.BackColor = System.Drawing.SystemColors.Control
        Appearance7.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance7.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance7.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance7.BorderColor = System.Drawing.SystemColors.Window
        Me.ugrdBulkLoad.DisplayLayout.Override.GroupByRowAppearance = Appearance7
        Appearance8.TextHAlign = Infragistics.Win.HAlign.Center
        Me.ugrdBulkLoad.DisplayLayout.Override.HeaderAppearance = Appearance8
        Me.ugrdBulkLoad.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti
        Me.ugrdBulkLoad.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance9.BackColor = System.Drawing.Color.White
        Appearance9.BorderColor = System.Drawing.Color.Silver
        Me.ugrdBulkLoad.DisplayLayout.Override.RowAppearance = Appearance9
        Me.ugrdBulkLoad.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[False]
        Appearance10.BackColor = System.Drawing.SystemColors.ControlLight
        Me.ugrdBulkLoad.DisplayLayout.Override.TemplateAddRowAppearance = Appearance10
        Me.ugrdBulkLoad.DisplayLayout.Override.WrapHeaderText = Infragistics.Win.DefaultableBoolean.[True]
        Me.ugrdBulkLoad.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.ugrdBulkLoad.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        ValueList1.Key = "TaxClass"
        ValueList2.Key = "NationalClass"
        Me.ugrdBulkLoad.DisplayLayout.ValueLists.AddRange(New Infragistics.Win.ValueList() {ValueList1, ValueList2})
        Me.ugrdBulkLoad.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        Me.ugrdBulkLoad.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ugrdBulkLoad.Location = New System.Drawing.Point(12, 71)
        Me.ugrdBulkLoad.Name = "ugrdBulkLoad"
        Me.ugrdBulkLoad.Size = New System.Drawing.Size(961, 515)
        Me.ugrdBulkLoad.TabIndex = 3
        Me.ugrdBulkLoad.Text = "UltraGrid1"
        '
        'btnValidate
        '
        Me.btnValidate.Enabled = False
        Me.btnValidate.Location = New System.Drawing.Point(713, 601)
        Me.btnValidate.Name = "btnValidate"
        Me.btnValidate.Size = New System.Drawing.Size(75, 23)
        Me.btnValidate.TabIndex = 0
        Me.btnValidate.Text = "Validate"
        Me.ToolTip1.SetToolTip(Me.btnValidate, "Validate Item Changes")
        Me.btnValidate.UseVisualStyleBackColor = True
        '
        'FileGroupBox
        '
        Me.FileGroupBox.Controls.Add(Me.btnSelectFile)
        Me.FileGroupBox.Controls.Add(Me.txtFile)
        Me.FileGroupBox.Location = New System.Drawing.Point(12, 12)
        Me.FileGroupBox.Name = "FileGroupBox"
        Me.FileGroupBox.Size = New System.Drawing.Size(345, 53)
        Me.FileGroupBox.TabIndex = 9
        Me.FileGroupBox.TabStop = False
        Me.FileGroupBox.Text = "File Selection"
        '
        'ImportData
        '
        Me.AcceptButton = Me.btnSelectFile
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnExit
        Me.ClientSize = New System.Drawing.Size(976, 636)
        Me.Controls.Add(Me.FileGroupBox)
        Me.Controls.Add(Me.btnValidate)
        Me.Controls.Add(Me.ugrdBulkLoad)
        Me.Controls.Add(Me.btnUpload)
        Me.Controls.Add(Me.btnExit)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Name = "ImportData"
        Me.ShowInTaskbar = False
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show
        Me.Text = "Item Maintenance Bulk Load"
        CType(Me.ugrdBulkLoad, System.ComponentModel.ISupportInitialize).EndInit()
        Me.FileGroupBox.ResumeLayout(False)
        Me.FileGroupBox.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnSelectFile As System.Windows.Forms.Button
    Friend WithEvents selectFileDialog As System.Windows.Forms.OpenFileDialog
    Friend WithEvents txtFile As System.Windows.Forms.TextBox
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnUpload As System.Windows.Forms.Button
    Friend WithEvents ugrdBulkLoad As Infragistics.Win.UltraWinGrid.UltraGrid
    Friend WithEvents btnValidate As System.Windows.Forms.Button
    Friend WithEvents FileGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip

End Class
