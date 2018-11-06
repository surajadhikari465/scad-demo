<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_ManageGroups
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
        Dim Appearance13 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance14 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance15 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance16 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance17 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance18 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance19 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance20 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance21 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance22 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance23 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance24 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Me.UltraGrid_Groups = New Infragistics.Win.UltraWinGrid.UltraGrid
        Me.Button_Add = New System.Windows.Forms.Button
        Me.Button_Edit = New System.Windows.Forms.Button
        Me.Button_Remove = New System.Windows.Forms.Button
        Me.Button_Ok = New System.Windows.Forms.Button
        Me.Button_Cancel = New System.Windows.Forms.Button
        Me.Label_Message = New System.Windows.Forms.Label
        Me.ItemGroupBOBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.Button_Unlock = New System.Windows.Forms.Button
        CType(Me.UltraGrid_Groups, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ItemGroupBOBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'UltraGrid_Groups
        '
        Me.UltraGrid_Groups.DataSource = Me.ItemGroupBOBindingSource
        Appearance13.BackColor = System.Drawing.SystemColors.Window
        Appearance13.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.UltraGrid_Groups.DisplayLayout.Appearance = Appearance13
        Me.UltraGrid_Groups.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn
        Me.UltraGrid_Groups.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.UltraGrid_Groups.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Appearance14.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance14.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance14.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance14.BorderColor = System.Drawing.SystemColors.Window
        Me.UltraGrid_Groups.DisplayLayout.GroupByBox.Appearance = Appearance14
        Appearance15.ForeColor = System.Drawing.SystemColors.GrayText
        Me.UltraGrid_Groups.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance15
        Me.UltraGrid_Groups.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Appearance16.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance16.BackColor2 = System.Drawing.SystemColors.Control
        Appearance16.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance16.ForeColor = System.Drawing.SystemColors.GrayText
        Me.UltraGrid_Groups.DisplayLayout.GroupByBox.PromptAppearance = Appearance16
        Me.UltraGrid_Groups.DisplayLayout.MaxColScrollRegions = 1
        Me.UltraGrid_Groups.DisplayLayout.MaxRowScrollRegions = 1
        Appearance17.BackColor = System.Drawing.SystemColors.Window
        Appearance17.ForeColor = System.Drawing.SystemColors.ControlText
        Me.UltraGrid_Groups.DisplayLayout.Override.ActiveCellAppearance = Appearance17
        Appearance18.BackColor = System.Drawing.SystemColors.Highlight
        Appearance18.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.UltraGrid_Groups.DisplayLayout.Override.ActiveRowAppearance = Appearance18
        Me.UltraGrid_Groups.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No
        Me.UltraGrid_Groups.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.NotAllowed
        Me.UltraGrid_Groups.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.None
        Me.UltraGrid_Groups.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed
        Me.UltraGrid_Groups.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.[False]
        Me.UltraGrid_Groups.DisplayLayout.Override.AllowRowSummaries = Infragistics.Win.UltraWinGrid.AllowRowSummaries.[False]
        Me.UltraGrid_Groups.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.[False]
        Me.UltraGrid_Groups.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.UltraGrid_Groups.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance19.BackColor = System.Drawing.SystemColors.Window
        Me.UltraGrid_Groups.DisplayLayout.Override.CardAreaAppearance = Appearance19
        Appearance20.BorderColor = System.Drawing.Color.Silver
        Appearance20.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.UltraGrid_Groups.DisplayLayout.Override.CellAppearance = Appearance20
        Me.UltraGrid_Groups.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        Me.UltraGrid_Groups.DisplayLayout.Override.CellPadding = 0
        Appearance21.BackColor = System.Drawing.SystemColors.Control
        Appearance21.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance21.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance21.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance21.BorderColor = System.Drawing.SystemColors.Window
        Me.UltraGrid_Groups.DisplayLayout.Override.GroupByRowAppearance = Appearance21
        Appearance22.TextHAlign = Infragistics.Win.HAlign.Left
        Me.UltraGrid_Groups.DisplayLayout.Override.HeaderAppearance = Appearance22
        Me.UltraGrid_Groups.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti
        Me.UltraGrid_Groups.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance23.BackColor = System.Drawing.SystemColors.Window
        Appearance23.BorderColor = System.Drawing.Color.Silver
        Me.UltraGrid_Groups.DisplayLayout.Override.RowAppearance = Appearance23
        Me.UltraGrid_Groups.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[False]
        Me.UltraGrid_Groups.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed
        Me.UltraGrid_Groups.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.UltraGrid_Groups.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.UltraGrid_Groups.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.[Single]
        Appearance24.BackColor = System.Drawing.SystemColors.ControlLight
        Me.UltraGrid_Groups.DisplayLayout.Override.TemplateAddRowAppearance = Appearance24
        Me.UltraGrid_Groups.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.UltraGrid_Groups.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.UltraGrid_Groups.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.UltraGrid_Groups.Location = New System.Drawing.Point(12, 35)
        Me.UltraGrid_Groups.Name = "UltraGrid_Groups"
        Me.UltraGrid_Groups.Size = New System.Drawing.Size(441, 214)
        Me.UltraGrid_Groups.TabIndex = 0
        Me.UltraGrid_Groups.Text = "UltraGrid1"
        '
        'Button_Add
        '
        Me.Button_Add.Location = New System.Drawing.Point(459, 35)
        Me.Button_Add.Name = "Button_Add"
        Me.Button_Add.Size = New System.Drawing.Size(75, 23)
        Me.Button_Add.TabIndex = 1
        Me.Button_Add.Text = "Add"
        Me.Button_Add.UseVisualStyleBackColor = True
        '
        'Button_Edit
        '
        Me.Button_Edit.Location = New System.Drawing.Point(459, 65)
        Me.Button_Edit.Name = "Button_Edit"
        Me.Button_Edit.Size = New System.Drawing.Size(75, 23)
        Me.Button_Edit.TabIndex = 2
        Me.Button_Edit.Text = "Edit"
        Me.Button_Edit.UseVisualStyleBackColor = True
        '
        'Button_Remove
        '
        Me.Button_Remove.Location = New System.Drawing.Point(459, 123)
        Me.Button_Remove.Name = "Button_Remove"
        Me.Button_Remove.Size = New System.Drawing.Size(75, 23)
        Me.Button_Remove.TabIndex = 4
        Me.Button_Remove.Text = "Remove"
        Me.Button_Remove.UseVisualStyleBackColor = True
        '
        'Button_Ok
        '
        Me.Button_Ok.Location = New System.Drawing.Point(459, 274)
        Me.Button_Ok.Name = "Button_Ok"
        Me.Button_Ok.Size = New System.Drawing.Size(75, 23)
        Me.Button_Ok.TabIndex = 6
        Me.Button_Ok.Text = "OK"
        Me.Button_Ok.UseVisualStyleBackColor = True
        '
        'Button_Cancel
        '
        Me.Button_Cancel.Location = New System.Drawing.Point(378, 274)
        Me.Button_Cancel.Name = "Button_Cancel"
        Me.Button_Cancel.Size = New System.Drawing.Size(75, 23)
        Me.Button_Cancel.TabIndex = 5
        Me.Button_Cancel.Text = "Cancel"
        Me.Button_Cancel.UseVisualStyleBackColor = True
        '
        'Label_Message
        '
        Me.Label_Message.AutoSize = True
        Me.Label_Message.Location = New System.Drawing.Point(12, 256)
        Me.Label_Message.Name = "Label_Message"
        Me.Label_Message.Size = New System.Drawing.Size(0, 13)
        Me.Label_Message.TabIndex = 6
        Me.Label_Message.Visible = False
        '
        'ItemGroupBOBindingSource
        '
        Me.ItemGroupBOBindingSource.DataSource = GetType(WholeFoods.IRMA.EPromotions.BusinessLogic.ItemGroupBO)
        '
        'Button_Unlock
        '
        Me.Button_Unlock.Enabled = False
        Me.Button_Unlock.Location = New System.Drawing.Point(459, 94)
        Me.Button_Unlock.Name = "Button_Unlock"
        Me.Button_Unlock.Size = New System.Drawing.Size(75, 23)
        Me.Button_Unlock.TabIndex = 3
        Me.Button_Unlock.Text = "Unlock"
        Me.Button_Unlock.UseVisualStyleBackColor = True
        '
        'Form_ManageGroups
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(537, 303)
        Me.Controls.Add(Me.Button_Unlock)
        Me.Controls.Add(Me.Label_Message)
        Me.Controls.Add(Me.Button_Cancel)
        Me.Controls.Add(Me.Button_Ok)
        Me.Controls.Add(Me.Button_Remove)
        Me.Controls.Add(Me.Button_Edit)
        Me.Controls.Add(Me.Button_Add)
        Me.Controls.Add(Me.UltraGrid_Groups)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Form_ManageGroups"
        Me.ShowInTaskbar = False
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Manage Groups"
        CType(Me.UltraGrid_Groups, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ItemGroupBOBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ItemGroupBOBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents UltraGrid_Groups As Infragistics.Win.UltraWinGrid.UltraGrid
    Friend WithEvents Button_Add As System.Windows.Forms.Button
    Friend WithEvents Button_Edit As System.Windows.Forms.Button
    Friend WithEvents Button_Remove As System.Windows.Forms.Button
    Friend WithEvents Button_Ok As System.Windows.Forms.Button
    Friend WithEvents Button_Cancel As System.Windows.Forms.Button
    Friend WithEvents Label_Message As System.Windows.Forms.Label
    Friend WithEvents Button_Unlock As System.Windows.Forms.Button
End Class
