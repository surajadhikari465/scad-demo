<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_ManageTaxJurisdiction
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form_ManageTaxJurisdiction))
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
        Me.Button_Ok = New System.Windows.Forms.Button
        Me.Button_Add = New System.Windows.Forms.Button
        Me.Button_Delete = New System.Windows.Forms.Button
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.UltraGrid_TaxJurisdiction = New Infragistics.Win.UltraWinGrid.UltraGrid
        Me.Button_Cancel = New System.Windows.Forms.Button
        Me.GroupBox1.SuspendLayout()
        CType(Me.UltraGrid_TaxJurisdiction, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Button_Ok
        '
        resources.ApplyResources(Me.Button_Ok, "Button_Ok")
        Me.Button_Ok.Name = "Button_Ok"
        Me.Button_Ok.UseVisualStyleBackColor = True
        '
        'Button_Add
        '
        resources.ApplyResources(Me.Button_Add, "Button_Add")
        Me.Button_Add.Name = "Button_Add"
        Me.Button_Add.UseVisualStyleBackColor = True
        '
        'Button_Delete
        '
        resources.ApplyResources(Me.Button_Delete, "Button_Delete")
        Me.Button_Delete.Name = "Button_Delete"
        Me.Button_Delete.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        resources.ApplyResources(Me.GroupBox1, "GroupBox1")
        Me.GroupBox1.Controls.Add(Me.UltraGrid_TaxJurisdiction)
        Me.GroupBox1.Controls.Add(Me.Button_Delete)
        Me.GroupBox1.Controls.Add(Me.Button_Add)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.TabStop = False
        '
        'UltraGrid_TaxJurisdiction
        '
        resources.ApplyResources(Me.UltraGrid_TaxJurisdiction, "UltraGrid_TaxJurisdiction")
        Appearance1.BackColor = System.Drawing.SystemColors.Window
        Appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption
        resources.ApplyResources(Appearance1.FontData, "Appearance1.FontData")
        resources.ApplyResources(Appearance1, "Appearance1")
        Appearance1.ForceApplyResources = "FontData|"
        Me.UltraGrid_TaxJurisdiction.DisplayLayout.Appearance = Appearance1
        Me.UltraGrid_TaxJurisdiction.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        Me.UltraGrid_TaxJurisdiction.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.UltraGrid_TaxJurisdiction.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance2.BorderColor = System.Drawing.SystemColors.Window
        resources.ApplyResources(Appearance2.FontData, "Appearance2.FontData")
        resources.ApplyResources(Appearance2, "Appearance2")
        Appearance2.ForceApplyResources = "FontData|"
        Me.UltraGrid_TaxJurisdiction.DisplayLayout.GroupByBox.Appearance = Appearance2
        Appearance3.ForeColor = System.Drawing.SystemColors.GrayText
        resources.ApplyResources(Appearance3.FontData, "Appearance3.FontData")
        resources.ApplyResources(Appearance3, "Appearance3")
        Appearance3.ForceApplyResources = "FontData|"
        Me.UltraGrid_TaxJurisdiction.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance3
        Me.UltraGrid_TaxJurisdiction.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.UltraGrid_TaxJurisdiction.DisplayLayout.GroupByBox.Hidden = True
        Appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance4.BackColor2 = System.Drawing.SystemColors.Control
        Appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance4.ForeColor = System.Drawing.SystemColors.GrayText
        resources.ApplyResources(Appearance4.FontData, "Appearance4.FontData")
        resources.ApplyResources(Appearance4, "Appearance4")
        Appearance4.ForceApplyResources = "FontData|"
        Me.UltraGrid_TaxJurisdiction.DisplayLayout.GroupByBox.PromptAppearance = Appearance4
        Me.UltraGrid_TaxJurisdiction.DisplayLayout.MaxColScrollRegions = 1
        Me.UltraGrid_TaxJurisdiction.DisplayLayout.MaxRowScrollRegions = 1
        Appearance5.BackColor = System.Drawing.SystemColors.Window
        Appearance5.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Appearance5.FontData, "Appearance5.FontData")
        resources.ApplyResources(Appearance5, "Appearance5")
        Appearance5.ForceApplyResources = "FontData|"
        Me.UltraGrid_TaxJurisdiction.DisplayLayout.Override.ActiveCellAppearance = Appearance5
        Appearance6.BackColor = System.Drawing.SystemColors.Highlight
        Appearance6.ForeColor = System.Drawing.SystemColors.HighlightText
        resources.ApplyResources(Appearance6.FontData, "Appearance6.FontData")
        resources.ApplyResources(Appearance6, "Appearance6")
        Appearance6.ForceApplyResources = "FontData|"
        Me.UltraGrid_TaxJurisdiction.DisplayLayout.Override.ActiveRowAppearance = Appearance6
        Me.UltraGrid_TaxJurisdiction.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.TemplateOnTop
        Me.UltraGrid_TaxJurisdiction.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.[True]
        Me.UltraGrid_TaxJurisdiction.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.[True]
        Me.UltraGrid_TaxJurisdiction.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.UltraGrid_TaxJurisdiction.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance7.BackColor = System.Drawing.SystemColors.Window
        resources.ApplyResources(Appearance7.FontData, "Appearance7.FontData")
        resources.ApplyResources(Appearance7, "Appearance7")
        Appearance7.ForceApplyResources = "FontData|"
        Me.UltraGrid_TaxJurisdiction.DisplayLayout.Override.CardAreaAppearance = Appearance7
        Appearance8.BorderColor = System.Drawing.Color.Silver
        Appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        resources.ApplyResources(Appearance8.FontData, "Appearance8.FontData")
        resources.ApplyResources(Appearance8, "Appearance8")
        Appearance8.ForceApplyResources = "FontData|"
        Me.UltraGrid_TaxJurisdiction.DisplayLayout.Override.CellAppearance = Appearance8
        Me.UltraGrid_TaxJurisdiction.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.UltraGrid_TaxJurisdiction.DisplayLayout.Override.CellPadding = 0
        Appearance9.BackColor = System.Drawing.SystemColors.Control
        Appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance9.BorderColor = System.Drawing.SystemColors.Window
        resources.ApplyResources(Appearance9.FontData, "Appearance9.FontData")
        resources.ApplyResources(Appearance9, "Appearance9")
        Appearance9.ForceApplyResources = "FontData|"
        Me.UltraGrid_TaxJurisdiction.DisplayLayout.Override.GroupByRowAppearance = Appearance9
        resources.ApplyResources(Appearance10, "Appearance10")
        resources.ApplyResources(Appearance10.FontData, "Appearance10.FontData")
        Appearance10.ForceApplyResources = "FontData|"
        Me.UltraGrid_TaxJurisdiction.DisplayLayout.Override.HeaderAppearance = Appearance10
        Me.UltraGrid_TaxJurisdiction.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti
        Me.UltraGrid_TaxJurisdiction.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance11.BackColor = System.Drawing.SystemColors.Window
        Appearance11.BorderColor = System.Drawing.Color.Silver
        resources.ApplyResources(Appearance11.FontData, "Appearance11.FontData")
        resources.ApplyResources(Appearance11, "Appearance11")
        Appearance11.ForceApplyResources = "FontData|"
        Me.UltraGrid_TaxJurisdiction.DisplayLayout.Override.RowAppearance = Appearance11
        Me.UltraGrid_TaxJurisdiction.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[True]
        Me.UltraGrid_TaxJurisdiction.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.[Single]
        Appearance12.BackColor = System.Drawing.SystemColors.ControlLight
        resources.ApplyResources(Appearance12.FontData, "Appearance12.FontData")
        resources.ApplyResources(Appearance12, "Appearance12")
        Appearance12.ForceApplyResources = "FontData|"
        Me.UltraGrid_TaxJurisdiction.DisplayLayout.Override.TemplateAddRowAppearance = Appearance12
        Me.UltraGrid_TaxJurisdiction.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.UltraGrid_TaxJurisdiction.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.UltraGrid_TaxJurisdiction.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        Me.UltraGrid_TaxJurisdiction.Name = "UltraGrid_TaxJurisdiction"
        '
        'Button_Cancel
        '
        resources.ApplyResources(Me.Button_Cancel, "Button_Cancel")
        Me.Button_Cancel.Name = "Button_Cancel"
        Me.Button_Cancel.UseVisualStyleBackColor = True
        '
        'Form_ManageTaxJurisdiction
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.Button_Cancel)
        Me.Controls.Add(Me.Button_Ok)
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "Form_ManageTaxJurisdiction"
        Me.ShowInTaskbar = False
        Me.GroupBox1.ResumeLayout(False)
        CType(Me.UltraGrid_TaxJurisdiction, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Button_Ok As System.Windows.Forms.Button
    Friend WithEvents Button_Add As System.Windows.Forms.Button
    Friend WithEvents Button_Delete As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Button_Cancel As System.Windows.Forms.Button
    Friend WithEvents UltraGrid_TaxJurisdiction As Infragistics.Win.UltraWinGrid.UltraGrid
End Class
