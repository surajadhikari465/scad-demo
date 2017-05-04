<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_ManageRoleConflicts
    Inherits Form_IRMABase

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form_ManageRoleConflicts))
        Me.Button_Close = New System.Windows.Forms.Button
        Me.DropDownList_Role1 = New System.Windows.Forms.ComboBox
        Me.grdConflicts = New System.Windows.Forms.DataGridView
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.DropDownList_Role2 = New System.Windows.Forms.ComboBox
        Me.Button_AddConflict = New System.Windows.Forms.Button
        CType(Me.grdConflicts, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Button_Close
        '
        Me.Button_Close.Location = New System.Drawing.Point(308, 339)
        Me.Button_Close.Name = "Button_Close"
        Me.Button_Close.Size = New System.Drawing.Size(82, 31)
        Me.Button_Close.TabIndex = 7
        Me.Button_Close.Text = "Close"
        Me.Button_Close.UseVisualStyleBackColor = True
        '
        'DropDownList_Role1
        '
        Me.DropDownList_Role1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.DropDownList_Role1.FormattingEnabled = True
        Me.DropDownList_Role1.Location = New System.Drawing.Point(86, 6)
        Me.DropDownList_Role1.Name = "DropDownList_Role1"
        Me.DropDownList_Role1.Size = New System.Drawing.Size(228, 21)
        Me.DropDownList_Role1.TabIndex = 26
        '
        'grdConflicts
        '
        Me.grdConflicts.AllowUserToAddRows = False
        Me.grdConflicts.AllowUserToDeleteRows = False
        Me.grdConflicts.AllowUserToResizeColumns = False
        Me.grdConflicts.AllowUserToResizeRows = False
        Me.grdConflicts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.grdConflicts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.grdConflicts.Location = New System.Drawing.Point(9, 84)
        Me.grdConflicts.Name = "grdConflicts"
        Me.grdConflicts.RowHeadersVisible = False
        Me.grdConflicts.Size = New System.Drawing.Size(381, 240)
        Me.grdConflicts.TabIndex = 28
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(28, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(42, 13)
        Me.Label1.TabIndex = 30
        Me.Label1.Text = "Role 1:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(27, 42)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(42, 13)
        Me.Label2.TabIndex = 32
        Me.Label2.Text = "Role 2:"
        '
        'DropDownList_Role2
        '
        Me.DropDownList_Role2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.DropDownList_Role2.FormattingEnabled = True
        Me.DropDownList_Role2.Location = New System.Drawing.Point(86, 39)
        Me.DropDownList_Role2.Name = "DropDownList_Role2"
        Me.DropDownList_Role2.Size = New System.Drawing.Size(228, 21)
        Me.DropDownList_Role2.TabIndex = 31
        '
        'Button_AddConflict
        '
        Me.Button_AddConflict.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button_AddConflict.ForeColor = System.Drawing.Color.Black
        Me.Button_AddConflict.Image = CType(resources.GetObject("Button_AddConflict.Image"), System.Drawing.Image)
        Me.Button_AddConflict.Location = New System.Drawing.Point(351, 14)
        Me.Button_AddConflict.Name = "Button_AddConflict"
        Me.Button_AddConflict.Size = New System.Drawing.Size(39, 33)
        Me.Button_AddConflict.TabIndex = 33
        Me.Button_AddConflict.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.Button_AddConflict.UseVisualStyleBackColor = True
        '
        'Form_ManageRoleConflicts
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.ClientSize = New System.Drawing.Size(403, 382)
        Me.Controls.Add(Me.Button_AddConflict)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.DropDownList_Role2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.grdConflicts)
        Me.Controls.Add(Me.DropDownList_Role1)
        Me.Controls.Add(Me.Button_Close)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Form_ManageRoleConflicts"
        Me.ShowInTaskbar = False
        Me.Text = "Manage Role Conflicts"
        CType(Me.grdConflicts, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Button_Close As System.Windows.Forms.Button
    Friend WithEvents DropDownList_Role1 As System.Windows.Forms.ComboBox
    Friend WithEvents grdConflicts As System.Windows.Forms.DataGridView
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents DropDownList_Role2 As System.Windows.Forms.ComboBox
    Friend WithEvents Button_AddConflict As System.Windows.Forms.Button

End Class
