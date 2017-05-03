<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmSpecialsByEndDate
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
    Public WithEvents _optDateType_1 As System.Windows.Forms.RadioButton
	Public WithEvents _optDateType_0 As System.Windows.Forms.RadioButton
	Public WithEvents _lblLabel_0 As System.Windows.Forms.Label
	Public WithEvents fraDate As System.Windows.Forms.GroupBox
	Public WithEvents chkPrintOnly As System.Windows.Forms.CheckBox
	Public WithEvents _cmbField_0 As System.Windows.Forms.ComboBox
	Public WithEvents _cmbField_1 As System.Windows.Forms.ComboBox
	Public WithEvents cmdReport As System.Windows.Forms.Button
	Public WithEvents cmdExit As System.Windows.Forms.Button
    Public WithEvents _lblLabel_1 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_5 As System.Windows.Forms.Label
	Public WithEvents cmbField As Microsoft.VisualBasic.Compatibility.VB6.ComboBoxArray
	Public WithEvents lblLabel As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	Public WithEvents optDateType As Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray
	Public WithEvents txtField As Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSpecialsByEndDate))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdReport = New System.Windows.Forms.Button
        Me.cmdExit = New System.Windows.Forms.Button
        Me.fraDate = New System.Windows.Forms.GroupBox
        Me.dtpEndDate = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
        Me.dtpStartDate = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
        Me._optDateType_1 = New System.Windows.Forms.RadioButton
        Me._optDateType_0 = New System.Windows.Forms.RadioButton
        Me._lblLabel_0 = New System.Windows.Forms.Label
        Me.chkPrintOnly = New System.Windows.Forms.CheckBox
        Me._cmbField_0 = New System.Windows.Forms.ComboBox
        Me._cmbField_1 = New System.Windows.Forms.ComboBox
        Me._lblLabel_1 = New System.Windows.Forms.Label
        Me._lblLabel_5 = New System.Windows.Forms.Label
        Me.cmbField = New Microsoft.VisualBasic.Compatibility.VB6.ComboBoxArray(Me.components)
        Me.lblLabel = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(Me.components)
        Me.optDateType = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(Me.components)
        Me.txtField = New Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray(Me.components)
        Me.fraDate.SuspendLayout()
        CType(Me.dtpEndDate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dtpStartDate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cmbField, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.optDateType, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtField, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdReport
        '
        Me.cmdReport.BackColor = System.Drawing.SystemColors.Control
        Me.cmdReport.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdReport.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdReport.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdReport.Image = CType(resources.GetObject("cmdReport.Image"), System.Drawing.Image)
        Me.cmdReport.Location = New System.Drawing.Point(200, 176)
        Me.cmdReport.Name = "cmdReport"
        Me.cmdReport.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdReport.Size = New System.Drawing.Size(41, 41)
        Me.cmdReport.TabIndex = 3
        Me.cmdReport.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdReport, "Print")
        Me.cmdReport.UseVisualStyleBackColor = False
        '
        'cmdExit
        '
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdExit.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Image = CType(resources.GetObject("cmdExit.Image"), System.Drawing.Image)
        Me.cmdExit.Location = New System.Drawing.Point(248, 176)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(41, 41)
        Me.cmdExit.TabIndex = 4
        Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdExit, "Exit")
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'fraDate
        '
        Me.fraDate.BackColor = System.Drawing.SystemColors.Control
        Me.fraDate.Controls.Add(Me.dtpEndDate)
        Me.fraDate.Controls.Add(Me.dtpStartDate)
        Me.fraDate.Controls.Add(Me._optDateType_1)
        Me.fraDate.Controls.Add(Me._optDateType_0)
        Me.fraDate.Controls.Add(Me._lblLabel_0)
        Me.fraDate.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraDate.Location = New System.Drawing.Point(8, 64)
        Me.fraDate.Name = "fraDate"
        Me.fraDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraDate.Size = New System.Drawing.Size(281, 73)
        Me.fraDate.TabIndex = 2
        Me.fraDate.TabStop = False
        Me.fraDate.Text = "Dates"
        '
        'dtpEndDate
        '
        Me.dtpEndDate.Location = New System.Drawing.Point(160, 36)
        Me.dtpEndDate.Name = "dtpEndDate"
        Me.dtpEndDate.Size = New System.Drawing.Size(85, 21)
        Me.dtpEndDate.TabIndex = 3
        '
        'dtpStartDate
        '
        Me.dtpStartDate.Location = New System.Drawing.Point(53, 36)
        Me.dtpStartDate.Name = "dtpStartDate"
        Me.dtpStartDate.Size = New System.Drawing.Size(85, 21)
        Me.dtpStartDate.TabIndex = 2
        '
        '_optDateType_1
        '
        Me._optDateType_1.BackColor = System.Drawing.SystemColors.Control
        Me._optDateType_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._optDateType_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optDateType_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optDateType.SetIndex(Me._optDateType_1, CType(1, Short))
        Me._optDateType_1.Location = New System.Drawing.Point(160, 16)
        Me._optDateType_1.Name = "_optDateType_1"
        Me._optDateType_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optDateType_1.Size = New System.Drawing.Size(65, 17)
        Me._optDateType_1.TabIndex = 1
        Me._optDateType_1.TabStop = True
        Me._optDateType_1.Text = "End"
        Me._optDateType_1.UseVisualStyleBackColor = False
        '
        '_optDateType_0
        '
        Me._optDateType_0.BackColor = System.Drawing.SystemColors.Control
        Me._optDateType_0.Checked = True
        Me._optDateType_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._optDateType_0.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optDateType_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optDateType.SetIndex(Me._optDateType_0, CType(0, Short))
        Me._optDateType_0.Location = New System.Drawing.Point(64, 16)
        Me._optDateType_0.Name = "_optDateType_0"
        Me._optDateType_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optDateType_0.Size = New System.Drawing.Size(65, 17)
        Me._optDateType_0.TabIndex = 0
        Me._optDateType_0.TabStop = True
        Me._optDateType_0.Text = "Start"
        Me._optDateType_0.UseVisualStyleBackColor = False
        '
        '_lblLabel_0
        '
        Me._lblLabel_0.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_0.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_0, CType(0, Short))
        Me._lblLabel_0.Location = New System.Drawing.Point(136, 40)
        Me._lblLabel_0.Name = "_lblLabel_0"
        Me._lblLabel_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_0.Size = New System.Drawing.Size(25, 17)
        Me._lblLabel_0.TabIndex = 12
        Me._lblLabel_0.Text = "-"
        Me._lblLabel_0.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'chkPrintOnly
        '
        Me.chkPrintOnly.BackColor = System.Drawing.SystemColors.Control
        Me.chkPrintOnly.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkPrintOnly.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPrintOnly.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkPrintOnly.Location = New System.Drawing.Point(200, 152)
        Me.chkPrintOnly.Name = "chkPrintOnly"
        Me.chkPrintOnly.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkPrintOnly.Size = New System.Drawing.Size(89, 17)
        Me.chkPrintOnly.TabIndex = 2
        Me.chkPrintOnly.Text = "Print Only"
        Me.chkPrintOnly.UseVisualStyleBackColor = False
        Me.chkPrintOnly.Visible = False
        '
        '_cmbField_0
        '
        Me._cmbField_0.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me._cmbField_0.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me._cmbField_0.BackColor = System.Drawing.SystemColors.Window
        Me._cmbField_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmbField_0.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me._cmbField_0.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmbField_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbField.SetIndex(Me._cmbField_0, CType(0, Short))
        Me._cmbField_0.Location = New System.Drawing.Point(112, 8)
        Me._cmbField_0.Name = "_cmbField_0"
        Me._cmbField_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmbField_0.Size = New System.Drawing.Size(177, 22)
        Me._cmbField_0.Sorted = True
        Me._cmbField_0.TabIndex = 0
        '
        '_cmbField_1
        '
        Me._cmbField_1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me._cmbField_1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me._cmbField_1.BackColor = System.Drawing.SystemColors.Window
        Me._cmbField_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmbField_1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me._cmbField_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmbField_1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbField.SetIndex(Me._cmbField_1, CType(1, Short))
        Me._cmbField_1.Location = New System.Drawing.Point(112, 32)
        Me._cmbField_1.Name = "_cmbField_1"
        Me._cmbField_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmbField_1.Size = New System.Drawing.Size(177, 22)
        Me._cmbField_1.Sorted = True
        Me._cmbField_1.TabIndex = 1
        '
        '_lblLabel_1
        '
        Me._lblLabel_1.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_1, CType(1, Short))
        Me._lblLabel_1.Location = New System.Drawing.Point(8, 8)
        Me._lblLabel_1.Name = "_lblLabel_1"
        Me._lblLabel_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_1.Size = New System.Drawing.Size(97, 17)
        Me._lblLabel_1.TabIndex = 10
        Me._lblLabel_1.Text = "Store :"
        Me._lblLabel_1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_5
        '
        Me._lblLabel_5.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_5.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_5, CType(5, Short))
        Me._lblLabel_5.Location = New System.Drawing.Point(8, 32)
        Me._lblLabel_5.Name = "_lblLabel_5"
        Me._lblLabel_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_5.Size = New System.Drawing.Size(97, 17)
        Me._lblLabel_5.TabIndex = 11
        Me._lblLabel_5.Text = "Sub-Team :"
        Me._lblLabel_5.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'cmbField
        '
        '
        'frmSpecialsByEndDate
        '
        Me.AcceptButton = Me.cmdReport
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdExit
        Me.ClientSize = New System.Drawing.Size(299, 228)
        Me.Controls.Add(Me.fraDate)
        Me.Controls.Add(Me.chkPrintOnly)
        Me.Controls.Add(Me._cmbField_0)
        Me.Controls.Add(Me._cmbField_1)
        Me.Controls.Add(Me.cmdReport)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me._lblLabel_1)
        Me.Controls.Add(Me._lblLabel_5)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Location = New System.Drawing.Point(387, 405)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmSpecialsByEndDate"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Specials Report"
        Me.fraDate.ResumeLayout(False)
        Me.fraDate.PerformLayout()
        CType(Me.dtpEndDate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dtpStartDate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cmbField, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.optDateType, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtField, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents dtpStartDate As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
    Friend WithEvents dtpEndDate As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
#End Region
End Class