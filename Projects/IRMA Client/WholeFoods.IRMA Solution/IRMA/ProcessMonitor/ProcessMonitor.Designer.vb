<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ProcessMonitor
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
        Dim Appearance37 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim UltraGridBand1 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1)
        Dim UltraGridColumn1 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("job name", -1, Nothing, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, False)
        Dim Appearance1 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim UltraGridColumn2 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("status")
        Dim Appearance2 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim UltraGridColumn3 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("lastrun")
        Dim Appearance3 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance4 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim UltraGridColumn4 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("servername")
        Dim Appearance5 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim UltraGridColumn5 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("statusdescription")
        Dim Appearance6 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim UltraGridColumn6 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("details")
        Dim Appearance7 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance44 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance45 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance46 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance47 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance48 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance49 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance50 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance51 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance52 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance53 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance54 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim UltraDataColumn1 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("job name")
        Dim UltraDataColumn2 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("status")
        Dim UltraDataColumn3 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("lastrun")
        Dim UltraDataColumn4 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("servername")
        Dim UltraDataColumn5 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("statusdescription")
        Dim UltraDataColumn6 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("details")
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ProcessMonitor))
        Me.UltraGrid_JobStatusList = New Infragistics.Win.UltraWinGrid.UltraGrid
        Me.UltraDataSource1 = New Infragistics.Win.UltraWinDataSource.UltraDataSource(Me.components)
        Me.Button_Refresh = New System.Windows.Forms.Button
        Me.Button1 = New System.Windows.Forms.Button
        Me.rdoOFF = New System.Windows.Forms.RadioButton
        Me.rdoON = New System.Windows.Forms.RadioButton
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        CType(Me.UltraGrid_JobStatusList, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.UltraDataSource1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'UltraGrid_JobStatusList
        '
        Me.UltraGrid_JobStatusList.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.UltraGrid_JobStatusList.DataSource = Me.UltraDataSource1
        Appearance37.BackColor = System.Drawing.SystemColors.Window
        Appearance37.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.UltraGrid_JobStatusList.DisplayLayout.Appearance = Appearance37
        Me.UltraGrid_JobStatusList.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        UltraGridColumn1.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn1.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn1.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        Appearance1.FontData.BoldAsString = "True"
        UltraGridColumn1.Header.Appearance = Appearance1
        UltraGridColumn1.Header.Caption = "Job Name"
        UltraGridColumn1.Header.VisiblePosition = 0
        UltraGridColumn1.Width = 134
        UltraGridColumn2.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn2.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn2.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        Appearance2.FontData.BoldAsString = "True"
        UltraGridColumn2.Header.Appearance = Appearance2
        UltraGridColumn2.Header.Caption = "Job Status"
        UltraGridColumn2.Header.VisiblePosition = 1
        UltraGridColumn2.Width = 92
        UltraGridColumn3.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn3.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        Appearance3.TextHAlignAsString = "Right"
        UltraGridColumn3.CellAppearance = Appearance3
        UltraGridColumn3.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn3.Format = "M/d/yyyy h:mm tt"
        Appearance4.FontData.BoldAsString = "True"
        Appearance4.TextHAlignAsString = "Center"
        UltraGridColumn3.Header.Appearance = Appearance4
        UltraGridColumn3.Header.Caption = "Last Run"
        UltraGridColumn3.Header.TextOrientation = New Infragistics.Win.TextOrientationInfo(0, Infragistics.Win.TextFlowDirection.Horizontal)
        UltraGridColumn3.Header.VisiblePosition = 2
        UltraGridColumn3.Width = 122
        UltraGridColumn4.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn4.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn4.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        Appearance5.FontData.BoldAsString = "True"
        UltraGridColumn4.Header.Appearance = Appearance5
        UltraGridColumn4.Header.Caption = "Server Name"
        UltraGridColumn4.Header.VisiblePosition = 3
        UltraGridColumn4.Width = 111
        UltraGridColumn5.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn5.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn5.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Appearance6.FontData.BoldAsString = "True"
        UltraGridColumn5.Header.Appearance = Appearance6
        UltraGridColumn5.Header.Caption = "Status Description"
        UltraGridColumn5.Header.VisiblePosition = 4
        UltraGridColumn5.Width = 135
        UltraGridColumn6.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn6.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn6.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Appearance7.FontData.BoldAsString = "True"
        UltraGridColumn6.Header.Appearance = Appearance7
        UltraGridColumn6.Header.Caption = "Job Status Details"
        UltraGridColumn6.Header.VisiblePosition = 5
        UltraGridColumn6.Width = 133
        UltraGridBand1.Columns.AddRange(New Object() {UltraGridColumn1, UltraGridColumn2, UltraGridColumn3, UltraGridColumn4, UltraGridColumn5, UltraGridColumn6})
        Me.UltraGrid_JobStatusList.DisplayLayout.BandsSerializer.Add(UltraGridBand1)
        Me.UltraGrid_JobStatusList.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.UltraGrid_JobStatusList.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Appearance44.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance44.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance44.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance44.BorderColor = System.Drawing.SystemColors.Window
        Me.UltraGrid_JobStatusList.DisplayLayout.GroupByBox.Appearance = Appearance44
        Appearance45.ForeColor = System.Drawing.SystemColors.GrayText
        Me.UltraGrid_JobStatusList.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance45
        Me.UltraGrid_JobStatusList.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Appearance46.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance46.BackColor2 = System.Drawing.SystemColors.Control
        Appearance46.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance46.ForeColor = System.Drawing.SystemColors.GrayText
        Me.UltraGrid_JobStatusList.DisplayLayout.GroupByBox.PromptAppearance = Appearance46
        Me.UltraGrid_JobStatusList.DisplayLayout.MaxColScrollRegions = 1
        Me.UltraGrid_JobStatusList.DisplayLayout.MaxRowScrollRegions = 1
        Appearance47.BackColor = System.Drawing.SystemColors.Window
        Appearance47.ForeColor = System.Drawing.SystemColors.ControlText
        Me.UltraGrid_JobStatusList.DisplayLayout.Override.ActiveCellAppearance = Appearance47
        Appearance48.BackColor = System.Drawing.SystemColors.Highlight
        Appearance48.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.UltraGrid_JobStatusList.DisplayLayout.Override.ActiveRowAppearance = Appearance48
        Me.UltraGrid_JobStatusList.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.UltraGrid_JobStatusList.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance49.BackColor = System.Drawing.SystemColors.Window
        Me.UltraGrid_JobStatusList.DisplayLayout.Override.CardAreaAppearance = Appearance49
        Appearance50.BorderColor = System.Drawing.Color.Silver
        Appearance50.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.UltraGrid_JobStatusList.DisplayLayout.Override.CellAppearance = Appearance50
        Me.UltraGrid_JobStatusList.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.UltraGrid_JobStatusList.DisplayLayout.Override.CellPadding = 0
        Appearance51.BackColor = System.Drawing.SystemColors.Control
        Appearance51.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance51.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance51.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance51.BorderColor = System.Drawing.SystemColors.Window
        Me.UltraGrid_JobStatusList.DisplayLayout.Override.GroupByRowAppearance = Appearance51
        Appearance52.TextHAlignAsString = "Left"
        Me.UltraGrid_JobStatusList.DisplayLayout.Override.HeaderAppearance = Appearance52
        Me.UltraGrid_JobStatusList.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti
        Me.UltraGrid_JobStatusList.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance53.BackColor = System.Drawing.SystemColors.Window
        Appearance53.BorderColor = System.Drawing.Color.Silver
        Me.UltraGrid_JobStatusList.DisplayLayout.Override.RowAppearance = Appearance53
        Me.UltraGrid_JobStatusList.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[False]
        Appearance54.BackColor = System.Drawing.SystemColors.ControlLight
        Me.UltraGrid_JobStatusList.DisplayLayout.Override.TemplateAddRowAppearance = Appearance54
        Me.UltraGrid_JobStatusList.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.UltraGrid_JobStatusList.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.UltraGrid_JobStatusList.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand
        Me.UltraGrid_JobStatusList.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.UltraGrid_JobStatusList.Location = New System.Drawing.Point(12, 60)
        Me.UltraGrid_JobStatusList.Name = "UltraGrid_JobStatusList"
        Me.UltraGrid_JobStatusList.Size = New System.Drawing.Size(729, 341)
        Me.UltraGrid_JobStatusList.TabIndex = 1
        Me.UltraGrid_JobStatusList.Text = "Process Monitor"
        '
        'UltraDataSource1
        '
        Me.UltraDataSource1.AllowAdd = False
        Me.UltraDataSource1.AllowDelete = False
        Me.UltraDataSource1.Band.AllowAdd = Infragistics.Win.DefaultableBoolean.[False]
        Me.UltraDataSource1.Band.AllowDelete = Infragistics.Win.DefaultableBoolean.[False]
        UltraDataColumn3.DataType = GetType(Date)
        Me.UltraDataSource1.Band.Columns.AddRange(New Object() {UltraDataColumn1, UltraDataColumn2, UltraDataColumn3, UltraDataColumn4, UltraDataColumn5, UltraDataColumn6})
        '
        'Button_Refresh
        '
        Me.Button_Refresh.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.Button_Refresh.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Refresh.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.Button_Refresh.Image = CType(resources.GetObject("Button_Refresh.Image"), System.Drawing.Image)
        Me.Button_Refresh.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Button_Refresh.Location = New System.Drawing.Point(581, 22)
        Me.Button_Refresh.Name = "Button_Refresh"
        Me.Button_Refresh.Size = New System.Drawing.Size(85, 32)
        Me.Button_Refresh.TabIndex = 3
        Me.Button_Refresh.Text = "&Refresh"
        Me.Button_Refresh.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Button_Refresh.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Button_Refresh.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.Button1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.Button1.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Button1.Image = CType(resources.GetObject("Button1.Image"), System.Drawing.Image)
        Me.Button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Button1.Location = New System.Drawing.Point(672, 22)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(69, 32)
        Me.Button1.TabIndex = 4
        Me.Button1.Text = "&Close"
        Me.Button1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Button1.UseVisualStyleBackColor = True
        '
        'rdoOFF
        '
        Me.rdoOFF.AutoSize = True
        Me.rdoOFF.Checked = True
        Me.rdoOFF.Location = New System.Drawing.Point(66, 18)
        Me.rdoOFF.Name = "rdoOFF"
        Me.rdoOFF.Size = New System.Drawing.Size(49, 17)
        Me.rdoOFF.TabIndex = 5
        Me.rdoOFF.TabStop = True
        Me.rdoOFF.Text = "Hide"
        Me.rdoOFF.UseVisualStyleBackColor = True
        '
        'rdoON
        '
        Me.rdoON.AutoSize = True
        Me.rdoON.Location = New System.Drawing.Point(6, 18)
        Me.rdoON.Name = "rdoON"
        Me.rdoON.Size = New System.Drawing.Size(54, 17)
        Me.rdoON.TabIndex = 6
        Me.rdoON.Text = "Show"
        Me.rdoON.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.rdoOFF)
        Me.GroupBox1.Controls.Add(Me.rdoON)
        Me.GroupBox1.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(132, 42)
        Me.GroupBox1.TabIndex = 8
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Job Status Details"
        '
        'ProcessMonitor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(752, 413)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Button_Refresh)
        Me.Controls.Add(Me.UltraGrid_JobStatusList)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimizeBox = False
        Me.Name = "ProcessMonitor"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Process Monitor"
        CType(Me.UltraGrid_JobStatusList, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.UltraDataSource1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents UltraGrid_JobStatusList As Infragistics.Win.UltraWinGrid.UltraGrid
    Friend WithEvents UltraDataSource1 As Infragistics.Win.UltraWinDataSource.UltraDataSource
    Friend WithEvents Button_Refresh As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents rdoOFF As System.Windows.Forms.RadioButton
    Friend WithEvents rdoON As System.Windows.Forms.RadioButton
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
End Class
