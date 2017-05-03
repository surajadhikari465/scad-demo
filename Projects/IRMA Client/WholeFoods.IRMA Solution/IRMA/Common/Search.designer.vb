<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmSearch
#Region "Windows Form Designer generated code "
	<System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
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
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSearch))
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
        Dim Appearance14 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdSearch = New System.Windows.Forms.Button
        Me.cmdExit = New System.Windows.Forms.Button
        Me.cmdSelect = New System.Windows.Forms.Button
        Me.gbxSearchBy = New System.Windows.Forms.GroupBox
        Me.optPSVendorID = New System.Windows.Forms.RadioButton
        Me.optVendorID = New System.Windows.Forms.RadioButton
        Me.optCompany = New System.Windows.Forms.RadioButton
        Me.lblLabel = New System.Windows.Forms.Label
        Me.gbxBody = New System.Windows.Forms.GroupBox
        Me.ugrdSearchResults = New Infragistics.Win.UltraWinGrid.UltraGrid
        Me.txtSearch = New System.Windows.Forms.TextBox
        Me.gbxSearchBy.SuspendLayout()
        Me.gbxBody.SuspendLayout()
        CType(Me.ugrdSearchResults, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdSearch
        '
        Me.cmdSearch.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSearch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSearch.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSearch.Image = CType(resources.GetObject("cmdSearch.Image"), System.Drawing.Image)
        Me.cmdSearch.Location = New System.Drawing.Point(424, 12)
        Me.cmdSearch.Name = "cmdSearch"
        Me.cmdSearch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSearch.Size = New System.Drawing.Size(21, 21)
        Me.cmdSearch.TabIndex = 1
        Me.cmdSearch.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdSearch, "Search")
        Me.cmdSearch.UseVisualStyleBackColor = False
        '
        'cmdExit
        '
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdExit.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Image = CType(resources.GetObject("cmdExit.Image"), System.Drawing.Image)
        Me.cmdExit.Location = New System.Drawing.Point(406, 241)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(41, 41)
        Me.cmdExit.TabIndex = 4
        Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdExit, "Exit")
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'cmdSelect
        '
        Me.cmdSelect.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSelect.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSelect.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSelect.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSelect.Image = CType(resources.GetObject("cmdSelect.Image"), System.Drawing.Image)
        Me.cmdSelect.Location = New System.Drawing.Point(358, 241)
        Me.cmdSelect.Name = "cmdSelect"
        Me.cmdSelect.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSelect.Size = New System.Drawing.Size(41, 41)
        Me.cmdSelect.TabIndex = 3
        Me.cmdSelect.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdSelect, "Select")
        Me.cmdSelect.UseVisualStyleBackColor = False
        '
        'gbxSearchBy
        '
        Me.gbxSearchBy.Controls.Add(Me.optPSVendorID)
        Me.gbxSearchBy.Controls.Add(Me.optVendorID)
        Me.gbxSearchBy.Controls.Add(Me.optCompany)
        Me.gbxSearchBy.Controls.Add(Me.lblLabel)
        Me.gbxSearchBy.Location = New System.Drawing.Point(7, 2)
        Me.gbxSearchBy.Name = "gbxSearchBy"
        Me.gbxSearchBy.Size = New System.Drawing.Size(458, 35)
        Me.gbxSearchBy.TabIndex = 1
        Me.gbxSearchBy.TabStop = False
        '
        'optPSVendorID
        '
        Me.optPSVendorID.BackColor = System.Drawing.SystemColors.Control
        Me.optPSVendorID.Cursor = System.Windows.Forms.Cursors.Default
        Me.optPSVendorID.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optPSVendorID.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optPSVendorID.Location = New System.Drawing.Point(313, 10)
        Me.optPSVendorID.Name = "optPSVendorID"
        Me.optPSVendorID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optPSVendorID.Size = New System.Drawing.Size(107, 20)
        Me.optPSVendorID.TabIndex = 3
        Me.optPSVendorID.TabStop = True
        Me.optPSVendorID.Text = "PS Vendor ID"
        Me.optPSVendorID.UseVisualStyleBackColor = False
        '
        'optVendorID
        '
        Me.optVendorID.BackColor = System.Drawing.SystemColors.Control
        Me.optVendorID.Cursor = System.Windows.Forms.Cursors.Default
        Me.optVendorID.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optVendorID.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optVendorID.Location = New System.Drawing.Point(211, 10)
        Me.optVendorID.Name = "optVendorID"
        Me.optVendorID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optVendorID.Size = New System.Drawing.Size(82, 20)
        Me.optVendorID.TabIndex = 2
        Me.optVendorID.TabStop = True
        Me.optVendorID.Text = "Vendor ID"
        Me.optVendorID.UseVisualStyleBackColor = False
        '
        'optCompany
        '
        Me.optCompany.BackColor = System.Drawing.SystemColors.Control
        Me.optCompany.Checked = True
        Me.optCompany.Cursor = System.Windows.Forms.Cursors.Default
        Me.optCompany.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optCompany.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optCompany.Location = New System.Drawing.Point(92, 11)
        Me.optCompany.Name = "optCompany"
        Me.optCompany.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optCompany.Size = New System.Drawing.Size(113, 19)
        Me.optCompany.TabIndex = 1
        Me.optCompany.TabStop = True
        Me.optCompany.Text = "Company Name"
        Me.optCompany.UseVisualStyleBackColor = False
        '
        'lblLabel
        '
        Me.lblLabel.BackColor = System.Drawing.Color.Transparent
        Me.lblLabel.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLabel.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLabel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.Location = New System.Drawing.Point(16, 11)
        Me.lblLabel.Name = "lblLabel"
        Me.lblLabel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLabel.Size = New System.Drawing.Size(68, 19)
        Me.lblLabel.TabIndex = 0
        Me.lblLabel.Text = "Search By :"
        Me.lblLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'gbxBody
        '
        Me.gbxBody.Controls.Add(Me.ugrdSearchResults)
        Me.gbxBody.Controls.Add(Me.cmdSearch)
        Me.gbxBody.Controls.Add(Me.txtSearch)
        Me.gbxBody.Controls.Add(Me.cmdExit)
        Me.gbxBody.Controls.Add(Me.cmdSelect)
        Me.gbxBody.Location = New System.Drawing.Point(7, 38)
        Me.gbxBody.Name = "gbxBody"
        Me.gbxBody.Size = New System.Drawing.Size(458, 288)
        Me.gbxBody.TabIndex = 0
        Me.gbxBody.TabStop = False
        '
        'ugrdSearchResults
        '
        Appearance1.BackColor = System.Drawing.SystemColors.ControlLight
        Appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.ugrdSearchResults.DisplayLayout.Appearance = Appearance1
        Me.ugrdSearchResults.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn
        Me.ugrdSearchResults.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Appearance2.FontData.BoldAsString = "True"
        Me.ugrdSearchResults.DisplayLayout.CaptionAppearance = Appearance2
        Me.ugrdSearchResults.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[True]
        Appearance3.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance3.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance3.BorderColor = System.Drawing.SystemColors.Window
        Me.ugrdSearchResults.DisplayLayout.GroupByBox.Appearance = Appearance3
        Appearance4.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugrdSearchResults.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance4
        Me.ugrdSearchResults.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.ugrdSearchResults.DisplayLayout.GroupByBox.Hidden = True
        Appearance5.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance5.BackColor2 = System.Drawing.SystemColors.Control
        Appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance5.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugrdSearchResults.DisplayLayout.GroupByBox.PromptAppearance = Appearance5
        Me.ugrdSearchResults.DisplayLayout.MaxColScrollRegions = 1
        Me.ugrdSearchResults.DisplayLayout.MaxRowScrollRegions = 1
        Appearance6.BackColor = System.Drawing.SystemColors.Window
        Appearance6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ugrdSearchResults.DisplayLayout.Override.ActiveCellAppearance = Appearance6
        Me.ugrdSearchResults.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.ugrdSearchResults.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance7.BackColor = System.Drawing.SystemColors.Window
        Me.ugrdSearchResults.DisplayLayout.Override.CardAreaAppearance = Appearance7
        Appearance8.BorderColor = System.Drawing.Color.Silver
        Appearance8.FontData.BoldAsString = "True"
        Appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.ugrdSearchResults.DisplayLayout.Override.CellAppearance = Appearance8
        Me.ugrdSearchResults.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.ugrdSearchResults.DisplayLayout.Override.CellPadding = 0
        Appearance9.FontData.BoldAsString = "True"
        Me.ugrdSearchResults.DisplayLayout.Override.FixedHeaderAppearance = Appearance9
        Appearance10.BackColor = System.Drawing.SystemColors.Control
        Appearance10.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance10.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance10.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance10.BorderColor = System.Drawing.SystemColors.Window
        Me.ugrdSearchResults.DisplayLayout.Override.GroupByRowAppearance = Appearance10
        Appearance11.FontData.BoldAsString = "True"
        Appearance11.TextHAlignAsString = "Left"
        Me.ugrdSearchResults.DisplayLayout.Override.HeaderAppearance = Appearance11
        Me.ugrdSearchResults.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle
        Me.ugrdSearchResults.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance12.BackColor = System.Drawing.SystemColors.ControlLight
        Me.ugrdSearchResults.DisplayLayout.Override.RowAlternateAppearance = Appearance12
        Appearance13.BackColor = System.Drawing.SystemColors.Window
        Appearance13.BorderColor = System.Drawing.Color.Silver
        Me.ugrdSearchResults.DisplayLayout.Override.RowAppearance = Appearance13
        Me.ugrdSearchResults.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[True]
        Me.ugrdSearchResults.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed
        Me.ugrdSearchResults.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.ugrdSearchResults.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.ugrdSearchResults.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.[Single]
        Appearance14.BackColor = System.Drawing.SystemColors.ControlLight
        Me.ugrdSearchResults.DisplayLayout.Override.TemplateAddRowAppearance = Appearance14
        Me.ugrdSearchResults.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.ugrdSearchResults.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.ugrdSearchResults.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand
        Me.ugrdSearchResults.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        Me.ugrdSearchResults.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ugrdSearchResults.Location = New System.Drawing.Point(9, 39)
        Me.ugrdSearchResults.Name = "ugrdSearchResults"
        Me.ugrdSearchResults.Size = New System.Drawing.Size(438, 196)
        Me.ugrdSearchResults.TabIndex = 27
        Me.ugrdSearchResults.Text = "Search Results"
        '
        'txtSearch
        '
        Me.txtSearch.AcceptsReturn = True
        Me.txtSearch.BackColor = System.Drawing.SystemColors.Window
        Me.txtSearch.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSearch.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSearch.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtSearch.Location = New System.Drawing.Point(9, 13)
        Me.txtSearch.MaxLength = 50
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtSearch.Size = New System.Drawing.Size(413, 20)
        Me.txtSearch.TabIndex = 0
        '
        'frmSearch
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(472, 333)
        Me.Controls.Add(Me.gbxBody)
        Me.Controls.Add(Me.gbxSearchBy)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Location = New System.Drawing.Point(476, 460)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmSearch"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.gbxSearchBy.ResumeLayout(False)
        Me.gbxBody.ResumeLayout(False)
        Me.gbxBody.PerformLayout()
        CType(Me.ugrdSearchResults, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents gbxSearchBy As System.Windows.Forms.GroupBox
    Public WithEvents optPSVendorID As System.Windows.Forms.RadioButton
    Public WithEvents optVendorID As System.Windows.Forms.RadioButton
    Public WithEvents optCompany As System.Windows.Forms.RadioButton
    Public WithEvents lblLabel As System.Windows.Forms.Label
    Friend WithEvents gbxBody As System.Windows.Forms.GroupBox
    Public WithEvents cmdSearch As System.Windows.Forms.Button
    Public WithEvents txtSearch As System.Windows.Forms.TextBox
    Public WithEvents cmdExit As System.Windows.Forms.Button
    Public WithEvents cmdSelect As System.Windows.Forms.Button
    Friend WithEvents ugrdSearchResults As Infragistics.Win.UltraWinGrid.UltraGrid
#End Region 
End Class