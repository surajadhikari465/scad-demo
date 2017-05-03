<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmOrdersAllocateOrderList
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
	Public WithEvents cmdExit As System.Windows.Forms.Button
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmOrdersAllocateOrderList))
        Dim Appearance1 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim UltraGridBand1 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1)
        Dim UltraGridColumn1 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("OrderHeader_ID")
        Dim Appearance2 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance3 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim UltraGridColumn2 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyName")
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
        Dim Appearance16 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance17 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdExit = New System.Windows.Forms.Button
        Me.cmdSelect = New System.Windows.Forms.Button
        Me.ugrdOrders = New Infragistics.Win.UltraWinGrid.UltraGrid
        CType(Me.ugrdOrders, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdExit
        '
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdExit.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Image = CType(resources.GetObject("cmdExit.Image"), System.Drawing.Image)
        Me.cmdExit.Location = New System.Drawing.Point(488, 280)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(41, 41)
        Me.cmdExit.TabIndex = 2
        Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdExit, "Exit")
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'cmdSelect
        '
        Me.cmdSelect.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSelect.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSelect.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSelect.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSelect.Image = CType(resources.GetObject("cmdSelect.Image"), System.Drawing.Image)
        Me.cmdSelect.Location = New System.Drawing.Point(441, 281)
        Me.cmdSelect.Name = "cmdSelect"
        Me.cmdSelect.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSelect.Size = New System.Drawing.Size(41, 41)
        Me.cmdSelect.TabIndex = 1
        Me.cmdSelect.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdSelect, "Select Order")
        Me.cmdSelect.UseVisualStyleBackColor = False
        '
        'ugrdOrders
        '
        Appearance1.BackColor = System.Drawing.SystemColors.ControlLight
        Appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.ugrdOrders.DisplayLayout.Appearance = Appearance1
        Me.ugrdOrders.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        UltraGridColumn1.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        Appearance2.TextHAlign = Infragistics.Win.HAlign.Right
        UltraGridColumn1.CellAppearance = Appearance2
        UltraGridColumn1.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        Appearance3.TextHAlign = Infragistics.Win.HAlign.Right
        UltraGridColumn1.Header.Appearance = Appearance3
        UltraGridColumn1.Header.Caption = "Order"
        UltraGridColumn1.Header.VisiblePosition = 0
        UltraGridColumn2.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn2.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn2.Header.Caption = "Receiving Location"
        UltraGridColumn2.Header.VisiblePosition = 1
        UltraGridColumn2.MinWidth = 185
        UltraGridBand1.Columns.AddRange(New Object() {UltraGridColumn1, UltraGridColumn2})
        UltraGridBand1.RowLayoutStyle = Infragistics.Win.UltraWinGrid.RowLayoutStyle.None
        Me.ugrdOrders.DisplayLayout.BandsSerializer.Add(UltraGridBand1)
        Me.ugrdOrders.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Appearance4.FontData.BoldAsString = "True"
        Me.ugrdOrders.DisplayLayout.CaptionAppearance = Appearance4
        Me.ugrdOrders.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Appearance5.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance5.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance5.BorderColor = System.Drawing.SystemColors.Window
        Me.ugrdOrders.DisplayLayout.GroupByBox.Appearance = Appearance5
        Appearance6.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugrdOrders.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance6
        Me.ugrdOrders.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.ugrdOrders.DisplayLayout.GroupByBox.Hidden = True
        Appearance7.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance7.BackColor2 = System.Drawing.SystemColors.Control
        Appearance7.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance7.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugrdOrders.DisplayLayout.GroupByBox.PromptAppearance = Appearance7
        Me.ugrdOrders.DisplayLayout.MaxColScrollRegions = 1
        Me.ugrdOrders.DisplayLayout.MaxRowScrollRegions = 1
        Appearance8.BackColor = System.Drawing.SystemColors.Window
        Appearance8.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ugrdOrders.DisplayLayout.Override.ActiveCellAppearance = Appearance8
        Appearance9.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugrdOrders.DisplayLayout.Override.ActiveRowAppearance = Appearance9
        Me.ugrdOrders.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.ugrdOrders.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance10.BackColor = System.Drawing.SystemColors.Window
        Me.ugrdOrders.DisplayLayout.Override.CardAreaAppearance = Appearance10
        Appearance11.BorderColor = System.Drawing.Color.Silver
        Appearance11.FontData.BoldAsString = "True"
        Appearance11.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.ugrdOrders.DisplayLayout.Override.CellAppearance = Appearance11
        Me.ugrdOrders.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.ugrdOrders.DisplayLayout.Override.CellPadding = 0
        Appearance12.FontData.BoldAsString = "True"
        Me.ugrdOrders.DisplayLayout.Override.FixedHeaderAppearance = Appearance12
        Appearance13.BackColor = System.Drawing.SystemColors.Control
        Appearance13.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance13.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance13.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance13.BorderColor = System.Drawing.SystemColors.Window
        Me.ugrdOrders.DisplayLayout.Override.GroupByRowAppearance = Appearance13
        Appearance14.FontData.BoldAsString = "True"
        Appearance14.TextHAlign = Infragistics.Win.HAlign.Left
        Me.ugrdOrders.DisplayLayout.Override.HeaderAppearance = Appearance14
        Me.ugrdOrders.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle
        Me.ugrdOrders.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance15.BackColor = System.Drawing.SystemColors.Control
        Me.ugrdOrders.DisplayLayout.Override.RowAlternateAppearance = Appearance15
        Appearance16.BackColor = System.Drawing.SystemColors.Window
        Appearance16.BorderColor = System.Drawing.Color.Silver
        Me.ugrdOrders.DisplayLayout.Override.RowAppearance = Appearance16
        Me.ugrdOrders.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[True]
        Me.ugrdOrders.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed
        Me.ugrdOrders.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.ugrdOrders.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.ugrdOrders.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.[Single]
        Appearance17.BackColor = System.Drawing.SystemColors.ControlLight
        Me.ugrdOrders.DisplayLayout.Override.TemplateAddRowAppearance = Appearance17
        Me.ugrdOrders.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.ugrdOrders.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.ugrdOrders.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand
        Me.ugrdOrders.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        Me.ugrdOrders.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ugrdOrders.Location = New System.Drawing.Point(16, 12)
        Me.ugrdOrders.Name = "ugrdOrders"
        Me.ugrdOrders.Size = New System.Drawing.Size(513, 263)
        Me.ugrdOrders.TabIndex = 28
        '
        'frmOrdersAllocateOrderList
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdExit
        Me.ClientSize = New System.Drawing.Size(543, 334)
        Me.Controls.Add(Me.ugrdOrders)
        Me.Controls.Add(Me.cmdSelect)
        Me.Controls.Add(Me.cmdExit)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Location = New System.Drawing.Point(3, 29)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmOrdersAllocateOrderList"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "FSA - Orders List"
        CType(Me.ugrdOrders, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Public WithEvents cmdSelect As System.Windows.Forms.Button
    Friend WithEvents ugrdOrders As Infragistics.Win.UltraWinGrid.UltraGrid
#End Region 
End Class