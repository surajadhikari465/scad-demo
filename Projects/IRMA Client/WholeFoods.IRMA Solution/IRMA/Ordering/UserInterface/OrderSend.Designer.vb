<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmOrderSend
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
	Public WithEvents cmdSendOrder As System.Windows.Forms.Button
    Public WithEvents lblLabel As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmOrderSend))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdExit = New System.Windows.Forms.Button
        Me.cmdSendOrder = New System.Windows.Forms.Button
        Me.lblLabel = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(Me.components)
        Me._optPOTrans_0 = New System.Windows.Forms.RadioButton
        Me._optPOTrans_1 = New System.Windows.Forms.RadioButton
        Me._optPOTrans_2 = New System.Windows.Forms.RadioButton
        Me._txtField_0 = New System.Windows.Forms.TextBox
        Me._txtField_1 = New System.Windows.Forms.TextBox
        Me._labelTestEnvironment = New System.Windows.Forms.Label
        Me._optPOTrans_3 = New System.Windows.Forms.RadioButton
        Me.CheckBox_DropShip = New System.Windows.Forms.CheckBox
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.cmdExit.Location = New System.Drawing.Point(285, 102)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(41, 41)
        Me.cmdExit.TabIndex = 2
        Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdExit, "Exit")
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'cmdSendOrder
        '
        Me.cmdSendOrder.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdSendOrder.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSendOrder.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSendOrder.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.cmdSendOrder.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSendOrder.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSendOrder.Image = CType(resources.GetObject("cmdSendOrder.Image"), System.Drawing.Image)
        Me.cmdSendOrder.Location = New System.Drawing.Point(238, 102)
        Me.cmdSendOrder.Name = "cmdSendOrder"
        Me.cmdSendOrder.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSendOrder.Size = New System.Drawing.Size(41, 41)
        Me.cmdSendOrder.TabIndex = 1
        Me.cmdSendOrder.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdSendOrder, "Send Now")
        Me.cmdSendOrder.UseVisualStyleBackColor = False
        '
        '_optPOTrans_0
        '
        Me._optPOTrans_0.AutoSize = True
        Me._optPOTrans_0.Enabled = False
        Me._optPOTrans_0.Location = New System.Drawing.Point(15, 13)
        Me._optPOTrans_0.Name = "_optPOTrans_0"
        Me._optPOTrans_0.Size = New System.Drawing.Size(43, 18)
        Me._optPOTrans_0.TabIndex = 3
        Me._optPOTrans_0.TabStop = True
        Me._optPOTrans_0.Text = "Fax"
        Me._optPOTrans_0.UseVisualStyleBackColor = True
        '
        '_optPOTrans_1
        '
        Me._optPOTrans_1.AutoSize = True
        Me._optPOTrans_1.Enabled = False
        Me._optPOTrans_1.Location = New System.Drawing.Point(15, 38)
        Me._optPOTrans_1.Name = "_optPOTrans_1"
        Me._optPOTrans_1.Size = New System.Drawing.Size(49, 18)
        Me._optPOTrans_1.TabIndex = 4
        Me._optPOTrans_1.TabStop = True
        Me._optPOTrans_1.Text = "Email"
        Me._optPOTrans_1.UseVisualStyleBackColor = True
        '
        '_optPOTrans_2
        '
        Me._optPOTrans_2.AutoSize = True
        Me._optPOTrans_2.Enabled = False
        Me._optPOTrans_2.Location = New System.Drawing.Point(15, 88)
        Me._optPOTrans_2.Name = "_optPOTrans_2"
        Me._optPOTrans_2.Size = New System.Drawing.Size(59, 18)
        Me._optPOTrans_2.TabIndex = 5
        Me._optPOTrans_2.TabStop = True
        Me._optPOTrans_2.Text = "Manual"
        Me._optPOTrans_2.UseVisualStyleBackColor = True
        '
        '_txtField_0
        '
        Me._txtField_0.Enabled = False
        Me._txtField_0.Location = New System.Drawing.Point(80, 12)
        Me._txtField_0.Name = "_txtField_0"
        Me._txtField_0.Size = New System.Drawing.Size(246, 20)
        Me._txtField_0.TabIndex = 6
        '
        '_txtField_1
        '
        Me._txtField_1.Enabled = False
        Me._txtField_1.Location = New System.Drawing.Point(80, 38)
        Me._txtField_1.Name = "_txtField_1"
        Me._txtField_1.Size = New System.Drawing.Size(246, 20)
        Me._txtField_1.TabIndex = 7
        '
        '_labelTestEnvironment
        '
        Me._labelTestEnvironment.AutoSize = True
        Me._labelTestEnvironment.Location = New System.Drawing.Point(12, 125)
        Me._labelTestEnvironment.Name = "_labelTestEnvironment"
        Me._labelTestEnvironment.Size = New System.Drawing.Size(197, 14)
        Me._labelTestEnvironment.TabIndex = 8
        Me._labelTestEnvironment.Text = "You are working in a Test environment."
        Me._labelTestEnvironment.Visible = False
        '
        '_optPOTrans_3
        '
        Me._optPOTrans_3.AutoSize = True
        Me._optPOTrans_3.Enabled = False
        Me._optPOTrans_3.Location = New System.Drawing.Point(15, 64)
        Me._optPOTrans_3.Name = "_optPOTrans_3"
        Me._optPOTrans_3.Size = New System.Drawing.Size(72, 18)
        Me._optPOTrans_3.TabIndex = 9
        Me._optPOTrans_3.TabStop = True
        Me._optPOTrans_3.Text = "Electronic"
        Me._optPOTrans_3.UseVisualStyleBackColor = True
        '
        'CheckBox_DropShip
        '
        Me.CheckBox_DropShip.AutoSize = True
        Me.CheckBox_DropShip.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckBox_DropShip.Location = New System.Drawing.Point(253, 64)
        Me.CheckBox_DropShip.Name = "CheckBox_DropShip"
        Me.CheckBox_DropShip.Size = New System.Drawing.Size(73, 18)
        Me.CheckBox_DropShip.TabIndex = 10
        Me.CheckBox_DropShip.Text = "Drop Ship"
        Me.CheckBox_DropShip.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckBox_DropShip.UseVisualStyleBackColor = True
        Me.CheckBox_DropShip.Visible = False
        '
        'frmOrderSend
        '
        Me.AcceptButton = Me.cmdSendOrder
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdExit
        Me.ClientSize = New System.Drawing.Size(344, 151)
        Me.Controls.Add(Me.CheckBox_DropShip)
        Me.Controls.Add(Me._optPOTrans_3)
        Me.Controls.Add(Me._labelTestEnvironment)
        Me.Controls.Add(Me._txtField_1)
        Me.Controls.Add(Me._txtField_0)
        Me.Controls.Add(Me._optPOTrans_2)
        Me.Controls.Add(Me._optPOTrans_1)
        Me.Controls.Add(Me._optPOTrans_0)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.cmdSendOrder)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Location = New System.Drawing.Point(3, 29)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmOrderSend"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.Text = "Order Send"
        Me.TopMost = True
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents _optPOTrans_0 As System.Windows.Forms.RadioButton
    Friend WithEvents _optPOTrans_1 As System.Windows.Forms.RadioButton
    Friend WithEvents _optPOTrans_2 As System.Windows.Forms.RadioButton
    Friend WithEvents _txtField_0 As System.Windows.Forms.TextBox
    Friend WithEvents _txtField_1 As System.Windows.Forms.TextBox
    Friend WithEvents _labelTestEnvironment As System.Windows.Forms.Label
    Friend WithEvents _optPOTrans_3 As System.Windows.Forms.RadioButton
    Friend WithEvents CheckBox_DropShip As System.Windows.Forms.CheckBox
#End Region 
End Class