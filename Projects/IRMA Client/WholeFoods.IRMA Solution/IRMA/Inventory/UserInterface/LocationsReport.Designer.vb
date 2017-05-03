<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmLocationsReport
#Region "Windows Form Designer generated code "
	<System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
        MyBase.New()

        IsInitializing = True

		'This call is required by the Windows Form Designer.
        InitializeComponent()

        IsInitializing = False

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
	Public WithEvents cboStore As System.Windows.Forms.ComboBox
	Public WithEvents cmdExit As System.Windows.Forms.Button
	Public WithEvents cmdReport As System.Windows.Forms.Button
	Public WithEvents lblStore As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmLocationsReport))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdExit = New System.Windows.Forms.Button
        Me.cmdReport = New System.Windows.Forms.Button
        Me.cboStore = New System.Windows.Forms.ComboBox
        Me.lblStore = New System.Windows.Forms.Label
        Me.SuspendLayout()
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
        'cboStore
        '
        Me.cboStore.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cboStore.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboStore.BackColor = System.Drawing.SystemColors.Window
        Me.cboStore.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboStore.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.cboStore, "cboStore")
        Me.cboStore.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboStore.Name = "cboStore"
        Me.cboStore.Sorted = True
        '
        'lblStore
        '
        Me.lblStore.BackColor = System.Drawing.Color.Transparent
        Me.lblStore.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblStore, "lblStore")
        Me.lblStore.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblStore.Name = "lblStore"
        '
        'frmLocationsReport
        '
        Me.AcceptButton = Me.cmdReport
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdExit
        Me.Controls.Add(Me.cboStore)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.cmdReport)
        Me.Controls.Add(Me.lblStore)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmLocationsReport"
        Me.ShowInTaskbar = False
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class