<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_ManagePricingMethods
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
        Dim Appearance1 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim UltraGridBand1 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1)
        Dim UltraGridColumn1 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("POSFileWriterKey")
        Dim UltraGridColumn2 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("PricingMethod_Key")
        Dim UltraGridColumn3 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("PricingMethod_ID", -1, Nothing, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, False)
        Dim UltraGridColumn4 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("POSFileWriterCode", 0)
        Dim UltraGridColumn5 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("PricingMethod_Name", 1)
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
        Me.ComboBox_FileWriter = New System.Windows.Forms.ComboBox
        Me.ComboBox_PricingMethod = New System.Windows.Forms.ComboBox
        Me.TextBox_PricingMethodValue = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label_PricingMethod = New System.Windows.Forms.Label
        Me.Label_FileWriter = New System.Windows.Forms.Label
        Me.Button_Add = New System.Windows.Forms.Button
        Me.Button_Close = New System.Windows.Forms.Button
        Me.UltraGrid_PricingMethods = New Infragistics.Win.UltraWinGrid.UltraGrid
        Me.Button_Remove = New System.Windows.Forms.Button
        CType(Me.UltraGrid_PricingMethods, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ComboBox_FileWriter
        '
        Me.ComboBox_FileWriter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_FileWriter.FormattingEnabled = True
        Me.ComboBox_FileWriter.Location = New System.Drawing.Point(174, 12)
        Me.ComboBox_FileWriter.Name = "ComboBox_FileWriter"
        Me.ComboBox_FileWriter.Size = New System.Drawing.Size(148, 21)
        Me.ComboBox_FileWriter.TabIndex = 0
        '
        'ComboBox_PricingMethod
        '
        Me.ComboBox_PricingMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_PricingMethod.FormattingEnabled = True
        Me.ComboBox_PricingMethod.Location = New System.Drawing.Point(174, 50)
        Me.ComboBox_PricingMethod.Name = "ComboBox_PricingMethod"
        Me.ComboBox_PricingMethod.Size = New System.Drawing.Size(148, 21)
        Me.ComboBox_PricingMethod.TabIndex = 1
        '
        'TextBox_PricingMethodValue
        '
        Me.TextBox_PricingMethodValue.Location = New System.Drawing.Point(174, 88)
        Me.TextBox_PricingMethodValue.Name = "TextBox_PricingMethodValue"
        Me.TextBox_PricingMethodValue.Size = New System.Drawing.Size(51, 22)
        Me.TextBox_PricingMethodValue.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(59, 91)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(108, 17)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Pricing Method Value"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label_PricingMethod
        '
        Me.Label_PricingMethod.Location = New System.Drawing.Point(59, 53)
        Me.Label_PricingMethod.Name = "Label_PricingMethod"
        Me.Label_PricingMethod.Size = New System.Drawing.Size(108, 18)
        Me.Label_PricingMethod.TabIndex = 5
        Me.Label_PricingMethod.Text = "Pricing Method"
        Me.Label_PricingMethod.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label_FileWriter
        '
        Me.Label_FileWriter.Location = New System.Drawing.Point(59, 15)
        Me.Label_FileWriter.Name = "Label_FileWriter"
        Me.Label_FileWriter.Size = New System.Drawing.Size(108, 11)
        Me.Label_FileWriter.TabIndex = 6
        Me.Label_FileWriter.Text = "File Writer"
        Me.Label_FileWriter.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Button_Add
        '
        Me.Button_Add.Location = New System.Drawing.Point(102, 129)
        Me.Button_Add.Name = "Button_Add"
        Me.Button_Add.Size = New System.Drawing.Size(75, 23)
        Me.Button_Add.TabIndex = 7
        Me.Button_Add.Text = "Add"
        Me.Button_Add.UseVisualStyleBackColor = True
        '
        'Button_Close
        '
        Me.Button_Close.Location = New System.Drawing.Point(162, 297)
        Me.Button_Close.Name = "Button_Close"
        Me.Button_Close.Size = New System.Drawing.Size(75, 23)
        Me.Button_Close.TabIndex = 8
        Me.Button_Close.Text = "Close"
        Me.Button_Close.UseVisualStyleBackColor = True
        '
        'UltraGrid_PricingMethods
        '
        Appearance1.BackColor = System.Drawing.SystemColors.Window
        Appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.UltraGrid_PricingMethods.DisplayLayout.Appearance = Appearance1
        Me.UltraGrid_PricingMethods.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        UltraGridColumn1.Header.Caption = "File Writer"
        UltraGridColumn1.Header.VisiblePosition = 3
        UltraGridColumn1.Hidden = True
        UltraGridColumn1.Width = 47
        UltraGridColumn2.Header.Caption = "PricingMethod ID"
        UltraGridColumn2.Header.VisiblePosition = 4
        UltraGridColumn2.Hidden = True
        UltraGridColumn2.Width = 59
        UltraGridColumn3.Header.Caption = "POS Pricing Method Value"
        UltraGridColumn3.Header.VisiblePosition = 2
        UltraGridColumn3.Width = 156
        UltraGridColumn4.Header.Caption = "File Writer"
        UltraGridColumn4.Header.VisiblePosition = 0
        UltraGridColumn4.Width = 61
        UltraGridColumn5.Header.Caption = "PricingMethod Type"
        UltraGridColumn5.Header.VisiblePosition = 1
        UltraGridColumn5.Width = 121
        UltraGridBand1.Columns.AddRange(New Object() {UltraGridColumn1, UltraGridColumn2, UltraGridColumn3, UltraGridColumn4, UltraGridColumn5})
        Me.UltraGrid_PricingMethods.DisplayLayout.BandsSerializer.Add(UltraGridBand1)
        Me.UltraGrid_PricingMethods.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.UltraGrid_PricingMethods.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance2.BorderColor = System.Drawing.SystemColors.Window
        Me.UltraGrid_PricingMethods.DisplayLayout.GroupByBox.Appearance = Appearance2
        Appearance3.ForeColor = System.Drawing.SystemColors.GrayText
        Me.UltraGrid_PricingMethods.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance3
        Me.UltraGrid_PricingMethods.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.UltraGrid_PricingMethods.DisplayLayout.GroupByBox.Hidden = True
        Appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance4.BackColor2 = System.Drawing.SystemColors.Control
        Appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance4.ForeColor = System.Drawing.SystemColors.GrayText
        Me.UltraGrid_PricingMethods.DisplayLayout.GroupByBox.PromptAppearance = Appearance4
        Me.UltraGrid_PricingMethods.DisplayLayout.MaxColScrollRegions = 1
        Me.UltraGrid_PricingMethods.DisplayLayout.MaxRowScrollRegions = 1
        Appearance5.BackColor = System.Drawing.SystemColors.Window
        Appearance5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.UltraGrid_PricingMethods.DisplayLayout.Override.ActiveCellAppearance = Appearance5
        Me.UltraGrid_PricingMethods.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No
        Me.UltraGrid_PricingMethods.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.NotAllowed
        Me.UltraGrid_PricingMethods.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.[False]
        Me.UltraGrid_PricingMethods.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.UltraGrid_PricingMethods.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance6.BackColor = System.Drawing.SystemColors.Window
        Me.UltraGrid_PricingMethods.DisplayLayout.Override.CardAreaAppearance = Appearance6
        Appearance7.BorderColor = System.Drawing.Color.Silver
        Appearance7.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.UltraGrid_PricingMethods.DisplayLayout.Override.CellAppearance = Appearance7
        Me.UltraGrid_PricingMethods.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.UltraGrid_PricingMethods.DisplayLayout.Override.CellPadding = 0
        Appearance8.BackColor = System.Drawing.SystemColors.Control
        Appearance8.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance8.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance8.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance8.BorderColor = System.Drawing.SystemColors.Window
        Me.UltraGrid_PricingMethods.DisplayLayout.Override.GroupByRowAppearance = Appearance8
        Appearance9.TextHAlign = Infragistics.Win.HAlign.Left
        Me.UltraGrid_PricingMethods.DisplayLayout.Override.HeaderAppearance = Appearance9
        Me.UltraGrid_PricingMethods.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti
        Me.UltraGrid_PricingMethods.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance10.BackColor = System.Drawing.SystemColors.Window
        Appearance10.BorderColor = System.Drawing.Color.Silver
        Me.UltraGrid_PricingMethods.DisplayLayout.Override.RowAppearance = Appearance10
        Me.UltraGrid_PricingMethods.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[True]
        Me.UltraGrid_PricingMethods.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.UltraGrid_PricingMethods.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.UltraGrid_PricingMethods.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.[Single]
        Appearance11.BackColor = System.Drawing.SystemColors.ControlLight
        Me.UltraGrid_PricingMethods.DisplayLayout.Override.TemplateAddRowAppearance = Appearance11
        Me.UltraGrid_PricingMethods.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.UltraGrid_PricingMethods.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.UltraGrid_PricingMethods.Location = New System.Drawing.Point(17, 169)
        Me.UltraGrid_PricingMethods.Name = "UltraGrid_PricingMethods"
        Me.UltraGrid_PricingMethods.Size = New System.Drawing.Size(359, 110)
        Me.UltraGrid_PricingMethods.TabIndex = 9
        Me.UltraGrid_PricingMethods.Text = "UltraGrid1"
        '
        'Button_Remove
        '
        Me.Button_Remove.Location = New System.Drawing.Point(233, 129)
        Me.Button_Remove.Name = "Button_Remove"
        Me.Button_Remove.Size = New System.Drawing.Size(75, 23)
        Me.Button_Remove.TabIndex = 10
        Me.Button_Remove.Text = "Remove"
        Me.Button_Remove.UseVisualStyleBackColor = True
        '
        'Form_ManagePricingMethods
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.ClientSize = New System.Drawing.Size(399, 332)
        Me.Controls.Add(Me.Button_Remove)
        Me.Controls.Add(Me.UltraGrid_PricingMethods)
        Me.Controls.Add(Me.Button_Close)
        Me.Controls.Add(Me.Button_Add)
        Me.Controls.Add(Me.Label_FileWriter)
        Me.Controls.Add(Me.Label_PricingMethod)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TextBox_PricingMethodValue)
        Me.Controls.Add(Me.ComboBox_PricingMethod)
        Me.Controls.Add(Me.ComboBox_FileWriter)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Form_ManagePricingMethods"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Manage Pricing Methods"
        CType(Me.UltraGrid_PricingMethods, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ComboBox_FileWriter As System.Windows.Forms.ComboBox
    Friend WithEvents ComboBox_PricingMethod As System.Windows.Forms.ComboBox
    Friend WithEvents TextBox_PricingMethodValue As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label_PricingMethod As System.Windows.Forms.Label
    Friend WithEvents Label_FileWriter As System.Windows.Forms.Label
    Friend WithEvents Button_Add As System.Windows.Forms.Button
    Friend WithEvents Button_Close As System.Windows.Forms.Button
    Friend WithEvents UltraGrid_PricingMethods As Infragistics.Win.UltraWinGrid.UltraGrid
    Friend WithEvents Button_Remove As System.Windows.Forms.Button
End Class
