<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmCycleCountAddCount
#Region "Windows Form Designer generated code "
	<System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
		MyBase.New()

        IsInitializing = True

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        Isinitializing = False

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
	Public WithEvents optLocation As System.Windows.Forms.RadioButton
	Public WithEvents optSubTeam As System.Windows.Forms.RadioButton
	Public WithEvents _lblLabel_4 As System.Windows.Forms.Label
	Public WithEvents lblLabel As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCycleCountAddCount))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdExit = New System.Windows.Forms.Button
        Me.optLocation = New System.Windows.Forms.RadioButton
        Me.optSubTeam = New System.Windows.Forms.RadioButton
        Me._lblLabel_4 = New System.Windows.Forms.Label
        Me.lblLabel = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(Me.components)
        Me.txtStartScan = New System.Windows.Forms.DateTimePicker
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdExit
        '
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdExit, "cmdExit")
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Name = "cmdExit"
        Me.ToolTip1.SetToolTip(Me.cmdExit, resources.GetString("cmdExit.ToolTip"))
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'optLocation
        '
        Me.optLocation.BackColor = System.Drawing.SystemColors.Control
        Me.optLocation.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.optLocation, "optLocation")
        Me.optLocation.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optLocation.Name = "optLocation"
        Me.optLocation.TabStop = True
        Me.optLocation.UseVisualStyleBackColor = False
        '
        'optSubTeam
        '
        Me.optSubTeam.BackColor = System.Drawing.SystemColors.Control
        Me.optSubTeam.Checked = True
        Me.optSubTeam.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.optSubTeam, "optSubTeam")
        Me.optSubTeam.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optSubTeam.Name = "optSubTeam"
        Me.optSubTeam.TabStop = True
        Me.optSubTeam.UseVisualStyleBackColor = False
        '
        '_lblLabel_4
        '
        Me._lblLabel_4.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_4.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lblLabel_4, "_lblLabel_4")
        Me._lblLabel_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_4, CType(4, Short))
        Me._lblLabel_4.Name = "_lblLabel_4"
        '
        'txtStartScan
        '
        resources.ApplyResources(Me.txtStartScan, "txtStartScan")
        Me.txtStartScan.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.txtStartScan.Name = "txtStartScan"
        '
        'frmCycleCountAddCount
        '
        Me.AcceptButton = Me.cmdExit
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.Controls.Add(Me.txtStartScan)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.optLocation)
        Me.Controls.Add(Me.optSubTeam)
        Me.Controls.Add(Me._lblLabel_4)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmCycleCountAddCount"
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents txtStartScan As System.Windows.Forms.DateTimePicker
#End Region 
End Class