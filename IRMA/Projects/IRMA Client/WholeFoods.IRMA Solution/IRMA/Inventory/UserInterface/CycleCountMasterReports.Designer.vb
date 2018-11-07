<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmCycleCountMasterReports
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
	Public WithEvents chkRetail As System.Windows.Forms.CheckBox
	Public WithEvents chkIngredients As System.Windows.Forms.CheckBox
	Public WithEvents optMasterSummary As System.Windows.Forms.RadioButton
	Public WithEvents optMasterDetail As System.Windows.Forms.RadioButton
	Public WithEvents fraReportType As System.Windows.Forms.GroupBox
	Public WithEvents cmdReport As System.Windows.Forms.Button
	Public WithEvents cmdExit As System.Windows.Forms.Button
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCycleCountMasterReports))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdReport = New System.Windows.Forms.Button
        Me.cmdExit = New System.Windows.Forms.Button
        Me.fraReportType = New System.Windows.Forms.GroupBox
        Me.chkRetail = New System.Windows.Forms.CheckBox
        Me.chkIngredients = New System.Windows.Forms.CheckBox
        Me.optMasterSummary = New System.Windows.Forms.RadioButton
        Me.optMasterDetail = New System.Windows.Forms.RadioButton
        Me.fraReportType.SuspendLayout()
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
        resources.ApplyResources(Me.cmdExit, "cmdExit")
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Name = "cmdExit"
        Me.ToolTip1.SetToolTip(Me.cmdExit, resources.GetString("cmdExit.ToolTip"))
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'fraReportType
        '
        Me.fraReportType.BackColor = System.Drawing.SystemColors.Control
        Me.fraReportType.Controls.Add(Me.chkRetail)
        Me.fraReportType.Controls.Add(Me.chkIngredients)
        Me.fraReportType.Controls.Add(Me.optMasterSummary)
        Me.fraReportType.Controls.Add(Me.optMasterDetail)
        resources.ApplyResources(Me.fraReportType, "fraReportType")
        Me.fraReportType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraReportType.Name = "fraReportType"
        Me.fraReportType.TabStop = False
        '
        'chkRetail
        '
        Me.chkRetail.BackColor = System.Drawing.SystemColors.Control
        Me.chkRetail.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.chkRetail, "chkRetail")
        Me.chkRetail.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkRetail.Name = "chkRetail"
        Me.chkRetail.UseVisualStyleBackColor = False
        '
        'chkIngredients
        '
        Me.chkIngredients.BackColor = System.Drawing.SystemColors.Control
        Me.chkIngredients.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.chkIngredients, "chkIngredients")
        Me.chkIngredients.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIngredients.Name = "chkIngredients"
        Me.chkIngredients.UseVisualStyleBackColor = False
        '
        'optMasterSummary
        '
        Me.optMasterSummary.BackColor = System.Drawing.SystemColors.Control
        Me.optMasterSummary.Checked = True
        Me.optMasterSummary.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.optMasterSummary, "optMasterSummary")
        Me.optMasterSummary.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optMasterSummary.Name = "optMasterSummary"
        Me.optMasterSummary.TabStop = True
        Me.optMasterSummary.UseVisualStyleBackColor = False
        '
        'optMasterDetail
        '
        Me.optMasterDetail.BackColor = System.Drawing.SystemColors.Control
        Me.optMasterDetail.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.optMasterDetail, "optMasterDetail")
        Me.optMasterDetail.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optMasterDetail.Name = "optMasterDetail"
        Me.optMasterDetail.TabStop = True
        Me.optMasterDetail.UseVisualStyleBackColor = False
        '
        'frmCycleCountMasterReports
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdExit
        Me.Controls.Add(Me.fraReportType)
        Me.Controls.Add(Me.cmdReport)
        Me.Controls.Add(Me.cmdExit)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmCycleCountMasterReports"
        Me.fraReportType.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class