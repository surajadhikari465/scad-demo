<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmCycleCountItemAdd
#Region "Windows Form Designer generated code "
    <System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
        MyBase.New()

        IsInitializing = True

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
	Public WithEvents _OptSelection_0 As System.Windows.Forms.RadioButton
	Public WithEvents _OptSelection_1 As System.Windows.Forms.RadioButton
	Public WithEvents _OptSelection_2 As System.Windows.Forms.RadioButton
	Public WithEvents _OptSelection_5 As System.Windows.Forms.RadioButton
	Public WithEvents _OptSelection_4 As System.Windows.Forms.RadioButton
	Public WithEvents _OptSelection_6 As System.Windows.Forms.RadioButton
	Public WithEvents _OptSelection_7 As System.Windows.Forms.RadioButton
	Public WithEvents Frame1 As System.Windows.Forms.GroupBox
	Public WithEvents cmbSelection As System.Windows.Forms.ComboBox
	Public WithEvents txtNumber As System.Windows.Forms.TextBox
	Public WithEvents lblSelection As System.Windows.Forms.Label
	Public WithEvents frmSelection As System.Windows.Forms.GroupBox
	Public WithEvents cmdAdd As System.Windows.Forms.Button
	Public WithEvents cmdExit As System.Windows.Forms.Button
	Public WithEvents OptSelection As Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCycleCountItemAdd))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdAdd = New System.Windows.Forms.Button
        Me.cmdExit = New System.Windows.Forms.Button
        Me.Frame1 = New System.Windows.Forms.GroupBox
        Me._OptSelection_0 = New System.Windows.Forms.RadioButton
        Me._OptSelection_1 = New System.Windows.Forms.RadioButton
        Me._OptSelection_2 = New System.Windows.Forms.RadioButton
        Me._OptSelection_5 = New System.Windows.Forms.RadioButton
        Me._OptSelection_4 = New System.Windows.Forms.RadioButton
        Me._OptSelection_6 = New System.Windows.Forms.RadioButton
        Me._OptSelection_7 = New System.Windows.Forms.RadioButton
        Me.frmSelection = New System.Windows.Forms.GroupBox
        Me.cmbSelection = New System.Windows.Forms.ComboBox
        Me.txtNumber = New System.Windows.Forms.TextBox
        Me.lblSelection = New System.Windows.Forms.Label
        Me.OptSelection = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(Me.components)
        Me.Frame1.SuspendLayout()
        Me.frmSelection.SuspendLayout()
        CType(Me.OptSelection, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdAdd
        '
        Me.cmdAdd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAdd.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdAdd, "cmdAdd")
        Me.cmdAdd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAdd.Name = "cmdAdd"
        Me.ToolTip1.SetToolTip(Me.cmdAdd, resources.GetString("cmdAdd.ToolTip"))
        Me.cmdAdd.UseVisualStyleBackColor = False
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
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me._OptSelection_0)
        Me.Frame1.Controls.Add(Me._OptSelection_1)
        Me.Frame1.Controls.Add(Me._OptSelection_2)
        Me.Frame1.Controls.Add(Me._OptSelection_5)
        Me.Frame1.Controls.Add(Me._OptSelection_4)
        Me.Frame1.Controls.Add(Me._OptSelection_6)
        Me.Frame1.Controls.Add(Me._OptSelection_7)
        resources.ApplyResources(Me.Frame1, "Frame1")
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Name = "Frame1"
        Me.Frame1.TabStop = False
        '
        '_OptSelection_0
        '
        Me._OptSelection_0.BackColor = System.Drawing.SystemColors.Control
        Me._OptSelection_0.Checked = True
        Me._OptSelection_0.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._OptSelection_0, "_OptSelection_0")
        Me._OptSelection_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.OptSelection.SetIndex(Me._OptSelection_0, CType(0, Short))
        Me._OptSelection_0.Name = "_OptSelection_0"
        Me._OptSelection_0.TabStop = True
        Me._OptSelection_0.UseVisualStyleBackColor = False
        '
        '_OptSelection_1
        '
        Me._OptSelection_1.BackColor = System.Drawing.SystemColors.Control
        Me._OptSelection_1.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._OptSelection_1, "_OptSelection_1")
        Me._OptSelection_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.OptSelection.SetIndex(Me._OptSelection_1, CType(1, Short))
        Me._OptSelection_1.Name = "_OptSelection_1"
        Me._OptSelection_1.TabStop = True
        Me._OptSelection_1.UseVisualStyleBackColor = False
        '
        '_OptSelection_2
        '
        Me._OptSelection_2.BackColor = System.Drawing.SystemColors.Control
        Me._OptSelection_2.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._OptSelection_2, "_OptSelection_2")
        Me._OptSelection_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.OptSelection.SetIndex(Me._OptSelection_2, CType(2, Short))
        Me._OptSelection_2.Name = "_OptSelection_2"
        Me._OptSelection_2.TabStop = True
        Me._OptSelection_2.UseVisualStyleBackColor = False
        '
        '_OptSelection_5
        '
        Me._OptSelection_5.BackColor = System.Drawing.SystemColors.Control
        Me._OptSelection_5.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._OptSelection_5, "_OptSelection_5")
        Me._OptSelection_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.OptSelection.SetIndex(Me._OptSelection_5, CType(5, Short))
        Me._OptSelection_5.Name = "_OptSelection_5"
        Me._OptSelection_5.TabStop = True
        Me._OptSelection_5.UseVisualStyleBackColor = False
        '
        '_OptSelection_4
        '
        Me._OptSelection_4.BackColor = System.Drawing.SystemColors.Control
        Me._OptSelection_4.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._OptSelection_4, "_OptSelection_4")
        Me._OptSelection_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.OptSelection.SetIndex(Me._OptSelection_4, CType(4, Short))
        Me._OptSelection_4.Name = "_OptSelection_4"
        Me._OptSelection_4.TabStop = True
        Me._OptSelection_4.UseVisualStyleBackColor = False
        '
        '_OptSelection_6
        '
        Me._OptSelection_6.BackColor = System.Drawing.SystemColors.Control
        Me._OptSelection_6.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._OptSelection_6, "_OptSelection_6")
        Me._OptSelection_6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.OptSelection.SetIndex(Me._OptSelection_6, CType(6, Short))
        Me._OptSelection_6.Name = "_OptSelection_6"
        Me._OptSelection_6.TabStop = True
        Me._OptSelection_6.UseVisualStyleBackColor = False
        '
        '_OptSelection_7
        '
        Me._OptSelection_7.BackColor = System.Drawing.SystemColors.Control
        Me._OptSelection_7.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._OptSelection_7, "_OptSelection_7")
        Me._OptSelection_7.ForeColor = System.Drawing.SystemColors.ControlText
        Me.OptSelection.SetIndex(Me._OptSelection_7, CType(7, Short))
        Me._OptSelection_7.Name = "_OptSelection_7"
        Me._OptSelection_7.TabStop = True
        Me._OptSelection_7.UseVisualStyleBackColor = False
        '
        'frmSelection
        '
        Me.frmSelection.BackColor = System.Drawing.SystemColors.Control
        Me.frmSelection.Controls.Add(Me.cmbSelection)
        Me.frmSelection.Controls.Add(Me.txtNumber)
        Me.frmSelection.Controls.Add(Me.lblSelection)
        resources.ApplyResources(Me.frmSelection, "frmSelection")
        Me.frmSelection.ForeColor = System.Drawing.SystemColors.ControlText
        Me.frmSelection.Name = "frmSelection"
        Me.frmSelection.TabStop = False
        '
        'cmbSelection
        '
        Me.cmbSelection.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbSelection.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbSelection.BackColor = System.Drawing.SystemColors.Window
        Me.cmbSelection.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbSelection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.cmbSelection, "cmbSelection")
        Me.cmbSelection.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbSelection.Name = "cmbSelection"
        Me.cmbSelection.Sorted = True
        '
        'txtNumber
        '
        Me.txtNumber.AcceptsReturn = True
        Me.txtNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.txtNumber, "txtNumber")
        Me.txtNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtNumber.Name = "txtNumber"
        '
        'lblSelection
        '
        Me.lblSelection.BackColor = System.Drawing.Color.Transparent
        Me.lblSelection.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblSelection, "lblSelection")
        Me.lblSelection.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSelection.Name = "lblSelection"
        '
        'OptSelection
        '
        '
        'frmCycleCountItemAdd
        '
        Me.AcceptButton = Me.cmdAdd
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.Controls.Add(Me.Frame1)
        Me.Controls.Add(Me.frmSelection)
        Me.Controls.Add(Me.cmdAdd)
        Me.Controls.Add(Me.cmdExit)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmCycleCountItemAdd"
        Me.Frame1.ResumeLayout(False)
        Me.frmSelection.ResumeLayout(False)
        Me.frmSelection.PerformLayout()
        CType(Me.OptSelection, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class