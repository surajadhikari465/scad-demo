<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DeletedStoresList
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
        Dim Appearance1 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridBand1 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1)
        Dim UltraGridColumn1 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Store_No")
        Dim UltraGridColumn2 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Store_Name", -1, Nothing, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, False)
        Dim Appearance2 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance3 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance4 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance5 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance6 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance7 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance8 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance9 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance10 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance11 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance12 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance13 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance14 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Me.ugrdSelectList = New Infragistics.Win.UltraWinGrid.UltraGrid()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.lblMsgTop = New System.Windows.Forms.Label()
        Me.lblMsgBottom = New System.Windows.Forms.Label()
        CType(Me.ugrdSelectList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ugrdSelectList
        '
        Appearance1.BackColor = System.Drawing.SystemColors.ControlLight
        Appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.ugrdSelectList.DisplayLayout.Appearance = Appearance1
        Me.ugrdSelectList.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        UltraGridColumn1.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn1.Header.VisiblePosition = 1
        UltraGridColumn1.Hidden = True
        UltraGridColumn2.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn2.Header.Caption = "Stores"
        UltraGridColumn2.Header.VisiblePosition = 0
        UltraGridColumn2.Width = 426
        UltraGridBand1.Columns.AddRange(New Object() {UltraGridColumn1, UltraGridColumn2})
        Me.ugrdSelectList.DisplayLayout.BandsSerializer.Add(UltraGridBand1)
        Me.ugrdSelectList.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Appearance2.FontData.BoldAsString = "True"
        Me.ugrdSelectList.DisplayLayout.CaptionAppearance = Appearance2
        Me.ugrdSelectList.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Appearance3.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance3.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance3.BorderColor = System.Drawing.SystemColors.Window
        Me.ugrdSelectList.DisplayLayout.GroupByBox.Appearance = Appearance3
        Appearance4.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugrdSelectList.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance4
        Me.ugrdSelectList.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.ugrdSelectList.DisplayLayout.GroupByBox.Hidden = True
        Appearance5.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance5.BackColor2 = System.Drawing.SystemColors.Control
        Appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance5.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugrdSelectList.DisplayLayout.GroupByBox.PromptAppearance = Appearance5
        Me.ugrdSelectList.DisplayLayout.MaxColScrollRegions = 1
        Me.ugrdSelectList.DisplayLayout.MaxRowScrollRegions = 1
        Appearance6.BackColor = System.Drawing.SystemColors.Window
        Appearance6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ugrdSelectList.DisplayLayout.Override.ActiveCellAppearance = Appearance6
        Me.ugrdSelectList.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.ugrdSelectList.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance7.BackColor = System.Drawing.SystemColors.Window
        Me.ugrdSelectList.DisplayLayout.Override.CardAreaAppearance = Appearance7
        Appearance8.BorderColor = System.Drawing.Color.Silver
        Appearance8.FontData.BoldAsString = "True"
        Appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.ugrdSelectList.DisplayLayout.Override.CellAppearance = Appearance8
        Me.ugrdSelectList.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.ugrdSelectList.DisplayLayout.Override.CellPadding = 0
        Appearance9.FontData.BoldAsString = "True"
        Me.ugrdSelectList.DisplayLayout.Override.FixedHeaderAppearance = Appearance9
        Appearance10.BackColor = System.Drawing.SystemColors.Control
        Appearance10.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance10.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance10.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance10.BorderColor = System.Drawing.SystemColors.Window
        Me.ugrdSelectList.DisplayLayout.Override.GroupByRowAppearance = Appearance10
        Appearance11.FontData.BoldAsString = "True"
        Appearance11.TextHAlignAsString = "Left"
        Me.ugrdSelectList.DisplayLayout.Override.HeaderAppearance = Appearance11
        Me.ugrdSelectList.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle
        Me.ugrdSelectList.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance12.BackColor = System.Drawing.SystemColors.ControlLight
        Me.ugrdSelectList.DisplayLayout.Override.RowAlternateAppearance = Appearance12
        Appearance13.BackColor = System.Drawing.SystemColors.Window
        Appearance13.BorderColor = System.Drawing.Color.Silver
        Me.ugrdSelectList.DisplayLayout.Override.RowAppearance = Appearance13
        Me.ugrdSelectList.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[True]
        Me.ugrdSelectList.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed
        Me.ugrdSelectList.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.ugrdSelectList.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.ugrdSelectList.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Extended
        Appearance14.BackColor = System.Drawing.SystemColors.ControlLight
        Me.ugrdSelectList.DisplayLayout.Override.TemplateAddRowAppearance = Appearance14
        Me.ugrdSelectList.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.ugrdSelectList.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.ugrdSelectList.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand
        Me.ugrdSelectList.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        Me.ugrdSelectList.Font = New System.Drawing.Font("Arial", 8.0!)
        Me.ugrdSelectList.Location = New System.Drawing.Point(12, 33)
        Me.ugrdSelectList.Name = "ugrdSelectList"
        Me.ugrdSelectList.Size = New System.Drawing.Size(447, 292)
        Me.ugrdSelectList.TabIndex = 31
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(197, 375)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(75, 23)
        Me.btnOK.TabIndex = 32
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'lblMsgTop
        '
        Me.lblMsgTop.AutoSize = True
        Me.lblMsgTop.Location = New System.Drawing.Point(12, 9)
        Me.lblMsgTop.Name = "lblMsgTop"
        Me.lblMsgTop.Size = New System.Drawing.Size(39, 13)
        Me.lblMsgTop.TabIndex = 33
        Me.lblMsgTop.Text = "Label1"
        '
        'lblMsgBottom
        '
        Me.lblMsgBottom.AutoSize = True
        Me.lblMsgBottom.Location = New System.Drawing.Point(12, 337)
        Me.lblMsgBottom.Name = "lblMsgBottom"
        Me.lblMsgBottom.Size = New System.Drawing.Size(39, 13)
        Me.lblMsgBottom.TabIndex = 34
        Me.lblMsgBottom.Text = "Label2"
        '
        'DeletedStoresList
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(481, 442)
        Me.Controls.Add(Me.lblMsgBottom)
        Me.Controls.Add(Me.lblMsgTop)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.ugrdSelectList)
        Me.Name = "DeletedStoresList"
        Me.Text = "DeletedStoresList"
        CType(Me.ugrdSelectList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ugrdSelectList As Infragistics.Win.UltraWinGrid.UltraGrid
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents lblMsgTop As System.Windows.Forms.Label
    Friend WithEvents lblMsgBottom As System.Windows.Forms.Label
End Class
