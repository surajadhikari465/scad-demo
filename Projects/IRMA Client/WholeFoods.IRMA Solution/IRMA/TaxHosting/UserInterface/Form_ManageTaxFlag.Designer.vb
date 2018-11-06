<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_ManageTaxFlag
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
        Me.Button_Delete = New System.Windows.Forms.Button()
        Me.Button_Edit = New System.Windows.Forms.Button()
        Me.Button_Add = New System.Windows.Forms.Button()
        Me.Button_Ok = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Button_AddTaxClass = New System.Windows.Forms.Button()
        Me.UltraGrid_TaxFlag = New Infragistics.Win.UltraWinGrid.UltraGrid()
        Me.Button_AddJurisdiction = New System.Windows.Forms.Button()
        Me.Label_TaxClass = New System.Windows.Forms.Label()
        Me.ComboBox_TaxClass = New System.Windows.Forms.ComboBox()
        Me.Label_Jurisdiction = New System.Windows.Forms.Label()
        Me.ComboBox_Jurisdiction = New System.Windows.Forms.ComboBox()
        Me.Button_Cancel = New System.Windows.Forms.Button()
        Me.GroupBox1.SuspendLayout()
        CType(Me.UltraGrid_TaxFlag, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Button_Delete
        '
        Me.Button_Delete.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Delete.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Button_Delete.Location = New System.Drawing.Point(574, 189)
        Me.Button_Delete.Name = "Button_Delete"
        Me.Button_Delete.Size = New System.Drawing.Size(75, 23)
        Me.Button_Delete.TabIndex = 18
        Me.Button_Delete.Text = "Delete"
        Me.Button_Delete.UseVisualStyleBackColor = True
        '
        'Button_Edit
        '
        Me.Button_Edit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Edit.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Button_Edit.Location = New System.Drawing.Point(574, 160)
        Me.Button_Edit.Name = "Button_Edit"
        Me.Button_Edit.Size = New System.Drawing.Size(75, 23)
        Me.Button_Edit.TabIndex = 17
        Me.Button_Edit.Text = "Edit"
        Me.Button_Edit.UseVisualStyleBackColor = True
        '
        'Button_Add
        '
        Me.Button_Add.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Add.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Button_Add.Location = New System.Drawing.Point(574, 131)
        Me.Button_Add.Name = "Button_Add"
        Me.Button_Add.Size = New System.Drawing.Size(75, 23)
        Me.Button_Add.TabIndex = 16
        Me.Button_Add.Text = "Add"
        Me.Button_Add.UseVisualStyleBackColor = True
        '
        'Button_Ok
        '
        Me.Button_Ok.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Ok.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Button_Ok.Location = New System.Drawing.Point(506, 332)
        Me.Button_Ok.Name = "Button_Ok"
        Me.Button_Ok.Size = New System.Drawing.Size(75, 23)
        Me.Button_Ok.TabIndex = 15
        Me.Button_Ok.Text = "OK"
        Me.Button_Ok.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.Button_AddTaxClass)
        Me.GroupBox1.Controls.Add(Me.UltraGrid_TaxFlag)
        Me.GroupBox1.Controls.Add(Me.Button_AddJurisdiction)
        Me.GroupBox1.Controls.Add(Me.Label_TaxClass)
        Me.GroupBox1.Controls.Add(Me.Button_Delete)
        Me.GroupBox1.Controls.Add(Me.Button_Edit)
        Me.GroupBox1.Controls.Add(Me.ComboBox_TaxClass)
        Me.GroupBox1.Controls.Add(Me.Button_Add)
        Me.GroupBox1.Controls.Add(Me.Label_Jurisdiction)
        Me.GroupBox1.Controls.Add(Me.ComboBox_Jurisdiction)
        Me.GroupBox1.Location = New System.Drawing.Point(13, 9)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(655, 317)
        Me.GroupBox1.TabIndex = 19
        Me.GroupBox1.TabStop = False
        '
        'Button_AddTaxClass
        '
        Me.Button_AddTaxClass.Location = New System.Drawing.Point(252, 86)
        Me.Button_AddTaxClass.Name = "Button_AddTaxClass"
        Me.Button_AddTaxClass.Size = New System.Drawing.Size(75, 23)
        Me.Button_AddTaxClass.TabIndex = 20
        Me.Button_AddTaxClass.Text = "Add"
        Me.Button_AddTaxClass.UseVisualStyleBackColor = True
        '
        'UltraGrid_TaxFlag
        '
        Me.UltraGrid_TaxFlag.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Appearance1.BackColor = System.Drawing.SystemColors.Window
        Appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.UltraGrid_TaxFlag.DisplayLayout.Appearance = Appearance1
        Me.UltraGrid_TaxFlag.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        Me.UltraGrid_TaxFlag.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.UltraGrid_TaxFlag.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance2.BorderColor = System.Drawing.SystemColors.Window
        Me.UltraGrid_TaxFlag.DisplayLayout.GroupByBox.Appearance = Appearance2
        Appearance3.ForeColor = System.Drawing.SystemColors.GrayText
        Me.UltraGrid_TaxFlag.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance3
        Me.UltraGrid_TaxFlag.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.UltraGrid_TaxFlag.DisplayLayout.GroupByBox.Hidden = True
        Appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance4.BackColor2 = System.Drawing.SystemColors.Control
        Appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance4.ForeColor = System.Drawing.SystemColors.GrayText
        Me.UltraGrid_TaxFlag.DisplayLayout.GroupByBox.PromptAppearance = Appearance4
        Me.UltraGrid_TaxFlag.DisplayLayout.MaxColScrollRegions = 1
        Me.UltraGrid_TaxFlag.DisplayLayout.MaxRowScrollRegions = 1
        Appearance5.BackColor = System.Drawing.SystemColors.Window
        Appearance5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.UltraGrid_TaxFlag.DisplayLayout.Override.ActiveCellAppearance = Appearance5
        Appearance6.BackColor = System.Drawing.SystemColors.Highlight
        Appearance6.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.UltraGrid_TaxFlag.DisplayLayout.Override.ActiveRowAppearance = Appearance6
        Me.UltraGrid_TaxFlag.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No
        Me.UltraGrid_TaxFlag.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.[False]
        Me.UltraGrid_TaxFlag.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.UltraGrid_TaxFlag.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance7.BackColor = System.Drawing.SystemColors.Window
        Me.UltraGrid_TaxFlag.DisplayLayout.Override.CardAreaAppearance = Appearance7
        Appearance8.BorderColor = System.Drawing.Color.Silver
        Appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.UltraGrid_TaxFlag.DisplayLayout.Override.CellAppearance = Appearance8
        Me.UltraGrid_TaxFlag.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.UltraGrid_TaxFlag.DisplayLayout.Override.CellPadding = 0
        Appearance9.BackColor = System.Drawing.SystemColors.Control
        Appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance9.BorderColor = System.Drawing.SystemColors.Window
        Me.UltraGrid_TaxFlag.DisplayLayout.Override.GroupByRowAppearance = Appearance9
        Appearance10.TextHAlignAsString = "Left"
        Me.UltraGrid_TaxFlag.DisplayLayout.Override.HeaderAppearance = Appearance10
        Me.UltraGrid_TaxFlag.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti
        Me.UltraGrid_TaxFlag.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance11.BackColor = System.Drawing.SystemColors.Window
        Appearance11.BorderColor = System.Drawing.Color.Silver
        Me.UltraGrid_TaxFlag.DisplayLayout.Override.RowAppearance = Appearance11
        Me.UltraGrid_TaxFlag.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[True]
        Me.UltraGrid_TaxFlag.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.[Single]
        Appearance12.BackColor = System.Drawing.SystemColors.ControlLight
        Me.UltraGrid_TaxFlag.DisplayLayout.Override.TemplateAddRowAppearance = Appearance12
        Me.UltraGrid_TaxFlag.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.UltraGrid_TaxFlag.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.UltraGrid_TaxFlag.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        Me.UltraGrid_TaxFlag.Location = New System.Drawing.Point(20, 131)
        Me.UltraGrid_TaxFlag.Name = "UltraGrid_TaxFlag"
        Me.UltraGrid_TaxFlag.Size = New System.Drawing.Size(548, 170)
        Me.UltraGrid_TaxFlag.TabIndex = 21
        '
        'Button_AddJurisdiction
        '
        Me.Button_AddJurisdiction.Location = New System.Drawing.Point(252, 32)
        Me.Button_AddJurisdiction.Name = "Button_AddJurisdiction"
        Me.Button_AddJurisdiction.Size = New System.Drawing.Size(75, 23)
        Me.Button_AddJurisdiction.TabIndex = 19
        Me.Button_AddJurisdiction.Text = "Add"
        Me.Button_AddJurisdiction.UseVisualStyleBackColor = True
        '
        'Label_TaxClass
        '
        Me.Label_TaxClass.AutoSize = True
        Me.Label_TaxClass.Location = New System.Drawing.Point(17, 68)
        Me.Label_TaxClass.Name = "Label_TaxClass"
        Me.Label_TaxClass.Size = New System.Drawing.Size(89, 13)
        Me.Label_TaxClass.TabIndex = 3
        Me.Label_TaxClass.Text = "Tax Classification"
        '
        'ComboBox_TaxClass
        '
        Me.ComboBox_TaxClass.FormattingEnabled = True
        Me.ComboBox_TaxClass.Location = New System.Drawing.Point(20, 86)
        Me.ComboBox_TaxClass.Name = "ComboBox_TaxClass"
        Me.ComboBox_TaxClass.Size = New System.Drawing.Size(214, 21)
        Me.ComboBox_TaxClass.TabIndex = 2
        '
        'Label_Jurisdiction
        '
        Me.Label_Jurisdiction.AutoSize = True
        Me.Label_Jurisdiction.Location = New System.Drawing.Point(17, 19)
        Me.Label_Jurisdiction.Name = "Label_Jurisdiction"
        Me.Label_Jurisdiction.Size = New System.Drawing.Size(59, 13)
        Me.Label_Jurisdiction.TabIndex = 1
        Me.Label_Jurisdiction.Text = "Jurisdiction"
        '
        'ComboBox_Jurisdiction
        '
        Me.ComboBox_Jurisdiction.FormattingEnabled = True
        Me.ComboBox_Jurisdiction.Location = New System.Drawing.Point(20, 35)
        Me.ComboBox_Jurisdiction.Name = "ComboBox_Jurisdiction"
        Me.ComboBox_Jurisdiction.Size = New System.Drawing.Size(214, 21)
        Me.ComboBox_Jurisdiction.TabIndex = 0
        '
        'Button_Cancel
        '
        Me.Button_Cancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Cancel.Location = New System.Drawing.Point(425, 332)
        Me.Button_Cancel.Name = "Button_Cancel"
        Me.Button_Cancel.Size = New System.Drawing.Size(75, 23)
        Me.Button_Cancel.TabIndex = 20
        Me.Button_Cancel.Text = "Cancel"
        Me.Button_Cancel.UseVisualStyleBackColor = True
        '
        'Form_ManageTaxFlag
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(680, 362)
        Me.Controls.Add(Me.Button_Cancel)
        Me.Controls.Add(Me.Button_Ok)
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "Form_ManageTaxFlag"
        Me.ShowInTaskbar = False
        Me.Text = "Manage Tax Flags"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.UltraGrid_TaxFlag, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Button_Delete As System.Windows.Forms.Button
    Friend WithEvents Button_Edit As System.Windows.Forms.Button
    Friend WithEvents Button_Add As System.Windows.Forms.Button
    Friend WithEvents Button_Ok As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label_TaxClass As System.Windows.Forms.Label
    Friend WithEvents ComboBox_TaxClass As System.Windows.Forms.ComboBox
    Friend WithEvents Label_Jurisdiction As System.Windows.Forms.Label
    Friend WithEvents ComboBox_Jurisdiction As System.Windows.Forms.ComboBox
    Friend WithEvents Button_AddTaxClass As System.Windows.Forms.Button
    Friend WithEvents Button_AddJurisdiction As System.Windows.Forms.Button
    Friend WithEvents Button_Cancel As System.Windows.Forms.Button
    Friend WithEvents UltraGrid_TaxFlag As Infragistics.Win.UltraWinGrid.UltraGrid
End Class
