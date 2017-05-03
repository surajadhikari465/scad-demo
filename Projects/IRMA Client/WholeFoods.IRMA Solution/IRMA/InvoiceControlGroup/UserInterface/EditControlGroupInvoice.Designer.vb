<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class EditControlGroupInvoice
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(EditControlGroupInvoice))
        Me.GroupBox_DocumentType = New System.Windows.Forms.GroupBox
        Me.RadioButton_DocumentType_3rdPartyFreight = New System.Windows.Forms.RadioButton
        Me.RadioButton_DocumentType_Invoice = New System.Windows.Forms.RadioButton
        Me.TextBox_InvoiceTotal = New System.Windows.Forms.TextBox
        Me.TextBox_InvoiceNum = New System.Windows.Forms.TextBox
        Me.TextBox_TotalInvoiceCost = New System.Windows.Forms.TextBox
        Me.Button_Exit = New System.Windows.Forms.Button
        Me.Label_InvFrghtTot = New System.Windows.Forms.Label
        Me.Label_InvoiceTotal = New System.Windows.Forms.Label
        Me.Label_CostSubtotal = New System.Windows.Forms.Label
        Me.Label_InvoiceNum = New System.Windows.Forms.Label
        Me.Label_Date = New System.Windows.Forms.Label
        Me.Label_CostTotal = New System.Windows.Forms.Label
        Me.UltraDateTime_InvoiceDate = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
        Me.Label_InvoiceCost = New System.Windows.Forms.Label
        Me.Label_PONum = New System.Windows.Forms.Label
        Me.TextBox_VendorKey = New System.Windows.Forms.TextBox
        Me.Label_VendorKey = New System.Windows.Forms.Label
        Me.Button_SearchVendor = New System.Windows.Forms.Button
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.Button_SearchOrder = New System.Windows.Forms.Button
        Me.CheckBox_CreditInvoice = New System.Windows.Forms.CheckBox
        Me.UltraNumericEditor_PONum = New Infragistics.Win.UltraWinEditors.UltraNumericEditor
        Me.UltraNumericEditor_InvFrghtTot = New Infragistics.Win.UltraWinEditors.UltraNumericEditor
        Me.UltraNumericEditor_InvoiceCost = New Infragistics.Win.UltraWinEditors.UltraNumericEditor
        Me.Label_VendorID = New System.Windows.Forms.Label
        Me.UltraNumericEditor_VendorID = New Infragistics.Win.UltraWinEditors.UltraNumericEditor
        Me.GroupBox_DocumentType.SuspendLayout()
        CType(Me.UltraDateTime_InvoiceDate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.UltraNumericEditor_PONum, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.UltraNumericEditor_InvFrghtTot, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.UltraNumericEditor_InvoiceCost, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.UltraNumericEditor_VendorID, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GroupBox_DocumentType
        '
        Me.GroupBox_DocumentType.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox_DocumentType.Controls.Add(Me.RadioButton_DocumentType_3rdPartyFreight)
        Me.GroupBox_DocumentType.Controls.Add(Me.RadioButton_DocumentType_Invoice)
        Me.GroupBox_DocumentType.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox_DocumentType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBox_DocumentType.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox_DocumentType.Name = "GroupBox_DocumentType"
        Me.GroupBox_DocumentType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBox_DocumentType.Size = New System.Drawing.Size(258, 45)
        Me.GroupBox_DocumentType.TabIndex = 1
        Me.GroupBox_DocumentType.TabStop = False
        Me.GroupBox_DocumentType.Text = "Document Type"
        '
        'RadioButton_DocumentType_3rdPartyFreight
        '
        Me.RadioButton_DocumentType_3rdPartyFreight.BackColor = System.Drawing.SystemColors.Control
        Me.RadioButton_DocumentType_3rdPartyFreight.Cursor = System.Windows.Forms.Cursors.Default
        Me.RadioButton_DocumentType_3rdPartyFreight.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioButton_DocumentType_3rdPartyFreight.ForeColor = System.Drawing.SystemColors.ControlText
        Me.RadioButton_DocumentType_3rdPartyFreight.Location = New System.Drawing.Point(94, 16)
        Me.RadioButton_DocumentType_3rdPartyFreight.Name = "RadioButton_DocumentType_3rdPartyFreight"
        Me.RadioButton_DocumentType_3rdPartyFreight.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.RadioButton_DocumentType_3rdPartyFreight.Size = New System.Drawing.Size(135, 20)
        Me.RadioButton_DocumentType_3rdPartyFreight.TabIndex = 2
        Me.RadioButton_DocumentType_3rdPartyFreight.Text = "3rd Party Freight Inv"
        Me.RadioButton_DocumentType_3rdPartyFreight.UseVisualStyleBackColor = False
        '
        'RadioButton_DocumentType_Invoice
        '
        Me.RadioButton_DocumentType_Invoice.BackColor = System.Drawing.SystemColors.Control
        Me.RadioButton_DocumentType_Invoice.Checked = True
        Me.RadioButton_DocumentType_Invoice.Cursor = System.Windows.Forms.Cursors.Default
        Me.RadioButton_DocumentType_Invoice.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioButton_DocumentType_Invoice.ForeColor = System.Drawing.SystemColors.ControlText
        Me.RadioButton_DocumentType_Invoice.Location = New System.Drawing.Point(6, 16)
        Me.RadioButton_DocumentType_Invoice.Name = "RadioButton_DocumentType_Invoice"
        Me.RadioButton_DocumentType_Invoice.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.RadioButton_DocumentType_Invoice.Size = New System.Drawing.Size(66, 17)
        Me.RadioButton_DocumentType_Invoice.TabIndex = 1
        Me.RadioButton_DocumentType_Invoice.TabStop = True
        Me.RadioButton_DocumentType_Invoice.Text = "Invoice"
        Me.RadioButton_DocumentType_Invoice.UseVisualStyleBackColor = False
        '
        'TextBox_InvoiceTotal
        '
        Me.TextBox_InvoiceTotal.BackColor = System.Drawing.SystemColors.ControlLight
        Me.TextBox_InvoiceTotal.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextBox_InvoiceTotal.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox_InvoiceTotal.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextBox_InvoiceTotal.Location = New System.Drawing.Point(150, 182)
        Me.TextBox_InvoiceTotal.MaxLength = 0
        Me.TextBox_InvoiceTotal.Name = "TextBox_InvoiceTotal"
        Me.TextBox_InvoiceTotal.ReadOnly = True
        Me.TextBox_InvoiceTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextBox_InvoiceTotal.Size = New System.Drawing.Size(85, 20)
        Me.TextBox_InvoiceTotal.TabIndex = 100
        Me.TextBox_InvoiceTotal.TabStop = False
        Me.TextBox_InvoiceTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextBox_InvoiceNum
        '
        Me.TextBox_InvoiceNum.AcceptsReturn = True
        Me.TextBox_InvoiceNum.BackColor = System.Drawing.SystemColors.Window
        Me.TextBox_InvoiceNum.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextBox_InvoiceNum.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox_InvoiceNum.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextBox_InvoiceNum.Location = New System.Drawing.Point(150, 134)
        Me.TextBox_InvoiceNum.MaxLength = 16
        Me.TextBox_InvoiceNum.Name = "TextBox_InvoiceNum"
        Me.TextBox_InvoiceNum.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextBox_InvoiceNum.Size = New System.Drawing.Size(121, 20)
        Me.TextBox_InvoiceNum.TabIndex = 6
        Me.TextBox_InvoiceNum.Tag = "String"
        '
        'TextBox_TotalInvoiceCost
        '
        Me.TextBox_TotalInvoiceCost.AcceptsReturn = True
        Me.TextBox_TotalInvoiceCost.BackColor = System.Drawing.SystemColors.ControlLight
        Me.TextBox_TotalInvoiceCost.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextBox_TotalInvoiceCost.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox_TotalInvoiceCost.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextBox_TotalInvoiceCost.Location = New System.Drawing.Point(150, 282)
        Me.TextBox_TotalInvoiceCost.MaxLength = 0
        Me.TextBox_TotalInvoiceCost.Name = "TextBox_TotalInvoiceCost"
        Me.TextBox_TotalInvoiceCost.ReadOnly = True
        Me.TextBox_TotalInvoiceCost.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextBox_TotalInvoiceCost.Size = New System.Drawing.Size(85, 20)
        Me.TextBox_TotalInvoiceCost.TabIndex = 103
        Me.TextBox_TotalInvoiceCost.TabStop = False
        Me.TextBox_TotalInvoiceCost.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Button_Exit
        '
        Me.Button_Exit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Exit.BackColor = System.Drawing.SystemColors.Control
        Me.Button_Exit.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button_Exit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Button_Exit.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button_Exit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button_Exit.Image = CType(resources.GetObject("Button_Exit.Image"), System.Drawing.Image)
        Me.Button_Exit.Location = New System.Drawing.Point(296, 319)
        Me.Button_Exit.Name = "Button_Exit"
        Me.Button_Exit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button_Exit.Size = New System.Drawing.Size(41, 41)
        Me.Button_Exit.TabIndex = 12
        Me.Button_Exit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.Button_Exit.UseVisualStyleBackColor = False
        '
        'Label_InvFrghtTot
        '
        Me.Label_InvFrghtTot.BackColor = System.Drawing.Color.Transparent
        Me.Label_InvFrghtTot.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label_InvFrghtTot.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_InvFrghtTot.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label_InvFrghtTot.Location = New System.Drawing.Point(54, 209)
        Me.Label_InvFrghtTot.Name = "Label_InvFrghtTot"
        Me.Label_InvFrghtTot.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label_InvFrghtTot.Size = New System.Drawing.Size(89, 17)
        Me.Label_InvFrghtTot.TabIndex = 110
        Me.Label_InvFrghtTot.Text = "Freight Total :"
        Me.Label_InvFrghtTot.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me.ToolTip1.SetToolTip(Me.Label_InvFrghtTot, "Invoice Freight, " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Not 3rd Party Freight")
        '
        'Label_InvoiceTotal
        '
        Me.Label_InvoiceTotal.BackColor = System.Drawing.Color.Transparent
        Me.Label_InvoiceTotal.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label_InvoiceTotal.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_InvoiceTotal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label_InvoiceTotal.Location = New System.Drawing.Point(54, 185)
        Me.Label_InvoiceTotal.Name = "Label_InvoiceTotal"
        Me.Label_InvoiceTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label_InvoiceTotal.Size = New System.Drawing.Size(89, 17)
        Me.Label_InvoiceTotal.TabIndex = 109
        Me.Label_InvoiceTotal.Text = "Invoice Total :"
        Me.Label_InvoiceTotal.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me.ToolTip1.SetToolTip(Me.Label_InvoiceTotal, "Sum of Freight Total & Cost Total")
        '
        'Label_CostSubtotal
        '
        Me.Label_CostSubtotal.BackColor = System.Drawing.Color.Transparent
        Me.Label_CostSubtotal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label_CostSubtotal.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label_CostSubtotal.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_CostSubtotal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label_CostSubtotal.Location = New System.Drawing.Point(150, 234)
        Me.Label_CostSubtotal.Name = "Label_CostSubtotal"
        Me.Label_CostSubtotal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label_CostSubtotal.Size = New System.Drawing.Size(85, 17)
        Me.Label_CostSubtotal.TabIndex = 107
        Me.Label_CostSubtotal.Text = "Cost Subtotal"
        Me.Label_CostSubtotal.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Label_InvoiceNum
        '
        Me.Label_InvoiceNum.BackColor = System.Drawing.Color.Transparent
        Me.Label_InvoiceNum.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label_InvoiceNum.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_InvoiceNum.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label_InvoiceNum.Location = New System.Drawing.Point(54, 137)
        Me.Label_InvoiceNum.Name = "Label_InvoiceNum"
        Me.Label_InvoiceNum.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label_InvoiceNum.Size = New System.Drawing.Size(89, 17)
        Me.Label_InvoiceNum.TabIndex = 106
        Me.Label_InvoiceNum.Text = "Invoice # :"
        Me.Label_InvoiceNum.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label_Date
        '
        Me.Label_Date.BackColor = System.Drawing.Color.Transparent
        Me.Label_Date.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label_Date.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_Date.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label_Date.Location = New System.Drawing.Point(54, 162)
        Me.Label_Date.Name = "Label_Date"
        Me.Label_Date.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label_Date.Size = New System.Drawing.Size(89, 17)
        Me.Label_Date.TabIndex = 105
        Me.Label_Date.Text = " Date :"
        Me.Label_Date.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label_CostTotal
        '
        Me.Label_CostTotal.BackColor = System.Drawing.Color.Transparent
        Me.Label_CostTotal.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label_CostTotal.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_CostTotal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label_CostTotal.Location = New System.Drawing.Point(54, 285)
        Me.Label_CostTotal.Name = "Label_CostTotal"
        Me.Label_CostTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label_CostTotal.Size = New System.Drawing.Size(89, 17)
        Me.Label_CostTotal.TabIndex = 104
        Me.Label_CostTotal.Text = "Cost Total :"
        Me.Label_CostTotal.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me.ToolTip1.SetToolTip(Me.Label_CostTotal, "Sum of Cost Entries")
        '
        'UltraDateTime_InvoiceDate
        '
        Me.UltraDateTime_InvoiceDate.DateTime = New Date(1980, 1, 1, 0, 0, 0, 0)
        Me.UltraDateTime_InvoiceDate.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.UltraDateTime_InvoiceDate.Location = New System.Drawing.Point(150, 158)
        Me.UltraDateTime_InvoiceDate.MaskInput = ""
        Me.UltraDateTime_InvoiceDate.MaxDate = New Date(2099, 12, 31, 0, 0, 0, 0)
        Me.UltraDateTime_InvoiceDate.MinDate = New Date(1980, 1, 1, 0, 0, 0, 0)
        Me.UltraDateTime_InvoiceDate.Name = "UltraDateTime_InvoiceDate"
        Me.UltraDateTime_InvoiceDate.Size = New System.Drawing.Size(85, 21)
        Me.UltraDateTime_InvoiceDate.TabIndex = 7
        Me.UltraDateTime_InvoiceDate.Value = Nothing
        '
        'Label_InvoiceCost
        '
        Me.Label_InvoiceCost.BackColor = System.Drawing.Color.Transparent
        Me.Label_InvoiceCost.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label_InvoiceCost.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_InvoiceCost.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label_InvoiceCost.Location = New System.Drawing.Point(4, 250)
        Me.Label_InvoiceCost.Name = "Label_InvoiceCost"
        Me.Label_InvoiceCost.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label_InvoiceCost.Size = New System.Drawing.Size(140, 17)
        Me.Label_InvoiceCost.TabIndex = 112
        Me.Label_InvoiceCost.Text = "Cost Subtotal :"
        Me.Label_InvoiceCost.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label_PONum
        '
        Me.Label_PONum.BackColor = System.Drawing.Color.Transparent
        Me.Label_PONum.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label_PONum.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_PONum.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label_PONum.Location = New System.Drawing.Point(29, 113)
        Me.Label_PONum.Name = "Label_PONum"
        Me.Label_PONum.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label_PONum.Size = New System.Drawing.Size(114, 20)
        Me.Label_PONum.TabIndex = 114
        Me.Label_PONum.Text = "Purchase Order # :"
        Me.Label_PONum.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'TextBox_VendorKey
        '
        Me.TextBox_VendorKey.AcceptsReturn = True
        Me.TextBox_VendorKey.BackColor = System.Drawing.SystemColors.Window
        Me.TextBox_VendorKey.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextBox_VendorKey.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox_VendorKey.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextBox_VendorKey.Location = New System.Drawing.Point(149, 60)
        Me.TextBox_VendorKey.MaxLength = 10
        Me.TextBox_VendorKey.Name = "TextBox_VendorKey"
        Me.TextBox_VendorKey.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextBox_VendorKey.Size = New System.Drawing.Size(121, 20)
        Me.TextBox_VendorKey.TabIndex = 3
        Me.TextBox_VendorKey.Tag = "String"
        '
        'Label_VendorKey
        '
        Me.Label_VendorKey.BackColor = System.Drawing.Color.Transparent
        Me.Label_VendorKey.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label_VendorKey.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_VendorKey.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label_VendorKey.Location = New System.Drawing.Point(29, 63)
        Me.Label_VendorKey.Name = "Label_VendorKey"
        Me.Label_VendorKey.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label_VendorKey.Size = New System.Drawing.Size(114, 20)
        Me.Label_VendorKey.TabIndex = 116
        Me.Label_VendorKey.Text = "Vendor Key :"
        Me.Label_VendorKey.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Button_SearchVendor
        '
        Me.Button_SearchVendor.BackColor = System.Drawing.Color.Silver
        Me.Button_SearchVendor.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button_SearchVendor.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button_SearchVendor.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button_SearchVendor.Image = CType(resources.GetObject("Button_SearchVendor.Image"), System.Drawing.Image)
        Me.Button_SearchVendor.Location = New System.Drawing.Point(276, 63)
        Me.Button_SearchVendor.Name = "Button_SearchVendor"
        Me.Button_SearchVendor.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button_SearchVendor.Size = New System.Drawing.Size(29, 20)
        Me.Button_SearchVendor.TabIndex = 117
        Me.Button_SearchVendor.TabStop = False
        Me.Button_SearchVendor.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.Button_SearchVendor, "Search For Vendor")
        Me.Button_SearchVendor.UseVisualStyleBackColor = False
        '
        'Button_SearchOrder
        '
        Me.Button_SearchOrder.BackColor = System.Drawing.Color.Silver
        Me.Button_SearchOrder.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button_SearchOrder.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button_SearchOrder.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button_SearchOrder.Image = CType(resources.GetObject("Button_SearchOrder.Image"), System.Drawing.Image)
        Me.Button_SearchOrder.Location = New System.Drawing.Point(277, 111)
        Me.Button_SearchOrder.Name = "Button_SearchOrder"
        Me.Button_SearchOrder.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button_SearchOrder.Size = New System.Drawing.Size(29, 20)
        Me.Button_SearchOrder.TabIndex = 118
        Me.Button_SearchOrder.TabStop = False
        Me.Button_SearchOrder.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.Button_SearchOrder, "Search For Order")
        Me.Button_SearchOrder.UseVisualStyleBackColor = False
        '
        'CheckBox_CreditInvoice
        '
        Me.CheckBox_CreditInvoice.AutoSize = True
        Me.CheckBox_CreditInvoice.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.CheckBox_CreditInvoice.Location = New System.Drawing.Point(247, 185)
        Me.CheckBox_CreditInvoice.Name = "CheckBox_CreditInvoice"
        Me.CheckBox_CreditInvoice.Size = New System.Drawing.Size(102, 18)
        Me.CheckBox_CreditInvoice.TabIndex = 8
        Me.CheckBox_CreditInvoice.Text = "Credit Invoice"
        Me.CheckBox_CreditInvoice.UseVisualStyleBackColor = True
        '
        'UltraNumericEditor_PONum
        '
        Me.UltraNumericEditor_PONum.AlwaysInEditMode = True
        Me.UltraNumericEditor_PONum.Location = New System.Drawing.Point(149, 110)
        Me.UltraNumericEditor_PONum.MaxValue = 1999999999
        Me.UltraNumericEditor_PONum.MinValue = 1
        Me.UltraNumericEditor_PONum.Name = "UltraNumericEditor_PONum"
        Me.UltraNumericEditor_PONum.Nullable = True
        Me.UltraNumericEditor_PONum.PromptChar = Global.Microsoft.VisualBasic.ChrW(32)
        Me.UltraNumericEditor_PONum.Size = New System.Drawing.Size(122, 21)
        Me.UltraNumericEditor_PONum.TabIndex = 5
        Me.UltraNumericEditor_PONum.Value = Nothing
        '
        'UltraNumericEditor_InvFrghtTot
        '
        Me.UltraNumericEditor_InvFrghtTot.AlwaysInEditMode = True
        Me.UltraNumericEditor_InvFrghtTot.Location = New System.Drawing.Point(149, 206)
        Me.UltraNumericEditor_InvFrghtTot.MaskInput = "{LOC}nnnnnnn.nn"
        Me.UltraNumericEditor_InvFrghtTot.MaxValue = 922337203685477.62
        Me.UltraNumericEditor_InvFrghtTot.MinValue = 0
        Me.UltraNumericEditor_InvFrghtTot.Name = "UltraNumericEditor_InvFrghtTot"
        Me.UltraNumericEditor_InvFrghtTot.Nullable = True
        Me.UltraNumericEditor_InvFrghtTot.NullText = "0"
        Me.UltraNumericEditor_InvFrghtTot.NumericType = Infragistics.Win.UltraWinEditors.NumericType.[Double]
        Me.UltraNumericEditor_InvFrghtTot.PromptChar = Global.Microsoft.VisualBasic.ChrW(32)
        Me.UltraNumericEditor_InvFrghtTot.Size = New System.Drawing.Size(86, 21)
        Me.UltraNumericEditor_InvFrghtTot.TabIndex = 9
        '
        'UltraNumericEditor_InvoiceCost
        '
        Me.UltraNumericEditor_InvoiceCost.AlwaysInEditMode = True
        Me.UltraNumericEditor_InvoiceCost.Location = New System.Drawing.Point(149, 250)
        Me.UltraNumericEditor_InvoiceCost.MaskInput = "{LOC}nnnnnnn.nn"
        Me.UltraNumericEditor_InvoiceCost.MaxValue = 922337203685477.62
        Me.UltraNumericEditor_InvoiceCost.MinValue = 0
        Me.UltraNumericEditor_InvoiceCost.Name = "UltraNumericEditor_InvoiceCost"
        Me.UltraNumericEditor_InvoiceCost.Nullable = True
        Me.UltraNumericEditor_InvoiceCost.NullText = "0"
        Me.UltraNumericEditor_InvoiceCost.NumericType = Infragistics.Win.UltraWinEditors.NumericType.[Double]
        Me.UltraNumericEditor_InvoiceCost.PromptChar = Global.Microsoft.VisualBasic.ChrW(32)
        Me.UltraNumericEditor_InvoiceCost.Size = New System.Drawing.Size(86, 21)
        Me.UltraNumericEditor_InvoiceCost.TabIndex = 10
        '
        'Label_VendorID
        '
        Me.Label_VendorID.AutoSize = True
        Me.Label_VendorID.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.Label_VendorID.Location = New System.Drawing.Point(76, 87)
        Me.Label_VendorID.Name = "Label_VendorID"
        Me.Label_VendorID.Size = New System.Drawing.Size(67, 14)
        Me.Label_VendorID.TabIndex = 125
        Me.Label_VendorID.Text = "Vendor ID :"
        '
        'UltraNumericEditor_VendorID
        '
        Me.UltraNumericEditor_VendorID.Location = New System.Drawing.Point(149, 83)
        Me.UltraNumericEditor_VendorID.MaskInput = "nnnnnnnnn"
        Me.UltraNumericEditor_VendorID.MinValue = 0
        Me.UltraNumericEditor_VendorID.Name = "UltraNumericEditor_VendorID"
        Me.UltraNumericEditor_VendorID.Nullable = True
        Me.UltraNumericEditor_VendorID.Size = New System.Drawing.Size(121, 21)
        Me.UltraNumericEditor_VendorID.TabIndex = 4
        Me.UltraNumericEditor_VendorID.Value = Nothing
        '
        'EditControlGroupInvoice
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(349, 372)
        Me.Controls.Add(Me.UltraNumericEditor_VendorID)
        Me.Controls.Add(Me.Label_VendorID)
        Me.Controls.Add(Me.UltraNumericEditor_InvoiceCost)
        Me.Controls.Add(Me.UltraNumericEditor_InvFrghtTot)
        Me.Controls.Add(Me.UltraNumericEditor_PONum)
        Me.Controls.Add(Me.CheckBox_CreditInvoice)
        Me.Controls.Add(Me.Button_SearchOrder)
        Me.Controls.Add(Me.Button_SearchVendor)
        Me.Controls.Add(Me.TextBox_VendorKey)
        Me.Controls.Add(Me.Label_VendorKey)
        Me.Controls.Add(Me.Label_PONum)
        Me.Controls.Add(Me.Label_InvoiceCost)
        Me.Controls.Add(Me.GroupBox_DocumentType)
        Me.Controls.Add(Me.TextBox_InvoiceTotal)
        Me.Controls.Add(Me.TextBox_InvoiceNum)
        Me.Controls.Add(Me.TextBox_TotalInvoiceCost)
        Me.Controls.Add(Me.Button_Exit)
        Me.Controls.Add(Me.Label_InvFrghtTot)
        Me.Controls.Add(Me.Label_InvoiceTotal)
        Me.Controls.Add(Me.Label_CostSubtotal)
        Me.Controls.Add(Me.Label_InvoiceNum)
        Me.Controls.Add(Me.Label_Date)
        Me.Controls.Add(Me.Label_CostTotal)
        Me.Controls.Add(Me.UltraDateTime_InvoiceDate)
        Me.Name = "EditControlGroupInvoice"
        Me.Text = "Control Group Invoice Data"
        Me.GroupBox_DocumentType.ResumeLayout(False)
        CType(Me.UltraDateTime_InvoiceDate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.UltraNumericEditor_PONum, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.UltraNumericEditor_InvFrghtTot, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.UltraNumericEditor_InvoiceCost, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.UltraNumericEditor_VendorID, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Public WithEvents GroupBox_DocumentType As System.Windows.Forms.GroupBox
    Public WithEvents RadioButton_DocumentType_Invoice As System.Windows.Forms.RadioButton
    Public WithEvents TextBox_InvoiceTotal As System.Windows.Forms.TextBox
    Public WithEvents TextBox_InvoiceNum As System.Windows.Forms.TextBox
    Public WithEvents TextBox_TotalInvoiceCost As System.Windows.Forms.TextBox
    Public WithEvents Button_Exit As System.Windows.Forms.Button
    Public WithEvents Label_InvFrghtTot As System.Windows.Forms.Label
    Public WithEvents Label_InvoiceTotal As System.Windows.Forms.Label
    Public WithEvents Label_CostSubtotal As System.Windows.Forms.Label
    Public WithEvents Label_InvoiceNum As System.Windows.Forms.Label
    Public WithEvents Label_Date As System.Windows.Forms.Label
    Public WithEvents Label_CostTotal As System.Windows.Forms.Label
    Friend WithEvents UltraDateTime_InvoiceDate As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
    Public WithEvents Label_InvoiceCost As System.Windows.Forms.Label
    Public WithEvents Label_PONum As System.Windows.Forms.Label
    Public WithEvents TextBox_VendorKey As System.Windows.Forms.TextBox
    Public WithEvents Label_VendorKey As System.Windows.Forms.Label
    Public WithEvents Button_SearchVendor As System.Windows.Forms.Button
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Public WithEvents Button_SearchOrder As System.Windows.Forms.Button
    Public WithEvents RadioButton_DocumentType_3rdPartyFreight As System.Windows.Forms.RadioButton
    Friend WithEvents CheckBox_CreditInvoice As System.Windows.Forms.CheckBox
    Friend WithEvents UltraNumericEditor_PONum As Infragistics.Win.UltraWinEditors.UltraNumericEditor
    Friend WithEvents UltraNumericEditor_InvFrghtTot As Infragistics.Win.UltraWinEditors.UltraNumericEditor
    Friend WithEvents UltraNumericEditor_InvoiceCost As Infragistics.Win.UltraWinEditors.UltraNumericEditor
    Friend WithEvents Label_VendorID As System.Windows.Forms.Label
    Friend WithEvents UltraNumericEditor_VendorID As Infragistics.Win.UltraWinEditors.UltraNumericEditor
End Class
