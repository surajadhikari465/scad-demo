<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ImportDataHistoryDetails
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
        Dim UltraGridBand1 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1)
        Dim UltraGridColumn1 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Identifier")
        Dim UltraGridColumn2 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Sub Team")
        Dim UltraGridColumn3 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("POS Description")
        Dim UltraGridColumn4 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Description")
        Dim UltraGridColumn5 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Tax Class", -1, 1279235)
        Dim UltraGridColumn6 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Food Stamps")
        Dim UltraGridColumn7 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Restricted Hours")
        Dim UltraGridColumn8 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Employee Discountable")
        Dim UltraGridColumn9 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Discontinued")
        Dim UltraGridColumn10 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("National Class", -1, 21052594)
        Dim UltraGridColumn11 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("ItemUploadDetail_ID")
        Dim UltraGridColumn12 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Sub Team No")
        Dim UltraGridColumn13 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Item Identifier Valid")
        Dim UltraGridColumn14 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Sub Team Allowed")
        Dim UltraGridColumn15 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Item_Key")
        Dim Appearance1 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance2 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance3 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance4 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance5 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance6 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance7 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance8 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance9 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim ValueList1 As Infragistics.Win.ValueList = New Infragistics.Win.ValueList(1279235)
        Dim ValueList2 As Infragistics.Win.ValueList = New Infragistics.Win.ValueList(21052594)
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ImportDataHistoryDetails))
        Me.ugrdList = New Infragistics.Win.UltraWinGrid.UltraGrid
        Me.btnExit = New System.Windows.Forms.Button
        Me.HeaderInfoLabel = New System.Windows.Forms.Label
        Me.cmdItemEdit = New System.Windows.Forms.Button
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        CType(Me.ugrdList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ugrdList
        '
        Me.ugrdList.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        UltraGridColumn1.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn1.Header.VisiblePosition = 0
        UltraGridColumn1.Width = 131
        UltraGridColumn2.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn2.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn2.Header.VisiblePosition = 1
        UltraGridColumn2.MinWidth = 30
        UltraGridColumn2.Width = 117
        UltraGridColumn3.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn3.Header.VisiblePosition = 2
        UltraGridColumn3.MaxLength = 26
        UltraGridColumn3.Width = 124
        UltraGridColumn4.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn4.Header.VisiblePosition = 3
        UltraGridColumn4.MaxLength = 60
        UltraGridColumn4.Width = 117
        UltraGridColumn5.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn5.Header.VisiblePosition = 4
        UltraGridColumn5.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList
        UltraGridColumn5.Width = 59
        UltraGridColumn6.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn6.Header.VisiblePosition = 5
        UltraGridColumn6.Width = 70
        UltraGridColumn7.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn7.CellMultiLine = Infragistics.Win.DefaultableBoolean.[True]
        UltraGridColumn7.Header.VisiblePosition = 6
        UltraGridColumn7.Width = 81
        UltraGridColumn8.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn8.CellMultiLine = Infragistics.Win.DefaultableBoolean.[True]
        UltraGridColumn8.Header.VisiblePosition = 7
        UltraGridColumn8.Width = 87
        UltraGridColumn9.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn9.Header.VisiblePosition = 8
        UltraGridColumn9.Width = 87
        UltraGridColumn10.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn10.Header.VisiblePosition = 9
        UltraGridColumn10.MaxLength = 90
        UltraGridColumn10.MinWidth = 40
        UltraGridColumn10.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList
        UltraGridColumn10.Width = 85
        UltraGridColumn11.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
        UltraGridColumn11.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn11.Header.VisiblePosition = 10
        UltraGridColumn11.Hidden = True
        UltraGridColumn12.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn12.Header.VisiblePosition = 11
        UltraGridColumn12.Hidden = True
        UltraGridColumn13.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn13.Header.Caption = "Valid Identifier"
        UltraGridColumn13.Header.VisiblePosition = 12
        UltraGridColumn13.Hidden = True
        UltraGridColumn13.MaxWidth = 65
        UltraGridColumn13.MinWidth = 10
        UltraGridColumn14.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn14.Header.Caption = "SubTeam Allowed"
        UltraGridColumn14.Header.VisiblePosition = 13
        UltraGridColumn14.Hidden = True
        UltraGridColumn14.MaxWidth = 50
        UltraGridColumn15.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn15.Header.VisiblePosition = 14
        UltraGridColumn15.Hidden = True
        UltraGridColumn15.MaxWidth = 50
        UltraGridBand1.Columns.AddRange(New Object() {UltraGridColumn1, UltraGridColumn2, UltraGridColumn3, UltraGridColumn4, UltraGridColumn5, UltraGridColumn6, UltraGridColumn7, UltraGridColumn8, UltraGridColumn9, UltraGridColumn10, UltraGridColumn11, UltraGridColumn12, UltraGridColumn13, UltraGridColumn14, UltraGridColumn15})
        Me.ugrdList.DisplayLayout.BandsSerializer.Add(UltraGridBand1)
        Me.ugrdList.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.ugrdList.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Appearance1.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance1.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance1.BorderColor = System.Drawing.SystemColors.Window
        Me.ugrdList.DisplayLayout.GroupByBox.Appearance = Appearance1
        Appearance2.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugrdList.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance2
        Me.ugrdList.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.ugrdList.DisplayLayout.GroupByBox.Hidden = True
        Appearance3.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance3.BackColor2 = System.Drawing.SystemColors.Control
        Appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance3.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugrdList.DisplayLayout.GroupByBox.PromptAppearance = Appearance3
        Me.ugrdList.DisplayLayout.MaxColScrollRegions = 1
        Me.ugrdList.DisplayLayout.MaxRowScrollRegions = 1
        Me.ugrdList.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.ugrdList.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance4.BackColor = System.Drawing.SystemColors.Window
        Me.ugrdList.DisplayLayout.Override.CardAreaAppearance = Appearance4
        Appearance5.BorderColor = System.Drawing.Color.Silver
        Appearance5.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.ugrdList.DisplayLayout.Override.CellAppearance = Appearance5
        Me.ugrdList.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.ugrdList.DisplayLayout.Override.CellPadding = 0
        Appearance6.BackColor = System.Drawing.SystemColors.Control
        Appearance6.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance6.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance6.BorderColor = System.Drawing.SystemColors.Window
        Me.ugrdList.DisplayLayout.Override.GroupByRowAppearance = Appearance6
        Appearance7.TextHAlign = Infragistics.Win.HAlign.Center
        Me.ugrdList.DisplayLayout.Override.HeaderAppearance = Appearance7
        Me.ugrdList.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti
        Me.ugrdList.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance8.BackColor = System.Drawing.SystemColors.ControlLight
        Me.ugrdList.DisplayLayout.Override.RowAlternateAppearance = Appearance8
        Me.ugrdList.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[False]
        Appearance9.BackColor = System.Drawing.SystemColors.ControlLight
        Me.ugrdList.DisplayLayout.Override.TemplateAddRowAppearance = Appearance9
        Me.ugrdList.DisplayLayout.Override.WrapHeaderText = Infragistics.Win.DefaultableBoolean.[True]
        Me.ugrdList.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.ugrdList.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        ValueList1.Key = "TaxClass"
        ValueList2.Key = "NationalClass"
        Me.ugrdList.DisplayLayout.ValueLists.AddRange(New Infragistics.Win.ValueList() {ValueList1, ValueList2})
        Me.ugrdList.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        Me.ugrdList.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ugrdList.Location = New System.Drawing.Point(5, 40)
        Me.ugrdList.Name = "ugrdList"
        Me.ugrdList.Size = New System.Drawing.Size(960, 497)
        Me.ugrdList.TabIndex = 2
        Me.ugrdList.Text = "UltraGrid1"
        '
        'btnExit
        '
        Me.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnExit.Location = New System.Drawing.Point(890, 561)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(75, 23)
        Me.btnExit.TabIndex = 0
        Me.btnExit.Text = "Exit"
        Me.ToolTip1.SetToolTip(Me.btnExit, "Exit")
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'HeaderInfoLabel
        '
        Me.HeaderInfoLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HeaderInfoLabel.Location = New System.Drawing.Point(12, 9)
        Me.HeaderInfoLabel.Name = "HeaderInfoLabel"
        Me.HeaderInfoLabel.Size = New System.Drawing.Size(800, 25)
        Me.HeaderInfoLabel.TabIndex = 10
        '
        'cmdItemEdit
        '
        Me.cmdItemEdit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdItemEdit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdItemEdit.Font = New System.Drawing.Font("Arial", 8.0!)
        Me.cmdItemEdit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdItemEdit.Image = CType(resources.GetObject("cmdItemEdit.Image"), System.Drawing.Image)
        Me.cmdItemEdit.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.cmdItemEdit.Location = New System.Drawing.Point(5, 543)
        Me.cmdItemEdit.Name = "cmdItemEdit"
        Me.cmdItemEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdItemEdit.Size = New System.Drawing.Size(41, 41)
        Me.cmdItemEdit.TabIndex = 1
        Me.cmdItemEdit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdItemEdit, "View Item")
        Me.cmdItemEdit.UseVisualStyleBackColor = False
        '
        'ImportDataHistoryDetails
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnExit
        Me.ClientSize = New System.Drawing.Size(967, 593)
        Me.Controls.Add(Me.cmdItemEdit)
        Me.Controls.Add(Me.HeaderInfoLabel)
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.ugrdList)
        Me.Name = "ImportDataHistoryDetails"
        Me.ShowInTaskbar = False
        Me.Text = "Item Maintenance Load Audit / History Details"
        CType(Me.ugrdList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ugrdList As Infragistics.Win.UltraWinGrid.UltraGrid
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents HeaderInfoLabel As System.Windows.Forms.Label
    Public WithEvents cmdItemEdit As System.Windows.Forms.Button
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
End Class
