<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ItemAdd_ScalePluConflict
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
        Dim Appearance13 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridBand1 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1)
        Dim UltraGridColumn1 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("ItemIdentifier")
        Dim UltraGridColumn2 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("ItemDesc")
        Dim UltraGridColumn3 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("ItemSubTeam")
        Dim Appearance14 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance15 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance16 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance17 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance18 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance19 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance20 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance21 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance22 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance23 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance24 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Me.Button_OK = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.RadioButton_Send5Digits = New System.Windows.Forms.RadioButton()
        Me.RadioButton_DoNotSendToScale = New System.Windows.Forms.RadioButton()
        Me.RadioButton_CancelSave = New System.Windows.Forms.RadioButton()
        Me.UltraGrid_PluConflicts = New Infragistics.Win.UltraWinGrid.UltraGrid()
        Me.Label_ErrorMsg1 = New System.Windows.Forms.Label()
        Me.Label_ErrorMsg2 = New System.Windows.Forms.Label()
        Me.GroupBox1.SuspendLayout()
        CType(Me.UltraGrid_PluConflicts, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Button_OK
        '
        Me.Button_OK.Location = New System.Drawing.Point(171, 221)
        Me.Button_OK.Name = "Button_OK"
        Me.Button_OK.Size = New System.Drawing.Size(75, 23)
        Me.Button_OK.TabIndex = 1
        Me.Button_OK.Text = "OK"
        Me.Button_OK.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.RadioButton_Send5Digits)
        Me.GroupBox1.Controls.Add(Me.RadioButton_DoNotSendToScale)
        Me.GroupBox1.Controls.Add(Me.RadioButton_CancelSave)
        Me.GroupBox1.Location = New System.Drawing.Point(78, 125)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(258, 90)
        Me.GroupBox1.TabIndex = 2
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Choose an option"
        '
        'RadioButton_Send5Digits
        '
        Me.RadioButton_Send5Digits.AutoSize = True
        Me.RadioButton_Send5Digits.Location = New System.Drawing.Point(15, 65)
        Me.RadioButton_Send5Digits.Name = "RadioButton_Send5Digits"
        Me.RadioButton_Send5Digits.Size = New System.Drawing.Size(144, 17)
        Me.RadioButton_Send5Digits.TabIndex = 2
        Me.RadioButton_Send5Digits.TabStop = True
        Me.RadioButton_Send5Digits.Text = "Send 5 digits for this PLU"
        Me.RadioButton_Send5Digits.UseVisualStyleBackColor = True
        '
        'RadioButton_DoNotSendToScale
        '
        Me.RadioButton_DoNotSendToScale.AutoSize = True
        Me.RadioButton_DoNotSendToScale.Location = New System.Drawing.Point(15, 42)
        Me.RadioButton_DoNotSendToScale.Name = "RadioButton_DoNotSendToScale"
        Me.RadioButton_DoNotSendToScale.Size = New System.Drawing.Size(241, 17)
        Me.RadioButton_DoNotSendToScale.TabIndex = 1
        Me.RadioButton_DoNotSendToScale.TabStop = True
        Me.RadioButton_DoNotSendToScale.Text = "Keep this PLU but do not send it to the scales"
        Me.RadioButton_DoNotSendToScale.UseVisualStyleBackColor = True
        '
        'RadioButton_CancelSave
        '
        Me.RadioButton_CancelSave.AutoSize = True
        Me.RadioButton_CancelSave.Location = New System.Drawing.Point(15, 19)
        Me.RadioButton_CancelSave.Name = "RadioButton_CancelSave"
        Me.RadioButton_CancelSave.Size = New System.Drawing.Size(162, 17)
        Me.RadioButton_CancelSave.TabIndex = 0
        Me.RadioButton_CancelSave.TabStop = True
        Me.RadioButton_CancelSave.Text = "Cancel and enter a new PLU"
        Me.RadioButton_CancelSave.UseVisualStyleBackColor = True
        '
        'UltraGrid_PluConflicts
        '
        Appearance13.BackColor = System.Drawing.SystemColors.Window
        Appearance13.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.UltraGrid_PluConflicts.DisplayLayout.Appearance = Appearance13
        Me.UltraGrid_PluConflicts.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn
        UltraGridColumn1.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn1.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn1.Header.Caption = "Identifier"
        UltraGridColumn1.Header.VisiblePosition = 0
        UltraGridColumn1.RowLayoutColumnInfo.OriginX = 0
        UltraGridColumn1.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn1.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(88, 0)
        UltraGridColumn1.RowLayoutColumnInfo.SpanX = 1
        UltraGridColumn1.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn1.Width = 146
        UltraGridColumn2.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn2.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn2.Header.Caption = "Description"
        UltraGridColumn2.Header.VisiblePosition = 1
        UltraGridColumn2.RowLayoutColumnInfo.OriginX = 1
        UltraGridColumn2.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn2.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn2.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn2.Width = 146
        UltraGridColumn3.Header.VisiblePosition = 2
        UltraGridColumn3.Hidden = True
        UltraGridBand1.Columns.AddRange(New Object() {UltraGridColumn1, UltraGridColumn2, UltraGridColumn3})
        Me.UltraGrid_PluConflicts.DisplayLayout.BandsSerializer.Add(UltraGridBand1)
        Me.UltraGrid_PluConflicts.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.UltraGrid_PluConflicts.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Appearance14.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance14.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance14.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance14.BorderColor = System.Drawing.SystemColors.Window
        Me.UltraGrid_PluConflicts.DisplayLayout.GroupByBox.Appearance = Appearance14
        Appearance15.ForeColor = System.Drawing.SystemColors.GrayText
        Me.UltraGrid_PluConflicts.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance15
        Me.UltraGrid_PluConflicts.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.UltraGrid_PluConflicts.DisplayLayout.GroupByBox.Hidden = True
        Appearance16.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance16.BackColor2 = System.Drawing.SystemColors.Control
        Appearance16.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance16.ForeColor = System.Drawing.SystemColors.GrayText
        Me.UltraGrid_PluConflicts.DisplayLayout.GroupByBox.PromptAppearance = Appearance16
        Me.UltraGrid_PluConflicts.DisplayLayout.MaxColScrollRegions = 1
        Me.UltraGrid_PluConflicts.DisplayLayout.MaxRowScrollRegions = 1
        Appearance17.BackColor = System.Drawing.SystemColors.Window
        Appearance17.ForeColor = System.Drawing.SystemColors.ControlText
        Me.UltraGrid_PluConflicts.DisplayLayout.Override.ActiveCellAppearance = Appearance17
        Appearance18.BackColor = System.Drawing.SystemColors.Highlight
        Appearance18.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.UltraGrid_PluConflicts.DisplayLayout.Override.ActiveRowAppearance = Appearance18
        Me.UltraGrid_PluConflicts.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No
        Me.UltraGrid_PluConflicts.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.[False]
        Me.UltraGrid_PluConflicts.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.[False]
        Me.UltraGrid_PluConflicts.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.UltraGrid_PluConflicts.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance19.BackColor = System.Drawing.SystemColors.Window
        Me.UltraGrid_PluConflicts.DisplayLayout.Override.CardAreaAppearance = Appearance19
        Appearance20.BorderColor = System.Drawing.Color.Silver
        Appearance20.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.UltraGrid_PluConflicts.DisplayLayout.Override.CellAppearance = Appearance20
        Me.UltraGrid_PluConflicts.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.UltraGrid_PluConflicts.DisplayLayout.Override.CellPadding = 0
        Appearance21.BackColor = System.Drawing.SystemColors.Control
        Appearance21.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance21.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance21.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance21.BorderColor = System.Drawing.SystemColors.Window
        Me.UltraGrid_PluConflicts.DisplayLayout.Override.GroupByRowAppearance = Appearance21
        Appearance22.TextHAlignAsString = "Left"
        Me.UltraGrid_PluConflicts.DisplayLayout.Override.HeaderAppearance = Appearance22
        Me.UltraGrid_PluConflicts.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti
        Me.UltraGrid_PluConflicts.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance23.BackColor = System.Drawing.SystemColors.Window
        Appearance23.BorderColor = System.Drawing.Color.Silver
        Me.UltraGrid_PluConflicts.DisplayLayout.Override.RowAppearance = Appearance23
        Me.UltraGrid_PluConflicts.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[False]
        Me.UltraGrid_PluConflicts.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.UltraGrid_PluConflicts.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.UltraGrid_PluConflicts.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.None
        Appearance24.BackColor = System.Drawing.SystemColors.ControlLight
        Me.UltraGrid_PluConflicts.DisplayLayout.Override.TemplateAddRowAppearance = Appearance24
        Me.UltraGrid_PluConflicts.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.UltraGrid_PluConflicts.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.UltraGrid_PluConflicts.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.UltraGrid_PluConflicts.Location = New System.Drawing.Point(61, 22)
        Me.UltraGrid_PluConflicts.Name = "UltraGrid_PluConflicts"
        Me.UltraGrid_PluConflicts.Size = New System.Drawing.Size(294, 75)
        Me.UltraGrid_PluConflicts.TabIndex = 3
        Me.UltraGrid_PluConflicts.Text = "UltraGrid1"
        '
        'Label_ErrorMsg1
        '
        Me.Label_ErrorMsg1.AutoSize = True
        Me.Label_ErrorMsg1.Location = New System.Drawing.Point(12, 6)
        Me.Label_ErrorMsg1.Name = "Label_ErrorMsg1"
        Me.Label_ErrorMsg1.Size = New System.Drawing.Size(405, 13)
        Me.Label_ErrorMsg1.TabIndex = 4
        Me.Label_ErrorMsg1.Text = "For the SubTeam you selected, this scale PLU already exists for the following ite" & _
            "m(s):"
        '
        'Label_ErrorMsg2
        '
        Me.Label_ErrorMsg2.AutoSize = True
        Me.Label_ErrorMsg2.Location = New System.Drawing.Point(12, 100)
        Me.Label_ErrorMsg2.Name = "Label_ErrorMsg2"
        Me.Label_ErrorMsg2.Size = New System.Drawing.Size(208, 13)
        Me.Label_ErrorMsg2.TabIndex = 5
        Me.Label_ErrorMsg2.Text = "Please select one of the following choices:"
        '
        'ItemAdd_ScalePluConflict
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(426, 255)
        Me.Controls.Add(Me.Label_ErrorMsg2)
        Me.Controls.Add(Me.Label_ErrorMsg1)
        Me.Controls.Add(Me.UltraGrid_PluConflicts)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Button_OK)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ItemAdd_ScalePluConflict"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.Text = "Scale PLU Conflict"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.UltraGrid_PluConflicts, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Button_OK As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents RadioButton_Send5Digits As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton_DoNotSendToScale As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton_CancelSave As System.Windows.Forms.RadioButton
    Friend WithEvents UltraGrid_PluConflicts As Infragistics.Win.UltraWinGrid.UltraGrid
    Friend WithEvents Label_ErrorMsg1 As System.Windows.Forms.Label
    Friend WithEvents Label_ErrorMsg2 As System.Windows.Forms.Label
End Class
