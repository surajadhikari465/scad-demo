<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmCycleCountHistoryEdit
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
    Public WithEvents cmdExit As System.Windows.Forms.Button
    Public WithEvents txtDateTime As System.Windows.Forms.TextBox
	Public WithEvents txtItemDesc As System.Windows.Forms.TextBox
	Public WithEvents _lblLabel_7 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_0 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_6 As System.Windows.Forms.Label
	Public WithEvents lblLabel As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCycleCountHistoryEdit))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdExit = New System.Windows.Forms.Button
        Me.txtDateTime = New System.Windows.Forms.TextBox
        Me.txtItemDesc = New System.Windows.Forms.TextBox
        Me._lblLabel_7 = New System.Windows.Forms.Label
        Me._lblLabel_0 = New System.Windows.Forms.Label
        Me._lblLabel_6 = New System.Windows.Forms.Label
        Me.lblLabel = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(Me.components)
        Me.fraUnits = New System.Windows.Forms.GroupBox
        Me.txtUnits2 = New Infragistics.Win.UltraWinEditors.UltraNumericEditor
        Me.txtCases = New Infragistics.Win.UltraWinEditors.UltraNumericEditor
        Me.txtUnits = New Infragistics.Win.UltraWinEditors.UltraNumericEditor
        Me.optUnits2 = New System.Windows.Forms.RadioButton
        Me.optCases = New System.Windows.Forms.RadioButton
        Me.optUnits = New System.Windows.Forms.RadioButton
        Me.lblUnits2 = New System.Windows.Forms.Label
        Me.lblCases = New System.Windows.Forms.Label
        Me.lblUnits = New System.Windows.Forms.Label
        Me._lblLabel_4 = New System.Windows.Forms.Label
        Me.txtPackSize = New Infragistics.Win.UltraWinEditors.UltraNumericEditor
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.fraUnits.SuspendLayout()
        CType(Me.txtUnits2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtCases, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtUnits, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtPackSize, System.ComponentModel.ISupportInitialize).BeginInit()
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
        'txtDateTime
        '
        Me.txtDateTime.AcceptsReturn = True
        Me.txtDateTime.BackColor = System.Drawing.SystemColors.ControlLight
        Me.txtDateTime.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.txtDateTime, "txtDateTime")
        Me.txtDateTime.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDateTime.Name = "txtDateTime"
        Me.txtDateTime.ReadOnly = True
        Me.txtDateTime.TabStop = False
        Me.txtDateTime.Tag = "Integer"
        '
        'txtItemDesc
        '
        Me.txtItemDesc.AcceptsReturn = True
        Me.txtItemDesc.BackColor = System.Drawing.SystemColors.ControlLight
        Me.txtItemDesc.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.txtItemDesc, "txtItemDesc")
        Me.txtItemDesc.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtItemDesc.Name = "txtItemDesc"
        Me.txtItemDesc.ReadOnly = True
        Me.txtItemDesc.TabStop = False
        Me.txtItemDesc.Tag = "Integer"
        '
        '_lblLabel_7
        '
        Me._lblLabel_7.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_7.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lblLabel_7, "_lblLabel_7")
        Me._lblLabel_7.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_7, CType(7, Short))
        Me._lblLabel_7.Name = "_lblLabel_7"
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
        '_lblLabel_6
        '
        Me._lblLabel_6.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_6.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lblLabel_6, "_lblLabel_6")
        Me._lblLabel_6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_6, CType(6, Short))
        Me._lblLabel_6.Name = "_lblLabel_6"
        '
        'fraUnits
        '
        Me.fraUnits.BackColor = System.Drawing.SystemColors.Control
        Me.fraUnits.Controls.Add(Me.txtUnits2)
        Me.fraUnits.Controls.Add(Me.txtCases)
        Me.fraUnits.Controls.Add(Me.txtUnits)
        Me.fraUnits.Controls.Add(Me.optUnits2)
        Me.fraUnits.Controls.Add(Me.optCases)
        Me.fraUnits.Controls.Add(Me.optUnits)
        Me.fraUnits.Controls.Add(Me.lblUnits2)
        Me.fraUnits.Controls.Add(Me.lblCases)
        Me.fraUnits.Controls.Add(Me.lblUnits)
        Me.fraUnits.Controls.Add(Me._lblLabel_4)
        resources.ApplyResources(Me.fraUnits, "fraUnits")
        Me.fraUnits.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraUnits.Name = "fraUnits"
        Me.fraUnits.TabStop = False
        '
        'txtUnits2
        '
        resources.ApplyResources(Me.txtUnits2, "txtUnits2")
        Me.txtUnits2.FormatString = "#####.00"
        Me.txtUnits2.MaskInput = "{double:5.2}"
        Me.txtUnits2.MaxValue = 999999999
        Me.txtUnits2.MinValue = 0
        Me.txtUnits2.Name = "txtUnits2"
        Me.txtUnits2.Nullable = True
        Me.txtUnits2.NumericType = Infragistics.Win.UltraWinEditors.NumericType.[Double]
        '
        'txtCases
        '
        resources.ApplyResources(Me.txtCases, "txtCases")
        Me.txtCases.FormatString = "#####.00"
        Me.txtCases.MaskInput = "{double:4.2}"
        Me.txtCases.MaxValue = 99999
        Me.txtCases.MinValue = 0
        Me.txtCases.Name = "txtCases"
        Me.txtCases.Nullable = True
        Me.txtCases.NumericType = Infragistics.Win.UltraWinEditors.NumericType.[Double]
        '
        'txtUnits
        '
        resources.ApplyResources(Me.txtUnits, "txtUnits")
        Me.txtUnits.FormatString = "#########.00"
        Me.txtUnits.MaskInput = "{double:5.2}"
        Me.txtUnits.MaxValue = 999999999
        Me.txtUnits.MinValue = 0
        Me.txtUnits.Name = "txtUnits"
        Me.txtUnits.Nullable = True
        Me.txtUnits.NumericType = Infragistics.Win.UltraWinEditors.NumericType.[Double]
        '
        'optUnits2
        '
        Me.optUnits2.BackColor = System.Drawing.SystemColors.Control
        Me.optUnits2.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.optUnits2, "optUnits2")
        Me.optUnits2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optUnits2.Name = "optUnits2"
        Me.optUnits2.TabStop = True
        Me.optUnits2.UseVisualStyleBackColor = False
        '
        'optCases
        '
        Me.optCases.BackColor = System.Drawing.SystemColors.Control
        Me.optCases.Checked = True
        Me.optCases.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.optCases, "optCases")
        Me.optCases.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optCases.Name = "optCases"
        Me.optCases.TabStop = True
        Me.optCases.UseVisualStyleBackColor = False
        '
        'optUnits
        '
        Me.optUnits.BackColor = System.Drawing.SystemColors.Control
        Me.optUnits.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.optUnits, "optUnits")
        Me.optUnits.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optUnits.Name = "optUnits"
        Me.optUnits.TabStop = True
        Me.optUnits.UseVisualStyleBackColor = False
        '
        'lblUnits2
        '
        Me.lblUnits2.BackColor = System.Drawing.Color.Transparent
        Me.lblUnits2.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblUnits2, "lblUnits2")
        Me.lblUnits2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblUnits2.Name = "lblUnits2"
        '
        'lblCases
        '
        Me.lblCases.BackColor = System.Drawing.Color.Transparent
        Me.lblCases.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblCases, "lblCases")
        Me.lblCases.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCases.Name = "lblCases"
        '
        'lblUnits
        '
        Me.lblUnits.BackColor = System.Drawing.Color.Transparent
        Me.lblUnits.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblUnits, "lblUnits")
        Me.lblUnits.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblUnits.Name = "lblUnits"
        '
        '_lblLabel_4
        '
        Me._lblLabel_4.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_4.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._lblLabel_4, "_lblLabel_4")
        Me._lblLabel_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLabel_4.Name = "_lblLabel_4"
        '
        'txtPackSize
        '
        resources.ApplyResources(Me.txtPackSize, "txtPackSize")
        Me.txtPackSize.FormatString = "####.00"
        Me.txtPackSize.MaskInput = "{double:4.2}"
        Me.txtPackSize.MaxValue = 9999
        Me.txtPackSize.MinValue = 0
        Me.txtPackSize.Name = "txtPackSize"
        Me.txtPackSize.Nullable = True
        Me.txtPackSize.NumericType = Infragistics.Win.UltraWinEditors.NumericType.[Double]
        '
        'frmCycleCountHistoryEdit
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.Controls.Add(Me.txtPackSize)
        Me.Controls.Add(Me.fraUnits)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.txtDateTime)
        Me.Controls.Add(Me.txtItemDesc)
        Me.Controls.Add(Me._lblLabel_7)
        Me.Controls.Add(Me._lblLabel_0)
        Me.Controls.Add(Me._lblLabel_6)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmCycleCountHistoryEdit"
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).EndInit()
        Me.fraUnits.ResumeLayout(False)
        Me.fraUnits.PerformLayout()
        CType(Me.txtUnits2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtCases, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtUnits, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtPackSize, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Public WithEvents fraUnits As System.Windows.Forms.GroupBox
    Public WithEvents optUnits2 As System.Windows.Forms.RadioButton
    Public WithEvents optCases As System.Windows.Forms.RadioButton
    Public WithEvents optUnits As System.Windows.Forms.RadioButton
    Public WithEvents lblUnits2 As System.Windows.Forms.Label
    Public WithEvents lblCases As System.Windows.Forms.Label
    Public WithEvents lblUnits As System.Windows.Forms.Label
    Public WithEvents _lblLabel_4 As System.Windows.Forms.Label
    Friend WithEvents txtPackSize As Infragistics.Win.UltraWinEditors.UltraNumericEditor
    Friend WithEvents txtUnits2 As Infragistics.Win.UltraWinEditors.UltraNumericEditor
    Friend WithEvents txtCases As Infragistics.Win.UltraWinEditors.UltraNumericEditor
    Friend WithEvents txtUnits As Infragistics.Win.UltraWinEditors.UltraNumericEditor
#End Region 
End Class