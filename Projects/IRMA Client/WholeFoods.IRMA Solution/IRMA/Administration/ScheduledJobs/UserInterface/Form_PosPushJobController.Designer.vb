<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_POSPushJobController
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
        Dim Appearance37 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridBand1 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1)
        Dim UltraGridColumn1 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("job name")
        Dim Appearance8 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridColumn2 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("status", -1, Nothing, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, False)
        Dim Appearance9 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridColumn3 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("lastrun")
        Dim Appearance10 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance11 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridColumn4 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("servername")
        Dim Appearance12 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridColumn5 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("statusdescription")
        Dim Appearance13 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridColumn6 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("details")
        Dim Appearance14 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance44 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance45 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance46 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance47 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance48 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance49 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance50 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance51 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance52 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance53 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance54 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form_POSPushJobController))
        Me.Button_StartJob = New System.Windows.Forms.Button()
        Me.ugrdJobStatus = New Infragistics.Win.UltraWinGrid.UltraGrid()
        Me.Button_Refresh = New System.Windows.Forms.Button()
        Me.lblPushRunning = New System.Windows.Forms.Label()
        CType(Me.ugrdJobStatus, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Button_StartJob
        '
        Me.Button_StartJob.Location = New System.Drawing.Point(12, 12)
        Me.Button_StartJob.Name = "Button_StartJob"
        Me.Button_StartJob.Size = New System.Drawing.Size(152, 23)
        Me.Button_StartJob.TabIndex = 0
        Me.Button_StartJob.Text = "Start Scale/POS Push"
        Me.Button_StartJob.UseVisualStyleBackColor = True
        '
        'ugrdJobStatus
        '
        Me.ugrdJobStatus.Anchor = System.Windows.Forms.AnchorStyles.None
        Appearance37.BackColor = System.Drawing.SystemColors.Window
        Appearance37.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.ugrdJobStatus.DisplayLayout.Appearance = Appearance37
        Me.ugrdJobStatus.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        UltraGridColumn1.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn1.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn1.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        Appearance8.FontData.BoldAsString = "True"
        UltraGridColumn1.Header.Appearance = Appearance8
        UltraGridColumn1.Header.Caption = "Job Name"
        UltraGridColumn1.Header.VisiblePosition = 0
        UltraGridColumn1.Width = 124
        UltraGridColumn2.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn2.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn2.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        Appearance9.FontData.BoldAsString = "True"
        UltraGridColumn2.Header.Appearance = Appearance9
        UltraGridColumn2.Header.Caption = "Job Status"
        UltraGridColumn2.Header.VisiblePosition = 1
        UltraGridColumn2.Width = 129
        UltraGridColumn3.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn3.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        Appearance10.TextHAlignAsString = "Right"
        UltraGridColumn3.CellAppearance = Appearance10
        UltraGridColumn3.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn3.Format = "M/d/yyyy h:mm tt"
        Appearance11.FontData.BoldAsString = "True"
        Appearance11.TextHAlignAsString = "Center"
        UltraGridColumn3.Header.Appearance = Appearance11
        UltraGridColumn3.Header.Caption = "Last Run"
        UltraGridColumn3.Header.TextOrientation = New Infragistics.Win.TextOrientationInfo(0, Infragistics.Win.TextFlowDirection.Horizontal)
        UltraGridColumn3.Header.VisiblePosition = 2
        UltraGridColumn3.Width = 169
        UltraGridColumn4.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn4.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn4.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        Appearance12.FontData.BoldAsString = "True"
        UltraGridColumn4.Header.Appearance = Appearance12
        UltraGridColumn4.Header.Caption = "Server Name"
        UltraGridColumn4.Header.VisiblePosition = 3
        UltraGridColumn4.Width = 140
        UltraGridColumn5.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn5.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn5.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        Appearance13.FontData.BoldAsString = "True"
        UltraGridColumn5.Header.Appearance = Appearance13
        UltraGridColumn5.Header.Caption = "Status Description"
        UltraGridColumn5.Header.VisiblePosition = 4
        UltraGridColumn5.Width = 163
        UltraGridColumn6.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn6.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn6.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        Appearance14.FontData.BoldAsString = "True"
        UltraGridColumn6.Header.Appearance = Appearance14
        UltraGridColumn6.Header.Caption = "Job Status Details"
        UltraGridColumn6.Header.VisiblePosition = 5
        UltraGridColumn6.Width = 157
        UltraGridBand1.Columns.AddRange(New Object() {UltraGridColumn1, UltraGridColumn2, UltraGridColumn3, UltraGridColumn4, UltraGridColumn5, UltraGridColumn6})
        Me.ugrdJobStatus.DisplayLayout.BandsSerializer.Add(UltraGridBand1)
        Me.ugrdJobStatus.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.ugrdJobStatus.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Appearance44.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance44.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance44.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance44.BorderColor = System.Drawing.SystemColors.Window
        Me.ugrdJobStatus.DisplayLayout.GroupByBox.Appearance = Appearance44
        Appearance45.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugrdJobStatus.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance45
        Me.ugrdJobStatus.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Appearance46.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance46.BackColor2 = System.Drawing.SystemColors.Control
        Appearance46.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance46.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugrdJobStatus.DisplayLayout.GroupByBox.PromptAppearance = Appearance46
        Me.ugrdJobStatus.DisplayLayout.MaxColScrollRegions = 1
        Me.ugrdJobStatus.DisplayLayout.MaxRowScrollRegions = 1
        Appearance47.BackColor = System.Drawing.SystemColors.Window
        Appearance47.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ugrdJobStatus.DisplayLayout.Override.ActiveCellAppearance = Appearance47
        Appearance48.BackColor = System.Drawing.SystemColors.Highlight
        Appearance48.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.ugrdJobStatus.DisplayLayout.Override.ActiveRowAppearance = Appearance48
        Me.ugrdJobStatus.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.ugrdJobStatus.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance49.BackColor = System.Drawing.SystemColors.Window
        Me.ugrdJobStatus.DisplayLayout.Override.CardAreaAppearance = Appearance49
        Appearance50.BorderColor = System.Drawing.Color.Silver
        Appearance50.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.ugrdJobStatus.DisplayLayout.Override.CellAppearance = Appearance50
        Me.ugrdJobStatus.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        Me.ugrdJobStatus.DisplayLayout.Override.CellPadding = 0
        Appearance51.BackColor = System.Drawing.SystemColors.Control
        Appearance51.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance51.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance51.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance51.BorderColor = System.Drawing.SystemColors.Window
        Me.ugrdJobStatus.DisplayLayout.Override.GroupByRowAppearance = Appearance51
        Appearance52.TextHAlignAsString = "Left"
        Me.ugrdJobStatus.DisplayLayout.Override.HeaderAppearance = Appearance52
        Me.ugrdJobStatus.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.[Select]
        Me.ugrdJobStatus.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance53.BackColor = System.Drawing.SystemColors.Window
        Appearance53.BorderColor = System.Drawing.Color.Silver
        Me.ugrdJobStatus.DisplayLayout.Override.RowAppearance = Appearance53
        Me.ugrdJobStatus.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[False]
        Me.ugrdJobStatus.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None
        Appearance54.BackColor = System.Drawing.SystemColors.ControlLight
        Me.ugrdJobStatus.DisplayLayout.Override.TemplateAddRowAppearance = Appearance54
        Me.ugrdJobStatus.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.ugrdJobStatus.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.ugrdJobStatus.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand
        Me.ugrdJobStatus.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ugrdJobStatus.Location = New System.Drawing.Point(62, 82)
        Me.ugrdJobStatus.Name = "ugrdJobStatus"
        Me.ugrdJobStatus.Size = New System.Drawing.Size(884, 111)
        Me.ugrdJobStatus.TabIndex = 34
        Me.ugrdJobStatus.Text = "Process Monitor"
        '
        'Button_Refresh
        '
        Me.Button_Refresh.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.Button_Refresh.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Refresh.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.Button_Refresh.Image = CType(resources.GetObject("Button_Refresh.Image"), System.Drawing.Image)
        Me.Button_Refresh.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Button_Refresh.Location = New System.Drawing.Point(963, 45)
        Me.Button_Refresh.Name = "Button_Refresh"
        Me.Button_Refresh.Size = New System.Drawing.Size(85, 32)
        Me.Button_Refresh.TabIndex = 35
        Me.Button_Refresh.Text = "&Refresh"
        Me.Button_Refresh.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Button_Refresh.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Button_Refresh.UseVisualStyleBackColor = True
        '
        'lblPushRunning
        '
        Me.lblPushRunning.AutoSize = True
        Me.lblPushRunning.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPushRunning.ForeColor = System.Drawing.Color.Red
        Me.lblPushRunning.Location = New System.Drawing.Point(99, 50)
        Me.lblPushRunning.Name = "lblPushRunning"
        Me.lblPushRunning.Size = New System.Drawing.Size(603, 19)
        Me.lblPushRunning.TabIndex = 36
        Me.lblPushRunning.Text = "The POS Push is already running.  Please click the Refresh button update the stat" & _
    "us."
        '
        'Form_POSPushJobController
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(1058, 232)
        Me.Controls.Add(Me.lblPushRunning)
        Me.Controls.Add(Me.Button_Refresh)
        Me.Controls.Add(Me.ugrdJobStatus)
        Me.Controls.Add(Me.Button_StartJob)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MinimizeBox = False
        Me.Name = "Form_POSPushJobController"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Scale/POS Push Controller"
        CType(Me.ugrdJobStatus, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Button_StartJob As System.Windows.Forms.Button
    Friend WithEvents ugrdJobStatus As Infragistics.Win.UltraWinGrid.UltraGrid
    Friend WithEvents Button_Refresh As System.Windows.Forms.Button
    Friend WithEvents lblPushRunning As System.Windows.Forms.Label
End Class
