<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmNewPrimVend
#Region "Windows Form Designer generated code "
    <System.Diagnostics.DebuggerNonUserCode()> Private Sub New()
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
	Public WithEvents cmdItems As System.Windows.Forms.Button
	Public WithEvents txtCurVend As System.Windows.Forms.TextBox
	Public WithEvents cmdExit As System.Windows.Forms.Button
	Public WithEvents cmdSubmit As System.Windows.Forms.Button
    Public WithEvents lblVendor As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmNewPrimVend))
        Dim Appearance1 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim UltraGridBand1 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1)
        Dim UltraGridColumn1 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Vendor_ID")
        Dim UltraGridColumn2 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyName")
        Dim UltraGridColumn3 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("ItmCnt", -1, Nothing, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, False)
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
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdItems = New System.Windows.Forms.Button
        Me.cmdExit = New System.Windows.Forms.Button
        Me.cmdSubmit = New System.Windows.Forms.Button
        Me.txtCurVend = New System.Windows.Forms.TextBox
        Me.lblVendor = New System.Windows.Forms.Label
        Me.ugrdVendList = New Infragistics.Win.UltraWinGrid.UltraGrid
        Me.BottomToolStripPanel = New System.Windows.Forms.ToolStripPanel
        Me.TopToolStripPanel = New System.Windows.Forms.ToolStripPanel
        Me.RightToolStripPanel = New System.Windows.Forms.ToolStripPanel
        Me.LeftToolStripPanel = New System.Windows.Forms.ToolStripPanel
        Me.ContentPanel = New System.Windows.Forms.ToolStripContentPanel
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip
        Me.tsslMessage = New System.Windows.Forms.ToolStripStatusLabel
        CType(Me.ugrdVendList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdItems
        '
        Me.cmdItems.BackColor = System.Drawing.SystemColors.Control
        Me.cmdItems.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdItems, "cmdItems")
        Me.cmdItems.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdItems.Name = "cmdItems"
        Me.ToolTip1.SetToolTip(Me.cmdItems, resources.GetString("cmdItems.ToolTip"))
        Me.cmdItems.UseVisualStyleBackColor = False
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
        'cmdSubmit
        '
        Me.cmdSubmit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSubmit.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdSubmit, "cmdSubmit")
        Me.cmdSubmit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSubmit.Name = "cmdSubmit"
        Me.ToolTip1.SetToolTip(Me.cmdSubmit, resources.GetString("cmdSubmit.ToolTip"))
        Me.cmdSubmit.UseVisualStyleBackColor = False
        '
        'txtCurVend
        '
        Me.txtCurVend.AcceptsReturn = True
        Me.txtCurVend.BackColor = System.Drawing.SystemColors.Window
        Me.txtCurVend.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.txtCurVend, "txtCurVend")
        Me.txtCurVend.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCurVend.Name = "txtCurVend"
        '
        'lblVendor
        '
        Me.lblVendor.BackColor = System.Drawing.SystemColors.Control
        Me.lblVendor.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblVendor, "lblVendor")
        Me.lblVendor.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblVendor.Name = "lblVendor"
        '
        'ugrdVendList
        '
        Appearance1.BackColor = System.Drawing.SystemColors.ControlLight
        Appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.ugrdVendList.DisplayLayout.Appearance = Appearance1
        UltraGridColumn1.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn1.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn1.Header.Caption = resources.GetString("resource.Caption")
        UltraGridColumn1.Header.VisiblePosition = 0
        UltraGridColumn1.Hidden = True
        UltraGridColumn2.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn2.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn2.Header.Caption = resources.GetString("resource.Caption1")
        UltraGridColumn2.Header.VisiblePosition = 1
        UltraGridColumn2.Width = 430
        UltraGridColumn3.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn3.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn3.Header.Caption = resources.GetString("resource.Caption2")
        UltraGridColumn3.Header.VisiblePosition = 2
        UltraGridColumn3.Width = 307
        UltraGridBand1.Columns.AddRange(New Object() {UltraGridColumn1, UltraGridColumn2, UltraGridColumn3})
        UltraGridBand1.RowLayoutStyle = Infragistics.Win.UltraWinGrid.RowLayoutStyle.None
        Me.ugrdVendList.DisplayLayout.BandsSerializer.Add(UltraGridBand1)
        Me.ugrdVendList.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.ugrdVendList.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance2.BorderColor = System.Drawing.SystemColors.Window
        Me.ugrdVendList.DisplayLayout.GroupByBox.Appearance = Appearance2
        Appearance3.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugrdVendList.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance3
        Me.ugrdVendList.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.ugrdVendList.DisplayLayout.GroupByBox.Hidden = True
        Appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance4.BackColor2 = System.Drawing.SystemColors.Control
        Appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance4.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugrdVendList.DisplayLayout.GroupByBox.PromptAppearance = Appearance4
        Me.ugrdVendList.DisplayLayout.MaxColScrollRegions = 1
        Me.ugrdVendList.DisplayLayout.MaxRowScrollRegions = 1
        Appearance5.BackColor = System.Drawing.SystemColors.Window
        Appearance5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ugrdVendList.DisplayLayout.Override.ActiveCellAppearance = Appearance5
        Me.ugrdVendList.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.ugrdVendList.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance6.BackColor = System.Drawing.SystemColors.Window
        Me.ugrdVendList.DisplayLayout.Override.CardAreaAppearance = Appearance6
        Appearance7.BorderColor = System.Drawing.Color.Silver
        Appearance7.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.ugrdVendList.DisplayLayout.Override.CellAppearance = Appearance7
        Me.ugrdVendList.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.ugrdVendList.DisplayLayout.Override.CellPadding = 0
        Appearance8.BackColor = System.Drawing.SystemColors.Control
        Appearance8.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance8.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance8.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance8.BorderColor = System.Drawing.SystemColors.Window
        Me.ugrdVendList.DisplayLayout.Override.GroupByRowAppearance = Appearance8
        Appearance9.FontData.BoldAsString = resources.GetString("resource.BoldAsString")
        Appearance9.TextHAlign = Infragistics.Win.HAlign.Left
        Me.ugrdVendList.DisplayLayout.Override.HeaderAppearance = Appearance9
        Me.ugrdVendList.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti
        Me.ugrdVendList.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance10.BackColor = System.Drawing.SystemColors.ControlLight
        Me.ugrdVendList.DisplayLayout.Override.RowAlternateAppearance = Appearance10
        Appearance11.BackColor = System.Drawing.SystemColors.Window
        Appearance11.BorderColor = System.Drawing.Color.Silver
        Me.ugrdVendList.DisplayLayout.Override.RowAppearance = Appearance11
        Me.ugrdVendList.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[True]
        Me.ugrdVendList.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.ugrdVendList.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.ugrdVendList.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.[Single]
        Appearance12.BackColor = System.Drawing.SystemColors.ControlLight
        Me.ugrdVendList.DisplayLayout.Override.TemplateAddRowAppearance = Appearance12
        Me.ugrdVendList.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.ugrdVendList.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.ugrdVendList.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        resources.ApplyResources(Me.ugrdVendList, "ugrdVendList")
        Me.ugrdVendList.Name = "ugrdVendList"
        '
        'BottomToolStripPanel
        '
        resources.ApplyResources(Me.BottomToolStripPanel, "BottomToolStripPanel")
        Me.BottomToolStripPanel.Name = "BottomToolStripPanel"
        Me.BottomToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal
        Me.BottomToolStripPanel.RowMargin = New System.Windows.Forms.Padding(3, 0, 0, 0)
        '
        'TopToolStripPanel
        '
        resources.ApplyResources(Me.TopToolStripPanel, "TopToolStripPanel")
        Me.TopToolStripPanel.Name = "TopToolStripPanel"
        Me.TopToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal
        Me.TopToolStripPanel.RowMargin = New System.Windows.Forms.Padding(3, 0, 0, 0)
        '
        'RightToolStripPanel
        '
        resources.ApplyResources(Me.RightToolStripPanel, "RightToolStripPanel")
        Me.RightToolStripPanel.Name = "RightToolStripPanel"
        Me.RightToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal
        Me.RightToolStripPanel.RowMargin = New System.Windows.Forms.Padding(3, 0, 0, 0)
        '
        'LeftToolStripPanel
        '
        resources.ApplyResources(Me.LeftToolStripPanel, "LeftToolStripPanel")
        Me.LeftToolStripPanel.Name = "LeftToolStripPanel"
        Me.LeftToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal
        Me.LeftToolStripPanel.RowMargin = New System.Windows.Forms.Padding(3, 0, 0, 0)
        '
        'ContentPanel
        '
        resources.ApplyResources(Me.ContentPanel, "ContentPanel")
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsslMessage})
        resources.ApplyResources(Me.StatusStrip1, "StatusStrip1")
        Me.StatusStrip1.Name = "StatusStrip1"
        '
        'tsslMessage
        '
        resources.ApplyResources(Me.tsslMessage, "tsslMessage")
        Me.tsslMessage.Name = "tsslMessage"
        '
        'frmNewPrimVend
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.ugrdVendList)
        Me.Controls.Add(Me.cmdItems)
        Me.Controls.Add(Me.txtCurVend)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.cmdSubmit)
        Me.Controls.Add(Me.lblVendor)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Name = "frmNewPrimVend"
        Me.ShowInTaskbar = False
        CType(Me.ugrdVendList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ugrdVendList As Infragistics.Win.UltraWinGrid.UltraGrid
    Friend WithEvents BottomToolStripPanel As System.Windows.Forms.ToolStripPanel
    Friend WithEvents TopToolStripPanel As System.Windows.Forms.ToolStripPanel
    Friend WithEvents RightToolStripPanel As System.Windows.Forms.ToolStripPanel
    Friend WithEvents LeftToolStripPanel As System.Windows.Forms.ToolStripPanel
    Friend WithEvents ContentPanel As System.Windows.Forms.ToolStripContentPanel
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents tsslMessage As System.Windows.Forms.ToolStripStatusLabel
#End Region
End Class