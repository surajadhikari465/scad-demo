<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class InvoiceControlGroupSearch
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(InvoiceControlGroupSearch))
        Me.Label_ControlGroupID = New System.Windows.Forms.Label
        Me.GroupBox_SearchResults = New System.Windows.Forms.GroupBox
        Me.UltraGrid_ControlGroups = New Infragistics.Win.UltraWinGrid.UltraGrid
        Me.Button_Select = New System.Windows.Forms.Button
        Me.Button_Exit = New System.Windows.Forms.Button
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.GroupBox_Status = New System.Windows.Forms.GroupBox
        Me.RadioButton_AllStatus = New System.Windows.Forms.RadioButton
        Me.RadioButton_StatusOpen = New System.Windows.Forms.RadioButton
        Me.RadioButton_ClosedStatus = New System.Windows.Forms.RadioButton
        Me.Button_Search = New System.Windows.Forms.Button
        Me.GroupBox_SearchCriteria = New System.Windows.Forms.GroupBox
        Me.UltraNumericEditor_ControlGroupID = New Infragistics.Win.UltraWinEditors.UltraNumericEditor
        Me.GroupBox_SearchResults.SuspendLayout()
        CType(Me.UltraGrid_ControlGroups, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox_Status.SuspendLayout()
        Me.GroupBox_SearchCriteria.SuspendLayout()
        CType(Me.UltraNumericEditor_ControlGroupID, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label_ControlGroupID
        '
        Me.Label_ControlGroupID.AutoSize = True
        Me.Label_ControlGroupID.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.Label_ControlGroupID.Location = New System.Drawing.Point(17, 76)
        Me.Label_ControlGroupID.Name = "Label_ControlGroupID"
        Me.Label_ControlGroupID.Size = New System.Drawing.Size(104, 14)
        Me.Label_ControlGroupID.TabIndex = 134
        Me.Label_ControlGroupID.Text = "Control Group ID :"
        '
        'GroupBox_SearchResults
        '
        Me.GroupBox_SearchResults.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox_SearchResults.Controls.Add(Me.UltraGrid_ControlGroups)
        Me.GroupBox_SearchResults.Location = New System.Drawing.Point(12, 137)
        Me.GroupBox_SearchResults.Name = "GroupBox_SearchResults"
        Me.GroupBox_SearchResults.Size = New System.Drawing.Size(485, 231)
        Me.GroupBox_SearchResults.TabIndex = 137
        Me.GroupBox_SearchResults.TabStop = False
        Me.GroupBox_SearchResults.Text = "Search Results"
        '
        'UltraGrid_ControlGroups
        '
        Me.UltraGrid_ControlGroups.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Appearance1.BackColor = System.Drawing.SystemColors.Window
        Appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.UltraGrid_ControlGroups.DisplayLayout.Appearance = Appearance1
        Me.UltraGrid_ControlGroups.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        Me.UltraGrid_ControlGroups.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.UltraGrid_ControlGroups.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance2.BorderColor = System.Drawing.SystemColors.Window
        Me.UltraGrid_ControlGroups.DisplayLayout.GroupByBox.Appearance = Appearance2
        Appearance3.ForeColor = System.Drawing.SystemColors.GrayText
        Me.UltraGrid_ControlGroups.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance3
        Me.UltraGrid_ControlGroups.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.UltraGrid_ControlGroups.DisplayLayout.GroupByBox.Hidden = True
        Appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance4.BackColor2 = System.Drawing.SystemColors.Control
        Appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance4.ForeColor = System.Drawing.SystemColors.GrayText
        Me.UltraGrid_ControlGroups.DisplayLayout.GroupByBox.PromptAppearance = Appearance4
        Me.UltraGrid_ControlGroups.DisplayLayout.MaxColScrollRegions = 1
        Me.UltraGrid_ControlGroups.DisplayLayout.MaxRowScrollRegions = 1
        Appearance5.BackColor = System.Drawing.SystemColors.Window
        Appearance5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.UltraGrid_ControlGroups.DisplayLayout.Override.ActiveCellAppearance = Appearance5
        Appearance6.BackColor = System.Drawing.SystemColors.Highlight
        Appearance6.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.UltraGrid_ControlGroups.DisplayLayout.Override.ActiveRowAppearance = Appearance6
        Me.UltraGrid_ControlGroups.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No
        Me.UltraGrid_ControlGroups.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.[False]
        Me.UltraGrid_ControlGroups.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.[False]
        Me.UltraGrid_ControlGroups.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.UltraGrid_ControlGroups.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance7.BackColor = System.Drawing.SystemColors.Window
        Me.UltraGrid_ControlGroups.DisplayLayout.Override.CardAreaAppearance = Appearance7
        Appearance8.BorderColor = System.Drawing.Color.Silver
        Appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.UltraGrid_ControlGroups.DisplayLayout.Override.CellAppearance = Appearance8
        Me.UltraGrid_ControlGroups.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.UltraGrid_ControlGroups.DisplayLayout.Override.CellPadding = 0
        Appearance9.BackColor = System.Drawing.SystemColors.Control
        Appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance9.BorderColor = System.Drawing.SystemColors.Window
        Me.UltraGrid_ControlGroups.DisplayLayout.Override.GroupByRowAppearance = Appearance9
        Appearance10.TextHAlign = Infragistics.Win.HAlign.Left
        Me.UltraGrid_ControlGroups.DisplayLayout.Override.HeaderAppearance = Appearance10
        Me.UltraGrid_ControlGroups.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti
        Me.UltraGrid_ControlGroups.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance11.BackColor = System.Drawing.SystemColors.Window
        Appearance11.BorderColor = System.Drawing.Color.Silver
        Me.UltraGrid_ControlGroups.DisplayLayout.Override.RowAppearance = Appearance11
        Me.UltraGrid_ControlGroups.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[True]
        Me.UltraGrid_ControlGroups.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.[Single]
        Appearance12.BackColor = System.Drawing.SystemColors.ControlLight
        Me.UltraGrid_ControlGroups.DisplayLayout.Override.TemplateAddRowAppearance = Appearance12
        Me.UltraGrid_ControlGroups.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.UltraGrid_ControlGroups.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.UltraGrid_ControlGroups.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        Me.UltraGrid_ControlGroups.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.UltraGrid_ControlGroups.Location = New System.Drawing.Point(6, 19)
        Me.UltraGrid_ControlGroups.Name = "UltraGrid_ControlGroups"
        Me.UltraGrid_ControlGroups.Size = New System.Drawing.Size(473, 206)
        Me.UltraGrid_ControlGroups.TabIndex = 143
        Me.UltraGrid_ControlGroups.Text = "UltraGrid1"
        '
        'Button_Select
        '
        Me.Button_Select.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Select.BackColor = System.Drawing.SystemColors.Control
        Me.Button_Select.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button_Select.Font = New System.Drawing.Font("Arial", 8.0!)
        Me.Button_Select.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button_Select.Image = CType(resources.GetObject("Button_Select.Image"), System.Drawing.Image)
        Me.Button_Select.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Button_Select.Location = New System.Drawing.Point(409, 374)
        Me.Button_Select.Name = "Button_Select"
        Me.Button_Select.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button_Select.Size = New System.Drawing.Size(41, 41)
        Me.Button_Select.TabIndex = 6
        Me.Button_Select.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.Button_Select, "Select Control Group")
        Me.Button_Select.UseVisualStyleBackColor = False
        '
        'Button_Exit
        '
        Me.Button_Exit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Exit.BackColor = System.Drawing.SystemColors.Control
        Me.Button_Exit.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button_Exit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Button_Exit.Font = New System.Drawing.Font("Arial", 8.0!)
        Me.Button_Exit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button_Exit.Image = CType(resources.GetObject("Button_Exit.Image"), System.Drawing.Image)
        Me.Button_Exit.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Button_Exit.Location = New System.Drawing.Point(456, 374)
        Me.Button_Exit.Name = "Button_Exit"
        Me.Button_Exit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button_Exit.Size = New System.Drawing.Size(41, 41)
        Me.Button_Exit.TabIndex = 7
        Me.Button_Exit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.Button_Exit, "Exit")
        Me.Button_Exit.UseVisualStyleBackColor = False
        '
        'GroupBox_Status
        '
        Me.GroupBox_Status.Controls.Add(Me.RadioButton_AllStatus)
        Me.GroupBox_Status.Controls.Add(Me.RadioButton_StatusOpen)
        Me.GroupBox_Status.Controls.Add(Me.RadioButton_ClosedStatus)
        Me.GroupBox_Status.Location = New System.Drawing.Point(20, 19)
        Me.GroupBox_Status.Name = "GroupBox_Status"
        Me.GroupBox_Status.Size = New System.Drawing.Size(211, 48)
        Me.GroupBox_Status.TabIndex = 140
        Me.GroupBox_Status.TabStop = False
        Me.GroupBox_Status.Text = "Control Group Status"
        '
        'RadioButton_AllStatus
        '
        Me.RadioButton_AllStatus.AutoSize = True
        Me.RadioButton_AllStatus.Checked = True
        Me.RadioButton_AllStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioButton_AllStatus.Location = New System.Drawing.Point(6, 19)
        Me.RadioButton_AllStatus.Name = "RadioButton_AllStatus"
        Me.RadioButton_AllStatus.Size = New System.Drawing.Size(39, 17)
        Me.RadioButton_AllStatus.TabIndex = 1
        Me.RadioButton_AllStatus.TabStop = True
        Me.RadioButton_AllStatus.Text = "All"
        Me.RadioButton_AllStatus.UseVisualStyleBackColor = True
        '
        'RadioButton_StatusOpen
        '
        Me.RadioButton_StatusOpen.AutoSize = True
        Me.RadioButton_StatusOpen.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioButton_StatusOpen.Location = New System.Drawing.Point(66, 19)
        Me.RadioButton_StatusOpen.Name = "RadioButton_StatusOpen"
        Me.RadioButton_StatusOpen.Size = New System.Drawing.Size(55, 17)
        Me.RadioButton_StatusOpen.TabIndex = 2
        Me.RadioButton_StatusOpen.Text = "Open"
        Me.RadioButton_StatusOpen.UseVisualStyleBackColor = True
        '
        'RadioButton_ClosedStatus
        '
        Me.RadioButton_ClosedStatus.AutoSize = True
        Me.RadioButton_ClosedStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioButton_ClosedStatus.Location = New System.Drawing.Point(142, 19)
        Me.RadioButton_ClosedStatus.Name = "RadioButton_ClosedStatus"
        Me.RadioButton_ClosedStatus.Size = New System.Drawing.Size(63, 17)
        Me.RadioButton_ClosedStatus.TabIndex = 3
        Me.RadioButton_ClosedStatus.TabStop = True
        Me.RadioButton_ClosedStatus.Text = "Closed"
        Me.RadioButton_ClosedStatus.UseVisualStyleBackColor = True
        '
        'Button_Search
        '
        Me.Button_Search.Location = New System.Drawing.Point(333, 71)
        Me.Button_Search.Name = "Button_Search"
        Me.Button_Search.Size = New System.Drawing.Size(143, 23)
        Me.Button_Search.TabIndex = 5
        Me.Button_Search.Text = "Search for Control Groups"
        Me.Button_Search.UseVisualStyleBackColor = True
        '
        'GroupBox_SearchCriteria
        '
        Me.GroupBox_SearchCriteria.Controls.Add(Me.UltraNumericEditor_ControlGroupID)
        Me.GroupBox_SearchCriteria.Controls.Add(Me.GroupBox_Status)
        Me.GroupBox_SearchCriteria.Controls.Add(Me.Button_Search)
        Me.GroupBox_SearchCriteria.Controls.Add(Me.Label_ControlGroupID)
        Me.GroupBox_SearchCriteria.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox_SearchCriteria.Name = "GroupBox_SearchCriteria"
        Me.GroupBox_SearchCriteria.Size = New System.Drawing.Size(485, 104)
        Me.GroupBox_SearchCriteria.TabIndex = 142
        Me.GroupBox_SearchCriteria.TabStop = False
        Me.GroupBox_SearchCriteria.Text = "Search Criteria"
        '
        'UltraNumericEditor_ControlGroupID
        '
        Me.UltraNumericEditor_ControlGroupID.AlwaysInEditMode = True
        Me.UltraNumericEditor_ControlGroupID.Location = New System.Drawing.Point(127, 72)
        Me.UltraNumericEditor_ControlGroupID.MaxValue = 1999999999
        Me.UltraNumericEditor_ControlGroupID.MinValue = 1
        Me.UltraNumericEditor_ControlGroupID.Name = "UltraNumericEditor_ControlGroupID"
        Me.UltraNumericEditor_ControlGroupID.Nullable = True
        Me.UltraNumericEditor_ControlGroupID.NullText = """"""
        Me.UltraNumericEditor_ControlGroupID.PromptChar = Global.Microsoft.VisualBasic.ChrW(32)
        Me.UltraNumericEditor_ControlGroupID.Size = New System.Drawing.Size(75, 21)
        Me.UltraNumericEditor_ControlGroupID.TabIndex = 4
        Me.UltraNumericEditor_ControlGroupID.Value = Nothing
        '
        'InvoiceControlGroupSearch
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(509, 427)
        Me.Controls.Add(Me.GroupBox_SearchCriteria)
        Me.Controls.Add(Me.Button_Select)
        Me.Controls.Add(Me.Button_Exit)
        Me.Controls.Add(Me.GroupBox_SearchResults)
        Me.Name = "InvoiceControlGroupSearch"
        Me.Text = "Invoice Control Group Search"
        Me.GroupBox_SearchResults.ResumeLayout(False)
        CType(Me.UltraGrid_ControlGroups, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox_Status.ResumeLayout(False)
        Me.GroupBox_Status.PerformLayout()
        Me.GroupBox_SearchCriteria.ResumeLayout(False)
        Me.GroupBox_SearchCriteria.PerformLayout()
        CType(Me.UltraNumericEditor_ControlGroupID, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label_ControlGroupID As System.Windows.Forms.Label
    Friend WithEvents GroupBox_SearchResults As System.Windows.Forms.GroupBox
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Public WithEvents Button_Select As System.Windows.Forms.Button
    Public WithEvents Button_Exit As System.Windows.Forms.Button
    Friend WithEvents GroupBox_Status As System.Windows.Forms.GroupBox
    Friend WithEvents RadioButton_AllStatus As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton_StatusOpen As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton_ClosedStatus As System.Windows.Forms.RadioButton
    Friend WithEvents Button_Search As System.Windows.Forms.Button
    Friend WithEvents GroupBox_SearchCriteria As System.Windows.Forms.GroupBox
    Friend WithEvents UltraNumericEditor_ControlGroupID As Infragistics.Win.UltraWinEditors.UltraNumericEditor
    Friend WithEvents UltraGrid_ControlGroups As Infragistics.Win.UltraWinGrid.UltraGrid
End Class
