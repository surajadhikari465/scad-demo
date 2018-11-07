<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ReasonCodeMaintenance
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ReasonCodeMaintenance))
        Dim Appearance1 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridBand1 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1)
        Dim UltraGridColumn1 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("ReasonCode")
        Dim Appearance13 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance14 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridColumn2 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("ReasonCodeDesc", -1, Nothing, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, False)
        Dim Appearance15 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridColumn3 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("ReasonCodeExtDesc")
        Dim Appearance16 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridColumn4 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("ReasonCodeDetailID", 0)
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
        Me.cmbReasonCodeTypes = New System.Windows.Forms.ComboBox()
        Me.cmdExit = New System.Windows.Forms.Button()
        Me.AddReasonCodeType = New System.Windows.Forms.Button()
        Me.UltraGrid_ReasonCodeMaintenance = New Infragistics.Win.UltraWinGrid.UltraGrid()
        Me.AddReasonCodeDetail = New System.Windows.Forms.Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.EditMappings = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        CType(Me.UltraGrid_ReasonCodeMaintenance, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmbReasonCodeTypes
        '
        Me.cmbReasonCodeTypes.FormattingEnabled = True
        Me.cmbReasonCodeTypes.Location = New System.Drawing.Point(15, 33)
        Me.cmbReasonCodeTypes.Name = "cmbReasonCodeTypes"
        Me.cmbReasonCodeTypes.Size = New System.Drawing.Size(245, 21)
        Me.cmbReasonCodeTypes.TabIndex = 0
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
        Me.cmdExit.Location = New System.Drawing.Point(566, 368)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(41, 41)
        Me.cmdExit.TabIndex = 27
        Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdExit, "Exit")
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'AddReasonCodeType
        '
        Me.AddReasonCodeType.Location = New System.Drawing.Point(343, 31)
        Me.AddReasonCodeType.Name = "AddReasonCodeType"
        Me.AddReasonCodeType.Size = New System.Drawing.Size(91, 23)
        Me.AddReasonCodeType.TabIndex = 51
        Me.AddReasonCodeType.Text = "Add Type"
        Me.ToolTip1.SetToolTip(Me.AddReasonCodeType, "Add New Reason Code Type")
        Me.AddReasonCodeType.UseVisualStyleBackColor = True
        '
        'UltraGrid_ReasonCodeMaintenance
        '
        Appearance1.BackColor = System.Drawing.SystemColors.Window
        Appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.UltraGrid_ReasonCodeMaintenance.DisplayLayout.Appearance = Appearance1
        UltraGridColumn1.CellActivation = Infragistics.Win.UltraWinGrid.Activation.Disabled
        Appearance13.FontData.BoldAsString = "True"
        UltraGridColumn1.CellAppearance = Appearance13
        Appearance14.FontData.BoldAsString = "True"
        UltraGridColumn1.Header.Appearance = Appearance14
        UltraGridColumn1.Header.Caption = "Abbreviation"
        UltraGridColumn1.Header.VisiblePosition = 0
        Appearance15.FontData.BoldAsString = "True"
        UltraGridColumn2.Header.Appearance = Appearance15
        UltraGridColumn2.Header.Caption = "Description"
        UltraGridColumn2.Header.VisiblePosition = 1
        UltraGridColumn2.Width = 237
        Appearance16.FontData.BoldAsString = "True"
        UltraGridColumn3.Header.Appearance = Appearance16
        UltraGridColumn3.Header.Caption = "Extended Description"
        UltraGridColumn3.Header.VisiblePosition = 2
        UltraGridColumn3.Width = 298
        UltraGridColumn4.Header.VisiblePosition = 3
        UltraGridColumn4.Hidden = True
        UltraGridBand1.Columns.AddRange(New Object() {UltraGridColumn1, UltraGridColumn2, UltraGridColumn3, UltraGridColumn4})
        Me.UltraGrid_ReasonCodeMaintenance.DisplayLayout.BandsSerializer.Add(UltraGridBand1)
        Me.UltraGrid_ReasonCodeMaintenance.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.UltraGrid_ReasonCodeMaintenance.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance2.BorderColor = System.Drawing.SystemColors.Window
        Me.UltraGrid_ReasonCodeMaintenance.DisplayLayout.GroupByBox.Appearance = Appearance2
        Appearance3.ForeColor = System.Drawing.SystemColors.GrayText
        Me.UltraGrid_ReasonCodeMaintenance.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance3
        Me.UltraGrid_ReasonCodeMaintenance.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.UltraGrid_ReasonCodeMaintenance.DisplayLayout.GroupByBox.Hidden = True
        Appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance4.BackColor2 = System.Drawing.SystemColors.Control
        Appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance4.ForeColor = System.Drawing.SystemColors.GrayText
        Me.UltraGrid_ReasonCodeMaintenance.DisplayLayout.GroupByBox.PromptAppearance = Appearance4
        Me.UltraGrid_ReasonCodeMaintenance.DisplayLayout.MaxColScrollRegions = 1
        Me.UltraGrid_ReasonCodeMaintenance.DisplayLayout.MaxRowScrollRegions = 1
        Appearance5.BackColor = System.Drawing.SystemColors.Window
        Appearance5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.UltraGrid_ReasonCodeMaintenance.DisplayLayout.Override.ActiveCellAppearance = Appearance5
        Appearance6.BackColor = System.Drawing.SystemColors.Highlight
        Appearance6.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.UltraGrid_ReasonCodeMaintenance.DisplayLayout.Override.ActiveRowAppearance = Appearance6
        Me.UltraGrid_ReasonCodeMaintenance.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.UltraGrid_ReasonCodeMaintenance.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance7.BackColor = System.Drawing.SystemColors.Window
        Me.UltraGrid_ReasonCodeMaintenance.DisplayLayout.Override.CardAreaAppearance = Appearance7
        Appearance8.BorderColor = System.Drawing.Color.Silver
        Appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.UltraGrid_ReasonCodeMaintenance.DisplayLayout.Override.CellAppearance = Appearance8
        Me.UltraGrid_ReasonCodeMaintenance.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.UltraGrid_ReasonCodeMaintenance.DisplayLayout.Override.CellPadding = 0
        Me.UltraGrid_ReasonCodeMaintenance.DisplayLayout.Override.GroupByColumnsHidden = Infragistics.Win.DefaultableBoolean.[True]
        Appearance9.BackColor = System.Drawing.SystemColors.Control
        Appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance9.BorderColor = System.Drawing.SystemColors.Window
        Me.UltraGrid_ReasonCodeMaintenance.DisplayLayout.Override.GroupByRowAppearance = Appearance9
        Appearance10.TextHAlignAsString = "Left"
        Me.UltraGrid_ReasonCodeMaintenance.DisplayLayout.Override.HeaderAppearance = Appearance10
        Me.UltraGrid_ReasonCodeMaintenance.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti
        Me.UltraGrid_ReasonCodeMaintenance.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance11.BackColor = System.Drawing.SystemColors.Window
        Appearance11.BorderColor = System.Drawing.Color.Silver
        Me.UltraGrid_ReasonCodeMaintenance.DisplayLayout.Override.RowAppearance = Appearance11
        Me.UltraGrid_ReasonCodeMaintenance.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[False]
        Appearance12.BackColor = System.Drawing.SystemColors.ControlLight
        Me.UltraGrid_ReasonCodeMaintenance.DisplayLayout.Override.TemplateAddRowAppearance = Appearance12
        Me.UltraGrid_ReasonCodeMaintenance.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.UltraGrid_ReasonCodeMaintenance.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.UltraGrid_ReasonCodeMaintenance.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        Me.UltraGrid_ReasonCodeMaintenance.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.UltraGrid_ReasonCodeMaintenance.Location = New System.Drawing.Point(13, 87)
        Me.UltraGrid_ReasonCodeMaintenance.Name = "UltraGrid_ReasonCodeMaintenance"
        Me.UltraGrid_ReasonCodeMaintenance.Size = New System.Drawing.Size(599, 274)
        Me.UltraGrid_ReasonCodeMaintenance.TabIndex = 52
        '
        'AddReasonCodeDetail
        '
        Me.AddReasonCodeDetail.Location = New System.Drawing.Point(440, 31)
        Me.AddReasonCodeDetail.Name = "AddReasonCodeDetail"
        Me.AddReasonCodeDetail.Size = New System.Drawing.Size(75, 23)
        Me.AddReasonCodeDetail.TabIndex = 53
        Me.AddReasonCodeDetail.Text = "Add Detail"
        Me.ToolTip1.SetToolTip(Me.AddReasonCodeDetail, "Add New Reason Code Detail")
        Me.AddReasonCodeDetail.UseVisualStyleBackColor = True
        '
        'EditMappings
        '
        Me.EditMappings.Location = New System.Drawing.Point(521, 31)
        Me.EditMappings.Name = "EditMappings"
        Me.EditMappings.Size = New System.Drawing.Size(90, 23)
        Me.EditMappings.TabIndex = 54
        Me.EditMappings.Text = "Edit Mappings"
        Me.EditMappings.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(12, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(72, 13)
        Me.Label1.TabIndex = 55
        Me.Label1.Text = "Code Type:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(15, 68)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(110, 13)
        Me.Label2.TabIndex = 56
        Me.Label2.Text = "Current Mappings:"
        '
        'ReasonCodeMaintenance
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(646, 414)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.EditMappings)
        Me.Controls.Add(Me.AddReasonCodeDetail)
        Me.Controls.Add(Me.UltraGrid_ReasonCodeMaintenance)
        Me.Controls.Add(Me.AddReasonCodeType)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.cmbReasonCodeTypes)
        Me.Name = "ReasonCodeMaintenance"
        Me.Text = "Reason Code Maintenance"
        CType(Me.UltraGrid_ReasonCodeMaintenance, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cmbReasonCodeTypes As System.Windows.Forms.ComboBox
    Public WithEvents cmdExit As System.Windows.Forms.Button
    Friend WithEvents AddReasonCodeType As System.Windows.Forms.Button
    Friend WithEvents UltraGrid_ReasonCodeMaintenance As Infragistics.Win.UltraWinGrid.UltraGrid
    Friend WithEvents AddReasonCodeDetail As System.Windows.Forms.Button
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents EditMappings As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
End Class
