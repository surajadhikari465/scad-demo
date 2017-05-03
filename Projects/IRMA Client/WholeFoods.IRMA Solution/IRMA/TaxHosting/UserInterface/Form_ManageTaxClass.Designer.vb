<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_ManageTaxClass
    Inherits Form_IRMABase

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
        Dim Appearance1 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
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
        Me.Button_Cancel = New System.Windows.Forms.Button()
        Me.Button_Delete = New System.Windows.Forms.Button()
        Me.Button_Add = New System.Windows.Forms.Button()
        Me.Button_Ok = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.UltraGrid_TaxClass = New Infragistics.Win.UltraWinGrid.UltraGrid()
        Me.GroupBox1.SuspendLayout()
        CType(Me.UltraGrid_TaxClass, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Button_Cancel
        '
        Me.Button_Cancel.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Button_Cancel.Location = New System.Drawing.Point(297, 320)
        Me.Button_Cancel.Name = "Button_Cancel"
        Me.Button_Cancel.Size = New System.Drawing.Size(75, 23)
        Me.Button_Cancel.TabIndex = 21
        Me.Button_Cancel.Text = "Cancel"
        Me.Button_Cancel.UseVisualStyleBackColor = True
        Me.Button_Cancel.Visible = False
        '
        'Button_Delete
        '
        Me.Button_Delete.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Button_Delete.Location = New System.Drawing.Point(356, 49)
        Me.Button_Delete.Name = "Button_Delete"
        Me.Button_Delete.Size = New System.Drawing.Size(75, 23)
        Me.Button_Delete.TabIndex = 19
        Me.Button_Delete.Text = "Delete"
        Me.Button_Delete.UseVisualStyleBackColor = True
        '
        'Button_Add
        '
        Me.Button_Add.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Button_Add.Location = New System.Drawing.Point(356, 20)
        Me.Button_Add.Name = "Button_Add"
        Me.Button_Add.Size = New System.Drawing.Size(75, 23)
        Me.Button_Add.TabIndex = 17
        Me.Button_Add.Text = "Add"
        Me.Button_Add.UseVisualStyleBackColor = True
        '
        'Button_Ok
        '
        Me.Button_Ok.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Button_Ok.Location = New System.Drawing.Point(378, 320)
        Me.Button_Ok.Name = "Button_Ok"
        Me.Button_Ok.Size = New System.Drawing.Size(75, 23)
        Me.Button_Ok.TabIndex = 16
        Me.Button_Ok.Text = "OK"
        Me.Button_Ok.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.UltraGrid_TaxClass)
        Me.GroupBox1.Controls.Add(Me.Button_Add)
        Me.GroupBox1.Controls.Add(Me.Button_Delete)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(441, 302)
        Me.GroupBox1.TabIndex = 20
        Me.GroupBox1.TabStop = False
        '
        'UltraGrid_TaxClass
        '
        Appearance1.BackColor = System.Drawing.SystemColors.Window
        Appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.UltraGrid_TaxClass.DisplayLayout.Appearance = Appearance1
        Me.UltraGrid_TaxClass.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        Me.UltraGrid_TaxClass.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.UltraGrid_TaxClass.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance2.BorderColor = System.Drawing.SystemColors.Window
        Me.UltraGrid_TaxClass.DisplayLayout.GroupByBox.Appearance = Appearance2
        Appearance3.ForeColor = System.Drawing.SystemColors.GrayText
        Me.UltraGrid_TaxClass.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance3
        Me.UltraGrid_TaxClass.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.UltraGrid_TaxClass.DisplayLayout.GroupByBox.Hidden = True
        Appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance4.BackColor2 = System.Drawing.SystemColors.Control
        Appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance4.ForeColor = System.Drawing.SystemColors.GrayText
        Me.UltraGrid_TaxClass.DisplayLayout.GroupByBox.PromptAppearance = Appearance4
        Me.UltraGrid_TaxClass.DisplayLayout.MaxColScrollRegions = 1
        Me.UltraGrid_TaxClass.DisplayLayout.MaxRowScrollRegions = 1
        Appearance5.BackColor = System.Drawing.SystemColors.Window
        Appearance5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.UltraGrid_TaxClass.DisplayLayout.Override.ActiveCellAppearance = Appearance5
        Appearance6.BackColor = System.Drawing.SystemColors.Highlight
        Appearance6.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.UltraGrid_TaxClass.DisplayLayout.Override.ActiveRowAppearance = Appearance6
        Me.UltraGrid_TaxClass.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.TemplateOnTop
        Me.UltraGrid_TaxClass.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.[True]
        Me.UltraGrid_TaxClass.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.[True]
        Me.UltraGrid_TaxClass.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.UltraGrid_TaxClass.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance7.BackColor = System.Drawing.SystemColors.Window
        Me.UltraGrid_TaxClass.DisplayLayout.Override.CardAreaAppearance = Appearance7
        Appearance8.BorderColor = System.Drawing.Color.Silver
        Appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.UltraGrid_TaxClass.DisplayLayout.Override.CellAppearance = Appearance8
        Me.UltraGrid_TaxClass.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.UltraGrid_TaxClass.DisplayLayout.Override.CellPadding = 0
        Appearance9.BackColor = System.Drawing.SystemColors.Control
        Appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance9.BorderColor = System.Drawing.SystemColors.Window
        Me.UltraGrid_TaxClass.DisplayLayout.Override.GroupByRowAppearance = Appearance9
        Appearance10.TextHAlignAsString = "Left"
        Me.UltraGrid_TaxClass.DisplayLayout.Override.HeaderAppearance = Appearance10
        Me.UltraGrid_TaxClass.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti
        Me.UltraGrid_TaxClass.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance11.BackColor = System.Drawing.SystemColors.Window
        Appearance11.BorderColor = System.Drawing.Color.Silver
        Me.UltraGrid_TaxClass.DisplayLayout.Override.RowAppearance = Appearance11
        Me.UltraGrid_TaxClass.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[True]
        Me.UltraGrid_TaxClass.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.[Single]
        Appearance12.BackColor = System.Drawing.SystemColors.ControlLight
        Me.UltraGrid_TaxClass.DisplayLayout.Override.TemplateAddRowAppearance = Appearance12
        Me.UltraGrid_TaxClass.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.UltraGrid_TaxClass.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.UltraGrid_TaxClass.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        Me.UltraGrid_TaxClass.Location = New System.Drawing.Point(16, 19)
        Me.UltraGrid_TaxClass.Name = "UltraGrid_TaxClass"
        Me.UltraGrid_TaxClass.Size = New System.Drawing.Size(334, 271)
        Me.UltraGrid_TaxClass.TabIndex = 22
        Me.UltraGrid_TaxClass.Text = "UltraGrid1"
        '
        'Form_ManageTaxClass
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(464, 351)
        Me.Controls.Add(Me.Button_Cancel)
        Me.Controls.Add(Me.Button_Ok)
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "Form_ManageTaxClass"
        Me.ShowInTaskbar = False
        Me.Text = "Manage Tax Class"
        Me.GroupBox1.ResumeLayout(False)
        CType(Me.UltraGrid_TaxClass, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Button_Cancel As System.Windows.Forms.Button
    Friend WithEvents Button_Delete As System.Windows.Forms.Button
    Friend WithEvents Button_Add As System.Windows.Forms.Button
    Friend WithEvents Button_Ok As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents UltraGrid_TaxClass As Infragistics.Win.UltraWinGrid.UltraGrid
End Class
