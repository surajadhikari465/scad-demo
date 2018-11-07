<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class InvoiceDiscrepanciesReport
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(InvoiceDiscrepanciesReport))
        Me.txtVendorName = New System.Windows.Forms.TextBox()
        Me.cmdCompanySearch = New System.Windows.Forms.Button()
        Me.lblVendor = New System.Windows.Forms.Label()
        Me._lblLabel_4 = New System.Windows.Forms.Label()
        Me._lblLabel_0 = New System.Windows.Forms.Label()
        Me.cmbStore = New System.Windows.Forms.ComboBox()
        Me._lblLabel_1 = New System.Windows.Forms.Label()
        Me.cmdExit = New System.Windows.Forms.Button()
        Me.cmdReport = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.optSpecificDiscrepancies = New System.Windows.Forms.RadioButton()
        Me.optPaymentDiscrepancies = New System.Windows.Forms.RadioButton()
        Me.optVendorInvoiceComplete = New System.Windows.Forms.RadioButton()
        Me.chkNoIDDiscrepancies = New System.Windows.Forms.CheckBox()
        Me.chkPackDiscrepancies = New System.Windows.Forms.CheckBox()
        Me.chkQuantityDiscrepancies = New System.Windows.Forms.CheckBox()
        Me.chkCostDiscrepancies = New System.Windows.Forms.CheckBox()
        Me.ToolTips = New System.Windows.Forms.ToolTip(Me.components)
        Me.DateTimePickerStartDate = New System.Windows.Forms.DateTimePicker()
        Me.DateTimePickerEndDate = New System.Windows.Forms.DateTimePicker()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtVendorName
        '
        Me.txtVendorName.AcceptsReturn = True
        Me.txtVendorName.BackColor = System.Drawing.SystemColors.Window
        Me.txtVendorName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtVendorName.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Bold)
        Me.txtVendorName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtVendorName.Location = New System.Drawing.Point(107, 12)
        Me.txtVendorName.MaxLength = 0
        Me.txtVendorName.Name = "txtVendorName"
        Me.txtVendorName.ReadOnly = True
        Me.txtVendorName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtVendorName.Size = New System.Drawing.Size(225, 20)
        Me.txtVendorName.TabIndex = 0
        '
        'cmdCompanySearch
        '
        Me.cmdCompanySearch.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCompanySearch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCompanySearch.Font = New System.Drawing.Font("Arial", 8.0!)
        Me.cmdCompanySearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCompanySearch.Image = CType(resources.GetObject("cmdCompanySearch.Image"), System.Drawing.Image)
        Me.cmdCompanySearch.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.cmdCompanySearch.Location = New System.Drawing.Point(338, 12)
        Me.cmdCompanySearch.Name = "cmdCompanySearch"
        Me.cmdCompanySearch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCompanySearch.Size = New System.Drawing.Size(22, 24)
        Me.cmdCompanySearch.TabIndex = 1
        Me.cmdCompanySearch.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.cmdCompanySearch.UseVisualStyleBackColor = False
        '
        'lblVendor
        '
        Me.lblVendor.BackColor = System.Drawing.SystemColors.Control
        Me.lblVendor.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblVendor.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.lblVendor.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblVendor.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblVendor.Location = New System.Drawing.Point(47, 15)
        Me.lblVendor.Name = "lblVendor"
        Me.lblVendor.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblVendor.Size = New System.Drawing.Size(54, 18)
        Me.lblVendor.TabIndex = 3
        Me.lblVendor.Text = "Vendor : "
        Me.lblVendor.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_4
        '
        Me._lblLabel_4.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_4.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabel_4.Location = New System.Drawing.Point(208, 66)
        Me._lblLabel_4.Name = "_lblLabel_4"
        Me._lblLabel_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_4.Size = New System.Drawing.Size(17, 17)
        Me._lblLabel_4.TabIndex = 17
        Me._lblLabel_4.Text = "-"
        Me._lblLabel_4.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        '_lblLabel_0
        '
        Me._lblLabel_0.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_0.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabel_0.Location = New System.Drawing.Point(13, 69)
        Me._lblLabel_0.Name = "_lblLabel_0"
        Me._lblLabel_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_0.Size = New System.Drawing.Size(89, 17)
        Me._lblLabel_0.TabIndex = 16
        Me._lblLabel_0.Text = "Closed Date :"
        Me._lblLabel_0.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'cmbStore
        '
        Me.cmbStore.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbStore.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbStore.BackColor = System.Drawing.SystemColors.Window
        Me.cmbStore.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbStore.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbStore.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbStore.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbStore.Location = New System.Drawing.Point(107, 38)
        Me.cmbStore.Name = "cmbStore"
        Me.cmbStore.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbStore.Size = New System.Drawing.Size(225, 22)
        Me.cmbStore.Sorted = True
        Me.cmbStore.TabIndex = 2
        '
        '_lblLabel_1
        '
        Me._lblLabel_1.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabel_1.Location = New System.Drawing.Point(31, 41)
        Me._lblLabel_1.Name = "_lblLabel_1"
        Me._lblLabel_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_1.Size = New System.Drawing.Size(70, 17)
        Me._lblLabel_1.TabIndex = 19
        Me._lblLabel_1.Text = "Store :"
        Me._lblLabel_1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'cmdExit
        '
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdExit.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Image = CType(resources.GetObject("cmdExit.Image"), System.Drawing.Image)
        Me.cmdExit.Location = New System.Drawing.Point(318, 280)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(41, 41)
        Me.cmdExit.TabIndex = 6
        Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'cmdReport
        '
        Me.cmdReport.BackColor = System.Drawing.SystemColors.Control
        Me.cmdReport.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdReport.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdReport.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdReport.Image = CType(resources.GetObject("cmdReport.Image"), System.Drawing.Image)
        Me.cmdReport.Location = New System.Drawing.Point(270, 280)
        Me.cmdReport.Name = "cmdReport"
        Me.cmdReport.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdReport.Size = New System.Drawing.Size(41, 41)
        Me.cmdReport.TabIndex = 5
        Me.cmdReport.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.cmdReport.UseVisualStyleBackColor = False
        '
        'GroupBox1
        '
        Me.GroupBox1.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox1.Controls.Add(Me.optSpecificDiscrepancies)
        Me.GroupBox1.Controls.Add(Me.optPaymentDiscrepancies)
        Me.GroupBox1.Controls.Add(Me.optVendorInvoiceComplete)
        Me.GroupBox1.Controls.Add(Me.chkNoIDDiscrepancies)
        Me.GroupBox1.Controls.Add(Me.chkPackDiscrepancies)
        Me.GroupBox1.Controls.Add(Me.chkQuantityDiscrepancies)
        Me.GroupBox1.Controls.Add(Me.chkCostDiscrepancies)
        Me.GroupBox1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBox1.Location = New System.Drawing.Point(15, 92)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBox1.Size = New System.Drawing.Size(317, 182)
        Me.GroupBox1.TabIndex = 71
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Items to View"
        '
        'optSpecificDiscrepancies
        '
        Me.optSpecificDiscrepancies.AutoSize = True
        Me.optSpecificDiscrepancies.Location = New System.Drawing.Point(19, 63)
        Me.optSpecificDiscrepancies.Name = "optSpecificDiscrepancies"
        Me.optSpecificDiscrepancies.Size = New System.Drawing.Size(149, 18)
        Me.optSpecificDiscrepancies.TabIndex = 84
        Me.optSpecificDiscrepancies.TabStop = True
        Me.optSpecificDiscrepancies.Text = "Specific Discrepancies"
        Me.ToolTips.SetToolTip(Me.optSpecificDiscrepancies, "Choose specific discrepancies")
        Me.optSpecificDiscrepancies.UseVisualStyleBackColor = True
        '
        'optPaymentDiscrepancies
        '
        Me.optPaymentDiscrepancies.AutoSize = True
        Me.optPaymentDiscrepancies.Location = New System.Drawing.Point(19, 39)
        Me.optPaymentDiscrepancies.Name = "optPaymentDiscrepancies"
        Me.optPaymentDiscrepancies.Size = New System.Drawing.Size(154, 18)
        Me.optPaymentDiscrepancies.TabIndex = 83
        Me.optPaymentDiscrepancies.TabStop = True
        Me.optPaymentDiscrepancies.Text = "Payment Discrepancies"
        Me.ToolTips.SetToolTip(Me.optPaymentDiscrepancies, "Show only items which result in a payment discrepancy")
        Me.optPaymentDiscrepancies.UseVisualStyleBackColor = True
        '
        'optVendorInvoiceComplete
        '
        Me.optVendorInvoiceComplete.AutoSize = True
        Me.optVendorInvoiceComplete.Enabled = False
        Me.optVendorInvoiceComplete.Location = New System.Drawing.Point(19, 15)
        Me.optVendorInvoiceComplete.Name = "optVendorInvoiceComplete"
        Me.optVendorInvoiceComplete.Size = New System.Drawing.Size(178, 18)
        Me.optVendorInvoiceComplete.TabIndex = 82
        Me.optVendorInvoiceComplete.TabStop = True
        Me.optVendorInvoiceComplete.Text = "Vendor's Invoice (All Items)"
        Me.ToolTips.SetToolTip(Me.optVendorInvoiceComplete, "Show all items, as sent in on the vendor's invoice")
        Me.optVendorInvoiceComplete.UseVisualStyleBackColor = True
        '
        'chkNoIDDiscrepancies
        '
        Me.chkNoIDDiscrepancies.BackColor = System.Drawing.SystemColors.Control
        Me.chkNoIDDiscrepancies.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkNoIDDiscrepancies.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkNoIDDiscrepancies.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkNoIDDiscrepancies.Location = New System.Drawing.Point(44, 156)
        Me.chkNoIDDiscrepancies.Name = "chkNoIDDiscrepancies"
        Me.chkNoIDDiscrepancies.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkNoIDDiscrepancies.Size = New System.Drawing.Size(175, 17)
        Me.chkNoIDDiscrepancies.TabIndex = 81
        Me.chkNoIDDiscrepancies.Text = "Item Not Found/Ordered"
        Me.ToolTips.SetToolTip(Me.chkNoIDDiscrepancies, "Item on the invoice was not found on the purchase order")
        Me.chkNoIDDiscrepancies.UseVisualStyleBackColor = False
        '
        'chkPackDiscrepancies
        '
        Me.chkPackDiscrepancies.BackColor = System.Drawing.SystemColors.Control
        Me.chkPackDiscrepancies.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkPackDiscrepancies.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPackDiscrepancies.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkPackDiscrepancies.Location = New System.Drawing.Point(44, 87)
        Me.chkPackDiscrepancies.Name = "chkPackDiscrepancies"
        Me.chkPackDiscrepancies.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkPackDiscrepancies.Size = New System.Drawing.Size(153, 17)
        Me.chkPackDiscrepancies.TabIndex = 80
        Me.chkPackDiscrepancies.Text = "Pack"
        Me.ToolTips.SetToolTip(Me.chkPackDiscrepancies, "An item's pack on the invoice is different from the purchase order")
        Me.chkPackDiscrepancies.UseVisualStyleBackColor = False
        '
        'chkQuantityDiscrepancies
        '
        Me.chkQuantityDiscrepancies.BackColor = System.Drawing.SystemColors.Control
        Me.chkQuantityDiscrepancies.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkQuantityDiscrepancies.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkQuantityDiscrepancies.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkQuantityDiscrepancies.Location = New System.Drawing.Point(44, 110)
        Me.chkQuantityDiscrepancies.Name = "chkQuantityDiscrepancies"
        Me.chkQuantityDiscrepancies.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkQuantityDiscrepancies.Size = New System.Drawing.Size(153, 17)
        Me.chkQuantityDiscrepancies.TabIndex = 79
        Me.chkQuantityDiscrepancies.Text = "Quantity"
        Me.ToolTips.SetToolTip(Me.chkQuantityDiscrepancies, "An item's quantity on the invoice is different from the purchase order")
        Me.chkQuantityDiscrepancies.UseVisualStyleBackColor = False
        '
        'chkCostDiscrepancies
        '
        Me.chkCostDiscrepancies.BackColor = System.Drawing.SystemColors.Control
        Me.chkCostDiscrepancies.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkCostDiscrepancies.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkCostDiscrepancies.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkCostDiscrepancies.Location = New System.Drawing.Point(44, 133)
        Me.chkCostDiscrepancies.Name = "chkCostDiscrepancies"
        Me.chkCostDiscrepancies.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkCostDiscrepancies.Size = New System.Drawing.Size(153, 17)
        Me.chkCostDiscrepancies.TabIndex = 78
        Me.chkCostDiscrepancies.Text = "Cost"
        Me.ToolTips.SetToolTip(Me.chkCostDiscrepancies, "An item's cost on the invoice is different from the purchase order")
        Me.chkCostDiscrepancies.UseVisualStyleBackColor = False
        '
        'DateTimePickerStartDate
        '
        Me.DateTimePickerStartDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.DateTimePickerStartDate.Location = New System.Drawing.Point(108, 66)
        Me.DateTimePickerStartDate.Name = "DateTimePickerStartDate"
        Me.DateTimePickerStartDate.Size = New System.Drawing.Size(91, 20)
        Me.DateTimePickerStartDate.TabIndex = 72
        '
        'DateTimePickerEndDate
        '
        Me.DateTimePickerEndDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.DateTimePickerEndDate.Location = New System.Drawing.Point(233, 66)
        Me.DateTimePickerEndDate.Name = "DateTimePickerEndDate"
        Me.DateTimePickerEndDate.Size = New System.Drawing.Size(99, 20)
        Me.DateTimePickerEndDate.TabIndex = 73
        '
        'InvoiceDiscrepanciesReport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(371, 334)
        Me.Controls.Add(Me.DateTimePickerEndDate)
        Me.Controls.Add(Me.DateTimePickerStartDate)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.cmdReport)
        Me.Controls.Add(Me.cmbStore)
        Me.Controls.Add(Me._lblLabel_1)
        Me.Controls.Add(Me._lblLabel_4)
        Me.Controls.Add(Me._lblLabel_0)
        Me.Controls.Add(Me.txtVendorName)
        Me.Controls.Add(Me.cmdCompanySearch)
        Me.Controls.Add(Me.lblVendor)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "InvoiceDiscrepanciesReport"
        Me.ShowInTaskbar = False
        Me.Text = "Invoice Discrepancies Report"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Public WithEvents txtVendorName As System.Windows.Forms.TextBox
    Public WithEvents cmdCompanySearch As System.Windows.Forms.Button
    Public WithEvents lblVendor As System.Windows.Forms.Label
    Public WithEvents _lblLabel_4 As System.Windows.Forms.Label
    Public WithEvents _lblLabel_0 As System.Windows.Forms.Label
    Public WithEvents cmbStore As System.Windows.Forms.ComboBox
    Public WithEvents _lblLabel_1 As System.Windows.Forms.Label
    Public WithEvents cmdExit As System.Windows.Forms.Button
    Public WithEvents cmdReport As System.Windows.Forms.Button
    Public WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Public WithEvents chkNoIDDiscrepancies As System.Windows.Forms.CheckBox
    Public WithEvents chkPackDiscrepancies As System.Windows.Forms.CheckBox
    Public WithEvents chkQuantityDiscrepancies As System.Windows.Forms.CheckBox
    Public WithEvents chkCostDiscrepancies As System.Windows.Forms.CheckBox
    Friend WithEvents optVendorInvoiceComplete As System.Windows.Forms.RadioButton
    Friend WithEvents optSpecificDiscrepancies As System.Windows.Forms.RadioButton
    Friend WithEvents optPaymentDiscrepancies As System.Windows.Forms.RadioButton
    Friend WithEvents ToolTips As System.Windows.Forms.ToolTip
    Friend WithEvents DateTimePickerStartDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents DateTimePickerEndDate As System.Windows.Forms.DateTimePicker
End Class
