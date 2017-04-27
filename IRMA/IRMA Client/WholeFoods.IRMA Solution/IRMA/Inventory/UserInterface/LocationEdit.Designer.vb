<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmLocationEdit
#Region "Windows Form Designer generated code "
	<System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
        MyBase.New()

        isinitializing = True

		'This call is required by the Windows Form Designer.
		InitializeComponent()

        isinitializing = False

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
	Public WithEvents cmdLocItems As System.Windows.Forms.Button
	Public WithEvents cmdApply As System.Windows.Forms.Button
	Public WithEvents cmdExit As System.Windows.Forms.Button
	Public WithEvents cboStore As System.Windows.Forms.ComboBox
	Public WithEvents cboSubTeam As System.Windows.Forms.ComboBox
	Public WithEvents txtLocName As System.Windows.Forms.TextBox
	Public WithEvents txtLocDesc As System.Windows.Forms.TextBox
	Public WithEvents txtNotes As System.Windows.Forms.TextBox
	Public WithEvents Label5 As System.Windows.Forms.Label
	Public WithEvents Label4 As System.Windows.Forms.Label
	Public WithEvents lblLabel As System.Windows.Forms.Label
	Public WithEvents Label1 As System.Windows.Forms.Label
	Public WithEvents Label3 As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmLocationEdit))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdLocItems = New System.Windows.Forms.Button
        Me.cmdApply = New System.Windows.Forms.Button
        Me.cmdExit = New System.Windows.Forms.Button
        Me.cboStore = New System.Windows.Forms.ComboBox
        Me.cboSubTeam = New System.Windows.Forms.ComboBox
        Me.txtLocName = New System.Windows.Forms.TextBox
        Me.txtLocDesc = New System.Windows.Forms.TextBox
        Me.txtNotes = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.lblLabel = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'cmdLocItems
        '
        Me.cmdLocItems.BackColor = System.Drawing.SystemColors.Control
        Me.cmdLocItems.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdLocItems, "cmdLocItems")
        Me.cmdLocItems.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdLocItems.Name = "cmdLocItems"
        Me.ToolTip1.SetToolTip(Me.cmdLocItems, resources.GetString("cmdLocItems.ToolTip"))
        Me.cmdLocItems.UseVisualStyleBackColor = False
        '
        'cmdApply
        '
        Me.cmdApply.BackColor = System.Drawing.SystemColors.Control
        Me.cmdApply.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdApply, "cmdApply")
        Me.cmdApply.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdApply.Name = "cmdApply"
        Me.ToolTip1.SetToolTip(Me.cmdApply, resources.GetString("cmdApply.ToolTip"))
        Me.cmdApply.UseVisualStyleBackColor = False
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
        'cboSubTeam
        '
        Me.cboSubTeam.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cboSubTeam.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboSubTeam.BackColor = System.Drawing.SystemColors.Window
        Me.cboSubTeam.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboSubTeam.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.cboSubTeam, "cboSubTeam")
        Me.cboSubTeam.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboSubTeam.Name = "cboSubTeam"
        Me.cboSubTeam.Sorted = True
        '
        'txtLocName
        '
        Me.txtLocName.AcceptsReturn = True
        Me.txtLocName.BackColor = System.Drawing.SystemColors.Window
        Me.txtLocName.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.txtLocName, "txtLocName")
        Me.txtLocName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtLocName.Name = "txtLocName"
        Me.txtLocName.Tag = "String"
        '
        'txtLocDesc
        '
        Me.txtLocDesc.AcceptsReturn = True
        Me.txtLocDesc.BackColor = System.Drawing.SystemColors.Window
        Me.txtLocDesc.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.txtLocDesc, "txtLocDesc")
        Me.txtLocDesc.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtLocDesc.Name = "txtLocDesc"
        Me.txtLocDesc.Tag = "String"
        '
        'txtNotes
        '
        Me.txtNotes.AcceptsReturn = True
        Me.txtNotes.BackColor = System.Drawing.SystemColors.Window
        Me.txtNotes.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.txtNotes, "txtNotes")
        Me.txtNotes.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtNotes.Name = "txtNotes"
        Me.txtNotes.Tag = "String"
        '
        'Label5
        '
        Me.Label5.BackColor = System.Drawing.SystemColors.Control
        Me.Label5.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.Label5, "Label5")
        Me.Label5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label5.Name = "Label5"
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.SystemColors.Control
        Me.Label4.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.Label4, "Label4")
        Me.Label4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label4.Name = "Label4"
        '
        'lblLabel
        '
        Me.lblLabel.BackColor = System.Drawing.Color.Transparent
        Me.lblLabel.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblLabel, "lblLabel")
        Me.lblLabel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.Name = "lblLabel"
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Name = "Label1"
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.Label3, "Label3")
        Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label3.Name = "Label3"
        '
        'frmLocationEdit
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.Controls.Add(Me.cmdLocItems)
        Me.Controls.Add(Me.cmdApply)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.cboStore)
        Me.Controls.Add(Me.cboSubTeam)
        Me.Controls.Add(Me.txtLocName)
        Me.Controls.Add(Me.txtLocDesc)
        Me.Controls.Add(Me.txtNotes)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.lblLabel)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label3)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmLocationEdit"
        Me.ShowInTaskbar = False
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
#End Region
End Class