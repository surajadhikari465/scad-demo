<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmCycleCountMasterClose
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
	Public WithEvents chkResetInvCnt As System.Windows.Forms.CheckBox
	Public WithEvents cmdClose As System.Windows.Forms.Button
	Public WithEvents cmdExit As System.Windows.Forms.Button
	Public WithEvents chkZeroItems As System.Windows.Forms.CheckBox
	Public WithEvents _optCloseStamp_0 As System.Windows.Forms.RadioButton
	Public WithEvents _optCloseStamp_1 As System.Windows.Forms.RadioButton
	Public WithEvents _optCloseStamp_2 As System.Windows.Forms.RadioButton
	Public WithEvents _optCloseStamp_3 As System.Windows.Forms.RadioButton
	Public WithEvents Frame1 As System.Windows.Forms.GroupBox
	Public WithEvents optCloseStamp As Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCycleCountMasterClose))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdClose = New System.Windows.Forms.Button
        Me.cmdExit = New System.Windows.Forms.Button
        Me.chkResetInvCnt = New System.Windows.Forms.CheckBox
        Me.chkZeroItems = New System.Windows.Forms.CheckBox
        Me.Frame1 = New System.Windows.Forms.GroupBox
        Me._optCloseStamp_0 = New System.Windows.Forms.RadioButton
        Me._optCloseStamp_1 = New System.Windows.Forms.RadioButton
        Me._optCloseStamp_2 = New System.Windows.Forms.RadioButton
        Me._optCloseStamp_3 = New System.Windows.Forms.RadioButton
        Me.optCloseStamp = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(Me.components)
        Me.Frame1.SuspendLayout()
        CType(Me.optCloseStamp, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdClose
        '
        Me.cmdClose.BackColor = System.Drawing.SystemColors.Control
        Me.cmdClose.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdClose, "cmdClose")
        Me.cmdClose.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdClose.Name = "cmdClose"
        Me.ToolTip1.SetToolTip(Me.cmdClose, resources.GetString("cmdClose.ToolTip"))
        Me.cmdClose.UseVisualStyleBackColor = False
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
        'chkResetInvCnt
        '
        Me.chkResetInvCnt.BackColor = System.Drawing.SystemColors.Control
        Me.chkResetInvCnt.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.chkResetInvCnt, "chkResetInvCnt")
        Me.chkResetInvCnt.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkResetInvCnt.Name = "chkResetInvCnt"
        Me.chkResetInvCnt.UseVisualStyleBackColor = False
        '
        'chkZeroItems
        '
        Me.chkZeroItems.BackColor = System.Drawing.SystemColors.Control
        Me.chkZeroItems.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.chkZeroItems, "chkZeroItems")
        Me.chkZeroItems.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkZeroItems.Name = "chkZeroItems"
        Me.chkZeroItems.UseVisualStyleBackColor = False
        '
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me._optCloseStamp_0)
        Me.Frame1.Controls.Add(Me._optCloseStamp_1)
        Me.Frame1.Controls.Add(Me._optCloseStamp_2)
        Me.Frame1.Controls.Add(Me._optCloseStamp_3)
        resources.ApplyResources(Me.Frame1, "Frame1")
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Name = "Frame1"
        Me.Frame1.TabStop = False
        '
        '_optCloseStamp_0
        '
        Me._optCloseStamp_0.BackColor = System.Drawing.SystemColors.Control
        Me._optCloseStamp_0.Checked = True
        Me._optCloseStamp_0.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._optCloseStamp_0, "_optCloseStamp_0")
        Me._optCloseStamp_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optCloseStamp.SetIndex(Me._optCloseStamp_0, CType(0, Short))
        Me._optCloseStamp_0.Name = "_optCloseStamp_0"
        Me._optCloseStamp_0.TabStop = True
        Me._optCloseStamp_0.UseVisualStyleBackColor = False
        '
        '_optCloseStamp_1
        '
        Me._optCloseStamp_1.BackColor = System.Drawing.SystemColors.Control
        Me._optCloseStamp_1.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._optCloseStamp_1, "_optCloseStamp_1")
        Me._optCloseStamp_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optCloseStamp.SetIndex(Me._optCloseStamp_1, CType(1, Short))
        Me._optCloseStamp_1.Name = "_optCloseStamp_1"
        Me._optCloseStamp_1.TabStop = True
        Me._optCloseStamp_1.UseVisualStyleBackColor = False
        '
        '_optCloseStamp_2
        '
        Me._optCloseStamp_2.BackColor = System.Drawing.SystemColors.Control
        Me._optCloseStamp_2.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._optCloseStamp_2, "_optCloseStamp_2")
        Me._optCloseStamp_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optCloseStamp.SetIndex(Me._optCloseStamp_2, CType(2, Short))
        Me._optCloseStamp_2.Name = "_optCloseStamp_2"
        Me._optCloseStamp_2.TabStop = True
        Me._optCloseStamp_2.UseVisualStyleBackColor = False
        '
        '_optCloseStamp_3
        '
        Me._optCloseStamp_3.BackColor = System.Drawing.SystemColors.Control
        Me._optCloseStamp_3.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._optCloseStamp_3, "_optCloseStamp_3")
        Me._optCloseStamp_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optCloseStamp.SetIndex(Me._optCloseStamp_3, CType(3, Short))
        Me._optCloseStamp_3.Name = "_optCloseStamp_3"
        Me._optCloseStamp_3.TabStop = True
        Me._optCloseStamp_3.UseVisualStyleBackColor = False
        '
        'frmCycleCountMasterClose
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdExit
        Me.Controls.Add(Me.chkResetInvCnt)
        Me.Controls.Add(Me.cmdClose)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.chkZeroItems)
        Me.Controls.Add(Me.Frame1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmCycleCountMasterClose"
        Me.Frame1.ResumeLayout(False)
        CType(Me.optCloseStamp, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class