<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmShelfLifeAdd
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
	Public WithEvents txtShelfLife_Name As System.Windows.Forms.TextBox
	Public WithEvents lblLabel As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmShelfLifeAdd))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdAdd = New System.Windows.Forms.Button
        Me.cmdExit = New System.Windows.Forms.Button
        Me.txtShelfLife_Name = New System.Windows.Forms.TextBox
        Me.lblLabel = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'cmdAdd
        '
        Me.cmdAdd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAdd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAdd.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAdd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAdd.Image = CType(resources.GetObject("cmdAdd.Image"), System.Drawing.Image)
        Me.cmdAdd.Location = New System.Drawing.Point(248, 40)
        Me.cmdAdd.Name = "cmdAdd"
        Me.cmdAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAdd.Size = New System.Drawing.Size(41, 41)
        Me.cmdAdd.TabIndex = 2
        Me.cmdAdd.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdAdd, "Add Shelf Life")
        Me.cmdAdd.UseVisualStyleBackColor = False
        '
        'cmdExit
        '
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Image = CType(resources.GetObject("cmdExit.Image"), System.Drawing.Image)
        Me.cmdExit.Location = New System.Drawing.Point(296, 40)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(41, 41)
        Me.cmdExit.TabIndex = 3
        Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdExit, "Cancel Add")
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'txtShelfLife_Name
        '
        Me.txtShelfLife_Name.AcceptsReturn = True
        Me.txtShelfLife_Name.BackColor = System.Drawing.SystemColors.Window
        Me.txtShelfLife_Name.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtShelfLife_Name.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtShelfLife_Name.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtShelfLife_Name.Location = New System.Drawing.Point(80, 16)
        Me.txtShelfLife_Name.MaxLength = 25
        Me.txtShelfLife_Name.Name = "txtShelfLife_Name"
        Me.txtShelfLife_Name.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtShelfLife_Name.Size = New System.Drawing.Size(257, 19)
        Me.txtShelfLife_Name.TabIndex = 1
        '
        'lblLabel
        '
        Me.lblLabel.BackColor = System.Drawing.Color.Transparent
        Me.lblLabel.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLabel.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLabel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.Location = New System.Drawing.Point(8, 16)
        Me.lblLabel.Name = "lblLabel"
        Me.lblLabel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLabel.Size = New System.Drawing.Size(65, 17)
        Me.lblLabel.TabIndex = 0
        Me.lblLabel.Text = "Shelf Life :"
        Me.lblLabel.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'frmShelfLifeAdd
        '
        Me.AcceptButton = Me.cmdAdd
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdExit
        Me.ClientSize = New System.Drawing.Size(355, 89)
        Me.Controls.Add(Me.cmdAdd)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.txtShelfLife_Name)
        Me.Controls.Add(Me.lblLabel)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Location = New System.Drawing.Point(99, 319)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmShelfLifeAdd"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "New Shelf Life"
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class