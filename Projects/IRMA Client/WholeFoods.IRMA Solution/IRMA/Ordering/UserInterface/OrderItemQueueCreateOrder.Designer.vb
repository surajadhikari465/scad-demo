<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmOrderItemQueueCreateOrder
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
    Public WithEvents chkSend As System.Windows.Forms.CheckBox
	Public WithEvents cmdCreate As System.Windows.Forms.Button
	Public WithEvents cmdExit As System.Windows.Forms.Button
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmOrderItemQueueCreateOrder))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdCreate = New System.Windows.Forms.Button
        Me.cmdExit = New System.Windows.Forms.Button
        Me.chkSend = New System.Windows.Forms.CheckBox
        Me.optFax = New System.Windows.Forms.RadioButton
        Me.optEmail = New System.Windows.Forms.RadioButton
        Me.optManual = New System.Windows.Forms.RadioButton
        Me.txtFax = New System.Windows.Forms.TextBox
        Me.txtEmail = New System.Windows.Forms.TextBox
        Me.optElectronic = New System.Windows.Forms.RadioButton
        Me.SuspendLayout()
        '
        'cmdCreate
        '
        Me.cmdCreate.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCreate.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCreate.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCreate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCreate.Image = CType(resources.GetObject("cmdCreate.Image"), System.Drawing.Image)
        Me.cmdCreate.Location = New System.Drawing.Point(201, 123)
        Me.cmdCreate.Name = "cmdCreate"
        Me.cmdCreate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCreate.Size = New System.Drawing.Size(41, 41)
        Me.cmdCreate.TabIndex = 2
        Me.cmdCreate.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdCreate, "Create Order")
        Me.cmdCreate.UseVisualStyleBackColor = False
        '
        'cmdExit
        '
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdExit.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Image = CType(resources.GetObject("cmdExit.Image"), System.Drawing.Image)
        Me.cmdExit.Location = New System.Drawing.Point(249, 123)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(41, 41)
        Me.cmdExit.TabIndex = 3
        Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdExit, "Exit")
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'chkSend
        '
        Me.chkSend.BackColor = System.Drawing.SystemColors.Control
        Me.chkSend.Checked = True
        Me.chkSend.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkSend.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkSend.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkSend.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkSend.Location = New System.Drawing.Point(12, 14)
        Me.chkSend.Name = "chkSend"
        Me.chkSend.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkSend.Size = New System.Drawing.Size(65, 22)
        Me.chkSend.TabIndex = 1
        Me.chkSend.Text = "Send"
        Me.chkSend.UseVisualStyleBackColor = False
        '
        'optFax
        '
        Me.optFax.AutoSize = True
        Me.optFax.Location = New System.Drawing.Point(12, 44)
        Me.optFax.Name = "optFax"
        Me.optFax.Size = New System.Drawing.Size(43, 18)
        Me.optFax.TabIndex = 4
        Me.optFax.TabStop = True
        Me.optFax.Text = "Fax"
        Me.optFax.UseVisualStyleBackColor = True
        '
        'optEmail
        '
        Me.optEmail.AutoSize = True
        Me.optEmail.Location = New System.Drawing.Point(12, 68)
        Me.optEmail.Name = "optEmail"
        Me.optEmail.Size = New System.Drawing.Size(49, 18)
        Me.optEmail.TabIndex = 5
        Me.optEmail.TabStop = True
        Me.optEmail.Text = "Email"
        Me.optEmail.UseVisualStyleBackColor = True
        '
        'optManual
        '
        Me.optManual.AutoSize = True
        Me.optManual.Location = New System.Drawing.Point(12, 120)
        Me.optManual.Name = "optManual"
        Me.optManual.Size = New System.Drawing.Size(59, 18)
        Me.optManual.TabIndex = 6
        Me.optManual.TabStop = True
        Me.optManual.Text = "Manual"
        Me.optManual.UseVisualStyleBackColor = True
        '
        'txtFax
        '
        Me.txtFax.Location = New System.Drawing.Point(74, 42)
        Me.txtFax.Name = "txtFax"
        Me.txtFax.Size = New System.Drawing.Size(142, 20)
        Me.txtFax.TabIndex = 7
        '
        'txtEmail
        '
        Me.txtEmail.Location = New System.Drawing.Point(74, 68)
        Me.txtEmail.Name = "txtEmail"
        Me.txtEmail.Size = New System.Drawing.Size(142, 20)
        Me.txtEmail.TabIndex = 8
        '
        'optElectronic
        '
        Me.optElectronic.AutoSize = True
        Me.optElectronic.Location = New System.Drawing.Point(12, 96)
        Me.optElectronic.Name = "optElectronic"
        Me.optElectronic.Size = New System.Drawing.Size(72, 18)
        Me.optElectronic.TabIndex = 9
        Me.optElectronic.TabStop = True
        Me.optElectronic.Text = "Electronic"
        Me.optElectronic.UseVisualStyleBackColor = True
        '
        'frmOrderItemQueueCreateOrder
        '
        Me.AcceptButton = Me.cmdCreate
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdExit
        Me.ClientSize = New System.Drawing.Size(299, 176)
        Me.Controls.Add(Me.optElectronic)
        Me.Controls.Add(Me.txtEmail)
        Me.Controls.Add(Me.txtFax)
        Me.Controls.Add(Me.optManual)
        Me.Controls.Add(Me.optEmail)
        Me.Controls.Add(Me.optFax)
        Me.Controls.Add(Me.chkSend)
        Me.Controls.Add(Me.cmdCreate)
        Me.Controls.Add(Me.cmdExit)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Location = New System.Drawing.Point(263, 580)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmOrderItemQueueCreateOrder"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Create Queued Item Order"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents optFax As System.Windows.Forms.RadioButton
    Friend WithEvents optEmail As System.Windows.Forms.RadioButton
    Friend WithEvents optManual As System.Windows.Forms.RadioButton
    Friend WithEvents txtFax As System.Windows.Forms.TextBox
    Friend WithEvents txtEmail As System.Windows.Forms.TextBox
    Friend WithEvents optElectronic As System.Windows.Forms.RadioButton
#End Region 
End Class