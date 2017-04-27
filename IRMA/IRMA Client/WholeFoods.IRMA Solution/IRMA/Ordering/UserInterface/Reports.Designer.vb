<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmReports
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
	Public WithEvents cmdInvoice As System.Windows.Forms.Button
	Public WithEvents cmdCheckList As System.Windows.Forms.Button
    Public WithEvents cmdExit As System.Windows.Forms.Button
	Public WithEvents cmdPurchaseOrder As System.Windows.Forms.Button
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmReports))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdExit = New System.Windows.Forms.Button
        Me.cmdInvoice = New System.Windows.Forms.Button
        Me.cmdCheckList = New System.Windows.Forms.Button
        Me.cmdPurchaseOrder = New System.Windows.Forms.Button
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
        Me.cmdExit.Location = New System.Drawing.Point(203, 80)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(41, 41)
        Me.cmdExit.TabIndex = 3
        Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdExit, "Exit")
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'cmdInvoice
        '
        Me.cmdInvoice.BackColor = System.Drawing.SystemColors.Control
        Me.cmdInvoice.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdInvoice.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdInvoice.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdInvoice.Location = New System.Drawing.Point(12, 88)
        Me.cmdInvoice.Name = "cmdInvoice"
        Me.cmdInvoice.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdInvoice.Size = New System.Drawing.Size(177, 33)
        Me.cmdInvoice.TabIndex = 2
        Me.cmdInvoice.Text = "Invoice"
        Me.cmdInvoice.UseVisualStyleBackColor = False
        '
        'cmdCheckList
        '
        Me.cmdCheckList.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCheckList.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCheckList.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCheckList.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCheckList.Location = New System.Drawing.Point(12, 48)
        Me.cmdCheckList.Name = "cmdCheckList"
        Me.cmdCheckList.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCheckList.Size = New System.Drawing.Size(177, 33)
        Me.cmdCheckList.TabIndex = 1
        Me.cmdCheckList.Text = "Receiving Check List"
        Me.cmdCheckList.UseVisualStyleBackColor = False
        '
        'cmdPurchaseOrder
        '
        Me.cmdPurchaseOrder.BackColor = System.Drawing.SystemColors.Control
        Me.cmdPurchaseOrder.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdPurchaseOrder.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdPurchaseOrder.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdPurchaseOrder.Location = New System.Drawing.Point(12, 8)
        Me.cmdPurchaseOrder.Name = "cmdPurchaseOrder"
        Me.cmdPurchaseOrder.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdPurchaseOrder.Size = New System.Drawing.Size(177, 33)
        Me.cmdPurchaseOrder.TabIndex = 0
        Me.cmdPurchaseOrder.Text = "Purchase Order"
        Me.cmdPurchaseOrder.UseVisualStyleBackColor = False
        '
        'frmReports
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdExit
        Me.ClientSize = New System.Drawing.Size(255, 134)
        Me.Controls.Add(Me.cmdInvoice)
        Me.Controls.Add(Me.cmdCheckList)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.cmdPurchaseOrder)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Location = New System.Drawing.Point(445, 206)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmReports"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Reports"
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class