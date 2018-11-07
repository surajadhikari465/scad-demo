<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmCostExceptionReport
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
	Public WithEvents txtCost As System.Windows.Forms.TextBox
	Public WithEvents _optType_2 As System.Windows.Forms.RadioButton
	Public WithEvents _optType_1 As System.Windows.Forms.RadioButton
	Public WithEvents Frame1 As System.Windows.Forms.GroupBox
	Public WithEvents chkIncludeDiscontinued As System.Windows.Forms.CheckBox
	Public WithEvents cboSubTeam As System.Windows.Forms.ComboBox
	Public WithEvents cmdExit As System.Windows.Forms.Button
	Public WithEvents cmdReport As System.Windows.Forms.Button
	Public WithEvents cboStore As System.Windows.Forms.ComboBox
	Public WithEvents chkPrintOnly As System.Windows.Forms.CheckBox
    Public WithEvents _lblLabel_2 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_5 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_1 As System.Windows.Forms.Label
	Public WithEvents lblLabel As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	Public WithEvents optType As Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCostExceptionReport))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdExit = New System.Windows.Forms.Button
        Me.cmdReport = New System.Windows.Forms.Button
        Me.Frame1 = New System.Windows.Forms.GroupBox
        Me.txtCost = New System.Windows.Forms.TextBox
        Me._optType_2 = New System.Windows.Forms.RadioButton
        Me._optType_1 = New System.Windows.Forms.RadioButton
        Me.chkIncludeDiscontinued = New System.Windows.Forms.CheckBox
        Me.cboSubTeam = New System.Windows.Forms.ComboBox
        Me.cboStore = New System.Windows.Forms.ComboBox
        Me.chkPrintOnly = New System.Windows.Forms.CheckBox
        Me._lblLabel_2 = New System.Windows.Forms.Label
        Me._lblLabel_5 = New System.Windows.Forms.Label
        Me._lblLabel_1 = New System.Windows.Forms.Label
        Me.lblLabel = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(Me.components)
        Me.optType = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(Me.components)
        Me.Frame1.SuspendLayout()
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.optType, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdExit
        '
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdExit.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Image = CType(resources.GetObject("cmdExit.Image"), System.Drawing.Image)
        Me.cmdExit.Location = New System.Drawing.Point(240, 160)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(41, 41)
        Me.cmdExit.TabIndex = 9
        Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdExit, "Exit")
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'cmdReport
        '
        Me.cmdReport.BackColor = System.Drawing.SystemColors.Control
        Me.cmdReport.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdReport.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdReport.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdReport.Image = CType(resources.GetObject("cmdReport.Image"), System.Drawing.Image)
        Me.cmdReport.Location = New System.Drawing.Point(192, 160)
        Me.cmdReport.Name = "cmdReport"
        Me.cmdReport.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdReport.Size = New System.Drawing.Size(41, 41)
        Me.cmdReport.TabIndex = 8
        Me.cmdReport.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdReport, "Print")
        Me.cmdReport.UseVisualStyleBackColor = False
        '
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me.txtCost)
        Me.Frame1.Controls.Add(Me._optType_2)
        Me.Frame1.Controls.Add(Me._optType_1)
        Me.Frame1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Location = New System.Drawing.Point(24, 56)
        Me.Frame1.Name = "Frame1"
        Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame1.Size = New System.Drawing.Size(257, 73)
        Me.Frame1.TabIndex = 2
        Me.Frame1.TabStop = False
        Me.Frame1.Text = "Exception Type"
        '
        'txtCost
        '
        Me.txtCost.AcceptsReturn = True
        Me.txtCost.BackColor = System.Drawing.SystemColors.Window
        Me.txtCost.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCost.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCost.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCost.Location = New System.Drawing.Point(80, 40)
        Me.txtCost.MaxLength = 8
        Me.txtCost.Name = "txtCost"
        Me.txtCost.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCost.Size = New System.Drawing.Size(65, 20)
        Me.txtCost.TabIndex = 5
        Me.txtCost.Tag = "Currency"
        Me.txtCost.Text = "20.00"
        '
        '_optType_2
        '
        Me._optType_2.BackColor = System.Drawing.SystemColors.Control
        Me._optType_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._optType_2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optType_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optType.SetIndex(Me._optType_2, CType(2, Short))
        Me._optType_2.Location = New System.Drawing.Point(16, 40)
        Me._optType_2.Name = "_optType_2"
        Me._optType_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optType_2.Size = New System.Drawing.Size(129, 17)
        Me._optType_2.TabIndex = 4
        Me._optType_2.TabStop = True
        Me._optType_2.Text = "Cost > "
        Me._optType_2.UseVisualStyleBackColor = False
        '
        '_optType_1
        '
        Me._optType_1.BackColor = System.Drawing.SystemColors.Control
        Me._optType_1.Checked = True
        Me._optType_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._optType_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optType_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optType.SetIndex(Me._optType_1, CType(1, Short))
        Me._optType_1.Location = New System.Drawing.Point(16, 16)
        Me._optType_1.Name = "_optType_1"
        Me._optType_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optType_1.Size = New System.Drawing.Size(129, 17)
        Me._optType_1.TabIndex = 3
        Me._optType_1.TabStop = True
        Me._optType_1.Text = "Cost > Price"
        Me._optType_1.UseVisualStyleBackColor = False
        '
        'chkIncludeDiscontinued
        '
        Me.chkIncludeDiscontinued.BackColor = System.Drawing.SystemColors.Control
        Me.chkIncludeDiscontinued.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIncludeDiscontinued.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIncludeDiscontinued.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIncludeDiscontinued.Location = New System.Drawing.Point(208, 136)
        Me.chkIncludeDiscontinued.Name = "chkIncludeDiscontinued"
        Me.chkIncludeDiscontinued.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIncludeDiscontinued.Size = New System.Drawing.Size(25, 17)
        Me.chkIncludeDiscontinued.TabIndex = 6
        Me.chkIncludeDiscontinued.UseVisualStyleBackColor = False
        '
        'cboSubTeam
        '
        Me.cboSubTeam.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cboSubTeam.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboSubTeam.BackColor = System.Drawing.SystemColors.Window
        Me.cboSubTeam.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboSubTeam.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSubTeam.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboSubTeam.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboSubTeam.Location = New System.Drawing.Point(104, 32)
        Me.cboSubTeam.Name = "cboSubTeam"
        Me.cboSubTeam.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboSubTeam.Size = New System.Drawing.Size(177, 22)
        Me.cboSubTeam.Sorted = True
        Me.cboSubTeam.TabIndex = 1
        '
        'cboStore
        '
        Me.cboStore.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cboStore.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboStore.BackColor = System.Drawing.SystemColors.Window
        Me.cboStore.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboStore.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboStore.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboStore.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboStore.Location = New System.Drawing.Point(104, 8)
        Me.cboStore.Name = "cboStore"
        Me.cboStore.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboStore.Size = New System.Drawing.Size(177, 22)
        Me.cboStore.Sorted = True
        Me.cboStore.TabIndex = 0
        '
        'chkPrintOnly
        '
        Me.chkPrintOnly.BackColor = System.Drawing.SystemColors.Control
        Me.chkPrintOnly.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkPrintOnly.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPrintOnly.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkPrintOnly.Location = New System.Drawing.Point(88, 168)
        Me.chkPrintOnly.Name = "chkPrintOnly"
        Me.chkPrintOnly.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkPrintOnly.Size = New System.Drawing.Size(97, 17)
        Me.chkPrintOnly.TabIndex = 7
        Me.chkPrintOnly.Text = "Print Only"
        Me.chkPrintOnly.UseVisualStyleBackColor = False
        '
        '_lblLabel_2
        '
        Me._lblLabel_2.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_2, CType(2, Short))
        Me._lblLabel_2.Location = New System.Drawing.Point(40, 136)
        Me._lblLabel_2.Name = "_lblLabel_2"
        Me._lblLabel_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_2.Size = New System.Drawing.Size(161, 17)
        Me._lblLabel_2.TabIndex = 12
        Me._lblLabel_2.Text = "Include Discontinued :"
        Me._lblLabel_2.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_5
        '
        Me._lblLabel_5.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_5.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_5, CType(5, Short))
        Me._lblLabel_5.Location = New System.Drawing.Point(0, 32)
        Me._lblLabel_5.Name = "_lblLabel_5"
        Me._lblLabel_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_5.Size = New System.Drawing.Size(97, 17)
        Me._lblLabel_5.TabIndex = 11
        Me._lblLabel_5.Text = "Sub-Team :"
        Me._lblLabel_5.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_1
        '
        Me._lblLabel_1.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_1, CType(1, Short))
        Me._lblLabel_1.Location = New System.Drawing.Point(0, 8)
        Me._lblLabel_1.Name = "_lblLabel_1"
        Me._lblLabel_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_1.Size = New System.Drawing.Size(97, 17)
        Me._lblLabel_1.TabIndex = 10
        Me._lblLabel_1.Text = "Store :"
        Me._lblLabel_1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'optType
        '
        '
        'frmCostExceptionReport
        '
        Me.AcceptButton = Me.cmdReport
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdExit
        Me.ClientSize = New System.Drawing.Size(303, 212)
        Me.Controls.Add(Me.Frame1)
        Me.Controls.Add(Me.chkIncludeDiscontinued)
        Me.Controls.Add(Me.cboSubTeam)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.cmdReport)
        Me.Controls.Add(Me.cboStore)
        Me.Controls.Add(Me.chkPrintOnly)
        Me.Controls.Add(Me._lblLabel_2)
        Me.Controls.Add(Me._lblLabel_5)
        Me.Controls.Add(Me._lblLabel_1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Location = New System.Drawing.Point(301, 288)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmCostExceptionReport"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Cost Exception Report"
        Me.Frame1.ResumeLayout(False)
        Me.Frame1.PerformLayout()
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.optType, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class