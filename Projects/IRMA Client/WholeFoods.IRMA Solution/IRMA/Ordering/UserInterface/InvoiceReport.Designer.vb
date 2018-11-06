<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmInvoiceReport
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
	Public WithEvents cmdReport As System.Windows.Forms.Button
	Public WithEvents chkPrintOnly As System.Windows.Forms.CheckBox
	Public WithEvents _optSort_3 As System.Windows.Forms.RadioButton
	Public WithEvents _optSort_2 As System.Windows.Forms.RadioButton
	Public WithEvents _optSort_1 As System.Windows.Forms.RadioButton
	Public WithEvents _optSort_0 As System.Windows.Forms.RadioButton
    Public WithEvents SortByFrame As System.Windows.Forms.GroupBox
    Public WithEvents _optIdentifier_1 As System.Windows.Forms.RadioButton
    Public WithEvents _optIdentifier_0 As System.Windows.Forms.RadioButton
    Public WithEvents Frame1 As System.Windows.Forms.GroupBox
    Public WithEvents optIdentifier As Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray
    Public WithEvents optSort As Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInvoiceReport))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdExit = New System.Windows.Forms.Button
        Me.cmdReport = New System.Windows.Forms.Button
        Me.chkPrintOnly = New System.Windows.Forms.CheckBox
        Me.SortByFrame = New System.Windows.Forms.GroupBox
        Me._optSort_3 = New System.Windows.Forms.RadioButton
        Me._optSort_2 = New System.Windows.Forms.RadioButton
        Me._optSort_1 = New System.Windows.Forms.RadioButton
        Me._optSort_0 = New System.Windows.Forms.RadioButton
        Me.Frame1 = New System.Windows.Forms.GroupBox
        Me._optIdentifier_1 = New System.Windows.Forms.RadioButton
        Me._optIdentifier_0 = New System.Windows.Forms.RadioButton
        Me.optIdentifier = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(Me.components)
        Me.optSort = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(Me.components)
        Me.InvoiceTypeGroupBox = New System.Windows.Forms.GroupBox
        Me.VendorInvoiceRadioButton = New System.Windows.Forms.RadioButton
        Me.POInvoiceRadioButton = New System.Windows.Forms.RadioButton
        Me.GroupByCatCheckBox = New System.Windows.Forms.CheckBox
        Me.SortByFrame.SuspendLayout()
        Me.Frame1.SuspendLayout()
        CType(Me.optIdentifier, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.optSort, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.InvoiceTypeGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdExit
        '
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdExit.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Image = CType(resources.GetObject("cmdExit.Image"), System.Drawing.Image)
        Me.cmdExit.Location = New System.Drawing.Point(320, 152)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(41, 41)
        Me.cmdExit.TabIndex = 10
        Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdExit, "Exit")
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'cmdReport
        '
        Me.cmdReport.BackColor = System.Drawing.SystemColors.Control
        Me.cmdReport.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdReport.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdReport.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdReport.Image = CType(resources.GetObject("cmdReport.Image"), System.Drawing.Image)
        Me.cmdReport.Location = New System.Drawing.Point(273, 152)
        Me.cmdReport.Name = "cmdReport"
        Me.cmdReport.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdReport.Size = New System.Drawing.Size(41, 41)
        Me.cmdReport.TabIndex = 9
        Me.cmdReport.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdReport, "Print")
        Me.cmdReport.UseVisualStyleBackColor = False
        '
        'chkPrintOnly
        '
        Me.chkPrintOnly.BackColor = System.Drawing.SystemColors.Control
        Me.chkPrintOnly.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkPrintOnly.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPrintOnly.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkPrintOnly.Location = New System.Drawing.Point(224, 129)
        Me.chkPrintOnly.Name = "chkPrintOnly"
        Me.chkPrintOnly.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkPrintOnly.Size = New System.Drawing.Size(81, 17)
        Me.chkPrintOnly.TabIndex = 8
        Me.chkPrintOnly.Text = "Print Only"
        Me.chkPrintOnly.UseVisualStyleBackColor = False
        '
        'SortByFrame
        '
        Me.SortByFrame.BackColor = System.Drawing.SystemColors.Control
        Me.SortByFrame.Controls.Add(Me._optSort_3)
        Me.SortByFrame.Controls.Add(Me._optSort_2)
        Me.SortByFrame.Controls.Add(Me._optSort_1)
        Me.SortByFrame.Controls.Add(Me._optSort_0)
        Me.SortByFrame.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SortByFrame.ForeColor = System.Drawing.SystemColors.ControlText
        Me.SortByFrame.Location = New System.Drawing.Point(216, 8)
        Me.SortByFrame.Name = "SortByFrame"
        Me.SortByFrame.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.SortByFrame.Size = New System.Drawing.Size(145, 89)
        Me.SortByFrame.TabIndex = 3
        Me.SortByFrame.TabStop = False
        Me.SortByFrame.Text = "Sort By"
        '
        '_optSort_3
        '
        Me._optSort_3.BackColor = System.Drawing.SystemColors.Control
        Me._optSort_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._optSort_3.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optSort_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optSort.SetIndex(Me._optSort_3, CType(3, Short))
        Me._optSort_3.Location = New System.Drawing.Point(8, 64)
        Me._optSort_3.Name = "_optSort_3"
        Me._optSort_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optSort_3.Size = New System.Drawing.Size(129, 17)
        Me._optSort_3.TabIndex = 7
        Me._optSort_3.TabStop = True
        Me._optSort_3.Text = "Category"
        Me._optSort_3.UseVisualStyleBackColor = False
        '
        '_optSort_2
        '
        Me._optSort_2.BackColor = System.Drawing.SystemColors.Control
        Me._optSort_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._optSort_2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optSort_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optSort.SetIndex(Me._optSort_2, CType(2, Short))
        Me._optSort_2.Location = New System.Drawing.Point(8, 48)
        Me._optSort_2.Name = "_optSort_2"
        Me._optSort_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optSort_2.Size = New System.Drawing.Size(129, 17)
        Me._optSort_2.TabIndex = 6
        Me._optSort_2.TabStop = True
        Me._optSort_2.Text = "Identifier"
        Me._optSort_2.UseVisualStyleBackColor = False
        '
        '_optSort_1
        '
        Me._optSort_1.BackColor = System.Drawing.SystemColors.Control
        Me._optSort_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._optSort_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optSort_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optSort.SetIndex(Me._optSort_1, CType(1, Short))
        Me._optSort_1.Location = New System.Drawing.Point(8, 32)
        Me._optSort_1.Name = "_optSort_1"
        Me._optSort_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optSort_1.Size = New System.Drawing.Size(129, 17)
        Me._optSort_1.TabIndex = 5
        Me._optSort_1.TabStop = True
        Me._optSort_1.Text = "Line Cost"
        Me._optSort_1.UseVisualStyleBackColor = False
        '
        '_optSort_0
        '
        Me._optSort_0.BackColor = System.Drawing.SystemColors.Control
        Me._optSort_0.Checked = True
        Me._optSort_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._optSort_0.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optSort_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optSort.SetIndex(Me._optSort_0, CType(0, Short))
        Me._optSort_0.Location = New System.Drawing.Point(8, 16)
        Me._optSort_0.Name = "_optSort_0"
        Me._optSort_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optSort_0.Size = New System.Drawing.Size(129, 17)
        Me._optSort_0.TabIndex = 4
        Me._optSort_0.TabStop = True
        Me._optSort_0.Text = "Line Number"
        Me._optSort_0.UseVisualStyleBackColor = False
        '
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me._optIdentifier_1)
        Me.Frame1.Controls.Add(Me._optIdentifier_0)
        Me.Frame1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Location = New System.Drawing.Point(8, 8)
        Me.Frame1.Name = "Frame1"
        Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame1.Size = New System.Drawing.Size(202, 89)
        Me.Frame1.TabIndex = 0
        Me.Frame1.TabStop = False
        Me.Frame1.Text = "Item Identfier"
        '
        '_optIdentifier_1
        '
        Me._optIdentifier_1.BackColor = System.Drawing.SystemColors.Control
        Me._optIdentifier_1.Checked = True
        Me._optIdentifier_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._optIdentifier_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optIdentifier_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optIdentifier.SetIndex(Me._optIdentifier_1, CType(1, Short))
        Me._optIdentifier_1.Location = New System.Drawing.Point(8, 32)
        Me._optIdentifier_1.Name = "_optIdentifier_1"
        Me._optIdentifier_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optIdentifier_1.Size = New System.Drawing.Size(185, 17)
        Me._optIdentifier_1.TabIndex = 2
        Me._optIdentifier_1.TabStop = True
        Me._optIdentifier_1.Text = "IRMA Identifier Only"
        Me._optIdentifier_1.UseVisualStyleBackColor = False
        '
        '_optIdentifier_0
        '
        Me._optIdentifier_0.BackColor = System.Drawing.SystemColors.Control
        Me._optIdentifier_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._optIdentifier_0.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optIdentifier_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optIdentifier.SetIndex(Me._optIdentifier_0, CType(0, Short))
        Me._optIdentifier_0.Location = New System.Drawing.Point(8, 16)
        Me._optIdentifier_0.Name = "_optIdentifier_0"
        Me._optIdentifier_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optIdentifier_0.Size = New System.Drawing.Size(185, 17)
        Me._optIdentifier_0.TabIndex = 1
        Me._optIdentifier_0.TabStop = True
        Me._optIdentifier_0.Text = "Vendor ID (When Available)"
        Me._optIdentifier_0.UseVisualStyleBackColor = False
        '
        'InvoiceTypeGroupBox
        '
        Me.InvoiceTypeGroupBox.BackColor = System.Drawing.SystemColors.Control
        Me.InvoiceTypeGroupBox.Controls.Add(Me.VendorInvoiceRadioButton)
        Me.InvoiceTypeGroupBox.Controls.Add(Me.POInvoiceRadioButton)
        Me.InvoiceTypeGroupBox.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.InvoiceTypeGroupBox.ForeColor = System.Drawing.SystemColors.ControlText
        Me.InvoiceTypeGroupBox.Location = New System.Drawing.Point(8, 104)
        Me.InvoiceTypeGroupBox.Name = "InvoiceTypeGroupBox"
        Me.InvoiceTypeGroupBox.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.InvoiceTypeGroupBox.Size = New System.Drawing.Size(202, 89)
        Me.InvoiceTypeGroupBox.TabIndex = 11
        Me.InvoiceTypeGroupBox.TabStop = False
        Me.InvoiceTypeGroupBox.Text = "Invoice Type"
        '
        'VendorInvoiceRadioButton
        '
        Me.VendorInvoiceRadioButton.BackColor = System.Drawing.SystemColors.Control
        Me.VendorInvoiceRadioButton.Checked = True
        Me.VendorInvoiceRadioButton.Cursor = System.Windows.Forms.Cursors.Default
        Me.VendorInvoiceRadioButton.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.VendorInvoiceRadioButton.ForeColor = System.Drawing.SystemColors.ControlText
        Me.VendorInvoiceRadioButton.Location = New System.Drawing.Point(8, 32)
        Me.VendorInvoiceRadioButton.Name = "VendorInvoiceRadioButton"
        Me.VendorInvoiceRadioButton.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.VendorInvoiceRadioButton.Size = New System.Drawing.Size(185, 17)
        Me.VendorInvoiceRadioButton.TabIndex = 2
        Me.VendorInvoiceRadioButton.TabStop = True
        Me.VendorInvoiceRadioButton.Text = "Vendor Invoice"
        Me.VendorInvoiceRadioButton.UseVisualStyleBackColor = False
        '
        'POInvoiceRadioButton
        '
        Me.POInvoiceRadioButton.BackColor = System.Drawing.SystemColors.Control
        Me.POInvoiceRadioButton.Cursor = System.Windows.Forms.Cursors.Default
        Me.POInvoiceRadioButton.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.POInvoiceRadioButton.ForeColor = System.Drawing.SystemColors.ControlText
        Me.POInvoiceRadioButton.Location = New System.Drawing.Point(8, 16)
        Me.POInvoiceRadioButton.Name = "POInvoiceRadioButton"
        Me.POInvoiceRadioButton.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.POInvoiceRadioButton.Size = New System.Drawing.Size(185, 17)
        Me.POInvoiceRadioButton.TabIndex = 1
        Me.POInvoiceRadioButton.TabStop = True
        Me.POInvoiceRadioButton.Text = "PO Invoice"
        Me.POInvoiceRadioButton.UseVisualStyleBackColor = False
        '
        'GroupByCatCheckBox
        '
        Me.GroupByCatCheckBox.BackColor = System.Drawing.SystemColors.Control
        Me.GroupByCatCheckBox.Cursor = System.Windows.Forms.Cursors.Default
        Me.GroupByCatCheckBox.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupByCatCheckBox.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupByCatCheckBox.Location = New System.Drawing.Point(224, 106)
        Me.GroupByCatCheckBox.Name = "GroupByCatCheckBox"
        Me.GroupByCatCheckBox.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupByCatCheckBox.Size = New System.Drawing.Size(137, 17)
        Me.GroupByCatCheckBox.TabIndex = 12
        Me.GroupByCatCheckBox.Text = "Group by Category"
        Me.GroupByCatCheckBox.UseVisualStyleBackColor = False
        '
        'frmInvoiceReport
        '
        Me.AcceptButton = Me.cmdReport
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdExit
        Me.ClientSize = New System.Drawing.Size(370, 201)
        Me.Controls.Add(Me.GroupByCatCheckBox)
        Me.Controls.Add(Me.InvoiceTypeGroupBox)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.cmdReport)
        Me.Controls.Add(Me.chkPrintOnly)
        Me.Controls.Add(Me.SortByFrame)
        Me.Controls.Add(Me.Frame1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Location = New System.Drawing.Point(181, 305)
        Me.Name = "frmInvoiceReport"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Purchase Order Invoice"
        Me.SortByFrame.ResumeLayout(False)
        Me.Frame1.ResumeLayout(False)
        CType(Me.optIdentifier, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.optSort, System.ComponentModel.ISupportInitialize).EndInit()
        Me.InvoiceTypeGroupBox.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Public WithEvents InvoiceTypeGroupBox As System.Windows.Forms.GroupBox
    Public WithEvents VendorInvoiceRadioButton As System.Windows.Forms.RadioButton
    Public WithEvents POInvoiceRadioButton As System.Windows.Forms.RadioButton
    Public WithEvents GroupByCatCheckBox As System.Windows.Forms.CheckBox
#End Region 
End Class