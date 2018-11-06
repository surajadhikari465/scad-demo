<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_InstanceDataFlags
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form_InstanceDataFlags))
        Dim Appearance23 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance24 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance25 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance26 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance27 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance28 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance29 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance30 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance31 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance32 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance33 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance34 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance12 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim UltraGridBand1 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1)
        Dim UltraGridColumn1 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("StoreName", -1, Nothing, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, False)
        Dim UltraGridColumn2 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("FlagValue")
        Dim UltraGridColumn3 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("StoreNo")
        Dim UltraGridColumn4 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("FlagKey")
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
        Me.btn_Close = New System.Windows.Forms.Button
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.ugInstanceDataFlags = New Infragistics.Win.UltraWinGrid.UltraGrid
        Me.Button_OK = New System.Windows.Forms.Button
        Me.GroupBox_CurrentStoreOverrides = New System.Windows.Forms.GroupBox
        Me.Button_Delete = New System.Windows.Forms.Button
        Me.Button_Add = New System.Windows.Forms.Button
        Me.UltraGrid_StoreOverrides = New Infragistics.Win.UltraWinGrid.UltraGrid
        Me.GroupBox2.SuspendLayout()
        CType(Me.ugInstanceDataFlags, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox_CurrentStoreOverrides.SuspendLayout()
        CType(Me.UltraGrid_StoreOverrides, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btn_Close
        '
        Me.btn_Close.Image = CType(resources.GetObject("btn_Close.Image"), System.Drawing.Image)
        Me.btn_Close.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_Close.Location = New System.Drawing.Point(314, 426)
        Me.btn_Close.Name = "btn_Close"
        Me.btn_Close.Size = New System.Drawing.Size(65, 23)
        Me.btn_Close.TabIndex = 17
        Me.btn_Close.Text = "Close"
        Me.btn_Close.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btn_Close.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.ugInstanceDataFlags)
        Me.GroupBox2.Location = New System.Drawing.Point(12, 2)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(367, 206)
        Me.GroupBox2.TabIndex = 16
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Instance Data Flags"
        '
        'ugInstanceDataFlags
        '
        Appearance23.BackColor = System.Drawing.SystemColors.Window
        Appearance23.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.ugInstanceDataFlags.DisplayLayout.Appearance = Appearance23
        Me.ugInstanceDataFlags.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.ugInstanceDataFlags.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Appearance24.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance24.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance24.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance24.BorderColor = System.Drawing.SystemColors.Window
        Me.ugInstanceDataFlags.DisplayLayout.GroupByBox.Appearance = Appearance24
        Appearance25.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugInstanceDataFlags.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance25
        Me.ugInstanceDataFlags.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.ugInstanceDataFlags.DisplayLayout.GroupByBox.Hidden = True
        Appearance26.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance26.BackColor2 = System.Drawing.SystemColors.Control
        Appearance26.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance26.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugInstanceDataFlags.DisplayLayout.GroupByBox.PromptAppearance = Appearance26
        Me.ugInstanceDataFlags.DisplayLayout.MaxColScrollRegions = 1
        Me.ugInstanceDataFlags.DisplayLayout.MaxRowScrollRegions = 1
        Appearance27.BackColor = System.Drawing.SystemColors.Window
        Appearance27.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ugInstanceDataFlags.DisplayLayout.Override.ActiveCellAppearance = Appearance27
        Appearance28.BackColor = System.Drawing.SystemColors.Highlight
        Appearance28.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.ugInstanceDataFlags.DisplayLayout.Override.ActiveRowAppearance = Appearance28
        Me.ugInstanceDataFlags.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.ugInstanceDataFlags.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance29.BackColor = System.Drawing.SystemColors.Window
        Me.ugInstanceDataFlags.DisplayLayout.Override.CardAreaAppearance = Appearance29
        Appearance30.BorderColor = System.Drawing.Color.Silver
        Appearance30.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.ugInstanceDataFlags.DisplayLayout.Override.CellAppearance = Appearance30
        Me.ugInstanceDataFlags.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.ugInstanceDataFlags.DisplayLayout.Override.CellPadding = 0
        Appearance31.BackColor = System.Drawing.SystemColors.Control
        Appearance31.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance31.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance31.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance31.BorderColor = System.Drawing.SystemColors.Window
        Me.ugInstanceDataFlags.DisplayLayout.Override.GroupByRowAppearance = Appearance31
        Appearance32.TextHAlignAsString = "Left"
        Me.ugInstanceDataFlags.DisplayLayout.Override.HeaderAppearance = Appearance32
        Me.ugInstanceDataFlags.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti
        Me.ugInstanceDataFlags.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance33.BackColor = System.Drawing.SystemColors.Window
        Appearance33.BorderColor = System.Drawing.Color.Silver
        Me.ugInstanceDataFlags.DisplayLayout.Override.RowAppearance = Appearance33
        Me.ugInstanceDataFlags.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[False]
        Appearance34.BackColor = System.Drawing.SystemColors.ControlLight
        Me.ugInstanceDataFlags.DisplayLayout.Override.TemplateAddRowAppearance = Appearance34
        Me.ugInstanceDataFlags.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.ugInstanceDataFlags.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.ugInstanceDataFlags.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        Me.ugInstanceDataFlags.Location = New System.Drawing.Point(6, 21)
        Me.ugInstanceDataFlags.Name = "ugInstanceDataFlags"
        Me.ugInstanceDataFlags.Size = New System.Drawing.Size(350, 179)
        Me.ugInstanceDataFlags.TabIndex = 0
        Me.ugInstanceDataFlags.Text = "UltraGrid1"
        '
        'Button_OK
        '
        Me.Button_OK.Image = CType(resources.GetObject("Button_OK.Image"), System.Drawing.Image)
        Me.Button_OK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Button_OK.Location = New System.Drawing.Point(243, 426)
        Me.Button_OK.Name = "Button_OK"
        Me.Button_OK.Size = New System.Drawing.Size(65, 23)
        Me.Button_OK.TabIndex = 15
        Me.Button_OK.Text = "Apply"
        Me.Button_OK.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Button_OK.UseVisualStyleBackColor = True
        '
        'GroupBox_CurrentStoreOverrides
        '
        Me.GroupBox_CurrentStoreOverrides.Controls.Add(Me.Button_Delete)
        Me.GroupBox_CurrentStoreOverrides.Controls.Add(Me.Button_Add)
        Me.GroupBox_CurrentStoreOverrides.Controls.Add(Me.UltraGrid_StoreOverrides)
        Me.GroupBox_CurrentStoreOverrides.Location = New System.Drawing.Point(12, 214)
        Me.GroupBox_CurrentStoreOverrides.Name = "GroupBox_CurrentStoreOverrides"
        Me.GroupBox_CurrentStoreOverrides.Size = New System.Drawing.Size(367, 206)
        Me.GroupBox_CurrentStoreOverrides.TabIndex = 14
        Me.GroupBox_CurrentStoreOverrides.TabStop = False
        Me.GroupBox_CurrentStoreOverrides.Text = "Current Store Overrides"
        '
        'Button_Delete
        '
        Me.Button_Delete.Image = CType(resources.GetObject("Button_Delete.Image"), System.Drawing.Image)
        Me.Button_Delete.Location = New System.Drawing.Point(328, 48)
        Me.Button_Delete.Name = "Button_Delete"
        Me.Button_Delete.Size = New System.Drawing.Size(28, 23)
        Me.Button_Delete.TabIndex = 3
        Me.Button_Delete.UseVisualStyleBackColor = True
        '
        'Button_Add
        '
        Me.Button_Add.Image = CType(resources.GetObject("Button_Add.Image"), System.Drawing.Image)
        Me.Button_Add.Location = New System.Drawing.Point(328, 19)
        Me.Button_Add.Name = "Button_Add"
        Me.Button_Add.Size = New System.Drawing.Size(28, 23)
        Me.Button_Add.TabIndex = 2
        Me.Button_Add.UseVisualStyleBackColor = True
        '
        'UltraGrid_StoreOverrides
        '
        Appearance12.BackColor = System.Drawing.SystemColors.Window
        Appearance12.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.UltraGrid_StoreOverrides.DisplayLayout.Appearance = Appearance12
        Me.UltraGrid_StoreOverrides.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        UltraGridColumn1.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn1.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridColumn1.Header.Caption = "Store Name"
        UltraGridColumn1.Header.VisiblePosition = 0
        UltraGridColumn1.Width = 123
        UltraGridColumn2.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.Edit
        UltraGridColumn2.Header.Caption = "Value"
        UltraGridColumn2.Header.VisiblePosition = 1
        UltraGridColumn2.Width = 172
        UltraGridColumn3.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn3.Header.VisiblePosition = 2
        UltraGridColumn3.Hidden = True
        UltraGridColumn3.Width = 74
        UltraGridColumn4.Header.VisiblePosition = 3
        UltraGridColumn4.Hidden = True
        UltraGridColumn4.Width = 98
        UltraGridBand1.Columns.AddRange(New Object() {UltraGridColumn1, UltraGridColumn2, UltraGridColumn3, UltraGridColumn4})
        Me.UltraGrid_StoreOverrides.DisplayLayout.BandsSerializer.Add(UltraGridBand1)
        Me.UltraGrid_StoreOverrides.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.UltraGrid_StoreOverrides.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Appearance13.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance13.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance13.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance13.BorderColor = System.Drawing.SystemColors.Window
        Me.UltraGrid_StoreOverrides.DisplayLayout.GroupByBox.Appearance = Appearance13
        Appearance14.ForeColor = System.Drawing.SystemColors.GrayText
        Me.UltraGrid_StoreOverrides.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance14
        Me.UltraGrid_StoreOverrides.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.UltraGrid_StoreOverrides.DisplayLayout.GroupByBox.Hidden = True
        Appearance15.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance15.BackColor2 = System.Drawing.SystemColors.Control
        Appearance15.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance15.ForeColor = System.Drawing.SystemColors.GrayText
        Me.UltraGrid_StoreOverrides.DisplayLayout.GroupByBox.PromptAppearance = Appearance15
        Me.UltraGrid_StoreOverrides.DisplayLayout.MaxColScrollRegions = 1
        Me.UltraGrid_StoreOverrides.DisplayLayout.MaxRowScrollRegions = 1
        Appearance16.BackColor = System.Drawing.SystemColors.Window
        Appearance16.ForeColor = System.Drawing.SystemColors.ControlText
        Me.UltraGrid_StoreOverrides.DisplayLayout.Override.ActiveCellAppearance = Appearance16
        Me.UltraGrid_StoreOverrides.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No
        Me.UltraGrid_StoreOverrides.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.NotAllowed
        Me.UltraGrid_StoreOverrides.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.UltraGrid_StoreOverrides.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance17.BackColor = System.Drawing.SystemColors.Window
        Me.UltraGrid_StoreOverrides.DisplayLayout.Override.CardAreaAppearance = Appearance17
        Appearance18.BorderColor = System.Drawing.Color.Silver
        Appearance18.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.UltraGrid_StoreOverrides.DisplayLayout.Override.CellAppearance = Appearance18
        Me.UltraGrid_StoreOverrides.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.UltraGrid_StoreOverrides.DisplayLayout.Override.CellPadding = 0
        Appearance19.BackColor = System.Drawing.SystemColors.Control
        Appearance19.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance19.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance19.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance19.BorderColor = System.Drawing.SystemColors.Window
        Me.UltraGrid_StoreOverrides.DisplayLayout.Override.GroupByRowAppearance = Appearance19
        Appearance20.TextHAlignAsString = "Left"
        Me.UltraGrid_StoreOverrides.DisplayLayout.Override.HeaderAppearance = Appearance20
        Me.UltraGrid_StoreOverrides.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti
        Me.UltraGrid_StoreOverrides.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance21.BackColor = System.Drawing.SystemColors.Window
        Appearance21.BorderColor = System.Drawing.Color.Silver
        Me.UltraGrid_StoreOverrides.DisplayLayout.Override.RowAppearance = Appearance21
        Me.UltraGrid_StoreOverrides.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[True]
        Me.UltraGrid_StoreOverrides.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.UltraGrid_StoreOverrides.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.UltraGrid_StoreOverrides.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.[Single]
        Appearance22.BackColor = System.Drawing.SystemColors.ControlLight
        Me.UltraGrid_StoreOverrides.DisplayLayout.Override.TemplateAddRowAppearance = Appearance22
        Me.UltraGrid_StoreOverrides.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.UltraGrid_StoreOverrides.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.UltraGrid_StoreOverrides.Location = New System.Drawing.Point(6, 19)
        Me.UltraGrid_StoreOverrides.Name = "UltraGrid_StoreOverrides"
        Me.UltraGrid_StoreOverrides.Size = New System.Drawing.Size(316, 179)
        Me.UltraGrid_StoreOverrides.TabIndex = 1
        Me.UltraGrid_StoreOverrides.Text = "UltraGrid1"
        '
        'Form_InstanceDataFlags
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(390, 457)
        Me.Controls.Add(Me.btn_Close)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.Button_OK)
        Me.Controls.Add(Me.GroupBox_CurrentStoreOverrides)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Form_InstanceDataFlags"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.Text = "Configure Instance Data Flags"
        Me.GroupBox2.ResumeLayout(False)
        CType(Me.ugInstanceDataFlags, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox_CurrentStoreOverrides.ResumeLayout(False)
        CType(Me.UltraGrid_StoreOverrides, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btn_Close As System.Windows.Forms.Button
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents ugInstanceDataFlags As Infragistics.Win.UltraWinGrid.UltraGrid
    Friend WithEvents Button_OK As System.Windows.Forms.Button
    Friend WithEvents GroupBox_CurrentStoreOverrides As System.Windows.Forms.GroupBox
    Friend WithEvents Button_Delete As System.Windows.Forms.Button
    Friend WithEvents Button_Add As System.Windows.Forms.Button
    Friend WithEvents UltraGrid_StoreOverrides As Infragistics.Win.UltraWinGrid.UltraGrid
End Class
