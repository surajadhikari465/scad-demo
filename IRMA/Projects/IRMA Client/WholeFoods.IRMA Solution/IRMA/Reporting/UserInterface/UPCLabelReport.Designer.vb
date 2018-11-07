<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmUpcLabelReport
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
	Public WithEvents opt5267 As System.Windows.Forms.RadioButton
	Public WithEvents opt5260 As System.Windows.Forms.RadioButton
	Public WithEvents Frame1 As System.Windows.Forms.GroupBox
	Public WithEvents txtPackageDesc As System.Windows.Forms.TextBox
	Public WithEvents txtIdentifier As System.Windows.Forms.TextBox
	Public WithEvents txtItemDesc As System.Windows.Forms.TextBox
	Public WithEvents cmdItemSearch As System.Windows.Forms.Button
	Public WithEvents chkPrintOnly As System.Windows.Forms.CheckBox
	Public WithEvents cmdInventoryScan As System.Windows.Forms.Button
    Public WithEvents cmdExit As System.Windows.Forms.Button
	Public WithEvents _lblLabel_14 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_0 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_1 As System.Windows.Forms.Label
	Public WithEvents lblLabel As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUpcLabelReport))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdItemSearch = New System.Windows.Forms.Button
        Me.cmdInventoryScan = New System.Windows.Forms.Button
        Me.cmdExit = New System.Windows.Forms.Button
        Me.Frame1 = New System.Windows.Forms.GroupBox
        Me.opt5267 = New System.Windows.Forms.RadioButton
        Me.opt5260 = New System.Windows.Forms.RadioButton
        Me.txtPackageDesc = New System.Windows.Forms.TextBox
        Me.txtIdentifier = New System.Windows.Forms.TextBox
        Me.txtItemDesc = New System.Windows.Forms.TextBox
        Me.chkPrintOnly = New System.Windows.Forms.CheckBox
        Me._lblLabel_14 = New System.Windows.Forms.Label
        Me._lblLabel_0 = New System.Windows.Forms.Label
        Me._lblLabel_1 = New System.Windows.Forms.Label
        Me.lblLabel = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(Me.components)
        Me.Frame1.SuspendLayout()
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdItemSearch
        '
        Me.cmdItemSearch.BackColor = System.Drawing.SystemColors.Control
        Me.cmdItemSearch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdItemSearch.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdItemSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdItemSearch.Image = CType(resources.GetObject("cmdItemSearch.Image"), System.Drawing.Image)
        Me.cmdItemSearch.Location = New System.Drawing.Point(280, 120)
        Me.cmdItemSearch.Name = "cmdItemSearch"
        Me.cmdItemSearch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdItemSearch.Size = New System.Drawing.Size(41, 39)
        Me.cmdItemSearch.TabIndex = 3
        Me.cmdItemSearch.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdItemSearch, "Item Search")
        Me.cmdItemSearch.UseVisualStyleBackColor = False
        '
        'cmdInventoryScan
        '
        Me.cmdInventoryScan.BackColor = System.Drawing.SystemColors.Control
        Me.cmdInventoryScan.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdInventoryScan.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdInventoryScan.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdInventoryScan.Image = CType(resources.GetObject("cmdInventoryScan.Image"), System.Drawing.Image)
        Me.cmdInventoryScan.Location = New System.Drawing.Point(336, 120)
        Me.cmdInventoryScan.Name = "cmdInventoryScan"
        Me.cmdInventoryScan.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdInventoryScan.Size = New System.Drawing.Size(41, 41)
        Me.cmdInventoryScan.TabIndex = 1
        Me.cmdInventoryScan.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdInventoryScan, "Print")
        Me.cmdInventoryScan.UseVisualStyleBackColor = False
        '
        'cmdExit
        '
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdExit.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Image = CType(resources.GetObject("cmdExit.Image"), System.Drawing.Image)
        Me.cmdExit.Location = New System.Drawing.Point(384, 120)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(41, 41)
        Me.cmdExit.TabIndex = 2
        Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdExit, "Exit")
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me.opt5267)
        Me.Frame1.Controls.Add(Me.opt5260)
        Me.Frame1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Location = New System.Drawing.Point(248, 32)
        Me.Frame1.Name = "Frame1"
        Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame1.Size = New System.Drawing.Size(177, 73)
        Me.Frame1.TabIndex = 10
        Me.Frame1.TabStop = False
        Me.Frame1.Text = "Label Type"
        '
        'opt5267
        '
        Me.opt5267.BackColor = System.Drawing.SystemColors.Control
        Me.opt5267.Cursor = System.Windows.Forms.Cursors.Default
        Me.opt5267.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.opt5267.ForeColor = System.Drawing.SystemColors.ControlText
        Me.opt5267.Location = New System.Drawing.Point(8, 40)
        Me.opt5267.Name = "opt5267"
        Me.opt5267.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.opt5267.Size = New System.Drawing.Size(161, 25)
        Me.opt5267.TabIndex = 12
        Me.opt5267.TabStop = True
        Me.opt5267.Tag = "Avery5267"
        Me.opt5267.Text = "Avery 5267 - 80 Labels"
        Me.opt5267.UseVisualStyleBackColor = False
        '
        'opt5260
        '
        Me.opt5260.BackColor = System.Drawing.SystemColors.Control
        Me.opt5260.Checked = True
        Me.opt5260.Cursor = System.Windows.Forms.Cursors.Default
        Me.opt5260.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.opt5260.ForeColor = System.Drawing.SystemColors.ControlText
        Me.opt5260.Location = New System.Drawing.Point(8, 16)
        Me.opt5260.Name = "opt5260"
        Me.opt5260.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.opt5260.Size = New System.Drawing.Size(161, 25)
        Me.opt5260.TabIndex = 11
        Me.opt5260.TabStop = True
        Me.opt5260.Tag = "Avery5260"
        Me.opt5260.Text = "Avery 5260 - 30 Labels"
        Me.opt5260.UseVisualStyleBackColor = False
        '
        'txtPackageDesc
        '
        Me.txtPackageDesc.AcceptsReturn = True
        Me.txtPackageDesc.BackColor = System.Drawing.SystemColors.ControlLight
        Me.txtPackageDesc.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPackageDesc.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPackageDesc.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPackageDesc.Location = New System.Drawing.Point(96, 32)
        Me.txtPackageDesc.MaxLength = 0
        Me.txtPackageDesc.Name = "txtPackageDesc"
        Me.txtPackageDesc.ReadOnly = True
        Me.txtPackageDesc.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPackageDesc.Size = New System.Drawing.Size(145, 20)
        Me.txtPackageDesc.TabIndex = 8
        Me.txtPackageDesc.TabStop = False
        Me.txtPackageDesc.Tag = "Double"
        '
        'txtIdentifier
        '
        Me.txtIdentifier.AcceptsReturn = True
        Me.txtIdentifier.BackColor = System.Drawing.SystemColors.ControlLight
        Me.txtIdentifier.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtIdentifier.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtIdentifier.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtIdentifier.Location = New System.Drawing.Point(96, 56)
        Me.txtIdentifier.MaxLength = 18
        Me.txtIdentifier.Name = "txtIdentifier"
        Me.txtIdentifier.ReadOnly = True
        Me.txtIdentifier.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtIdentifier.Size = New System.Drawing.Size(145, 20)
        Me.txtIdentifier.TabIndex = 6
        Me.txtIdentifier.TabStop = False
        Me.txtIdentifier.Tag = "Integer"
        '
        'txtItemDesc
        '
        Me.txtItemDesc.AcceptsReturn = True
        Me.txtItemDesc.BackColor = System.Drawing.SystemColors.ControlLight
        Me.txtItemDesc.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtItemDesc.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtItemDesc.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtItemDesc.Location = New System.Drawing.Point(96, 8)
        Me.txtItemDesc.MaxLength = 0
        Me.txtItemDesc.Name = "txtItemDesc"
        Me.txtItemDesc.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtItemDesc.Size = New System.Drawing.Size(329, 20)
        Me.txtItemDesc.TabIndex = 4
        '
        'chkPrintOnly
        '
        Me.chkPrintOnly.BackColor = System.Drawing.SystemColors.Control
        Me.chkPrintOnly.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkPrintOnly.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPrintOnly.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkPrintOnly.Location = New System.Drawing.Point(104, 136)
        Me.chkPrintOnly.Name = "chkPrintOnly"
        Me.chkPrintOnly.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkPrintOnly.Size = New System.Drawing.Size(97, 17)
        Me.chkPrintOnly.TabIndex = 0
        Me.chkPrintOnly.Text = "Print Only"
        Me.chkPrintOnly.UseVisualStyleBackColor = False
        '
        '_lblLabel_14
        '
        Me._lblLabel_14.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_14.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_14.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_14.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_14, CType(14, Short))
        Me._lblLabel_14.Location = New System.Drawing.Point(-16, 32)
        Me._lblLabel_14.Name = "_lblLabel_14"
        Me._lblLabel_14.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_14.Size = New System.Drawing.Size(105, 17)
        Me._lblLabel_14.TabIndex = 9
        Me._lblLabel_14.Text = "Pkg Desc :"
        Me._lblLabel_14.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_0
        '
        Me._lblLabel_0.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_0.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_0, CType(0, Short))
        Me._lblLabel_0.Location = New System.Drawing.Point(-16, 56)
        Me._lblLabel_0.Name = "_lblLabel_0"
        Me._lblLabel_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_0.Size = New System.Drawing.Size(105, 17)
        Me._lblLabel_0.TabIndex = 7
        Me._lblLabel_0.Text = "Identifier :"
        Me._lblLabel_0.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_1
        '
        Me._lblLabel_1.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_1, CType(1, Short))
        Me._lblLabel_1.Location = New System.Drawing.Point(16, 8)
        Me._lblLabel_1.Name = "_lblLabel_1"
        Me._lblLabel_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_1.Size = New System.Drawing.Size(73, 17)
        Me._lblLabel_1.TabIndex = 5
        Me._lblLabel_1.Text = "Item Desc :"
        Me._lblLabel_1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'frmUpcLabelReport
        '
        Me.AcceptButton = Me.cmdItemSearch
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdExit
        Me.ClientSize = New System.Drawing.Size(442, 172)
        Me.Controls.Add(Me.Frame1)
        Me.Controls.Add(Me.txtPackageDesc)
        Me.Controls.Add(Me.txtIdentifier)
        Me.Controls.Add(Me.txtItemDesc)
        Me.Controls.Add(Me.cmdItemSearch)
        Me.Controls.Add(Me.chkPrintOnly)
        Me.Controls.Add(Me.cmdInventoryScan)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me._lblLabel_14)
        Me.Controls.Add(Me._lblLabel_0)
        Me.Controls.Add(Me._lblLabel_1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Location = New System.Drawing.Point(325, 129)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmUpcLabelReport"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "UPC Label Report"
        Me.Frame1.ResumeLayout(False)
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
#End Region 
End Class