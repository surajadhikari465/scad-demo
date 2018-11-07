<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmOrdersDesc
#Region "Windows Form Designer generated code "
	<System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
        MyBase.New()
        Me.IsInitializing = True
		'This call is required by the Windows Form Designer.
        InitializeComponent()
        Me.IsInitializing = False
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmOrdersDesc))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdExit = New System.Windows.Forms.Button()
        Me.txtField = New System.Windows.Forms.TextBox()
        Me.lblCharacterCount = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'cmdExit
        '
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Image = CType(resources.GetObject("cmdExit.Image"), System.Drawing.Image)
        Me.cmdExit.Location = New System.Drawing.Point(424, 170)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(41, 41)
        Me.cmdExit.TabIndex = 1
        Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdExit, "Exit")
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'txtField
        '
        Me.txtField.AcceptsReturn = True
        Me.txtField.BackColor = System.Drawing.SystemColors.Window
        Me.txtField.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtField.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtField.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.Location = New System.Drawing.Point(8, 8)
        Me.txtField.MaxLength = 5000
        Me.txtField.Multiline = True
        Me.txtField.Name = "txtField"
        Me.txtField.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtField.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtField.Size = New System.Drawing.Size(457, 155)
        Me.txtField.TabIndex = 0
        '
        'lblCharacterCount
        '
        Me.lblCharacterCount.AutoSize = True
        Me.lblCharacterCount.Location = New System.Drawing.Point(5, 182)
        Me.lblCharacterCount.Name = "lblCharacterCount"
        Me.lblCharacterCount.Size = New System.Drawing.Size(134, 14)
        Me.lblCharacterCount.TabIndex = 2
        Me.lblCharacterCount.Text = "{0} of {1} characters used"
        Me.lblCharacterCount.Visible = False
        '
        'frmOrdersDesc
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(476, 215)
        Me.Controls.Add(Me.lblCharacterCount)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.txtField)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Location = New System.Drawing.Point(119, 243)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmOrdersDesc"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Order Notes"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblCharacterCount As System.Windows.Forms.Label
#End Region 
End Class