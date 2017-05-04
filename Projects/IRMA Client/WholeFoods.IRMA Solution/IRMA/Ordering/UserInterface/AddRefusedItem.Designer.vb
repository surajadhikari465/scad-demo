<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmAddRefusedItem
#Region "Windows Form Designer generated code "
    <System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
        MyBase.New()
        'This call is required by the Windows Form Designer.
        InitializeComponent()
    End Sub
    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
        If Disposing Then
            If Not components Is Nothing Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(Disposing)
    End Sub
    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer
    Public ToolTip1 As System.Windows.Forms.ToolTip
    Public WithEvents cmdExit As System.Windows.Forms.Button
    Public WithEvents cmdAdd As System.Windows.Forms.Button
    Public WithEvents lblCode As System.Windows.Forms.Label
    Public WithEvents lblIdentifier As System.Windows.Forms.Label
    Public WithEvents cmbField As Microsoft.VisualBasic.Compatibility.VB6.ComboBoxArray
    Public WithEvents lblLabel As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
    Public WithEvents txtField As Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAddRefusedItem))
        Dim Appearance4 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance5 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance6 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance16 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance17 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance18 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance19 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance20 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance21 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance22 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance23 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance24 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdExit = New System.Windows.Forms.Button()
        Me.cmdAdd = New System.Windows.Forms.Button()
        Me.cmdClear = New System.Windows.Forms.Button()
        Me.lblCode = New System.Windows.Forms.Label()
        Me.lblIdentifier = New System.Windows.Forms.Label()
        Me.cmbField = New Microsoft.VisualBasic.Compatibility.VB6.ComboBoxArray(Me.components)
        Me.lblLabel = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(Me.components)
        Me.txtField = New Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray(Me.components)
        Me.txtIdentifier = New System.Windows.Forms.TextBox()
        Me.txtVIN = New System.Windows.Forms.TextBox()
        Me.lblVIN = New System.Windows.Forms.Label()
        Me.txtDescription = New System.Windows.Forms.TextBox()
        Me.lblDescription = New System.Windows.Forms.Label()
        Me.lblInvoiceCost = New System.Windows.Forms.Label()
        Me.lblInvoiceQuantity = New System.Windows.Forms.Label()
        Me.lblUnit = New System.Windows.Forms.Label()
        Me.ucmbReasonCode = New Infragistics.Win.UltraWinGrid.UltraCombo()
        Me.txtInvoiceCost = New Infragistics.Win.UltraWinEditors.UltraNumericEditor()
        Me.txtInvoiceQuantity = New Infragistics.Win.UltraWinEditors.UltraNumericEditor()
        Me.cmbUnits = New System.Windows.Forms.ComboBox()
        Me.GroupBoxItemAttributes = New System.Windows.Forms.GroupBox()
        CType(Me.cmbField, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtField, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ucmbReasonCode, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtInvoiceCost, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtInvoiceQuantity, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBoxItemAttributes.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdExit
        '
        Me.cmdExit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdExit.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Image = CType(resources.GetObject("cmdExit.Image"), System.Drawing.Image)
        Me.cmdExit.Location = New System.Drawing.Point(401, 251)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(41, 41)
        Me.cmdExit.TabIndex = 10
        Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdExit, "Exit")
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'cmdAdd
        '
        Me.cmdAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdAdd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAdd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAdd.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAdd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAdd.Image = CType(resources.GetObject("cmdAdd.Image"), System.Drawing.Image)
        Me.cmdAdd.Location = New System.Drawing.Point(354, 251)
        Me.cmdAdd.Name = "cmdAdd"
        Me.cmdAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAdd.Size = New System.Drawing.Size(41, 41)
        Me.cmdAdd.TabIndex = 9
        Me.cmdAdd.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdAdd, "Add Refused Item")
        Me.cmdAdd.UseVisualStyleBackColor = False
        '
        'cmdClear
        '
        Me.cmdClear.BackColor = System.Drawing.SystemColors.Control
        Me.cmdClear.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdClear.Font = New System.Drawing.Font("Arial", 8.0!)
        Me.cmdClear.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdClear.Image = CType(resources.GetObject("cmdClear.Image"), System.Drawing.Image)
        Me.cmdClear.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.cmdClear.Location = New System.Drawing.Point(307, 251)
        Me.cmdClear.Name = "cmdClear"
        Me.cmdClear.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdClear.Size = New System.Drawing.Size(41, 41)
        Me.cmdClear.TabIndex = 101
        Me.cmdClear.TabStop = False
        Me.cmdClear.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdClear, "Clear Data")
        Me.cmdClear.UseVisualStyleBackColor = False
        '
        'lblCode
        '
        Me.lblCode.BackColor = System.Drawing.Color.Transparent
        Me.lblCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCode.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me.lblCode, CType(4, Short))
        Me.lblCode.Location = New System.Drawing.Point(22, 197)
        Me.lblCode.Name = "lblCode"
        Me.lblCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCode.Size = New System.Drawing.Size(97, 17)
        Me.lblCode.TabIndex = 20
        Me.lblCode.Text = "Reason Code :"
        Me.lblCode.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblIdentifier
        '
        Me.lblIdentifier.BackColor = System.Drawing.Color.Transparent
        Me.lblIdentifier.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblIdentifier.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIdentifier.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me.lblIdentifier, CType(1, Short))
        Me.lblIdentifier.Location = New System.Drawing.Point(30, 35)
        Me.lblIdentifier.Name = "lblIdentifier"
        Me.lblIdentifier.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblIdentifier.Size = New System.Drawing.Size(89, 17)
        Me.lblIdentifier.TabIndex = 11
        Me.lblIdentifier.Text = "Identifier :"
        Me.lblIdentifier.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'txtIdentifier
        '
        Me.txtIdentifier.AcceptsReturn = True
        Me.txtIdentifier.BackColor = System.Drawing.SystemColors.Window
        Me.txtIdentifier.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtIdentifier.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtIdentifier.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtIdentifier.Location = New System.Drawing.Point(125, 32)
        Me.txtIdentifier.MaxLength = 16
        Me.txtIdentifier.Name = "txtIdentifier"
        Me.txtIdentifier.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtIdentifier.Size = New System.Drawing.Size(165, 20)
        Me.txtIdentifier.TabIndex = 1
        Me.txtIdentifier.Tag = "String"
        '
        'txtVIN
        '
        Me.txtVIN.AcceptsReturn = True
        Me.txtVIN.BackColor = System.Drawing.SystemColors.Window
        Me.txtVIN.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtVIN.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtVIN.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtVIN.Location = New System.Drawing.Point(125, 58)
        Me.txtVIN.MaxLength = 16
        Me.txtVIN.Name = "txtVIN"
        Me.txtVIN.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtVIN.Size = New System.Drawing.Size(164, 20)
        Me.txtVIN.TabIndex = 2
        Me.txtVIN.Tag = "String"
        '
        'lblVIN
        '
        Me.lblVIN.BackColor = System.Drawing.Color.Transparent
        Me.lblVIN.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblVIN.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblVIN.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblVIN.Location = New System.Drawing.Point(30, 61)
        Me.lblVIN.Name = "lblVIN"
        Me.lblVIN.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblVIN.Size = New System.Drawing.Size(89, 17)
        Me.lblVIN.TabIndex = 37
        Me.lblVIN.Text = "Vendor ID/VIN :"
        Me.lblVIN.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'txtDescription
        '
        Me.txtDescription.AcceptsReturn = True
        Me.txtDescription.BackColor = System.Drawing.SystemColors.Window
        Me.txtDescription.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDescription.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDescription.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDescription.Location = New System.Drawing.Point(125, 84)
        Me.txtDescription.MaxLength = 16
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDescription.Size = New System.Drawing.Size(289, 20)
        Me.txtDescription.TabIndex = 3
        Me.txtDescription.Tag = "String"
        '
        'lblDescription
        '
        Me.lblDescription.BackColor = System.Drawing.Color.Transparent
        Me.lblDescription.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDescription.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDescription.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDescription.Location = New System.Drawing.Point(30, 87)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDescription.Size = New System.Drawing.Size(89, 17)
        Me.lblDescription.TabIndex = 39
        Me.lblDescription.Text = "Description :"
        Me.lblDescription.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblInvoiceCost
        '
        Me.lblInvoiceCost.BackColor = System.Drawing.Color.Transparent
        Me.lblInvoiceCost.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInvoiceCost.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInvoiceCost.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblInvoiceCost.Location = New System.Drawing.Point(30, 142)
        Me.lblInvoiceCost.Name = "lblInvoiceCost"
        Me.lblInvoiceCost.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInvoiceCost.Size = New System.Drawing.Size(89, 17)
        Me.lblInvoiceCost.TabIndex = 43
        Me.lblInvoiceCost.Text = "Invoice Cost :"
        Me.lblInvoiceCost.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblInvoiceQuantity
        '
        Me.lblInvoiceQuantity.BackColor = System.Drawing.Color.Transparent
        Me.lblInvoiceQuantity.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInvoiceQuantity.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInvoiceQuantity.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblInvoiceQuantity.Location = New System.Drawing.Point(6, 169)
        Me.lblInvoiceQuantity.Name = "lblInvoiceQuantity"
        Me.lblInvoiceQuantity.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInvoiceQuantity.Size = New System.Drawing.Size(113, 17)
        Me.lblInvoiceQuantity.TabIndex = 45
        Me.lblInvoiceQuantity.Text = "Invoice Quantity :"
        Me.lblInvoiceQuantity.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblUnit
        '
        Me.lblUnit.BackColor = System.Drawing.Color.Transparent
        Me.lblUnit.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblUnit.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUnit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblUnit.Location = New System.Drawing.Point(30, 113)
        Me.lblUnit.Name = "lblUnit"
        Me.lblUnit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblUnit.Size = New System.Drawing.Size(89, 17)
        Me.lblUnit.TabIndex = 47
        Me.lblUnit.Text = "UOM :"
        Me.lblUnit.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'ucmbReasonCode
        '
        Me.ucmbReasonCode.CheckedListSettings.CheckStateMember = ""
        Appearance4.BackColor = System.Drawing.SystemColors.Window
        Appearance4.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.ucmbReasonCode.DisplayLayout.Appearance = Appearance4
        Me.ucmbReasonCode.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.ucmbReasonCode.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Appearance5.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance5.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance5.BorderColor = System.Drawing.SystemColors.Window
        Me.ucmbReasonCode.DisplayLayout.GroupByBox.Appearance = Appearance5
        Appearance6.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ucmbReasonCode.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance6
        Me.ucmbReasonCode.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Appearance16.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance16.BackColor2 = System.Drawing.SystemColors.Control
        Appearance16.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance16.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ucmbReasonCode.DisplayLayout.GroupByBox.PromptAppearance = Appearance16
        Me.ucmbReasonCode.DisplayLayout.MaxColScrollRegions = 1
        Me.ucmbReasonCode.DisplayLayout.MaxRowScrollRegions = 1
        Appearance17.BackColor = System.Drawing.SystemColors.Window
        Appearance17.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ucmbReasonCode.DisplayLayout.Override.ActiveCellAppearance = Appearance17
        Appearance18.BackColor = System.Drawing.SystemColors.Highlight
        Appearance18.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.ucmbReasonCode.DisplayLayout.Override.ActiveRowAppearance = Appearance18
        Me.ucmbReasonCode.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.ucmbReasonCode.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance19.BackColor = System.Drawing.SystemColors.Window
        Me.ucmbReasonCode.DisplayLayout.Override.CardAreaAppearance = Appearance19
        Appearance20.BorderColor = System.Drawing.Color.Silver
        Appearance20.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.ucmbReasonCode.DisplayLayout.Override.CellAppearance = Appearance20
        Me.ucmbReasonCode.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.ucmbReasonCode.DisplayLayout.Override.CellPadding = 0
        Appearance21.BackColor = System.Drawing.SystemColors.Control
        Appearance21.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance21.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance21.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance21.BorderColor = System.Drawing.SystemColors.Window
        Me.ucmbReasonCode.DisplayLayout.Override.GroupByRowAppearance = Appearance21
        Appearance22.TextHAlignAsString = "Left"
        Me.ucmbReasonCode.DisplayLayout.Override.HeaderAppearance = Appearance22
        Me.ucmbReasonCode.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti
        Me.ucmbReasonCode.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance23.BackColor = System.Drawing.SystemColors.Window
        Appearance23.BorderColor = System.Drawing.Color.Silver
        Me.ucmbReasonCode.DisplayLayout.Override.RowAppearance = Appearance23
        Me.ucmbReasonCode.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[False]
        Appearance24.BackColor = System.Drawing.SystemColors.ControlLight
        Me.ucmbReasonCode.DisplayLayout.Override.TemplateAddRowAppearance = Appearance24
        Me.ucmbReasonCode.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.ucmbReasonCode.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.ucmbReasonCode.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        Me.ucmbReasonCode.Enabled = False
        Me.ucmbReasonCode.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ucmbReasonCode.Location = New System.Drawing.Point(125, 192)
        Me.ucmbReasonCode.Name = "ucmbReasonCode"
        Me.ucmbReasonCode.Size = New System.Drawing.Size(70, 22)
        Me.ucmbReasonCode.TabIndex = 7
        '
        'txtInvoiceCost
        '
        Me.txtInvoiceCost.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInvoiceCost.Location = New System.Drawing.Point(125, 138)
        Me.txtInvoiceCost.MaskClipMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeBoth
        Me.txtInvoiceCost.MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeLiterals
        Me.txtInvoiceCost.MaskInput = "{double:6.2}"
        Me.txtInvoiceCost.MaxValue = 100000
        Me.txtInvoiceCost.MinValue = 0
        Me.txtInvoiceCost.Name = "txtInvoiceCost"
        Me.txtInvoiceCost.Nullable = True
        Me.txtInvoiceCost.NullText = "0.00"
        Me.txtInvoiceCost.NumericType = Infragistics.Win.UltraWinEditors.NumericType.[Double]
        Me.txtInvoiceCost.PromptChar = Global.Microsoft.VisualBasic.ChrW(32)
        Me.txtInvoiceCost.Size = New System.Drawing.Size(70, 21)
        Me.txtInvoiceCost.TabIndex = 5
        '
        'txtInvoiceQuantity
        '
        Me.txtInvoiceQuantity.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInvoiceQuantity.Location = New System.Drawing.Point(125, 165)
        Me.txtInvoiceQuantity.MaskClipMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeBoth
        Me.txtInvoiceQuantity.MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeLiterals
        Me.txtInvoiceQuantity.MaskInput = "{double:6.2}"
        Me.txtInvoiceQuantity.MaxValue = 100000
        Me.txtInvoiceQuantity.MinValue = 0
        Me.txtInvoiceQuantity.Name = "txtInvoiceQuantity"
        Me.txtInvoiceQuantity.Nullable = True
        Me.txtInvoiceQuantity.NullText = "0.00"
        Me.txtInvoiceQuantity.NumericType = Infragistics.Win.UltraWinEditors.NumericType.[Double]
        Me.txtInvoiceQuantity.PromptChar = Global.Microsoft.VisualBasic.ChrW(32)
        Me.txtInvoiceQuantity.Size = New System.Drawing.Size(70, 21)
        Me.txtInvoiceQuantity.TabIndex = 6
        '
        'cmbUnits
        '
        Me.cmbUnits.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbUnits.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbUnits.BackColor = System.Drawing.SystemColors.Window
        Me.cmbUnits.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbUnits.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbUnits.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbUnits.Items.AddRange(New Object() {"EACH", "CASE", "POUND", "KILOGRAM"})
        Me.cmbUnits.Location = New System.Drawing.Point(125, 110)
        Me.cmbUnits.Name = "cmbUnits"
        Me.cmbUnits.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbUnits.Size = New System.Drawing.Size(115, 22)
        Me.cmbUnits.TabIndex = 4
        '
        'GroupBoxItemAttributes
        '
        Me.GroupBoxItemAttributes.Controls.Add(Me.txtIdentifier)
        Me.GroupBoxItemAttributes.Controls.Add(Me.cmbUnits)
        Me.GroupBoxItemAttributes.Controls.Add(Me.lblIdentifier)
        Me.GroupBoxItemAttributes.Controls.Add(Me.lblCode)
        Me.GroupBoxItemAttributes.Controls.Add(Me.txtInvoiceQuantity)
        Me.GroupBoxItemAttributes.Controls.Add(Me.lblVIN)
        Me.GroupBoxItemAttributes.Controls.Add(Me.txtInvoiceCost)
        Me.GroupBoxItemAttributes.Controls.Add(Me.txtVIN)
        Me.GroupBoxItemAttributes.Controls.Add(Me.ucmbReasonCode)
        Me.GroupBoxItemAttributes.Controls.Add(Me.lblDescription)
        Me.GroupBoxItemAttributes.Controls.Add(Me.lblUnit)
        Me.GroupBoxItemAttributes.Controls.Add(Me.txtDescription)
        Me.GroupBoxItemAttributes.Controls.Add(Me.lblInvoiceQuantity)
        Me.GroupBoxItemAttributes.Controls.Add(Me.lblInvoiceCost)
        Me.GroupBoxItemAttributes.Location = New System.Drawing.Point(13, 13)
        Me.GroupBoxItemAttributes.Name = "GroupBoxItemAttributes"
        Me.GroupBoxItemAttributes.Size = New System.Drawing.Size(429, 231)
        Me.GroupBoxItemAttributes.TabIndex = 104
        Me.GroupBoxItemAttributes.TabStop = False
        Me.GroupBoxItemAttributes.Text = "Item Attributes"
        '
        'frmAddRefusedItem
        '
        Me.AcceptButton = Me.cmdAdd
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdExit
        Me.ClientSize = New System.Drawing.Size(454, 304)
        Me.Controls.Add(Me.GroupBoxItemAttributes)
        Me.Controls.Add(Me.cmdClear)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.cmdAdd)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Location = New System.Drawing.Point(267, 358)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmAddRefusedItem"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Add Refused Item"
        CType(Me.cmbField, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtField, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ucmbReasonCode, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtInvoiceCost, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtInvoiceQuantity, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBoxItemAttributes.ResumeLayout(False)
        Me.GroupBoxItemAttributes.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Public WithEvents txtIdentifier As System.Windows.Forms.TextBox
    Public WithEvents txtVIN As System.Windows.Forms.TextBox
    Public WithEvents lblVIN As System.Windows.Forms.Label
    Public WithEvents txtDescription As System.Windows.Forms.TextBox
    Public WithEvents lblDescription As System.Windows.Forms.Label
    Public WithEvents lblInvoiceCost As System.Windows.Forms.Label
    Public WithEvents lblInvoiceQuantity As System.Windows.Forms.Label
    Public WithEvents lblUnit As System.Windows.Forms.Label
    Friend WithEvents ucmbReasonCode As Infragistics.Win.UltraWinGrid.UltraCombo
    Friend WithEvents txtInvoiceCost As Infragistics.Win.UltraWinEditors.UltraNumericEditor
    Friend WithEvents txtInvoiceQuantity As Infragistics.Win.UltraWinEditors.UltraNumericEditor
    Public WithEvents cmdClear As System.Windows.Forms.Button
    Public WithEvents cmbUnits As System.Windows.Forms.ComboBox
    Friend WithEvents GroupBoxItemAttributes As System.Windows.Forms.GroupBox
#End Region
End Class