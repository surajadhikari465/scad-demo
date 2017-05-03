<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmItemOnHandComparison
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
	Public WithEvents chkPrintOnly As System.Windows.Forms.CheckBox
	Public WithEvents cmdReport As System.Windows.Forms.Button
	Public WithEvents cmdExit As System.Windows.Forms.Button
	Public WithEvents cmbSubTeam As System.Windows.Forms.ComboBox
	Public WithEvents cmbStore2 As System.Windows.Forms.ComboBox
	Public WithEvents cmbStore1 As System.Windows.Forms.ComboBox
    Public WithEvents _lblLabel_5 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_0 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_1 As System.Windows.Forms.Label
	Public WithEvents lblLabel As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmItemOnHandComparison))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdReport = New System.Windows.Forms.Button
        Me.cmdExit = New System.Windows.Forms.Button
        Me.chkPrintOnly = New System.Windows.Forms.CheckBox
        Me.cmbSubTeam = New System.Windows.Forms.ComboBox
        Me.cmbStore2 = New System.Windows.Forms.ComboBox
        Me.cmbStore1 = New System.Windows.Forms.ComboBox
        Me._lblLabel_5 = New System.Windows.Forms.Label
        Me._lblLabel_0 = New System.Windows.Forms.Label
        Me._lblLabel_1 = New System.Windows.Forms.Label
        Me.lblLabel = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(Me.components)
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdReport
        '
        Me.cmdReport.BackColor = System.Drawing.SystemColors.Control
        Me.cmdReport.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdReport, "cmdReport")
        Me.cmdReport.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdReport.Name = "cmdReport"
        Me.ToolTip1.SetToolTip(Me.cmdReport, resources.GetString("cmdReport.ToolTip"))
        Me.cmdReport.UseVisualStyleBackColor = False
        '
        'cmdExit
        '
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        resources.ApplyResources(Me.cmdExit, "cmdExit")
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Name = "cmdExit"
        Me.ToolTip1.SetToolTip(Me.cmdExit, resources.GetString("cmdExit.ToolTip"))
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'chkPrintOnly
        '
        Me.chkPrintOnly.BackColor = System.Drawing.SystemColors.Control
        Me.chkPrintOnly.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.chkPrintOnly, "chkPrintOnly")
        Me.chkPrintOnly.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkPrintOnly.Name = "chkPrintOnly"
        Me.chkPrintOnly.UseVisualStyleBackColor = False
        '
        'cmbSubTeam
        '
        Me.cmbSubTeam.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbSubTeam.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbSubTeam.BackColor = System.Drawing.SystemColors.Window
        Me.cmbSubTeam.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbSubTeam.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.cmbSubTeam, "cmbSubTeam")
        Me.cmbSubTeam.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbSubTeam.Name = "cmbSubTeam"
        Me.cmbSubTeam.Sorted = True
        '
        'cmbStore2
        '
        Me.cmbStore2.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbStore2.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbStore2.BackColor = System.Drawing.SystemColors.Window
        Me.cmbStore2.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbStore2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.cmbStore2, "cmbStore2")
        Me.cmbStore2.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbStore2.Name = "cmbStore2"
        Me.cmbStore2.Sorted = True
        '
        'cmbStore1
        '
        Me.cmbStore1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbStore1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbStore1.BackColor = System.Drawing.SystemColors.Window
        Me.cmbStore1.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbStore1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.cmbStore1, "cmbStore1")
        Me.cmbStore1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbStore1.Name = "cmbStore1"
        Me.cmbStore1.Sorted = True
        '
        '_lblLabel_5
        '
        Me._lblLabel_5.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_5.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lblLabel_5, "_lblLabel_5")
        Me._lblLabel_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_5, CType(5, Short))
        Me._lblLabel_5.Name = "_lblLabel_5"
        '
        '_lblLabel_0
        '
        Me._lblLabel_0.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_0.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lblLabel_0, "_lblLabel_0")
        Me._lblLabel_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_0, CType(0, Short))
        Me._lblLabel_0.Name = "_lblLabel_0"
        '
        '_lblLabel_1
        '
        Me._lblLabel_1.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_1.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lblLabel_1, "_lblLabel_1")
        Me._lblLabel_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_1, CType(1, Short))
        Me._lblLabel_1.Name = "_lblLabel_1"
        '
        'frmItemOnHandComparison
        '
        Me.AcceptButton = Me.cmdReport
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdExit
        Me.Controls.Add(Me.chkPrintOnly)
        Me.Controls.Add(Me.cmdReport)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.cmbSubTeam)
        Me.Controls.Add(Me.cmbStore2)
        Me.Controls.Add(Me.cmbStore1)
        Me.Controls.Add(Me._lblLabel_5)
        Me.Controls.Add(Me._lblLabel_0)
        Me.Controls.Add(Me._lblLabel_1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmItemOnHandComparison"
        Me.ShowInTaskbar = False
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class