<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmBrandAdd
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
	Public WithEvents cmdAdd As System.Windows.Forms.Button
	Public WithEvents cmdExit As System.Windows.Forms.Button
	Public WithEvents txtBrand_Name As System.Windows.Forms.TextBox
	Public WithEvents lblLabel As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmBrandAdd))
		Me.components = New System.ComponentModel.Container()
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(components)
		Me.cmdAdd = New System.Windows.Forms.Button
		Me.cmdExit = New System.Windows.Forms.Button
		Me.txtBrand_Name = New System.Windows.Forms.TextBox
		Me.lblLabel = New System.Windows.Forms.Label
		Me.SuspendLayout()
		Me.ToolTip1.Active = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.Text = "New Brand"
		Me.ClientSize = New System.Drawing.Size(339, 90)
		Me.Location = New System.Drawing.Point(218, 229)
		Me.ShowInTaskbar = False
		Me.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ControlBox = True
		Me.Enabled = True
		Me.KeyPreview = False
		Me.MaximizeBox = True
		Me.MinimizeBox = True
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.HelpButton = False
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.Name = "frmBrandAdd"
		Me.cmdAdd.TextAlign = System.Drawing.ContentAlignment.BottomCenter
		Me.AcceptButton = Me.cmdAdd
		Me.cmdAdd.Size = New System.Drawing.Size(41, 41)
		Me.cmdAdd.Location = New System.Drawing.Point(232, 40)
		Me.cmdAdd.Image = CType(resources.GetObject("cmdAdd.Image"), System.Drawing.Image)
		Me.cmdAdd.TabIndex = 2
		Me.ToolTip1.SetToolTip(Me.cmdAdd, "Add Brand")
		Me.cmdAdd.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmdAdd.BackColor = System.Drawing.SystemColors.Control
		Me.cmdAdd.CausesValidation = True
		Me.cmdAdd.Enabled = True
		Me.cmdAdd.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdAdd.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdAdd.TabStop = True
		Me.cmdAdd.Name = "cmdAdd"
		Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
		Me.CancelButton = Me.cmdExit
		Me.cmdExit.Size = New System.Drawing.Size(41, 41)
		Me.cmdExit.Location = New System.Drawing.Point(280, 40)
		Me.cmdExit.Image = CType(resources.GetObject("cmdExit.Image"), System.Drawing.Image)
		Me.cmdExit.TabIndex = 3
		Me.ToolTip1.SetToolTip(Me.cmdExit, "Cancel Add")
		Me.cmdExit.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
		Me.cmdExit.CausesValidation = True
		Me.cmdExit.Enabled = True
		Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdExit.TabStop = True
		Me.cmdExit.Name = "cmdExit"
		Me.txtBrand_Name.AutoSize = False
		Me.txtBrand_Name.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.txtBrand_Name.Size = New System.Drawing.Size(257, 19)
		Me.txtBrand_Name.Location = New System.Drawing.Point(64, 16)
		Me.txtBrand_Name.Maxlength = 25
		Me.txtBrand_Name.TabIndex = 1
		Me.txtBrand_Name.AcceptsReturn = True
		Me.txtBrand_Name.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtBrand_Name.BackColor = System.Drawing.SystemColors.Window
		Me.txtBrand_Name.CausesValidation = True
		Me.txtBrand_Name.Enabled = True
		Me.txtBrand_Name.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtBrand_Name.HideSelection = True
		Me.txtBrand_Name.ReadOnly = False
		Me.txtBrand_Name.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtBrand_Name.MultiLine = False
		Me.txtBrand_Name.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtBrand_Name.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtBrand_Name.TabStop = True
		Me.txtBrand_Name.Visible = True
		Me.txtBrand_Name.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtBrand_Name.Name = "txtBrand_Name"
		Me.lblLabel.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.lblLabel.Text = "Brand :"
		Me.lblLabel.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblLabel.Size = New System.Drawing.Size(49, 17)
		Me.lblLabel.Location = New System.Drawing.Point(8, 16)
		Me.lblLabel.TabIndex = 0
		Me.lblLabel.BackColor = System.Drawing.Color.Transparent
		Me.lblLabel.Enabled = True
		Me.lblLabel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblLabel.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblLabel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblLabel.UseMnemonic = True
		Me.lblLabel.Visible = True
		Me.lblLabel.AutoSize = False
		Me.lblLabel.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblLabel.Name = "lblLabel"
		Me.Controls.Add(cmdAdd)
		Me.Controls.Add(cmdExit)
		Me.Controls.Add(txtBrand_Name)
		Me.Controls.Add(lblLabel)
		Me.ResumeLayout(False)
		Me.PerformLayout()
	End Sub
#End Region 
End Class