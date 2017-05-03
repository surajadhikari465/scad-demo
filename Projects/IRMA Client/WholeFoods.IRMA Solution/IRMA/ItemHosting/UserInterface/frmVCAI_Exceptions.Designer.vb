<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmVCAI_Exceptions
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
	Public WithEvents cmdSelect As System.Windows.Forms.Button
	Public WithEvents cmdExit As System.Windows.Forms.Button
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmVCAI_Exceptions))
        Dim Appearance1 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim UltraGridBand1 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1)
        Dim UltraGridColumn1 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Vendor_ID")
        Dim UltraGridColumn2 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyName", -1, Nothing, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, False)
        Dim UltraGridColumn3 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("ExceptionCount")
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
        Me.cmdSelect = New System.Windows.Forms.Button
        Me.cmdExit = New System.Windows.Forms.Button
        Me.ugrdVCAI_Exceptions = New Infragistics.Win.UltraWinGrid.UltraGrid
        CType(Me.ugrdVCAI_Exceptions, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdSelect
        '
        Me.cmdSelect.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSelect.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdSelect, "cmdSelect")
        Me.cmdSelect.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSelect.Name = "cmdSelect"
        Me.ToolTip1.SetToolTip(Me.cmdSelect, resources.GetString("cmdSelect.ToolTip"))
        Me.cmdSelect.UseVisualStyleBackColor = False
        '
        'cmdExit
        '
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdExit, "cmdExit")
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Name = "cmdExit"
        Me.ToolTip1.SetToolTip(Me.cmdExit, resources.GetString("cmdExit.ToolTip"))
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'ugrdVCAI_Exceptions
        '
        Appearance1.BackColor = System.Drawing.SystemColors.ControlLight
        Appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.ugrdVCAI_Exceptions.DisplayLayout.Appearance = Appearance1
        Me.ugrdVCAI_Exceptions.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn
        UltraGridColumn1.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn1.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn1.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn1.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn1.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn1.Header.VisiblePosition = 0
        UltraGridColumn1.Hidden = True
        UltraGridColumn2.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn2.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn2.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn2.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn2.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn2.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn2.Header.Caption = resources.GetString("resource.Caption")
        UltraGridColumn2.Header.VisiblePosition = 1
        UltraGridColumn2.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(405, 0)
        UltraGridColumn3.AllowGroupBy = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn3.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.[False]
        UltraGridColumn3.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
        UltraGridColumn3.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn3.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn3.Header.Caption = resources.GetString("resource.Caption1")
        UltraGridColumn3.Header.VisiblePosition = 2
        UltraGridBand1.Columns.AddRange(New Object() {UltraGridColumn1, UltraGridColumn2, UltraGridColumn3})
        UltraGridBand1.RowLayoutStyle = Infragistics.Win.UltraWinGrid.RowLayoutStyle.None
        Me.ugrdVCAI_Exceptions.DisplayLayout.BandsSerializer.Add(UltraGridBand1)
        Me.ugrdVCAI_Exceptions.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Appearance2.FontData.BoldAsString = resources.GetString("resource.BoldAsString")
        Me.ugrdVCAI_Exceptions.DisplayLayout.CaptionAppearance = Appearance2
        Me.ugrdVCAI_Exceptions.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[True]
        Appearance3.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance3.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance3.BorderColor = System.Drawing.SystemColors.Window
        Me.ugrdVCAI_Exceptions.DisplayLayout.GroupByBox.Appearance = Appearance3
        Appearance4.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugrdVCAI_Exceptions.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance4
        Me.ugrdVCAI_Exceptions.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.ugrdVCAI_Exceptions.DisplayLayout.GroupByBox.Hidden = True
        Appearance5.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance5.BackColor2 = System.Drawing.SystemColors.Control
        Appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance5.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugrdVCAI_Exceptions.DisplayLayout.GroupByBox.PromptAppearance = Appearance5
        Me.ugrdVCAI_Exceptions.DisplayLayout.MaxColScrollRegions = 1
        Me.ugrdVCAI_Exceptions.DisplayLayout.MaxRowScrollRegions = 1
        Appearance6.BackColor = System.Drawing.SystemColors.Window
        Appearance6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ugrdVCAI_Exceptions.DisplayLayout.Override.ActiveCellAppearance = Appearance6
        Me.ugrdVCAI_Exceptions.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.ugrdVCAI_Exceptions.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance7.BackColor = System.Drawing.SystemColors.Window
        Me.ugrdVCAI_Exceptions.DisplayLayout.Override.CardAreaAppearance = Appearance7
        Appearance8.BorderColor = System.Drawing.Color.Silver
        Appearance8.FontData.BoldAsString = resources.GetString("resource.BoldAsString1")
        Appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.ugrdVCAI_Exceptions.DisplayLayout.Override.CellAppearance = Appearance8
        Me.ugrdVCAI_Exceptions.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.ugrdVCAI_Exceptions.DisplayLayout.Override.CellPadding = 0
        Appearance9.FontData.BoldAsString = resources.GetString("resource.BoldAsString2")
        Me.ugrdVCAI_Exceptions.DisplayLayout.Override.FixedHeaderAppearance = Appearance9
        Appearance10.BackColor = System.Drawing.SystemColors.Control
        Appearance10.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance10.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance10.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance10.BorderColor = System.Drawing.SystemColors.Window
        Me.ugrdVCAI_Exceptions.DisplayLayout.Override.GroupByRowAppearance = Appearance10
        Appearance11.FontData.BoldAsString = resources.GetString("resource.BoldAsString3")
        Appearance11.TextHAlign = Infragistics.Win.HAlign.Left
        Me.ugrdVCAI_Exceptions.DisplayLayout.Override.HeaderAppearance = Appearance11
        Me.ugrdVCAI_Exceptions.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle
        Me.ugrdVCAI_Exceptions.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance12.BackColor = System.Drawing.SystemColors.ControlLight
        Me.ugrdVCAI_Exceptions.DisplayLayout.Override.RowAlternateAppearance = Appearance12
        Appearance13.BackColor = System.Drawing.SystemColors.Window
        Appearance13.BorderColor = System.Drawing.Color.Silver
        Me.ugrdVCAI_Exceptions.DisplayLayout.Override.RowAppearance = Appearance13
        Me.ugrdVCAI_Exceptions.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[True]
        Me.ugrdVCAI_Exceptions.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed
        Me.ugrdVCAI_Exceptions.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.ugrdVCAI_Exceptions.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.ugrdVCAI_Exceptions.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.[Single]
        Appearance14.BackColor = System.Drawing.SystemColors.ControlLight
        Me.ugrdVCAI_Exceptions.DisplayLayout.Override.TemplateAddRowAppearance = Appearance14
        Me.ugrdVCAI_Exceptions.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.ugrdVCAI_Exceptions.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.ugrdVCAI_Exceptions.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand
        Me.ugrdVCAI_Exceptions.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        resources.ApplyResources(Me.ugrdVCAI_Exceptions, "ugrdVCAI_Exceptions")
        Me.ugrdVCAI_Exceptions.Name = "ugrdVCAI_Exceptions"
        '
        'frmVCAI_Exceptions
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.Controls.Add(Me.ugrdVCAI_Exceptions)
        Me.Controls.Add(Me.cmdSelect)
        Me.Controls.Add(Me.cmdExit)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmVCAI_Exceptions"
        Me.ShowInTaskbar = False
        CType(Me.ugrdVCAI_Exceptions, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ugrdVCAI_Exceptions As Infragistics.Win.UltraWinGrid.UltraGrid
#End Region 
End Class