<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmTGMLoad
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
	Public cdbFileOpen As System.Windows.Forms.OpenFileDialog
	Public WithEvents pbCheck As System.Windows.Forms.ProgressBar
	Public WithEvents pbDownload As System.Windows.Forms.ProgressBar
	Public WithEvents pbImport As System.Windows.Forms.ProgressBar
	Public WithEvents _Label1_2 As System.Windows.Forms.Label
	Public WithEvents _Label1_1 As System.Windows.Forms.Label
	Public WithEvents _Label1_0 As System.Windows.Forms.Label
	Public WithEvents Label1 As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cdbFileOpen = New System.Windows.Forms.OpenFileDialog
        Me.pbCheck = New System.Windows.Forms.ProgressBar
        Me.pbDownload = New System.Windows.Forms.ProgressBar
        Me.pbImport = New System.Windows.Forms.ProgressBar
        Me._Label1_2 = New System.Windows.Forms.Label
        Me._Label1_1 = New System.Windows.Forms.Label
        Me._Label1_0 = New System.Windows.Forms.Label
        Me.Label1 = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(Me.components)
        CType(Me.Label1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cdbFileOpen
        '
        Me.cdbFileOpen.DefaultExt = "DAT"
        '
        'pbCheck
        '
        Me.pbCheck.Location = New System.Drawing.Point(8, 24)
        Me.pbCheck.Name = "pbCheck"
        Me.pbCheck.Size = New System.Drawing.Size(265, 17)
        Me.pbCheck.TabIndex = 0
        '
        'pbDownload
        '
        Me.pbDownload.Location = New System.Drawing.Point(8, 64)
        Me.pbDownload.Name = "pbDownload"
        Me.pbDownload.Size = New System.Drawing.Size(265, 17)
        Me.pbDownload.TabIndex = 2
        '
        'pbImport
        '
        Me.pbImport.Location = New System.Drawing.Point(8, 104)
        Me.pbImport.Name = "pbImport"
        Me.pbImport.Size = New System.Drawing.Size(265, 17)
        Me.pbImport.TabIndex = 5
        '
        '_Label1_2
        '
        Me._Label1_2.BackColor = System.Drawing.Color.Transparent
        Me._Label1_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._Label1_2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._Label1_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.SetIndex(Me._Label1_2, CType(2, Short))
        Me._Label1_2.Location = New System.Drawing.Point(8, 88)
        Me._Label1_2.Name = "_Label1_2"
        Me._Label1_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._Label1_2.Size = New System.Drawing.Size(185, 17)
        Me._Label1_2.TabIndex = 4
        Me._Label1_2.Text = "File Import"
        '
        '_Label1_1
        '
        Me._Label1_1.BackColor = System.Drawing.Color.Transparent
        Me._Label1_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._Label1_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._Label1_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.SetIndex(Me._Label1_1, CType(1, Short))
        Me._Label1_1.Location = New System.Drawing.Point(8, 48)
        Me._Label1_1.Name = "_Label1_1"
        Me._Label1_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._Label1_1.Size = New System.Drawing.Size(185, 17)
        Me._Label1_1.TabIndex = 3
        Me._Label1_1.Text = "Data Download"
        '
        '_Label1_0
        '
        Me._Label1_0.BackColor = System.Drawing.Color.Transparent
        Me._Label1_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._Label1_0.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._Label1_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.SetIndex(Me._Label1_0, CType(0, Short))
        Me._Label1_0.Location = New System.Drawing.Point(8, 8)
        Me._Label1_0.Name = "_Label1_0"
        Me._Label1_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._Label1_0.Size = New System.Drawing.Size(185, 17)
        Me._Label1_0.TabIndex = 1
        Me._Label1_0.Text = "File Consistency Check"
        '
        'frmTGMLoad
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(282, 131)
        Me.Controls.Add(Me.pbCheck)
        Me.Controls.Add(Me.pbDownload)
        Me.Controls.Add(Me.pbImport)
        Me.Controls.Add(Me._Label1_2)
        Me.Controls.Add(Me._Label1_1)
        Me.Controls.Add(Me._Label1_0)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Location = New System.Drawing.Point(284, 160)
        Me.Name = "frmTGMLoad"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "TGM Load"
        CType(Me.Label1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class