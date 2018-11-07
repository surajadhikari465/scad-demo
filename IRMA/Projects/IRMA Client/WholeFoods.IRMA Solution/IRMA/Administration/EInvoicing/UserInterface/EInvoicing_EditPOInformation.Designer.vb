<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class EInvoicing_EditPOInformation
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
        Me.components = New System.ComponentModel.Container()
        Dim Appearance25 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridBand1 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1)
        Dim UltraGridColumn1 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("PO", -1, Nothing, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, False)
        Dim UltraGridColumn2 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Source")
        Dim UltraGridColumn3 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Store_Name")
        Dim UltraGridColumn4 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("OrderDate")
        Dim UltraGridColumn5 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("OrderHeaderDesc")
        Dim Appearance26 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance27 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance28 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance29 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance30 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance31 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance32 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance33 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance34 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance35 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance36 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Me.Label_OrignalPO = New System.Windows.Forms.Label()
        Me.Label_VendorName = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Button_Search = New System.Windows.Forms.Button()
        Me.Button_Select = New System.Windows.Forms.Button()
        Me.Button_Cancel = New System.Windows.Forms.Button()
        Me.TextBox_POSearch = New System.Windows.Forms.MaskedTextBox()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.Label_ErrorMessage = New System.Windows.Forms.Label()
        Me.UltraGrid_ValidPurchaseOrders = New Infragistics.Win.UltraWinGrid.UltraGrid()
        Me.Label_Information = New System.Windows.Forms.Label()
        Me.RichTextBox1 = New System.Windows.Forms.RichTextBox()
        Me.GroupBox1.SuspendLayout()
        CType(Me.UltraGrid_ValidPurchaseOrders, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label_OrignalPO
        '
        Me.Label_OrignalPO.AutoSize = True
        Me.Label_OrignalPO.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_OrignalPO.Location = New System.Drawing.Point(6, 16)
        Me.Label_OrignalPO.Name = "Label_OrignalPO"
        Me.Label_OrignalPO.Size = New System.Drawing.Size(36, 13)
        Me.Label_OrignalPO.TabIndex = 0
        Me.Label_OrignalPO.Text = "PO#:"
        '
        'Label_VendorName
        '
        Me.Label_VendorName.AutoSize = True
        Me.Label_VendorName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_VendorName.Location = New System.Drawing.Point(6, 29)
        Me.Label_VendorName.Name = "Label_VendorName"
        Me.Label_VendorName.Size = New System.Drawing.Size(51, 13)
        Me.Label_VendorName.TabIndex = 1
        Me.Label_VendorName.Text = "Vendor:"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label_VendorName)
        Me.GroupBox1.Controls.Add(Me.Label_OrignalPO)
        Me.GroupBox1.Location = New System.Drawing.Point(7, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(322, 54)
        Me.GroupBox1.TabIndex = 2
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Original PO Information"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(385, 48)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(113, 13)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "Enter New PO Number:"
        '
        'Button_Search
        '
        Me.Button_Search.Location = New System.Drawing.Point(600, 41)
        Me.Button_Search.Name = "Button_Search"
        Me.Button_Search.Size = New System.Drawing.Size(85, 26)
        Me.Button_Search.TabIndex = 5
        Me.Button_Search.Text = "S&earch"
        Me.Button_Search.UseVisualStyleBackColor = True
        '
        'Button_Select
        '
        Me.Button_Select.Enabled = False
        Me.Button_Select.Location = New System.Drawing.Point(543, 245)
        Me.Button_Select.Name = "Button_Select"
        Me.Button_Select.Size = New System.Drawing.Size(68, 25)
        Me.Button_Select.TabIndex = 7
        Me.Button_Select.Text = "&Select"
        Me.Button_Select.UseVisualStyleBackColor = True
        '
        'Button_Cancel
        '
        Me.Button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Button_Cancel.Location = New System.Drawing.Point(621, 245)
        Me.Button_Cancel.Name = "Button_Cancel"
        Me.Button_Cancel.Size = New System.Drawing.Size(68, 25)
        Me.Button_Cancel.TabIndex = 8
        Me.Button_Cancel.Text = "&Cancel"
        Me.Button_Cancel.UseVisualStyleBackColor = True
        '
        'TextBox_POSearch
        '
        Me.TextBox_POSearch.HidePromptOnLeave = True
        Me.TextBox_POSearch.Location = New System.Drawing.Point(504, 45)
        Me.TextBox_POSearch.Mask = "0000000000000000"
        Me.TextBox_POSearch.Name = "TextBox_POSearch"
        Me.TextBox_POSearch.Size = New System.Drawing.Size(90, 21)
        Me.TextBox_POSearch.TabIndex = 9
        '
        'Label_ErrorMessage
        '
        Me.Label_ErrorMessage.AutoSize = True
        Me.Label_ErrorMessage.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_ErrorMessage.ForeColor = System.Drawing.Color.Red
        Me.Label_ErrorMessage.Location = New System.Drawing.Point(335, 12)
        Me.Label_ErrorMessage.Name = "Label_ErrorMessage"
        Me.Label_ErrorMessage.Size = New System.Drawing.Size(0, 14)
        Me.Label_ErrorMessage.TabIndex = 10
        '
        'UltraGrid_ValidPurchaseOrders
        '
        Appearance25.BackColor = System.Drawing.SystemColors.Window
        Appearance25.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.UltraGrid_ValidPurchaseOrders.DisplayLayout.Appearance = Appearance25
        Me.UltraGrid_ValidPurchaseOrders.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        UltraGridColumn1.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn1.Header.VisiblePosition = 0
        UltraGridColumn1.Width = 120
        UltraGridColumn2.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn2.Header.VisiblePosition = 1
        UltraGridColumn2.Width = 151
        UltraGridColumn3.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn3.Header.Caption = "Location"
        UltraGridColumn3.Header.VisiblePosition = 2
        UltraGridColumn3.Width = 129
        UltraGridColumn4.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn4.Header.Caption = "Order Date"
        UltraGridColumn4.Header.VisiblePosition = 3
        UltraGridColumn4.Width = 119
        UltraGridColumn5.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn5.Header.Caption = "Order Notes"
        UltraGridColumn5.Header.VisiblePosition = 4
        UltraGridColumn5.Width = 152
        UltraGridBand1.Columns.AddRange(New Object() {UltraGridColumn1, UltraGridColumn2, UltraGridColumn3, UltraGridColumn4, UltraGridColumn5})
        Me.UltraGrid_ValidPurchaseOrders.DisplayLayout.BandsSerializer.Add(UltraGridBand1)
        Me.UltraGrid_ValidPurchaseOrders.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.UltraGrid_ValidPurchaseOrders.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Appearance26.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance26.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance26.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance26.BorderColor = System.Drawing.SystemColors.Window
        Me.UltraGrid_ValidPurchaseOrders.DisplayLayout.GroupByBox.Appearance = Appearance26
        Appearance27.ForeColor = System.Drawing.SystemColors.GrayText
        Me.UltraGrid_ValidPurchaseOrders.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance27
        Me.UltraGrid_ValidPurchaseOrders.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Appearance28.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance28.BackColor2 = System.Drawing.SystemColors.Control
        Appearance28.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance28.ForeColor = System.Drawing.SystemColors.GrayText
        Me.UltraGrid_ValidPurchaseOrders.DisplayLayout.GroupByBox.PromptAppearance = Appearance28
        Me.UltraGrid_ValidPurchaseOrders.DisplayLayout.MaxColScrollRegions = 1
        Me.UltraGrid_ValidPurchaseOrders.DisplayLayout.MaxRowScrollRegions = 1
        Appearance29.BackColor = System.Drawing.SystemColors.Window
        Appearance29.ForeColor = System.Drawing.SystemColors.ControlText
        Me.UltraGrid_ValidPurchaseOrders.DisplayLayout.Override.ActiveCellAppearance = Appearance29
        Appearance30.BackColor = System.Drawing.SystemColors.Highlight
        Appearance30.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.UltraGrid_ValidPurchaseOrders.DisplayLayout.Override.ActiveRowAppearance = Appearance30
        Me.UltraGrid_ValidPurchaseOrders.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No
        Me.UltraGrid_ValidPurchaseOrders.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.NotAllowed
        Me.UltraGrid_ValidPurchaseOrders.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed
        Me.UltraGrid_ValidPurchaseOrders.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.[False]
        Me.UltraGrid_ValidPurchaseOrders.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.[False]
        Me.UltraGrid_ValidPurchaseOrders.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.UltraGrid_ValidPurchaseOrders.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance31.BackColor = System.Drawing.SystemColors.Window
        Me.UltraGrid_ValidPurchaseOrders.DisplayLayout.Override.CardAreaAppearance = Appearance31
        Appearance32.BorderColor = System.Drawing.Color.Silver
        Appearance32.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.UltraGrid_ValidPurchaseOrders.DisplayLayout.Override.CellAppearance = Appearance32
        Me.UltraGrid_ValidPurchaseOrders.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.UltraGrid_ValidPurchaseOrders.DisplayLayout.Override.CellPadding = 0
        Appearance33.BackColor = System.Drawing.SystemColors.Control
        Appearance33.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance33.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance33.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance33.BorderColor = System.Drawing.SystemColors.Window
        Me.UltraGrid_ValidPurchaseOrders.DisplayLayout.Override.GroupByRowAppearance = Appearance33
        Appearance34.TextHAlignAsString = "Left"
        Me.UltraGrid_ValidPurchaseOrders.DisplayLayout.Override.HeaderAppearance = Appearance34
        Me.UltraGrid_ValidPurchaseOrders.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti
        Me.UltraGrid_ValidPurchaseOrders.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance35.BackColor = System.Drawing.SystemColors.Window
        Appearance35.BorderColor = System.Drawing.Color.Silver
        Me.UltraGrid_ValidPurchaseOrders.DisplayLayout.Override.RowAppearance = Appearance35
        Me.UltraGrid_ValidPurchaseOrders.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[False]
        Me.UltraGrid_ValidPurchaseOrders.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed
        Me.UltraGrid_ValidPurchaseOrders.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.UltraGrid_ValidPurchaseOrders.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.UltraGrid_ValidPurchaseOrders.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.[Single]
        Appearance36.BackColor = System.Drawing.SystemColors.ControlLight
        Me.UltraGrid_ValidPurchaseOrders.DisplayLayout.Override.TemplateAddRowAppearance = Appearance36
        Me.UltraGrid_ValidPurchaseOrders.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.UltraGrid_ValidPurchaseOrders.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.UltraGrid_ValidPurchaseOrders.Font = New System.Drawing.Font("Calibri", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.UltraGrid_ValidPurchaseOrders.Location = New System.Drawing.Point(16, 78)
        Me.UltraGrid_ValidPurchaseOrders.Name = "UltraGrid_ValidPurchaseOrders"
        Me.UltraGrid_ValidPurchaseOrders.Size = New System.Drawing.Size(673, 156)
        Me.UltraGrid_ValidPurchaseOrders.TabIndex = 11
        '
        'Label_Information
        '
        Me.Label_Information.AutoSize = True
        Me.Label_Information.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_Information.ForeColor = System.Drawing.Color.Red
        Me.Label_Information.Location = New System.Drawing.Point(13, 245)
        Me.Label_Information.Name = "Label_Information"
        Me.Label_Information.Size = New System.Drawing.Size(0, 14)
        Me.Label_Information.TabIndex = 12
        '
        'RichTextBox1
        '
        Me.RichTextBox1.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.RichTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.RichTextBox1.Enabled = False
        Me.RichTextBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RichTextBox1.Location = New System.Drawing.Point(19, 245)
        Me.RichTextBox1.Name = "RichTextBox1"
        Me.RichTextBox1.Size = New System.Drawing.Size(518, 35)
        Me.RichTextBox1.TabIndex = 13
        Me.RichTextBox1.Text = "New PO must be Open or Closed as 'None' and not already associated with an E-Invo" & _
            "ice."
        '
        'EInvoicing_EditPOInformation
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Button_Cancel
        Me.ClientSize = New System.Drawing.Size(701, 281)
        Me.ControlBox = False
        Me.Controls.Add(Me.RichTextBox1)
        Me.Controls.Add(Me.Label_Information)
        Me.Controls.Add(Me.UltraGrid_ValidPurchaseOrders)
        Me.Controls.Add(Me.Label_ErrorMessage)
        Me.Controls.Add(Me.TextBox_POSearch)
        Me.Controls.Add(Me.Button_Cancel)
        Me.Controls.Add(Me.Button_Select)
        Me.Controls.Add(Me.Button_Search)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.GroupBox1)
        Me.Font = New System.Drawing.Font("Calibri", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MinimizeBox = False
        Me.Name = "EInvoicing_EditPOInformation"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Edit PO Information"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.UltraGrid_ValidPurchaseOrders, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label_OrignalPO As System.Windows.Forms.Label
    Friend WithEvents Label_VendorName As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Button_Search As System.Windows.Forms.Button
    Friend WithEvents Button_Select As System.Windows.Forms.Button
    Friend WithEvents Button_Cancel As System.Windows.Forms.Button
    Friend WithEvents TextBox_POSearch As System.Windows.Forms.MaskedTextBox
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents Label_ErrorMessage As System.Windows.Forms.Label
    Friend WithEvents UltraGrid_ValidPurchaseOrders As Infragistics.Win.UltraWinGrid.UltraGrid
    Friend WithEvents Label_Information As System.Windows.Forms.Label
    Friend WithEvents RichTextBox1 As System.Windows.Forms.RichTextBox

End Class
