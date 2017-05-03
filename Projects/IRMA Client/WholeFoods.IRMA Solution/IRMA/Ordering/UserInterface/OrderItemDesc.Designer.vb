<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmOrdersItemDesc
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
	Public WithEvents txtField As System.Windows.Forms.TextBox
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmOrdersItemDesc))
		Me.components = New System.ComponentModel.Container()
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(components)
		Me.cmdExit = New System.Windows.Forms.Button
		Me.txtField = New System.Windows.Forms.TextBox
		Me.SuspendLayout()
		Me.ToolTip1.Active = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.Text = "Line Item Comment"
		Me.ClientSize = New System.Drawing.Size(476, 226)
		Me.Location = New System.Drawing.Point(267, 522)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.ShowInTaskbar = False
		Me.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ControlBox = True
		Me.Enabled = True
		Me.KeyPreview = False
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.HelpButton = False
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.Name = "frmOrdersItemDesc"
		Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
		Me.cmdExit.Size = New System.Drawing.Size(41, 41)
		Me.cmdExit.Location = New System.Drawing.Point(424, 176)
		Me.cmdExit.Image = CType(resources.GetObject("cmdExit.Image"), System.Drawing.Image)
		Me.cmdExit.TabIndex = 1
		Me.ToolTip1.SetToolTip(Me.cmdExit, "Exit")
		Me.cmdExit.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
		Me.cmdExit.CausesValidation = True
		Me.cmdExit.Enabled = True
		Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdExit.TabStop = True
		Me.cmdExit.Name = "cmdExit"
		Me.txtField.AutoSize = False
		Me.txtField.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.txtField.Size = New System.Drawing.Size(457, 155)
		Me.txtField.Location = New System.Drawing.Point(8, 8)
		Me.txtField.Maxlength = 255
		Me.txtField.MultiLine = True
		Me.txtField.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
		Me.txtField.TabIndex = 0
		Me.txtField.AcceptsReturn = True
		Me.txtField.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtField.BackColor = System.Drawing.SystemColors.Window
		Me.txtField.CausesValidation = True
		Me.txtField.Enabled = True
		Me.txtField.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtField.HideSelection = True
		Me.txtField.ReadOnly = False
		Me.txtField.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtField.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtField.TabStop = True
		Me.txtField.Visible = True
		Me.txtField.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtField.Name = "txtField"
		Me.Controls.Add(cmdExit)
		Me.Controls.Add(txtField)
		Me.ResumeLayout(False)
		Me.PerformLayout()
	End Sub
#End Region 
End Class