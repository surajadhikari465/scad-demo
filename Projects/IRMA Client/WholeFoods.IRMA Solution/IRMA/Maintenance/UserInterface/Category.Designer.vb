<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmCategory
#Region "Windows Form Designer generated code "
	<System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
		MyBase.New()
        'This call is required by the Windows Form Designer.
        Me.IsInitializing = True
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
	Public WithEvents cmbSubTeam As System.Windows.Forms.ComboBox
	Public WithEvents cmdUnlock As System.Windows.Forms.Button
	Public WithEvents cmdCategorySearch As System.Windows.Forms.Button
	Public WithEvents cmdDelete As System.Windows.Forms.Button
	Public WithEvents cmdAdd As System.Windows.Forms.Button
	Public WithEvents cmdExit As System.Windows.Forms.Button
	Public WithEvents txtField As System.Windows.Forms.TextBox
	Public WithEvents _lblLabel_2 As System.Windows.Forms.Label
	Public WithEvents lblReadOnly As System.Windows.Forms.Label
	Public WithEvents _lblLabel_1 As System.Windows.Forms.Label
	Public WithEvents lblLabel As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCategory))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdUnlock = New System.Windows.Forms.Button
        Me.cmdCategorySearch = New System.Windows.Forms.Button
        Me.cmdDelete = New System.Windows.Forms.Button
        Me.cmdAdd = New System.Windows.Forms.Button
        Me.cmdExit = New System.Windows.Forms.Button
        Me.cmbSubTeam = New System.Windows.Forms.ComboBox
        Me.txtField = New System.Windows.Forms.TextBox
        Me._lblLabel_2 = New System.Windows.Forms.Label
        Me.lblReadOnly = New System.Windows.Forms.Label
        Me._lblLabel_1 = New System.Windows.Forms.Label
        Me.lblLabel = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(Me.components)
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdUnlock
        '
        Me.cmdUnlock.BackColor = System.Drawing.SystemColors.Control
        Me.cmdUnlock.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdUnlock.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdUnlock.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdUnlock.Image = CType(resources.GetObject("cmdUnlock.Image"), System.Drawing.Image)
        Me.cmdUnlock.Location = New System.Drawing.Point(272, 96)
        Me.cmdUnlock.Name = "cmdUnlock"
        Me.cmdUnlock.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdUnlock.Size = New System.Drawing.Size(41, 41)
        Me.cmdUnlock.TabIndex = 7
        Me.cmdUnlock.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdUnlock, "Unlock Class")
        Me.cmdUnlock.UseVisualStyleBackColor = False
        '
        'cmdCategorySearch
        '
        Me.cmdCategorySearch.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCategorySearch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCategorySearch.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCategorySearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCategorySearch.Image = CType(resources.GetObject("cmdCategorySearch.Image"), System.Drawing.Image)
        Me.cmdCategorySearch.Location = New System.Drawing.Point(336, 16)
        Me.cmdCategorySearch.Name = "cmdCategorySearch"
        Me.cmdCategorySearch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCategorySearch.Size = New System.Drawing.Size(25, 20)
        Me.cmdCategorySearch.TabIndex = 4
        Me.cmdCategorySearch.TabStop = False
        Me.cmdCategorySearch.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdCategorySearch, "Search for Class")
        Me.cmdCategorySearch.UseVisualStyleBackColor = False
        '
        'cmdDelete
        '
        Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDelete.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDelete.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDelete.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDelete.Image = CType(resources.GetObject("cmdDelete.Image"), System.Drawing.Image)
        Me.cmdDelete.Location = New System.Drawing.Point(224, 96)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDelete.Size = New System.Drawing.Size(41, 41)
        Me.cmdDelete.TabIndex = 6
        Me.cmdDelete.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdDelete, "Remove Class")
        Me.cmdDelete.UseVisualStyleBackColor = False
        '
        'cmdAdd
        '
        Me.cmdAdd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAdd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAdd.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAdd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAdd.Image = CType(resources.GetObject("cmdAdd.Image"), System.Drawing.Image)
        Me.cmdAdd.Location = New System.Drawing.Point(176, 96)
        Me.cmdAdd.Name = "cmdAdd"
        Me.cmdAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAdd.Size = New System.Drawing.Size(41, 41)
        Me.cmdAdd.TabIndex = 5
        Me.cmdAdd.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdAdd, "Add Class")
        Me.cmdAdd.UseVisualStyleBackColor = False
        '
        'cmdExit
        '
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Image = CType(resources.GetObject("cmdExit.Image"), System.Drawing.Image)
        Me.cmdExit.Location = New System.Drawing.Point(320, 96)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(41, 41)
        Me.cmdExit.TabIndex = 8
        Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdExit, "Exit")
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'cmbSubTeam
        '
        Me.cmbSubTeam.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbSubTeam.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbSubTeam.BackColor = System.Drawing.SystemColors.Window
        Me.cmbSubTeam.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbSubTeam.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbSubTeam.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbSubTeam.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbSubTeam.Location = New System.Drawing.Point(88, 40)
        Me.cmbSubTeam.Name = "cmbSubTeam"
        Me.cmbSubTeam.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbSubTeam.Size = New System.Drawing.Size(273, 22)
        Me.cmbSubTeam.Sorted = True
        Me.cmbSubTeam.TabIndex = 3
        '
        'txtField
        '
        Me.txtField.AcceptsReturn = True
        Me.txtField.BackColor = System.Drawing.SystemColors.Window
        Me.txtField.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtField.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtField.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.Location = New System.Drawing.Point(88, 16)
        Me.txtField.MaxLength = 35
        Me.txtField.Name = "txtField"
        Me.txtField.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtField.Size = New System.Drawing.Size(249, 20)
        Me.txtField.TabIndex = 1
        Me.txtField.Tag = "String"
        '
        '_lblLabel_2
        '
        Me._lblLabel_2.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_2, CType(2, Short))
        Me._lblLabel_2.Location = New System.Drawing.Point(8, 40)
        Me._lblLabel_2.Name = "_lblLabel_2"
        Me._lblLabel_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_2.Size = New System.Drawing.Size(73, 17)
        Me._lblLabel_2.TabIndex = 2
        Me._lblLabel_2.Text = "Sub-Team :"
        Me._lblLabel_2.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblReadOnly
        '
        Me.lblReadOnly.BackColor = System.Drawing.Color.Transparent
        Me.lblReadOnly.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReadOnly.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReadOnly.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.lblReadOnly.Location = New System.Drawing.Point(0, 104)
        Me.lblReadOnly.Name = "lblReadOnly"
        Me.lblReadOnly.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblReadOnly.Size = New System.Drawing.Size(177, 25)
        Me.lblReadOnly.TabIndex = 9
        Me.lblReadOnly.Text = "Read Only"
        Me.lblReadOnly.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.lblReadOnly.Visible = False
        '
        '_lblLabel_1
        '
        Me._lblLabel_1.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_1, CType(1, Short))
        Me._lblLabel_1.Location = New System.Drawing.Point(8, 16)
        Me._lblLabel_1.Name = "_lblLabel_1"
        Me._lblLabel_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_1.Size = New System.Drawing.Size(73, 17)
        Me._lblLabel_1.TabIndex = 0
        Me._lblLabel_1.Text = "Class :"
        Me._lblLabel_1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'frmCategory
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(376, 149)
        Me.Controls.Add(Me.cmbSubTeam)
        Me.Controls.Add(Me.cmdUnlock)
        Me.Controls.Add(Me.cmdCategorySearch)
        Me.Controls.Add(Me.cmdDelete)
        Me.Controls.Add(Me.cmdAdd)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.txtField)
        Me.Controls.Add(Me._lblLabel_2)
        Me.Controls.Add(Me.lblReadOnly)
        Me.Controls.Add(Me._lblLabel_1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Location = New System.Drawing.Point(378, 438)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmCategory"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Class Information"
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
#End Region 
End Class