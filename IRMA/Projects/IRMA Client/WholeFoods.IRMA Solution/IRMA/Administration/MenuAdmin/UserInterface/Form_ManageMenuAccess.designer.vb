<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_ManageMenuAccess
    Inherits System.Windows.Forms.Form

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form_ManageMenuAccess))
        Me.cmdClose = New System.Windows.Forms.Button
        Me.grdMenus = New System.Windows.Forms.DataGridView
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.CheckBox_All = New System.Windows.Forms.CheckBox
        Me.Button_Add = New System.Windows.Forms.Button
        CType(Me.grdMenus, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdClose
        '
        Me.cmdClose.Location = New System.Drawing.Point(555, 434)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New System.Drawing.Size(75, 23)
        Me.cmdClose.TabIndex = 18
        Me.cmdClose.Text = "Close"
        Me.cmdClose.UseVisualStyleBackColor = True
        '
        'grdMenus
        '
        Me.grdMenus.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.grdMenus.Location = New System.Drawing.Point(12, 53)
        Me.grdMenus.MultiSelect = False
        Me.grdMenus.Name = "grdMenus"
        Me.grdMenus.RowHeadersVisible = False
        Me.grdMenus.Size = New System.Drawing.Size(618, 368)
        Me.grdMenus.TabIndex = 75
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(109, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(263, 18)
        Me.Label1.TabIndex = 76
        Me.Label1.Text = "Please uncheck any menus that you wish to hide.  "
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(77, 27)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(338, 13)
        Me.Label2.TabIndex = 77
        Me.Label2.Text = "The change will take effect immediately and no save is needed."
        '
        'CheckBox_All
        '
        Me.CheckBox_All.AutoSize = True
        Me.CheckBox_All.Location = New System.Drawing.Point(591, 30)
        Me.CheckBox_All.Name = "CheckBox_All"
        Me.CheckBox_All.Size = New System.Drawing.Size(39, 17)
        Me.CheckBox_All.TabIndex = 78
        Me.CheckBox_All.Text = "All"
        Me.CheckBox_All.UseVisualStyleBackColor = True
        '
        'Button_Add
        '
        Me.Button_Add.Image = CType(resources.GetObject("Button_Add.Image"), System.Drawing.Image)
        Me.Button_Add.Location = New System.Drawing.Point(445, 5)
        Me.Button_Add.Name = "Button_Add"
        Me.Button_Add.Size = New System.Drawing.Size(33, 35)
        Me.Button_Add.TabIndex = 79
        Me.Button_Add.UseVisualStyleBackColor = True
        '
        'Form_ManageMenuAccess
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(646, 466)
        Me.Controls.Add(Me.Button_Add)
        Me.Controls.Add(Me.CheckBox_All)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.grdMenus)
        Me.Controls.Add(Me.cmdClose)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Form_ManageMenuAccess"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Manage Menu Access"
        CType(Me.grdMenus, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cmdClose As System.Windows.Forms.Button
    Friend WithEvents grdMenus As System.Windows.Forms.DataGridView
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents CheckBox_All As System.Windows.Forms.CheckBox
    Friend WithEvents Button_Add As System.Windows.Forms.Button

    'Private Sub Button_Add_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    'End Sub

    'Private Sub Button_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    'End Sub
End Class
