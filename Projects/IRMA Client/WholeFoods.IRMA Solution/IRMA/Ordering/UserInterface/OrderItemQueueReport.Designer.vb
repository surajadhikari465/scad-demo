<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmOrderItemQueueReport
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
	Public WithEvents _optSortBy_2 As System.Windows.Forms.RadioButton
	Public WithEvents _optSortBy_1 As System.Windows.Forms.RadioButton
	Public WithEvents _optSortBy_0 As System.Windows.Forms.RadioButton
	Public WithEvents fraSort As System.Windows.Forms.GroupBox
	Public WithEvents cmdReports As System.Windows.Forms.Button
	Public WithEvents chkPrintOnly As System.Windows.Forms.CheckBox
	Public WithEvents cmdExit As System.Windows.Forms.Button
    Public WithEvents optSortBy As Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmOrderItemQueueReport))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdReports = New System.Windows.Forms.Button
        Me.cmdExit = New System.Windows.Forms.Button
        Me.fraSort = New System.Windows.Forms.GroupBox
        Me._optSortBy_2 = New System.Windows.Forms.RadioButton
        Me._optSortBy_1 = New System.Windows.Forms.RadioButton
        Me._optSortBy_0 = New System.Windows.Forms.RadioButton
        Me.chkPrintOnly = New System.Windows.Forms.CheckBox
        Me.optSortBy = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(Me.components)
        Me.fraSort.SuspendLayout()
        CType(Me.optSortBy, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdReports
        '
        Me.cmdReports.BackColor = System.Drawing.SystemColors.Control
        Me.cmdReports.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdReports.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdReports.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdReports.Image = CType(resources.GetObject("cmdReports.Image"), System.Drawing.Image)
        Me.cmdReports.Location = New System.Drawing.Point(64, 136)
        Me.cmdReports.Name = "cmdReports"
        Me.cmdReports.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdReports.Size = New System.Drawing.Size(41, 41)
        Me.cmdReports.TabIndex = 2
        Me.cmdReports.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdReports, "Order Reports")
        Me.cmdReports.UseVisualStyleBackColor = False
        '
        'cmdExit
        '
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdExit.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Image = CType(resources.GetObject("cmdExit.Image"), System.Drawing.Image)
        Me.cmdExit.Location = New System.Drawing.Point(112, 136)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(41, 41)
        Me.cmdExit.TabIndex = 0
        Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdExit, "Exit")
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'fraSort
        '
        Me.fraSort.BackColor = System.Drawing.SystemColors.Control
        Me.fraSort.Controls.Add(Me._optSortBy_2)
        Me.fraSort.Controls.Add(Me._optSortBy_1)
        Me.fraSort.Controls.Add(Me._optSortBy_0)
        Me.fraSort.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraSort.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraSort.Location = New System.Drawing.Point(8, 8)
        Me.fraSort.Name = "fraSort"
        Me.fraSort.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraSort.Size = New System.Drawing.Size(145, 97)
        Me.fraSort.TabIndex = 3
        Me.fraSort.TabStop = False
        Me.fraSort.Text = "Sort Order"
        '
        '_optSortBy_2
        '
        Me._optSortBy_2.BackColor = System.Drawing.SystemColors.Control
        Me._optSortBy_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._optSortBy_2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optSortBy_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optSortBy.SetIndex(Me._optSortBy_2, CType(2, Short))
        Me._optSortBy_2.Location = New System.Drawing.Point(24, 72)
        Me._optSortBy_2.Name = "_optSortBy_2"
        Me._optSortBy_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optSortBy_2.Size = New System.Drawing.Size(103, 19)
        Me._optSortBy_2.TabIndex = 6
        Me._optSortBy_2.TabStop = True
        Me._optSortBy_2.Text = "Identifier"
        Me._optSortBy_2.UseVisualStyleBackColor = False
        '
        '_optSortBy_1
        '
        Me._optSortBy_1.BackColor = System.Drawing.SystemColors.Control
        Me._optSortBy_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._optSortBy_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optSortBy_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optSortBy.SetIndex(Me._optSortBy_1, CType(1, Short))
        Me._optSortBy_1.Location = New System.Drawing.Point(24, 48)
        Me._optSortBy_1.Name = "_optSortBy_1"
        Me._optSortBy_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optSortBy_1.Size = New System.Drawing.Size(89, 17)
        Me._optSortBy_1.TabIndex = 5
        Me._optSortBy_1.TabStop = True
        Me._optSortBy_1.Text = "Description"
        Me._optSortBy_1.UseVisualStyleBackColor = False
        '
        '_optSortBy_0
        '
        Me._optSortBy_0.BackColor = System.Drawing.SystemColors.Control
        Me._optSortBy_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._optSortBy_0.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optSortBy_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optSortBy.SetIndex(Me._optSortBy_0, CType(0, Short))
        Me._optSortBy_0.Location = New System.Drawing.Point(24, 24)
        Me._optSortBy_0.Name = "_optSortBy_0"
        Me._optSortBy_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optSortBy_0.Size = New System.Drawing.Size(113, 17)
        Me._optSortBy_0.TabIndex = 4
        Me._optSortBy_0.TabStop = True
        Me._optSortBy_0.Text = "Primary Vendor"
        Me._optSortBy_0.UseVisualStyleBackColor = False
        '
        'chkPrintOnly
        '
        Me.chkPrintOnly.BackColor = System.Drawing.SystemColors.Control
        Me.chkPrintOnly.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkPrintOnly.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkPrintOnly.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPrintOnly.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkPrintOnly.Location = New System.Drawing.Point(64, 112)
        Me.chkPrintOnly.Name = "chkPrintOnly"
        Me.chkPrintOnly.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkPrintOnly.Size = New System.Drawing.Size(89, 17)
        Me.chkPrintOnly.TabIndex = 1
        Me.chkPrintOnly.Text = "Print Only"
        Me.chkPrintOnly.UseVisualStyleBackColor = False
        '
        'optSortBy
        '
        '
        'frmOrderItemQueueReport
        '
        Me.AcceptButton = Me.cmdReports
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdExit
        Me.ClientSize = New System.Drawing.Size(161, 185)
        Me.Controls.Add(Me.fraSort)
        Me.Controls.Add(Me.cmdReports)
        Me.Controls.Add(Me.chkPrintOnly)
        Me.Controls.Add(Me.cmdExit)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Location = New System.Drawing.Point(3, 29)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmOrderItemQueueReport"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Queue Report"
        Me.fraSort.ResumeLayout(False)
        CType(Me.optSortBy, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class