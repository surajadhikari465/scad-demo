<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Team
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
        Me.ComboBox_Teams = New System.Windows.Forms.ComboBox
        Me.Button_EditTeam = New System.Windows.Forms.Button
        Me.Button_AddTeam = New System.Windows.Forms.Button
        Me.Button_Close = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'ComboBox_Teams
        '
        Me.ComboBox_Teams.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_Teams.FormattingEnabled = True
        Me.ComboBox_Teams.Location = New System.Drawing.Point(128, 33)
        Me.ComboBox_Teams.Name = "ComboBox_Teams"
        Me.ComboBox_Teams.Size = New System.Drawing.Size(255, 21)
        Me.ComboBox_Teams.TabIndex = 0
        '
        'Button_EditTeam
        '
        Me.Button_EditTeam.AutoSize = True
        Me.Button_EditTeam.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.Button_EditTeam.Location = New System.Drawing.Point(340, 64)
        Me.Button_EditTeam.Name = "Button_EditTeam"
        Me.Button_EditTeam.Size = New System.Drawing.Size(65, 23)
        Me.Button_EditTeam.TabIndex = 1
        Me.Button_EditTeam.Text = "Edit Team"
        Me.Button_EditTeam.UseVisualStyleBackColor = True
        '
        'Button_AddTeam
        '
        Me.Button_AddTeam.AutoSize = True
        Me.Button_AddTeam.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.Button_AddTeam.Location = New System.Drawing.Point(265, 64)
        Me.Button_AddTeam.Name = "Button_AddTeam"
        Me.Button_AddTeam.Size = New System.Drawing.Size(69, 23)
        Me.Button_AddTeam.TabIndex = 2
        Me.Button_AddTeam.Text = "New Team"
        Me.Button_AddTeam.UseVisualStyleBackColor = True
        '
        'Button_Close
        '
        Me.Button_Close.AutoSize = True
        Me.Button_Close.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.Button_Close.Location = New System.Drawing.Point(411, 64)
        Me.Button_Close.Name = "Button_Close"
        Me.Button_Close.Size = New System.Drawing.Size(43, 23)
        Me.Button_Close.TabIndex = 3
        Me.Button_Close.Text = "Close"
        Me.Button_Close.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(83, 36)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(42, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Team:"
        '
        'Team
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(461, 96)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Button_Close)
        Me.Controls.Add(Me.Button_AddTeam)
        Me.Controls.Add(Me.Button_EditTeam)
        Me.Controls.Add(Me.ComboBox_Teams)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Team"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Team Management"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ComboBox_Teams As System.Windows.Forms.ComboBox
    Friend WithEvents Button_EditTeam As System.Windows.Forms.Button
    Friend WithEvents Button_AddTeam As System.Windows.Forms.Button
    Friend WithEvents Button_Close As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
End Class
