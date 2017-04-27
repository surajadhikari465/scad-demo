<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmConversion
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
	Public WithEvents _cmbField_0 As System.Windows.Forms.ComboBox
	Public WithEvents _cmbField_1 As System.Windows.Forms.ComboBox
	Public WithEvents _cmdBrandSearch_9 As System.Windows.Forms.Button
	Public WithEvents _cmdBrandSearch_8 As System.Windows.Forms.Button
	Public WithEvents cmdDelete As System.Windows.Forms.Button
	Public WithEvents _cmbField_2 As System.Windows.Forms.ComboBox
	Public WithEvents _txtField_3 As System.Windows.Forms.TextBox
	Public WithEvents cmdExit As System.Windows.Forms.Button
	Public WithEvents cmdAdd As System.Windows.Forms.Button
	Public WithEvents _lblLabel_0 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_1 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_2 As System.Windows.Forms.Label
	Public WithEvents _lblLabel_3 As System.Windows.Forms.Label
	Public WithEvents cmbField As Microsoft.VisualBasic.Compatibility.VB6.ComboBoxArray
	Public WithEvents cmdBrandSearch As Microsoft.VisualBasic.Compatibility.VB6.ButtonArray
	Public WithEvents lblLabel As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	Public WithEvents txtField As Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmConversion))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me._cmdBrandSearch_9 = New System.Windows.Forms.Button
        Me._cmdBrandSearch_8 = New System.Windows.Forms.Button
        Me.cmdDelete = New System.Windows.Forms.Button
        Me.cmdExit = New System.Windows.Forms.Button
        Me.cmdAdd = New System.Windows.Forms.Button
        Me._cmbField_0 = New System.Windows.Forms.ComboBox
        Me._cmbField_1 = New System.Windows.Forms.ComboBox
        Me._cmbField_2 = New System.Windows.Forms.ComboBox
        Me._txtField_3 = New System.Windows.Forms.TextBox
        Me._lblLabel_0 = New System.Windows.Forms.Label
        Me._lblLabel_1 = New System.Windows.Forms.Label
        Me._lblLabel_2 = New System.Windows.Forms.Label
        Me._lblLabel_3 = New System.Windows.Forms.Label
        Me.cmbField = New Microsoft.VisualBasic.Compatibility.VB6.ComboBoxArray(Me.components)
        Me.cmdBrandSearch = New Microsoft.VisualBasic.Compatibility.VB6.ButtonArray(Me.components)
        Me.lblLabel = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(Me.components)
        Me.txtField = New Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray(Me.components)
        CType(Me.cmbField, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cmdBrandSearch, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtField, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        '_cmdBrandSearch_9
        '
        Me._cmdBrandSearch_9.BackColor = System.Drawing.SystemColors.Control
        Me._cmdBrandSearch_9.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdBrandSearch_9.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdBrandSearch_9.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdBrandSearch_9.Image = CType(resources.GetObject("_cmdBrandSearch_9.Image"), System.Drawing.Image)
        Me.cmdBrandSearch.SetIndex(Me._cmdBrandSearch_9, CType(9, Short))
        Me._cmdBrandSearch_9.Location = New System.Drawing.Point(303, 32)
        Me._cmdBrandSearch_9.Name = "_cmdBrandSearch_9"
        Me._cmdBrandSearch_9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdBrandSearch_9.Size = New System.Drawing.Size(25, 22)
        Me._cmdBrandSearch_9.TabIndex = 3
        Me._cmdBrandSearch_9.TabStop = False
        Me._cmdBrandSearch_9.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me._cmdBrandSearch_9, "Search for Unit")
        Me._cmdBrandSearch_9.UseVisualStyleBackColor = False
        '
        '_cmdBrandSearch_8
        '
        Me._cmdBrandSearch_8.BackColor = System.Drawing.SystemColors.Control
        Me._cmdBrandSearch_8.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdBrandSearch_8.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdBrandSearch_8.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdBrandSearch_8.Image = CType(resources.GetObject("_cmdBrandSearch_8.Image"), System.Drawing.Image)
        Me.cmdBrandSearch.SetIndex(Me._cmdBrandSearch_8, CType(8, Short))
        Me._cmdBrandSearch_8.Location = New System.Drawing.Point(303, 8)
        Me._cmdBrandSearch_8.Name = "_cmdBrandSearch_8"
        Me._cmdBrandSearch_8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdBrandSearch_8.Size = New System.Drawing.Size(25, 22)
        Me._cmdBrandSearch_8.TabIndex = 1
        Me._cmdBrandSearch_8.TabStop = False
        Me._cmdBrandSearch_8.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me._cmdBrandSearch_8, "Search for Unit")
        Me._cmdBrandSearch_8.UseVisualStyleBackColor = False
        '
        'cmdDelete
        '
        Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDelete.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDelete.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDelete.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDelete.Image = CType(resources.GetObject("cmdDelete.Image"), System.Drawing.Image)
        Me.cmdDelete.Location = New System.Drawing.Point(240, 72)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDelete.Size = New System.Drawing.Size(41, 41)
        Me.cmdDelete.TabIndex = 9
        Me.cmdDelete.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdDelete, "Remove Unit Conversion")
        Me.cmdDelete.UseVisualStyleBackColor = False
        '
        'cmdExit
        '
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Image = CType(resources.GetObject("cmdExit.Image"), System.Drawing.Image)
        Me.cmdExit.Location = New System.Drawing.Point(287, 72)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(41, 41)
        Me.cmdExit.TabIndex = 10
        Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdExit, "Exit")
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'cmdAdd
        '
        Me.cmdAdd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAdd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAdd.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAdd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAdd.Image = CType(resources.GetObject("cmdAdd.Image"), System.Drawing.Image)
        Me.cmdAdd.Location = New System.Drawing.Point(193, 72)
        Me.cmdAdd.Name = "cmdAdd"
        Me.cmdAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAdd.Size = New System.Drawing.Size(41, 41)
        Me.cmdAdd.TabIndex = 8
        Me.cmdAdd.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdAdd, "Add Unit Conversion")
        Me.cmdAdd.UseVisualStyleBackColor = False
        '
        '_cmbField_0
        '
        Me._cmbField_0.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me._cmbField_0.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me._cmbField_0.BackColor = System.Drawing.SystemColors.ControlLight
        Me._cmbField_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmbField_0.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me._cmbField_0.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmbField_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbField.SetIndex(Me._cmbField_0, CType(0, Short))
        Me._cmbField_0.Location = New System.Drawing.Point(80, 8)
        Me._cmbField_0.Name = "_cmbField_0"
        Me._cmbField_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmbField_0.Size = New System.Drawing.Size(217, 22)
        Me._cmbField_0.TabIndex = 12
        '
        '_cmbField_1
        '
        Me._cmbField_1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me._cmbField_1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me._cmbField_1.BackColor = System.Drawing.SystemColors.ControlLight
        Me._cmbField_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmbField_1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me._cmbField_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmbField_1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbField.SetIndex(Me._cmbField_1, CType(1, Short))
        Me._cmbField_1.Location = New System.Drawing.Point(80, 32)
        Me._cmbField_1.Name = "_cmbField_1"
        Me._cmbField_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmbField_1.Size = New System.Drawing.Size(217, 22)
        Me._cmbField_1.TabIndex = 11
        '
        '_cmbField_2
        '
        Me._cmbField_2.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me._cmbField_2.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me._cmbField_2.BackColor = System.Drawing.SystemColors.Window
        Me._cmbField_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmbField_2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me._cmbField_2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmbField_2.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbField.SetIndex(Me._cmbField_2, CType(2, Short))
        Me._cmbField_2.Items.AddRange(New Object() {"*", "/", "+", "-"})
        Me._cmbField_2.Location = New System.Drawing.Point(80, 56)
        Me._cmbField_2.Name = "_cmbField_2"
        Me._cmbField_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmbField_2.Size = New System.Drawing.Size(89, 22)
        Me._cmbField_2.TabIndex = 5
        '
        '_txtField_3
        '
        Me._txtField_3.AcceptsReturn = True
        Me._txtField_3.BackColor = System.Drawing.SystemColors.Window
        Me._txtField_3.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtField_3.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtField_3.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtField.SetIndex(Me._txtField_3, CType(3, Short))
        Me._txtField_3.Location = New System.Drawing.Point(80, 80)
        Me._txtField_3.MaxLength = 10
        Me._txtField_3.Name = "_txtField_3"
        Me._txtField_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtField_3.Size = New System.Drawing.Size(89, 20)
        Me._txtField_3.TabIndex = 7
        Me._txtField_3.Tag = "EXTCURRENCY"
        '
        '_lblLabel_0
        '
        Me._lblLabel_0.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_0.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_0, CType(0, Short))
        Me._lblLabel_0.Location = New System.Drawing.Point(8, 8)
        Me._lblLabel_0.Name = "_lblLabel_0"
        Me._lblLabel_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_0.Size = New System.Drawing.Size(65, 17)
        Me._lblLabel_0.TabIndex = 0
        Me._lblLabel_0.Text = "From Unit :"
        Me._lblLabel_0.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_1
        '
        Me._lblLabel_1.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_1, CType(1, Short))
        Me._lblLabel_1.Location = New System.Drawing.Point(8, 32)
        Me._lblLabel_1.Name = "_lblLabel_1"
        Me._lblLabel_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_1.Size = New System.Drawing.Size(65, 17)
        Me._lblLabel_1.TabIndex = 2
        Me._lblLabel_1.Text = "To Unit :"
        Me._lblLabel_1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_2
        '
        Me._lblLabel_2.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_2, CType(2, Short))
        Me._lblLabel_2.Location = New System.Drawing.Point(8, 56)
        Me._lblLabel_2.Name = "_lblLabel_2"
        Me._lblLabel_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_2.Size = New System.Drawing.Size(65, 17)
        Me._lblLabel_2.TabIndex = 4
        Me._lblLabel_2.Text = "Operation :"
        Me._lblLabel_2.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblLabel_3
        '
        Me._lblLabel_3.BackColor = System.Drawing.Color.Transparent
        Me._lblLabel_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLabel_3.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLabel_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLabel.SetIndex(Me._lblLabel_3, CType(3, Short))
        Me._lblLabel_3.Location = New System.Drawing.Point(8, 80)
        Me._lblLabel_3.Name = "_lblLabel_3"
        Me._lblLabel_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLabel_3.Size = New System.Drawing.Size(65, 17)
        Me._lblLabel_3.TabIndex = 6
        Me._lblLabel_3.Text = "Factor :"
        Me._lblLabel_3.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'cmbField
        '
        '
        'cmdBrandSearch
        '
        '
        'txtField
        '
        '
        'frmConversion
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(333, 121)
        Me.Controls.Add(Me._cmbField_0)
        Me.Controls.Add(Me._cmbField_1)
        Me.Controls.Add(Me._cmdBrandSearch_9)
        Me.Controls.Add(Me._cmdBrandSearch_8)
        Me.Controls.Add(Me.cmdDelete)
        Me.Controls.Add(Me._cmbField_2)
        Me.Controls.Add(Me._txtField_3)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.cmdAdd)
        Me.Controls.Add(Me._lblLabel_0)
        Me.Controls.Add(Me._lblLabel_1)
        Me.Controls.Add(Me._lblLabel_2)
        Me.Controls.Add(Me._lblLabel_3)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Location = New System.Drawing.Point(181, 150)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmConversion"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Unit Conversion"
        CType(Me.cmbField, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cmdBrandSearch, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblLabel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtField, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
#End Region 
End Class