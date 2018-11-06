<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBatchReceiveClose
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmBatchReceiveClose))
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
        Dim Appearance13 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Me.cmbFacility = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.cmdExit = New System.Windows.Forms.Button
        Me.cmdReceiveAndClose = New System.Windows.Forms.Button
        Me.cmdSelectAll = New System.Windows.Forms.Button
        Me.dtpEndDate = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
        Me.dtpStartDate = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
        Me.lblDates = New System.Windows.Forms.Label
        Me.lblDash = New System.Windows.Forms.Label
        Me.cmdSearch = New System.Windows.Forms.Button
        Me.cmbSubteam = New System.Windows.Forms.ComboBox
        Me.ugOrderList = New Infragistics.Win.UltraWinGrid.UltraGrid
        Me.imgIcons = New System.Windows.Forms.ImageList(Me.components)
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        CType(Me.dtpEndDate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dtpStartDate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ugOrderList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmbFacility
        '
        Me.cmbFacility.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.cmbFacility.FormattingEnabled = True
        Me.cmbFacility.Location = New System.Drawing.Point(190, 17)
        Me.cmbFacility.Name = "cmbFacility"
        Me.cmbFacility.Size = New System.Drawing.Size(191, 22)
        Me.cmbFacility.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(134, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(50, 14)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Facility :"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(125, 53)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(62, 14)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Subteam :"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'cmdExit
        '
        Me.cmdExit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdExit.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Image = CType(resources.GetObject("cmdExit.Image"), System.Drawing.Image)
        Me.cmdExit.Location = New System.Drawing.Point(641, 485)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(41, 41)
        Me.cmdExit.TabIndex = 13
        Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdExit, "Exit")
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'cmdReceiveAndClose
        '
        Me.cmdReceiveAndClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdReceiveAndClose.BackColor = System.Drawing.SystemColors.Control
        Me.cmdReceiveAndClose.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdReceiveAndClose.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdReceiveAndClose.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdReceiveAndClose.Image = CType(resources.GetObject("cmdReceiveAndClose.Image"), System.Drawing.Image)
        Me.cmdReceiveAndClose.Location = New System.Drawing.Point(594, 485)
        Me.cmdReceiveAndClose.Name = "cmdReceiveAndClose"
        Me.cmdReceiveAndClose.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdReceiveAndClose.Size = New System.Drawing.Size(41, 41)
        Me.cmdReceiveAndClose.TabIndex = 34
        Me.cmdReceiveAndClose.Tag = "BDA"
        Me.cmdReceiveAndClose.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdReceiveAndClose, "Receive and Close All Selected Orders")
        Me.cmdReceiveAndClose.UseVisualStyleBackColor = False
        '
        'cmdSelectAll
        '
        Me.cmdSelectAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdSelectAll.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSelectAll.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSelectAll.Font = New System.Drawing.Font("Arial", 8.0!)
        Me.cmdSelectAll.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSelectAll.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.cmdSelectAll.Location = New System.Drawing.Point(547, 485)
        Me.cmdSelectAll.Name = "cmdSelectAll"
        Me.cmdSelectAll.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSelectAll.Size = New System.Drawing.Size(41, 41)
        Me.cmdSelectAll.TabIndex = 39
        Me.cmdSelectAll.Tag = "Select"
        Me.cmdSelectAll.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdSelectAll, "Select All")
        Me.cmdSelectAll.UseVisualStyleBackColor = False
        '
        'dtpEndDate
        '
        Me.dtpEndDate.DateTime = New Date(1980, 1, 1, 0, 0, 0, 0)
        Me.dtpEndDate.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.dtpEndDate.Location = New System.Drawing.Point(296, 82)
        Me.dtpEndDate.MaskInput = ""
        Me.dtpEndDate.MaxDate = New Date(2099, 12, 31, 0, 0, 0, 0)
        Me.dtpEndDate.MinDate = New Date(1980, 1, 1, 0, 0, 0, 0)
        Me.dtpEndDate.Name = "dtpEndDate"
        Me.dtpEndDate.Size = New System.Drawing.Size(85, 21)
        Me.dtpEndDate.TabIndex = 43
        Me.dtpEndDate.Value = Nothing
        '
        'dtpStartDate
        '
        Me.dtpStartDate.DateTime = New Date(1980, 1, 1, 0, 0, 0, 0)
        Me.dtpStartDate.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.dtpStartDate.Location = New System.Drawing.Point(190, 83)
        Me.dtpStartDate.MaskInput = ""
        Me.dtpStartDate.MaxDate = New Date(2099, 12, 31, 0, 0, 0, 0)
        Me.dtpStartDate.MinDate = New Date(1980, 1, 1, 0, 0, 0, 0)
        Me.dtpStartDate.Name = "dtpStartDate"
        Me.dtpStartDate.Size = New System.Drawing.Size(85, 21)
        Me.dtpStartDate.TabIndex = 42
        Me.dtpStartDate.Value = Nothing
        '
        'lblDates
        '
        Me.lblDates.BackColor = System.Drawing.Color.Transparent
        Me.lblDates.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDates.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.lblDates.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDates.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblDates.Location = New System.Drawing.Point(62, 86)
        Me.lblDates.Name = "lblDates"
        Me.lblDates.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDates.Size = New System.Drawing.Size(127, 17)
        Me.lblDates.TabIndex = 40
        Me.lblDates.Text = "Expected Date Range :"
        Me.lblDates.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblDash
        '
        Me.lblDash.BackColor = System.Drawing.Color.Transparent
        Me.lblDash.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDash.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold)
        Me.lblDash.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDash.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblDash.Location = New System.Drawing.Point(278, 82)
        Me.lblDash.Name = "lblDash"
        Me.lblDash.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDash.Size = New System.Drawing.Size(17, 17)
        Me.lblDash.TabIndex = 41
        Me.lblDash.Text = "-"
        Me.lblDash.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'cmdSearch
        '
        Me.cmdSearch.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSearch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSearch.Font = New System.Drawing.Font("Arial", 8.0!)
        Me.cmdSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSearch.Image = CType(resources.GetObject("cmdSearch.Image"), System.Drawing.Image)
        Me.cmdSearch.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.cmdSearch.Location = New System.Drawing.Point(392, 64)
        Me.cmdSearch.Name = "cmdSearch"
        Me.cmdSearch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSearch.Size = New System.Drawing.Size(42, 41)
        Me.cmdSearch.TabIndex = 51
        Me.cmdSearch.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdSearch, "Start Search")
        Me.cmdSearch.UseVisualStyleBackColor = False
        '
        'cmbSubteam
        '
        Me.cmbSubteam.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbSubteam.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbSubteam.BackColor = System.Drawing.SystemColors.Window
        Me.cmbSubteam.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbSubteam.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbSubteam.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbSubteam.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbSubteam.Location = New System.Drawing.Point(190, 50)
        Me.cmbSubteam.Name = "cmbSubteam"
        Me.cmbSubteam.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbSubteam.Size = New System.Drawing.Size(191, 22)
        Me.cmbSubteam.Sorted = True
        Me.cmbSubteam.TabIndex = 52
        '
        'ugOrderList
        '
        Me.ugOrderList.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Appearance1.BackColor = System.Drawing.Color.White
        Me.ugOrderList.DisplayLayout.Appearance = Appearance1
        Me.ugOrderList.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.ugOrderList.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance2.BorderColor = System.Drawing.SystemColors.Window
        Me.ugOrderList.DisplayLayout.GroupByBox.Appearance = Appearance2
        Appearance3.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugOrderList.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance3
        Me.ugOrderList.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.ugOrderList.DisplayLayout.GroupByBox.Hidden = True
        Appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance4.BackColor2 = System.Drawing.SystemColors.Control
        Appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance4.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugOrderList.DisplayLayout.GroupByBox.PromptAppearance = Appearance4
        Me.ugOrderList.DisplayLayout.MaxColScrollRegions = 1
        Me.ugOrderList.DisplayLayout.MaxRowScrollRegions = 1
        Appearance5.BackColor = System.Drawing.SystemColors.Window
        Appearance5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ugOrderList.DisplayLayout.Override.ActiveCellAppearance = Appearance5
        Appearance6.BackColor = System.Drawing.SystemColors.Highlight
        Appearance6.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.ugOrderList.DisplayLayout.Override.ActiveRowAppearance = Appearance6
        Me.ugOrderList.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No
        Me.ugOrderList.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.[False]
        Me.ugOrderList.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.[False]
        Me.ugOrderList.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.None
        Me.ugOrderList.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance7.BackColor = System.Drawing.Color.Transparent
        Me.ugOrderList.DisplayLayout.Override.CardAreaAppearance = Appearance7
        Appearance8.BorderColor = System.Drawing.Color.Silver
        Appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.ugOrderList.DisplayLayout.Override.CellAppearance = Appearance8
        Me.ugOrderList.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        Me.ugOrderList.DisplayLayout.Override.CellPadding = 3
        Me.ugOrderList.DisplayLayout.Override.ColumnAutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.AllRowsInBand
        Appearance9.BackColor = System.Drawing.SystemColors.Control
        Appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance9.BorderColor = System.Drawing.SystemColors.Window
        Me.ugOrderList.DisplayLayout.Override.GroupByRowAppearance = Appearance9
        Appearance10.TextHAlign = Infragistics.Win.HAlign.Left
        Me.ugOrderList.DisplayLayout.Override.HeaderAppearance = Appearance10
        Me.ugOrderList.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti
        Me.ugOrderList.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance11.BorderColor = System.Drawing.Color.LightGray
        Appearance11.TextVAlign = Infragistics.Win.VAlign.Middle
        Me.ugOrderList.DisplayLayout.Override.RowAppearance = Appearance11
        Me.ugOrderList.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[False]
        Appearance12.BackColor = System.Drawing.Color.LightSteelBlue
        Appearance12.BorderColor = System.Drawing.Color.Black
        Appearance12.ForeColor = System.Drawing.Color.Black
        Me.ugOrderList.DisplayLayout.Override.SelectedRowAppearance = Appearance12
        Me.ugOrderList.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.ugOrderList.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.ugOrderList.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.ExtendedAutoDrag
        Appearance13.BackColor = System.Drawing.SystemColors.ControlLight
        Me.ugOrderList.DisplayLayout.Override.TemplateAddRowAppearance = Appearance13
        Me.ugOrderList.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.None
        Me.ugOrderList.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.ugOrderList.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.ugOrderList.DisplayLayout.TabNavigation = Infragistics.Win.UltraWinGrid.TabNavigation.NextControl
        Me.ugOrderList.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        Me.ugOrderList.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ugOrderList.Location = New System.Drawing.Point(60, 110)
        Me.ugOrderList.Name = "ugOrderList"
        Me.ugOrderList.Size = New System.Drawing.Size(575, 369)
        Me.ugOrderList.TabIndex = 8
        Me.ugOrderList.Text = "UltraGrid1"
        '
        'imgIcons
        '
        Me.imgIcons.ImageStream = CType(resources.GetObject("imgIcons.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imgIcons.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.imgIcons.Images.SetKeyName(0, "All")
        Me.imgIcons.Images.SetKeyName(1, "None")
        '
        'frmBatchReceiveClose
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(694, 538)
        Me.Controls.Add(Me.cmbSubteam)
        Me.Controls.Add(Me.cmdSearch)
        Me.Controls.Add(Me.dtpEndDate)
        Me.Controls.Add(Me.dtpStartDate)
        Me.Controls.Add(Me.lblDates)
        Me.Controls.Add(Me.lblDash)
        Me.Controls.Add(Me.cmdSelectAll)
        Me.Controls.Add(Me.cmdReceiveAndClose)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.ugOrderList)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cmbFacility)
        Me.Name = "frmBatchReceiveClose"
        Me.Text = "Batch Receive and Close"
        CType(Me.dtpEndDate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dtpStartDate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ugOrderList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cmbFacility As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Public WithEvents cmdExit As System.Windows.Forms.Button
    Public WithEvents cmdReceiveAndClose As System.Windows.Forms.Button
    Public WithEvents cmdSelectAll As System.Windows.Forms.Button
    Friend WithEvents dtpEndDate As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
    Friend WithEvents dtpStartDate As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
    Public WithEvents lblDates As System.Windows.Forms.Label
    Public WithEvents lblDash As System.Windows.Forms.Label
    Public WithEvents cmdSearch As System.Windows.Forms.Button
    Public WithEvents cmbSubteam As System.Windows.Forms.ComboBox
    Friend WithEvents ugOrderList As Infragistics.Win.UltraWinGrid.UltraGrid
    Public WithEvents imgIcons As System.Windows.Forms.ImageList
    Public WithEvents ToolTip1 As System.Windows.Forms.ToolTip
End Class
