<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_TitleDeleteConflicts
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
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form_TitleDeleteConflicts))
        Me.Button_Save = New System.Windows.Forms.Button
        Me.Button_Cancel = New System.Windows.Forms.Button
        Me.ComboBox_Titles = New System.Windows.Forms.ComboBox
        Me.BindingSource1 = New System.Windows.Forms.BindingSource(Me.components)
        Me.grdUsers = New System.Windows.Forms.DataGridView
        Me.CheckBox_ApplyAll = New System.Windows.Forms.CheckBox
        Me.Label1 = New System.Windows.Forms.Label
        CType(Me.BindingSource1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.grdUsers, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Button_Save
        '
        Me.Button_Save.Image = CType(resources.GetObject("Button_Save.Image"), System.Drawing.Image)
        Me.Button_Save.Location = New System.Drawing.Point(511, 421)
        Me.Button_Save.Name = "Button_Save"
        Me.Button_Save.Size = New System.Drawing.Size(87, 31)
        Me.Button_Save.TabIndex = 8
        Me.Button_Save.Text = "Save"
        Me.Button_Save.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Button_Save.UseVisualStyleBackColor = True
        '
        'Button_Cancel
        '
        Me.Button_Cancel.Location = New System.Drawing.Point(423, 421)
        Me.Button_Cancel.Name = "Button_Cancel"
        Me.Button_Cancel.Size = New System.Drawing.Size(82, 31)
        Me.Button_Cancel.TabIndex = 7
        Me.Button_Cancel.Text = "Close"
        Me.Button_Cancel.UseVisualStyleBackColor = True
        '
        'ComboBox_Titles
        '
        Me.ComboBox_Titles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_Titles.Enabled = False
        Me.ComboBox_Titles.FormattingEnabled = True
        Me.ComboBox_Titles.Location = New System.Drawing.Point(140, 12)
        Me.ComboBox_Titles.Name = "ComboBox_Titles"
        Me.ComboBox_Titles.Size = New System.Drawing.Size(199, 21)
        Me.ComboBox_Titles.TabIndex = 26
        '
        'grdUsers
        '
        Me.grdUsers.AllowUserToAddRows = False
        Me.grdUsers.AllowUserToDeleteRows = False
        Me.grdUsers.AllowUserToResizeColumns = False
        Me.grdUsers.AllowUserToResizeRows = False
        Me.grdUsers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.grdUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.grdUsers.Location = New System.Drawing.Point(12, 53)
        Me.grdUsers.Name = "grdUsers"
        Me.grdUsers.Size = New System.Drawing.Size(586, 346)
        Me.grdUsers.TabIndex = 28
        '
        'CheckBox_ApplyAll
        '
        Me.CheckBox_ApplyAll.AutoSize = True
        Me.CheckBox_ApplyAll.Location = New System.Drawing.Point(357, 14)
        Me.CheckBox_ApplyAll.Name = "CheckBox_ApplyAll"
        Me.CheckBox_ApplyAll.Size = New System.Drawing.Size(85, 17)
        Me.CheckBox_ApplyAll.TabIndex = 29
        Me.CheckBox_ApplyAll.Text = "Apply to All"
        Me.CheckBox_ApplyAll.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(106, 17)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(28, 13)
        Me.Label1.TabIndex = 30
        Me.Label1.Text = "Title"
        '
        'Form_TitleDeleteConflicts
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.ClientSize = New System.Drawing.Size(607, 465)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.CheckBox_ApplyAll)
        Me.Controls.Add(Me.grdUsers)
        Me.Controls.Add(Me.ComboBox_Titles)
        Me.Controls.Add(Me.Button_Save)
        Me.Controls.Add(Me.Button_Cancel)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Form_TitleDeleteConflicts"
        Me.ShowInTaskbar = False
        Me.Text = "Manage Titles"
        CType(Me.BindingSource1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.grdUsers, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Button_Save As System.Windows.Forms.Button
    Friend WithEvents Button_Cancel As System.Windows.Forms.Button
    Friend WithEvents ComboBox_Titles As System.Windows.Forms.ComboBox
    Friend WithEvents BindingSource1 As System.Windows.Forms.BindingSource
    Friend WithEvents grdUsers As System.Windows.Forms.DataGridView
    Friend WithEvents CheckBox_ApplyAll As System.Windows.Forms.CheckBox
    Friend WithEvents Label1 As System.Windows.Forms.Label

End Class
